// ROGUES
// SERVER
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Passive_Scamp : Passive
{
	[Separator("Suit Shield Effect Data")]
	public StandardActorEffectData m_shieldEffectData;
	[Separator("Whether to zero out energy when shield is depleted")]
	public bool m_clearEnergyOnSuitRemoval;
	[Separator("Energy Orbs")]
	public int m_orbDuration = 4;
	[Space(5f)]
	public int m_orbNumToSpawn = 5;
	public float m_orbMinSpawnDist = 1f;
	public float m_orbMaxSpawnDist = 10f;
	public int m_orbEnergyGainOnTrigger = 15;
	public StandardEffectInfo m_orbTriggerEffect;
	[Header("-- Whether to clear orbs on death")]
	public bool m_clearOrbsOnDeath = true;
	[Separator("Reset Energy on Respawn?")]
	public bool m_resetEnergyOnRespawn;
	[Separator("Approximate duration of orb spawn animation")]
	public float m_orbSpawnAnimDuration = 3f; // TODO SCAMP ?
	[Separator("Sequences for Energy Orb")]
	public GameObject m_orbSpawnCasterSeqPrefab;
	[Header("-- Optional sequence to show projectile towards each spawned orb")]
	public GameObject m_orbSpawnProjectileSeqPrefab;
	[Space(10f)]
	public GameObject m_orbPersistentSeqPrefab;
	public GameObject m_orbTriggerSeqPrefab;
	
	
#if SERVER
	// custom
	private ScampSuitToggle m_ultimateAbility;
	private AbilityData.ActionType m_ultimateAbilityActionType;
	private ScampAoeTether m_tetherAbility;
	private AbilityData.ActionType m_tetherAbilityActionType;
	private ScampDashAndAoe m_dashAbility;
	private AbilityData.ActionType m_dashAbilityActionType;
	private Scamp_SyncComponent m_syncComp;
	private bool m_pendingShield;
	private bool m_pendingSuitGainEffect;
	private int m_pendingCdrOnTether;
	private int m_furyBallNextDashTurn = 2;
	private int m_onFootNextDashTurn = 2;
	
	public int FuryBallDashCooldownRemaining => Math.Max(0, m_furyBallNextDashTurn - GameFlowData.Get().CurrentTurn);
	public int OnFootDashCooldownRemaining => Math.Max(0, m_onFootNextDashTurn - GameFlowData.Get().CurrentTurn);

	private static readonly List<Int2> s_orbLocations = new List<Int2>
	{
		new Int2(3, 3),
		new Int2(-3, 3),
		new Int2(3, -3),
		new Int2(-3, -3),
		new Int2(4, 0),
		new Int2(-4, 0),
		new Int2(0, 4),
		new Int2(0, -4),
		new Int2(4, 1),
		new Int2(-4, 1),
		new Int2(4, -1),
		new Int2(-4, -1),
		new Int2(1, 4),
		new Int2(1, -4),
		new Int2(-1, 4),
		new Int2(-1, -4),
	};
#endif

	public int GetMaxSuitShield()
	{
		return m_shieldEffectData.m_absorbAmount;
	}
	
#if SERVER
	// custom
	private int NumOrbsToSpawn => m_orbNumToSpawn + m_ultimateAbility.GetExtraOrbsToSpawnOnSuitLost();
	
	// custom
	protected override void OnStartup()
	{
		base.OnStartup();

		AbilityData abilityData = Owner.GetAbilityData();
		m_ultimateAbility = abilityData.GetAbilityOfType<ScampSuitToggle>();
		m_ultimateAbilityActionType = abilityData.GetActionTypeOfAbility(m_ultimateAbility);
		m_tetherAbility = abilityData.GetAbilityOfType<ScampAoeTether>();
		m_tetherAbilityActionType = abilityData.GetActionTypeOfAbility(m_tetherAbility);
		m_dashAbility = abilityData.GetAbilityOfType<ScampDashAndAoe>();
		m_dashAbilityActionType = abilityData.GetActionTypeOfAbility(m_dashAbility);
		m_syncComp = Owner.GetComponent<Scamp_SyncComponent>();
	}
	
	// custom
	public override void OnTurnStart()
	{
		base.OnTurnStart();

		int currentTurn = GameFlowData.Get().CurrentTurn;
		if (currentTurn == 1)
		{
			CreateShield();
		}
		else if (m_syncComp.m_lastSuitLostTurn == currentTurn - 1)
		{
			ApplyEffectForSuitLost();
		}

		if (m_pendingSuitGainEffect)
		{
			ApplyEffectForSuitGained();
			m_pendingSuitGainEffect = false;
		}
		
		m_syncComp.Networkm_suitWasActiveOnTurnStart = m_syncComp.m_suitActive;
		m_syncComp.Networkm_suitShieldingOnTurnStart = (uint)GetCurrentAbsorb();
		
		if (!m_syncComp.m_suitActive && Owner.HitPoints < m_dashAbility.GetShieldDownNoCooldownHealthThresh())
		{
			m_onFootNextDashTurn = currentTurn;
			Owner.GetAbilityData().OverrideCooldown(m_dashAbilityActionType, OnFootDashCooldownRemaining);
		}
	}

	// custom
	public override void OnTurnEnd()
	{
		base.OnTurnEnd();
		CheckShield();
		m_syncComp.CallRpcSetAnimParamForSuit(m_syncComp.m_suitActive);

		if (m_pendingCdrOnTether > 0)
		{
			ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(Owner, Owner.GetFreePos()));
			actorHitResults.AddMiscHitEvent(new MiscHitEventData_AddToCasterCooldown(
				m_tetherAbilityActionType,
				-m_pendingCdrOnTether));
			actorHitResults.SetIgnoreTechpointInteractionForHit(true);
			MovementResults.SetupAndExecuteAbilityResultsOutsideResolution(
				Owner,
				Owner,
				actorHitResults,
				m_tetherAbility);
			m_pendingCdrOnTether = 0;
		}

		if (Owner.IsDead() && m_resetEnergyOnRespawn)
		{
			Owner.SetTechPoints(0);
		}
	}

	// custom
	public override void OnAbilityPhaseEnd(AbilityPriority phase)
	{
		base.OnAbilityPhaseEnd(phase);
		
		if (phase == AbilityPriority.Prep_Defense
		    && !m_syncComp.m_suitWasActiveOnTurnStart
		    && ServerActionBuffer.Get().HasStoredAbilityRequestOfType(Owner, typeof(ScampSuitToggle)))
		{
			SetShieldActive(true); // trying to time suit up animation
		}
	}

	// custom
	public override void OnActorRespawn()
	{
		base.OnActorRespawn();
		m_pendingShield = true;
	}

	// custom
	public override void OnDied(List<UnresolvedHealthChange> killers)
	{
		base.OnDied(killers);
		
		if (m_clearOrbsOnDeath)
		{
			DestroyOrbs();
		}
		if (m_resetEnergyOnRespawn)
		{
			Owner.SetTechPoints(0);
		}
		
		Owner.GetAbilityData().OverrideCooldown(m_dashAbilityActionType, FuryBallDashCooldownRemaining);
	}

	// custom
	public override void OnResolveStart(bool hasAbilities, bool hasMovement)
	{
		base.OnResolveStart(hasAbilities, hasMovement);

		if (!Owner.IsDead())
		{
			m_syncComp.CallRpcResetAttackParam();
		}
	}

	// custom
	public override void OnAbilitiesDone()
	{
		base.OnAbilitiesDone();

		if (m_pendingShield)
		{
			CreateShield();
			m_pendingShield = false;
		}
		else
		{
			CheckShield();
		}
	}

	// custom
	private void CheckShield()
	{
		List<StandardActorEffect> effects = GetShieldEffects();
		if (m_syncComp.m_suitActive && !effects.Any(e => e.CanAbsorb()))
		{
			DestroyShield(effects);
			m_syncComp.CallRpcPlayShieldRemoveAnim();
			m_syncComp.Networkm_lastSuitLostTurn = (uint)GameFlowData.Get().CurrentTurn;
			SetShieldActive(false);

			if (m_clearEnergyOnSuitRemoval)
			{
				Owner.SetTechPoints(0);
			}
		}
	}

	// custom
	public List<ScampOrbEffect> GetOrbs()
	{
		return ServerEffectManager.Get()
			.GetWorldEffectsByCaster(Owner, typeof(ScampOrbEffect))
			.Select(e => e as ScampOrbEffect)
			.ToList();
	}

	// custom
	private void CreateShield()
	{
		ActorHitParameters actorHitParameters = new ActorHitParameters(Owner, Owner.GetFreePos());
		ActorHitResults actorHitResults = new ActorHitResults(actorHitParameters);
		StandardActorEffect shieldEffect = new StandardActorEffect(
			m_ultimateAbility.AsEffectSource(),
			Owner.GetCurrentBoardSquare(),
			Owner,
			Owner,
			m_shieldEffectData);
		shieldEffect.SetIgnoredForStatistics(true);
		actorHitResults.AddEffect(shieldEffect);
		actorHitResults.AddMiscHitEvent(
			new MiscHitEventData_OverrideCooldown(
				m_dashAbilityActionType,
				FuryBallDashCooldownRemaining - 1));
		MovementResults.SetupAndExecuteAbilityResultsOutsideResolution(
			Owner,
			Owner,
			actorHitResults,
			m_ultimateAbility);
		SetShieldActive(true);
	}

	// custom
	public void SetShieldActive(bool isActive)
	{
		m_syncComp.CallRpcResetTargetersForSuitMode(isActive);
		m_syncComp.Networkm_suitActive = isActive;
		m_syncComp.CallRpcSetAnimParamForSuit(isActive);
	}

	// custom
	private void DestroyShield(List<StandardActorEffect> effects)
	{
		if (Owner.IsDead())
		{
			return;
		}
		
		ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(Owner, Owner.GetFreePos()));
		foreach (StandardActorEffect effect in effects)
		{
			actorHitResults.AddEffectForRemoval(effect);
		}
		actorHitResults.AddMiscHitEvent(
			new MiscHitEventData_OverrideCooldown(
				m_ultimateAbilityActionType,
				m_ultimateAbility.GetCooldownOverrideOnSuitDestroy()));
		actorHitResults.AddMiscHitEvent(
			new MiscHitEventData_OverrideCooldown(
				m_dashAbilityActionType,
				OnFootDashCooldownRemaining - 1));
		
		SequenceSource seqSource = new SequenceSource(null, null);
		MovementResults movementResults = new MovementResults(MovementStage.INVALID);
		movementResults.SetupForHitOutsideResolution(
			Owner,
			Owner,
			actorHitResults,
			m_ultimateAbility,
			null,
			Owner.GetCurrentBoardSquare(),
			seqSource,
			true);
		
		movementResults.AddSequenceStartOverride(
			new ServerClientUtils.SequenceStartData(
				m_orbSpawnCasterSeqPrefab,
				Owner.GetCurrentBoardSquare(),
				Owner.AsArray(),
				Owner,
				seqSource),
			seqSource);

		foreach (BoardSquare boardSquare in GetSquaresToSpawnOrbsOn())
		{
			PositionHitResults positionHitResults = new PositionHitResults(new PositionHitParameters(boardSquare.ToVector3()));
			positionHitResults.AddEffect(
				new ScampOrbEffect(
					m_ultimateAbility.AsEffectSource(),
					boardSquare,
					Owner,
					m_orbDuration,
					m_orbEnergyGainOnTrigger,
					m_orbTriggerEffect,
					m_orbPersistentSeqPrefab,
					m_orbTriggerSeqPrefab));
			
			movementResults.AddSequenceStartOverride(
				new ServerClientUtils.SequenceStartData(
					m_orbSpawnProjectileSeqPrefab,
					boardSquare.ToVector3(),
					Owner.AsArray(),
					Owner,
					seqSource),
				seqSource,
				false);
			movementResults.GetPowerUpResults().StorePositionHit(positionHitResults);
		}
		movementResults.ExecuteUnexecutedMovementHits(false);
		if (ServerResolutionManager.Get() != null)
		{
			ServerResolutionManager.Get().SendNonResolutionActionToClients(movementResults);
		}
	}
	
	// custom
	private void ApplyEffectForSuitLost()
	{
		ApplyEffectForSuit(m_ultimateAbility.GetEffectForSuitLost());
	}
	
	// custom
	private void ApplyEffectForSuitGained()
	{
		ApplyEffectForSuit(m_ultimateAbility.GetEffectForSuitGained());
	}

	// custom
	private void ApplyEffectForSuit(StandardEffectInfo effect)
	{
		if (!effect.m_applyEffect || Owner.IsDead())
		{
			return;
		}
		
		ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(Owner, Owner.GetFreePos()));
		actorHitResults.AddStandardEffectInfo(effect);
		MovementResults.SetupAndExecuteAbilityResultsOutsideResolution(
			Owner,
			Owner,
			actorHitResults,
			m_ultimateAbility);
	}

	// custom
	private void DestroyOrbs()
	{
		List<ScampOrbEffect> orbEffects = GetOrbs();
		if (orbEffects.Count == 0)
		{
			return;
		}

		ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(Owner, Owner.GetFreePos()));
		foreach (ScampOrbEffect orbEffect in orbEffects)
		{
			actorHitResults.AddEffectForRemoval(orbEffect);
		}
		
		SequenceSource seqSource = new SequenceSource(null, null);
		MovementResults movementResults = new MovementResults(MovementStage.INVALID);
		movementResults.SetupForHitOutsideResolution(
			Owner,
			Owner,
			actorHitResults,
			m_ultimateAbility,
			null,
			Owner.GetCurrentBoardSquare(),
			seqSource,
			true);

		foreach (ScampOrbEffect orbEffect in orbEffects)
		{
			movementResults.AddSequenceStartOverride(orbEffect.GetEffectEndSeqData(seqSource), seqSource);
		}
		movementResults.ExecuteUnexecutedMovementHits(false);
		if (ServerResolutionManager.Get() != null)
		{
			ServerResolutionManager.Get().SendNonResolutionActionToClients(movementResults);
		}
	}
	
	// custom
	private List<BoardSquare> GetSquaresToSpawnOrbsOn()
	{
		int numOrbsToSpawn = NumOrbsToSpawn;
		List<BoardSquare> res = new List<BoardSquare>();
		BoardSquare centerSquare = Owner.GetCurrentBoardSquare();
		foreach (Int2 delta in s_orbLocations)
		{
			int x = centerSquare.x + delta.x;
			int y = centerSquare.y + delta.y;
			BoardSquare orbSquare = Board.Get().GetSquareFromIndex(x, y);

			if (orbSquare != null
			    && orbSquare.IsValidForGameplay()
			    && centerSquare.GetLOS(x, y))
			{
				res.Add(orbSquare);

				if (res.Count == numOrbsToSpawn)
				{
					return res;
				}
			}
		}

		IEnumerable<BoardSquare> squaresInRadius = AreaEffectUtils
			.GetSquaresInRadius(centerSquare, m_orbMaxSpawnDist, false, Owner)
			.Where(
				s =>
				{
					float dist = centerSquare.HorizontalDistanceInSquaresTo(s);
					return dist >= m_orbMinSpawnDist
					       && dist <= m_orbMaxSpawnDist
					       && s.IsValidForGameplay()
					       && centerSquare.GetLOS(s.x, s.y)
					       && !res.Contains(s);
				})
			.OrderBy(s => centerSquare.HorizontalDistanceInSquaresTo(s));

		int missing = numOrbsToSpawn - res.Count;
		res.AddRange(squaresInRadius.Take(missing)); // theoretically can be not enough but what can we do at this point
		return res;
	}

	// custom
	public List<StandardActorEffect> GetShieldEffects()
	{
		return ServerEffectManager
			.Get()
			.GetEffectsOnTargetByCaster(Owner, Owner, typeof(StandardActorEffect))
			.Where(e => e.Parent.Ability == m_ultimateAbility && e.Absorbtion.m_absorbAmount > 0)
			.Select(e => e as StandardActorEffect)
			.ToList();
	}
	
	// custom
	public int GetCurrentAbsorb()
	{
		return GetShieldEffects().Select(e => e.Absorbtion.m_absorbRemaining).Sum();
	}
	
	// custom
	public void SetPendingCdrOnTether(int cdr)
	{
		m_pendingCdrOnTether = cdr;
	}
	
	// custom
	public void OnTetherBroken()
	{
		m_pendingCdrOnTether = 0;
	}
	
	// custom
	public override void OnAddToCooldownAttemptOnHit(
		AbilityData.ActionType actionType,
		int cooldownChangeDesired,
		int finalCooldown)
	{
		if (actionType == m_dashAbilityActionType && cooldownChangeDesired < 0)
		{
			m_furyBallNextDashTurn += cooldownChangeDesired;
			m_onFootNextDashTurn += cooldownChangeDesired;
		}
	}
	
	// custom
	public void OnDash()
	{
		int nextDashTurn = GameFlowData.Get().CurrentTurn + m_dashAbility.GetModdedCooldown() + 1;
		if (m_syncComp != null && m_syncComp.m_suitWasActiveOnTurnStart)
		{
			m_furyBallNextDashTurn = nextDashTurn;
		}
		else
		{
			m_onFootNextDashTurn = nextDashTurn;
		}
	}
	
	// custom
	public void OnSuitToggle()
	{
		if (!m_syncComp.m_suitWasActiveOnTurnStart)
		{
			m_pendingSuitGainEffect = true;
		}
	}
#endif
}

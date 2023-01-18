// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

public class ExoAnchorLaser : Ability
{
	[Space(20f)]
	[Header("-- First Cast Damage (non-anchored)")]
	public int m_laserDamageAmount = 25;
	public LaserTargetingInfo m_laserInfo;
	[Header("-- Barrier For Beam")]
	public StandardBarrierData m_laserBarrier;
	[Header("-- Cone to Sweep Across")]
	public int m_sweepDamageAmount = 25;
	public float m_sweepConeBackwardOffset;
	public float m_minConeAngle;
	public float m_maxConeAngle = 180f;
	public ActorModelData.ActionAnimationType m_anchoredActionAnimType;
	public float m_turnToTargetSweepDegreesPerSecond = 90f;
	[Header("-- Extra Damage: for anchored turns")]
	public int m_extraDamagePerTurnAnchored;
	public int m_maxExtraDamageForAnchored;
	[Header("-- Extra Damage: for distance")]
	public float m_extraDamageAtZeroDist;
	public float m_extraDamageChangePerDist;
	[Header("-- Effect while anchored and cooldown when finished")]
	public StandardEffectInfo m_effectOnCaster;
	public int m_cooldownOnEnd;
	public int m_anchoredTechPointCost;
	public StandardEffectInfo m_effectOnAnchorEnd;
	[Header("-- Pending Status for Anchored and NOT using sweep")]
	public List<StatusType> m_statusWhenAnchoredAndNotSweeping;
	[Header("-- Alternate Tooltip while anchored")]
	public string m_anchoredToolTip;
	[HideInInspector]
	public string m_anchoredFinalFullTooltip;
	[Header("-- Animation --")]
	public int m_animIndexForSweep = 6;
	[Header("-- Sequences")]
	public GameObject m_laserExtendSequencePrefab;
	public GameObject m_sweepSequencePrefab;
	public GameObject m_unanchorAnimSequencePrefab;
	public GameObject m_persistentLaserBarrierSequence;

	private Exo_SyncComponent m_syncComponent;
	private AbilityMod_ExoAnchorLaser m_abilityMod;
	private LaserTargetingInfo m_cachedLaserInfo;
	private StandardBarrierData m_cachedLaserBarrier;
	private StandardEffectInfo m_cachedEffectOnCaster;
	private StandardEffectInfo m_cachedEffectOnAnchorEnd;

#if SERVER
	// added in rogues
	private Passive_Exo m_passive;
	private List<ActorData> m_lastGatheredHitEnemiesList;
	private Barrier m_barrierInstance;
#endif

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Anchor Laser";
		}
		SetupTargeter();
		ActorStatus component = GetComponent<ActorStatus>();
		if (component != null)
		{
			component.AddAbilityForPassivePendingStatus(this);
		}
	}

	private void OnDestroy()
	{
		ActorStatus component = GetComponent<ActorStatus>();
		if (component != null)
		{
			component.RemoveAbilityForPassivePendingStatus(this);
		}
	}

	public void SetupTargeter()
	{
		SetCachedFields();
#if SERVER
		// added in rogues
		PassiveData component = GetComponent<PassiveData>();
		if (component != null)
		{
			m_passive = (component.GetPassiveOfType(typeof(Passive_Exo)) as Passive_Exo);
		}
#endif
		if (m_syncComponent == null)
		{
			m_syncComponent = GetComponent<Exo_SyncComponent>();
		}
		if (m_syncComponent == null)
		{
			Log.Error("Missing Exo_SyncComponent on Exo's actorData prefab. ExoAnchorLaser won't function!");
		}
		AbilityUtil_Targeter targeter = new AbilityUtil_Targeter_SweepSingleClickCone(
			this,
			GetMinConeAngle(),
			GetMaxConeAngle(),
			GetLaserInfo().range,
			m_sweepConeBackwardOffset,
			0.2f,
			GetLaserInfo(),
			m_syncComponent);
		targeter.SetAffectedGroups(true, false, false);
		Targeter = targeter;
	}

	public override string GetSetupNotesForEditor()
	{
		return "<color=cyan>-- For Art --</color>\n"
		       + SetupNoteVarName("Laser Extend Sequence Prefab")
		       + "\nFor initial cast, when laser is not already out. Only for gameplay hits and timing of when actual visual show up, no vfx spawned.\n\n"
		       + SetupNoteVarName("Sweep Sequence Prefab")
		       + "\nfor laser visual, rotation of the actor, removing the previous laser, and gameplay hit timing when sweeping\n\n"
		       + SetupNoteVarName("Unanchor Anim Sequence Prefab")
		       + "\nfor setting idle type when un-anchoring and removing the previous laser vfx\n\n"
		       + SetupNoteVarName("Persistent Laser Barrier Sequence")
		       + "\nfor persistent laser visuals (which is a barrier internally), and optionally ExoLaserHittingWallSequence for a continuing impact vfx\n\n";
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return GetLaserInfo().range;
	}

	public override bool CanTriggerAnimAtIndexForTaunt(int animIndex)
	{
		return base.CanTriggerAnimAtIndexForTaunt(animIndex) || animIndex == m_animIndexForSweep;
	}

	public override ActorModelData.ActionAnimationType GetActionAnimType(List<AbilityTarget> targets, ActorData caster)
	{
		if (m_syncComponent != null && m_syncComponent.m_anchored)
		{
			return (ActorModelData.ActionAnimationType)m_animIndexForSweep;
		}
		return base.GetActionAnimType();
	}

	private void SetCachedFields()
	{
		m_cachedLaserInfo = m_abilityMod != null
			? m_abilityMod.m_laserInfoMod.GetModifiedValue(m_laserInfo)
			: m_laserInfo;
		// rogues
		// if (m_laserBarrier == null)
		// {
		// 	m_laserBarrier = ScriptableObject.CreateInstance<StandardBarrierData>();
		// }
		m_cachedLaserBarrier = m_abilityMod != null
			? m_abilityMod.m_laserBarrierMod.GetModifiedValue(m_laserBarrier)
			: m_laserBarrier;
		m_cachedEffectOnCaster = m_abilityMod != null
			? m_abilityMod.m_effectOnCasterMod.GetModifiedValue(m_effectOnCaster)
			: m_effectOnCaster;
		m_cachedEffectOnAnchorEnd = m_abilityMod != null
			? m_abilityMod.m_effectOnAnchorEndMod.GetModifiedValue(m_effectOnAnchorEnd)
			: m_effectOnAnchorEnd;
	}

	public int GetLaserDamageAmount()
	{
		return m_abilityMod != null
			? m_abilityMod.m_laserDamageAmountMod.GetModifiedValue(m_laserDamageAmount)
			: m_laserDamageAmount;
	}

	public LaserTargetingInfo GetLaserInfo()
	{
		return m_cachedLaserInfo ?? m_laserInfo;
	}

	public StandardBarrierData GetLaserBarrier()
	{
		return m_cachedLaserBarrier ?? m_laserBarrier;
	}

	public int GetSweepDamageAmount()
	{
		return m_abilityMod != null
			? m_abilityMod.m_sweepDamageAmountMod.GetModifiedValue(m_sweepDamageAmount)
			: m_sweepDamageAmount;
	}

	public float GetSweepConeBackwardOffset()
	{
		return m_abilityMod != null
			? m_abilityMod.m_sweepConeBackwardOffsetMod.GetModifiedValue(m_sweepConeBackwardOffset)
			: m_sweepConeBackwardOffset;
	}

	public float GetMinConeAngle()
	{
		return m_abilityMod != null
			? m_abilityMod.m_minConeAngleMod.GetModifiedValue(m_minConeAngle)
			: m_minConeAngle;
	}

	public float GetMaxConeAngle()
	{
		return m_abilityMod != null
			? m_abilityMod.m_maxConeAngleMod.GetModifiedValue(m_maxConeAngle)
			: m_maxConeAngle;
	}

	public int GetExtraDamagePerTurnAnchored()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraDamagePerTurnAnchoredMod.GetModifiedValue(m_extraDamagePerTurnAnchored)
			: m_extraDamagePerTurnAnchored;
	}

	public int GetMaxExtraDamageForAnchored()
	{
		return m_abilityMod != null
			? m_abilityMod.m_maxExtraDamageForAnchoredMod.GetModifiedValue(m_maxExtraDamageForAnchored)
			: m_maxExtraDamageForAnchored;
	}

	public float GetExtraDamageAtZeroDist()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraDamageAtZeroDistMod.GetModifiedValue(m_extraDamageAtZeroDist)
			: m_extraDamageAtZeroDist;
	}

	public float GetExtraDamageChangePerDist()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraDamageChangePerDistMod.GetModifiedValue(m_extraDamageChangePerDist)
			: m_extraDamageChangePerDist;
	}

	public StandardEffectInfo GetEffectOnCaster()
	{
		return m_cachedEffectOnCaster ?? m_effectOnCaster;
	}

	public int GetCooldownOnEnd()
	{
		return m_abilityMod != null
			? m_abilityMod.m_cooldownOnEndMod.GetModifiedValue(m_cooldownOnEnd)
			: m_cooldownOnEnd;
	}

	public int GetAnchoredTechPointCost()
	{
		return m_abilityMod != null
			? m_abilityMod.m_anchoredTechPointCostMod.GetModifiedValue(m_anchoredTechPointCost)
			: m_anchoredTechPointCost;
	}

	public StandardEffectInfo GetEffectOnAnchorEnd()
	{
		return m_cachedEffectOnAnchorEnd ?? m_effectOnAnchorEnd;
	}

	public bool ShouldUpdateMovementOnAnchorChange()
	{
		List<StatusType> list = m_statusWhenAnchoredAndNotSweeping;
		if (m_abilityMod != null && m_abilityMod.m_useStatusWhenAnchoredAndNotSweepingOverride)
		{
			list = m_abilityMod.m_statusWhenAnchoredAndNotSweepingOverride;
		}
		return list != null && list.Count > 0;
	}

	public bool HasPendingStatusTurnOfAnchorEnd(StatusType status)
	{
		List<StatusType> list = m_statusWhenAnchoredAndNotSweeping;
		if (m_abilityMod != null && m_abilityMod.m_useStatusWhenAnchoredAndNotSweepingOverride)
		{
			list = m_abilityMod.m_statusWhenAnchoredAndNotSweepingOverride;
		}
		return list != null && list.Contains(status);
	}

	public int GetTotalDamage(Vector3 startPos, Vector3 hitPos, int baseDamage, bool checkDurationBonus)
	{
		int damage = baseDamage;
		if (m_syncComponent != null)
		{
			if (GetExtraDamagePerTurnAnchored() > 0 && checkDurationBonus)
			{
				int turnsAnchored = m_syncComponent.m_turnsAnchored;
				int extraDamageForAnchor = Mathf.Max(0, turnsAnchored) * GetExtraDamagePerTurnAnchored();
				if (GetMaxExtraDamageForAnchored() > 0)
				{
					extraDamageForAnchor = Mathf.Min(extraDamageForAnchor, GetMaxExtraDamageForAnchored());
				}
				damage += extraDamageForAnchor;
			}
			if (GetExtraDamageAtZeroDist() > 0f || GetExtraDamageChangePerDist() > 0f)
			{
				float extraDamageAtZeroDist = GetExtraDamageAtZeroDist();
				float distAdjusted = VectorUtils.HorizontalPlaneDistInSquares(startPos, hitPos) - 1.4f;
				if (distAdjusted > 0f)
				{
					extraDamageAtZeroDist += GetExtraDamageChangePerDist() * distAdjusted;
				}
				damage += Mathf.Max(0, Mathf.RoundToInt(extraDamageAtZeroDist));
			}
		}
		return damage;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, GetLaserDamageAmount());
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> symbolToValue = new Dictionary<AbilityTooltipSymbol, int>();
		int totalDamage = m_syncComponent != null && m_syncComponent.m_anchored
			? GetTotalDamage(ActorData.GetFreePos(), targetActor.GetFreePos(), GetSweepDamageAmount(), true)
			: GetTotalDamage(ActorData.GetFreePos(), targetActor.GetFreePos(), GetLaserDamageAmount(), false);
		AddNameplateValueForSingleHit(ref symbolToValue, Targeter, targetActor, totalDamage);
		return symbolToValue;
	}

	public override int GetModdedCost()
	{
		return m_syncComponent != null && m_syncComponent.m_wasAnchoredOnTurnStart
			? GetAnchoredTechPointCost()
			: base.GetModdedCost();
	}

	public override ActorModelData.ActionAnimationType GetActionAnimType()
	{
		return m_syncComponent != null && m_syncComponent.m_anchored
			? m_anchoredActionAnimType
			: base.GetActionAnimType();
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_ExoAnchorLaser abilityMod_ExoAnchorLaser = modAsBase as AbilityMod_ExoAnchorLaser;
		AddTokenInt(tokens, "Sweep_Angle", "max angle from the previous direction to sweep the laser", (int)(abilityMod_ExoAnchorLaser != null
			? abilityMod_ExoAnchorLaser.m_maxConeAngleMod.GetModifiedValue(m_maxConeAngle)
			: m_maxConeAngle));
		AddTokenInt(tokens, "Damage_Laser", string.Empty, abilityMod_ExoAnchorLaser != null
			? abilityMod_ExoAnchorLaser.m_laserDamageAmountMod.GetModifiedValue(m_laserDamageAmount)
			: m_laserDamageAmount);
		StandardBarrierData standardBarrierData = abilityMod_ExoAnchorLaser != null
			? abilityMod_ExoAnchorLaser.m_laserBarrierMod.GetModifiedValue(m_laserBarrier)
			: m_laserBarrier;
		standardBarrierData.AddTooltipTokens(tokens, "Laser_Barrier", abilityMod_ExoAnchorLaser != null, m_laserBarrier);
		AddTokenInt(tokens, "Damage_Sweep", string.Empty, abilityMod_ExoAnchorLaser != null
			? abilityMod_ExoAnchorLaser.m_sweepDamageAmountMod.GetModifiedValue(m_sweepDamageAmount)
			: m_sweepDamageAmount);
		AddTokenInt(tokens, "ExtraDamagePerTurnAnchored", string.Empty, abilityMod_ExoAnchorLaser != null
			? abilityMod_ExoAnchorLaser.m_extraDamagePerTurnAnchoredMod.GetModifiedValue(m_extraDamagePerTurnAnchored)
			: m_extraDamagePerTurnAnchored);
		AddTokenInt(tokens, "MaxExtraDamageForAnchored", string.Empty, abilityMod_ExoAnchorLaser != null
			? abilityMod_ExoAnchorLaser.m_maxExtraDamageForAnchoredMod.GetModifiedValue(m_maxExtraDamageForAnchored)
			: m_maxExtraDamageForAnchored);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_ExoAnchorLaser != null
			? abilityMod_ExoAnchorLaser.m_effectOnCasterMod.GetModifiedValue(m_effectOnCaster)
			: m_effectOnCaster, "EffectOnCaster", m_effectOnCaster);
		AddTokenInt(tokens, "Cooldown", string.Empty, abilityMod_ExoAnchorLaser != null
			? abilityMod_ExoAnchorLaser.m_cooldownOnEndMod.GetModifiedValue(m_cooldownOnEnd)
			: m_cooldownOnEnd);
		AddTokenInt(tokens, "AnchoredTechPointCost", string.Empty, abilityMod_ExoAnchorLaser != null
			? abilityMod_ExoAnchorLaser.m_anchoredTechPointCostMod.GetModifiedValue(m_anchoredTechPointCost)
			: m_anchoredTechPointCost);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_ExoAnchorLaser != null
			? abilityMod_ExoAnchorLaser.m_effectOnAnchorEndMod.GetModifiedValue(m_effectOnAnchorEnd)
			: m_effectOnAnchorEnd, "EffectOnAnchorEnd", m_effectOnAnchorEnd);
	}

	// removed in rogues
	public override string GetFullTooltip()
	{
		return m_syncComponent != null
		       && m_syncComponent.m_anchored
		       && !string.IsNullOrEmpty(m_anchoredToolTip)
			? TooltipTokenEntry.GetTooltipWithSubstitutes(string.IsNullOrEmpty(m_anchoredFinalFullTooltip)
				? m_anchoredToolTip
				: m_anchoredFinalFullTooltip, 
				null)
			: base.GetFullTooltip();
	}

	// removed in rogues
	public override void SetUnlocalizedTooltipAndStatusTypes(AbilityMod mod = null)
	{
		if (!string.IsNullOrEmpty(m_anchoredToolTip))
		{
			m_anchoredFinalFullTooltip = TooltipTokenEntry.GetTooltipWithSubstitutes(m_anchoredToolTip, GetTooltipTokenEntries(mod));
		}
		base.SetUnlocalizedTooltipAndStatusTypes(mod);
	}

	public override bool HasPassivePendingStatus(StatusType status, ActorData owner)
	{
		return m_syncComponent != null
		       && m_syncComponent.m_wasAnchoredOnTurnStart
		       && owner != null
		       && !owner.GetAbilityData().HasQueuedAction(AbilityData.ActionType.ABILITY_4)  // , true in rogues
		       && HasPendingStatusTurnOfAnchorEnd(status);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_ExoAnchorLaser))
		{
			m_abilityMod = abilityMod as AbilityMod_ExoAnchorLaser;
			SetupTargeter();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}

	public float GetRotateToTargetDuration(float sweepAngle)
	{
		return m_syncComponent != null
		       && m_syncComponent.m_anchored
		       && m_syncComponent.m_turnsAnchored > 0
			? sweepAngle / m_turnToTargetSweepDegreesPerSecond
			: 0.2f;
	}

	public override bool ShouldRotateToTargetPos()
	{
		return m_syncComponent == null
		       || !m_syncComponent.m_anchored
		       || m_syncComponent.m_turnsAnchored <= 0;
	}

#if SERVER
	// added in rogues
	public override void Run(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		bool isAnchored = false;
		if (m_passive != null)
		{
			isAnchored = m_passive.IsAnchored();
			m_passive.SetAnchored(true);
			m_passive.m_laserLastCastTurn = GameFlowData.Get().CurrentTurn;
			m_passive.m_persistingBarrierInstance = m_barrierInstance;
			m_passive.m_anchorLaserHitActorsThisTurn = m_lastGatheredHitEnemiesList;
			m_passive.ClearExitAnchorAnimTrigger();
		}
		if (m_syncComponent != null)
		{
			if (isAnchored)
			{
				bool laserSweepAnimDirection = Vector3.Cross(m_syncComponent.m_anchoredLaserAimDirection, targets[0].AimDirection).y > 0f;
				m_passive.SetLaserSweepAnimDirection(laserSweepAnimDirection);
				m_syncComponent.Networkm_anchoredLaserAimDirection = GetTargeterClampedAimDirection(
					m_syncComponent.m_anchoredLaserAimDirection,
					targets[0].AimDirection,
					out _,
					out _);
				m_passive.m_currentConsecutiveSweeps++;
				return;
			}
			m_syncComponent.Networkm_anchoredLaserAimDirection = targets[0].AimDirection;
			m_passive.m_currentConsecutiveSweeps = 0;
		}
	}

	// added in rogues
	public override List<ServerClientUtils.SequenceStartData> GetAbilityRunSequenceStartDataList(
		List<AbilityTarget> targets,
		ActorData caster,
		ServerAbilityUtils.AbilityRunData additionalData)
	{
		List<ServerClientUtils.SequenceStartData> list = new List<ServerClientUtils.SequenceStartData>();
		float sweepAngle = 0f;
		float forwardAngle = 0f;
		Vector3 aimDirection = targets[0].AimDirection;
		bool isAnchored = false;
		if (m_passive != null)
		{
			isAnchored = m_passive.IsAnchored();
		}
		if (m_syncComponent != null && isAnchored)
		{
			aimDirection = GetTargeterClampedAimDirection(
				m_syncComponent.m_anchoredLaserAimDirection,
				targets[0].AimDirection,
				out sweepAngle,
				out forwardAngle);
		}
		GetBarrierPosAndFacing(aimDirection, caster, out var targetPos, out _, out _);
		if (m_syncComponent != null && isAnchored)
		{
			list.Add(new ServerClientUtils.SequenceStartData(
				m_sweepSequencePrefab,
				caster.GetLoSCheckPos() + aimDirection,
				additionalData.m_abilityResults.HitActorsArray(),
				caster,
				additionalData.m_sequenceSource,
				new ExoSweepLaserSequence.ExtraParams
				{
					angleInDegrees = sweepAngle,
					forwardAngle = forwardAngle,
					lengthInSquares = GetLaserInfo().range,
					rotationDuration = GetRotateToTargetDuration(sweepAngle)
				}.ToArray()));
			list.Add(new ServerClientUtils.SequenceStartData(
				SequenceLookup.Get().GetSimpleHitSequencePrefab(),
				targetPos,
				new ActorData[0],
				caster,
				additionalData.m_sequenceSource));
		}
		else
		{
			list.Add(new ServerClientUtils.SequenceStartData(
				m_laserExtendSequencePrefab,
				targetPos,
				additionalData.m_abilityResults.HitActorsArray(),
				caster,
				additionalData.m_sequenceSource));
		}
		return list;
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		List<NonActorTargetInfo> list = new List<NonActorTargetInfo>();
		Vector3 aimDirection = targets[0].AimDirection;
		bool isAnchored = false;
		if (m_passive != null)
		{
			isAnchored = m_passive.IsAnchored();
		}
		bool isSweeping = m_syncComponent != null && isAnchored;
		List<ActorData> sweepHitActors;
		if (isSweeping)
		{
			aimDirection = GetTargeterClampedAimDirection(
				m_syncComponent.m_anchoredLaserAimDirection,
				targets[0].AimDirection,
				out _,
				out _);
			sweepHitActors = GetSweepHitActors(
				m_syncComponent.m_anchoredLaserAimDirection,
				aimDirection,
				caster,
				list);
		}
		else
		{
			sweepHitActors = AreaEffectUtils.GetActorsInLaser(
				caster.GetLoSCheckPos(),
				aimDirection,
				GetLaserInfo().range,
				GetLaserInfo().width,
				caster,
				caster.GetOtherTeams(),
				GetLaserInfo().penetrateLos,
				GetLaserInfo().maxTargets,
				false,
				true,
				out _,
				list);
		}
		if (ServerAbilityUtils.CurrentlyGatheringRealResults())
		{
			m_lastGatheredHitEnemiesList = sweepHitActors;
		}
		int baseDamage = isSweeping ? GetSweepDamageAmount() : GetLaserDamageAmount();
		foreach (ActorData actorData in sweepHitActors)
		{
			ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(actorData, caster.GetLoSCheckPos()));
			int totalDamage = GetTotalDamage(caster.GetFreePos(), actorData.GetFreePos(), baseDamage, isSweeping);
			actorHitResults.SetBaseDamage(totalDamage);
			abilityResults.StoreActorHit(actorHitResults);
		}
		ActorHitResults casterHitResults = new ActorHitResults(new ActorHitParameters(caster, caster.GetFreePos()));
		casterHitResults.SetIgnoreTechpointInteractionForHit(true);
		if (isAnchored)
		{
			casterHitResults.AddBarrierForRemoval(m_passive.m_persistingBarrierInstance, true);
		}
		if (!ServerEffectManager.Get().HasEffect(caster, typeof(ExoAnchoredLaserCooldownEffect)))
		{
			ExoAnchoredLaserCooldownEffect effect = new ExoAnchoredLaserCooldownEffect(
				AsEffectSource(),
				null,
				caster,
				caster,
				GetEffectOnCaster().m_effectData,
				GetCooldownOnEnd(),
				GetEffectOnAnchorEnd(),
				m_unanchorAnimSequencePrefab);
			casterHitResults.AddEffect(effect);
		}
		MiscHitEventData_OverrideCooldown hitEvent = new MiscHitEventData_OverrideCooldown(caster.GetAbilityData().GetActionTypeOfAbility(this), 0);
		casterHitResults.AddMiscHitEvent(hitEvent);
		abilityResults.StoreActorHit(casterHitResults);
		abilityResults.StoreNonActorTargetInfo(list);
		CreateBarrierForLaser(aimDirection, caster, ref abilityResults);
	}

	// added in rogues
	private void CreateBarrierForLaser(Vector3 aimDirection, ActorData caster, ref AbilityResults abilityResults)
	{
		GetBarrierPosAndFacing(aimDirection, caster, out Vector3 barrierPos, out Vector3 barrierFacing, out float barrierSizeInSquares);
		StandardBarrierData laserBarrier = GetLaserBarrier();
		LinkedBarrierData linkData = new LinkedBarrierData();
		List<Barrier> list = new List<Barrier>();
		PositionHitResults positionHitResults = new PositionHitResults(new PositionHitParameters(barrierPos));
		for (int i = 0; i < 3; i++)
		{
			float num2 = 0f;
			if (i == 1)
			{
				num2 = -GameWideData.Get().m_actorTargetingRadiusInSquares * Board.Get().squareSize;
			}
			else if (i == 2)
			{
				num2 = GameWideData.Get().m_actorTargetingRadiusInSquares * Board.Get().squareSize;
			}
			Vector3 vector3 = barrierFacing.normalized * num2;
			string abilityName = m_abilityName;
			Vector3 center = barrierPos + vector3;
			Vector3 facingDir = (i == 1) ? (-barrierFacing) : barrierFacing;
			float width = barrierSizeInSquares;
			bool bidirectional = i == 0;
			BlockingRules blocksVision = laserBarrier.m_blocksVision;
			BlockingRules blocksAbilities = laserBarrier.m_blocksAbilities;
			BlockingRules blocksMovement = laserBarrier.m_blocksMovement;
			BlockingRules blocksPositionTargeting = laserBarrier.m_blocksPositionTargeting;
			int maxDuration = laserBarrier.m_maxDuration;
			List<GameObject> barrierSequencePrefabs;
			if (i != 0)
			{
				barrierSequencePrefabs = new List<GameObject>();
			}
			else
			{
				barrierSequencePrefabs = new List<GameObject> { m_persistentLaserBarrierSequence };
			}
			Barrier barrier = new Barrier(
				abilityName,
				center,
				facingDir,
				width,
				bidirectional,
				blocksVision,
				blocksAbilities,
				blocksMovement,
				blocksPositionTargeting,
				maxDuration,
				caster,
				barrierSequencePrefabs,
				true,
				laserBarrier.m_onEnemyMovedThrough,
				laserBarrier.m_onAllyMovedThrough,
				laserBarrier.m_maxHits,
				laserBarrier.m_endOnCasterDeath,
				abilityResults.SequenceSource)
				{
					m_removeAtPhaseEndIfCasterKnockedBack = false
				};
			barrier.SetSourceAbility(this);
			list.Add(barrier);
			if (ServerAbilityUtils.CurrentlyGatheringRealResults() && i == 0)
			{
				m_barrierInstance = barrier;
			}
			positionHitResults.AddBarrier(barrier);
		}
		BarrierManager.Get().LinkBarriers(list, linkData);
		abilityResults.StorePositionHit(positionHitResults);
	}

	// added in rogues
	public override bool ShouldBarrierHitThisMover(ActorData mover)
	{
		return m_passive == null
		       || m_passive.m_anchorLaserHitActorsThisTurn == null
		       || !m_passive.m_anchorLaserHitActorsThisTurn.Contains(mover);
	}

	// added in rogues
	public override int GetBarrierDamageForActor(int baseDamage, ActorData mover, Vector3 hitPos, Barrier barrier)
	{
		int result = baseDamage;
		if (barrier != null && barrier.Caster != null && !barrier.Caster.IsDead() && mover != null)
		{
			Vector3 endPos = barrier.GetEndPos2();
			result = GetTotalDamage(endPos, hitPos, baseDamage, true);
		}
		return result;
	}

	// added in rogues
	private void GetBarrierPosAndFacing(
		Vector3 aimDirection,
		ActorData caster,
		out Vector3 barrierPos,
		out Vector3 barrierFacing,
		out float barrierSizeInSquares)
	{
		Vector3 laserEndPoint = VectorUtils.GetLaserEndPoint(
			caster.GetLoSCheckPos(),
			aimDirection,
			GetLaserBarrier().m_width * Board.Get().m_squareSize,
			GetLaserInfo().penetrateLos,
			caster);
		barrierSizeInSquares = (caster.GetFreePos() - laserEndPoint).magnitude / Board.Get().m_squareSize;
		barrierPos = (caster.GetFreePos() + laserEndPoint) / 2f;
		barrierPos.y = caster.GetFreePos().y;
		barrierFacing = Vector3.Cross(Vector3.up, aimDirection);
	}

	// added in rogues
	private Vector3 GetTargeterClampedAimDirection(Vector3 startAimDirection, Vector3 endAimDirection, out float sweepAngle, out float coneCenterDegrees)
	{
		float num = VectorUtils.HorizontalAngle_Deg(startAimDirection);
		sweepAngle = Vector3.Angle(startAimDirection, endAimDirection);
		float maxConeAngle = GetMaxConeAngle();
		float minConeAngle = GetMinConeAngle();
		if (maxConeAngle > 0f && sweepAngle > maxConeAngle)
		{
			endAimDirection = Vector3.RotateTowards(endAimDirection, startAimDirection, 0.0174532924f * (sweepAngle - maxConeAngle), 0f);
			sweepAngle = maxConeAngle;
		}
		else if (minConeAngle > 0f && sweepAngle < minConeAngle)
		{
			endAimDirection = Vector3.RotateTowards(endAimDirection, startAimDirection, 0.0174532924f * (sweepAngle - minConeAngle), 0f);
			sweepAngle = minConeAngle;
		}
		coneCenterDegrees = num;
		if (Vector3.Cross(startAimDirection, endAimDirection).y > 0f)
		{
			coneCenterDegrees -= sweepAngle * 0.5f;
		}
		else
		{
			coneCenterDegrees += sweepAngle * 0.5f;
		}
		return endAimDirection;
	}

	// added in rogues
	public List<ActorData> GetSweepHitActors(Vector3 sweepStartAimDirection, Vector3 sweepEndAimDirection, ActorData caster, List<NonActorTargetInfo> nonActorTargetInfo)
	{
		float num = VectorUtils.HorizontalAngle_Deg(sweepStartAimDirection);
		float num2 = Vector3.Angle(sweepStartAimDirection, sweepEndAimDirection);
		if (Vector3.Cross(sweepStartAimDirection, sweepEndAimDirection).y > 0f)
		{
			num -= num2 * 0.5f;
		}
		else
		{
			num += num2 * 0.5f;
		}
		List<ActorData> list = AreaEffectUtils.GetActorsInCone(
			caster.GetFreePos(),
			num,
			num2,
			GetLaserInfo().range,
			GetSweepConeBackwardOffset(),
			GetLaserInfo().penetrateLos,
			caster,
			caster.GetOtherTeams(),
			nonActorTargetInfo);
		if (m_passive != null && m_passive.m_persistingBarrierInstance != null)
		{
			for (int i = list.Count - 1; i >= 0; i--)
			{
				if (m_passive.m_persistingBarrierInstance.ActorMovedThroughThisTurn(list[i]))
				{
					list.RemoveAt(i);
				}
			}
		}
		return list;
	}

	// added in rogues
	public override void OnExecutedActorHit_General(ActorData caster, ActorData target, ActorHitResults results)
	{
		if (results.FinalDamage > 0)
		{
			caster.GetFreelancerStats().AddToValueOfStat(FreelancerStats.ExoStats.UltDamage, results.FinalDamage);
		}
		if (caster == target && m_passive != null)
		{
			int valueOfStat = caster.GetFreelancerStats().GetValueOfStat(FreelancerStats.ExoStats.MaxConsecutiveUltSweeps);
			int currentConsecutiveSweeps = m_passive.m_currentConsecutiveSweeps;
			if (currentConsecutiveSweeps > valueOfStat)
			{
				caster.GetFreelancerStats().SetValueOfStat(FreelancerStats.ExoStats.MaxConsecutiveUltSweeps, currentConsecutiveSweeps);
			}
		}
	}
#endif
}

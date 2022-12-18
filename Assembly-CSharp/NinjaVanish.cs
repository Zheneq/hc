// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

public class NinjaVanish : Ability
{
	[Separator("Evade/Move settings")]
	public bool m_canQueueMoveAfterEvade;
	[Header("-- Whether to skip dash (if want to be a combat ability, etc) --")]
	public bool m_skipEvade;
	[Separator("Self Hit - Effects / Heal on Turn Start")]
	public StandardEffectInfo m_effectOnSelf;
	public StandardEffectInfo m_selfEffectOnNextTurn;
	[Header("-- Heal on Self on next turn start if inside field --")]
	public int m_selfHealOnTurnStartIfInField;
	[Separator("Initial Cast Hit On Enemy")]
	public StandardEffectInfo m_effectOnEnemy;
	[Separator("Duration for barrier and ground effect")]
	public int m_smokeFieldDuration = 2;
	[Separator("Vision Blocking Barrier")]
	public float m_barrierWidth = 3f;
	public StandardBarrierData m_visionBlockBarrierData;
	[Separator("Ground Effect")]
	public GroundEffectField m_groundEffectData;
	[Separator("Cooldown Reduction if only ability used in turn")]
	public int m_cdrIfOnlyAbilityUsed;
	public bool m_cdrConsiderCatalyst;
	[Header("-- Sequences --")]
	public GameObject m_castSequencePrefab;
	
	private AbilityMod_NinjaVanish m_abilityMod;
	private TargetData[] m_noEvadeTargetData = new TargetData[0];
	private StandardEffectInfo m_cachedEffectOnSelf;
	private StandardEffectInfo m_cachedEffectOnEnemy;
	private StandardEffectInfo m_cachedSelfEffectOnNextTurn;
	private StandardBarrierData m_cachedVisionBlockBarrierData;
	private GroundEffectField m_cachedGroundEffectData;

#if SERVER
	// added in rogues
	private Passive_Ninja m_passive;
	// added in rogues
	private AbilityData m_abilityData;
	// added in rogues
	private AbilityData.ActionType m_myActionType = AbilityData.ActionType.INVALID_ACTION;
#endif

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "NinjaVanish";
		}
		Setup();
	}

	private void Setup()
	{
		SetCachedFields();
#if SERVER
		// added in rogues
		m_passive = GetPassiveOfType(typeof(Passive_Ninja)) as Passive_Ninja;
		m_abilityData = GetComponent<AbilityData>();
		if (m_abilityData != null)
		{
			m_myActionType = m_abilityData.GetActionTypeOfAbility(this);
		}
#endif
		if (SkipEvade())
		{
			Targeter = new AbilityUtil_Targeter_Shape(
				this,
				GetGroundEffectData() != null
					? GetGroundEffectData().shape
					: AbilityAreaShape.SingleSquare, 
				true,
				AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape,
				GetEffectOnEnemy().m_applyEffect, 
				false,
				AbilityUtil_Targeter.AffectsActor.Always)
			{
				m_customCenterPosDelegate = GetCenterPosForTargeter
			};
		}
		else
		{
			Targeter = new AbilityUtil_Targeter_Charge(
				this, 
				AbilityAreaShape.SingleSquare,
				false,
				AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape,
				false);
			Targeter.SetShowArcToShape(false);
		}
	}

	public override string GetSetupNotesForEditor()
	{
		return "<color=cyan>-- For Design --</color>\nIf using [Skip Evade] option, please set phase to one that is not Evasion.";
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return !SkipEvade() && base.CanShowTargetableRadiusPreview();
	}

	public override TargetData[] GetTargetData()
	{
		return SkipEvade()
			? m_noEvadeTargetData
			: base.GetTargetData();
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod.AddToken_EffectInfo(tokens, m_effectOnSelf, "EffectOnSelf", m_effectOnSelf);
		AbilityMod.AddToken_EffectInfo(tokens, m_selfEffectOnNextTurn, "SelfEffectOnNextTurn", m_selfEffectOnNextTurn);
		AddTokenInt(tokens, "SelfHealOnTurnStartIfInField", string.Empty, m_selfHealOnTurnStartIfInField);
		AbilityMod.AddToken_EffectInfo(tokens, m_effectOnEnemy, "EffectOnEnemy", m_effectOnEnemy);
		AddTokenInt(tokens, "SmokeFieldDuration", string.Empty, m_smokeFieldDuration);
		m_visionBlockBarrierData.AddTooltipTokens(tokens, "VisionBlockBarrierData");
		AddTokenInt(tokens, "CdrIfOnlyAbilityUsed", string.Empty, m_cdrIfOnlyAbilityUsed);
	}

	private void SetCachedFields()
	{
		m_cachedEffectOnSelf = m_abilityMod != null
			? m_abilityMod.m_effectOnSelfMod.GetModifiedValue(m_effectOnSelf)
			: m_effectOnSelf;
		m_cachedEffectOnEnemy = m_abilityMod != null
			? m_abilityMod.m_effectOnEnemyMod.GetModifiedValue(m_effectOnEnemy)
			: m_effectOnEnemy;
		m_cachedSelfEffectOnNextTurn = m_abilityMod != null
			? m_abilityMod.m_selfEffectOnNextTurnMod.GetModifiedValue(m_selfEffectOnNextTurn)
			: m_selfEffectOnNextTurn;
		// added in rogues
		// if (m_visionBlockBarrierData == null)
		// {
		// 	m_visionBlockBarrierData = ScriptableObject.CreateInstance<StandardBarrierData>();
		// }
		m_cachedVisionBlockBarrierData = m_abilityMod != null
			? m_abilityMod.m_visionBlockBarrierDataMod.GetModifiedValue(m_visionBlockBarrierData)
			: m_visionBlockBarrierData;
		// added in rogues
		// if (m_groundEffectData == null)
		// {
		// 	m_groundEffectData = ScriptableObject.CreateInstance<GroundEffectField>();
		// }
		m_cachedGroundEffectData = m_abilityMod != null
			? m_abilityMod.m_groundEffectDataMod.GetModifiedValue(m_groundEffectData)
			: m_groundEffectData;
		int smokeFieldDuration = Mathf.Max(1, GetSmokeFieldDuration());
		m_cachedVisionBlockBarrierData.m_maxDuration = smokeFieldDuration;
		m_cachedGroundEffectData.duration = smokeFieldDuration;
	}

	public bool CanQueueMoveAfterEvade()
	{
		return m_abilityMod != null
			? m_abilityMod.m_canQueueMoveAfterEvadeMod.GetModifiedValue(m_canQueueMoveAfterEvade)
			: m_canQueueMoveAfterEvade;
	}

	public bool SkipEvade()
	{
		return m_abilityMod != null
			? m_abilityMod.m_skipEvadeMod.GetModifiedValue(m_skipEvade)
			: m_skipEvade;
	}

	public StandardEffectInfo GetEffectOnSelf()
	{
		return m_cachedEffectOnSelf ?? m_effectOnSelf;
	}

	public StandardEffectInfo GetSelfEffectOnNextTurn()
	{
		return m_cachedSelfEffectOnNextTurn ?? m_selfEffectOnNextTurn;
	}

	public int GetSelfHealOnTurnStartIfInField()
	{
		return m_abilityMod != null
			? m_abilityMod.m_selfHealOnTurnStartIfInFieldMod.GetModifiedValue(m_selfHealOnTurnStartIfInField)
			: m_selfHealOnTurnStartIfInField;
	}

	public StandardEffectInfo GetEffectOnEnemy()
	{
		return m_cachedEffectOnEnemy ?? m_effectOnEnemy;
	}

	public int GetSmokeFieldDuration()
	{
		return m_abilityMod != null
			? m_abilityMod.m_smokeFieldDurationMod.GetModifiedValue(m_smokeFieldDuration)
			: m_smokeFieldDuration;
	}

	public float GetBarrierWidth()
	{
		return m_abilityMod != null
			? m_abilityMod.m_barrierWidthMod.GetModifiedValue(m_barrierWidth)
			: m_barrierWidth;
	}

	public StandardBarrierData GetVisionBlockBarrierData()
	{
		return m_cachedVisionBlockBarrierData ?? m_visionBlockBarrierData;
	}

	public GroundEffectField GetGroundEffectData()
	{
		return m_cachedGroundEffectData ?? m_groundEffectData;
	}

	public int GetCdrIfOnlyAbilityUsed()
	{
		return m_abilityMod != null
			? m_abilityMod.m_cdrIfOnlyAbilityUsedMod.GetModifiedValue(m_cdrIfOnlyAbilityUsed)
			: m_cdrIfOnlyAbilityUsed;
	}

	public bool CdrConsiderCatalyst()
	{
		return m_abilityMod != null
			? m_abilityMod.m_cdrConsiderCatalystMod.GetModifiedValue(m_cdrConsiderCatalyst)
			: m_cdrConsiderCatalyst;
	}

	internal override bool IsStealthEvade()
	{
		return true;
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return SkipEvade()
			? ActorData.MovementType.None
			: ActorData.MovementType.Charge;
	}

	public override bool CanOverrideMoveStartSquare()
	{
		return CanQueueMoveAfterEvade() && !SkipEvade();
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		if (SkipEvade())
		{
			return true;
		}
		BoardSquare targetSquare = Board.Get().GetSquare(target.GridPos);
		return targetSquare != null
		       && targetSquare.IsValidForGameplay()
		       && targetSquare != caster.GetCurrentBoardSquare();
	}

	private Vector3 GetCenterPosForTargeter(ActorData caster, AbilityTarget currentTarget)
	{
		Vector3 result = caster.GetFreePos();
		if (caster.GetActorTargeting() != null && GetRunPriority() > AbilityPriority.Evasion)
		{
			BoardSquare evadeDestinationForTargeter = caster.GetActorTargeting().GetEvadeDestinationForTargeter();
			if (evadeDestinationForTargeter != null)
			{
				result = evadeDestinationForTargeter.ToVector3();
			}
		}
		return result;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		return new List<AbilityTooltipNumber>();
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_NinjaVanish))
		{
			m_abilityMod = abilityMod as AbilityMod_NinjaVanish;
			Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}
	
#if SERVER
	// added in rogues
	public override BoardSquare GetModifiedMoveStartSquare(ActorData caster, List<AbilityTarget> targets)
	{
		BoardSquare square = Board.Get().GetSquare(targets[0].GridPos);
		if (square != null)
		{
			return square;
		}
		return base.GetModifiedMoveStartSquare(caster, targets);
	}

	// added in rogues
	public override void Run(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		if (m_passive != null && GetSelfEffectOnNextTurn().m_applyEffect)
		{
			m_passive.m_vanishSelfEffectOnTurnStart = GetSelfEffectOnNextTurn();
		}
	}

	// added in rogues
	public override List<ServerClientUtils.SequenceStartData> GetAbilityRunSequenceStartDataList(
		List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		BoardSquare square = caster.GetSquareAtPhaseStart();
		if (square == null)
		{
			square = caster.GetCurrentBoardSquare();
		}
		return new List<ServerClientUtils.SequenceStartData>
		{
			new ServerClientUtils.SequenceStartData(
				m_castSequencePrefab,
				square.ToVector3(),
				additionalData.m_abilityResults.HitActorsArray(),
				caster,
				additionalData.m_sequenceSource)
		};
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(caster, caster.GetFreePos()));
		actorHitResults.AddStandardEffectInfo(GetEffectOnSelf());
		if (GetCdrIfOnlyAbilityUsed() > 0 && m_abilityData != null)
		{
			bool flag = false;
			int num = CdrConsiderCatalyst() ? AbilityData.LAST_CARD : AbilityData.ABILITY_4;
			for (int i = 0; i <= num && !flag; i++)
			{
				if (i != (int)m_myActionType && m_abilityData.HasQueuedAction((AbilityData.ActionType)i))  // , true in rogues
				{
					flag = true;
				}
			}
			if (!flag)
			{
				actorHitResults.AddMiscHitEvent(new MiscHitEventData_AddToCasterCooldown(m_myActionType, -1 * GetCdrIfOnlyAbilityUsed()));
			}
		}
		abilityResults.StoreActorHit(actorHitResults);
		BoardSquare casterSquare = caster.GetSquareAtPhaseStart();
		if (casterSquare == null)
		{
			casterSquare = caster.GetCurrentBoardSquare();
		}
		Vector3 vector = casterSquare.ToVector3();
		PositionHitResults positionHitResults = new PositionHitResults(new PositionHitParameters(vector));
		Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(GetGroundEffectData().shape, vector, casterSquare);
		float radiusInWorld = 0.5f * GetBarrierWidth() * Board.Get().squareSize;
		List<BarrierPoseInfo> barrierPosesForRegularPolygon = BarrierPoseInfo.GetBarrierPosesForRegularPolygon(centerOfShape, 4, radiusInWorld);
		if (barrierPosesForRegularPolygon != null)
		{
			GetVisionBlockBarrierData().m_width = GetBarrierWidth() + 0.05f;
			foreach (BarrierPoseInfo barrierPoseInfo in barrierPosesForRegularPolygon)
			{
				Barrier barrier = new Barrier(m_abilityName, barrierPoseInfo.midpoint, barrierPoseInfo.facingDirection, caster, GetVisionBlockBarrierData());
				barrier.SetSourceAbility(this);
				positionHitResults.AddBarrier(barrier);
			}
		}
		NinjaVanishGroundEffect effect = new NinjaVanishGroundEffect(AsEffectSource(), casterSquare, vector, null, caster, GetGroundEffectData(), GetSelfHealOnTurnStartIfInField());
		positionHitResults.AddEffect(effect);
		abilityResults.StorePositionHit(positionHitResults);
		if (GetEffectOnEnemy().m_applyEffect)
		{
			foreach (ActorData actorData in GetInitialHitActors(targets, caster))
			{
				ActorHitResults actorHitResults2 = new ActorHitResults(new ActorHitParameters(actorData, actorData.GetFreePos()));
				actorHitResults2.AddStandardEffectInfo(GetEffectOnEnemy());
				abilityResults.StoreActorHit(actorHitResults2);
			}
		}
	}

	// added in rogues
	private List<ActorData> GetInitialHitActors(List<AbilityTarget> targets, ActorData caster)
	{
		BoardSquare squareAtPhaseStart = caster.GetSquareAtPhaseStart();
		if (squareAtPhaseStart != null)
		{
			return AreaEffectUtils.GetActorsInShape(
				GetGroundEffectData().shape,
				squareAtPhaseStart.ToVector3(),
				squareAtPhaseStart,
				GetGroundEffectData().penetrateLos,
				caster,
				caster.GetOtherTeams(),
				null);
		}
		return new List<ActorData>();
	}

	// added in rogues
	public override void OnExecutedActorHit_General(ActorData caster, ActorData target, ActorHitResults results)
	{
		if (results.HasHitResultsTag(HitResultsTags.DeathmarkDetonation))
		{
			caster.GetFreelancerStats().IncrementValueOfStat(FreelancerStats.TeleportingNinjaStats.NumDetonationsOfMark);
		}
	}
#endif
}

// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

public class MantaCreateBarriers : Ability
{
	[Header("-- Whether require Manta to be inside target area --")]
	public bool m_requireCasterInShape = true;
	public AbilityAreaShape m_targetAreaShape = AbilityAreaShape.Five_x_Five;
	[Header("-- Barriers")]
	[Separator("NOTE: you can also use MantaCreateBarriersChainFinal for damage stuff!")]
	public bool m_delayBarriersUntilStartOfNextTurn;
	public int m_prisonSides = 8;
	public float m_prisonRadius = 3.5f;
	public StandardBarrierData m_prisonBarrierData;
	public AbilityAreaShape m_shapeForTargeter = AbilityAreaShape.Seven_x_Seven;
	[Tooltip("WARNING: don't do this if it's a Blast phase ability unless the walls don't block abilities")]
	public bool m_createBarriersImmediately;
	[Header("-- Ground effect")]
	public StandardGroundEffectInfo m_groundEffectInfo;
	public int m_damageOnCast = 30;
	[Header("-- On Cast Ally Hit (applies to caster as well)")]
	public int m_allyHealOnCast;
	public StandardEffectInfo m_effectOnAlliesOnCast;
	[Header("-- Sequences -------------------------------------------------")]
	public GameObject m_castSequencePrefab;

	private Manta_SyncComponent m_syncComp;
	private AbilityMod_MantaCreateBarriers m_abilityMod;
	private MantaCreateBarriersChainFinal m_finalDamageChain;
	private StandardBarrierData m_cachedPrisonBarrierData;
	private StandardEffectInfo m_cachedEffectOnAlliesOnCast;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Lair";
		}
		if (m_prisonSides < 3)
		{
			m_prisonSides = 4;
		}
		foreach (Ability ability in GetChainAbilities())
		{
			if (ability != null && ability is MantaCreateBarriersChainFinal final)
			{
				m_finalDamageChain = final;
				break;
			}
		}
		m_syncComp = GetComponent<Manta_SyncComponent>();
		Setup();
		ResetTooltipAndTargetingNumbers();
	}

	private void Setup()
	{
		SetCachedFields();
		Targeter = new AbilityUtil_Targeter_TeslaPrison(
			this,
			TrackerTeslaPrison.PrisonWallSegmentType.RegularPolygon,
			0,
			0,
			GetPrisonSides(),
			GetPrisonRadius(),
			GetShapeForTargeter(),
			true);
		Targeter.SetAffectedGroups(true, IncludeAllies(), false);
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return GetPrisonRadius();
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		m_prisonBarrierData.AddTooltipTokens(tokens, "PrisonBarrierData");
		AddTokenInt(tokens, "DamageOnCast", string.Empty, m_damageOnCast);
		AddTokenInt(tokens, "AllyHealOnCast", string.Empty, m_allyHealOnCast);
		AbilityMod.AddToken_EffectInfo(tokens, m_effectOnAlliesOnCast, "EffectOnAlliesOnCast", m_effectOnAlliesOnCast);
	}

	private void SetCachedFields()
	{
		// added in rogues
		// if (m_prisonBarrierData == null)
		// {
		// 	m_prisonBarrierData = ScriptableObject.CreateInstance<StandardBarrierData>();
		// }
		m_cachedPrisonBarrierData = m_abilityMod != null
			? m_abilityMod.m_prisonBarrierDataMod.GetModifiedValue(m_prisonBarrierData)
			: m_prisonBarrierData;
		m_cachedEffectOnAlliesOnCast = m_abilityMod != null
			? m_abilityMod.m_effectOnAlliesOnCastMod.GetModifiedValue(m_effectOnAlliesOnCast)
			: m_effectOnAlliesOnCast;
	}

	public bool RequireCasterInShape()
	{
		return m_abilityMod != null
			? m_abilityMod.m_requireCasterInShapeMod.GetModifiedValue(m_requireCasterInShape)
			: m_requireCasterInShape;
	}

	public AbilityAreaShape GetTargetAreaShape()
	{
		return m_abilityMod != null
			? m_abilityMod.m_targetAreaShapeMod.GetModifiedValue(m_targetAreaShape)
			: m_targetAreaShape;
	}

	public bool DelayBarriersUntilStartOfNextTurn()
	{
		return m_abilityMod != null
			? m_abilityMod.m_delayBarriersUntilStartOfNextTurnMod.GetModifiedValue(m_delayBarriersUntilStartOfNextTurn)
			: m_delayBarriersUntilStartOfNextTurn;
	}

	public int GetPrisonSides()
	{
		return m_abilityMod != null
			? m_abilityMod.m_prisonSidesMod.GetModifiedValue(m_prisonSides)
			: m_prisonSides;
	}

	public float GetPrisonRadius()
	{
		return m_abilityMod != null
			? m_abilityMod.m_prisonRadiusMod.GetModifiedValue(m_prisonRadius)
			: m_prisonRadius;
	}

	public AbilityAreaShape GetShapeForTargeter()
	{
		return m_abilityMod != null
			? m_abilityMod.m_shapeForTargeterMod.GetModifiedValue(m_shapeForTargeter)
			: m_shapeForTargeter;
	}

	public bool CreateBarriersImmediately()
	{
		return m_abilityMod != null
			? m_abilityMod.m_createBarriersImmediatelyMod.GetModifiedValue(m_createBarriersImmediately)
			: m_createBarriersImmediately;
	}

	public StandardGroundEffectInfo GetGroundEffectInfo()
	{
		return m_abilityMod != null && m_abilityMod.m_groundEffectInfoMod.m_applyGroundEffect
			? m_abilityMod.m_groundEffectInfoMod
			: m_groundEffectInfo;
	}

	public int GetDamageOnCast()
	{
		return m_abilityMod != null
			? m_abilityMod.m_damageOnCastMod.GetModifiedValue(m_damageOnCast)
			: m_damageOnCast;
	}

	public int GetAllyHealOnCast()
	{
		return m_abilityMod != null
			? m_abilityMod.m_allyHealOnCastMod.GetModifiedValue(m_allyHealOnCast)
			: m_allyHealOnCast;
	}

	public StandardEffectInfo GetEffectOnAlliesOnCast()
	{
		return m_cachedEffectOnAlliesOnCast ?? m_effectOnAlliesOnCast;
	}

	private StandardBarrierData GetPrisonBarrierData()
	{
		return m_cachedPrisonBarrierData ?? m_prisonBarrierData;
	}

	private bool ShouldAddVisionProvider()
	{
		return m_abilityMod != null 
		       && m_abilityMod.m_addVisionProviderInsideBarriers.GetModifiedValue(false);
	}

	public bool IncludeAllies()
	{
		return GetAllyHealOnCast() > 0 || GetEffectOnAlliesOnCast().m_applyEffect;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_MantaCreateBarriers))
		{
			m_abilityMod = abilityMod as AbilityMod_MantaCreateBarriers;
			Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		if (m_finalDamageChain != null)
		{
			numbers.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Enemy, m_finalDamageChain.GetDamageOnCast()));
		}
		else
		{
			numbers.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Enemy, GetDamageOnCast()));
			m_groundEffectInfo.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Enemy, AbilityTooltipSubject.Ally);
			GetEffectOnAlliesOnCast().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Ally);
			GetEffectOnAlliesOnCast().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Self);
			AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Ally, GetAllyHealOnCast());
			AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Self, GetAllyHealOnCast());
		}
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		int damage = m_finalDamageChain != null
			? m_finalDamageChain.GetDamageOnCast()
			: GetDamageOnCast();
		if (GetGroundEffectInfo().m_applyGroundEffect && GetGroundEffectInfo().m_groundEffectData.damageAmount > 0)
		{
			damage += GetGroundEffectInfo().m_groundEffectData.damageAmount;
		}
// #if SERVER
// 		// added in rogues
// 		if (m_syncComp != null)
// 		{
// 			damage += m_syncComp.GetDirtyFightingExtraDamage(targetActor);
// 		}
// #endif
		dictionary[AbilityTooltipSymbol.Damage] = damage;
		return dictionary;
	}

	public override int GetAdditionalTechPointGainForNameplateItem(ActorData caster, int currentTargeterIndex)
	{
		if (m_syncComp == null)
		{
			return base.GetAdditionalTechPointGainForNameplateItem(caster, currentTargeterIndex);
		}
		
		int energy = 0;
		foreach (AbilityUtil_Targeter.ActorTarget actorTarget in Targeters[currentTargeterIndex].GetActorsInRange())
		{
			energy += m_syncComp.GetDirtyFightingExtraTP(actorTarget.m_actor);
		}
		return energy;
	}

	// removed in rogues
	public override string GetAccessoryTargeterNumberString(ActorData targetActor, AbilityTooltipSymbol symbolType, int baseValue)
	{
		return symbolType == AbilityTooltipSymbol.Damage && m_syncComp != null
			? m_syncComp.GetAccessoryStringForDamage(targetActor, ActorData, this)
			: null;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		if (!m_requireCasterInShape || caster.GetCurrentBoardSquare() == null)
		{
			return true;
		}
		BoardSquare targetSquare = Board.Get().GetSquare(target.GridPos);
		if (targetSquare != null)
		{
			return AreaEffectUtils.IsSquareInShape(
				caster.GetCurrentBoardSquare(),
				GetTargetAreaShape(),
				target.FreePos,
				targetSquare,
				true,
				caster);
		}
		return false;
	}

	public override bool AllowInvalidSquareForSquareBasedTarget()
	{
		return true;
	}
	
#if SERVER
	// added in rogues
	public override List<ServerClientUtils.SequenceStartData> GetAbilityRunSequenceStartDataList(
		List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		List<ServerClientUtils.SequenceStartData> abilityRunSequenceStartDataList =
			base.GetAbilityRunSequenceStartDataList(targets, caster, additionalData);
		Vector3 targetPos = Board.Get().GetSquare(targets[0].GridPos).ToVector3();
		targetPos.y = Board.Get().BaselineHeight;
		abilityRunSequenceStartDataList.Add(new ServerClientUtils.SequenceStartData(
			m_castSequencePrefab,
			targetPos,
			additionalData.m_abilityResults.HitActorsArray(),
			caster,
			additionalData.m_sequenceSource));
		return abilityRunSequenceStartDataList;
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		BoardSquare targetSquare = Board.Get().GetSquare(targets[0].GridPos);
		Vector3 targetPos = targetSquare != null
			? targetSquare.ToVector3()
			: targets[0].FreePos;
		targetPos.y = Board.Get().BaselineHeight;
		float squareSize = Board.Get().squareSize;
		PositionHitResults positionHitResults = new PositionHitResults(new PositionHitParameters(targetPos));
		List<BarrierPoseInfo> barrierPosesForRegularPolygon =
			BarrierPoseInfo.GetBarrierPosesForRegularPolygon(targetPos, m_prisonSides, m_prisonRadius * squareSize);
		GetPrisonBarrierData().m_width = barrierPosesForRegularPolygon[0].widthInWorld / squareSize + 0.25f;
		foreach (BarrierPoseInfo barrierPose in barrierPosesForRegularPolygon)
		{
			Barrier barrier = new Barrier(
				m_abilityName, barrierPose.midpoint, -1f * barrierPose.facingDirection, caster, GetPrisonBarrierData());
			barrier.SetSourceAbility(this);
			positionHitResults.AddBarrier(barrier);
		}
		if (ShouldAddVisionProvider())
		{
			PositionVisionProviderEffect effect = new PositionVisionProviderEffect(
				AsEffectSource(),
				targetSquare,
				caster,
				GetPrisonBarrierData().m_maxDuration,
				m_prisonRadius + 0.5f,
				VisionProviderInfo.BrushRevealType.Always,
				true,
				null);
			positionHitResults.AddEffect(effect);
		}
		abilityResults.StorePositionHit(positionHitResults);
		StandardGroundEffectInfo groundEffectInfo = GetGroundEffectInfo();
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
		GroundEffectField groundEffectData = groundEffectInfo.m_groundEffectData;
		List<Team> affectedTeams = new List<Team>();
		affectedTeams.AddRange(caster.GetOtherTeams());
		if (IncludeAllies())
		{
			affectedTeams.Add(caster.GetTeam());
		}
		List<ActorData> actorsInShape = AreaEffectUtils.GetActorsInShape(
			groundEffectInfo.m_groundEffectData.shape, targets[0], true, caster, affectedTeams, nonActorTargetInfo);
		Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(groundEffectData.shape, targets[0]);
		centerOfShape.y = Board.Get().m_baselineHeight;
		ActorHitResults casterHitResults = new ActorHitResults(new ActorHitParameters(caster, caster.GetFreePos()));
		foreach (ActorData actorData in actorsInShape)
		{
			bool isNotCaster = true;
			ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(actorData, centerOfShape));
			if (groundEffectInfo.m_applyGroundEffect)
			{
				groundEffectInfo.SetupActorHitResult(ref actorHitResults, caster, actorData.GetCurrentBoardSquare());
			}
			if (actorData.GetTeam() == caster.GetTeam())
			{
				if (actorData == caster && groundEffectInfo.m_applyGroundEffect)
				{
					casterHitResults.SetBaseHealing(GetAllyHealOnCast());
					casterHitResults.AddStandardEffectInfo(GetEffectOnAlliesOnCast());
					isNotCaster = false;
				}
				else
				{
					actorHitResults.SetBaseHealing(GetAllyHealOnCast());
					actorHitResults.AddStandardEffectInfo(GetEffectOnAlliesOnCast());
				}
			}
			else if (GetDamageOnCast() > 0)
			{
				actorHitResults.AddBaseDamage(GetDamageOnCast());
			}
			if (isNotCaster)
			{
				abilityResults.StoreActorHit(actorHitResults);
			}
		}
		abilityResults.StoreNonActorTargetInfo(nonActorTargetInfo);
		if (groundEffectInfo.m_applyGroundEffect)
		{
			StandardGroundEffect standardGroundEffect =
				new StandardGroundEffect(AsEffectSource(), targetSquare, centerOfShape, null, caster, groundEffectData);
			standardGroundEffect.AddToActorsHitThisTurn(actorsInShape);
			casterHitResults.AddEffect(standardGroundEffect);
			abilityResults.StoreActorHit(casterHitResults);
		}
	}

	// added in rogues
	public override void OnExecutedActorHit_Ability(ActorData caster, ActorData target, ActorHitResults results)
	{
		if (caster.GetTeam() != target.GetTeam())
		{
			caster.GetFreelancerStats().IncrementValueOfStat(FreelancerStats.MantaStats.NumEnemiesHitByUltCast);
		}
	}
#endif
}

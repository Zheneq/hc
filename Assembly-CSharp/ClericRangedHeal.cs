using System.Collections.Generic;
using UnityEngine;

public class ClericRangedHeal : Ability
{
	public enum ExtraHealApplyTiming
	{
		CombatEndOfInitialTurn,
		PrepPhaseOfNextTurn
	}

	[Separator("On Hit Heal/Effect")]
	public int m_healAmount = 30;
	public int m_selfHealIfTargetingAlly = 15;
	public StandardEffectInfo m_targetHitEffect;
	[Separator("Extra Heal Based on Enemy Hits")]
	public ExtraHealApplyTiming m_extraHealApplyTiming; // TODO CLERIC unused (not used in the ability or any of the mods), CombatEndOfInitialTurn
	public int m_extraHealOnEnemyHit; // TODO CLERIC unused (not used in the ability or any of the mods)
	public int m_extraHealOnSubseqEnemyHit; // TODO CLERIC unused (not used in the ability or any of the mods)
	[Separator("Extra Heal Based on Current Health")]
	public float m_healPerPercentHealthLost;
	[Separator("On Self")]
	public StandardEffectInfo m_effectOnSelf;
	[Separator("Effect in Radius")]
	public float m_enemyDebuffRadiusAroundTarget;
	public float m_enemyDebuffRadiusAroundCaster;
	public bool m_enemyDebuffRadiusIgnoreLoS;
	public StandardEffectInfo m_enemyDebuffInRadiusEffect;
	[Separator("Reactions")]
	public StandardEffectInfo m_reactionEffectForHealTarget; // TODO CLERIC unused (not used in the ability or any of the mods)
	public StandardEffectInfo m_reactionEffectForCaster; // TODO CLERIC unused (not used in the ability or any of the mods)
	[Separator("Sequences")]
	public GameObject m_castSequencePrefab;
	public GameObject m_reactionProjectileSequencePrefab; // TODO CLERIC unused (not used in the ability or any of the mods)
	[Header("-- For Extra Heal Effect, if Extra Heal On Enemy Hit is used")]
	public GameObject m_extraHealPersistentSeqPrefab; // TODO CLERIC unused (not used in the ability or any of the mods)
	public GameObject m_extraHealTriggerSeqPrefab; // TODO CLERIC unused (not used in the ability or any of the mods)

	private AbilityMod_ClericRangedHeal m_abilityMod;
	private ClericAreaBuff m_buffAbility;
	private StandardEffectInfo m_cachedTargetHitEffect;
	private StandardEffectInfo m_cachedEffectOnSelf;
	private StandardEffectInfo m_cachedReactionEffectForHealTarget;
	private StandardEffectInfo m_cachedReactionEffectForCaster;
	private StandardEffectInfo m_cachedEnemyDebuffInRadiusEffect;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Cleric Ranged Heal";
		}
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		SetCachedFields();
		m_buffAbility = GetAbilityOfType(typeof(ClericAreaBuff)) as ClericAreaBuff;
		if (GetEnemyDebuffRadiusAroundCaster() > 0f)
		{
			Targeter = new AbilityUtil_Targeter_AoE_AroundActor(
				this,
				GetEnemyDebuffRadiusAroundCaster(),
				m_enemyDebuffRadiusIgnoreLoS,
				true,
				false,
				-1,
				false,
				false);
			Targeter.SetAffectedGroups(true, false, true);
			Targeter.SetShowArcToShape(false);
		}
		else
		{
			Targeter = new AbilityUtil_Targeter_AoE_AroundActor(
				this,
				GetEnemyDebuffRadiusAroundTarget(),
				m_enemyDebuffRadiusIgnoreLoS);
			Targeter.SetAffectedGroups(true, false, true);
			Targeter.SetShowArcToShape(true);
		}
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		return HasTargetableActorsInDecision(caster,
			       false,
			       true,
			       true,
			       ValidateCheckPath.Ignore,
			       m_targetData[0].m_checkLineOfSight,
			       false)
		       && base.CustomCanCastValidation(caster);
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		BoardSquare targetSquare = Board.Get().GetSquare(target.GridPos);
		return targetSquare != null
		       && targetSquare.OccupantActor != null
		       && targetSquare.OccupantActor.GetTeam() == caster.GetTeam()
		       && !targetSquare.OccupantActor.IgnoreForAbilityHits;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_ClericRangedHeal))
		{
			m_abilityMod = abilityMod as AbilityMod_ClericRangedHeal;
			SetupTargeter();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}

	private void SetCachedFields()
	{
		m_cachedTargetHitEffect = m_abilityMod != null
			? m_abilityMod.m_targetHitEffectMod.GetModifiedValue(m_targetHitEffect)
			: m_targetHitEffect;
		m_cachedEffectOnSelf = m_abilityMod != null
			? m_abilityMod.m_effectOnSelfMod.GetModifiedValue(m_effectOnSelf)
			: m_effectOnSelf;
		m_cachedReactionEffectForHealTarget = m_abilityMod != null
			? m_abilityMod.m_reactionEffectForHealTargetMod.GetModifiedValue(m_reactionEffectForHealTarget)
			: m_reactionEffectForHealTarget;
		m_cachedReactionEffectForCaster = m_abilityMod != null
			? m_abilityMod.m_reactionEffectForCasterMod.GetModifiedValue(m_reactionEffectForCaster)
			: m_reactionEffectForCaster;
		m_cachedEnemyDebuffInRadiusEffect = m_abilityMod != null
			? m_abilityMod.m_enemyDebuffInRadiusEffectMod.GetModifiedValue(m_enemyDebuffInRadiusEffect)
			: m_enemyDebuffInRadiusEffect;
	}

	public int GetHealAmount()
	{
		return m_abilityMod != null
			? m_abilityMod.m_healAmountMod.GetModifiedValue(m_healAmount)
			: m_healAmount;
	}

	public int GetSelfHealIfTargetingAlly()
	{
		return m_abilityMod != null
			? m_abilityMod.m_selfHealIfTargetingAllyMod.GetModifiedValue(m_selfHealIfTargetingAlly)
			: m_selfHealIfTargetingAlly;
	}

	public StandardEffectInfo GetTargetHitEffect()
	{
		return m_cachedTargetHitEffect ?? m_targetHitEffect;
	}

	// TODO CLERIC unused (not used in the ability or any of the mods)
	public int GetExtraHealOnEnemyHit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraHealOnEnemyHitMod.GetModifiedValue(m_extraHealOnEnemyHit)
			: m_extraHealOnEnemyHit;
	}

	// TODO CLERIC unused (not used in the ability or any of the mods)
	public int GetExtraHealOnSubseqEnemyHit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraHealOnSubseqEnemyHitMod.GetModifiedValue(m_extraHealOnSubseqEnemyHit)
			: m_extraHealOnSubseqEnemyHit;
	}

	public int GetExtraHealPerTargetDistance()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraHealPerTargetDistanceMod.GetModifiedValue(0)
			: 0;
	}

	public int GetSelfHealAdjustIfTargetingSelf()
	{
		return m_abilityMod != null
			? m_abilityMod.m_selfHealAdjustIfTargetingSelfMod.GetModifiedValue(0)
			: 0;
	}

	public float GetHealPerPercentHealthLost()
	{
		return m_abilityMod != null
			? m_abilityMod.m_healPerPercentHealthLostMod.GetModifiedValue(m_healPerPercentHealthLost)
			: m_healPerPercentHealthLost;
	}

	public StandardEffectInfo GetEffectOnSelf()
	{
		return m_cachedEffectOnSelf ?? m_effectOnSelf;
	}

	// TODO CLERIC unused (not used in the ability or any of the mods)
	public StandardEffectInfo GetReactionEffectForHealTarget()
	{
		return m_cachedReactionEffectForHealTarget ?? m_reactionEffectForHealTarget;
	}

	// TODO CLERIC unused (not used in the ability or any of the mods)
	public StandardEffectInfo GetReactionEffectForCaster()
	{
		return m_cachedReactionEffectForCaster ?? m_reactionEffectForCaster;
	}

	public float GetEnemyDebuffRadiusAroundTarget()
	{
		return m_abilityMod != null
			? m_abilityMod.m_enemyDebuffRadiusAroundTargetMod.GetModifiedValue(m_enemyDebuffRadiusAroundTarget)
			: m_enemyDebuffRadiusAroundTarget;
	}

	public float GetEnemyDebuffRadiusAroundCaster()
	{
		return m_abilityMod != null
			? m_abilityMod.m_enemyDebuffRadiusAroundCasterMod.GetModifiedValue(m_enemyDebuffRadiusAroundCaster)
			: m_enemyDebuffRadiusAroundCaster;
	}

	public StandardEffectInfo GetEnemyDebuffInRadiusEffect()
	{
		return m_cachedEnemyDebuffInRadiusEffect ?? m_enemyDebuffInRadiusEffect;
	}

	public int GetTechPointGainPerIncomingHit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_techPointGainPerIncomingHitThisTurn.GetModifiedValue(0)
			: 0;
	}

	public int CalcExtraHealFromMissingHealth(ActorData healTarget)
	{
		int extraHeal = 0;
		if (GetHealPerPercentHealthLost() > 0f && healTarget.HitPoints < healTarget.GetMaxHitPoints())
		{
			int percent = Mathf.CeilToInt((1f - healTarget.GetHitPointPercent()) * 100f);
			extraHeal = Mathf.RoundToInt(GetHealPerPercentHealthLost() * percent);
		}
		return extraHeal;
	}

	public int CalcFinalHealOnActor(ActorData forActor, ActorData caster, ActorData actorOnTargetedSquare)
	{
		int healing = 0;
		bool isSelf = caster == actorOnTargetedSquare;
		int baseHealingModded = GetHealAmount();
		int baseHealing = m_healAmount;
		if (forActor == caster && !isSelf)
		{
			baseHealingModded = GetSelfHealIfTargetingAlly();
			baseHealing = m_selfHealIfTargetingAlly;
		}
		if (baseHealing > baseHealingModded)
		{
			baseHealing = baseHealingModded;
		}
		int extraHealForDistance = 0;
		if (GetExtraHealPerTargetDistance() != 0 && !isSelf)
		{
			float dist = actorOnTargetedSquare.GetCurrentBoardSquare().HorizontalDistanceInSquaresTo(caster.GetCurrentBoardSquare());
			if (dist > 0f)
			{
				dist -= 1f;
			}
			extraHealForDistance += Mathf.RoundToInt(GetExtraHealPerTargetDistance() * dist);
		}
		healing = Mathf.Max(baseHealing, baseHealingModded + extraHealForDistance);
		if (isSelf)
		{
			healing = Mathf.Max(0, healing + GetSelfHealAdjustIfTargetingSelf());
		}
		healing += CalcExtraHealFromMissingHealth(forActor);
		if (m_buffAbility != null
		    && m_buffAbility.GetExtraHealForPurifyOnBuffedAllies() != 0
		    && m_buffAbility.IsActorInBuffShape(forActor))
		{
			healing += m_buffAbility.GetExtraHealForPurifyOnBuffedAllies();
		}
		return healing;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AddTokenInt(tokens, "HealAmount", string.Empty, m_healAmount);
		AddTokenInt(tokens, "SelfHealIfTargetingAlly", string.Empty, m_selfHealIfTargetingAlly);
		AbilityMod.AddToken_EffectInfo(tokens, m_targetHitEffect, "TargetHitEffect", m_targetHitEffect);
		AddTokenInt(tokens, "ExtraHealOnEnemyHit", string.Empty, m_extraHealOnEnemyHit);
		AddTokenInt(tokens, "ExtraHealOnSubseqEnemyHit", string.Empty, m_extraHealOnSubseqEnemyHit);
		AbilityMod.AddToken_EffectInfo(tokens, m_effectOnSelf, "EffectOnSelf", m_effectOnSelf);
		AbilityMod.AddToken_EffectInfo(tokens, m_reactionEffectForHealTarget, "ReactionEffectForHealTarget", m_reactionEffectForHealTarget);
		AbilityMod.AddToken_EffectInfo(tokens, m_reactionEffectForCaster, "ReactionEffectForCaster", m_reactionEffectForCaster);
		AbilityMod.AddToken_EffectInfo(tokens, m_enemyDebuffInRadiusEffect, "EnemyDebuffInRadiusEffect", m_enemyDebuffInRadiusEffect);
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		return new List<AbilityTooltipNumber>
		{
			new AbilityTooltipNumber(AbilityTooltipSymbol.Healing, AbilityTooltipSubject.Ally, m_healAmount),
			new AbilityTooltipNumber(AbilityTooltipSymbol.Healing, AbilityTooltipSubject.Self, m_selfHealIfTargetingAlly)
		};
	}

	public override bool GetCustomTargeterNumbers(ActorData targetActor, int currentTargeterIndex, TargetingNumberUpdateScratch results)
	{
		AbilityUtil_Targeter_AoE_AroundActor targeter = Targeter as AbilityUtil_Targeter_AoE_AroundActor;
		if (targeter == null
		    || ActorData.GetTeam() != targetActor.GetTeam()
		    || targeter.m_lastCenterActor == null)
		{
			return false;
		}
		results.m_healing = CalcFinalHealOnActor(targetActor, ActorData, targeter.m_lastCenterActor);
		return true;
	}
	
#if SERVER
	// custom
	public override List<ServerClientUtils.SequenceStartData> GetAbilityRunSequenceStartDataList(
		List<AbilityTarget> targets,
		ActorData caster,
		ServerAbilityUtils.AbilityRunData additionalData)
	{
		BoardSquare targetSquare = Board.Get().GetSquare(targets[0].GridPos);
		return new List<ServerClientUtils.SequenceStartData>
		{
			new ServerClientUtils.SequenceStartData(
				m_castSequencePrefab,
				targetSquare.ToVector3(),
				additionalData.m_abilityResults.HitActorsArray(),
				caster,
				additionalData.m_sequenceSource)
		};
	}
	
	// custom
	public override void GatherAbilityResults(
		List<AbilityTarget> targets,
		ActorData caster,
		ref AbilityResults abilityResults)
	{
		float radius = GetEnemyDebuffRadiusAroundCaster() > 0f
			? GetEnemyDebuffRadiusAroundCaster()
			: GetEnemyDebuffRadiusAroundTarget();
		BoardSquare targetSquare = Board.Get().GetSquare(targets[0].GridPos);
		Vector3 refPos = targetSquare.ToVector3();
		ActorData targetActor = targetSquare.OccupantActor;
		
		if (targetActor == null)
		{
			return;
		}

		int healing = CalcFinalHealOnActor(targetActor, caster, targetActor);
		ActorHitResults casterHitResults = new ActorHitResults(new ActorHitParameters(caster, refPos));
		if (targetActor == caster)
		{
			casterHitResults.AddBaseHealing(healing);
			casterHitResults.AddStandardEffectInfo(GetEffectOnSelf());
		}
		// see how the targeter is configured in SetupTargeter (EnemyDebuffRadius is always zero though)
		else if (targetActor.GetTeam() == caster.GetTeam() && GetEnemyDebuffRadiusAroundCaster() <= 0f) 
		{
			ActorHitParameters hitParams = new ActorHitParameters(targetActor, refPos);
			ActorHitResults hitResults = new ActorHitResults(healing, HitActionType.Healing, hitParams);
			hitResults.AddStandardEffectInfo(GetTargetHitEffect());
			// TODO CLERIC (not used in the ability or any of the mods)
			// hitResults.AddEffect(new ClericRangedHealEffect(
			// 	AsEffectSource(),
			// 	caster.GetCurrentBoardSquare(),
			// 	targetActor,
			// 	caster,
			// 	0,
			// 	GetReactionEffectForHealTarget()));
			abilityResults.StoreActorHit(hitResults);

			int selfHealing = CalcFinalHealOnActor(caster, caster, targetActor);
			casterHitResults.AddBaseHealing(selfHealing);
			casterHitResults.AddStandardEffectInfo(GetEffectOnSelf());
		}
		if (GetTechPointGainPerIncomingHit() > 0)
		{
			casterHitResults.AddEffect(new ClericRangedHealEffect(
				AsEffectSource(),
				caster.GetCurrentBoardSquare(),
				caster,
				caster,
				GetTechPointGainPerIncomingHit(),
				GetReactionEffectForCaster()));
		}
		abilityResults.StoreActorHit(casterHitResults);
		
		List<ActorData> hitActors = AreaEffectUtils.GetActorsInRadius(
			refPos,
			radius,
			m_enemyDebuffRadiusIgnoreLoS,
			caster,
			caster.GetOtherTeams(),
			null);
		foreach (ActorData hitActor in hitActors)
		{
			ActorHitParameters hitParams = new ActorHitParameters(hitActor, refPos);
			ActorHitResults hitResults = new ActorHitResults(GetEnemyDebuffInRadiusEffect(), hitParams);
			abilityResults.StoreActorHit(hitResults);
		}
	}
#endif
}

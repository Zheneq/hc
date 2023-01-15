// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

public class RageBeastSelfHeal : Ability
{
	public bool m_healOverTime = true;
	public StandardActorEffectData m_standardActorEffectData;
	public int m_healingOnCastIfUnder = 4;
	public int m_healingOnTickIfUnder = 4;
	public int m_healingOnCastIfOver = 2;
	public int m_healingOnTickIfOver = 2;
	public int m_healthThreshold = 10;

	private AbilityMod_RageBeastSelfHeal m_abilityMod;
	private StandardActorEffectData m_cachedStandardActorEffectData;

	private void Start()
	{
		Setup();
	}

	private void Setup()
	{
		SetCachedFields();
		Targeter = new AbilityUtil_Targeter_Shape(
			this,
			AbilityAreaShape.SingleSquare,
			true,
			AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape,
			false,
			false,
			AbilityUtil_Targeter.AffectsActor.Always);
		Targeter.ShowArcToShape = false;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		return new List<AbilityTooltipNumber>
		{
			new AbilityTooltipNumber(AbilityTooltipSymbol.Healing, AbilityTooltipSubject.HighHP, m_healingOnCastIfUnder),
			new AbilityTooltipNumber(AbilityTooltipSymbol.Healing, AbilityTooltipSubject.LowHP, m_healingOnCastIfOver)
		};
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		List<AbilityTooltipSubject> tooltipSubjectTypes = Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes == null || !tooltipSubjectTypes.Contains(AbilityTooltipSubject.Self))
		{
			return null;
		}
		return new Dictionary<AbilityTooltipSymbol, int>
		{
			[AbilityTooltipSymbol.Healing] = GetHealingForCurrentHealth(targetActor)
		};
	}

	public override List<int> Debug_GetExpectedNumbersInTooltip()
	{
		List<int> list = base.Debug_GetExpectedNumbersInTooltip();
		list.Add(m_healthThreshold);
		return list;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_RageBeastSelfHeal abilityMod_RageBeastSelfHeal = modAsBase as AbilityMod_RageBeastSelfHeal;
		StandardActorEffectData effectData = abilityMod_RageBeastSelfHeal != null
			? abilityMod_RageBeastSelfHeal.m_standardActorEffectDataMod.GetModifiedValue(m_standardActorEffectData)
			: m_standardActorEffectData;
		effectData.AddTooltipTokens(tokens, "StandardActorEffectData", abilityMod_RageBeastSelfHeal != null, m_standardActorEffectData);
		AddTokenInt(tokens, "HealingOnCastIfUnder", string.Empty, abilityMod_RageBeastSelfHeal != null
			? abilityMod_RageBeastSelfHeal.m_lowHealthHealOnCastMod.GetModifiedValue(m_healingOnCastIfUnder)
			: m_healingOnCastIfUnder);
		AddTokenInt(tokens, "HealingOnTickIfUnder", string.Empty, abilityMod_RageBeastSelfHeal != null
			? abilityMod_RageBeastSelfHeal.m_lowHealthHealOnTickMod.GetModifiedValue(m_healingOnTickIfUnder)
			: m_healingOnTickIfUnder);
		AddTokenInt(tokens, "HealingOnCastIfOver", string.Empty, abilityMod_RageBeastSelfHeal != null
			? abilityMod_RageBeastSelfHeal.m_highHealthOnCastMod.GetModifiedValue(m_healingOnCastIfOver)
			: m_healingOnCastIfOver);
		AddTokenInt(tokens, "HealingOnTickIfOver", string.Empty, abilityMod_RageBeastSelfHeal != null
			? abilityMod_RageBeastSelfHeal.m_highHealthOnTickMod.GetModifiedValue(m_healingOnTickIfOver)
			: m_healingOnTickIfOver);
		AddTokenInt(tokens, "HealthThreshold", string.Empty, abilityMod_RageBeastSelfHeal != null
			? abilityMod_RageBeastSelfHeal.m_healthThresholdMod.GetModifiedValue(m_healthThreshold)
			: m_healthThreshold);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_RageBeastSelfHeal))
		{
			m_abilityMod = abilityMod as AbilityMod_RageBeastSelfHeal;
			Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}

	private void SetCachedFields()
	{
		m_cachedStandardActorEffectData = m_abilityMod != null
			? m_abilityMod.m_standardActorEffectDataMod.GetModifiedValue(m_standardActorEffectData)
			: m_standardActorEffectData;
	}

	public StandardActorEffectData GetStandardActorEffectData()
	{
		return m_cachedStandardActorEffectData ?? m_standardActorEffectData;
	}

	public bool ShouldHealOverTime()
	{
		return m_abilityMod != null
			? m_abilityMod.m_healOverTimeMod.GetModifiedValue(m_healOverTime)
			: m_healOverTime;
	}

	public int ModdedHealthThreshold()
	{
		return m_abilityMod != null
			? m_abilityMod.m_healthThresholdMod.GetModifiedValue(m_healthThreshold)
			: m_healthThreshold;
	}

	public int ModdedHealOnCastIfUnder()
	{
		return m_abilityMod != null
			? m_abilityMod.m_lowHealthHealOnCastMod.GetModifiedValue(m_healingOnCastIfUnder)
			: m_healingOnCastIfUnder;
	}

	public int ModdedHealOnTickIfUnder()
	{
		return m_abilityMod != null
			? m_abilityMod.m_lowHealthHealOnTickMod.GetModifiedValue(m_healingOnTickIfUnder)
			: m_healingOnTickIfUnder;
	}

	public int ModdedHealOnCastIfOver()
	{
		return m_abilityMod != null
			? m_abilityMod.m_highHealthOnCastMod.GetModifiedValue(m_healingOnCastIfOver)
			: m_healingOnCastIfOver;
	}

	public int ModdedHealOnTickIfOver()
	{
		return m_abilityMod != null
			? m_abilityMod.m_highHealthOnTickMod.GetModifiedValue(m_healingOnTickIfOver)
			: m_healingOnTickIfOver;
	}

	public override bool DoesTargetActorMatchTooltipSubject(AbilityTooltipSubject subjectType, ActorData targetActor, Vector3 damageOrigin, ActorData targetingActor)
	{
		if (subjectType != AbilityTooltipSubject.HighHP && subjectType != AbilityTooltipSubject.LowHP)
		{
			return base.DoesTargetActorMatchTooltipSubject(subjectType, targetActor, damageOrigin, targetingActor);
		}
		bool isLowHp = targetActor.HitPoints <= ModdedHealthThreshold();
		return subjectType == AbilityTooltipSubject.LowHP ? isLowHp : !isLowHp;
	}

	private int GetHealingForCurrentHealth(ActorData caster)
	{
		if (caster.HitPoints <= ModdedHealthThreshold())
		{
			GetStandardActorEffectData().m_healingPerTurn = ModdedHealOnTickIfUnder();
			return ModdedHealOnCastIfUnder();
		}
		else
		{
			GetStandardActorEffectData().m_healingPerTurn = ModdedHealOnTickIfOver();
			return ModdedHealOnCastIfOver();
		}
	}
	
#if SERVER
	// added in rogues
	public override ServerClientUtils.SequenceStartData GetAbilityRunSequenceStartData(
		List<AbilityTarget> targets,
		ActorData caster,
		ServerAbilityUtils.AbilityRunData additionalData)
	{
		return new ServerClientUtils.SequenceStartData(
			AsEffectSource().GetSequencePrefab(),
			caster.GetFreePos(),
			new[] { caster },
			caster,
			additionalData.m_sequenceSource);
	}

	// added in rogues
	public override void GatherAbilityResults(
		List<AbilityTarget> targets,
		ActorData caster,
		ref AbilityResults abilityResults)
	{
		int healingForCurrentHealth = GetHealingForCurrentHealth(caster);
		ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(caster, caster.GetFreePos()));
		actorHitResults.SetBaseHealing(healingForCurrentHealth);
		if (ShouldHealOverTime())
		{
			if (ServerEffectManager.Get().GetEffect(caster, typeof(RagebeastSelfHealEffect)) is RagebeastSelfHealEffect selfHealEffect)
			{
				selfHealEffect.SetSkipGatherResultsAndHits(true);
				actorHitResults.AddEffectForRemoval(selfHealEffect, ServerEffectManager.Get().GetActorEffects(caster));
			}

			actorHitResults.AddEffect(new RagebeastSelfHealEffect(
				AsEffectSource(),
				caster.GetCurrentBoardSquare(),
				caster,
				caster,
				GetStandardActorEffectData(),
				RunPriority));
		}
		abilityResults.StoreActorHit(actorHitResults);
	}

	// added in rogues
	public override void OnExecutedActorHit_General(ActorData caster, ActorData target, ActorHitResults results)
	{
		if (caster == target && results.FinalHealing > 0)
		{
			caster.GetFreelancerStats().AddToValueOfStat(FreelancerStats.RageBeastStats.HealingFromSelfHeal, results.FinalHealing);
		}
	}
#endif
}

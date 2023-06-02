// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

public class MantaRegeneration : Ability
{
	[Header("-- Healing --")]
	public int m_maxRegeneration = 500;
	public int m_turnsOfRegeneration = 2;
	public float m_damageToHealRatio = 1f;
	public int m_techPointGainPerIncomingHit;
	public AbilityPriority m_healInPhase = AbilityPriority.Combat_Damage;
	[Header("  (( base effect data for healing, no need to specify healing here ))")]
	public StandardActorEffectData m_healEffectData;
	public StandardEffectInfo m_otherSelfEffect;
	[Header("-- Sequences --")]
	public GameObject m_castSequencePrefab;
	public GameObject m_incomingHitImpactSequencePrefab;
	
	private AbilityMod_MantaRegeneration m_abilityMod;
	private StandardActorEffectData m_cachedHealEffectData;
	private StandardEffectInfo m_cachedOtherSelfEffect;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Regeneration";
		}
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
		Targeter.SetShowArcToShape(false);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		GetOtherSelfEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Self);
		return numbers;
	}

	private void SetCachedFields()
	{
		m_cachedHealEffectData = m_abilityMod != null
			? m_abilityMod.m_healEffectDataMod.GetModifiedValue(m_healEffectData)
			: m_healEffectData;
		m_cachedOtherSelfEffect = m_abilityMod != null
			? m_abilityMod.m_otherSelfEffectMod.GetModifiedValue(m_otherSelfEffect)
			: m_otherSelfEffect;
	}

	public int GetMaxRegeneration()
	{
		return m_abilityMod != null
			? m_abilityMod.m_maxRegenerationMod.GetModifiedValue(m_maxRegeneration)
			: m_maxRegeneration;
	}

	public int GetTurnsOfRegeneration()
	{
		return m_abilityMod != null
			? m_abilityMod.m_turnsOfRegenerationMod.GetModifiedValue(m_turnsOfRegeneration)
			: m_turnsOfRegeneration;
	}

	public float GetDamageToHealRatio()
	{
		return m_abilityMod != null
			? m_abilityMod.m_damageToHealRatioMod.GetModifiedValue(m_damageToHealRatio)
			: m_damageToHealRatio;
	}

	public int GetTechPointGainPerHit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_techPointGainPerIncomingHit.GetModifiedValue(m_techPointGainPerIncomingHit)
			: m_techPointGainPerIncomingHit;
	}

	public AbilityModCooldownReduction GetCooldownReductionOnNoDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_cooldownReductionsWhenNoHits
			: null;
	}

	public StandardActorEffectData GetHealEffectData()
	{
		return m_cachedHealEffectData ?? m_healEffectData;
	}

	public StandardEffectInfo GetOtherSelfEffect()
	{
		return m_cachedOtherSelfEffect ?? m_otherSelfEffect;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_MantaRegeneration))
		{
			m_abilityMod = abilityMod as AbilityMod_MantaRegeneration;
			Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AddTokenInt(tokens, "MaxRegeneration", string.Empty, m_maxRegeneration);
		AddTokenInt(tokens, "TurnsOfRegeneration", string.Empty, m_turnsOfRegeneration);
		AddTokenFloatAsPct(tokens, "DamageToHealRatio", string.Empty, m_damageToHealRatio);
		m_healEffectData.AddTooltipTokens(tokens, "HealEffectData");
		AbilityMod.AddToken_EffectInfo(tokens, m_otherSelfEffect, "OtherSelfEffect", m_otherSelfEffect);
	}
	
#if SERVER
	// added in rogues
	public override List<ServerClientUtils.SequenceStartData> GetAbilityRunSequenceStartDataList(
		List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		List<ServerClientUtils.SequenceStartData> list = new List<ServerClientUtils.SequenceStartData>();
		foreach (var hitActor in additionalData.m_abilityResults.HitActorList())
		{
			ServerClientUtils.SequenceStartData item = new ServerClientUtils.SequenceStartData(
				m_castSequencePrefab, hitActor.GetFreePos(), hitActor.AsArray(), caster, additionalData.m_sequenceSource);
			list.Add(item);
		}
		return list;
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(caster, caster.GetFreePos()));
		
		// custom
		// you can have two concurrent effects with cdr on ult + brain juice, but the first effect would have been done healing by now
		// also you can have cdr on regen itself, but if it fired, you have no regen from that effect
		// so we are fine with resetting DamageReceivedForRegeneration and counting a new value
		List<Effect> activeEffects = ServerEffectManager.Get().GetEffectsOnTargetByCaster(caster, caster, typeof(MantaRegenerationEffect));
		if (activeEffects.Count > 0) // always else in rogues
		{
			actorHitResults.AddEffectForRefresh(activeEffects[0], ServerEffectManager.Get().GetActorEffects(caster));
		}
		else
		{
			// rogues
			MantaRegenerationEffect mantaRegenerationEffect = new MantaRegenerationEffect(
				AsEffectSource(),
				caster.GetCurrentBoardSquare(),
				caster,
				caster,
				GetHealEffectData(),
				GetMaxRegeneration(),
				GetTurnsOfRegeneration(),
				GetDamageToHealRatio(),
				GetTechPointGainPerHit(),
				m_incomingHitImpactSequencePrefab);
			mantaRegenerationEffect.SetHitPhaseBeforeStart(m_healInPhase);
			actorHitResults.AddEffect(mantaRegenerationEffect);
		}
		// end custom
		
		actorHitResults.AddStandardEffectInfo(GetOtherSelfEffect());
		abilityResults.StoreActorHit(actorHitResults);
	}

	// added in rogues
	public override void OnExecutedActorHit_General(ActorData caster, ActorData target, ActorHitResults results)
	{
		if (results.FinalHealing > 0)
		{
			caster.GetFreelancerStats().AddToValueOfStat(FreelancerStats.MantaStats.HealingFromSelfHeal, results.FinalHealing);
		}
	}
#endif
}

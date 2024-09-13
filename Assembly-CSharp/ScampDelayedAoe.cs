// ROGUES
// SERVER
using System.Collections.Generic;
using AbilityContextNamespace;
using UnityEngine;

public class ScampDelayedAoe : GenericAbility_Container
{
	[Separator("Effect Data for Delayed Aoe (for caster)")]
	public StandardActorEffectData m_delayedEffectBase;
	[Separator("Extra Damage if in Shield Down form")]
	public int m_extraDamageIfShieldDownForm;
	[Header("-- If >= 0, will multiply on delayed AoE damage after first damage turn")]
	public float m_subseqTurnDamageMultiplier = -1f;
	public bool m_subseqTurnNoEnergyGain;
	[Separator("Disable in Shield Down mode?")]
	public bool m_disableIfShieldsDown = true;
	[Separator("Anim Index for delayed Aoe Triggering")]
	public int m_animIndexOnTrigger;
	[Separator("Sequences - For Delayed Effect")]
	public GameObject m_onTriggerSequencePrefab;

	private Scamp_SyncComponent m_syncComp;
	private Passive_Scamp m_passive;
	private AbilityMod_ScampDelayedAoe m_abilityMod;
	
	private const string c_missingShields = "MissingShields";
	public static ContextNameKeyPair s_cvarMissingShields = new ContextNameKeyPair("MissingShields");

	private StandardActorEffectData m_cachedDelayedEffectBase;

	public override string GetUsageForEditor()
	{
		return base.GetUsageForEditor()
		       + ContextVars.GetContextUsageStr(s_cvarMissingShields.GetName(), "amount of missing shield on Scamp", false);
	}

	public override List<string> GetContextNamesForEditor()
	{
		List<string> contextNamesForEditor = base.GetContextNamesForEditor();
		contextNamesForEditor.Add("MissingShields");
		return contextNamesForEditor;
	}

	protected override void SetupTargetersAndCachedVars()
	{
		m_syncComp = GetComponent<Scamp_SyncComponent>();
		m_passive = GetPassiveOfType<Passive_Scamp>();
		SetCachedFields();
		base.SetupTargetersAndCachedVars();
	}

	private void SetCachedFields()
	{
		m_cachedDelayedEffectBase = m_abilityMod != null
			? m_abilityMod.m_delayedEffectBaseMod.GetModifiedValue(m_delayedEffectBase)
			: m_delayedEffectBase;
	}

	public StandardActorEffectData GetDelayedEffectBase()
	{
		return m_cachedDelayedEffectBase ?? m_delayedEffectBase;
	}

	public int GetExtraDamageIfShieldDownForm()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraDamageIfShieldDownFormMod.GetModifiedValue(m_extraDamageIfShieldDownForm)
			: m_extraDamageIfShieldDownForm;
	}

	public float GetSubseqTurnDamageMultiplier()
	{
		return m_abilityMod != null
			? m_abilityMod.m_subseqTurnDamageMultiplierMod.GetModifiedValue(m_subseqTurnDamageMultiplier)
			: m_subseqTurnDamageMultiplier;
	}

	public bool SubseqTurnNoEnergyGain()
	{
		return m_abilityMod != null
			? m_abilityMod.m_subseqTurnNoEnergyGainMod.GetModifiedValue(m_subseqTurnNoEnergyGain)
			: m_subseqTurnNoEnergyGain;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddSpecificTooltipTokens(tokens, modAsBase);
		m_delayedEffectBase.AddTooltipTokens(tokens, "DelayedEffectBase");
		AddTokenInt(tokens, "ExtraDamageIfShieldDownForm", string.Empty, m_extraDamageIfShieldDownForm);
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		return m_syncComp != null && m_syncComp.m_suitWasActiveOnTurnStart
		       || !m_disableIfShieldsDown;
	}

	public override void PreProcessTargetingNumbers(
		ActorData targetActor,
		int currentTargetIndex,
		Dictionary<ActorData, ActorHitContext> hitContext,
		ContextVars abilityContext)
	{
		int missingShields = 0;
		if (m_passive != null && m_syncComp != null)
		{
			missingShields = m_passive.GetMaxSuitShield() - (int)m_syncComp.m_suitShieldingOnTurnStart;
			missingShields = Mathf.Max(0, missingShields);
		}
		abilityContext.SetValue(s_cvarMissingShields.GetKey(), missingShields);
	}

	protected override void GenModImpl_SetModRef(AbilityMod abilityMod)
	{
		m_abilityMod = abilityMod as AbilityMod_ScampDelayedAoe;
	}

	protected override void GenModImpl_ClearModRef()
	{
		m_abilityMod = null;
	}
	
#if SERVER
	// custom
	protected override void PreProcessForCalcAbilityHits(
		List<AbilityTarget> targets,
		ActorData caster,
		Dictionary<ActorData, ActorHitContext> actorHitContextMap,
		ContextVars abilityContext)
	{
		base.PreProcessForCalcAbilityHits(targets, caster, actorHitContextMap, abilityContext);
		
		abilityContext.SetValue(s_cvarMissingShields.GetKey(), m_passive.GetMaxSuitShield() - m_syncComp.m_suitShieldingOnTurnStart);
	}

	// custom
	protected override void ProcessGatheredHits(
		List<AbilityTarget> targets,
		ActorData caster,
		AbilityResults abilityResults,
		List<ActorHitResults> actorHitResults,
		List<PositionHitResults> positionHitResults,
		List<NonActorTargetInfo> nonActorTargetInfo)
	{
		base.ProcessGatheredHits(targets, caster, abilityResults, actorHitResults, positionHitResults, nonActorTargetInfo);

		actorHitResults.Clear();
		positionHitResults.Clear();
		nonActorTargetInfo.Clear();

		ActorHitResults casterHitResults = new ActorHitResults(new ActorHitParameters(caster, caster.GetFreePos()));
		casterHitResults.AddEffect(new ScampDelayedAoeEffect(
			AsEffectSource(),
			caster,
			caster,
			GetDelayedEffectBase(),
			GetRadius(),
			GetOnHitAuthoredData(),
			GetExtraDamageIfShieldDownForm(),
			GetSubseqTurnDamageMultiplier(),
			SubseqTurnNoEnergyGain(),
			m_animIndexOnTrigger,
			m_onTriggerSequencePrefab,
			m_syncComp,
			m_passive));
		actorHitResults.Add(casterHitResults);
	}

	// custom
	private float GetRadius()
	{
		return (GetTargetSelectComp() as TargetSelect_AoeRadius)?.m_radius ?? 0;
	}
	
	// custom
	public override void OnExecutedActorHit_Effect(ActorData caster, ActorData target, ActorHitResults results)
	{
		if (results.FinalDamage > 0)
		{
			caster.GetFreelancerStats().AddToValueOfStat(
				FreelancerStats.ScampStats.DelayedAoeDamageDone,
				results.FinalDamage);
		}
	}
#endif
}

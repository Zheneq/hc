using System.Collections.Generic;
using AbilityContextNamespace;
using UnityEngine;

public class IceborgSelfShield : GenericAbility_Container
{
	[Separator("Health to be considered low health if below")]
	public int m_lowHealthThresh;
	[Separator("Shield if all shield depleted on first turn")]
	public int m_shieldOnNextTurnIfDepleted;
	[Separator("Sequences")]
	public GameObject m_shieldRemoveSeqPrefab;

	private AbilityMod_IceborgSelfShield m_abilityMod;
	private Iceborg_SyncComponent m_syncComp;

#if SERVER
	// custom
	private Passive_Iceborg m_passive;
#endif

	public override List<string> GetContextNamesForEditor()
	{
		List<string> contextNamesForEditor = base.GetContextNamesForEditor();
		contextNamesForEditor.Add(ContextKeys.s_CasterLowHealth.GetName());
		return contextNamesForEditor;
	}

	public override string GetUsageForEditor()
	{
		string usageForEditor = base.GetUsageForEditor();
		return usageForEditor + ContextVars.GetContextUsageStr(
			ContextKeys.s_CasterLowHealth.GetName(), 
			"set to 1 if caster is low health, 0 otherwise",
			false);
	}

	protected override void SetupTargetersAndCachedVars()
	{
		m_syncComp = GetComponent<Iceborg_SyncComponent>();
		base.SetupTargetersAndCachedVars();
		
#if SERVER
		// custom
		PassiveData passiveData = GetComponent<PassiveData>();
		if (passiveData != null)
		{
			m_passive = passiveData.GetPassiveOfType(typeof(Passive_Iceborg)) as Passive_Iceborg;
		}
#endif
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddSpecificTooltipTokens(tokens, modAsBase);
		AddTokenInt(tokens, "LowHealthThresh", string.Empty, m_lowHealthThresh);
		AddTokenInt(tokens, "ShieldOnNextTurnIfDepleted", string.Empty, m_shieldOnNextTurnIfDepleted);
	}

	public int GetLowHealthThresh()
	{
		return m_abilityMod != null
			? m_abilityMod.m_lowHealthThreshMod.GetModifiedValue(m_lowHealthThresh)
			: m_lowHealthThresh;
	}

	public int GetShieldOnNextTurnIfDepleted()
	{
		return m_abilityMod != null
			? m_abilityMod.m_shieldOnNextTurnIfDepletedMod.GetModifiedValue(m_shieldOnNextTurnIfDepleted)
			: m_shieldOnNextTurnIfDepleted;
	}

	public bool IsCasterLowHealth(ActorData caster)
	{
		return GetLowHealthThresh() > 0
		       && caster.HitPoints < GetLowHealthThresh();
	}

	public override List<StatusType> GetStatusToApplyWhenRequested()
	{
		return m_abilityMod != null
		       && m_syncComp != null
		       && m_syncComp.m_selfShieldLowHealthOnTurnStart
			? m_abilityMod.m_lowHealthStatusWhenRequested
			: base.GetStatusToApplyWhenRequested();
	}

	protected override void GenModImpl_SetModRef(AbilityMod abilityMod)
	{
		m_abilityMod = abilityMod as AbilityMod_IceborgSelfShield;
	}

	protected override void GenModImpl_ClearModRef()
	{
		m_abilityMod = null;
	}
	
#if SERVER
	// custom
	public override void Run(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		base.Run(targets, caster, additionalData);
		m_passive?.OnSelfShieldCast();
	}

	// custom
	protected override void PreProcessForCalcAbilityHits(
		List<AbilityTarget> targets,
		ActorData caster,
		Dictionary<ActorData, ActorHitContext> actorHitContextMap,
		ContextVars abilityContext)
	{
		base.PreProcessForCalcAbilityHits(targets, caster, actorHitContextMap, abilityContext);

		if (m_syncComp.m_selfShieldLowHealthOnTurnStart)
		{
			abilityContext.SetValue(ContextKeys.s_CasterLowHealth.GetKey(), 1);
		}
	}
	
	// custom
	public override void OnExecutedActorHit_Ability(ActorData caster, ActorData target, ActorHitResults results)
	{
		if (results.AppliedStatus(StatusType.Rooted) || results.AppliedStatus(StatusType.Snared))
		{
			caster.GetFreelancerStats().IncrementValueOfStat(FreelancerStats.IceborgStats.NumSlowsPlusRootsApplied);
		}
	}
	
	// custom
	public override void OnEffectAbsorbedDamage(ActorData effectCaster, int damageAbsorbed)
	{
		effectCaster.GetFreelancerStats().AddToValueOfStat(FreelancerStats.IceborgStats.SelfShieldEffectiveShielding, damageAbsorbed);
	}
#endif
}

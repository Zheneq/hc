using System.Collections.Generic;
using UnityEngine;

public class IceborgNovaOnReact : GenericAbility_Container
{
	[Separator("On Hit Data for React Hits", "yellow")]
	public OnHitAuthoredData m_reactOnHitData;
	[Space(10f)]
	public int m_reactDuration = 1;
	public bool m_reactRequireDamage = true;
	public bool m_reactEffectEndEarlyIfTriggered;
	[Separator("Energy on bearer/caster per reaction")]
	public int m_energyOnTargetPerReaction;
	public int m_energyOnCasterPerReaction;
	[Separator("Passive Bonus Energy Gain for Nova Core Triggering")]
	public int m_extraEnergyPerNovaCoreTrigger;
	[Separator("Damage Threshold to apply instance to self on turn start. Ignored if <= 0")]
	public int m_damageThreshForInstanceOnSelf; // TODO ICEBORG unused in any mod
	[Separator("Sequences")]
	public GameObject m_reactPersistentSeqPrefab;
	public GameObject m_reactOnTriggerSeqPrefab;

	private AbilityMod_IceborgNovaOnReact m_abilityMod;
	private Iceborg_SyncComponent m_syncComp;
	private OnHitAuthoredData m_cachedReactOnHitData;

	public override string GetOnHitDataDesc()
	{
		return base.GetOnHitDataDesc() + "\n-- On Hit Data for Reaction --\n" + m_reactOnHitData.GetInEditorDesc();
	}

	protected override void SetupTargetersAndCachedVars()
	{
		m_syncComp = GetComponent<Iceborg_SyncComponent>();
		base.SetupTargetersAndCachedVars();
		SetCachedFields();
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddSpecificTooltipTokens(tokens, modAsBase);
		m_reactOnHitData.AddTooltipTokens(tokens);
		AddTokenInt(tokens, "ReactDuration", string.Empty, m_reactDuration);
		AddTokenInt(tokens, "EnergyOnTargetPerReaction", string.Empty, m_energyOnTargetPerReaction);
		AddTokenInt(tokens, "EnergyOnCasterPerReaction", string.Empty, m_energyOnCasterPerReaction);
		AddTokenInt(tokens, "ExtraEnergyPerNovaCoreTrigger", string.Empty, m_extraEnergyPerNovaCoreTrigger);
		AddTokenInt(tokens, "DamageThreshForInstanceOnSelf", string.Empty, m_damageThreshForInstanceOnSelf);
		if (m_syncComp == null)
		{
			m_syncComp = GetComponent<Iceborg_SyncComponent>();
		}
		if (m_syncComp != null)
		{
			m_syncComp.AddTooltipTokens(tokens);
		}
	}

	private void SetCachedFields()
	{
		m_cachedReactOnHitData = m_abilityMod != null
			? m_abilityMod.m_reactOnHitDataMod.GetModdedOnHitData(m_reactOnHitData)
			: m_reactOnHitData;
	}

	public OnHitAuthoredData GetReactOnHitData()
	{
		return m_cachedReactOnHitData ?? m_reactOnHitData;
	}

	public int GetReactDuration()
	{
		return m_abilityMod != null
			? m_abilityMod.m_reactDurationMod.GetModifiedValue(m_reactDuration)
			: m_reactDuration;
	}

	public bool ReactRequireDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_reactRequireDamageMod.GetModifiedValue(m_reactRequireDamage)
			: m_reactRequireDamage;
	}

	public bool ReactEffectEndEarlyIfTriggered()
	{
		return m_abilityMod != null
			? m_abilityMod.m_reactEffectEndEarlyIfTriggeredMod.GetModifiedValue(m_reactEffectEndEarlyIfTriggered)
			: m_reactEffectEndEarlyIfTriggered;
	}

	public int GetEnergyOnTargetPerReaction()
	{
		return m_abilityMod != null
			? m_abilityMod.m_energyOnTargetPerReactionMod.GetModifiedValue(m_energyOnTargetPerReaction)
			: m_energyOnTargetPerReaction;
	}

	public int GetEnergyOnCasterPerReaction()
	{
		return m_abilityMod != null
			? m_abilityMod.m_energyOnCasterPerReactionMod.GetModifiedValue(m_energyOnCasterPerReaction)
			: m_energyOnCasterPerReaction;
	}

	public int GetExtraEnergyPerNovaCoreTrigger()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraEnergyPerNovaCoreTriggerMod.GetModifiedValue(m_extraEnergyPerNovaCoreTrigger)
			: m_extraEnergyPerNovaCoreTrigger;
	}

	// TODO ICEBORG unused in any mod
	public int GetDamageThreshForInstanceOnSelf()
	{
		return m_abilityMod != null
			? m_abilityMod.m_damageThreshForInstanceOnSelfMod.GetModifiedValue(m_damageThreshForInstanceOnSelf)
			: m_damageThreshForInstanceOnSelf;
	}

	protected override void GenModImpl_SetModRef(AbilityMod abilityMod)
	{
		m_abilityMod = abilityMod as AbilityMod_IceborgNovaOnReact;
	}

	protected override void GenModImpl_ClearModRef()
	{
		m_abilityMod = null;
	}
	
#if SERVER
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
		foreach (ActorHitResults hitResults in actorHitResults)
		{
			hitResults.AddEffect(CreateNovaOnReactEffect(hitResults.m_hitParameters.Target, caster));
		}
    }

    // custom
    public IceborgNovaOnReactEffect CreateNovaOnReactEffect(ActorData target, ActorData caster)
    {
		StandardActorEffectData effectData = new StandardActorEffectData();
		effectData.InitWithDefaultValues();
		effectData.m_duration = GetReactDuration();
		effectData.m_sequencePrefabs = new[] { m_reactPersistentSeqPrefab };
		return new IceborgNovaOnReactEffect(
			AsEffectSource(), 
			target.GetCurrentBoardSquare(), 
			target, 
			caster, 
			effectData, 
			ReactEffectEndEarlyIfTriggered(),
			GetReactOnHitData(),
			ReactRequireDamage(),
			m_reactOnTriggerSeqPrefab,
			GetEnergyOnTargetPerReaction(),
			GetEnergyOnCasterPerReaction());
    }
	
    // custom
    public override void OnExecutedActorHit_Ability(ActorData caster, ActorData target, ActorHitResults results)
    {
	    if (results.AppliedStatus(StatusType.Rooted) || results.AppliedStatus(StatusType.Snared))
	    {
		    caster.GetFreelancerStats().IncrementValueOfStat(FreelancerStats.IceborgStats.NumSlowsPlusRootsApplied);
	    }
    }
#endif
}

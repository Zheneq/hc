// ROGUES
// SERVER
using System.Collections.Generic;
using System.Linq;

public class FireborgSuperheat : GenericAbility_Container
{
    [Separator("Superheat")]
    public int m_superheatDuration = 2;
    public int m_igniteExtraDamageIfSuperheated;

    private Fireborg_SyncComponent m_syncComp;
    private AbilityMod_FireborgSuperheat m_abilityMod;
    
#if SERVER
    // custom
    private FireborgDamageAura m_damageAuraAbility;
    private AbilityData.ActionType m_damageAuraAbilityActionType;
#endif

    protected override void SetupTargetersAndCachedVars()
    {
        m_syncComp = GetComponent<Fireborg_SyncComponent>();
        base.SetupTargetersAndCachedVars();
        
#if SERVER
        // custom
        AbilityData abilityData = GetComponent<AbilityData>();
        if (abilityData != null)
        {
            m_damageAuraAbility = abilityData.GetAbilityOfType<FireborgDamageAura>();
            m_damageAuraAbilityActionType = abilityData.GetActionTypeOfAbility(m_damageAuraAbility);
        }
        
        OnHitEffecField effectOnSelfField = m_cachedOnHitData.m_allyHitEffectFields.FirstOrDefault(f => f.m_identifier == "SelfShield");
        if (effectOnSelfField != null)
        {
            StandardActorEffectData newEffectData = effectOnSelfField.m_effect.m_effectData.GetShallowCopy();
            newEffectData.m_duration = GetSuperheatDuration(); // override animation duration
            effectOnSelfField.m_effect.m_effectData = newEffectData;
        }
#endif
    }

    protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
    {
        base.AddSpecificTooltipTokens(tokens, modAsBase);
        AddTokenInt(tokens, "SuperheatDuration", string.Empty, m_superheatDuration);
        AddTokenInt(tokens, "IgniteExtraDamageIfSuperheated", string.Empty, m_igniteExtraDamageIfSuperheated);
    }

    public int GetSuperheatDuration() // TODO FIREBORG does not affect sequence-holding effect, does not increase cooldown
    {
        return m_abilityMod != null
            ? m_abilityMod.m_superheatDurationMod.GetModifiedValue(m_superheatDuration)
            : m_superheatDuration;
    }

    public int GetIgniteExtraDamageIfSuperheated() // TODO FIREBORG unused, always 0
    {
        return m_abilityMod != null
            ? m_abilityMod.m_igniteExtraDamageIfSuperheatedMod.GetModifiedValue(m_igniteExtraDamageIfSuperheated)
            : m_igniteExtraDamageIfSuperheated;
    }

    protected override void GenModImpl_SetModRef(AbilityMod abilityMod)
    {
        m_abilityMod = abilityMod as AbilityMod_FireborgSuperheat;
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

        m_syncComp.Networkm_superheatLastCastTurn = GameFlowData.Get().CurrentTurn;
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

        if (m_damageAuraAbility != null && m_damageAuraAbility.GetCdrOnUltCast() > 0)
        {
            ActorHitResults casterHitResults = GetOrAddHitResults(caster, actorHitResults);
            casterHitResults.AddMiscHitEvent(
                new MiscHitEventData_AddToCasterCooldown(
                    m_damageAuraAbilityActionType,
                    -m_damageAuraAbility.GetCdrOnUltCast()));
        }
    }
#endif
}
// ROGUES
// SERVER
using System.Collections.Generic;

public class DinoForceChase : GenericAbility_Container
{
    [Separator("Cooldown reduction on knockback ability")]
    public int m_cdrOnKnockbackAbility;
    [Separator("Energy Per Unstoppable Enemy (if ability is combat phase or later)")]
    public int m_energyPerUnstoppableEnemyHit;

    private AbilityMod_DinoForceChase m_abilityMod;
    private AbilityData.ActionType m_knockbackActionType = AbilityData.ActionType.INVALID_ACTION;
    
#if SERVER
    // custom
    private Passive_Dino m_passive;
#endif

    protected override void SetupTargetersAndCachedVars()
    {
#if SERVER
        // custom
        m_passive = GetPassiveOfType(typeof(Passive_Dino)) as Passive_Dino;
#endif
        AbilityData abilityData = GetComponent<AbilityData>();
        DinoTargetedKnockback knockbackAbility = GetAbilityOfType<DinoTargetedKnockback>();
        if (abilityData != null && knockbackAbility != null)
        {
            m_knockbackActionType = abilityData.GetActionTypeOfAbility(knockbackAbility);
        }

        base.SetupTargetersAndCachedVars();
    }

    public int GetCdrOnKnockbackAbility()
    {
        return m_abilityMod != null
            ? m_abilityMod.m_cdrOnKnockbackAbilityMod.GetModifiedValue(m_cdrOnKnockbackAbility)
            : m_cdrOnKnockbackAbility;
    }

    public int GetEnergyPerUnstoppableEnemyHit()
    {
        return m_abilityMod != null
            ? m_abilityMod.m_energyPerUnstoppableEnemyHitMod.GetModifiedValue(m_energyPerUnstoppableEnemyHit)
            : m_energyPerUnstoppableEnemyHit;
    }

    protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
    {
        base.AddSpecificTooltipTokens(tokens, modAsBase);
        AddTokenInt(tokens, "CdrOnKnockbackAbility", string.Empty, m_cdrOnKnockbackAbility);
        AddTokenInt(tokens, "EnergyPerUnstoppableEnemyHit", string.Empty, m_energyPerUnstoppableEnemyHit);
    }

    protected override void GenModImpl_SetModRef(AbilityMod abilityMod)
    {
        m_abilityMod = abilityMod as AbilityMod_DinoForceChase;
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

        foreach (ActorHitResults actorHitResult in actorHitResults)
        {
            ActorData hitActor = actorHitResult.m_hitParameters.Target;

            if (hitActor.GetTeam() == caster.GetTeam())
            {
                continue;
            }
            
            actorHitResult.AddMiscHitEvent(new MiscHitEventData(MiscHitEventType.TargetForceChaseCaster));

            if (ServerAbilityUtils.CurrentlyGatheringRealResults())
            {
                m_passive.AddActorInForceChase(hitActor);
            }
        }

        if (GetCdrOnKnockbackAbility() > 0 && m_knockbackActionType != AbilityData.ActionType.INVALID_ACTION)
        {
            GetOrAddHitResults(caster, actorHitResults)
                .AddMiscHitEvent(new MiscHitEventData_AddToCasterCooldown(
                    m_knockbackActionType, -1 * GetCdrOnKnockbackAbility()));
        }
    }
#endif
}
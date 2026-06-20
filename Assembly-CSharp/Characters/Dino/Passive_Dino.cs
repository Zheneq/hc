// ROGUES
// SERVER
using System.Collections.Generic;
using System.Linq;

// empty in reactor, missing in rogues. custom
public class Passive_Dino : Passive
{
#if SERVER
    private Dino_SyncComponent m_syncComp;
    private DinoLayerCones m_primaryAbility;
    private DinoTargetedKnockback m_knockbackAbility;
    private DinoForceChase m_forceChaseAbility;
    private AbilityData.ActionType m_dashActionType;

    private int m_pendingPowerLevel = -1;
    private List<ActorData> m_actorsPendingKnockback;
    private readonly List<ActorData> m_actorsInForceChase = new List<ActorData>();
    public int m_dashCooldownAdjust;

    protected override void OnStartup()
    {
        base.OnStartup();
        m_syncComp = GetComponent<Dino_SyncComponent>();
        m_primaryAbility = Owner.GetAbilityData().GetAbilityOfType(typeof(DinoLayerCones)) as DinoLayerCones;
        m_knockbackAbility =
            Owner.GetAbilityData().GetAbilityOfType(typeof(DinoTargetedKnockback)) as DinoTargetedKnockback;
        m_forceChaseAbility = Owner.GetAbilityData().GetAbilityOfType(typeof(DinoForceChase)) as DinoForceChase;
        m_dashActionType = Owner.GetAbilityData().GetActionTypeOfAbilityOfType(typeof(DinoDashOrShield));
        Owner.OnKnockbackHitExecutedDelegate += OnKnockbackMovementHitExecuted;
    }

    private void OnDestroy()
    {
        Owner.OnKnockbackHitExecutedDelegate -= OnKnockbackMovementHitExecuted;
    }

    public override void OnAbilitiesDone()
    {
        base.OnAbilitiesDone();

        int energyCompensation = 0;
        foreach (ActorData hitActor in m_actorsInForceChase)
        {
            if (hitActor.GetActorStatus().IsMovementDebuffImmune())
            {
                energyCompensation += m_forceChaseAbility.GetEnergyPerUnstoppableEnemyHit();
            }
            else
            {
                Owner.GetFreelancerStats().IncrementValueOfStat(FreelancerStats.DinoStats.ForceChaseNumChases);
            }
        }
        m_actorsInForceChase.Clear();

        if (energyCompensation > 0)
        {
            ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(Owner, Owner.GetFreePos()));
            actorHitResults.AddDirectTechPointGainOnCaster(energyCompensation);
            MovementResults.SetupAndExecuteAbilityResultsOutsideResolution(
                Owner,
                Owner,
                actorHitResults,
                m_forceChaseAbility);
        }
    }

    public void AddActorInForceChase(ActorData actor)
    {
        m_actorsInForceChase.Add(actor);
    }

    public List<ActorData> GetActorsPendingKnockback()
    {
        return m_actorsPendingKnockback;
    }

    public override void OnTurnStart()
    {
        base.OnTurnStart();

        if (GameFlowData.Get().CurrentTurn == 1 && m_knockbackAbility != null)
        {
            ActorHitResults hitRes = new ActorHitResults(new ActorHitParameters(Owner, Owner.GetFreePos()));
            hitRes.AddEffect(
                new DinoTargetedKnockbackPassiveEffect(
                    m_knockbackAbility.AsEffectSource(),
                    Owner.GetCurrentBoardSquare(),
                    Owner,
                    this,
                    m_knockbackAbility.DoHitsAroundKnockbackDest(),
                    m_knockbackAbility.GetHitsAroundKnockbackDestShape(),
                    m_knockbackAbility.GetKnockbackDestOnHitData(),
                    m_knockbackAbility.m_onKnockbackDestHitSeqPrefab));


            MovementResults movementResults = new MovementResults(MovementStage.INVALID);
            movementResults.SetupGameplayDataForAbility(m_knockbackAbility, hitRes, Owner);
            movementResults.ExecuteUnexecutedMovementHits(false);
        }

        if (m_syncComp != null)
        {
            bool isInReadyStance = IsInReadyStance();
            if (isInReadyStance)
            {
                m_syncComp.CallRpcSetDashReadyStanceAnimParams(1, true);
            }

            m_syncComp.CallRpcResetDashOrShieldTargeter(isInReadyStance);
        }
    }

    private bool IsInReadyStance()
    {
        return m_syncComp != null && m_syncComp.m_dashOrShieldInReadyStance;
    }

    public override void OnAbilityPhaseStart(AbilityPriority phase)
    {
        base.OnAbilityPhaseStart(phase);

        if (phase == AbilityPriority.Prep_Offense && IsInReadyStance())
        {
            m_syncComp.CallRpcSetDashReadyStanceAnimParams(0, false);
        }

        if (phase == AbilityPriority.Combat_Damage
            && m_knockbackAbility != null
            && ServerActionBuffer.Get().HasStoredAbilityRequestOfType(Owner, typeof(DinoTargetedKnockback)))
        {
            AbilityRequest abilityRequest = ServerActionBuffer.Get().GetAllStoredAbilityRequests()
                .FirstOrDefault(ar => ar.m_ability == m_knockbackAbility);
            if (abilityRequest != null)
            {
                GenericAbility_TargetSelectBase targetSelectComp = m_knockbackAbility.GetTargetSelectComp();
                targetSelectComp.CalcHitTargets(abilityRequest.m_targets, Owner, null);
                m_actorsPendingKnockback = targetSelectComp.GetActorHitContextMap().Keys
                    .Where(a => a.GetTeam() != Owner.GetTeam()).ToList();
            }
        }
    }

    public override void OnAbilityPhaseEnd(AbilityPriority phase)
    {
        base.OnAbilityPhaseEnd(phase);

        if (phase == AbilityPriority.Combat_Knockback)
        {
            m_actorsPendingKnockback = null;
        }
    }

    public override void OnTurnEnd()
    {
        base.OnTurnEnd();

        if (m_syncComp != null)
        {
            if (m_primaryAbility != null)
            {
                if (m_pendingPowerLevel >= 0)
                {
                    m_syncComp.Networkm_layerConePowerLevel = (short)m_pendingPowerLevel;
                }
                else
                {
                    m_syncComp.Networkm_layerConePowerLevel++;
                }

                m_pendingPowerLevel = -1;
            }

            m_syncComp.Networkm_dashOrShieldInReadyStance =
                m_syncComp.m_dashOrShieldLastCastTurn == GameFlowData.Get().CurrentTurn;
        }
    }

    private void OnKnockbackMovementHitExecuted(ActorData target, ActorHitResults hitRes)
    {
        if (hitRes.HasDamage
            && ServerActionBuffer.Get().HasStoredAbilityRequestOfType(Owner, typeof(DinoTargetedKnockback)))
        {
            Owner.GetFreelancerStats().AddToValueOfStat(
                FreelancerStats.DinoStats.KnockbackDamageOnCastAndKnockback,
                hitRes.FinalDamage);
        }
    }

    public override void OnMiscHitEventUpdate(List<MiscHitEventPassiveUpdateParams> updateParams)
    {
        foreach (MiscHitEventPassiveUpdateParams param in updateParams)
        {
            if (param is SetPowerLevelParam powerLevelParam)
            {
                m_pendingPowerLevel = powerLevelParam.m_powerLevel;
            }
        }
    }
    
    public override void OnAddToCooldownAttemptOnHit(
        AbilityData.ActionType actionType,
        int cooldownChangeDesired,
        int finalCooldown)
    {
        if (actionType == m_dashActionType
            && cooldownChangeDesired < 0
            && m_syncComp.m_dashOrShieldLastCastTurn == GameFlowData.Get().CurrentTurn)
        {
            m_dashCooldownAdjust += cooldownChangeDesired;
        }
    }

    public class SetPowerLevelParam : MiscHitEventPassiveUpdateParams
    {
        public readonly int m_powerLevel;

        public SetPowerLevelParam(int powerLevel)
        {
            m_powerLevel = powerLevel;
        }
    }
#endif
}
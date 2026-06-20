// ROGUES
// SERVER
using System.Collections.Generic;

#if SERVER
// added in rogues
public class CardModifyCooldownEffect : Effect
{
    private int m_cooldownChangeAmount;
    private List<AbilityData.ActionType> m_actionsToModify;

    public CardModifyCooldownEffect(
        EffectSource parent,
        ActorData target,
        ActorData caster,
        List<AbilityData.ActionType> actionsToModify,
        int cooldownChangeAmount)
        : base(parent, null, target, caster)
    {
        HitPhase = AbilityPriority.Combat_Final;
        m_time.duration = 1;
        m_actionsToModify = actionsToModify;
        m_cooldownChangeAmount = cooldownChangeAmount;
    }

    public override void GatherEffectResults(ref EffectResults effectResults, bool isReal)
    {
        if (Target == null || m_actionsToModify == null)
        {
            return;
        }
        ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(Target, Target.GetFreePos()));
        foreach (AbilityData.ActionType actionType in m_actionsToModify)
        {
            actorHitResults.AddMiscHitEvent(
                new MiscHitEventData_AddToCasterCooldown(actionType, m_cooldownChangeAmount, true));
            actorHitResults.AddMiscHitEvent(
                new MiscHitEventData_ProgressCasterStockRefreshTime(actionType, -1 * m_cooldownChangeAmount));
        }
        effectResults.StoreActorHit(actorHitResults);
    }
}
#endif

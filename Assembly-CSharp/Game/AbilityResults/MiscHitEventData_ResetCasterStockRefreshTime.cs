// ROGUES
// SERVER
// server-only, missing in reactor
#if SERVER
public class MiscHitEventData_ResetCasterStockRefreshTime : MiscHitEventData
{
	public AbilityData.ActionType m_actionType;

	public MiscHitEventData_ResetCasterStockRefreshTime(AbilityData.ActionType actionType) : base(MiscHitEventType.ResetCasterStockRefreshTime)
	{
		this.m_actionType = actionType;
	}

	public override void ExecuteMiscHitEvent(ActorHitResults actorHitResult)
	{
		if (this.m_actionType != AbilityData.ActionType.INVALID_ACTION)
		{
			AbilityData abilityData = actorHitResult.m_hitParameters.Caster.GetAbilityData();
			if (abilityData != null && abilityData.GetAbilityOfActionType(this.m_actionType) != null)
			{
				abilityData.OverrideStockRefreshCountdown(this.m_actionType, abilityData.GetStockRefreshDurationForAbility(this.m_actionType));
			}
		}
	}
}
#endif

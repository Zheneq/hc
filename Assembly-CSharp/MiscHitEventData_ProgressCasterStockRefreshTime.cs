// ROGUES
// SERVER
// server-only, missing in reactor
#if SERVER
public class MiscHitEventData_ProgressCasterStockRefreshTime : MiscHitEventData
{
	public AbilityData.ActionType m_actionType;

	public int m_progressAmount;

	public MiscHitEventData_ProgressCasterStockRefreshTime(AbilityData.ActionType actionType, int progressAmount) : base(MiscHitEventType.ProgressCasterStockRefreshTime)
	{
		this.m_actionType = actionType;
		this.m_progressAmount = progressAmount;
	}

	public override void ExecuteMiscHitEvent(ActorHitResults actorHitResult)
	{
		if (this.m_actionType != AbilityData.ActionType.INVALID_ACTION && this.m_progressAmount > 0)
		{
			AbilityData abilityData = actorHitResult.m_hitParameters.Caster.GetAbilityData();
			if (abilityData != null && abilityData.GetAbilityOfActionType(this.m_actionType) != null)
			{
				abilityData.ProgressStockRefreshTimeForAction(this.m_actionType, this.m_progressAmount);
			}
		}
	}
}
#endif

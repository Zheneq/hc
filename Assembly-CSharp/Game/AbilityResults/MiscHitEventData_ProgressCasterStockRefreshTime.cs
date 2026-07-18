// ROGUES
// SERVER
// server-only, missing in reactor
#if SERVER
public class MiscHitEventData_ProgressCasterStockRefreshTime : MiscHitEventData
{
	public AbilityData.ActionType m_actionType;
	public int m_progressAmount;

	public MiscHitEventData_ProgressCasterStockRefreshTime(AbilityData.ActionType actionType, int progressAmount)
		: base(MiscHitEventType.ProgressCasterStockRefreshTime)
	{
		m_actionType = actionType;
		m_progressAmount = progressAmount;
	}

	public override void ExecuteMiscHitEvent(ActorHitResults actorHitResult)
	{
		if (m_actionType == AbilityData.ActionType.INVALID_ACTION || m_progressAmount <= 0)
		{
			return;
		}
		AbilityData abilityData = actorHitResult.m_hitParameters.Caster.GetAbilityData();
		if (abilityData == null || abilityData.GetAbilityOfActionType(m_actionType) == null)
		{
			return;
		}
		abilityData.ProgressStockRefreshTimeForAction(m_actionType, m_progressAmount);
	}
}
#endif

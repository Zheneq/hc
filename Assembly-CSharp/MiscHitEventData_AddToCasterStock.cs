// ROGUES
// SERVER
using UnityEngine;

// server-only, missing in reactor
#if SERVER
public class MiscHitEventData_AddToCasterStock : MiscHitEventData
{
	public AbilityData.ActionType m_actionType;

	public int m_addAmount;

	public MiscHitEventData_AddToCasterStock(AbilityData.ActionType actionType, int addAmount) : base(MiscHitEventType.AddToCasterStockRemaining)
	{
		this.m_actionType = actionType;
		this.m_addAmount = addAmount;
	}

	public override void ExecuteMiscHitEvent(ActorHitResults actorHitResult)
	{
		if (this.m_actionType != AbilityData.ActionType.INVALID_ACTION)
		{
			AbilityData abilityData = actorHitResult.m_hitParameters.Caster.GetAbilityData();
			if (abilityData != null && abilityData.GetAbilityOfActionType(this.m_actionType) != null)
			{
				int desiredAmount = Mathf.Clamp(abilityData.GetStocksRemaining(this.m_actionType) + this.m_addAmount, 0, abilityData.GetMaxStocksCount(this.m_actionType));
				abilityData.OverrideStockRemaining(this.m_actionType, desiredAmount);
			}
		}
	}
}
#endif

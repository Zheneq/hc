// ROGUES
// SERVER
using UnityEngine;

// server-only, missing in reactor
#if SERVER
public class MiscHitEventData_OverrideCooldown : MiscHitEventData
{
	public AbilityData.ActionType m_actionType;

	public int m_overrideValue;

	public MiscHitEventData_OverrideCooldown(AbilityData.ActionType actionType, int overrideValue) : base(MiscHitEventType.OverrideCasterAbilityCooldown)
	{
		this.m_actionType = actionType;
		this.m_overrideValue = overrideValue;
	}

	public override void ExecuteMiscHitEvent(ActorHitResults actorHitResult)
	{
		if (this.m_actionType != AbilityData.ActionType.INVALID_ACTION)
		{
			AbilityData abilityData = actorHitResult.m_hitParameters.Caster.GetAbilityData();
			if (abilityData != null)
			{
				abilityData.OverrideCooldown(this.m_actionType, Mathf.Max(0, this.m_overrideValue + 1));
			}
		}
	}
}
#endif

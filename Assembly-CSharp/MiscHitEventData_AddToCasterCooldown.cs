// ROGUES
// SERVER
using UnityEngine;

// server-only, missing in reactor
#if SERVER
public class MiscHitEventData_AddToCasterCooldown : MiscHitEventData
{
	public AbilityData.ActionType m_actionType;

	public int m_addAmount;

	public bool m_ignoreCooldownMax;

	public MiscHitEventData_AddToCasterCooldown(AbilityData.ActionType actionType, int addAmount) : base(MiscHitEventType.AddToCasterAbilityCooldown)
	{
		this.m_actionType = actionType;
		this.m_addAmount = addAmount;
	}

	public override void ExecuteMiscHitEvent(ActorHitResults actorHitResult)
	{
		if (this.m_actionType != AbilityData.ActionType.INVALID_ACTION && this.m_addAmount != 0)
		{
			AbilityData abilityData = actorHitResult.m_hitParameters.Caster.GetAbilityData();
			if (abilityData != null)
			{
				Ability abilityOfActionType = abilityData.GetAbilityOfActionType(this.m_actionType);
				if (abilityOfActionType != null)
				{
					int cooldownRemaining = abilityData.GetCooldownRemaining(this.m_actionType);
					int num = abilityOfActionType.GetModdedCooldown() + 1;
					if (this.m_ignoreCooldownMax)
					{
						num = int.MaxValue;
					}
					int num2 = Mathf.Clamp(cooldownRemaining + this.m_addAmount, 0, num);
					PassiveData passiveData = actorHitResult.m_hitParameters.Caster.GetPassiveData();
					if (passiveData != null)
					{
						passiveData.OnAddToCooldownAttemptOnHit(this.m_actionType, this.m_addAmount, num2);
					}
					abilityData.OverrideCooldown(this.m_actionType, num2);
				}
			}
		}
	}
}
#endif

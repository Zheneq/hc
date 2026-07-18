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

	public MiscHitEventData_AddToCasterCooldown(
		AbilityData.ActionType actionType,
		int addAmount,
		bool ignoreCooldownMax = false)
		: base(MiscHitEventType.AddToCasterAbilityCooldown)
	{
		m_actionType = actionType;
		m_addAmount = addAmount;
		m_ignoreCooldownMax = ignoreCooldownMax;
	}

	public override void ExecuteMiscHitEvent(ActorHitResults actorHitResult)
	{
		if (m_actionType == AbilityData.ActionType.INVALID_ACTION || m_addAmount == 0)
		{
			return;
		}
		AbilityData abilityData = actorHitResult.m_hitParameters.Caster.GetAbilityData();
		if (abilityData == null)
		{
			return;
		}
		Ability abilityOfActionType = abilityData.GetAbilityOfActionType(m_actionType);
		if (abilityOfActionType == null)
		{
			return;
		}
		int cooldownRemaining = abilityData.GetCooldownRemaining(m_actionType);
		int cooldownMax = abilityOfActionType.GetModdedCooldown() + 1;
		if (m_ignoreCooldownMax)
		{
			cooldownMax = int.MaxValue;
		}
		int cooldown = Mathf.Clamp(cooldownRemaining + m_addAmount, 0, cooldownMax);
		PassiveData passiveData = actorHitResult.m_hitParameters.Caster.GetPassiveData();
		if (passiveData != null)
		{
			passiveData.OnAddToCooldownAttemptOnHit(m_actionType, m_addAmount, cooldown);
		}
		abilityData.OverrideCooldown(m_actionType, cooldown);
	}
}
#endif

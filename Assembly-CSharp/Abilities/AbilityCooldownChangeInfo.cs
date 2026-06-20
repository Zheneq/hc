using System;

[Serializable]
public class AbilityCooldownChangeInfo
{
	public AbilityData.ActionType abilitySlot = AbilityData.ActionType.INVALID_ACTION;

	public int cooldownAddAmount;
}

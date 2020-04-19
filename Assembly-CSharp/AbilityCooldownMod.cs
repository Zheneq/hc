using System;
using UnityEngine;

[Serializable]
public class AbilityCooldownMod
{
	public AbilityData.ActionType abilitySlot = AbilityData.ActionType.INVALID_ACTION;

	public AbilityModPropertyInt modAmount;

	public void ModifyCooldown(AbilityData abilityData)
	{
		if (abilityData != null && this.abilitySlot != AbilityData.ActionType.INVALID_ACTION)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityCooldownMod.ModifyCooldown(AbilityData)).MethodHandle;
			}
			int num = abilityData.GetCooldownRemaining(this.abilitySlot);
			num = Mathf.Max(0, this.modAmount.GetModifiedValue(num));
			abilityData.OverrideCooldown(this.abilitySlot, num);
		}
	}
}

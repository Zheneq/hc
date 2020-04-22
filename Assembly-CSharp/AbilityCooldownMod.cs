using System;
using UnityEngine;

[Serializable]
public class AbilityCooldownMod
{
	public AbilityData.ActionType abilitySlot = AbilityData.ActionType.INVALID_ACTION;

	public AbilityModPropertyInt modAmount;

	public void ModifyCooldown(AbilityData abilityData)
	{
		if (!(abilityData != null) || abilitySlot == AbilityData.ActionType.INVALID_ACTION)
		{
			return;
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			int cooldownRemaining = abilityData.GetCooldownRemaining(abilitySlot);
			cooldownRemaining = Mathf.Max(0, modAmount.GetModifiedValue(cooldownRemaining));
			abilityData.OverrideCooldown(abilitySlot, cooldownRemaining);
			return;
		}
	}
}

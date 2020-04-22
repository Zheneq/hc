using System;
using UnityEngine;

[Serializable]
public class AbilityStockMod
{
	public AbilityData.ActionType abilitySlot = AbilityData.ActionType.INVALID_ACTION;

	public AbilityModPropertyInt availableStockModAmount;

	public AbilityModPropertyInt refreshTimeRemainingModAmount;

	public void ModifyStockCountAndRefreshTime(AbilityData abilityData)
	{
		if (!(abilityData != null))
		{
			return;
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (abilitySlot == AbilityData.ActionType.INVALID_ACTION)
			{
				return;
			}
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				int maxStocksCount = abilityData.GetMaxStocksCount(abilitySlot);
				int num = Mathf.Max(0, maxStocksCount - abilityData.GetConsumedStocksCount(abilitySlot));
				int num2 = Mathf.Max(0, availableStockModAmount.GetModifiedValue(num));
				if (num != num2)
				{
					abilityData.OverrideStockRemaining(abilitySlot, num2);
				}
				int stockRefreshCountdown = abilityData.GetStockRefreshCountdown(abilitySlot);
				int num3 = Mathf.Max(0, refreshTimeRemainingModAmount.GetModifiedValue(stockRefreshCountdown));
				if (stockRefreshCountdown != num3)
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						abilityData.OverrideStockRefreshCountdown(abilitySlot, num3);
						return;
					}
				}
				return;
			}
		}
	}
}

using System;

[Serializable]
public class SeasonCurrencyReward : SeasonReward
{
	public QuestCurrencyReward CurrencyReward;

	public InventoryItemTemplate GetItemTemplate()
	{
		InventoryItemTemplate inventoryItemTemplate = new InventoryItemTemplate();
		inventoryItemTemplate.DisplayName = this.CurrencyReward.Amount + " ";
		bool flag = this.CurrencyReward.Amount != 1;
		CurrencyType type = this.CurrencyReward.Type;
		switch (type)
		{
		case CurrencyType.ISO:
		{
			InventoryItemTemplate inventoryItemTemplate2 = inventoryItemTemplate;
			inventoryItemTemplate2.DisplayName += StringUtil.TR("ISO", "Rewards");
			break;
		}
		case CurrencyType.ModToken:
		{
			InventoryItemTemplate inventoryItemTemplate3 = inventoryItemTemplate;
			string displayName = inventoryItemTemplate3.DisplayName;
			string str;
			if (flag)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				str = StringUtil.TR("ModTokens", "Rewards");
			}
			else
			{
				str = StringUtil.TR("ModToken", "Rewards");
			}
			inventoryItemTemplate3.DisplayName = displayName + str;
			break;
		}
		default:
			if (type != CurrencyType.FreelancerCurrency)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(SeasonCurrencyReward.GetItemTemplate()).MethodHandle;
				}
				string str2 = this.CurrencyReward.Type.ToString();
				string term = str2 + ((!flag) ? string.Empty : "s");
				InventoryItemTemplate inventoryItemTemplate4 = inventoryItemTemplate;
				inventoryItemTemplate4.DisplayName += StringUtil.TR(term, "Rewards");
			}
			else
			{
				InventoryItemTemplate inventoryItemTemplate5 = inventoryItemTemplate;
				inventoryItemTemplate5.DisplayName += StringUtil.TR("FreelancerCurrency", "Global");
			}
			break;
		case CurrencyType.GGPack:
		{
			InventoryItemTemplate inventoryItemTemplate6 = inventoryItemTemplate;
			string displayName2 = inventoryItemTemplate6.DisplayName;
			string str3;
			if (flag)
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				str3 = StringUtil.TR("GGBoosts", "Rewards");
			}
			else
			{
				str3 = StringUtil.TR("GGBoost", "Rewards");
			}
			inventoryItemTemplate6.DisplayName = displayName2 + str3;
			break;
		}
		}
		inventoryItemTemplate.Rarity = InventoryItemRarity.Uncommon;
		inventoryItemTemplate.Type = InventoryItemType.Currency;
		inventoryItemTemplate.Enabled = true;
		inventoryItemTemplate.TypeSpecificData = new int[2];
		inventoryItemTemplate.TypeSpecificData[0] = (int)this.CurrencyReward.Type;
		inventoryItemTemplate.TypeSpecificData[1] = this.CurrencyReward.Amount;
		return inventoryItemTemplate;
	}
}

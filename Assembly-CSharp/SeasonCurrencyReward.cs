using System;
using System.Text;

[Serializable]
public class SeasonCurrencyReward : SeasonReward
{
	public QuestCurrencyReward CurrencyReward;

	public InventoryItemTemplate GetItemTemplate()
	{
		InventoryItemTemplate inventoryItemTemplate = new InventoryItemTemplate();
		inventoryItemTemplate.DisplayName = new StringBuilder().Append(CurrencyReward.Amount).Append(" ").ToString();
		bool flag = CurrencyReward.Amount != 1;
		switch (CurrencyReward.Type)
		{
		default:
		{
			string str3 = CurrencyReward.Type.ToString();
			string term = new StringBuilder().Append(str3).Append((!flag) ? string.Empty : "s").ToString();
			inventoryItemTemplate.DisplayName += StringUtil.TR(term, "Rewards");
			break;
		}
		case CurrencyType.ModToken:
		{
			string displayName2 = inventoryItemTemplate.DisplayName;
			string str2;
			if (flag)
			{
				str2 = StringUtil.TR("ModTokens", "Rewards");
			}
			else
			{
				str2 = StringUtil.TR("ModToken", "Rewards");
			}

			inventoryItemTemplate.DisplayName = new StringBuilder().Append(displayName2).Append(str2).ToString();
			break;
		}
		case CurrencyType.GGPack:
		{
			string displayName = inventoryItemTemplate.DisplayName;
			string str;
			if (flag)
			{
				str = StringUtil.TR("GGBoosts", "Rewards");
			}
			else
			{
				str = StringUtil.TR("GGBoost", "Rewards");
			}

			inventoryItemTemplate.DisplayName = new StringBuilder().Append(displayName).Append(str).ToString();
			break;
		}
		case CurrencyType.ISO:
			inventoryItemTemplate.DisplayName += StringUtil.TR("ISO", "Rewards");
			break;
		case CurrencyType.FreelancerCurrency:
			inventoryItemTemplate.DisplayName += StringUtil.TR("FreelancerCurrency", "Global");
			break;
		}
		inventoryItemTemplate.Rarity = InventoryItemRarity.Uncommon;
		inventoryItemTemplate.Type = InventoryItemType.Currency;
		inventoryItemTemplate.Enabled = true;
		inventoryItemTemplate.TypeSpecificData = new int[2];
		inventoryItemTemplate.TypeSpecificData[0] = (int)CurrencyReward.Type;
		inventoryItemTemplate.TypeSpecificData[1] = CurrencyReward.Amount;
		return inventoryItemTemplate;
	}
}

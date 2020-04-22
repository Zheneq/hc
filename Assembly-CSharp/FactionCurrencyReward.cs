using System;

[Serializable]
public class FactionCurrencyReward : FactionReward
{
	public QuestCurrencyReward CurrencyReward;

	public InventoryItemTemplate GetItemTemplate()
	{
		InventoryItemTemplate inventoryItemTemplate = new InventoryItemTemplate();
		inventoryItemTemplate.DisplayName = CurrencyReward.Amount + " ";
		bool flag = CurrencyReward.Amount != 1;
		switch (CurrencyReward.Type)
		{
		case CurrencyType.ModToken:
			inventoryItemTemplate.DisplayName += ((!flag) ? StringUtil.TR("ModToken", "Rewards") : StringUtil.TR("ModTokens", "Rewards"));
			break;
		case CurrencyType.GGPack:
			inventoryItemTemplate.DisplayName += ((!flag) ? StringUtil.TR("GGBoost", "Rewards") : StringUtil.TR("GGBoosts", "Rewards"));
			break;
		case CurrencyType.ISO:
			inventoryItemTemplate.DisplayName += StringUtil.TR("ISO", "Rewards");
			break;
		default:
		{
			string str = CurrencyReward.Type.ToString();
			string term = str + ((!flag) ? string.Empty : "s");
			inventoryItemTemplate.DisplayName += StringUtil.TR(term, "Rewards");
			break;
		}
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

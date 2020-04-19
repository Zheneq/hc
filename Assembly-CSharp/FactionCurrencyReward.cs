using System;

[Serializable]
public class FactionCurrencyReward : FactionReward
{
	public QuestCurrencyReward CurrencyReward;

	public InventoryItemTemplate GetItemTemplate()
	{
		InventoryItemTemplate inventoryItemTemplate = new InventoryItemTemplate();
		inventoryItemTemplate.DisplayName = this.CurrencyReward.Amount + " ";
		bool flag = this.CurrencyReward.Amount != 1;
		switch (this.CurrencyReward.Type)
		{
		case CurrencyType.ISO:
		{
			InventoryItemTemplate inventoryItemTemplate2 = inventoryItemTemplate;
			inventoryItemTemplate2.DisplayName += StringUtil.TR("ISO", "Rewards");
			goto IL_156;
		}
		case CurrencyType.ModToken:
		{
			InventoryItemTemplate inventoryItemTemplate3 = inventoryItemTemplate;
			inventoryItemTemplate3.DisplayName += ((!flag) ? StringUtil.TR("ModToken", "Rewards") : StringUtil.TR("ModTokens", "Rewards"));
			goto IL_156;
		}
		case CurrencyType.GGPack:
		{
			InventoryItemTemplate inventoryItemTemplate4 = inventoryItemTemplate;
			inventoryItemTemplate4.DisplayName += ((!flag) ? StringUtil.TR("GGBoost", "Rewards") : StringUtil.TR("GGBoosts", "Rewards"));
			goto IL_156;
		}
		}
		string str = this.CurrencyReward.Type.ToString();
		string term = str + ((!flag) ? string.Empty : "s");
		InventoryItemTemplate inventoryItemTemplate5 = inventoryItemTemplate;
		inventoryItemTemplate5.DisplayName += StringUtil.TR(term, "Rewards");
		IL_156:
		inventoryItemTemplate.Rarity = InventoryItemRarity.Uncommon;
		inventoryItemTemplate.Type = InventoryItemType.Currency;
		inventoryItemTemplate.Enabled = true;
		inventoryItemTemplate.TypeSpecificData = new int[2];
		inventoryItemTemplate.TypeSpecificData[0] = (int)this.CurrencyReward.Type;
		inventoryItemTemplate.TypeSpecificData[1] = this.CurrencyReward.Amount;
		return inventoryItemTemplate;
	}
}

using System;

[Serializable]
public class CharacterTaunt
{
	public string m_tauntName;

	public string m_flavorText;

	public string m_obtainedText;

	public bool m_isHidden;

	public int m_uniqueID;

	public AbilityData.ActionType m_actionForTaunt;

	[AssetFileSelector("Assets/StreamingAssets/Video/taunts/", "", ".ogv")]
	public string m_tauntVideoPath;

	public GameBalanceVars.TauntUnlockData m_tauntUnlockData;

	public int symbol_001D()
	{
		int result = 0;
		if (this.m_tauntUnlockData != null && this.m_tauntUnlockData.m_unlockData != null && this.m_tauntUnlockData.m_unlockData.UnlockConditions != null)
		{
			foreach (GameBalanceVars.UnlockCondition unlockCondition in this.m_tauntUnlockData.m_unlockData.UnlockConditions)
			{
				if (unlockCondition.ConditionType == GameBalanceVars.UnlockData.UnlockType.Purchase)
				{
					return unlockCondition.typeSpecificData2;
				}
			}
		}
		return result;
	}

	public int symbol_000E()
	{
		if (this.m_tauntUnlockData != null)
		{
			InventoryItemRarity rarity = this.m_tauntUnlockData.Rarity;
			if (rarity == InventoryItemRarity.Uncommon)
			{
				return 0x64;
			}
			if (rarity == InventoryItemRarity.Rare)
			{
				return 0x12C;
			}
			if (rarity == InventoryItemRarity.Epic)
			{
				return 0x4B0;
			}
			if (rarity == InventoryItemRarity.Legendary)
			{
				return 0x5DC;
			}
		}
		return 0;
	}

	public static bool symbol_001D(InventoryItemRarity symbol_001D)
	{
		if (symbol_001D != InventoryItemRarity.Uncommon)
		{
			if (symbol_001D != InventoryItemRarity.Rare && symbol_001D != InventoryItemRarity.Epic)
			{
				return symbol_001D == InventoryItemRarity.Legendary;
			}
		}
		return true;
	}
}

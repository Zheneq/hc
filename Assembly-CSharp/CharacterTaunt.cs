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

	public int _001D()
	{
		int result = 0;
		if (m_tauntUnlockData != null && m_tauntUnlockData.m_unlockData != null && m_tauntUnlockData.m_unlockData.UnlockConditions != null)
		{
			GameBalanceVars.UnlockCondition[] unlockConditions = m_tauntUnlockData.m_unlockData.UnlockConditions;
			foreach (GameBalanceVars.UnlockCondition unlockCondition in unlockConditions)
			{
				if (unlockCondition.ConditionType != GameBalanceVars.UnlockData.UnlockType.Purchase)
				{
					continue;
				}
				while (true)
				{
					return unlockCondition.typeSpecificData2;
				}
			}
		}
		return result;
	}

	public int _000E()
	{
		if (m_tauntUnlockData != null)
		{
			InventoryItemRarity rarity = m_tauntUnlockData.Rarity;
			if (rarity == InventoryItemRarity.Uncommon)
			{
				while (true)
				{
					return 100;
				}
			}
			if (rarity == InventoryItemRarity.Rare)
			{
				return 300;
			}
			if (rarity == InventoryItemRarity.Epic)
			{
				while (true)
				{
					return 1200;
				}
			}
			if (rarity == InventoryItemRarity.Legendary)
			{
				return 1500;
			}
		}
		return 0;
	}

	public static bool _001D(InventoryItemRarity _001D)
	{
		int result;
		if (_001D != InventoryItemRarity.Uncommon)
		{
			if (_001D != InventoryItemRarity.Rare && _001D != InventoryItemRarity.Epic)
			{
				result = ((_001D == InventoryItemRarity.Legendary) ? 1 : 0);
				goto IL_0030;
			}
		}
		result = 1;
		goto IL_0030;
		IL_0030:
		return (byte)result != 0;
	}
}

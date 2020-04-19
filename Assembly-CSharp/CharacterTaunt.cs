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

	public int \u001D()
	{
		int result = 0;
		if (this.m_tauntUnlockData != null && this.m_tauntUnlockData.m_unlockData != null && this.m_tauntUnlockData.m_unlockData.UnlockConditions != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(CharacterTaunt.\u001D()).MethodHandle;
			}
			foreach (GameBalanceVars.UnlockCondition unlockCondition in this.m_tauntUnlockData.m_unlockData.UnlockConditions)
			{
				if (unlockCondition.ConditionType == GameBalanceVars.UnlockData.UnlockType.Purchase)
				{
					for (;;)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					return unlockCondition.typeSpecificData2;
				}
			}
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		return result;
	}

	public int \u000E()
	{
		if (this.m_tauntUnlockData != null)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(CharacterTaunt.\u000E()).MethodHandle;
			}
			InventoryItemRarity rarity = this.m_tauntUnlockData.Rarity;
			if (rarity == InventoryItemRarity.Uncommon)
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
				return 0x64;
			}
			if (rarity == InventoryItemRarity.Rare)
			{
				return 0x12C;
			}
			if (rarity == InventoryItemRarity.Epic)
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				return 0x4B0;
			}
			if (rarity == InventoryItemRarity.Legendary)
			{
				return 0x5DC;
			}
		}
		return 0;
	}

	public static bool \u001D(InventoryItemRarity \u001D)
	{
		if (\u001D != InventoryItemRarity.Uncommon)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(CharacterTaunt.\u001D(InventoryItemRarity)).MethodHandle;
			}
			if (\u001D != InventoryItemRarity.Rare && \u001D != InventoryItemRarity.Epic)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				return \u001D == InventoryItemRarity.Legendary;
			}
		}
		return true;
	}
}

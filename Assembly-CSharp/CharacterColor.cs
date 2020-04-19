using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CharacterColor
{
	public string m_name;

	public Color m_UIDisplayColor;

	[AssetFileSelector("Assets/UI/Textures/Resources/QuestRewards/", "QuestRewards/", ".png")]
	public string m_iconResourceString;

	public PrefabResourceLink m_heroPrefab;

	public string m_description;

	public string m_flavorText;

	public StyleLevelType m_styleLevel;

	public bool m_isHidden;

	public GameBalanceVars.ColorUnlockData m_colorUnlockData;

	public int m_sortOrder;

	public int m_requiredLevelForEquip;

	public Sprite m_loadingProfileIcon;

	[Header("-- Prefab Replacements --")]
	public PrefabResourceLink[] m_satellitePrefabs;

	[Header("-- Linked Colors --")]
	public List<CharacterLinkedColor> m_linkedColors;

	public PrefabReplacement[] m_replacementSequences;

	public static string GetIconResourceStringForStyleLevelType(StyleLevelType type)
	{
		string result = string.Empty;
		if (type == StyleLevelType.Advanced)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(CharacterColor.GetIconResourceStringForStyleLevelType(StyleLevelType)).MethodHandle;
			}
			result = "skin_advancedIcon";
		}
		else if (type == StyleLevelType.Expert)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			result = "skin_expertIcon";
		}
		else if (type == StyleLevelType.Mastery)
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
			result = "skin_MasteryIcon";
		}
		return result;
	}

	public int \u001D()
	{
		int result = 0;
		if (this.m_colorUnlockData != null && this.m_colorUnlockData.m_unlockData != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(CharacterColor.\u001D()).MethodHandle;
			}
			if (this.m_colorUnlockData.m_unlockData.UnlockConditions != null)
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
				foreach (GameBalanceVars.UnlockCondition unlockCondition in this.m_colorUnlockData.m_unlockData.UnlockConditions)
				{
					if (unlockCondition.ConditionType == GameBalanceVars.UnlockData.UnlockType.Purchase)
					{
						return unlockCondition.typeSpecificData2;
					}
				}
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
			}
		}
		return result;
	}
}

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
			result = "skin_advancedIcon";
		}
		else if (type == StyleLevelType.Expert)
		{
			result = "skin_expertIcon";
		}
		else if (type == StyleLevelType.Mastery)
		{
			result = "skin_MasteryIcon";
		}
		return result;
	}

	public int _001D()
	{
		int result = 0;
		if (m_colorUnlockData != null && m_colorUnlockData.m_unlockData != null)
		{
			if (m_colorUnlockData.m_unlockData.UnlockConditions != null)
			{
				GameBalanceVars.UnlockCondition[] unlockConditions = m_colorUnlockData.m_unlockData.UnlockConditions;
				foreach (GameBalanceVars.UnlockCondition unlockCondition in unlockConditions)
				{
					if (unlockCondition.ConditionType == GameBalanceVars.UnlockData.UnlockType.Purchase)
					{
						return unlockCondition.typeSpecificData2;
					}
				}
			}
		}
		return result;
	}
}

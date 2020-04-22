using System;
using UnityEngine;

public class GameResultBadgeData : MonoBehaviour
{
	[Serializable]
	public class StatDescription
	{
		public enum StatUnitType
		{
			None,
			PerTurn,
			PerLife,
			PerTwentyLives,
			Percentage
		}

		public string DisplayName;

		public string Description;

		public StatDisplaySettings.StatType m_StatType;

		public float LowWatermark = float.MinValue;

		public StatUnitType StatUnit;

		public bool LowerIsBetter;

		public static string GetStatUnit(StatDescription description)
		{
			if (description != null)
			{
				if (description.StatUnit == StatUnitType.PerTurn)
				{
					return StringUtil.TR("PerTurn", "Global");
				}
				if (description.StatUnit == StatUnitType.PerLife)
				{
					while (true)
					{
						return StringUtil.TR("PerLife", "Global");
					}
				}
				if (description.StatUnit == StatUnitType.PerTwentyLives)
				{
					while (true)
					{
						return StringUtil.TR("PerTwentyLives", "Global");
					}
				}
				if (description.StatUnit == StatUnitType.Percentage)
				{
					return "%";
				}
			}
			return string.Empty;
		}
	}

	[Serializable]
	public class ConsolidatedBadgeGroup
	{
		public string BadgeGroupDisplayName;

		[TextArea(2, 5)]
		public string BadgeGroupDescription;

		public GameBalanceVars.GameResultBadge.BadgeRole DisplayCategory;

		public int[] BadgeIDs;
	}

	public Color BadgeGroupGoldRequirementColorHex = Color.yellow;

	public Color BadgeGroupSilverRequirementColorHex = Color.gray;

	public Color BadgeGroupBronzeRequirementColorHex = Color.magenta;

	public StatDescription[] StatDescriptions;

	public ConsolidatedBadgeGroup[] BadgeGroups;

	public GameBalanceVars.GameResultBadge[] GameResultBadges;

	private static GameResultBadgeData s_instance;

	public bool IsStatLowerBetter(StatDisplaySettings.StatType StatType)
	{
		for (int i = 0; i < StatDescriptions.Length; i++)
		{
			if (StatDescriptions[i].m_StatType == StatType)
			{
				return StatDescriptions[i].LowerIsBetter;
			}
		}
		while (true)
		{
			return false;
		}
	}

	public static string ReplaceBadgeStringTokens(string input, GameBalanceVars.GameResultBadge BadgeData, CharacterType characterType)
	{
		string text = input;
		if (characterType.IsValidForHumanGameplay())
		{
			text = text.Replace("[FreelancerName]", characterType.GetDisplayName());
			text = text.Replace("[FreelancerRole]", StringUtil.TR(GameWideData.Get().GetCharacterResourceLink(characterType).m_characterRole.ToString(), "Global"));
		}
		else
		{
			text = text.Replace("[FreelancerName]", StringUtil.TR("InvalidCharacterTypeForBadges", "Global"));
			text = text.Replace("[FreelancerRole]", StringUtil.TR("InvalidCharacterRoleForBadges", "Global"));
		}
		return text.Replace("[PercentileToObtain]", BadgeData.GlobalPercentileToObtain.ToString());
	}

	public static string GetBadgeGroupRequirementDescription(GameBalanceVars.GameResultBadge BadgeData, CharacterType characterType)
	{
		string input = StringUtil.TR_BadgeGroupRequirementDescriptionKey(BadgeData.UniqueBadgeID);
		return ReplaceBadgeStringTokens(input, BadgeData, characterType);
	}

	public static string GetBadgeDescription(GameBalanceVars.GameResultBadge BadgeData, CharacterType characterType)
	{
		string input = StringUtil.TR_BadgeDescription(BadgeData.UniqueBadgeID);
		return ReplaceBadgeStringTokens(input, BadgeData, characterType);
	}

	public StatDescription GetStatDescription(StatDisplaySettings.StatType TypeOfStat)
	{
		for (int i = 0; i < StatDescriptions.Length; i++)
		{
			if (StatDescriptions[i].m_StatType != TypeOfStat)
			{
				continue;
			}
			while (true)
			{
				return StatDescriptions[i];
			}
		}
		while (true)
		{
			return null;
		}
	}

	public GameBalanceVars.LobbyStatSettings[] GetStatSettings()
	{
		GameBalanceVars.LobbyStatSettings[] array = new GameBalanceVars.LobbyStatSettings[StatDescriptions.Length];
		for (int i = 0; i < StatDescriptions.Length; i++)
		{
			array[i] = new GameBalanceVars.LobbyStatSettings();
			array[i].LowWatermark = StatDescriptions[i].LowWatermark;
			array[i].m_StatType = StatDescriptions[i].m_StatType;
			array[i].LowerIsBetter = StatDescriptions[i].LowerIsBetter;
		}
		return array;
	}

	public static GameResultBadgeData Get()
	{
		return s_instance;
	}

	public void Awake()
	{
		s_instance = this;
	}

	public GameBalanceVars.GameResultBadge GetBadgeInfo(int badgeID)
	{
		for (int i = 0; i < GameResultBadges.Length; i++)
		{
			if (GameResultBadges[i].UniqueBadgeID != badgeID)
			{
				continue;
			}
			while (true)
			{
				return GameResultBadges[i];
			}
		}
		while (true)
		{
			return null;
		}
	}

	private void OnValidate()
	{
		GameBalanceVars.EnsureUniqueIDs(GameResultBadges);
	}
}

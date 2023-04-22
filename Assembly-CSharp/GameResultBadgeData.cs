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
			if (description == null)
			{
				return string.Empty;
			}
			switch (description.StatUnit)
			{
				case StatUnitType.PerTurn:
					return StringUtil.TR("PerTurn", "Global");
				case StatUnitType.PerLife:
					return StringUtil.TR("PerLife", "Global");
				case StatUnitType.PerTwentyLives:
					return StringUtil.TR("PerTwentyLives", "Global");
				case StatUnitType.Percentage:
					return "%";
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
		foreach (StatDescription statDesc in StatDescriptions)
		{
			if (statDesc.m_StatType == StatType)
			{
				return statDesc.LowerIsBetter;
			}
		}

		return false;
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
		foreach (StatDescription statDesc in StatDescriptions)
		{
			if (statDesc.m_StatType == TypeOfStat)
			{
				return statDesc;
			}
		}

		return null;
	}

	public GameBalanceVars.LobbyStatSettings[] GetStatSettings()
	{
		GameBalanceVars.LobbyStatSettings[] array = new GameBalanceVars.LobbyStatSettings[StatDescriptions.Length];
		for (int i = 0; i < StatDescriptions.Length; i++)
		{
			array[i] = new GameBalanceVars.LobbyStatSettings
			{
				LowWatermark = StatDescriptions[i].LowWatermark,
				m_StatType = StatDescriptions[i].m_StatType,
				LowerIsBetter = StatDescriptions[i].LowerIsBetter
			};
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
		foreach (GameBalanceVars.GameResultBadge badge in GameResultBadges)
		{
			if (badge.UniqueBadgeID == badgeID)
			{
				return badge;
			}
		}

		return null;
	}

	private void OnValidate()
	{
		GameBalanceVars.EnsureUniqueIDs(GameResultBadges);
	}
}

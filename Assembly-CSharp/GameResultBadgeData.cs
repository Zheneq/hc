using System;
using UnityEngine;

public class GameResultBadgeData : MonoBehaviour
{
	public Color BadgeGroupGoldRequirementColorHex = Color.yellow;

	public Color BadgeGroupSilverRequirementColorHex = Color.gray;

	public Color BadgeGroupBronzeRequirementColorHex = Color.magenta;

	public GameResultBadgeData.StatDescription[] StatDescriptions;

	public GameResultBadgeData.ConsolidatedBadgeGroup[] BadgeGroups;

	public GameBalanceVars.GameResultBadge[] GameResultBadges;

	private static GameResultBadgeData s_instance;

	public bool IsStatLowerBetter(StatDisplaySettings.StatType StatType)
	{
		for (int i = 0; i < this.StatDescriptions.Length; i++)
		{
			if (this.StatDescriptions[i].m_StatType == StatType)
			{
				return this.StatDescriptions[i].LowerIsBetter;
			}
		}
		return false;
	}

	public static string ReplaceBadgeStringTokens(string input, GameBalanceVars.GameResultBadge BadgeData, CharacterType characterType)
	{
		string text;
		if (characterType.IsValidForHumanGameplay())
		{
			text = input.Replace("[FreelancerName]", characterType.GetDisplayName());
			text = text.Replace("[FreelancerRole]", StringUtil.TR(GameWideData.Get().GetCharacterResourceLink(characterType).m_characterRole.ToString(), "Global"));
		}
		else
		{
			text = input.Replace("[FreelancerName]", StringUtil.TR("InvalidCharacterTypeForBadges", "Global"));
			text = text.Replace("[FreelancerRole]", StringUtil.TR("InvalidCharacterRoleForBadges", "Global"));
		}
		return text.Replace("[PercentileToObtain]", BadgeData.GlobalPercentileToObtain.ToString());
	}

	public static string GetBadgeGroupRequirementDescription(GameBalanceVars.GameResultBadge BadgeData, CharacterType characterType)
	{
		string input = StringUtil.TR_BadgeGroupRequirementDescriptionKey(BadgeData.UniqueBadgeID);
		return GameResultBadgeData.ReplaceBadgeStringTokens(input, BadgeData, characterType);
	}

	public static string GetBadgeDescription(GameBalanceVars.GameResultBadge BadgeData, CharacterType characterType)
	{
		string input = StringUtil.TR_BadgeDescription(BadgeData.UniqueBadgeID);
		return GameResultBadgeData.ReplaceBadgeStringTokens(input, BadgeData, characterType);
	}

	public GameResultBadgeData.StatDescription GetStatDescription(StatDisplaySettings.StatType TypeOfStat)
	{
		for (int i = 0; i < this.StatDescriptions.Length; i++)
		{
			if (this.StatDescriptions[i].m_StatType == TypeOfStat)
			{
				return this.StatDescriptions[i];
			}
		}
		return null;
	}

	public GameBalanceVars.LobbyStatSettings[] GetStatSettings()
	{
		GameBalanceVars.LobbyStatSettings[] array = new GameBalanceVars.LobbyStatSettings[this.StatDescriptions.Length];
		for (int i = 0; i < this.StatDescriptions.Length; i++)
		{
			array[i] = new GameBalanceVars.LobbyStatSettings();
			array[i].LowWatermark = this.StatDescriptions[i].LowWatermark;
			array[i].m_StatType = this.StatDescriptions[i].m_StatType;
			array[i].LowerIsBetter = this.StatDescriptions[i].LowerIsBetter;
		}
		return array;
	}

	public static GameResultBadgeData Get()
	{
		return GameResultBadgeData.s_instance;
	}

	public void Awake()
	{
		GameResultBadgeData.s_instance = this;
	}

	public GameBalanceVars.GameResultBadge GetBadgeInfo(int badgeID)
	{
		for (int i = 0; i < this.GameResultBadges.Length; i++)
		{
			if (this.GameResultBadges[i].UniqueBadgeID == badgeID)
			{
				return this.GameResultBadges[i];
			}
		}
		return null;
	}

	private void OnValidate()
	{
		GameBalanceVars.EnsureUniqueIDs<GameBalanceVars.GameResultBadge>(this.GameResultBadges);
	}

	[Serializable]
	public class StatDescription
	{
		public string DisplayName;

		public string Description;

		public StatDisplaySettings.StatType m_StatType;

		public float LowWatermark = float.MinValue;

		public GameResultBadgeData.StatDescription.StatUnitType StatUnit;

		public bool LowerIsBetter;

		public static string GetStatUnit(GameResultBadgeData.StatDescription description)
		{
			if (description != null)
			{
				if (description.StatUnit == GameResultBadgeData.StatDescription.StatUnitType.PerTurn)
				{
					return StringUtil.TR("PerTurn", "Global");
				}
				if (description.StatUnit == GameResultBadgeData.StatDescription.StatUnitType.PerLife)
				{
					return StringUtil.TR("PerLife", "Global");
				}
				if (description.StatUnit == GameResultBadgeData.StatDescription.StatUnitType.PerTwentyLives)
				{
					return StringUtil.TR("PerTwentyLives", "Global");
				}
				if (description.StatUnit == GameResultBadgeData.StatDescription.StatUnitType.Percentage)
				{
					return "%";
				}
			}
			return string.Empty;
		}

		public enum StatUnitType
		{
			None,
			PerTurn,
			PerLife,
			PerTwentyLives,
			Percentage
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
}

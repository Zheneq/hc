using System;
using System.Collections.Generic;

[Serializable]
public class SeasonTemplate
{
	[Serializable]
	public class TierReward
	{
		public int Tier;

		public SeasonRewards Rewards;
	}

	[Serializable]
	public class SeasonEndRewards
	{
		public List<QuestCurrencyReward> CurrencyRewards;

		public List<QuestUnlockReward> UnlockRewards;

		public List<QuestItemReward> ItemRewards;
	}

	[Serializable]
	public class ConditionalSeasonEndRewards : SeasonEndRewards
	{
		public QuestPrerequisites Prerequisites;
	}

	public int[] SeasonLevelExperience;

	public int m_experiencePerLevelAfter;

	public int Index;

	public string DisplayName;

	public string DisplaySubTitle;

	public string SeasonEndHeader;

	public string IconFilename;

	public bool Enabled;

	public bool IsTutorial;

	public bool DisplayStats = true;

	public int PlayerFacingSeasonNumberOverride;

	public QuestPrerequisites Prerequisites;

	public List<SeasonChapter> Chapters;

	public SeasonRewards Rewards;

	public int MaxRewardVersion;

	public TierReward[] TierRewards;

	public SeasonEndRewards EndRewards;

	public List<ConditionalSeasonEndRewards> ConditionalEndRewards;

	public string GetDisplayName()
	{
		return StringUtil.TR_SeasonName(Index);
	}

	public string GetDisplaySubTitle()
	{
		return StringUtil.TR_SeasonSubTitle(Index);
	}

	public string GetSeasonEndHeader()
	{
		return StringUtil.TR_SeasonEndHeader(Index);
	}

	public int GetSeasonExperience(int level)
	{
		if (level > 0)
		{
			if (level - 1 < SeasonLevelExperience.Length)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						return SeasonLevelExperience[level - 1];
					}
				}
			}
			return m_experiencePerLevelAfter;
		}
		return 1;
	}

	public int GetPlayerFacingSeasonNumber()
	{
		if (PlayerFacingSeasonNumberOverride > 0)
		{
			return PlayerFacingSeasonNumberOverride;
		}
		return Index;
	}
}

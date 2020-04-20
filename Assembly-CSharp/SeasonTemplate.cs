using System;
using System.Collections.Generic;

[Serializable]
public class SeasonTemplate
{
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

	public SeasonTemplate.TierReward[] TierRewards;

	public SeasonTemplate.SeasonEndRewards EndRewards;

	public List<SeasonTemplate.ConditionalSeasonEndRewards> ConditionalEndRewards;

	public string GetDisplayName()
	{
		return StringUtil.TR_SeasonName(this.Index);
	}

	public string GetDisplaySubTitle()
	{
		return StringUtil.TR_SeasonSubTitle(this.Index);
	}

	public string GetSeasonEndHeader()
	{
		return StringUtil.TR_SeasonEndHeader(this.Index);
	}

	public int GetSeasonExperience(int level)
	{
		if (level <= 0)
		{
			return 1;
		}
		if (level - 1 < this.SeasonLevelExperience.Length)
		{
			return this.SeasonLevelExperience[level - 1];
		}
		return this.m_experiencePerLevelAfter;
	}

	public int GetPlayerFacingSeasonNumber()
	{
		if (this.PlayerFacingSeasonNumberOverride > 0)
		{
			return this.PlayerFacingSeasonNumberOverride;
		}
		return this.Index;
	}

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
	public class ConditionalSeasonEndRewards : SeasonTemplate.SeasonEndRewards
	{
		public QuestPrerequisites Prerequisites;
	}
}

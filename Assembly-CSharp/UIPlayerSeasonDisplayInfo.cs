using System;
using System.Collections.Generic;
using UnityEngine;

public class UIPlayerSeasonDisplayInfo
{
	public int SeasonNumber;

	public int CurrentChapter;

	public string SeasonName;

	public string SeasonEndTime;

	public int PlayerSeasonLevel;

	public int CommunityRank;

	public float currentPercentThroughCommunityRank;

	public float currentPercentThroughPlayerSeasonLevel;

	public int currentXPThroughPlayerLevel;

	public int currentLevelDisplayIndex;

	public List<UISeasonRepeatingRewardInfo> RepeatingRewards;

	public List<UISeasonChapterEntry> ChapterEntries;

	public List<UISeasonRewardEntry> SeasonRewardEntries;

	public List<UISeasonRewardEntry> FullSeasonRewardEntries;

	public List<UISeasonCommunityRankRewardEntry> CommunityRankRewardEntries;

	public UIPlayerSeasonDisplayInfo()
	{
		this.SeasonName = string.Empty;
		this.SeasonEndTime = string.Empty;
		this.ChapterEntries = new List<UISeasonChapterEntry>();
		this.SeasonRewardEntries = new List<UISeasonRewardEntry>();
		this.FullSeasonRewardEntries = new List<UISeasonRewardEntry>();
		this.CommunityRankRewardEntries = new List<UISeasonCommunityRankRewardEntry>();
		this.RepeatingRewards = new List<UISeasonRepeatingRewardInfo>();
	}

	public override bool Equals(object obj)
	{
		if (!(obj is UIPlayerSeasonDisplayInfo))
		{
			return false;
		}
		UIPlayerSeasonDisplayInfo uiplayerSeasonDisplayInfo = (UIPlayerSeasonDisplayInfo)obj;
		bool flag = this.SeasonRewardEntries.Count == uiplayerSeasonDisplayInfo.SeasonRewardEntries.Count;
		if (flag)
		{
			for (int i = 0; i < this.SeasonRewardEntries.Count; i++)
			{
				if (!this.SeasonRewardEntries[i].Equals(uiplayerSeasonDisplayInfo.SeasonRewardEntries[i]))
				{
					flag = false;
					goto IL_97;
				}
			}
		}
		IL_97:
		bool flag2 = this.FullSeasonRewardEntries.Count == uiplayerSeasonDisplayInfo.FullSeasonRewardEntries.Count;
		if (flag2)
		{
			for (int j = 0; j < this.FullSeasonRewardEntries.Count; j++)
			{
				if (!this.FullSeasonRewardEntries[j].Equals(uiplayerSeasonDisplayInfo.FullSeasonRewardEntries[j]))
				{
					flag2 = false;
					goto IL_118;
				}
			}
		}
		IL_118:
		bool flag3 = this.RepeatingRewards.Count == uiplayerSeasonDisplayInfo.RepeatingRewards.Count;
		if (flag3)
		{
			for (int k = 0; k < this.RepeatingRewards.Count; k++)
			{
				if (!this.RepeatingRewards[k].Equals(uiplayerSeasonDisplayInfo.RepeatingRewards[k]))
				{
					flag3 = false;
					goto IL_19E;
				}
			}
		}
		IL_19E:
		bool flag4 = this.ChapterEntries.Count == uiplayerSeasonDisplayInfo.ChapterEntries.Count;
		if (flag4)
		{
			for (int l = 0; l < this.ChapterEntries.Count; l++)
			{
				if (!this.ChapterEntries[l].Equals(uiplayerSeasonDisplayInfo.ChapterEntries[l]))
				{
					flag4 = false;
					goto IL_214;
				}
			}
		}
		IL_214:
		if (this.SeasonNumber == uiplayerSeasonDisplayInfo.SeasonNumber)
		{
			if (this.CurrentChapter == uiplayerSeasonDisplayInfo.CurrentChapter)
			{
				if (this.SeasonName == uiplayerSeasonDisplayInfo.SeasonName)
				{
					if (this.PlayerSeasonLevel == uiplayerSeasonDisplayInfo.PlayerSeasonLevel && this.currentPercentThroughPlayerSeasonLevel == uiplayerSeasonDisplayInfo.currentPercentThroughPlayerSeasonLevel)
					{
						if (this.currentXPThroughPlayerLevel == uiplayerSeasonDisplayInfo.currentXPThroughPlayerLevel)
						{
							if (this.currentLevelDisplayIndex == uiplayerSeasonDisplayInfo.currentLevelDisplayIndex)
							{
								if (flag)
								{
									if (flag2 && flag3)
									{
										return flag4;
									}
								}
							}
						}
					}
				}
			}
		}
		return false;
	}

	public override int GetHashCode()
	{
		return this.SeasonNumber.GetHashCode() ^ this.CurrentChapter.GetHashCode() ^ this.PlayerSeasonLevel.GetHashCode() ^ this.currentPercentThroughPlayerSeasonLevel.GetHashCode() ^ this.currentXPThroughPlayerLevel.GetHashCode() ^ this.currentLevelDisplayIndex.GetHashCode();
	}

	public List<UISeasonRepeatingRewardInfo> GetRepeatingRewardsForLevel(int level)
	{
		List<UISeasonRepeatingRewardInfo> list = new List<UISeasonRepeatingRewardInfo>();
		foreach (UISeasonRepeatingRewardInfo uiseasonRepeatingRewardInfo in this.RepeatingRewards)
		{
			if (level >= uiseasonRepeatingRewardInfo.StartLevel)
			{
				if ((level - uiseasonRepeatingRewardInfo.StartLevel) % uiseasonRepeatingRewardInfo.RepeatEveryXLevels == 0)
				{
					list.Add(uiseasonRepeatingRewardInfo);
				}
			}
		}
		return list;
	}

	public void Clear()
	{
		this.SeasonName = string.Empty;
		this.SeasonEndTime = string.Empty;
		for (int i = 0; i < this.ChapterEntries.Count; i++)
		{
			this.ChapterEntries[i].Clear();
		}
		this.ChapterEntries.Clear();
		for (int j = 0; j < this.SeasonRewardEntries.Count; j++)
		{
			this.SeasonRewardEntries[j].Clear();
		}
		this.SeasonRewardEntries.Clear();
		for (int k = 0; k < this.FullSeasonRewardEntries.Count; k++)
		{
			this.FullSeasonRewardEntries[k].Clear();
		}
		this.FullSeasonRewardEntries.Clear();
		for (int l = 0; l < this.CommunityRankRewardEntries.Count; l++)
		{
			this.CommunityRankRewardEntries[l].Clear();
		}
		this.CommunityRankRewardEntries.Clear();
		this.PlayerSeasonLevel = 0;
		this.CommunityRank = 0;
		this.currentPercentThroughCommunityRank = 0f;
		this.currentPercentThroughPlayerSeasonLevel = 0f;
	}

	public void Setup(int seasonNumber, PersistedAccountData accountData)
	{
		this.SeasonNumber = seasonNumber;
		ExperienceComponent seasonExperienceComponent = accountData.QuestComponent.GetSeasonExperienceComponent(this.SeasonNumber);
		this.PlayerSeasonLevel = seasonExperienceComponent.Level;
		this.CommunityRank = Mathf.FloorToInt(UnityEngine.Random.value * 4f);
		this.currentPercentThroughCommunityRank = 0f;
		this.currentXPThroughPlayerLevel = seasonExperienceComponent.XPProgressThroughLevel;
		int seasonExperience = SeasonWideData.Get().GetSeasonExperience(this.SeasonNumber, this.PlayerSeasonLevel);
		this.currentPercentThroughPlayerSeasonLevel = (float)this.currentXPThroughPlayerLevel / (float)seasonExperience;
		int count = SeasonWideData.Get().GetSeasonTemplate(this.SeasonNumber).Chapters.Count;
		this.SeasonName = SeasonWideData.Get().GetSeasonTemplate(this.SeasonNumber).GetDisplayName();
		TimeRange timeRange = SeasonWideData.Get().GetSeasonTemplate(this.SeasonNumber).Prerequisites.GetTimeRange();
		if (timeRange.StartTime == null)
		{
			timeRange.StartTime = new DateTime?(new DateTime(0x7E0, 9, 0x1E, 0, 0, 0));
		}
		this.SeasonEndTime = string.Format(StringUtil.TR("DayMonthYear", "Global"), timeRange.StartTime.Value.Day, StringUtil.TR("Month" + timeRange.StartTime.Value.Month, "Global"), timeRange.StartTime.Value.Year) + " - ";
		if (timeRange.EndTime != null)
		{
			this.SeasonEndTime += string.Format(StringUtil.TR("DayMonthYear", "Global"), timeRange.EndTime.Value.Day, StringUtil.TR("Month" + timeRange.EndTime.Value.Month, "Global"), timeRange.EndTime.Value.Year);
		}
		else
		{
			this.SeasonEndTime += "????";
		}
		this.SeasonEndTime = string.Format(StringUtil.TR("DateInPacificTime", "Global"), this.SeasonEndTime);
		List<int> unlockedSeasonChapters = accountData.QuestComponent.GetUnlockedSeasonChapters(this.SeasonNumber);
		int num = 0;
		for (int i = 0; i < unlockedSeasonChapters.Count; i++)
		{
			if (unlockedSeasonChapters[i] > num)
			{
				num = unlockedSeasonChapters[i];
			}
		}
		this.CurrentChapter = num;
		List<SeasonChapter> chapters = SeasonWideData.Get().GetSeasonTemplate(this.SeasonNumber).Chapters;
		for (int j = 0; j < count; j++)
		{
			UISeasonChapterEntry uiseasonChapterEntry = new UISeasonChapterEntry();
			SeasonChapter previousChapterInfo = null;
			if (j > 0)
			{
				previousChapterInfo = chapters[j - 1];
			}
			SeasonChapter nextChapterInfo = null;
			if (j + 1 < count)
			{
				nextChapterInfo = chapters[j + 1];
			}
			uiseasonChapterEntry.Setup(chapters[j], previousChapterInfo, nextChapterInfo, this.SeasonNumber, j);
			this.ChapterEntries.Add(uiseasonChapterEntry);
		}
		if (this.SeasonNumber != accountData.QuestComponent.ActiveSeason)
		{
			return;
		}
		List<SeasonReward> allRewards = SeasonWideData.Get().GetSeasonTemplate(this.SeasonNumber).Rewards.GetAllRewards();
		int num2 = 0;
		for (int k = 0; k < allRewards.Count; k++)
		{
			if (allRewards[k].level > num2)
			{
				num2 = allRewards[k].level;
			}
			if (allRewards[k].repeatEveryXLevels > 0)
			{
				UISeasonRepeatingRewardInfo item = new UISeasonRepeatingRewardInfo(allRewards[k]);
				this.RepeatingRewards.Add(item);
			}
		}
		List<UISeasonRepeatingRewardInfo> repeatingRewards = this.RepeatingRewards;
		
		repeatingRewards.Sort(delegate(UISeasonRepeatingRewardInfo info1, UISeasonRepeatingRewardInfo info2)
			{
				if (info1 == null)
				{
					if (info2 == null)
					{
						return 0;
					}
				}
				if (info1 == null)
				{
					return 1;
				}
				if (info2 == null)
				{
					return -1;
				}
				return info1.CompareTo(info2);
			});
		int num3 = Mathf.Max(seasonExperienceComponent.Level + 0x64, num2 + 1);
		List<SeasonReward>[] array = new List<SeasonReward>[num3];
		for (int l = 0; l < allRewards.Count; l++)
		{
			int level = allRewards[l].level;
			if (array[level] == null)
			{
				array[level] = new List<SeasonReward>();
			}
			if (allRewards[l].repeatEveryXLevels == 0)
			{
				bool flag = true;
				if (level <= this.PlayerSeasonLevel)
				{
					if (allRewards[l] is SeasonItemReward)
					{
						SeasonItemReward seasonItemReward = allRewards[l] as SeasonItemReward;
						if (!seasonItemReward.Conditions.IsNullOrEmpty<QuestCondition>() && accountData.QuestComponent.SeasonItemRewardsGranted.ContainsKey(level))
						{
							flag = accountData.QuestComponent.SeasonItemRewardsGranted[level].Contains(seasonItemReward.ItemReward.ItemTemplateId);
						}
					}
				}
				if (flag)
				{
					array[allRewards[l].level].Add(allRewards[l]);
				}
			}
		}
		int m = 1;
		while (m < num3)
		{
			List<UISeasonRepeatingRewardInfo> repeatingRewardsForLevel = this.GetRepeatingRewardsForLevel(m);
			bool flag2;
			if (array != null && array[m] != null)
			{
				flag2 = (array[m].Count > 0);
			}
			else
			{
				flag2 = false;
			}
			bool flag3 = flag2;
			if (flag3)
			{
				goto IL_658;
			}
			if (repeatingRewardsForLevel.Count > 0)
			{
				goto IL_658;
			}
			if (m == 1)
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					goto IL_658;
				}
			}
			IL_78F:
			List<SeasonReward> list = new List<SeasonReward>();
			if (array[m] != null)
			{
				using (List<SeasonReward>.Enumerator enumerator = array[m].GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						SeasonReward item2 = enumerator.Current;
						list.Add(item2);
					}
				}
			}
			else
			{
				for (int n = 0; n < repeatingRewardsForLevel.Count; n++)
				{
					list.Add(repeatingRewardsForLevel[n].GetSeasonRewardReference());
				}
			}
			UISeasonRewardEntry uiseasonRewardEntry = new UISeasonRewardEntry();
			uiseasonRewardEntry.Init(m, m <= this.PlayerSeasonLevel, list, null, m == this.PlayerSeasonLevel);
			this.FullSeasonRewardEntries.Add(uiseasonRewardEntry);
			m++;
			continue;
			IL_658:
			bool flag4 = false;
			using (List<UISeasonRepeatingRewardInfo>.Enumerator enumerator2 = repeatingRewardsForLevel.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					UISeasonRepeatingRewardInfo uiseasonRepeatingRewardInfo = enumerator2.Current;
					if (uiseasonRepeatingRewardInfo.RepeatEveryXLevels > 1)
					{
						flag4 = true;
						goto IL_6B1;
					}
				}
			}
			IL_6B1:
			if (!flag3 && !flag4)
			{
				if (m != 1 && m != this.PlayerSeasonLevel)
				{
					if (m != this.PlayerSeasonLevel + 1)
					{
						goto IL_78F;
					}
				}
			}
			if (m == this.PlayerSeasonLevel)
			{
				this.currentLevelDisplayIndex = this.SeasonRewardEntries.Count;
			}
			UISeasonRewardEntry uiseasonRewardEntry2 = new UISeasonRewardEntry();
			if (array[m] == null)
			{
				array[m] = new List<SeasonReward>();
			}
			for (int num4 = 0; num4 < repeatingRewardsForLevel.Count; num4++)
			{
				array[m].Add(repeatingRewardsForLevel[num4].GetSeasonRewardReference());
			}
			uiseasonRewardEntry2.Init(m, m <= this.PlayerSeasonLevel, array[m], null, m == this.PlayerSeasonLevel);
			this.SeasonRewardEntries.Add(uiseasonRewardEntry2);
			goto IL_78F;
		}
	}
}

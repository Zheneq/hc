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
		SeasonName = string.Empty;
		SeasonEndTime = string.Empty;
		ChapterEntries = new List<UISeasonChapterEntry>();
		SeasonRewardEntries = new List<UISeasonRewardEntry>();
		FullSeasonRewardEntries = new List<UISeasonRewardEntry>();
		CommunityRankRewardEntries = new List<UISeasonCommunityRankRewardEntry>();
		RepeatingRewards = new List<UISeasonRepeatingRewardInfo>();
	}

	public override bool Equals(object obj)
	{
		if (!(obj is UIPlayerSeasonDisplayInfo))
		{
			while (true)
			{
				return false;
			}
		}
		UIPlayerSeasonDisplayInfo uIPlayerSeasonDisplayInfo = (UIPlayerSeasonDisplayInfo)obj;
		bool flag = SeasonRewardEntries.Count == uIPlayerSeasonDisplayInfo.SeasonRewardEntries.Count;
		if (flag)
		{
			int num = 0;
			while (true)
			{
				if (num < SeasonRewardEntries.Count)
				{
					if (!SeasonRewardEntries[num].Equals(uIPlayerSeasonDisplayInfo.SeasonRewardEntries[num]))
					{
						flag = false;
						break;
					}
					num++;
					continue;
				}
				break;
			}
		}
		bool flag2 = FullSeasonRewardEntries.Count == uIPlayerSeasonDisplayInfo.FullSeasonRewardEntries.Count;
		if (flag2)
		{
			int num2 = 0;
			while (true)
			{
				if (num2 < FullSeasonRewardEntries.Count)
				{
					if (!FullSeasonRewardEntries[num2].Equals(uIPlayerSeasonDisplayInfo.FullSeasonRewardEntries[num2]))
					{
						flag2 = false;
						break;
					}
					num2++;
					continue;
				}
				break;
			}
		}
		bool flag3 = RepeatingRewards.Count == uIPlayerSeasonDisplayInfo.RepeatingRewards.Count;
		if (flag3)
		{
			int num3 = 0;
			while (true)
			{
				if (num3 < RepeatingRewards.Count)
				{
					if (!RepeatingRewards[num3].Equals(uIPlayerSeasonDisplayInfo.RepeatingRewards[num3]))
					{
						flag3 = false;
						break;
					}
					num3++;
					continue;
				}
				break;
			}
		}
		bool flag4 = ChapterEntries.Count == uIPlayerSeasonDisplayInfo.ChapterEntries.Count;
		if (flag4)
		{
			int num4 = 0;
			while (true)
			{
				if (num4 < ChapterEntries.Count)
				{
					if (!ChapterEntries[num4].Equals(uIPlayerSeasonDisplayInfo.ChapterEntries[num4]))
					{
						flag4 = false;
						break;
					}
					num4++;
					continue;
				}
				break;
			}
		}
		int result;
		if (SeasonNumber == uIPlayerSeasonDisplayInfo.SeasonNumber)
		{
			if (CurrentChapter == uIPlayerSeasonDisplayInfo.CurrentChapter)
			{
				if (SeasonName == uIPlayerSeasonDisplayInfo.SeasonName)
				{
					if (PlayerSeasonLevel == uIPlayerSeasonDisplayInfo.PlayerSeasonLevel && currentPercentThroughPlayerSeasonLevel == uIPlayerSeasonDisplayInfo.currentPercentThroughPlayerSeasonLevel)
					{
						if (currentXPThroughPlayerLevel == uIPlayerSeasonDisplayInfo.currentXPThroughPlayerLevel)
						{
							if (currentLevelDisplayIndex == uIPlayerSeasonDisplayInfo.currentLevelDisplayIndex)
							{
								if (flag)
								{
									if (flag2 && flag3)
									{
										result = (flag4 ? 1 : 0);
										goto IL_02e5;
									}
								}
							}
						}
					}
				}
			}
		}
		result = 0;
		goto IL_02e5;
		IL_02e5:
		return (byte)result != 0;
	}

	public override int GetHashCode()
	{
		return SeasonNumber.GetHashCode() ^ CurrentChapter.GetHashCode() ^ PlayerSeasonLevel.GetHashCode() ^ currentPercentThroughPlayerSeasonLevel.GetHashCode() ^ currentXPThroughPlayerLevel.GetHashCode() ^ currentLevelDisplayIndex.GetHashCode();
	}

	public List<UISeasonRepeatingRewardInfo> GetRepeatingRewardsForLevel(int level)
	{
		List<UISeasonRepeatingRewardInfo> list = new List<UISeasonRepeatingRewardInfo>();
		foreach (UISeasonRepeatingRewardInfo repeatingReward in RepeatingRewards)
		{
			if (level >= repeatingReward.StartLevel)
			{
				if ((level - repeatingReward.StartLevel) % repeatingReward.RepeatEveryXLevels == 0)
				{
					list.Add(repeatingReward);
				}
			}
		}
		return list;
	}

	public void Clear()
	{
		SeasonName = string.Empty;
		SeasonEndTime = string.Empty;
		for (int i = 0; i < ChapterEntries.Count; i++)
		{
			ChapterEntries[i].Clear();
		}
		while (true)
		{
			ChapterEntries.Clear();
			for (int j = 0; j < SeasonRewardEntries.Count; j++)
			{
				SeasonRewardEntries[j].Clear();
			}
			while (true)
			{
				SeasonRewardEntries.Clear();
				for (int k = 0; k < FullSeasonRewardEntries.Count; k++)
				{
					FullSeasonRewardEntries[k].Clear();
				}
				FullSeasonRewardEntries.Clear();
				for (int l = 0; l < CommunityRankRewardEntries.Count; l++)
				{
					CommunityRankRewardEntries[l].Clear();
				}
				while (true)
				{
					CommunityRankRewardEntries.Clear();
					PlayerSeasonLevel = 0;
					CommunityRank = 0;
					currentPercentThroughCommunityRank = 0f;
					currentPercentThroughPlayerSeasonLevel = 0f;
					return;
				}
			}
		}
	}

	public void Setup(int seasonNumber, PersistedAccountData accountData)
	{
		SeasonNumber = seasonNumber;
		ExperienceComponent seasonExperienceComponent = accountData.QuestComponent.GetSeasonExperienceComponent(SeasonNumber);
		PlayerSeasonLevel = seasonExperienceComponent.Level;
		CommunityRank = Mathf.FloorToInt(UnityEngine.Random.value * 4f);
		currentPercentThroughCommunityRank = 0f;
		currentXPThroughPlayerLevel = seasonExperienceComponent.XPProgressThroughLevel;
		int seasonExperience = SeasonWideData.Get().GetSeasonExperience(SeasonNumber, PlayerSeasonLevel);
		currentPercentThroughPlayerSeasonLevel = (float)currentXPThroughPlayerLevel / (float)seasonExperience;
		int count = SeasonWideData.Get().GetSeasonTemplate(SeasonNumber).Chapters.Count;
		SeasonName = SeasonWideData.Get().GetSeasonTemplate(SeasonNumber).GetDisplayName();
		TimeRange timeRange = SeasonWideData.Get().GetSeasonTemplate(SeasonNumber).Prerequisites.GetTimeRange();
		if (!timeRange.StartTime.HasValue)
		{
			timeRange.StartTime = new DateTime(2016, 9, 30, 0, 0, 0);
		}
		SeasonEndTime = string.Format(StringUtil.TR("DayMonthYear", "Global"), timeRange.StartTime.Value.Day, StringUtil.TR("Month" + timeRange.StartTime.Value.Month, "Global"), timeRange.StartTime.Value.Year) + " - ";
		if (timeRange.EndTime.HasValue)
		{
			SeasonEndTime += string.Format(StringUtil.TR("DayMonthYear", "Global"), timeRange.EndTime.Value.Day, StringUtil.TR("Month" + timeRange.EndTime.Value.Month, "Global"), timeRange.EndTime.Value.Year);
		}
		else
		{
			SeasonEndTime += "????";
		}
		SeasonEndTime = string.Format(StringUtil.TR("DateInPacificTime", "Global"), SeasonEndTime);
		List<int> unlockedSeasonChapters = accountData.QuestComponent.GetUnlockedSeasonChapters(SeasonNumber);
		int num = 0;
		for (int i = 0; i < unlockedSeasonChapters.Count; i++)
		{
			if (unlockedSeasonChapters[i] > num)
			{
				num = unlockedSeasonChapters[i];
			}
		}
		while (true)
		{
			CurrentChapter = num;
			List<SeasonChapter> chapters = SeasonWideData.Get().GetSeasonTemplate(SeasonNumber).Chapters;
			for (int j = 0; j < count; j++)
			{
				UISeasonChapterEntry uISeasonChapterEntry = new UISeasonChapterEntry();
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
				uISeasonChapterEntry.Setup(chapters[j], previousChapterInfo, nextChapterInfo, SeasonNumber, j);
				ChapterEntries.Add(uISeasonChapterEntry);
			}
			while (true)
			{
				if (SeasonNumber != accountData.QuestComponent.ActiveSeason)
				{
					while (true)
					{
						switch (2)
						{
						default:
							return;
						case 0:
							break;
						}
					}
				}
				List<SeasonReward> allRewards = SeasonWideData.Get().GetSeasonTemplate(SeasonNumber).Rewards.GetAllRewards();
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
						RepeatingRewards.Add(item);
					}
				}
				List<UISeasonRepeatingRewardInfo> repeatingRewards = RepeatingRewards;
				
				repeatingRewards.Sort(delegate(UISeasonRepeatingRewardInfo info1, UISeasonRepeatingRewardInfo info2)
					{
						if (info1 == null)
						{
							if (info2 == null)
							{
								while (true)
								{
									switch (6)
									{
									case 0:
										break;
									default:
										return 0;
									}
								}
							}
						}
						if (info1 == null)
						{
							while (true)
							{
								switch (6)
								{
								case 0:
									break;
								default:
									return 1;
								}
							}
						}
						if (info2 == null)
						{
							while (true)
							{
								switch (2)
								{
								case 0:
									break;
								default:
									return -1;
								}
							}
						}
						return info1.CompareTo(info2);
					});
				int num3 = Mathf.Max(seasonExperienceComponent.Level + 100, num2 + 1);
				List<SeasonReward>[] array = new List<SeasonReward>[num3];
				for (int l = 0; l < allRewards.Count; l++)
				{
					int level = allRewards[l].level;
					if (array[level] == null)
					{
						array[level] = new List<SeasonReward>();
					}
					if (allRewards[l].repeatEveryXLevels != 0)
					{
						continue;
					}
					bool flag = true;
					if (level <= PlayerSeasonLevel)
					{
						if (allRewards[l] is SeasonItemReward)
						{
							SeasonItemReward seasonItemReward = allRewards[l] as SeasonItemReward;
							if (!seasonItemReward.Conditions.IsNullOrEmpty() && accountData.QuestComponent.SeasonItemRewardsGranted.ContainsKey(level))
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
				for (int num4 = 1; num4 < num3; num4++)
				{
					List<UISeasonRepeatingRewardInfo> repeatingRewardsForLevel = GetRepeatingRewardsForLevel(num4);
					int num5;
					if (array != null && array[num4] != null)
					{
						num5 = ((array[num4].Count > 0) ? 1 : 0);
					}
					else
					{
						num5 = 0;
					}
					bool flag2 = (byte)num5 != 0;
					if (!flag2)
					{
						if (repeatingRewardsForLevel.Count <= 0)
						{
							if (num4 != 1)
							{
								goto IL_078f;
							}
						}
					}
					bool flag3 = false;
					using (List<UISeasonRepeatingRewardInfo>.Enumerator enumerator = repeatingRewardsForLevel.GetEnumerator())
					{
						while (true)
						{
							if (!enumerator.MoveNext())
							{
								break;
							}
							UISeasonRepeatingRewardInfo current = enumerator.Current;
							if (current.RepeatEveryXLevels > 1)
							{
								while (true)
								{
									switch (4)
									{
									case 0:
										break;
									default:
										flag3 = true;
										goto end_IL_0666;
									}
								}
							}
						}
						end_IL_0666:;
					}
					if (!flag2 && !flag3)
					{
						if (num4 != 1 && num4 != PlayerSeasonLevel)
						{
							if (num4 != PlayerSeasonLevel + 1)
							{
								goto IL_078f;
							}
						}
					}
					if (num4 == PlayerSeasonLevel)
					{
						currentLevelDisplayIndex = SeasonRewardEntries.Count;
					}
					UISeasonRewardEntry uISeasonRewardEntry = new UISeasonRewardEntry();
					if (array[num4] == null)
					{
						array[num4] = new List<SeasonReward>();
					}
					for (int m = 0; m < repeatingRewardsForLevel.Count; m++)
					{
						array[num4].Add(repeatingRewardsForLevel[m].GetSeasonRewardReference());
					}
					uISeasonRewardEntry.Init(num4, num4 <= PlayerSeasonLevel, array[num4], null, num4 == PlayerSeasonLevel);
					SeasonRewardEntries.Add(uISeasonRewardEntry);
					goto IL_078f;
					IL_078f:
					List<SeasonReward> list = new List<SeasonReward>();
					if (array[num4] != null)
					{
						using (List<SeasonReward>.Enumerator enumerator2 = array[num4].GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								SeasonReward current2 = enumerator2.Current;
								list.Add(current2);
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
					UISeasonRewardEntry uISeasonRewardEntry2 = new UISeasonRewardEntry();
					uISeasonRewardEntry2.Init(num4, num4 <= PlayerSeasonLevel, list, null, num4 == PlayerSeasonLevel);
					FullSeasonRewardEntries.Add(uISeasonRewardEntry2);
				}
				while (true)
				{
					switch (7)
					{
					default:
						return;
					case 0:
						break;
					}
				}
			}
		}
	}
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;

[Serializable]
public class QuestComponent
{
	public Dictionary<int, QuestProgress> Progress
	{
		get;
		set;
	}

	public DateTime StartOfMostRecentDailyPick
	{
		get;
		set;
	}

	public DateTime StartOfMostRecentDailyList
	{
		get;
		set;
	}

	public List<int> OfferedDailies
	{
		get;
		set;
	}

	public Dictionary<int, QuestMetaData> QuestMetaDatas
	{
		get;
		set;
	}

	public int ActiveSeason
	{
		get;
		set;
	}

	public Dictionary<int, ExperienceComponent> SeasonExperience
	{
		get;
		set;
	}

	public Dictionary<int, List<int>> UnlockedSeasonChapters
	{
		get;
		set;
	}

	public Dictionary<int, List<int>> CompletedSeasonChapters
	{
		get;
		set;
	}

	public Dictionary<int, List<int>> NotifiedSeasonChapters
	{
		get;
		set;
	}

	public Dictionary<int, int> SeasonRewardVersion
	{
		get;
		set;
	}

	public Dictionary<int, int> FactionCompetitionLoginRewardVersion
	{
		get;
		set;
	}

	public Dictionary<int, Dictionary<int, FactionTierAndVersion>> FactionCompetitionRewards
	{
		get;
		set;
	}

	public int NotifySeasonEnded
	{
		get;
		set;
	}

	public int NotifiedChapterStarted
	{
		get;
		set;
	}

	public DateTime LastChapterUnlockNotification
	{
		get;
		set;
	}

	public Dictionary<int, List<int>> SeasonItemRewardsGranted
	{
		get;
		set;
	}

	[Obsolete]
	public Dictionary<int, int> CompletedQuests
	{
		get;
		set;
	}

	[Obsolete]
	public Dictionary<int, int> RejectedQuestCount
	{
		get;
		set;
	}

	[Obsolete]
	public Dictionary<int, int> AbandonedQuests
	{
		get;
		set;
	}

	[JsonIgnore]
	public int SeasonLevel
	{
		get
		{
			if (ActiveSeason == 0)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						return 0;
					}
				}
			}
			if (SeasonExperience.ContainsKey(ActiveSeason))
			{
				return SeasonExperience[ActiveSeason].Level;
			}
			return 1;
		}
		set
		{
			if (ActiveSeason == 0)
			{
				return;
			}
			if (value < SeasonExperience[ActiveSeason].Level)
			{
				for (int num = SeasonExperience[ActiveSeason].Level; num > value; num--)
				{
					SeasonItemRewardsGranted.Remove(num);
				}
			}
			SeasonExperience[ActiveSeason].Level = value;
		}
	}

	[JsonIgnore]
	public int SeasonXPProgressThroughLevel
	{
		get
		{
			if (ActiveSeason == 0)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						return 0;
					}
				}
			}
			return SeasonExperience[ActiveSeason].XPProgressThroughLevel;
		}
		set
		{
			if (ActiveSeason == 0)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						return;
					}
				}
			}
			SeasonExperience[ActiveSeason].XPProgressThroughLevel = value;
		}
	}

	[JsonIgnore]
	public int HighestSeasonChapter
	{
		get
		{
			int num = 0;
			if (ActiveSeason == 0)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						return num;
					}
				}
			}
			if (GetUnlockedSeasonChapters(ActiveSeason) == null)
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
			using (List<int>.Enumerator enumerator = GetUnlockedSeasonChapters(ActiveSeason).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					int current = enumerator.Current;
					if (current > num)
					{
						num = current;
					}
				}
			}
			return num + 1;
		}
		set
		{
			if (ActiveSeason == 0)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						return;
					}
				}
			}
			if (value < 1)
			{
				while (true)
				{
					switch (1)
					{
					default:
						return;
					case 0:
						break;
					}
				}
			}
			if (!UnlockedSeasonChapters.ContainsKey(ActiveSeason))
			{
				UnlockedSeasonChapters[ActiveSeason] = new List<int>();
			}
			UnlockedSeasonChapters[ActiveSeason].Clear();
			for (int i = 0; i < value; i++)
			{
				UnlockedSeasonChapters[ActiveSeason].Add(i);
			}
			while (true)
			{
				switch (5)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
	}

	public QuestComponent()
	{
		Progress = new Dictionary<int, QuestProgress>();
		StartOfMostRecentDailyList = DateTime.MinValue;
		StartOfMostRecentDailyPick = DateTime.MinValue;
		OfferedDailies = new List<int>();
		QuestMetaDatas = new Dictionary<int, QuestMetaData>();
		ActiveSeason = 0;
		SeasonExperience = new Dictionary<int, ExperienceComponent>();
		UnlockedSeasonChapters = new Dictionary<int, List<int>>();
		NotifiedSeasonChapters = new Dictionary<int, List<int>>();
		CompletedSeasonChapters = new Dictionary<int, List<int>>();
		SeasonRewardVersion = new Dictionary<int, int>();
		FactionCompetitionLoginRewardVersion = new Dictionary<int, int>();
		FactionCompetitionRewards = new Dictionary<int, Dictionary<int, FactionTierAndVersion>>();
		NotifySeasonEnded = 0;
		SeasonItemRewardsGranted = new Dictionary<int, List<int>>();
	}

	public List<int> GetUnlockedSeasonChapters(int season)
	{
		if (UnlockedSeasonChapters.ContainsKey(season))
		{
			return UnlockedSeasonChapters[season];
		}
		return new List<int>();
	}

	public List<int> GetCompletedSeasonChapters(int season)
	{
		if (CompletedSeasonChapters.ContainsKey(season))
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return CompletedSeasonChapters[season];
				}
			}
		}
		return new List<int>();
	}

	public ExperienceComponent GetSeasonExperienceComponent(int season)
	{
		if (SeasonExperience.ContainsKey(season))
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return SeasonExperience[season];
				}
			}
		}
		return new ExperienceComponent();
	}

	public int GetCompletedCount(int questId)
	{
		if (QuestMetaDatas != null)
		{
			if (QuestMetaDatas.TryGetValue(questId, out QuestMetaData value))
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
						return value.CompletedCount;
					}
				}
			}
		}
		return 0;
	}

	public int GetRejectedCount(int questId)
	{
		if (QuestMetaDatas != null)
		{
			if (QuestMetaDatas.TryGetValue(questId, out QuestMetaData value))
			{
				return value.RejectedCount;
			}
		}
		return 0;
	}

	public int GetAbandonedCount(int questId)
	{
		if (QuestMetaDatas != null)
		{
			if (QuestMetaDatas.TryGetValue(questId, out QuestMetaData value))
			{
				return value.AbandonedCount;
			}
		}
		return 0;
	}

	public int GetWeight(int questId)
	{
		if (QuestMetaDatas != null && QuestMetaDatas.TryGetValue(questId, out QuestMetaData value))
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return value.Weight;
				}
			}
		}
		return 0;
	}

	public QuestMetaData GetOrCreateQuestMetaData(int questId)
	{
		if (!QuestMetaDatas.TryGetValue(questId, out QuestMetaData value))
		{
			value = new QuestMetaData();
			value.UtcCompletedTimes = new List<DateTime>();
			QuestMetaDatas[questId] = value;
		}
		return value;
	}

	public void SetCompletedCount(int questId, int count)
	{
		QuestMetaData orCreateQuestMetaData = GetOrCreateQuestMetaData(questId);
		orCreateQuestMetaData.CompletedCount = count;
		orCreateQuestMetaData.PstAbandonDate = null;
	}

	public void SetRejectedCount(int questId, int count)
	{
		GetOrCreateQuestMetaData(questId).RejectedCount = count;
	}

	public void SetAbandonedCount(int questId, int count)
	{
		QuestMetaData orCreateQuestMetaData = GetOrCreateQuestMetaData(questId);
		orCreateQuestMetaData.AbandonedCount = count;
		orCreateQuestMetaData.PstAbandonDate = null;
	}

	public void SetWeight(int questId, int weight)
	{
		GetOrCreateQuestMetaData(questId).Weight = weight;
	}

	public void SetCompletedDate(int questId, DateTime utcTime)
	{
		QuestMetaData orCreateQuestMetaData = GetOrCreateQuestMetaData(questId);
		if (orCreateQuestMetaData.UtcCompletedTimes == null)
		{
			orCreateQuestMetaData.UtcCompletedTimes = new List<DateTime>();
		}
		orCreateQuestMetaData.UtcCompletedTimes.Add(utcTime);
	}

	public int GetReactorLevel(List<SeasonTemplate> seasons)
	{
		int num = 0;
		using (Dictionary<int, ExperienceComponent>.Enumerator enumerator = SeasonExperience.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<int, ExperienceComponent> current = enumerator.Current;
				int num2 = 0;
				while (true)
				{
					if (num2 >= seasons.Count)
					{
						break;
					}
					if (seasons[num2].Index == current.Key)
					{
						if (!seasons[num2].IsTutorial)
						{
							num += current.Value.Level;
						}
						break;
					}
					num2++;
				}
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return num;
				}
			}
		}
	}

	public void GrantedSeasonItemReward(int seasonLevel, int itemTemplateId)
	{
		if (!SeasonItemRewardsGranted.ContainsKey(seasonLevel))
		{
			SeasonItemRewardsGranted.Add(seasonLevel, new List<int>());
		}
		SeasonItemRewardsGranted[seasonLevel].Add(itemTemplateId);
	}
}

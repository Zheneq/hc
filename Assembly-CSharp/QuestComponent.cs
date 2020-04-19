using System;
using System.Collections.Generic;
using Newtonsoft.Json;

[Serializable]
public class QuestComponent
{
	public QuestComponent()
	{
		this.Progress = new Dictionary<int, QuestProgress>();
		this.StartOfMostRecentDailyList = DateTime.MinValue;
		this.StartOfMostRecentDailyPick = DateTime.MinValue;
		this.OfferedDailies = new List<int>();
		this.QuestMetaDatas = new Dictionary<int, QuestMetaData>();
		this.ActiveSeason = 0;
		this.SeasonExperience = new Dictionary<int, ExperienceComponent>();
		this.UnlockedSeasonChapters = new Dictionary<int, List<int>>();
		this.NotifiedSeasonChapters = new Dictionary<int, List<int>>();
		this.CompletedSeasonChapters = new Dictionary<int, List<int>>();
		this.SeasonRewardVersion = new Dictionary<int, int>();
		this.FactionCompetitionLoginRewardVersion = new Dictionary<int, int>();
		this.FactionCompetitionRewards = new Dictionary<int, Dictionary<int, FactionTierAndVersion>>();
		this.NotifySeasonEnded = 0;
		this.SeasonItemRewardsGranted = new Dictionary<int, List<int>>();
	}

	public Dictionary<int, QuestProgress> Progress { get; set; }

	public DateTime StartOfMostRecentDailyPick { get; set; }

	public DateTime StartOfMostRecentDailyList { get; set; }

	public List<int> OfferedDailies { get; set; }

	public Dictionary<int, QuestMetaData> QuestMetaDatas { get; set; }

	public int ActiveSeason { get; set; }

	public Dictionary<int, ExperienceComponent> SeasonExperience { get; set; }

	public Dictionary<int, List<int>> UnlockedSeasonChapters { get; set; }

	public Dictionary<int, List<int>> CompletedSeasonChapters { get; set; }

	public Dictionary<int, List<int>> NotifiedSeasonChapters { get; set; }

	public Dictionary<int, int> SeasonRewardVersion { get; set; }

	public Dictionary<int, int> FactionCompetitionLoginRewardVersion { get; set; }

	public Dictionary<int, Dictionary<int, FactionTierAndVersion>> FactionCompetitionRewards { get; set; }

	public int NotifySeasonEnded { get; set; }

	public int NotifiedChapterStarted { get; set; }

	public DateTime LastChapterUnlockNotification { get; set; }

	public Dictionary<int, List<int>> SeasonItemRewardsGranted { get; set; }

	[Obsolete]
	public Dictionary<int, int> CompletedQuests { get; set; }

	[Obsolete]
	public Dictionary<int, int> RejectedQuestCount { get; set; }

	[Obsolete]
	public Dictionary<int, int> AbandonedQuests { get; set; }

	public List<int> GetUnlockedSeasonChapters(int season)
	{
		if (this.UnlockedSeasonChapters.ContainsKey(season))
		{
			return this.UnlockedSeasonChapters[season];
		}
		return new List<int>();
	}

	public List<int> GetCompletedSeasonChapters(int season)
	{
		if (this.CompletedSeasonChapters.ContainsKey(season))
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(QuestComponent.GetCompletedSeasonChapters(int)).MethodHandle;
			}
			return this.CompletedSeasonChapters[season];
		}
		return new List<int>();
	}

	public ExperienceComponent GetSeasonExperienceComponent(int season)
	{
		if (this.SeasonExperience.ContainsKey(season))
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(QuestComponent.GetSeasonExperienceComponent(int)).MethodHandle;
			}
			return this.SeasonExperience[season];
		}
		return new ExperienceComponent();
	}

	public int GetCompletedCount(int questId)
	{
		if (this.QuestMetaDatas != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(QuestComponent.GetCompletedCount(int)).MethodHandle;
			}
			QuestMetaData questMetaData;
			if (this.QuestMetaDatas.TryGetValue(questId, out questMetaData))
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				return questMetaData.CompletedCount;
			}
		}
		return 0;
	}

	public int GetRejectedCount(int questId)
	{
		if (this.QuestMetaDatas != null)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(QuestComponent.GetRejectedCount(int)).MethodHandle;
			}
			QuestMetaData questMetaData;
			if (this.QuestMetaDatas.TryGetValue(questId, out questMetaData))
			{
				return questMetaData.RejectedCount;
			}
		}
		return 0;
	}

	public int GetAbandonedCount(int questId)
	{
		if (this.QuestMetaDatas != null)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(QuestComponent.GetAbandonedCount(int)).MethodHandle;
			}
			QuestMetaData questMetaData;
			if (this.QuestMetaDatas.TryGetValue(questId, out questMetaData))
			{
				return questMetaData.AbandonedCount;
			}
		}
		return 0;
	}

	public int GetWeight(int questId)
	{
		QuestMetaData questMetaData;
		if (this.QuestMetaDatas != null && this.QuestMetaDatas.TryGetValue(questId, out questMetaData))
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(QuestComponent.GetWeight(int)).MethodHandle;
			}
			return questMetaData.Weight;
		}
		return 0;
	}

	public QuestMetaData GetOrCreateQuestMetaData(int questId)
	{
		QuestMetaData questMetaData;
		if (!this.QuestMetaDatas.TryGetValue(questId, out questMetaData))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(QuestComponent.GetOrCreateQuestMetaData(int)).MethodHandle;
			}
			questMetaData = new QuestMetaData();
			questMetaData.UtcCompletedTimes = new List<DateTime>();
			this.QuestMetaDatas[questId] = questMetaData;
		}
		return questMetaData;
	}

	public void SetCompletedCount(int questId, int count)
	{
		QuestMetaData orCreateQuestMetaData = this.GetOrCreateQuestMetaData(questId);
		orCreateQuestMetaData.CompletedCount = count;
		orCreateQuestMetaData.PstAbandonDate = null;
	}

	public void SetRejectedCount(int questId, int count)
	{
		this.GetOrCreateQuestMetaData(questId).RejectedCount = count;
	}

	public void SetAbandonedCount(int questId, int count)
	{
		QuestMetaData orCreateQuestMetaData = this.GetOrCreateQuestMetaData(questId);
		orCreateQuestMetaData.AbandonedCount = count;
		orCreateQuestMetaData.PstAbandonDate = null;
	}

	public void SetWeight(int questId, int weight)
	{
		this.GetOrCreateQuestMetaData(questId).Weight = weight;
	}

	public void SetCompletedDate(int questId, DateTime utcTime)
	{
		QuestMetaData orCreateQuestMetaData = this.GetOrCreateQuestMetaData(questId);
		if (orCreateQuestMetaData.UtcCompletedTimes == null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(QuestComponent.SetCompletedDate(int, DateTime)).MethodHandle;
			}
			orCreateQuestMetaData.UtcCompletedTimes = new List<DateTime>();
		}
		orCreateQuestMetaData.UtcCompletedTimes.Add(utcTime);
	}

	public int GetReactorLevel(List<SeasonTemplate> seasons)
	{
		int num = 0;
		using (Dictionary<int, ExperienceComponent>.Enumerator enumerator = this.SeasonExperience.GetEnumerator())
		{
			IL_8F:
			while (enumerator.MoveNext())
			{
				KeyValuePair<int, ExperienceComponent> keyValuePair = enumerator.Current;
				for (int i = 0; i < seasons.Count; i++)
				{
					if (seasons[i].Index == keyValuePair.Key)
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
							RuntimeMethodHandle runtimeMethodHandle = methodof(QuestComponent.GetReactorLevel(List<SeasonTemplate>)).MethodHandle;
						}
						if (!seasons[i].IsTutorial)
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
							num += keyValuePair.Value.Level;
						}
						goto IL_8F;
					}
				}
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		return num;
	}

	public void GrantedSeasonItemReward(int seasonLevel, int itemTemplateId)
	{
		if (!this.SeasonItemRewardsGranted.ContainsKey(seasonLevel))
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(QuestComponent.GrantedSeasonItemReward(int, int)).MethodHandle;
			}
			this.SeasonItemRewardsGranted.Add(seasonLevel, new List<int>());
		}
		this.SeasonItemRewardsGranted[seasonLevel].Add(itemTemplateId);
	}

	[JsonIgnore]
	public int SeasonLevel
	{
		get
		{
			if (this.ActiveSeason == 0)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(QuestComponent.get_SeasonLevel()).MethodHandle;
				}
				return 0;
			}
			if (this.SeasonExperience.ContainsKey(this.ActiveSeason))
			{
				return this.SeasonExperience[this.ActiveSeason].Level;
			}
			return 1;
		}
		set
		{
			if (this.ActiveSeason == 0)
			{
				return;
			}
			if (value < this.SeasonExperience[this.ActiveSeason].Level)
			{
				for (int i = this.SeasonExperience[this.ActiveSeason].Level; i > value; i--)
				{
					this.SeasonItemRewardsGranted.Remove(i);
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
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(QuestComponent.set_SeasonLevel(int)).MethodHandle;
				}
			}
			this.SeasonExperience[this.ActiveSeason].Level = value;
		}
	}

	[JsonIgnore]
	public int SeasonXPProgressThroughLevel
	{
		get
		{
			if (this.ActiveSeason == 0)
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(QuestComponent.get_SeasonXPProgressThroughLevel()).MethodHandle;
				}
				return 0;
			}
			return this.SeasonExperience[this.ActiveSeason].XPProgressThroughLevel;
		}
		set
		{
			if (this.ActiveSeason == 0)
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(QuestComponent.set_SeasonXPProgressThroughLevel(int)).MethodHandle;
				}
				return;
			}
			this.SeasonExperience[this.ActiveSeason].XPProgressThroughLevel = value;
		}
	}

	[JsonIgnore]
	public int HighestSeasonChapter
	{
		get
		{
			int num = 0;
			if (this.ActiveSeason == 0)
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
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(QuestComponent.get_HighestSeasonChapter()).MethodHandle;
				}
				return num;
			}
			if (this.GetUnlockedSeasonChapters(this.ActiveSeason) == null)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				return 1;
			}
			using (List<int>.Enumerator enumerator = this.GetUnlockedSeasonChapters(this.ActiveSeason).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					int num2 = enumerator.Current;
					if (num2 > num)
					{
						for (;;)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							break;
						}
						num = num2;
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
			return num + 1;
		}
		set
		{
			if (this.ActiveSeason == 0)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(QuestComponent.set_HighestSeasonChapter(int)).MethodHandle;
				}
				return;
			}
			if (value < 1)
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				return;
			}
			if (!this.UnlockedSeasonChapters.ContainsKey(this.ActiveSeason))
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
				this.UnlockedSeasonChapters[this.ActiveSeason] = new List<int>();
			}
			this.UnlockedSeasonChapters[this.ActiveSeason].Clear();
			for (int i = 0; i < value; i++)
			{
				this.UnlockedSeasonChapters[this.ActiveSeason].Add(i);
			}
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
		}
	}
}

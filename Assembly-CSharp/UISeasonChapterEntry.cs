using System;
using System.Collections.Generic;

public class UISeasonChapterEntry
{
	public List<QuestCurrencyReward> CurrencyRewards;

	public List<QuestUnlockReward> UnlockRewards;

	public List<QuestItemReward> ItemRewards;

	public string SeasonChapterName;

	public List<SeasonStorytime> SeasonChapterStory;

	public List<UIChallengeDisplayInfo> ChallengeInfo;

	public List<UISeasonQuestDisplayInfo> QuestInfo;

	public bool AreQuestsStatic;

	public bool AreOtherConditionsFromPreviousChapterMet;

	public bool AreAllQuestsCompleteFromPreviousChapter;

	public bool IsChapterLocked;

	public bool IsChapterHidden;

	public bool IsChapterViewable;

	public string UnlockChapterString;

	public int NumQuestsToAdvance;

	public DateTime StartDate;

	public DateTime EndDate;

	private SeasonChapter m_chapterRef;

	public UISeasonChapterEntry()
	{
		this.QuestInfo = new List<UISeasonQuestDisplayInfo>();
		this.ChallengeInfo = new List<UIChallengeDisplayInfo>();
		this.SeasonChapterStory = new List<SeasonStorytime>();
		this.CurrencyRewards = new List<QuestCurrencyReward>();
		this.UnlockRewards = new List<QuestUnlockReward>();
		this.ItemRewards = new List<QuestItemReward>();
	}

	public override bool Equals(object obj)
	{
		if (!(obj is UISeasonChapterEntry))
		{
			return false;
		}
		UISeasonChapterEntry uiseasonChapterEntry = (UISeasonChapterEntry)obj;
		bool flag = this.ChallengeInfo.Count == uiseasonChapterEntry.ChallengeInfo.Count;
		if (flag)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UISeasonChapterEntry.Equals(object)).MethodHandle;
			}
			for (int i = 0; i < this.ChallengeInfo.Count; i++)
			{
				if (!this.ChallengeInfo[i].Equals(uiseasonChapterEntry.ChallengeInfo[i]))
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
					flag = false;
					goto IL_95;
				}
			}
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		IL_95:
		bool flag2 = this.QuestInfo.Count == uiseasonChapterEntry.QuestInfo.Count;
		if (flag2)
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
			for (int j = 0; j < this.QuestInfo.Count; j++)
			{
				if (!this.QuestInfo[j].Equals(uiseasonChapterEntry.QuestInfo[j]))
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
					flag2 = false;
					goto IL_118;
				}
			}
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		IL_118:
		if (this.SeasonChapterName.Equals(uiseasonChapterEntry.SeasonChapterName))
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
			if (flag)
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
				if (flag2 && this.AreOtherConditionsFromPreviousChapterMet.Equals(uiseasonChapterEntry.AreOtherConditionsFromPreviousChapterMet) && this.AreAllQuestsCompleteFromPreviousChapter.Equals(uiseasonChapterEntry.AreAllQuestsCompleteFromPreviousChapter) && this.IsChapterLocked.Equals(uiseasonChapterEntry.IsChapterLocked))
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
					return this.IsChapterHidden.Equals(uiseasonChapterEntry.IsChapterHidden);
				}
			}
		}
		return false;
	}

	public override int GetHashCode()
	{
		if (this.SeasonChapterName != null && this.ChallengeInfo != null && this.QuestInfo != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UISeasonChapterEntry.GetHashCode()).MethodHandle;
			}
			return this.SeasonChapterName.GetHashCode() ^ this.ChallengeInfo.GetHashCode() ^ this.QuestInfo.GetHashCode() ^ this.AreOtherConditionsFromPreviousChapterMet.GetHashCode() ^ this.AreAllQuestsCompleteFromPreviousChapter.GetHashCode() ^ this.IsChapterLocked.GetHashCode() ^ this.IsChapterHidden.GetHashCode();
		}
		return this.AreOtherConditionsFromPreviousChapterMet.GetHashCode() ^ this.AreAllQuestsCompleteFromPreviousChapter.GetHashCode() ^ this.IsChapterLocked.GetHashCode() ^ this.IsChapterHidden.GetHashCode();
	}

	public void Setup(SeasonChapter chapterInfo, SeasonChapter previousChapterInfo, SeasonChapter nextChapterInfo, int seasonNumber, int chapterIndex)
	{
		this.SeasonChapterName = StringUtil.TR_SeasonChapterName(seasonNumber, chapterIndex + 1);
		using (List<SeasonStorytime>.Enumerator enumerator = chapterInfo.StorytimePanels.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				SeasonStorytime item = enumerator.Current;
				this.SeasonChapterStory.Add(item);
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UISeasonChapterEntry.Setup(SeasonChapter, SeasonChapter, SeasonChapter, int, int)).MethodHandle;
			}
		}
		this.AreQuestsStatic = !chapterInfo.NormalQuests.IsNullOrEmpty<int>();
		TimeRange timeRange = chapterInfo.Prerequisites.GetTimeRange();
		TimeRange timeRange2;
		if (nextChapterInfo != null)
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
			timeRange2 = nextChapterInfo.Prerequisites.GetTimeRange();
		}
		else
		{
			timeRange2 = null;
		}
		TimeRange timeRange3 = timeRange2;
		TimeRange timeRange4;
		if (previousChapterInfo != null)
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
			timeRange4 = previousChapterInfo.Prerequisites.GetTimeRange();
		}
		else
		{
			timeRange4 = null;
		}
		TimeRange timeRange5 = timeRange4;
		TimeRange timeRange6 = SeasonWideData.Get().GetSeasonTemplate(seasonNumber).Prerequisites.GetTimeRange();
		if (timeRange.EndTime != null)
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
			this.EndDate = timeRange.EndTime.Value;
		}
		else if (timeRange3 != null && timeRange3.StartTime != null)
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
			this.EndDate = timeRange3.StartTime.Value;
		}
		else if (timeRange6.EndTime != null)
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
			this.EndDate = timeRange6.EndTime.Value;
		}
		else
		{
			this.EndDate = DateTime.MaxValue;
		}
		if (timeRange.StartTime != null)
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
			this.StartDate = timeRange.StartTime.Value;
		}
		else
		{
			if (timeRange5 != null)
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
				if (timeRange5.EndTime != null)
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
					this.StartDate = timeRange5.EndTime.Value;
					goto IL_222;
				}
			}
			if (timeRange6.StartTime != null)
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
				this.StartDate = timeRange6.StartTime.Value;
			}
			else
			{
				this.StartDate = DateTime.MinValue;
			}
		}
		IL_222:
		List<int> chapterQuests = UISeasonsPanel.GetChapterQuests(chapterInfo, seasonNumber, chapterIndex);
		int count = chapterQuests.Count;
		PersistedAccountData playerAccountData = ClientGameManager.Get().GetPlayerAccountData();
		bool flag = true;
		int num = 0;
		if (previousChapterInfo != null)
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
			using (List<int>.Enumerator enumerator2 = UISeasonsPanel.GetChapterQuests(previousChapterInfo, seasonNumber, chapterIndex - 1).GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					int questId = enumerator2.Current;
					if (playerAccountData.QuestComponent.GetCompletedCount(questId) <= 0)
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
						flag = false;
					}
					else
					{
						num++;
					}
				}
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
			}
		}
		this.NumQuestsToAdvance = chapterInfo.NumQuestsToAdvance;
		bool areAllQuestsCompleteFromPreviousChapter;
		if (!flag)
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
			if (this.NumQuestsToAdvance == num)
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
				areAllQuestsCompleteFromPreviousChapter = (this.NumQuestsToAdvance != 0);
			}
			else
			{
				areAllQuestsCompleteFromPreviousChapter = false;
			}
		}
		else
		{
			areAllQuestsCompleteFromPreviousChapter = true;
		}
		this.AreAllQuestsCompleteFromPreviousChapter = areAllQuestsCompleteFromPreviousChapter;
		this.AreOtherConditionsFromPreviousChapterMet = QuestWideData.AreConditionsMet(chapterInfo.Prerequisites.Conditions, chapterInfo.Prerequisites.LogicStatement, false);
		bool isChapterLocked;
		if (this.AreAllQuestsCompleteFromPreviousChapter)
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
			isChapterLocked = !this.AreOtherConditionsFromPreviousChapterMet;
		}
		else
		{
			isChapterLocked = true;
		}
		this.IsChapterLocked = isChapterLocked;
		this.IsChapterHidden = chapterInfo.Hidden;
		this.IsChapterViewable = this.AreOtherConditionsFromPreviousChapterMet;
		if (playerAccountData.QuestComponent.GetUnlockedSeasonChapters(seasonNumber).Contains(chapterIndex))
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
			this.IsChapterLocked = false;
			this.IsChapterHidden = false;
			this.IsChapterViewable = true;
		}
		this.UnlockChapterString = StringUtil.TR_SeasonChapterUnlock(seasonNumber, chapterIndex + 1);
		for (int i = 0; i < count; i++)
		{
			UISeasonQuestDisplayInfo uiseasonQuestDisplayInfo = new UISeasonQuestDisplayInfo();
			uiseasonQuestDisplayInfo.Setup(chapterQuests[i], seasonNumber, chapterIndex + 1, this.AreQuestsStatic, this.StartDate, this.EndDate);
			this.QuestInfo.Add(uiseasonQuestDisplayInfo);
		}
		using (List<QuestCurrencyReward>.Enumerator enumerator3 = chapterInfo.CurrencyRewards.GetEnumerator())
		{
			while (enumerator3.MoveNext())
			{
				QuestCurrencyReward item2 = enumerator3.Current;
				this.CurrencyRewards.Add(item2);
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
		using (List<QuestItemReward>.Enumerator enumerator4 = chapterInfo.ItemRewards.GetEnumerator())
		{
			while (enumerator4.MoveNext())
			{
				QuestItemReward item3 = enumerator4.Current;
				this.ItemRewards.Add(item3);
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
		foreach (QuestUnlockReward item4 in chapterInfo.UnlockRewards)
		{
			this.UnlockRewards.Add(item4);
		}
	}

	public void Clear()
	{
		this.CurrencyRewards.Clear();
		this.ItemRewards.Clear();
		this.UnlockRewards.Clear();
		this.SeasonChapterStory.Clear();
		this.QuestInfo.Clear();
		this.ChallengeInfo.Clear();
	}
}

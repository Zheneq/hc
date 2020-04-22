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
		QuestInfo = new List<UISeasonQuestDisplayInfo>();
		ChallengeInfo = new List<UIChallengeDisplayInfo>();
		SeasonChapterStory = new List<SeasonStorytime>();
		CurrencyRewards = new List<QuestCurrencyReward>();
		UnlockRewards = new List<QuestUnlockReward>();
		ItemRewards = new List<QuestItemReward>();
	}

	public override bool Equals(object obj)
	{
		if (!(obj is UISeasonChapterEntry))
		{
			return false;
		}
		UISeasonChapterEntry uISeasonChapterEntry = (UISeasonChapterEntry)obj;
		bool flag = ChallengeInfo.Count == uISeasonChapterEntry.ChallengeInfo.Count;
		if (flag)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			int num = 0;
			while (true)
			{
				if (num < ChallengeInfo.Count)
				{
					if (!ChallengeInfo[num].Equals(uISeasonChapterEntry.ChallengeInfo[num]))
					{
						while (true)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
						flag = false;
						break;
					}
					num++;
					continue;
				}
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				break;
			}
		}
		bool flag2 = QuestInfo.Count == uISeasonChapterEntry.QuestInfo.Count;
		if (flag2)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			int num2 = 0;
			while (true)
			{
				if (num2 < QuestInfo.Count)
				{
					if (!QuestInfo[num2].Equals(uISeasonChapterEntry.QuestInfo[num2]))
					{
						while (true)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							break;
						}
						flag2 = false;
						break;
					}
					num2++;
					continue;
				}
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				break;
			}
		}
		int result;
		if (SeasonChapterName.Equals(uISeasonChapterEntry.SeasonChapterName))
		{
			while (true)
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
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (flag2 && AreOtherConditionsFromPreviousChapterMet.Equals(uISeasonChapterEntry.AreOtherConditionsFromPreviousChapterMet) && AreAllQuestsCompleteFromPreviousChapter.Equals(uISeasonChapterEntry.AreAllQuestsCompleteFromPreviousChapter) && IsChapterLocked.Equals(uISeasonChapterEntry.IsChapterLocked))
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					result = (IsChapterHidden.Equals(uISeasonChapterEntry.IsChapterHidden) ? 1 : 0);
					goto IL_01a3;
				}
			}
		}
		result = 0;
		goto IL_01a3;
		IL_01a3:
		return (byte)result != 0;
	}

	public override int GetHashCode()
	{
		if (SeasonChapterName != null && ChallengeInfo != null && QuestInfo != null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return SeasonChapterName.GetHashCode() ^ ChallengeInfo.GetHashCode() ^ QuestInfo.GetHashCode() ^ AreOtherConditionsFromPreviousChapterMet.GetHashCode() ^ AreAllQuestsCompleteFromPreviousChapter.GetHashCode() ^ IsChapterLocked.GetHashCode() ^ IsChapterHidden.GetHashCode();
				}
			}
		}
		return AreOtherConditionsFromPreviousChapterMet.GetHashCode() ^ AreAllQuestsCompleteFromPreviousChapter.GetHashCode() ^ IsChapterLocked.GetHashCode() ^ IsChapterHidden.GetHashCode();
	}

	public void Setup(SeasonChapter chapterInfo, SeasonChapter previousChapterInfo, SeasonChapter nextChapterInfo, int seasonNumber, int chapterIndex)
	{
		SeasonChapterName = StringUtil.TR_SeasonChapterName(seasonNumber, chapterIndex + 1);
		using (List<SeasonStorytime>.Enumerator enumerator = chapterInfo.StorytimePanels.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				SeasonStorytime current = enumerator.Current;
				SeasonChapterStory.Add(current);
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					goto end_IL_0021;
				}
			}
			end_IL_0021:;
		}
		AreQuestsStatic = !chapterInfo.NormalQuests.IsNullOrEmpty();
		TimeRange timeRange = chapterInfo.Prerequisites.GetTimeRange();
		object obj;
		if (nextChapterInfo != null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			obj = nextChapterInfo.Prerequisites.GetTimeRange();
		}
		else
		{
			obj = null;
		}
		TimeRange timeRange2 = (TimeRange)obj;
		object obj2;
		if (previousChapterInfo != null)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			obj2 = previousChapterInfo.Prerequisites.GetTimeRange();
		}
		else
		{
			obj2 = null;
		}
		TimeRange timeRange3 = (TimeRange)obj2;
		TimeRange timeRange4 = SeasonWideData.Get().GetSeasonTemplate(seasonNumber).Prerequisites.GetTimeRange();
		if (timeRange.EndTime.HasValue)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			EndDate = timeRange.EndTime.Value;
		}
		else if (timeRange2 != null && timeRange2.StartTime.HasValue)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			EndDate = timeRange2.StartTime.Value;
		}
		else if (timeRange4.EndTime.HasValue)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			EndDate = timeRange4.EndTime.Value;
		}
		else
		{
			EndDate = DateTime.MaxValue;
		}
		if (timeRange.StartTime.HasValue)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			StartDate = timeRange.StartTime.Value;
		}
		else
		{
			if (timeRange3 != null)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (timeRange3.EndTime.HasValue)
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					StartDate = timeRange3.EndTime.Value;
					goto IL_0222;
				}
			}
			if (timeRange4.StartTime.HasValue)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				StartDate = timeRange4.StartTime.Value;
			}
			else
			{
				StartDate = DateTime.MinValue;
			}
		}
		goto IL_0222;
		IL_0222:
		List<int> chapterQuests = UISeasonsPanel.GetChapterQuests(chapterInfo, seasonNumber, chapterIndex);
		int count = chapterQuests.Count;
		PersistedAccountData playerAccountData = ClientGameManager.Get().GetPlayerAccountData();
		bool flag = true;
		int num = 0;
		if (previousChapterInfo != null)
		{
			while (true)
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
					int current2 = enumerator2.Current;
					if (playerAccountData.QuestComponent.GetCompletedCount(current2) <= 0)
					{
						while (true)
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
				while (true)
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
		NumQuestsToAdvance = chapterInfo.NumQuestsToAdvance;
		int areAllQuestsCompleteFromPreviousChapter;
		if (!flag)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (NumQuestsToAdvance == num)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				areAllQuestsCompleteFromPreviousChapter = ((NumQuestsToAdvance != 0) ? 1 : 0);
			}
			else
			{
				areAllQuestsCompleteFromPreviousChapter = 0;
			}
		}
		else
		{
			areAllQuestsCompleteFromPreviousChapter = 1;
		}
		AreAllQuestsCompleteFromPreviousChapter = ((byte)areAllQuestsCompleteFromPreviousChapter != 0);
		AreOtherConditionsFromPreviousChapterMet = QuestWideData.AreConditionsMet(chapterInfo.Prerequisites.Conditions, chapterInfo.Prerequisites.LogicStatement);
		int isChapterLocked;
		if (AreAllQuestsCompleteFromPreviousChapter)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			isChapterLocked = ((!AreOtherConditionsFromPreviousChapterMet) ? 1 : 0);
		}
		else
		{
			isChapterLocked = 1;
		}
		IsChapterLocked = ((byte)isChapterLocked != 0);
		IsChapterHidden = chapterInfo.Hidden;
		IsChapterViewable = AreOtherConditionsFromPreviousChapterMet;
		if (playerAccountData.QuestComponent.GetUnlockedSeasonChapters(seasonNumber).Contains(chapterIndex))
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			IsChapterLocked = false;
			IsChapterHidden = false;
			IsChapterViewable = true;
		}
		UnlockChapterString = StringUtil.TR_SeasonChapterUnlock(seasonNumber, chapterIndex + 1);
		for (int i = 0; i < count; i++)
		{
			UISeasonQuestDisplayInfo uISeasonQuestDisplayInfo = new UISeasonQuestDisplayInfo();
			uISeasonQuestDisplayInfo.Setup(chapterQuests[i], seasonNumber, chapterIndex + 1, AreQuestsStatic, StartDate, EndDate);
			QuestInfo.Add(uISeasonQuestDisplayInfo);
		}
		using (List<QuestCurrencyReward>.Enumerator enumerator3 = chapterInfo.CurrencyRewards.GetEnumerator())
		{
			while (enumerator3.MoveNext())
			{
				QuestCurrencyReward current3 = enumerator3.Current;
				CurrencyRewards.Add(current3);
			}
			while (true)
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
				QuestItemReward current4 = enumerator4.Current;
				ItemRewards.Add(current4);
			}
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		foreach (QuestUnlockReward unlockReward in chapterInfo.UnlockRewards)
		{
			UnlockRewards.Add(unlockReward);
		}
	}

	public void Clear()
	{
		CurrencyRewards.Clear();
		ItemRewards.Clear();
		UnlockRewards.Clear();
		SeasonChapterStory.Clear();
		QuestInfo.Clear();
		ChallengeInfo.Clear();
	}
}

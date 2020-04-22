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
			int num = 0;
			while (true)
			{
				if (num < ChallengeInfo.Count)
				{
					if (!ChallengeInfo[num].Equals(uISeasonChapterEntry.ChallengeInfo[num]))
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
		bool flag2 = QuestInfo.Count == uISeasonChapterEntry.QuestInfo.Count;
		if (flag2)
		{
			int num2 = 0;
			while (true)
			{
				if (num2 < QuestInfo.Count)
				{
					if (!QuestInfo[num2].Equals(uISeasonChapterEntry.QuestInfo[num2]))
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
		int result;
		if (SeasonChapterName.Equals(uISeasonChapterEntry.SeasonChapterName))
		{
			if (flag)
			{
				if (flag2 && AreOtherConditionsFromPreviousChapterMet.Equals(uISeasonChapterEntry.AreOtherConditionsFromPreviousChapterMet) && AreAllQuestsCompleteFromPreviousChapter.Equals(uISeasonChapterEntry.AreAllQuestsCompleteFromPreviousChapter) && IsChapterLocked.Equals(uISeasonChapterEntry.IsChapterLocked))
				{
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
			EndDate = timeRange.EndTime.Value;
		}
		else if (timeRange2 != null && timeRange2.StartTime.HasValue)
		{
			EndDate = timeRange2.StartTime.Value;
		}
		else if (timeRange4.EndTime.HasValue)
		{
			EndDate = timeRange4.EndTime.Value;
		}
		else
		{
			EndDate = DateTime.MaxValue;
		}
		if (timeRange.StartTime.HasValue)
		{
			StartDate = timeRange.StartTime.Value;
		}
		else
		{
			if (timeRange3 != null)
			{
				if (timeRange3.EndTime.HasValue)
				{
					StartDate = timeRange3.EndTime.Value;
					goto IL_0222;
				}
			}
			if (timeRange4.StartTime.HasValue)
			{
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
			using (List<int>.Enumerator enumerator2 = UISeasonsPanel.GetChapterQuests(previousChapterInfo, seasonNumber, chapterIndex - 1).GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					int current2 = enumerator2.Current;
					if (playerAccountData.QuestComponent.GetCompletedCount(current2) <= 0)
					{
						flag = false;
					}
					else
					{
						num++;
					}
				}
			}
		}
		NumQuestsToAdvance = chapterInfo.NumQuestsToAdvance;
		int areAllQuestsCompleteFromPreviousChapter;
		if (!flag)
		{
			if (NumQuestsToAdvance == num)
			{
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
		}
		using (List<QuestItemReward>.Enumerator enumerator4 = chapterInfo.ItemRewards.GetEnumerator())
		{
			while (enumerator4.MoveNext())
			{
				QuestItemReward current4 = enumerator4.Current;
				ItemRewards.Add(current4);
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

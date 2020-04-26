using System;

public class UISeasonQuestDisplayInfo : UIBaseQuestDisplayInfo
{
	public bool IsQuestStatic;

	public int SeasonId;

	public int ChapterId;

	public DateTime ChapterStartDate;

	public DateTime ChapterEndDate;

	public virtual void Setup(int questIndex, int seasonNumber, int chapterId, bool isQuestStatic, DateTime chapterStartDate, DateTime chapterEndDate)
	{
		base.Setup(questIndex);
		SeasonId = seasonNumber;
		ChapterId = chapterId;
		IsQuestStatic = isQuestStatic;
		ChapterStartDate = chapterStartDate;
		ChapterEndDate = chapterEndDate;
		if (isQuestStatic)
		{
			return;
		}
		while (true)
		{
			if (!ClientGameManager.Get().IsPlayerAccountDataAvailable())
			{
				return;
			}
			while (true)
			{
				QuestComponent questComponent = ClientGameManager.Get().GetPlayerAccountData().QuestComponent;
				QuestMetaData orCreateQuestMetaData = questComponent.GetOrCreateQuestMetaData(questIndex);
				if (!orCreateQuestMetaData.UtcCompletedTimes.IsNullOrEmpty())
				{
					for (int i = 0; i < orCreateQuestMetaData.UtcCompletedTimes.Count; i++)
					{
						if (!(orCreateQuestMetaData.UtcCompletedTimes[i] > chapterStartDate) || !(orCreateQuestMetaData.UtcCompletedTimes[i] <= chapterEndDate))
						{
							continue;
						}
						while (true)
						{
							Completed = true;
							return;
						}
					}
				}
				Completed = false;
				return;
			}
		}
	}

	public override bool Equals(object obj)
	{
		if (obj is UISeasonQuestDisplayInfo)
		{
			if (base.Equals(obj))
			{
				UISeasonQuestDisplayInfo uISeasonQuestDisplayInfo = obj as UISeasonQuestDisplayInfo;
				int result;
				if (SeasonId == uISeasonQuestDisplayInfo.SeasonId)
				{
					result = ((ChapterId == uISeasonQuestDisplayInfo.ChapterId) ? 1 : 0);
				}
				else
				{
					result = 0;
				}
				return (byte)result != 0;
			}
		}
		return false;
	}

	public override int GetHashCode()
	{
		int num = Completed.GetHashCode() ^ SeasonId.GetHashCode() ^ ChapterId.GetHashCode();
		if (QuestProgressRef != null)
		{
			if (QuestProgressRef.ObjectiveProgressLastDate != null)
			{
				if (QuestProgressRef.ObjectiveProgress != null)
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							break;
						default:
							return num ^ QuestProgressRef.ObjectiveProgressLastDate.GetHashCode() ^ QuestProgressRef.ObjectiveProgress.GetHashCode();
						}
					}
				}
			}
		}
		return num;
	}
}

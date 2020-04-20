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
		this.SeasonId = seasonNumber;
		this.ChapterId = chapterId;
		this.IsQuestStatic = isQuestStatic;
		this.ChapterStartDate = chapterStartDate;
		this.ChapterEndDate = chapterEndDate;
		if (!isQuestStatic)
		{
			if (ClientGameManager.Get().IsPlayerAccountDataAvailable())
			{
				QuestComponent questComponent = ClientGameManager.Get().GetPlayerAccountData().QuestComponent;
				QuestMetaData orCreateQuestMetaData = questComponent.GetOrCreateQuestMetaData(questIndex);
				if (!orCreateQuestMetaData.UtcCompletedTimes.IsNullOrEmpty<DateTime>())
				{
					for (int i = 0; i < orCreateQuestMetaData.UtcCompletedTimes.Count; i++)
					{
						if (orCreateQuestMetaData.UtcCompletedTimes[i] > chapterStartDate && orCreateQuestMetaData.UtcCompletedTimes[i] <= chapterEndDate)
						{
							this.Completed = true;
							return;
						}
					}
				}
				this.Completed = false;
			}
		}
	}

	public override bool Equals(object obj)
	{
		if (obj is UISeasonQuestDisplayInfo)
		{
			if (base.Equals(obj))
			{
				UISeasonQuestDisplayInfo uiseasonQuestDisplayInfo = obj as UISeasonQuestDisplayInfo;
				bool result;
				if (this.SeasonId == uiseasonQuestDisplayInfo.SeasonId)
				{
					result = (this.ChapterId == uiseasonQuestDisplayInfo.ChapterId);
				}
				else
				{
					result = false;
				}
				return result;
			}
		}
		return false;
	}

	public override int GetHashCode()
	{
		int num = this.Completed.GetHashCode() ^ this.SeasonId.GetHashCode() ^ this.ChapterId.GetHashCode();
		if (this.QuestProgressRef != null)
		{
			if (this.QuestProgressRef.ObjectiveProgressLastDate != null)
			{
				if (this.QuestProgressRef.ObjectiveProgress != null)
				{
					return num ^ this.QuestProgressRef.ObjectiveProgressLastDate.GetHashCode() ^ this.QuestProgressRef.ObjectiveProgress.GetHashCode();
				}
			}
		}
		return num;
	}
}

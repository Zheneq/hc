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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UISeasonQuestDisplayInfo.Setup(int, int, int, bool, DateTime, DateTime)).MethodHandle;
			}
			if (ClientGameManager.Get().IsPlayerAccountDataAvailable())
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
				QuestComponent questComponent = ClientGameManager.Get().GetPlayerAccountData().QuestComponent;
				QuestMetaData orCreateQuestMetaData = questComponent.GetOrCreateQuestMetaData(questIndex);
				if (!orCreateQuestMetaData.UtcCompletedTimes.IsNullOrEmpty<DateTime>())
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
					for (int i = 0; i < orCreateQuestMetaData.UtcCompletedTimes.Count; i++)
					{
						if (orCreateQuestMetaData.UtcCompletedTimes[i] > chapterStartDate && orCreateQuestMetaData.UtcCompletedTimes[i] <= chapterEndDate)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UISeasonQuestDisplayInfo.Equals(object)).MethodHandle;
			}
			if (base.Equals(obj))
			{
				UISeasonQuestDisplayInfo uiseasonQuestDisplayInfo = obj as UISeasonQuestDisplayInfo;
				bool result;
				if (this.SeasonId == uiseasonQuestDisplayInfo.SeasonId)
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
					result = (this.ChapterId == uiseasonQuestDisplayInfo.ChapterId);
				}
				else
				{
					result = false;
				}
				return result;
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
		return false;
	}

	public override int GetHashCode()
	{
		int num = this.Completed.GetHashCode() ^ this.SeasonId.GetHashCode() ^ this.ChapterId.GetHashCode();
		if (this.QuestProgressRef != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UISeasonQuestDisplayInfo.GetHashCode()).MethodHandle;
			}
			if (this.QuestProgressRef.ObjectiveProgressLastDate != null)
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
				if (this.QuestProgressRef.ObjectiveProgress != null)
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
					return num ^ this.QuestProgressRef.ObjectiveProgressLastDate.GetHashCode() ^ this.QuestProgressRef.ObjectiveProgress.GetHashCode();
				}
			}
		}
		return num;
	}
}

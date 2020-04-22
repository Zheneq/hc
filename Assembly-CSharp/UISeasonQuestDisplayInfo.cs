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
			switch (2)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (!ClientGameManager.Get().IsPlayerAccountDataAvailable())
			{
				return;
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				QuestComponent questComponent = ClientGameManager.Get().GetPlayerAccountData().QuestComponent;
				QuestMetaData orCreateQuestMetaData = questComponent.GetOrCreateQuestMetaData(questIndex);
				if (!orCreateQuestMetaData.UtcCompletedTimes.IsNullOrEmpty())
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
					for (int i = 0; i < orCreateQuestMetaData.UtcCompletedTimes.Count; i++)
					{
						if (!(orCreateQuestMetaData.UtcCompletedTimes[i] > chapterStartDate) || !(orCreateQuestMetaData.UtcCompletedTimes[i] <= chapterEndDate))
						{
							continue;
						}
						while (true)
						{
							switch (6)
							{
							case 0:
								continue;
							}
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
			while (true)
			{
				switch (2)
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
			if (base.Equals(obj))
			{
				UISeasonQuestDisplayInfo uISeasonQuestDisplayInfo = obj as UISeasonQuestDisplayInfo;
				int result;
				if (SeasonId == uISeasonQuestDisplayInfo.SeasonId)
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
					result = ((ChapterId == uISeasonQuestDisplayInfo.ChapterId) ? 1 : 0);
				}
				else
				{
					result = 0;
				}
				return (byte)result != 0;
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
		}
		return false;
	}

	public override int GetHashCode()
	{
		int num = Completed.GetHashCode() ^ SeasonId.GetHashCode() ^ ChapterId.GetHashCode();
		if (QuestProgressRef != null)
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
			if (QuestProgressRef.ObjectiveProgressLastDate != null)
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

using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class SeasonQuestActionRequest : WebSocketMessage
	{
		public enum ActionType
		{
			RerollSeasonQuests,
			SetSeasonQuest
		}

		public ActionType Action;
		public int SeasonId;
		public int ChapterId;
		public int SlotNum;
		public int QuestId;
	}
}

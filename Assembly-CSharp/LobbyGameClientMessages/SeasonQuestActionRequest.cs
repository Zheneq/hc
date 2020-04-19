using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class SeasonQuestActionRequest : WebSocketMessage
	{
		public SeasonQuestActionRequest.ActionType Action;

		public int SeasonId;

		public int ChapterId;

		public int SlotNum;

		public int QuestId;

		public enum ActionType
		{
			\u001D,
			\u000E
		}
	}
}

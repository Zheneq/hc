using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class QuestCompleteNotification : WebSocketMessage
	{
		public int questId;

		public int rejectedCount;
	}
}

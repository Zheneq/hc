using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class AbandonDailyQuestRequest : WebSocketMessage
	{
		public int questId;
	}
}

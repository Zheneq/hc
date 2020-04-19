using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class PickDailyQuestRequest : WebSocketMessage
	{
		public int questId;
	}
}

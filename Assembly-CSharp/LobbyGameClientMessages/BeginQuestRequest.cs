using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class BeginQuestRequest : WebSocketMessage
	{
		public int QuestId;
	}
}

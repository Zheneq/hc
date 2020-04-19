using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class CompleteQuestRequest : WebSocketMessage
	{
		public int QuestId;
	}
}

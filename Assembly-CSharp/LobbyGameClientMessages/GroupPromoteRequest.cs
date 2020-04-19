using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class GroupPromoteRequest : WebSocketMessage
	{
		public string Name;

		public long AccountId;
	}
}

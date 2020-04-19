using System;
using System.Collections.Generic;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class GroupChatRequest : WebSocketMessage
	{
		public string Text;

		public List<int> RequestedEmojis;

		public long AccountId;
	}
}

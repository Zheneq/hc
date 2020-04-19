using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class GroupLeaveRequest : WebSocketMessage
	{
		public long AccountId;
	}
}

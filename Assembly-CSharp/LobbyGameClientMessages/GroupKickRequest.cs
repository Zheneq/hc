using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class GroupKickRequest : WebSocketMessage
	{
		public string MemberName;

		public long AccountId;
	}
}

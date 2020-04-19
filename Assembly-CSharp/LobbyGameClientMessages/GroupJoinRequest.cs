using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class GroupJoinRequest : WebSocketMessage
	{
		public string FriendHandle;
	}
}

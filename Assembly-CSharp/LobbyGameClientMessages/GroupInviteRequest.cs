using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class GroupInviteRequest : WebSocketMessage
	{
		public string FriendHandle;
	}
}

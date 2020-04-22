using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class FriendStatusNotification : WebSocketMessage
	{
		public FriendList FriendList;
	}
}

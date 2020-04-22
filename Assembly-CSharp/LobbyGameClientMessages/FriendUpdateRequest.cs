using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class FriendUpdateRequest : WebSocketMessage
	{
		public string FriendHandle;

		public long FriendAccountId;

		public FriendOperation FriendOperation;

		public string StringData;
	}
}

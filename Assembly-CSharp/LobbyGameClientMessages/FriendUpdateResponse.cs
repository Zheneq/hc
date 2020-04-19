using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class FriendUpdateResponse : WebSocketResponseMessage
	{
		public string FriendHandle;

		public long FriendAccountId;

		public FriendOperation FriendOperation;

		public LocalizationPayload LocalizedFailure;
	}
}

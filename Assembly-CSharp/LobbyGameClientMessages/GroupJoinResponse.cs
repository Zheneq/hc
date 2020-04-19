using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class GroupJoinResponse : WebSocketResponseMessage
	{
		public string FriendHandle;

		public LocalizationPayload LocalizedFailure;
	}
}

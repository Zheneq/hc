using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class GroupInviteResponse : WebSocketResponseMessage
	{
		public string FriendHandle;

		public LocalizationPayload LocalizedFailure;
	}
}

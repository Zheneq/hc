using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class GroupConfirmationResponse : WebSocketResponseMessage
	{
		public long GroupId;

		public GroupInviteResponseType Acceptance;

		public long ConfirmationNumber;

		public long JoinerAccountId;
	}
}

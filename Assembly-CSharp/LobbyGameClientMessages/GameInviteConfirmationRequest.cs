using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class GameInviteConfirmationRequest : WebSocketMessage
	{
		public string GameCreatorHandle;

		public long GameCreatorAccountId;

		public int InitialRequestId;
	}
}

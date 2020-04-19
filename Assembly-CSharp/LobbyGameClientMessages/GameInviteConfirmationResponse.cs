using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class GameInviteConfirmationResponse : WebSocketResponseMessage
	{
		public bool Accepted;

		public long GameCreatorAccountId;

		public int InitialRequestId;

		public LocalizationPayload LocalizedFailure;
	}
}

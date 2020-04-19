using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class CreateGameResponse : WebSocketResponseMessage
	{
		public LocalizationPayload LocalizedFailure;

		public bool AllowRetry;
	}
}

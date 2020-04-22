using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class SetGameSubTypeResponse : WebSocketResponseMessage
	{
		public LocalizationPayload LocalizedFailure;
	}
}

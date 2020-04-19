using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class JoinMatchmakingQueueResponse : WebSocketResponseMessage
	{
		public LocalizationPayload LocalizedFailure;
	}
}

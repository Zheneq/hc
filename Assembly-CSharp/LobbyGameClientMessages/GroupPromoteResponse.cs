using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class GroupPromoteResponse : WebSocketResponseMessage
	{
		public LocalizationPayload LocalizedFailure;
	}
}

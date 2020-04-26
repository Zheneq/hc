using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class GroupLeaveResponse : WebSocketResponseMessage
	{
		public LocalizationPayload LocalizedFailure;
	}
}

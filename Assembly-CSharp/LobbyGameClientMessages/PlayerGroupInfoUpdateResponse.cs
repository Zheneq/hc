using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class PlayerGroupInfoUpdateResponse : WebSocketResponseMessage
	{
		public LocalizationPayload LocalizedFailure;
	}
}

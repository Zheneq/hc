using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class RankedSelectionResponse : WebSocketResponseMessage
	{
		public LocalizationPayload LocalizedFailure;
	}
}

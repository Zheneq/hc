using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class ForcedCharacterChangeFromServerNotification : WebSocketMessage
	{
		public LobbyCharacterInfo ChararacterInfo;
	}
}

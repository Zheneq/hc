using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class PreviousGameInfoResponse : WebSocketResponseMessage
	{
		public LobbyGameInfo PreviousGameInfo;
	}
}

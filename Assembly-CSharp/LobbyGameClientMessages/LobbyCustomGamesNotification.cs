using System;
using System.Collections.Generic;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class LobbyCustomGamesNotification : WebSocketMessage
	{
		public List<LobbyGameInfo> CustomGameInfos;
	}
}

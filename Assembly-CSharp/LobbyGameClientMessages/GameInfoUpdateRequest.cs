using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class GameInfoUpdateRequest : WebSocketMessage
	{
		public LobbyGameInfo GameInfo;

		public LobbyTeamInfo TeamInfo;
	}
}

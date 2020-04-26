using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class GameInfoUpdateResponse : WebSocketResponseMessage
	{
		public LobbyGameInfo GameInfo;

		public LobbyTeamInfo TeamInfo;

		public LocalizationPayload LocalizedFailure;
	}
}

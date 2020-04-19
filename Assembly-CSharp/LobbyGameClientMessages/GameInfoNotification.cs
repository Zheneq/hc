using System;
using System.Collections.Generic;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class GameInfoNotification : WebSocketMessage
	{
		public LobbyGameInfo GameInfo;

		public LobbyTeamInfo TeamInfo;

		public LobbyPlayerInfo PlayerInfo;

		public TierPlacement TierCurrent;

		public TierPlacement TierChangeMin;

		public TierPlacement TierChangeMax;

		public Dictionary<int, ForbiddenDevKnowledge> DevOnly;
	}
}

using System;
using System.Collections.Generic;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class FactionCompetitionNotification : WebSocketMessage
	{
		public int ActiveIndex;

		public Dictionary<int, long> Scores;
	}
}

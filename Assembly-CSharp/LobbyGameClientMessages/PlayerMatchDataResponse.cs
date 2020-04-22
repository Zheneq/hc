using System;
using System.Collections.Generic;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class PlayerMatchDataResponse : WebSocketResponseMessage
	{
		public List<PersistedCharacterMatchData> MatchData;
	}
}

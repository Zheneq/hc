using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class TrustBoostUsedNotification : WebSocketMessage
	{
		public int CompetitionIndex;

		public int FactionIndex;

		public int Amount;
	}
}

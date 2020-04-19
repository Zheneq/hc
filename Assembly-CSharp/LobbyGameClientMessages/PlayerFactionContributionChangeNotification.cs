using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class PlayerFactionContributionChangeNotification : WebSocketMessage
	{
		public int CompetitionId;

		public int FactionId;

		public int AmountChanged;

		public int TotalXP;

		public long AccountID;
	}
}

using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class SeasonStatusNotification : WebSocketResponseMessage
	{
		public int SeasonEndedIndex;

		public int SeasonLevelEarnedFromEnded;

		public int SeasonStartedIndex;

		public int TotalSeasonLevel;
	}
}

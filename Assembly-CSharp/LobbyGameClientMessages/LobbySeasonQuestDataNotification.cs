using System;
using System.Collections.Generic;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class LobbySeasonQuestDataNotification : WebSocketMessage
	{
		public Dictionary<int, SeasonChapterQuests> SeasonChapterQuests;
	}
}

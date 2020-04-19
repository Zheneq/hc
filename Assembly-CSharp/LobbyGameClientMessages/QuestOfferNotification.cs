using System;
using System.Collections.Generic;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class QuestOfferNotification : WebSocketMessage
	{
		public bool OfferDailyQuest;

		public List<int> DailyQuestIds;

		public Dictionary<int, int> RejectedQuestCount;
	}
}

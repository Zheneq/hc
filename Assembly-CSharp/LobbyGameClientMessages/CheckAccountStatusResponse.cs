using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class CheckAccountStatusResponse : WebSocketResponseMessage
	{
		public QuestOfferNotification QuestOffers;
	}
}

using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class PurchaseTitleRequest : WebSocketMessage
	{
		public CurrencyType CurrencyType;

		public int TitleId;
	}
}

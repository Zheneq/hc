using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class PurchaseBannerForegroundRequest : WebSocketMessage
	{
		public CurrencyType CurrencyType;

		public int BannerForegroundId;
	}
}

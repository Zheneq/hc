using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class PurchaseBannerBackgroundRequest : WebSocketMessage
	{
		public CurrencyType CurrencyType;

		public int BannerBackgroundId;
	}
}

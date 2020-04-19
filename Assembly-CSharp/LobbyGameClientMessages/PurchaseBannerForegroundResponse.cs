using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class PurchaseBannerForegroundResponse : WebSocketResponseMessage
	{
		public PurchaseResult Result;

		public CurrencyType CurrencyType;

		public int BannerForegroundId;
	}
}

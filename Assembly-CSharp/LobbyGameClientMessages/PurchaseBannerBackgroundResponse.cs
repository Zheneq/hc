using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class PurchaseBannerBackgroundResponse : WebSocketResponseMessage
	{
		public PurchaseResult Result;

		public CurrencyType CurrencyType;

		public int BannerBackgroundId;
	}
}

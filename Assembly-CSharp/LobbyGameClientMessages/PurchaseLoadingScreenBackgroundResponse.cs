using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class PurchaseLoadingScreenBackgroundResponse : WebSocketResponseMessage
	{
		public PurchaseResult Result;

		public CurrencyType CurrencyType;

		public int LoadingScreenBackgroundId;
	}
}

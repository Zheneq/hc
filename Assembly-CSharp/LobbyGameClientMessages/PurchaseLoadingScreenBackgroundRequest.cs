using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class PurchaseLoadingScreenBackgroundRequest : WebSocketMessage
	{
		public CurrencyType CurrencyType;

		public int LoadingScreenBackgroundId;
	}
}

using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class PendingPurchaseResult : WebSocketMessage
	{
		public PurchaseResult Result;

		public PendingPurchaseDetails Details;
	}
}

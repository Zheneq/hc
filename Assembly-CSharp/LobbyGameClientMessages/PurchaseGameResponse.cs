using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class PurchaseGameResponse : WebSocketResponseMessage
	{
		public PurchaseResult Result;

		public int GamePackIndex;

		public long PaymentMethodId;

		public string AccountCurrency;
	}
}

using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class PurchaseGGPackResponse : WebSocketResponseMessage
	{
		public PurchaseResult Result;

		public int GGPackIndex;

		public long PaymentMethodId;

		public string AccountCurrency;
	}
}

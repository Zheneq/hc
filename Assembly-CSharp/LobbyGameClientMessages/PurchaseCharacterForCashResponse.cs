using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class PurchaseCharacterForCashResponse : WebSocketResponseMessage
	{
		public PurchaseResult Result;

		public CharacterType CharacterType;

		public long PaymentMethodId;

		public string AccountCurrency;
	}
}

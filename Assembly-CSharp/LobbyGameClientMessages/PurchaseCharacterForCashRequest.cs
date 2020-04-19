using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class PurchaseCharacterForCashRequest : WebSocketMessage
	{
		public CharacterType CharacterType;

		public long PaymentMethodId;

		public string AccountCurrency;
	}
}

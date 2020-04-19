using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class PurchaseTintForCashRequest : WebSocketMessage
	{
		public CharacterType CharacterType;

		public int SkinId;

		public int TextureId;

		public int TintId;

		public long PaymentMethodId;

		public string AccountCurrency;
	}
}

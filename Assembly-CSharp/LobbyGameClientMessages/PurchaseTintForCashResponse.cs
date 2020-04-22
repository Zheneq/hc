using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class PurchaseTintForCashResponse : WebSocketResponseMessage
	{
		public PurchaseResult Result;

		public CharacterType CharacterType;

		public int SkinId;

		public int TextureId;

		public int TintId;

		public long PaymentMethodId;

		public string AccountCurrency;
	}
}

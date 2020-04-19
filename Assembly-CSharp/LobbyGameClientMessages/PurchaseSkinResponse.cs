using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class PurchaseSkinResponse : WebSocketResponseMessage
	{
		public PurchaseResult Result;

		public CurrencyType CurrencyType;

		public CharacterType CharacterType;

		public int SkinId;
	}
}

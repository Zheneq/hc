using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class PurchaseTauntResponse : WebSocketResponseMessage
	{
		public PurchaseResult Result;

		public CurrencyType CurrencyType;

		public CharacterType CharacterType;

		public int TauntId;
	}
}

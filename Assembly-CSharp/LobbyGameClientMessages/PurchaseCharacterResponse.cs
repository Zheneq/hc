using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class PurchaseCharacterResponse : WebSocketResponseMessage
	{
		public PurchaseResult Result;

		public CurrencyType CurrencyType;

		public CharacterType CharacterType;
	}
}

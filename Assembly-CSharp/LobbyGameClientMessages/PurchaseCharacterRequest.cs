using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class PurchaseCharacterRequest : WebSocketMessage
	{
		public CurrencyType CurrencyType;

		public CharacterType CharacterType;
	}
}

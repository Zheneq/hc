using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class PurchaseTauntRequest : WebSocketMessage
	{
		public CurrencyType CurrencyType;

		public CharacterType CharacterType;

		public int TauntId;
	}
}

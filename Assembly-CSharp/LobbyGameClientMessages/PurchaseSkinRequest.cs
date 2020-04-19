using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class PurchaseSkinRequest : WebSocketMessage
	{
		public CurrencyType CurrencyType;

		public CharacterType CharacterType;

		public int SkinId;
	}
}

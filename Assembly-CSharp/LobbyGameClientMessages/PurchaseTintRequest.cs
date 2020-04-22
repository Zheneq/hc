using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class PurchaseTintRequest : WebSocketMessage
	{
		public CurrencyType CurrencyType;

		public CharacterType CharacterType;

		public int SkinId;

		public int TextureId;

		public int TintId;
	}
}

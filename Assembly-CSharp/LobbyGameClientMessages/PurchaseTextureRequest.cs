using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class PurchaseTextureRequest : WebSocketMessage
	{
		public CurrencyType CurrencyType;

		public CharacterType CharacterType;

		public int SkinId;

		public int TextureId;
	}
}

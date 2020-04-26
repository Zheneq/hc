using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class PurchaseTextureResponse : WebSocketResponseMessage
	{
		public PurchaseResult Result;

		public CurrencyType CurrencyType;

		public CharacterType CharacterType;

		public int SkinId;

		public int TextureId;
	}
}

using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class PurchaseTintResponse : WebSocketResponseMessage
	{
		public PurchaseResult Result;

		public CurrencyType CurrencyType;

		public CharacterType CharacterType;

		public int SkinId;

		public int TextureId;

		public int TintId;
	}
}

using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class PurchaseChatEmojiResponse : WebSocketResponseMessage
	{
		public PurchaseResult Result;

		public CurrencyType CurrencyType;

		public int EmojiID;
	}
}

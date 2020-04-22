using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class PurchaseChatEmojiRequest : WebSocketMessage
	{
		public CurrencyType CurrencyType;

		public int EmojiID;
	}
}

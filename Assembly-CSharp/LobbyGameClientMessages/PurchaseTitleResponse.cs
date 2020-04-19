using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class PurchaseTitleResponse : WebSocketResponseMessage
	{
		public PurchaseResult Result;

		public CurrencyType CurrencyType;

		public int TitleId;
	}
}

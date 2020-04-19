using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class PurchaseOverconResponse : WebSocketResponseMessage
	{
		public PurchaseResult Result;

		public CurrencyType CurrencyType;

		public int OverconId;
	}
}

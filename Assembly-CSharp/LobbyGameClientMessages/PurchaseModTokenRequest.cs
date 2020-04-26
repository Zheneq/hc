using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class PurchaseModTokenRequest : WebSocketMessage
	{
		public int NumToPurchase;
	}
}

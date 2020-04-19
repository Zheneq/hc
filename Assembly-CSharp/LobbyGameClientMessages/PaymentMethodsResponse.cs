using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class PaymentMethodsResponse : WebSocketResponseMessage
	{
		public PaymentMethodList PaymentMethodList;
	}
}

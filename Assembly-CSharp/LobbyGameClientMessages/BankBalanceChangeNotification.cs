using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class BankBalanceChangeNotification : WebSocketResponseMessage
	{
		public CurrencyData NewBalance;
	}
}

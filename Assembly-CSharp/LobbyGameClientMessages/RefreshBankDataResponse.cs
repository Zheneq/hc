using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class RefreshBankDataResponse : WebSocketResponseMessage
	{
		public CurrencyWallet Wallet;
	}
}

using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class PlayerAccountDataUpdateNotification : WebSocketResponseMessage
	{
		public PersistedAccountData AccountData;
	}
}

using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class SteamMtxConfirm : WebSocketMessage
	{
		public bool authorized;

		public ulong orderId;
	}
}

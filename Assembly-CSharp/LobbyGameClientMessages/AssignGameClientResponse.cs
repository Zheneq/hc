using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class AssignGameClientResponse : WebSocketResponseMessage
	{
		public LobbySessionInfo SessionInfo;

		public LobbyGameClientProxyInfo ProxyInfo;

		public string LobbyServerAddress;
	}
}

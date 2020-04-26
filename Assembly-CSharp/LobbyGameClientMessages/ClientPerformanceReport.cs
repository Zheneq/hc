using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class ClientPerformanceReport : WebSocketMessage
	{
		public LobbyGameClientPerformanceInfo PerformanceInfo;
	}
}

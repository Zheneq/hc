using System;
using System.Collections.Generic;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class ClientErrorSummary : WebSocketMessage
	{
		public Dictionary<uint, uint> ReportCount;
	}
}

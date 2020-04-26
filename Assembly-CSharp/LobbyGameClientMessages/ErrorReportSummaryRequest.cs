using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class ErrorReportSummaryRequest : WebSocketMessage
	{
		public uint CrashReportHash;
	}
}

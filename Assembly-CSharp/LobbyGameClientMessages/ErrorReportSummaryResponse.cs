using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class ErrorReportSummaryResponse : WebSocketResponseMessage
	{
		public ClientErrorReport ClientErrorReport;
	}
}

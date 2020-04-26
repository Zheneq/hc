using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class CrashReportArchiveNameRequest : WebSocketMessage
	{
		public int NumArchiveBytes;

		public bool DevBuild;
	}
}

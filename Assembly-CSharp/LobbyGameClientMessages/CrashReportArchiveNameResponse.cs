using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class CrashReportArchiveNameResponse : WebSocketResponseMessage
	{
		public string ArchiveName;
	}
}

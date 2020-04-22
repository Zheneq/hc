using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class ClientErrorReport : WebSocketMessage
	{
		public string LogString;

		public string StackTrace;

		public uint StackTraceHash;

		public float Time;

		public int _001D()
		{
			return LogString.Length + StackTrace.Length + 4 + 4;
		}
	}
}

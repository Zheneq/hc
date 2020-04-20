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

		public int symbol_001D()
		{
			return this.LogString.Length + this.StackTrace.Length + 4 + 4;
		}
	}
}

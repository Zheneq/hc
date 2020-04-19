using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class DEBUG_SetEloRequest : WebSocketMessage
	{
		public DEBUG_SetEloRequest.CommandType Command;

		public GameType GameType;

		public enum CommandType
		{
			\u001D,
			\u000E,
			\u0012,
			\u0015,
			\u0016,
			\u0013
		}
	}
}

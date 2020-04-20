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
			symbol_001D,
			symbol_000E,
			symbol_0012,
			symbol_0015,
			symbol_0016,
			symbol_0013
		}
	}
}

using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class DEBUG_SetEloRequest : WebSocketMessage
	{
		public enum CommandType
		{
			_001D,
			_000E,
			_0012,
			_0015,
			_0016,
			_0013
		}

		public CommandType Command;

		public GameType GameType;
	}
}

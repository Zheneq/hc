using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class DEBUG_SetTierRequest : WebSocketMessage
	{
		public enum CommandType
		{
			_001D,
			_000E
		}

		public CommandType Command;

		public int TierValue;

		public int SetPoints;
	}
}

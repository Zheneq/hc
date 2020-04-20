using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class DEBUG_SetTierRequest : WebSocketMessage
	{
		public DEBUG_SetTierRequest.CommandType Command;

		public int TierValue;

		public int SetPoints;

		public enum CommandType
		{
			symbol_001D,
			symbol_000E
		}
	}
}

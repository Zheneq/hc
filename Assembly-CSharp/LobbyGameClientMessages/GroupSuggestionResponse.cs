using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class GroupSuggestionResponse : WebSocketResponseMessage
	{
		public GroupSuggestionResponse.Status SuggestionStatus;

		public long SuggesterAccountId;

		public enum Status
		{
			symbol_001D,
			symbol_000E,
			symbol_0012
		}
	}
}

using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class GroupSuggestionResponse : WebSocketResponseMessage
	{
		public enum Status
		{
			_001D,
			_000E,
			_0012
		}

		public Status SuggestionStatus;

		public long SuggesterAccountId;
	}
}

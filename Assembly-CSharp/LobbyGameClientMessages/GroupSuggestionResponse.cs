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
			\u001D,
			\u000E,
			\u0012
		}
	}
}

using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class GroupSuggestionRequest : WebSocketMessage
	{
		public long LeaderAccountId;

		public string SuggestedAccountFullHandle;

		public string SuggesterAccountName;

		public long SuggesterAccountId;
	}
}

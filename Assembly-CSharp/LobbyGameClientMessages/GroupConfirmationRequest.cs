using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class GroupConfirmationRequest : WebSocketMessage
	{
		public long GroupId;

		public string LeaderName;

		public string LeaderFullHandle;

		public string JoinerName;

		public long JoinerAccountId;

		public long ConfirmationNumber;

		public TimeSpan ExpirationTime;

		public GroupConfirmationRequest.JoinType Type;

		public enum JoinType
		{
			\u001D,
			\u000E
		}
	}
}

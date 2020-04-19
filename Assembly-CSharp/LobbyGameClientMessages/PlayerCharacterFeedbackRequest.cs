using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class PlayerCharacterFeedbackRequest : WebSocketMessage
	{
		public PlayerFeedbackData FeedbackData;
	}
}

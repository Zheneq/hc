using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class MarkTutorialSkippedRequest : WebSocketMessage
	{
		public TutorialVersion Progress;
	}
}

using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class UpdateMatchmakingQueueRequest : WebSocketMessage
	{
		public BotDifficulty EnemyDifficulty;
	}
}

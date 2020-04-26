using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class JoinMatchmakingQueueRequest : WebSocketMessage
	{
		public GameType GameType;

		public BotDifficulty AllyBotDifficulty;

		public BotDifficulty EnemyBotDifficulty;
	}
}

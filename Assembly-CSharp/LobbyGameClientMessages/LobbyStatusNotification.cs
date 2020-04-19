using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class LobbyStatusNotification : WebSocketMessage
	{
		public LocalizationPayload LocalizedFailure;

		public bool AllowRelogin;

		public ServerLockState ServerLockState;

		public ConnectionQueueInfo ConnectionQueueInfo;

		public ClientAccessLevel ClientAccessLevel;

		public bool HasPurchasedGame;

		public int HighestPurchasedGamePack;

		public ServerMessageOverrides ServerMessageOverrides;

		public LobbyGameplayOverrides GameplayOverrides;

		public DateTime UtcNow;

		public DateTime PacificNow;

		public TimeSpan TimeOffset;

		public TimeSpan? ErrorReportRate;
	}
}

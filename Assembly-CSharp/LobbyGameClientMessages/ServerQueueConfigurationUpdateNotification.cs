using System;
using System.Collections.Generic;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class ServerQueueConfigurationUpdateNotification : WebSocketMessage
	{
		public Dictionary<GameType, GameTypeAvailability> GameTypeAvailabilies;

		public Dictionary<CharacterType, RequirementCollection> FreeRotationAdditions;

		public List<LocalizationPayload> TierInstanceNames;

		public bool AllowBadges;

		public int NewPlayerPvPQueueDuration;
	}
}

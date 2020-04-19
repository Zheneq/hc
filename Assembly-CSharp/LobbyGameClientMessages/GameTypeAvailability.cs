using System;
using System.Collections.Generic;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class GameTypeAvailability
	{
		public Dictionary<int, RequirementCollection> QueueableGroupSizes;

		public bool IsActive;

		public int MinMatchesToAppearOnLeaderboard;

		public int MaxWillFillPerTeam;

		public int TeamAPlayers;

		public int TeamBPlayers;

		public int TeamABots;

		public int TeamBBots;

		public GameType BlockedExperienceAlternativeGameType;

		public List<MatchmakingQueueConfig.QueueEntryExperience> BlockedExperienceEntries;

		public GameLeavingPenalty GameLeavingPenalty;

		public List<TierDefinitions> PerTierDefinitions;

		public DateTime? PenaltyTimeout;

		public DateTime? ParoleTimeout;

		public RequirementCollection Requirements;

		public List<GameSubType> SubTypes;

		public Dictionary<ushort, DateTime> XPPenaltyTimeout;
	}
}

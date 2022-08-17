using System;
using System.Collections.Generic;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class MatchResultsNotification : WebSocketMessage
	{
		[Serializable]
		public class CurrencyReward
		{
			public CurrencyType Type;
			public int BaseGained;
			public int EventGained;
			public int WinGained;
			public int QuestGained;
			public int GGGained;
			public int LevelUpGained;
		}

		public List<CurrencyReward> CurrencyRewards;
		public int BaseXpGained;
		public int WinXpGained;
		public int GGXpGained;
		public int ConsumableXpGained;
		public int PlayWithFriendXpGained;
		public int QuestXpGained;
		public int EventBonusXpGained;
		public int FirstWinXpGained;
		public int QueueTimeXpGained;
		public int FreelancerOwnedXPGained;
		public int AccountLevelAtStart = 1;
		public int CharacterLevelAtStart = 1;
		public int AccountXpAtStart;
		public int CharacterXpAtStart;
		public int SeasonLevelAtStart;
		public int SeasonXpAtStart;
		public int FactionCompetitionId;
		public int FactionId;
		public Dictionary<string, int> FactionContributionAmounts;
		public List<BadgeAndParticipantInfo> BadgeAndParticipantsInfo;
		public bool FirstWinOccured;
		public float TotalBadgePoints;
		public int NumCharactersPlayed;
	}
}

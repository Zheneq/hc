using System;
using System.Collections.Generic;

namespace LobbyGameClientMessages
{
	[Serializable]
	public struct RankedResolutionPhaseData
	{
		public List<RankedResolutionPlayerState> UnselectedPlayerStates;

		public List<RankedResolutionPlayerState> PlayersOnDeck;

		public TimeSpan TimeLeftInSubPhase;

		public List<CharacterType> FriendlyBans;

		public List<CharacterType> EnemyBans;

		public Dictionary<int, CharacterType> FriendlyTeamSelections;

		public Dictionary<int, CharacterType> EnemyTeamSelections;

		public List<RankedTradeData> TradeActions;

		public List<int> PlayerIdByImporance;

		public bool \u001D(int \u001D)
		{
			RankedResolutionPhaseData.<IsPlayerOnDeck>c__AnonStorey0 <IsPlayerOnDeck>c__AnonStorey = new RankedResolutionPhaseData.<IsPlayerOnDeck>c__AnonStorey0();
			<IsPlayerOnDeck>c__AnonStorey.\u001D = \u001D;
			return this.PlayersOnDeck.Exists(new Predicate<RankedResolutionPlayerState>(<IsPlayerOnDeck>c__AnonStorey.\u000E));
		}
	}
}

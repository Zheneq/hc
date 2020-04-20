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

		public bool symbol_001D(int symbol_001D)
		{
			RankedResolutionPhaseData._IsPlayerOnDeckc__AnonStorey0 _IsPlayerOnDeckc__AnonStorey = new RankedResolutionPhaseData._IsPlayerOnDeckc__AnonStorey0();
			_IsPlayerOnDeckc__AnonStorey.symbol_001D = symbol_001D;
			return this.PlayersOnDeck.Exists(new Predicate<RankedResolutionPlayerState>(_IsPlayerOnDeckc__AnonStorey.symbol_000E));
		}
	}
}

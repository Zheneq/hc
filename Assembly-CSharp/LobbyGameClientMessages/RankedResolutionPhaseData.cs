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

        public bool IsPlayerSelecting(int playerId)
        {
            return PlayersOnDeck.Exists(check =>
                check.PlayerId == playerId && check.OnDeckness == RankedResolutionPlayerState.ReadyState.Selected);
        }
    }
}
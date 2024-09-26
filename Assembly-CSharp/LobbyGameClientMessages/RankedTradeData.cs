using System;

namespace LobbyGameClientMessages
{
    [Serializable]
    public struct RankedTradeData
    {
        public enum TradeActionType
        {
            AcceptOrOffer,
            Reject,
            StopTrading
        }

        public TradeActionType TradeAction;
        public CharacterType DesiredCharacter;
        public int AskedPlayerId;
        public CharacterType OfferedCharacter;
        public int OfferingPlayerId;
    }
}
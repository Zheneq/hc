using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public struct RankedTradeData
	{
		public enum TradeActionType
		{
			_001D,
			_000E,
			_0012
		}

		public TradeActionType TradeAction;

		public CharacterType DesiredCharacter;

		public int AskedPlayerId;

		public CharacterType OfferedCharacter;

		public int OfferingPlayerId;
	}
}

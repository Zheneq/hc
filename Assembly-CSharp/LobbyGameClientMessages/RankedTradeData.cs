using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public struct RankedTradeData
	{
		public RankedTradeData.TradeActionType TradeAction;

		public CharacterType DesiredCharacter;

		public int AskedPlayerId;

		public CharacterType OfferedCharacter;

		public int OfferingPlayerId;

		public enum TradeActionType
		{
			\u001D,
			\u000E,
			\u0012
		}
	}
}

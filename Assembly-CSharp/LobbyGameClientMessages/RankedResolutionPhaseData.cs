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

		public bool _001D(int _001D)
		{
			return PlayersOnDeck.Exists(delegate(RankedResolutionPlayerState _001D)
			{
				int result;
				if (_001D.PlayerId == _001D)
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					result = ((_001D.OnDeckness == RankedResolutionPlayerState.ReadyState._0012) ? 1 : 0);
				}
				else
				{
					result = 0;
				}
				return (byte)result != 0;
			});
		}
	}
}

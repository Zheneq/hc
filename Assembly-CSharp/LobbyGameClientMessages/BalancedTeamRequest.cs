using System;
using System.Collections.Generic;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class BalancedTeamRequest : WebSocketMessage
	{
		public List<BalanceTeamSlot> Slots;
	}
}

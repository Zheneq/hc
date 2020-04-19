using System;
using System.Collections.Generic;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class BalancedTeamResponse : WebSocketResponseMessage
	{
		public List<BalanceTeamSlot> Slots;

		public LocalizationPayload LocalizedFailure;
	}
}

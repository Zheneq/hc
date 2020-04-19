using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class DEBUG_ClearPenaltyRequest : WebSocketMessage
	{
		public bool ClearDodgePenalty;

		public bool ClearLeavePenalty;
	}
}

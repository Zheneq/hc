using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class LeaveGameRequest : WebSocketMessage
	{
		public bool IsPermanent;

		public GameResult GameResult;
	}
}

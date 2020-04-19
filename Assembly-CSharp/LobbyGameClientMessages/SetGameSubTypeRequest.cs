using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class SetGameSubTypeRequest : WebSocketMessage
	{
		public GameType gameType;

		public ushort SubTypeMask;
	}
}

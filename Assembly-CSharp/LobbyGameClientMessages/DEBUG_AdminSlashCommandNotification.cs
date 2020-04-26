using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class DEBUG_AdminSlashCommandNotification : WebSocketMessage
	{
		public string Command;

		public string Arguments;
	}
}

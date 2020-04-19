using System;
using System.Collections.Generic;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class CustomKeyBindNotification : WebSocketMessage
	{
		public Dictionary<int, KeyCodeData> CustomKeyBinds;
	}
}

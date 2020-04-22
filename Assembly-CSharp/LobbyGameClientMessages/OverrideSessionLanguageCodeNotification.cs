using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class OverrideSessionLanguageCodeNotification : WebSocketMessage
	{
		public string LanguageCode;
	}
}

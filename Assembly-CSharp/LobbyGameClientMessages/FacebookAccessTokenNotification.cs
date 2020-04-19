using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class FacebookAccessTokenNotification : WebSocketMessage
	{
		public string AccessToken;
	}
}

using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class FacebookGetUserTokenResponse : WebSocketResponseMessage
	{
		public OAuthInfo OAuthInfo;
	}
}

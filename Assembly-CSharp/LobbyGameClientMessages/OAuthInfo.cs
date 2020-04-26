using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class OAuthInfo
	{
		public string ClientId;

		public string RedirectUri;

		public string Scope;

		public string UserToken;
	}
}

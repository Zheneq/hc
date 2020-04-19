using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class SelectBannerRequest : WebSocketMessage
	{
		public int BannerID;
	}
}

using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class SelectBannerResponse : WebSocketResponseMessage
	{
		public int ForegroundBannerID;

		public int BackgroundBannerID;
	}
}

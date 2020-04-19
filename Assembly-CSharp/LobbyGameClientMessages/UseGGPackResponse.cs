using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class UseGGPackResponse : WebSocketResponseMessage
	{
		public string GGPackUserName;

		public int GGPackUserTitle;

		public int GGPackUserTitleLevel;

		public int GGPackUserBannerForeground;

		public int GGPackUserBannerBackground;

		public int GGPackUserRibbon;

		public LocalizationPayload LocalizedFailure;
	}
}

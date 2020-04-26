using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class UseGGPackNotification : WebSocketMessage
	{
		public string GGPackUserName;

		public int NumGGPacksUsed;

		public int GGPackUserTitle;

		public int GGPackUserTitleLevel;

		public int GGPackUserBannerForeground;

		public int GGPackUserBannerBackground;

		public int GGPackUserRibbon;
	}
}

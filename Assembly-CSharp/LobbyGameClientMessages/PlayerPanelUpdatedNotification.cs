using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class PlayerPanelUpdatedNotification : WebSocketMessage
	{
		public int originalSelectedTitleID { get; set; }

		public int originalSelectedForegroundBannerID { get; set; }

		public int originalSelectedBackgroundBannerID { get; set; }

		public int originalSelectedRibbonID { get; set; }
	}
}

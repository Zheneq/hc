using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class SelectTitleRequest : WebSocketMessage
	{
		public int TitleID;
	}
}

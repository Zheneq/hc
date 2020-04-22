using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class SelectTitleResponse : WebSocketResponseMessage
	{
		public int CurrentTitleID;
	}
}

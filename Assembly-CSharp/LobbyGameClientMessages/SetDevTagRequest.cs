using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class SetDevTagRequest : WebSocketMessage
	{
		public bool active;
	}
}

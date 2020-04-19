using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class UseOverconResponse : WebSocketResponseMessage
	{
		public int OverconId;

		public int ActorId;
	}
}

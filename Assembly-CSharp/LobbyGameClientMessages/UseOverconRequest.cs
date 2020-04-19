using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class UseOverconRequest : WebSocketMessage
	{
		public int OverconId;

		public string OverconName;

		public int ActorId;

		public int Turn;
	}
}

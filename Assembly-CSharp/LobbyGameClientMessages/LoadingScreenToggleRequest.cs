using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class LoadingScreenToggleRequest : WebSocketMessage
	{
		public int LoadingScreenId;

		public bool NewState;
	}
}

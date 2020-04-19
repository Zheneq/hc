using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class LoadingScreenToggleResponse : WebSocketResponseMessage
	{
		public int LoadingScreenId;

		public bool CurrentState;
	}
}

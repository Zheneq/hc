using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class UpdateUIStateRequest : WebSocketMessage
	{
		public AccountComponent.UIStateIdentifier UIState;

		public int StateValue;
	}
}

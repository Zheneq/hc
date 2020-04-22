using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class LobbyGameplayOverridesNotification : WebSocketMessage
	{
		public LobbyGameplayOverrides GameplayOverrides;
	}
}

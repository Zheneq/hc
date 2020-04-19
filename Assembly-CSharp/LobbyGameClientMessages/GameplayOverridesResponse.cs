using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class GameplayOverridesResponse : WebSocketResponseMessage
	{
		public LobbyGameplayOverrides GameplayOverrides;
	}
}

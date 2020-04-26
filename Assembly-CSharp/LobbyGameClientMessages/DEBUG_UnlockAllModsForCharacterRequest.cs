using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class DEBUG_UnlockAllModsForCharacterRequest : WebSocketMessage
	{
		public CharacterType Character;
	}
}

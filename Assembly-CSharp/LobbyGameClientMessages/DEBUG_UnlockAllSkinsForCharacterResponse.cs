using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class DEBUG_UnlockAllSkinsForCharacterResponse : WebSocketResponseMessage
	{
		public CharacterType Character;
	}
}

using System;
using System.Collections.Generic;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class DEBUG_UnlockAllModsForCharacterResponse : WebSocketResponseMessage
	{
		public CharacterType Character;

		public List<PlayerModData> UnlockData;
	}
}

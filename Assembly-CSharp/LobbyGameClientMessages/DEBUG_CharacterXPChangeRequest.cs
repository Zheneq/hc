using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class DEBUG_CharacterXPChangeRequest : WebSocketMessage
	{
		public CharacterType character;

		public int amount;
	}
}

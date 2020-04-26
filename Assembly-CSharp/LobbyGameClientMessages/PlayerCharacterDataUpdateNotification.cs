using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class PlayerCharacterDataUpdateNotification : WebSocketResponseMessage
	{
		public PersistedCharacterData CharacterData;
	}
}

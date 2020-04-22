using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class UpdateRemoteCharacterRequest : WebSocketMessage
	{
		public CharacterType[] Characters;

		public int[] RemoteSlotIndexes;
	}
}

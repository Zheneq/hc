using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class PlayerInfoUpdateResponse : WebSocketResponseMessage
	{
		public LobbyPlayerInfo PlayerInfo;

		public LobbyCharacterInfo CharacterInfo;

		public LobbyPlayerInfoUpdate OriginalPlayerInfoUpdate;

		public LocalizationPayload LocalizedFailure;
	}
}

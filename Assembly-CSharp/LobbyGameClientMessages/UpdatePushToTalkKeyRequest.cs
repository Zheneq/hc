using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class UpdatePushToTalkKeyRequest : WebSocketMessage
	{
		public int KeyType;

		public int KeyCode;

		public string KeyName;
	}
}

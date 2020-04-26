using System;
using System.Collections.Generic;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class ChatNotification : WebSocketMessage
	{
		public long SenderAccountId;

		public string SenderHandle;

		public Team SenderTeam;

		public string RecipientHandle;

		public CharacterType CharacterType;

		public ConsoleMessageType ConsoleMessageType;

		public string Text;

		public LocalizationPayload LocalizedText;

		public List<int> EmojisAllowed;

		public bool DisplayDevTag;
	}
}

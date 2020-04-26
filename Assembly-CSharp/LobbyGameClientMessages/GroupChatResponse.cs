using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class GroupChatResponse : WebSocketResponseMessage
	{
		public string Text;

		public LocalizationPayload LocalizedFailure;
	}
}

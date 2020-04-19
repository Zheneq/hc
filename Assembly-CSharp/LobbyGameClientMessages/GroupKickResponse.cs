using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class GroupKickResponse : WebSocketResponseMessage
	{
		public string MemberName;

		public LocalizationPayload LocalizedFailure;
	}
}

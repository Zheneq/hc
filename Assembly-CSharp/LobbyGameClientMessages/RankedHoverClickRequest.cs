using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class RankedHoverClickRequest : WebSocketMessage
	{
		public CharacterType Selection;
	}
}

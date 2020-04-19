using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class RankedBanRequest : WebSocketMessage
	{
		public CharacterType Selection;
	}
}

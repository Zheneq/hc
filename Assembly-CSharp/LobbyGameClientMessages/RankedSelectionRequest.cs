using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class RankedSelectionRequest : WebSocketMessage
	{
		public CharacterType Selection;
	}
}

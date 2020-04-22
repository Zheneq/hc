using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class PurchaseLoadoutSlotRequest : WebSocketMessage
	{
		public CharacterType Character;
	}
}

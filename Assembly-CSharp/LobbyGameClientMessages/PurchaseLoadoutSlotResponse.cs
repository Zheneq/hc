using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class PurchaseLoadoutSlotResponse : WebSocketResponseMessage
	{
		public CharacterType Character;
	}
}

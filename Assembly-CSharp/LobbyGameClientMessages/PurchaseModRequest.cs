using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class PurchaseModRequest : WebSocketMessage
	{
		public CharacterType Character;

		public PlayerModData UnlockData;
	}
}

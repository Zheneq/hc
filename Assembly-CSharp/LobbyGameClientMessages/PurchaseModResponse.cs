using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class PurchaseModResponse : WebSocketResponseMessage
	{
		public CharacterType Character;

		public PlayerModData UnlockData;
	}
}

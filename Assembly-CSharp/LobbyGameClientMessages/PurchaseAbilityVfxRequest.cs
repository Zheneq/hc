using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class PurchaseAbilityVfxRequest : WebSocketMessage
	{
		public CurrencyType CurrencyType;

		public CharacterType CharacterType;

		public int AbilityId;

		public int VfxId;
	}
}

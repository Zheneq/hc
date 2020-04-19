using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class PurchaseAbilityVfxResponse : WebSocketResponseMessage
	{
		public PurchaseResult Result;

		public CurrencyType CurrencyType;

		public CharacterType CharacterType;

		public int AbilityId;

		public int VfxId;
	}
}

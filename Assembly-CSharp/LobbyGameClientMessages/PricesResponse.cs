using System;
using System.Collections.Generic;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class PricesResponse : WebSocketResponseMessage
	{
		public List<GamePackPriceOverride> gamePackPrices;

		public List<LootMatrixPackPriceOverride> lootMatrixPackPrices;

		public List<CharacterPriceOverride> characterPrices;

		public List<GGPackPriceOverride> ggPackPrices;

		public List<StylePriceOverride> stylePrices;

		public List<StoreItemPriceOverride> storeItemPrices;

		public DateTime PacificTimeWithServerTimeOffset;
	}
}

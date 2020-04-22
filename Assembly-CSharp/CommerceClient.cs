using LobbyGameClientMessages;
using Steamworks;
using System;
using System.Collections.Generic;

public class CommerceClient
{
	private static CommerceClient s_instance;

	public DateTime LastPacificTimePriceRequestWithServerTimeOffset;

	public List<LootMatrixPackPriceOverride> m_overriddenLootMatrixPacks;

	public List<GamePackPriceOverride> m_overriddenGamePacks;

	public List<CharacterPriceOverride> m_overriddenCharacters;

	public List<GGPackPriceOverride> m_overriddenGGPacks;

	public List<StylePriceOverride> m_overriddenStyles;

	public List<StoreItemPriceOverride> m_overriddenStoreItems;

	protected Callback<MicroTxnAuthorizationResponse_t> m_steamMtxResponse;

	public CommerceClient()
	{
		s_instance = this;
		m_overriddenLootMatrixPacks = new List<LootMatrixPackPriceOverride>();
		m_overriddenGamePacks = new List<GamePackPriceOverride>();
		m_overriddenGGPacks = new List<GGPackPriceOverride>();
		m_overriddenStyles = new List<StylePriceOverride>();
		m_overriddenStoreItems = new List<StoreItemPriceOverride>();
		ClientGameManager.Get().OnLobbyStatusNotification += HandleLobbyStatusNotification;
		HandleLobbyStatusNotification(null);
		if (!SteamManager.Initialized)
		{
			return;
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_steamMtxResponse = Callback<MicroTxnAuthorizationResponse_t>.Create(OnSteamMtxResponse);
			return;
		}
	}

	~CommerceClient()
	{
		if (ClientGameManager.Get() != null)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			ClientGameManager.Get().OnLobbyStatusNotification -= HandleLobbyStatusNotification;
		}
		s_instance = null;
	}

	private void OnSteamMtxResponse(MicroTxnAuthorizationResponse_t pCallback)
	{
		ClientGameManager.Get().LobbyInterface.SendSteamMtxConfirm(pCallback.m_bAuthorized != 0, pCallback.m_ulOrderID);
		UIStorePanel.Get().NotifySteamResponseReceived();
	}

	private void HandleLobbyStatusNotification(LobbyStatusNotification notification)
	{
		if (ClientGameManager.Get().ClientAccessLevel > ClientAccessLevel.Locked)
		{
			RequestPrices();
		}
	}

	public static CommerceClient Get()
	{
		if (s_instance == null)
		{
			s_instance = new CommerceClient();
		}
		return s_instance;
	}

	public void RequestPrices()
	{
		if (ClientGameManager.Get().LobbyInterface == null)
		{
			return;
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (ClientGameManager.Get().ClientAccessLevel > ClientAccessLevel.Locked)
			{
				ClientGameManager.Get().LobbyInterface.RequestPrices(HandlePrices);
			}
			return;
		}
	}

	public void HandlePrices(PricesResponse response)
	{
		int num;
		if (response.lootMatrixPackPrices == null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			num = 0;
		}
		else
		{
			num = response.lootMatrixPackPrices.Count;
		}
		int num2 = num;
		int num3;
		if (response.gamePackPrices == null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			num3 = 0;
		}
		else
		{
			num3 = response.gamePackPrices.Count;
		}
		int num4 = num3;
		int num5;
		if (response.characterPrices == null)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			num5 = 0;
		}
		else
		{
			num5 = response.characterPrices.Count;
		}
		int num6 = num5;
		int num7 = (response.ggPackPrices != null) ? response.ggPackPrices.Count : 0;
		int num8;
		if (response.stylePrices == null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			num8 = 0;
		}
		else
		{
			num8 = response.stylePrices.Count;
		}
		int num9 = num8;
		int num10;
		if (response.storeItemPrices == null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			num10 = 0;
		}
		else
		{
			num10 = response.storeItemPrices.Count;
		}
		int num11 = num10;
		Log.Info("Got {0} game pack price(s), {1} loot matrix pack price(s), {2} Character price(s), {3} gg price(s), {4} style price(s), and {5} store item price(s) from server.", num4, num2, num6, num7, num9, num11);
		if (response.gamePackPrices != null)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			m_overriddenGamePacks = response.gamePackPrices;
		}
		if (response.lootMatrixPackPrices != null)
		{
			m_overriddenLootMatrixPacks = response.lootMatrixPackPrices;
		}
		if (response.characterPrices != null)
		{
			m_overriddenCharacters = response.characterPrices;
		}
		if (response.ggPackPrices != null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			m_overriddenGGPacks = response.ggPackPrices;
		}
		if (response.stylePrices != null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			m_overriddenStyles = response.stylePrices;
		}
		if (response.storeItemPrices != null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			m_overriddenStoreItems = response.storeItemPrices;
		}
		LastPacificTimePriceRequestWithServerTimeOffset = response.PacificTimeWithServerTimeOffset;
	}

	public int GetISOPriceForInventoryItem(int inventoryItemID)
	{
		InventoryItemTemplate itemTemplate = InventoryWideData.Get().GetItemTemplate(inventoryItemID);
		return itemTemplate.ISOPrice;
	}

	public float GetLootMatrixPackPrice(string productCode, string currencyCode)
	{
		foreach (LootMatrixPackPriceOverride overriddenLootMatrixPack in m_overriddenLootMatrixPacks)
		{
			if (overriddenLootMatrixPack.productCode == productCode && overriddenLootMatrixPack.prices != null)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						return overriddenLootMatrixPack.prices.GetPrice(currencyCode);
					}
				}
			}
		}
		LootMatrixPack[] lootMatrixPacks = LootMatrixPackData.Get().m_lootMatrixPacks;
		foreach (LootMatrixPack lootMatrixPack in lootMatrixPacks)
		{
			if (!(lootMatrixPack.ProductCode == productCode))
			{
				continue;
			}
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				return lootMatrixPack.Prices.GetPrice(currencyCode);
			}
		}
		return 0f;
	}

	public float GetGamePackPrice(string productCode, string currencyCode, out float originalPrice)
	{
		originalPrice = 0f;
		GamePack[] gamePacks = GamePackData.Get().m_gamePacks;
		foreach (GamePack gamePack in gamePacks)
		{
			if (gamePack.ProductCode == productCode)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				originalPrice = gamePack.Prices.GetPrice(currencyCode);
				continue;
			}
			GamePackUpgrade[] upgrades = gamePack.Upgrades;
			foreach (GamePackUpgrade gamePackUpgrade in upgrades)
			{
				if (gamePackUpgrade.ProductCode == productCode)
				{
					originalPrice = gamePackUpgrade.Prices.GetPrice(currencyCode);
				}
			}
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			foreach (GamePackPriceOverride overriddenGamePack in m_overriddenGamePacks)
			{
				if (overriddenGamePack.productCode == productCode && overriddenGamePack.prices != null)
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							break;
						default:
							return overriddenGamePack.prices.GetPrice(currencyCode);
						}
					}
				}
			}
			return originalPrice;
		}
	}

	public float GetFreelancerPrice(CharacterType type, string currencyCode)
	{
		if (!m_overriddenCharacters.IsNullOrEmpty())
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			foreach (CharacterPriceOverride overriddenCharacter in m_overriddenCharacters)
			{
				if (overriddenCharacter.characterType == type)
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					if (overriddenCharacter.prices != null)
					{
						return overriddenCharacter.prices.GetPrice(currencyCode);
					}
				}
			}
		}
		CharacterResourceLink[] characterResourceLinks = GameWideData.Get().m_characterResourceLinks;
		foreach (CharacterResourceLink characterResourceLink in characterResourceLinks)
		{
			if (characterResourceLink.m_characterType != type)
			{
				continue;
			}
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				return characterResourceLink.Prices.GetPrice(currencyCode);
			}
		}
		return 0f;
	}

	public float GetGGPackPrice(string productCode, string currencyCode)
	{
		using (List<GGPackPriceOverride>.Enumerator enumerator = m_overriddenGGPacks.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				GGPackPriceOverride current = enumerator.Current;
				if (current.productCode == productCode)
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					if (current.prices != null)
					{
						while (true)
						{
							switch (3)
							{
							case 0:
								break;
							default:
								return current.prices.GetPrice(currencyCode);
							}
						}
					}
				}
			}
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		GGPack[] ggPacks = GameWideData.Get().m_ggPackData.m_ggPacks;
		foreach (GGPack gGPack in ggPacks)
		{
			if (gGPack.ProductCode == productCode)
			{
				return gGPack.Prices.GetPrice(currencyCode);
			}
		}
		return 0f;
	}

	public float GetStylePrice(CharacterType charType, int skinIndex, int textureIndex, int colorIndex, string currencyCode)
	{
		using (List<StylePriceOverride>.Enumerator enumerator = m_overriddenStyles.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				StylePriceOverride current = enumerator.Current;
				if (current.prices != null)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					if (current.characterType == charType && current.skinIndex == skinIndex && current.textureIndex == textureIndex)
					{
						while (true)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
						if (current.colorIndex == colorIndex)
						{
							while (true)
							{
								switch (5)
								{
								case 0:
									break;
								default:
									return current.prices.GetPrice(currencyCode);
								}
							}
						}
					}
				}
			}
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		CharacterResourceLink characterResourceLink = GameWideData.Get().GetCharacterResourceLink(charType);
		GameBalanceVars.ColorUnlockData colorUnlockData = characterResourceLink.m_skins[skinIndex].m_patterns[textureIndex].m_colors[colorIndex].m_colorUnlockData;
		return colorUnlockData.Prices.GetPrice(currencyCode);
	}

	public float GetStoreItemPrice(int templateId, string currencyCode, out float originalPrice)
	{
		originalPrice = 0f;
		GameBalanceVars.StoreItemForPurchase[] storeItemsForPurchase = GameBalanceVars.Get().StoreItemsForPurchase;
		int num = 0;
		while (true)
		{
			if (num < storeItemsForPurchase.Length)
			{
				GameBalanceVars.StoreItemForPurchase storeItemForPurchase = storeItemsForPurchase[num];
				if (storeItemForPurchase.m_itemTemplateId == templateId)
				{
					originalPrice = storeItemForPurchase.Prices.GetPrice(currencyCode);
					break;
				}
				num++;
				continue;
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			break;
		}
		using (List<StoreItemPriceOverride>.Enumerator enumerator = m_overriddenStoreItems.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				StoreItemPriceOverride current = enumerator.Current;
				if (current.inventoryTemplateId == templateId)
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					if (current.prices != null)
					{
						while (true)
						{
							switch (3)
							{
							case 0:
								break;
							default:
								return current.prices.GetPrice(currencyCode);
							}
						}
					}
				}
			}
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		return originalPrice;
	}
}

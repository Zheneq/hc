using System;
using System.Collections.Generic;
using LobbyGameClientMessages;
using Steamworks;

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
		CommerceClient.s_instance = this;
		this.m_overriddenLootMatrixPacks = new List<LootMatrixPackPriceOverride>();
		this.m_overriddenGamePacks = new List<GamePackPriceOverride>();
		this.m_overriddenGGPacks = new List<GGPackPriceOverride>();
		this.m_overriddenStyles = new List<StylePriceOverride>();
		this.m_overriddenStoreItems = new List<StoreItemPriceOverride>();
		ClientGameManager.Get().OnLobbyStatusNotification += this.HandleLobbyStatusNotification;
		this.HandleLobbyStatusNotification(null);
		if (SteamManager.Initialized)
		{
			this.m_steamMtxResponse = Callback<MicroTxnAuthorizationResponse_t>.Create(new Callback<MicroTxnAuthorizationResponse_t>.DispatchDelegate(this.OnSteamMtxResponse));
		}
	}

	protected override void Finalize()
	{
		try
		{
			if (ClientGameManager.Get() != null)
			{
				ClientGameManager.Get().OnLobbyStatusNotification -= this.HandleLobbyStatusNotification;
			}
			CommerceClient.s_instance = null;
		}
		finally
		{
			base.Finalize();
		}
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
			this.RequestPrices();
		}
	}

	public static CommerceClient Get()
	{
		if (CommerceClient.s_instance == null)
		{
			CommerceClient.s_instance = new CommerceClient();
		}
		return CommerceClient.s_instance;
	}

	public void RequestPrices()
	{
		if (ClientGameManager.Get().LobbyInterface != null)
		{
			if (ClientGameManager.Get().ClientAccessLevel > ClientAccessLevel.Locked)
			{
				ClientGameManager.Get().LobbyInterface.RequestPrices(new Action<PricesResponse>(this.HandlePrices));
			}
		}
	}

	public void HandlePrices(PricesResponse response)
	{
		int num;
		if (response.lootMatrixPackPrices == null)
		{
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
			num10 = 0;
		}
		else
		{
			num10 = response.storeItemPrices.Count;
		}
		int num11 = num10;
		Log.Info("Got {0} game pack price(s), {1} loot matrix pack price(s), {2} Character price(s), {3} gg price(s), {4} style price(s), and {5} store item price(s) from server.", new object[]
		{
			num4,
			num2,
			num6,
			num7,
			num9,
			num11
		});
		if (response.gamePackPrices != null)
		{
			this.m_overriddenGamePacks = response.gamePackPrices;
		}
		if (response.lootMatrixPackPrices != null)
		{
			this.m_overriddenLootMatrixPacks = response.lootMatrixPackPrices;
		}
		if (response.characterPrices != null)
		{
			this.m_overriddenCharacters = response.characterPrices;
		}
		if (response.ggPackPrices != null)
		{
			this.m_overriddenGGPacks = response.ggPackPrices;
		}
		if (response.stylePrices != null)
		{
			this.m_overriddenStyles = response.stylePrices;
		}
		if (response.storeItemPrices != null)
		{
			this.m_overriddenStoreItems = response.storeItemPrices;
		}
		this.LastPacificTimePriceRequestWithServerTimeOffset = response.PacificTimeWithServerTimeOffset;
	}

	public int GetISOPriceForInventoryItem(int inventoryItemID)
	{
		InventoryItemTemplate itemTemplate = InventoryWideData.Get().GetItemTemplate(inventoryItemID);
		return itemTemplate.ISOPrice;
	}

	public float GetLootMatrixPackPrice(string productCode, string currencyCode)
	{
		foreach (LootMatrixPackPriceOverride lootMatrixPackPriceOverride in this.m_overriddenLootMatrixPacks)
		{
			if (lootMatrixPackPriceOverride.productCode == productCode && lootMatrixPackPriceOverride.prices != null)
			{
				return lootMatrixPackPriceOverride.prices.GetPrice(currencyCode);
			}
		}
		foreach (LootMatrixPack lootMatrixPack in LootMatrixPackData.Get().m_lootMatrixPacks)
		{
			if (lootMatrixPack.ProductCode == productCode)
			{
				return lootMatrixPack.Prices.GetPrice(currencyCode);
			}
		}
		return 0f;
	}

	public unsafe float GetGamePackPrice(string productCode, string currencyCode, out float originalPrice)
	{
		originalPrice = 0f;
		foreach (GamePack gamePack in GamePackData.Get().m_gamePacks)
		{
			if (gamePack.ProductCode == productCode)
			{
				originalPrice = gamePack.Prices.GetPrice(currencyCode);
			}
			else
			{
				foreach (GamePackUpgrade gamePackUpgrade in gamePack.Upgrades)
				{
					if (gamePackUpgrade.ProductCode == productCode)
					{
						originalPrice = gamePackUpgrade.Prices.GetPrice(currencyCode);
					}
				}
			}
		}
		foreach (GamePackPriceOverride gamePackPriceOverride in this.m_overriddenGamePacks)
		{
			if (gamePackPriceOverride.productCode == productCode && gamePackPriceOverride.prices != null)
			{
				return gamePackPriceOverride.prices.GetPrice(currencyCode);
			}
		}
		return originalPrice;
	}

	public float GetFreelancerPrice(CharacterType type, string currencyCode)
	{
		if (!this.m_overriddenCharacters.IsNullOrEmpty<CharacterPriceOverride>())
		{
			foreach (CharacterPriceOverride characterPriceOverride in this.m_overriddenCharacters)
			{
				if (characterPriceOverride.characterType == type)
				{
					if (characterPriceOverride.prices != null)
					{
						return characterPriceOverride.prices.GetPrice(currencyCode);
					}
				}
			}
		}
		foreach (CharacterResourceLink characterResourceLink in GameWideData.Get().m_characterResourceLinks)
		{
			if (characterResourceLink.m_characterType == type)
			{
				return characterResourceLink.Prices.GetPrice(currencyCode);
			}
		}
		return 0f;
	}

	public float GetGGPackPrice(string productCode, string currencyCode)
	{
		using (List<GGPackPriceOverride>.Enumerator enumerator = this.m_overriddenGGPacks.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				GGPackPriceOverride ggpackPriceOverride = enumerator.Current;
				if (ggpackPriceOverride.productCode == productCode)
				{
					if (ggpackPriceOverride.prices != null)
					{
						return ggpackPriceOverride.prices.GetPrice(currencyCode);
					}
				}
			}
		}
		foreach (GGPack ggpack in GameWideData.Get().m_ggPackData.m_ggPacks)
		{
			if (ggpack.ProductCode == productCode)
			{
				return ggpack.Prices.GetPrice(currencyCode);
			}
		}
		return 0f;
	}

	public float GetStylePrice(CharacterType charType, int skinIndex, int textureIndex, int colorIndex, string currencyCode)
	{
		using (List<StylePriceOverride>.Enumerator enumerator = this.m_overriddenStyles.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				StylePriceOverride stylePriceOverride = enumerator.Current;
				if (stylePriceOverride.prices != null)
				{
					if (stylePriceOverride.characterType == charType && stylePriceOverride.skinIndex == skinIndex && stylePriceOverride.textureIndex == textureIndex)
					{
						if (stylePriceOverride.colorIndex == colorIndex)
						{
							return stylePriceOverride.prices.GetPrice(currencyCode);
						}
					}
				}
			}
		}
		CharacterResourceLink characterResourceLink = GameWideData.Get().GetCharacterResourceLink(charType);
		GameBalanceVars.ColorUnlockData colorUnlockData = characterResourceLink.m_skins[skinIndex].m_patterns[textureIndex].m_colors[colorIndex].m_colorUnlockData;
		return colorUnlockData.Prices.GetPrice(currencyCode);
	}

	public unsafe float GetStoreItemPrice(int templateId, string currencyCode, out float originalPrice)
	{
		originalPrice = 0f;
		foreach (GameBalanceVars.StoreItemForPurchase storeItemForPurchase in GameBalanceVars.Get().StoreItemsForPurchase)
		{
			if (storeItemForPurchase.m_itemTemplateId == templateId)
			{
				originalPrice = storeItemForPurchase.Prices.GetPrice(currencyCode);
				IL_54:
				using (List<StoreItemPriceOverride>.Enumerator enumerator = this.m_overriddenStoreItems.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						StoreItemPriceOverride storeItemPriceOverride = enumerator.Current;
						if (storeItemPriceOverride.inventoryTemplateId == templateId)
						{
							if (storeItemPriceOverride.prices != null)
							{
								return storeItemPriceOverride.prices.GetPrice(currencyCode);
							}
						}
					}
				}
				return originalPrice;
			}
		}
		goto IL_54;
	}
}

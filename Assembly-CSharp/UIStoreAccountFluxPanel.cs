using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.UI;

public class UIStoreAccountFluxPanel : UIStoreBaseInventoryPanel
{
	public Toggle m_ownedToggle;

	private UIPurchaseableItem m_newItemToPurchase;

	protected new void Start()
	{
		base.Start();
		ClientGameManager.Get().OnPlayerBannerChange += OnPlayerBannerChange;
		ClientGameManager.Get().OnPlayerTitleChange += OnPlayerTitleChange;
		ClientGameManager.Get().OnCharacterDataUpdated += OnCharacterUpdated;
		ClientGameManager.Get().OnLoadingScreenBackgroundToggled += OnLoadingScreenBackgroundToggled;
		UITooltipHoverObject component = m_ownedToggle.GetComponent<UITooltipHoverObject>();
		
		component.Setup(TooltipType.Simple, delegate(UITooltipBase tooltip)
			{
				UISimpleTooltip uISimpleTooltip = (UISimpleTooltip)tooltip;
				uISimpleTooltip.Setup(StringUtil.TR("Owned", "Store"));
				return true;
			});
	}

	protected new void OnDestroy()
	{
		base.OnDestroy();
		if (!(ClientGameManager.Get() != null))
		{
			return;
		}
		while (true)
		{
			ClientGameManager.Get().OnPlayerBannerChange -= OnPlayerBannerChange;
			ClientGameManager.Get().OnPlayerTitleChange -= OnPlayerTitleChange;
			ClientGameManager.Get().OnCharacterDataUpdated -= OnCharacterUpdated;
			ClientGameManager.Get().OnLoadingScreenBackgroundToggled -= OnLoadingScreenBackgroundToggled;
			return;
		}
	}

	private void OnLoadingScreenBackgroundToggled(int id, bool isActive)
	{
		RefreshPage();
	}

	private void OnPlayerBannerChange(GameBalanceVars.PlayerBanner emblem, GameBalanceVars.PlayerBanner banner)
	{
		RefreshPage();
	}

	private void OnPlayerTitleChange(string title)
	{
		RefreshPage();
	}

	private void OnCharacterUpdated(PersistedCharacterData charData)
	{
		RefreshPage();
	}

	protected override GameBalanceVars.PlayerUnlockable[] GetRawItemsList()
	{
		List<GameBalanceVars.PlayerUnlockable> list = new List<GameBalanceVars.PlayerUnlockable>();
		List<GameBalanceVars.PlayerUnlockable> list2 = new List<GameBalanceVars.PlayerUnlockable>();
		GameBalanceVars.StoreItemForPurchase[] storeItemsForPurchase = GameBalanceVars.Get().StoreItemsForPurchase;
		foreach (GameBalanceVars.StoreItemForPurchase storeItemForPurchase in storeItemsForPurchase)
		{
			if (storeItemForPurchase.GetUnlockFreelancerCurrencyPrice() > 0)
			{
				list2.Add(storeItemForPurchase);
			}
		}
		list.AddRange(SortItems(list2));
		list2.Clear();
		GameBalanceVars.ChatEmoticon[] chatEmojis = GameBalanceVars.Get().ChatEmojis;
		foreach (GameBalanceVars.ChatEmoticon chatEmoticon in chatEmojis)
		{
			if (chatEmoticon.GetUnlockFreelancerCurrencyPrice() > 0)
			{
				list2.Add(chatEmoticon);
			}
		}
		while (true)
		{
			list.AddRange(SortItems(list2));
			list2.Clear();
			GameBalanceVars.OverconUnlockData[] overcons = GameBalanceVars.Get().Overcons;
			foreach (GameBalanceVars.OverconUnlockData overconUnlockData in overcons)
			{
				if (overconUnlockData.GetUnlockFreelancerCurrencyPrice() > 0)
				{
					list2.Add(overconUnlockData);
				}
			}
			while (true)
			{
				list.AddRange(SortItems(list2));
				list2.Clear();
				GameBalanceVars.PlayerBanner[] playerBanners = GameBalanceVars.Get().PlayerBanners;
				foreach (GameBalanceVars.PlayerBanner playerBanner in playerBanners)
				{
					if (playerBanner.GetUnlockFreelancerCurrencyPrice() > 0)
					{
						list2.Add(playerBanner);
					}
				}
				list.AddRange(SortItems(list2));
				list2.Clear();
				GameBalanceVars.PlayerTitle[] playerTitles = GameBalanceVars.Get().PlayerTitles;
				foreach (GameBalanceVars.PlayerTitle playerTitle in playerTitles)
				{
					if (playerTitle.GetUnlockFreelancerCurrencyPrice() > 0)
					{
						list2.Add(playerTitle);
					}
				}
				while (true)
				{
					list.AddRange(SortItems(list2));
					list2.Clear();
					GameBalanceVars.LoadingScreenBackground[] loadingScreenBackgrounds = GameBalanceVars.Get().LoadingScreenBackgrounds;
					foreach (GameBalanceVars.LoadingScreenBackground loadingScreenBackground in loadingScreenBackgrounds)
					{
						if (loadingScreenBackground.GetUnlockFreelancerCurrencyPrice() > 0)
						{
							list2.Add(loadingScreenBackground);
						}
					}
					while (true)
					{
						list.AddRange(SortItems(list2));
						list2.Clear();
						GameBalanceVars.CharacterUnlockData[] characterUnlockData = GameBalanceVars.Get().characterUnlockData;
						foreach (GameBalanceVars.CharacterUnlockData characterUnlockData2 in characterUnlockData)
						{
							if (GameWideData.Get().GetCharacterResourceLink(characterUnlockData2.character).m_isHidden)
							{
								continue;
							}
							GameBalanceVars.SkinUnlockData[] skinUnlockData = characterUnlockData2.skinUnlockData;
							foreach (GameBalanceVars.SkinUnlockData skinUnlockData2 in skinUnlockData)
							{
								GameBalanceVars.PatternUnlockData[] patternUnlockData = skinUnlockData2.patternUnlockData;
								foreach (GameBalanceVars.PatternUnlockData patternUnlockData2 in patternUnlockData)
								{
									GameBalanceVars.ColorUnlockData[] colorUnlockData = patternUnlockData2.colorUnlockData;
									foreach (GameBalanceVars.ColorUnlockData colorUnlockData2 in colorUnlockData)
									{
										if (colorUnlockData2.GetUnlockFreelancerCurrencyPrice() > 0)
										{
											list2.Add(colorUnlockData2);
										}
									}
									while (true)
									{
										switch (6)
										{
										case 0:
											break;
										default:
											goto end_IL_0303;
										}
										continue;
										end_IL_0303:
										break;
									}
								}
								while (true)
								{
									switch (6)
									{
									case 0:
										break;
									default:
										goto end_IL_031b;
									}
									continue;
									end_IL_031b:
									break;
								}
							}
							list.AddRange(SortItems(list2));
							list2.Clear();
						}
						while (true)
						{
							return list.ToArray();
						}
					}
				}
			}
		}
	}

	protected override Toggle[] GetFilters()
	{
		return new Toggle[1]
		{
			m_ownedToggle
		};
	}

	protected override bool ShouldCheckmark(GameBalanceVars.PlayerUnlockable item)
	{
		int result;
		if (item is GameBalanceVars.PlayerBanner)
		{
			if (ClientGameManager.Get() != null)
			{
				if (ClientGameManager.Get().IsPlayerAccountDataAvailable())
				{
					if (ClientGameManager.Get().GetPlayerAccountData().AccountComponent.SelectedBackgroundBannerID != item.ID)
					{
						result = ((ClientGameManager.Get().GetPlayerAccountData().AccountComponent.SelectedForegroundBannerID == item.ID) ? 1 : 0);
					}
					else
					{
						result = 1;
					}
					goto IL_00a0;
				}
			}
			result = 0;
			goto IL_00a0;
		}
		if (item is GameBalanceVars.PlayerTitle)
		{
			while (true)
			{
				int result2;
				switch (3)
				{
				case 0:
					break;
				default:
					{
						if (ClientGameManager.Get() != null)
						{
							if (ClientGameManager.Get().IsPlayerAccountDataAvailable())
							{
								result2 = ((ClientGameManager.Get().GetPlayerAccountData().AccountComponent.SelectedTitleID == item.ID) ? 1 : 0);
								goto IL_010f;
							}
						}
						result2 = 0;
						goto IL_010f;
					}
					IL_010f:
					return (byte)result2 != 0;
				}
			}
		}
		if (item is GameBalanceVars.LoadingScreenBackground)
		{
			while (true)
			{
				int result3;
				switch (7)
				{
				case 0:
					break;
				default:
					{
						if (ClientGameManager.Get() != null)
						{
							if (ClientGameManager.Get().IsPlayerAccountDataAvailable())
							{
								result3 = (ClientGameManager.Get().GetPlayerAccountData().AccountComponent.IsLoadingScreenBackgroundActive(item.ID) ? 1 : 0);
								goto IL_0168;
							}
						}
						result3 = 0;
						goto IL_0168;
					}
					IL_0168:
					return (byte)result3 != 0;
				}
			}
		}
		return false;
		IL_00a0:
		return (byte)result != 0;
	}

	protected override bool ShouldFilter(GameBalanceVars.PlayerUnlockable item)
	{
		if (m_ownedToggle.isOn)
		{
			if (!(ClientGameManager.Get() == null))
			{
				if (ClientGameManager.Get().IsPlayerAccountDataAvailable())
				{
					if (item is GameBalanceVars.ColorUnlockData)
					{
						CharacterType index = (CharacterType)item.Index1;
						CharacterComponent characterComponent = ClientGameManager.Get().GetPlayerCharacterData(index).CharacterComponent;
						return !characterComponent.GetSkin(item.Index2).GetPattern(item.Index3).GetColor(item.ID)
							.Unlocked;
					}
					AccountComponent accountComponent = ClientGameManager.Get().GetPlayerAccountData().AccountComponent;
					if (item is GameBalanceVars.OverconUnlockData)
					{
						return !accountComponent.IsOverconUnlocked(item.ID);
					}
					if (item is GameBalanceVars.ChatEmoticon)
					{
						while (true)
						{
							return !accountComponent.IsChatEmojiUnlocked(item.ID);
						}
					}
					if (item is GameBalanceVars.PlayerBanner)
					{
						while (true)
						{
							return !accountComponent.UnlockedBannerIDs.Contains(item.ID);
						}
					}
					if (item is GameBalanceVars.PlayerTitle)
					{
						while (true)
						{
							return !accountComponent.UnlockedTitleIDs.Contains(item.ID);
						}
					}
					if (item is GameBalanceVars.LoadingScreenBackground)
					{
						while (true)
						{
							return !accountComponent.IsLoadingScreenBackgroundUnlocked(item.ID);
						}
					}
					if (item is GameBalanceVars.StoreItemForPurchase)
					{
						while (true)
						{
							switch (7)
							{
							case 0:
								break;
							default:
								return true;
							}
						}
					}
					goto IL_018d;
				}
			}
			return true;
		}
		goto IL_018d;
		IL_018d:
		return false;
	}

	protected override void ItemClicked(GameBalanceVars.PlayerUnlockable item)
	{
		if (!item.IsOwned())
		{
			return;
		}
		if (item is GameBalanceVars.PlayerTitle)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					ClientGameManager.Get().RequestTitleSelect(item.ID, null);
					return;
				}
			}
		}
		if (item is GameBalanceVars.PlayerBanner)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					ClientGameManager.Get().RequestBannerSelect(item.ID, null);
					return;
				}
			}
		}
		if (item is GameBalanceVars.PlayerRibbon)
		{
			ClientGameManager.Get().RequestRibbonSelect(item.ID, null);
		}
		else
		{
			if (!(item is GameBalanceVars.LoadingScreenBackground))
			{
				return;
			}
			while (true)
			{
				bool flag = ClientGameManager.Get().GetPlayerAccountData().AccountComponent.IsLoadingScreenBackgroundActive(item.ID);
				ClientGameManager.Get().RequestLoadingScreenBackgroundToggle(item.ID, !flag, null);
				return;
			}
		}
	}

	public override TooltipType? GetItemTooltipType()
	{
		return TooltipType.Titled;
	}

	public override bool ItemTooltipPopulate(UITooltipBase tooltip, UIStoreItemBtn slot, GameBalanceVars.PlayerUnlockable item)
	{
		if (item is GameBalanceVars.ColorUnlockData)
		{
			CharacterType index = (CharacterType)item.Index1;
			string text = StringUtil.TR_CharacterPatternColorDescription(index.ToString(), item.Index2 + 1, item.Index3 + 1, item.ID + 1);
			if (text.Trim().Length > 0)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
					{
						UITitledTooltip uITitledTooltip = tooltip as UITitledTooltip;
						CharacterType index2 = (CharacterType)item.Index1;
						string tooltipTitle = StringUtil.TR_CharacterPatternColorName(index2.ToString(), item.Index2 + 1, item.Index3 + 1, item.ID + 1);
						uITitledTooltip.Setup(tooltipTitle, text, string.Empty);
						return true;
					}
					}
				}
			}
		}
		else if (item is GameBalanceVars.StoreItemForPurchase)
		{
			string text2 = StringUtil.TR_InventoryItemDescription((item as GameBalanceVars.StoreItemForPurchase).m_itemTemplateId);
			if (text2.Trim().Length > 0)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
					{
						UITitledTooltip uITitledTooltip2 = tooltip as UITitledTooltip;
						string tooltipTitle2 = StringUtil.TR_InventoryItemName((item as GameBalanceVars.StoreItemForPurchase).m_itemTemplateId);
						uITitledTooltip2.Setup(tooltipTitle2, text2, string.Empty);
						return true;
					}
					}
				}
			}
		}
		else if (item is GameBalanceVars.LoadingScreenBackground)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
				{
					GameBalanceVars.LoadingScreenBackground loadingScreenBackground = item as GameBalanceVars.LoadingScreenBackground;
					string tooltipText = new StringBuilder().AppendLine(loadingScreenBackground.GetObtainedDescription()).Append(loadingScreenBackground.GetPurchaseDescription()).ToString();
					UITitledTooltip uITitledTooltip3 = tooltip as UITitledTooltip;
					uITitledTooltip3.Setup(loadingScreenBackground.GetLoadingScreenBackgroundName(), tooltipText, string.Empty);
					return true;
				}
				}
			}
		}
		return false;
	}

	protected override void PurchaseItem(GameBalanceVars.PlayerUnlockable item, CurrencyType type)
	{
		m_newItemToPurchase = new UIPurchaseableItem();
		if (type == CurrencyType.NONE)
		{
			m_newItemToPurchase.m_purchaseForCash = true;
		}
		else
		{
			m_newItemToPurchase.m_currencyType = type;
		}
		if (item is GameBalanceVars.OverconUnlockData)
		{
			m_newItemToPurchase.m_itemType = PurchaseItemType.Overcon;
			m_newItemToPurchase.m_overconID = item.ID;
		}
		else if (item is GameBalanceVars.ChatEmoticon)
		{
			m_newItemToPurchase.m_itemType = PurchaseItemType.Emoticon;
			m_newItemToPurchase.m_emoticonID = item.ID;
		}
		else if (item is GameBalanceVars.PlayerBanner)
		{
			m_newItemToPurchase.m_itemType = PurchaseItemType.Banner;
			m_newItemToPurchase.m_bannerID = item.ID;
		}
		else if (item is GameBalanceVars.PlayerTitle)
		{
			m_newItemToPurchase.m_itemType = PurchaseItemType.Title;
			m_newItemToPurchase.m_titleID = item.ID;
		}
		else if (item is GameBalanceVars.StoreItemForPurchase)
		{
			m_newItemToPurchase.m_itemType = PurchaseItemType.InventoryItem;
			m_newItemToPurchase.m_inventoryTemplateId = (item as GameBalanceVars.StoreItemForPurchase).m_itemTemplateId;
			m_newItemToPurchase.m_overlayText = (item as GameBalanceVars.StoreItemForPurchase).m_overlayText;
		}
		else if (item is GameBalanceVars.ColorUnlockData)
		{
			GameBalanceVars.ColorUnlockData colorUnlockData = item as GameBalanceVars.ColorUnlockData;
			m_newItemToPurchase.m_itemType = PurchaseItemType.Tint;
			m_newItemToPurchase.m_charLink = GameWideData.Get().GetCharacterResourceLink((CharacterType)colorUnlockData.Index1);
			m_newItemToPurchase.m_skinIndex = colorUnlockData.Index2;
			m_newItemToPurchase.m_textureIndex = colorUnlockData.Index3;
			m_newItemToPurchase.m_tintIndex = colorUnlockData.ID;
		}
		else if (item is GameBalanceVars.LoadingScreenBackground)
		{
			m_newItemToPurchase.m_itemType = PurchaseItemType.LoadingScreenBackground;
			m_newItemToPurchase.m_loadingScreenBackgroundId = item.ID;
		}
		UIStorePanel.Get().OpenPurchaseDialog(m_newItemToPurchase);
	}
}

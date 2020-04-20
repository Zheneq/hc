using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class UIStoreAccountFluxPanel : UIStoreBaseInventoryPanel
{
	public Toggle m_ownedToggle;

	private UIPurchaseableItem m_newItemToPurchase;

	protected new void Start()
	{
		base.Start();
		ClientGameManager.Get().OnPlayerBannerChange += this.OnPlayerBannerChange;
		ClientGameManager.Get().OnPlayerTitleChange += this.OnPlayerTitleChange;
		ClientGameManager.Get().OnCharacterDataUpdated += this.OnCharacterUpdated;
		ClientGameManager.Get().OnLoadingScreenBackgroundToggled += this.OnLoadingScreenBackgroundToggled;
		UITooltipHoverObject component = this.m_ownedToggle.GetComponent<UITooltipHoverObject>();
		UITooltipObject uitooltipObject = component;
		TooltipType tooltipType = TooltipType.Simple;
		
		uitooltipObject.Setup(tooltipType, delegate(UITooltipBase tooltip)
			{
				UISimpleTooltip uisimpleTooltip = (UISimpleTooltip)tooltip;
				uisimpleTooltip.Setup(StringUtil.TR("Owned", "Store"));
				return true;
			}, null);
	}

	protected new void OnDestroy()
	{
		base.OnDestroy();
		if (ClientGameManager.Get() != null)
		{
			ClientGameManager.Get().OnPlayerBannerChange -= this.OnPlayerBannerChange;
			ClientGameManager.Get().OnPlayerTitleChange -= this.OnPlayerTitleChange;
			ClientGameManager.Get().OnCharacterDataUpdated -= this.OnCharacterUpdated;
			ClientGameManager.Get().OnLoadingScreenBackgroundToggled -= this.OnLoadingScreenBackgroundToggled;
		}
	}

	private void OnLoadingScreenBackgroundToggled(int id, bool isActive)
	{
		base.RefreshPage();
	}

	private void OnPlayerBannerChange(GameBalanceVars.PlayerBanner emblem, GameBalanceVars.PlayerBanner banner)
	{
		base.RefreshPage();
	}

	private void OnPlayerTitleChange(string title)
	{
		base.RefreshPage();
	}

	private void OnCharacterUpdated(PersistedCharacterData charData)
	{
		base.RefreshPage();
	}

	protected override GameBalanceVars.PlayerUnlockable[] GetRawItemsList()
	{
		List<GameBalanceVars.PlayerUnlockable> list = new List<GameBalanceVars.PlayerUnlockable>();
		List<GameBalanceVars.PlayerUnlockable> list2 = new List<GameBalanceVars.PlayerUnlockable>();
		foreach (GameBalanceVars.StoreItemForPurchase storeItemForPurchase in GameBalanceVars.Get().StoreItemsForPurchase)
		{
			if (storeItemForPurchase.GetUnlockFreelancerCurrencyPrice() > 0)
			{
				list2.Add(storeItemForPurchase);
			}
		}
		list.AddRange(base.SortItems(list2));
		list2.Clear();
		foreach (GameBalanceVars.ChatEmoticon chatEmoticon in GameBalanceVars.Get().ChatEmojis)
		{
			if (chatEmoticon.GetUnlockFreelancerCurrencyPrice() > 0)
			{
				list2.Add(chatEmoticon);
			}
		}
		list.AddRange(base.SortItems(list2));
		list2.Clear();
		foreach (GameBalanceVars.OverconUnlockData overconUnlockData in GameBalanceVars.Get().Overcons)
		{
			if (overconUnlockData.GetUnlockFreelancerCurrencyPrice() > 0)
			{
				list2.Add(overconUnlockData);
			}
		}
		list.AddRange(base.SortItems(list2));
		list2.Clear();
		foreach (GameBalanceVars.PlayerBanner playerBanner in GameBalanceVars.Get().PlayerBanners)
		{
			if (playerBanner.GetUnlockFreelancerCurrencyPrice() > 0)
			{
				list2.Add(playerBanner);
			}
		}
		list.AddRange(base.SortItems(list2));
		list2.Clear();
		foreach (GameBalanceVars.PlayerTitle playerTitle in GameBalanceVars.Get().PlayerTitles)
		{
			if (playerTitle.GetUnlockFreelancerCurrencyPrice() > 0)
			{
				list2.Add(playerTitle);
			}
		}
		list.AddRange(base.SortItems(list2));
		list2.Clear();
		foreach (GameBalanceVars.LoadingScreenBackground loadingScreenBackground in GameBalanceVars.Get().LoadingScreenBackgrounds)
		{
			if (loadingScreenBackground.GetUnlockFreelancerCurrencyPrice() > 0)
			{
				list2.Add(loadingScreenBackground);
			}
		}
		list.AddRange(base.SortItems(list2));
		list2.Clear();
		foreach (GameBalanceVars.CharacterUnlockData characterUnlockData2 in GameBalanceVars.Get().characterUnlockData)
		{
			if (GameWideData.Get().GetCharacterResourceLink(characterUnlockData2.character).m_isHidden)
			{
			}
			else
			{
				foreach (GameBalanceVars.SkinUnlockData skinUnlockData2 in characterUnlockData2.skinUnlockData)
				{
					foreach (GameBalanceVars.PatternUnlockData patternUnlockData2 in skinUnlockData2.patternUnlockData)
					{
						foreach (GameBalanceVars.ColorUnlockData colorUnlockData2 in patternUnlockData2.colorUnlockData)
						{
							if (colorUnlockData2.GetUnlockFreelancerCurrencyPrice() > 0)
							{
								list2.Add(colorUnlockData2);
							}
						}
					}
				}
				list.AddRange(base.SortItems(list2));
				list2.Clear();
			}
		}
		return list.ToArray();
	}

	protected override Toggle[] GetFilters()
	{
		return new Toggle[]
		{
			this.m_ownedToggle
		};
	}

	protected override bool ShouldCheckmark(GameBalanceVars.PlayerUnlockable item)
	{
		if (item is GameBalanceVars.PlayerBanner)
		{
			int result;
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
					return result != 0;
				}
			}
			result = 0;
			return result != 0;
		}
		if (item is GameBalanceVars.PlayerTitle)
		{
			if (ClientGameManager.Get() != null)
			{
				if (ClientGameManager.Get().IsPlayerAccountDataAvailable())
				{
					return ClientGameManager.Get().GetPlayerAccountData().AccountComponent.SelectedTitleID == item.ID;
				}
			}
			return false;
		}
		if (item is GameBalanceVars.LoadingScreenBackground)
		{
			if (ClientGameManager.Get() != null)
			{
				if (ClientGameManager.Get().IsPlayerAccountDataAvailable())
				{
					return ClientGameManager.Get().GetPlayerAccountData().AccountComponent.IsLoadingScreenBackgroundActive(item.ID);
				}
			}
			return false;
		}
		return false;
	}

	protected override bool ShouldFilter(GameBalanceVars.PlayerUnlockable item)
	{
		if (this.m_ownedToggle.isOn)
		{
			if (!(ClientGameManager.Get() == null))
			{
				if (!ClientGameManager.Get().IsPlayerAccountDataAvailable())
				{
				}
				else
				{
					if (item is GameBalanceVars.ColorUnlockData)
					{
						CharacterType index = (CharacterType)item.Index1;
						CharacterComponent characterComponent = ClientGameManager.Get().GetPlayerCharacterData(index).CharacterComponent;
						return !characterComponent.GetSkin(item.Index2).GetPattern(item.Index3).GetColor(item.ID).Unlocked;
					}
					AccountComponent accountComponent = ClientGameManager.Get().GetPlayerAccountData().AccountComponent;
					if (item is GameBalanceVars.OverconUnlockData)
					{
						return !accountComponent.IsOverconUnlocked(item.ID);
					}
					if (item is GameBalanceVars.ChatEmoticon)
					{
						return !accountComponent.IsChatEmojiUnlocked(item.ID);
					}
					if (item is GameBalanceVars.PlayerBanner)
					{
						return !accountComponent.UnlockedBannerIDs.Contains(item.ID);
					}
					if (item is GameBalanceVars.PlayerTitle)
					{
						return !accountComponent.UnlockedTitleIDs.Contains(item.ID);
					}
					if (item is GameBalanceVars.LoadingScreenBackground)
					{
						return !accountComponent.IsLoadingScreenBackgroundUnlocked(item.ID);
					}
					if (item is GameBalanceVars.StoreItemForPurchase)
					{
						return true;
					}
					return false;
				}
			}
			return true;
		}
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
			ClientGameManager.Get().RequestTitleSelect(item.ID, null);
		}
		else if (item is GameBalanceVars.PlayerBanner)
		{
			ClientGameManager.Get().RequestBannerSelect(item.ID, null);
		}
		else if (item is GameBalanceVars.PlayerRibbon)
		{
			ClientGameManager.Get().RequestRibbonSelect(item.ID, null);
		}
		else if (item is GameBalanceVars.LoadingScreenBackground)
		{
			bool flag = ClientGameManager.Get().GetPlayerAccountData().AccountComponent.IsLoadingScreenBackgroundActive(item.ID);
			ClientGameManager.Get().RequestLoadingScreenBackgroundToggle(item.ID, !flag, null);
		}
	}

	public override TooltipType? GetItemTooltipType()
	{
		return new TooltipType?(TooltipType.Titled);
	}

	public override bool ItemTooltipPopulate(UITooltipBase tooltip, UIStoreItemBtn slot, GameBalanceVars.PlayerUnlockable item)
	{
		if (item is GameBalanceVars.ColorUnlockData)
		{
			CharacterType index = (CharacterType)item.Index1;
			string text = StringUtil.TR_CharacterPatternColorDescription(index.ToString(), item.Index2 + 1, item.Index3 + 1, item.ID + 1);
			if (text.Trim().Length > 0)
			{
				UITitledTooltip uititledTooltip = tooltip as UITitledTooltip;
				CharacterType index2 = (CharacterType)item.Index1;
				string tooltipTitle = StringUtil.TR_CharacterPatternColorName(index2.ToString(), item.Index2 + 1, item.Index3 + 1, item.ID + 1);
				uititledTooltip.Setup(tooltipTitle, text, string.Empty);
				return true;
			}
		}
		else if (item is GameBalanceVars.StoreItemForPurchase)
		{
			string text2 = StringUtil.TR_InventoryItemDescription((item as GameBalanceVars.StoreItemForPurchase).m_itemTemplateId);
			if (text2.Trim().Length > 0)
			{
				UITitledTooltip uititledTooltip2 = tooltip as UITitledTooltip;
				string tooltipTitle2 = StringUtil.TR_InventoryItemName((item as GameBalanceVars.StoreItemForPurchase).m_itemTemplateId);
				uititledTooltip2.Setup(tooltipTitle2, text2, string.Empty);
				return true;
			}
		}
		else if (item is GameBalanceVars.LoadingScreenBackground)
		{
			GameBalanceVars.LoadingScreenBackground loadingScreenBackground = item as GameBalanceVars.LoadingScreenBackground;
			string tooltipText = loadingScreenBackground.GetObtainedDescription() + Environment.NewLine + loadingScreenBackground.GetPurchaseDescription();
			UITitledTooltip uititledTooltip3 = tooltip as UITitledTooltip;
			uititledTooltip3.Setup(loadingScreenBackground.GetLoadingScreenBackgroundName(), tooltipText, string.Empty);
			return true;
		}
		return false;
	}

	protected override void PurchaseItem(GameBalanceVars.PlayerUnlockable item, CurrencyType type)
	{
		this.m_newItemToPurchase = new UIPurchaseableItem();
		if (type == CurrencyType.NONE)
		{
			this.m_newItemToPurchase.m_purchaseForCash = true;
		}
		else
		{
			this.m_newItemToPurchase.m_currencyType = type;
		}
		if (item is GameBalanceVars.OverconUnlockData)
		{
			this.m_newItemToPurchase.m_itemType = PurchaseItemType.Overcon;
			this.m_newItemToPurchase.m_overconID = item.ID;
		}
		else if (item is GameBalanceVars.ChatEmoticon)
		{
			this.m_newItemToPurchase.m_itemType = PurchaseItemType.Emoticon;
			this.m_newItemToPurchase.m_emoticonID = item.ID;
		}
		else if (item is GameBalanceVars.PlayerBanner)
		{
			this.m_newItemToPurchase.m_itemType = PurchaseItemType.Banner;
			this.m_newItemToPurchase.m_bannerID = item.ID;
		}
		else if (item is GameBalanceVars.PlayerTitle)
		{
			this.m_newItemToPurchase.m_itemType = PurchaseItemType.Title;
			this.m_newItemToPurchase.m_titleID = item.ID;
		}
		else if (item is GameBalanceVars.StoreItemForPurchase)
		{
			this.m_newItemToPurchase.m_itemType = PurchaseItemType.InventoryItem;
			this.m_newItemToPurchase.m_inventoryTemplateId = (item as GameBalanceVars.StoreItemForPurchase).m_itemTemplateId;
			this.m_newItemToPurchase.m_overlayText = (item as GameBalanceVars.StoreItemForPurchase).m_overlayText;
		}
		else if (item is GameBalanceVars.ColorUnlockData)
		{
			GameBalanceVars.ColorUnlockData colorUnlockData = item as GameBalanceVars.ColorUnlockData;
			this.m_newItemToPurchase.m_itemType = PurchaseItemType.Tint;
			this.m_newItemToPurchase.m_charLink = GameWideData.Get().GetCharacterResourceLink((CharacterType)colorUnlockData.Index1);
			this.m_newItemToPurchase.m_skinIndex = colorUnlockData.Index2;
			this.m_newItemToPurchase.m_textureIndex = colorUnlockData.Index3;
			this.m_newItemToPurchase.m_tintIndex = colorUnlockData.ID;
		}
		else if (item is GameBalanceVars.LoadingScreenBackground)
		{
			this.m_newItemToPurchase.m_itemType = PurchaseItemType.LoadingScreenBackground;
			this.m_newItemToPurchase.m_loadingScreenBackgroundId = item.ID;
		}
		UIStorePanel.Get().OpenPurchaseDialog(this.m_newItemToPurchase, null);
	}
}

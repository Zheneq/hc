﻿using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class UIStoreAccountBannerPanel : UIStoreBaseInventoryPanel
{
	public Toggle m_ownedToggle;

	public Toggle m_titleToggle;

	public Toggle m_backgroundToggle;

	public Toggle m_emblemToggle;

	public Toggle m_ribbonToggle;

	protected new void Start()
	{
		base.Start();
		ClientGameManager.Get().OnPlayerBannerChange += this.OnPlayerBannerChange;
		ClientGameManager.Get().OnPlayerTitleChange += this.OnPlayerTitleChange;
		this.SetupTooltip(this.m_ownedToggle, StringUtil.TR("Owned", "Store"));
		this.SetupTooltip(this.m_titleToggle, StringUtil.TR("Titles", "Rewards"));
		this.SetupTooltip(this.m_backgroundToggle, StringUtil.TR("Banners", "Store"));
		this.SetupTooltip(this.m_emblemToggle, StringUtil.TR("Emblems", "Rewards"));
		this.SetupTooltip(this.m_ribbonToggle, StringUtil.TR("Ribbons", "OverlayScreensScene"));
	}

	protected new void OnDestroy()
	{
		base.OnDestroy();
		if (ClientGameManager.Get() != null)
		{
			ClientGameManager.Get().OnPlayerBannerChange -= this.OnPlayerBannerChange;
			ClientGameManager.Get().OnPlayerTitleChange -= this.OnPlayerTitleChange;
		}
	}

	private void OnPlayerBannerChange(GameBalanceVars.PlayerBanner emblem, GameBalanceVars.PlayerBanner banner)
	{
		base.RefreshPage();
	}

	private void OnPlayerTitleChange(string title)
	{
		base.RefreshPage();
	}

	protected override GameBalanceVars.PlayerUnlockable[] GetRawItemsList()
	{
		List<GameBalanceVars.PlayerUnlockable> list = new List<GameBalanceVars.PlayerUnlockable>();
		List<GameBalanceVars.PlayerUnlockable> list2 = new List<GameBalanceVars.PlayerUnlockable>();
		List<GameBalanceVars.PlayerUnlockable> list3 = new List<GameBalanceVars.PlayerUnlockable>();
		List<GameBalanceVars.PlayerUnlockable> list4 = new List<GameBalanceVars.PlayerUnlockable>();
		List<GameBalanceVars.PlayerUnlockable> list5 = new List<GameBalanceVars.PlayerUnlockable>();
		GameBalanceVars.PlayerBanner[] playerBanners = GameBalanceVars.Get().PlayerBanners;
		int i = 0;
		while (i < playerBanners.Length)
		{
			GameBalanceVars.PlayerBanner playerBanner = playerBanners[i];
			if (this.m_charType == CharacterType.None)
			{
				goto IL_6D;
			}
			if (playerBanner.m_relatedCharacter == this.m_charType)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					goto IL_6D;
				}
			}
			IL_93:
			i++;
			continue;
			IL_6D:
			if (playerBanner.m_type == GameBalanceVars.PlayerBanner.BannerType.Background)
			{
				list2.Add(playerBanner);
				goto IL_93;
			}
			list3.Add(playerBanner);
			goto IL_93;
		}
		GameBalanceVars.PlayerTitle[] playerTitles = GameBalanceVars.Get().PlayerTitles;
		int j = 0;
		while (j < playerTitles.Length)
		{
			GameBalanceVars.PlayerTitle playerTitle = playerTitles[j];
			if (this.m_charType == CharacterType.None)
			{
				goto IL_E6;
			}
			if (playerTitle.m_relatedCharacter == this.m_charType)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					goto IL_E6;
				}
			}
			IL_EE:
			j++;
			continue;
			IL_E6:
			list4.Add(playerTitle);
			goto IL_EE;
		}
		foreach (GameBalanceVars.PlayerRibbon item in GameBalanceVars.Get().PlayerRibbons)
		{
			if (this.m_charType == CharacterType.None)
			{
				list5.Add(item);
			}
		}
		list.AddRange(base.SortItems(list2));
		list.AddRange(base.SortItems(list3));
		list.AddRange(base.SortItems(list4));
		list.AddRange(base.SortItems(list5));
		return list.ToArray();
	}

	protected override Toggle[] GetFilters()
	{
		return new Toggle[]
		{
			this.m_ownedToggle,
			this.m_titleToggle,
			this.m_backgroundToggle,
			this.m_emblemToggle,
			this.m_ribbonToggle
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
			bool result2;
			if (ClientGameManager.Get() != null && ClientGameManager.Get().IsPlayerAccountDataAvailable())
			{
				result2 = (ClientGameManager.Get().GetPlayerAccountData().AccountComponent.SelectedTitleID == item.ID);
			}
			else
			{
				result2 = false;
			}
			return result2;
		}
		return false;
	}

	protected override bool ShouldFilter(GameBalanceVars.PlayerUnlockable item)
	{
		if (this.m_ownedToggle.isOn)
		{
			if (!(ClientGameManager.Get() == null))
			{
				if (ClientGameManager.Get().IsPlayerAccountDataAvailable())
				{
					AccountComponent accountComponent = ClientGameManager.Get().GetPlayerAccountData().AccountComponent;
					bool flag = false;
					bool flag2;
					if (!this.m_titleToggle.isOn && !this.m_emblemToggle.isOn)
					{
						if (!this.m_backgroundToggle.isOn)
						{
							flag2 = !this.m_ribbonToggle.isOn;
							goto IL_C0;
						}
					}
					flag2 = false;
					IL_C0:
					bool flag3 = flag2;
					if (item is GameBalanceVars.PlayerBanner)
					{
						GameBalanceVars.PlayerBanner playerBanner = item as GameBalanceVars.PlayerBanner;
						bool flag4;
						if (accountComponent.UnlockedBannerIDs.Contains(playerBanner.ID))
						{
							if (!this.m_emblemToggle.isOn || playerBanner.m_type != GameBalanceVars.PlayerBanner.BannerType.Foreground)
							{
								if (this.m_backgroundToggle.isOn)
								{
									if (playerBanner.m_type == GameBalanceVars.PlayerBanner.BannerType.Background)
									{
										goto IL_138;
									}
								}
								flag4 = flag3;
								goto IL_139;
							}
							IL_138:
							flag4 = true;
							IL_139:;
						}
						else
						{
							flag4 = false;
						}
						flag = flag4;
					}
					else if (item is GameBalanceVars.PlayerTitle)
					{
						flag = (accountComponent.UnlockedTitleIDs.Contains(item.ID) && (this.m_titleToggle.isOn || flag3));
					}
					else if (item is GameBalanceVars.PlayerRibbon)
					{
						bool flag5;
						if (accountComponent.UnlockedRibbonIDs.Contains(item.ID))
						{
							flag5 = (this.m_ribbonToggle.isOn || flag3);
						}
						else
						{
							flag5 = false;
						}
						flag = flag5;
					}
					return !flag;
				}
			}
			return true;
		}
		if (!this.m_titleToggle.isOn)
		{
			if (!this.m_emblemToggle.isOn)
			{
				if (!this.m_backgroundToggle.isOn)
				{
					if (!this.m_ribbonToggle.isOn)
					{
						return false;
					}
				}
			}
		}
		bool result = true;
		if (this.m_titleToggle.isOn)
		{
			if (item is GameBalanceVars.PlayerTitle)
			{
				result = false;
			}
		}
		if (this.m_ribbonToggle.isOn && item is GameBalanceVars.PlayerRibbon)
		{
			result = false;
		}
		if (!this.m_backgroundToggle.isOn)
		{
			if (!this.m_emblemToggle.isOn)
			{
				return result;
			}
		}
		if (item is GameBalanceVars.PlayerBanner)
		{
			GameBalanceVars.PlayerBanner playerBanner2 = item as GameBalanceVars.PlayerBanner;
			if (this.m_backgroundToggle.isOn)
			{
				if (playerBanner2.m_type == GameBalanceVars.PlayerBanner.BannerType.Background)
				{
					goto IL_316;
				}
			}
			if (!this.m_emblemToggle.isOn)
			{
				return result;
			}
			if (playerBanner2.m_type != GameBalanceVars.PlayerBanner.BannerType.Foreground)
			{
				return result;
			}
			IL_316:
			result = false;
		}
		return result;
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
	}

	public override TooltipType? GetItemTooltipType()
	{
		return new TooltipType?(TooltipType.BannerPreview);
	}

	public override bool ItemTooltipPopulate(UITooltipBase tooltip, UIStoreItemBtn slot, GameBalanceVars.PlayerUnlockable item)
	{
		GameBalanceVars.PlayerBanner playerBanner = null;
		GameBalanceVars.PlayerBanner playerBanner2 = null;
		GameBalanceVars.PlayerRibbon playerRibbon = null;
		string text = null;
		string previewTitle;
		if (item is GameBalanceVars.PlayerTitle)
		{
			text = GameBalanceVars.Get().GetTitle(item.ID, string.Empty, -1);
			previewTitle = StringUtil.TR("TitlePreview", "OverlayScreens");
		}
		else if (item is GameBalanceVars.PlayerRibbon)
		{
			playerRibbon = GameBalanceVars.Get().GetRibbon(item.ID);
			previewTitle = StringUtil.TR("RibbonPreview", "OverlayScreens");
		}
		else
		{
			if (!(item is GameBalanceVars.PlayerBanner))
			{
				return false;
			}
			GameBalanceVars.PlayerBanner playerBanner3 = item as GameBalanceVars.PlayerBanner;
			if (playerBanner3.m_type == GameBalanceVars.PlayerBanner.BannerType.Background)
			{
				playerBanner = playerBanner3;
				previewTitle = StringUtil.TR("BannerPreview", "OverlayScreens");
			}
			else
			{
				playerBanner2 = playerBanner3;
				previewTitle = StringUtil.TR("EmblemPreview", "OverlayScreens");
			}
		}
		AccountComponent accountComponent = ClientGameManager.Get().GetPlayerAccountData().AccountComponent;
		if (playerBanner2 == null)
		{
			playerBanner2 = GameBalanceVars.Get().GetBanner(accountComponent.SelectedForegroundBannerID);
		}
		if (playerBanner == null)
		{
			playerBanner = GameBalanceVars.Get().GetBanner(accountComponent.SelectedBackgroundBannerID);
		}
		if (text == null)
		{
			text = GameBalanceVars.Get().GetTitle(accountComponent.SelectedTitleID, string.Empty, -1);
		}
		if (playerRibbon == null)
		{
			playerRibbon = GameBalanceVars.Get().GetRibbon(accountComponent.SelectedRibbonID);
		}
		UIBannerPreviewTooltip uibannerPreviewTooltip = tooltip as UIBannerPreviewTooltip;
		uibannerPreviewTooltip.Setup(previewTitle, playerBanner, playerBanner2, playerRibbon, text);
		return true;
	}

	protected override void PurchaseItem(GameBalanceVars.PlayerUnlockable item, CurrencyType type)
	{
		UIPurchaseableItem uipurchaseableItem = new UIPurchaseableItem();
		UIPurchaseableItem uipurchaseableItem2 = uipurchaseableItem;
		PurchaseItemType itemType;
		if (item is GameBalanceVars.PlayerTitle)
		{
			itemType = PurchaseItemType.Title;
		}
		else
		{
			itemType = PurchaseItemType.Banner;
		}
		uipurchaseableItem2.m_itemType = itemType;
		uipurchaseableItem.m_bannerID = item.ID;
		uipurchaseableItem.m_titleID = item.ID;
		uipurchaseableItem.m_currencyType = type;
		UIStorePanel.Get().OpenPurchaseDialog(uipurchaseableItem);
	}

	private void SetupTooltip(Toggle toggle, string text)
	{
		UITooltipHoverObject component = toggle.GetComponent<UITooltipHoverObject>();
		component.Setup(TooltipType.Simple, delegate(UITooltipBase tooltip)
		{
			UISimpleTooltip uisimpleTooltip = (UISimpleTooltip)tooltip;
			uisimpleTooltip.Setup(text);
			return true;
		}, null);
	}
}

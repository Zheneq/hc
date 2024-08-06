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
		ClientGameManager.Get().OnPlayerBannerChange += OnPlayerBannerChange;
		ClientGameManager.Get().OnPlayerTitleChange += OnPlayerTitleChange;
		SetupTooltip(m_ownedToggle, StringUtil.TR("Owned", "Store"));
		SetupTooltip(m_titleToggle, StringUtil.TR("Titles", "Rewards"));
		SetupTooltip(m_backgroundToggle, StringUtil.TR("Banners", "Store"));
		SetupTooltip(m_emblemToggle, StringUtil.TR("Emblems", "Rewards"));
		SetupTooltip(m_ribbonToggle, StringUtil.TR("Ribbons", "OverlayScreensScene"));
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
			return;
		}
	}

	private void OnPlayerBannerChange(GameBalanceVars.PlayerBanner emblem, GameBalanceVars.PlayerBanner banner)
	{
		RefreshPage();
	}

	private void OnPlayerTitleChange(string title)
	{
		RefreshPage();
	}

	protected override GameBalanceVars.PlayerUnlockable[] GetRawItemsList()
	{
		List<GameBalanceVars.PlayerUnlockable> list = new List<GameBalanceVars.PlayerUnlockable>();
		List<GameBalanceVars.PlayerUnlockable> list2 = new List<GameBalanceVars.PlayerUnlockable>();
		List<GameBalanceVars.PlayerUnlockable> list3 = new List<GameBalanceVars.PlayerUnlockable>();
		List<GameBalanceVars.PlayerUnlockable> list4 = new List<GameBalanceVars.PlayerUnlockable>();
		List<GameBalanceVars.PlayerUnlockable> list5 = new List<GameBalanceVars.PlayerUnlockable>();
		GameBalanceVars.PlayerBanner[] playerBanners = GameBalanceVars.Get().PlayerBanners;
		foreach (GameBalanceVars.PlayerBanner playerBanner in playerBanners)
		{
			if (m_charType != 0)
			{
				if (playerBanner.m_relatedCharacter != m_charType)
				{
					continue;
				}
			}
			if (playerBanner.m_type == GameBalanceVars.PlayerBanner.BannerType.Background)
			{
				list2.Add(playerBanner);
			}
			else
			{
				list3.Add(playerBanner);
			}
		}
		GameBalanceVars.PlayerTitle[] playerTitles = GameBalanceVars.Get().PlayerTitles;
		foreach (GameBalanceVars.PlayerTitle playerTitle in playerTitles)
		{
			if (m_charType != 0)
			{
				if (playerTitle.m_relatedCharacter != m_charType)
				{
					continue;
				}
			}
			list4.Add(playerTitle);
		}
		GameBalanceVars.PlayerRibbon[] playerRibbons = GameBalanceVars.Get().PlayerRibbons;
		foreach (GameBalanceVars.PlayerRibbon item in playerRibbons)
		{
			if (m_charType == CharacterType.None)
			{
				list5.Add(item);
			}
		}
		list.AddRange(SortItems(list2));
		list.AddRange(SortItems(list3));
		list.AddRange(SortItems(list4));
		list.AddRange(SortItems(list5));
		return list.ToArray();
	}

	protected override Toggle[] GetFilters()
	{
		return new Toggle[5]
		{
			m_ownedToggle,
			m_titleToggle,
			m_backgroundToggle,
			m_emblemToggle,
			m_ribbonToggle
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
					goto IL_009c;
				}
			}
			result = 0;
			goto IL_009c;
		}
		if (item is GameBalanceVars.PlayerTitle)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
				{
					int result2;
					if (ClientGameManager.Get() != null && ClientGameManager.Get().IsPlayerAccountDataAvailable())
					{
						result2 = ((ClientGameManager.Get().GetPlayerAccountData().AccountComponent.SelectedTitleID == item.ID) ? 1 : 0);
					}
					else
					{
						result2 = 0;
					}
					return (byte)result2 != 0;
				}
				}
			}
		}
		return false;
		IL_009c:
		return (byte)result != 0;
	}

	protected override bool ShouldFilter(GameBalanceVars.PlayerUnlockable item)
	{
		if (m_ownedToggle.isOn)
		{
			while (true)
			{
				AccountComponent accountComponent;
				bool flag;
				int num;
				if (!(ClientGameManager.Get() == null))
				{
					if (ClientGameManager.Get().IsPlayerAccountDataAvailable())
					{
						accountComponent = ClientGameManager.Get().GetPlayerAccountData().AccountComponent;
						flag = false;
						if (!m_titleToggle.isOn && !m_emblemToggle.isOn)
						{
							if (!m_backgroundToggle.isOn)
							{
								num = ((!m_ribbonToggle.isOn) ? 1 : 0);
								goto IL_00c0;
							}
						}
						num = 0;
						goto IL_00c0;
					}
				}
				return true;
				IL_00c0:
				bool flag2 = (byte)num != 0;
				int num2;
				if (item is GameBalanceVars.PlayerBanner)
				{
					GameBalanceVars.PlayerBanner playerBanner = item as GameBalanceVars.PlayerBanner;
					if (accountComponent.UnlockedBannerIDs.Contains(playerBanner.ID))
					{
						if (m_emblemToggle.isOn && playerBanner.m_type == GameBalanceVars.PlayerBanner.BannerType.Foreground)
						{
							goto IL_0138;
						}
						if (m_backgroundToggle.isOn)
						{
							if (playerBanner.m_type == GameBalanceVars.PlayerBanner.BannerType.Background)
							{
								goto IL_0138;
							}
						}
						num2 = (flag2 ? 1 : 0);
					}
					else
					{
						num2 = 0;
					}
					goto IL_013c;
				}
				if (item is GameBalanceVars.PlayerTitle)
				{
					flag = (accountComponent.UnlockedTitleIDs.Contains(item.ID) && (m_titleToggle.isOn || flag2));
				}
				else if (item is GameBalanceVars.PlayerRibbon)
				{
					int num3;
					if (accountComponent.UnlockedRibbonIDs.Contains(item.ID))
					{
						num3 = ((m_ribbonToggle.isOn || flag2) ? 1 : 0);
					}
					else
					{
						num3 = 0;
					}
					flag = ((byte)num3 != 0);
				}
				goto IL_01c6;
				IL_01c6:
				return !flag;
				IL_013c:
				flag = ((byte)num2 != 0);
				goto IL_01c6;
				IL_0138:
				num2 = 1;
				goto IL_013c;
			}
		}
		if (!m_titleToggle.isOn)
		{
			if (!m_emblemToggle.isOn)
			{
				if (!m_backgroundToggle.isOn)
				{
					if (!m_ribbonToggle.isOn)
					{
						while (true)
						{
							switch (7)
							{
							case 0:
								break;
							default:
								return false;
							}
						}
					}
				}
			}
		}
		bool result = true;
		if (m_titleToggle.isOn)
		{
			if (item is GameBalanceVars.PlayerTitle)
			{
				result = false;
			}
		}
		if (m_ribbonToggle.isOn && item is GameBalanceVars.PlayerRibbon)
		{
			result = false;
		}
		if (!m_backgroundToggle.isOn)
		{
			if (!m_emblemToggle.isOn)
			{
				goto IL_0319;
			}
		}
		if (item is GameBalanceVars.PlayerBanner)
		{
			GameBalanceVars.PlayerBanner playerBanner2 = item as GameBalanceVars.PlayerBanner;
			if (m_backgroundToggle.isOn)
			{
				if (playerBanner2.m_type == GameBalanceVars.PlayerBanner.BannerType.Background)
				{
					goto IL_0316;
				}
			}
			if (m_emblemToggle.isOn)
			{
				if (playerBanner2.m_type == GameBalanceVars.PlayerBanner.BannerType.Foreground)
				{
					goto IL_0316;
				}
			}
		}
		goto IL_0319;
		IL_0319:
		return result;
		IL_0316:
		result = false;
		goto IL_0319;
	}

	protected override void ItemClicked(GameBalanceVars.PlayerUnlockable item)
	{
		if (!item.IsOwned())
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		if (item is GameBalanceVars.PlayerTitle)
		{
			while (true)
			{
				switch (1)
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
				switch (2)
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
	}

	public override TooltipType? GetItemTooltipType()
	{
		return TooltipType.BannerPreview;
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
			text = GameBalanceVars.Get().GetTitle(item.ID, string.Empty);
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
				while (true)
				{
					return false;
				}
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
#if !VANILLA && !SERVER
            // Custom titles
            text = GameBalanceVars.Get().GetTitle(accountComponent.SelectedTitleID, ClientGameManager.Get().Handle, string.Empty);
#else
			text = GameBalanceVars.Get().GetTitle(accountComponent.SelectedTitleID, string.Empty);
#endif
		}
		if (playerRibbon == null)
		{
			playerRibbon = GameBalanceVars.Get().GetRibbon(accountComponent.SelectedRibbonID);
		}
		UIBannerPreviewTooltip uIBannerPreviewTooltip = tooltip as UIBannerPreviewTooltip;
		uIBannerPreviewTooltip.Setup(previewTitle, playerBanner, playerBanner2, playerRibbon, text);
		return true;
	}

	protected override void PurchaseItem(GameBalanceVars.PlayerUnlockable item, CurrencyType type)
	{
		UIPurchaseableItem uIPurchaseableItem = new UIPurchaseableItem();
		int itemType;
		if (item is GameBalanceVars.PlayerTitle)
		{
			itemType = 13;
		}
		else
		{
			itemType = 12;
		}
		uIPurchaseableItem.m_itemType = (PurchaseItemType)itemType;
		uIPurchaseableItem.m_bannerID = item.ID;
		uIPurchaseableItem.m_titleID = item.ID;
		uIPurchaseableItem.m_currencyType = type;
		UIStorePanel.Get().OpenPurchaseDialog(uIPurchaseableItem);
	}

	private void SetupTooltip(Toggle toggle, string text)
	{
		UITooltipHoverObject component = toggle.GetComponent<UITooltipHoverObject>();
		component.Setup(TooltipType.Simple, delegate(UITooltipBase tooltip)
		{
			UISimpleTooltip uISimpleTooltip = (UISimpleTooltip)tooltip;
			uISimpleTooltip.Setup(text);
			return true;
		});
	}
}

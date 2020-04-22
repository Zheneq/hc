using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UICashShopItemBtn : MonoBehaviour
{
	public _SelectableBtn m_selectableBtn;

	public Image m_icon;

	public Image m_iconFg;

	public RectTransform m_textLayout;

	public TextMeshProUGUI m_nameText;

	public TextMeshProUGUI m_costText;

	public TextMeshProUGUI m_fluxCostText;

	public RectTransform m_ownedContainer;

	public RectTransform m_discountContainer;

	public TextMeshProUGUI m_discountedAmount;

	[Header("Special")]
	public Image m_lootMatrixIcon;

	public RectTransform m_lootMatrixTextLayout;

	public TextMeshProUGUI m_lootMatrixNameText;

	public TextMeshProUGUI m_lootMatrixCostText;

	public TextMeshProUGUI m_eventText;

	public _SelectableBtn m_viewContentBtn;

	public _SelectableBtn m_viewContentBundleBtn;

	private UIPurchaseableItem m_item;

	private void Awake()
	{
		m_selectableBtn.spriteController.callback = Clicked;
		if (m_viewContentBtn != null)
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
			m_selectableBtn.spriteController.AddSubButton(m_viewContentBtn.spriteController);
			m_viewContentBtn.spriteController.callback = ViewContentClicked;
		}
		if (!(m_viewContentBundleBtn != null))
		{
			return;
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			m_selectableBtn.spriteController.AddSubButton(m_viewContentBundleBtn.spriteController);
			m_viewContentBundleBtn.spriteController.callback = ViewContentClicked;
			return;
		}
	}

	public void Setup(UIPurchaseableItem item)
	{
		UIManager.SetGameObjectActive(m_ownedContainer, false);
		if (m_fluxCostText != null)
		{
			UIManager.SetGameObjectActive(m_fluxCostText, false);
		}
		HydrogenConfig hydrogenConfig = HydrogenConfig.Get();
		string accountCurrency = hydrogenConfig.Ticket.AccountCurrency;
		float num = 0f;
		float originalPrice = 0f;
		string text = null;
		if (m_lootMatrixIcon != null)
		{
			UIManager.SetGameObjectActive(m_lootMatrixIcon, false);
		}
		if (m_lootMatrixTextLayout != null)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			UIManager.SetGameObjectActive(m_lootMatrixTextLayout, false);
		}
		if (m_eventText != null)
		{
			UIManager.SetGameObjectActive(m_eventText, false);
		}
		if (m_viewContentBtn != null)
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
			UIManager.SetGameObjectActive(m_viewContentBtn, false);
		}
		if (m_viewContentBundleBtn != null)
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
			UIManager.SetGameObjectActive(m_viewContentBundleBtn, false);
		}
		if (m_iconFg != null)
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
			UIManager.SetGameObjectActive(m_iconFg, false);
		}
		m_item = item;
		if (item == null)
		{
			if (m_icon != null)
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
				UIManager.SetGameObjectActive(m_icon, false);
			}
			if (m_textLayout != null)
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
				UIManager.SetGameObjectActive(m_textLayout, false);
			}
			UIManager.SetGameObjectActive(m_discountContainer, false);
			m_selectableBtn.spriteController.SetClickable(false);
			return;
		}
		if (m_icon != null)
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
			UIManager.SetGameObjectActive(m_icon, true);
		}
		if (m_textLayout != null)
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
			UIManager.SetGameObjectActive(m_textLayout, true);
		}
		m_selectableBtn.spriteController.SetClickable(true);
		if (m_eventText != null)
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
			m_eventText.text = item.m_overlayText;
			UIManager.SetGameObjectActive(m_eventText, !item.m_overlayText.IsNullOrEmpty());
		}
		if (item.m_itemType == PurchaseItemType.GGBoost)
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
			originalPrice = item.m_ggPack.Prices.GetPrice(accountCurrency);
			num = CommerceClient.Get().GetGGPackPrice(item.m_ggPack.ProductCode, accountCurrency);
			m_nameText.text = string.Format(StringUtil.TR("PurchaseGGBoostsDesc", "Store"), item.m_ggPack.NumberOfBoosts);
			m_icon.sprite = item.m_ggPack.GGPackSprite;
			bool flag = true;
			int num2 = 0;
			while (true)
			{
				if (num2 < GameWideData.Get().m_ggPackData.m_ggPacks.Length)
				{
					if (GameWideData.Get().m_ggPackData.m_ggPacks[num2].SortOrder > item.m_ggPack.SortOrder)
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
						flag = false;
						break;
					}
					num2++;
					continue;
				}
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				break;
			}
			if (flag)
			{
				text = StringUtil.TR("BestValue", "Store");
			}
		}
		else if (item.m_itemType == PurchaseItemType.LootMatrixPack)
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
			LootMatrixPack lootMatrixPack = item.m_lootMatrixPack;
			bool flag2 = lootMatrixPack.IsInEvent();
			bool flag3 = false;
			bool isBundle = lootMatrixPack.IsBundle;
			originalPrice = lootMatrixPack.Prices.GetPrice(accountCurrency);
			num = CommerceClient.Get().GetLootMatrixPackPrice(lootMatrixPack.ProductCode, accountCurrency);
			int num3 = lootMatrixPack.NumberOfMatrixes;
			for (int i = 0; i < lootMatrixPack.BonusMatrixes.Length; i++)
			{
				if (lootMatrixPack.NumberOfMatrixes == 0)
				{
					num3 += lootMatrixPack.BonusMatrixes[i].NumberOfMatrixes;
				}
				if (!flag2)
				{
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
				if (flag3)
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
					break;
				}
				InventoryItemTemplate itemTemplate = InventoryWideData.Get().GetItemTemplate(lootMatrixPack.BonusMatrixes[i].LootMatrixId);
				if (itemTemplate.TypeSpecificData.Length > 1)
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
					flag3 = (itemTemplate.TypeSpecificData[1] == 1);
				}
			}
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			m_lootMatrixNameText.text = num3.ToString();
			m_eventText.text = lootMatrixPack.GetEventText();
			UIManager.SetGameObjectActive(m_eventText, flag2);
			if (m_nameText != null)
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
				m_nameText.text = lootMatrixPack.GetDescription();
			}
			UIManager.SetGameObjectActive(m_viewContentBtn, flag3);
			UIManager.SetGameObjectActive(m_viewContentBundleBtn, isBundle);
			Sprite sprite = (!flag2) ? lootMatrixPack.LootMatrixPackSprite : lootMatrixPack.EventPackSprite;
			m_lootMatrixIcon.sprite = sprite;
			if (m_icon != null)
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
				m_icon.sprite = sprite;
			}
			UIManager.SetGameObjectActive(m_lootMatrixIcon, !isBundle);
			if (m_icon != null)
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
				UIManager.SetGameObjectActive(m_icon, isBundle);
			}
			if (m_textLayout != null)
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
				UIManager.SetGameObjectActive(m_textLayout, isBundle);
			}
			if (m_lootMatrixTextLayout != null)
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
				UIManager.SetGameObjectActive(m_lootMatrixTextLayout, !isBundle);
			}
		}
		else if (item.m_itemType == PurchaseItemType.Character)
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
			originalPrice = item.m_charLink.Prices.GetPrice(accountCurrency);
			num = CommerceClient.Get().GetFreelancerPrice(item.m_charLink.m_characterType, accountCurrency);
			m_nameText.text = item.m_charLink.GetDisplayName();
			m_icon.sprite = Resources.Load<Sprite>(item.m_charLink.m_characterIconResourceString);
			bool unlocked = ClientGameManager.Get().GetPlayerCharacterData(item.m_charLink.m_characterType).CharacterComponent.Unlocked;
			UIManager.SetGameObjectActive(m_ownedContainer, unlocked);
			if (m_fluxCostText != null)
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
				int unlockFreelancerCurrencyPrice = item.m_charLink.m_charUnlockData.GetUnlockFreelancerCurrencyPrice();
				UIManager.SetGameObjectActive(m_fluxCostText, unlockFreelancerCurrencyPrice > 0);
				m_fluxCostText.text = "<sprite name=credit>" + UIStorePanel.FormatIntToString(unlockFreelancerCurrencyPrice, true);
			}
		}
		else if (item.m_itemType == PurchaseItemType.Game)
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
			num = CommerceClient.Get().GetGamePackPrice(item.m_gamePack.ProductCode, accountCurrency, out originalPrice);
			m_nameText.text = string.Format(StringUtil.TR("Edition", "Store"), item.m_gamePack.GetEditionName());
			m_icon.sprite = item.m_gamePack.Sprite;
		}
		else if (item.m_itemType == PurchaseItemType.Overcon)
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
			UIOverconData.NameToOverconEntry overconEntryById = UIOverconData.Get().GetOverconEntryById(item.m_overconID);
			num = 0f;
			originalPrice = 0f;
			m_nameText.text = overconEntryById.GetDisplayName();
			m_icon.sprite = Resources.Load<Sprite>(overconEntryById.m_iconSpritePath);
		}
		else if (item.m_itemType == PurchaseItemType.Tint)
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
			num = 0f;
			originalPrice = 0f;
			m_nameText.text = item.m_charLink.GetPatternColorName(item.m_skinIndex, item.m_textureIndex, item.m_tintIndex);
			m_icon.sprite = Resources.Load<Sprite>(item.m_charLink.m_skins[item.m_skinIndex].m_patterns[item.m_textureIndex].m_colors[item.m_tintIndex].m_iconResourceString);
		}
		else if (item.m_itemType == PurchaseItemType.Taunt)
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
			GameBalanceVars.TauntUnlockData tauntUnlockData = item.m_charLink.m_taunts[item.m_tauntIndex].m_tauntUnlockData;
			num = 0f;
			originalPrice = 0f;
			m_nameText.text = item.m_charLink.GetTauntName(item.m_tauntIndex);
			m_icon.sprite = Resources.Load<Sprite>(tauntUnlockData.GetSpritePath());
			UIManager.SetGameObjectActive(m_iconFg, true);
			m_iconFg.sprite = tauntUnlockData.GetItemFg();
		}
		else if (item.m_itemType == PurchaseItemType.InventoryItem)
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
			InventoryItemTemplate itemTemplate2 = InventoryWideData.Get().GetItemTemplate(item.m_inventoryTemplateId);
			num = CommerceClient.Get().GetStoreItemPrice(item.m_inventoryTemplateId, accountCurrency, out originalPrice);
			m_nameText.text = itemTemplate2.GetDisplayName();
			m_icon.sprite = Resources.Load<Sprite>(InventoryWideData.GetSpritePath(itemTemplate2));
			m_iconFg.sprite = InventoryWideData.GetItemFg(itemTemplate2);
			UIManager.SetGameObjectActive(m_iconFg, m_iconFg.sprite != null);
		}
		else if (item.m_itemType == PurchaseItemType.Banner)
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
			GameBalanceVars.PlayerBanner banner = GameBalanceVars.Get().GetBanner(item.m_bannerID);
			num = 0f;
			originalPrice = 0f;
			m_nameText.text = banner.GetBannerName();
			m_icon.sprite = Resources.Load<Sprite>(banner.m_iconResourceString);
		}
		else if (item.m_itemType == PurchaseItemType.Title)
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
			num = 0f;
			originalPrice = 0f;
			m_nameText.text = GameBalanceVars.Get().GetTitle(item.m_titleID, string.Empty);
			UIManager.SetGameObjectActive(m_icon, false);
		}
		else if (item.m_itemType == PurchaseItemType.Emoticon)
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
			GameBalanceVars.ChatEmoticon chatEmoticon = null;
			int num4 = 0;
			while (true)
			{
				if (num4 < GameBalanceVars.Get().ChatEmojis.Length)
				{
					if (GameBalanceVars.Get().ChatEmojis[num4].ID == item.m_emoticonID)
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
						chatEmoticon = GameBalanceVars.Get().ChatEmojis[num4];
						break;
					}
					num4++;
					continue;
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
				break;
			}
			num = 0f;
			originalPrice = 0f;
			m_nameText.text = chatEmoticon.GetEmojiName();
			m_icon.sprite = Resources.Load<Sprite>(chatEmoticon.GetSpritePath());
		}
		else if (item.m_itemType == PurchaseItemType.LoadingScreenBackground)
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
			GameBalanceVars.LoadingScreenBackground loadingScreenBackground = GameBalanceVars.Get().GetLoadingScreenBackground(item.m_loadingScreenBackgroundId);
			num = 0f;
			originalPrice = 0f;
			m_nameText.text = loadingScreenBackground.GetLoadingScreenBackgroundName();
			m_icon.sprite = Resources.Load<Sprite>(loadingScreenBackground.m_iconPath);
		}
		if (m_costText != null)
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
			if (num > 0f)
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
				m_costText.text = UIStorePanel.GetLocalizedPriceString(num, accountCurrency);
			}
			else
			{
				m_costText.text = string.Empty;
			}
		}
		if (m_lootMatrixCostText != null)
		{
			m_lootMatrixCostText.text = UIStorePanel.GetLocalizedPriceString(num, accountCurrency);
		}
		RectTransform discountContainer = m_discountContainer;
		int doActive;
		if (!(num < originalPrice))
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
			doActive = ((!text.IsNullOrEmpty()) ? 1 : 0);
		}
		else
		{
			doActive = 1;
		}
		UIManager.SetGameObjectActive(discountContainer, (byte)doActive != 0);
		if (num < originalPrice)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
				{
					int num5 = Mathf.RoundToInt((originalPrice - num) * 100f / originalPrice);
					m_discountedAmount.text = string.Format(StringUtil.TR("DiscountPercentOff", "Store"), num5);
					return;
				}
				}
			}
		}
		if (text.IsNullOrEmpty())
		{
			return;
		}
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			m_discountedAmount.text = text;
			return;
		}
	}

	private void Clicked(BaseEventData data)
	{
		if (m_item == null)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return;
				}
			}
		}
		if (m_item.m_itemType == PurchaseItemType.Character)
		{
			CharacterType characterType = m_item.m_charLink.m_characterType;
			UICashShopPanel uICashShopPanel = UICashShopPanel.Get();
			UICharacterBrowsersPanel characterBrowser = uICashShopPanel.m_characterBrowser;
			characterBrowser.Setup(characterType, characterBrowser.m_generalBrowserPanel);
			characterBrowser.SetVisible(true);
			uICashShopPanel.HideAllPanels();
			UIFrontEnd.PlaySound(FrontEndButtonSounds.Generic);
			return;
		}
		if (m_item.m_itemType != PurchaseItemType.Game)
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
			if (m_item.m_itemType != PurchaseItemType.GGBoost)
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
				if (m_item.m_itemType != PurchaseItemType.LootMatrixPack)
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
					if (m_item.m_itemType != PurchaseItemType.InventoryItem)
					{
						UIFrontEnd.Get().m_frontEndNavPanel.CollectionsBtnClicked(null);
						UIStorePanel.Get().SelectItem(m_item);
						UIFrontEnd.PlaySound(FrontEndButtonSounds.Generic);
						return;
					}
				}
			}
		}
		UIStorePanel.Get().OpenPurchaseDialog(m_item);
		m_selectableBtn.spriteController.ResetMouseState();
		UIFrontEnd.PlaySound(FrontEndButtonSounds.StorePurchased);
	}

	private void ViewContentClicked(BaseEventData data)
	{
		if (m_item == null)
		{
			return;
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_item.m_itemType != PurchaseItemType.LootMatrixPack)
			{
				return;
			}
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				List<InventoryItemTemplate> list = null;
				if (!m_item.m_lootMatrixPack.IsInEvent())
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
					if (!m_item.m_lootMatrixPack.IsBundle)
					{
						goto IL_0089;
					}
					while (true)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
				}
				list = InventoryWideData.GetTemplatesFromLootMatrixPack(m_item.m_lootMatrixPack);
				goto IL_0089;
				IL_0089:
				List<InventoryItemTemplate> list2 = list;
				if (_003C_003Ef__am_0024cache0 == null)
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
					_003C_003Ef__am_0024cache0 = ((InventoryItemTemplate x) => x.TypeSpecificData.Length <= 1 || x.TypeSpecificData[1] != 1);
				}
				list2.RemoveAll(_003C_003Ef__am_0024cache0);
				if (list == null)
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
					if (list.Count > 0)
					{
						while (true)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							UILootMatrixContentViewer.Get().Setup(list.ToArray(), m_item.m_lootMatrixPack.IsBundle);
							UILootMatrixContentViewer.Get().SetVisible(true);
							return;
						}
					}
					return;
				}
			}
		}
	}
}

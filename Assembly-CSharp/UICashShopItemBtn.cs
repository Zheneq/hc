using System;
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
		this.m_selectableBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.Clicked);
		if (this.m_viewContentBtn != null)
		{
			this.m_selectableBtn.spriteController.AddSubButton(this.m_viewContentBtn.spriteController);
			this.m_viewContentBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.ViewContentClicked);
		}
		if (this.m_viewContentBundleBtn != null)
		{
			this.m_selectableBtn.spriteController.AddSubButton(this.m_viewContentBundleBtn.spriteController);
			this.m_viewContentBundleBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.ViewContentClicked);
		}
	}

	public void Setup(UIPurchaseableItem item)
	{
		UIManager.SetGameObjectActive(this.m_ownedContainer, false, null);
		if (this.m_fluxCostText != null)
		{
			UIManager.SetGameObjectActive(this.m_fluxCostText, false, null);
		}
		HydrogenConfig hydrogenConfig = HydrogenConfig.Get();
		string accountCurrency = hydrogenConfig.Ticket.AccountCurrency;
		float num = 0f;
		float num2 = 0f;
		string text = null;
		if (this.m_lootMatrixIcon != null)
		{
			UIManager.SetGameObjectActive(this.m_lootMatrixIcon, false, null);
		}
		if (this.m_lootMatrixTextLayout != null)
		{
			UIManager.SetGameObjectActive(this.m_lootMatrixTextLayout, false, null);
		}
		if (this.m_eventText != null)
		{
			UIManager.SetGameObjectActive(this.m_eventText, false, null);
		}
		if (this.m_viewContentBtn != null)
		{
			UIManager.SetGameObjectActive(this.m_viewContentBtn, false, null);
		}
		if (this.m_viewContentBundleBtn != null)
		{
			UIManager.SetGameObjectActive(this.m_viewContentBundleBtn, false, null);
		}
		if (this.m_iconFg != null)
		{
			UIManager.SetGameObjectActive(this.m_iconFg, false, null);
		}
		this.m_item = item;
		if (item == null)
		{
			if (this.m_icon != null)
			{
				UIManager.SetGameObjectActive(this.m_icon, false, null);
			}
			if (this.m_textLayout != null)
			{
				UIManager.SetGameObjectActive(this.m_textLayout, false, null);
			}
			UIManager.SetGameObjectActive(this.m_discountContainer, false, null);
			this.m_selectableBtn.spriteController.SetClickable(false);
			return;
		}
		if (this.m_icon != null)
		{
			UIManager.SetGameObjectActive(this.m_icon, true, null);
		}
		if (this.m_textLayout != null)
		{
			UIManager.SetGameObjectActive(this.m_textLayout, true, null);
		}
		this.m_selectableBtn.spriteController.SetClickable(true);
		if (this.m_eventText != null)
		{
			this.m_eventText.text = item.m_overlayText;
			UIManager.SetGameObjectActive(this.m_eventText, !item.m_overlayText.IsNullOrEmpty(), null);
		}
		if (item.m_itemType == PurchaseItemType.GGBoost)
		{
			num2 = item.m_ggPack.Prices.GetPrice(accountCurrency);
			num = CommerceClient.Get().GetGGPackPrice(item.m_ggPack.ProductCode, accountCurrency);
			this.m_nameText.text = string.Format(StringUtil.TR("PurchaseGGBoostsDesc", "Store"), item.m_ggPack.NumberOfBoosts);
			this.m_icon.sprite = item.m_ggPack.GGPackSprite;
			bool flag = true;
			for (int i = 0; i < GameWideData.Get().m_ggPackData.m_ggPacks.Length; i++)
			{
				if (GameWideData.Get().m_ggPackData.m_ggPacks[i].SortOrder > item.m_ggPack.SortOrder)
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				text = StringUtil.TR("BestValue", "Store");
			}
		}
		else if (item.m_itemType == PurchaseItemType.LootMatrixPack)
		{
			LootMatrixPack lootMatrixPack = item.m_lootMatrixPack;
			bool flag2 = lootMatrixPack.IsInEvent();
			bool flag3 = false;
			bool isBundle = lootMatrixPack.IsBundle;
			num2 = lootMatrixPack.Prices.GetPrice(accountCurrency);
			num = CommerceClient.Get().GetLootMatrixPackPrice(lootMatrixPack.ProductCode, accountCurrency);
			int num3 = lootMatrixPack.NumberOfMatrixes;
			for (int j = 0; j < lootMatrixPack.BonusMatrixes.Length; j++)
			{
				if (lootMatrixPack.NumberOfMatrixes == 0)
				{
					num3 += lootMatrixPack.BonusMatrixes[j].NumberOfMatrixes;
				}
				if (flag2)
				{
					if (!flag3)
					{
						InventoryItemTemplate itemTemplate = InventoryWideData.Get().GetItemTemplate(lootMatrixPack.BonusMatrixes[j].LootMatrixId);
						if (itemTemplate.TypeSpecificData.Length > 1)
						{
							flag3 = (itemTemplate.TypeSpecificData[1] == 1);
						}
					}
				}
			}
			this.m_lootMatrixNameText.text = num3.ToString();
			this.m_eventText.text = lootMatrixPack.GetEventText();
			UIManager.SetGameObjectActive(this.m_eventText, flag2, null);
			if (this.m_nameText != null)
			{
				this.m_nameText.text = lootMatrixPack.GetDescription();
			}
			UIManager.SetGameObjectActive(this.m_viewContentBtn, flag3, null);
			UIManager.SetGameObjectActive(this.m_viewContentBundleBtn, isBundle, null);
			Sprite sprite = (!flag2) ? lootMatrixPack.LootMatrixPackSprite : lootMatrixPack.EventPackSprite;
			this.m_lootMatrixIcon.sprite = sprite;
			if (this.m_icon != null)
			{
				this.m_icon.sprite = sprite;
			}
			UIManager.SetGameObjectActive(this.m_lootMatrixIcon, !isBundle, null);
			if (this.m_icon != null)
			{
				UIManager.SetGameObjectActive(this.m_icon, isBundle, null);
			}
			if (this.m_textLayout != null)
			{
				UIManager.SetGameObjectActive(this.m_textLayout, isBundle, null);
			}
			if (this.m_lootMatrixTextLayout != null)
			{
				UIManager.SetGameObjectActive(this.m_lootMatrixTextLayout, !isBundle, null);
			}
		}
		else if (item.m_itemType == PurchaseItemType.Character)
		{
			num2 = item.m_charLink.Prices.GetPrice(accountCurrency);
			num = CommerceClient.Get().GetFreelancerPrice(item.m_charLink.m_characterType, accountCurrency);
			this.m_nameText.text = item.m_charLink.GetDisplayName();
			this.m_icon.sprite = Resources.Load<Sprite>(item.m_charLink.m_characterIconResourceString);
			bool unlocked = ClientGameManager.Get().GetPlayerCharacterData(item.m_charLink.m_characterType).CharacterComponent.Unlocked;
			UIManager.SetGameObjectActive(this.m_ownedContainer, unlocked, null);
			if (this.m_fluxCostText != null)
			{
				int unlockFreelancerCurrencyPrice = item.m_charLink.m_charUnlockData.GetUnlockFreelancerCurrencyPrice();
				UIManager.SetGameObjectActive(this.m_fluxCostText, unlockFreelancerCurrencyPrice > 0, null);
				this.m_fluxCostText.text = "<sprite name=credit>" + UIStorePanel.FormatIntToString(unlockFreelancerCurrencyPrice, true);
			}
		}
		else if (item.m_itemType == PurchaseItemType.Game)
		{
			num = CommerceClient.Get().GetGamePackPrice(item.m_gamePack.ProductCode, accountCurrency, out num2);
			this.m_nameText.text = string.Format(StringUtil.TR("Edition", "Store"), item.m_gamePack.GetEditionName());
			this.m_icon.sprite = item.m_gamePack.Sprite;
		}
		else if (item.m_itemType == PurchaseItemType.Overcon)
		{
			UIOverconData.NameToOverconEntry overconEntryById = UIOverconData.Get().GetOverconEntryById(item.m_overconID);
			num = 0f;
			num2 = 0f;
			this.m_nameText.text = overconEntryById.GetDisplayName();
			this.m_icon.sprite = Resources.Load<Sprite>(overconEntryById.m_iconSpritePath);
		}
		else if (item.m_itemType == PurchaseItemType.Tint)
		{
			num = 0f;
			num2 = 0f;
			this.m_nameText.text = item.m_charLink.GetPatternColorName(item.m_skinIndex, item.m_textureIndex, item.m_tintIndex);
			this.m_icon.sprite = Resources.Load<Sprite>(item.m_charLink.m_skins[item.m_skinIndex].m_patterns[item.m_textureIndex].m_colors[item.m_tintIndex].m_iconResourceString);
		}
		else if (item.m_itemType == PurchaseItemType.Taunt)
		{
			GameBalanceVars.TauntUnlockData tauntUnlockData = item.m_charLink.m_taunts[item.m_tauntIndex].m_tauntUnlockData;
			num = 0f;
			num2 = 0f;
			this.m_nameText.text = item.m_charLink.GetTauntName(item.m_tauntIndex);
			this.m_icon.sprite = Resources.Load<Sprite>(tauntUnlockData.GetSpritePath());
			UIManager.SetGameObjectActive(this.m_iconFg, true, null);
			this.m_iconFg.sprite = tauntUnlockData.GetItemFg();
		}
		else if (item.m_itemType == PurchaseItemType.InventoryItem)
		{
			InventoryItemTemplate itemTemplate2 = InventoryWideData.Get().GetItemTemplate(item.m_inventoryTemplateId);
			num = CommerceClient.Get().GetStoreItemPrice(item.m_inventoryTemplateId, accountCurrency, out num2);
			this.m_nameText.text = itemTemplate2.GetDisplayName();
			this.m_icon.sprite = Resources.Load<Sprite>(InventoryWideData.GetSpritePath(itemTemplate2));
			this.m_iconFg.sprite = InventoryWideData.GetItemFg(itemTemplate2);
			UIManager.SetGameObjectActive(this.m_iconFg, this.m_iconFg.sprite != null, null);
		}
		else if (item.m_itemType == PurchaseItemType.Banner)
		{
			GameBalanceVars.PlayerBanner banner = GameBalanceVars.Get().GetBanner(item.m_bannerID);
			num = 0f;
			num2 = 0f;
			this.m_nameText.text = banner.GetBannerName();
			this.m_icon.sprite = Resources.Load<Sprite>(banner.m_iconResourceString);
		}
		else if (item.m_itemType == PurchaseItemType.Title)
		{
			num = 0f;
			num2 = 0f;
			this.m_nameText.text = GameBalanceVars.Get().GetTitle(item.m_titleID, string.Empty, -1);
			UIManager.SetGameObjectActive(this.m_icon, false, null);
		}
		else if (item.m_itemType == PurchaseItemType.Emoticon)
		{
			GameBalanceVars.ChatEmoticon chatEmoticon = null;
			for (int k = 0; k < GameBalanceVars.Get().ChatEmojis.Length; k++)
			{
				if (GameBalanceVars.Get().ChatEmojis[k].ID == item.m_emoticonID)
				{
					chatEmoticon = GameBalanceVars.Get().ChatEmojis[k];
					break;
				}
			}
			num = 0f;
			num2 = 0f;
			this.m_nameText.text = chatEmoticon.GetEmojiName();
			this.m_icon.sprite = Resources.Load<Sprite>(chatEmoticon.GetSpritePath());
		}
		else if (item.m_itemType == PurchaseItemType.LoadingScreenBackground)
		{
			GameBalanceVars.LoadingScreenBackground loadingScreenBackground = GameBalanceVars.Get().GetLoadingScreenBackground(item.m_loadingScreenBackgroundId);
			num = 0f;
			num2 = 0f;
			this.m_nameText.text = loadingScreenBackground.GetLoadingScreenBackgroundName();
			this.m_icon.sprite = Resources.Load<Sprite>(loadingScreenBackground.m_iconPath);
		}
		if (this.m_costText != null)
		{
			if (num > 0f)
			{
				this.m_costText.text = UIStorePanel.GetLocalizedPriceString(num, accountCurrency);
			}
			else
			{
				this.m_costText.text = string.Empty;
			}
		}
		if (this.m_lootMatrixCostText != null)
		{
			this.m_lootMatrixCostText.text = UIStorePanel.GetLocalizedPriceString(num, accountCurrency);
		}
		Component discountContainer = this.m_discountContainer;
		bool doActive;
		if (num >= num2)
		{
			doActive = !text.IsNullOrEmpty();
		}
		else
		{
			doActive = true;
		}
		UIManager.SetGameObjectActive(discountContainer, doActive, null);
		if (num < num2)
		{
			int num4 = Mathf.RoundToInt((num2 - num) * 100f / num2);
			this.m_discountedAmount.text = string.Format(StringUtil.TR("DiscountPercentOff", "Store"), num4);
		}
		else if (!text.IsNullOrEmpty())
		{
			this.m_discountedAmount.text = text;
		}
	}

	private void Clicked(BaseEventData data)
	{
		if (this.m_item == null)
		{
			return;
		}
		if (this.m_item.m_itemType == PurchaseItemType.Character)
		{
			CharacterType characterType = this.m_item.m_charLink.m_characterType;
			UICashShopPanel uicashShopPanel = UICashShopPanel.Get();
			UICharacterBrowsersPanel characterBrowser = uicashShopPanel.m_characterBrowser;
			characterBrowser.Setup(characterType, characterBrowser.m_generalBrowserPanel);
			characterBrowser.SetVisible(true);
			uicashShopPanel.HideAllPanels();
			UIFrontEnd.PlaySound(FrontEndButtonSounds.Generic);
		}
		else
		{
			if (this.m_item.m_itemType != PurchaseItemType.Game)
			{
				if (this.m_item.m_itemType != PurchaseItemType.GGBoost)
				{
					if (this.m_item.m_itemType != PurchaseItemType.LootMatrixPack)
					{
						if (this.m_item.m_itemType != PurchaseItemType.InventoryItem)
						{
							UIFrontEnd.Get().m_frontEndNavPanel.CollectionsBtnClicked(null);
							UIStorePanel.Get().SelectItem(this.m_item);
							UIFrontEnd.PlaySound(FrontEndButtonSounds.Generic);
							return;
						}
					}
				}
			}
			UIStorePanel.Get().OpenPurchaseDialog(this.m_item);
			this.m_selectableBtn.spriteController.ResetMouseState();
			UIFrontEnd.PlaySound(FrontEndButtonSounds.StorePurchased);
		}
	}

	private void ViewContentClicked(BaseEventData data)
	{
		if (this.m_item != null)
		{
			if (this.m_item.m_itemType == PurchaseItemType.LootMatrixPack)
			{
				List<InventoryItemTemplate> list = null;
				if (!this.m_item.m_lootMatrixPack.IsInEvent())
				{
					if (!this.m_item.m_lootMatrixPack.IsBundle)
					{
						goto IL_89;
					}
				}
				list = InventoryWideData.GetTemplatesFromLootMatrixPack(this.m_item.m_lootMatrixPack);
				IL_89:
				List<InventoryItemTemplate> list2 = list;
				
				list2.RemoveAll(((InventoryItemTemplate x) => x.TypeSpecificData.Length <= 1 || x.TypeSpecificData[1] != 1));
				if (list != null)
				{
					if (list.Count > 0)
					{
						UILootMatrixContentViewer.Get().Setup(list.ToArray(), this.m_item.m_lootMatrixPack.IsBundle);
						UILootMatrixContentViewer.Get().SetVisible(true);
					}
				}
			}
		}
	}
}

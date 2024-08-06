using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIStorePurchaseItemDialogBox : UIDialogBox
{
	public TextMeshProUGUI m_dialogTitle;

	public _ButtonSwapSprite m_closeBtn;

	public Image m_disableCloseBtn;

	public TextMeshProUGUI m_descriptionTitle;

	public TextMeshProUGUI m_descriptionCurrentCredits;

	public TextMeshProUGUI m_descriptionAddCredits;

	public TextMeshProUGUI m_descriptionTotalCredits;

	public TextMeshProUGUI m_descriptionCostResults;

	public TextMeshProUGUI m_confirmPurchaseLabel;

	public TextMeshProUGUI m_purchaseResultLabel;

	public _ButtonSwapSprite m_purchaseResultBtn;

	public RectTransform m_confirmPurchaseContainer;

	public RectTransform m_waitingPurchaseContainer;

	public RectTransform m_resultPurchaseContainer;

	public RectTransform m_generalBtnContainer;

	public UIStorePurchaseBaseItem m_Item;

	public Image m_generalBtnDisabledImage;

	public _ButtonSwapSprite m_generalBtn;

	public TextMeshProUGUI[] m_generalBtnLabels;

	public _ButtonSwapSprite m_confirmBtn;

	public _ButtonSwapSprite m_cancelBtn;

	public Sprite m_tauntSprite;

	private const float MAX_PURCHASE_WAIT_TIME = 5f;

	private bool m_success;

	private UIPurchaseableItem m_itemRef;

	private float m_startPurchaseTime;

	private bool m_purchaseSuccess;

	private UIStorePurchaseItemDialogBox.PurchaseCloseDialogCallback m_closeDialogCallback;

	public override void Awake()
	{
		base.Awake();
		this.m_startPurchaseTime = -1f;
		this.m_closeBtn.callback = new _ButtonSwapSprite.ButtonClickCallback(this.CloseDialog);
		this.m_cancelBtn.callback = new _ButtonSwapSprite.ButtonClickCallback(this.CloseDialog);
		this.m_confirmBtn.callback = new _ButtonSwapSprite.ButtonClickCallback(this.PurchaseItem);
		this.m_generalBtn.callback = new _ButtonSwapSprite.ButtonClickCallback(this.GeneralButtonClicked);
		this.m_purchaseResultBtn.callback = new _ButtonSwapSprite.ButtonClickCallback(this.PurchaseResultOkClicked);
	}

	private void TimeoutPurchase()
	{
		this.m_purchaseResultLabel.text = StringUtil.TR("PurchaseHasTimedOut", "Store");
		this.m_purchaseSuccess = false;
		UIManager.SetGameObjectActive(this.m_waitingPurchaseContainer, false, null);
		UIManager.SetGameObjectActive(this.m_confirmPurchaseContainer, false, null);
		UIManager.SetGameObjectActive(this.m_resultPurchaseContainer, true, null);
		UIManager.SetGameObjectActive(this.m_generalBtnContainer, false, null);
		UIManager.SetGameObjectActive(this.m_disableCloseBtn, false, null);
	}

	public void PurchaseItem(BaseEventData data)
	{
		if (this.m_itemRef.m_itemType == PurchaseItemType.Character)
		{
			UIStorePanel.Get().RequestToPurchaseCharacter(this.m_itemRef.m_currencyType, this.m_itemRef.m_charLink.m_characterType);
		}
		else if (this.m_itemRef.m_itemType == PurchaseItemType.Skin)
		{
			UIStorePanel.Get().RequestToPurchaseSkin(this.m_itemRef.m_currencyType, this.m_itemRef.m_charLink.m_characterType, this.m_itemRef.m_skinIndex);
		}
		else if (this.m_itemRef.m_itemType == PurchaseItemType.Texture)
		{
			UIStorePanel.Get().RequestToPurchaseTexture(this.m_itemRef.m_currencyType, this.m_itemRef.m_charLink.m_characterType, this.m_itemRef.m_skinIndex, this.m_itemRef.m_textureIndex);
		}
		else if (this.m_itemRef.m_itemType == PurchaseItemType.Tint)
		{
			UIStorePanel.Get().RequestToPurchaseTint(this.m_itemRef.m_currencyType, this.m_itemRef.m_charLink.m_characterType, this.m_itemRef.m_skinIndex, this.m_itemRef.m_textureIndex, this.m_itemRef.m_tintIndex);
		}
		else if (this.m_itemRef.m_itemType == PurchaseItemType.Taunt)
		{
			UIStorePanel.Get().RequestToPurchaseTaunt(this.m_itemRef.m_currencyType, this.m_itemRef.m_charLink.m_characterType, this.m_itemRef.m_tauntIndex);
		}
		else if (this.m_itemRef.m_itemType == PurchaseItemType.InventoryItem)
		{
			UIStorePanel.Get().RequestToPurchaseInventoryItem(this.m_itemRef.m_inventoryTemplateId, this.m_itemRef.m_currencyType);
		}
		else if (this.m_itemRef.m_itemType == PurchaseItemType.Title)
		{
			UIStorePanel.Get().RequestToPurchaseTitle(this.m_itemRef.m_titleID, this.m_itemRef.m_currencyType);
		}
		else if (this.m_itemRef.m_itemType == PurchaseItemType.Banner)
		{
			if (GameBalanceVars.Get().GetBanner(this.m_itemRef.m_bannerID).m_type == GameBalanceVars.PlayerBanner.BannerType.Background)
			{
				UIStorePanel.Get().RequestToPurchaseBanner(this.m_itemRef.m_bannerID, this.m_itemRef.m_currencyType);
			}
			else
			{
				UIStorePanel.Get().RequestToPurchaseEmblem(this.m_itemRef.m_bannerID, this.m_itemRef.m_currencyType);
			}
		}
		else if (this.m_itemRef.m_itemType == PurchaseItemType.Emoticon)
		{
			UIStorePanel.Get().RequestToPurchaseEmoticon(this.m_itemRef.m_emoticonID, this.m_itemRef.m_currencyType);
		}
		else if (this.m_itemRef.m_itemType == PurchaseItemType.AbilityVfx)
		{
			UIStorePanel.Get().RequestToPurchaseAbilityVfx(this.m_itemRef.m_charLink.m_characterType, this.m_itemRef.m_abilityID, this.m_itemRef.m_abilityVfxID, this.m_itemRef.m_currencyType);
		}
		else if (this.m_itemRef.m_itemType == PurchaseItemType.Overcon)
		{
			UIStorePanel.Get().RequestToPurchaseOvercon(this.m_itemRef.m_overconID, this.m_itemRef.m_currencyType);
		}
		else if (this.m_itemRef.m_itemType == PurchaseItemType.LoadingScreenBackground)
		{
			UIStorePanel.Get().RequestToPurchaseLoadingScreenBackground(this.m_itemRef.m_loadingScreenBackgroundId, this.m_itemRef.m_currencyType);
		}
		this.m_startPurchaseTime = Time.time;
		UIManager.SetGameObjectActive(this.m_waitingPurchaseContainer, true, null);
		UIManager.SetGameObjectActive(this.m_confirmPurchaseContainer, false, null);
		UIManager.SetGameObjectActive(this.m_resultPurchaseContainer, false, null);
		UIManager.SetGameObjectActive(this.m_generalBtnContainer, false, null);
		UIManager.SetGameObjectActive(this.m_disableCloseBtn, true, null);
		this.m_purchaseSuccess = false;
	}

	private void SetGeneralButtonLabels(string newString)
	{
		for (int i = 0; i < this.m_generalBtnLabels.Length; i++)
		{
			this.m_generalBtnLabels[i].text = newString;
		}
	}

	public void NotifyPurchaseResponse(bool successful)
	{
		if (successful)
		{
			UIFrontEnd.PlaySound(FrontEndButtonSounds.PurchaseComplete);
			this.m_purchaseResultLabel.text = StringUtil.TR("PurchaseSuccessful", "Store");
			this.m_purchaseSuccess = true;
			this.Close();
		}
		else
		{
			this.m_purchaseResultLabel.text = StringUtil.TR("PurchaseFailed", "Store");
			this.m_purchaseSuccess = false;
		}
		UIManager.SetGameObjectActive(this.m_waitingPurchaseContainer, false, null);
		UIManager.SetGameObjectActive(this.m_confirmPurchaseContainer, false, null);
		UIManager.SetGameObjectActive(this.m_resultPurchaseContainer, true, null);
		UIManager.SetGameObjectActive(this.m_generalBtnContainer, false, null);
		UIManager.SetGameObjectActive(this.m_disableCloseBtn, false, null);
	}

	public void NotifyPurchaseInventoryItemResponse(bool successful)
	{
		this.NotifyPurchaseResponse(successful);
	}

	public void NotifyTauntPurchaseResponse(bool successful)
	{
		this.NotifyPurchaseResponse(successful);
		if (successful)
		{
			UICharacterTauntsPanel.RefreshActivePanels(this.m_itemRef.m_charLink, this.m_itemRef.m_tauntIndex);
		}
	}

	public void GeneralButtonClicked(BaseEventData data)
	{
		if (this.m_success)
		{
			UIFrontEnd.PlaySound(FrontEndButtonSounds.Cancel);
			this.m_confirmPurchaseLabel.text = StringUtil.TR("PurchaseConfirmation", "Store");
			UIManager.SetGameObjectActive(this.m_confirmPurchaseContainer, true, null);
			UIManager.SetGameObjectActive(this.m_generalBtnContainer, false, null);
			this.PurchaseItem(null);
		}
		else
		{
			this.Close();
			UIFrontEnd.Get().m_frontEndNavPanel.CollectionsBtnClicked(null);
			UIFrontEnd.PlaySound(FrontEndButtonSounds.SubMenuOpen);
		}
	}

	public void CloseDialog(BaseEventData data)
	{
		this.Close();
	}

	public void PurchaseResultOkClicked(BaseEventData data)
	{
		if (this.m_purchaseSuccess)
		{
			this.Close();
		}
		else
		{
			UIManager.SetGameObjectActive(this.m_resultPurchaseContainer, false, null);
			UIManager.SetGameObjectActive(this.m_generalBtnContainer, true, null);
		}
	}

	public override void ClearCallback()
	{
	}

	protected override void CloseCallback()
	{
	}

	public void Update()
	{
		if (UIStorePanel.Get().IsWaitingForPurchaseRequest)
		{
			if (Time.time - this.m_startPurchaseTime >= 5f)
			{
				this.TimeoutPurchase();
				UIStorePanel.Get().TimeOutPurchase();
			}
		}
	}

	public override void Close()
	{
		if (this.m_closeDialogCallback != null)
		{
			this.m_closeDialogCallback();
			this.m_closeDialogCallback = null;
		}
		base.Close();
	}

	public void Setup(UIPurchaseableItem item, UIStorePurchaseItemDialogBox.PurchaseCloseDialogCallback closeCallback = null)
	{
		this.m_itemRef = item;
		this.m_closeDialogCallback = closeCallback;
		string text = string.Empty;
		string text2 = string.Empty;
		int num = 0;
		int skinIndex = item.m_skinIndex;
		int textureIndex = item.m_textureIndex;
		int tintIndex = item.m_tintIndex;
		int tauntIndex = item.m_tauntIndex;
		GameBalanceVars.CharacterUnlockData characterUnlockData = null;
		if (item.m_charLink != null)
		{
			characterUnlockData = GameBalanceVars.Get().GetCharacterUnlockData(item.m_charLink.m_characterType);
		}
		UIManager.SetGameObjectActive(this.m_Item.m_itemFG, false, null);
		if (item.m_itemType == PurchaseItemType.Character)
		{
			text2 = StringUtil.TR("PurchaseFreelancer", "Store");
			text = string.Format(StringUtil.TR("PurchaseFreelancerDesc", "Store"), item.m_charLink.GetDisplayName());
			this.m_Item.m_itemIcon.sprite = Resources.Load<Sprite>(item.m_charLink.m_characterIconResourceString);
			if (item.m_currencyType == CurrencyType.FreelancerCurrency)
			{
				num = characterUnlockData.GetUnlockFreelancerCurrencyPrice();
			}
		}
		else if (item.m_itemType == PurchaseItemType.Skin)
		{
			CharacterSkin characterSkin = item.m_charLink.m_skins[skinIndex];
			text2 = StringUtil.TR("PurchaseSkin", "Store");
			text = string.Format(StringUtil.TR("PurchaseSkinDesc", "Store"), item.m_charLink.GetSkinName(skinIndex));
			this.m_Item.m_itemIcon.sprite = (Sprite)Resources.Load(characterSkin.m_skinSelectionIconPath, typeof(Sprite));
			if (item.m_currencyType == CurrencyType.ISO)
			{
				num = characterUnlockData.skinUnlockData[skinIndex].GetUnlockISOPrice();
			}
			else if (item.m_currencyType == CurrencyType.RankedCurrency)
			{
				num = characterUnlockData.skinUnlockData[skinIndex].GetUnlockRankedCurrencyPrice();
			}
		}
		else if (item.m_itemType == PurchaseItemType.Texture)
		{
			text2 = StringUtil.TR("PurchaseTexture", "Store");
			text = item.m_charLink.GetPatternName(skinIndex, textureIndex);
			if (item.m_currencyType == CurrencyType.ISO)
			{
				num = characterUnlockData.skinUnlockData[skinIndex].patternUnlockData[textureIndex].GetUnlockISOPrice();
			}
			else if (item.m_currencyType == CurrencyType.RankedCurrency)
			{
				num = characterUnlockData.skinUnlockData[skinIndex].patternUnlockData[textureIndex].GetUnlockRankedCurrencyPrice();
			}
		}
		else if (item.m_itemType == PurchaseItemType.Tint)
		{
			CharacterColor characterColor = item.m_charLink.m_skins[skinIndex].m_patterns[textureIndex].m_colors[tintIndex];
			text2 = StringUtil.TR("PurchaseStyle", "Store");
			text = string.Format(StringUtil.TR("PurchaseStyleDesc", "Store"), item.m_charLink.GetPatternColorName(skinIndex, textureIndex, tintIndex));
			this.m_Item.m_itemIcon.sprite = (Sprite)Resources.Load(characterColor.m_iconResourceString, typeof(Sprite));
			if (item.m_currencyType == CurrencyType.ISO)
			{
				num = characterUnlockData.skinUnlockData[skinIndex].patternUnlockData[textureIndex].colorUnlockData[tintIndex].GetUnlockISOPrice();
			}
			else if (item.m_currencyType == CurrencyType.RankedCurrency)
			{
				num = characterUnlockData.skinUnlockData[skinIndex].patternUnlockData[textureIndex].colorUnlockData[tintIndex].GetUnlockRankedCurrencyPrice();
			}
			else if (item.m_currencyType == CurrencyType.FreelancerCurrency)
			{
				num = characterUnlockData.skinUnlockData[skinIndex].patternUnlockData[textureIndex].colorUnlockData[tintIndex].GetUnlockFreelancerCurrencyPrice();
			}
		}
		else if (item.m_itemType == PurchaseItemType.Taunt)
		{
			text2 = StringUtil.TR("PurchaseTaunt", "Store");
			text = item.m_charLink.GetTauntName(tauntIndex);
			this.m_Item.m_itemIcon.sprite = this.m_tauntSprite;
			UIManager.SetGameObjectActive(this.m_Item.m_itemFG, true, null);
			AbilityData component = item.m_charLink.ActorDataPrefab.GetComponent<AbilityData>();
			if (item.m_charLink.m_taunts[tauntIndex].m_actionForTaunt == AbilityData.ActionType.ABILITY_0)
			{
				this.m_Item.m_itemFG.sprite = component.m_sprite0;
			}
			else if (item.m_charLink.m_taunts[tauntIndex].m_actionForTaunt == AbilityData.ActionType.ABILITY_1)
			{
				this.m_Item.m_itemFG.sprite = component.m_sprite1;
			}
			else if (item.m_charLink.m_taunts[tauntIndex].m_actionForTaunt == AbilityData.ActionType.ABILITY_2)
			{
				this.m_Item.m_itemFG.sprite = component.m_sprite2;
			}
			else if (item.m_charLink.m_taunts[tauntIndex].m_actionForTaunt == AbilityData.ActionType.ABILITY_3)
			{
				this.m_Item.m_itemFG.sprite = component.m_sprite3;
			}
			else if (item.m_charLink.m_taunts[tauntIndex].m_actionForTaunt == AbilityData.ActionType.ABILITY_4)
			{
				this.m_Item.m_itemFG.sprite = component.m_sprite4;
			}
			else if (item.m_charLink.m_taunts[tauntIndex].m_actionForTaunt == AbilityData.ActionType.ABILITY_5)
			{
				this.m_Item.m_itemFG.sprite = component.m_sprite5;
			}
			else if (item.m_charLink.m_taunts[tauntIndex].m_actionForTaunt == AbilityData.ActionType.ABILITY_6)
			{
				this.m_Item.m_itemFG.sprite = component.m_sprite6;
			}
			if (item.m_currencyType == CurrencyType.ISO)
			{
				num = characterUnlockData.tauntUnlockData[tauntIndex].GetUnlockISOPrice();
			}
			else if (item.m_currencyType == CurrencyType.RankedCurrency)
			{
				num = characterUnlockData.tauntUnlockData[tauntIndex].GetUnlockRankedCurrencyPrice();
			}
		}
		else if (item.m_itemType == PurchaseItemType.InventoryItem)
		{
			InventoryItemTemplate itemTemplate = InventoryWideData.Get().GetItemTemplate(item.m_inventoryTemplateId);
			text2 = string.Format(StringUtil.TR("PurchaseItem", "Store"), itemTemplate.GetDisplayName());
			text = string.Format(StringUtil.TR("PurchaseItemDesc", "Store"), itemTemplate.GetDisplayName());
			if (item.m_currencyType == CurrencyType.ISO)
			{
				num = CommerceClient.Get().GetISOPriceForInventoryItem(itemTemplate.Index);
			}
			else if (item.m_currencyType == CurrencyType.FreelancerCurrency)
			{
				GameBalanceVars gameBalanceVars = GameBalanceVars.Get();
				for (int i = 0; i < gameBalanceVars.StoreItemsForPurchase.Length; i++)
				{
					if (gameBalanceVars.StoreItemsForPurchase[i].m_itemTemplateId == item.m_inventoryTemplateId)
					{
						num = gameBalanceVars.StoreItemsForPurchase[i].GetUnlockFreelancerCurrencyPrice();
						goto IL_727;
					}
				}
			}
			IL_727:
			string spritePath = InventoryWideData.GetSpritePath(itemTemplate);
			if (!spritePath.IsNullOrEmpty())
			{
				this.m_Item.m_itemIcon.sprite = (Sprite)Resources.Load(spritePath, typeof(Sprite));
			}
			else
			{
				UIManager.SetGameObjectActive(this.m_Item.m_itemIcon, false, null);
			}
			UIManager.SetGameObjectActive(this.m_Item.m_itemFG, false, null);
		}
		else if (item.m_itemType == PurchaseItemType.Title)
		{
			GameBalanceVars gameBalanceVars2 = GameBalanceVars.Get();
#if !VANILLA && !SERVER
            string title = gameBalanceVars2.GetTitle(item.m_titleID, ClientGameManager.Get().Handle, string.Empty, -1);
#else 
			string title = gameBalanceVars2.GetTitle(item.m_titleID, string.Empty, -1);
#endif
			text2 = string.Format(StringUtil.TR("PurchaseItem", "Store"), title);
			text = string.Format(StringUtil.TR("PurchaseItemDesc", "Store"), title);
			for (int j = 0; j < gameBalanceVars2.PlayerTitles.Length; j++)
			{
				if (gameBalanceVars2.PlayerTitles[j].ID == item.m_titleID)
				{
					if (item.m_currencyType == CurrencyType.ISO)
					{
						num = gameBalanceVars2.PlayerTitles[j].GetUnlockISOPrice();
					}
					else if (item.m_currencyType == CurrencyType.RankedCurrency)
					{
						num = gameBalanceVars2.PlayerTitles[j].GetUnlockRankedCurrencyPrice();
					}
					else if (item.m_currencyType == CurrencyType.FreelancerCurrency)
					{
						num = gameBalanceVars2.PlayerTitles[j].GetUnlockFreelancerCurrencyPrice();
					}
					break;
				}
			}
			UIManager.SetGameObjectActive(this.m_Item.m_itemIcon, false, null);
			UIManager.SetGameObjectActive(this.m_Item.m_itemFG, false, null);
		}
		else if (item.m_itemType == PurchaseItemType.Banner)
		{
			GameBalanceVars gameBalanceVars3 = GameBalanceVars.Get();
			GameBalanceVars.PlayerBanner banner = gameBalanceVars3.GetBanner(item.m_bannerID);
			text2 = string.Format(StringUtil.TR("PurchaseItem", "Store"), banner.GetBannerName());
			text = string.Format(StringUtil.TR("PurchaseItemDesc", "Store"), banner.GetBannerName());
			if (item.m_currencyType == CurrencyType.ISO)
			{
				num = banner.GetUnlockISOPrice();
			}
			else if (item.m_currencyType == CurrencyType.RankedCurrency)
			{
				num = banner.GetUnlockRankedCurrencyPrice();
			}
			else if (item.m_currencyType == CurrencyType.FreelancerCurrency)
			{
				num = banner.GetUnlockFreelancerCurrencyPrice();
			}
			this.m_Item.m_itemIcon.sprite = Resources.Load<Sprite>(banner.m_iconResourceString);
			UIManager.SetGameObjectActive(this.m_Item.m_itemFG, false, null);
		}
		else if (item.m_itemType == PurchaseItemType.Emoticon)
		{
			GameBalanceVars gameBalanceVars4 = GameBalanceVars.Get();
			GameBalanceVars.ChatEmoticon chatEmoticon = null;
			for (int k = 0; k < gameBalanceVars4.ChatEmojis.Length; k++)
			{
				if (item.m_emoticonID == gameBalanceVars4.ChatEmojis[k].ID)
				{
					chatEmoticon = gameBalanceVars4.ChatEmojis[k];
					break;
				}
			}
			text2 = string.Format(StringUtil.TR("PurchaseItem", "Store"), chatEmoticon.GetEmojiName());
			text = chatEmoticon.GetEmojiName();
			if (item.m_currencyType == CurrencyType.ISO)
			{
				num = chatEmoticon.GetUnlockISOPrice();
			}
			else if (item.m_currencyType == CurrencyType.RankedCurrency)
			{
				num = chatEmoticon.GetUnlockRankedCurrencyPrice();
			}
			else if (item.m_currencyType == CurrencyType.FreelancerCurrency)
			{
				num = chatEmoticon.GetUnlockFreelancerCurrencyPrice();
			}
			this.m_Item.m_itemIcon.sprite = Resources.Load<Sprite>(chatEmoticon.IconPath);
			UIManager.SetGameObjectActive(this.m_Item.m_itemFG, false, null);
		}
		else if (item.m_itemType == PurchaseItemType.Overcon)
		{
			UIOverconData.NameToOverconEntry nameToOverconEntry = null;
			foreach (UIOverconData.NameToOverconEntry nameToOverconEntry2 in UIOverconData.Get().m_nameToOverconEntry)
			{
				if (nameToOverconEntry2.m_overconId == item.m_overconID)
				{
					nameToOverconEntry = nameToOverconEntry2;
					break;
				}
			}
			GameBalanceVars gameBalanceVars5 = GameBalanceVars.Get();
			GameBalanceVars.OverconUnlockData playerUnlockable = null;
			for (int l = 0; l < gameBalanceVars5.Overcons.Length; l++)
			{
				if (item.m_overconID == gameBalanceVars5.Overcons[l].ID)
				{
					playerUnlockable = gameBalanceVars5.Overcons[l];
				}
			}
			text2 = string.Format(StringUtil.TR("PurchaseItem", "Store"), nameToOverconEntry.GetDisplayName());
			text = string.Format(StringUtil.TR("PurchaseItemDesc", "Store"), nameToOverconEntry.GetDisplayName());
			if (item.m_currencyType == CurrencyType.ISO)
			{
				num = playerUnlockable.GetUnlockISOPrice();
			}
			else if (item.m_currencyType == CurrencyType.RankedCurrency)
			{
				num = playerUnlockable.GetUnlockRankedCurrencyPrice();
			}
			else if (item.m_currencyType == CurrencyType.FreelancerCurrency)
			{
				num = playerUnlockable.GetUnlockFreelancerCurrencyPrice();
			}
			this.m_Item.m_itemIcon.sprite = Resources.Load<Sprite>(nameToOverconEntry.m_iconSpritePath);
			UIManager.SetGameObjectActive(this.m_Item.m_itemFG, false, null);
		}
		else if (item.m_itemType == PurchaseItemType.AbilityVfx)
		{
			AbilityData component2 = item.m_charLink.ActorDataPrefab.GetComponent<AbilityData>();
			switch (item.m_abilityID)
			{
			case 0:
				this.m_Item.m_itemFG.sprite = component2.m_sprite0;
				break;
			case 1:
				this.m_Item.m_itemFG.sprite = component2.m_sprite1;
				break;
			case 2:
				this.m_Item.m_itemFG.sprite = component2.m_sprite2;
				break;
			case 3:
				this.m_Item.m_itemFG.sprite = component2.m_sprite3;
				break;
			case 4:
				this.m_Item.m_itemFG.sprite = component2.m_sprite4;
				break;
			}
			this.m_Item.m_itemIcon.sprite = Resources.Load<Sprite>("QuestRewards/vfxicon");
			UIManager.SetGameObjectActive(this.m_Item.m_itemFG, true, null);
			for (int m = 0; m < characterUnlockData.abilityVfxUnlockData.Length; m++)
			{
				if (characterUnlockData.abilityVfxUnlockData[m].Index2 == item.m_abilityID && characterUnlockData.abilityVfxUnlockData[m].ID == item.m_abilityVfxID)
				{
					if (item.m_currencyType == CurrencyType.ISO)
					{
						num = characterUnlockData.abilityVfxUnlockData[m].GetUnlockISOPrice();
					}
					else if (item.m_currencyType == CurrencyType.RankedCurrency)
					{
						num = characterUnlockData.abilityVfxUnlockData[m].GetUnlockRankedCurrencyPrice();
					}
					break;
				}
			}
			string vfxswapName = item.m_charLink.GetVFXSwapName(item.m_abilityID, item.m_abilityVfxID);
			text2 = vfxswapName;
			text = vfxswapName;
		}
		else if (item.m_itemType == PurchaseItemType.LoadingScreenBackground)
		{
			GameBalanceVars gameBalanceVars6 = GameBalanceVars.Get();
			GameBalanceVars.LoadingScreenBackground loadingScreenBackground = gameBalanceVars6.GetLoadingScreenBackground(item.m_loadingScreenBackgroundId);
			string loadingScreenBackgroundName = loadingScreenBackground.GetLoadingScreenBackgroundName();
			text2 = string.Format(StringUtil.TR("PurchaseItem", "Store"), loadingScreenBackgroundName);
			text = string.Format(StringUtil.TR("PurchaseItemDesc", "Store"), loadingScreenBackgroundName);
			if (item.m_currencyType == CurrencyType.ISO)
			{
				num = loadingScreenBackground.GetUnlockISOPrice();
			}
			else if (item.m_currencyType == CurrencyType.RankedCurrency)
			{
				num = loadingScreenBackground.GetUnlockRankedCurrencyPrice();
			}
			else if (item.m_currencyType == CurrencyType.FreelancerCurrency)
			{
				num = loadingScreenBackground.GetUnlockFreelancerCurrencyPrice();
			}
			this.m_Item.m_itemIcon.sprite = Resources.Load<Sprite>(loadingScreenBackground.m_iconPath);
			UIManager.SetGameObjectActive(this.m_Item.m_itemFG, false, null);
		}
		int num2 = 0;
		if (ClientGameManager.Get().PlayerWallet != null)
		{
			if (item.m_currencyType == CurrencyType.ISO)
			{
				num2 = ClientGameManager.Get().PlayerWallet.GetCurrentAmount(CurrencyType.ISO);
			}
			else if (item.m_currencyType == CurrencyType.RankedCurrency)
			{
				num2 = ClientGameManager.Get().PlayerWallet.GetCurrentAmount(CurrencyType.RankedCurrency);
			}
			else if (item.m_currencyType == CurrencyType.FreelancerCurrency)
			{
				num2 = ClientGameManager.Get().PlayerWallet.GetCurrentAmount(CurrencyType.FreelancerCurrency);
			}
		}
		this.m_success = (num2 >= num);
		if (this.m_success)
		{
			if (item.m_currencyType == CurrencyType.ISO)
			{
				this.m_descriptionCostResults.text = StringUtil.TR("ISORemaining", "Store");
			}
			else if (item.m_currencyType == CurrencyType.RankedCurrency)
			{
				this.m_descriptionCostResults.text = StringUtil.TR("RankedCurrencyRemaining", "Store");
			}
			else if (item.m_currencyType == CurrencyType.FreelancerCurrency)
			{
				this.m_descriptionCostResults.text = StringUtil.TR("FreelancerCurrencyRemaining", "Store");
			}
			this.SetGeneralButtonLabels(StringUtil.TR("CONFIRMPURCHASE", "Store"));
			this.m_generalBtn.SetClickable(true);
			UIManager.SetGameObjectActive(this.m_generalBtnDisabledImage, false, null);
		}
		else
		{
			string text3 = string.Empty;
			if (item.m_currencyType == CurrencyType.ISO)
			{
				text3 = StringUtil.TR("NotEnoughCurrency", "Global");
			}
			else if (item.m_currencyType == CurrencyType.RankedCurrency)
			{
				text3 = StringUtil.TR("NotEnoughRankedCurrency", "Global");
			}
			else if (item.m_currencyType == CurrencyType.FreelancerCurrency)
			{
				text3 = StringUtil.TR("NotEnoughFreelancerCurrency", "Global");
			}
			this.m_descriptionCostResults.text = text3;
			this.m_descriptionCostResults.color = Color.red;
			text2 = text3;
			this.SetGeneralButtonLabels(text3);
			this.m_generalBtn.SetClickable(false);
			UIManager.SetGameObjectActive(this.m_generalBtnDisabledImage, true, null);
		}
		string str = string.Empty;
		if (item.m_currencyType == CurrencyType.ISO)
		{
			str = "<sprite name=iso>";
		}
		else if (item.m_currencyType == CurrencyType.RankedCurrency)
		{
			str = "<sprite name=rankedCurrency>";
		}
		else if (item.m_currencyType == CurrencyType.FreelancerCurrency)
		{
			str = "<sprite name=credit>";
		}
		this.m_dialogTitle.text = text2;
		this.m_descriptionTitle.text = text;
		this.m_Item.m_headerNameLabel.text = text;
		this.m_descriptionCurrentCredits.text = str + UIStorePanel.FormatIntToString(num2, true);
		this.m_descriptionAddCredits.text = str + UIStorePanel.FormatIntToString(num, true);
		this.m_descriptionTotalCredits.text = str + UIStorePanel.FormatIntToString(Mathf.Abs(num2 - num), true);
		UIManager.SetGameObjectActive(this.m_Item.m_discountLabelContainer, false, null);
		UIManager.SetGameObjectActive(this.m_Item.m_headerPriceContainer, false, null);
		UIManager.SetGameObjectActive(this.m_Item.m_lockedIcon, false, null);
		UIManager.SetGameObjectActive(this.m_Item.m_ownedIcon, false, null);
		UIManager.SetGameObjectActive(this.m_Item.m_selectedCurrent, false, null);
		UIManager.SetGameObjectActive(this.m_Item.m_selectedInUse, false, null);
		UIManager.SetGameObjectActive(this.m_Item.m_realCurrencyIcon, false, null);
		UIManager.SetGameObjectActive(this.m_Item.m_gameCurrencyLabel, false, null);
		UIManager.SetGameObjectActive(this.m_Item.m_realCurrencyLabel, false, null);
		UIManager.SetGameObjectActive(this.m_resultPurchaseContainer, false, null);
		UIManager.SetGameObjectActive(this.m_waitingPurchaseContainer, false, null);
		UIManager.SetGameObjectActive(this.m_confirmPurchaseContainer, false, null);
		UIManager.SetGameObjectActive(this.m_generalBtnContainer, true, null);
		UIManager.SetGameObjectActive(this.m_disableCloseBtn, false, null);
	}

	public delegate void PurchaseCloseDialogCallback();
}

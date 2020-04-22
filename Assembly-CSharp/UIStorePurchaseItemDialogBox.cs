using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIStorePurchaseItemDialogBox : UIDialogBox
{
	public delegate void PurchaseCloseDialogCallback();

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

	private PurchaseCloseDialogCallback m_closeDialogCallback;

	public override void Awake()
	{
		base.Awake();
		m_startPurchaseTime = -1f;
		m_closeBtn.callback = CloseDialog;
		m_cancelBtn.callback = CloseDialog;
		m_confirmBtn.callback = PurchaseItem;
		m_generalBtn.callback = GeneralButtonClicked;
		m_purchaseResultBtn.callback = PurchaseResultOkClicked;
	}

	private void TimeoutPurchase()
	{
		m_purchaseResultLabel.text = StringUtil.TR("PurchaseHasTimedOut", "Store");
		m_purchaseSuccess = false;
		UIManager.SetGameObjectActive(m_waitingPurchaseContainer, false);
		UIManager.SetGameObjectActive(m_confirmPurchaseContainer, false);
		UIManager.SetGameObjectActive(m_resultPurchaseContainer, true);
		UIManager.SetGameObjectActive(m_generalBtnContainer, false);
		UIManager.SetGameObjectActive(m_disableCloseBtn, false);
	}

	public void PurchaseItem(BaseEventData data)
	{
		if (m_itemRef.m_itemType == PurchaseItemType.Character)
		{
			UIStorePanel.Get().RequestToPurchaseCharacter(m_itemRef.m_currencyType, m_itemRef.m_charLink.m_characterType);
		}
		else if (m_itemRef.m_itemType == PurchaseItemType.Skin)
		{
			UIStorePanel.Get().RequestToPurchaseSkin(m_itemRef.m_currencyType, m_itemRef.m_charLink.m_characterType, m_itemRef.m_skinIndex);
		}
		else if (m_itemRef.m_itemType == PurchaseItemType.Texture)
		{
			UIStorePanel.Get().RequestToPurchaseTexture(m_itemRef.m_currencyType, m_itemRef.m_charLink.m_characterType, m_itemRef.m_skinIndex, m_itemRef.m_textureIndex);
		}
		else if (m_itemRef.m_itemType == PurchaseItemType.Tint)
		{
			UIStorePanel.Get().RequestToPurchaseTint(m_itemRef.m_currencyType, m_itemRef.m_charLink.m_characterType, m_itemRef.m_skinIndex, m_itemRef.m_textureIndex, m_itemRef.m_tintIndex);
		}
		else if (m_itemRef.m_itemType == PurchaseItemType.Taunt)
		{
			UIStorePanel.Get().RequestToPurchaseTaunt(m_itemRef.m_currencyType, m_itemRef.m_charLink.m_characterType, m_itemRef.m_tauntIndex);
		}
		else if (m_itemRef.m_itemType == PurchaseItemType.InventoryItem)
		{
			UIStorePanel.Get().RequestToPurchaseInventoryItem(m_itemRef.m_inventoryTemplateId, m_itemRef.m_currencyType);
		}
		else if (m_itemRef.m_itemType == PurchaseItemType.Title)
		{
			UIStorePanel.Get().RequestToPurchaseTitle(m_itemRef.m_titleID, m_itemRef.m_currencyType);
		}
		else if (m_itemRef.m_itemType == PurchaseItemType.Banner)
		{
			if (GameBalanceVars.Get().GetBanner(m_itemRef.m_bannerID).m_type == GameBalanceVars.PlayerBanner.BannerType.Background)
			{
				UIStorePanel.Get().RequestToPurchaseBanner(m_itemRef.m_bannerID, m_itemRef.m_currencyType);
			}
			else
			{
				UIStorePanel.Get().RequestToPurchaseEmblem(m_itemRef.m_bannerID, m_itemRef.m_currencyType);
			}
		}
		else if (m_itemRef.m_itemType == PurchaseItemType.Emoticon)
		{
			UIStorePanel.Get().RequestToPurchaseEmoticon(m_itemRef.m_emoticonID, m_itemRef.m_currencyType);
		}
		else if (m_itemRef.m_itemType == PurchaseItemType.AbilityVfx)
		{
			UIStorePanel.Get().RequestToPurchaseAbilityVfx(m_itemRef.m_charLink.m_characterType, m_itemRef.m_abilityID, m_itemRef.m_abilityVfxID, m_itemRef.m_currencyType);
		}
		else if (m_itemRef.m_itemType == PurchaseItemType.Overcon)
		{
			UIStorePanel.Get().RequestToPurchaseOvercon(m_itemRef.m_overconID, m_itemRef.m_currencyType);
		}
		else if (m_itemRef.m_itemType == PurchaseItemType.LoadingScreenBackground)
		{
			UIStorePanel.Get().RequestToPurchaseLoadingScreenBackground(m_itemRef.m_loadingScreenBackgroundId, m_itemRef.m_currencyType);
		}
		m_startPurchaseTime = Time.time;
		UIManager.SetGameObjectActive(m_waitingPurchaseContainer, true);
		UIManager.SetGameObjectActive(m_confirmPurchaseContainer, false);
		UIManager.SetGameObjectActive(m_resultPurchaseContainer, false);
		UIManager.SetGameObjectActive(m_generalBtnContainer, false);
		UIManager.SetGameObjectActive(m_disableCloseBtn, true);
		m_purchaseSuccess = false;
	}

	private void SetGeneralButtonLabels(string newString)
	{
		for (int i = 0; i < m_generalBtnLabels.Length; i++)
		{
			m_generalBtnLabels[i].text = newString;
		}
	}

	public void NotifyPurchaseResponse(bool successful)
	{
		if (successful)
		{
			UIFrontEnd.PlaySound(FrontEndButtonSounds.PurchaseComplete);
			m_purchaseResultLabel.text = StringUtil.TR("PurchaseSuccessful", "Store");
			m_purchaseSuccess = true;
			Close();
		}
		else
		{
			m_purchaseResultLabel.text = StringUtil.TR("PurchaseFailed", "Store");
			m_purchaseSuccess = false;
		}
		UIManager.SetGameObjectActive(m_waitingPurchaseContainer, false);
		UIManager.SetGameObjectActive(m_confirmPurchaseContainer, false);
		UIManager.SetGameObjectActive(m_resultPurchaseContainer, true);
		UIManager.SetGameObjectActive(m_generalBtnContainer, false);
		UIManager.SetGameObjectActive(m_disableCloseBtn, false);
	}

	public void NotifyPurchaseInventoryItemResponse(bool successful)
	{
		NotifyPurchaseResponse(successful);
	}

	public void NotifyTauntPurchaseResponse(bool successful)
	{
		NotifyPurchaseResponse(successful);
		if (!successful)
		{
			return;
		}
		while (true)
		{
			UICharacterTauntsPanel.RefreshActivePanels(m_itemRef.m_charLink, m_itemRef.m_tauntIndex);
			return;
		}
	}

	public void GeneralButtonClicked(BaseEventData data)
	{
		if (m_success)
		{
			UIFrontEnd.PlaySound(FrontEndButtonSounds.Cancel);
			m_confirmPurchaseLabel.text = StringUtil.TR("PurchaseConfirmation", "Store");
			UIManager.SetGameObjectActive(m_confirmPurchaseContainer, true);
			UIManager.SetGameObjectActive(m_generalBtnContainer, false);
			PurchaseItem(null);
		}
		else
		{
			Close();
			UIFrontEnd.Get().m_frontEndNavPanel.CollectionsBtnClicked(null);
			UIFrontEnd.PlaySound(FrontEndButtonSounds.SubMenuOpen);
		}
	}

	public void CloseDialog(BaseEventData data)
	{
		Close();
	}

	public void PurchaseResultOkClicked(BaseEventData data)
	{
		if (m_purchaseSuccess)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					Close();
					return;
				}
			}
		}
		UIManager.SetGameObjectActive(m_resultPurchaseContainer, false);
		UIManager.SetGameObjectActive(m_generalBtnContainer, true);
	}

	public override void ClearCallback()
	{
	}

	protected override void CloseCallback()
	{
	}

	public void Update()
	{
		if (!UIStorePanel.Get().IsWaitingForPurchaseRequest)
		{
			return;
		}
		while (true)
		{
			if (Time.time - m_startPurchaseTime >= 5f)
			{
				while (true)
				{
					TimeoutPurchase();
					UIStorePanel.Get().TimeOutPurchase();
					return;
				}
			}
			return;
		}
	}

	public override void Close()
	{
		if (m_closeDialogCallback != null)
		{
			m_closeDialogCallback();
			m_closeDialogCallback = null;
		}
		base.Close();
	}

	public void Setup(UIPurchaseableItem item, PurchaseCloseDialogCallback closeCallback = null)
	{
		m_itemRef = item;
		m_closeDialogCallback = closeCallback;
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
		UIManager.SetGameObjectActive(m_Item.m_itemFG, false);
		if (item.m_itemType == PurchaseItemType.Character)
		{
			text2 = StringUtil.TR("PurchaseFreelancer", "Store");
			text = string.Format(StringUtil.TR("PurchaseFreelancerDesc", "Store"), item.m_charLink.GetDisplayName());
			m_Item.m_itemIcon.sprite = Resources.Load<Sprite>(item.m_charLink.m_characterIconResourceString);
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
			m_Item.m_itemIcon.sprite = (Sprite)Resources.Load(characterSkin.m_skinSelectionIconPath, typeof(Sprite));
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
			m_Item.m_itemIcon.sprite = (Sprite)Resources.Load(characterColor.m_iconResourceString, typeof(Sprite));
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
			m_Item.m_itemIcon.sprite = m_tauntSprite;
			UIManager.SetGameObjectActive(m_Item.m_itemFG, true);
			AbilityData component = item.m_charLink.ActorDataPrefab.GetComponent<AbilityData>();
			if (item.m_charLink.m_taunts[tauntIndex].m_actionForTaunt == AbilityData.ActionType.ABILITY_0)
			{
				m_Item.m_itemFG.sprite = component.m_sprite0;
			}
			else if (item.m_charLink.m_taunts[tauntIndex].m_actionForTaunt == AbilityData.ActionType.ABILITY_1)
			{
				m_Item.m_itemFG.sprite = component.m_sprite1;
			}
			else if (item.m_charLink.m_taunts[tauntIndex].m_actionForTaunt == AbilityData.ActionType.ABILITY_2)
			{
				m_Item.m_itemFG.sprite = component.m_sprite2;
			}
			else if (item.m_charLink.m_taunts[tauntIndex].m_actionForTaunt == AbilityData.ActionType.ABILITY_3)
			{
				m_Item.m_itemFG.sprite = component.m_sprite3;
			}
			else if (item.m_charLink.m_taunts[tauntIndex].m_actionForTaunt == AbilityData.ActionType.ABILITY_4)
			{
				m_Item.m_itemFG.sprite = component.m_sprite4;
			}
			else if (item.m_charLink.m_taunts[tauntIndex].m_actionForTaunt == AbilityData.ActionType.ABILITY_5)
			{
				m_Item.m_itemFG.sprite = component.m_sprite5;
			}
			else if (item.m_charLink.m_taunts[tauntIndex].m_actionForTaunt == AbilityData.ActionType.ABILITY_6)
			{
				m_Item.m_itemFG.sprite = component.m_sprite6;
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
				int num2 = 0;
				while (true)
				{
					if (num2 < gameBalanceVars.StoreItemsForPurchase.Length)
					{
						if (gameBalanceVars.StoreItemsForPurchase[num2].m_itemTemplateId == item.m_inventoryTemplateId)
						{
							num = gameBalanceVars.StoreItemsForPurchase[num2].GetUnlockFreelancerCurrencyPrice();
							break;
						}
						num2++;
						continue;
					}
					break;
				}
			}
			string spritePath = InventoryWideData.GetSpritePath(itemTemplate);
			if (!spritePath.IsNullOrEmpty())
			{
				m_Item.m_itemIcon.sprite = (Sprite)Resources.Load(spritePath, typeof(Sprite));
			}
			else
			{
				UIManager.SetGameObjectActive(m_Item.m_itemIcon, false);
			}
			UIManager.SetGameObjectActive(m_Item.m_itemFG, false);
		}
		else if (item.m_itemType == PurchaseItemType.Title)
		{
			GameBalanceVars gameBalanceVars2 = GameBalanceVars.Get();
			string title = gameBalanceVars2.GetTitle(item.m_titleID, string.Empty);
			text2 = string.Format(StringUtil.TR("PurchaseItem", "Store"), title);
			text = string.Format(StringUtil.TR("PurchaseItemDesc", "Store"), title);
			int num3 = 0;
			while (true)
			{
				if (num3 < gameBalanceVars2.PlayerTitles.Length)
				{
					if (gameBalanceVars2.PlayerTitles[num3].ID == item.m_titleID)
					{
						if (item.m_currencyType == CurrencyType.ISO)
						{
							num = gameBalanceVars2.PlayerTitles[num3].GetUnlockISOPrice();
						}
						else if (item.m_currencyType == CurrencyType.RankedCurrency)
						{
							num = gameBalanceVars2.PlayerTitles[num3].GetUnlockRankedCurrencyPrice();
						}
						else if (item.m_currencyType == CurrencyType.FreelancerCurrency)
						{
							num = gameBalanceVars2.PlayerTitles[num3].GetUnlockFreelancerCurrencyPrice();
						}
						break;
					}
					num3++;
					continue;
				}
				break;
			}
			UIManager.SetGameObjectActive(m_Item.m_itemIcon, false);
			UIManager.SetGameObjectActive(m_Item.m_itemFG, false);
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
			m_Item.m_itemIcon.sprite = Resources.Load<Sprite>(banner.m_iconResourceString);
			UIManager.SetGameObjectActive(m_Item.m_itemFG, false);
		}
		else if (item.m_itemType == PurchaseItemType.Emoticon)
		{
			GameBalanceVars gameBalanceVars4 = GameBalanceVars.Get();
			GameBalanceVars.ChatEmoticon chatEmoticon = null;
			for (int i = 0; i < gameBalanceVars4.ChatEmojis.Length; i++)
			{
				if (item.m_emoticonID == gameBalanceVars4.ChatEmojis[i].ID)
				{
					chatEmoticon = gameBalanceVars4.ChatEmojis[i];
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
			m_Item.m_itemIcon.sprite = Resources.Load<Sprite>(chatEmoticon.IconPath);
			UIManager.SetGameObjectActive(m_Item.m_itemFG, false);
		}
		else if (item.m_itemType == PurchaseItemType.Overcon)
		{
			UIOverconData.NameToOverconEntry nameToOverconEntry = null;
			foreach (UIOverconData.NameToOverconEntry item2 in UIOverconData.Get().m_nameToOverconEntry)
			{
				if (item2.m_overconId == item.m_overconID)
				{
					nameToOverconEntry = item2;
				}
			}
			GameBalanceVars gameBalanceVars5 = GameBalanceVars.Get();
			GameBalanceVars.OverconUnlockData playerUnlockable = null;
			for (int j = 0; j < gameBalanceVars5.Overcons.Length; j++)
			{
				if (item.m_overconID == gameBalanceVars5.Overcons[j].ID)
				{
					playerUnlockable = gameBalanceVars5.Overcons[j];
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
			m_Item.m_itemIcon.sprite = Resources.Load<Sprite>(nameToOverconEntry.m_iconSpritePath);
			UIManager.SetGameObjectActive(m_Item.m_itemFG, false);
		}
		else if (item.m_itemType == PurchaseItemType.AbilityVfx)
		{
			AbilityData component2 = item.m_charLink.ActorDataPrefab.GetComponent<AbilityData>();
			switch (item.m_abilityID)
			{
			case 0:
				m_Item.m_itemFG.sprite = component2.m_sprite0;
				break;
			case 1:
				m_Item.m_itemFG.sprite = component2.m_sprite1;
				break;
			case 2:
				m_Item.m_itemFG.sprite = component2.m_sprite2;
				break;
			case 3:
				m_Item.m_itemFG.sprite = component2.m_sprite3;
				break;
			case 4:
				m_Item.m_itemFG.sprite = component2.m_sprite4;
				break;
			}
			m_Item.m_itemIcon.sprite = Resources.Load<Sprite>("QuestRewards/vfxicon");
			UIManager.SetGameObjectActive(m_Item.m_itemFG, true);
			int num4 = 0;
			while (true)
			{
				if (num4 < characterUnlockData.abilityVfxUnlockData.Length)
				{
					if (characterUnlockData.abilityVfxUnlockData[num4].Index2 == item.m_abilityID && characterUnlockData.abilityVfxUnlockData[num4].ID == item.m_abilityVfxID)
					{
						if (item.m_currencyType == CurrencyType.ISO)
						{
							num = characterUnlockData.abilityVfxUnlockData[num4].GetUnlockISOPrice();
						}
						else if (item.m_currencyType == CurrencyType.RankedCurrency)
						{
							num = characterUnlockData.abilityVfxUnlockData[num4].GetUnlockRankedCurrencyPrice();
						}
						break;
					}
					num4++;
					continue;
				}
				break;
			}
			string vFXSwapName = item.m_charLink.GetVFXSwapName(item.m_abilityID, item.m_abilityVfxID);
			text2 = vFXSwapName;
			text = vFXSwapName;
		}
		else if (item.m_itemType == PurchaseItemType.LoadingScreenBackground)
		{
			GameBalanceVars gameBalanceVars6 = GameBalanceVars.Get();
			GameBalanceVars.LoadingScreenBackground loadingScreenBackground = null;
			loadingScreenBackground = gameBalanceVars6.GetLoadingScreenBackground(item.m_loadingScreenBackgroundId);
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
			m_Item.m_itemIcon.sprite = Resources.Load<Sprite>(loadingScreenBackground.m_iconPath);
			UIManager.SetGameObjectActive(m_Item.m_itemFG, false);
		}
		int num5 = 0;
		if (ClientGameManager.Get().PlayerWallet != null)
		{
			if (item.m_currencyType == CurrencyType.ISO)
			{
				num5 = ClientGameManager.Get().PlayerWallet.GetCurrentAmount(CurrencyType.ISO);
			}
			else if (item.m_currencyType == CurrencyType.RankedCurrency)
			{
				num5 = ClientGameManager.Get().PlayerWallet.GetCurrentAmount(CurrencyType.RankedCurrency);
			}
			else if (item.m_currencyType == CurrencyType.FreelancerCurrency)
			{
				num5 = ClientGameManager.Get().PlayerWallet.GetCurrentAmount(CurrencyType.FreelancerCurrency);
			}
		}
		m_success = (num5 >= num);
		if (m_success)
		{
			if (item.m_currencyType == CurrencyType.ISO)
			{
				m_descriptionCostResults.text = StringUtil.TR("ISORemaining", "Store");
			}
			else if (item.m_currencyType == CurrencyType.RankedCurrency)
			{
				m_descriptionCostResults.text = StringUtil.TR("RankedCurrencyRemaining", "Store");
			}
			else if (item.m_currencyType == CurrencyType.FreelancerCurrency)
			{
				m_descriptionCostResults.text = StringUtil.TR("FreelancerCurrencyRemaining", "Store");
			}
			SetGeneralButtonLabels(StringUtil.TR("CONFIRMPURCHASE", "Store"));
			m_generalBtn.SetClickable(true);
			UIManager.SetGameObjectActive(m_generalBtnDisabledImage, false);
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
			m_descriptionCostResults.text = text3;
			m_descriptionCostResults.color = Color.red;
			text2 = text3;
			SetGeneralButtonLabels(text3);
			m_generalBtn.SetClickable(false);
			UIManager.SetGameObjectActive(m_generalBtnDisabledImage, true);
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
		m_dialogTitle.text = text2;
		m_descriptionTitle.text = text;
		m_Item.m_headerNameLabel.text = text;
		m_descriptionCurrentCredits.text = str + UIStorePanel.FormatIntToString(num5, true);
		m_descriptionAddCredits.text = str + UIStorePanel.FormatIntToString(num, true);
		m_descriptionTotalCredits.text = str + UIStorePanel.FormatIntToString(Mathf.Abs(num5 - num), true);
		UIManager.SetGameObjectActive(m_Item.m_discountLabelContainer, false);
		UIManager.SetGameObjectActive(m_Item.m_headerPriceContainer, false);
		UIManager.SetGameObjectActive(m_Item.m_lockedIcon, false);
		UIManager.SetGameObjectActive(m_Item.m_ownedIcon, false);
		UIManager.SetGameObjectActive(m_Item.m_selectedCurrent, false);
		UIManager.SetGameObjectActive(m_Item.m_selectedInUse, false);
		UIManager.SetGameObjectActive(m_Item.m_realCurrencyIcon, false);
		UIManager.SetGameObjectActive(m_Item.m_gameCurrencyLabel, false);
		UIManager.SetGameObjectActive(m_Item.m_realCurrencyLabel, false);
		UIManager.SetGameObjectActive(m_resultPurchaseContainer, false);
		UIManager.SetGameObjectActive(m_waitingPurchaseContainer, false);
		UIManager.SetGameObjectActive(m_confirmPurchaseContainer, false);
		UIManager.SetGameObjectActive(m_generalBtnContainer, true);
		UIManager.SetGameObjectActive(m_disableCloseBtn, false);
	}
}

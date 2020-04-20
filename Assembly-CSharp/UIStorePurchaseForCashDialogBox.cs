using System;
using System.Collections.Generic;
using LobbyGameClientMessages;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIStorePurchaseForCashDialogBox : UIDialogBox
{
	public _ButtonSwapSprite m_closeBtn;

	public _ButtonSwapSprite m_failBtnOkay;

	public _ButtonSwapSprite m_successBtnOkay;

	public Image m_disableCloseBtn;

	public TextMeshProUGUI m_dialogTitle;

	public TextMeshProUGUI m_descriptionTitle;

	public TextMeshProUGUI m_descriptionCurrentCredits;

	public TextMeshProUGUI m_descriptionAddCredits;

	public TextMeshProUGUI m_descriptionTotalCredits;

	public RectTransform m_processingPurchaseContainer;

	public RectTransform m_failToPurchaseContainer;

	public RectTransform m_succeededToPurchaseContainer;

	public UIStoreCurrencyItem m_currencyItem;

	public Image m_addPaymentDisabledImage;

	public Image m_confirmButtonDisabledImage;

	public _ButtonSwapSprite m_addPaymentButton;

	public _ButtonSwapSprite m_confirmPurchaseButton;

	public TextMeshProUGUI m_priceLabel;

	public TextMeshProUGUI m_failPurchaseLabel;

	public RectTransform m_dropDownContainer;

	public RectTransform m_addPaymentBtnContainer;

	public GridLayoutGroup m_dropDownList;

	public TextMeshProUGUI[] m_dropDownTextLabels;

	public UIStorePaymentMethodDropdownItem m_paymentPrefab;

	public _DropdownMenuList m_dropdownMenu;

	public TextMeshProUGUI m_addOrUpdatePaymentInfoLabel;

	private UIPurchaseableItem m_itemRef;

	private bool m_openedURL;

	private bool m_processingPayment;

	private float m_processStartTime;

	private List<UIStorePaymentMethodDropdownItem> m_dropdownItems;

	private PaymentMethod m_selectedPaymentMethod;

	public override void Awake()
	{
		base.Awake();
		this.m_closeBtn.callback = new _ButtonSwapSprite.ButtonClickCallback(this.CloseButton);
		this.m_failBtnOkay.callback = new _ButtonSwapSprite.ButtonClickCallback(this.TryAgainButton);
		this.m_successBtnOkay.callback = new _ButtonSwapSprite.ButtonClickCallback(this.CloseButton);
		this.m_closeBtn.m_ignoreDialogboxes = true;
		UIManager.SetGameObjectActive(this.m_disableCloseBtn, false, null);
		UIManager.SetGameObjectActive(this.m_addPaymentDisabledImage, false, null);
		UIManager.SetGameObjectActive(this.m_confirmButtonDisabledImage, false, null);
		UIManager.SetGameObjectActive(this.m_currencyItem.m_hitBox, false, null);
		UIManager.SetGameObjectActive(this.m_processingPurchaseContainer, false, null);
		UIManager.SetGameObjectActive(this.m_failToPurchaseContainer, false, null);
		UIManager.SetGameObjectActive(this.m_succeededToPurchaseContainer, false, null);
		this.m_confirmPurchaseButton.callback = new _ButtonSwapSprite.ButtonClickCallback(this.PurchaseButtonClicked);
		this.m_addPaymentButton.callback = new _ButtonSwapSprite.ButtonClickCallback(this.AddPaymentMethodClicked);
	}

	public override void ClearCallback()
	{
	}

	protected override void CloseCallback()
	{
	}

	public void AddPaymentMethodClicked(BaseEventData data)
	{
		UIFrontEnd.PlaySound(FrontEndButtonSounds.GeneralExternalWebsite);
		this.m_openedURL = true;
		string url = ClientGameManager.Get().CommerceURL + "/account/payment/add-payment-method-flow.action";
		Application.OpenURL(url);
		this.m_dropdownMenu.SetListContainerVisible(false);
	}

	public void NotifyPurchaseResponse(bool successful)
	{
		UIManager.SetGameObjectActive(this.m_processingPurchaseContainer, false, null);
		UIManager.SetGameObjectActive(this.m_failToPurchaseContainer, !successful, null);
		UIManager.SetGameObjectActive(this.m_succeededToPurchaseContainer, successful, null);
		this.m_failPurchaseLabel.text = StringUtil.TR("PurchaseHasFailed", "Store");
	}

	public void PurchaseButtonClicked(BaseEventData data)
	{
		if (this.m_itemRef.m_itemType == PurchaseItemType.LootMatrixPack)
		{
			UIStorePanel.Get().RequestToPurchaseLootMatrixPack(this.m_itemRef.m_lootMatrixPack, this.m_selectedPaymentMethod);
		}
		else if (this.m_itemRef.m_itemType == PurchaseItemType.Character)
		{
			UIStorePanel.Get().RequestToPurchaseCharacterForCash(this.m_itemRef.m_charLink, this.m_selectedPaymentMethod);
		}
		else if (this.m_itemRef.m_itemType == PurchaseItemType.GGBoost)
		{
			UIStorePanel.Get().RequestToPurchaseGGPack(this.m_itemRef.m_ggPack, this.m_selectedPaymentMethod);
		}
		else if (this.m_itemRef.m_itemType == PurchaseItemType.Tint)
		{
			UIStorePanel.Get().RequestToPurchaseTintForCash(this.m_itemRef.m_charLink.m_characterType, this.m_itemRef.m_skinIndex, this.m_itemRef.m_textureIndex, this.m_itemRef.m_tintIndex, this.m_selectedPaymentMethod);
		}
		else if (this.m_itemRef.m_itemType == PurchaseItemType.InventoryItem)
		{
			UIStorePanel.Get().RequestToPurchaseStoreItems(this.m_itemRef.m_inventoryTemplateId, this.m_selectedPaymentMethod);
		}
	}

	private void TimeOutPurchase()
	{
		this.m_processingPayment = false;
		UIManager.SetGameObjectActive(this.m_disableCloseBtn, false, null);
		UIManager.SetGameObjectActive(this.m_processingPurchaseContainer, false, null);
		UIManager.SetGameObjectActive(this.m_failToPurchaseContainer, true, null);
		this.m_failPurchaseLabel.text = StringUtil.TR("PurchaseHasTimedOut", "Store");
	}

	private void SetProcessingPurchasing(bool processingPurchase)
	{
		if (this.m_processingPayment != processingPurchase)
		{
			UIManager.SetGameObjectActive(this.m_confirmButtonDisabledImage, processingPurchase, null);
			this.m_confirmPurchaseButton.SetClickable(!processingPurchase);
			UIManager.SetGameObjectActive(this.m_disableCloseBtn, processingPurchase, null);
			UIManager.SetGameObjectActive(this.m_processingPurchaseContainer, processingPurchase, null);
			if (processingPurchase)
			{
				this.m_processStartTime = Time.time;
			}
			this.m_processingPayment = processingPurchase;
		}
	}

	private void OnApplicationFocus(bool focusStatus)
	{
		if (focusStatus && this.m_openedURL)
		{
			this.m_openedURL = false;
			ClientGameManager.Get().RequestPaymentMethods(new Action<PaymentMethodsResponse>(UIStorePanel.Get().RefreshPayments));
		}
	}

	public void Update()
	{
		this.SetProcessingPurchasing(UIStorePanel.Get().IsWaitingForPurchaseRequest);
		if (SteamManager.UsingSteam)
		{
			if (!UIStorePanel.Get().IsWaitingForSteamPurchaseResponse)
			{
				if (this.m_processingPayment && Time.time - UIStorePanel.Get().TimeReceivedSteamPurchaseResponse > 30f)
				{
					this.TimeOutPurchase();
					UIStorePanel.Get().TimeOutPurchase();
				}
			}
		}
		else if (this.m_processingPayment)
		{
			if (Time.time - this.m_processStartTime > 30f)
			{
				this.TimeOutPurchase();
				UIStorePanel.Get().TimeOutPurchase();
			}
		}
	}

	public void RefreshPayments(PaymentMethodsResponse response)
	{
		if (response.Success)
		{
			this.ClearOldPaymentList();
			this.HandleRequestPaymentResponse(response);
		}
	}

	public void HandleRequestPaymentResponse(PaymentMethodsResponse response)
	{
		if (this.m_dropdownItems == null)
		{
			this.m_dropdownItems = new List<UIStorePaymentMethodDropdownItem>();
		}
		this.m_dropdownItems.Clear();
		int num = 0;
		PaymentMethod paymentMethod = null;
		if (response != null && response.Success)
		{
			if (response.PaymentMethodList != null)
			{
				if (!response.PaymentMethodList.IsError && response.PaymentMethodList.PaymentMethods != null)
				{
					int i = 0;
					while (i < response.PaymentMethodList.PaymentMethods.Count)
					{
						PaymentMethod paymentMethod2 = response.PaymentMethodList.PaymentMethods[i];
						UIStorePaymentMethodDropdownItem uistorePaymentMethodDropdownItem = UnityEngine.Object.Instantiate<UIStorePaymentMethodDropdownItem>(this.m_paymentPrefab);
						uistorePaymentMethodDropdownItem.transform.SetParent(this.m_dropDownList.transform);
						uistorePaymentMethodDropdownItem.transform.localPosition = Vector3.zero;
						uistorePaymentMethodDropdownItem.transform.localScale = Vector3.one;
						uistorePaymentMethodDropdownItem.SetPaymentMethod(paymentMethod2);
						uistorePaymentMethodDropdownItem.m_hitbox.m_ignoreDialogboxes = true;
						uistorePaymentMethodDropdownItem.m_hitbox.callback = new _ButtonSwapSprite.ButtonClickCallback(this.PaymentItemSelected);
						if (!paymentMethod2.isDefault)
						{
							goto IL_17A;
						}
						if (!(paymentMethod2.specificType != "Steam Wallet"))
						{
							goto IL_17A;
						}
						paymentMethod = paymentMethod2;
						uistorePaymentMethodDropdownItem.SetText(string.Format(StringUtil.TR("PaymentInfo", "Store"), paymentMethod2.specificType, paymentMethod2.maskedPaymentInfo));
						IL_1AD:
						this.m_dropdownItems.Add(uistorePaymentMethodDropdownItem);
						num++;
						i++;
						continue;
						IL_17A:
						uistorePaymentMethodDropdownItem.SetText(paymentMethod2.specificType + " " + paymentMethod2.maskedPaymentInfo);
						if (paymentMethod2.specificType == "Steam Wallet")
						{
							paymentMethod = paymentMethod2;
							goto IL_1AD;
						}
						goto IL_1AD;
					}
					if (!SteamManager.UsingSteam)
					{
						UIStorePaymentMethodDropdownItem uistorePaymentMethodDropdownItem2 = UnityEngine.Object.Instantiate<UIStorePaymentMethodDropdownItem>(this.m_paymentPrefab);
						uistorePaymentMethodDropdownItem2.transform.SetParent(this.m_dropDownList.transform);
						uistorePaymentMethodDropdownItem2.transform.localPosition = Vector3.zero;
						uistorePaymentMethodDropdownItem2.transform.localScale = Vector3.one;
						uistorePaymentMethodDropdownItem2.SetText(StringUtil.TR("AddNewPayment", "Store"));
						uistorePaymentMethodDropdownItem2.m_hitbox.m_ignoreDialogboxes = true;
						uistorePaymentMethodDropdownItem2.m_hitbox.callback = new _ButtonSwapSprite.ButtonClickCallback(this.AddPaymentMethodClicked);
						this.m_dropdownItems.Add(uistorePaymentMethodDropdownItem2);
						if (this.m_addOrUpdatePaymentInfoLabel != null)
						{
							UIManager.SetGameObjectActive(this.m_addOrUpdatePaymentInfoLabel, true, null);
						}
					}
					else if (this.m_addOrUpdatePaymentInfoLabel != null)
					{
						UIManager.SetGameObjectActive(this.m_addOrUpdatePaymentInfoLabel, false, null);
					}
				}
			}
		}
		UIManager.SetGameObjectActive(this.m_addPaymentBtnContainer, num == 0, null);
		UIManager.SetGameObjectActive(this.m_dropDownContainer, num > 0, null);
		string dropDownText = StringUtil.TR("None", "Global");
		if (paymentMethod != null)
		{
			this.m_selectedPaymentMethod = paymentMethod;
			string str = string.Empty;
			if (this.m_selectedPaymentMethod.isDefault && this.m_selectedPaymentMethod.generalType != "Steam Wallet")
			{
				str = StringUtil.TR("DefaultPaymentMethod", "Store");
			}
			dropDownText = this.m_selectedPaymentMethod.specificType + " " + this.m_selectedPaymentMethod.maskedPaymentInfo + str;
			UIManager.SetGameObjectActive(this.m_confirmButtonDisabledImage, this.m_selectedPaymentMethod == null, null);
			this.m_confirmPurchaseButton.SetClickable(this.m_selectedPaymentMethod != null);
		}
		this.SetDropDownText(dropDownText);
	}

	public void SetDropDownText(string newText)
	{
		for (int i = 0; i < this.m_dropDownTextLabels.Length; i++)
		{
			this.m_dropDownTextLabels[i].text = newText;
		}
	}

	public void PaymentItemSelected(BaseEventData data)
	{
		for (int i = 0; i < this.m_dropdownItems.Count; i++)
		{
			if (this.m_dropdownItems[i].m_hitbox.gameObject == (data as PointerEventData).pointerCurrentRaycast.gameObject)
			{
				this.m_selectedPaymentMethod = this.m_dropdownItems[i].GetPaymentMethod();
				this.m_dropdownItems[i].m_hitbox.ForceSetPointerEntered(false);
				string str = string.Empty;
				if (this.m_selectedPaymentMethod.isDefault)
				{
					if (this.m_selectedPaymentMethod.generalType != "Steam Wallet")
					{
						str = StringUtil.TR("DefaultPaymentMethod", "Store");
					}
				}
				this.SetDropDownText(this.m_selectedPaymentMethod.specificType + " " + this.m_selectedPaymentMethod.maskedPaymentInfo + str);
				this.m_dropdownMenu.ToggleListContainer(null);
				UIManager.SetGameObjectActive(this.m_confirmButtonDisabledImage, this.m_selectedPaymentMethod == null, null);
				this.m_confirmPurchaseButton.SetClickable(this.m_selectedPaymentMethod != null);
				return;
			}
		}
		for (;;)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			return;
		}
	}

	private void ClearOldPaymentList()
	{
		UIStorePaymentMethodDropdownItem[] componentsInChildren = this.m_dropDownList.GetComponentsInChildren<UIStorePaymentMethodDropdownItem>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			UnityEngine.Object.Destroy(componentsInChildren[i].gameObject);
		}
	}

	public void Setup(UIPurchaseableItem item, PaymentMethodsResponse response)
	{
		this.m_itemRef = item;
		HydrogenConfig hydrogenConfig = HydrogenConfig.Get();
		string accountCurrency = hydrogenConfig.Ticket.AccountCurrency;
		this.m_descriptionCurrentCredits.text = string.Empty;
		this.m_descriptionAddCredits.text = string.Empty;
		this.m_descriptionTotalCredits.text = string.Empty;
		this.m_currencyItem.m_headerPriceLabel.text = string.Empty;
		float num = 0f;
		float num2 = 0f;
		string text = string.Empty;
		string text2 = string.Empty;
		if (item.m_itemType == PurchaseItemType.LootMatrixPack)
		{
			if (item.m_lootMatrixPack.IsBundle)
			{
				text2 = StringUtil.TR("PurchaseBundle", "Store");
			}
			else
			{
				text2 = StringUtil.TR("PurchaseLootMatrixPacks", "Store");
			}
			LootMatrixPack lootMatrixPack = item.m_lootMatrixPack;
			bool flag = false;
			if (!lootMatrixPack.EventEndPacific.IsNullOrEmpty() && !lootMatrixPack.EventStartPacific.IsNullOrEmpty())
			{
				DateTime t = ClientGameManager.Get().PacificNow();
				DateTime t2 = Convert.ToDateTime(lootMatrixPack.EventStartPacific);
				DateTime t3 = Convert.ToDateTime(lootMatrixPack.EventEndPacific);
				flag = (t >= t2 && t < t3);
			}
			num2 = lootMatrixPack.Prices.GetPrice(accountCurrency);
			num = CommerceClient.Get().GetLootMatrixPackPrice(lootMatrixPack.ProductCode, accountCurrency);
			text = lootMatrixPack.GetDescription();
			this.m_currencyItem.m_itemIcon.sprite = ((!flag) ? lootMatrixPack.LootMatrixPackSprite : lootMatrixPack.EventPackSprite);
		}
		else if (item.m_itemType == PurchaseItemType.Character)
		{
			text2 = StringUtil.TR("PurchaseFreelancer", "Store");
			num2 = item.m_charLink.Prices.GetPrice(accountCurrency);
			num = CommerceClient.Get().GetFreelancerPrice(item.m_charLink.m_characterType, accountCurrency);
			text = string.Format(StringUtil.TR("PurchaseFreelancerDesc", "Store"), item.m_charLink.GetDisplayName());
			this.m_currencyItem.m_itemIcon.sprite = Resources.Load<Sprite>(item.m_charLink.m_characterIconResourceString);
		}
		else if (item.m_itemType == PurchaseItemType.GGBoost)
		{
			text2 = StringUtil.TR("PurchaseGGPack", "Store");
			num2 = item.m_ggPack.Prices.GetPrice(accountCurrency);
			num = CommerceClient.Get().GetGGPackPrice(item.m_ggPack.ProductCode, accountCurrency);
			text = string.Format(StringUtil.TR("PurchaseGGBoostsDesc", "Store"), item.m_ggPack.NumberOfBoosts);
			this.m_currencyItem.m_itemIcon.sprite = item.m_ggPack.GGPackSprite;
		}
		else if (item.m_itemType == PurchaseItemType.Tint)
		{
			text2 = StringUtil.TR("PurchaseSkin", "Store");
			CharacterColor characterColor = item.m_charLink.m_skins[item.m_skinIndex].m_patterns[item.m_textureIndex].m_colors[item.m_tintIndex];
			num2 = characterColor.m_colorUnlockData.Prices.GetPrice(accountCurrency);
			num = CommerceClient.Get().GetStylePrice(item.m_charLink.m_characterType, item.m_skinIndex, item.m_textureIndex, item.m_tintIndex, accountCurrency);
			text = item.m_charLink.GetPatternColorName(item.m_skinIndex, item.m_textureIndex, item.m_tintIndex);
			this.m_currencyItem.m_itemIcon.sprite = Resources.Load<Sprite>(characterColor.m_iconResourceString);
		}
		else if (item.m_itemType == PurchaseItemType.InventoryItem)
		{
			InventoryItemTemplate itemTemplate = InventoryWideData.Get().GetItemTemplate(item.m_inventoryTemplateId);
			text2 = string.Format(StringUtil.TR("PurchaseItem", "Store"), itemTemplate.GetDisplayName());
			num = CommerceClient.Get().GetStoreItemPrice(item.m_inventoryTemplateId, accountCurrency, out num2);
			text = string.Format(StringUtil.TR("PurchaseItemDesc", "Store"), itemTemplate.GetDisplayName());
			this.m_currencyItem.m_itemIcon.sprite = Resources.Load<Sprite>(InventoryWideData.GetSpritePath(itemTemplate));
		}
		if (num == 0f)
		{
			num = num2;
		}
		else if (num2 < num)
		{
			num2 = num;
		}
		string localizedPriceString = UIStorePanel.GetLocalizedPriceString(num, accountCurrency);
		this.m_currencyItem.m_realCurrencyLabel.text = localizedPriceString;
		this.m_priceLabel.text = localizedPriceString;
		this.m_descriptionTitle.text = text;
		float num3 = Mathf.Abs(num2 - num);
		if (num3 == 0f)
		{
			UIManager.SetGameObjectActive(this.m_currencyItem.m_discountLabelContainer, false, null);
			this.m_currencyItem.m_discountPriceLabel.text = string.Empty;
		}
		else
		{
			UIManager.SetGameObjectActive(this.m_currencyItem.m_discountLabelContainer, true, null);
			this.m_currencyItem.m_discountPriceLabel.text = string.Format(StringUtil.TR("DiscountAmount", "Store"), UIStorePanel.GetLocalizedPriceString(num3, accountCurrency));
		}
		this.m_dialogTitle.text = text2;
		UIManager.SetGameObjectActive(this.m_dropDownContainer, false, null);
		UIManager.SetGameObjectActive(this.m_addPaymentBtnContainer, true, null);
		this.ClearOldPaymentList();
		UIManager.SetGameObjectActive(this.m_confirmButtonDisabledImage, true, null);
		this.m_confirmPurchaseButton.SetClickable(false);
		this.HandleRequestPaymentResponse(response);
	}

	public void CloseButton(BaseEventData data)
	{
		this.Close();
	}

	public void TryAgainButton(BaseEventData data)
	{
		UIManager.SetGameObjectActive(this.m_failToPurchaseContainer, false, null);
	}
}

using LobbyGameClientMessages;
using System;
using System.Collections.Generic;
using System.Text;
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
		m_closeBtn.callback = CloseButton;
		m_failBtnOkay.callback = TryAgainButton;
		m_successBtnOkay.callback = CloseButton;
		m_closeBtn.m_ignoreDialogboxes = true;
		UIManager.SetGameObjectActive(m_disableCloseBtn, false);
		UIManager.SetGameObjectActive(m_addPaymentDisabledImage, false);
		UIManager.SetGameObjectActive(m_confirmButtonDisabledImage, false);
		UIManager.SetGameObjectActive(m_currencyItem.m_hitBox, false);
		UIManager.SetGameObjectActive(m_processingPurchaseContainer, false);
		UIManager.SetGameObjectActive(m_failToPurchaseContainer, false);
		UIManager.SetGameObjectActive(m_succeededToPurchaseContainer, false);
		m_confirmPurchaseButton.callback = PurchaseButtonClicked;
		m_addPaymentButton.callback = AddPaymentMethodClicked;
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
		m_openedURL = true;
		string url = new StringBuilder().Append(ClientGameManager.Get().CommerceURL).Append("/account/payment/add-payment-method-flow.action").ToString();
		Application.OpenURL(url);
		m_dropdownMenu.SetListContainerVisible(false);
	}

	public void NotifyPurchaseResponse(bool successful)
	{
		UIManager.SetGameObjectActive(m_processingPurchaseContainer, false);
		UIManager.SetGameObjectActive(m_failToPurchaseContainer, !successful);
		UIManager.SetGameObjectActive(m_succeededToPurchaseContainer, successful);
		m_failPurchaseLabel.text = StringUtil.TR("PurchaseHasFailed", "Store");
	}

	public void PurchaseButtonClicked(BaseEventData data)
	{
		if (m_itemRef.m_itemType == PurchaseItemType.LootMatrixPack)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					UIStorePanel.Get().RequestToPurchaseLootMatrixPack(m_itemRef.m_lootMatrixPack, m_selectedPaymentMethod);
					return;
				}
			}
		}
		if (m_itemRef.m_itemType == PurchaseItemType.Character)
		{
			UIStorePanel.Get().RequestToPurchaseCharacterForCash(m_itemRef.m_charLink, m_selectedPaymentMethod);
			return;
		}
		if (m_itemRef.m_itemType == PurchaseItemType.GGBoost)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					UIStorePanel.Get().RequestToPurchaseGGPack(m_itemRef.m_ggPack, m_selectedPaymentMethod);
					return;
				}
			}
		}
		if (m_itemRef.m_itemType == PurchaseItemType.Tint)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					UIStorePanel.Get().RequestToPurchaseTintForCash(m_itemRef.m_charLink.m_characterType, m_itemRef.m_skinIndex, m_itemRef.m_textureIndex, m_itemRef.m_tintIndex, m_selectedPaymentMethod);
					return;
				}
			}
		}
		if (m_itemRef.m_itemType != PurchaseItemType.InventoryItem)
		{
			return;
		}
		while (true)
		{
			UIStorePanel.Get().RequestToPurchaseStoreItems(m_itemRef.m_inventoryTemplateId, m_selectedPaymentMethod);
			return;
		}
	}

	private void TimeOutPurchase()
	{
		m_processingPayment = false;
		UIManager.SetGameObjectActive(m_disableCloseBtn, false);
		UIManager.SetGameObjectActive(m_processingPurchaseContainer, false);
		UIManager.SetGameObjectActive(m_failToPurchaseContainer, true);
		m_failPurchaseLabel.text = StringUtil.TR("PurchaseHasTimedOut", "Store");
	}

	private void SetProcessingPurchasing(bool processingPurchase)
	{
		if (m_processingPayment == processingPurchase)
		{
			return;
		}
		while (true)
		{
			UIManager.SetGameObjectActive(m_confirmButtonDisabledImage, processingPurchase);
			m_confirmPurchaseButton.SetClickable(!processingPurchase);
			UIManager.SetGameObjectActive(m_disableCloseBtn, processingPurchase);
			UIManager.SetGameObjectActive(m_processingPurchaseContainer, processingPurchase);
			if (processingPurchase)
			{
				m_processStartTime = Time.time;
			}
			m_processingPayment = processingPurchase;
			return;
		}
	}

	private void OnApplicationFocus(bool focusStatus)
	{
		if (!focusStatus || !m_openedURL)
		{
			return;
		}
		while (true)
		{
			m_openedURL = false;
			ClientGameManager.Get().RequestPaymentMethods(UIStorePanel.Get().RefreshPayments);
			return;
		}
	}

	public void Update()
	{
		SetProcessingPurchasing(UIStorePanel.Get().IsWaitingForPurchaseRequest);
		if (SteamManager.UsingSteam)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					if (!UIStorePanel.Get().IsWaitingForSteamPurchaseResponse)
					{
						while (true)
						{
							switch (1)
							{
							case 0:
								break;
							default:
								if (m_processingPayment && Time.time - UIStorePanel.Get().TimeReceivedSteamPurchaseResponse > 30f)
								{
									while (true)
									{
										switch (2)
										{
										case 0:
											break;
										default:
											TimeOutPurchase();
											UIStorePanel.Get().TimeOutPurchase();
											return;
										}
									}
								}
								return;
							}
						}
					}
					return;
				}
			}
		}
		if (!m_processingPayment)
		{
			return;
		}
		while (true)
		{
			if (Time.time - m_processStartTime > 30f)
			{
				while (true)
				{
					TimeOutPurchase();
					UIStorePanel.Get().TimeOutPurchase();
					return;
				}
			}
			return;
		}
	}

	public void RefreshPayments(PaymentMethodsResponse response)
	{
		if (response.Success)
		{
			ClearOldPaymentList();
			HandleRequestPaymentResponse(response);
		}
	}

	public void HandleRequestPaymentResponse(PaymentMethodsResponse response)
	{
		if (m_dropdownItems == null)
		{
			m_dropdownItems = new List<UIStorePaymentMethodDropdownItem>();
		}
		m_dropdownItems.Clear();
		int i = 0;
		PaymentMethod paymentMethod = null;
		if (response != null && response.Success)
		{
			if (response.PaymentMethodList != null)
			{
				if (!response.PaymentMethodList.IsError && response.PaymentMethodList.PaymentMethods != null)
				{
					UIStorePaymentMethodDropdownItem uIStorePaymentMethodDropdownItem;
					for (int j = 0; j < response.PaymentMethodList.PaymentMethods.Count; m_dropdownItems.Add(uIStorePaymentMethodDropdownItem), i++, j++)
					{
						PaymentMethod paymentMethod2 = response.PaymentMethodList.PaymentMethods[j];
						uIStorePaymentMethodDropdownItem = UnityEngine.Object.Instantiate(m_paymentPrefab);
						uIStorePaymentMethodDropdownItem.transform.SetParent(m_dropDownList.transform);
						uIStorePaymentMethodDropdownItem.transform.localPosition = Vector3.zero;
						uIStorePaymentMethodDropdownItem.transform.localScale = Vector3.one;
						uIStorePaymentMethodDropdownItem.SetPaymentMethod(paymentMethod2);
						uIStorePaymentMethodDropdownItem.m_hitbox.m_ignoreDialogboxes = true;
						uIStorePaymentMethodDropdownItem.m_hitbox.callback = PaymentItemSelected;
						if (paymentMethod2.isDefault)
						{
							if (paymentMethod2.specificType != "Steam Wallet")
							{
								paymentMethod = paymentMethod2;
								uIStorePaymentMethodDropdownItem.SetText(string.Format(StringUtil.TR("PaymentInfo", "Store"), paymentMethod2.specificType, paymentMethod2.maskedPaymentInfo));
								continue;
							}
						}
						uIStorePaymentMethodDropdownItem.SetText(new StringBuilder().Append(paymentMethod2.specificType).Append(" ").Append(paymentMethod2.maskedPaymentInfo).ToString());
						if (paymentMethod2.specificType == "Steam Wallet")
						{
							paymentMethod = paymentMethod2;
						}
					}
					if (!SteamManager.UsingSteam)
					{
						UIStorePaymentMethodDropdownItem uIStorePaymentMethodDropdownItem2 = UnityEngine.Object.Instantiate(m_paymentPrefab);
						uIStorePaymentMethodDropdownItem2.transform.SetParent(m_dropDownList.transform);
						uIStorePaymentMethodDropdownItem2.transform.localPosition = Vector3.zero;
						uIStorePaymentMethodDropdownItem2.transform.localScale = Vector3.one;
						uIStorePaymentMethodDropdownItem2.SetText(StringUtil.TR("AddNewPayment", "Store"));
						uIStorePaymentMethodDropdownItem2.m_hitbox.m_ignoreDialogboxes = true;
						uIStorePaymentMethodDropdownItem2.m_hitbox.callback = AddPaymentMethodClicked;
						m_dropdownItems.Add(uIStorePaymentMethodDropdownItem2);
						if (m_addOrUpdatePaymentInfoLabel != null)
						{
							UIManager.SetGameObjectActive(m_addOrUpdatePaymentInfoLabel, true);
						}
					}
					else if (m_addOrUpdatePaymentInfoLabel != null)
					{
						UIManager.SetGameObjectActive(m_addOrUpdatePaymentInfoLabel, false);
					}
				}
			}
		}
		UIManager.SetGameObjectActive(m_addPaymentBtnContainer, i == 0);
		UIManager.SetGameObjectActive(m_dropDownContainer, i > 0);
		string dropDownText = StringUtil.TR("None", "Global");
		if (paymentMethod != null)
		{
			m_selectedPaymentMethod = paymentMethod;
			string str = string.Empty;
			if (m_selectedPaymentMethod.isDefault && m_selectedPaymentMethod.generalType != "Steam Wallet")
			{
				str = StringUtil.TR("DefaultPaymentMethod", "Store");
			}

			dropDownText = new StringBuilder().Append(m_selectedPaymentMethod.specificType).Append(" ").Append(m_selectedPaymentMethod.maskedPaymentInfo).Append(str).ToString();
			UIManager.SetGameObjectActive(m_confirmButtonDisabledImage, m_selectedPaymentMethod == null);
			m_confirmPurchaseButton.SetClickable(m_selectedPaymentMethod != null);
		}
		SetDropDownText(dropDownText);
	}

	public void SetDropDownText(string newText)
	{
		for (int i = 0; i < m_dropDownTextLabels.Length; i++)
		{
			m_dropDownTextLabels[i].text = newText;
		}
		while (true)
		{
			return;
		}
	}

	public void PaymentItemSelected(BaseEventData data)
	{
		for (int i = 0; i < m_dropdownItems.Count; i++)
		{
			if (!(m_dropdownItems[i].m_hitbox.gameObject == (data as PointerEventData).pointerCurrentRaycast.gameObject))
			{
				continue;
			}
			while (true)
			{
				m_selectedPaymentMethod = m_dropdownItems[i].GetPaymentMethod();
				m_dropdownItems[i].m_hitbox.ForceSetPointerEntered(false);
				string str = string.Empty;
				if (m_selectedPaymentMethod.isDefault)
				{
					if (m_selectedPaymentMethod.generalType != "Steam Wallet")
					{
						str = StringUtil.TR("DefaultPaymentMethod", "Store");
					}
				}
				SetDropDownText(new StringBuilder().Append(m_selectedPaymentMethod.specificType).Append(" ").Append(m_selectedPaymentMethod.maskedPaymentInfo).Append(str).ToString());
				m_dropdownMenu.ToggleListContainer(null);
				UIManager.SetGameObjectActive(m_confirmButtonDisabledImage, m_selectedPaymentMethod == null);
				m_confirmPurchaseButton.SetClickable(m_selectedPaymentMethod != null);
				return;
			}
		}
		while (true)
		{
			switch (3)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	private void ClearOldPaymentList()
	{
		UIStorePaymentMethodDropdownItem[] componentsInChildren = m_dropDownList.GetComponentsInChildren<UIStorePaymentMethodDropdownItem>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			UnityEngine.Object.Destroy(componentsInChildren[i].gameObject);
		}
		while (true)
		{
			return;
		}
	}

	public void Setup(UIPurchaseableItem item, PaymentMethodsResponse response)
	{
		m_itemRef = item;
		HydrogenConfig hydrogenConfig = HydrogenConfig.Get();
		string accountCurrency = hydrogenConfig.Ticket.AccountCurrency;
		m_descriptionCurrentCredits.text = string.Empty;
		m_descriptionAddCredits.text = string.Empty;
		m_descriptionTotalCredits.text = string.Empty;
		m_currencyItem.m_headerPriceLabel.text = string.Empty;
		float num = 0f;
		float originalPrice = 0f;
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
			originalPrice = lootMatrixPack.Prices.GetPrice(accountCurrency);
			num = CommerceClient.Get().GetLootMatrixPackPrice(lootMatrixPack.ProductCode, accountCurrency);
			text = lootMatrixPack.GetDescription();
			m_currencyItem.m_itemIcon.sprite = ((!flag) ? lootMatrixPack.LootMatrixPackSprite : lootMatrixPack.EventPackSprite);
		}
		else if (item.m_itemType == PurchaseItemType.Character)
		{
			text2 = StringUtil.TR("PurchaseFreelancer", "Store");
			originalPrice = item.m_charLink.Prices.GetPrice(accountCurrency);
			num = CommerceClient.Get().GetFreelancerPrice(item.m_charLink.m_characterType, accountCurrency);
			text = string.Format(StringUtil.TR("PurchaseFreelancerDesc", "Store"), item.m_charLink.GetDisplayName());
			m_currencyItem.m_itemIcon.sprite = Resources.Load<Sprite>(item.m_charLink.m_characterIconResourceString);
		}
		else if (item.m_itemType == PurchaseItemType.GGBoost)
		{
			text2 = StringUtil.TR("PurchaseGGPack", "Store");
			originalPrice = item.m_ggPack.Prices.GetPrice(accountCurrency);
			num = CommerceClient.Get().GetGGPackPrice(item.m_ggPack.ProductCode, accountCurrency);
			text = string.Format(StringUtil.TR("PurchaseGGBoostsDesc", "Store"), item.m_ggPack.NumberOfBoosts);
			m_currencyItem.m_itemIcon.sprite = item.m_ggPack.GGPackSprite;
		}
		else if (item.m_itemType == PurchaseItemType.Tint)
		{
			text2 = StringUtil.TR("PurchaseSkin", "Store");
			CharacterColor characterColor = item.m_charLink.m_skins[item.m_skinIndex].m_patterns[item.m_textureIndex].m_colors[item.m_tintIndex];
			originalPrice = characterColor.m_colorUnlockData.Prices.GetPrice(accountCurrency);
			num = CommerceClient.Get().GetStylePrice(item.m_charLink.m_characterType, item.m_skinIndex, item.m_textureIndex, item.m_tintIndex, accountCurrency);
			text = item.m_charLink.GetPatternColorName(item.m_skinIndex, item.m_textureIndex, item.m_tintIndex);
			m_currencyItem.m_itemIcon.sprite = Resources.Load<Sprite>(characterColor.m_iconResourceString);
		}
		else if (item.m_itemType == PurchaseItemType.InventoryItem)
		{
			InventoryItemTemplate itemTemplate = InventoryWideData.Get().GetItemTemplate(item.m_inventoryTemplateId);
			text2 = string.Format(StringUtil.TR("PurchaseItem", "Store"), itemTemplate.GetDisplayName());
			num = CommerceClient.Get().GetStoreItemPrice(item.m_inventoryTemplateId, accountCurrency, out originalPrice);
			text = string.Format(StringUtil.TR("PurchaseItemDesc", "Store"), itemTemplate.GetDisplayName());
			m_currencyItem.m_itemIcon.sprite = Resources.Load<Sprite>(InventoryWideData.GetSpritePath(itemTemplate));
		}
		if (num == 0f)
		{
			num = originalPrice;
		}
		else if (originalPrice < num)
		{
			originalPrice = num;
		}
		string localizedPriceString = UIStorePanel.GetLocalizedPriceString(num, accountCurrency);
		m_currencyItem.m_realCurrencyLabel.text = localizedPriceString;
		m_priceLabel.text = localizedPriceString;
		m_descriptionTitle.text = text;
		float num2 = Mathf.Abs(originalPrice - num);
		if (num2 == 0f)
		{
			UIManager.SetGameObjectActive(m_currencyItem.m_discountLabelContainer, false);
			m_currencyItem.m_discountPriceLabel.text = string.Empty;
		}
		else
		{
			UIManager.SetGameObjectActive(m_currencyItem.m_discountLabelContainer, true);
			m_currencyItem.m_discountPriceLabel.text = string.Format(StringUtil.TR("DiscountAmount", "Store"), UIStorePanel.GetLocalizedPriceString(num2, accountCurrency));
		}
		m_dialogTitle.text = text2;
		UIManager.SetGameObjectActive(m_dropDownContainer, false);
		UIManager.SetGameObjectActive(m_addPaymentBtnContainer, true);
		ClearOldPaymentList();
		UIManager.SetGameObjectActive(m_confirmButtonDisabledImage, true);
		m_confirmPurchaseButton.SetClickable(false);
		HandleRequestPaymentResponse(response);
	}

	public void CloseButton(BaseEventData data)
	{
		Close();
	}

	public void TryAgainButton(BaseEventData data)
	{
		UIManager.SetGameObjectActive(m_failToPurchaseContainer, false);
	}
}

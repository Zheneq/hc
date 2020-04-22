using LobbyGameClientMessages;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIStorePurchaseGameDialogBox : UIDialogBox
{
	public _ButtonSwapSprite m_closeBtn;

	public _ButtonSwapSprite m_failBtnOkay;

	public _ButtonSwapSprite m_successBtnOkay;

	public TextMeshProUGUI m_headerText;

	public Image m_disableCloseBtn;

	public TextMeshProUGUI m_descriptionTitle;

	public TextMeshProUGUI m_totalCost;

	public Image m_itemImage;

	public RectTransform m_processingPurchaseContainer;

	public RectTransform m_failToPurchaseContainer;

	public RectTransform m_succeededToPurchaseContainer;

	public Image m_addPaymentDisabledImage;

	public Image m_confirmButtonDisabledImage;

	public _ButtonSwapSprite m_addPaymentButton;

	public _ButtonSwapSprite m_confirmPurchaseButton;

	public TextMeshProUGUI m_failPurchaseLabel;

	public RectTransform m_dropDownContainer;

	public RectTransform m_addPaymentBtnContainer;

	public GridLayoutGroup m_dropDownList;

	public TextMeshProUGUI[] m_dropDownTextLabels;

	public UIStorePaymentMethodDropdownItem m_paymentPrefab;

	public _DropdownMenuList m_dropdownMenu;

	public TextMeshProUGUI m_addOrUpdatePaymentInfoLabel;

	private bool m_openedURL;

	private bool m_processingPayment;

	private float m_processStartTime;

	private GamePack m_packReference;

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
		string url = ClientGameManager.Get().CommerceURL + "/account/payment/add-payment-method-flow.action";
		Application.OpenURL(url);
		m_dropdownMenu.SetListContainerVisible(false);
	}

	public override void Close()
	{
		if (!m_processingPayment)
		{
			base.Close();
		}
	}

	public void NotifyPurchaseResponse(bool successful)
	{
		m_processingPayment = false;
		UIManager.SetGameObjectActive(m_processingPurchaseContainer, false);
		UIManager.SetGameObjectActive(m_failToPurchaseContainer, !successful);
		UIManager.SetGameObjectActive(m_succeededToPurchaseContainer, successful);
		m_failPurchaseLabel.text = StringUtil.TR("PurchaseHasFailed", "Store");
		if (successful)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
				{
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					Close();
					UITakeoverManager.Get().ShowPurchaseGameTakeover();
					bool flag = false;
					if (AppState.GetCurrent() == AppState_CharacterSelect.Get())
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
						GameManager gameManager = GameManager.Get();
						if (gameManager != null)
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
							if (gameManager.GameInfo != null)
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
								if (gameManager.GameInfo.GameConfig != null)
								{
									flag = gameManager.GameInfo.GameStatus.IsActiveStatus();
								}
							}
						}
					}
					if (!flag)
					{
						while (true)
						{
							switch (2)
							{
							case 0:
								break;
							default:
								UIFrontEnd.Get().m_frontEndNavPanel.LandingPageBtnClicked(null);
								return;
							}
						}
					}
					UIFrontEnd.Get().m_frontEndNavPanel.PlayBtnClicked(null);
					return;
				}
				}
			}
		}
		m_closeBtn.SetClickable(true);
	}

	public void PurchaseButtonClicked(BaseEventData data)
	{
		UIStorePanel.Get().RequestToPurchaseGame(m_packReference, m_selectedPaymentMethod);
	}

	private void TimeOutPurchase()
	{
		m_processingPayment = false;
		UIManager.SetGameObjectActive(m_disableCloseBtn, false);
		UIManager.SetGameObjectActive(m_processingPurchaseContainer, false);
		UIManager.SetGameObjectActive(m_failToPurchaseContainer, true);
		m_failPurchaseLabel.text = StringUtil.TR("PurchaseHasTimedOut", "Store");
		m_closeBtn.SetClickable(true);
	}

	private void SetProcessingPurchasing(bool processingPurchase)
	{
		if (m_processingPayment == processingPurchase)
		{
			return;
		}
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			UIManager.SetGameObjectActive(m_confirmButtonDisabledImage, processingPurchase);
			UIManager.SetGameObjectActive(m_disableCloseBtn, processingPurchase);
			UIManager.SetGameObjectActive(m_processingPurchaseContainer, processingPurchase);
			if (processingPurchase)
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
				m_processStartTime = Time.time;
			}
			m_processingPayment = processingPurchase;
			if (m_processingPayment)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					UIManager.SetGameObjectActive(m_closeBtn.m_hoverImage, !m_processingPayment);
					UIManager.SetGameObjectActive(m_closeBtn.m_pressedImage, !m_processingPayment);
					m_closeBtn.SetClickable(!m_processingPayment);
					return;
				}
			}
			return;
		}
	}

	private void OnApplicationFocus(bool focusStatus)
	{
		if (!focusStatus)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_openedURL)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					m_openedURL = false;
					ClientGameManager.Get().RequestPaymentMethods(UIStorePanel.Get().RefreshPayments);
					return;
				}
			}
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
				switch (6)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					if (!UIStorePanel.Get().IsWaitingForSteamPurchaseResponse)
					{
						while (true)
						{
							switch (1)
							{
							case 0:
								break;
							default:
								if (m_processingPayment)
								{
									while (true)
									{
										switch (3)
										{
										case 0:
											break;
										default:
											if (Time.time - UIStorePanel.Get().TimeReceivedSteamPurchaseResponse > 30f)
											{
												while (true)
												{
													switch (1)
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
			switch (1)
			{
			case 0:
				continue;
			}
			if (Time.time - m_processStartTime > 30f)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
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
		if (!response.Success)
		{
			return;
		}
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			ClearOldPaymentList();
			HandleRequestPaymentResponse(response);
			return;
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
		if (response != null)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (response.Success && response.PaymentMethodList != null)
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
				if (!response.PaymentMethodList.IsError)
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
					if (response.PaymentMethodList.PaymentMethods != null)
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
						UIStorePaymentMethodDropdownItem uIStorePaymentMethodDropdownItem;
						for (int j = 0; j < response.PaymentMethodList.PaymentMethods.Count; m_dropdownItems.Add(uIStorePaymentMethodDropdownItem), i++, j++)
						{
							PaymentMethod paymentMethod2 = response.PaymentMethodList.PaymentMethods[j];
							uIStorePaymentMethodDropdownItem = Object.Instantiate(m_paymentPrefab);
							uIStorePaymentMethodDropdownItem.transform.SetParent(m_dropDownList.transform);
							uIStorePaymentMethodDropdownItem.transform.localPosition = Vector3.zero;
							uIStorePaymentMethodDropdownItem.transform.localScale = Vector3.one;
							uIStorePaymentMethodDropdownItem.SetPaymentMethod(paymentMethod2);
							uIStorePaymentMethodDropdownItem.m_hitbox.m_ignoreDialogboxes = true;
							uIStorePaymentMethodDropdownItem.m_hitbox.callback = PaymentItemSelected;
							if (paymentMethod2.isDefault)
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
								if (paymentMethod2.specificType != "Steam Wallet")
								{
									paymentMethod = paymentMethod2;
									uIStorePaymentMethodDropdownItem.SetText(string.Format(StringUtil.TR("PaymentInfo", "Store"), paymentMethod2.specificType, paymentMethod2.maskedPaymentInfo));
									continue;
								}
							}
							uIStorePaymentMethodDropdownItem.SetText(paymentMethod2.specificType + " " + paymentMethod2.maskedPaymentInfo);
							if (paymentMethod2.specificType == "Steam Wallet")
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
								paymentMethod = paymentMethod2;
							}
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
						if (!SteamManager.UsingSteam)
						{
							UIStorePaymentMethodDropdownItem uIStorePaymentMethodDropdownItem2 = Object.Instantiate(m_paymentPrefab);
							uIStorePaymentMethodDropdownItem2.transform.SetParent(m_dropDownList.transform);
							uIStorePaymentMethodDropdownItem2.transform.localPosition = Vector3.zero;
							uIStorePaymentMethodDropdownItem2.transform.localScale = Vector3.one;
							uIStorePaymentMethodDropdownItem2.SetText(StringUtil.TR("AddNewPayment", "Store"));
							uIStorePaymentMethodDropdownItem2.m_hitbox.m_ignoreDialogboxes = true;
							uIStorePaymentMethodDropdownItem2.m_hitbox.callback = AddPaymentMethodClicked;
							m_dropdownItems.Add(uIStorePaymentMethodDropdownItem2);
							if (m_addOrUpdatePaymentInfoLabel != null)
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
								UIManager.SetGameObjectActive(m_addOrUpdatePaymentInfoLabel, true);
							}
						}
						else if (m_addOrUpdatePaymentInfoLabel != null)
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
							UIManager.SetGameObjectActive(m_addOrUpdatePaymentInfoLabel, false);
						}
					}
				}
			}
		}
		UIManager.SetGameObjectActive(m_addPaymentBtnContainer, i == 0);
		UIManager.SetGameObjectActive(m_dropDownContainer, i > 0);
		string dropDownText = StringUtil.TR("None", "Global");
		if (paymentMethod != null)
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
			m_selectedPaymentMethod = paymentMethod;
			string str = string.Empty;
			if (m_selectedPaymentMethod.isDefault && m_selectedPaymentMethod.generalType != "Steam Wallet")
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
				str = StringUtil.TR("DefaultPaymentMethod", "Store");
			}
			dropDownText = m_selectedPaymentMethod.specificType + " " + m_selectedPaymentMethod.maskedPaymentInfo + str;
			UIManager.SetGameObjectActive(m_confirmButtonDisabledImage, m_selectedPaymentMethod == null);
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
			switch (4)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
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
				switch (3)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				m_selectedPaymentMethod = m_dropdownItems[i].GetPaymentMethod();
				m_dropdownItems[i].m_hitbox.ForceSetPointerEntered(false);
				string str = string.Empty;
				if (m_selectedPaymentMethod.isDefault && m_selectedPaymentMethod.generalType != "Steam Wallet")
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
					str = StringUtil.TR("DefaultPaymentMethod", "Store");
				}
				SetDropDownText(m_selectedPaymentMethod.specificType + " " + m_selectedPaymentMethod.maskedPaymentInfo + str);
				m_dropdownMenu.ToggleListContainer(null);
				UIManager.SetGameObjectActive(m_confirmButtonDisabledImage, m_selectedPaymentMethod == null);
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
			Object.Destroy(componentsInChildren[i].gameObject);
		}
	}

	public void Setup(UIPurchaseableItem item, PaymentMethodsResponse response)
	{
		m_packReference = item.m_gamePack;
		HydrogenConfig hydrogenConfig = HydrogenConfig.Get();
		string accountCurrency = hydrogenConfig.Ticket.AccountCurrency;
		bool hasPurchasedGame = ClientGameManager.Get().HasPurchasedGame;
		TextMeshProUGUI headerText = m_headerText;
		object term;
		if (hasPurchasedGame)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			term = "PurchaseUpgrade";
		}
		else
		{
			term = "PurchaseGame";
		}
		headerText.text = StringUtil.TR((string)term, "Store");
		float num2;
		float originalPrice;
		if (hasPurchasedGame)
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
			int highestPurchasedGamePack = ClientGameManager.Get().HighestPurchasedGamePack;
			GamePackUpgrade gamePackUpgrade = null;
			int num = 0;
			while (true)
			{
				if (num < m_packReference.Upgrades.Length)
				{
					if (m_packReference.Upgrades[num].AlreadyOwnedGamePack == highestPurchasedGamePack)
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
						gamePackUpgrade = m_packReference.Upgrades[num];
						break;
					}
					num++;
					continue;
				}
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				break;
			}
			if (gamePackUpgrade == null)
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
				Log.Error("No upgrade path specified from " + highestPurchasedGamePack + " to " + m_packReference.Index);
				num2 = 0f;
			}
			else
			{
				num2 = CommerceClient.Get().GetGamePackPrice(gamePackUpgrade.ProductCode, accountCurrency, out originalPrice);
			}
		}
		else
		{
			num2 = CommerceClient.Get().GetGamePackPrice(m_packReference.ProductCode, accountCurrency, out originalPrice);
		}
		string text = UIStorePanel.GetLocalizedPriceString(num2, accountCurrency);
		if (num2 == 0f)
		{
			text = "--";
		}
		m_descriptionTitle.text = string.Format(StringUtil.TR("Edition", "Store"), m_packReference.GetEditionName());
		m_totalCost.text = text;
		m_itemImage.sprite = m_packReference.Sprite;
		UIManager.SetGameObjectActive(m_dropDownContainer, false);
		UIManager.SetGameObjectActive(m_addPaymentBtnContainer, true);
		ClearOldPaymentList();
		UIManager.SetGameObjectActive(m_confirmButtonDisabledImage, true);
		HandleRequestPaymentResponse(response);
	}

	public void CloseButton(BaseEventData data)
	{
		if (!m_processingPayment)
		{
			Close();
		}
	}

	public void TryAgainButton(BaseEventData data)
	{
		UIManager.SetGameObjectActive(m_failToPurchaseContainer, false);
	}
}

using System;
using System.Collections.Generic;
using LobbyGameClientMessages;
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
		this.m_closeBtn.callback = new _ButtonSwapSprite.ButtonClickCallback(this.CloseButton);
		this.m_failBtnOkay.callback = new _ButtonSwapSprite.ButtonClickCallback(this.TryAgainButton);
		this.m_successBtnOkay.callback = new _ButtonSwapSprite.ButtonClickCallback(this.CloseButton);
		this.m_closeBtn.m_ignoreDialogboxes = true;
		UIManager.SetGameObjectActive(this.m_disableCloseBtn, false, null);
		UIManager.SetGameObjectActive(this.m_addPaymentDisabledImage, false, null);
		UIManager.SetGameObjectActive(this.m_confirmButtonDisabledImage, false, null);
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

	public override void Close()
	{
		if (!this.m_processingPayment)
		{
			base.Close();
		}
	}

	public void NotifyPurchaseResponse(bool successful)
	{
		this.m_processingPayment = false;
		UIManager.SetGameObjectActive(this.m_processingPurchaseContainer, false, null);
		UIManager.SetGameObjectActive(this.m_failToPurchaseContainer, !successful, null);
		UIManager.SetGameObjectActive(this.m_succeededToPurchaseContainer, successful, null);
		this.m_failPurchaseLabel.text = StringUtil.TR("PurchaseHasFailed", "Store");
		if (successful)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIStorePurchaseGameDialogBox.NotifyPurchaseResponse(bool)).MethodHandle;
			}
			this.Close();
			UITakeoverManager.Get().ShowPurchaseGameTakeover();
			bool flag = false;
			if (AppState.GetCurrent() == AppState_CharacterSelect.Get())
			{
				for (;;)
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
					for (;;)
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
						for (;;)
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
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				UIFrontEnd.Get().m_frontEndNavPanel.LandingPageBtnClicked(null);
			}
			else
			{
				UIFrontEnd.Get().m_frontEndNavPanel.PlayBtnClicked(null);
			}
		}
		else
		{
			this.m_closeBtn.SetClickable(true);
		}
	}

	public void PurchaseButtonClicked(BaseEventData data)
	{
		UIStorePanel.Get().RequestToPurchaseGame(this.m_packReference, this.m_selectedPaymentMethod);
	}

	private void TimeOutPurchase()
	{
		this.m_processingPayment = false;
		UIManager.SetGameObjectActive(this.m_disableCloseBtn, false, null);
		UIManager.SetGameObjectActive(this.m_processingPurchaseContainer, false, null);
		UIManager.SetGameObjectActive(this.m_failToPurchaseContainer, true, null);
		this.m_failPurchaseLabel.text = StringUtil.TR("PurchaseHasTimedOut", "Store");
		this.m_closeBtn.SetClickable(true);
	}

	private void SetProcessingPurchasing(bool processingPurchase)
	{
		if (this.m_processingPayment != processingPurchase)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIStorePurchaseGameDialogBox.SetProcessingPurchasing(bool)).MethodHandle;
			}
			UIManager.SetGameObjectActive(this.m_confirmButtonDisabledImage, processingPurchase, null);
			UIManager.SetGameObjectActive(this.m_disableCloseBtn, processingPurchase, null);
			UIManager.SetGameObjectActive(this.m_processingPurchaseContainer, processingPurchase, null);
			if (processingPurchase)
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				this.m_processStartTime = Time.time;
			}
			this.m_processingPayment = processingPurchase;
			if (this.m_processingPayment)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				UIManager.SetGameObjectActive(this.m_closeBtn.m_hoverImage, !this.m_processingPayment, null);
				UIManager.SetGameObjectActive(this.m_closeBtn.m_pressedImage, !this.m_processingPayment, null);
				this.m_closeBtn.SetClickable(!this.m_processingPayment);
			}
		}
	}

	private void OnApplicationFocus(bool focusStatus)
	{
		if (focusStatus)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIStorePurchaseGameDialogBox.OnApplicationFocus(bool)).MethodHandle;
			}
			if (this.m_openedURL)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				this.m_openedURL = false;
				ClientGameManager.Get().RequestPaymentMethods(new Action<PaymentMethodsResponse>(UIStorePanel.Get().RefreshPayments));
			}
		}
	}

	public void Update()
	{
		this.SetProcessingPurchasing(UIStorePanel.Get().IsWaitingForPurchaseRequest);
		if (SteamManager.UsingSteam)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIStorePurchaseGameDialogBox.Update()).MethodHandle;
			}
			if (!UIStorePanel.Get().IsWaitingForSteamPurchaseResponse)
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (this.m_processingPayment)
				{
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					if (Time.time - UIStorePanel.Get().TimeReceivedSteamPurchaseResponse > 30f)
					{
						for (;;)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							break;
						}
						this.TimeOutPurchase();
						UIStorePanel.Get().TimeOutPurchase();
					}
				}
			}
		}
		else if (this.m_processingPayment)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (Time.time - this.m_processStartTime > 30f)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				this.TimeOutPurchase();
				UIStorePanel.Get().TimeOutPurchase();
			}
		}
	}

	public void RefreshPayments(PaymentMethodsResponse response)
	{
		if (response.Success)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIStorePurchaseGameDialogBox.RefreshPayments(PaymentMethodsResponse)).MethodHandle;
			}
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
		if (response != null)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIStorePurchaseGameDialogBox.HandleRequestPaymentResponse(PaymentMethodsResponse)).MethodHandle;
			}
			if (response.Success && response.PaymentMethodList != null)
			{
				for (;;)
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
					for (;;)
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
						for (;;)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
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
							for (;;)
							{
								switch (3)
								{
								case 0:
									continue;
								}
								break;
							}
							if (!(paymentMethod2.specificType != "Steam Wallet"))
							{
								goto IL_17A;
							}
							paymentMethod = paymentMethod2;
							uistorePaymentMethodDropdownItem.SetText(string.Format(StringUtil.TR("PaymentInfo", "Store"), paymentMethod2.specificType, paymentMethod2.maskedPaymentInfo));
							IL_1B7:
							this.m_dropdownItems.Add(uistorePaymentMethodDropdownItem);
							num++;
							i++;
							continue;
							IL_17A:
							uistorePaymentMethodDropdownItem.SetText(paymentMethod2.specificType + " " + paymentMethod2.maskedPaymentInfo);
							if (paymentMethod2.specificType == "Steam Wallet")
							{
								for (;;)
								{
									switch (4)
									{
									case 0:
										continue;
									}
									break;
								}
								paymentMethod = paymentMethod2;
								goto IL_1B7;
							}
							goto IL_1B7;
						}
						for (;;)
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
								for (;;)
								{
									switch (5)
									{
									case 0:
										continue;
									}
									break;
								}
								UIManager.SetGameObjectActive(this.m_addOrUpdatePaymentInfoLabel, true, null);
							}
						}
						else if (this.m_addOrUpdatePaymentInfoLabel != null)
						{
							for (;;)
							{
								switch (6)
								{
								case 0:
									continue;
								}
								break;
							}
							UIManager.SetGameObjectActive(this.m_addOrUpdatePaymentInfoLabel, false, null);
						}
					}
				}
			}
		}
		UIManager.SetGameObjectActive(this.m_addPaymentBtnContainer, num == 0, null);
		UIManager.SetGameObjectActive(this.m_dropDownContainer, num > 0, null);
		string dropDownText = StringUtil.TR("None", "Global");
		if (paymentMethod != null)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			this.m_selectedPaymentMethod = paymentMethod;
			string str = string.Empty;
			if (this.m_selectedPaymentMethod.isDefault && this.m_selectedPaymentMethod.generalType != "Steam Wallet")
			{
				for (;;)
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
			dropDownText = this.m_selectedPaymentMethod.specificType + " " + this.m_selectedPaymentMethod.maskedPaymentInfo + str;
			UIManager.SetGameObjectActive(this.m_confirmButtonDisabledImage, this.m_selectedPaymentMethod == null, null);
		}
		this.SetDropDownText(dropDownText);
	}

	public void SetDropDownText(string newText)
	{
		for (int i = 0; i < this.m_dropDownTextLabels.Length; i++)
		{
			this.m_dropDownTextLabels[i].text = newText;
		}
		for (;;)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			break;
		}
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(UIStorePurchaseGameDialogBox.SetDropDownText(string)).MethodHandle;
		}
	}

	public void PaymentItemSelected(BaseEventData data)
	{
		for (int i = 0; i < this.m_dropdownItems.Count; i++)
		{
			if (this.m_dropdownItems[i].m_hitbox.gameObject == (data as PointerEventData).pointerCurrentRaycast.gameObject)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(UIStorePurchaseGameDialogBox.PaymentItemSelected(BaseEventData)).MethodHandle;
				}
				this.m_selectedPaymentMethod = this.m_dropdownItems[i].GetPaymentMethod();
				this.m_dropdownItems[i].m_hitbox.ForceSetPointerEntered(false);
				string str = string.Empty;
				if (this.m_selectedPaymentMethod.isDefault && this.m_selectedPaymentMethod.generalType != "Steam Wallet")
				{
					for (;;)
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
				this.SetDropDownText(this.m_selectedPaymentMethod.specificType + " " + this.m_selectedPaymentMethod.maskedPaymentInfo + str);
				this.m_dropdownMenu.ToggleListContainer(null);
				UIManager.SetGameObjectActive(this.m_confirmButtonDisabledImage, this.m_selectedPaymentMethod == null, null);
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
		this.m_packReference = item.m_gamePack;
		HydrogenConfig hydrogenConfig = HydrogenConfig.Get();
		string accountCurrency = hydrogenConfig.Ticket.AccountCurrency;
		bool hasPurchasedGame = ClientGameManager.Get().HasPurchasedGame;
		TMP_Text headerText = this.m_headerText;
		string term;
		if (hasPurchasedGame)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIStorePurchaseGameDialogBox.Setup(UIPurchaseableItem, PaymentMethodsResponse)).MethodHandle;
			}
			term = "PurchaseUpgrade";
		}
		else
		{
			term = "PurchaseGame";
		}
		headerText.text = StringUtil.TR(term, "Store");
		float num;
		if (hasPurchasedGame)
		{
			for (;;)
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
			for (int i = 0; i < this.m_packReference.Upgrades.Length; i++)
			{
				if (this.m_packReference.Upgrades[i].AlreadyOwnedGamePack == highestPurchasedGamePack)
				{
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					gamePackUpgrade = this.m_packReference.Upgrades[i];
					IL_E4:
					if (gamePackUpgrade == null)
					{
						for (;;)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							break;
						}
						Log.Error(string.Concat(new object[]
						{
							"No upgrade path specified from ",
							highestPurchasedGamePack,
							" to ",
							this.m_packReference.Index
						}), new object[0]);
						num = 0f;
					}
					else
					{
						float num2;
						num = CommerceClient.Get().GetGamePackPrice(gamePackUpgrade.ProductCode, accountCurrency, out num2);
					}
					goto IL_176;
				}
			}
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				goto IL_E4;
			}
		}
		else
		{
			float num2;
			num = CommerceClient.Get().GetGamePackPrice(this.m_packReference.ProductCode, accountCurrency, out num2);
		}
		IL_176:
		string text = UIStorePanel.GetLocalizedPriceString(num, accountCurrency);
		if (num == 0f)
		{
			text = "--";
		}
		this.m_descriptionTitle.text = string.Format(StringUtil.TR("Edition", "Store"), this.m_packReference.GetEditionName());
		this.m_totalCost.text = text;
		this.m_itemImage.sprite = this.m_packReference.Sprite;
		UIManager.SetGameObjectActive(this.m_dropDownContainer, false, null);
		UIManager.SetGameObjectActive(this.m_addPaymentBtnContainer, true, null);
		this.ClearOldPaymentList();
		UIManager.SetGameObjectActive(this.m_confirmButtonDisabledImage, true, null);
		this.HandleRequestPaymentResponse(response);
	}

	public void CloseButton(BaseEventData data)
	{
		if (!this.m_processingPayment)
		{
			this.Close();
		}
	}

	public void TryAgainButton(BaseEventData data)
	{
		UIManager.SetGameObjectActive(this.m_failToPurchaseContainer, false, null);
	}
}

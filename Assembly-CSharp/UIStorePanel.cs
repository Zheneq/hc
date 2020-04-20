using System;
using LobbyGameClientMessages;
using UnityEngine;
using UnityEngine.UI;

public class UIStorePanel : UIScene
{
	public RectTransform m_backgroundContainer;

	public RectTransform m_container;

	public RectTransform m_secondaryNavContainer;

	public UIStoreNavBtn m_accountBtn;

	public UIStoreNavBtn m_freelancerBtn;

	public UIStoreAccountPanel m_accountPanel;

	public UIStoreFreelancerPanel m_freelancerPanel;

	public const float PURCHASE_TIMEOUT_DURATION = 30f;

	private UIStoreNavBtn[] m_menuBtns;

	private UIStoreBasePanel[] m_panels;

	private static UIStorePanel s_instance;

	private bool m_waitingForPurchaseRequest;

	private UIStorePurchaseForCashDialogBox m_cashPurchaseDialogBox;

	private UIStorePurchaseGameDialogBox m_gamePurchaseDialogBox;

	private UIStorePurchaseItemDialogBox m_itemDialogBox;

	private PaymentMethodsResponse m_paymentResponse;

	private UIStorePanel.PurchaseCharacterCallback m_charResponseCallback;

	private bool m_isOpen;

	private bool m_isViewingHero;

	public static UIStorePanel Get()
	{
		return UIStorePanel.s_instance;
	}

	public override SceneType GetSceneType()
	{
		return SceneType.Collections;
	}

	public static string FormatIntToString(int value, bool localize = false)
	{
		string text = string.Empty;
		text = value.ToString().Reverse();
		int num = 0;
		for (int i = 0; i < text.Length; i++)
		{
			num++;
			if (num == 3)
			{
				if (i + 1 != text.Length)
				{
					num = 0;
					string text2 = (!localize) ? "," : StringUtil.TR("ThousandsSeparator", "Global");
					if (text2.Length > 1)
					{
						text2 = ",";
					}
					text = text.Insert(i + 1, text2);
					i++;
				}
			}
		}
		return text.Reverse();
	}

	public static string GetLocalizedPriceString(float price, string currencyCode)
	{
		string empty = string.Empty;
		float num = Mathf.Abs(price);
		int num2 = Mathf.FloorToInt(num);
		int num3 = Mathf.RoundToInt(num * 100f) % 0x64;
		string arg = num2.ToString();
		string text = num3.ToString();
		if (num3 < 0xA)
		{
			text = "0" + text;
		}
		string arg2 = string.Empty;
		if (currencyCode == "USD")
		{
			arg2 = "$";
		}
		else if (currencyCode == "EUR")
		{
			arg2 = "€";
		}
		else if (currencyCode == "GBP")
		{
			arg2 = "£";
		}
		else if (currencyCode == "BRL")
		{
			arg2 = "R$";
		}
		else if (currencyCode == "RUB")
		{
			arg2 = "RUB";
		}
		else
		{
			arg2 = "$";
		}
		string result;
		if (num3 > 0)
		{
			result = string.Format(StringUtil.TR("RealCurrencyDisplay", "Global"), arg2, arg, text);
		}
		else
		{
			result = string.Format(StringUtil.TR("RealCurrencyNoDecimalDisplay", "Global"), arg2, arg);
		}
		return result;
	}

	public void OpenFreelancerPage(CharacterResourceLink charLink)
	{
		UIStoreViewHeroPage.Get().Setup(charLink);
		this.SetMainPanelVisibility(false);
		UIStoreViewHeroPage.Get().SetVisible(true);
		this.m_isViewingHero = true;
	}

	public bool IsWaitingForPurchaseRequest
	{
		get
		{
			return this.m_waitingForPurchaseRequest;
		}
	}

	public bool IsWaitingForSteamPurchaseResponse { get; private set; }

	public float TimeReceivedSteamPurchaseResponse { get; private set; }

	public override void Awake()
	{
		UIStorePanel.s_instance = this;
		this.m_menuBtns = this.m_secondaryNavContainer.GetComponentsInChildren<UIStoreNavBtn>(true);
		this.m_panels = new UIStoreBasePanel[]
		{
			this.m_freelancerPanel,
			this.m_accountPanel
		};
		this.m_freelancerPanel.ScreenType = UIStorePanel.StorePanelScreen.Freelancer;
		this.m_accountPanel.ScreenType = UIStorePanel.StorePanelScreen.Account;
		if (this.m_accountBtn != null)
		{
			StaggerComponent.SetStaggerComponent(this.m_accountBtn.gameObject, true, true);
		}
		if (this.m_freelancerBtn != null)
		{
			StaggerComponent.SetStaggerComponent(this.m_freelancerBtn.gameObject, true, true);
		}
		if (HitchDetector.Get() != null)
		{
			HitchDetector.Get().AddNewLayoutGroup(this.m_secondaryNavContainer.GetComponentInChildren<LayoutGroup>(true));
		}
		this.NotifyNavBtnClicked(this.m_freelancerBtn);
		base.Awake();
	}

	private void OnDestroy()
	{
		UIStorePanel.s_instance = null;
	}

	public void TimeOutPurchase()
	{
		this.m_waitingForPurchaseRequest = false;
	}

	public void NotifySteamResponseReceived()
	{
		this.IsWaitingForSteamPurchaseResponse = false;
		this.TimeReceivedSteamPurchaseResponse = Time.time;
	}

	public void HandlePendingPurchaseResult(PendingPurchaseResult resultMsg)
	{
		if (resultMsg.Details.purchaseType == PurchaseType.Game)
		{
			this.m_waitingForPurchaseRequest = false;
			if (this.m_gamePurchaseDialogBox != null)
			{
				this.m_gamePurchaseDialogBox.NotifyPurchaseResponse(resultMsg.Result == PurchaseResult.Success);
			}
		}
		else
		{
			if (resultMsg.Details.purchaseType != PurchaseType.LootMatrixPack)
			{
				if (resultMsg.Details.purchaseType != PurchaseType.Character)
				{
					if (resultMsg.Details.purchaseType != PurchaseType.GGPack)
					{
						if (resultMsg.Details.purchaseType != PurchaseType.Tint)
						{
							if (resultMsg.Details.purchaseType != PurchaseType.InventoryItem)
							{
								return;
							}
						}
					}
				}
			}
			this.m_waitingForPurchaseRequest = false;
			if (this.m_cashPurchaseDialogBox != null)
			{
				this.m_cashPurchaseDialogBox.NotifyPurchaseResponse(resultMsg.Result == PurchaseResult.Success);
			}
		}
	}

	public void HandlePurchaseGameResponse(PurchaseGameResponse response)
	{
		if (response.Result != PurchaseResult.Processing)
		{
			this.m_waitingForPurchaseRequest = false;
			if (this.m_gamePurchaseDialogBox != null)
			{
				this.m_gamePurchaseDialogBox.NotifyPurchaseResponse(response.Success);
			}
		}
	}

	public void RequestToPurchaseGame(GamePack gamePack, PaymentMethod paymentInfo)
	{
		this.m_waitingForPurchaseRequest = true;
		if (SteamManager.UsingSteam)
		{
			this.IsWaitingForSteamPurchaseResponse = true;
		}
		ClientGameManager.Get().PurchaseGame(gamePack.Index, paymentInfo.id, new Action<PurchaseGameResponse>(this.HandlePurchaseGameResponse));
	}

	public void HandlePurchaseLootMatrixPackResponse(PurchaseLootMatrixPackResponse response)
	{
		if (response.Result != PurchaseResult.Processing)
		{
			this.m_waitingForPurchaseRequest = false;
			if (this.m_cashPurchaseDialogBox != null)
			{
				this.m_cashPurchaseDialogBox.NotifyPurchaseResponse(response.Success);
			}
		}
	}

	public void RequestToPurchaseLootMatrixPack(LootMatrixPack pack, PaymentMethod paymentInfo)
	{
		this.m_waitingForPurchaseRequest = true;
		if (SteamManager.UsingSteam)
		{
			this.IsWaitingForSteamPurchaseResponse = true;
		}
		ClientGameManager.Get().PurchaseLootMatrixPack(pack.Index, paymentInfo.id, new Action<PurchaseLootMatrixPackResponse>(this.HandlePurchaseLootMatrixPackResponse));
	}

	public void HandlePurchaseCharacterForCashResponse(PurchaseCharacterForCashResponse response)
	{
		if (response.Result != PurchaseResult.Processing)
		{
			this.m_waitingForPurchaseRequest = false;
			if (this.m_cashPurchaseDialogBox != null)
			{
				this.m_cashPurchaseDialogBox.NotifyPurchaseResponse(response.Success);
			}
			if (this.m_charResponseCallback != null)
			{
				this.m_charResponseCallback(response.Success, response.Result, response.CharacterType);
				this.m_charResponseCallback = null;
			}
		}
	}

	public void RequestToPurchaseCharacterForCash(CharacterResourceLink link, PaymentMethod paymentInfo)
	{
		this.m_waitingForPurchaseRequest = true;
		if (SteamManager.UsingSteam)
		{
			this.IsWaitingForSteamPurchaseResponse = true;
		}
		ClientGameManager.Get().PurchaseCharacterForCash(link.m_characterType, paymentInfo.id, new Action<PurchaseCharacterForCashResponse>(this.HandlePurchaseCharacterForCashResponse));
	}

	public void HandlePurchaseGGPackResponse(PurchaseGGPackResponse response)
	{
		if (response.Result != PurchaseResult.Processing)
		{
			this.m_waitingForPurchaseRequest = false;
			if (this.m_cashPurchaseDialogBox != null)
			{
				this.m_cashPurchaseDialogBox.NotifyPurchaseResponse(response.Success);
			}
		}
	}

	public void RequestToPurchaseGGPack(GGPack ggPack, PaymentMethod paymentInfo)
	{
		this.m_waitingForPurchaseRequest = true;
		if (SteamManager.UsingSteam)
		{
			this.IsWaitingForSteamPurchaseResponse = true;
		}
		ClientGameManager.Get().PurchaseGGPack(ggPack.Index, paymentInfo.id, new Action<PurchaseGGPackResponse>(this.HandlePurchaseGGPackResponse));
	}

	public void HandlePurchaseSkinResponse(PurchaseSkinResponse response)
	{
		if (response.Result != PurchaseResult.Processing)
		{
			this.m_waitingForPurchaseRequest = false;
			if (this.m_itemDialogBox != null)
			{
				this.m_itemDialogBox.NotifyPurchaseResponse(response.Success);
			}
			if (UICharacterSelectScreenController.Get() != null)
			{
				UICharacterSelectScreenController.Get().PurchaseCharacterSkinResponseHandler(response);
			}
		}
	}

	public void HandlePurchaseTextureResponse(PurchaseTextureResponse response)
	{
		if (response.Result != PurchaseResult.Processing)
		{
			this.m_waitingForPurchaseRequest = false;
			if (this.m_itemDialogBox != null)
			{
				this.m_itemDialogBox.NotifyPurchaseResponse(response.Success);
			}
		}
	}

	public void HandlePurchaseTintResponse(PurchaseTintResponse response)
	{
		if (response.Result != PurchaseResult.Processing)
		{
			this.m_waitingForPurchaseRequest = false;
			if (this.m_itemDialogBox != null)
			{
				this.m_itemDialogBox.NotifyPurchaseResponse(response.Success);
			}
			if (UICharacterSelectScreenController.Get() != null)
			{
				UICharacterSelectScreenController.Get().PurchaseCharacterTintResponseHandler(response);
			}
		}
	}

	public void HandlePurchaseTintForCashResponse(PurchaseTintForCashResponse response)
	{
		if (response.Result != PurchaseResult.Processing)
		{
			this.m_waitingForPurchaseRequest = false;
			if (this.m_cashPurchaseDialogBox != null)
			{
				this.m_cashPurchaseDialogBox.NotifyPurchaseResponse(response.Success);
			}
			if (UICharacterSelectScreenController.Get() != null)
			{
				UICharacterSelectScreenController.Get().PurchaseCharacterTintResponseHandler(response);
			}
		}
	}

	public void RequestToPurchaseTintForCash(CharacterType type, int skin, int texture, int tint, PaymentMethod paymentInfo)
	{
		this.m_waitingForPurchaseRequest = true;
		if (SteamManager.UsingSteam)
		{
			this.IsWaitingForSteamPurchaseResponse = true;
		}
		ClientGameManager.Get().PurchaseTintForCash(type, skin, texture, tint, paymentInfo.id, new Action<PurchaseTintForCashResponse>(this.HandlePurchaseTintForCashResponse));
	}

	public void HandlePurchaseStoreItemsResponse(PurchaseStoreItemForCashResponse response)
	{
		if (response.Result != PurchaseResult.Processing)
		{
			this.m_waitingForPurchaseRequest = false;
			if (this.m_cashPurchaseDialogBox != null)
			{
				this.m_cashPurchaseDialogBox.NotifyPurchaseResponse(response.Success);
			}
		}
	}

	public void RequestToPurchaseStoreItems(int inventoryTemplateId, PaymentMethod paymentInfo)
	{
		this.m_waitingForPurchaseRequest = true;
		if (SteamManager.UsingSteam)
		{
			this.IsWaitingForSteamPurchaseResponse = true;
		}
		ClientGameManager.Get().PurchaseStoreItemForCash(inventoryTemplateId, paymentInfo.id, new Action<PurchaseStoreItemForCashResponse>(this.HandlePurchaseStoreItemsResponse));
	}

	public void HandlePurchaseCharacterResponse(PurchaseCharacterResponse response)
	{
		if (response.Result != PurchaseResult.Processing)
		{
			this.m_waitingForPurchaseRequest = false;
			if (this.m_itemDialogBox != null)
			{
				this.m_itemDialogBox.NotifyPurchaseResponse(response.Success);
			}
			if (this.m_charResponseCallback != null)
			{
				this.m_charResponseCallback(response.Success, response.Result, response.CharacterType);
				this.m_charResponseCallback = null;
			}
		}
	}

	public void HandlePurchaseTauntResponse(PurchaseTauntResponse response)
	{
		if (response.Result != PurchaseResult.Processing)
		{
			this.m_waitingForPurchaseRequest = false;
			if (this.m_itemDialogBox != null)
			{
				this.m_itemDialogBox.NotifyTauntPurchaseResponse(response.Success);
			}
		}
	}

	public void HandlePurchaseInventoryItemResponse(PurchaseInventoryItemResponse response)
	{
		if (response.Result != PurchaseResult.Processing)
		{
			this.m_waitingForPurchaseRequest = false;
			if (this.m_itemDialogBox != null)
			{
				this.m_itemDialogBox.NotifyPurchaseInventoryItemResponse(response.Success);
			}
			if (InventoryWideData.Get().GetItemTemplate(response.InventoryItemID).Type != InventoryItemType.Skin)
			{
				if (InventoryWideData.Get().GetItemTemplate(response.InventoryItemID).Type != InventoryItemType.Style)
				{
					if (InventoryWideData.Get().GetItemTemplate(response.InventoryItemID).Type != InventoryItemType.Texture)
					{
						return;
					}
				}
			}
			if (UICharacterSelectScreenController.Get() != null)
			{
				UICharacterSelectScreenController.Get().UpdateSkinsPanel();
			}
		}
	}

	public void HandlePurchaseTitleResponse(PurchaseTitleResponse response)
	{
		if (response.Result != PurchaseResult.Processing)
		{
			this.m_waitingForPurchaseRequest = false;
			if (this.m_itemDialogBox != null)
			{
				this.m_itemDialogBox.NotifyPurchaseResponse(response.Success);
			}
		}
	}

	public void HandlePurchaseBannerResponse(PurchaseBannerBackgroundResponse response)
	{
		if (response.Result != PurchaseResult.Processing)
		{
			this.m_waitingForPurchaseRequest = false;
			if (this.m_itemDialogBox != null)
			{
				this.m_itemDialogBox.NotifyPurchaseResponse(response.Success);
			}
		}
	}

	public void HandlePurchaseEmblemResponse(PurchaseBannerForegroundResponse response)
	{
		if (response.Result != PurchaseResult.Processing)
		{
			this.m_waitingForPurchaseRequest = false;
			if (this.m_itemDialogBox != null)
			{
				this.m_itemDialogBox.NotifyPurchaseResponse(response.Success);
			}
		}
	}

	public void HandlePurchaseEmoticonResponse(PurchaseChatEmojiResponse response)
	{
		if (response.Result != PurchaseResult.Processing)
		{
			this.m_waitingForPurchaseRequest = false;
			if (this.m_itemDialogBox != null)
			{
				this.m_itemDialogBox.NotifyPurchaseResponse(response.Success);
			}
		}
	}

	public void HandlePurchaseAbilityVfxResponse(PurchaseAbilityVfxResponse response)
	{
		if (response.Result != PurchaseResult.Processing)
		{
			this.m_waitingForPurchaseRequest = false;
			if (this.m_itemDialogBox != null)
			{
				this.m_itemDialogBox.NotifyPurchaseResponse(response.Success);
			}
		}
	}

	public void HandlePurchaseOverconResponse(PurchaseOverconResponse response)
	{
		if (response.Result != PurchaseResult.Processing)
		{
			this.m_waitingForPurchaseRequest = false;
			if (this.m_itemDialogBox != null)
			{
				this.m_itemDialogBox.NotifyPurchaseResponse(response.Success);
			}
		}
	}

	public void HandlePurchaseLoadingScreenBackgroundResponse(PurchaseLoadingScreenBackgroundResponse response)
	{
		if (response.Result != PurchaseResult.Processing)
		{
			this.m_waitingForPurchaseRequest = false;
			if (this.m_itemDialogBox != null)
			{
				this.m_itemDialogBox.NotifyPurchaseResponse(response.Success);
			}
		}
	}

	public void RequestToPurchaseSkin(CurrencyType currency, CharacterType type, int skin)
	{
		this.m_waitingForPurchaseRequest = true;
		ClientGameManager.Get().PurchaseSkin(currency, type, skin, new Action<PurchaseSkinResponse>(this.HandlePurchaseSkinResponse));
	}

	public void RequestToPurchaseTexture(CurrencyType currency, CharacterType type, int skin, int texture)
	{
		this.m_waitingForPurchaseRequest = true;
		ClientGameManager.Get().PurchaseTexture(currency, type, skin, texture, new Action<PurchaseTextureResponse>(this.HandlePurchaseTextureResponse));
	}

	public void RequestToPurchaseTint(CurrencyType currency, CharacterType type, int skin, int texture, int tint)
	{
		this.m_waitingForPurchaseRequest = true;
		ClientGameManager.Get().PurchaseTint(currency, type, skin, texture, tint, new Action<PurchaseTintResponse>(this.HandlePurchaseTintResponse));
	}

	public void RequestToPurchaseCharacter(CurrencyType currency, CharacterType type)
	{
		this.m_waitingForPurchaseRequest = true;
		ClientGameManager.Get().PurchaseCharacter(currency, type, new Action<PurchaseCharacterResponse>(this.HandlePurchaseCharacterResponse));
	}

	public void RequestToPurchaseTaunt(CurrencyType currency, CharacterType type, int tauntIndex)
	{
		this.m_waitingForPurchaseRequest = true;
		ClientGameManager.Get().PurchaseTaunt(currency, type, tauntIndex, new Action<PurchaseTauntResponse>(this.HandlePurchaseTauntResponse));
	}

	public void RequestToPurchaseInventoryItem(int templateId, CurrencyType currencyType)
	{
		this.m_waitingForPurchaseRequest = true;
		ClientGameManager.Get().PurchaseInventoryItem(templateId, currencyType, new Action<PurchaseInventoryItemResponse>(this.HandlePurchaseInventoryItemResponse));
	}

	public void RequestToPurchaseTitle(int titleId, CurrencyType currencyType)
	{
		this.m_waitingForPurchaseRequest = true;
		ClientGameManager.Get().PurchaseTitle(titleId, currencyType, new Action<PurchaseTitleResponse>(this.HandlePurchaseTitleResponse));
	}

	public void RequestToPurchaseBanner(int bannerId, CurrencyType currencyType)
	{
		this.m_waitingForPurchaseRequest = true;
		ClientGameManager.Get().PurchaseBanner(bannerId, currencyType, new Action<PurchaseBannerBackgroundResponse>(this.HandlePurchaseBannerResponse));
	}

	public void RequestToPurchaseEmblem(int emblemId, CurrencyType currencyType)
	{
		this.m_waitingForPurchaseRequest = true;
		ClientGameManager.Get().PurchaseEmblem(emblemId, currencyType, new Action<PurchaseBannerForegroundResponse>(this.HandlePurchaseEmblemResponse));
	}

	public void RequestToPurchaseEmoticon(int emoticonId, CurrencyType currencyType)
	{
		this.m_waitingForPurchaseRequest = true;
		ClientGameManager.Get().PurchaseEmoticon(emoticonId, currencyType, new Action<PurchaseChatEmojiResponse>(this.HandlePurchaseEmoticonResponse));
	}

	public void RequestToPurchaseAbilityVfx(CharacterType charType, int abilityId, int vfxId, CurrencyType currencyType)
	{
		this.m_waitingForPurchaseRequest = true;
		ClientGameManager.Get().PurchaseAbilityVfx(charType, abilityId, vfxId, currencyType, new Action<PurchaseAbilityVfxResponse>(this.HandlePurchaseAbilityVfxResponse));
	}

	public void RequestToPurchaseOvercon(int overconId, CurrencyType currencyType)
	{
		this.m_waitingForPurchaseRequest = true;
		ClientGameManager.Get().PurchaseOvercon(overconId, currencyType, new Action<PurchaseOverconResponse>(this.HandlePurchaseOverconResponse));
	}

	public void RequestToPurchaseLoadingScreenBackground(int loadingScreenBackgroundId, CurrencyType currencyType)
	{
		this.m_waitingForPurchaseRequest = true;
		ClientGameManager.Get().PurchaseLoadingScreenBackground(loadingScreenBackgroundId, currencyType, new Action<PurchaseLoadingScreenBackgroundResponse>(this.HandlePurchaseLoadingScreenBackgroundResponse));
	}

	public void OpenPurchaseDialog(UIPurchaseableItem item, UIStorePanel.PurchaseCharacterCallback callback)
	{
		if (item.m_itemType == PurchaseItemType.Character)
		{
			this.m_charResponseCallback = callback;
		}
		this.OpenPurchaseDialog(item);
	}

	public void OpenPurchaseDialog(UIPurchaseableItem item, UIStorePurchaseItemDialogBox.PurchaseCloseDialogCallback callback = null)
	{
		if (item.m_itemType == PurchaseItemType.None)
		{
			return;
		}
		if (item.m_itemType == PurchaseItemType.Game)
		{
			this.m_gamePurchaseDialogBox = UIDialogPopupManager.OpenPurchaseGameDialog(item, this.m_paymentResponse, null);
			ClientGameManager.Get().RequestPaymentMethods(new Action<PaymentMethodsResponse>(UIStorePanel.Get().RefreshPayments));
		}
		else
		{
			if (item.m_itemType != PurchaseItemType.LootMatrixPack)
			{
				if (item.m_itemType != PurchaseItemType.GGBoost)
				{
					if (!item.m_purchaseForCash)
					{
						if (item.m_itemType != PurchaseItemType.Skin)
						{
							if (item.m_itemType != PurchaseItemType.Texture)
							{
								if (item.m_itemType != PurchaseItemType.Tint)
								{
									if (item.m_itemType != PurchaseItemType.Character)
									{
										if (item.m_itemType != PurchaseItemType.Taunt && item.m_itemType != PurchaseItemType.InventoryItem)
										{
											if (item.m_itemType != PurchaseItemType.Banner)
											{
												if (item.m_itemType != PurchaseItemType.Title)
												{
													if (item.m_itemType != PurchaseItemType.Emoticon)
													{
														if (item.m_itemType != PurchaseItemType.AbilityVfx && item.m_itemType != PurchaseItemType.Overcon)
														{
															if (item.m_itemType != PurchaseItemType.LoadingScreenBackground)
															{
																return;
															}
														}
													}
												}
											}
										}
									}
								}
							}
						}
						this.m_itemDialogBox = UIDialogPopupManager.OpenPurchaseItemDialog(item, null, callback);
						return;
					}
				}
			}
			this.m_cashPurchaseDialogBox = UIDialogPopupManager.OpenPurchaseForCashDialog(item, this.m_paymentResponse, null);
			ClientGameManager.Get().RequestPaymentMethods(new Action<PaymentMethodsResponse>(UIStorePanel.Get().RefreshPayments));
		}
	}

	public void RefreshPayments(PaymentMethodsResponse response)
	{
		if (this.m_gamePurchaseDialogBox != null)
		{
			this.m_gamePurchaseDialogBox.RefreshPayments(response);
		}
		if (this.m_cashPurchaseDialogBox != null)
		{
			this.m_cashPurchaseDialogBox.RefreshPayments(response);
		}
	}

	public void ToggleStore()
	{
		if (this.IsStoreOpen())
		{
			this.CloseStore();
		}
		else
		{
			this.OpenStore();
		}
	}

	public void SetMainPanelVisibility(bool visible)
	{
		this.m_isViewingHero = false;
		UIManager.SetGameObjectActive(this.m_container, visible, null);
		UIManager.SetGameObjectActive(this.m_backgroundContainer, this.m_container.gameObject.activeSelf, null);
		if (visible)
		{
			for (int i = 0; i < this.m_panels.Length; i++)
			{
				UIManager.SetGameObjectActive(this.m_panels[i], false, null);
			}
			for (int j = 0; j < this.m_menuBtns.Length; j++)
			{
				if (this.m_menuBtns[j].IsSelected())
				{
					this.SetScreenVisible(this.m_menuBtns[j]);
					return;
				}
			}
		}
	}

	public void OpenStore()
	{
		this.m_isOpen = true;
		this.m_isViewingHero = false;
		this.SetMainPanelVisibility(true);
		UIManager.SetGameObjectActive(this.m_secondaryNavContainer, true, null);
		ClientGameManager.Get().OnDisconnectedFromLobbyServer += this.HandleDisconnected;
		if (UIStoreViewHeroPage.Get().IsVisible())
		{
			UIStoreViewHeroPage.Get().SetVisible(false);
		}
		ClientGameManager.Get().NotifyStoreOpened();
		ClientGameManager.Get().RequestPaymentMethods(new Action<PaymentMethodsResponse>(this.HandleRequestPaymentResponse));
		CommerceClient.Get().RequestPrices();
		if (UICharacterSelectWorldObjects.Get().IsVisible())
		{
			UICharacterSelectWorldObjects.Get().SetVisible(false);
		}
		UINewUserFlowManager.OnDoneWithReadyButton();
		UIManager.SetGameObjectActive(this.m_container, true, null);
		UIManager.SetGameObjectActive(this.m_backgroundContainer, this.m_container.gameObject.activeSelf, null);
		UICharacterSelectWorldObjects.Get().PlayCameraAnimation("CamCloseupOUT");
	}

	public void CloseStore()
	{
		this.m_isOpen = false;
		this.SetMainPanelVisibility(false);
		UIManager.SetGameObjectActive(this.m_secondaryNavContainer, false, null);
		UIStoreViewHeroPage uistoreViewHeroPage = UIStoreViewHeroPage.Get();
		ClientGameManager.Get().OnDisconnectedFromLobbyServer -= this.HandleDisconnected;
		if (uistoreViewHeroPage != null)
		{
			if (uistoreViewHeroPage.IsVisible())
			{
				uistoreViewHeroPage.SetVisible(false);
				this.m_isViewingHero = false;
			}
		}
	}

	public void NotifyLoseFocus()
	{
		if (!this.m_isOpen)
		{
			return;
		}
		UIManager.SetGameObjectActive(this.m_secondaryNavContainer, false, null);
		if (this.m_isViewingHero)
		{
			UIStoreViewHeroPage.Get().NotifyLoseFocus();
		}
		else
		{
			UIManager.SetGameObjectActive(this.m_container, false, null);
		}
		UIManager.SetGameObjectActive(this.m_backgroundContainer, this.m_container.gameObject.activeSelf, null);
	}

	public void NotifyGetFocus()
	{
		if (!this.m_isOpen)
		{
			return;
		}
		UIManager.SetGameObjectActive(this.m_secondaryNavContainer, true, null);
		if (this.m_isViewingHero)
		{
			UIStoreViewHeroPage.Get().NotifyGetFocus();
		}
		else
		{
			UIManager.SetGameObjectActive(this.m_container, true, null);
		}
		UIManager.SetGameObjectActive(this.m_backgroundContainer, this.m_container.gameObject.activeSelf, null);
	}

	public bool IsStoreOpen()
	{
		return this.m_isOpen;
	}

	public bool IsVisible()
	{
		return this.m_container.gameObject.activeSelf;
	}

	public void ScreenToSetVisible(UIStorePanel.StorePanelScreen screenToSee)
	{
		for (int i = 0; i < this.m_panels.Length; i++)
		{
			this.m_panels[i].SetVisible(this.m_panels[i].ScreenType == screenToSee);
		}
	}

	public void HandleDisconnected(string message)
	{
		this.ClosePurchaseDialog();
	}

	public void HandleRequestPaymentResponse(PaymentMethodsResponse response)
	{
		this.m_paymentResponse = response;
		if (this.m_gamePurchaseDialogBox != null)
		{
			this.m_gamePurchaseDialogBox.HandleRequestPaymentResponse(this.m_paymentResponse);
		}
		if (this.m_cashPurchaseDialogBox != null)
		{
			this.m_cashPurchaseDialogBox.HandleRequestPaymentResponse(this.m_paymentResponse);
		}
	}

	private void SetScreenVisible(UIStoreNavBtn navBtnClicked)
	{
		if (navBtnClicked == this.m_freelancerBtn)
		{
			this.ScreenToSetVisible(UIStorePanel.StorePanelScreen.Freelancer);
		}
		else if (navBtnClicked == this.m_accountBtn)
		{
			this.ScreenToSetVisible(UIStorePanel.StorePanelScreen.Account);
		}
		else
		{
			this.ScreenToSetVisible(UIStorePanel.StorePanelScreen.None);
		}
	}

	public void NotifyNavBtnClicked(UIStoreNavBtn btnClicked)
	{
		for (int i = 0; i < this.m_menuBtns.Length; i++)
		{
			if (this.m_menuBtns[i] == btnClicked)
			{
				this.m_menuBtns[i].SetSelected(true);
				this.SetScreenVisible(this.m_menuBtns[i]);
			}
			else
			{
				this.m_menuBtns[i].SetSelected(false);
			}
		}
		if (UIStoreViewHeroPage.Get() != null)
		{
			if (UIStoreViewHeroPage.Get().IsVisible())
			{
				UIStoreViewHeroPage.Get().SetVisible(false);
				this.SetMainPanelVisibility(true);
			}
		}
	}

	public bool CanOpenMenu()
	{
		if (this.IsPurchaseDialogOpen())
		{
			return false;
		}
		if (!this.m_isOpen)
		{
			return true;
		}
		return !this.m_isViewingHero;
	}

	public bool IsPurchaseDialogOpen()
	{
		bool result;
		if (!(this.m_itemDialogBox != null) && !(this.m_gamePurchaseDialogBox != null))
		{
			result = (this.m_cashPurchaseDialogBox != null);
		}
		else
		{
			result = true;
		}
		return result;
	}

	private void Update()
	{
		if (Input.GetKeyUp(KeyCode.Escape))
		{
			if (this.m_isOpen)
			{
				if (!this.m_waitingForPurchaseRequest)
				{
					if (this.IsPurchaseDialogOpen())
					{
						this.ClosePurchaseDialog();
					}
					else if (this.m_isViewingHero)
					{
						if (UIStoreViewHeroPage.Get().IsVisible())
						{
							UIStoreViewHeroPage.Get().GoBackToStore();
						}
					}
					return;
				}
			}
		}
	}

	public void ClosePurchaseDialog()
	{
		if (this.m_itemDialogBox != null)
		{
			this.m_itemDialogBox.Close();
		}
		else if (this.m_gamePurchaseDialogBox != null)
		{
			this.m_gamePurchaseDialogBox.Close();
		}
		else if (this.m_cashPurchaseDialogBox != null)
		{
			this.m_cashPurchaseDialogBox.Close();
		}
	}

	public void SelectItem(InventoryItemTemplate template)
	{
		if (!this.IsStoreOpen())
		{
			this.OpenStore();
		}
		InventoryItemType type = template.Type;
		UIStoreBaseInventoryPanel uistoreBaseInventoryPanel;
		switch (type)
		{
		case InventoryItemType.TitleID:
		{
			GameBalanceVars.PlayerTitle playerTitle = null;
			for (int i = 0; i < GameBalanceVars.Get().PlayerTitles.Length; i++)
			{
				if (GameBalanceVars.Get().PlayerTitles[i].ID == template.TypeSpecificData[0])
				{
					playerTitle = GameBalanceVars.Get().PlayerTitles[i];
					break;
				}
			}
			if (playerTitle == null)
			{
				throw new Exception(string.Concat(new object[]
				{
					"Invalid title item: ",
					template.DisplayName,
					" ID ",
					template.Index
				}));
			}
			uistoreBaseInventoryPanel = this.OpenAndGetPanel(playerTitle.m_relatedCharacter, typeof(UIStoreAccountBannerPanel));
			break;
		}
		case InventoryItemType.BannerID:
		{
			GameBalanceVars.PlayerBanner banner = GameBalanceVars.Get().GetBanner(template.TypeSpecificData[0]);
			uistoreBaseInventoryPanel = this.OpenAndGetPanel(banner.m_relatedCharacter, typeof(UIStoreAccountBannerPanel));
			break;
		}
		default:
			if (type == InventoryItemType.FreelancerExpBonus)
			{
				uistoreBaseInventoryPanel = this.OpenAndGetPanel(CharacterType.None, typeof(UIStoreAccountFluxPanel));
				return;
			}
			if (type != InventoryItemType.LoadingScreenBackground)
			{
				throw new Exception("Selecting this type of item is not supported");
			}
			uistoreBaseInventoryPanel = this.OpenAndGetPanel(CharacterType.None, typeof(UIStoreAccountLoadingScreenPanel));
			return;
		case InventoryItemType.Style:
		{
			CharacterType characterType = (CharacterType)template.TypeSpecificData[0];
			if (characterType.IsValidForHumanGameplay())
			{
				uistoreBaseInventoryPanel = this.OpenAndGetPanel(characterType, typeof(UIStoreFreelancerStylesPanel));
			}
			else
			{
				uistoreBaseInventoryPanel = this.OpenAndGetPanel(characterType, typeof(UIStoreAccountFluxPanel));
			}
			break;
		}
		case InventoryItemType.Taunt:
			uistoreBaseInventoryPanel = this.OpenAndGetPanel((CharacterType)template.TypeSpecificData[0], typeof(UIStoreFreelancerTauntsPanel));
			break;
		case InventoryItemType.ChatEmoji:
			uistoreBaseInventoryPanel = this.OpenAndGetPanel(CharacterType.None, typeof(UIStoreAccountEmoticonsPanel));
			break;
		case InventoryItemType.AbilityVfxSwap:
			uistoreBaseInventoryPanel = this.OpenAndGetPanel((CharacterType)template.TypeSpecificData[0], typeof(UIStoreFreelancerVfxPanel));
			break;
		case InventoryItemType.Overcon:
			uistoreBaseInventoryPanel = this.OpenAndGetPanel(CharacterType.None, typeof(UIStoreAccountOverconsPanel));
			break;
		}
		if (uistoreBaseInventoryPanel != null)
		{
			uistoreBaseInventoryPanel.SelectItem(template);
		}
	}

	public void SelectItem(UIPurchaseableItem item)
	{
		if (item.m_itemType == PurchaseItemType.InventoryItem)
		{
			this.SelectItem(InventoryWideData.Get().GetItemTemplate(item.m_inventoryTemplateId));
			return;
		}
		if (!this.IsStoreOpen())
		{
			this.OpenStore();
		}
		PurchaseItemType itemType = item.m_itemType;
		UIStoreBaseInventoryPanel uistoreBaseInventoryPanel;
		switch (itemType)
		{
		case PurchaseItemType.Banner:
		{
			GameBalanceVars.PlayerBanner banner = GameBalanceVars.Get().GetBanner(item.m_bannerID);
			uistoreBaseInventoryPanel = this.OpenAndGetPanel(banner.m_relatedCharacter, typeof(UIStoreAccountBannerPanel));
			break;
		}
		case PurchaseItemType.Title:
		{
			GameBalanceVars.PlayerTitle playerTitle = null;
			int i = 0;
			while (i < GameBalanceVars.Get().PlayerTitles.Length)
			{
				if (GameBalanceVars.Get().PlayerTitles[i].ID == item.m_titleID)
				{
					playerTitle = GameBalanceVars.Get().PlayerTitles[i];
							break;
				}
				else
				{
					i++;
				}
			}
			if (playerTitle == null)
			{
				throw new Exception("Invalid title item: ID " + item.m_titleID);
			}
			uistoreBaseInventoryPanel = this.OpenAndGetPanel(playerTitle.m_relatedCharacter, typeof(UIStoreAccountBannerPanel));
			break;
		}
		case PurchaseItemType.Emoticon:
			uistoreBaseInventoryPanel = this.OpenAndGetPanel(CharacterType.None, typeof(UIStoreAccountEmoticonsPanel));
			break;
		case PurchaseItemType.AbilityVfx:
			uistoreBaseInventoryPanel = this.OpenAndGetPanel(item.m_charLink.m_characterType, typeof(UIStoreFreelancerVfxPanel));
			break;
		case PurchaseItemType.LoadingScreenBackground:
			uistoreBaseInventoryPanel = this.OpenAndGetPanel(CharacterType.None, typeof(UIStoreAccountLoadingScreenPanel));
			return;
		default:
			switch (itemType)
			{
			case PurchaseItemType.Tint:
				if (item.m_charLink.m_characterType.IsValidForHumanGameplay())
				{
					uistoreBaseInventoryPanel = this.OpenAndGetPanel(item.m_charLink.m_characterType, typeof(UIStoreFreelancerStylesPanel));
				}
				else
				{
					uistoreBaseInventoryPanel = this.OpenAndGetPanel(item.m_charLink.m_characterType, typeof(UIStoreAccountFluxPanel));
				}
				break;
			default:
				if (itemType != PurchaseItemType.Overcon)
				{
					throw new Exception("Selecting this type of item is not supported");
				}
				uistoreBaseInventoryPanel = this.OpenAndGetPanel(CharacterType.None, typeof(UIStoreAccountOverconsPanel));
				break;
			case PurchaseItemType.Taunt:
				uistoreBaseInventoryPanel = this.OpenAndGetPanel(item.m_charLink.m_characterType, typeof(UIStoreFreelancerTauntsPanel));
				break;
			}
			break;
		}
		if (uistoreBaseInventoryPanel != null)
		{
			uistoreBaseInventoryPanel.SelectItem(item);
		}
	}

	private UIStoreBaseInventoryPanel OpenAndGetPanel(CharacterType charType, Type panelType)
	{
		StorePanelData[] panels;
		if (!charType.IsValidForHumanGameplay())
		{
			this.NotifyNavBtnClicked(this.m_accountBtn);
			panels = this.m_accountPanel.m_panels;
		}
		else
		{
			CharacterResourceLink characterResourceLink = GameWideData.Get().GetCharacterResourceLink(charType);
			this.OpenFreelancerPage(characterResourceLink);
			panels = UIStoreViewHeroPage.Get().m_panels;
		}
		for (int i = 0; i < panels.Length; i++)
		{
			if (panels[i].Panel.GetType() == panelType)
			{
				if (!charType.IsValidForHumanGameplay())
				{
					this.m_accountPanel.SelectPanel(panels[i].Button);
					this.m_accountPanel.DisableInitialSelectPanel();
				}
				else
				{
					UIStoreViewHeroPage.Get().SelectPanel(panels[i].Button);
				}
				return panels[i].Panel;
			}
		}
		return null;
	}

	public enum StorePanelScreen
	{
		None,
		Account,
		Freelancer
	}

	public delegate void PurchaseCharacterCallback(bool success, PurchaseResult result, CharacterType characterType);
}

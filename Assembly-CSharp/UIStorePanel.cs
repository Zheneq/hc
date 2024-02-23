using LobbyGameClientMessages;
using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class UIStorePanel : UIScene
{
	public enum StorePanelScreen
	{
		None,
		Account,
		Freelancer
	}

	public delegate void PurchaseCharacterCallback(bool success, PurchaseResult result, CharacterType characterType);

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

	private PurchaseCharacterCallback m_charResponseCallback;

	private bool m_isOpen;

	private bool m_isViewingHero;

	public bool IsWaitingForPurchaseRequest
	{
		get { return m_waitingForPurchaseRequest; }
	}

	public bool IsWaitingForSteamPurchaseResponse
	{
		get;
		private set;
	}

	public float TimeReceivedSteamPurchaseResponse
	{
		get;
		private set;
	}

	public static UIStorePanel Get()
	{
		return s_instance;
	}

	public override SceneType GetSceneType()
	{
		return SceneType.Collections;
	}

	public static string FormatIntToString(int value, bool localize = false)
	{
		string empty = string.Empty;
		empty = value.ToString().Reverse();
		int num = 0;
		for (int i = 0; i < empty.Length; i++)
		{
			num++;
			if (num != 3)
			{
				continue;
			}
			if (i + 1 == empty.Length)
			{
				continue;
			}
			num = 0;
			string text = (!localize) ? "," : StringUtil.TR("ThousandsSeparator", "Global");
			if (text.Length > 1)
			{
				text = ",";
			}
			empty = empty.Insert(i + 1, text);
			i++;
		}
		while (true)
		{
			return empty.Reverse();
		}
	}

	public static string GetLocalizedPriceString(float price, string currencyCode)
	{
		string empty = string.Empty;
		float num = Mathf.Abs(price);
		int num2 = Mathf.FloorToInt(num);
		int num3 = Mathf.RoundToInt(num * 100f) % 100;
		string arg = num2.ToString();
		string text = num3.ToString();
		if (num3 < 10)
		{
			text = new StringBuilder().Append("0").Append(text).ToString();
		}
		string empty2 = string.Empty;
		switch (currencyCode)
		{
		case "USD":
			empty2 = "$";
			break;
		case "EUR":
			empty2 = "€";
			break;
		case "GBP":
			empty2 = "£";
			break;
		case "BRL":
			empty2 = "R$";
			break;
		case "RUB":
			empty2 = "RUB";
			break;
		default:
			empty2 = "$";
			break;
		}
		string result;
		if (num3 > 0)
		{
			result = string.Format(StringUtil.TR("RealCurrencyDisplay", "Global"), empty2, arg, text);
		}
		else
		{
			result = string.Format(StringUtil.TR("RealCurrencyNoDecimalDisplay", "Global"), empty2, arg);
		}
		return result;
	}

	public void OpenFreelancerPage(CharacterResourceLink charLink)
	{
		UIStoreViewHeroPage.Get().Setup(charLink);
		SetMainPanelVisibility(false);
		UIStoreViewHeroPage.Get().SetVisible(true);
		m_isViewingHero = true;
	}

	public override void Awake()
	{
		s_instance = this;
		m_menuBtns = m_secondaryNavContainer.GetComponentsInChildren<UIStoreNavBtn>(true);
		m_panels = new UIStoreBasePanel[2]
		{
			m_freelancerPanel,
			m_accountPanel
		};
		m_freelancerPanel.ScreenType = StorePanelScreen.Freelancer;
		m_accountPanel.ScreenType = StorePanelScreen.Account;
		if (m_accountBtn != null)
		{
			StaggerComponent.SetStaggerComponent(m_accountBtn.gameObject, true);
		}
		if (m_freelancerBtn != null)
		{
			StaggerComponent.SetStaggerComponent(m_freelancerBtn.gameObject, true);
		}
		if (HitchDetector.Get() != null)
		{
			HitchDetector.Get().AddNewLayoutGroup(m_secondaryNavContainer.GetComponentInChildren<LayoutGroup>(true));
		}
		NotifyNavBtnClicked(m_freelancerBtn);
		base.Awake();
	}

	private void OnDestroy()
	{
		s_instance = null;
	}

	public void TimeOutPurchase()
	{
		m_waitingForPurchaseRequest = false;
	}

	public void NotifySteamResponseReceived()
	{
		IsWaitingForSteamPurchaseResponse = false;
		TimeReceivedSteamPurchaseResponse = Time.time;
	}

	public void HandlePendingPurchaseResult(PendingPurchaseResult resultMsg)
	{
		if (resultMsg.Details.purchaseType == PurchaseType.Game)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					m_waitingForPurchaseRequest = false;
					if (m_gamePurchaseDialogBox != null)
					{
						while (true)
						{
							switch (3)
							{
							case 0:
								break;
							default:
								m_gamePurchaseDialogBox.NotifyPurchaseResponse(resultMsg.Result == PurchaseResult.Success);
								return;
							}
						}
					}
					return;
				}
			}
		}
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
		m_waitingForPurchaseRequest = false;
		if (m_cashPurchaseDialogBox != null)
		{
			m_cashPurchaseDialogBox.NotifyPurchaseResponse(resultMsg.Result == PurchaseResult.Success);
		}
	}

	public void HandlePurchaseGameResponse(PurchaseGameResponse response)
	{
		if (response.Result == PurchaseResult.Processing)
		{
			return;
		}
		while (true)
		{
			m_waitingForPurchaseRequest = false;
			if (m_gamePurchaseDialogBox != null)
			{
				m_gamePurchaseDialogBox.NotifyPurchaseResponse(response.Success);
			}
			return;
		}
	}

	public void RequestToPurchaseGame(GamePack gamePack, PaymentMethod paymentInfo)
	{
		m_waitingForPurchaseRequest = true;
		if (SteamManager.UsingSteam)
		{
			IsWaitingForSteamPurchaseResponse = true;
		}
		ClientGameManager.Get().PurchaseGame(gamePack.Index, paymentInfo.id, HandlePurchaseGameResponse);
	}

	public void HandlePurchaseLootMatrixPackResponse(PurchaseLootMatrixPackResponse response)
	{
		if (response.Result == PurchaseResult.Processing)
		{
			return;
		}
		while (true)
		{
			m_waitingForPurchaseRequest = false;
			if (m_cashPurchaseDialogBox != null)
			{
				m_cashPurchaseDialogBox.NotifyPurchaseResponse(response.Success);
			}
			return;
		}
	}

	public void RequestToPurchaseLootMatrixPack(LootMatrixPack pack, PaymentMethod paymentInfo)
	{
		m_waitingForPurchaseRequest = true;
		if (SteamManager.UsingSteam)
		{
			IsWaitingForSteamPurchaseResponse = true;
		}
		ClientGameManager.Get().PurchaseLootMatrixPack(pack.Index, paymentInfo.id, HandlePurchaseLootMatrixPackResponse);
	}

	public void HandlePurchaseCharacterForCashResponse(PurchaseCharacterForCashResponse response)
	{
		if (response.Result == PurchaseResult.Processing)
		{
			return;
		}
		while (true)
		{
			m_waitingForPurchaseRequest = false;
			if (m_cashPurchaseDialogBox != null)
			{
				m_cashPurchaseDialogBox.NotifyPurchaseResponse(response.Success);
			}
			if (m_charResponseCallback != null)
			{
				while (true)
				{
					m_charResponseCallback(response.Success, response.Result, response.CharacterType);
					m_charResponseCallback = null;
					return;
				}
			}
			return;
		}
	}

	public void RequestToPurchaseCharacterForCash(CharacterResourceLink link, PaymentMethod paymentInfo)
	{
		m_waitingForPurchaseRequest = true;
		if (SteamManager.UsingSteam)
		{
			IsWaitingForSteamPurchaseResponse = true;
		}
		ClientGameManager.Get().PurchaseCharacterForCash(link.m_characterType, paymentInfo.id, HandlePurchaseCharacterForCashResponse);
	}

	public void HandlePurchaseGGPackResponse(PurchaseGGPackResponse response)
	{
		if (response.Result == PurchaseResult.Processing)
		{
			return;
		}
		m_waitingForPurchaseRequest = false;
		if (!(m_cashPurchaseDialogBox != null))
		{
			return;
		}
		while (true)
		{
			m_cashPurchaseDialogBox.NotifyPurchaseResponse(response.Success);
			return;
		}
	}

	public void RequestToPurchaseGGPack(GGPack ggPack, PaymentMethod paymentInfo)
	{
		m_waitingForPurchaseRequest = true;
		if (SteamManager.UsingSteam)
		{
			IsWaitingForSteamPurchaseResponse = true;
		}
		ClientGameManager.Get().PurchaseGGPack(ggPack.Index, paymentInfo.id, HandlePurchaseGGPackResponse);
	}

	public void HandlePurchaseSkinResponse(PurchaseSkinResponse response)
	{
		if (response.Result == PurchaseResult.Processing)
		{
			return;
		}
		while (true)
		{
			m_waitingForPurchaseRequest = false;
			if (m_itemDialogBox != null)
			{
				m_itemDialogBox.NotifyPurchaseResponse(response.Success);
			}
			if (UICharacterSelectScreenController.Get() != null)
			{
				while (true)
				{
					UICharacterSelectScreenController.Get().PurchaseCharacterSkinResponseHandler(response);
					return;
				}
			}
			return;
		}
	}

	public void HandlePurchaseTextureResponse(PurchaseTextureResponse response)
	{
		if (response.Result == PurchaseResult.Processing)
		{
			return;
		}
		m_waitingForPurchaseRequest = false;
		if (!(m_itemDialogBox != null))
		{
			return;
		}
		while (true)
		{
			m_itemDialogBox.NotifyPurchaseResponse(response.Success);
			return;
		}
	}

	public void HandlePurchaseTintResponse(PurchaseTintResponse response)
	{
		if (response.Result == PurchaseResult.Processing)
		{
			return;
		}
		m_waitingForPurchaseRequest = false;
		if (m_itemDialogBox != null)
		{
			m_itemDialogBox.NotifyPurchaseResponse(response.Success);
		}
		if (!(UICharacterSelectScreenController.Get() != null))
		{
			return;
		}
		while (true)
		{
			UICharacterSelectScreenController.Get().PurchaseCharacterTintResponseHandler(response);
			return;
		}
	}

	public void HandlePurchaseTintForCashResponse(PurchaseTintForCashResponse response)
	{
		if (response.Result == PurchaseResult.Processing)
		{
			return;
		}
		while (true)
		{
			m_waitingForPurchaseRequest = false;
			if (m_cashPurchaseDialogBox != null)
			{
				m_cashPurchaseDialogBox.NotifyPurchaseResponse(response.Success);
			}
			if (UICharacterSelectScreenController.Get() != null)
			{
				while (true)
				{
					UICharacterSelectScreenController.Get().PurchaseCharacterTintResponseHandler(response);
					return;
				}
			}
			return;
		}
	}

	public void RequestToPurchaseTintForCash(CharacterType type, int skin, int texture, int tint, PaymentMethod paymentInfo)
	{
		m_waitingForPurchaseRequest = true;
		if (SteamManager.UsingSteam)
		{
			IsWaitingForSteamPurchaseResponse = true;
		}
		ClientGameManager.Get().PurchaseTintForCash(type, skin, texture, tint, paymentInfo.id, HandlePurchaseTintForCashResponse);
	}

	public void HandlePurchaseStoreItemsResponse(PurchaseStoreItemForCashResponse response)
	{
		if (response.Result == PurchaseResult.Processing)
		{
			return;
		}
		while (true)
		{
			m_waitingForPurchaseRequest = false;
			if (m_cashPurchaseDialogBox != null)
			{
				m_cashPurchaseDialogBox.NotifyPurchaseResponse(response.Success);
			}
			return;
		}
	}

	public void RequestToPurchaseStoreItems(int inventoryTemplateId, PaymentMethod paymentInfo)
	{
		m_waitingForPurchaseRequest = true;
		if (SteamManager.UsingSteam)
		{
			IsWaitingForSteamPurchaseResponse = true;
		}
		ClientGameManager.Get().PurchaseStoreItemForCash(inventoryTemplateId, paymentInfo.id, HandlePurchaseStoreItemsResponse);
	}

	public void HandlePurchaseCharacterResponse(PurchaseCharacterResponse response)
	{
		if (response.Result == PurchaseResult.Processing)
		{
			return;
		}
		while (true)
		{
			m_waitingForPurchaseRequest = false;
			if (m_itemDialogBox != null)
			{
				m_itemDialogBox.NotifyPurchaseResponse(response.Success);
			}
			if (m_charResponseCallback != null)
			{
				while (true)
				{
					m_charResponseCallback(response.Success, response.Result, response.CharacterType);
					m_charResponseCallback = null;
					return;
				}
			}
			return;
		}
	}

	public void HandlePurchaseTauntResponse(PurchaseTauntResponse response)
	{
		if (response.Result == PurchaseResult.Processing)
		{
			return;
		}
		while (true)
		{
			m_waitingForPurchaseRequest = false;
			if (m_itemDialogBox != null)
			{
				while (true)
				{
					m_itemDialogBox.NotifyTauntPurchaseResponse(response.Success);
					return;
				}
			}
			return;
		}
	}

	public void HandlePurchaseInventoryItemResponse(PurchaseInventoryItemResponse response)
	{
		if (response.Result == PurchaseResult.Processing)
		{
			return;
		}
		m_waitingForPurchaseRequest = false;
		if (m_itemDialogBox != null)
		{
			m_itemDialogBox.NotifyPurchaseInventoryItemResponse(response.Success);
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
		if (!(UICharacterSelectScreenController.Get() != null))
		{
			return;
		}
		while (true)
		{
			UICharacterSelectScreenController.Get().UpdateSkinsPanel();
			return;
		}
	}

	public void HandlePurchaseTitleResponse(PurchaseTitleResponse response)
	{
		if (response.Result != PurchaseResult.Processing)
		{
			m_waitingForPurchaseRequest = false;
			if (m_itemDialogBox != null)
			{
				m_itemDialogBox.NotifyPurchaseResponse(response.Success);
			}
		}
	}

	public void HandlePurchaseBannerResponse(PurchaseBannerBackgroundResponse response)
	{
		if (response.Result == PurchaseResult.Processing)
		{
			return;
		}
		while (true)
		{
			m_waitingForPurchaseRequest = false;
			if (m_itemDialogBox != null)
			{
				m_itemDialogBox.NotifyPurchaseResponse(response.Success);
			}
			return;
		}
	}

	public void HandlePurchaseEmblemResponse(PurchaseBannerForegroundResponse response)
	{
		if (response.Result == PurchaseResult.Processing)
		{
			return;
		}
		while (true)
		{
			m_waitingForPurchaseRequest = false;
			if (m_itemDialogBox != null)
			{
				while (true)
				{
					m_itemDialogBox.NotifyPurchaseResponse(response.Success);
					return;
				}
			}
			return;
		}
	}

	public void HandlePurchaseEmoticonResponse(PurchaseChatEmojiResponse response)
	{
		if (response.Result == PurchaseResult.Processing)
		{
			return;
		}
		while (true)
		{
			m_waitingForPurchaseRequest = false;
			if (m_itemDialogBox != null)
			{
				m_itemDialogBox.NotifyPurchaseResponse(response.Success);
			}
			return;
		}
	}

	public void HandlePurchaseAbilityVfxResponse(PurchaseAbilityVfxResponse response)
	{
		if (response.Result == PurchaseResult.Processing)
		{
			return;
		}
		while (true)
		{
			m_waitingForPurchaseRequest = false;
			if (m_itemDialogBox != null)
			{
				while (true)
				{
					m_itemDialogBox.NotifyPurchaseResponse(response.Success);
					return;
				}
			}
			return;
		}
	}

	public void HandlePurchaseOverconResponse(PurchaseOverconResponse response)
	{
		if (response.Result == PurchaseResult.Processing)
		{
			return;
		}
		while (true)
		{
			m_waitingForPurchaseRequest = false;
			if (m_itemDialogBox != null)
			{
				while (true)
				{
					m_itemDialogBox.NotifyPurchaseResponse(response.Success);
					return;
				}
			}
			return;
		}
	}

	public void HandlePurchaseLoadingScreenBackgroundResponse(PurchaseLoadingScreenBackgroundResponse response)
	{
		if (response.Result == PurchaseResult.Processing)
		{
			return;
		}
		m_waitingForPurchaseRequest = false;
		if (!(m_itemDialogBox != null))
		{
			return;
		}
		while (true)
		{
			m_itemDialogBox.NotifyPurchaseResponse(response.Success);
			return;
		}
	}

	public void RequestToPurchaseSkin(CurrencyType currency, CharacterType type, int skin)
	{
		m_waitingForPurchaseRequest = true;
		ClientGameManager.Get().PurchaseSkin(currency, type, skin, HandlePurchaseSkinResponse);
	}

	public void RequestToPurchaseTexture(CurrencyType currency, CharacterType type, int skin, int texture)
	{
		m_waitingForPurchaseRequest = true;
		ClientGameManager.Get().PurchaseTexture(currency, type, skin, texture, HandlePurchaseTextureResponse);
	}

	public void RequestToPurchaseTint(CurrencyType currency, CharacterType type, int skin, int texture, int tint)
	{
		m_waitingForPurchaseRequest = true;
		ClientGameManager.Get().PurchaseTint(currency, type, skin, texture, tint, HandlePurchaseTintResponse);
	}

	public void RequestToPurchaseCharacter(CurrencyType currency, CharacterType type)
	{
		m_waitingForPurchaseRequest = true;
		ClientGameManager.Get().PurchaseCharacter(currency, type, HandlePurchaseCharacterResponse);
	}

	public void RequestToPurchaseTaunt(CurrencyType currency, CharacterType type, int tauntIndex)
	{
		m_waitingForPurchaseRequest = true;
		ClientGameManager.Get().PurchaseTaunt(currency, type, tauntIndex, HandlePurchaseTauntResponse);
	}

	public void RequestToPurchaseInventoryItem(int templateId, CurrencyType currencyType)
	{
		m_waitingForPurchaseRequest = true;
		ClientGameManager.Get().PurchaseInventoryItem(templateId, currencyType, HandlePurchaseInventoryItemResponse);
	}

	public void RequestToPurchaseTitle(int titleId, CurrencyType currencyType)
	{
		m_waitingForPurchaseRequest = true;
		ClientGameManager.Get().PurchaseTitle(titleId, currencyType, HandlePurchaseTitleResponse);
	}

	public void RequestToPurchaseBanner(int bannerId, CurrencyType currencyType)
	{
		m_waitingForPurchaseRequest = true;
		ClientGameManager.Get().PurchaseBanner(bannerId, currencyType, HandlePurchaseBannerResponse);
	}

	public void RequestToPurchaseEmblem(int emblemId, CurrencyType currencyType)
	{
		m_waitingForPurchaseRequest = true;
		ClientGameManager.Get().PurchaseEmblem(emblemId, currencyType, HandlePurchaseEmblemResponse);
	}

	public void RequestToPurchaseEmoticon(int emoticonId, CurrencyType currencyType)
	{
		m_waitingForPurchaseRequest = true;
		ClientGameManager.Get().PurchaseEmoticon(emoticonId, currencyType, HandlePurchaseEmoticonResponse);
	}

	public void RequestToPurchaseAbilityVfx(CharacterType charType, int abilityId, int vfxId, CurrencyType currencyType)
	{
		m_waitingForPurchaseRequest = true;
		ClientGameManager.Get().PurchaseAbilityVfx(charType, abilityId, vfxId, currencyType, HandlePurchaseAbilityVfxResponse);
	}

	public void RequestToPurchaseOvercon(int overconId, CurrencyType currencyType)
	{
		m_waitingForPurchaseRequest = true;
		ClientGameManager.Get().PurchaseOvercon(overconId, currencyType, HandlePurchaseOverconResponse);
	}

	public void RequestToPurchaseLoadingScreenBackground(int loadingScreenBackgroundId, CurrencyType currencyType)
	{
		m_waitingForPurchaseRequest = true;
		ClientGameManager.Get().PurchaseLoadingScreenBackground(loadingScreenBackgroundId, currencyType, HandlePurchaseLoadingScreenBackgroundResponse);
	}

	public void OpenPurchaseDialog(UIPurchaseableItem item, PurchaseCharacterCallback callback)
	{
		if (item.m_itemType == PurchaseItemType.Character)
		{
			m_charResponseCallback = callback;
		}
		OpenPurchaseDialog(item);
	}

	public void OpenPurchaseDialog(UIPurchaseableItem item, UIStorePurchaseItemDialogBox.PurchaseCloseDialogCallback callback = null)
	{
		if (item.m_itemType == PurchaseItemType.None)
		{
			return;
		}
		if (item.m_itemType == PurchaseItemType.Game)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					m_gamePurchaseDialogBox = UIDialogPopupManager.OpenPurchaseGameDialog(item, m_paymentResponse);
					ClientGameManager.Get().RequestPaymentMethods(Get().RefreshPayments);
					return;
				}
			}
		}
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
					m_itemDialogBox = UIDialogPopupManager.OpenPurchaseItemDialog(item, null, callback);
					return;
				}
			}
		}
		m_cashPurchaseDialogBox = UIDialogPopupManager.OpenPurchaseForCashDialog(item, m_paymentResponse);
		ClientGameManager.Get().RequestPaymentMethods(Get().RefreshPayments);
	}

	public void RefreshPayments(PaymentMethodsResponse response)
	{
		if (m_gamePurchaseDialogBox != null)
		{
			m_gamePurchaseDialogBox.RefreshPayments(response);
		}
		if (m_cashPurchaseDialogBox != null)
		{
			m_cashPurchaseDialogBox.RefreshPayments(response);
		}
	}

	public void ToggleStore()
	{
		if (IsStoreOpen())
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					CloseStore();
					return;
				}
			}
		}
		OpenStore();
	}

	public void SetMainPanelVisibility(bool visible)
	{
		m_isViewingHero = false;
		UIManager.SetGameObjectActive(m_container, visible);
		UIManager.SetGameObjectActive(m_backgroundContainer, m_container.gameObject.activeSelf);
		if (!visible)
		{
			return;
		}
		while (true)
		{
			for (int i = 0; i < m_panels.Length; i++)
			{
				UIManager.SetGameObjectActive(m_panels[i], false);
			}
			while (true)
			{
				for (int j = 0; j < m_menuBtns.Length; j++)
				{
					if (m_menuBtns[j].IsSelected())
					{
						SetScreenVisible(m_menuBtns[j]);
						return;
					}
				}
				while (true)
				{
					switch (4)
					{
					default:
						return;
					case 0:
						break;
					}
				}
			}
		}
	}

	public void OpenStore()
	{
		m_isOpen = true;
		m_isViewingHero = false;
		SetMainPanelVisibility(true);
		UIManager.SetGameObjectActive(m_secondaryNavContainer, true);
		ClientGameManager.Get().OnDisconnectedFromLobbyServer += HandleDisconnected;
		if (UIStoreViewHeroPage.Get().IsVisible())
		{
			UIStoreViewHeroPage.Get().SetVisible(false);
		}
		ClientGameManager.Get().NotifyStoreOpened();
		ClientGameManager.Get().RequestPaymentMethods(HandleRequestPaymentResponse);
		CommerceClient.Get().RequestPrices();
		if (UICharacterSelectWorldObjects.Get().IsVisible())
		{
			UICharacterSelectWorldObjects.Get().SetVisible(false);
		}
		UINewUserFlowManager.OnDoneWithReadyButton();
		UIManager.SetGameObjectActive(m_container, true);
		UIManager.SetGameObjectActive(m_backgroundContainer, m_container.gameObject.activeSelf);
		UICharacterSelectWorldObjects.Get().PlayCameraAnimation("CamCloseupOUT");
	}

	public void CloseStore()
	{
		m_isOpen = false;
		SetMainPanelVisibility(false);
		UIManager.SetGameObjectActive(m_secondaryNavContainer, false);
		UIStoreViewHeroPage uIStoreViewHeroPage = UIStoreViewHeroPage.Get();
		ClientGameManager.Get().OnDisconnectedFromLobbyServer -= HandleDisconnected;
		if (!(uIStoreViewHeroPage != null))
		{
			return;
		}
		while (true)
		{
			if (uIStoreViewHeroPage.IsVisible())
			{
				while (true)
				{
					uIStoreViewHeroPage.SetVisible(false);
					m_isViewingHero = false;
					return;
				}
			}
			return;
		}
	}

	public void NotifyLoseFocus()
	{
		if (!m_isOpen)
		{
			return;
		}
		UIManager.SetGameObjectActive(m_secondaryNavContainer, false);
		if (m_isViewingHero)
		{
			UIStoreViewHeroPage.Get().NotifyLoseFocus();
		}
		else
		{
			UIManager.SetGameObjectActive(m_container, false);
		}
		UIManager.SetGameObjectActive(m_backgroundContainer, m_container.gameObject.activeSelf);
	}

	public void NotifyGetFocus()
	{
		if (!m_isOpen)
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
		UIManager.SetGameObjectActive(m_secondaryNavContainer, true);
		if (m_isViewingHero)
		{
			UIStoreViewHeroPage.Get().NotifyGetFocus();
		}
		else
		{
			UIManager.SetGameObjectActive(m_container, true);
		}
		UIManager.SetGameObjectActive(m_backgroundContainer, m_container.gameObject.activeSelf);
	}

	public bool IsStoreOpen()
	{
		return m_isOpen;
	}

	public bool IsVisible()
	{
		return m_container.gameObject.activeSelf;
	}

	public void ScreenToSetVisible(StorePanelScreen screenToSee)
	{
		for (int i = 0; i < m_panels.Length; i++)
		{
			m_panels[i].SetVisible(m_panels[i].ScreenType == screenToSee);
		}
		while (true)
		{
			return;
		}
	}

	public void HandleDisconnected(string message)
	{
		ClosePurchaseDialog();
	}

	public void HandleRequestPaymentResponse(PaymentMethodsResponse response)
	{
		m_paymentResponse = response;
		if (m_gamePurchaseDialogBox != null)
		{
			m_gamePurchaseDialogBox.HandleRequestPaymentResponse(m_paymentResponse);
		}
		if (!(m_cashPurchaseDialogBox != null))
		{
			return;
		}
		while (true)
		{
			m_cashPurchaseDialogBox.HandleRequestPaymentResponse(m_paymentResponse);
			return;
		}
	}

	private void SetScreenVisible(UIStoreNavBtn navBtnClicked)
	{
		if (navBtnClicked == m_freelancerBtn)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					ScreenToSetVisible(StorePanelScreen.Freelancer);
					return;
				}
			}
		}
		if (navBtnClicked == m_accountBtn)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					ScreenToSetVisible(StorePanelScreen.Account);
					return;
				}
			}
		}
		ScreenToSetVisible(StorePanelScreen.None);
	}

	public void NotifyNavBtnClicked(UIStoreNavBtn btnClicked)
	{
		for (int i = 0; i < m_menuBtns.Length; i++)
		{
			if (m_menuBtns[i] == btnClicked)
			{
				m_menuBtns[i].SetSelected(true);
				SetScreenVisible(m_menuBtns[i]);
			}
			else
			{
				m_menuBtns[i].SetSelected(false);
			}
		}
		while (true)
		{
			if (!(UIStoreViewHeroPage.Get() != null))
			{
				return;
			}
			while (true)
			{
				if (UIStoreViewHeroPage.Get().IsVisible())
				{
					while (true)
					{
						UIStoreViewHeroPage.Get().SetVisible(false);
						SetMainPanelVisibility(true);
						return;
					}
				}
				return;
			}
		}
	}

	public bool CanOpenMenu()
	{
		if (IsPurchaseDialogOpen())
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return false;
				}
			}
		}
		if (!m_isOpen)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return true;
				}
			}
		}
		if (m_isViewingHero)
		{
			return false;
		}
		return true;
	}

	public bool IsPurchaseDialogOpen()
	{
		int result;
		if (!(m_itemDialogBox != null) && !(m_gamePurchaseDialogBox != null))
		{
			result = ((m_cashPurchaseDialogBox != null) ? 1 : 0);
		}
		else
		{
			result = 1;
		}
		return (byte)result != 0;
	}

	private void Update()
	{
		if (!Input.GetKeyUp(KeyCode.Escape))
		{
			return;
		}
		while (true)
		{
			if (!m_isOpen)
			{
				return;
			}
			while (true)
			{
				if (m_waitingForPurchaseRequest)
				{
					while (true)
					{
						switch (4)
						{
						default:
							return;
						case 0:
							break;
						}
					}
				}
				if (IsPurchaseDialogOpen())
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							break;
						default:
							ClosePurchaseDialog();
							return;
						}
					}
				}
				if (!m_isViewingHero)
				{
					return;
				}
				while (true)
				{
					if (UIStoreViewHeroPage.Get().IsVisible())
					{
						while (true)
						{
							UIStoreViewHeroPage.Get().GoBackToStore();
							return;
						}
					}
					return;
				}
			}
		}
	}

	public void ClosePurchaseDialog()
	{
		if (m_itemDialogBox != null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					m_itemDialogBox.Close();
					return;
				}
			}
		}
		if (m_gamePurchaseDialogBox != null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					m_gamePurchaseDialogBox.Close();
					return;
				}
			}
		}
		if (m_cashPurchaseDialogBox != null)
		{
			m_cashPurchaseDialogBox.Close();
		}
	}

	public void SelectItem(InventoryItemTemplate template)
	{
		if (!IsStoreOpen())
		{
			OpenStore();
		}
		UIStoreBaseInventoryPanel uIStoreBaseInventoryPanel = null;
		InventoryItemType type = template.Type;
		switch (type)
		{
		default:
			while (true)
			{
				if (type != InventoryItemType.LoadingScreenBackground)
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							break;
						default:
							throw new Exception("Selecting this type of item is not supported");
						}
					}
				}
				uIStoreBaseInventoryPanel = OpenAndGetPanel(CharacterType.None, typeof(UIStoreAccountLoadingScreenPanel));
				return;
			}
		case InventoryItemType.Overcon:
			uIStoreBaseInventoryPanel = OpenAndGetPanel(CharacterType.None, typeof(UIStoreAccountOverconsPanel));
			break;
		case InventoryItemType.BannerID:
		{
			GameBalanceVars.PlayerBanner banner = GameBalanceVars.Get().GetBanner(template.TypeSpecificData[0]);
			uIStoreBaseInventoryPanel = OpenAndGetPanel(banner.m_relatedCharacter, typeof(UIStoreAccountBannerPanel));
			break;
		}
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
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						throw new Exception(new StringBuilder().Append("Invalid title item: ").Append(template.DisplayName).Append(" ID ").Append(template.Index).ToString());
					}
				}
			}
			uIStoreBaseInventoryPanel = OpenAndGetPanel(playerTitle.m_relatedCharacter, typeof(UIStoreAccountBannerPanel));
			break;
		}
		case InventoryItemType.ChatEmoji:
			uIStoreBaseInventoryPanel = OpenAndGetPanel(CharacterType.None, typeof(UIStoreAccountEmoticonsPanel));
			break;
		case InventoryItemType.Style:
		{
			CharacterType characterType = (CharacterType)template.TypeSpecificData[0];
			uIStoreBaseInventoryPanel = ((!characterType.IsValidForHumanGameplay()) ? OpenAndGetPanel(characterType, typeof(UIStoreAccountFluxPanel)) : OpenAndGetPanel(characterType, typeof(UIStoreFreelancerStylesPanel)));
			break;
		}
		case InventoryItemType.Taunt:
			uIStoreBaseInventoryPanel = OpenAndGetPanel((CharacterType)template.TypeSpecificData[0], typeof(UIStoreFreelancerTauntsPanel));
			break;
		case InventoryItemType.AbilityVfxSwap:
			uIStoreBaseInventoryPanel = OpenAndGetPanel((CharacterType)template.TypeSpecificData[0], typeof(UIStoreFreelancerVfxPanel));
			break;
		case InventoryItemType.FreelancerExpBonus:
			uIStoreBaseInventoryPanel = OpenAndGetPanel(CharacterType.None, typeof(UIStoreAccountFluxPanel));
			return;
		}
		if (!(uIStoreBaseInventoryPanel != null))
		{
			return;
		}
		while (true)
		{
			uIStoreBaseInventoryPanel.SelectItem(template);
			return;
		}
	}

	public void SelectItem(UIPurchaseableItem item)
	{
		if (item.m_itemType == PurchaseItemType.InventoryItem)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					SelectItem(InventoryWideData.Get().GetItemTemplate(item.m_inventoryTemplateId));
					return;
				}
			}
		}
		if (!IsStoreOpen())
		{
			OpenStore();
		}
		UIStoreBaseInventoryPanel uIStoreBaseInventoryPanel = null;
		switch (item.m_itemType)
		{
		case PurchaseItemType.Overcon:
			uIStoreBaseInventoryPanel = OpenAndGetPanel(CharacterType.None, typeof(UIStoreAccountOverconsPanel));
			break;
		case PurchaseItemType.Banner:
		{
			GameBalanceVars.PlayerBanner banner = GameBalanceVars.Get().GetBanner(item.m_bannerID);
			uIStoreBaseInventoryPanel = OpenAndGetPanel(banner.m_relatedCharacter, typeof(UIStoreAccountBannerPanel));
			break;
		}
		case PurchaseItemType.Title:
		{
			GameBalanceVars.PlayerTitle playerTitle = null;
			int num = 0;
			while (true)
			{
				if (num < GameBalanceVars.Get().PlayerTitles.Length)
				{
					if (GameBalanceVars.Get().PlayerTitles[num].ID == item.m_titleID)
					{
						playerTitle = GameBalanceVars.Get().PlayerTitles[num];
						break;
					}
					num++;
					continue;
				}
				break;
			}
			if (playerTitle == null)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						throw new Exception(new StringBuilder().Append("Invalid title item: ID ").Append(item.m_titleID).ToString());
					}
				}
			}
			uIStoreBaseInventoryPanel = OpenAndGetPanel(playerTitle.m_relatedCharacter, typeof(UIStoreAccountBannerPanel));
			break;
		}
		case PurchaseItemType.Emoticon:
			uIStoreBaseInventoryPanel = OpenAndGetPanel(CharacterType.None, typeof(UIStoreAccountEmoticonsPanel));
			break;
		case PurchaseItemType.Tint:
			if (item.m_charLink.m_characterType.IsValidForHumanGameplay())
			{
				uIStoreBaseInventoryPanel = OpenAndGetPanel(item.m_charLink.m_characterType, typeof(UIStoreFreelancerStylesPanel));
			}
			else
			{
				uIStoreBaseInventoryPanel = OpenAndGetPanel(item.m_charLink.m_characterType, typeof(UIStoreAccountFluxPanel));
			}
			break;
		case PurchaseItemType.Taunt:
			uIStoreBaseInventoryPanel = OpenAndGetPanel(item.m_charLink.m_characterType, typeof(UIStoreFreelancerTauntsPanel));
			break;
		case PurchaseItemType.AbilityVfx:
			uIStoreBaseInventoryPanel = OpenAndGetPanel(item.m_charLink.m_characterType, typeof(UIStoreFreelancerVfxPanel));
			break;
		case PurchaseItemType.LoadingScreenBackground:
			uIStoreBaseInventoryPanel = OpenAndGetPanel(CharacterType.None, typeof(UIStoreAccountLoadingScreenPanel));
			return;
		default:
			throw new Exception("Selecting this type of item is not supported");
		}
		if (!(uIStoreBaseInventoryPanel != null))
		{
			return;
		}
		while (true)
		{
			uIStoreBaseInventoryPanel.SelectItem(item);
			return;
		}
	}

	private UIStoreBaseInventoryPanel OpenAndGetPanel(CharacterType charType, Type panelType)
	{
		StorePanelData[] panels;
		if (!charType.IsValidForHumanGameplay())
		{
			NotifyNavBtnClicked(m_accountBtn);
			panels = m_accountPanel.m_panels;
		}
		else
		{
			CharacterResourceLink characterResourceLink = GameWideData.Get().GetCharacterResourceLink(charType);
			OpenFreelancerPage(characterResourceLink);
			panels = UIStoreViewHeroPage.Get().m_panels;
		}
		for (int i = 0; i < panels.Length; i++)
		{
			if (panels[i].Panel.GetType() != panelType)
			{
				continue;
			}
			if (!charType.IsValidForHumanGameplay())
			{
				m_accountPanel.SelectPanel(panels[i].Button);
				m_accountPanel.DisableInitialSelectPanel();
			}
			else
			{
				UIStoreViewHeroPage.Get().SelectPanel(panels[i].Button);
			}
			return panels[i].Panel;
		}
		while (true)
		{
			return null;
		}
	}
}

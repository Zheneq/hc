using LobbyGameClientMessages;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIStoreViewHeroPage : UIScene
{
	public StorePanelData[] m_panels;

	public UIStoreSideNavButton m_defaultPanelBtn;

	public UIStoreFreelancerStylesPanel m_stylePanel;

	public TextMeshProUGUI m_characterName;

	public Image m_characterIcon;

	public Image m_roleIcon;

	public TextMeshProUGUI m_charLevelText;

	public TextMeshProUGUI m_totalOwnedText;

	public TextMeshProUGUI m_totalTotalText;

	public Image m_ownedProgressBar;

	public RectTransform m_ownedCompleteContainer;

	public Image m_trustBanner;

	public _SelectableBtn m_backBtn;

	public RectTransform m_container;

	public RectTransform m_backgroundContainer;

	public Animator m_animationController;

	public Image m_modelHitbox;

	public RectTransform m_restrictedIsoUsePopup;

	public _ButtonSwapSprite m_restrictedIsoUseCloseBtn;

	public _ButtonSwapSprite m_restrictedIsoUseBuyGameBtn;

	public RectTransform m_buyHeroContainer;

	public _SelectableBtn m_buyInGameButton;

	public TextMeshProUGUI m_buyInGameLabel;

	public _SelectableBtn m_buyForCashButton;

	public TextMeshProUGUI m_buyForCashLabel;

	public _SelectableBtn m_buyForTokenButton;

	private static UIStoreViewHeroPage s_instance;

	private CharacterResourceLink m_charLink;

	public static UIStoreViewHeroPage Get()
	{
		return s_instance;
	}

	public override SceneType GetSceneType()
	{
		return SceneType.StoreViewHero;
	}

	public void SetRestrictedISOUSePopVisible(bool visible)
	{
		UIManager.SetGameObjectActive(m_restrictedIsoUsePopup, visible);
	}

	public override void Awake()
	{
		s_instance = this;
		SetRestrictedISOUSePopVisible(false);
		UIManager.SetGameObjectActive(m_restrictedIsoUsePopup, false);
		m_restrictedIsoUseCloseBtn.callback = delegate
		{
			SetRestrictedISOUSePopVisible(false);
			UIStorePanel.Get().NotifyGetFocus();
		};
		_ButtonSwapSprite restrictedIsoUseBuyGameBtn = m_restrictedIsoUseBuyGameBtn;
		if (_003C_003Ef__am_0024cache0 == null)
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
			_003C_003Ef__am_0024cache0 = delegate
			{
				Get().SetRestrictedISOUSePopVisible(false);
				UIFrontEnd.Get().m_frontEndNavPanel.CashShopBtnClicked(null);
				UICashShopPanel.Get().ButtonClicked(UICashShopPanel.Get().m_gameBtn);
			};
		}
		restrictedIsoUseBuyGameBtn.callback = _003C_003Ef__am_0024cache0;
		UIEventTriggerUtils.AddListener(m_modelHitbox.gameObject, EventTriggerType.PointerEnter, HighlightCharacter);
		UIEventTriggerUtils.AddListener(m_modelHitbox.gameObject, EventTriggerType.PointerExit, UnhighlightCharacter);
		m_backBtn.spriteController.callback = BackBtnClicked;
		m_buyInGameButton.spriteController.callback = BuyInGameClicked;
		m_buyForCashButton.spriteController.callback = BuyForCashClicked;
		m_buyForTokenButton.spriteController.callback = BuyForTokenClicked;
		base.Awake();
	}

	private void Start()
	{
		SetVisible(false);
		StorePanelData[] panels = m_panels;
		foreach (StorePanelData storePanelData in panels)
		{
			storePanelData.Button.m_button.spriteController.callback = NavButtonClicked;
			storePanelData.Panel.OnCountsRefreshed += CountChanged;
			storePanelData.Panel.Initialize();
			CountChanged(storePanelData.Panel, storePanelData.Panel.GetNumOwned(), storePanelData.Panel.GetNumTotal());
			storePanelData.Panel.SetParentContainer(base.gameObject);
			UIManager.SetGameObjectActive(storePanelData.Panel, false);
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
			SelectPanel(m_defaultPanelBtn);
			ClientGameManager.Get().OnCharacterDataUpdated += OnCharacterDataUpdated;
			ClientGameManager.Get().OnBankBalanceChange += HandleBankBalanceChange;
			return;
		}
	}

	private void OnDestroy()
	{
		s_instance = null;
		StorePanelData[] panels = m_panels;
		foreach (StorePanelData storePanelData in panels)
		{
			storePanelData.Panel.OnCountsRefreshed -= CountChanged;
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
			if (ClientGameManager.Get() != null)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					ClientGameManager.Get().OnCharacterDataUpdated -= OnCharacterDataUpdated;
					ClientGameManager.Get().OnBankBalanceChange -= HandleBankBalanceChange;
					return;
				}
			}
			return;
		}
	}

	public void BackBtnClicked(BaseEventData data)
	{
		GoBackToStore();
	}

	public void GoBackToStore()
	{
		SetVisible(false);
		UIStorePanel.Get().SetMainPanelVisibility(true);
	}

	private void NavButtonClicked(BaseEventData data)
	{
		if (data.selectedObject == null)
		{
			while (true)
			{
				switch (7)
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
		_ButtonSwapSprite component = data.selectedObject.GetComponent<_ButtonSwapSprite>();
		UIStoreSideNavButton component2 = component.selectableButton.GetComponent<UIStoreSideNavButton>();
		SelectPanel(component2);
	}

	public void SelectPanel(UIStoreSideNavButton btn)
	{
		StorePanelData[] panels = m_panels;
		foreach (StorePanelData storePanelData in panels)
		{
			storePanelData.Button.m_button.SetSelected(storePanelData.Button == btn, false, string.Empty, string.Empty);
			storePanelData.Panel.SetVisible(storePanelData.Button == btn);
			if (storePanelData.Button == btn)
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
				storePanelData.Panel.RefreshPage();
			}
		}
		while (true)
		{
			switch (5)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	public void Setup(CharacterResourceLink charLink)
	{
		PersistedCharacterData playerCharacterData = ClientGameManager.Get().GetPlayerCharacterData(charLink.m_characterType);
		m_charLink = charLink;
		m_characterName.text = charLink.GetDisplayName();
		m_characterIcon.sprite = Resources.Load<Sprite>(charLink.m_characterSelectIconResourceString);
		m_roleIcon.sprite = charLink.GetCharacterRoleIcon();
		m_charLevelText.text = playerCharacterData.ExperienceComponent.Level.ToString();
		string path = "Banners/Background/02_blue";
		GameBalanceVars.PlayerBanner banner = GameBalanceVars.Get().GetBanner(charLink.m_factionBannerID);
		if (banner != null)
		{
			path = banner.m_resourceString;
		}
		m_trustBanner.sprite = Resources.Load<Sprite>(path);
		for (int i = 0; i < m_panels.Length; i++)
		{
			m_panels[i].Panel.SetCharacter(charLink.m_characterType);
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
			OnCharacterDataUpdated(playerCharacterData);
			return;
		}
	}

	public bool IsVisible()
	{
		return m_container.gameObject.activeSelf;
	}

	public void SetVisible(bool visible)
	{
		if (visible)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
				{
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					for (int i = 0; i < m_panels.Length; i++)
					{
						UIManager.SetGameObjectActive(m_panels[i].Panel, false);
					}
					while (true)
					{
						switch (3)
						{
						case 0:
							break;
						default:
							SelectPanel(m_defaultPanelBtn);
							UIManager.SetGameObjectActive(m_container, true);
							UIManager.SetGameObjectActive(m_backgroundContainer, true);
							return;
						}
					}
				}
				}
			}
		}
		m_stylePanel.Display3dModel(false);
		if (m_animationController.gameObject.activeInHierarchy)
		{
			m_animationController.Play("HeroPageDefaultOUT");
		}
		UIManager.SetGameObjectActive(m_backgroundContainer, false);
	}

	public void NotifyLoseFocus()
	{
		m_stylePanel.Display3dModel(false);
		UIManager.SetGameObjectActive(m_container, false);
		UIManager.SetGameObjectActive(m_backgroundContainer, false);
	}

	public void NotifyGetFocus()
	{
		UICharacterStoreAndProgressWorldObjects.Get().SetVisible(true);
		UIManager.SetGameObjectActive(m_container, true);
		UIManager.SetGameObjectActive(m_backgroundContainer, true);
		m_stylePanel.Display3dModel(true);
	}

	public void HighlightCharacter(BaseEventData data)
	{
		UIActorModelData componentInChildren = UICharacterStoreAndProgressWorldObjects.Get().m_ringAnimations[0].GetComponentInChildren<UIActorModelData>();
		if (!(componentInChildren != null))
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			componentInChildren.SetMouseIsOver(true);
			return;
		}
	}

	public void UnhighlightCharacter(BaseEventData data)
	{
		UIActorModelData componentInChildren = UICharacterStoreAndProgressWorldObjects.Get().m_ringAnimations[0].GetComponentInChildren<UIActorModelData>();
		if (!(componentInChildren != null))
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
			componentInChildren.SetMouseIsOver(false);
			return;
		}
	}

	public CharacterType GetCurrentCharacterType()
	{
		if (m_charLink != null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return m_charLink.m_characterType;
				}
			}
		}
		return CharacterType.None;
	}

	private void CountChanged(UIStoreBasePanel panel, int ownedCount, int totalCount)
	{
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < m_panels.Length; i++)
		{
			if (m_panels[i].Panel == panel)
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
				m_panels[i].Button.m_ownedCount.text = ownedCount.ToString();
				m_panels[i].Button.m_totalCount.text = "/" + totalCount;
			}
			num += m_panels[i].Panel.GetNumTotal();
			num2 += m_panels[i].Panel.GetNumOwned();
		}
		m_totalOwnedText.text = num2.ToString();
		m_totalTotalText.text = "/" + num;
		if (num == 0)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					m_ownedProgressBar.fillAmount = 0f;
					UIManager.SetGameObjectActive(m_ownedCompleteContainer, false);
					return;
				}
			}
		}
		m_ownedProgressBar.fillAmount = (float)num2 / (float)num;
		UIManager.SetGameObjectActive(m_ownedCompleteContainer, num2 == num);
	}

	private void BuyInGameClicked(BaseEventData data)
	{
		UIPurchaseableItem uIPurchaseableItem = new UIPurchaseableItem();
		uIPurchaseableItem.m_itemType = PurchaseItemType.Character;
		uIPurchaseableItem.m_charLink = m_charLink;
		uIPurchaseableItem.m_currencyType = CurrencyType.FreelancerCurrency;
		UIStorePanel.Get().OpenPurchaseDialog(uIPurchaseableItem, PurchaseCharacterResponseHandler);
	}

	private void BuyForCashClicked(BaseEventData data)
	{
		UIPurchaseableItem uIPurchaseableItem = new UIPurchaseableItem();
		uIPurchaseableItem.m_itemType = PurchaseItemType.Character;
		uIPurchaseableItem.m_charLink = m_charLink;
		uIPurchaseableItem.m_purchaseForCash = true;
		UIStorePanel.Get().OpenPurchaseDialog(uIPurchaseableItem, PurchaseCharacterResponseHandler);
	}

	private void BuyForTokenClicked(BaseEventData data)
	{
		ClientGameManager.Get().PurchaseCharacter(CurrencyType.UnlockFreelancerToken, m_charLink.m_characterType, delegate(PurchaseCharacterResponse response)
		{
			PurchaseCharacterResponseHandler(response.Success, response.Result, response.CharacterType);
		});
	}

	private void PurchaseCharacterResponseHandler(bool success, PurchaseResult result, CharacterType characterType)
	{
		if (!success || result != PurchaseResult.Success)
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
			if (characterType == m_charLink.m_characterType)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					UIManager.SetGameObjectActive(m_buyHeroContainer, false);
					return;
				}
			}
			return;
		}
	}

	private void OnCharacterDataUpdated(PersistedCharacterData charData)
	{
		UpdateBuyButtons();
	}

	private void HandleBankBalanceChange(CurrencyData data)
	{
		if (data.m_Type != CurrencyType.UnlockFreelancerToken)
		{
			return;
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
			UpdateBuyButtons();
			return;
		}
	}

	private void UpdateBuyButtons()
	{
		ClientGameManager clientGameManager = ClientGameManager.Get();
		if (m_charLink == null)
		{
			return;
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
			if (clientGameManager == null)
			{
				return;
			}
			if (!clientGameManager.IsPlayerCharacterDataAvailable(m_charLink.m_characterType))
			{
				while (true)
				{
					switch (5)
					{
					default:
						return;
					case 0:
						break;
					}
				}
			}
			PersistedCharacterData playerCharacterData = ClientGameManager.Get().GetPlayerCharacterData(m_charLink.m_characterType);
			if (!clientGameManager.HasPurchasedGame)
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
				if (!playerCharacterData.CharacterComponent.Unlocked)
				{
					while (true)
					{
						RectTransform buyHeroContainer;
						int doActive;
						switch (7)
						{
						case 0:
							break;
						default:
							{
								int unlockFreelancerCurrencyPrice = m_charLink.m_charUnlockData.GetUnlockFreelancerCurrencyPrice();
								m_buyInGameLabel.text = "<sprite name=credit>" + unlockFreelancerCurrencyPrice;
								UIManager.SetGameObjectActive(m_buyInGameButton, unlockFreelancerCurrencyPrice > 0);
								string accountCurrency = HydrogenConfig.Get().Ticket.AccountCurrency;
								float freelancerPrice = CommerceClient.Get().GetFreelancerPrice(m_charLink.m_characterType, accountCurrency);
								m_buyForCashLabel.text = UIStorePanel.GetLocalizedPriceString(freelancerPrice, accountCurrency);
								UIManager.SetGameObjectActive(m_buyForCashButton, freelancerPrice > 0f);
								bool flag = ClientGameManager.Get().PlayerWallet.GetValue(CurrencyType.UnlockFreelancerToken).m_Amount > 0;
								UIManager.SetGameObjectActive(m_buyForTokenButton, flag);
								buyHeroContainer = m_buyHeroContainer;
								if (unlockFreelancerCurrencyPrice <= 0)
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
									if (!(freelancerPrice > 0f))
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
										doActive = (flag ? 1 : 0);
										goto IL_0194;
									}
								}
								doActive = 1;
								goto IL_0194;
							}
							IL_0194:
							UIManager.SetGameObjectActive(buyHeroContainer, (byte)doActive != 0);
							return;
						}
					}
				}
			}
			UIManager.SetGameObjectActive(m_buyHeroContainer, false);
			return;
		}
	}
}

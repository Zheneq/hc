using System;
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
		return UIStoreViewHeroPage.s_instance;
	}

	public override SceneType GetSceneType()
	{
		return SceneType.StoreViewHero;
	}

	public void SetRestrictedISOUSePopVisible(bool visible)
	{
		UIManager.SetGameObjectActive(this.m_restrictedIsoUsePopup, visible, null);
	}

	public override void Awake()
	{
		UIStoreViewHeroPage.s_instance = this;
		this.SetRestrictedISOUSePopVisible(false);
		UIManager.SetGameObjectActive(this.m_restrictedIsoUsePopup, false, null);
		this.m_restrictedIsoUseCloseBtn.callback = delegate(BaseEventData data)
		{
			this.SetRestrictedISOUSePopVisible(false);
			UIStorePanel.Get().NotifyGetFocus();
		};
		_ButtonSwapSprite restrictedIsoUseBuyGameBtn = this.m_restrictedIsoUseBuyGameBtn;
		
		restrictedIsoUseBuyGameBtn.callback = delegate(BaseEventData data)
			{
				UIStoreViewHeroPage.Get().SetRestrictedISOUSePopVisible(false);
				UIFrontEnd.Get().m_frontEndNavPanel.CashShopBtnClicked(null);
				UICashShopPanel.Get().ButtonClicked(UICashShopPanel.Get().m_gameBtn);
			};
		UIEventTriggerUtils.AddListener(this.m_modelHitbox.gameObject, EventTriggerType.PointerEnter, new UIEventTriggerUtils.EventDelegate(this.HighlightCharacter));
		UIEventTriggerUtils.AddListener(this.m_modelHitbox.gameObject, EventTriggerType.PointerExit, new UIEventTriggerUtils.EventDelegate(this.UnhighlightCharacter));
		this.m_backBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.BackBtnClicked);
		this.m_buyInGameButton.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.BuyInGameClicked);
		this.m_buyForCashButton.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.BuyForCashClicked);
		this.m_buyForTokenButton.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.BuyForTokenClicked);
		base.Awake();
	}

	private void Start()
	{
		this.SetVisible(false);
		foreach (StorePanelData storePanelData in this.m_panels)
		{
			storePanelData.Button.m_button.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.NavButtonClicked);
			storePanelData.Panel.OnCountsRefreshed += new Action<UIStoreBaseInventoryPanel, int, int>(this.CountChanged);
			storePanelData.Panel.Initialize();
			this.CountChanged(storePanelData.Panel, storePanelData.Panel.GetNumOwned(), storePanelData.Panel.GetNumTotal());
			storePanelData.Panel.SetParentContainer(base.gameObject);
			UIManager.SetGameObjectActive(storePanelData.Panel, false, null);
		}
		this.SelectPanel(this.m_defaultPanelBtn);
		ClientGameManager.Get().OnCharacterDataUpdated += this.OnCharacterDataUpdated;
		ClientGameManager.Get().OnBankBalanceChange += this.HandleBankBalanceChange;
	}

	private void OnDestroy()
	{
		UIStoreViewHeroPage.s_instance = null;
		foreach (StorePanelData storePanelData in this.m_panels)
		{
			storePanelData.Panel.OnCountsRefreshed -= new Action<UIStoreBaseInventoryPanel, int, int>(this.CountChanged);
		}
		if (ClientGameManager.Get() != null)
		{
			ClientGameManager.Get().OnCharacterDataUpdated -= this.OnCharacterDataUpdated;
			ClientGameManager.Get().OnBankBalanceChange -= this.HandleBankBalanceChange;
		}
	}

	public void BackBtnClicked(BaseEventData data)
	{
		this.GoBackToStore();
	}

	public void GoBackToStore()
	{
		this.SetVisible(false);
		UIStorePanel.Get().SetMainPanelVisibility(true);
	}

	private void NavButtonClicked(BaseEventData data)
	{
		if (data.selectedObject == null)
		{
			return;
		}
		_ButtonSwapSprite component = data.selectedObject.GetComponent<_ButtonSwapSprite>();
		UIStoreSideNavButton component2 = component.selectableButton.GetComponent<UIStoreSideNavButton>();
		this.SelectPanel(component2);
	}

	public void SelectPanel(UIStoreSideNavButton btn)
	{
		foreach (StorePanelData storePanelData in this.m_panels)
		{
			storePanelData.Button.m_button.SetSelected(storePanelData.Button == btn, false, string.Empty, string.Empty);
			storePanelData.Panel.SetVisible(storePanelData.Button == btn);
			if (storePanelData.Button == btn)
			{
				storePanelData.Panel.RefreshPage();
			}
		}
	}

	public void Setup(CharacterResourceLink charLink)
	{
		PersistedCharacterData playerCharacterData = ClientGameManager.Get().GetPlayerCharacterData(charLink.m_characterType);
		this.m_charLink = charLink;
		this.m_characterName.text = charLink.GetDisplayName();
		this.m_characterIcon.sprite = Resources.Load<Sprite>(charLink.m_characterSelectIconResourceString);
		this.m_roleIcon.sprite = charLink.GetCharacterRoleIcon();
		this.m_charLevelText.text = playerCharacterData.ExperienceComponent.Level.ToString();
		string path = "Banners/Background/02_blue";
		GameBalanceVars.PlayerBanner banner = GameBalanceVars.Get().GetBanner(charLink.m_factionBannerID);
		if (banner != null)
		{
			path = banner.m_resourceString;
		}
		this.m_trustBanner.sprite = Resources.Load<Sprite>(path);
		for (int i = 0; i < this.m_panels.Length; i++)
		{
			this.m_panels[i].Panel.SetCharacter(charLink.m_characterType);
		}
		this.OnCharacterDataUpdated(playerCharacterData);
	}

	public bool IsVisible()
	{
		return this.m_container.gameObject.activeSelf;
	}

	public void SetVisible(bool visible)
	{
		if (visible)
		{
			for (int i = 0; i < this.m_panels.Length; i++)
			{
				UIManager.SetGameObjectActive(this.m_panels[i].Panel, false, null);
			}
			this.SelectPanel(this.m_defaultPanelBtn);
			UIManager.SetGameObjectActive(this.m_container, true, null);
			UIManager.SetGameObjectActive(this.m_backgroundContainer, true, null);
		}
		else
		{
			this.m_stylePanel.Display3dModel(false);
			if (this.m_animationController.gameObject.activeInHierarchy)
			{
				this.m_animationController.Play("HeroPageDefaultOUT");
			}
			UIManager.SetGameObjectActive(this.m_backgroundContainer, false, null);
		}
	}

	public void NotifyLoseFocus()
	{
		this.m_stylePanel.Display3dModel(false);
		UIManager.SetGameObjectActive(this.m_container, false, null);
		UIManager.SetGameObjectActive(this.m_backgroundContainer, false, null);
	}

	public void NotifyGetFocus()
	{
		UICharacterStoreAndProgressWorldObjects.Get().SetVisible(true);
		UIManager.SetGameObjectActive(this.m_container, true, null);
		UIManager.SetGameObjectActive(this.m_backgroundContainer, true, null);
		this.m_stylePanel.Display3dModel(true);
	}

	public void HighlightCharacter(BaseEventData data)
	{
		UIActorModelData componentInChildren = UICharacterStoreAndProgressWorldObjects.Get().m_ringAnimations[0].GetComponentInChildren<UIActorModelData>();
		if (componentInChildren != null)
		{
			componentInChildren.SetMouseIsOver(true);
		}
	}

	public void UnhighlightCharacter(BaseEventData data)
	{
		UIActorModelData componentInChildren = UICharacterStoreAndProgressWorldObjects.Get().m_ringAnimations[0].GetComponentInChildren<UIActorModelData>();
		if (componentInChildren != null)
		{
			componentInChildren.SetMouseIsOver(false);
		}
	}

	public CharacterType GetCurrentCharacterType()
	{
		if (this.m_charLink != null)
		{
			return this.m_charLink.m_characterType;
		}
		return CharacterType.None;
	}

	private void CountChanged(UIStoreBasePanel panel, int ownedCount, int totalCount)
	{
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < this.m_panels.Length; i++)
		{
			if (this.m_panels[i].Panel == panel)
			{
				this.m_panels[i].Button.m_ownedCount.text = ownedCount.ToString();
				this.m_panels[i].Button.m_totalCount.text = "/" + totalCount.ToString();
			}
			num += this.m_panels[i].Panel.GetNumTotal();
			num2 += this.m_panels[i].Panel.GetNumOwned();
		}
		this.m_totalOwnedText.text = num2.ToString();
		this.m_totalTotalText.text = "/" + num.ToString();
		if (num == 0)
		{
			this.m_ownedProgressBar.fillAmount = 0f;
			UIManager.SetGameObjectActive(this.m_ownedCompleteContainer, false, null);
		}
		else
		{
			this.m_ownedProgressBar.fillAmount = (float)num2 / (float)num;
			UIManager.SetGameObjectActive(this.m_ownedCompleteContainer, num2 == num, null);
		}
	}

	private void BuyInGameClicked(BaseEventData data)
	{
		UIPurchaseableItem uipurchaseableItem = new UIPurchaseableItem();
		uipurchaseableItem.m_itemType = PurchaseItemType.Character;
		uipurchaseableItem.m_charLink = this.m_charLink;
		uipurchaseableItem.m_currencyType = CurrencyType.FreelancerCurrency;
		UIStorePanel.Get().OpenPurchaseDialog(uipurchaseableItem, new UIStorePanel.PurchaseCharacterCallback(this.PurchaseCharacterResponseHandler));
	}

	private void BuyForCashClicked(BaseEventData data)
	{
		UIPurchaseableItem uipurchaseableItem = new UIPurchaseableItem();
		uipurchaseableItem.m_itemType = PurchaseItemType.Character;
		uipurchaseableItem.m_charLink = this.m_charLink;
		uipurchaseableItem.m_purchaseForCash = true;
		UIStorePanel.Get().OpenPurchaseDialog(uipurchaseableItem, new UIStorePanel.PurchaseCharacterCallback(this.PurchaseCharacterResponseHandler));
	}

	private void BuyForTokenClicked(BaseEventData data)
	{
		ClientGameManager.Get().PurchaseCharacter(CurrencyType.UnlockFreelancerToken, this.m_charLink.m_characterType, delegate(PurchaseCharacterResponse response)
		{
			this.PurchaseCharacterResponseHandler(response.Success, response.Result, response.CharacterType);
		});
	}

	private void PurchaseCharacterResponseHandler(bool success, PurchaseResult result, CharacterType characterType)
	{
		if (success && result == PurchaseResult.Success)
		{
			if (characterType == this.m_charLink.m_characterType)
			{
				UIManager.SetGameObjectActive(this.m_buyHeroContainer, false, null);
			}
		}
	}

	private void OnCharacterDataUpdated(PersistedCharacterData charData)
	{
		this.UpdateBuyButtons();
	}

	private void HandleBankBalanceChange(CurrencyData data)
	{
		if (data.m_Type == CurrencyType.UnlockFreelancerToken)
		{
			this.UpdateBuyButtons();
		}
	}

	private void UpdateBuyButtons()
	{
		ClientGameManager clientGameManager = ClientGameManager.Get();
		if (!(this.m_charLink == null))
		{
			if (!(clientGameManager == null))
			{
				if (clientGameManager.IsPlayerCharacterDataAvailable(this.m_charLink.m_characterType))
				{
					PersistedCharacterData playerCharacterData = ClientGameManager.Get().GetPlayerCharacterData(this.m_charLink.m_characterType);
					if (!clientGameManager.HasPurchasedGame)
					{
						if (!playerCharacterData.CharacterComponent.Unlocked)
						{
							int unlockFreelancerCurrencyPrice = this.m_charLink.m_charUnlockData.GetUnlockFreelancerCurrencyPrice();
							this.m_buyInGameLabel.text = "<sprite name=credit>" + unlockFreelancerCurrencyPrice;
							UIManager.SetGameObjectActive(this.m_buyInGameButton, unlockFreelancerCurrencyPrice > 0, null);
							string accountCurrency = HydrogenConfig.Get().Ticket.AccountCurrency;
							float freelancerPrice = CommerceClient.Get().GetFreelancerPrice(this.m_charLink.m_characterType, accountCurrency);
							this.m_buyForCashLabel.text = UIStorePanel.GetLocalizedPriceString(freelancerPrice, accountCurrency);
							UIManager.SetGameObjectActive(this.m_buyForCashButton, freelancerPrice > 0f, null);
							bool flag = ClientGameManager.Get().PlayerWallet.GetValue(CurrencyType.UnlockFreelancerToken).m_Amount > 0;
							UIManager.SetGameObjectActive(this.m_buyForTokenButton, flag, null);
							Component buyHeroContainer = this.m_buyHeroContainer;
							bool doActive;
							if (unlockFreelancerCurrencyPrice <= 0)
							{
								if (freelancerPrice <= 0f)
								{
									doActive = flag;
									goto IL_194;
								}
							}
							doActive = true;
							IL_194:
							UIManager.SetGameObjectActive(buyHeroContainer, doActive, null);
							return;
						}
					}
					UIManager.SetGameObjectActive(this.m_buyHeroContainer, false, null);
					return;
				}
			}
		}
	}
}

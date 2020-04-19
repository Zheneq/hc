using System;
using I2.Loc;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UICashShopPanel : UIScene
{
	public RectTransform m_container;

	public UIStoreGamePanel m_gamePanel;

	public _SelectableBtn m_gameBtn;

	public UIStoreFeaturedPanel m_featuredPanel;

	public _SelectableBtn m_featuredBtn;

	public UIStoreGGPanel m_ggPanel;

	public _SelectableBtn m_ggBtn;

	public UIStoreLootMatrixPanel m_lootMatrixPanel;

	public _SelectableBtn m_lootMatrixBtn;

	public UIStoreCashShopCharacterPanel m_characterPanel;

	public _SelectableBtn m_characterBtn;

	public UICharacterBrowsersPanel m_characterBrowser;

	public _SelectableBtn m_miscBtn;

	public UIStoreCashShopStoreItemPanel m_miscPanel;

	public TextMeshProUGUI m_getEveryFreelancer;

	public TextMeshProUGUI[] m_gamepackTabTexts;

	[HideInInspector]
	public const bool m_enableStore = true;

	public RectTransform m_categoryContainer;

	private static UICashShopPanel s_instance;

	private bool m_isVisible;

	private _SelectableBtn m_selectedBtn;

	private int m_numLancers;

	public static UICashShopPanel Get()
	{
		return UICashShopPanel.s_instance;
	}

	public override void Awake()
	{
		UICashShopPanel.s_instance = this;
		UIManager.SetGameObjectActive(this.m_container, false, null);
		this.m_isVisible = false;
		base.Awake();
		this.m_gameBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.HandleButtonClickEvent);
		this.m_featuredBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.HandleButtonClickEvent);
		this.m_ggBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.HandleButtonClickEvent);
		this.m_lootMatrixBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.HandleButtonClickEvent);
		this.m_characterBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.HandleButtonClickEvent);
		this.m_miscBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.HandleButtonClickEvent);
		UIManager.SetGameObjectActive(this.m_featuredPanel, false, null);
		UIManager.SetGameObjectActive(this.m_gamePanel, false, null);
		UIManager.SetGameObjectActive(this.m_ggPanel, false, null);
		UIManager.SetGameObjectActive(this.m_lootMatrixPanel, false, null);
		UIManager.SetGameObjectActive(this.m_characterPanel, false, null);
		UIManager.SetGameObjectActive(this.m_miscPanel, false, null);
		this.m_characterBrowser.SetVisible(false);
		this.m_characterBrowser.m_closeBtn.callback = new _ButtonSwapSprite.ButtonClickCallback(this.CloseBtnClick);
		for (CharacterType characterType = CharacterType.None; characterType < CharacterType.Last; characterType++)
		{
			if (characterType.IsValidForHumanGameplay())
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
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(UICashShopPanel.Awake()).MethodHandle;
				}
				CharacterResourceLink characterResourceLink = GameWideData.Get().GetCharacterResourceLink(characterType);
				if (!characterResourceLink.m_isHidden && characterResourceLink.m_allowForPlayers)
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
					this.m_numLancers++;
				}
			}
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
		UIManager.SetGameObjectActive(this.m_categoryContainer, true, null);
	}

	private void Start()
	{
		ClientGameManager.Get().OnCharacterDataUpdated += this.OnCharacterDataUpdated;
		ClientGameManager.Get().OnAccountDataUpdated += this.OnAccountDataUpdated;
		ClientGameManager.Get().OnLobbyServerClientAccessLevelChange += this.HandleLobbyServerClientAccessLevelChange;
		this.HandleLobbyServerClientAccessLevelChange(ClientGameManager.Get().ClientAccessLevel, ClientGameManager.Get().ClientAccessLevel);
		LocalizationManager.OnLocalizeEvent += this.Localize;
		this.Localize();
	}

	private void OnDestroy()
	{
		if (ClientGameManager.Get() != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UICashShopPanel.OnDestroy()).MethodHandle;
			}
			ClientGameManager.Get().OnCharacterDataUpdated -= this.OnCharacterDataUpdated;
			ClientGameManager.Get().OnAccountDataUpdated -= this.OnAccountDataUpdated;
			ClientGameManager.Get().OnLobbyServerClientAccessLevelChange -= this.HandleLobbyServerClientAccessLevelChange;
		}
		LocalizationManager.OnLocalizeEvent -= this.Localize;
	}

	public override SceneType GetSceneType()
	{
		return SceneType.CashShop;
	}

	private void Localize()
	{
		this.m_getEveryFreelancer.text = string.Format(StringUtil.TR("NewGamePackFreelancerForever", "NewFrontEndScene"), this.m_numLancers);
		this.HandleLobbyServerClientAccessLevelChange(ClientGameManager.Get().ClientAccessLevel, ClientGameManager.Get().ClientAccessLevel);
	}

	public void SetVisible(bool visible)
	{
		if (visible == this.m_isVisible)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UICashShopPanel.SetVisible(bool)).MethodHandle;
			}
			return;
		}
		this.m_isVisible = visible;
		UIManager.SetGameObjectActive(this.m_container, visible, null);
		this.m_characterBrowser.SetVisible(false);
		this.ButtonClicked(this.m_featuredBtn);
		if (visible)
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
			if (UICharacterSelectWorldObjects.Get().IsVisible())
			{
				UICharacterSelectWorldObjects.Get().SetVisible(false);
			}
			AccountComponent.UIStateIdentifier uiState = AccountComponent.UIStateIdentifier.CashShopFeaturedItemsVersionViewed;
			int uistate = ClientGameManager.Get().GetPlayerAccountData().AccountComponent.GetUIState(uiState);
			int featuredItemsVersion = StoreWideData.Get().m_featuredItemsVersion;
			if (uistate < featuredItemsVersion)
			{
				ClientGameManager.Get().RequestUpdateUIState(uiState, featuredItemsVersion, null);
			}
		}
	}

	private void HandleButtonClickEvent(BaseEventData data)
	{
		PointerEventData pointerEventData = data as PointerEventData;
		if (pointerEventData != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UICashShopPanel.HandleButtonClickEvent(BaseEventData)).MethodHandle;
			}
			if (pointerEventData.pointerPress != null)
			{
				_ButtonSwapSprite component = pointerEventData.pointerPress.GetComponent<_ButtonSwapSprite>();
				if (component != null)
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
					if (!component.selectableButton.IsSelected())
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
						this.ButtonClicked(component.selectableButton);
					}
				}
			}
		}
	}

	public void HideAllPanels()
	{
		this.m_gamePanel.SetVisible(false);
		this.m_featuredPanel.SetVisible(false);
		this.m_ggPanel.SetVisible(false);
		this.m_lootMatrixPanel.SetVisible(false);
		this.m_characterPanel.SetVisible(false);
	}

	public void ButtonClicked(_SelectableBtn button)
	{
		this.m_selectedBtn = button;
		this.m_gameBtn.SetSelected(button == this.m_gameBtn, false, string.Empty, string.Empty);
		this.m_gamePanel.SetVisible(button == this.m_gameBtn);
		this.m_featuredBtn.SetSelected(button == this.m_featuredBtn, false, string.Empty, string.Empty);
		this.m_featuredPanel.SetVisible(button == this.m_featuredBtn);
		this.m_ggBtn.SetSelected(button == this.m_ggBtn, false, string.Empty, string.Empty);
		this.m_ggPanel.SetVisible(button == this.m_ggBtn);
		this.m_lootMatrixBtn.SetSelected(button == this.m_lootMatrixBtn, false, string.Empty, string.Empty);
		this.m_lootMatrixPanel.SetVisible(button == this.m_lootMatrixBtn);
		this.m_characterBtn.SetSelected(button == this.m_characterBtn, false, string.Empty, string.Empty);
		this.m_characterPanel.SetVisible(button == this.m_characterBtn);
		this.m_characterBrowser.SetVisible(false);
		this.m_miscBtn.SetSelected(button == this.m_miscBtn, false, string.Empty, string.Empty);
		this.m_miscPanel.SetVisible(button == this.m_miscBtn);
	}

	private void HandleLobbyServerClientAccessLevelChange(ClientAccessLevel oldLevel, ClientAccessLevel newLevel)
	{
		ClientGameManager clientGameManager = ClientGameManager.Get();
		UIManager.SetGameObjectActive(this.m_gameBtn, clientGameManager.HighestPurchasedGamePack < GameWideData.Get().m_gamePackData.m_gamePacks.Length - 1, null);
		UIManager.SetGameObjectActive(this.m_characterBtn, !clientGameManager.HasPurchasedGame, null);
		this.m_gamePanel.Setup();
		this.m_featuredPanel.Refresh();
		string text;
		if (clientGameManager.HasPurchasedGame)
		{
			text = StringUtil.TR("Upgrade", "OverlayScreensScene");
		}
		else
		{
			text = StringUtil.TR("GamePack", "Store");
		}
		for (int i = 0; i < this.m_gamepackTabTexts.Length; i++)
		{
			this.m_gamepackTabTexts[i].text = text;
		}
	}

	public void OnCharacterDataUpdated(PersistedCharacterData newData)
	{
		this.m_featuredPanel.Refresh();
	}

	public void OnAccountDataUpdated(PersistedAccountData newData)
	{
		this.m_featuredPanel.Refresh();
	}

	public bool IsVisible()
	{
		return this.m_isVisible;
	}

	public void NotifyLoseFocus()
	{
		UIManager.SetGameObjectActive(this.m_container, false, null);
	}

	public void NotifyGetFocus()
	{
		UIManager.SetGameObjectActive(this.m_container, true, null);
	}

	private void CloseBtnClick(BaseEventData data)
	{
		this.ButtonClicked(this.m_selectedBtn);
	}
}

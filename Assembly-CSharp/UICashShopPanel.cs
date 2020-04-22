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
		return s_instance;
	}

	public override void Awake()
	{
		s_instance = this;
		UIManager.SetGameObjectActive(m_container, false);
		m_isVisible = false;
		base.Awake();
		m_gameBtn.spriteController.callback = HandleButtonClickEvent;
		m_featuredBtn.spriteController.callback = HandleButtonClickEvent;
		m_ggBtn.spriteController.callback = HandleButtonClickEvent;
		m_lootMatrixBtn.spriteController.callback = HandleButtonClickEvent;
		m_characterBtn.spriteController.callback = HandleButtonClickEvent;
		m_miscBtn.spriteController.callback = HandleButtonClickEvent;
		UIManager.SetGameObjectActive(m_featuredPanel, false);
		UIManager.SetGameObjectActive(m_gamePanel, false);
		UIManager.SetGameObjectActive(m_ggPanel, false);
		UIManager.SetGameObjectActive(m_lootMatrixPanel, false);
		UIManager.SetGameObjectActive(m_characterPanel, false);
		UIManager.SetGameObjectActive(m_miscPanel, false);
		m_characterBrowser.SetVisible(false);
		m_characterBrowser.m_closeBtn.callback = CloseBtnClick;
		for (CharacterType characterType = CharacterType.None; characterType < CharacterType.Last; characterType++)
		{
			if (!characterType.IsValidForHumanGameplay())
			{
				continue;
			}
			CharacterResourceLink characterResourceLink = GameWideData.Get().GetCharacterResourceLink(characterType);
			if (!characterResourceLink.m_isHidden && characterResourceLink.m_allowForPlayers)
			{
				m_numLancers++;
			}
		}
		while (true)
		{
			UIManager.SetGameObjectActive(m_categoryContainer, true);
			return;
		}
	}

	private void Start()
	{
		ClientGameManager.Get().OnCharacterDataUpdated += OnCharacterDataUpdated;
		ClientGameManager.Get().OnAccountDataUpdated += OnAccountDataUpdated;
		ClientGameManager.Get().OnLobbyServerClientAccessLevelChange += HandleLobbyServerClientAccessLevelChange;
		HandleLobbyServerClientAccessLevelChange(ClientGameManager.Get().ClientAccessLevel, ClientGameManager.Get().ClientAccessLevel);
		LocalizationManager.OnLocalizeEvent += Localize;
		Localize();
	}

	private void OnDestroy()
	{
		if (ClientGameManager.Get() != null)
		{
			ClientGameManager.Get().OnCharacterDataUpdated -= OnCharacterDataUpdated;
			ClientGameManager.Get().OnAccountDataUpdated -= OnAccountDataUpdated;
			ClientGameManager.Get().OnLobbyServerClientAccessLevelChange -= HandleLobbyServerClientAccessLevelChange;
		}
		LocalizationManager.OnLocalizeEvent -= Localize;
	}

	public override SceneType GetSceneType()
	{
		return SceneType.CashShop;
	}

	private void Localize()
	{
		m_getEveryFreelancer.text = string.Format(StringUtil.TR("NewGamePackFreelancerForever", "NewFrontEndScene"), m_numLancers);
		HandleLobbyServerClientAccessLevelChange(ClientGameManager.Get().ClientAccessLevel, ClientGameManager.Get().ClientAccessLevel);
	}

	public void SetVisible(bool visible)
	{
		if (visible == m_isVisible)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		m_isVisible = visible;
		UIManager.SetGameObjectActive(m_container, visible);
		m_characterBrowser.SetVisible(false);
		ButtonClicked(m_featuredBtn);
		if (!visible)
		{
			return;
		}
		while (true)
		{
			if (UICharacterSelectWorldObjects.Get().IsVisible())
			{
				UICharacterSelectWorldObjects.Get().SetVisible(false);
			}
			AccountComponent.UIStateIdentifier uiState = AccountComponent.UIStateIdentifier.CashShopFeaturedItemsVersionViewed;
			int uIState = ClientGameManager.Get().GetPlayerAccountData().AccountComponent.GetUIState(uiState);
			int featuredItemsVersion = StoreWideData.Get().m_featuredItemsVersion;
			if (uIState < featuredItemsVersion)
			{
				ClientGameManager.Get().RequestUpdateUIState(uiState, featuredItemsVersion, null);
			}
			return;
		}
	}

	private void HandleButtonClickEvent(BaseEventData data)
	{
		PointerEventData pointerEventData = data as PointerEventData;
		if (pointerEventData == null)
		{
			return;
		}
		while (true)
		{
			if (!(pointerEventData.pointerPress != null))
			{
				return;
			}
			_ButtonSwapSprite component = pointerEventData.pointerPress.GetComponent<_ButtonSwapSprite>();
			if (!(component != null))
			{
				return;
			}
			while (true)
			{
				if (!component.selectableButton.IsSelected())
				{
					while (true)
					{
						ButtonClicked(component.selectableButton);
						return;
					}
				}
				return;
			}
		}
	}

	public void HideAllPanels()
	{
		m_gamePanel.SetVisible(false);
		m_featuredPanel.SetVisible(false);
		m_ggPanel.SetVisible(false);
		m_lootMatrixPanel.SetVisible(false);
		m_characterPanel.SetVisible(false);
	}

	public void ButtonClicked(_SelectableBtn button)
	{
		m_selectedBtn = button;
		m_gameBtn.SetSelected(button == m_gameBtn, false, string.Empty, string.Empty);
		m_gamePanel.SetVisible(button == m_gameBtn);
		m_featuredBtn.SetSelected(button == m_featuredBtn, false, string.Empty, string.Empty);
		m_featuredPanel.SetVisible(button == m_featuredBtn);
		m_ggBtn.SetSelected(button == m_ggBtn, false, string.Empty, string.Empty);
		m_ggPanel.SetVisible(button == m_ggBtn);
		m_lootMatrixBtn.SetSelected(button == m_lootMatrixBtn, false, string.Empty, string.Empty);
		m_lootMatrixPanel.SetVisible(button == m_lootMatrixBtn);
		m_characterBtn.SetSelected(button == m_characterBtn, false, string.Empty, string.Empty);
		m_characterPanel.SetVisible(button == m_characterBtn);
		m_characterBrowser.SetVisible(false);
		m_miscBtn.SetSelected(button == m_miscBtn, false, string.Empty, string.Empty);
		m_miscPanel.SetVisible(button == m_miscBtn);
	}

	private void HandleLobbyServerClientAccessLevelChange(ClientAccessLevel oldLevel, ClientAccessLevel newLevel)
	{
		ClientGameManager clientGameManager = ClientGameManager.Get();
		UIManager.SetGameObjectActive(m_gameBtn, clientGameManager.HighestPurchasedGamePack < GameWideData.Get().m_gamePackData.m_gamePacks.Length - 1);
		UIManager.SetGameObjectActive(m_characterBtn, !clientGameManager.HasPurchasedGame);
		m_gamePanel.Setup();
		m_featuredPanel.Refresh();
		string text = (!clientGameManager.HasPurchasedGame) ? StringUtil.TR("GamePack", "Store") : StringUtil.TR("Upgrade", "OverlayScreensScene");
		for (int i = 0; i < m_gamepackTabTexts.Length; i++)
		{
			m_gamepackTabTexts[i].text = text;
		}
	}

	public void OnCharacterDataUpdated(PersistedCharacterData newData)
	{
		m_featuredPanel.Refresh();
	}

	public void OnAccountDataUpdated(PersistedAccountData newData)
	{
		m_featuredPanel.Refresh();
	}

	public bool IsVisible()
	{
		return m_isVisible;
	}

	public void NotifyLoseFocus()
	{
		UIManager.SetGameObjectActive(m_container, false);
	}

	public void NotifyGetFocus()
	{
		UIManager.SetGameObjectActive(m_container, true);
	}

	private void CloseBtnClick(BaseEventData data)
	{
		ButtonClicked(m_selectedBtn);
	}
}

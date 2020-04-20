using System;
using System.Collections.Generic;
using System.Linq;
using LobbyGameClientMessages;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIRankedModeDraftScreen : UIScene
{
	public RectTransform m_draftScreenContainer;

	public TextMeshProUGUI m_MessageText;

	public Color m_blueTeamColor;

	public Color m_redTeamColor;

	public Color m_neutralColor;

	[Header("Countdown Numbers")]
	public TextMeshProUGUI m_gameCountdownTimer;

	public TextMeshProUGUI m_redCountdownTimer;

	public TextMeshProUGUI m_blueCountdownTimer;

	public Animator m_gameCountdownAC;

	public Animator m_redCountdownAC;

	public Animator m_blueCountdownAC;

	public UIRankedModeDraftCharacterEntry[] m_blueBans;

	public UIRankedModeDraftCharacterEntry[] m_redBans;

	public Image m_stageImage;

	public TextMeshProUGUI m_stageText;

	public Image m_introStageImage;

	public TextMeshProUGUI m_introStageText;

	public TextMeshProUGUI m_matchFoundText;

	public RectTransform m_introContainer;

	[Header("Subphase Notifications")]
	public Animator m_blueTeamTurnNotification;

	public Animator m_redTeamTurnNotification;

	public Animator m_singleSelectionAC;

	public Animator m_doubleSelectionAC;

	public Animator m_swapPhaseAC;

	public Animator m_loadoutPhaseAC;

	public Animator m_gameLoadingAC;

	public TextMeshProUGUI m_blueTeamTurnTextNotification;

	public TextMeshProUGUI m_redTeamTurnTextNotification;

	[Header("Single Select")]
	public RectTransform[] singleSelectionBlueTeam;

	public RectTransform[] singleSelectionRedTeam;

	public Image singleNoSelectionCharacter;

	public Image singleBrowseSelectionCharacter;

	public Image singleSelectionCharacter;

	public TextMeshProUGUI m_singleCharacterName;

	public Animator m_singleSelectionCharacterSelected;

	public Animator m_singleBlueSelectionCharacterSelected;

	public Animator m_singleRedTeamSelectionCharacterSelected;

	public TextMeshProUGUI m_singleBlueTeamSelectedCharacter;

	public TextMeshProUGUI m_singleRedTeamSelectedCharacter;

	public TextMeshProUGUI m_singleBlueTeamPlayerName;

	public TextMeshProUGUI m_singleRedTeamPlayerName;

	[Header("Double Select")]
	public RectTransform[] doubleSelectionBlueTeam;

	public RectTransform[] doubleSelectionRedTeam;

	public Image doubleNoSelectionLeftCharacter;

	public Image doubleBrowseSelectionLeftCharacter;

	public Image doubleSelectionLeftCharacter;

	public Image doubleNoSelectionRightCharacter;

	public Image doubleBrowseSelectionRightCharacter;

	public Image doubleSelectionRightCharacter;

	public TextMeshProUGUI m_leftCharacterName;

	public TextMeshProUGUI m_rightCharacterName;

	public Animator m_doubleLeftSelectionCharacterSelected;

	public Animator m_doubleLeftBlueSelectionCharacterSelected;

	public Animator m_doubleLeftRedTeamSelectionCharacterSelected;

	public TextMeshProUGUI m_doubleLeftBlueTeamSelectedCharacter;

	public TextMeshProUGUI m_doubleLeftRedTeamSelectedCharacter;

	public TextMeshProUGUI m_doubleLeftBlueTeamPlayerName;

	public TextMeshProUGUI m_doubleLeftRedTeamPlayerName;

	public Animator m_doubleRightSelectionCharacterSelected;

	public Animator m_doubleRightBlueSelectionCharacterSelected;

	public Animator m_doubleRightRedTeamSelectionCharacterSelected;

	public TextMeshProUGUI m_doubleRightBlueTeamSelectedCharacter;

	public TextMeshProUGUI m_doubleRightRedTeamSelectedCharacter;

	public TextMeshProUGUI m_doubleRightBlueTeamPlayerName;

	public TextMeshProUGUI m_doubleRightRedTeamPlayerName;

	public RectTransform m_swapContainer;

	public RectTransform m_versusContainer;

	public UIRankedModePlayerDraftEntry[] m_blueTeamMembers;

	public UIRankedModePlayerDraftEntry[] m_redTeamMembers;

	public _SelectableBtn m_skinsBtn;

	public _SelectableBtn m_abilitiesBtn;

	public _SelectableBtn m_catalystsBtn;

	public _SelectableBtn m_tauntsBtn;

	public LayoutGroup m_characterSelectContainer;

	public LayoutGroup m_firePowerLayoutGroup;

	public LayoutGroup m_supportLayoutGroup;

	public LayoutGroup m_frontlinerLayoutGroup;

	public UICharacterPanelSelectButton m_characterSelectBtnPrefab;

	public HorizontalLayoutGroup m_pagesContainer;

	public _SelectableBtn m_pageBtnPrefab;

	public HorizontalLayoutGroup m_searchFiltersContainer;

	public TMP_InputField m_searchInputField;

	public UICharacterSelectFactionFilter m_factionFilterPrefab;

	public UICharacterSelectFactionFilter m_notOnAFactionFilter;

	public RectTransform m_lockFreelancerContainer;

	public _SelectableBtn m_lockFreelancerBtn;

	public _SelectableBtn m_lockInBtn;

	public TextMeshProUGUI[] m_lockInText;

	public float m_timeForPageToSwap = 900f;

	public UIRankedCharacterSelectSettingsPanel m_rankedModeCharacterSettings;

	private List<UICharacterPanelSelectRankModeButton> m_characterListDisplayButtons = new List<UICharacterPanelSelectRankModeButton>();

	private List<_SelectableBtn> m_pageButtons = new List<_SelectableBtn>();

	private int m_currentCharacterPage;

	private int m_currentVisiblePage;

	private float m_startTime;

	private float m_journeyLength;

	private Vector2 m_startLocation;

	private Vector2 m_endLocation;

	private CharacterType m_assignedCharacterForGame;

	private CharacterType m_selectedSubPhaseCharacter;

	private CharacterType m_hoverCharacterForGame;

	private List<CharacterType> m_validCharacterTypes = new List<CharacterType>();

	private List<CharacterType> m_selectedCharacterTypes = new List<CharacterType>();

	private List<CharacterType> m_friendlyBannedCharacterTypes = new List<CharacterType>();

	private List<CharacterType> m_enemyBannedCharacterTypes = new List<CharacterType>();

	private Dictionary<int, CharacterType> m_playerIDsOnDeck = new Dictionary<int, CharacterType>();

	private List<int> m_playerIDsThatSelected = new List<int>();

	private bool m_initialized;

	private bool m_IsOnDeck;

	private FreelancerResolutionPhaseSubType m_lastSetupSelectionPhaseSubType;

	private FreelancerResolutionPhaseSubType m_lastPhaseForUpdateCenter;

	private int m_playerIDBeingAnimated;

	private Animator m_animatorCurrentlyAnimating;

	private LobbyGameInfo LastGameInfo;

	private LobbyTeamInfo LastTeamInfo;

	private LobbyPlayerInfo LastPlayerInfo;

	private EnterFreelancerResolutionPhaseNotification m_lastDraftNotification;

	private float m_phaseStartTime;

	private TimeSpan m_timeInPhase;

	private float m_loadoutSelectStartTime;

	private GameStatus m_lastGameStatus;

	private List<UIRankedModeDraftScreen.CenterNotification> m_stateQueues = new List<UIRankedModeDraftScreen.CenterNotification>();

	private UIRankedModeDraftScreen.CenterNotification m_currentState;

	private List<GameObject> m_centerStateObjects = new List<GameObject>();

	private CanvasGroup m_characterSelectContainerCanvasGroup;

	private Animator m_containerAC;

	private bool m_intendedLockInBtnStatus;

	private List<UICharacterSelectFactionFilter> m_filterButtons;

	private UICharacterSelectFactionFilter m_lastFilterBtnClicked;

	private static UIRankedModeDraftScreen s_instance;

	public bool IsVisible { get; private set; }

	public bool GameIsLaunching { get; private set; }

	public CharacterType HoveredCharacter
	{
		get
		{
			return this.m_hoverCharacterForGame;
		}
		private set
		{
			this.m_hoverCharacterForGame = value;
			if (value != CharacterType.None)
			{
				this.m_selectedSubPhaseCharacter = value;
				this.SetupCharacterSettings(value);
			}
		}
	}

	private void SetupCharacterSettings(CharacterType charType)
	{
		CharacterCardInfo characterCardInfo;
		CharacterVisualInfo characterVisualInfo;
		if (this.LastPlayerInfo.CharacterType == charType)
		{
			characterCardInfo = this.LastPlayerInfo.CharacterInfo.CharacterCards;
			characterVisualInfo = this.LastPlayerInfo.CharacterInfo.CharacterSkin;
		}
		else
		{
			PersistedCharacterData playerCharacterData = ClientGameManager.Get().GetPlayerCharacterData(charType);
			characterCardInfo = playerCharacterData.CharacterComponent.LastCards;
			characterVisualInfo = playerCharacterData.CharacterComponent.LastSkin;
		}
		this.m_rankedModeCharacterSettings.UpdateSelectedCharType(charType);
		CharacterResourceLink characterResourceLink = GameWideData.Get().GetCharacterResourceLink(charType);
		if (!this.m_rankedModeCharacterSettings.m_spellsSubPanel.GetDisplayedCardInfo().Equals(characterCardInfo))
		{
			this.m_rankedModeCharacterSettings.m_spellsSubPanel.Setup(charType, characterCardInfo, false, false);
		}
		if (!(this.m_rankedModeCharacterSettings.m_abilitiesSubPanel.GetDisplayedCharacter() == null))
		{
			if (this.m_rankedModeCharacterSettings.m_abilitiesSubPanel.GetDisplayedCharacter().m_characterType.Equals(characterResourceLink.m_characterType))
			{
				goto IL_143;
			}
		}
		this.m_rankedModeCharacterSettings.m_abilitiesSubPanel.Setup(characterResourceLink, false);
		IL_143:
		if (this.m_rankedModeCharacterSettings.m_skinsSubPanel.GetDisplayedCharacterType().Equals(characterResourceLink.m_characterType))
		{
			if (this.m_rankedModeCharacterSettings.m_skinsSubPanel.GetDisplayedVisualInfo().Equals(characterVisualInfo))
			{
				goto IL_1BF;
			}
		}
		this.m_rankedModeCharacterSettings.m_skinsSubPanel.Setup(characterResourceLink, characterVisualInfo, false);
		IL_1BF:
		if (!(this.m_rankedModeCharacterSettings.m_tauntsSubPanel.GetDisplayedCharacter() == null))
		{
			if (this.m_rankedModeCharacterSettings.m_tauntsSubPanel.GetDisplayedCharacter().m_characterType.Equals(characterResourceLink.m_characterType))
			{
				return;
			}
		}
		this.m_rankedModeCharacterSettings.m_tauntsSubPanel.Setup(characterResourceLink, false);
	}

	public CharacterType ClientClickedCharacter
	{
		get
		{
			return this.m_selectedSubPhaseCharacter;
		}
	}

	public CharacterType SelectedCharacter
	{
		get
		{
			return this.m_assignedCharacterForGame;
		}
		private set
		{
			if (value != CharacterType.None)
			{
				if (this.LastGameInfo != null)
				{
					if (this.m_assignedCharacterForGame != value)
					{
						this.m_assignedCharacterForGame = value;
						this.m_hoverCharacterForGame = value;
						this.m_selectedSubPhaseCharacter = value;
						this.SetupCharacterSettings(value);
					}
				}
			}
		}
	}

	public static UIRankedModeDraftScreen Get()
	{
		return UIRankedModeDraftScreen.s_instance;
	}

	public override SceneType GetSceneType()
	{
		return SceneType.RankDraft;
	}

	public override void Awake()
	{
		if (UIRankedModeDraftScreen.s_instance == null)
		{
			UIRankedModeDraftScreen.s_instance = this;
			ClientGameManager.Get().OnGameInfoNotification += this.HandleGameInfoNotification;
			ClientGameManager.Get().OnLobbyGameplayOverridesChange += this.OnLobbyGameplayOverridesUpdated;
			this.m_lockInBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.LockPhaseButtonClicked);
			this.m_lockInBtn.spriteController.m_soundToPlay = FrontEndButtonSounds.RankFreelancerSelectClick;
			this.m_lockFreelancerBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.LockFreelancerBtnClicked);
			this.m_lockFreelancerBtn.spriteController.m_soundToPlay = FrontEndButtonSounds.RankFreelancerLockin;
			this.m_skinsBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.SettingsButtonClicked);
			this.m_abilitiesBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.SettingsButtonClicked);
			this.m_catalystsBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.SettingsButtonClicked);
			this.m_tauntsBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.SettingsButtonClicked);
			this.m_searchInputField.onValueChanged.AddListener(new UnityAction<string>(this.EditedSearchInput));
			this.m_filterButtons = new List<UICharacterSelectFactionFilter>();
			List<CharacterType> list = new List<CharacterType>();
			list.AddRange((CharacterType[])Enum.GetValues(typeof(CharacterType)));
			this.m_filterButtons.Add(this.m_notOnAFactionFilter);
			List<FactionGroup> list2 = FactionWideData.Get().FactionGroupsToDisplayFilter();
			for (int i = 0; i < list2.Count; i++)
			{
				FactionGroup groupFilter = list2[i];
				UICharacterSelectFactionFilter uicharacterSelectFactionFilter = UnityEngine.Object.Instantiate<UICharacterSelectFactionFilter>(this.m_factionFilterPrefab);
				uicharacterSelectFactionFilter.transform.SetParent(this.m_searchFiltersContainer.transform);
				uicharacterSelectFactionFilter.transform.localPosition = Vector3.zero;
				uicharacterSelectFactionFilter.transform.localScale = Vector3.one;
				uicharacterSelectFactionFilter.Setup(groupFilter, new Action<UICharacterSelectFactionFilter>(this.ClickedOnFactionFilter));
				this.m_filterButtons.Add(uicharacterSelectFactionFilter);
				if (groupFilter.Characters != null)
				{
					list = list.Except(groupFilter.Characters).ToList<CharacterType>();
				}
				uicharacterSelectFactionFilter.m_btn.spriteController.GetComponent<UITooltipHoverObject>().Setup(TooltipType.Simple, delegate(UITooltipBase tooltip)
				{
					(tooltip as UISimpleTooltip).Setup(FactionGroup.GetDisplayName(groupFilter.FactionGroupID));
					return true;
				}, null);
			}
			this.m_notOnAFactionFilter.Setup(list, new Action<UICharacterSelectFactionFilter>(this.ClickedOnFactionFilter));
			UITooltipObject component = this.m_notOnAFactionFilter.m_btn.spriteController.GetComponent<UITooltipHoverObject>();
			TooltipType tooltipType = TooltipType.Simple;
			
			component.Setup(tooltipType, delegate(UITooltipBase tooltip)
				{
					(tooltip as UISimpleTooltip).Setup(StringUtil.TR("Wildcard", "Global"));
					return true;
				}, null);
			this.m_centerStateObjects.Add(this.m_blueTeamTurnNotification.gameObject);
			this.m_centerStateObjects.Add(this.m_redTeamTurnNotification.gameObject);
			this.m_centerStateObjects.Add(this.m_singleSelectionAC.gameObject);
			this.m_centerStateObjects.Add(this.m_doubleSelectionAC.gameObject);
			this.m_centerStateObjects.Add(this.m_swapPhaseAC.gameObject);
			this.m_centerStateObjects.Add(this.m_loadoutPhaseAC.gameObject);
			this.m_centerStateObjects.Add(this.m_gameLoadingAC.gameObject);
			this.m_containerAC = this.m_draftScreenContainer.GetComponent<Animator>();
			base.Awake();
		}
	}

	private bool IsBanned(CharacterType characterType)
	{
		if (!this.m_friendlyBannedCharacterTypes.IsNullOrEmpty<CharacterType>())
		{
			if (this.m_friendlyBannedCharacterTypes.Contains(characterType))
			{
				return true;
			}
		}
		bool result;
		if (!this.m_enemyBannedCharacterTypes.IsNullOrEmpty<CharacterType>())
		{
			result = this.m_enemyBannedCharacterTypes.Contains(characterType);
		}
		else
		{
			result = false;
		}
		return result;
	}

	public void EditedSearchInput(string input)
	{
		this.UpdateCharacterButtonHighlights();
	}

	public void ClickedOnFactionFilter(UICharacterSelectFactionFilter btn)
	{
		if (this.m_lastFilterBtnClicked != null)
		{
			if (this.m_lastFilterBtnClicked != btn)
			{
				this.m_lastFilterBtnClicked.m_btn.SetSelected(false, false, string.Empty, string.Empty);
			}
		}
		this.m_lastFilterBtnClicked = btn;
		this.UpdateCharacterButtonHighlights();
	}

	private void UpdateCharacterButtonHighlights()
	{
		for (int i = 0; i < this.m_characterListDisplayButtons.Count; i++)
		{
			if (this.m_characterListDisplayButtons[i] != null)
			{
				if (this.m_characterListDisplayButtons[i].GetComponent<CanvasGroup>() != null)
				{
					this.m_characterListDisplayButtons[i].GetComponent<CanvasGroup>().alpha = 1f;
				}
			}
		}
		if (this.m_lastFilterBtnClicked != null && this.m_lastFilterBtnClicked.m_btn.IsSelected())
		{
			for (int j = 0; j < this.m_characterListDisplayButtons.Count; j++)
			{
				if (!this.m_lastFilterBtnClicked.IsAvailable(this.m_characterListDisplayButtons[j].m_characterType))
				{
					CanvasGroup component = this.m_characterListDisplayButtons[j].GetComponent<CanvasGroup>();
					if (component != null)
					{
						component.alpha = 0.3f;
					}
				}
			}
		}
		if (!this.m_searchInputField.text.IsNullOrEmpty())
		{
			for (int k = 0; k < this.m_characterListDisplayButtons.Count; k++)
			{
				string text = string.Empty;
				CharacterResourceLink characterResourceLink = this.m_characterListDisplayButtons[k].GetCharacterResourceLink();
				if (characterResourceLink != null)
				{
					text = characterResourceLink.GetDisplayName();
				}
				if (!this.DoesSearchMatchDisplayName(this.m_searchInputField.text.ToLower(), text.ToLower()))
				{
					this.m_characterListDisplayButtons[k].GetComponent<CanvasGroup>().alpha = 0.3f;
				}
			}
		}
	}

	private bool DoesSearchMatchDisplayName(string searchText, string displayText)
	{
		for (int i = 0; i < searchText.Length; i++)
		{
			if (i >= displayText.Length)
			{
				break;
			}
			if (searchText[i] != displayText[i])
			{
				return false;
			}
		}
		return true;
	}

	public void DoQueueState(UIRankedModeDraftScreen.CenterNotification notification)
	{
		UIManager.SetGameObjectActive(this.m_blueTeamTurnNotification, notification == UIRankedModeDraftScreen.CenterNotification.BlueTeamNotification, null);
		UIManager.SetGameObjectActive(this.m_redTeamTurnNotification, notification == UIRankedModeDraftScreen.CenterNotification.RedTeamNotification, null);
		UIManager.SetGameObjectActive(this.m_singleSelectionAC, notification == UIRankedModeDraftScreen.CenterNotification.BlueTeamSingleSelectStart || notification == UIRankedModeDraftScreen.CenterNotification.RedTeamSingleSelectStart, null);
		UIManager.SetGameObjectActive(this.m_doubleSelectionAC, notification == UIRankedModeDraftScreen.CenterNotification.BlueTeamDoubleSelectStart || notification == UIRankedModeDraftScreen.CenterNotification.RedTeamDoubleSelectStart, null);
		UIManager.SetGameObjectActive(this.m_swapPhaseAC, notification == UIRankedModeDraftScreen.CenterNotification.TradePhase, null);
		UIManager.SetGameObjectActive(this.m_loadoutPhaseAC, notification == UIRankedModeDraftScreen.CenterNotification.LoadoutPhase, null);
		UIManager.SetGameObjectActive(this.m_gameLoadingAC, notification == UIRankedModeDraftScreen.CenterNotification.GameLoadPhase, null);
		if (this.m_lastDraftNotification.SubPhase.IsPickBanSubPhase())
		{
			this.m_blueTeamTurnTextNotification.text = StringUtil.TR("BlueBans", "RankMode");
			this.m_redTeamTurnTextNotification.text = StringUtil.TR("RedBans", "RankMode");
		}
		else if (this.m_lastDraftNotification.SubPhase.IsPickFreelancerSubPhase())
		{
			this.m_blueTeamTurnTextNotification.text = StringUtil.TR("BluePicks", "RankMode");
			this.m_redTeamTurnTextNotification.text = StringUtil.TR("RedPicks", "RankMode");
		}
		bool flag;
		if (notification != UIRankedModeDraftScreen.CenterNotification.BlueTeamNotification)
		{
			if (notification != UIRankedModeDraftScreen.CenterNotification.BlueTeamSingleSelectStart)
			{
				flag = (notification == UIRankedModeDraftScreen.CenterNotification.BlueTeamDoubleSelectStart);
				goto IL_150;
			}
		}
		flag = true;
		IL_150:
		bool flag2 = flag;
		bool flag3;
		if (notification != UIRankedModeDraftScreen.CenterNotification.RedTeamNotification)
		{
			if (notification != UIRankedModeDraftScreen.CenterNotification.RedTeamSingleSelectStart)
			{
				flag3 = (notification == UIRankedModeDraftScreen.CenterNotification.RedTeamDoubleSelectStart);
				goto IL_175;
			}
		}
		flag3 = true;
		IL_175:
		bool flag4 = flag3;
		this.SetCenterBackground(flag2, flag4);
		if (notification != UIRankedModeDraftScreen.CenterNotification.BlueTeamNotification)
		{
			if (notification != UIRankedModeDraftScreen.CenterNotification.RedTeamNotification)
			{
				return;
			}
		}
		RankedResolutionPhaseData? rankedData = this.m_lastDraftNotification.RankedData;
		this.UpdateCenterVisuals(rankedData.Value, flag2, flag4);
	}

	private bool IsDoubleSelectinReadyToAdvance()
	{
		if (this.m_currentState != UIRankedModeDraftScreen.CenterNotification.BlueTeamDoubleSelectStart)
		{
			if (this.m_currentState != UIRankedModeDraftScreen.CenterNotification.RedTeamDoubleSelectStart)
			{
				return false;
			}
		}
		if (this.m_stateQueues.Count > 0)
		{
			if (this.m_stateQueues[0] != UIRankedModeDraftScreen.CenterNotification.RedTeamDoubleSelectEnd)
			{
				if (this.m_stateQueues[0] != UIRankedModeDraftScreen.CenterNotification.BlueTeamDoubleSelectEnd)
				{
					return false;
				}
			}
			if (!this.m_doubleRightSelectionCharacterSelected.gameObject.activeInHierarchy)
			{
				if (!this.m_doubleLeftSelectionCharacterSelected.gameObject.activeInHierarchy)
				{
					return true;
				}
			}
		}
		return false;
	}

	private bool IsAnyCenterStateActive()
	{
		for (int i = 0; i < this.m_centerStateObjects.Count; i++)
		{
			if (this.m_centerStateObjects[i].gameObject.activeSelf)
			{
				return true;
			}
		}
		return false;
	}

	private void ClearAllStates()
	{
		for (int i = 0; i < this.m_centerStateObjects.Count; i++)
		{
			UIManager.SetGameObjectActive(this.m_centerStateObjects[i], false, null);
		}
	}

	private void QueueCenterState(UIRankedModeDraftScreen.CenterNotification notification)
	{
		if (notification == UIRankedModeDraftScreen.CenterNotification.None)
		{
			return;
		}
		if (notification != UIRankedModeDraftScreen.CenterNotification.LoadoutPhase)
		{
			if (notification != UIRankedModeDraftScreen.CenterNotification.GameLoadPhase)
			{
				goto IL_3C;
			}
		}
		this.ClearAllStates();
		this.m_stateQueues.Clear();
		IL_3C:
		if (this.m_stateQueues.Count > 0)
		{
			if (this.m_stateQueues[this.m_stateQueues.Count - 1] == notification)
			{
				return;
			}
		}
		else if (this.m_currentState == notification)
		{
			return;
		}
		this.m_stateQueues.Add(notification);
	}

	public void SettingsButtonClicked(BaseEventData data)
	{
		UICharacterSelectCharacterSettingsPanel.TabPanel tab = UICharacterSelectCharacterSettingsPanel.TabPanel.None;
		if ((data as PointerEventData).pointerCurrentRaycast.gameObject == this.m_skinsBtn.spriteController.gameObject)
		{
			tab = UICharacterSelectCharacterSettingsPanel.TabPanel.Skins;
		}
		else if ((data as PointerEventData).pointerCurrentRaycast.gameObject == this.m_abilitiesBtn.spriteController.gameObject)
		{
			tab = UICharacterSelectCharacterSettingsPanel.TabPanel.Abilities;
		}
		else if ((data as PointerEventData).pointerCurrentRaycast.gameObject == this.m_catalystsBtn.spriteController.gameObject)
		{
			tab = UICharacterSelectCharacterSettingsPanel.TabPanel.Catalysts;
		}
		else if ((data as PointerEventData).pointerCurrentRaycast.gameObject == this.m_tauntsBtn.spriteController.gameObject)
		{
			tab = UICharacterSelectCharacterSettingsPanel.TabPanel.Taunts;
		}
		UIRankedCharacterSelectSettingsPanel.Get().SetVisible(true, tab);
	}

	private void OnDestroy()
	{
		UIRankedModeDraftScreen.s_instance = null;
		if (ClientGameManager.Get() != null)
		{
			ClientGameManager.Get().OnGameInfoNotification -= this.HandleGameInfoNotification;
			ClientGameManager.Get().OnLobbyGameplayOverridesChange -= this.OnLobbyGameplayOverridesUpdated;
		}
	}

	private void SetFreelancerSettingButtonsVisible(bool visible)
	{
		UIManager.SetGameObjectActive(this.m_skinsBtn, visible, null);
		UIManager.SetGameObjectActive(this.m_abilitiesBtn, visible, null);
		UIManager.SetGameObjectActive(this.m_catalystsBtn, visible, null);
		UIManager.SetGameObjectActive(this.m_tauntsBtn, visible, null);
	}

	public void OnLobbyGameplayOverridesUpdated(LobbyGameplayOverrides gameplayOverrides)
	{
		this.CheckCharacterListValidity();
	}

	public void HandleGameInfoNotification(GameInfoNotification notification)
	{
		if (notification.GameInfo == null)
		{
			Log.Error("Why is GameInfo null?", new object[0]);
			return;
		}
		if (notification.PlayerInfo == null)
		{
			return;
		}
		if (notification.TeamInfo == null)
		{
			Log.Warning("Why GameInfoNotification.TeamInfo null?", new object[0]);
			return;
		}
		if (notification.TeamInfo.TeamPlayerInfo == null)
		{
			Log.Warning("Why GameInfoNotification.TeamInfo.TeamPlayerInfo null?", new object[0]);
			return;
		}
		if (notification.GameInfo.GameStatus == GameStatus.Stopped)
		{
			AppState_GroupCharacterSelect.Get().Enter();
			return;
		}
		if (notification.PlayerInfo.AccountId != 0L)
		{
			if (notification.TeamInfo.TeamPlayerInfo.Count != 0)
			{
				if (notification.GameInfo.GameStatus == GameStatus.LoadoutSelecting && this.m_lastGameStatus != GameStatus.LoadoutSelecting)
				{
					this.m_loadoutSelectStartTime = Time.realtimeSinceStartup;
					this.SetFreelancerSettingButtonsVisible(true);
				}
				bool flag = false;
				if (this.LastGameInfo == null)
				{
					flag = true;
					this.m_phaseStartTime = Time.time;
				}
				this.LastGameInfo = notification.GameInfo;
				this.LastTeamInfo = notification.TeamInfo;
				this.LastPlayerInfo = notification.PlayerInfo;
				this.m_lastGameStatus = this.LastGameInfo.GameStatus;
				if (this.IsVisible)
				{
					bool flag2 = false;
					if (notification.GameInfo.GameStatus == GameStatus.FreelancerSelecting)
					{
						this.GameIsLaunching = false;
					}
					else
					{
						if (notification.GameInfo.GameStatus != GameStatus.Stopped)
						{
							if (notification.GameInfo.GameStatus == GameStatus.LoadoutSelecting)
							{
								flag2 = true;
								this.UpdateGameLaunching(notification);
								goto IL_22F;
							}
						}
						if (notification.GameInfo.GameStatus != GameStatus.Stopped)
						{
							if (notification.GameInfo.GameStatus > GameStatus.LoadoutSelecting)
							{
								if (!this.m_stateQueues.Contains(UIRankedModeDraftScreen.CenterNotification.GameLoadPhase))
								{
									if (this.m_currentState != UIRankedModeDraftScreen.CenterNotification.GameLoadPhase)
									{
										this.QueueCenterState(UIRankedModeDraftScreen.CenterNotification.GameLoadPhase);
									}
								}
								this.UpdateGameLaunching(notification);
							}
						}
					}
					IL_22F:
					if (flag2)
					{
						if (!this.m_stateQueues.Contains(UIRankedModeDraftScreen.CenterNotification.LoadoutPhase) && this.m_currentState != UIRankedModeDraftScreen.CenterNotification.LoadoutPhase)
						{
							this.QueueCenterState(UIRankedModeDraftScreen.CenterNotification.LoadoutPhase);
						}
					}
					if (this.m_lastDraftNotification != null && this.m_lastDraftNotification.SubPhase == FreelancerResolutionPhaseSubType.FREELANCER_TRADE)
					{
						if (this.LastGameInfo.GameStatus == GameStatus.FreelancerSelecting && !this.m_stateQueues.Contains(UIRankedModeDraftScreen.CenterNotification.TradePhase))
						{
							if (this.m_currentState != UIRankedModeDraftScreen.CenterNotification.TradePhase)
							{
								this.QueueCenterState(UIRankedModeDraftScreen.CenterNotification.TradePhase);
							}
						}
					}
					MapData mapData = GameWideData.Get().GetMapData(notification.GameInfo.GameConfig.Map);
					string mapDisplayName = GameWideData.Get().GetMapDisplayName(notification.GameInfo.GameConfig.Map);
					Sprite sprite;
					if (mapData != null)
					{
						sprite = (Resources.Load(mapData.ResourceImageSpriteLocation, typeof(Sprite)) as Sprite);
					}
					else
					{
						sprite = (Resources.Load("Stages/information_stage_image", typeof(Sprite)) as Sprite);
					}
					this.m_stageImage.sprite = sprite;
					this.m_introStageImage.sprite = sprite;
					this.m_stageText.text = mapDisplayName;
					this.m_introStageText.text = mapDisplayName;
					if (notification.GameInfo.GameConfig.GameType == GameType.Ranked)
					{
						this.m_matchFoundText.text = StringUtil.TR("RankedMatchFound", "OverlayScreensScene");
					}
					else
					{
						this.m_matchFoundText.text = string.Format(StringUtil.TR("SubtypeFound", "Global"), StringUtil.TR(notification.GameInfo.GameConfig.InstanceSubType.LocalizedName));
					}
					this.SetupPlayerLists();
					this.UpdateNotification(this.m_lastDraftNotification, true && !flag);
				}
				return;
			}
		}
	}

	private void SetupPlayerLists()
	{
		if (this.LastGameInfo != null)
		{
			if (this.m_lastDraftNotification != null)
			{
				if (this.m_lastDraftNotification.RankedData != null)
				{
					RankedResolutionPhaseData value = this.m_lastDraftNotification.RankedData.Value;
					Team team = this.LastPlayerInfo.TeamId;
					if (this.LastPlayerInfo.TeamId == Team.Spectator)
					{
						team = Team.TeamA;
					}
					int num = 0;
					int num2 = 0;
					for (int i = 0; i < value.PlayerIdByImporance.Count; i++)
					{
						int num3 = value.PlayerIdByImporance[i];
						IEnumerable<LobbyPlayerInfo> enumerable = this.LastTeamInfo.TeamInfo(team);
						foreach (LobbyPlayerInfo lobbyPlayerInfo in enumerable)
						{
							if (lobbyPlayerInfo.PlayerId == num3)
							{
								if (num < this.m_blueTeamMembers.Length)
								{
									this.m_blueTeamMembers[num].Setup(lobbyPlayerInfo, false);
									num++;
								}
								break;
							}
						}
						IEnumerable<LobbyPlayerInfo> enumerable2 = this.LastTeamInfo.TeamInfo(team.OtherTeam());
						IEnumerator<LobbyPlayerInfo> enumerator2 = enumerable2.GetEnumerator();
						try
						{
							while (enumerator2.MoveNext())
							{
								LobbyPlayerInfo lobbyPlayerInfo2 = enumerator2.Current;
								if (lobbyPlayerInfo2.PlayerId == num3)
								{
									if (num2 < this.m_redTeamMembers.Length)
									{
										this.m_redTeamMembers[num2].Setup(lobbyPlayerInfo2, true);
										num2++;
									}
									goto IL_1B4;
								}
							}
						}
						finally
						{
							if (enumerator2 != null)
							{
								enumerator2.Dispose();
							}
						}
						IL_1B4:;
					}
				}
			}
		}
	}

	private void Update()
	{
		if (this.m_lastDraftNotification != null)
		{
			if (!this.GameIsLaunching)
			{
				if (this.m_lastDraftNotification.RankedData != null)
				{
					if (UICharacterSelectWorldObjects.Get().IsVisible())
					{
						UICharacterSelectWorldObjects.Get().SetVisible(false);
					}
					if ((double)(Time.time - this.m_phaseStartTime) < this.m_timeInPhase.TotalSeconds)
					{
						float num = (float)this.m_timeInPhase.TotalSeconds - Time.time + this.m_phaseStartTime;
						int num2 = Mathf.RoundToInt(num);
						RankedResolutionPhaseData value = this.m_lastDraftNotification.RankedData.Value;
						Team currentTeam = this.GetCurrentTeam(value);
						if (currentTeam != Team.TeamA)
						{
							if (currentTeam != Team.TeamB)
							{
								if (this.m_gameCountdownTimer.text != num2.ToString())
								{
									this.m_gameCountdownTimer.text = num2.ToString();
									this.m_gameCountdownAC.Play("RankedNumberTextCountdown", 1, 0f);
								}
								this.m_redCountdownTimer.text = string.Empty;
								this.m_blueCountdownTimer.text = string.Empty;
								goto IL_28A;
							}
						}
						if (currentTeam == this.LastPlayerInfo.TeamId)
						{
							this.m_gameCountdownTimer.text = string.Empty;
							this.m_redCountdownTimer.text = string.Empty;
							if (this.m_blueCountdownTimer.text != num2.ToString())
							{
								this.m_blueCountdownTimer.text = num2.ToString();
								this.m_blueCountdownAC.Play("RankedNumberTextCountdown", 1, 0f);
							}
						}
						else
						{
							this.m_gameCountdownTimer.text = string.Empty;
							if (this.m_redCountdownTimer.text != num2.ToString())
							{
								this.m_redCountdownTimer.text = num2.ToString();
								this.m_redCountdownAC.Play("RankedNumberTextCountdown", 1, 0f);
							}
							this.m_blueCountdownTimer.text = string.Empty;
						}
						IL_28A:
						if (num2 <= 5)
						{
							if (Mathf.RoundToInt(num + Time.deltaTime) != num2)
							{
								UIFrontEnd.PlaySound(FrontEndButtonSounds.RankModeTimerTick);
							}
						}
					}
					if (this.SelectedCharacter != CharacterType.None)
					{
						if (this.SelectedCharacter != this.ClientClickedCharacter)
						{
							int playerId = GameManager.Get().PlayerInfo.PlayerId;
							if (!this.m_lastDraftNotification.RankedData.Value.symbol_001D(playerId))
							{
								this.SetupCharacterSettings(this.SelectedCharacter);
							}
						}
					}
				}
			}
		}
		if (this.GameIsLaunching)
		{
			this.m_gameCountdownTimer.text = string.Empty;
			if (this.LastGameInfo != null)
			{
				if (this.LastGameInfo.GameStatus == GameStatus.LoadoutSelecting)
				{
					float num3 = Mathf.Max(0f, (float)this.LastGameInfo.LoadoutSelectTimeout.TotalSeconds - (Time.realtimeSinceStartup - this.m_loadoutSelectStartTime));
					int num4 = Mathf.RoundToInt(num3);
					if (this.m_gameCountdownTimer.text != num4.ToString())
					{
						this.m_gameCountdownTimer.text = num4.ToString();
						this.m_gameCountdownAC.Play("RankedNumberTextCountdown", 1, 0f);
					}
					if (num4 < 6 && Mathf.RoundToInt(num3 + Time.deltaTime) != num4)
					{
						UIFrontEnd.PlaySound(FrontEndButtonSounds.RankModeTimerTick);
					}
				}
			}
			this.m_redCountdownTimer.text = string.Empty;
			this.m_blueCountdownTimer.text = string.Empty;
		}
		float axis = Input.GetAxis("Mouse ScrollWheel");
		if (axis > 0f)
		{
			this.SetPageIndex(this.m_currentCharacterPage + 1);
		}
		else if (axis < 0f)
		{
			this.SetPageIndex(this.m_currentCharacterPage - 1);
		}
		if (!this.m_introContainer.gameObject.activeSelf)
		{
			if (this.IsAnyCenterStateActive())
			{
				if (!this.IsDoubleSelectinReadyToAdvance())
				{
					goto IL_524;
				}
			}
			if (this.m_stateQueues.Count > 0)
			{
				this.DoQueueState(this.m_stateQueues[0]);
				this.m_currentState = this.m_stateQueues[0];
				this.m_stateQueues.RemoveAt(0);
			}
		}
		IL_524:
		if (this.m_characterSelectContainerCanvasGroup == null)
		{
			this.m_characterSelectContainerCanvasGroup = this.m_characterSelectContainer.GetComponent<CanvasGroup>();
		}
		if (this.m_journeyLength <= 0f)
		{
			if (this.m_currentCharacterPage == this.m_currentVisiblePage)
			{
				goto IL_7F3;
			}
		}
		float num5 = (Time.time - this.m_startTime) * this.m_timeForPageToSwap;
		float num6 = num5 / this.m_journeyLength;
		Vector2 anchoredPosition = Vector2.Lerp(this.m_startLocation, this.m_endLocation, num6);
		if (!float.IsNaN(anchoredPosition.x))
		{
			if (!float.IsNaN(anchoredPosition.y))
			{
				(this.m_characterSelectContainer.transform as RectTransform).anchoredPosition = anchoredPosition;
				if (this.m_characterSelectContainerCanvasGroup != null)
				{
					if (this.m_currentCharacterPage != this.m_currentVisiblePage)
					{
						this.m_characterSelectContainerCanvasGroup.alpha = 1f - num6;
					}
					else
					{
						this.m_characterSelectContainerCanvasGroup.alpha = num6;
					}
				}
				if (num6 >= 1f)
				{
					if (this.m_characterSelectContainerCanvasGroup.alpha <= 0f)
					{
						Vector2 anchoredPosition2 = (this.m_characterSelectContainer.gameObject.transform as RectTransform).anchoredPosition;
						if (this.m_endLocation.x < 0f)
						{
							(this.m_characterSelectContainer.gameObject.transform as RectTransform).anchoredPosition = new Vector2(anchoredPosition2.x * -1f, anchoredPosition2.y);
							this.m_startTime = Time.time;
							this.m_startLocation = anchoredPosition2;
							this.m_endLocation = new Vector2(0f, anchoredPosition2.y);
							this.m_journeyLength = Vector2.Distance(this.m_startLocation, this.m_endLocation);
						}
						else if (this.m_endLocation.x > 0f)
						{
							(this.m_characterSelectContainer.gameObject.transform as RectTransform).anchoredPosition = new Vector2(anchoredPosition2.x * -1f, anchoredPosition2.y);
							this.m_startTime = Time.time;
							this.m_startLocation = anchoredPosition2;
							this.m_endLocation = new Vector2(0f, anchoredPosition2.y);
							this.m_journeyLength = Vector2.Distance(this.m_startLocation, this.m_endLocation);
						}
						this.UpdateCharacterButtons();
					}
					else
					{
						this.m_journeyLength = 0f;
					}
				}
			}
		}
		IL_7F3:
		if (this.IsCenterSelectAnimating())
		{
			UIManager.SetGameObjectActive(this.m_lockInBtn, false, null);
		}
		else
		{
			string text;
			if (this.m_lastDraftNotification != null && this.m_lastDraftNotification.SubPhase.IsPickBanSubPhase())
			{
				text = StringUtil.TR("Ban", "OverlayScreensScene");
			}
			else
			{
				text = StringUtil.TR("LockIn", "OverlayScreensScene");
			}
			for (int i = 0; i < this.m_lockInText.Length; i++)
			{
				this.m_lockInText[i].text = text;
			}
			UIManager.SetGameObjectActive(this.m_lockInBtn, this.m_intendedLockInBtnStatus, null);
			this.m_lockInBtn.SetDisabled(this.m_selectedSubPhaseCharacter == this.SelectedCharacter);
		}
		if (this.m_containerAC == null)
		{
			this.m_containerAC = this.m_draftScreenContainer.GetComponent<Animator>();
		}
		if (this.m_containerAC != null)
		{
			if (this.m_containerAC.gameObject.activeInHierarchy)
			{
				if (this.m_containerAC.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
				{
					this.DoCharacterSelectContainerActiveCheck();
				}
			}
		}
		UIManager.SetGameObjectActive(this.m_searchFiltersContainer, this.m_characterSelectContainer.gameObject.activeSelf, null);
	}

	private bool IsCenterSelectAnimating()
	{
		bool result = false;
		if (this.m_singleSelectionCharacterSelected.gameObject.activeInHierarchy)
		{
			result = true;
		}
		else if (this.m_doubleRightSelectionCharacterSelected.gameObject.activeInHierarchy && this.doubleSelectionLeftCharacter.gameObject.activeInHierarchy)
		{
			result = true;
		}
		else if (this.m_doubleLeftSelectionCharacterSelected.gameObject.activeInHierarchy)
		{
			if (this.doubleSelectionRightCharacter.gameObject.activeInHierarchy)
			{
				result = true;
			}
		}
		return result;
	}

	public void LockFreelancerBtnClicked(BaseEventData data)
	{
		if (this.m_lastDraftNotification.SubPhase == FreelancerResolutionPhaseSubType.FREELANCER_TRADE)
		{
			UIManager.SetGameObjectActive(this.m_lockFreelancerContainer, false, null);
			ClientGameManager.Get().SendRankedTradeRequest_StopTrading();
		}
	}

	public void LockPhaseButtonClicked(BaseEventData data)
	{
		if (this.m_selectedSubPhaseCharacter != CharacterType.None)
		{
			if (this.SelectedCharacter != this.m_selectedSubPhaseCharacter)
			{
				if (this.m_lastDraftNotification.SubPhase.IsPickBanSubPhase())
				{
					ClientGameManager.Get().SendRankedBanRequest(this.m_selectedSubPhaseCharacter);
				}
				else
				{
					ClientGameManager.Get().SendRankedSelectRequest(this.m_selectedSubPhaseCharacter);
				}
			}
		}
	}

	private bool DidPlayerLockInDuringSwapPhase(RankedResolutionPhaseData data, long playerID)
	{
		bool result = false;
		if (data.TradeActions != null)
		{
			using (List<RankedTradeData>.Enumerator enumerator = data.TradeActions.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					RankedTradeData rankedTradeData = enumerator.Current;
					if ((long)rankedTradeData.OfferingPlayerId == playerID)
					{
						if (rankedTradeData.TradeAction == RankedTradeData.TradeActionType.symbol_0012)
						{
							return true;
						}
					}
				}
			}
		}
		return result;
	}

	private void UpdateHoverSelfStatus(RankedResolutionPhaseData data)
	{
		if (this.m_lastDraftNotification.SubPhase.IsPickBanSubPhase())
		{
			return;
		}
		if (this.LastGameInfo != null)
		{
			int playerId = this.LastPlayerInfo.PlayerId;
			int i = 0;
			while (i < this.m_blueTeamMembers.Length)
			{
				if (this.m_blueTeamMembers[i].PlayerID == playerId)
				{
					if (this.HoveredCharacter != CharacterType.None)
					{
						if (!this.m_selectedCharacterTypes.Contains(this.HoveredCharacter))
						{
							if (!this.IsBanned(this.HoveredCharacter))
							{
								CharacterResourceLink characterResourceLink = GameWideData.Get().GetCharacterResourceLink(this.HoveredCharacter);
								this.m_blueTeamMembers[i].SetBrowseCharacterImageVisible(true);
								this.m_blueTeamMembers[i].SetHoverCharacter(characterResourceLink);
							}
						}
					}
					IL_FF:
					if (this.m_playerIDsOnDeck.Count == 1)
					{
						KeyValuePair<int, CharacterType> selectedChar = new KeyValuePair<int, CharacterType>(playerId, this.HoveredCharacter);
						if (this.m_playerIDsOnDeck.ContainsKey(playerId))
						{
							this.SetupSelection(selectedChar, data, this.m_singleCharacterName, this.singleNoSelectionCharacter, this.singleBrowseSelectionCharacter, this.singleSelectionCharacter, this.m_singleSelectionCharacterSelected, this.m_singleBlueSelectionCharacterSelected, this.m_singleBlueTeamSelectedCharacter, this.m_singleBlueTeamPlayerName, true, false);
						}
						return;
					}
					if (this.m_playerIDsOnDeck.Count == 2)
					{
						bool flag = true;
						using (Dictionary<int, CharacterType>.Enumerator enumerator = this.m_playerIDsOnDeck.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								KeyValuePair<int, CharacterType> selectedChar2 = enumerator.Current;
								if (flag)
								{
									this.SetupSelection(selectedChar2, data, this.m_leftCharacterName, this.doubleNoSelectionLeftCharacter, this.doubleBrowseSelectionLeftCharacter, this.doubleSelectionLeftCharacter, this.m_doubleLeftSelectionCharacterSelected, this.m_doubleLeftBlueSelectionCharacterSelected, this.m_doubleLeftBlueTeamSelectedCharacter, this.m_doubleLeftBlueTeamPlayerName, true, false);
									flag = false;
								}
								else
								{
									this.SetupSelection(selectedChar2, data, this.m_rightCharacterName, this.doubleNoSelectionRightCharacter, this.doubleBrowseSelectionRightCharacter, this.doubleSelectionRightCharacter, this.m_doubleRightSelectionCharacterSelected, this.m_doubleRightBlueSelectionCharacterSelected, this.m_doubleRightBlueTeamSelectedCharacter, this.m_doubleRightBlueTeamPlayerName, true, false);
								}
							}
						}
						return;
					}
					return;
				}
				else
				{
					i++;
				}
			}
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				goto IL_FF;
			}
		}
	}

	private void UpdateHoverStatus(RankedResolutionPhaseData data)
	{
		bool flag = this.m_lastDraftNotification.SubPhase.IsPickBanSubPhase();
		int i = 0;
		while (i < this.m_blueTeamMembers.Length)
		{
			if (!flag)
			{
				goto IL_77;
			}
			if (!this.m_playerIDsOnDeck.ContainsKey(this.OurPlayerId))
			{
				goto IL_77;
			}
			if (this.m_blueTeamMembers[i].PlayerID != this.OurPlayerId)
			{
				goto IL_77;
			}
			IL_330:
			i++;
			continue;
			IL_77:
			if (data.FriendlyTeamSelections.ContainsKey(this.m_blueTeamMembers[i].PlayerID))
			{
				this.m_blueTeamMembers[i].SetBrowseCharacterImageVisible(false);
				goto IL_330;
			}
			if (this.m_playerIDsOnDeck.ContainsKey(this.m_blueTeamMembers[i].PlayerID))
			{
				if (this.m_playerIDsOnDeck[this.m_blueTeamMembers[i].PlayerID] != CharacterType.None)
				{
					if (this.IsBanned(this.m_playerIDsOnDeck[this.m_blueTeamMembers[i].PlayerID]) || this.m_selectedCharacterTypes.Contains(this.m_playerIDsOnDeck[this.m_blueTeamMembers[i].PlayerID]))
					{
						this.m_blueTeamMembers[i].SetBrowseCharacterImageVisible(false);
						UIManager.SetGameObjectActive(this.m_blueTeamMembers[i].m_noCharacterImage, true, null);
					}
					else if (this.OurPlayerId != this.m_blueTeamMembers[i].PlayerID)
					{
						CharacterResourceLink characterResourceLink = GameWideData.Get().GetCharacterResourceLink(this.m_playerIDsOnDeck[this.m_blueTeamMembers[i].PlayerID]);
						this.m_blueTeamMembers[i].SetBrowseCharacterImageVisible(true);
						this.m_blueTeamMembers[i].SetHoverCharacter(characterResourceLink);
					}
					else if (this.HoveredCharacter != CharacterType.None)
					{
						CharacterResourceLink characterResourceLink2 = GameWideData.Get().GetCharacterResourceLink(this.HoveredCharacter);
						this.m_blueTeamMembers[i].SetBrowseCharacterImageVisible(true);
						this.m_blueTeamMembers[i].SetHoverCharacter(characterResourceLink2);
					}
				}
				goto IL_330;
			}
			bool browseCharacterImageVisible = false;
			using (List<RankedResolutionPlayerState>.Enumerator enumerator = data.UnselectedPlayerStates.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					RankedResolutionPlayerState rankedResolutionPlayerState = enumerator.Current;
					if (rankedResolutionPlayerState.PlayerId == this.m_blueTeamMembers[i].PlayerID)
					{
						if (rankedResolutionPlayerState.Intention != CharacterType.None)
						{
							if (!this.IsBanned(rankedResolutionPlayerState.Intention))
							{
								if (!this.m_selectedCharacterTypes.Contains(rankedResolutionPlayerState.Intention))
								{
									CharacterResourceLink characterResourceLink3 = GameWideData.Get().GetCharacterResourceLink(rankedResolutionPlayerState.Intention);
									if (characterResourceLink3 != null)
									{
										browseCharacterImageVisible = true;
										this.m_blueTeamMembers[i].SetHoverCharacter(characterResourceLink3);
										goto IL_2F7;
									}
									goto IL_2F7;
								}
							}
							this.m_blueTeamMembers[i].SetBrowseCharacterImageVisible(false);
							UIManager.SetGameObjectActive(this.m_blueTeamMembers[i].m_noCharacterImage, true, null);
						}
						IL_2F7:
						goto IL_321;
					}
				}
			}
			IL_321:
			this.m_blueTeamMembers[i].SetBrowseCharacterImageVisible(browseCharacterImageVisible);
			goto IL_330;
		}
		for (int j = 0; j < this.m_redTeamMembers.Length; j++)
		{
			if (data.EnemyTeamSelections.ContainsKey(this.m_redTeamMembers[j].PlayerID))
			{
				this.m_redTeamMembers[j].SetBrowseCharacterImageVisible(false);
			}
			else
			{
				bool flag2 = false;
				if (this.m_playerIDsOnDeck.ContainsKey(this.m_redTeamMembers[j].PlayerID) && this.m_playerIDsOnDeck[this.m_redTeamMembers[j].PlayerID] != CharacterType.None)
				{
					if (!this.IsBanned(this.m_playerIDsOnDeck[this.m_redTeamMembers[j].PlayerID]))
					{
						CharacterResourceLink characterResourceLink4 = GameWideData.Get().GetCharacterResourceLink(this.m_playerIDsOnDeck[this.m_redTeamMembers[j].PlayerID]);
						this.m_redTeamMembers[j].SetBrowseCharacterImageVisible(true);
						this.m_redTeamMembers[j].SetHoverCharacter(characterResourceLink4);
						flag2 = true;
					}
				}
				if (!flag2)
				{
					this.m_redTeamMembers[j].SetBrowseCharacterImageVisible(false);
				}
			}
		}
	}

	private void CheckSelectedCharForCenterPiece(bool isOnBlue, RankedResolutionPhaseData data)
	{
		if (this.m_playerIDsOnDeck.Count == 1)
		{
			UIManager.SetGameObjectActive(this.m_singleSelectionCharacterSelected, true, null);
			using (Dictionary<int, CharacterType>.Enumerator enumerator = this.m_playerIDsOnDeck.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<int, CharacterType> keyValuePair = enumerator.Current;
					KeyValuePair<int, CharacterType> selectedChar = keyValuePair;
					TextMeshProUGUI singleCharacterName = this.m_singleCharacterName;
					Image noCharacter = this.singleNoSelectionCharacter;
					Image browseCharacter = this.singleBrowseSelectionCharacter;
					Image selectedCharacter = this.singleSelectionCharacter;
					Animator singleSelectionCharacterSelected = this.m_singleSelectionCharacterSelected;
					Animator selectedCharacterNameAnimator;
					if (isOnBlue)
					{
						selectedCharacterNameAnimator = this.m_singleBlueSelectionCharacterSelected;
					}
					else
					{
						selectedCharacterNameAnimator = this.m_singleRedTeamSelectionCharacterSelected;
					}
					this.SetupSelection(selectedChar, data, singleCharacterName, noCharacter, browseCharacter, selectedCharacter, singleSelectionCharacterSelected, selectedCharacterNameAnimator, (!isOnBlue) ? this.m_singleRedTeamSelectedCharacter : this.m_singleBlueTeamSelectedCharacter, (!isOnBlue) ? this.m_singleRedTeamPlayerName : this.m_singleBlueTeamPlayerName, isOnBlue, !isOnBlue);
				}
			}
		}
		else
		{
			bool flag = !this.doubleSelectionLeftCharacter.gameObject.activeInHierarchy;
			using (Dictionary<int, CharacterType>.Enumerator enumerator2 = this.m_playerIDsOnDeck.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					KeyValuePair<int, CharacterType> keyValuePair2 = enumerator2.Current;
					if (flag)
					{
						flag = false;
						KeyValuePair<int, CharacterType> selectedChar2 = keyValuePair2;
						TextMeshProUGUI leftCharacterName = this.m_leftCharacterName;
						Image noCharacter2 = this.doubleNoSelectionLeftCharacter;
						Image browseCharacter2 = this.doubleBrowseSelectionLeftCharacter;
						Image selectedCharacter2 = this.doubleSelectionLeftCharacter;
						Animator doubleLeftSelectionCharacterSelected = this.m_doubleLeftSelectionCharacterSelected;
						Animator selectedCharacterNameAnimator2;
						if (isOnBlue)
						{
							selectedCharacterNameAnimator2 = this.m_doubleLeftBlueSelectionCharacterSelected;
						}
						else
						{
							selectedCharacterNameAnimator2 = this.m_doubleLeftRedTeamSelectionCharacterSelected;
						}
						TextMeshProUGUI selectedCharacterText;
						if (isOnBlue)
						{
							selectedCharacterText = this.m_doubleLeftBlueTeamSelectedCharacter;
						}
						else
						{
							selectedCharacterText = this.m_doubleLeftRedTeamSelectedCharacter;
						}
						this.SetupSelection(selectedChar2, data, leftCharacterName, noCharacter2, browseCharacter2, selectedCharacter2, doubleLeftSelectionCharacterSelected, selectedCharacterNameAnimator2, selectedCharacterText, (!isOnBlue) ? this.m_doubleLeftRedTeamPlayerName : this.m_doubleLeftBlueTeamPlayerName, isOnBlue, !isOnBlue);
					}
					else
					{
						KeyValuePair<int, CharacterType> selectedChar3 = keyValuePair2;
						TextMeshProUGUI rightCharacterName = this.m_rightCharacterName;
						Image noCharacter3 = this.doubleNoSelectionRightCharacter;
						Image browseCharacter3 = this.doubleBrowseSelectionRightCharacter;
						Image selectedCharacter3 = this.doubleSelectionRightCharacter;
						Animator doubleRightSelectionCharacterSelected = this.m_doubleRightSelectionCharacterSelected;
						Animator selectedCharacterNameAnimator3 = (!isOnBlue) ? this.m_doubleRightRedTeamSelectionCharacterSelected : this.m_doubleRightBlueSelectionCharacterSelected;
						TextMeshProUGUI selectedCharacterText2 = (!isOnBlue) ? this.m_doubleRightRedTeamSelectedCharacter : this.m_doubleRightBlueTeamSelectedCharacter;
						TextMeshProUGUI playerName;
						if (isOnBlue)
						{
							playerName = this.m_doubleRightBlueTeamPlayerName;
						}
						else
						{
							playerName = this.m_doubleRightRedTeamPlayerName;
						}
						this.SetupSelection(selectedChar3, data, rightCharacterName, noCharacter3, browseCharacter3, selectedCharacter3, doubleRightSelectionCharacterSelected, selectedCharacterNameAnimator3, selectedCharacterText2, playerName, isOnBlue, !isOnBlue);
					}
				}
			}
		}
	}

	private void UpdatePlayerSelecting(RankedResolutionPhaseData data)
	{
		for (int i = 0; i < this.m_blueTeamMembers.Length; i++)
		{
			this.m_blueTeamMembers[i].SetAsSelecting(data.symbol_001D(this.m_blueTeamMembers[i].PlayerID));
		}
		for (int j = 0; j < this.m_redTeamMembers.Length; j++)
		{
			this.m_redTeamMembers[j].SetAsSelecting(data.symbol_001D(this.m_redTeamMembers[j].PlayerID));
		}
	}

	private void DoCharacterSelectContainerActiveCheck()
	{
		bool flag;
		if (this.m_lastDraftNotification != null)
		{
			flag = this.m_lastDraftNotification.SubPhase.IsPickBanSubPhase();
		}
		else
		{
			flag = false;
		}
		bool flag2 = flag;
		UIManager.SetGameObjectActive(this.m_characterSelectContainer, true, null);
		foreach (UICharacterPanelSelectRankModeButton uicharacterPanelSelectButton in this.m_characterSelectContainer.GetComponentsInChildren<UICharacterPanelSelectRankModeButton>(true))
		{
			bool clickable;
			if (!flag2)
			{
				clickable = (this.SelectedCharacter == CharacterType.None);
			}
			else
			{
				clickable = true;
			}
			uicharacterPanelSelectButton.SetClickable(clickable);
		}
	}

	private void UpdateRankData(RankedResolutionPhaseData data, bool updateFromGameInfoUpdate = false)
	{
		if (this.LastGameInfo != null)
		{
			this.m_timeInPhase = data.TimeLeftInSubPhase;
			if (!updateFromGameInfoUpdate)
			{
				this.m_phaseStartTime = Time.time;
			}
			this.m_IsOnDeck = data.symbol_001D(this.OurPlayerId);
			this.DoCharacterSelectContainerActiveCheck();
			bool intendedLockInBtnStatus;
			if (this.m_IsOnDeck)
			{
				if (!this.IsBanned(this.HoveredCharacter) && !this.m_selectedCharacterTypes.Contains(this.HoveredCharacter))
				{
					if (!this.m_lastDraftNotification.SubPhase.IsPickBanSubPhase())
					{
						intendedLockInBtnStatus = this.m_lastDraftNotification.SubPhase.IsPickFreelancerSubPhase();
					}
					else
					{
						intendedLockInBtnStatus = true;
					}
					goto IL_DA;
				}
			}
			intendedLockInBtnStatus = false;
			IL_DA:
			this.m_intendedLockInBtnStatus = intendedLockInBtnStatus;
			if (this.m_IsOnDeck)
			{
				if (!updateFromGameInfoUpdate)
				{
					this.HoveredCharacter = data.PlayersOnDeck.Find((RankedResolutionPlayerState p) => p.PlayerId == this.OurPlayerId).Intention;
				}
			}
			else
			{
				this.HoveredCharacter = CharacterType.None;
			}
			this.SetFreelancerSettingButtonsVisible(this.m_currentState >= UIRankedModeDraftScreen.CenterNotification.LoadoutPhase);
			UIManager.SetGameObjectActive(this.m_lockFreelancerContainer, false, null);
			if (this.m_lastDraftNotification.SubPhase == FreelancerResolutionPhaseSubType.FREELANCER_TRADE)
			{
				if (this.LastGameInfo.GameStatus == GameStatus.FreelancerSelecting)
				{
					if (!this.m_stateQueues.Contains(UIRankedModeDraftScreen.CenterNotification.TradePhase))
					{
						if (this.m_currentState != UIRankedModeDraftScreen.CenterNotification.TradePhase)
						{
							this.QueueCenterState(UIRankedModeDraftScreen.CenterNotification.TradePhase);
						}
					}
				}
			}
			if (!this.m_lastDraftNotification.SubPhase.IsPickBanSubPhase())
			{
				for (int i = 0; i < this.m_blueBans.Length; i++)
				{
					this.m_blueBans[i].SetAsSelecting(false);
					this.m_redBans[i].SetAsSelecting(false);
				}
			}
			for (int j = 0; j < this.m_blueBans.Length; j++)
			{
				if (j >= data.FriendlyBans.Count)
				{
					break;
				}
				CharacterType characterType = data.FriendlyBans[j];
				if (this.m_blueBans[j] != null)
				{
					CharacterResourceLink characterResourceLink = GameWideData.Get().GetCharacterResourceLink(characterType);
					if (characterResourceLink != null)
					{
						this.m_blueBans[j].SetSelectedCharacterImageVisible(true);
						if (this.m_blueBans[j].GetSelectedCharacter() == CharacterType.None)
						{
							UIFrontEnd.PlaySound(FrontEndButtonSounds.RankModeBanPlayer);
							this.CheckSelectedCharForCenterPiece(true, data);
						}
						this.m_blueBans[j].SetCharacter(characterResourceLink);
					}
				}
				if (!this.m_friendlyBannedCharacterTypes.Contains(characterType))
				{
					this.m_friendlyBannedCharacterTypes.Add(characterType);
				}
			}
			for (int k = 0; k < this.m_redBans.Length; k++)
			{
				if (k >= data.EnemyBans.Count)
				{
					break;
				}
				CharacterType characterType2 = data.EnemyBans[k];
				if (this.m_redBans[k] != null)
				{
					CharacterResourceLink characterResourceLink2 = GameWideData.Get().GetCharacterResourceLink(characterType2);
					if (characterResourceLink2 != null)
					{
						this.m_redBans[k].SetSelectedCharacterImageVisible(true);
						if (this.m_redBans[k].GetSelectedCharacter() == CharacterType.None)
						{
							UIFrontEnd.PlaySound(FrontEndButtonSounds.RankModeBanPlayer);
							this.CheckSelectedCharForCenterPiece(false, data);
						}
						this.m_redBans[k].SetCharacter(characterResourceLink2);
					}
				}
				if (!this.m_enemyBannedCharacterTypes.Contains(characterType2))
				{
					this.m_enemyBannedCharacterTypes.Add(characterType2);
				}
			}
			long accountId = ClientGameManager.Get().GetPlayerAccountData().AccountId;
			for (int l = 0; l < this.m_blueTeamMembers.Length; l++)
			{
				this.m_blueTeamMembers[l].CanBeTraded = false;
				bool selectedCharacterImageVisible = false;
				if (data.FriendlyTeamSelections.ContainsKey(this.m_blueTeamMembers[l].PlayerID))
				{
					CharacterResourceLink characterResourceLink3 = GameWideData.Get().GetCharacterResourceLink(data.FriendlyTeamSelections[this.m_blueTeamMembers[l].PlayerID]);
					if (characterResourceLink3 != null)
					{
						selectedCharacterImageVisible = true;
						if (this.m_blueTeamMembers[l].GetSelectedCharacter() == CharacterType.None)
						{
							UIFrontEnd.PlaySound(FrontEndButtonSounds.RankModePickPlayer);
							this.CheckSelectedCharForCenterPiece(true, data);
						}
						this.m_blueTeamMembers[l].SetCharacter(characterResourceLink3);
						if (!this.IsBanned(characterResourceLink3.m_characterType))
						{
							this.m_friendlyBannedCharacterTypes.Add(characterResourceLink3.m_characterType);
						}
						this.m_blueTeamMembers[l].CanBeTraded = !this.DidPlayerLockInDuringSwapPhase(data, (long)this.m_blueTeamMembers[l].PlayerID);
					}
					if (this.m_blueTeamMembers[l].AccountID == accountId)
					{
						this.SelectedCharacter = this.m_blueTeamMembers[l].GetSelectedCharacter();
					}
				}
				this.m_blueTeamMembers[l].SetTradePhase(this.m_lastDraftNotification.SubPhase == FreelancerResolutionPhaseSubType.FREELANCER_TRADE);
				if (this.m_lastDraftNotification.SubPhase != FreelancerResolutionPhaseSubType.FREELANCER_TRADE)
				{
					this.m_blueTeamMembers[l].SetCharacterLocked(false);
				}
				UIManager.SetGameObjectActive(this.m_blueTeamMembers[l], true, null);
				this.m_blueTeamMembers[l].SetSelectedCharacterImageVisible(selectedCharacterImageVisible);
			}
			for (int m = 0; m < this.m_redTeamMembers.Length; m++)
			{
				bool selectedCharacterImageVisible2 = false;
				if (data.EnemyTeamSelections.ContainsKey(this.m_redTeamMembers[m].PlayerID))
				{
					CharacterResourceLink characterResourceLink4 = GameWideData.Get().GetCharacterResourceLink(data.EnemyTeamSelections[this.m_redTeamMembers[m].PlayerID]);
					if (characterResourceLink4 != null)
					{
						selectedCharacterImageVisible2 = true;
						if (this.m_redTeamMembers[m].GetSelectedCharacter() == CharacterType.None)
						{
							UIFrontEnd.PlaySound(FrontEndButtonSounds.RankModePickPlayer);
							this.CheckSelectedCharForCenterPiece(false, data);
						}
						this.m_redTeamMembers[m].SetCharacter(characterResourceLink4);
						if (!this.IsBanned(characterResourceLink4.m_characterType))
						{
							this.m_enemyBannedCharacterTypes.Add(characterResourceLink4.m_characterType);
						}
					}
				}
				UIManager.SetGameObjectActive(this.m_redTeamMembers[m], true, null);
				this.m_redTeamMembers[m].SetSelectedCharacterImageVisible(selectedCharacterImageVisible2);
			}
			if (this.m_currentCharacterPage == -1)
			{
				this.SetPageIndex(0);
			}
			this.CheckCharacterListValidity();
		}
	}

	public Team GetCurrentTeam(RankedResolutionPhaseData data)
	{
		if (this.LastTeamInfo != null && this.LastPlayerInfo != null)
		{
			foreach (LobbyPlayerInfo lobbyPlayerInfo in this.LastTeamInfo.TeamPlayerInfo)
			{
				if (data.symbol_001D(lobbyPlayerInfo.PlayerId))
				{
					return lobbyPlayerInfo.TeamId;
				}
			}
			if (this.m_lastDraftNotification == null)
			{
				return Team.Invalid;
			}
			if (!this.m_lastDraftNotification.SubPhase.IsPickFreelancerSubPhase())
			{
				if (!this.m_lastDraftNotification.SubPhase.IsPickBanSubPhase())
				{
					return Team.Invalid;
				}
			}
			if (this.LastPlayerInfo.TeamId == Team.Spectator)
			{
				return Team.TeamA;
			}
			return this.LastPlayerInfo.TeamId.OtherTeam();
		}
		return Team.Invalid;
	}

	private void SetCenterBackground(bool isOnBlue, bool isOnRed)
	{
		for (int i = 0; i < this.singleSelectionBlueTeam.Length; i++)
		{
			UIManager.SetGameObjectActive(this.singleSelectionBlueTeam[i], isOnBlue, null);
		}
		for (int j = 0; j < this.singleSelectionRedTeam.Length; j++)
		{
			UIManager.SetGameObjectActive(this.singleSelectionRedTeam[j], isOnRed, null);
		}
		for (int k = 0; k < this.doubleSelectionBlueTeam.Length; k++)
		{
			UIManager.SetGameObjectActive(this.doubleSelectionBlueTeam[k], isOnBlue, null);
		}
		for (int l = 0; l < this.doubleSelectionRedTeam.Length; l++)
		{
			UIManager.SetGameObjectActive(this.doubleSelectionRedTeam[l], isOnRed, null);
		}
	}

	private void PrintData(RankedResolutionPhaseData data)
	{
		string text = "LAST RANKED RESOLUTION PHASE DATA!\n";
		text += "Blue team info:\n";
		for (int i = 0; i < this.m_blueTeamMembers.Length; i++)
		{
			using (List<RankedResolutionPlayerState>.Enumerator enumerator = data.UnselectedPlayerStates.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					RankedResolutionPlayerState rankedResolutionPlayerState = enumerator.Current;
					if (rankedResolutionPlayerState.PlayerId == this.m_blueTeamMembers[i].PlayerID)
					{
						text += string.Format("PlayerID is {0}, PlayerName is {1}, Is On deck: {2}\n", rankedResolutionPlayerState.PlayerId, this.m_blueTeamMembers[i].m_playerName, rankedResolutionPlayerState.OnDeckness);
						goto IL_BE;
					}
				}
			}
			IL_BE:;
		}
		text += "Red team info:\n";
		for (int j = 0; j < this.m_redTeamMembers.Length; j++)
		{
			using (List<RankedResolutionPlayerState>.Enumerator enumerator2 = data.UnselectedPlayerStates.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					RankedResolutionPlayerState rankedResolutionPlayerState2 = enumerator2.Current;
					if (rankedResolutionPlayerState2.PlayerId == this.m_redTeamMembers[j].PlayerID)
					{
						text += string.Format("PlayerID is {0}, PlayerName is {1}, Is On deck: {2}\n", rankedResolutionPlayerState2.PlayerId, this.m_redTeamMembers[j].m_playerName.text, rankedResolutionPlayerState2.OnDeckness);
						goto IL_195;
					}
				}
			}
			IL_195:;
		}
		Debug.Log(text);
	}

	private void UpdateCenter(RankedResolutionPhaseData data, bool updateFromGameInfoUpdate)
	{
		if (this.LastGameInfo != null)
		{
			bool flag = false;
			bool flag2 = false;
			int i = 0;
			while (i < this.m_blueTeamMembers.Length)
			{
				if (data.symbol_001D(this.m_blueTeamMembers[i].PlayerID))
				{
					flag = true;
					IL_67:
					for (int j = 0; j < this.m_redTeamMembers.Length; j++)
					{
						if (data.symbol_001D(this.m_redTeamMembers[j].PlayerID))
						{
							flag2 = true;
							IL_A8:
							bool flag3 = true;
							foreach (RankedResolutionPlayerState rankedResolutionPlayerState in data.PlayersOnDeck)
							{
								if (this.m_playerIDsOnDeck.ContainsKey(rankedResolutionPlayerState.PlayerId))
								{
									flag3 = false;
								}
							}
							if (this.m_lastDraftNotification.SubPhase == this.m_lastPhaseForUpdateCenter)
							{
								if (!flag3)
								{
									foreach (RankedResolutionPlayerState rankedResolutionPlayerState2 in data.PlayersOnDeck)
									{
										this.m_playerIDsOnDeck[rankedResolutionPlayerState2.PlayerId] = rankedResolutionPlayerState2.Intention;
									}
									goto IL_39C;
								}
							}
							if (this.m_playerIDsOnDeck.Count > 0)
							{
								bool flag4 = false;
								bool flag5 = false;
								for (int k = 0; k < this.m_blueTeamMembers.Length; k++)
								{
									if (this.m_playerIDsOnDeck.ContainsKey(this.m_blueTeamMembers[k].PlayerID))
									{
										flag4 = true;
										break;
									}
								}
								for (int l = 0; l < this.m_redTeamMembers.Length; l++)
								{
									if (this.m_playerIDsOnDeck.ContainsKey(this.m_redTeamMembers[l].PlayerID))
									{
										flag5 = true;
										break;
									}
								}
								if (flag5)
								{
									if (this.m_playerIDsOnDeck.Count == 1)
									{
										this.QueueCenterState(UIRankedModeDraftScreen.CenterNotification.RedTeamSingleSelectEnd);
									}
									else
									{
										this.QueueCenterState(UIRankedModeDraftScreen.CenterNotification.RedTeamDoubleSelectEnd);
									}
								}
								else if (flag4)
								{
									if (this.m_playerIDsOnDeck.Count == 1)
									{
										this.QueueCenterState(UIRankedModeDraftScreen.CenterNotification.BlueTeamSingleSelectEnd);
									}
									else
									{
										this.QueueCenterState(UIRankedModeDraftScreen.CenterNotification.BlueTeamDoubleSelectEnd);
									}
								}
							}
							if (!this.m_lastDraftNotification.SubPhase.IsPickBanSubPhase())
							{
								if (this.m_lastDraftNotification.SubPhase.IsPickFreelancerSubPhase())
								{
								}
								else
								{
									if (this.m_lastDraftNotification.SubPhase == FreelancerResolutionPhaseSubType.FREELANCER_TRADE)
									{
										this.QueueCenterState(UIRankedModeDraftScreen.CenterNotification.TradePhase);
										goto IL_2E7;
									}
									goto IL_2E7;
								}
							}
							UIRankedModeDraftScreen.CenterNotification centerNotification = UIRankedModeDraftScreen.CenterNotification.None;
							int count = data.PlayersOnDeck.Count;
							if (flag2)
							{
								this.QueueCenterState(UIRankedModeDraftScreen.CenterNotification.RedTeamNotification);
								if (count == 1)
								{
									centerNotification = UIRankedModeDraftScreen.CenterNotification.RedTeamSingleSelectStart;
								}
								else if (count == 2)
								{
									centerNotification = UIRankedModeDraftScreen.CenterNotification.RedTeamDoubleSelectStart;
								}
							}
							else if (flag)
							{
								this.QueueCenterState(UIRankedModeDraftScreen.CenterNotification.BlueTeamNotification);
								if (count == 1)
								{
									centerNotification = UIRankedModeDraftScreen.CenterNotification.BlueTeamSingleSelectStart;
								}
								else if (count == 2)
								{
									centerNotification = UIRankedModeDraftScreen.CenterNotification.BlueTeamDoubleSelectStart;
								}
							}
							if (centerNotification != UIRankedModeDraftScreen.CenterNotification.None)
							{
								this.QueueCenterState(centerNotification);
							}
							IL_2E7:
							this.m_playerIDsOnDeck.Clear();
							using (List<RankedResolutionPlayerState>.Enumerator enumerator3 = data.PlayersOnDeck.GetEnumerator())
							{
								while (enumerator3.MoveNext())
								{
									RankedResolutionPlayerState rankedResolutionPlayerState3 = enumerator3.Current;
									this.m_playerIDsOnDeck.Add(rankedResolutionPlayerState3.PlayerId, rankedResolutionPlayerState3.Intention);
								}
							}
							IL_39C:
							if (!updateFromGameInfoUpdate)
							{
								this.UpdateCenterVisuals(data, flag, flag2);
							}
							this.m_lastPhaseForUpdateCenter = this.m_lastDraftNotification.SubPhase;
							return;
						}
					}
					for (;;)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						goto IL_A8;
					}
				}
				else
				{
					i++;
				}
			}
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				goto IL_67;
			}
		}
	}

	private void UpdateCenterVisuals(RankedResolutionPhaseData data, bool isOnBlueTeam, bool isOnRedTeam)
	{
		if (this.m_playerIDsOnDeck.Count == 1)
		{
			foreach (KeyValuePair<int, CharacterType> keyValuePair in this.m_playerIDsOnDeck)
			{
				KeyValuePair<int, CharacterType> selectedChar = keyValuePair;
				RankedResolutionPhaseData data2 = data;
				TextMeshProUGUI singleCharacterName = this.m_singleCharacterName;
				Image noCharacter = this.singleNoSelectionCharacter;
				Image browseCharacter = this.singleBrowseSelectionCharacter;
				Image selectedCharacter = this.singleSelectionCharacter;
				Animator singleSelectionCharacterSelected = this.m_singleSelectionCharacterSelected;
				Animator selectedCharacterNameAnimator = (!isOnBlueTeam) ? this.m_singleRedTeamSelectionCharacterSelected : this.m_singleBlueSelectionCharacterSelected;
				TextMeshProUGUI selectedCharacterText;
				if (isOnBlueTeam)
				{
					selectedCharacterText = this.m_singleBlueTeamSelectedCharacter;
				}
				else
				{
					selectedCharacterText = this.m_singleRedTeamSelectedCharacter;
				}
				this.SetupSelection(selectedChar, data2, singleCharacterName, noCharacter, browseCharacter, selectedCharacter, singleSelectionCharacterSelected, selectedCharacterNameAnimator, selectedCharacterText, (!isOnBlueTeam) ? this.m_singleRedTeamPlayerName : this.m_singleBlueTeamPlayerName, isOnBlueTeam, isOnRedTeam);
			}
		}
		else if (this.m_playerIDsOnDeck.Count == 2)
		{
			bool flag = true;
			using (Dictionary<int, CharacterType>.Enumerator enumerator2 = this.m_playerIDsOnDeck.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					KeyValuePair<int, CharacterType> keyValuePair2 = enumerator2.Current;
					if (flag)
					{
						KeyValuePair<int, CharacterType> selectedChar2 = keyValuePair2;
						RankedResolutionPhaseData data3 = data;
						TextMeshProUGUI leftCharacterName = this.m_leftCharacterName;
						Image noCharacter2 = this.doubleNoSelectionLeftCharacter;
						Image browseCharacter2 = this.doubleBrowseSelectionLeftCharacter;
						Image selectedCharacter2 = this.doubleSelectionLeftCharacter;
						Animator selectedCharacterAnimator;
						if (this.m_playerIDsOnDeck.Count == data.PlayersOnDeck.Count)
						{
							selectedCharacterAnimator = this.m_doubleLeftSelectionCharacterSelected;
						}
						else
						{
							selectedCharacterAnimator = null;
						}
						Animator selectedCharacterNameAnimator2;
						if (isOnBlueTeam)
						{
							selectedCharacterNameAnimator2 = this.m_doubleLeftBlueSelectionCharacterSelected;
						}
						else
						{
							selectedCharacterNameAnimator2 = this.m_doubleLeftRedTeamSelectionCharacterSelected;
						}
						TextMeshProUGUI selectedCharacterText2;
						if (isOnBlueTeam)
						{
							selectedCharacterText2 = this.m_doubleLeftBlueTeamSelectedCharacter;
						}
						else
						{
							selectedCharacterText2 = this.m_doubleLeftRedTeamSelectedCharacter;
						}
						this.SetupSelection(selectedChar2, data3, leftCharacterName, noCharacter2, browseCharacter2, selectedCharacter2, selectedCharacterAnimator, selectedCharacterNameAnimator2, selectedCharacterText2, (!isOnBlueTeam) ? this.m_doubleLeftRedTeamPlayerName : this.m_doubleLeftBlueTeamPlayerName, isOnBlueTeam, isOnRedTeam);
						flag = false;
					}
					else
					{
						KeyValuePair<int, CharacterType> selectedChar3 = keyValuePair2;
						RankedResolutionPhaseData data4 = data;
						TextMeshProUGUI rightCharacterName = this.m_rightCharacterName;
						Image noCharacter3 = this.doubleNoSelectionRightCharacter;
						Image browseCharacter3 = this.doubleBrowseSelectionRightCharacter;
						Image selectedCharacter3 = this.doubleSelectionRightCharacter;
						Animator selectedCharacterAnimator2;
						if (this.m_playerIDsOnDeck.Count == data.PlayersOnDeck.Count)
						{
							selectedCharacterAnimator2 = this.m_doubleRightSelectionCharacterSelected;
						}
						else
						{
							selectedCharacterAnimator2 = null;
						}
						Animator selectedCharacterNameAnimator3;
						if (isOnBlueTeam)
						{
							selectedCharacterNameAnimator3 = this.m_doubleRightBlueSelectionCharacterSelected;
						}
						else
						{
							selectedCharacterNameAnimator3 = this.m_doubleRightRedTeamSelectionCharacterSelected;
						}
						TextMeshProUGUI selectedCharacterText3;
						if (isOnBlueTeam)
						{
							selectedCharacterText3 = this.m_doubleRightBlueTeamSelectedCharacter;
						}
						else
						{
							selectedCharacterText3 = this.m_doubleRightRedTeamSelectedCharacter;
						}
						this.SetupSelection(selectedChar3, data4, rightCharacterName, noCharacter3, browseCharacter3, selectedCharacter3, selectedCharacterAnimator2, selectedCharacterNameAnimator3, selectedCharacterText3, (!isOnBlueTeam) ? this.m_doubleRightRedTeamPlayerName : this.m_doubleRightBlueTeamPlayerName, isOnBlueTeam, isOnRedTeam);
					}
				}
			}
		}
	}

	public void SetupFreelancerSelect(RankedResolutionPhaseData data)
	{
		if (this.LastGameInfo != null)
		{
			if (this.HoveredCharacter != this.m_selectedSubPhaseCharacter)
			{
				if (this.m_selectedSubPhaseCharacter != CharacterType.None)
				{
					if (data.symbol_001D(this.LastPlayerInfo.PlayerId))
					{
						if (!this.m_selectedCharacterTypes.Contains(this.m_selectedSubPhaseCharacter))
						{
							if (!this.IsBanned(this.m_selectedSubPhaseCharacter))
							{
								ClientGameManager.Get().UpdateSelectedCharacter(this.m_selectedSubPhaseCharacter, 0);
								ClientGameManager.Get().SendRankedHoverClickRequest(this.m_selectedSubPhaseCharacter);
								this.m_intendedLockInBtnStatus = true;
								this.HoveredCharacter = this.m_selectedSubPhaseCharacter;
							}
						}
					}
				}
			}
		}
	}

	private bool CharacterSelectAnimIsPlaying()
	{
		if (!this.m_doubleLeftSelectionCharacterSelected.gameObject.activeSelf)
		{
			if (!this.m_doubleRightSelectionCharacterSelected.gameObject.activeSelf)
			{
				return this.m_singleSelectionCharacterSelected.gameObject.activeSelf;
			}
		}
		return true;
	}

	private void SetupSelection(KeyValuePair<int, CharacterType> selectedChar, RankedResolutionPhaseData data, TextMeshProUGUI nameDisplay, Image NoCharacter, Image BrowseCharacter, Image SelectedCharacter, Animator SelectedCharacterAnimator, Animator SelectedCharacterNameAnimator, TextMeshProUGUI SelectedCharacterText, TextMeshProUGUI PlayerName, bool isFriendly, bool isEnemy)
	{
		bool flag = false;
		bool flag2 = false;
		bool flag3 = true;
		if (!this.m_selectedCharacterTypes.Contains(selectedChar.Value))
		{
			if (!this.IsBanned(selectedChar.Value) && !data.FriendlyBans.Contains(selectedChar.Value))
			{
				if (!data.EnemyBans.Contains(selectedChar.Value))
				{
					goto IL_88;
				}
			}
		}
		flag3 = false;
		IL_88:
		if (isFriendly)
		{
			bool flag4;
			if (this.m_lastSetupSelectionPhaseSubType.IsPickBanSubPhase())
			{
				flag4 = data.FriendlyBans.Contains(selectedChar.Value);
			}
			else if (this.m_lastSetupSelectionPhaseSubType.IsPickFreelancerSubPhase())
			{
				flag4 = data.FriendlyTeamSelections.ContainsKey(selectedChar.Key);
			}
			else
			{
				flag4 = false;
			}
			if (flag4)
			{
				CharacterType value = selectedChar.Value;
				if (value != CharacterType.None)
				{
					flag = true;
					bool flag5 = true;
					if (this.m_lastSetupSelectionPhaseSubType.IsPickFreelancerSubPhase())
					{
						if (this.m_playerIDsThatSelected.Contains(selectedChar.Key))
						{
							flag5 = false;
						}
					}
					if (flag5)
					{
						SelectedCharacterText.text = value.GetDisplayName();
						SelectedCharacter.sprite = GameWideData.Get().GetCharacterResourceLink(value).ActorDataPrefab.GetComponent<ActorData>().GetAliveHUDIcon();
						UIManager.SetGameObjectActive(NoCharacter, false, null);
						UIManager.SetGameObjectActive(BrowseCharacter, false, null);
						UIManager.SetGameObjectActive(SelectedCharacter, true, null);
						if (SelectedCharacterAnimator != null)
						{
							UIManager.SetGameObjectActive(SelectedCharacterAnimator, true, null);
							this.m_playerIDBeingAnimated = selectedChar.Key;
							this.m_animatorCurrentlyAnimating = SelectedCharacterAnimator;
						}
						UIManager.SetGameObjectActive(SelectedCharacterNameAnimator, true, null);
						UIManager.SetGameObjectActive(SelectedCharacterNameAnimator.transform.parent, true, null);
						if (this.m_currentState != UIRankedModeDraftScreen.CenterNotification.BlueTeamDoubleSelectStart)
						{
							if (this.m_currentState != UIRankedModeDraftScreen.CenterNotification.BlueTeamSingleSelectStart)
							{
								if (!this.m_stateQueues.Contains(UIRankedModeDraftScreen.CenterNotification.BlueTeamDoubleSelectStart))
								{
									if (!this.m_stateQueues.Contains(UIRankedModeDraftScreen.CenterNotification.BlueTeamSingleSelectStart))
									{
										goto IL_2CA;
									}
								}
								while (this.m_currentState != UIRankedModeDraftScreen.CenterNotification.BlueTeamDoubleSelectStart)
								{
									if (this.m_currentState == UIRankedModeDraftScreen.CenterNotification.BlueTeamSingleSelectStart)
									{
										for (;;)
										{
											switch (4)
											{
											case 0:
												continue;
											}
											goto IL_2BE;
										}
									}
									else
									{
										this.m_currentState = this.m_stateQueues[0];
										this.m_stateQueues.RemoveAt(0);
									}
								}
								IL_2BE:
								this.DoQueueState(this.m_currentState);
							}
						}
					}
					IL_2CA:
					if (this.m_lastSetupSelectionPhaseSubType.IsPickFreelancerSubPhase())
					{
						if (!this.m_playerIDsThatSelected.Contains(selectedChar.Key))
						{
							this.m_playerIDsThatSelected.Add(selectedChar.Key);
							for (int i = 0; i < this.m_blueTeamMembers.Length; i++)
							{
								if (this.m_blueTeamMembers[i].PlayerID == selectedChar.Key)
								{
									flag2 = true;
									PlayerName.text = this.m_blueTeamMembers[i].m_playerName.text;
								}
							}
						}
					}
				}
			}
		}
		if (isEnemy)
		{
			bool flag6;
			if (this.m_lastSetupSelectionPhaseSubType.IsPickBanSubPhase())
			{
				flag6 = data.EnemyBans.Contains(selectedChar.Value);
			}
			else if (this.m_lastSetupSelectionPhaseSubType.IsPickFreelancerSubPhase())
			{
				flag6 = data.EnemyTeamSelections.ContainsKey(selectedChar.Key);
			}
			else
			{
				flag6 = false;
			}
			if (flag6)
			{
				CharacterType value2 = selectedChar.Value;
				if (value2 != CharacterType.None)
				{
					flag = true;
					bool flag7 = true;
					if (this.m_lastSetupSelectionPhaseSubType.IsPickFreelancerSubPhase())
					{
						if (this.m_playerIDsThatSelected.Contains(selectedChar.Key))
						{
							flag7 = false;
						}
					}
					if (flag7)
					{
						SelectedCharacterText.text = value2.GetDisplayName();
						SelectedCharacter.sprite = GameWideData.Get().GetCharacterResourceLink(value2).ActorDataPrefab.GetComponent<ActorData>().GetAliveHUDIcon();
						UIManager.SetGameObjectActive(NoCharacter, false, null);
						UIManager.SetGameObjectActive(BrowseCharacter, false, null);
						UIManager.SetGameObjectActive(SelectedCharacter, true, null);
						if (SelectedCharacterAnimator != null)
						{
							UIManager.SetGameObjectActive(SelectedCharacterAnimator, true, null);
							this.m_playerIDBeingAnimated = selectedChar.Key;
							this.m_animatorCurrentlyAnimating = SelectedCharacterAnimator;
						}
						UIManager.SetGameObjectActive(SelectedCharacterNameAnimator, true, null);
						UIManager.SetGameObjectActive(SelectedCharacterNameAnimator.transform.parent, true, null);
						if (this.m_currentState != UIRankedModeDraftScreen.CenterNotification.RedTeamDoubleSelectStart)
						{
							if (this.m_currentState != UIRankedModeDraftScreen.CenterNotification.RedTeamSingleSelectStart)
							{
								if (!this.m_stateQueues.Contains(UIRankedModeDraftScreen.CenterNotification.RedTeamDoubleSelectStart))
								{
									if (!this.m_stateQueues.Contains(UIRankedModeDraftScreen.CenterNotification.RedTeamSingleSelectStart))
									{
										goto IL_5A8;
									}
								}
								while (this.m_currentState != UIRankedModeDraftScreen.CenterNotification.RedTeamDoubleSelectStart)
								{
									if (this.m_currentState == UIRankedModeDraftScreen.CenterNotification.RedTeamSingleSelectStart)
									{
										for (;;)
										{
											switch (4)
											{
											case 0:
												continue;
											}
											goto IL_59C;
										}
									}
									else
									{
										this.m_currentState = this.m_stateQueues[0];
										this.m_stateQueues.RemoveAt(0);
									}
								}
								IL_59C:
								this.DoQueueState(this.m_currentState);
							}
						}
					}
					IL_5A8:
					if (this.m_lastSetupSelectionPhaseSubType.IsPickFreelancerSubPhase() && !this.m_playerIDsThatSelected.Contains(selectedChar.Key))
					{
						this.m_playerIDsThatSelected.Add(selectedChar.Key);
					}
				}
			}
		}
		if (!flag2)
		{
			bool flag8 = true;
			if (this.m_playerIDsThatSelected.Count > 0 && this.m_playerIDsThatSelected[this.m_playerIDsThatSelected.Count - 1] == selectedChar.Key)
			{
				flag8 = false;
			}
			if (this.m_animatorCurrentlyAnimating != null)
			{
				if (this.m_animatorCurrentlyAnimating.gameObject.activeInHierarchy)
				{
					if (this.m_lastSetupSelectionPhaseSubType == this.m_lastDraftNotification.SubPhase)
					{
						for (int j = 0; j < this.m_blueTeamMembers.Length; j++)
						{
							if (this.m_blueTeamMembers[j].PlayerID == this.m_playerIDBeingAnimated)
							{
								flag8 = false;
								PlayerName.text = this.m_blueTeamMembers[j].m_playerName.text;
							}
						}
					}
				}
			}
			if (flag8)
			{
				PlayerName.text = string.Empty;
			}
		}
		this.m_lastSetupSelectionPhaseSubType = this.m_lastDraftNotification.SubPhase;
		if (!flag && !this.CharacterSelectAnimIsPlaying())
		{
			if (selectedChar.Value != CharacterType.None && flag3)
			{
				BrowseCharacter.sprite = GameWideData.Get().GetCharacterResourceLink(selectedChar.Value).ActorDataPrefab.GetComponent<ActorData>().GetAliveHUDIcon();
				UIManager.SetGameObjectActive(NoCharacter, false, null);
				UIManager.SetGameObjectActive(BrowseCharacter, true, null);
				UIManager.SetGameObjectActive(SelectedCharacter, false, null);
			}
			else
			{
				UIManager.SetGameObjectActive(NoCharacter, true, null);
				UIManager.SetGameObjectActive(BrowseCharacter, false, null);
				UIManager.SetGameObjectActive(SelectedCharacter, false, null);
			}
		}
		if (!flag)
		{
			if (!flag3 && !this.CharacterSelectAnimIsPlaying())
			{
				UIManager.SetGameObjectActive(NoCharacter, true, null);
				UIManager.SetGameObjectActive(BrowseCharacter, false, null);
				UIManager.SetGameObjectActive(SelectedCharacter, false, null);
			}
		}
		nameDisplay.text = string.Empty;
	}

	public void SetupBanSelect(RankedResolutionPhaseData data)
	{
		if (this.LastGameInfo != null)
		{
			Team currentTeam = this.GetCurrentTeam(data);
			for (int i = 0; i < this.m_blueBans.Length; i++)
			{
				UIManager.SetGameObjectActive(this.m_blueBans[i], true, null);
			}
			for (int j = 0; j < this.m_redBans.Length; j++)
			{
				UIManager.SetGameObjectActive(this.m_redBans[j], true, null);
			}
			if (data.PlayersOnDeck.Count > 0)
			{
				foreach (RankedResolutionPlayerState rankedResolutionPlayerState in data.PlayersOnDeck)
				{
					if (rankedResolutionPlayerState.Intention != CharacterType.None)
					{
						CharacterResourceLink characterResourceLink = GameWideData.Get().GetCharacterResourceLink(rankedResolutionPlayerState.Intention);
						if (this.GetCurrentTeam(data) == this.LastPlayerInfo.TeamId)
						{
							goto IL_106;
						}
						if (this.LastPlayerInfo.TeamId == Team.Spectator)
						{
							for (;;)
							{
								switch (1)
								{
								case 0:
									continue;
								}
								goto IL_106;
							}
						}
						else if (data.EnemyBans.Count < this.m_redBans.Length)
						{
							this.m_redBans[data.EnemyBans.Count].SetBrowseCharacterImageVisible(true);
							this.m_redBans[data.EnemyBans.Count].SetHoverCharacter(characterResourceLink);
						}
						continue;
						IL_106:
						if (data.FriendlyBans.Count < this.m_blueBans.Length)
						{
							this.m_blueBans[data.FriendlyBans.Count].SetBrowseCharacterImageVisible(true);
							this.m_blueBans[data.FriendlyBans.Count].SetHoverCharacter(characterResourceLink);
						}
					}
					else
					{
						if (data.FriendlyBans.Count < this.m_blueBans.Length)
						{
							this.m_blueBans[data.FriendlyBans.Count].SetBrowseCharacterImageVisible(false);
						}
						if (data.EnemyBans.Count < this.m_redBans.Length)
						{
							this.m_redBans[data.EnemyBans.Count].SetBrowseCharacterImageVisible(false);
						}
					}
				}
			}
			int k = 0;
			while (k < this.m_redBans.Length)
			{
				this.m_redBans[k].SetSelectedCharacterImageVisible(data.EnemyBans.Count > k);
				if (currentTeam == this.LastPlayerInfo.TeamId)
				{
					goto IL_29C;
				}
				if (this.LastPlayerInfo.TeamId == Team.Spectator)
				{
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						goto IL_29C;
					}
				}
				else
				{
					this.m_redBans[k].SetAsSelecting(data.EnemyBans.Count == k);
				}
				IL_2CD:
				k++;
				continue;
				IL_29C:
				this.m_redBans[k].SetAsSelecting(false);
				goto IL_2CD;
			}
			int l = 0;
			while (l < this.m_blueBans.Length)
			{
				this.m_blueBans[l].SetSelectedCharacterImageVisible(data.FriendlyBans.Count > l);
				if (currentTeam == this.LastPlayerInfo.TeamId)
				{
					goto IL_338;
				}
				if (this.LastPlayerInfo.TeamId == Team.Spectator)
				{
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						goto IL_338;
					}
				}
				else
				{
					this.m_blueBans[l].SetAsSelecting(false);
				}
				IL_367:
				l++;
				continue;
				IL_338:
				this.m_blueBans[l].SetAsSelecting(data.FriendlyBans.Count == l);
				goto IL_367;
			}
		}
	}

	private string SubphaseToDisplayName(FreelancerResolutionPhaseSubType subPhase, TeamType teamType, bool isSelf)
	{
		string result = string.Empty;
		switch (subPhase)
		{
		case FreelancerResolutionPhaseSubType.PICK_BANS1:
		case FreelancerResolutionPhaseSubType.PICK_BANS2:
			if (isSelf)
			{
				result = StringUtil.TR("SelectFreelancerBan", "RankMode");
			}
			else if (teamType == TeamType.Ally)
			{
				result = StringUtil.TR("WaitingBlueTeamBan", "RankMode");
			}
			else if (teamType == TeamType.Enemy)
			{
				result = StringUtil.TR("WaitingRedTeamBan", "RankMode");
			}
			break;
		case FreelancerResolutionPhaseSubType.PICK_FREELANCER1:
		case FreelancerResolutionPhaseSubType.PICK_FREELANCER2:
			if (isSelf)
			{
				result = StringUtil.TR("SelectFreelancer", "RankMode");
			}
			else if (teamType == TeamType.Ally)
			{
				result = StringUtil.TR("WaitingBlueTeamSelect", "RankMode");
			}
			else if (teamType == TeamType.Enemy)
			{
				result = StringUtil.TR("WaitingRedTeamSelect", "RankMode");
			}
			break;
		}
		return result;
	}

	internal int OurPlayerId
	{
		get
		{
			long accountId = ClientGameManager.Get().GetPlayerAccountData().AccountId;
			foreach (UIRankedModePlayerDraftEntry uirankedModePlayerDraftEntry in this.m_blueTeamMembers)
			{
				if (uirankedModePlayerDraftEntry.AccountID == accountId)
				{
					return uirankedModePlayerDraftEntry.PlayerID;
				}
			}
			return -1;
		}
	}

	public void SetupInstructions(RankedResolutionPhaseData data)
	{
		Team currentTeam = this.GetCurrentTeam(data);
		TeamType teamType = TeamType.Any;
		if (currentTeam != Team.TeamA)
		{
			if (currentTeam != Team.TeamB)
			{
				this.m_MessageText.color = this.m_neutralColor;
				goto IL_9E;
			}
		}
		if (currentTeam != this.LastPlayerInfo.TeamId)
		{
			if (this.LastPlayerInfo.TeamId == Team.Spectator)
			{
				if (currentTeam == Team.TeamA)
				{
					for (;;)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						goto IL_76;
					}
				}
			}
			this.m_MessageText.color = this.m_redTeamColor;
			teamType = TeamType.Enemy;
			goto IL_9E;
		}
		IL_76:
		this.m_MessageText.color = this.m_blueTeamColor;
		teamType = TeamType.Ally;
		IL_9E:
		if (this.LastGameInfo != null && this.LastGameInfo.GameStatus != GameStatus.Stopped)
		{
			if (this.LastGameInfo.GameStatus > GameStatus.FreelancerSelecting)
			{
				return;
			}
		}
		this.m_MessageText.text = this.SubphaseToDisplayName(this.m_lastDraftNotification.SubPhase, teamType, data.symbol_001D(this.OurPlayerId));
	}

	public void NotifyFreelancerTrades(RankedResolutionPhaseData data)
	{
		long accountId = ClientGameManager.Get().GetPlayerAccountData().AccountId;
		int num = -1;
		bool selfLockedIn = false;
		for (int i = 0; i < this.m_blueTeamMembers.Length; i++)
		{
			if (this.m_blueTeamMembers[i].AccountID == accountId)
			{
				num = this.m_blueTeamMembers[i].PlayerID;
				selfLockedIn = this.DidPlayerLockInDuringSwapPhase(data, (long)this.m_blueTeamMembers[i].PlayerID);
			}
			this.m_blueTeamMembers[i].SetAsSelecting(false);
		}
		for (int j = 0; j < this.m_redTeamMembers.Length; j++)
		{
			this.m_redTeamMembers[j].SetAsSelecting(false);
		}
		for (int k = 0; k < this.m_blueTeamMembers.Length; k++)
		{
			UIRankedModePlayerDraftEntry.TradeStatus status = UIRankedModePlayerDraftEntry.TradeStatus.NoTrade;
			using (List<RankedTradeData>.Enumerator enumerator = data.TradeActions.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					RankedTradeData rankedTradeData = enumerator.Current;
					if (rankedTradeData.TradeAction == RankedTradeData.TradeActionType.symbol_001D)
					{
						if (rankedTradeData.AskedPlayerId == this.m_blueTeamMembers[k].PlayerID)
						{
							if (rankedTradeData.OfferingPlayerId == num)
							{
								status = UIRankedModePlayerDraftEntry.TradeStatus.TradeRequestSent;
								goto IL_1F4;
							}
						}
						if (rankedTradeData.AskedPlayerId == num)
						{
							if (rankedTradeData.OfferingPlayerId == this.m_blueTeamMembers[k].PlayerID)
							{
								status = UIRankedModePlayerDraftEntry.TradeStatus.TradeRequestReceived;
								goto IL_1F4;
							}
						}
						continue;
					}
					if (rankedTradeData.TradeAction != RankedTradeData.TradeActionType.symbol_0012)
					{
						continue;
					}
					if (rankedTradeData.OfferingPlayerId != this.m_blueTeamMembers[k].PlayerID)
					{
						if (rankedTradeData.AskedPlayerId != this.m_blueTeamMembers[k].PlayerID)
						{
							continue;
						}
					}
					status = UIRankedModePlayerDraftEntry.TradeStatus.StopTrading;
					IL_1F4:
					goto IL_204;
				}
			}
			IL_204:
			this.m_blueTeamMembers[k].SetTradeStatus(status, this.m_blueTeamMembers[k].PlayerID == num, selfLockedIn);
		}
	}

	private string GameStatusToDisplayString(GameStatus status)
	{
		string result = string.Empty;
		switch (status)
		{
		case GameStatus.Launching:
		case GameStatus.Launched:
		case GameStatus.Connecting:
		case GameStatus.Connected:
		case GameStatus.Authenticated:
		case GameStatus.Loading:
		case GameStatus.Loaded:
			result = StringUtil.TR("LaunchingGame", "RankMode");
			break;
		}
		return result;
	}

	private void UpdateGameLaunching(GameInfoNotification notification)
	{
		this.GameIsLaunching = true;
		this.m_MessageText.text = this.GameStatusToDisplayString(notification.GameInfo.GameStatus);
		for (int i = 0; i < this.m_blueTeamMembers.Length; i++)
		{
			this.m_blueTeamMembers[i].SetTradePhase(false);
		}
		UIManager.SetGameObjectActive(this.m_lockFreelancerContainer, false, null);
		if (notification.GameInfo.GameStatus >= GameStatus.Launching && notification.GameInfo.GameStatus != GameStatus.Stopped)
		{
			this.SetFreelancerSettingButtonsVisible(false);
			UIRankedCharacterSelectSettingsPanel.Get().SetVisible(false, UICharacterSelectCharacterSettingsPanel.TabPanel.None);
		}
	}

	public void UpdateNotification(EnterFreelancerResolutionPhaseNotification notification, bool updateFromGameInfoUpdate = false)
	{
		if (this.GameIsLaunching)
		{
			return;
		}
		if (notification != null)
		{
			if (notification.RankedData != null)
			{
				RankedResolutionPhaseData value = notification.RankedData.Value;
				this.SetupInstructions(value);
				this.UpdateRankData(value, updateFromGameInfoUpdate);
				this.UpdatePlayerSelecting(value);
				switch (notification.SubPhase)
				{
				case FreelancerResolutionPhaseSubType.PICK_BANS1:
				case FreelancerResolutionPhaseSubType.PICK_BANS2:
					this.SetupBanSelect(value);
					break;
				case FreelancerResolutionPhaseSubType.PICK_FREELANCER1:
				case FreelancerResolutionPhaseSubType.PICK_FREELANCER2:
					this.SetupFreelancerSelect(value);
					break;
				case FreelancerResolutionPhaseSubType.FREELANCER_TRADE:
					this.NotifyFreelancerTrades(value);
					break;
				}
				this.UpdateHoverStatus(value);
				this.UpdateCenter(value, updateFromGameInfoUpdate);
				return;
			}
		}
	}

	public void NotifyButtonClicked(UICharacterPanelSelectRankModeButton btn)
	{
		if (this.m_IsOnDeck)
		{
			bool intendedLockInBtnStatus = false;
			this.m_selectedSubPhaseCharacter = CharacterType.None;
			for (int i = 0; i < this.m_characterListDisplayButtons.Count; i++)
			{
				if (this.m_characterListDisplayButtons[i] == btn)
				{
					this.m_selectedSubPhaseCharacter = this.m_characterListDisplayButtons[i].m_characterType;
					this.m_characterListDisplayButtons[i].SetSelected(true);
					if (!this.m_selectedCharacterTypes.Contains(this.m_characterListDisplayButtons[i].m_characterType))
					{
						if (!this.IsBanned(this.m_characterListDisplayButtons[i].m_characterType))
						{
							this.HoveredCharacter = this.m_selectedSubPhaseCharacter;
							RankedResolutionPhaseData? rankedData = this.m_lastDraftNotification.RankedData;
							RankedResolutionPhaseData value = rankedData.Value;
							if (this.m_lastDraftNotification.SubPhase.IsPickFreelancerSubPhase())
							{
								this.UpdateHoverSelfStatus(value);
							}
							if (!this.m_lastDraftNotification.SubPhase.IsPickBanSubPhase())
							{
								ClientGameManager.Get().UpdateSelectedCharacter(this.m_selectedSubPhaseCharacter, 0);
							}
							ClientGameManager.Get().SendRankedHoverClickRequest(this.m_selectedSubPhaseCharacter);
							intendedLockInBtnStatus = true;
						}
					}
				}
				else
				{
					this.m_characterListDisplayButtons[i].SetSelected(false);
				}
			}
			this.m_intendedLockInBtnStatus = intendedLockInBtnStatus;
		}
		else
		{
			this.m_selectedSubPhaseCharacter = CharacterType.None;
			for (int j = 0; j < this.m_characterListDisplayButtons.Count; j++)
			{
				if (this.m_characterListDisplayButtons[j] == btn)
				{
					this.m_selectedSubPhaseCharacter = this.m_characterListDisplayButtons[j].m_characterType;
					this.m_characterListDisplayButtons[j].SetSelected(true);
					if (!this.m_selectedCharacterTypes.Contains(this.m_characterListDisplayButtons[j].m_characterType))
					{
						if (!this.IsBanned(this.m_characterListDisplayButtons[j].m_characterType))
						{
							ClientGameManager.Get().UpdateSelectedCharacter(this.m_selectedSubPhaseCharacter, 0);
							ClientGameManager.Get().SendRankedHoverClickRequest(this.m_selectedSubPhaseCharacter);
							this.SetupCharacterSettings(this.m_selectedSubPhaseCharacter);
							this.SetFreelancerSettingButtonsVisible(this.m_currentState >= UIRankedModeDraftScreen.CenterNotification.LoadoutPhase);
						}
					}
				}
				else
				{
					this.m_characterListDisplayButtons[j].SetSelected(false);
				}
			}
		}
	}

	public void HandleResolvingDuplicateFreelancerNotification(EnterFreelancerResolutionPhaseNotification notification)
	{
		this.Initialize();
		this.m_lastDraftNotification = notification;
		this.SetupPlayerLists();
		this.UpdateNotification(notification, false);
	}

	private void GetListOfVisibleCharacterTypes()
	{
		this.m_validCharacterTypes.Clear();
		GameManager gameManager = GameManager.Get();
		for (int i = 0; i < 0x28; i++)
		{
			CharacterType characterType = (CharacterType)i;
			if (gameManager.IsCharacterAllowedForPlayers(characterType))
			{
				if (gameManager.IsCharacterAllowedForGameType(characterType, GameType.Ranked, null, null))
				{
					this.m_validCharacterTypes.Add(characterType);
				}
			}
		}
	}

	private void Initialize()
	{
		if (this.m_initialized)
		{
			return;
		}
		this.GameIsLaunching = false;
		this.m_lastSetupSelectionPhaseSubType = FreelancerResolutionPhaseSubType.UNDEFINED;
		this.m_assignedCharacterForGame = CharacterType.None;
		this.m_hoverCharacterForGame = CharacterType.None;
		this.m_selectedSubPhaseCharacter = CharacterType.None;
		UICharacterSelectCharacterSettingsPanel uicharacterSelectCharacterSettingsPanel = UIRankedCharacterSelectSettingsPanel.Get();
		if (uicharacterSelectCharacterSettingsPanel != null)
		{
			uicharacterSelectCharacterSettingsPanel.SetVisible(false, UICharacterSelectCharacterSettingsPanel.TabPanel.None);
		}
		this.m_selectedCharacterTypes.Clear();
		this.m_enemyBannedCharacterTypes.Clear();
		this.m_friendlyBannedCharacterTypes.Clear();
		this.m_playerIDsThatSelected.Clear();
		this.GetListOfVisibleCharacterTypes();
		this.m_initialized = true;
		this.m_MessageText.text = string.Empty;
		this.m_gameCountdownTimer.text = string.Empty;
		this.m_redCountdownTimer.text = string.Empty;
		this.m_blueCountdownTimer.text = string.Empty;
		this.m_stageText.text = string.Empty;
		this.ClearAllStates();
		UIManager.SetGameObjectActive(this.m_pagesContainer, false, null);
		UIManager.SetGameObjectActive(this.m_lockFreelancerContainer, false, null);
		this.m_intendedLockInBtnStatus = false;
		this.m_currentCharacterPage = -1;
		this.m_currentVisiblePage = -1;
		for (int i = 0; i < this.m_blueBans.Length; i++)
		{
			UIManager.SetGameObjectActive(this.m_blueBans[i], false, null);
			this.m_blueBans[i].Init();
		}
		for (int j = 0; j < this.m_redBans.Length; j++)
		{
			UIManager.SetGameObjectActive(this.m_redBans[j], false, null);
			this.m_redBans[j].Init();
		}
		for (int k = 0; k < this.m_blueTeamMembers.Length; k++)
		{
			this.m_blueTeamMembers[k].Init();
			this.m_blueTeamMembers[k].SetTradePhase(false);
		}
		for (int l = 0; l < this.m_redTeamMembers.Length; l++)
		{
			this.m_redTeamMembers[l].Init();
			this.m_redTeamMembers[l].SetTradePhase(false);
		}
		for (int m = 0; m < this.m_characterListDisplayButtons.Count; m++)
		{
			UnityEngine.Object.Destroy(this.m_characterListDisplayButtons[m].gameObject);
		}
		this.m_pageButtons.Clear();
		this.m_characterListDisplayButtons.Clear();
		CharacterType[] array = (CharacterType[])Enum.GetValues(typeof(CharacterType));
		List<CharacterType> list = new List<CharacterType>();
		List<CharacterType> list2 = new List<CharacterType>();
		List<CharacterType> list3 = new List<CharacterType>();
		for (int n = 0; n < array.Length; n++)
		{
			try
			{
				if (array[n] != CharacterType.TestFreelancer1 && array[n] != CharacterType.TestFreelancer2)
				{
					if (array[n] == CharacterType.None)
					{
					}
					else
					{
						CharacterResourceLink characterResourceLink = GameWideData.Get().GetCharacterResourceLink(array[n]);
						if (characterResourceLink.m_characterRole == CharacterRole.Assassin)
						{
							list.Add(array[n]);
						}
						if (characterResourceLink.m_characterRole == CharacterRole.Support)
						{
							list3.Add(array[n]);
						}
						if (characterResourceLink.m_characterRole == CharacterRole.Tank)
						{
							list2.Add(array[n]);
						}
					}
				}
			}
			catch
			{
			}
		}
		list.Sort(new Comparison<CharacterType>(this.CompareCharacterTypeName));
		list2.Sort(new Comparison<CharacterType>(this.CompareCharacterTypeName));
		list3.Sort(new Comparison<CharacterType>(this.CompareCharacterTypeName));
		int num = Mathf.CeilToInt((float)list.Count / 2f);
		int num2 = Mathf.CeilToInt((float)list2.Count / 2f);
		int num3 = Mathf.CeilToInt((float)list3.Count / 2f);
		int num4 = 0;
		for (int num5 = 0; num5 < list.Count; num5++)
		{
			UICharacterPanelSelectRankModeButton uicharacterPanelSelectRankModeButton = UnityEngine.Object.Instantiate<UICharacterPanelSelectButton>(this.m_characterSelectBtnPrefab) as UICharacterPanelSelectRankModeButton;
			uicharacterPanelSelectRankModeButton.m_characterType = list[num5];
			if (num5 - num4 * num >= num)
			{
				num4++;
			}
			UIManager.ReparentTransform(uicharacterPanelSelectRankModeButton.gameObject.transform, this.m_firePowerLayoutGroup.gameObject.transform);
			this.m_characterListDisplayButtons.Add(uicharacterPanelSelectRankModeButton);
		}
		num4 = 0;
		for (int num6 = 0; num6 < list2.Count; num6++)
		{
			UICharacterPanelSelectRankModeButton uicharacterPanelSelectRankModeButton2 = UnityEngine.Object.Instantiate<UICharacterPanelSelectButton>(this.m_characterSelectBtnPrefab) as UICharacterPanelSelectRankModeButton;
			uicharacterPanelSelectRankModeButton2.m_characterType = list2[num6];
			if (num6 - num4 * num2 >= num2)
			{
				num4++;
			}
			UIManager.ReparentTransform(uicharacterPanelSelectRankModeButton2.gameObject.transform, this.m_frontlinerLayoutGroup.gameObject.transform);
			this.m_characterListDisplayButtons.Add(uicharacterPanelSelectRankModeButton2);
		}
		num4 = 0;
		for (int num7 = 0; num7 < list3.Count; num7++)
		{
			UICharacterPanelSelectRankModeButton uicharacterPanelSelectRankModeButton3 = UnityEngine.Object.Instantiate<UICharacterPanelSelectButton>(this.m_characterSelectBtnPrefab) as UICharacterPanelSelectRankModeButton;
			uicharacterPanelSelectRankModeButton3.m_characterType = list3[num7];
			if (num7 - num4 * num3 >= num3)
			{
				num4++;
			}
			UIManager.ReparentTransform(uicharacterPanelSelectRankModeButton3.gameObject.transform, this.m_supportLayoutGroup.gameObject.transform);
			this.m_characterListDisplayButtons.Add(uicharacterPanelSelectRankModeButton3);
		}
		this.SetPageIndex(0);
		UIManager.SetGameObjectActive(this.m_introContainer, true, null);
		this.m_draftScreenContainer.GetComponent<CanvasGroup>().alpha = 1f;
	}

	private int CompareCharacterTypeName(CharacterType CharA, CharacterType CharB)
	{
		return CharA.GetDisplayName().CompareTo(CharB.GetDisplayName());
	}

	private void SetupPageButton(_SelectableBtn btn, int pageIndex)
	{
		TextMeshProUGUI[] componentsInChildren = btn.GetComponentsInChildren<TextMeshProUGUI>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].text = (pageIndex + 1).ToString();
		}
		btn.SetSelected(false, false, string.Empty, string.Empty);
		btn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.PageClicked);
	}

	private void PageClicked(BaseEventData data)
	{
		for (int i = 0; i < this.m_pageButtons.Count; i++)
		{
			if (this.m_pageButtons[i].spriteController.m_hitBoxImage.gameObject == (data as PointerEventData).pointerCurrentRaycast.gameObject)
			{
				this.SetPageIndex(i);
				return;
			}
		}
		for (;;)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			return;
		}
	}

	private bool IsCharacterTypeSelectable(CharacterType type)
	{
		if (!this.m_selectedCharacterTypes.Contains(type))
		{
			if (!this.IsBanned(type))
			{
				if (!GameManager.Get().IsCharacterAllowedForGameType(type, GameType.Ranked, null, null))
				{
					return false;
				}
				return true;
			}
		}
		return false;
	}

	private bool IsCharacterVisibleForPlayer(CharacterType type)
	{
		return GameManager.Get().IsCharacterAllowedForPlayers(type);
	}

	private bool IsCharacterAvailableForPlayer(CharacterType type)
	{
		ClientGameManager clientGameManager = ClientGameManager.Get();
		PersistedCharacterData playerCharacterData = clientGameManager.GetPlayerCharacterData(type);
		if (playerCharacterData != null)
		{
			if (playerCharacterData.CharacterComponent.Unlocked)
			{
				return true;
			}
		}
		if (!clientGameManager.IsCharacterAvailable(type, GameType.Ranked))
		{
			return false;
		}
		return true;
	}

	private void CheckCharacterListValidity()
	{
		bool flag = this.m_lastDraftNotification != null && this.m_lastDraftNotification.SubPhase.IsPickBanSubPhase();
		for (int i = 0; i < this.m_characterListDisplayButtons.Count; i++)
		{
			CharacterType characterType = this.m_characterListDisplayButtons[i].m_characterType;
			bool flag2 = this.IsCharacterAvailableForPlayer(characterType);
			bool flag3;
			if (this.IsCharacterTypeSelectable(characterType))
			{
				if (!flag2)
				{
					flag3 = flag;
				}
				else
				{
					flag3 = true;
				}
			}
			else
			{
				flag3 = false;
			}
			bool enabled = flag3;
			bool flag4 = this.IsCharacterVisibleForPlayer(characterType);
			if (flag4)
			{
				this.m_characterListDisplayButtons[i].SetEnabled(enabled, ClientGameManager.Get().GetPlayerCharacterData(characterType));
				this.m_characterListDisplayButtons[i].SetSelected(this.HoveredCharacter == this.m_characterListDisplayButtons[i].m_characterType || this.SelectedCharacter == this.m_characterListDisplayButtons[i].m_characterType || this.m_selectedSubPhaseCharacter == this.m_characterListDisplayButtons[i].m_characterType);
				UIManager.SetGameObjectActive(this.m_characterListDisplayButtons[i], true, null);
			}
			else
			{
				UIManager.SetGameObjectActive(this.m_characterListDisplayButtons[i], false, null);
			}
		}
	}

	private void UpdateCharacterButtons()
	{
		this.m_currentVisiblePage = this.m_currentCharacterPage;
		bool flag;
		if (this.m_lastDraftNotification != null)
		{
			flag = this.m_lastDraftNotification.SubPhase.IsPickBanSubPhase();
		}
		else
		{
			flag = false;
		}
		bool flag2 = flag;
		for (int i = 0; i < this.m_characterListDisplayButtons.Count; i++)
		{
			CharacterType characterType = this.m_characterListDisplayButtons[i].m_characterType;
			bool flag3 = this.IsCharacterAvailableForPlayer(characterType);
			bool flag4;
			if (this.IsCharacterTypeSelectable(characterType))
			{
				if (!flag3)
				{
					flag4 = flag2;
				}
				else
				{
					flag4 = true;
				}
			}
			else
			{
				flag4 = false;
			}
			bool flag5 = flag4;
			bool flag6 = this.IsCharacterVisibleForPlayer(characterType);
			if (flag6)
			{
				UICharacterPanelSelectButton uicharacterPanelSelectButton = this.m_characterListDisplayButtons[i];
				bool isAvailable = flag5;
				if (this.HoveredCharacter == this.m_characterListDisplayButtons[i].m_characterType)
				{
					goto IL_110;
				}
				if (this.SelectedCharacter == this.m_characterListDisplayButtons[i].m_characterType)
				{
					goto IL_110;
				}
				bool selected = this.m_selectedSubPhaseCharacter == this.m_characterListDisplayButtons[i].m_characterType;
				IL_111:
				uicharacterPanelSelectButton.Setup(isAvailable, selected);
				UIManager.SetGameObjectActive(this.m_characterListDisplayButtons[i], true, null);
				goto IL_13E;
				IL_110:
				selected = true;
				goto IL_111;
			}
			UIManager.SetGameObjectActive(this.m_characterListDisplayButtons[i], false, null);
			IL_13E:;
		}
	}

	private void SetPageIndex(int index)
	{
		this.UpdateCharacterButtons();
		if (!this.IsVisible)
		{
			return;
		}
		this.Initialize();
	}

	public void DismantleRankDraft()
	{
		this.m_assignedCharacterForGame = CharacterType.None;
		this.m_hoverCharacterForGame = CharacterType.None;
		this.m_selectedSubPhaseCharacter = CharacterType.None;
		this.m_lastSetupSelectionPhaseSubType = FreelancerResolutionPhaseSubType.UNDEFINED;
		this.IsVisible = false;
		this.m_initialized = false;
		this.LastGameInfo = null;
		this.LastPlayerInfo = null;
		this.LastTeamInfo = null;
		this.m_lastDraftNotification = null;
		this.m_currentState = UIRankedModeDraftScreen.CenterNotification.None;
		this.m_stateQueues.Clear();
		this.m_playerIDsOnDeck.Clear();
		this.ClearAllStates();
		UIManager.SetGameObjectActive(UIFrontEnd.Get().m_frontEndNavPanel, true, null);
		UIManager.SetGameObjectActive(this.m_draftScreenContainer, false, null);
		UIRankedCharacterSelectSettingsPanel.Get().SetVisible(false, UICharacterSelectCharacterSettingsPanel.TabPanel.None);
		UIManager.SetGameObjectActive(this.m_singleSelectionCharacterSelected, false, null);
		UIManager.SetGameObjectActive(this.m_doubleRightSelectionCharacterSelected, false, null);
		UIManager.SetGameObjectActive(this.m_doubleLeftSelectionCharacterSelected, false, null);
		for (int i = 0; i < this.m_blueTeamMembers.Length; i++)
		{
			this.m_blueTeamMembers[i].Dismantle();
		}
		for (int j = 0; j < this.m_redTeamMembers.Length; j++)
		{
			this.m_redTeamMembers[j].Dismantle();
		}
	}

	public void SetupRankDraft()
	{
		UIPlayerProgressPanel.Get().SetVisible(false, true);
		this.IsVisible = true;
		UIFrontEnd.Get().m_frontEndNavPanel.SetNavButtonSelected(UIFrontEnd.Get().m_frontEndNavPanel.m_PlayBtn);
		UIStorePanel.Get().ClosePurchaseDialog();
		UIRankedModeSelectScreen.Get().SetVisible(false);
		UIManager.SetGameObjectActive(UIFrontEnd.Get().m_frontEndNavPanel, false, null);
		UIManager.SetGameObjectActive(this.m_draftScreenContainer, true, null);
		UIRAFProgramScreen.Get().SetVisible(false);
		this.Initialize();
	}

	public void SetDraftScreenVisible(bool visible)
	{
		if (!this.IsVisible)
		{
			return;
		}
		UIManager.SetGameObjectActive(this.m_draftScreenContainer, visible, null);
		if (visible)
		{
			if (this.m_containerAC == null)
			{
				this.m_containerAC = this.m_draftScreenContainer.GetComponent<Animator>();
			}
			if (this.m_containerAC != null)
			{
				this.m_containerAC.Play("RankedModeSetup", 0, 1f);
			}
		}
	}

	[Serializable]
	public class BrowseCharacterImages
	{
		public Image m_unselected;

		public Image m_browsingCharacter;

		public Image m_selectedCharacter;
	}

	public enum CenterNotification
	{
		None,
		BlueTeamNotification,
		RedTeamNotification,
		BlueTeamSingleSelectStart,
		BlueTeamSingleSelectEnd,
		RedTeamSingleSelectStart,
		RedTeamSingleSelectEnd,
		BlueTeamDoubleSelectStart,
		BlueTeamDoubleSelectEnd,
		RedTeamDoubleSelectStart,
		RedTeamDoubleSelectEnd,
		TradePhase,
		LoadoutPhase,
		GameLoadPhase
	}
}

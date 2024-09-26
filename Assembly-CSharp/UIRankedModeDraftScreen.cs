using System;
using System.Collections.Generic;
using System.Linq;
using LobbyGameClientMessages;
using TMPro;
using UnityEngine;
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
	private List<CenterNotification> m_stateQueues = new List<CenterNotification>();
	private CenterNotification m_currentState;
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
		get => m_hoverCharacterForGame;
		private set
		{
			m_hoverCharacterForGame = value;
			if (value != CharacterType.None)
			{
				m_selectedSubPhaseCharacter = value;
				SetupCharacterSettings(value);
			}
		}
	}

	private void SetupCharacterSettings(CharacterType charType)
	{
		CharacterCardInfo characterCardInfo;
		CharacterVisualInfo characterVisualInfo;
		if (LastPlayerInfo.CharacterType == charType)
		{
			characterCardInfo = LastPlayerInfo.CharacterInfo.CharacterCards;
			characterVisualInfo = LastPlayerInfo.CharacterInfo.CharacterSkin;
		}
		else
		{
			PersistedCharacterData playerCharacterData = ClientGameManager.Get().GetPlayerCharacterData(charType);
			characterCardInfo = playerCharacterData.CharacterComponent.LastCards;
			characterVisualInfo = playerCharacterData.CharacterComponent.LastSkin;
		}
		
		m_rankedModeCharacterSettings.UpdateSelectedCharType(charType);
		CharacterResourceLink characterResourceLink = GameWideData.Get().GetCharacterResourceLink(charType);
		
		if (!m_rankedModeCharacterSettings.m_spellsSubPanel.GetDisplayedCardInfo().Equals(characterCardInfo))
		{
			m_rankedModeCharacterSettings.m_spellsSubPanel.Setup(charType, characterCardInfo);
		}

		if (m_rankedModeCharacterSettings.m_abilitiesSubPanel.GetDisplayedCharacter() == null
		    || !m_rankedModeCharacterSettings.m_abilitiesSubPanel.GetDisplayedCharacter().m_characterType
			    .Equals(characterResourceLink.m_characterType))
		{
			m_rankedModeCharacterSettings.m_abilitiesSubPanel.Setup(characterResourceLink);
		}

		if (!m_rankedModeCharacterSettings.m_skinsSubPanel.GetDisplayedCharacterType()
			    .Equals(characterResourceLink.m_characterType) || !m_rankedModeCharacterSettings.m_skinsSubPanel
			    .GetDisplayedVisualInfo().Equals(characterVisualInfo))
		{
			m_rankedModeCharacterSettings.m_skinsSubPanel.Setup(characterResourceLink, characterVisualInfo);
		}

		if (m_rankedModeCharacterSettings.m_tauntsSubPanel.GetDisplayedCharacter() == null
		    || !m_rankedModeCharacterSettings.m_tauntsSubPanel.GetDisplayedCharacter().m_characterType
			    .Equals(characterResourceLink.m_characterType))
		{
			m_rankedModeCharacterSettings.m_tauntsSubPanel.Setup(characterResourceLink);
		}
	}

	public CharacterType ClientClickedCharacter => m_selectedSubPhaseCharacter;

	public CharacterType SelectedCharacter
	{
		get => m_assignedCharacterForGame;
		private set
		{
			if (value != CharacterType.None
			    && LastGameInfo != null
			    && m_assignedCharacterForGame != value)
			{
				m_assignedCharacterForGame = value;
				m_hoverCharacterForGame = value;
				m_selectedSubPhaseCharacter = value;
				SetupCharacterSettings(value);
			}
		}
	}

	public static UIRankedModeDraftScreen Get()
	{
		return s_instance;
	}

	public override SceneType GetSceneType()
	{
		return SceneType.RankDraft;
	}

	public override void Awake()
	{
		if (s_instance != null)
		{
			return;
		}
		
		s_instance = this;
		ClientGameManager.Get().OnGameInfoNotification += HandleGameInfoNotification;
		ClientGameManager.Get().OnLobbyGameplayOverridesChange += OnLobbyGameplayOverridesUpdated;
		m_lockInBtn.spriteController.callback = LockPhaseButtonClicked;
		m_lockInBtn.spriteController.m_soundToPlay = FrontEndButtonSounds.RankFreelancerSelectClick;
		m_lockFreelancerBtn.spriteController.callback = LockFreelancerBtnClicked;
		m_lockFreelancerBtn.spriteController.m_soundToPlay = FrontEndButtonSounds.RankFreelancerLockin;
		m_skinsBtn.spriteController.callback = SettingsButtonClicked;
		m_abilitiesBtn.spriteController.callback = SettingsButtonClicked;
		m_catalystsBtn.spriteController.callback = SettingsButtonClicked;
		m_tauntsBtn.spriteController.callback = SettingsButtonClicked;
		m_searchInputField.onValueChanged.AddListener(EditedSearchInput);
		
		m_filterButtons = new List<UICharacterSelectFactionFilter>();
		List<CharacterType> characters = new List<CharacterType>();
		characters.AddRange((CharacterType[])Enum.GetValues(typeof(CharacterType)));
		m_filterButtons.Add(m_notOnAFactionFilter);
		foreach (FactionGroup faction in FactionWideData.Get().FactionGroupsToDisplayFilter())
		{
			UICharacterSelectFactionFilter uicharacterSelectFactionFilter = Instantiate(m_factionFilterPrefab);
			uicharacterSelectFactionFilter.transform.SetParent(m_searchFiltersContainer.transform);
			uicharacterSelectFactionFilter.transform.localPosition = Vector3.zero;
			uicharacterSelectFactionFilter.transform.localScale = Vector3.one;
			uicharacterSelectFactionFilter.Setup(faction, ClickedOnFactionFilter);
			m_filterButtons.Add(uicharacterSelectFactionFilter);
			if (faction.Characters != null)
			{
				characters = characters.Except(faction.Characters).ToList();
			}

			uicharacterSelectFactionFilter.m_btn.spriteController.GetComponent<UITooltipHoverObject>().Setup(
				TooltipType.Simple,
				delegate(UITooltipBase tooltip)
			{
				(tooltip as UISimpleTooltip).Setup(FactionGroup.GetDisplayName(faction.FactionGroupID));
				return true;
			});
		}
		m_notOnAFactionFilter.Setup(characters, ClickedOnFactionFilter);
		UITooltipObject tooltipHoverObject = m_notOnAFactionFilter.m_btn.spriteController.GetComponent<UITooltipHoverObject>();
		TooltipType tooltipType = TooltipType.Simple;
			
		tooltipHoverObject.Setup(tooltipType, delegate(UITooltipBase tooltip)
		{
			(tooltip as UISimpleTooltip).Setup(StringUtil.TR("Wildcard", "Global"));
			return true;
		});
		m_centerStateObjects.Add(m_blueTeamTurnNotification.gameObject);
		m_centerStateObjects.Add(m_redTeamTurnNotification.gameObject);
		m_centerStateObjects.Add(m_singleSelectionAC.gameObject);
		m_centerStateObjects.Add(m_doubleSelectionAC.gameObject);
		m_centerStateObjects.Add(m_swapPhaseAC.gameObject);
		m_centerStateObjects.Add(m_loadoutPhaseAC.gameObject);
		m_centerStateObjects.Add(m_gameLoadingAC.gameObject);
		m_containerAC = m_draftScreenContainer.GetComponent<Animator>();
		base.Awake();
	}

	private bool IsBanned(CharacterType characterType)
	{
		return !m_friendlyBannedCharacterTypes.IsNullOrEmpty() && m_friendlyBannedCharacterTypes.Contains(characterType)
		       || !m_enemyBannedCharacterTypes.IsNullOrEmpty() && m_enemyBannedCharacterTypes.Contains(characterType);
	}

	public void EditedSearchInput(string input)
	{
		UpdateCharacterButtonHighlights();
	}

	public void ClickedOnFactionFilter(UICharacterSelectFactionFilter btn)
	{
		if (m_lastFilterBtnClicked != null && m_lastFilterBtnClicked != btn)
		{
			m_lastFilterBtnClicked.m_btn.SetSelected(false, false, string.Empty, string.Empty);
		}
		m_lastFilterBtnClicked = btn;
		UpdateCharacterButtonHighlights();
	}

	private void UpdateCharacterButtonHighlights()
	{
		foreach (UICharacterPanelSelectRankModeButton btn in m_characterListDisplayButtons)
		{
			if (btn != null && btn.GetComponent<CanvasGroup>() != null)
			{
				btn.GetComponent<CanvasGroup>().alpha = 1f;
			}
		}
		if (m_lastFilterBtnClicked != null && m_lastFilterBtnClicked.m_btn.IsSelected())
		{
			foreach (UICharacterPanelSelectRankModeButton btn in m_characterListDisplayButtons)
			{
				if (!m_lastFilterBtnClicked.IsAvailable(btn.m_characterType))
				{
					CanvasGroup canvasGroup = btn.GetComponent<CanvasGroup>();
					if (canvasGroup != null)
					{
						canvasGroup.alpha = 0.3f;
					}
				}
			}
		}
		if (!m_searchInputField.text.IsNullOrEmpty())
		{
			foreach (UICharacterPanelSelectRankModeButton btn in m_characterListDisplayButtons)
			{
				string text = string.Empty;
				CharacterResourceLink characterResourceLink = btn.GetCharacterResourceLink();
				if (characterResourceLink != null)
				{
					text = characterResourceLink.GetDisplayName();
				}
				if (!DoesSearchMatchDisplayName(m_searchInputField.text.ToLower(), text.ToLower()))
				{
					btn.GetComponent<CanvasGroup>().alpha = 0.3f;
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

	public void DoQueueState(CenterNotification notification)
	{
		UIManager.SetGameObjectActive(m_blueTeamTurnNotification, notification == CenterNotification.BlueTeamNotification);
		UIManager.SetGameObjectActive(m_redTeamTurnNotification, notification == CenterNotification.RedTeamNotification);
		UIManager.SetGameObjectActive(
			m_singleSelectionAC,
			notification == CenterNotification.BlueTeamSingleSelectStart
			|| notification == CenterNotification.RedTeamSingleSelectStart);
		UIManager.SetGameObjectActive(
			m_doubleSelectionAC,
			notification == CenterNotification.BlueTeamDoubleSelectStart
			|| notification == CenterNotification.RedTeamDoubleSelectStart);
		UIManager.SetGameObjectActive(m_swapPhaseAC, notification == CenterNotification.TradePhase);
		UIManager.SetGameObjectActive(m_loadoutPhaseAC, notification == CenterNotification.LoadoutPhase);
		UIManager.SetGameObjectActive(m_gameLoadingAC, notification == CenterNotification.GameLoadPhase);
		if (m_lastDraftNotification.SubPhase.IsPickBanSubPhase())
		{
			m_blueTeamTurnTextNotification.text = StringUtil.TR("BlueBans", "RankMode");
			m_redTeamTurnTextNotification.text = StringUtil.TR("RedBans", "RankMode");
		}
		else if (m_lastDraftNotification.SubPhase.IsPickFreelancerSubPhase())
		{
			m_blueTeamTurnTextNotification.text = StringUtil.TR("BluePicks", "RankMode");
			m_redTeamTurnTextNotification.text = StringUtil.TR("RedPicks", "RankMode");
		}

		bool isOnBlueTeam = notification == CenterNotification.BlueTeamNotification
		             || notification == CenterNotification.BlueTeamSingleSelectStart
		             || notification == CenterNotification.BlueTeamDoubleSelectStart;
		bool isOnRedTeam = notification == CenterNotification.RedTeamNotification
		             || notification == CenterNotification.RedTeamSingleSelectStart
		             || notification == CenterNotification.RedTeamDoubleSelectStart;
		SetCenterBackground(isOnBlueTeam, isOnRedTeam);
		if (notification != CenterNotification.BlueTeamNotification
		    && notification != CenterNotification.RedTeamNotification)
		{
			return;
		}
		RankedResolutionPhaseData? rankedData = m_lastDraftNotification.RankedData;
		UpdateCenterVisuals(rankedData.Value, isOnBlueTeam, isOnRedTeam);
	}

	private bool IsDoubleSelectinReadyToAdvance()
	{
		return (m_currentState == CenterNotification.BlueTeamDoubleSelectStart
		        || m_currentState == CenterNotification.RedTeamDoubleSelectStart)
		       && m_stateQueues.Count > 0
		       && (m_stateQueues[0] == CenterNotification.RedTeamDoubleSelectEnd
		           || m_stateQueues[0] == CenterNotification.BlueTeamDoubleSelectEnd)
		       && !m_doubleRightSelectionCharacterSelected.gameObject.activeInHierarchy
		       && !m_doubleLeftSelectionCharacterSelected.gameObject.activeInHierarchy;
	}

	private bool IsAnyCenterStateActive()
	{
		foreach (GameObject obj in m_centerStateObjects)
		{
			if (obj.gameObject.activeSelf)
			{
				return true;
			}
		}

		return false;
	}

	private void ClearAllStates()
	{
		foreach (GameObject obj in m_centerStateObjects)
		{
			UIManager.SetGameObjectActive(obj, false);
		}
	}

	private void QueueCenterState(CenterNotification notification)
	{
		if (notification == CenterNotification.None)
		{
			return;
		}

		if (notification == CenterNotification.LoadoutPhase || notification == CenterNotification.GameLoadPhase)
		{
			ClearAllStates();
			m_stateQueues.Clear();
		}

		if (m_stateQueues.Count > 0)
		{
			if (m_stateQueues[m_stateQueues.Count - 1] != notification)
			{
				m_stateQueues.Add(notification);
			}
		}
		else
		{
			if (m_currentState != notification)
			{
				m_stateQueues.Add(notification);
			}
		}
	}

	public void SettingsButtonClicked(BaseEventData data)
	{
		UICharacterSelectCharacterSettingsPanel.TabPanel tab = UICharacterSelectCharacterSettingsPanel.TabPanel.None;
		GameObject obj = (data as PointerEventData).pointerCurrentRaycast.gameObject;
		if (obj == m_skinsBtn.spriteController.gameObject)
		{
			tab = UICharacterSelectCharacterSettingsPanel.TabPanel.Skins;
		}
		else if (obj == m_abilitiesBtn.spriteController.gameObject)
		{
			tab = UICharacterSelectCharacterSettingsPanel.TabPanel.Abilities;
		}
		else if (obj == m_catalystsBtn.spriteController.gameObject)
		{
			tab = UICharacterSelectCharacterSettingsPanel.TabPanel.Catalysts;
		}
		else if (obj == m_tauntsBtn.spriteController.gameObject)
		{
			tab = UICharacterSelectCharacterSettingsPanel.TabPanel.Taunts;
		}
		UIRankedCharacterSelectSettingsPanel.Get().SetVisible(true, tab);
	}

	private void OnDestroy()
	{
		s_instance = null;
		if (ClientGameManager.Get() != null)
		{
			ClientGameManager.Get().OnGameInfoNotification -= HandleGameInfoNotification;
			ClientGameManager.Get().OnLobbyGameplayOverridesChange -= OnLobbyGameplayOverridesUpdated;
		}
	}

	private void SetFreelancerSettingButtonsVisible(bool visible)
	{
		UIManager.SetGameObjectActive(m_skinsBtn, visible);
		UIManager.SetGameObjectActive(m_abilitiesBtn, visible);
		UIManager.SetGameObjectActive(m_catalystsBtn, visible);
		UIManager.SetGameObjectActive(m_tauntsBtn, visible);
	}

	public void OnLobbyGameplayOverridesUpdated(LobbyGameplayOverrides gameplayOverrides)
	{
		CheckCharacterListValidity();
	}

	public void HandleGameInfoNotification(GameInfoNotification notification)
	{
		if (notification.GameInfo == null)
		{
			Log.Error("Why is GameInfo null?");
			return;
		}
		if (notification.PlayerInfo == null)
		{
			return;
		}
		if (notification.TeamInfo == null)
		{
			Log.Warning("Why GameInfoNotification.TeamInfo null?");
			return;
		}
		if (notification.TeamInfo.TeamPlayerInfo == null)
		{
			Log.Warning("Why GameInfoNotification.TeamInfo.TeamPlayerInfo null?");
			return;
		}
		if (notification.GameInfo.GameStatus == GameStatus.Stopped)
		{
			AppState_GroupCharacterSelect.Get().Enter();
			return;
		}

		if (notification.PlayerInfo.AccountId == 0L || notification.TeamInfo.TeamPlayerInfo.Count == 0)
		{
			return;
		}
		
		if (notification.GameInfo.GameStatus == GameStatus.LoadoutSelecting
		    && m_lastGameStatus != GameStatus.LoadoutSelecting)
		{
			m_loadoutSelectStartTime = Time.realtimeSinceStartup;
			SetFreelancerSettingButtonsVisible(true);
		}
		bool flag = false;
		if (LastGameInfo == null)
		{
			flag = true;
			m_phaseStartTime = Time.time;
		}
		LastGameInfo = notification.GameInfo;
		LastTeamInfo = notification.TeamInfo;
		LastPlayerInfo = notification.PlayerInfo;
		m_lastGameStatus = LastGameInfo.GameStatus;
		if (IsVisible)
		{
			bool isLoadoutSelecting = false;
			if (notification.GameInfo.GameStatus == GameStatus.FreelancerSelecting)
			{
				GameIsLaunching = false;
			}
			else
			{
				if (notification.GameInfo.GameStatus != GameStatus.Stopped
				    && notification.GameInfo.GameStatus == GameStatus.LoadoutSelecting)
				{
					isLoadoutSelecting = true;
					UpdateGameLaunching(notification);
				}
				else if (notification.GameInfo.GameStatus != GameStatus.Stopped
				         && notification.GameInfo.GameStatus > GameStatus.LoadoutSelecting)
				{
					if (!m_stateQueues.Contains(CenterNotification.GameLoadPhase)
					    && m_currentState != CenterNotification.GameLoadPhase)
					{
						QueueCenterState(CenterNotification.GameLoadPhase);
					}
					UpdateGameLaunching(notification);
				}
			}
			if (isLoadoutSelecting
			    && !m_stateQueues.Contains(CenterNotification.LoadoutPhase)
			    && m_currentState != CenterNotification.LoadoutPhase)
			{
				QueueCenterState(CenterNotification.LoadoutPhase);
			}
			if (m_lastDraftNotification != null
			    && m_lastDraftNotification.SubPhase == FreelancerResolutionPhaseSubType.FREELANCER_TRADE
			    && LastGameInfo.GameStatus == GameStatus.FreelancerSelecting
			    && !m_stateQueues.Contains(CenterNotification.TradePhase)
			    && m_currentState != CenterNotification.TradePhase)
			{
				QueueCenterState(CenterNotification.TradePhase);
			}
			MapData mapData = GameWideData.Get().GetMapData(notification.GameInfo.GameConfig.Map);
			string mapDisplayName = GameWideData.Get().GetMapDisplayName(notification.GameInfo.GameConfig.Map);
			Sprite sprite = mapData != null
				? Resources.Load(mapData.ResourceImageSpriteLocation, typeof(Sprite)) as Sprite
				: Resources.Load("Stages/information_stage_image", typeof(Sprite)) as Sprite;
			m_stageImage.sprite = sprite;
			m_introStageImage.sprite = sprite;
			m_stageText.text = mapDisplayName;
			m_introStageText.text = mapDisplayName;
			m_matchFoundText.text = notification.GameInfo.GameConfig.GameType == GameType.Ranked
				? StringUtil.TR("RankedMatchFound", "OverlayScreensScene")
				: string.Format(
					StringUtil.TR("SubtypeFound", "Global"),
					StringUtil.TR(notification.GameInfo.GameConfig.InstanceSubType.LocalizedName));
			SetupPlayerLists();
			UpdateNotification(m_lastDraftNotification, true && !flag);
		}
	}

	private void SetupPlayerLists()
	{
		if (LastGameInfo == null
		    || m_lastDraftNotification == null
		    || m_lastDraftNotification.RankedData == null)
		{
			return;
		}
		
		RankedResolutionPhaseData value = m_lastDraftNotification.RankedData.Value;
		Team team = LastPlayerInfo.TeamId;
		if (LastPlayerInfo.TeamId == Team.Spectator)
		{
			team = Team.TeamA;
		}
		int numBlue = 0;
		int numRed = 0;
		foreach (int playerId in value.PlayerIdByImporance)
		{
			foreach (LobbyPlayerInfo lobbyPlayerInfo in LastTeamInfo.TeamInfo(team))
			{
				if (lobbyPlayerInfo.PlayerId != playerId)
				{
					continue;
				}
				if (numBlue < m_blueTeamMembers.Length)
				{
					m_blueTeamMembers[numBlue].Setup(lobbyPlayerInfo);
					numBlue++;
				}
				break;
			}

			foreach (LobbyPlayerInfo lobbyPlayerInfo in LastTeamInfo.TeamInfo(team.OtherTeam()))
			{
				if (lobbyPlayerInfo.PlayerId != playerId)
				{
					continue;
				}
				if (numRed < m_redTeamMembers.Length)
				{
					m_redTeamMembers[numRed].Setup(lobbyPlayerInfo, true);
					numRed++;
				}

				break;
			}
		}
	}

	private void Update()
	{
		if (m_lastDraftNotification != null)
		{
			if (!GameIsLaunching)
			{
				if (m_lastDraftNotification.RankedData != null)
				{
					if (UICharacterSelectWorldObjects.Get().IsVisible())
					{
						UICharacterSelectWorldObjects.Get().SetVisible(false);
					}
					if (Time.time - m_phaseStartTime < m_timeInPhase.TotalSeconds)
					{
						float num = (float)m_timeInPhase.TotalSeconds - Time.time + m_phaseStartTime;
						int num2 = Mathf.RoundToInt(num);
						RankedResolutionPhaseData value = m_lastDraftNotification.RankedData.Value;
						Team currentTeam = GetCurrentTeam(value);
						if (currentTeam != Team.TeamA)
						{
							if (currentTeam != Team.TeamB)
							{
								if (m_gameCountdownTimer.text != num2.ToString())
								{
									m_gameCountdownTimer.text = num2.ToString();
									m_gameCountdownAC.Play("RankedNumberTextCountdown", 1, 0f);
								}
								m_redCountdownTimer.text = string.Empty;
								m_blueCountdownTimer.text = string.Empty;
								goto IL_28A;
							}
						}
						if (currentTeam == LastPlayerInfo.TeamId)
						{
							m_gameCountdownTimer.text = string.Empty;
							m_redCountdownTimer.text = string.Empty;
							if (m_blueCountdownTimer.text != num2.ToString())
							{
								m_blueCountdownTimer.text = num2.ToString();
								m_blueCountdownAC.Play("RankedNumberTextCountdown", 1, 0f);
							}
						}
						else
						{
							m_gameCountdownTimer.text = string.Empty;
							if (m_redCountdownTimer.text != num2.ToString())
							{
								m_redCountdownTimer.text = num2.ToString();
								m_redCountdownAC.Play("RankedNumberTextCountdown", 1, 0f);
							}
							m_blueCountdownTimer.text = string.Empty;
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
					if (SelectedCharacter != CharacterType.None)
					{
						if (SelectedCharacter != ClientClickedCharacter)
						{
							int playerId = GameManager.Get().PlayerInfo.PlayerId;
							if (!m_lastDraftNotification.RankedData.Value._001D(playerId))
							{
								SetupCharacterSettings(SelectedCharacter);
							}
						}
					}
				}
			}
		}
		if (GameIsLaunching)
		{
			m_gameCountdownTimer.text = string.Empty;
			if (LastGameInfo != null)
			{
				if (LastGameInfo.GameStatus == GameStatus.LoadoutSelecting)
				{
					float num3 = Mathf.Max(0f, (float)LastGameInfo.LoadoutSelectTimeout.TotalSeconds - (Time.realtimeSinceStartup - m_loadoutSelectStartTime));
					int num4 = Mathf.RoundToInt(num3);
					if (m_gameCountdownTimer.text != num4.ToString())
					{
						m_gameCountdownTimer.text = num4.ToString();
						m_gameCountdownAC.Play("RankedNumberTextCountdown", 1, 0f);
					}
					if (num4 < 6 && Mathf.RoundToInt(num3 + Time.deltaTime) != num4)
					{
						UIFrontEnd.PlaySound(FrontEndButtonSounds.RankModeTimerTick);
					}
				}
			}
			m_redCountdownTimer.text = string.Empty;
			m_blueCountdownTimer.text = string.Empty;
		}
		float axis = Input.GetAxis("Mouse ScrollWheel");
		if (axis > 0f)
		{
			SetPageIndex(m_currentCharacterPage + 1);
		}
		else if (axis < 0f)
		{
			SetPageIndex(m_currentCharacterPage - 1);
		}
		if (!m_introContainer.gameObject.activeSelf)
		{
			if (IsAnyCenterStateActive())
			{
				if (!IsDoubleSelectinReadyToAdvance())
				{
					goto IL_524;
				}
			}
			if (m_stateQueues.Count > 0)
			{
				DoQueueState(m_stateQueues[0]);
				m_currentState = m_stateQueues[0];
				m_stateQueues.RemoveAt(0);
			}
		}
		IL_524:
		if (m_characterSelectContainerCanvasGroup == null)
		{
			m_characterSelectContainerCanvasGroup = m_characterSelectContainer.GetComponent<CanvasGroup>();
		}
		if (m_journeyLength <= 0f)
		{
			if (m_currentCharacterPage == m_currentVisiblePage)
			{
				goto IL_7F3;
			}
		}
		float num5 = (Time.time - m_startTime) * m_timeForPageToSwap;
		float num6 = num5 / m_journeyLength;
		Vector2 anchoredPosition = Vector2.Lerp(m_startLocation, m_endLocation, num6);
		if (!float.IsNaN(anchoredPosition.x))
		{
			if (!float.IsNaN(anchoredPosition.y))
			{
				(m_characterSelectContainer.transform as RectTransform).anchoredPosition = anchoredPosition;
				if (m_characterSelectContainerCanvasGroup != null)
				{
					if (m_currentCharacterPage != m_currentVisiblePage)
					{
						m_characterSelectContainerCanvasGroup.alpha = 1f - num6;
					}
					else
					{
						m_characterSelectContainerCanvasGroup.alpha = num6;
					}
				}
				if (num6 >= 1f)
				{
					if (m_characterSelectContainerCanvasGroup.alpha <= 0f)
					{
						Vector2 anchoredPosition2 = (m_characterSelectContainer.gameObject.transform as RectTransform).anchoredPosition;
						if (m_endLocation.x < 0f)
						{
							(m_characterSelectContainer.gameObject.transform as RectTransform).anchoredPosition = new Vector2(anchoredPosition2.x * -1f, anchoredPosition2.y);
							m_startTime = Time.time;
							m_startLocation = anchoredPosition2;
							m_endLocation = new Vector2(0f, anchoredPosition2.y);
							m_journeyLength = Vector2.Distance(m_startLocation, m_endLocation);
						}
						else if (m_endLocation.x > 0f)
						{
							(m_characterSelectContainer.gameObject.transform as RectTransform).anchoredPosition = new Vector2(anchoredPosition2.x * -1f, anchoredPosition2.y);
							m_startTime = Time.time;
							m_startLocation = anchoredPosition2;
							m_endLocation = new Vector2(0f, anchoredPosition2.y);
							m_journeyLength = Vector2.Distance(m_startLocation, m_endLocation);
						}
						UpdateCharacterButtons();
					}
					else
					{
						m_journeyLength = 0f;
					}
				}
			}
		}
		IL_7F3:
		if (IsCenterSelectAnimating())
		{
			UIManager.SetGameObjectActive(m_lockInBtn, false);
		}
		else
		{
			string text;
			if (m_lastDraftNotification != null && m_lastDraftNotification.SubPhase.IsPickBanSubPhase())
			{
				text = StringUtil.TR("Ban", "OverlayScreensScene");
			}
			else
			{
				text = StringUtil.TR("LockIn", "OverlayScreensScene");
			}
			for (int i = 0; i < m_lockInText.Length; i++)
			{
				m_lockInText[i].text = text;
			}
			UIManager.SetGameObjectActive(m_lockInBtn, m_intendedLockInBtnStatus);
			m_lockInBtn.SetDisabled(m_selectedSubPhaseCharacter == SelectedCharacter);
		}
		if (m_containerAC == null)
		{
			m_containerAC = m_draftScreenContainer.GetComponent<Animator>();
		}
		if (m_containerAC != null)
		{
			if (m_containerAC.gameObject.activeInHierarchy)
			{
				if (m_containerAC.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
				{
					DoCharacterSelectContainerActiveCheck();
				}
			}
		}
		UIManager.SetGameObjectActive(m_searchFiltersContainer, m_characterSelectContainer.gameObject.activeSelf);
	}

	private bool IsCenterSelectAnimating()
	{
		bool result = false;
		if (m_singleSelectionCharacterSelected.gameObject.activeInHierarchy)
		{
			result = true;
		}
		else if (m_doubleRightSelectionCharacterSelected.gameObject.activeInHierarchy && doubleSelectionLeftCharacter.gameObject.activeInHierarchy)
		{
			result = true;
		}
		else if (m_doubleLeftSelectionCharacterSelected.gameObject.activeInHierarchy)
		{
			if (doubleSelectionRightCharacter.gameObject.activeInHierarchy)
			{
				result = true;
			}
		}
		return result;
	}

	public void LockFreelancerBtnClicked(BaseEventData data)
	{
		if (m_lastDraftNotification.SubPhase == FreelancerResolutionPhaseSubType.FREELANCER_TRADE)
		{
			UIManager.SetGameObjectActive(m_lockFreelancerContainer, false);
			ClientGameManager.Get().SendRankedTradeRequest_StopTrading();
		}
	}

	public void LockPhaseButtonClicked(BaseEventData data)
	{
		if (m_selectedSubPhaseCharacter != CharacterType.None)
		{
			if (SelectedCharacter != m_selectedSubPhaseCharacter)
			{
				if (m_lastDraftNotification.SubPhase.IsPickBanSubPhase())
				{
					ClientGameManager.Get().SendRankedBanRequest(m_selectedSubPhaseCharacter);
				}
				else
				{
					ClientGameManager.Get().SendRankedSelectRequest(m_selectedSubPhaseCharacter);
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
					if (rankedTradeData.OfferingPlayerId == playerID)
					{
						if (rankedTradeData.TradeAction == RankedTradeData.TradeActionType.StopTrading)
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
		if (m_lastDraftNotification.SubPhase.IsPickBanSubPhase())
		{
			return;
		}
		if (LastGameInfo != null)
		{
			int playerId = LastPlayerInfo.PlayerId;
			int i = 0;
			while (i < m_blueTeamMembers.Length)
			{
				if (m_blueTeamMembers[i].PlayerID == playerId)
				{
					if (HoveredCharacter != CharacterType.None)
					{
						if (!m_selectedCharacterTypes.Contains(HoveredCharacter))
						{
							if (!IsBanned(HoveredCharacter))
							{
								CharacterResourceLink characterResourceLink = GameWideData.Get().GetCharacterResourceLink(HoveredCharacter);
								m_blueTeamMembers[i].SetBrowseCharacterImageVisible(true);
								m_blueTeamMembers[i].SetHoverCharacter(characterResourceLink);
							}
						}
					}
					break;
				}

				i++;
			}
			if (m_playerIDsOnDeck.Count == 1)
			{
				KeyValuePair<int, CharacterType> selectedChar = new KeyValuePair<int, CharacterType>(playerId, HoveredCharacter);
				if (m_playerIDsOnDeck.ContainsKey(playerId))
				{
					SetupSelection(selectedChar, data, m_singleCharacterName, singleNoSelectionCharacter, singleBrowseSelectionCharacter, singleSelectionCharacter, m_singleSelectionCharacterSelected, m_singleBlueSelectionCharacterSelected, m_singleBlueTeamSelectedCharacter, m_singleBlueTeamPlayerName, true, false);
				}
				return;
			}
			if (m_playerIDsOnDeck.Count == 2)
			{
				bool flag = true;
				using (Dictionary<int, CharacterType>.Enumerator enumerator = m_playerIDsOnDeck.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<int, CharacterType> selectedChar2 = enumerator.Current;
						if (flag)
						{
							SetupSelection(selectedChar2, data, m_leftCharacterName, doubleNoSelectionLeftCharacter, doubleBrowseSelectionLeftCharacter, doubleSelectionLeftCharacter, m_doubleLeftSelectionCharacterSelected, m_doubleLeftBlueSelectionCharacterSelected, m_doubleLeftBlueTeamSelectedCharacter, m_doubleLeftBlueTeamPlayerName, true, false);
							flag = false;
						}
						else
						{
							SetupSelection(selectedChar2, data, m_rightCharacterName, doubleNoSelectionRightCharacter, doubleBrowseSelectionRightCharacter, doubleSelectionRightCharacter, m_doubleRightSelectionCharacterSelected, m_doubleRightBlueSelectionCharacterSelected, m_doubleRightBlueTeamSelectedCharacter, m_doubleRightBlueTeamPlayerName, true, false);
						}
					}
				}
			}
		}
	}

	private void UpdateHoverStatus(RankedResolutionPhaseData data)
	{
		bool flag = m_lastDraftNotification.SubPhase.IsPickBanSubPhase();
		int i = 0;
		while (i < m_blueTeamMembers.Length)
		{
			if (!flag)
			{
				goto IL_77;
			}
			if (!m_playerIDsOnDeck.ContainsKey(OurPlayerId))
			{
				goto IL_77;
			}
			if (m_blueTeamMembers[i].PlayerID != OurPlayerId)
			{
				goto IL_77;
			}
			IL_330:
			i++;
			continue;
			IL_77:
			if (data.FriendlyTeamSelections.ContainsKey(m_blueTeamMembers[i].PlayerID))
			{
				m_blueTeamMembers[i].SetBrowseCharacterImageVisible(false);
				goto IL_330;
			}
			if (m_playerIDsOnDeck.ContainsKey(m_blueTeamMembers[i].PlayerID))
			{
				if (m_playerIDsOnDeck[m_blueTeamMembers[i].PlayerID] != CharacterType.None)
				{
					if (IsBanned(m_playerIDsOnDeck[m_blueTeamMembers[i].PlayerID]) || m_selectedCharacterTypes.Contains(m_playerIDsOnDeck[m_blueTeamMembers[i].PlayerID]))
					{
						m_blueTeamMembers[i].SetBrowseCharacterImageVisible(false);
						UIManager.SetGameObjectActive(m_blueTeamMembers[i].m_noCharacterImage, true);
					}
					else if (OurPlayerId != m_blueTeamMembers[i].PlayerID)
					{
						CharacterResourceLink characterResourceLink = GameWideData.Get().GetCharacterResourceLink(m_playerIDsOnDeck[m_blueTeamMembers[i].PlayerID]);
						m_blueTeamMembers[i].SetBrowseCharacterImageVisible(true);
						m_blueTeamMembers[i].SetHoverCharacter(characterResourceLink);
					}
					else if (HoveredCharacter != CharacterType.None)
					{
						CharacterResourceLink characterResourceLink2 = GameWideData.Get().GetCharacterResourceLink(HoveredCharacter);
						m_blueTeamMembers[i].SetBrowseCharacterImageVisible(true);
						m_blueTeamMembers[i].SetHoverCharacter(characterResourceLink2);
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
					if (rankedResolutionPlayerState.PlayerId == m_blueTeamMembers[i].PlayerID)
					{
						if (rankedResolutionPlayerState.Intention != CharacterType.None)
						{
							if (!IsBanned(rankedResolutionPlayerState.Intention))
							{
								if (!m_selectedCharacterTypes.Contains(rankedResolutionPlayerState.Intention))
								{
									CharacterResourceLink characterResourceLink3 = GameWideData.Get().GetCharacterResourceLink(rankedResolutionPlayerState.Intention);
									if (characterResourceLink3 != null)
									{
										browseCharacterImageVisible = true;
										m_blueTeamMembers[i].SetHoverCharacter(characterResourceLink3);
									}
									goto IL_2F7;
								}
							}
							m_blueTeamMembers[i].SetBrowseCharacterImageVisible(false);
							UIManager.SetGameObjectActive(m_blueTeamMembers[i].m_noCharacterImage, true);
						}
						IL_2F7:
						goto IL_321;
					}
				}
			}
			IL_321:
			m_blueTeamMembers[i].SetBrowseCharacterImageVisible(browseCharacterImageVisible);
			goto IL_330;
		}
		for (int j = 0; j < m_redTeamMembers.Length; j++)
		{
			if (data.EnemyTeamSelections.ContainsKey(m_redTeamMembers[j].PlayerID))
			{
				m_redTeamMembers[j].SetBrowseCharacterImageVisible(false);
			}
			else
			{
				bool flag2 = false;
				if (m_playerIDsOnDeck.ContainsKey(m_redTeamMembers[j].PlayerID) && m_playerIDsOnDeck[m_redTeamMembers[j].PlayerID] != CharacterType.None)
				{
					if (!IsBanned(m_playerIDsOnDeck[m_redTeamMembers[j].PlayerID]))
					{
						CharacterResourceLink characterResourceLink4 = GameWideData.Get().GetCharacterResourceLink(m_playerIDsOnDeck[m_redTeamMembers[j].PlayerID]);
						m_redTeamMembers[j].SetBrowseCharacterImageVisible(true);
						m_redTeamMembers[j].SetHoverCharacter(characterResourceLink4);
						flag2 = true;
					}
				}
				if (!flag2)
				{
					m_redTeamMembers[j].SetBrowseCharacterImageVisible(false);
				}
			}
		}
	}

	private void CheckSelectedCharForCenterPiece(bool isOnBlue, RankedResolutionPhaseData data)
	{
		if (m_playerIDsOnDeck.Count == 1)
		{
			UIManager.SetGameObjectActive(m_singleSelectionCharacterSelected, true);
			using (Dictionary<int, CharacterType>.Enumerator enumerator = m_playerIDsOnDeck.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<int, CharacterType> keyValuePair = enumerator.Current;
					KeyValuePair<int, CharacterType> selectedChar = keyValuePair;
					TextMeshProUGUI singleCharacterName = m_singleCharacterName;
					Image noCharacter = singleNoSelectionCharacter;
					Image browseCharacter = singleBrowseSelectionCharacter;
					Image selectedCharacter = singleSelectionCharacter;
					Animator singleSelectionCharacterSelected = m_singleSelectionCharacterSelected;
					Animator selectedCharacterNameAnimator;
					if (isOnBlue)
					{
						selectedCharacterNameAnimator = m_singleBlueSelectionCharacterSelected;
					}
					else
					{
						selectedCharacterNameAnimator = m_singleRedTeamSelectionCharacterSelected;
					}
					SetupSelection(selectedChar, data, singleCharacterName, noCharacter, browseCharacter, selectedCharacter, singleSelectionCharacterSelected, selectedCharacterNameAnimator, (!isOnBlue) ? m_singleRedTeamSelectedCharacter : m_singleBlueTeamSelectedCharacter, (!isOnBlue) ? m_singleRedTeamPlayerName : m_singleBlueTeamPlayerName, isOnBlue, !isOnBlue);
				}
			}
		}
		else
		{
			bool flag = !doubleSelectionLeftCharacter.gameObject.activeInHierarchy;
			using (Dictionary<int, CharacterType>.Enumerator enumerator2 = m_playerIDsOnDeck.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					KeyValuePair<int, CharacterType> keyValuePair2 = enumerator2.Current;
					if (flag)
					{
						flag = false;
						KeyValuePair<int, CharacterType> selectedChar2 = keyValuePair2;
						TextMeshProUGUI leftCharacterName = m_leftCharacterName;
						Image noCharacter2 = doubleNoSelectionLeftCharacter;
						Image browseCharacter2 = doubleBrowseSelectionLeftCharacter;
						Image selectedCharacter2 = doubleSelectionLeftCharacter;
						Animator doubleLeftSelectionCharacterSelected = m_doubleLeftSelectionCharacterSelected;
						Animator selectedCharacterNameAnimator2;
						if (isOnBlue)
						{
							selectedCharacterNameAnimator2 = m_doubleLeftBlueSelectionCharacterSelected;
						}
						else
						{
							selectedCharacterNameAnimator2 = m_doubleLeftRedTeamSelectionCharacterSelected;
						}
						TextMeshProUGUI selectedCharacterText;
						if (isOnBlue)
						{
							selectedCharacterText = m_doubleLeftBlueTeamSelectedCharacter;
						}
						else
						{
							selectedCharacterText = m_doubleLeftRedTeamSelectedCharacter;
						}
						SetupSelection(selectedChar2, data, leftCharacterName, noCharacter2, browseCharacter2, selectedCharacter2, doubleLeftSelectionCharacterSelected, selectedCharacterNameAnimator2, selectedCharacterText, (!isOnBlue) ? m_doubleLeftRedTeamPlayerName : m_doubleLeftBlueTeamPlayerName, isOnBlue, !isOnBlue);
					}
					else
					{
						KeyValuePair<int, CharacterType> selectedChar3 = keyValuePair2;
						TextMeshProUGUI rightCharacterName = m_rightCharacterName;
						Image noCharacter3 = doubleNoSelectionRightCharacter;
						Image browseCharacter3 = doubleBrowseSelectionRightCharacter;
						Image selectedCharacter3 = doubleSelectionRightCharacter;
						Animator doubleRightSelectionCharacterSelected = m_doubleRightSelectionCharacterSelected;
						Animator selectedCharacterNameAnimator3 = (!isOnBlue) ? m_doubleRightRedTeamSelectionCharacterSelected : m_doubleRightBlueSelectionCharacterSelected;
						TextMeshProUGUI selectedCharacterText2 = (!isOnBlue) ? m_doubleRightRedTeamSelectedCharacter : m_doubleRightBlueTeamSelectedCharacter;
						TextMeshProUGUI playerName;
						if (isOnBlue)
						{
							playerName = m_doubleRightBlueTeamPlayerName;
						}
						else
						{
							playerName = m_doubleRightRedTeamPlayerName;
						}
						SetupSelection(selectedChar3, data, rightCharacterName, noCharacter3, browseCharacter3, selectedCharacter3, doubleRightSelectionCharacterSelected, selectedCharacterNameAnimator3, selectedCharacterText2, playerName, isOnBlue, !isOnBlue);
					}
				}
			}
		}
	}

	private void UpdatePlayerSelecting(RankedResolutionPhaseData data)
	{
		for (int i = 0; i < m_blueTeamMembers.Length; i++)
		{
			m_blueTeamMembers[i].SetAsSelecting(data._001D(m_blueTeamMembers[i].PlayerID));
		}
		for (int j = 0; j < m_redTeamMembers.Length; j++)
		{
			m_redTeamMembers[j].SetAsSelecting(data._001D(m_redTeamMembers[j].PlayerID));
		}
	}

	private void DoCharacterSelectContainerActiveCheck()
	{
		bool flag;
		if (m_lastDraftNotification != null)
		{
			flag = m_lastDraftNotification.SubPhase.IsPickBanSubPhase();
		}
		else
		{
			flag = false;
		}
		bool flag2 = flag;
		UIManager.SetGameObjectActive(m_characterSelectContainer, true);
		foreach (UICharacterPanelSelectRankModeButton uicharacterPanelSelectButton in m_characterSelectContainer.GetComponentsInChildren<UICharacterPanelSelectRankModeButton>(true))
		{
			bool clickable;
			if (!flag2)
			{
				clickable = (SelectedCharacter == CharacterType.None);
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
		if (LastGameInfo != null)
		{
			m_timeInPhase = data.TimeLeftInSubPhase;
			if (!updateFromGameInfoUpdate)
			{
				m_phaseStartTime = Time.time;
			}
			m_IsOnDeck = data._001D(OurPlayerId);
			DoCharacterSelectContainerActiveCheck();
			bool intendedLockInBtnStatus;
			if (m_IsOnDeck)
			{
				if (!IsBanned(HoveredCharacter) && !m_selectedCharacterTypes.Contains(HoveredCharacter))
				{
					if (!m_lastDraftNotification.SubPhase.IsPickBanSubPhase())
					{
						intendedLockInBtnStatus = m_lastDraftNotification.SubPhase.IsPickFreelancerSubPhase();
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
			m_intendedLockInBtnStatus = intendedLockInBtnStatus;
			if (m_IsOnDeck)
			{
				if (!updateFromGameInfoUpdate)
				{
					HoveredCharacter = data.PlayersOnDeck.Find(p => p.PlayerId == OurPlayerId).Intention;
				}
			}
			else
			{
				HoveredCharacter = CharacterType.None;
			}
			SetFreelancerSettingButtonsVisible(m_currentState >= CenterNotification.LoadoutPhase);
			UIManager.SetGameObjectActive(m_lockFreelancerContainer, false);
			if (m_lastDraftNotification.SubPhase == FreelancerResolutionPhaseSubType.FREELANCER_TRADE)
			{
				if (LastGameInfo.GameStatus == GameStatus.FreelancerSelecting)
				{
					if (!m_stateQueues.Contains(CenterNotification.TradePhase))
					{
						if (m_currentState != CenterNotification.TradePhase)
						{
							QueueCenterState(CenterNotification.TradePhase);
						}
					}
				}
			}
			if (!m_lastDraftNotification.SubPhase.IsPickBanSubPhase())
			{
				for (int i = 0; i < m_blueBans.Length; i++)
				{
					m_blueBans[i].SetAsSelecting(false);
					m_redBans[i].SetAsSelecting(false);
				}
			}
			for (int j = 0; j < m_blueBans.Length; j++)
			{
				if (j >= data.FriendlyBans.Count)
				{
					break;
				}
				CharacterType characterType = data.FriendlyBans[j];
				if (m_blueBans[j] != null)
				{
					CharacterResourceLink characterResourceLink = GameWideData.Get().GetCharacterResourceLink(characterType);
					if (characterResourceLink != null)
					{
						m_blueBans[j].SetSelectedCharacterImageVisible(true);
						if (m_blueBans[j].GetSelectedCharacter() == CharacterType.None)
						{
							UIFrontEnd.PlaySound(FrontEndButtonSounds.RankModeBanPlayer);
							CheckSelectedCharForCenterPiece(true, data);
						}
						m_blueBans[j].SetCharacter(characterResourceLink);
					}
				}
				if (!m_friendlyBannedCharacterTypes.Contains(characterType))
				{
					m_friendlyBannedCharacterTypes.Add(characterType);
				}
			}
			for (int k = 0; k < m_redBans.Length; k++)
			{
				if (k >= data.EnemyBans.Count)
				{
					break;
				}
				CharacterType characterType2 = data.EnemyBans[k];
				if (m_redBans[k] != null)
				{
					CharacterResourceLink characterResourceLink2 = GameWideData.Get().GetCharacterResourceLink(characterType2);
					if (characterResourceLink2 != null)
					{
						m_redBans[k].SetSelectedCharacterImageVisible(true);
						if (m_redBans[k].GetSelectedCharacter() == CharacterType.None)
						{
							UIFrontEnd.PlaySound(FrontEndButtonSounds.RankModeBanPlayer);
							CheckSelectedCharForCenterPiece(false, data);
						}
						m_redBans[k].SetCharacter(characterResourceLink2);
					}
				}
				if (!m_enemyBannedCharacterTypes.Contains(characterType2))
				{
					m_enemyBannedCharacterTypes.Add(characterType2);
				}
			}
			long accountId = ClientGameManager.Get().GetPlayerAccountData().AccountId;
			for (int l = 0; l < m_blueTeamMembers.Length; l++)
			{
				m_blueTeamMembers[l].CanBeTraded = false;
				bool selectedCharacterImageVisible = false;
				if (data.FriendlyTeamSelections.ContainsKey(m_blueTeamMembers[l].PlayerID))
				{
					CharacterResourceLink characterResourceLink3 = GameWideData.Get().GetCharacterResourceLink(data.FriendlyTeamSelections[m_blueTeamMembers[l].PlayerID]);
					if (characterResourceLink3 != null)
					{
						selectedCharacterImageVisible = true;
						if (m_blueTeamMembers[l].GetSelectedCharacter() == CharacterType.None)
						{
							UIFrontEnd.PlaySound(FrontEndButtonSounds.RankModePickPlayer);
							CheckSelectedCharForCenterPiece(true, data);
						}
						m_blueTeamMembers[l].SetCharacter(characterResourceLink3);
						if (!IsBanned(characterResourceLink3.m_characterType))
						{
							m_friendlyBannedCharacterTypes.Add(characterResourceLink3.m_characterType);
						}
						m_blueTeamMembers[l].CanBeTraded = !DidPlayerLockInDuringSwapPhase(data, m_blueTeamMembers[l].PlayerID);
					}
					if (m_blueTeamMembers[l].AccountID == accountId)
					{
						SelectedCharacter = m_blueTeamMembers[l].GetSelectedCharacter();
					}
				}
				m_blueTeamMembers[l].SetTradePhase(m_lastDraftNotification.SubPhase == FreelancerResolutionPhaseSubType.FREELANCER_TRADE);
				if (m_lastDraftNotification.SubPhase != FreelancerResolutionPhaseSubType.FREELANCER_TRADE)
				{
					m_blueTeamMembers[l].SetCharacterLocked(false);
				}
				UIManager.SetGameObjectActive(m_blueTeamMembers[l], true);
				m_blueTeamMembers[l].SetSelectedCharacterImageVisible(selectedCharacterImageVisible);
			}
			for (int m = 0; m < m_redTeamMembers.Length; m++)
			{
				bool selectedCharacterImageVisible2 = false;
				if (data.EnemyTeamSelections.ContainsKey(m_redTeamMembers[m].PlayerID))
				{
					CharacterResourceLink characterResourceLink4 = GameWideData.Get().GetCharacterResourceLink(data.EnemyTeamSelections[m_redTeamMembers[m].PlayerID]);
					if (characterResourceLink4 != null)
					{
						selectedCharacterImageVisible2 = true;
						if (m_redTeamMembers[m].GetSelectedCharacter() == CharacterType.None)
						{
							UIFrontEnd.PlaySound(FrontEndButtonSounds.RankModePickPlayer);
							CheckSelectedCharForCenterPiece(false, data);
						}
						m_redTeamMembers[m].SetCharacter(characterResourceLink4);
						if (!IsBanned(characterResourceLink4.m_characterType))
						{
							m_enemyBannedCharacterTypes.Add(characterResourceLink4.m_characterType);
						}
					}
				}
				UIManager.SetGameObjectActive(m_redTeamMembers[m], true);
				m_redTeamMembers[m].SetSelectedCharacterImageVisible(selectedCharacterImageVisible2);
			}
			if (m_currentCharacterPage == -1)
			{
				SetPageIndex(0);
			}
			CheckCharacterListValidity();
		}
	}

	public Team GetCurrentTeam(RankedResolutionPhaseData data)
	{
		if (LastTeamInfo != null && LastPlayerInfo != null)
		{
			foreach (LobbyPlayerInfo lobbyPlayerInfo in LastTeamInfo.TeamPlayerInfo)
			{
				if (data._001D(lobbyPlayerInfo.PlayerId))
				{
					return lobbyPlayerInfo.TeamId;
				}
			}
			if (m_lastDraftNotification == null)
			{
				return Team.Invalid;
			}
			if (!m_lastDraftNotification.SubPhase.IsPickFreelancerSubPhase())
			{
				if (!m_lastDraftNotification.SubPhase.IsPickBanSubPhase())
				{
					return Team.Invalid;
				}
			}
			if (LastPlayerInfo.TeamId == Team.Spectator)
			{
				return Team.TeamA;
			}
			return LastPlayerInfo.TeamId.OtherTeam();
		}
		return Team.Invalid;
	}

	private void SetCenterBackground(bool isOnBlue, bool isOnRed)
	{
		for (int i = 0; i < singleSelectionBlueTeam.Length; i++)
		{
			UIManager.SetGameObjectActive(singleSelectionBlueTeam[i], isOnBlue);
		}
		for (int j = 0; j < singleSelectionRedTeam.Length; j++)
		{
			UIManager.SetGameObjectActive(singleSelectionRedTeam[j], isOnRed);
		}
		for (int k = 0; k < doubleSelectionBlueTeam.Length; k++)
		{
			UIManager.SetGameObjectActive(doubleSelectionBlueTeam[k], isOnBlue);
		}
		for (int l = 0; l < doubleSelectionRedTeam.Length; l++)
		{
			UIManager.SetGameObjectActive(doubleSelectionRedTeam[l], isOnRed);
		}
	}

	private void PrintData(RankedResolutionPhaseData data)
	{
		string text = "LAST RANKED RESOLUTION PHASE DATA!\n";
		text += "Blue team info:\n";
		for (int i = 0; i < m_blueTeamMembers.Length; i++)
		{
			using (List<RankedResolutionPlayerState>.Enumerator enumerator = data.UnselectedPlayerStates.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					RankedResolutionPlayerState rankedResolutionPlayerState = enumerator.Current;
					if (rankedResolutionPlayerState.PlayerId == m_blueTeamMembers[i].PlayerID)
					{
						text += string.Format("PlayerID is {0}, PlayerName is {1}, Is On deck: {2}\n", rankedResolutionPlayerState.PlayerId, m_blueTeamMembers[i].m_playerName, rankedResolutionPlayerState.OnDeckness);
						goto IL_BE;
					}
				}
			}
			IL_BE:;
		}
		text += "Red team info:\n";
		for (int j = 0; j < m_redTeamMembers.Length; j++)
		{
			using (List<RankedResolutionPlayerState>.Enumerator enumerator2 = data.UnselectedPlayerStates.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					RankedResolutionPlayerState rankedResolutionPlayerState2 = enumerator2.Current;
					if (rankedResolutionPlayerState2.PlayerId == m_redTeamMembers[j].PlayerID)
					{
						text += string.Format("PlayerID is {0}, PlayerName is {1}, Is On deck: {2}\n", rankedResolutionPlayerState2.PlayerId, m_redTeamMembers[j].m_playerName.text, rankedResolutionPlayerState2.OnDeckness);
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
		if (LastGameInfo != null)
		{
			bool flag = false;
			bool flag2 = false;
			int i = 0;
			while (i < m_blueTeamMembers.Length)
			{
				if (data._001D(m_blueTeamMembers[i].PlayerID))
				{
					flag = true;
					break;

				}

				i++;
			}

			for (int j = 0; j < m_redTeamMembers.Length; j++)
			{
				if (data._001D(m_redTeamMembers[j].PlayerID))
				{
					flag2 = true;
					break;
				}
			}
			bool flag3 = true;
			foreach (RankedResolutionPlayerState rankedResolutionPlayerState in data.PlayersOnDeck)
			{
				if (m_playerIDsOnDeck.ContainsKey(rankedResolutionPlayerState.PlayerId))
				{
					flag3 = false;
				}
			}
			if (m_lastDraftNotification.SubPhase == m_lastPhaseForUpdateCenter)
			{
				if (!flag3)
				{
					foreach (RankedResolutionPlayerState rankedResolutionPlayerState2 in data.PlayersOnDeck)
					{
						m_playerIDsOnDeck[rankedResolutionPlayerState2.PlayerId] = rankedResolutionPlayerState2.Intention;
					}
					goto IL_39C;
				}
			}
			if (m_playerIDsOnDeck.Count > 0)
			{
				bool flag4 = false;
				bool flag5 = false;
				for (int k = 0; k < m_blueTeamMembers.Length; k++)
				{
					if (m_playerIDsOnDeck.ContainsKey(m_blueTeamMembers[k].PlayerID))
					{
						flag4 = true;
						break;
					}
				}
				for (int l = 0; l < m_redTeamMembers.Length; l++)
				{
					if (m_playerIDsOnDeck.ContainsKey(m_redTeamMembers[l].PlayerID))
					{
						flag5 = true;
						break;
					}
				}
				if (flag5)
				{
					if (m_playerIDsOnDeck.Count == 1)
					{
						QueueCenterState(CenterNotification.RedTeamSingleSelectEnd);
					}
					else
					{
						QueueCenterState(CenterNotification.RedTeamDoubleSelectEnd);
					}
				}
				else if (flag4)
				{
					if (m_playerIDsOnDeck.Count == 1)
					{
						QueueCenterState(CenterNotification.BlueTeamSingleSelectEnd);
					}
					else
					{
						QueueCenterState(CenterNotification.BlueTeamDoubleSelectEnd);
					}
				}
			}
			if (!m_lastDraftNotification.SubPhase.IsPickBanSubPhase())
			{
				if (m_lastDraftNotification.SubPhase.IsPickFreelancerSubPhase())
				{
				}
				else
				{
					if (m_lastDraftNotification.SubPhase == FreelancerResolutionPhaseSubType.FREELANCER_TRADE)
					{
						QueueCenterState(CenterNotification.TradePhase);
					}
					goto IL_2E7;
				}
			}
			CenterNotification centerNotification = CenterNotification.None;
			int count = data.PlayersOnDeck.Count;
			if (flag2)
			{
				QueueCenterState(CenterNotification.RedTeamNotification);
				if (count == 1)
				{
					centerNotification = CenterNotification.RedTeamSingleSelectStart;
				}
				else if (count == 2)
				{
					centerNotification = CenterNotification.RedTeamDoubleSelectStart;
				}
			}
			else if (flag)
			{
				QueueCenterState(CenterNotification.BlueTeamNotification);
				if (count == 1)
				{
					centerNotification = CenterNotification.BlueTeamSingleSelectStart;
				}
				else if (count == 2)
				{
					centerNotification = CenterNotification.BlueTeamDoubleSelectStart;
				}
			}
			if (centerNotification != CenterNotification.None)
			{
				QueueCenterState(centerNotification);
			}
		IL_2E7:
			m_playerIDsOnDeck.Clear();
			using (List<RankedResolutionPlayerState>.Enumerator enumerator3 = data.PlayersOnDeck.GetEnumerator())
			{
				while (enumerator3.MoveNext())
				{
					RankedResolutionPlayerState rankedResolutionPlayerState3 = enumerator3.Current;
					m_playerIDsOnDeck.Add(rankedResolutionPlayerState3.PlayerId, rankedResolutionPlayerState3.Intention);
				}
			}
		IL_39C:
			if (!updateFromGameInfoUpdate)
			{
				UpdateCenterVisuals(data, flag, flag2);
			}
			m_lastPhaseForUpdateCenter = m_lastDraftNotification.SubPhase;
		}
	}

	private void UpdateCenterVisuals(RankedResolutionPhaseData data, bool isOnBlueTeam, bool isOnRedTeam)
	{
		if (m_playerIDsOnDeck.Count == 1)
		{
			foreach (KeyValuePair<int, CharacterType> keyValuePair in m_playerIDsOnDeck)
			{
				KeyValuePair<int, CharacterType> selectedChar = keyValuePair;
				RankedResolutionPhaseData data2 = data;
				TextMeshProUGUI singleCharacterName = m_singleCharacterName;
				Image noCharacter = singleNoSelectionCharacter;
				Image browseCharacter = singleBrowseSelectionCharacter;
				Image selectedCharacter = singleSelectionCharacter;
				Animator singleSelectionCharacterSelected = m_singleSelectionCharacterSelected;
				Animator selectedCharacterNameAnimator = (!isOnBlueTeam) ? m_singleRedTeamSelectionCharacterSelected : m_singleBlueSelectionCharacterSelected;
				TextMeshProUGUI selectedCharacterText;
				if (isOnBlueTeam)
				{
					selectedCharacterText = m_singleBlueTeamSelectedCharacter;
				}
				else
				{
					selectedCharacterText = m_singleRedTeamSelectedCharacter;
				}
				SetupSelection(selectedChar, data2, singleCharacterName, noCharacter, browseCharacter, selectedCharacter, singleSelectionCharacterSelected, selectedCharacterNameAnimator, selectedCharacterText, (!isOnBlueTeam) ? m_singleRedTeamPlayerName : m_singleBlueTeamPlayerName, isOnBlueTeam, isOnRedTeam);
			}
		}
		else if (m_playerIDsOnDeck.Count == 2)
		{
			bool flag = true;
			using (Dictionary<int, CharacterType>.Enumerator enumerator2 = m_playerIDsOnDeck.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					KeyValuePair<int, CharacterType> keyValuePair2 = enumerator2.Current;
					if (flag)
					{
						KeyValuePair<int, CharacterType> selectedChar2 = keyValuePair2;
						RankedResolutionPhaseData data3 = data;
						TextMeshProUGUI leftCharacterName = m_leftCharacterName;
						Image noCharacter2 = doubleNoSelectionLeftCharacter;
						Image browseCharacter2 = doubleBrowseSelectionLeftCharacter;
						Image selectedCharacter2 = doubleSelectionLeftCharacter;
						Animator selectedCharacterAnimator;
						if (m_playerIDsOnDeck.Count == data.PlayersOnDeck.Count)
						{
							selectedCharacterAnimator = m_doubleLeftSelectionCharacterSelected;
						}
						else
						{
							selectedCharacterAnimator = null;
						}
						Animator selectedCharacterNameAnimator2;
						if (isOnBlueTeam)
						{
							selectedCharacterNameAnimator2 = m_doubleLeftBlueSelectionCharacterSelected;
						}
						else
						{
							selectedCharacterNameAnimator2 = m_doubleLeftRedTeamSelectionCharacterSelected;
						}
						TextMeshProUGUI selectedCharacterText2;
						if (isOnBlueTeam)
						{
							selectedCharacterText2 = m_doubleLeftBlueTeamSelectedCharacter;
						}
						else
						{
							selectedCharacterText2 = m_doubleLeftRedTeamSelectedCharacter;
						}
						SetupSelection(selectedChar2, data3, leftCharacterName, noCharacter2, browseCharacter2, selectedCharacter2, selectedCharacterAnimator, selectedCharacterNameAnimator2, selectedCharacterText2, (!isOnBlueTeam) ? m_doubleLeftRedTeamPlayerName : m_doubleLeftBlueTeamPlayerName, isOnBlueTeam, isOnRedTeam);
						flag = false;
					}
					else
					{
						KeyValuePair<int, CharacterType> selectedChar3 = keyValuePair2;
						RankedResolutionPhaseData data4 = data;
						TextMeshProUGUI rightCharacterName = m_rightCharacterName;
						Image noCharacter3 = doubleNoSelectionRightCharacter;
						Image browseCharacter3 = doubleBrowseSelectionRightCharacter;
						Image selectedCharacter3 = doubleSelectionRightCharacter;
						Animator selectedCharacterAnimator2;
						if (m_playerIDsOnDeck.Count == data.PlayersOnDeck.Count)
						{
							selectedCharacterAnimator2 = m_doubleRightSelectionCharacterSelected;
						}
						else
						{
							selectedCharacterAnimator2 = null;
						}
						Animator selectedCharacterNameAnimator3;
						if (isOnBlueTeam)
						{
							selectedCharacterNameAnimator3 = m_doubleRightBlueSelectionCharacterSelected;
						}
						else
						{
							selectedCharacterNameAnimator3 = m_doubleRightRedTeamSelectionCharacterSelected;
						}
						TextMeshProUGUI selectedCharacterText3;
						if (isOnBlueTeam)
						{
							selectedCharacterText3 = m_doubleRightBlueTeamSelectedCharacter;
						}
						else
						{
							selectedCharacterText3 = m_doubleRightRedTeamSelectedCharacter;
						}
						SetupSelection(selectedChar3, data4, rightCharacterName, noCharacter3, browseCharacter3, selectedCharacter3, selectedCharacterAnimator2, selectedCharacterNameAnimator3, selectedCharacterText3, (!isOnBlueTeam) ? m_doubleRightRedTeamPlayerName : m_doubleRightBlueTeamPlayerName, isOnBlueTeam, isOnRedTeam);
					}
				}
			}
		}
	}

	public void SetupFreelancerSelect(RankedResolutionPhaseData data)
	{
		if (LastGameInfo != null)
		{
			if (HoveredCharacter != m_selectedSubPhaseCharacter)
			{
				if (m_selectedSubPhaseCharacter != CharacterType.None)
				{
					if (data._001D(LastPlayerInfo.PlayerId))
					{
						if (!m_selectedCharacterTypes.Contains(m_selectedSubPhaseCharacter))
						{
							if (!IsBanned(m_selectedSubPhaseCharacter))
							{
								ClientGameManager.Get().UpdateSelectedCharacter(m_selectedSubPhaseCharacter);
								ClientGameManager.Get().SendRankedHoverClickRequest(m_selectedSubPhaseCharacter);
								m_intendedLockInBtnStatus = true;
								HoveredCharacter = m_selectedSubPhaseCharacter;
							}
						}
					}
				}
			}
		}
	}

	private bool CharacterSelectAnimIsPlaying()
	{
		if (!m_doubleLeftSelectionCharacterSelected.gameObject.activeSelf)
		{
			if (!m_doubleRightSelectionCharacterSelected.gameObject.activeSelf)
			{
				return m_singleSelectionCharacterSelected.gameObject.activeSelf;
			}
		}
		return true;
	}

	private void SetupSelection(KeyValuePair<int, CharacterType> selectedChar, RankedResolutionPhaseData data, TextMeshProUGUI nameDisplay, Image NoCharacter, Image BrowseCharacter, Image SelectedCharacter, Animator SelectedCharacterAnimator, Animator SelectedCharacterNameAnimator, TextMeshProUGUI SelectedCharacterText, TextMeshProUGUI PlayerName, bool isFriendly, bool isEnemy)
	{
		bool flag = false;
		bool flag2 = false;
		bool flag3 = true;
		if (!m_selectedCharacterTypes.Contains(selectedChar.Value))
		{
			if (!IsBanned(selectedChar.Value) && !data.FriendlyBans.Contains(selectedChar.Value))
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
			if (m_lastSetupSelectionPhaseSubType.IsPickBanSubPhase())
			{
				flag4 = data.FriendlyBans.Contains(selectedChar.Value);
			}
			else if (m_lastSetupSelectionPhaseSubType.IsPickFreelancerSubPhase())
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
					if (m_lastSetupSelectionPhaseSubType.IsPickFreelancerSubPhase())
					{
						if (m_playerIDsThatSelected.Contains(selectedChar.Key))
						{
							flag5 = false;
						}
					}
					if (flag5)
					{
						SelectedCharacterText.text = value.GetDisplayName();
						SelectedCharacter.sprite = GameWideData.Get().GetCharacterResourceLink(value).ActorDataPrefab.GetComponent<ActorData>().GetAliveHUDIcon();
						UIManager.SetGameObjectActive(NoCharacter, false);
						UIManager.SetGameObjectActive(BrowseCharacter, false);
						UIManager.SetGameObjectActive(SelectedCharacter, true);
						if (SelectedCharacterAnimator != null)
						{
							UIManager.SetGameObjectActive(SelectedCharacterAnimator, true);
							m_playerIDBeingAnimated = selectedChar.Key;
							m_animatorCurrentlyAnimating = SelectedCharacterAnimator;
						}
						UIManager.SetGameObjectActive(SelectedCharacterNameAnimator, true);
						UIManager.SetGameObjectActive(SelectedCharacterNameAnimator.transform.parent, true);
						if (m_currentState != CenterNotification.BlueTeamDoubleSelectStart)
						{
							if (m_currentState != CenterNotification.BlueTeamSingleSelectStart)
							{
								if (!m_stateQueues.Contains(CenterNotification.BlueTeamDoubleSelectStart))
								{
									if (!m_stateQueues.Contains(CenterNotification.BlueTeamSingleSelectStart))
									{
										goto IL_2CA;
									}
								}
								while (m_currentState != CenterNotification.BlueTeamDoubleSelectStart)
								{
									if (m_currentState == CenterNotification.BlueTeamSingleSelectStart)
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

									m_currentState = m_stateQueues[0];
									m_stateQueues.RemoveAt(0);
								}
								IL_2BE:
								DoQueueState(m_currentState);
							}
						}
					}
					IL_2CA:
					if (m_lastSetupSelectionPhaseSubType.IsPickFreelancerSubPhase())
					{
						if (!m_playerIDsThatSelected.Contains(selectedChar.Key))
						{
							m_playerIDsThatSelected.Add(selectedChar.Key);
							for (int i = 0; i < m_blueTeamMembers.Length; i++)
							{
								if (m_blueTeamMembers[i].PlayerID == selectedChar.Key)
								{
									flag2 = true;
									PlayerName.text = m_blueTeamMembers[i].m_playerName.text;
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
			if (m_lastSetupSelectionPhaseSubType.IsPickBanSubPhase())
			{
				flag6 = data.EnemyBans.Contains(selectedChar.Value);
			}
			else if (m_lastSetupSelectionPhaseSubType.IsPickFreelancerSubPhase())
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
					if (m_lastSetupSelectionPhaseSubType.IsPickFreelancerSubPhase())
					{
						if (m_playerIDsThatSelected.Contains(selectedChar.Key))
						{
							flag7 = false;
						}
					}
					if (flag7)
					{
						SelectedCharacterText.text = value2.GetDisplayName();
						SelectedCharacter.sprite = GameWideData.Get().GetCharacterResourceLink(value2).ActorDataPrefab.GetComponent<ActorData>().GetAliveHUDIcon();
						UIManager.SetGameObjectActive(NoCharacter, false);
						UIManager.SetGameObjectActive(BrowseCharacter, false);
						UIManager.SetGameObjectActive(SelectedCharacter, true);
						if (SelectedCharacterAnimator != null)
						{
							UIManager.SetGameObjectActive(SelectedCharacterAnimator, true);
							m_playerIDBeingAnimated = selectedChar.Key;
							m_animatorCurrentlyAnimating = SelectedCharacterAnimator;
						}
						UIManager.SetGameObjectActive(SelectedCharacterNameAnimator, true);
						UIManager.SetGameObjectActive(SelectedCharacterNameAnimator.transform.parent, true);
						if (m_currentState != CenterNotification.RedTeamDoubleSelectStart)
						{
							if (m_currentState != CenterNotification.RedTeamSingleSelectStart)
							{
								if (!m_stateQueues.Contains(CenterNotification.RedTeamDoubleSelectStart))
								{
									if (!m_stateQueues.Contains(CenterNotification.RedTeamSingleSelectStart))
									{
										goto IL_5A8;
									}
								}
								while (m_currentState != CenterNotification.RedTeamDoubleSelectStart)
								{
									if (m_currentState == CenterNotification.RedTeamSingleSelectStart)
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

									m_currentState = m_stateQueues[0];
									m_stateQueues.RemoveAt(0);
								}
								IL_59C:
								DoQueueState(m_currentState);
							}
						}
					}
					IL_5A8:
					if (m_lastSetupSelectionPhaseSubType.IsPickFreelancerSubPhase() && !m_playerIDsThatSelected.Contains(selectedChar.Key))
					{
						m_playerIDsThatSelected.Add(selectedChar.Key);
					}
				}
			}
		}
		if (!flag2)
		{
			bool flag8 = true;
			if (m_playerIDsThatSelected.Count > 0 && m_playerIDsThatSelected[m_playerIDsThatSelected.Count - 1] == selectedChar.Key)
			{
				flag8 = false;
			}
			if (m_animatorCurrentlyAnimating != null)
			{
				if (m_animatorCurrentlyAnimating.gameObject.activeInHierarchy)
				{
					if (m_lastSetupSelectionPhaseSubType == m_lastDraftNotification.SubPhase)
					{
						for (int j = 0; j < m_blueTeamMembers.Length; j++)
						{
							if (m_blueTeamMembers[j].PlayerID == m_playerIDBeingAnimated)
							{
								flag8 = false;
								PlayerName.text = m_blueTeamMembers[j].m_playerName.text;
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
		m_lastSetupSelectionPhaseSubType = m_lastDraftNotification.SubPhase;
		if (!flag && !CharacterSelectAnimIsPlaying())
		{
			if (selectedChar.Value != CharacterType.None && flag3)
			{
				BrowseCharacter.sprite = GameWideData.Get().GetCharacterResourceLink(selectedChar.Value).ActorDataPrefab.GetComponent<ActorData>().GetAliveHUDIcon();
				UIManager.SetGameObjectActive(NoCharacter, false);
				UIManager.SetGameObjectActive(BrowseCharacter, true);
				UIManager.SetGameObjectActive(SelectedCharacter, false);
			}
			else
			{
				UIManager.SetGameObjectActive(NoCharacter, true);
				UIManager.SetGameObjectActive(BrowseCharacter, false);
				UIManager.SetGameObjectActive(SelectedCharacter, false);
			}
		}
		if (!flag)
		{
			if (!flag3 && !CharacterSelectAnimIsPlaying())
			{
				UIManager.SetGameObjectActive(NoCharacter, true);
				UIManager.SetGameObjectActive(BrowseCharacter, false);
				UIManager.SetGameObjectActive(SelectedCharacter, false);
			}
		}
		nameDisplay.text = string.Empty;
	}

	public void SetupBanSelect(RankedResolutionPhaseData data)
	{
		if (LastGameInfo != null)
		{
			Team currentTeam = GetCurrentTeam(data);
			for (int i = 0; i < m_blueBans.Length; i++)
			{
				UIManager.SetGameObjectActive(m_blueBans[i], true);
			}
			for (int j = 0; j < m_redBans.Length; j++)
			{
				UIManager.SetGameObjectActive(m_redBans[j], true);
			}
			if (data.PlayersOnDeck.Count > 0)
			{
				foreach (RankedResolutionPlayerState rankedResolutionPlayerState in data.PlayersOnDeck)
				{
					if (rankedResolutionPlayerState.Intention != CharacterType.None)
					{
						CharacterResourceLink characterResourceLink = GameWideData.Get().GetCharacterResourceLink(rankedResolutionPlayerState.Intention);
						if (GetCurrentTeam(data) == LastPlayerInfo.TeamId)
						{
							goto IL_106;
						}
						if (LastPlayerInfo.TeamId == Team.Spectator)
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

						if (data.EnemyBans.Count < m_redBans.Length)
						{
							m_redBans[data.EnemyBans.Count].SetBrowseCharacterImageVisible(true);
							m_redBans[data.EnemyBans.Count].SetHoverCharacter(characterResourceLink);
						}
						continue;
						IL_106:
						if (data.FriendlyBans.Count < m_blueBans.Length)
						{
							m_blueBans[data.FriendlyBans.Count].SetBrowseCharacterImageVisible(true);
							m_blueBans[data.FriendlyBans.Count].SetHoverCharacter(characterResourceLink);
						}
					}
					else
					{
						if (data.FriendlyBans.Count < m_blueBans.Length)
						{
							m_blueBans[data.FriendlyBans.Count].SetBrowseCharacterImageVisible(false);
						}
						if (data.EnemyBans.Count < m_redBans.Length)
						{
							m_redBans[data.EnemyBans.Count].SetBrowseCharacterImageVisible(false);
						}
					}
				}
			}
			int k = 0;
			while (k < m_redBans.Length)
			{
				m_redBans[k].SetSelectedCharacterImageVisible(data.EnemyBans.Count > k);
				if (currentTeam == LastPlayerInfo.TeamId)
				{
					goto IL_29C;
				}
				if (LastPlayerInfo.TeamId == Team.Spectator)
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

				m_redBans[k].SetAsSelecting(data.EnemyBans.Count == k);
				IL_2CD:
				k++;
				continue;
				IL_29C:
				m_redBans[k].SetAsSelecting(false);
				goto IL_2CD;
			}
			int l = 0;
			while (l < m_blueBans.Length)
			{
				m_blueBans[l].SetSelectedCharacterImageVisible(data.FriendlyBans.Count > l);
				if (currentTeam == LastPlayerInfo.TeamId)
				{
					goto IL_338;
				}
				if (LastPlayerInfo.TeamId == Team.Spectator)
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

				m_blueBans[l].SetAsSelecting(false);
				IL_367:
				l++;
				continue;
				IL_338:
				m_blueBans[l].SetAsSelecting(data.FriendlyBans.Count == l);
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
			foreach (UIRankedModePlayerDraftEntry uirankedModePlayerDraftEntry in m_blueTeamMembers)
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
		Team currentTeam = GetCurrentTeam(data);
		TeamType teamType = TeamType.Any;
		if (currentTeam != Team.TeamA)
		{
			if (currentTeam != Team.TeamB)
			{
				m_MessageText.color = m_neutralColor;
				goto IL_9E;
			}
		}
		if (currentTeam != LastPlayerInfo.TeamId)
		{
			if (LastPlayerInfo.TeamId == Team.Spectator)
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
			m_MessageText.color = m_redTeamColor;
			teamType = TeamType.Enemy;
			goto IL_9E;
		}
		IL_76:
		m_MessageText.color = m_blueTeamColor;
		teamType = TeamType.Ally;
		IL_9E:
		if (LastGameInfo != null && LastGameInfo.GameStatus != GameStatus.Stopped)
		{
			if (LastGameInfo.GameStatus > GameStatus.FreelancerSelecting)
			{
				return;
			}
		}
		m_MessageText.text = SubphaseToDisplayName(m_lastDraftNotification.SubPhase, teamType, data._001D(OurPlayerId));
	}

	public void NotifyFreelancerTrades(RankedResolutionPhaseData data)
	{
		long accountId = ClientGameManager.Get().GetPlayerAccountData().AccountId;
		int num = -1;
		bool selfLockedIn = false;
		for (int i = 0; i < m_blueTeamMembers.Length; i++)
		{
			if (m_blueTeamMembers[i].AccountID == accountId)
			{
				num = m_blueTeamMembers[i].PlayerID;
				selfLockedIn = DidPlayerLockInDuringSwapPhase(data, m_blueTeamMembers[i].PlayerID);
			}
			m_blueTeamMembers[i].SetAsSelecting(false);
		}
		for (int j = 0; j < m_redTeamMembers.Length; j++)
		{
			m_redTeamMembers[j].SetAsSelecting(false);
		}
		for (int k = 0; k < m_blueTeamMembers.Length; k++)
		{
			UIRankedModePlayerDraftEntry.TradeStatus status = UIRankedModePlayerDraftEntry.TradeStatus.NoTrade;
			using (List<RankedTradeData>.Enumerator enumerator = data.TradeActions.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					RankedTradeData rankedTradeData = enumerator.Current;
					if (rankedTradeData.TradeAction == RankedTradeData.TradeActionType.AcceptOrOffer)
					{
						if (rankedTradeData.AskedPlayerId == m_blueTeamMembers[k].PlayerID)
						{
							if (rankedTradeData.OfferingPlayerId == num)
							{
								status = UIRankedModePlayerDraftEntry.TradeStatus.TradeRequestSent;
								goto IL_1F4;
							}
						}
						if (rankedTradeData.AskedPlayerId == num)
						{
							if (rankedTradeData.OfferingPlayerId == m_blueTeamMembers[k].PlayerID)
							{
								status = UIRankedModePlayerDraftEntry.TradeStatus.TradeRequestReceived;
								goto IL_1F4;
							}
						}
						continue;
					}
					if (rankedTradeData.TradeAction != RankedTradeData.TradeActionType.StopTrading)
					{
						continue;
					}
					if (rankedTradeData.OfferingPlayerId != m_blueTeamMembers[k].PlayerID)
					{
						if (rankedTradeData.AskedPlayerId != m_blueTeamMembers[k].PlayerID)
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
			m_blueTeamMembers[k].SetTradeStatus(status, m_blueTeamMembers[k].PlayerID == num, selfLockedIn);
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
		GameIsLaunching = true;
		m_MessageText.text = GameStatusToDisplayString(notification.GameInfo.GameStatus);
		for (int i = 0; i < m_blueTeamMembers.Length; i++)
		{
			m_blueTeamMembers[i].SetTradePhase(false);
		}
		UIManager.SetGameObjectActive(m_lockFreelancerContainer, false);
		if (notification.GameInfo.GameStatus >= GameStatus.Launching && notification.GameInfo.GameStatus != GameStatus.Stopped)
		{
			SetFreelancerSettingButtonsVisible(false);
			UIRankedCharacterSelectSettingsPanel.Get().SetVisible(false);
		}
	}

	public void UpdateNotification(EnterFreelancerResolutionPhaseNotification notification, bool updateFromGameInfoUpdate = false)
	{
		if (GameIsLaunching)
		{
			return;
		}
		if (notification != null)
		{
			if (notification.RankedData != null)
			{
				RankedResolutionPhaseData value = notification.RankedData.Value;
				SetupInstructions(value);
				UpdateRankData(value, updateFromGameInfoUpdate);
				UpdatePlayerSelecting(value);
				switch (notification.SubPhase)
				{
				case FreelancerResolutionPhaseSubType.PICK_BANS1:
				case FreelancerResolutionPhaseSubType.PICK_BANS2:
					SetupBanSelect(value);
					break;
				case FreelancerResolutionPhaseSubType.PICK_FREELANCER1:
				case FreelancerResolutionPhaseSubType.PICK_FREELANCER2:
					SetupFreelancerSelect(value);
					break;
				case FreelancerResolutionPhaseSubType.FREELANCER_TRADE:
					NotifyFreelancerTrades(value);
					break;
				}
				UpdateHoverStatus(value);
				UpdateCenter(value, updateFromGameInfoUpdate);
			}
		}
	}

	public void NotifyButtonClicked(UICharacterPanelSelectRankModeButton btn)
	{
		if (m_IsOnDeck)
		{
			bool intendedLockInBtnStatus = false;
			m_selectedSubPhaseCharacter = CharacterType.None;
			for (int i = 0; i < m_characterListDisplayButtons.Count; i++)
			{
				if (m_characterListDisplayButtons[i] == btn)
				{
					m_selectedSubPhaseCharacter = m_characterListDisplayButtons[i].m_characterType;
					m_characterListDisplayButtons[i].SetSelected(true);
					if (!m_selectedCharacterTypes.Contains(m_characterListDisplayButtons[i].m_characterType))
					{
						if (!IsBanned(m_characterListDisplayButtons[i].m_characterType))
						{
							HoveredCharacter = m_selectedSubPhaseCharacter;
							RankedResolutionPhaseData? rankedData = m_lastDraftNotification.RankedData;
							RankedResolutionPhaseData value = rankedData.Value;
							if (m_lastDraftNotification.SubPhase.IsPickFreelancerSubPhase())
							{
								UpdateHoverSelfStatus(value);
							}
							if (!m_lastDraftNotification.SubPhase.IsPickBanSubPhase())
							{
								ClientGameManager.Get().UpdateSelectedCharacter(m_selectedSubPhaseCharacter);
							}
							ClientGameManager.Get().SendRankedHoverClickRequest(m_selectedSubPhaseCharacter);
							intendedLockInBtnStatus = true;
						}
					}
				}
				else
				{
					m_characterListDisplayButtons[i].SetSelected(false);
				}
			}
			m_intendedLockInBtnStatus = intendedLockInBtnStatus;
		}
		else
		{
			m_selectedSubPhaseCharacter = CharacterType.None;
			for (int j = 0; j < m_characterListDisplayButtons.Count; j++)
			{
				if (m_characterListDisplayButtons[j] == btn)
				{
					m_selectedSubPhaseCharacter = m_characterListDisplayButtons[j].m_characterType;
					m_characterListDisplayButtons[j].SetSelected(true);
					if (!m_selectedCharacterTypes.Contains(m_characterListDisplayButtons[j].m_characterType))
					{
						if (!IsBanned(m_characterListDisplayButtons[j].m_characterType))
						{
							ClientGameManager.Get().UpdateSelectedCharacter(m_selectedSubPhaseCharacter);
							ClientGameManager.Get().SendRankedHoverClickRequest(m_selectedSubPhaseCharacter);
							SetupCharacterSettings(m_selectedSubPhaseCharacter);
							SetFreelancerSettingButtonsVisible(m_currentState >= CenterNotification.LoadoutPhase);
						}
					}
				}
				else
				{
					m_characterListDisplayButtons[j].SetSelected(false);
				}
			}
		}
	}

	public void HandleResolvingDuplicateFreelancerNotification(EnterFreelancerResolutionPhaseNotification notification)
	{
		Initialize();
		m_lastDraftNotification = notification;
		SetupPlayerLists();
		UpdateNotification(notification);
	}

	private void GetListOfVisibleCharacterTypes()
	{
		m_validCharacterTypes.Clear();
		GameManager gameManager = GameManager.Get();
		for (int i = 0; i < 0x28; i++)
		{
			CharacterType characterType = (CharacterType)i;
			if (gameManager.IsCharacterAllowedForPlayers(characterType))
			{
				if (gameManager.IsCharacterAllowedForGameType(characterType, GameType.Ranked, null, null))
				{
					m_validCharacterTypes.Add(characterType);
				}
			}
		}
	}

	private void Initialize()
	{
		if (m_initialized)
		{
			return;
		}
		GameIsLaunching = false;
		m_lastSetupSelectionPhaseSubType = FreelancerResolutionPhaseSubType.UNDEFINED;
		m_assignedCharacterForGame = CharacterType.None;
		m_hoverCharacterForGame = CharacterType.None;
		m_selectedSubPhaseCharacter = CharacterType.None;
		UICharacterSelectCharacterSettingsPanel uicharacterSelectCharacterSettingsPanel = UIRankedCharacterSelectSettingsPanel.Get();
		if (uicharacterSelectCharacterSettingsPanel != null)
		{
			uicharacterSelectCharacterSettingsPanel.SetVisible(false);
		}
		m_selectedCharacterTypes.Clear();
		m_enemyBannedCharacterTypes.Clear();
		m_friendlyBannedCharacterTypes.Clear();
		m_playerIDsThatSelected.Clear();
		GetListOfVisibleCharacterTypes();
		m_initialized = true;
		m_MessageText.text = string.Empty;
		m_gameCountdownTimer.text = string.Empty;
		m_redCountdownTimer.text = string.Empty;
		m_blueCountdownTimer.text = string.Empty;
		m_stageText.text = string.Empty;
		ClearAllStates();
		UIManager.SetGameObjectActive(m_pagesContainer, false);
		UIManager.SetGameObjectActive(m_lockFreelancerContainer, false);
		m_intendedLockInBtnStatus = false;
		m_currentCharacterPage = -1;
		m_currentVisiblePage = -1;
		foreach (UIRankedModeDraftCharacterEntry blueBan in m_blueBans)
		{
			UIManager.SetGameObjectActive(blueBan, false);
			blueBan.Init();
		}
		foreach (UIRankedModeDraftCharacterEntry redBan in m_redBans)
		{
			UIManager.SetGameObjectActive(redBan, false);
			redBan.Init();
		}
		foreach (UIRankedModePlayerDraftEntry blueTeamMember in m_blueTeamMembers)
		{
			blueTeamMember.Init();
			blueTeamMember.SetTradePhase(false);
		}
		foreach (UIRankedModePlayerDraftEntry redTeamMember in m_redTeamMembers)
		{
			redTeamMember.Init();
			redTeamMember.SetTradePhase(false);
		}
		foreach (UICharacterPanelSelectRankModeButton btn in m_characterListDisplayButtons)
		{
			Destroy(btn.gameObject);
		}
		m_pageButtons.Clear();
		m_characterListDisplayButtons.Clear();
		CharacterType[] allCharacters = (CharacterType[])Enum.GetValues(typeof(CharacterType));
		List<CharacterType> listAssassins = new List<CharacterType>();
		List<CharacterType> listTanks = new List<CharacterType>();
		List<CharacterType> listSupports = new List<CharacterType>();
		foreach (CharacterType characterType in allCharacters)
		{
			try
			{
				if (characterType == CharacterType.TestFreelancer1
				    || characterType == CharacterType.TestFreelancer2
				    || characterType == CharacterType.None)
				{
					continue;
				}
				
				CharacterResourceLink characterResourceLink = GameWideData.Get().GetCharacterResourceLink(characterType);
				switch (characterResourceLink.m_characterRole)
				{
					case CharacterRole.Assassin:
						listAssassins.Add(characterType);
						break;
					case CharacterRole.Support:
						listSupports.Add(characterType);
						break;
					case CharacterRole.Tank:
						listTanks.Add(characterType);
						break;
				}
			}
			catch
			{
			}
		}
		
		listAssassins.Sort(CompareCharacterTypeName);
		listTanks.Sort(CompareCharacterTypeName);
		listSupports.Sort(CompareCharacterTypeName);
		
		int numAssassinsPerRow = Mathf.CeilToInt(listAssassins.Count / 2f);
		int numTanksPerRow = Mathf.CeilToInt(listTanks.Count / 2f);
		int numSupportsPerRow = Mathf.CeilToInt(listSupports.Count / 2f);
		int currentRow = 0;
		for (int i = 0; i < listAssassins.Count; i++)
		{
			UICharacterPanelSelectRankModeButton btn = Instantiate(m_characterSelectBtnPrefab) as UICharacterPanelSelectRankModeButton;
			btn.m_characterType = listAssassins[i];
			if (i - currentRow * numAssassinsPerRow >= numAssassinsPerRow)
			{
				currentRow++;
			}
			UIManager.ReparentTransform(btn.gameObject.transform, m_firePowerLayoutGroup.gameObject.transform);
			m_characterListDisplayButtons.Add(btn);
		}
		currentRow = 0;
		for (int i = 0; i < listTanks.Count; i++)
		{
			UICharacterPanelSelectRankModeButton btn = Instantiate(m_characterSelectBtnPrefab) as UICharacterPanelSelectRankModeButton;
			btn.m_characterType = listTanks[i];
			if (i - currentRow * numTanksPerRow >= numTanksPerRow)
			{
				currentRow++;
			}
			UIManager.ReparentTransform(btn.gameObject.transform, m_frontlinerLayoutGroup.gameObject.transform);
			m_characterListDisplayButtons.Add(btn);
		}
		currentRow = 0;
		for (int i = 0; i < listSupports.Count; i++)
		{
			UICharacterPanelSelectRankModeButton btn = Instantiate(m_characterSelectBtnPrefab) as UICharacterPanelSelectRankModeButton;
			btn.m_characterType = listSupports[i];
			if (i - currentRow * numSupportsPerRow >= numSupportsPerRow)
			{
				currentRow++;
			}
			UIManager.ReparentTransform(btn.gameObject.transform, m_supportLayoutGroup.gameObject.transform);
			m_characterListDisplayButtons.Add(btn);
		}
		SetPageIndex(0);
		UIManager.SetGameObjectActive(m_introContainer, true);
		m_draftScreenContainer.GetComponent<CanvasGroup>().alpha = 1f;
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
		btn.spriteController.callback = PageClicked;
	}

	private void PageClicked(BaseEventData data)
	{
		for (int i = 0; i < m_pageButtons.Count; i++)
		{
			if (m_pageButtons[i].spriteController.m_hitBoxImage.gameObject == (data as PointerEventData).pointerCurrentRaycast.gameObject)
			{
				SetPageIndex(i);
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
		if (!m_selectedCharacterTypes.Contains(type))
		{
			if (!IsBanned(type))
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
		bool isPickingBans = m_lastDraftNotification != null && m_lastDraftNotification.SubPhase.IsPickBanSubPhase();
		foreach (UICharacterPanelSelectRankModeButton btn in m_characterListDisplayButtons)
		{
			CharacterType characterType = btn.m_characterType;
			bool isAvailableForPlayer = IsCharacterAvailableForPlayer(characterType);
			bool isAvailable = IsCharacterTypeSelectable(characterType) && (isAvailableForPlayer || isPickingBans);
			if (IsCharacterVisibleForPlayer(characterType))
			{
				btn.SetEnabled(isAvailable, ClientGameManager.Get().GetPlayerCharacterData(characterType));
				btn.SetSelected(
					HoveredCharacter == btn.m_characterType
					|| SelectedCharacter == btn.m_characterType
					|| m_selectedSubPhaseCharacter == btn.m_characterType);
				UIManager.SetGameObjectActive(btn, true);
			}
			else
			{
				UIManager.SetGameObjectActive(btn, false);
			}
		}
	}

	private void UpdateCharacterButtons()
	{
		m_currentVisiblePage = m_currentCharacterPage;
		bool isPickingBans = m_lastDraftNotification != null && m_lastDraftNotification.SubPhase.IsPickBanSubPhase();
		foreach (UICharacterPanelSelectRankModeButton btn in m_characterListDisplayButtons)
		{
			CharacterType characterType = btn.m_characterType;
			bool isAvailableForPlayer = IsCharacterAvailableForPlayer(characterType);
			bool isAvailable = IsCharacterTypeSelectable(characterType) && (isAvailableForPlayer || isPickingBans);
			if (IsCharacterVisibleForPlayer(characterType))
			{
				bool selected = HoveredCharacter == btn.m_characterType
				                || SelectedCharacter == btn.m_characterType
				                || m_selectedSubPhaseCharacter == btn.m_characterType;
				btn.Setup(isAvailable, selected);
				UIManager.SetGameObjectActive(btn, true);
			}
			else
			{
				UIManager.SetGameObjectActive(btn, false);
			}
		}
	}

	private void SetPageIndex(int index)
	{
		UpdateCharacterButtons();
		if (!IsVisible)
		{
			return;
		}
		Initialize();
	}

	public void DismantleRankDraft()
	{
		m_assignedCharacterForGame = CharacterType.None;
		m_hoverCharacterForGame = CharacterType.None;
		m_selectedSubPhaseCharacter = CharacterType.None;
		m_lastSetupSelectionPhaseSubType = FreelancerResolutionPhaseSubType.UNDEFINED;
		IsVisible = false;
		m_initialized = false;
		LastGameInfo = null;
		LastPlayerInfo = null;
		LastTeamInfo = null;
		m_lastDraftNotification = null;
		m_currentState = CenterNotification.None;
		m_stateQueues.Clear();
		m_playerIDsOnDeck.Clear();
		ClearAllStates();
		UIManager.SetGameObjectActive(UIFrontEnd.Get().m_frontEndNavPanel, true);
		UIManager.SetGameObjectActive(m_draftScreenContainer, false);
		UIRankedCharacterSelectSettingsPanel.Get().SetVisible(false);
		UIManager.SetGameObjectActive(m_singleSelectionCharacterSelected, false);
		UIManager.SetGameObjectActive(m_doubleRightSelectionCharacterSelected, false);
		UIManager.SetGameObjectActive(m_doubleLeftSelectionCharacterSelected, false);
		for (int i = 0; i < m_blueTeamMembers.Length; i++)
		{
			m_blueTeamMembers[i].Dismantle();
		}
		for (int j = 0; j < m_redTeamMembers.Length; j++)
		{
			m_redTeamMembers[j].Dismantle();
		}
	}

	public void SetupRankDraft()
	{
		UIPlayerProgressPanel.Get().SetVisible(false);
		IsVisible = true;
		UIFrontEnd.Get().m_frontEndNavPanel.SetNavButtonSelected(UIFrontEnd.Get().m_frontEndNavPanel.m_PlayBtn);
		UIStorePanel.Get().ClosePurchaseDialog();
		UIRankedModeSelectScreen.Get().SetVisible(false);
		UIManager.SetGameObjectActive(UIFrontEnd.Get().m_frontEndNavPanel, false);
		UIManager.SetGameObjectActive(m_draftScreenContainer, true);
		UIRAFProgramScreen.Get().SetVisible(false);
		Initialize();
	}

	public void SetDraftScreenVisible(bool visible)
	{
		if (!IsVisible)
		{
			return;
		}
		UIManager.SetGameObjectActive(m_draftScreenContainer, visible);
		if (visible)
		{
			if (m_containerAC == null)
			{
				m_containerAC = m_draftScreenContainer.GetComponent<Animator>();
			}
			if (m_containerAC != null)
			{
				m_containerAC.Play("RankedModeSetup", 0, 1f);
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

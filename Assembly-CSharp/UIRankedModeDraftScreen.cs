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

    private List<UICharacterPanelSelectRankModeButton> m_characterListDisplayButtons =
        new List<UICharacterPanelSelectRankModeButton>();
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
        UITooltipObject tooltipHoverObject =
            m_notOnAFactionFilter.m_btn.spriteController.GetComponent<UITooltipHoverObject>();
        TooltipType tooltipType = TooltipType.Simple;

        tooltipHoverObject.Setup(
            tooltipType,
            delegate(UITooltipBase tooltip)
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
            m_lastFilterBtnClicked.m_btn.SetSelected(false);
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
        UIManager.SetGameObjectActive(
            m_blueTeamTurnNotification,
            notification == CenterNotification.BlueTeamNotification);
        UIManager.SetGameObjectActive(
            m_redTeamTurnNotification,
            notification == CenterNotification.RedTeamNotification);
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
        switch (notification)
        {
            case CenterNotification.None:
                return;
            case CenterNotification.LoadoutPhase:
            case CenterNotification.GameLoadPhase:
                ClearAllStates();
                m_stateQueues.Clear();
                break;
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
        if (m_lastDraftNotification != null
            && !GameIsLaunching
            && m_lastDraftNotification.RankedData != null)
        {
            if (UICharacterSelectWorldObjects.Get().IsVisible())
            {
                UICharacterSelectWorldObjects.Get().SetVisible(false);
            }

            if (Time.time - m_phaseStartTime < m_timeInPhase.TotalSeconds)
            {
                float countdownTimerFloat = (float)m_timeInPhase.TotalSeconds - Time.time + m_phaseStartTime;
                int countdownTimer = Mathf.RoundToInt(countdownTimerFloat);
                RankedResolutionPhaseData value = m_lastDraftNotification.RankedData.Value;
                Team currentTeam = GetCurrentTeam(value);
                if (currentTeam != Team.TeamA && currentTeam != Team.TeamB)
                {
                    if (m_gameCountdownTimer.text != countdownTimer.ToString())
                    {
                        m_gameCountdownTimer.text = countdownTimer.ToString();
                        m_gameCountdownAC.Play("RankedNumberTextCountdown", 1, 0f);
                    }

                    m_redCountdownTimer.text = string.Empty;
                    m_blueCountdownTimer.text = string.Empty;
                }
                else if (currentTeam == LastPlayerInfo.TeamId)
                {
                    m_gameCountdownTimer.text = string.Empty;
                    m_redCountdownTimer.text = string.Empty;
                    if (m_blueCountdownTimer.text != countdownTimer.ToString())
                    {
                        m_blueCountdownTimer.text = countdownTimer.ToString();
                        m_blueCountdownAC.Play("RankedNumberTextCountdown", 1, 0f);
                    }
                }
                else
                {
                    m_gameCountdownTimer.text = string.Empty;
                    if (m_redCountdownTimer.text != countdownTimer.ToString())
                    {
                        m_redCountdownTimer.text = countdownTimer.ToString();
                        m_redCountdownAC.Play("RankedNumberTextCountdown", 1, 0f);
                    }

                    m_blueCountdownTimer.text = string.Empty;
                }

                if (countdownTimer <= 5 && Mathf.RoundToInt(countdownTimerFloat + Time.deltaTime) != countdownTimer)
                {
                    UIFrontEnd.PlaySound(FrontEndButtonSounds.RankModeTimerTick);
                }
            }

            if (SelectedCharacter != CharacterType.None && SelectedCharacter != ClientClickedCharacter)
            {
                int playerId = GameManager.Get().PlayerInfo.PlayerId;
                if (!m_lastDraftNotification.RankedData.Value._001D(playerId))
                {
                    SetupCharacterSettings(SelectedCharacter);
                }
            }
        }

        if (GameIsLaunching)
        {
            m_gameCountdownTimer.text = string.Empty;
            if (LastGameInfo != null && LastGameInfo.GameStatus == GameStatus.LoadoutSelecting)
            {
                float countdownTimerFloat = Mathf.Max(
                    0f,
                    (float)LastGameInfo.LoadoutSelectTimeout.TotalSeconds
                    - (Time.realtimeSinceStartup - m_loadoutSelectStartTime));
                int countdownTimer = Mathf.RoundToInt(countdownTimerFloat);
                if (m_gameCountdownTimer.text != countdownTimer.ToString())
                {
                    m_gameCountdownTimer.text = countdownTimer.ToString();
                    m_gameCountdownAC.Play("RankedNumberTextCountdown", 1, 0f);
                }

                if (countdownTimer < 6 && Mathf.RoundToInt(countdownTimerFloat + Time.deltaTime) != countdownTimer)
                {
                    UIFrontEnd.PlaySound(FrontEndButtonSounds.RankModeTimerTick);
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

        if (!m_introContainer.gameObject.activeSelf
            && (!IsAnyCenterStateActive() || IsDoubleSelectinReadyToAdvance())
            && m_stateQueues.Count > 0)
        {
            DoQueueState(m_stateQueues[0]);
            m_currentState = m_stateQueues[0];
            m_stateQueues.RemoveAt(0);
        }

        if (m_characterSelectContainerCanvasGroup == null)
        {
            m_characterSelectContainerCanvasGroup = m_characterSelectContainer.GetComponent<CanvasGroup>();
        }

        if (m_journeyLength > 0f || m_currentCharacterPage != m_currentVisiblePage)
        {
            float share = (Time.time - m_startTime) * m_timeForPageToSwap;
            float alpha = share / m_journeyLength;
            Vector2 interpolatedPos = Vector2.Lerp(m_startLocation, m_endLocation, alpha);
            if (!float.IsNaN(interpolatedPos.x) && !float.IsNaN(interpolatedPos.y))
            {
                (m_characterSelectContainer.transform as RectTransform).anchoredPosition = interpolatedPos;
                if (m_characterSelectContainerCanvasGroup != null)
                {
                    m_characterSelectContainerCanvasGroup.alpha = m_currentCharacterPage != m_currentVisiblePage
                        ? 1f - alpha
                        : alpha;
                }

                if (alpha >= 1f)
                {
                    if (m_characterSelectContainerCanvasGroup.alpha <= 0f)
                    {
                        Vector2 anchoredPosition = (m_characterSelectContainer.gameObject.transform as RectTransform)
                            .anchoredPosition;
                        if (m_endLocation.x < 0f)
                        {
                            (m_characterSelectContainer.gameObject.transform as RectTransform).anchoredPosition =
                                new Vector2(anchoredPosition.x * -1f, anchoredPosition.y);
                            m_startTime = Time.time;
                            m_startLocation = anchoredPosition;
                            m_endLocation = new Vector2(0f, anchoredPosition.y);
                            m_journeyLength = Vector2.Distance(m_startLocation, m_endLocation);
                        }
                        else if (m_endLocation.x > 0f)
                        {
                            (m_characterSelectContainer.gameObject.transform as RectTransform).anchoredPosition =
                                new Vector2(anchoredPosition.x * -1f, anchoredPosition.y);
                            m_startTime = Time.time;
                            m_startLocation = anchoredPosition;
                            m_endLocation = new Vector2(0f, anchoredPosition.y);
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

        if (IsCenterSelectAnimating())
        {
            UIManager.SetGameObjectActive(m_lockInBtn, false);
        }
        else
        {
            string text = m_lastDraftNotification != null && m_lastDraftNotification.SubPhase.IsPickBanSubPhase()
                ? StringUtil.TR("Ban", "OverlayScreensScene")
                : StringUtil.TR("LockIn", "OverlayScreensScene");
            foreach (TextMeshProUGUI textMesh in m_lockInText)
            {
                textMesh.text = text;
            }

            UIManager.SetGameObjectActive(m_lockInBtn, m_intendedLockInBtnStatus);
            m_lockInBtn.SetDisabled(m_selectedSubPhaseCharacter == SelectedCharacter);
        }

        if (m_containerAC == null)
        {
            m_containerAC = m_draftScreenContainer.GetComponent<Animator>();
        }

        if (m_containerAC != null
            && m_containerAC.gameObject.activeInHierarchy
            && m_containerAC.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            DoCharacterSelectContainerActiveCheck();
        }

        UIManager.SetGameObjectActive(m_searchFiltersContainer, m_characterSelectContainer.gameObject.activeSelf);
    }

    private bool IsCenterSelectAnimating()
    {
        return m_singleSelectionCharacterSelected.gameObject.activeInHierarchy
               || m_doubleRightSelectionCharacterSelected.gameObject.activeInHierarchy
               && doubleSelectionLeftCharacter.gameObject.activeInHierarchy
               || m_doubleLeftSelectionCharacterSelected.gameObject.activeInHierarchy
               && doubleSelectionRightCharacter.gameObject.activeInHierarchy;
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
        if (m_selectedSubPhaseCharacter != CharacterType.None && SelectedCharacter != m_selectedSubPhaseCharacter)
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

    private bool DidPlayerLockInDuringSwapPhase(RankedResolutionPhaseData data, long playerID)
    {
        if (data.TradeActions == null)
        {
            return false;
        }

        foreach (RankedTradeData rankedTradeData in data.TradeActions)
        {
            if (rankedTradeData.OfferingPlayerId == playerID
                && rankedTradeData.TradeAction == RankedTradeData.TradeActionType.StopTrading)
            {
                return true;
            }
        }

        return false;
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

            foreach (UIRankedModePlayerDraftEntry teammate in m_blueTeamMembers)
            {
                if (teammate.PlayerID != playerId)
                {
                    continue;
                }

                if (HoveredCharacter != CharacterType.None
                    && !m_selectedCharacterTypes.Contains(HoveredCharacter)
                    && !IsBanned(HoveredCharacter))
                {
                    CharacterResourceLink characterResourceLink =
                        GameWideData.Get().GetCharacterResourceLink(HoveredCharacter);
                    teammate.SetBrowseCharacterImageVisible(true);
                    teammate.SetHoverCharacter(characterResourceLink);
                }

                break;
            }

            switch (m_playerIDsOnDeck.Count)
            {
                case 1:
                {
                    KeyValuePair<int, CharacterType> selectedChar =
                        new KeyValuePair<int, CharacterType>(playerId, HoveredCharacter);
                    if (m_playerIDsOnDeck.ContainsKey(playerId))
                    {
                        SetupSelection(
                            selectedChar,
                            data,
                            m_singleCharacterName,
                            singleNoSelectionCharacter,
                            singleBrowseSelectionCharacter,
                            singleSelectionCharacter,
                            m_singleSelectionCharacterSelected,
                            m_singleBlueSelectionCharacterSelected,
                            m_singleBlueTeamSelectedCharacter,
                            m_singleBlueTeamPlayerName,
                            true,
                            false);
                    }

                    return;
                }
                case 2:
                {
                    bool flag = true;
                    foreach (KeyValuePair<int, CharacterType> selectedChar in m_playerIDsOnDeck)
                    {
                        if (flag)
                        {
                            SetupSelection(
                                selectedChar,
                                data,
                                m_leftCharacterName,
                                doubleNoSelectionLeftCharacter,
                                doubleBrowseSelectionLeftCharacter,
                                doubleSelectionLeftCharacter,
                                m_doubleLeftSelectionCharacterSelected,
                                m_doubleLeftBlueSelectionCharacterSelected,
                                m_doubleLeftBlueTeamSelectedCharacter,
                                m_doubleLeftBlueTeamPlayerName,
                                true,
                                false);
                            flag = false;
                        }
                        else
                        {
                            SetupSelection(
                                selectedChar,
                                data,
                                m_rightCharacterName,
                                doubleNoSelectionRightCharacter,
                                doubleBrowseSelectionRightCharacter,
                                doubleSelectionRightCharacter,
                                m_doubleRightSelectionCharacterSelected,
                                m_doubleRightBlueSelectionCharacterSelected,
                                m_doubleRightBlueTeamSelectedCharacter,
                                m_doubleRightBlueTeamPlayerName,
                                true,
                                false);
                        }
                    }

                    break;
                }
            }
        }
    }

    private void UpdateHoverStatus(RankedResolutionPhaseData data)
    {
        bool isPickBan = m_lastDraftNotification.SubPhase.IsPickBanSubPhase();
        foreach (UIRankedModePlayerDraftEntry teammate in m_blueTeamMembers)
        {
            if (isPickBan && m_playerIDsOnDeck.ContainsKey(OurPlayerId) && teammate.PlayerID == OurPlayerId)
            {
                continue;
            }

            if (data.FriendlyTeamSelections.ContainsKey(teammate.PlayerID))
            {
                teammate.SetBrowseCharacterImageVisible(false);
                continue;
            }

            if (m_playerIDsOnDeck.ContainsKey(teammate.PlayerID))
            {
                if (m_playerIDsOnDeck[teammate.PlayerID] != CharacterType.None)
                {
                    if (IsBanned(m_playerIDsOnDeck[teammate.PlayerID])
                        || m_selectedCharacterTypes.Contains(m_playerIDsOnDeck[teammate.PlayerID]))
                    {
                        teammate.SetBrowseCharacterImageVisible(false);
                        UIManager.SetGameObjectActive(teammate.m_noCharacterImage, true);
                    }
                    else if (OurPlayerId != teammate.PlayerID)
                    {
                        CharacterResourceLink characterResourceLink =
                            GameWideData.Get().GetCharacterResourceLink(m_playerIDsOnDeck[teammate.PlayerID]);
                        teammate.SetBrowseCharacterImageVisible(true);
                        teammate.SetHoverCharacter(characterResourceLink);
                    }
                    else if (HoveredCharacter != CharacterType.None)
                    {
                        CharacterResourceLink characterResourceLink =
                            GameWideData.Get().GetCharacterResourceLink(HoveredCharacter);
                        teammate.SetBrowseCharacterImageVisible(true);
                        teammate.SetHoverCharacter(characterResourceLink);
                    }
                }
            }
            else
            {
                bool browseCharacterImageVisible = false;
                foreach (RankedResolutionPlayerState rankedResolutionPlayerState in data.UnselectedPlayerStates)
                {
                    if (rankedResolutionPlayerState.PlayerId != teammate.PlayerID)
                    {
                        continue;
                    }

                    if (rankedResolutionPlayerState.Intention != CharacterType.None)
                    {
                        if (!IsBanned(rankedResolutionPlayerState.Intention)
                            && !m_selectedCharacterTypes.Contains(rankedResolutionPlayerState.Intention))
                        {
                            CharacterResourceLink characterResourceLink =
                                GameWideData.Get().GetCharacterResourceLink(rankedResolutionPlayerState.Intention);
                            if (characterResourceLink != null)
                            {
                                browseCharacterImageVisible = true;
                                teammate.SetHoverCharacter(characterResourceLink);
                            }
                        }
                        else
                        {
                            teammate.SetBrowseCharacterImageVisible(false);
                            UIManager.SetGameObjectActive(teammate.m_noCharacterImage, true);
                        }
                    }

                    break;
                }

                teammate.SetBrowseCharacterImageVisible(browseCharacterImageVisible);
            }
        }

        foreach (UIRankedModePlayerDraftEntry enemy in m_redTeamMembers)
        {
            if (data.EnemyTeamSelections.ContainsKey(enemy.PlayerID))
            {
                enemy.SetBrowseCharacterImageVisible(false);
            }
            else
            {
                bool updated = false;
                if (m_playerIDsOnDeck.ContainsKey(enemy.PlayerID)
                    && m_playerIDsOnDeck[enemy.PlayerID] != CharacterType.None
                    && !IsBanned(m_playerIDsOnDeck[enemy.PlayerID]))
                {
                    CharacterResourceLink characterResourceLink =
                        GameWideData.Get().GetCharacterResourceLink(m_playerIDsOnDeck[enemy.PlayerID]);
                    enemy.SetBrowseCharacterImageVisible(true);
                    enemy.SetHoverCharacter(characterResourceLink);
                    updated = true;
                }

                if (!updated)
                {
                    enemy.SetBrowseCharacterImageVisible(false);
                }
            }
        }
    }

    private void CheckSelectedCharForCenterPiece(bool isOnBlue, RankedResolutionPhaseData data)
    {
        if (m_playerIDsOnDeck.Count == 1)
        {
            UIManager.SetGameObjectActive(m_singleSelectionCharacterSelected, true);
            foreach (KeyValuePair<int, CharacterType> selectedChar in m_playerIDsOnDeck)
            {
                SetupSelection(
                    selectedChar,
                    data,
                    m_singleCharacterName,
                    singleNoSelectionCharacter,
                    singleBrowseSelectionCharacter,
                    singleSelectionCharacter,
                    m_singleSelectionCharacterSelected,
                    isOnBlue
                        ? m_singleBlueSelectionCharacterSelected
                        : m_singleRedTeamSelectionCharacterSelected,
                    isOnBlue
                        ? m_singleBlueTeamSelectedCharacter
                        : m_singleRedTeamSelectedCharacter,
                    isOnBlue
                        ? m_singleBlueTeamPlayerName
                        : m_singleRedTeamPlayerName,
                    isOnBlue,
                    !isOnBlue);
            }
        }
        else
        {
            bool isLeft = !doubleSelectionLeftCharacter.gameObject.activeInHierarchy;
            foreach (KeyValuePair<int, CharacterType> selectedChar in m_playerIDsOnDeck)
            {
                if (isLeft)
                {
                    isLeft = false;
                    SetupSelection(
                        selectedChar,
                        data,
                        m_leftCharacterName,
                        doubleNoSelectionLeftCharacter,
                        doubleBrowseSelectionLeftCharacter,
                        doubleSelectionLeftCharacter,
                        m_doubleLeftSelectionCharacterSelected,
                        isOnBlue
                            ? m_doubleLeftBlueSelectionCharacterSelected
                            : m_doubleLeftRedTeamSelectionCharacterSelected,
                        isOnBlue
                            ? m_doubleLeftBlueTeamSelectedCharacter
                            : m_doubleLeftRedTeamSelectedCharacter,
                        isOnBlue
                            ? m_doubleLeftBlueTeamPlayerName
                            : m_doubleLeftRedTeamPlayerName,
                        isOnBlue,
                        !isOnBlue);
                }
                else
                {
                    SetupSelection(
                        selectedChar,
                        data,
                        m_rightCharacterName,
                        doubleNoSelectionRightCharacter,
                        doubleBrowseSelectionRightCharacter,
                        doubleSelectionRightCharacter,
                        m_doubleRightSelectionCharacterSelected,
                        isOnBlue
                            ? m_doubleRightBlueSelectionCharacterSelected
                            : m_doubleRightRedTeamSelectionCharacterSelected,
                        isOnBlue
                            ? m_doubleRightBlueTeamSelectedCharacter
                            : m_doubleRightRedTeamSelectedCharacter,
                        isOnBlue
                            ? m_doubleRightBlueTeamPlayerName
                            : m_doubleRightRedTeamPlayerName,
                        isOnBlue,
                        !isOnBlue);
                }
            }
        }
    }

    private void UpdatePlayerSelecting(RankedResolutionPhaseData data)
    {
        foreach (UIRankedModePlayerDraftEntry teammate in m_blueTeamMembers)
        {
            teammate.SetAsSelecting(data._001D(teammate.PlayerID));
        }

        foreach (UIRankedModePlayerDraftEntry enemy in m_redTeamMembers)
        {
            enemy.SetAsSelecting(data._001D(enemy.PlayerID));
        }
    }

    private void DoCharacterSelectContainerActiveCheck()
    {
        bool isPickBan = m_lastDraftNotification != null && m_lastDraftNotification.SubPhase.IsPickBanSubPhase();
        UIManager.SetGameObjectActive(m_characterSelectContainer, true);
        UICharacterPanelSelectRankModeButton[] buttons =
            m_characterSelectContainer.GetComponentsInChildren<UICharacterPanelSelectRankModeButton>(true);
        foreach (UICharacterPanelSelectRankModeButton btn in buttons)
        {
            bool clickable = isPickBan || SelectedCharacter == CharacterType.None;
            btn.SetClickable(clickable);
        }
    }

    private void UpdateRankData(RankedResolutionPhaseData data, bool updateFromGameInfoUpdate = false)
    {
        if (LastGameInfo == null)
        {
            return;
        }

        m_timeInPhase = data.TimeLeftInSubPhase;
        if (!updateFromGameInfoUpdate)
        {
            m_phaseStartTime = Time.time;
        }

        m_IsOnDeck = data._001D(OurPlayerId);
        DoCharacterSelectContainerActiveCheck();
        bool intendedLockInBtnStatus;
        if (m_IsOnDeck
            && !IsBanned(HoveredCharacter)
            && !m_selectedCharacterTypes.Contains(HoveredCharacter))
        {
            intendedLockInBtnStatus = m_lastDraftNotification.SubPhase.IsPickBanSubPhase()
                                      || m_lastDraftNotification.SubPhase.IsPickFreelancerSubPhase();
        }
        else
        {
            intendedLockInBtnStatus = false;
        }

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
        if (m_lastDraftNotification.SubPhase == FreelancerResolutionPhaseSubType.FREELANCER_TRADE
            && LastGameInfo.GameStatus == GameStatus.FreelancerSelecting
            && !m_stateQueues.Contains(CenterNotification.TradePhase)
            && m_currentState != CenterNotification.TradePhase)
        {
            QueueCenterState(CenterNotification.TradePhase);
        }

        if (!m_lastDraftNotification.SubPhase.IsPickBanSubPhase())
        {
            for (int i = 0; i < m_blueBans.Length; i++)
            {
                m_blueBans[i].SetAsSelecting(false);
                m_redBans[i].SetAsSelecting(false);
            }
        }

        for (int i = 0; i < m_blueBans.Length; i++)
        {
            if (i >= data.FriendlyBans.Count)
            {
                break;
            }

            CharacterType bannedCharacter = data.FriendlyBans[i];
            if (m_blueBans[i] != null)
            {
                CharacterResourceLink characterResourceLink =
                    GameWideData.Get().GetCharacterResourceLink(bannedCharacter);
                if (characterResourceLink != null)
                {
                    m_blueBans[i].SetSelectedCharacterImageVisible(true);
                    if (m_blueBans[i].GetSelectedCharacter() == CharacterType.None)
                    {
                        UIFrontEnd.PlaySound(FrontEndButtonSounds.RankModeBanPlayer);
                        CheckSelectedCharForCenterPiece(true, data);
                    }

                    m_blueBans[i].SetCharacter(characterResourceLink);
                }
            }

            if (!m_friendlyBannedCharacterTypes.Contains(bannedCharacter))
            {
                m_friendlyBannedCharacterTypes.Add(bannedCharacter);
            }
        }

        for (int i = 0; i < m_redBans.Length; i++)
        {
            if (i >= data.EnemyBans.Count)
            {
                break;
            }

            CharacterType bannedCharacter = data.EnemyBans[i];
            if (m_redBans[i] != null)
            {
                CharacterResourceLink characterResourceLink =
                    GameWideData.Get().GetCharacterResourceLink(bannedCharacter);
                if (characterResourceLink != null)
                {
                    m_redBans[i].SetSelectedCharacterImageVisible(true);
                    if (m_redBans[i].GetSelectedCharacter() == CharacterType.None)
                    {
                        UIFrontEnd.PlaySound(FrontEndButtonSounds.RankModeBanPlayer);
                        CheckSelectedCharForCenterPiece(false, data);
                    }

                    m_redBans[i].SetCharacter(characterResourceLink);
                }
            }

            if (!m_enemyBannedCharacterTypes.Contains(bannedCharacter))
            {
                m_enemyBannedCharacterTypes.Add(bannedCharacter);
            }
        }

        long accountId = ClientGameManager.Get().GetPlayerAccountData().AccountId;
        foreach (UIRankedModePlayerDraftEntry teammate in m_blueTeamMembers)
        {
            teammate.CanBeTraded = false;
            bool selectedCharacterImageVisible = false;
            if (data.FriendlyTeamSelections.ContainsKey(teammate.PlayerID))
            {
                CharacterResourceLink characterResourceLink =
                    GameWideData.Get().GetCharacterResourceLink(data.FriendlyTeamSelections[teammate.PlayerID]);
                if (characterResourceLink != null)
                {
                    selectedCharacterImageVisible = true;
                    if (teammate.GetSelectedCharacter() == CharacterType.None)
                    {
                        UIFrontEnd.PlaySound(FrontEndButtonSounds.RankModePickPlayer);
                        CheckSelectedCharForCenterPiece(true, data);
                    }

                    teammate.SetCharacter(characterResourceLink);
                    if (!IsBanned(characterResourceLink.m_characterType))
                    {
                        m_friendlyBannedCharacterTypes.Add(characterResourceLink.m_characterType);
                    }

                    teammate.CanBeTraded = !DidPlayerLockInDuringSwapPhase(data, teammate.PlayerID);
                }

                if (teammate.AccountID == accountId)
                {
                    SelectedCharacter = teammate.GetSelectedCharacter();
                }
            }

            teammate.SetTradePhase(
                m_lastDraftNotification.SubPhase == FreelancerResolutionPhaseSubType.FREELANCER_TRADE);
            if (m_lastDraftNotification.SubPhase != FreelancerResolutionPhaseSubType.FREELANCER_TRADE)
            {
                teammate.SetCharacterLocked(false);
            }

            UIManager.SetGameObjectActive(teammate, true);
            teammate.SetSelectedCharacterImageVisible(selectedCharacterImageVisible);
        }

        foreach (UIRankedModePlayerDraftEntry enemy in m_redTeamMembers)
        {
            bool selectedCharacterImageVisible = false;
            if (data.EnemyTeamSelections.ContainsKey(enemy.PlayerID))
            {
                CharacterResourceLink characterResourceLink =
                    GameWideData.Get().GetCharacterResourceLink(data.EnemyTeamSelections[enemy.PlayerID]);
                if (characterResourceLink != null)
                {
                    selectedCharacterImageVisible = true;
                    if (enemy.GetSelectedCharacter() == CharacterType.None)
                    {
                        UIFrontEnd.PlaySound(FrontEndButtonSounds.RankModePickPlayer);
                        CheckSelectedCharForCenterPiece(false, data);
                    }

                    enemy.SetCharacter(characterResourceLink);
                    if (!IsBanned(characterResourceLink.m_characterType))
                    {
                        m_enemyBannedCharacterTypes.Add(characterResourceLink.m_characterType);
                    }
                }
            }

            UIManager.SetGameObjectActive(enemy, true);
            enemy.SetSelectedCharacterImageVisible(selectedCharacterImageVisible);
        }

        if (m_currentCharacterPage == -1)
        {
            SetPageIndex(0);
        }

        CheckCharacterListValidity();
    }

    public Team GetCurrentTeam(RankedResolutionPhaseData data)
    {
        if (LastTeamInfo == null || LastPlayerInfo == null)
        {
            return Team.Invalid;
        }

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

        if (!m_lastDraftNotification.SubPhase.IsPickFreelancerSubPhase()
            && !m_lastDraftNotification.SubPhase.IsPickBanSubPhase())
        {
            return Team.Invalid;
        }

        if (LastPlayerInfo.TeamId == Team.Spectator)
        {
            return Team.TeamA;
        }

        return LastPlayerInfo.TeamId.OtherTeam();
    }

    private void SetCenterBackground(bool isOnBlue, bool isOnRed)
    {
        foreach (RectTransform t in singleSelectionBlueTeam)
        {
            UIManager.SetGameObjectActive(t, isOnBlue);
        }

        foreach (RectTransform t in singleSelectionRedTeam)
        {
            UIManager.SetGameObjectActive(t, isOnRed);
        }

        foreach (RectTransform t in doubleSelectionBlueTeam)
        {
            UIManager.SetGameObjectActive(t, isOnBlue);
        }

        foreach (RectTransform t in doubleSelectionRedTeam)
        {
            UIManager.SetGameObjectActive(t, isOnRed);
        }
    }

    private void PrintData(RankedResolutionPhaseData data)
    {
        string text = "LAST RANKED RESOLUTION PHASE DATA!\n";
        text += "Blue team info:\n";
        foreach (UIRankedModePlayerDraftEntry teammate in m_blueTeamMembers)
        {
            foreach (RankedResolutionPlayerState state in data.UnselectedPlayerStates)
            {
                if (state.PlayerId != teammate.PlayerID)
                {
                    continue;
                }

                text += string.Format(
                    "PlayerID is {0}, PlayerName is {1}, Is On deck: {2}\n",
                    state.PlayerId,
                    teammate.m_playerName,
                    state.OnDeckness);
                break;
            }
        }

        text += "Red team info:\n";
        foreach (UIRankedModePlayerDraftEntry enemy in m_redTeamMembers)
        {
            foreach (RankedResolutionPlayerState state in data.UnselectedPlayerStates)
            {
                if (state.PlayerId != enemy.PlayerID)
                {
                    continue;
                }

                text += string.Format(
                    "PlayerID is {0}, PlayerName is {1}, Is On deck: {2}\n",
                    state.PlayerId,
                    enemy.m_playerName.text,
                    state.OnDeckness);
                break;
            }
        }

        Debug.Log(text);
    }

    private void UpdateCenter(RankedResolutionPhaseData data, bool updateFromGameInfoUpdate)
    {
        if (LastGameInfo == null)
        {
            return;
        }

        bool isOnBlueTeam = false;
        bool isOnRedTeam = false;

        foreach (UIRankedModePlayerDraftEntry teammate in m_blueTeamMembers)
        {
            if (data._001D(teammate.PlayerID))
            {
                isOnBlueTeam = true;
                break;
            }
        }

        foreach (UIRankedModePlayerDraftEntry enemy in m_redTeamMembers)
        {
            if (data._001D(enemy.PlayerID))
            {
                isOnRedTeam = true;
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

        if (m_lastDraftNotification.SubPhase == m_lastPhaseForUpdateCenter && !flag3)
        {
            foreach (RankedResolutionPlayerState rankedResolutionPlayerState2 in data.PlayersOnDeck)
            {
                m_playerIDsOnDeck[rankedResolutionPlayerState2.PlayerId] = rankedResolutionPlayerState2.Intention;
            }
        }
        else
        {
            if (m_playerIDsOnDeck.Count > 0)
            {
                bool isTeammateOnDeck = false;
                bool isEnemyOnDeck = false;
                foreach (UIRankedModePlayerDraftEntry teammate in m_blueTeamMembers)
                {
                    if (m_playerIDsOnDeck.ContainsKey(teammate.PlayerID))
                    {
                        isTeammateOnDeck = true;
                        break;
                    }
                }

                foreach (UIRankedModePlayerDraftEntry enemy in m_redTeamMembers)
                {
                    if (m_playerIDsOnDeck.ContainsKey(enemy.PlayerID))
                    {
                        isEnemyOnDeck = true;
                        break;
                    }
                }

                if (isEnemyOnDeck)
                {
                    QueueCenterState(
                        m_playerIDsOnDeck.Count == 1
                            ? CenterNotification.RedTeamSingleSelectEnd
                            : CenterNotification.RedTeamDoubleSelectEnd);
                }
                else if (isTeammateOnDeck)
                {
                    QueueCenterState(
                        m_playerIDsOnDeck.Count == 1
                            ? CenterNotification.BlueTeamSingleSelectEnd
                            : CenterNotification.BlueTeamDoubleSelectEnd);
                }
            }

            if (!m_lastDraftNotification.SubPhase.IsPickBanSubPhase()
                && !m_lastDraftNotification.SubPhase.IsPickFreelancerSubPhase())
            {
                if (m_lastDraftNotification.SubPhase == FreelancerResolutionPhaseSubType.FREELANCER_TRADE)
                {
                    QueueCenterState(CenterNotification.TradePhase);
                }
            }
            else
            {
                CenterNotification centerNotification = CenterNotification.None;
                int count = data.PlayersOnDeck.Count;
                if (isOnRedTeam)
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
                else if (isOnBlueTeam)
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
            }

            m_playerIDsOnDeck.Clear();
            foreach (RankedResolutionPlayerState state in data.PlayersOnDeck)
            {
                m_playerIDsOnDeck.Add(state.PlayerId, state.Intention);
            }
        }

        if (!updateFromGameInfoUpdate)
        {
            UpdateCenterVisuals(data, isOnBlueTeam, isOnRedTeam);
        }

        m_lastPhaseForUpdateCenter = m_lastDraftNotification.SubPhase;
    }

    private void UpdateCenterVisuals(RankedResolutionPhaseData data, bool isOnBlueTeam, bool isOnRedTeam)
    {
        switch (m_playerIDsOnDeck.Count)
        {
            case 1:
            {
                foreach (KeyValuePair<int, CharacterType> selectedChar in m_playerIDsOnDeck)
                {
                    SetupSelection(
                        selectedChar,
                        data,
                        m_singleCharacterName,
                        singleNoSelectionCharacter,
                        singleBrowseSelectionCharacter,
                        singleSelectionCharacter,
                        m_singleSelectionCharacterSelected,
                        isOnBlueTeam
                            ? m_singleBlueSelectionCharacterSelected
                            : m_singleRedTeamSelectionCharacterSelected,
                        isOnBlueTeam
                            ? m_singleBlueTeamSelectedCharacter
                            : m_singleRedTeamSelectedCharacter,
                        isOnBlueTeam
                            ? m_singleBlueTeamPlayerName
                            : m_singleRedTeamPlayerName,
                        isOnBlueTeam,
                        isOnRedTeam);
                }

                break;
            }
            case 2:
            {
                bool isLeft = true;

                foreach (KeyValuePair<int, CharacterType> selectedChar in m_playerIDsOnDeck)
                {
                    if (isLeft)
                    {
                        SetupSelection(
                            selectedChar,
                            data,
                            m_leftCharacterName,
                            doubleNoSelectionLeftCharacter,
                            doubleBrowseSelectionLeftCharacter,
                            doubleSelectionLeftCharacter,
                            m_playerIDsOnDeck.Count == data.PlayersOnDeck.Count
                                ? m_doubleLeftSelectionCharacterSelected
                                : null,
                            isOnBlueTeam
                                ? m_doubleLeftBlueSelectionCharacterSelected
                                : m_doubleLeftRedTeamSelectionCharacterSelected,
                            isOnBlueTeam
                                ? m_doubleLeftBlueTeamSelectedCharacter
                                : m_doubleLeftRedTeamSelectedCharacter,
                            isOnBlueTeam
                                ? m_doubleLeftBlueTeamPlayerName
                                : m_doubleLeftRedTeamPlayerName,
                            isOnBlueTeam,
                            isOnRedTeam);
                        isLeft = false;
                    }
                    else
                    {
                        SetupSelection(
                            selectedChar,
                            data,
                            m_rightCharacterName,
                            doubleNoSelectionRightCharacter,
                            doubleBrowseSelectionRightCharacter,
                            doubleSelectionRightCharacter,
                            m_playerIDsOnDeck.Count == data.PlayersOnDeck.Count
                                ? m_doubleRightSelectionCharacterSelected
                                : null,
                            isOnBlueTeam
                                ? m_doubleRightBlueSelectionCharacterSelected
                                : m_doubleRightRedTeamSelectionCharacterSelected,
                            isOnBlueTeam
                                ? m_doubleRightBlueTeamSelectedCharacter
                                : m_doubleRightRedTeamSelectedCharacter,
                            !isOnBlueTeam
                                ? m_doubleRightRedTeamPlayerName
                                : m_doubleRightBlueTeamPlayerName,
                            isOnBlueTeam,
                            isOnRedTeam);
                    }
                }

                break;
            }
        }
    }

    public void SetupFreelancerSelect(RankedResolutionPhaseData data)
    {
        if (LastGameInfo != null
            && HoveredCharacter != m_selectedSubPhaseCharacter
            && m_selectedSubPhaseCharacter != CharacterType.None
            && data._001D(LastPlayerInfo.PlayerId)
            && !m_selectedCharacterTypes.Contains(m_selectedSubPhaseCharacter)
            && !IsBanned(m_selectedSubPhaseCharacter))
        {
            ClientGameManager.Get().UpdateSelectedCharacter(m_selectedSubPhaseCharacter);
            ClientGameManager.Get().SendRankedHoverClickRequest(m_selectedSubPhaseCharacter);
            m_intendedLockInBtnStatus = true;
            HoveredCharacter = m_selectedSubPhaseCharacter;
        }
    }

    private bool CharacterSelectAnimIsPlaying()
    {
        return m_doubleLeftSelectionCharacterSelected.gameObject.activeSelf
               || m_doubleRightSelectionCharacterSelected.gameObject.activeSelf
               || m_singleSelectionCharacterSelected.gameObject.activeSelf;
    }

    private void SetupSelection(
        KeyValuePair<int, CharacterType> selectedChar,
        RankedResolutionPhaseData data,
        TextMeshProUGUI nameDisplay,
        Image NoCharacter,
        Image BrowseCharacter,
        Image SelectedCharacter,
        Animator SelectedCharacterAnimator,
        Animator SelectedCharacterNameAnimator,
        TextMeshProUGUI SelectedCharacterText,
        TextMeshProUGUI PlayerName,
        bool isFriendly,
        bool isEnemy)
    {
        bool isAnythingSelected = false;
        bool isPlayerFound = false;
        bool isValid = !m_selectedCharacterTypes.Contains(selectedChar.Value)
                       && !IsBanned(selectedChar.Value)
                       && !data.FriendlyBans.Contains(selectedChar.Value)
                       && !data.EnemyBans.Contains(selectedChar.Value);

        if (isFriendly)
        {
            bool isLockedIn = m_lastSetupSelectionPhaseSubType.IsPickBanSubPhase()
                ? data.FriendlyBans.Contains(selectedChar.Value)
                : m_lastSetupSelectionPhaseSubType.IsPickFreelancerSubPhase()
                  && data.FriendlyTeamSelections.ContainsKey(selectedChar.Key);

            if (isLockedIn)
            {
                CharacterType character = selectedChar.Value;
                if (character != CharacterType.None)
                {
                    isAnythingSelected = true;
                    if (!m_lastSetupSelectionPhaseSubType.IsPickFreelancerSubPhase()
                        || !m_playerIDsThatSelected.Contains(selectedChar.Key))
                    {
                        SelectedCharacterText.text = character.GetDisplayName();
                        SelectedCharacter.sprite = GameWideData.Get().GetCharacterResourceLink(character)
                            .ActorDataPrefab.GetComponent<ActorData>().GetAliveHUDIcon();
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
                        if (m_currentState != CenterNotification.BlueTeamDoubleSelectStart
                            && m_currentState != CenterNotification.BlueTeamSingleSelectStart
                            && (m_stateQueues.Contains(CenterNotification.BlueTeamDoubleSelectStart)
                                || m_stateQueues.Contains(CenterNotification.BlueTeamSingleSelectStart)))
                        {
                            while (m_currentState != CenterNotification.BlueTeamDoubleSelectStart)
                            {
                                if (m_currentState == CenterNotification.BlueTeamSingleSelectStart)
                                {
                                    break;
                                }

                                m_currentState = m_stateQueues[0];
                                m_stateQueues.RemoveAt(0);
                            }

                            DoQueueState(m_currentState);
                        }
                    }

                    if (m_lastSetupSelectionPhaseSubType.IsPickFreelancerSubPhase()
                        && !m_playerIDsThatSelected.Contains(selectedChar.Key))
                    {
                        m_playerIDsThatSelected.Add(selectedChar.Key);
                        foreach (UIRankedModePlayerDraftEntry teammate in m_blueTeamMembers)
                        {
                            if (teammate.PlayerID != selectedChar.Key)
                            {
                                continue;
                            }

                            isPlayerFound = true;
                            PlayerName.text = teammate.m_playerName.text;
                        }
                    }
                }
            }
        }

        if (isEnemy)
        {
            bool isLockedIn = m_lastSetupSelectionPhaseSubType.IsPickBanSubPhase()
                ? data.EnemyBans.Contains(selectedChar.Value)
                : m_lastSetupSelectionPhaseSubType.IsPickFreelancerSubPhase()
                  && data.EnemyTeamSelections.ContainsKey(selectedChar.Key);

            if (isLockedIn)
            {
                CharacterType character = selectedChar.Value;
                if (character != CharacterType.None)
                {
                    isAnythingSelected = true;
                    bool isAnim = !m_lastSetupSelectionPhaseSubType.IsPickFreelancerSubPhase()
                                  || !m_playerIDsThatSelected.Contains(selectedChar.Key);
                    if (isAnim)
                    {
                        SelectedCharacterText.text = character.GetDisplayName();
                        SelectedCharacter.sprite = GameWideData
                            .Get()
                            .GetCharacterResourceLink(character)
                            .ActorDataPrefab
                            .GetComponent<ActorData>()
                            .GetAliveHUDIcon();
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
                        if (m_currentState != CenterNotification.RedTeamDoubleSelectStart
                            && m_currentState != CenterNotification.RedTeamSingleSelectStart
                            && (m_stateQueues.Contains(CenterNotification.RedTeamDoubleSelectStart)
                                || m_stateQueues.Contains(CenterNotification.RedTeamSingleSelectStart)))
                        {
                            while (m_currentState != CenterNotification.RedTeamDoubleSelectStart)
                            {
                                if (m_currentState == CenterNotification.RedTeamSingleSelectStart)
                                {
                                    break;
                                }

                                m_currentState = m_stateQueues[0];
                                m_stateQueues.RemoveAt(0);
                            }

                            DoQueueState(m_currentState);
                        }
                    }

                    if (m_lastSetupSelectionPhaseSubType.IsPickFreelancerSubPhase()
                        && !m_playerIDsThatSelected.Contains(selectedChar.Key))
                    {
                        m_playerIDsThatSelected.Add(selectedChar.Key);
                    }
                }
            }
        }

        if (!isPlayerFound)
        {
            bool isUpdateNeeded = m_playerIDsThatSelected.Count <= 0
                                  || m_playerIDsThatSelected[m_playerIDsThatSelected.Count - 1] != selectedChar.Key;
            if (m_animatorCurrentlyAnimating != null
                && m_animatorCurrentlyAnimating.gameObject.activeInHierarchy
                && m_lastSetupSelectionPhaseSubType == m_lastDraftNotification.SubPhase)
            {
                foreach (var teammate in m_blueTeamMembers)
                {
                    if (teammate.PlayerID == m_playerIDBeingAnimated)
                    {
                        isUpdateNeeded = false;
                        PlayerName.text = teammate.m_playerName.text;
                    }
                }
            }

            if (isUpdateNeeded)
            {
                PlayerName.text = string.Empty;
            }
        }

        m_lastSetupSelectionPhaseSubType = m_lastDraftNotification.SubPhase;
        if (!isAnythingSelected && !CharacterSelectAnimIsPlaying())
        {
            if (selectedChar.Value != CharacterType.None && isValid)
            {
                BrowseCharacter.sprite = GameWideData
                    .Get()
                    .GetCharacterResourceLink(selectedChar.Value)
                    .ActorDataPrefab
                    .GetComponent<ActorData>()
                    .GetAliveHUDIcon();
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

        if (!isAnythingSelected
            && !isValid
            && !CharacterSelectAnimIsPlaying())
        {
            UIManager.SetGameObjectActive(NoCharacter, true);
            UIManager.SetGameObjectActive(BrowseCharacter, false);
            UIManager.SetGameObjectActive(SelectedCharacter, false);
        }

        nameDisplay.text = string.Empty;
    }

    public void SetupBanSelect(RankedResolutionPhaseData data)
    {
        if (LastGameInfo == null)
        {
            return;
        }

        Team currentTeam = GetCurrentTeam(data);
        foreach (UIRankedModeDraftCharacterEntry ban in m_blueBans)
        {
            UIManager.SetGameObjectActive(ban, true);
        }

        foreach (UIRankedModeDraftCharacterEntry ban in m_redBans)
        {
            UIManager.SetGameObjectActive(ban, true);
        }

        if (data.PlayersOnDeck.Count > 0)
        {
            foreach (RankedResolutionPlayerState rankedResolutionPlayerState in data.PlayersOnDeck)
            {
                if (rankedResolutionPlayerState.Intention != CharacterType.None)
                {
                    CharacterResourceLink characterResourceLink =
                        GameWideData.Get().GetCharacterResourceLink(rankedResolutionPlayerState.Intention);
                    if (GetCurrentTeam(data) != LastPlayerInfo.TeamId && LastPlayerInfo.TeamId != Team.Spectator)
                    {
                        if (data.EnemyBans.Count < m_redBans.Length)
                        {
                            m_redBans[data.EnemyBans.Count].SetBrowseCharacterImageVisible(true);
                            m_redBans[data.EnemyBans.Count].SetHoverCharacter(characterResourceLink);
                        }

                        continue;
                    }

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

        for (int i = 0; i < m_redBans.Length; i++)
        {
            m_redBans[i].SetSelectedCharacterImageVisible(data.EnemyBans.Count > i);
            if (currentTeam != LastPlayerInfo.TeamId && LastPlayerInfo.TeamId != Team.Spectator)
            {
                m_redBans[i].SetAsSelecting(data.EnemyBans.Count == i);
            }
            else
            {
                m_redBans[i].SetAsSelecting(false);
            }
        }

        for (int i = 0; i < m_blueBans.Length; i++)
        {
            m_blueBans[i].SetSelectedCharacterImageVisible(data.FriendlyBans.Count > i);
            if (currentTeam != LastPlayerInfo.TeamId && LastPlayerInfo.TeamId != Team.Spectator)
            {
                m_blueBans[i].SetAsSelecting(false);
            }
            else
            {
                m_blueBans[i].SetAsSelecting(data.FriendlyBans.Count == i);
            }
        }
    }

    private string SubphaseToDisplayName(FreelancerResolutionPhaseSubType subPhase, TeamType teamType, bool isSelf)
    {
        switch (subPhase)
        {
            case FreelancerResolutionPhaseSubType.PICK_BANS1:
            case FreelancerResolutionPhaseSubType.PICK_BANS2:
                if (isSelf)
                {
                    return StringUtil.TR("SelectFreelancerBan", "RankMode");
                }

                if (teamType == TeamType.Ally)
                {
                    return StringUtil.TR("WaitingBlueTeamBan", "RankMode");
                }

                if (teamType == TeamType.Enemy)
                {
                    return StringUtil.TR("WaitingRedTeamBan", "RankMode");
                }

                break;
            case FreelancerResolutionPhaseSubType.PICK_FREELANCER1:
            case FreelancerResolutionPhaseSubType.PICK_FREELANCER2:
                if (isSelf)
                {
                    return StringUtil.TR("SelectFreelancer", "RankMode");
                }

                if (teamType == TeamType.Ally)
                {
                    return StringUtil.TR("WaitingBlueTeamSelect", "RankMode");
                }

                if (teamType == TeamType.Enemy)
                {
                    return StringUtil.TR("WaitingRedTeamSelect", "RankMode");
                }

                break;
        }

        return string.Empty;
    }

    internal int OurPlayerId
    {
        get
        {
            long accountId = ClientGameManager.Get().GetPlayerAccountData().AccountId;
            foreach (UIRankedModePlayerDraftEntry teammate in m_blueTeamMembers)
            {
                if (teammate.AccountID == accountId)
                {
                    return teammate.PlayerID;
                }
            }

            return -1;
        }
    }

    public void SetupInstructions(RankedResolutionPhaseData data)
    {
        Team currentTeam = GetCurrentTeam(data);
        TeamType teamType = TeamType.Any;

        if (currentTeam != Team.TeamA && currentTeam != Team.TeamB)
        {
            m_MessageText.color = m_neutralColor;
        }
        else if (currentTeam != LastPlayerInfo.TeamId
                 && (LastPlayerInfo.TeamId != Team.Spectator || currentTeam != Team.TeamA))
        {
            m_MessageText.color = m_redTeamColor;
            teamType = TeamType.Enemy;
        }
        else
        {
            m_MessageText.color = m_blueTeamColor;
            teamType = TeamType.Ally;
        }

        if (LastGameInfo == null
            || LastGameInfo.GameStatus == GameStatus.Stopped
            || LastGameInfo.GameStatus <= GameStatus.FreelancerSelecting)
        {
            m_MessageText.text = SubphaseToDisplayName(
                m_lastDraftNotification.SubPhase,
                teamType,
                data._001D(OurPlayerId));
        }
    }

    public void NotifyFreelancerTrades(RankedResolutionPhaseData data)
    {
        long accountId = ClientGameManager.Get().GetPlayerAccountData().AccountId;
        int num = -1;
        bool selfLockedIn = false;
        foreach (UIRankedModePlayerDraftEntry teammate in m_blueTeamMembers)
        {
            if (teammate.AccountID == accountId)
            {
                num = teammate.PlayerID;
                selfLockedIn = DidPlayerLockInDuringSwapPhase(data, teammate.PlayerID);
            }

            teammate.SetAsSelecting(false);
        }

        foreach (UIRankedModePlayerDraftEntry enemy in m_redTeamMembers)
        {
            enemy.SetAsSelecting(false);
        }

        foreach (UIRankedModePlayerDraftEntry teammate in m_blueTeamMembers)
        {
            UIRankedModePlayerDraftEntry.TradeStatus status = UIRankedModePlayerDraftEntry.TradeStatus.NoTrade;
            foreach (RankedTradeData rankedTradeData in data.TradeActions)
            {
                if (rankedTradeData.TradeAction == RankedTradeData.TradeActionType.AcceptOrOffer)
                {
                    if (rankedTradeData.AskedPlayerId == teammate.PlayerID && rankedTradeData.OfferingPlayerId == num)
                    {
                        status = UIRankedModePlayerDraftEntry.TradeStatus.TradeRequestSent;
                        break;
                    }

                    if (rankedTradeData.AskedPlayerId == num && rankedTradeData.OfferingPlayerId == teammate.PlayerID)
                    {
                        status = UIRankedModePlayerDraftEntry.TradeStatus.TradeRequestReceived;
                        break;
                    }
                }
                else if (rankedTradeData.TradeAction == RankedTradeData.TradeActionType.StopTrading)
                {
                    if (rankedTradeData.OfferingPlayerId == teammate.PlayerID
                        || rankedTradeData.AskedPlayerId == teammate.PlayerID)
                    {
                        status = UIRankedModePlayerDraftEntry.TradeStatus.StopTrading;
                        break;
                    }
                }
            }

            teammate.SetTradeStatus(status, teammate.PlayerID == num, selfLockedIn);
        }
    }

    private string GameStatusToDisplayString(GameStatus status)
    {
        switch (status)
        {
            case GameStatus.Launching:
            case GameStatus.Launched:
            case GameStatus.Connecting:
            case GameStatus.Connected:
            case GameStatus.Authenticated:
            case GameStatus.Loading:
            case GameStatus.Loaded:
                return StringUtil.TR("LaunchingGame", "RankMode");
        }

        return string.Empty;
    }

    private void UpdateGameLaunching(GameInfoNotification notification)
    {
        GameIsLaunching = true;
        m_MessageText.text = GameStatusToDisplayString(notification.GameInfo.GameStatus);
        foreach (UIRankedModePlayerDraftEntry teammate in m_blueTeamMembers)
        {
            teammate.SetTradePhase(false);
        }

        UIManager.SetGameObjectActive(m_lockFreelancerContainer, false);
        if (notification.GameInfo.GameStatus >= GameStatus.Launching
            && notification.GameInfo.GameStatus != GameStatus.Stopped)
        {
            SetFreelancerSettingButtonsVisible(false);
            UIRankedCharacterSelectSettingsPanel.Get().SetVisible(false);
        }
    }

    public void UpdateNotification(
        EnterFreelancerResolutionPhaseNotification notification,
        bool updateFromGameInfoUpdate = false)
    {
        if (GameIsLaunching
            || notification == null
            || notification.RankedData == null)
        {
            return;
        }

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

    public void NotifyButtonClicked(UICharacterPanelSelectRankModeButton btn)
    {
        if (m_IsOnDeck)
        {
            bool intendedLockInBtnStatus = false;
            m_selectedSubPhaseCharacter = CharacterType.None;
            foreach (UICharacterPanelSelectRankModeButton button in m_characterListDisplayButtons)
            {
                if (button == btn)
                {
                    m_selectedSubPhaseCharacter = button.m_characterType;
                    button.SetSelected(true);
                    if (!m_selectedCharacterTypes.Contains(button.m_characterType)
                        && !IsBanned(button.m_characterType))
                    {
                        HoveredCharacter = m_selectedSubPhaseCharacter;
                        RankedResolutionPhaseData value = m_lastDraftNotification.RankedData.Value;
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
                else
                {
                    button.SetSelected(false);
                }
            }

            m_intendedLockInBtnStatus = intendedLockInBtnStatus;
        }
        else
        {
            m_selectedSubPhaseCharacter = CharacterType.None;
            foreach (UICharacterPanelSelectRankModeButton button in m_characterListDisplayButtons)
            {
                if (button == btn)
                {
                    m_selectedSubPhaseCharacter = button.m_characterType;
                    button.SetSelected(true);
                    if (!m_selectedCharacterTypes.Contains(button.m_characterType) && !IsBanned(button.m_characterType))
                    {
                        ClientGameManager.Get().UpdateSelectedCharacter(m_selectedSubPhaseCharacter);
                        ClientGameManager.Get().SendRankedHoverClickRequest(m_selectedSubPhaseCharacter);
                        SetupCharacterSettings(m_selectedSubPhaseCharacter);
                        SetFreelancerSettingButtonsVisible(m_currentState >= CenterNotification.LoadoutPhase);
                    }
                }
                else
                {
                    button.SetSelected(false);
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
        for (CharacterType characterType = CharacterType.None; characterType < CharacterType.Last; characterType++)
        {
            if (gameManager.IsCharacterAllowedForPlayers(characterType)
                && gameManager.IsCharacterAllowedForGameType(characterType, GameType.Ranked, null, null))
            {
                m_validCharacterTypes.Add(characterType);
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
        UICharacterSelectCharacterSettingsPanel settingsPanel = UIRankedCharacterSelectSettingsPanel.Get();
        if (settingsPanel != null)
        {
            settingsPanel.SetVisible(false);
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

                CharacterResourceLink characterResourceLink =
                    GameWideData.Get().GetCharacterResourceLink(characterType);
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
            UICharacterPanelSelectRankModeButton btn =
                Instantiate(m_characterSelectBtnPrefab) as UICharacterPanelSelectRankModeButton;
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
            UICharacterPanelSelectRankModeButton btn =
                Instantiate(m_characterSelectBtnPrefab) as UICharacterPanelSelectRankModeButton;
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
            UICharacterPanelSelectRankModeButton btn =
                Instantiate(m_characterSelectBtnPrefab) as UICharacterPanelSelectRankModeButton;
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
        foreach (TextMeshProUGUI txt in btn.GetComponentsInChildren<TextMeshProUGUI>())
        {
            txt.text = (pageIndex + 1).ToString();
        }

        btn.SetSelected(false);
        btn.spriteController.callback = PageClicked;
    }

    private void PageClicked(BaseEventData data)
    {
        for (int i = 0; i < m_pageButtons.Count; i++)
        {
            if (m_pageButtons[i].spriteController.m_hitBoxImage.gameObject
                == (data as PointerEventData).pointerCurrentRaycast.gameObject)
            {
                SetPageIndex(i);
                return;
            }
        }
    }

    private bool IsCharacterTypeSelectable(CharacterType type)
    {
        return !m_selectedCharacterTypes.Contains(type)
               && !IsBanned(type)
               && GameManager.Get().IsCharacterAllowedForGameType(type, GameType.Ranked, null, null);
    }

    private bool IsCharacterVisibleForPlayer(CharacterType type)
    {
        return GameManager.Get().IsCharacterAllowedForPlayers(type);
    }

    private bool IsCharacterAvailableForPlayer(CharacterType type)
    {
        ClientGameManager clientGameManager = ClientGameManager.Get();
        PersistedCharacterData playerCharacterData = clientGameManager.GetPlayerCharacterData(type);
        return playerCharacterData != null && playerCharacterData.CharacterComponent.Unlocked
               || clientGameManager.IsCharacterAvailable(type, GameType.Ranked);
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
        if (IsVisible)
        {
            Initialize();
        }
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
        foreach (UIRankedModePlayerDraftEntry teammate in m_blueTeamMembers)
        {
            teammate.Dismantle();
        }

        foreach (UIRankedModePlayerDraftEntry enemy in m_redTeamMembers)
        {
            enemy.Dismantle();
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
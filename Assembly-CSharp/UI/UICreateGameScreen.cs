using LobbyGameClientMessages;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UICreateGameScreen : UIScene
{
    public class SubTypeButtonSelection
    {
        public _SelectableBtn DropdownButton;
        public GameSubType SubType;
    }

    public class MapSelectButton
    {
        public _ToggleSwap ToggleBtn;
        public GameMapConfig MapConfig;
    }

    public static int MAX_GAMENAME_SIZE = 21;

    public _ButtonSwapSprite m_cancelButton;
    public _ButtonSwapSprite m_createButton;
    public TMP_InputField m_gameNameInputField;
    public UITeamSizeButton[] m_teamAPlayersButtons;
    public UITeamSizeButton[] m_teamBPlayersButtons;
    public UITeamSizeButton[] m_spectatorButtons;
    public TMP_InputField m_maxRoundTime;
    public TMP_InputField m_roundTime;
    public Toggle m_allowDuplicateCharacters;
    public Toggle m_allowPausing;
    public Toggle m_timeBankToggle;
    public GridLayoutGroup m_mapListContainer;
    public _ToggleSwap m_mapListEntryPrefab;
    public List<MapSelectButton> m_mapList = new List<MapSelectButton>();
    public RectTransform[] m_containers;
    public _SelectableBtn m_gameSubTypeDropdownBtn;
    public RectTransform m_gameSubtypeContainer;
    public LayoutGroup m_gameSubTypeItemParent;
    public _SelectableBtn m_gameSubTypeItem;

    private static UICreateGameScreen s_instance;

    private List<SubTypeButtonSelection> GameSubTypeButtons = new List<SubTypeButtonSelection>();
    private ushort SelectedSubTypeMask;
    private GameSubType SelectedGameSubtype;
    private GameMapConfig SelectedMapConfig;

    public static UICreateGameScreen Get()
    {
        return s_instance;
    }

    public override SceneType GetSceneType()
    {
        return SceneType.CreateGameSettings;
    }

    public override void Awake()
    {
        s_instance = this;
        base.Awake();
        m_cancelButton.callback = CancelClicked;
        m_createButton.callback = CreateClicked;
        m_createButton.m_soundToPlay = FrontEndButtonSounds.Generic;
        for (int i = 0; i < m_teamAPlayersButtons.Length; i++)
        {
            m_teamAPlayersButtons[i].SetChecked(false);
            m_teamAPlayersButtons[i].SetTeam(0);
            m_teamAPlayersButtons[i].SetIndex(i);
            m_teamAPlayersButtons[i].m_callback = TeamSizeButtonClicked;
        }

        for (int i = 0; i < m_teamBPlayersButtons.Length; i++)
        {
            m_teamBPlayersButtons[i].SetChecked(false);
            m_teamBPlayersButtons[i].SetTeam(1);
            m_teamBPlayersButtons[i].SetIndex(i);
            m_teamBPlayersButtons[i].m_callback = TeamSizeButtonClicked;
        }

        for (int i = 0; i < m_spectatorButtons.Length; i++)
        {
            m_spectatorButtons[i].SetChecked(false);
            m_spectatorButtons[i].SetTeam(2);
            m_spectatorButtons[i].SetIndex(i);
            m_spectatorButtons[i].m_callback = TeamSizeButtonClicked;
        }

        ScrollRect scrollRect = m_mapListContainer.GetComponentInParent<ScrollRect>();
        if (scrollRect != null)
        {
            scrollRect.verticalScrollbar.gameObject.AddComponent<_MouseEventPasser>().AddNewHandler(scrollRect);
            scrollRect.scrollSensitivity = 100f;
        }

        _ToggleSwap[] componentsInChildren = m_mapListContainer.transform.GetComponentsInChildren<_ToggleSwap>(true);
        foreach (_ToggleSwap toggleSwap in componentsInChildren)
        {
            if (scrollRect != null)
            {
                toggleSwap.m_onButton.gameObject.AddComponent<_MouseEventPasser>().AddNewHandler(scrollRect);
                toggleSwap.m_offButton.gameObject.AddComponent<_MouseEventPasser>().AddNewHandler(scrollRect);
                UIEventTriggerUtils.AddListener(
                    toggleSwap.gameObject,
                    EventTriggerType.Scroll,
                    delegate(BaseEventData data) { scrollRect.OnScroll((PointerEventData)data); });
            }

            toggleSwap.transform.SetParent(m_mapListContainer.transform);
            toggleSwap.transform.localPosition = Vector3.zero;
            toggleSwap.transform.localScale = Vector3.one;
            toggleSwap.changedNotify = MapClicked;
            m_mapList.Add(
                new MapSelectButton
                {
                    MapConfig = null,
                    ToggleBtn = toggleSwap
                });
        }

        m_gameSubTypeDropdownBtn.spriteController.callback = DropdownClicked;
        UIManager.SetGameObjectActive(m_gameSubtypeContainer, false);
        m_gameNameInputField.onValueChanged.AddListener(EditGameName);
        m_roundTime.onValueChanged.AddListener(EditRoundTime);
        if (m_maxRoundTime != null && m_maxRoundTime.transform.parent != null)
        {
            UIManager.SetGameObjectActive(m_maxRoundTime.transform.parent, false);
        }

        UIManager.SetGameObjectActive(m_teamBPlayersButtons[0], false);
        UIManager.SetGameObjectActive(m_teamAPlayersButtons[0], false);
    }

    private void SetGameSubTypeDropdownListVisible(bool visible)
    {
        UIManager.SetGameObjectActive(m_gameSubtypeContainer, visible);
    }

    public void DropdownClicked(BaseEventData data)
    {
        SetGameSubTypeDropdownListVisible(!m_gameSubtypeContainer.gameObject.activeSelf);
    }

    private void EditGameName(string name)
    {
        if (m_gameNameInputField.text.Length > MAX_GAMENAME_SIZE)
        {
            m_gameNameInputField.text = m_gameNameInputField.text.Substring(0, MAX_GAMENAME_SIZE);
        }
    }

    private void EditRoundTime(string name)
    {
        if (m_roundTime.text.IsNullOrEmpty())
        {
            return;
        }

        try
        {
            TimeSpan turnTimeSpan = GameSubType.ConformTurnTimeSpanFromSeconds(double.Parse(m_roundTime.text));
            string text = Mathf.FloorToInt((float)turnTimeSpan.TotalSeconds).ToString();
            m_roundTime.text = text;
        }
        catch (FormatException)
        {
            m_roundTime.text = GameManager.Get().GameConfig.TurnTime.ToString();
        }
    }

    private void EditMaxRoundTime(string name)
    {
        if (m_maxRoundTime.text.IsNullOrEmpty())
        {
            return;
        }

        try
        {
            m_maxRoundTime.text = int.Parse(m_maxRoundTime.text).ToString();
        }
        catch (FormatException)
        {
            m_maxRoundTime.text = string.Empty;
        }
    }

    private void SetupOptionRestrictions(SubTypeButtonSelection selection)
    {
        if (selection.SubType.HasMod(GameSubType.SubTypeMods.RankedFreelancerSelection))
        {
            for (int i = 0; i < m_teamAPlayersButtons.Length; i++)
            {
                m_teamAPlayersButtons[i].SetChecked(i == 4);
                m_teamAPlayersButtons[i].Clickable = false;
            }

            for (int i = 0; i < m_teamBPlayersButtons.Length; i++)
            {
                m_teamBPlayersButtons[i].SetChecked(i == 4);
                m_teamBPlayersButtons[i].Clickable = false;
            }
        }
        else
        {
            foreach (UITeamSizeButton btn in m_teamAPlayersButtons)
            {
                btn.Clickable = true;
            }

            foreach (UITeamSizeButton btn in m_teamBPlayersButtons)
            {
                btn.Clickable = true;
            }
        }
    }

    private void SetupMapButtons(SubTypeButtonSelection selection)
    {
        ScrollRect scrollRect = m_mapListContainer.GetComponentInParent<ScrollRect>();
        int selectedMap = -1;
        int i = 0;
        foreach (GameMapConfig gameMapConfig in selection.SubType.GameMapConfigs)
        {
            if (!gameMapConfig.IsActive)
            {
                continue;
            }

            GameWideData.Get().GetMapDisplayName(gameMapConfig.Map);
            if (i >= m_mapList.Count)
            {
                _ToggleSwap toggleSwap = Instantiate(m_mapListEntryPrefab);
                if (scrollRect != null)
                {
                    toggleSwap.m_onButton.gameObject.AddComponent<_MouseEventPasser>().AddNewHandler(scrollRect);
                    toggleSwap.m_offButton.gameObject.AddComponent<_MouseEventPasser>().AddNewHandler(scrollRect);
                    UIEventTriggerUtils.AddListener(
                        toggleSwap.gameObject,
                        EventTriggerType.Scroll,
                        delegate(BaseEventData data) { scrollRect.OnScroll((PointerEventData)data); });
                }

                toggleSwap.transform.SetParent(m_mapListContainer.transform);
                toggleSwap.transform.localPosition = Vector3.zero;
                toggleSwap.transform.localScale = Vector3.one;
                toggleSwap.changedNotify = MapClicked;
                m_mapList.Add(
                    new MapSelectButton
                    {
                        MapConfig = gameMapConfig,
                        ToggleBtn = toggleSwap
                    });
            }

            _ToggleSwap toggleBtn = m_mapList[i].ToggleBtn;
            m_mapList[i].MapConfig = gameMapConfig;
            UIManager.SetGameObjectActive(toggleBtn, true);
            toggleBtn.gameObject.GetComponent<TextMeshProUGUI>().text =
                GameWideData.Get().GetMapDisplayName(gameMapConfig.Map);
            if (ClientGameManager.Get().IsMapInGameType(GameType.Custom, gameMapConfig.Map, out bool isActive) &&
                !isActive)
            {
                toggleBtn.gameObject.GetComponent<TextMeshProUGUI>().fontStyle |= FontStyles.Strikethrough;
            }
            else
            {
                if (selectedMap == -1)
                {
                    selectedMap = i;
                }
            }

            i++;
        }

        int count = selection.SubType.GameMapConfigs.Count;
        if (selectedMap != -1)
        {
            for (int j = 0; j < m_mapList.Count; j++)
            {
                m_mapList[j].ToggleBtn.SetOn(j == selectedMap);
            }

            SelectedMapConfig = m_mapList[selectedMap].MapConfig;
        }

        Vector2 cellSize = m_mapListContainer.cellSize;
        float y = cellSize.y;
        Vector2 spacing = m_mapListContainer.spacing;
        float y2 = (y + spacing.y) * (float)count * -1f;
        RectTransform obj = m_mapListContainer.gameObject.transform as RectTransform;
        Vector2 offsetMin = (m_mapListContainer.gameObject.transform as RectTransform).offsetMin;
        obj.offsetMin = new Vector2(offsetMin.x, y2);
        RectTransform obj2 = m_mapListContainer.gameObject.transform as RectTransform;
        Vector2 offsetMax = (m_mapListContainer.gameObject.transform as RectTransform).offsetMax;
        obj2.offsetMax = new Vector2(offsetMax.x, 0f);
        for (int k = count; k < m_mapList.Count; k++)
        {
            UIManager.SetGameObjectActive(m_mapList[k].ToggleBtn, false);
            m_mapList[k].MapConfig = null;
        }
    }

    private void SubTypeClickedHelper(SubTypeButtonSelection selected)
    {
        SelectedSubTypeMask = 0;
        SelectedGameSubtype = null;
        foreach (SubTypeButtonSelection subTypeButton in GameSubTypeButtons)
        {
            if (subTypeButton == selected)
            {
                TextMeshProUGUI[] subTypeLabels =
                    m_gameSubTypeDropdownBtn.GetComponentsInChildren<TextMeshProUGUI>(true);
                foreach (TextMeshProUGUI subTypeLabel in subTypeLabels)
                {
                    subTypeLabel.text = selected.SubType.GetNameAsPayload().ToString();
                }

                Dictionary<ushort, GameSubType> gameTypeSubTypes =
                    ClientGameManager.Get().GetGameTypeSubTypes(GameType.Custom);
                foreach (KeyValuePair<ushort, GameSubType> item in gameTypeSubTypes)
                {
                    if (item.Value == selected.SubType)
                    {
                        SelectedSubTypeMask = item.Key;
                        SelectedGameSubtype = item.Value;
                        break;
                    }
                }

                subTypeButton.DropdownButton.SetSelected(true, true, string.Empty, string.Empty);
                SetupMapButtons(selected);
                SetupOptionRestrictions(selected);
            }
            else
            {
                subTypeButton.DropdownButton.SetSelected(false, true, string.Empty, string.Empty);
            }
        }

        if (SelectedSubTypeMask != 0)
        {
            Debug.Log($"Selected SubType {selected.SubType.GetNameAsPayload().Term} with Mask {SelectedSubTypeMask}");
        }
    }

    public void SubTypeClicked(BaseEventData data)
    {
        SetGameSubTypeDropdownListVisible(false);
        foreach (SubTypeButtonSelection subTypeButton in GameSubTypeButtons)
        {
            if (subTypeButton.DropdownButton.spriteController.gameObject
                == (data as PointerEventData).pointerCurrentRaycast.gameObject)
            {
                SubTypeClickedHelper(subTypeButton);
                return;
            }
        }
    }

    private void PopulateSubtypes()
    {
        _SelectableBtn[] componentsInChildren = m_gameSubTypeItemParent.GetComponentsInChildren<_SelectableBtn>(true);
        int num = 0;
        GameSubTypeButtons.Clear();
        if (ClientGameManager.Get().GameTypeAvailabilies.ContainsKey(GameType.Custom))
        {
            GameTypeAvailability gameTypeAvailability = ClientGameManager.Get().GameTypeAvailabilies[GameType.Custom];
            IQueueRequirementApplicant applicant = ClientGameManager.Get().QueueRequirementApplicant;
            IQueueRequirementSystemInfo systemInfo = ClientGameManager.Get().QueueRequirementSystemInfo;
            foreach (GameSubType subType in gameTypeAvailability.SubTypes)
            {
                if (!subType.Requirements.IsNullOrEmpty()
                    && subType.Requirements.Exists(p => !p.DoesApplicantPass(
                        systemInfo,
                        applicant,
                        GameType.Custom,
                        subType)))
                {
                    continue;
                }

                _SelectableBtn selectableBtn;
                if (num < componentsInChildren.Length)
                {
                    selectableBtn = componentsInChildren[num];
                    num++;
                }
                else
                {
                    selectableBtn = Instantiate(m_gameSubTypeItem);
                    UIManager.ReparentTransform(selectableBtn.transform, m_gameSubTypeItemParent.transform);
                }

                GameSubTypeButtons.Add(
                    new SubTypeButtonSelection
                    {
                        DropdownButton = selectableBtn,
                        SubType = subType
                    });
                selectableBtn.spriteController.callback = SubTypeClicked;
                selectableBtn.SetSelected(false, false, string.Empty, string.Empty);
                TextMeshProUGUI[] componentsInChildren2 = selectableBtn.GetComponentsInChildren<TextMeshProUGUI>(true);
                for (int i = 0; i < componentsInChildren2.Length; i++)
                {
                    componentsInChildren2[i].text = subType.GetNameAsPayload().ToString();
                }
            }

            Vector2 sizeDelta = m_gameSubtypeContainer.sizeDelta;
            m_gameSubtypeContainer.sizeDelta = new Vector2(
                sizeDelta.x,
                16f + 30f * (float)gameTypeAvailability.SubTypes.Count);
        }
        else
        {
            Log.Error("No game sub types for custom!");
        }

        for (int j = num; j < componentsInChildren.Length; j++)
        {
            UIManager.SetGameObjectActive(componentsInChildren[j], false);
        }

        if (GameSubTypeButtons.Count > 0)
        {
            SubTypeClickedHelper(GameSubTypeButtons[0]);
        }
    }

    public void SetVisible(bool visible)
    {
        if (visible)
        {
            ClientGameManager clientGameManager = ClientGameManager.Get();
            if (!m_mapList.IsNullOrEmpty())
            {
                foreach (MapSelectButton mapSelectButton in m_mapList)
                {
                    if (mapSelectButton.MapConfig == null)
                    {
                        continue;
                    }

                    if (clientGameManager.IsMapInGameType(
                            GameType.Custom,
                            mapSelectButton.MapConfig.Map,
                            out bool isActive)
                        && !isActive)
                    {
                        mapSelectButton.ToggleBtn.gameObject.GetComponent<TextMeshProUGUI>().fontStyle |=
                            FontStyles.Strikethrough;
                    }
                    else
                    {
                        mapSelectButton.ToggleBtn.gameObject.GetComponent<TextMeshProUGUI>().fontStyle &=
                            ~FontStyles.Strikethrough;
                    }
                }
            }

            SetGameSubTypeDropdownListVisible(false);
        }

        UIManager.Get().SetSceneVisible(GetSceneType(), visible, new SceneVisibilityParameters());
        foreach (RectTransform container in m_containers)
        {
            UIManager.SetGameObjectActive(container, visible);
        }

        m_cancelButton.ResetMouseState();
        m_createButton.ResetMouseState();
    }

    public void Setup()
    {
        LobbyGameConfig lobbyGameConfig = new LobbyGameConfig();
        int numA = MathUtil.Clamp(lobbyGameConfig.TeamAPlayers, 0, m_teamAPlayersButtons.Length);
        int numB = MathUtil.Clamp(lobbyGameConfig.TeamBPlayers, 0, m_teamBPlayersButtons.Length);
        int numSpectators = MathUtil.Clamp(lobbyGameConfig.Spectators, 0, m_spectatorButtons.Length);
        SetChecked(m_teamAPlayersButtons, numA);
        SetChecked(m_teamBPlayersButtons, numB);
        SetChecked(m_spectatorButtons, numSpectators);
        m_timeBankToggle.isOn = true;
        m_roundTime.text = lobbyGameConfig.TurnTime.ToString();
        int playerNameLength = UIFrontEnd.Get().m_playerPanel.m_playerHandle.LastIndexOf('#');
        int maxLength = Mathf.Min(UIFrontEnd.Get().m_playerPanel.m_playerHandle.Length, MAX_GAMENAME_SIZE - 7);
        if (playerNameLength < maxLength)
        {
            maxLength = playerNameLength;
        }

        m_gameNameInputField.text = string.Format(
            StringUtil.TR("CreateGameTitle", "Global"),
            UIFrontEnd.Get().m_playerPanel.m_playerHandle.Substring(0, maxLength));
        PopulateSubtypes();
    }

    public void MapClicked(_ToggleSwap btn)
    {
        foreach (MapSelectButton mapSelectButton in m_mapList)
        {
            if (mapSelectButton.ToggleBtn == btn
                && ClientGameManager.Get().IsMapInGameType(
                    GameType.Custom,
                    mapSelectButton.MapConfig.Map,
                    out bool isActive)
                && !isActive)
            {
                mapSelectButton.ToggleBtn.SetOn(false);
                UIFrontEnd.PlaySound(FrontEndButtonSounds.NotifyWarning);
                return;
            }
        }

        UIFrontEnd.PlaySound(FrontEndButtonSounds.OptionsChoice);
        foreach (MapSelectButton mapSelectButton in m_mapList)
        {
            if (mapSelectButton.ToggleBtn == btn)
            {
                SelectedMapConfig = mapSelectButton.MapConfig;
                mapSelectButton.ToggleBtn.SetOn(true);
            }
            else
            {
                mapSelectButton.ToggleBtn.SetOn(false);
            }
        }
    }

    public void CreateClicked(BaseEventData data)
    {
        LobbyGameConfig lobbyGameConfig = new LobbyGameConfig();
#if VANILLA
        List<GameMapConfig> list = new List<GameMapConfig> { SelectedMapConfig };
#endif
        lobbyGameConfig.Map = SelectedMapConfig.Map;
        int teamAPlayers = GetChecked(m_teamAPlayersButtons);
        int teamBPlayers = GetChecked(m_teamBPlayersButtons);
        int spectators = GetChecked(m_spectatorButtons);
        string roomName = m_gameNameInputField.text;
        if (roomName.IsNullOrEmpty())
        {
            int length = Mathf.Min(UIFrontEnd.Get().m_playerPanel.m_playerHandle.Length, MAX_GAMENAME_SIZE - 7);
            roomName = string.Format(
                StringUtil.TR("CreateGameTitle", "Global"),
                UIFrontEnd.Get().m_playerPanel.m_playerHandle.Substring(0, length));
        }

        lobbyGameConfig.RoomName = roomName;
        lobbyGameConfig.TeamAPlayers = teamAPlayers;
        lobbyGameConfig.TeamBPlayers = teamBPlayers;
        lobbyGameConfig.Spectators = spectators;
        lobbyGameConfig.GameType = GameType.Custom;
        lobbyGameConfig.SubTypes = new List<GameSubType> { SelectedGameSubtype };
        lobbyGameConfig.InstanceSubTypeBit = SelectedSubTypeMask;
        if (lobbyGameConfig.InstanceSubType.GameOverrides == null)
        {
            lobbyGameConfig.InstanceSubType.GameOverrides = new GameValueOverrides();
        }

        try
        {
            lobbyGameConfig.InstanceSubType.GameOverrides.SetTimeSpanOverride(
                GameValueOverrides.OverrideAbleGameValue.TurnTimeSpan,
                GameSubType.ConformTurnTimeSpanFromSeconds(double.Parse(m_roundTime.text)));
        }
        catch (Exception exception)
        {
            Log.Exception(exception);
        }

        lobbyGameConfig.SetGameOption(GameOptionFlag.AllowDuplicateCharacters, m_allowDuplicateCharacters.isOn);
        lobbyGameConfig.SetGameOption(GameOptionFlag.AllowPausing, m_allowPausing.isOn);
        try
        {
            if (m_timeBankToggle.isOn)
            {
                lobbyGameConfig.InstanceSubType.GameOverrides.SetIntOverride(
                    GameValueOverrides.OverrideAbleGameValue.InitialTimeBankConsumables,
                    null);
            }
            else
            {
                lobbyGameConfig.InstanceSubType.GameOverrides.SetIntOverride(
                    GameValueOverrides.OverrideAbleGameValue.InitialTimeBankConsumables,
                    0);
            }
        }
        catch (Exception exception)
        {
            Log.Exception(exception);
        }

        AppState_CreateGame.Get().OnCreateClicked(lobbyGameConfig);
    }

    public void CancelClicked(BaseEventData data)
    {
        AppState_CreateGame.Get().OnCancelClicked();
    }

    public void TeamSizeButtonClicked(UITeamSizeButton btnClicked)
    {
        UIFrontEnd.PlaySound(FrontEndButtonSounds.OptionsChoice);
        int team = btnClicked.GetTeam();
        switch (team)
        {
            case 0:
            {
                foreach (UITeamSizeButton teamSizeBtn in m_teamAPlayersButtons)
                {
                    teamSizeBtn.SetChecked(teamSizeBtn == btnClicked);
                }

                break;
            }
            case 1:
            {
                foreach (UITeamSizeButton teamSizeBtn in m_teamBPlayersButtons)
                {
                    teamSizeBtn.SetChecked(teamSizeBtn == btnClicked);
                }

                break;
            }
            case 2:
            {
                foreach (UITeamSizeButton teamSizeBtn in m_spectatorButtons)
                {
                    teamSizeBtn.SetChecked(teamSizeBtn == btnClicked);
                }

                break;
            }
        }
    }

    private void SetChecked(UITeamSizeButton[] buttons, int index)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].SetChecked(i == index);
        }
    }

    private int GetChecked(UITeamSizeButton[] buttons)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i].IsChecked())
            {
                return i;
            }
        }

        return 0;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)
#if EVOS
            // custom: Update can run during frontend loading, before UIFrontEnd & chat exist
            && AppState.GetCurrent() == AppState_CreateGame.Get()
#endif
            && UIFrontEnd.Get().CanMenuEscape()
            && !UIFrontEnd.Get().m_frontEndChatConsole.EscapeJustPressed())
        {
            CancelClicked(null);
        }

        if (!Input.GetMouseButtonDown(0))
        {
            return;
        }

        bool shouldHideGameTypes = true;
        bool isFriendListSelected = EventSystem.current.currentSelectedGameObject != null
                                    && EventSystem.current.currentSelectedGameObject
                                        .GetComponentInParent<FriendListBannerMenu>() != null;
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(-1))
        {
            StandaloneInputModuleWithEventDataAccess component =
                EventSystem.current.gameObject.GetComponent<StandaloneInputModuleWithEventDataAccess>();
            if (component != null
                && component.GetLastPointerEventDataPublic(-1).pointerEnter != null)
            {
                _SelectableBtn btn = component.GetLastPointerEventDataPublic(-1).pointerEnter
                    .GetComponentInParent<_SelectableBtn>();
                bool isGameTypeSelected = false;
                if (btn == null)
                {
                    // TODO CLIENT LOW dead loop?
                    while (btn != null)
                    {
                        if (btn == m_gameSubTypeDropdownBtn)
                        {
                            isGameTypeSelected = true;
                            break;
                        }

                        btn = btn.transform.parent.GetComponentInParent<_SelectableBtn>();
                    }
                }

                if (btn != null || isGameTypeSelected || isFriendListSelected)
                {
                    shouldHideGameTypes = false;
                }
            }
        }

        if (shouldHideGameTypes)
        {
            SetGameSubTypeDropdownListVisible(false);
        }
    }
}
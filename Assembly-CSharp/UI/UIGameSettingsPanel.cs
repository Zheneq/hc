using System;
using System.Collections.Generic;
using System.Linq;
using LobbyGameClientMessages;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = System.Random;

public class UIGameSettingsPanel : UIScene
{
    public class MapSelectButton
    {
        public _ToggleSwap ToggleBtn;
        public GameMapConfig MapConfig;
    }

    public _ButtonSwapSprite m_cancelButton;
    public _ButtonSwapSprite m_updateButton;
    public _ButtonSwapSprite m_balanceTeams;
    public InputField m_gameNameInputField;
    public UITeamSizeButton[] m_teamAPlayersButtons;
    public UITeamSizeButton[] m_teamBPlayersButtons;
    public UITeamSizeButton[] m_spectatorButtons;
    public InputField m_roundTime;
    public InputField m_maxRoundTime;
    public Toggle m_allowDuplicateCharacters;
    public Toggle m_allowPausing;
    public Toggle m_useTimeBank;
    public _ToggleSwap m_mapItemPrefab;
    public GridLayoutGroup m_mapListContainer;
    public List<MapSelectButton> m_theMapList = new List<MapSelectButton>();
    public UITeamMemberEntry[] m_teamAMemberEntries;
    public UITeamMemberEntry[] m_teamBMemberEntries;
    public UITeamMemberEntry[] m_spectatorMemberEntries;
    public GameObject m_teamAMemberEntriesContainer;
    public GameObject m_teamBMemberEntriesContainer;
    public GameObject m_spectatorMemberEntriesContainer;
    public Color m_teamSizeButtonTextSelectedColor = Color.white;
    public Color m_teamSizeButtonTextUnselectedColor = Color.black;
    public RectTransform[] m_containers;
    [HideInInspector]
    public bool m_lastVisible;

    private bool isSetup;
    private static UIGameSettingsPanel s_instance;
    private LobbyTeamInfo m_teamInfo;
    private LobbyPlayerInfo m_playerInfo;
    private Random m_random = new Random();

    private const int RANKED_TEAM_SIZE = 4;

    public static UIGameSettingsPanel Get()
    {
        return s_instance;
    }

    public override SceneType GetSceneType()
    {
        return SceneType.CustomGameSettings;
    }

    public override void Awake()
    {
        s_instance = this;
        m_cancelButton.callback = CancelClicked;
        m_updateButton.callback = UpdateClicked;
        m_balanceTeams.callback = BalanceTeamsClicked;
        m_updateButton.m_soundToPlay = FrontEndButtonSounds.Generic;
        m_gameNameInputField.onValueChanged.AddListener(EditGameName);
        m_roundTime.onValueChanged.AddListener(EditRoundTime);
        if (m_maxRoundTime != null && m_maxRoundTime.transform.parent != null)
        {
            UIManager.SetGameObjectActive(m_maxRoundTime.transform.parent, false);
        }

        _ToggleSwap[] mapEntries = m_mapListContainer.transform.GetComponentsInChildren<_ToggleSwap>(true);
        ScrollRect mapListScroll = m_mapListContainer.GetComponentInParent<ScrollRect>();
        foreach (_ToggleSwap mapEntry in mapEntries)
        {
            if (mapListScroll != null)
            {
                mapEntry.m_onButton.gameObject.AddComponent<_MouseEventPasser>().AddNewHandler(mapListScroll);
                mapEntry.m_offButton.gameObject.AddComponent<_MouseEventPasser>().AddNewHandler(mapListScroll);
                UIEventTriggerUtils.AddListener(
                    mapEntry.gameObject,
                    EventTriggerType.Scroll,
                    delegate(BaseEventData data) { mapListScroll.OnScroll((PointerEventData)data); });
            }

            mapEntry.transform.SetParent(m_mapListContainer.transform);
            mapEntry.transform.localPosition = Vector3.zero;
            mapEntry.transform.localScale = Vector3.one;
            mapEntry.changedNotify = MapClicked;
            m_theMapList.Add(
                new MapSelectButton
                {
                    MapConfig = null,
                    ToggleBtn = mapEntry
                });
        }

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

        foreach (UITeamMemberEntry memberEntry in m_teamAMemberEntries)
        {
            memberEntry.SetTeamId(Team.TeamA);
        }

        foreach (UITeamMemberEntry memberEntry in m_teamBMemberEntries)
        {
            memberEntry.SetTeamId(Team.TeamB);
        }

        foreach (UITeamMemberEntry memberEntry in m_spectatorMemberEntries)
        {
            memberEntry.SetTeamId(Team.Spectator);
        }

        UIManager.SetGameObjectActive(m_teamBPlayersButtons[0], false);
        UIManager.SetGameObjectActive(m_teamAPlayersButtons[0], false);
        SetVisible(false);
        base.Awake();
    }

    private void EditGameName(string name)
    {
        if (m_gameNameInputField.text.Length > UICreateGameScreen.MAX_GAMENAME_SIZE)
        {
            m_gameNameInputField.text = m_gameNameInputField.text.Substring(0, UICreateGameScreen.MAX_GAMENAME_SIZE);
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
            m_roundTime.text = Mathf.FloorToInt(
                    (float)GameSubType.ConformTurnTimeSpanFromSeconds(double.Parse(m_roundTime.text)).TotalSeconds)
                .ToString();
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

    private void SetupOptionRestrictions(GameSubType GameSubtype)
    {
        if (GameSubtype.HasMod(GameSubType.SubTypeMods.RankedFreelancerSelection))
        {
            for (int i = 0; i < m_teamAPlayersButtons.Length; i++)
            {
                m_teamAPlayersButtons[i].SetChecked(i == RANKED_TEAM_SIZE);
                m_teamAPlayersButtons[i].Clickable = false;
            }

            for (int i = 0; i < m_teamBPlayersButtons.Length; i++)
            {
                m_teamBPlayersButtons[i].SetChecked(i == RANKED_TEAM_SIZE);
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

    public void SetVisible(bool visible)
    {
        m_lastVisible = visible;
        if (visible)
        {
            ClientGameManager clientGameManager = ClientGameManager.Get();
            SetupOptionRestrictions(GameManager.Get().GameConfig.InstanceSubType);
            if (!m_theMapList.IsNullOrEmpty())
            {
                foreach (var mapEntry in m_theMapList)
                {
                    if (mapEntry.MapConfig == null)
                    {
                        continue;
                    }

                    if (clientGameManager.IsMapInGameType(GameType.Custom, mapEntry.MapConfig.Map, out bool isActive)
                        && !isActive)
                    {
                        mapEntry.ToggleBtn.gameObject.GetComponent<TextMeshProUGUI>().fontStyle |=
                            FontStyles.Strikethrough;
                    }
                    else
                    {
                        mapEntry.ToggleBtn.gameObject.GetComponent<TextMeshProUGUI>().fontStyle &= (FontStyles)(-65);
                    }
                }
            }
        }

        m_cancelButton.ResetMouseState();
        m_updateButton.ResetMouseState();
        foreach (RectTransform container in m_containers)
        {
            UIManager.SetGameObjectActive(container, visible);
        }

        if (UICharacterSelectScreenController.Get() != null)
        {
            if (visible)
            {
                UICharacterSelectScreenController.Get().UpdateReadyCancelButtonStates();
                if (UICharacterSelectScreenController.Get().m_changeFreelancerBtn.gameObject.activeSelf)
                {
                    UIManager.SetGameObjectActive(UICharacterSelectScreenController.Get().m_changeFreelancerBtn, false);
                }

                UICharacterSelectScreenController.Get().SetCharacterSelectVisible(false);
                UICharacterSelectScreenController.Get().m_charSettingsPanel.SetVisible(false);
                if (UIPlayerProgressPanel.Get().IsVisible())
                {
                    UIPlayerProgressPanel.Get().SetVisible(false);
                }
            }
            else
            {
                if (AppState.GetCurrent() == AppState_CharacterSelect.Get()
                    && !UICharacterSelectScreenController.Get().m_changeFreelancerBtn.gameObject.activeSelf)
                {
                    UIManager.SetGameObjectActive(UICharacterSelectScreenController.Get().m_changeFreelancerBtn, true);
                }

                UICharacterSelectScreenController.Get().UpdateReadyCancelButtonStates();
            }
        }

        if (UICharacterScreen.Get() != null)
        {
            UICharacterScreen.Get().DoRefreshFunctions((ushort)UICharacterScreen.RefreshFunctionType.RefreshSideButtonVisibility);
        }
    }

    private void SetupMapButtons(LobbyGameConfig gameConfig)
    {
        GameSubType instanceSubType = gameConfig.InstanceSubType;
        ScrollRect mapListScroll = m_mapListContainer.GetComponentInParent<ScrollRect>();
        int i = 0;
        foreach (GameMapConfig gameMapConfig in instanceSubType.GameMapConfigs)
        {
            if (!gameMapConfig.IsActive)
            {
                continue;
            }

            GameWideData.Get().GetMapDisplayName(gameMapConfig.Map);
            if (i >= m_theMapList.Count)
            {
                _ToggleSwap toggleSwap = Instantiate(m_mapItemPrefab);
                if (mapListScroll != null)
                {
                    toggleSwap.m_onButton.gameObject.AddComponent<_MouseEventPasser>().AddNewHandler(mapListScroll);
                    toggleSwap.m_offButton.gameObject.AddComponent<_MouseEventPasser>().AddNewHandler(mapListScroll);
                    UIEventTriggerUtils.AddListener(
                        toggleSwap.gameObject,
                        EventTriggerType.Scroll,
                        delegate(BaseEventData data) { mapListScroll.OnScroll((PointerEventData)data); });
                }

                toggleSwap.transform.SetParent(m_mapListContainer.transform);
                toggleSwap.transform.localPosition = Vector3.zero;
                toggleSwap.transform.localScale = Vector3.one;
                toggleSwap.changedNotify = MapClicked;
                m_theMapList.Add(
                    new MapSelectButton
                    {
                        MapConfig = gameMapConfig,
                        ToggleBtn = toggleSwap
                    });
            }

            _ToggleSwap toggleBtn = m_theMapList[i].ToggleBtn;
            m_theMapList[i].MapConfig = gameMapConfig;
            toggleBtn.SetOn(gameConfig.Map == gameMapConfig.Map);
            UIManager.SetGameObjectActive(toggleBtn, true);
            toggleBtn.gameObject.GetComponent<TextMeshProUGUI>().text =
                GameWideData.Get().GetMapDisplayName(gameMapConfig.Map);
            if (ClientGameManager.Get().IsMapInGameType(GameType.Custom, gameMapConfig.Map, out bool isActive)
                && !isActive)
            {
                toggleBtn.gameObject.GetComponent<TextMeshProUGUI>().fontStyle |= FontStyles.Strikethrough;
            }

            i++;
        }

        int count = instanceSubType.GameMapConfigs.Count;
        (m_mapListContainer.gameObject.transform as RectTransform).offsetMin =
            new Vector2(
                (m_mapListContainer.gameObject.transform as RectTransform).offsetMin.x,
                (m_mapListContainer.cellSize.y + m_mapListContainer.spacing.y) * count * -1f);
        (m_mapListContainer.gameObject.transform as RectTransform).offsetMax =
            new Vector2(
                (m_mapListContainer.gameObject.transform as RectTransform).offsetMax.x,
                0f);

        for (int j = count; j < m_theMapList.Count; j++)
        {
            UIManager.SetGameObjectActive(m_theMapList[j].ToggleBtn, false);
            m_theMapList[j].MapConfig = null;
        }
    }

    public void Setup(LobbyGameConfig gameConfig, LobbyTeamInfo teamInfo, LobbyPlayerInfo playerInfo)
    {
        isSetup = false;
        m_teamInfo = teamInfo;
        m_playerInfo = playerInfo;
        SetChecked(m_teamAPlayersButtons, gameConfig.TeamAPlayers);
        SetChecked(m_teamBPlayersButtons, gameConfig.TeamBPlayers);
        SetChecked(m_spectatorButtons, gameConfig.Spectators);
        PopulateTeam(gameConfig.TeamAPlayers, teamInfo.TeamAPlayerInfo, m_teamAMemberEntries);
        PopulateTeam(gameConfig.TeamBPlayers, teamInfo.TeamBPlayerInfo, m_teamBMemberEntries);
        PopulateTeam(gameConfig.Spectators, teamInfo.SpectatorInfo, m_spectatorMemberEntries);
        InputField gameNameInputField = m_gameNameInputField;
        string text = gameConfig.RoomName;
        if (text == null)
        {
            text = string.Empty;
        }

        gameNameInputField.text = text;
        m_roundTime.text = gameConfig.TurnTime.ToString();
        m_allowDuplicateCharacters.isOn = gameConfig.HasGameOption(GameOptionFlag.AllowDuplicateCharacters);
        m_allowPausing.isOn = gameConfig.HasGameOption(GameOptionFlag.AllowPausing);
        bool isOn = true;
        if (gameConfig.InstanceSubType.GameOverrides != null)
        {
            int? initialTimeBankConsumables = gameConfig.InstanceSubType.GameOverrides.InitialTimeBankConsumables;
            if (initialTimeBankConsumables.HasValue && initialTimeBankConsumables.GetValueOrDefault() <= 0)
            {
                isOn = false;
            }
        }

        m_useTimeBank.isOn = isOn;
        SetupMapButtons(gameConfig);
        SetInteractable(m_playerInfo.IsGameOwner);
        isSetup = true;
    }

    public void UpdateCharacterList(LobbyPlayerInfo playerInfo, LobbyTeamInfo teamInfo, LobbyGameInfo gameInfo)
    {
        m_teamInfo = teamInfo;
        m_playerInfo = playerInfo;
        PopulateTeam(GetChecked(m_teamAPlayersButtons), teamInfo.TeamAPlayerInfo, m_teamAMemberEntries);
        PopulateTeam(GetChecked(m_teamBPlayersButtons), teamInfo.TeamBPlayerInfo, m_teamBMemberEntries);
        PopulateTeam(GetChecked(m_spectatorButtons), teamInfo.SpectatorInfo, m_spectatorMemberEntries);
    }

    public void MapClicked(_ToggleSwap btn)
    {
        foreach (MapSelectButton mapEntry in m_theMapList)
        {
            if (mapEntry.ToggleBtn == btn
                && ClientGameManager.Get().IsMapInGameType(
                    GameType.Custom,
                    mapEntry.MapConfig.Map,
                    out bool isActive)
                && !isActive)
            {
                mapEntry.ToggleBtn.SetOn(false);
                UIFrontEnd.PlaySound(FrontEndButtonSounds.NotifyWarning);
                return;
            }
        }

        UIFrontEnd.PlaySound(FrontEndButtonSounds.OptionsChoice);
        foreach (MapSelectButton mapEntry in m_theMapList)
        {
            mapEntry.ToggleBtn.SetOn(mapEntry.ToggleBtn == btn);
        }
    }

    private void UpdateClickedHelper(bool closeSettingsWindow = true)
    {
        string map = string.Empty;

        foreach (MapSelectButton btn in m_theMapList)
        {
            if (btn.ToggleBtn.IsChecked())
            {
                map = btn.MapConfig.Map;
                break;
            }
        }

        LobbyGameConfig lobbyGameConfig = new LobbyGameConfig
        {
            Map = map,
            RoomName = m_gameNameInputField.text,
            TeamAPlayers = GetChecked(m_teamAPlayersButtons),
            TeamBPlayers = GetChecked(m_teamBPlayersButtons),
            Spectators = GetChecked(m_spectatorButtons),
            GameType = GameType.Custom,
            SubTypes = new List<GameSubType> { GameManager.Get().GameConfig.InstanceSubType },
            InstanceSubTypeBit = GameManager.Get().GameConfig.InstanceSubTypeBit
        };

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
        catch (Exception ex)
        {
            Log.Exception(ex);
        }

        if (m_allowDuplicateCharacters.isOn)
        {
            lobbyGameConfig.GameOptionFlags =
                lobbyGameConfig.GameOptionFlags.WithGameOption(GameOptionFlag.AllowDuplicateCharacters);
        }

        if (m_allowPausing.isOn)
        {
            lobbyGameConfig.GameOptionFlags =
                lobbyGameConfig.GameOptionFlags.WithGameOption(GameOptionFlag.AllowPausing);
        }

        try
        {
            if (m_useTimeBank.isOn)
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
        catch (Exception ex)
        {
            Log.Exception(ex);
        }

        m_teamInfo.TeamPlayerInfo.Clear();
        int nextSlot = 1;
        foreach (UITeamMemberEntry uITeamMemberEntry in m_teamAMemberEntries)
        {
            LobbyPlayerInfo playerInfo = uITeamMemberEntry.GetPlayerInfo();
            if (playerInfo != null)
            {
                playerInfo.CustomGameVisualSlot = nextSlot;
                m_teamInfo.TeamPlayerInfo.Add(playerInfo);
            }

            nextSlot++;
        }

        nextSlot = 1;
        foreach (UITeamMemberEntry uITeamMemberEntry in m_teamBMemberEntries)
        {
            LobbyPlayerInfo playerInfo = uITeamMemberEntry.GetPlayerInfo();
            if (playerInfo != null)
            {
                playerInfo.CustomGameVisualSlot = nextSlot;
                m_teamInfo.TeamPlayerInfo.Add(playerInfo);
            }

            nextSlot++;
        }

        nextSlot = 1;
        foreach (UITeamMemberEntry uITeamMemberEntry in m_spectatorMemberEntries)
        {
            LobbyPlayerInfo playerInfo = uITeamMemberEntry.GetPlayerInfo();
            if (playerInfo != null)
            {
                playerInfo.CustomGameVisualSlot = nextSlot;
                m_teamInfo.TeamPlayerInfo.Add(playerInfo);
            }

            nextSlot++;
        }

        AppState_CharacterSelect.Get().OnUpdateGameSettingsClicked(lobbyGameConfig, m_teamInfo, closeSettingsWindow);
    }

    public void UpdateClicked(BaseEventData data)
    {
        UpdateClickedHelper();
    }

    public void BalanceTeamsClicked(BaseEventData data)
    {
        BalancedTeamRequest request = new BalancedTeamRequest
        {
            Slots = new List<BalanceTeamSlot>()
        };

        foreach (UITeamMemberEntry teamMemberEntry in m_teamAMemberEntries.Union(m_teamBMemberEntries))
        {
            LobbyPlayerInfo playerInfo = teamMemberEntry.GetPlayerInfo();
            if (playerInfo == null || playerInfo.IsSpectator)
            {
                continue;
            }

            request.Slots.Add(
                new BalanceTeamSlot
                {
                    Team = teamMemberEntry.GetTeamId(),
                    PlayerId = playerInfo.PlayerId,
                    AccountId = playerInfo.AccountId,
                    SelectedCharacter = playerInfo.CharacterType,
                    BotDifficulty = playerInfo.Difficulty
                });
        }

        ClientGameManager.Get().LobbyInterface.RequestBalancedTeam(
            request,
            delegate(BalancedTeamResponse response)
            {
                if (!response.Success)
                {
                    UIFrontEnd.PlaySound(FrontEndButtonSounds.OptionsCancel);
                    TextConsole.Get().Write(
                        new TextConsole.Message
                        {
                            Text = response.LocalizedFailure != null
                                ? response.LocalizedFailure.ToString()
                                : response.ErrorMessage,
                            MessageType = ConsoleMessageType.SystemMessage
                        });
                    return;
                }

                UIFrontEnd.PlaySound(FrontEndButtonSounds.OptionsOK);
                List<LobbyPlayerInfo> teamAPlayers = new List<LobbyPlayerInfo>();
                List<LobbyPlayerInfo> teamBPlayers = new List<LobbyPlayerInfo>();

                foreach (BalanceTeamSlot slot in response.Slots)
                {
                    if (!request.Slots.Exists(p => p.PlayerId == slot.PlayerId && p.Team != slot.Team))
                    {
                        continue;
                    }

                    UITeamMemberEntry uITeamMemberEntry = m_teamAMemberEntries.Union(m_teamBMemberEntries)
                        .FirstOrDefault(p => p.GetPlayerInfo() != null && p.GetPlayerInfo().PlayerId == slot.PlayerId);

                    LobbyPlayerInfo playerInfo = uITeamMemberEntry != null ? uITeamMemberEntry.GetPlayerInfo() : null;
                    if (playerInfo == null)
                    {
                        continue;
                    }

                    RemovePlayer(playerInfo);
                    if (slot.Team == Team.TeamA)
                    {
                        teamAPlayers.Add(playerInfo);
                    }
                    else
                    {
                        teamBPlayers.Add(playerInfo);
                    }
                }

                foreach (LobbyPlayerInfo playerInfo in teamAPlayers)
                {
                    playerInfo.TeamId = Team.TeamA;
                    AddPlayer(playerInfo);
                }

                foreach (LobbyPlayerInfo playerInfo in teamBPlayers)
                {
                    playerInfo.TeamId = Team.TeamB;
                    AddPlayer(playerInfo);
                }

                UpdateClickedHelper(false);
            });
    }

    public void CancelClicked(BaseEventData data)
    {
        AppState_CharacterSelect.Get().OnCancelGameSettingsClicked();
    }

    public void TeamSizeButtonClicked(UITeamSizeButton btnClicked)
    {
        UIFrontEnd.PlaySound(FrontEndButtonSounds.OptionsChoice);
        switch (btnClicked.GetTeam())
        {
            case 0:
                UpdateTeamSize(m_teamAMemberEntries, m_teamAPlayersButtons, btnClicked);
                break;
            case 1:
                UpdateTeamSize(m_teamBMemberEntries, m_teamBPlayersButtons, btnClicked);
                break;
            case 2:
                UpdateTeamSize(m_spectatorMemberEntries, m_spectatorButtons, btnClicked);
                break;
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

    public void PopulateTeam(
        int teamSize,
        IEnumerable<LobbyPlayerInfo> teamPlayerInfo,
        UITeamMemberEntry[] teamMemberEntries)
    {
        if (teamSize > teamMemberEntries.Length)
        {
            teamSize = teamMemberEntries.Length;
        }

        bool[] usedSlots = new bool[teamSize];
        if (teamPlayerInfo != null)
        {
            foreach (LobbyPlayerInfo gameOwner in teamPlayerInfo)
            {
                int freeSlot = teamSize;
                for (int i = 0; i < usedSlots.Length; i++)
                {
                    if (!usedSlots[i])
                    {
                        freeSlot = i;
                        break;
                    }
                }

                if (freeSlot < teamSize && gameOwner.IsGameOwner)
                {
                    if (gameOwner.CustomGameVisualSlot != 0
                        && 0 < gameOwner.CustomGameVisualSlot
                        && gameOwner.CustomGameVisualSlot < usedSlots.Length + 1
                        && !usedSlots[gameOwner.CustomGameVisualSlot - 1])
                    {
                        freeSlot = gameOwner.CustomGameVisualSlot - 1;
                    }

                    teamMemberEntries[freeSlot].SetTeamPlayerInfo(gameOwner);
                    UIManager.SetGameObjectActive(teamMemberEntries[freeSlot], true);
                    usedSlots[freeSlot] = true;
                    break;
                }
            }

            foreach (LobbyPlayerInfo player in teamPlayerInfo)
            {
                int freeSlot = teamSize;
                for (int i = 0; i < usedSlots.Length; i++)
                {
                    if (!usedSlots[i])
                    {
                        freeSlot = i;
                        break;
                    }
                }

                if (freeSlot < teamSize
                    && !player.IsNPCBot
                    && !player.IsGameOwner)
                {
                    if (player.CustomGameVisualSlot != 0
                        && 0 < player.CustomGameVisualSlot
                        && player.CustomGameVisualSlot < usedSlots.Length + 1
                        && !usedSlots[player.CustomGameVisualSlot - 1])
                    {
                        freeSlot = player.CustomGameVisualSlot - 1;
                    }

                    teamMemberEntries[freeSlot].SetTeamPlayerInfo(player);
                    UIManager.SetGameObjectActive(teamMemberEntries[freeSlot], true);
                    usedSlots[freeSlot] = true;
                }
            }

            foreach (LobbyPlayerInfo bot in teamPlayerInfo)
            {
                int freeSlot = teamSize;
                for (int i = 0; i < usedSlots.Length; i++)
                {
                    if (!usedSlots[i])
                    {
                        freeSlot = i;
                        break;
                    }
                }

                if (freeSlot < teamSize && bot.IsNPCBot)
                {
                    if (bot.CustomGameVisualSlot != 0
                        && 0 < bot.CustomGameVisualSlot
                        && bot.CustomGameVisualSlot < usedSlots.Length + 1
                        && !usedSlots[bot.CustomGameVisualSlot - 1])
                    {
                        freeSlot = bot.CustomGameVisualSlot - 1;
                    }

                    teamMemberEntries[freeSlot].SetTeamPlayerInfo(bot);
                    UIManager.SetGameObjectActive(teamMemberEntries[freeSlot], true);
                    usedSlots[freeSlot] = true;
                }
            }
        }

        for (int i = 0; i < teamMemberEntries.Length; i++)
        {
            if (i < teamSize)
            {
                if (!usedSlots[i])
                {
                    teamMemberEntries[i].SetTeamPlayerInfo(null);
                }

                UIManager.SetGameObjectActive(teamMemberEntries[i], true);
            }
            else
            {
                teamMemberEntries[i].SetEmptyPlayerInfo();
                UIManager.SetGameObjectActive(teamMemberEntries[i], false);
            }
        }
    }

    public void SetInteractable(bool interactable)
    {
        foreach (MapSelectButton btn in m_theMapList)
        {
            btn.ToggleBtn.SetClickable(interactable);
        }

        foreach (UITeamSizeButton btn in m_teamAPlayersButtons)
        {
            btn.m_btnHitBox.interactable = interactable;
        }

        foreach (UITeamSizeButton btn in m_teamBPlayersButtons)
        {
            btn.m_btnHitBox.interactable = interactable;
        }

        foreach (UITeamSizeButton btn in m_spectatorButtons)
        {
            btn.m_btnHitBox.interactable = interactable;
        }

        m_roundTime.interactable = interactable;
        m_maxRoundTime.interactable = interactable;
    }

    public void AddBot(UITeamMemberEntry teamMemberEntry, CharacterType characterType = CharacterType.None)
    {
        if (characterType == CharacterType.None)
        {
            characterType = GetUnusedBotCharacter(teamMemberEntry.GetTeamId());
        }

        teamMemberEntry.SetTeamPlayerInfo(
            new LobbyPlayerInfo
            {
                IsNPCBot = true,
                Difficulty = BotDifficulty.Hard,
                TeamId = teamMemberEntry.GetTeamId(),
                CharacterInfo =
                {
                    CharacterType = characterType
                },
                Handle = GameWideData.Get().GetCharacterResourceLink(characterType).GetDisplayName()
            });
        UpdateClickedHelper(false);
    }

    public void RemoveBot(UITeamMemberEntry teamMemberEntry)
    {
        RemovePlayer(teamMemberEntry.GetPlayerInfo());
        UpdateClickedHelper(false);
    }

    public void SetControllingPlayerInfo(UITeamMemberEntry teamMemberEntry, LobbyPlayerInfo controllingPlayerInfo)
    {
        if (teamMemberEntry.m_playerInfo.IsRemoteControlled)
        {
            teamMemberEntry.m_playerInfo.ControllingPlayerId = 0;
            teamMemberEntry.m_playerInfo.IsNPCBot = true;
        }

        if (controllingPlayerInfo != null)
        {
            LobbyPlayerInfo lobbyPlayerInfo = controllingPlayerInfo.Clone();
            lobbyPlayerInfo.IsGameOwner = false;
            lobbyPlayerInfo.Handle = $"{controllingPlayerInfo.GetHandle()}";
            lobbyPlayerInfo.PlayerId = 0;
            lobbyPlayerInfo.CharacterInfo = teamMemberEntry.m_playerInfo.CharacterInfo.Clone();
            lobbyPlayerInfo.ControllingPlayerId = controllingPlayerInfo.PlayerId;
            lobbyPlayerInfo.TeamId = teamMemberEntry.GetTeamId();
            teamMemberEntry.SetTeamPlayerInfo(lobbyPlayerInfo);
            UpdateClickedHelper(false);
        }
        else
        {
            AddBot(teamMemberEntry, teamMemberEntry.m_playerInfo.CharacterType);
        }
    }

    public void KickPlayer(UITeamMemberEntry teamMemberEntry)
    {
        RemovePlayer(teamMemberEntry.GetPlayerInfo());
        UpdateClickedHelper(false);
    }

    public void RemovePlayer(LobbyPlayerInfo playerInfo)
    {
        if (playerInfo == null)
        {
            return;
        }

        UITeamMemberEntry[] teamMemberEntries = GetTeamMemberEntries(playerInfo.TeamId);
        bool flag = false;
        for (int i = 0; i < teamMemberEntries.Length; i++)
        {
            if (teamMemberEntries[i].GetPlayerInfo() == playerInfo)
            {
                flag = true;
            }

            if (!flag)
            {
                continue;
            }

            if (i + 1 < teamMemberEntries.Length)
            {
                teamMemberEntries[i].SetTeamPlayerInfo(teamMemberEntries[i + 1].GetPlayerInfo());
                teamMemberEntries[i + 1].SetTeamPlayerInfo(null);
            }
            else
            {
                teamMemberEntries[i].SetTeamPlayerInfo(null);
            }
        }
    }

    private UITeamMemberEntry[] GetTeamMemberEntries(Team team)
    {
        switch (team)
        {
            case Team.TeamA:
                return m_teamAMemberEntries;
            case Team.TeamB:
                return m_teamBMemberEntries;
            case Team.Spectator:
                return m_spectatorMemberEntries;
            default:
                throw new Exception("unrecognized team");
        }
    }

    private int GetNumValidTeamMemberEntries(Team team)
    {
        int num = 0;
        UITeamMemberEntry[] teamMemberEntries = GetTeamMemberEntries(team);
        foreach (UITeamMemberEntry uITeamMemberEntry in teamMemberEntries)
        {
            if (uITeamMemberEntry.m_playerInfo != null)
            {
                num++;
            }
        }

        return num;
    }

    public void AddPlayer(LobbyPlayerInfo playerInfo)
    {
        if (playerInfo == null)
        {
            return;
        }

        foreach (UITeamMemberEntry teamMemberEntry in GetTeamMemberEntries(playerInfo.TeamId))
        {
            if (teamMemberEntry.GetPlayerInfo() == null)
            {
                teamMemberEntry.SetTeamPlayerInfo(playerInfo);
                return;
            }
        }
    }

    public void SwapTeam(UITeamMemberEntry teamMemberEntry)
    {
        LobbyPlayerInfo playerInfo = teamMemberEntry.GetPlayerInfo();
        if (playerInfo == null)
        {
            return;
        }

        Team enemyTeam = playerInfo.TeamId == Team.TeamA ? Team.TeamB : Team.TeamA;
        int enemyTeamMemberNum = GetNumValidTeamMemberEntries(enemyTeam);
        int enemyTeamSize = GetChecked(enemyTeam == Team.TeamA ? m_teamAPlayersButtons : m_teamBPlayersButtons);
        if (enemyTeamMemberNum < enemyTeamSize)
        {
            RemovePlayer(playerInfo);
            playerInfo.TeamId = enemyTeam;
            AddPlayer(playerInfo);
            UpdateClickedHelper(false);
        }
    }

    public void SwapSpectator(UITeamMemberEntry teamMemberEntry)
    {
        LobbyPlayerInfo playerInfo = teamMemberEntry.GetPlayerInfo();
        if (playerInfo == null)
        {
            return;
        }

        foreach (UITeamMemberEntry entry in m_teamAMemberEntries)
        {
            LobbyPlayerInfo entryPlayerInfo = entry.GetPlayerInfo();
            if (entryPlayerInfo != null
                && entryPlayerInfo.IsRemoteControlled
                && entryPlayerInfo.ControllingPlayerId == playerInfo.PlayerId)
            {
                return;
            }
        }

        foreach (UITeamMemberEntry entry in m_teamBMemberEntries)
        {
            LobbyPlayerInfo entryPlayerInfo = entry.GetPlayerInfo();
            if (entryPlayerInfo != null
                && entryPlayerInfo.IsRemoteControlled
                && entryPlayerInfo.ControllingPlayerId == playerInfo.PlayerId)
            {
                return;
            }
        }

        int teamAMemberNum = GetNumValidTeamMemberEntries(Team.TeamA);
        int teamBMemberNum = GetNumValidTeamMemberEntries(Team.TeamB);
        int spectatorNum = GetNumValidTeamMemberEntries(Team.Spectator);
        int teamASize = GetChecked(m_teamAPlayersButtons);
        int teamBSize = GetChecked(m_teamBPlayersButtons);
        int spectatorSize = GetChecked(m_spectatorButtons);
        Team teamId;
        if (playerInfo.TeamId == Team.Spectator)
        {
            if (teamAMemberNum < teamASize)
            {
                teamId = Team.TeamA;
            }
            else
            {
                if (teamBMemberNum >= teamBSize)
                {
                    return;
                }

                teamId = Team.TeamB;
            }
        }
        else
        {
            if (spectatorNum >= spectatorSize)
            {
                return;
            }

            teamId = Team.Spectator;
        }

        RemovePlayer(playerInfo);
        playerInfo.TeamId = teamId;
        AddPlayer(playerInfo);
        UpdateClickedHelper(false);
    }

    public void UpdateTeamSize(
        UITeamMemberEntry[] teamMemberEntries,
        UITeamSizeButton[] teamPlayersButtons,
        UITeamSizeButton btnClicked)
    {
        bool flag = true;
        while (flag)
        {
            flag = false;
            for (int i = teamMemberEntries.Length - 1; i > 0; i--)
            {
                if (teamMemberEntries[i].GetPlayerInfo() != null && teamMemberEntries[i - 1].GetPlayerInfo() == null)
                {
                    teamMemberEntries[i - 1].SetTeamPlayerInfo(teamMemberEntries[i].GetPlayerInfo());
                    teamMemberEntries[i].SetTeamPlayerInfo(null);
                    flag = true;
                }
            }
        }

        int teamMemberNum = 0;
        foreach (UITeamMemberEntry teamMemberEntry in teamMemberEntries)
        {
            if (teamMemberEntry.GetPlayerInfo() != null)
            {
                teamMemberNum++;
            }
        }

        int desiredTeamMemberNum = Mathf.Max(btnClicked.GetIndex(), teamMemberNum);
        for (int i = 0; i < teamMemberEntries.Length; i++)
        {
            if (i < desiredTeamMemberNum)
            {
                if (!teamMemberEntries[i].gameObject.activeInHierarchy)
                {
                    teamMemberEntries[i].SetTeamPlayerInfo(null);
                }

                UIManager.SetGameObjectActive(teamMemberEntries[i], true);
            }
            else if (teamMemberEntries[i].GetPlayerInfo() == null)
            {
                UIManager.SetGameObjectActive(teamMemberEntries[i], false);
            }
        }

        for (int i = 0; i < teamPlayersButtons.Length; i++)
        {
            teamPlayersButtons[i].SetChecked(i == desiredTeamMemberNum);
        }

        UpdateClickedHelper(false);
    }

    public CharacterType GetUnusedBotCharacter(Team team)
    {
        GameManager gameManager = GameManager.Get();
        GameType gameType = GameType.Custom;
        List<CharacterType> characterTypes = gameManager.GameplayOverrides.GetCharacterTypes().ToList();
        characterTypes.Shuffle(m_random);
        foreach (CharacterType current in characterTypes)
        {
            CharacterConfig characterConfig = gameManager.GameplayOverrides.GetCharacterConfig(current);
            if (gameManager.GameplayOverrides.IsCharacterAllowedForBots(characterConfig.CharacterType)
                && gameManager.GameplayOverrides.IsCharacterAllowedForGameType(
                    characterConfig.CharacterType,
                    gameType,
                    null,
                    null)
                && !IsCharacterTaken(team, characterConfig.CharacterType))
            {
                return characterConfig.CharacterType;
            }
        }

        throw new Exception("Could not find a bot character type");
    }

    public bool IsCharacterTaken(Team team, CharacterType character)
    {
        foreach (UITeamMemberEntry teamMemberEntry in GetTeamMemberEntries(team))
        {
            if (teamMemberEntry.GetPlayerInfo() != null
                && teamMemberEntry.GetPlayerInfo().CharacterType == character)
            {
                return true;
            }
        }

        return false;
    }
}
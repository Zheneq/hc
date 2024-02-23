using LobbyGameClientMessages;
using System;
using System.Collections.Generic;
using System.Text;
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
            m_mapList.Add(new MapSelectButton
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
        int num = -1;
        int num2 = 0;
        foreach (GameMapConfig gameMapConfig in selection.SubType.GameMapConfigs)
        {
            if (!gameMapConfig.IsActive)
            {
                continue;
            }

            GameWideData.Get().GetMapDisplayName(gameMapConfig.Map);
            if (num2 >= m_mapList.Count)
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
                m_mapList.Add(new MapSelectButton
                {
                    MapConfig = gameMapConfig,
                    ToggleBtn = toggleSwap
                });
            }

            _ToggleSwap toggleBtn = m_mapList[num2].ToggleBtn;
            m_mapList[num2].MapConfig = gameMapConfig;
            UIManager.SetGameObjectActive(toggleBtn, true);
            toggleBtn.gameObject.GetComponent<TextMeshProUGUI>().text =
                GameWideData.Get().GetMapDisplayName(gameMapConfig.Map);
            bool isActive;
            if (ClientGameManager.Get().IsMapInGameType(GameType.Custom, gameMapConfig.Map, out isActive) &&
                !isActive)
            {
                toggleBtn.gameObject.GetComponent<TextMeshProUGUI>().fontStyle |= FontStyles.Strikethrough;
            }
            else
            {
                if (num == -1)
                {
                    num = num2;
                }
            }

            num2++;
        }

        int count = selection.SubType.GameMapConfigs.Count;
        if (num != -1)
        {
            for (int j = 0; j < m_mapList.Count; j++)
            {
                m_mapList[j].ToggleBtn.SetOn(j == num);
            }

            SelectedMapConfig = m_mapList[num].MapConfig;
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
		for (int i = 0; i < GameSubTypeButtons.Count; i++)
		{
			if (GameSubTypeButtons[i] == selected)
			{
				TextMeshProUGUI[] componentsInChildren = m_gameSubTypeDropdownBtn.GetComponentsInChildren<TextMeshProUGUI>(true);
				for (int j = 0; j < componentsInChildren.Length; j++)
				{
					componentsInChildren[j].text = selected.SubType.GetNameAsPayload().ToString();
				}
				Dictionary<ushort, GameSubType> gameTypeSubTypes = ClientGameManager.Get().GetGameTypeSubTypes(GameType.Custom);
				foreach (KeyValuePair<ushort, GameSubType> item in gameTypeSubTypes)
				{
					if (item.Value == selected.SubType)
					{
						SelectedSubTypeMask = item.Key;
						SelectedGameSubtype = item.Value;
						break;
					}
				}
				GameSubTypeButtons[i].DropdownButton.SetSelected(true, true, string.Empty, string.Empty);
				SetupMapButtons(selected);
				SetupOptionRestrictions(selected);
			}
			else
			{
				GameSubTypeButtons[i].DropdownButton.SetSelected(false, true, string.Empty, string.Empty);
			}
		}
		while (true)
		{
			if (SelectedSubTypeMask != 0)
			{
				Debug.Log(new StringBuilder().Append("Selected SubType ").Append(selected.SubType.GetNameAsPayload().Term).Append(" with Mask ").Append(SelectedSubTypeMask).ToString());
			}
			return;
		}
	}

	public void SubTypeClicked(BaseEventData data)
	{
		SetGameSubTypeDropdownListVisible(false);
		int num = 0;
		while (true)
		{
			if (num < GameSubTypeButtons.Count)
			{
				if (GameSubTypeButtons[num].DropdownButton.spriteController.gameObject == (data as PointerEventData).pointerCurrentRaycast.gameObject)
				{
					break;
				}
				num++;
				continue;
			}
			return;
		}
		while (true)
		{
			SubTypeClickedHelper(GameSubTypeButtons[num]);
			return;
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
			using (List<GameSubType>.Enumerator enumerator = gameTypeAvailability.SubTypes.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					GameSubType SubType = enumerator.Current;
					if (!SubType.Requirements.IsNullOrEmpty())
					{
						if (SubType.Requirements.Exists((QueueRequirement p) => !p.DoesApplicantPass(systemInfo, applicant, GameType.Custom, SubType)))
						{
							continue;
						}
					}
					_SelectableBtn selectableBtn = null;
					if (num < componentsInChildren.Length)
					{
						selectableBtn = componentsInChildren[num];
						num++;
					}
					else
					{
						selectableBtn = UnityEngine.Object.Instantiate(m_gameSubTypeItem);
						UIManager.ReparentTransform(selectableBtn.transform, m_gameSubTypeItemParent.transform);
					}
					GameSubTypeButtons.Add(new SubTypeButtonSelection
					{
						DropdownButton = selectableBtn,
						SubType = SubType
					});
					selectableBtn.spriteController.callback = SubTypeClicked;
					selectableBtn.SetSelected(false, false, string.Empty, string.Empty);
					TextMeshProUGUI[] componentsInChildren2 = selectableBtn.GetComponentsInChildren<TextMeshProUGUI>(true);
					for (int i = 0; i < componentsInChildren2.Length; i++)
					{
						componentsInChildren2[i].text = SubType.GetNameAsPayload().ToString();
					}
				}
			}
			RectTransform gameSubtypeContainer = m_gameSubtypeContainer;
			Vector2 sizeDelta = m_gameSubtypeContainer.sizeDelta;
			gameSubtypeContainer.sizeDelta = new Vector2(sizeDelta.x, 16f + 30f * (float)gameTypeAvailability.SubTypes.Count);
		}
		else
		{
			Log.Error("No game sub types for custom!");
		}
		for (int j = num; j < componentsInChildren.Length; j++)
		{
			UIManager.SetGameObjectActive(componentsInChildren[j], false);
		}
		while (true)
		{
			if (GameSubTypeButtons.Count > 0)
			{
				while (true)
				{
					SubTypeClickedHelper(GameSubTypeButtons[0]);
					return;
				}
			}
			return;
		}
	}

	public void SetVisible(bool visible)
	{
		if (visible)
		{
			ClientGameManager clientGameManager = ClientGameManager.Get();
			if (!m_mapList.IsNullOrEmpty())
			{
				for (int i = 0; i < m_mapList.Count; i++)
				{
					if (m_mapList[i].MapConfig == null)
					{
						continue;
					}

					bool isActive;
					if (clientGameManager.IsMapInGameType(GameType.Custom, m_mapList[i].MapConfig.Map, out isActive))
					{
						if (!isActive)
						{
							m_mapList[i].ToggleBtn.gameObject.GetComponent<TextMeshProUGUI>().fontStyle |= FontStyles.Strikethrough;
							continue;
						}
					}
					m_mapList[i].ToggleBtn.gameObject.GetComponent<TextMeshProUGUI>().fontStyle &= (FontStyles)(-65);
				}
			}
			SetGameSubTypeDropdownListVisible(false);
		}
		UIManager.Get().SetSceneVisible(GetSceneType(), visible, new SceneVisibilityParameters());
		for (int j = 0; j < m_containers.Length; j++)
		{
			UIManager.SetGameObjectActive(m_containers[j], visible);
		}
		while (true)
		{
			m_cancelButton.ResetMouseState();
			m_createButton.ResetMouseState();
			return;
		}
	}

	public void Setup()
	{
		LobbyGameConfig lobbyGameConfig = new LobbyGameConfig();
		int index = MathUtil.Clamp(lobbyGameConfig.TeamAPlayers, 0, m_teamAPlayersButtons.Length);
		int index2 = MathUtil.Clamp(lobbyGameConfig.TeamBPlayers, 0, m_teamBPlayersButtons.Length);
		int index3 = MathUtil.Clamp(lobbyGameConfig.Spectators, 0, m_spectatorButtons.Length);
		SetChecked(m_teamAPlayersButtons, index);
		SetChecked(m_teamBPlayersButtons, index2);
		SetChecked(m_spectatorButtons, index3);
		m_timeBankToggle.isOn = true;
		m_roundTime.text = lobbyGameConfig.TurnTime.ToString();
		int num = UIFrontEnd.Get().m_playerPanel.m_playerHandle.LastIndexOf('#');
		int num2 = Mathf.Min(UIFrontEnd.Get().m_playerPanel.m_playerHandle.Length, MAX_GAMENAME_SIZE - 7);
		if (num < num2)
		{
			num2 = num;
		}
		m_gameNameInputField.text = string.Format(StringUtil.TR("CreateGameTitle", "Global"), UIFrontEnd.Get().m_playerPanel.m_playerHandle.Substring(0, num2));
		PopulateSubtypes();
	}

	public void MapClicked(_ToggleSwap btn)
	{
		for (int i = 0; i < m_mapList.Count; i++)
		{
			bool isActive;
			if (!(m_mapList[i].ToggleBtn == btn) || !ClientGameManager.Get().IsMapInGameType(GameType.Custom, m_mapList[i].MapConfig.Map, out isActive))
			{
				continue;
			}
			if (isActive)
			{
				continue;
			}
			while (true)
			{
				m_mapList[i].ToggleBtn.SetOn(false);
				UIFrontEnd.PlaySound(FrontEndButtonSounds.NotifyWarning);
				return;
			}
		}
		while (true)
		{
			UIFrontEnd.PlaySound(FrontEndButtonSounds.OptionsChoice);
			for (int j = 0; j < m_mapList.Count; j++)
			{
				if (m_mapList[j].ToggleBtn == btn)
				{
					SelectedMapConfig = m_mapList[j].MapConfig;
					m_mapList[j].ToggleBtn.SetOn(true);
				}
				else
				{
					m_mapList[j].ToggleBtn.SetOn(false);
				}
			}
			while (true)
			{
				switch (7)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
	}

	public void CreateClicked(BaseEventData data)
	{
		LobbyGameConfig lobbyGameConfig = new LobbyGameConfig();
		List<GameMapConfig> list = new List<GameMapConfig>();
		list.Add(SelectedMapConfig);
		lobbyGameConfig.Map = SelectedMapConfig.Map;
		int @checked = GetChecked(m_teamAPlayersButtons);
		int checked2 = GetChecked(m_teamBPlayersButtons);
		int checked3 = GetChecked(m_spectatorButtons);
		string text = m_gameNameInputField.text;
		if (text.IsNullOrEmpty())
		{
			int length = Mathf.Min(UIFrontEnd.Get().m_playerPanel.m_playerHandle.Length, MAX_GAMENAME_SIZE - 7);
			text = string.Format(StringUtil.TR("CreateGameTitle", "Global"), UIFrontEnd.Get().m_playerPanel.m_playerHandle.Substring(0, length));
		}
		lobbyGameConfig.RoomName = text;
		lobbyGameConfig.TeamAPlayers = @checked;
		lobbyGameConfig.TeamBPlayers = checked2;
		lobbyGameConfig.Spectators = checked3;
		lobbyGameConfig.GameType = GameType.Custom;
		lobbyGameConfig.SubTypes = new List<GameSubType>();
		lobbyGameConfig.SubTypes.Add(SelectedGameSubtype);
		lobbyGameConfig.InstanceSubTypeBit = SelectedSubTypeMask;
		if (lobbyGameConfig.InstanceSubType.GameOverrides == null)
		{
			lobbyGameConfig.InstanceSubType.GameOverrides = new GameValueOverrides();
		}
		try
		{
			lobbyGameConfig.InstanceSubType.GameOverrides.SetTimeSpanOverride(GameValueOverrides.OverrideAbleGameValue.TurnTimeSpan, GameSubType.ConformTurnTimeSpanFromSeconds(double.Parse(m_roundTime.text)));
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
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						lobbyGameConfig.InstanceSubType.GameOverrides.SetIntOverride(GameValueOverrides.OverrideAbleGameValue.InitialTimeBankConsumables, null);
						goto end_IL_01b1;
					}
				}
			}
			lobbyGameConfig.InstanceSubType.GameOverrides.SetIntOverride(GameValueOverrides.OverrideAbleGameValue.InitialTimeBankConsumables, 0);
			end_IL_01b1:;
		}
		catch (Exception exception2)
		{
			Log.Exception(exception2);
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
		if (btnClicked.GetTeam() == 0)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
				{
					for (int i = 0; i < m_teamAPlayersButtons.Length; i++)
					{
						m_teamAPlayersButtons[i].SetChecked(m_teamAPlayersButtons[i] == btnClicked);
					}
					return;
				}
				}
			}
		}
		if (btnClicked.GetTeam() == 1)
		{
			for (int j = 0; j < m_teamBPlayersButtons.Length; j++)
			{
				m_teamBPlayersButtons[j].SetChecked(m_teamBPlayersButtons[j] == btnClicked);
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
		if (btnClicked.GetTeam() != 2)
		{
			return;
		}
		while (true)
		{
			for (int k = 0; k < m_spectatorButtons.Length; k++)
			{
				m_spectatorButtons[k].SetChecked(m_spectatorButtons[k] == btnClicked);
			}
			while (true)
			{
				switch (3)
				{
				default:
					return;
				case 0:
					break;
				}
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
			if (!buttons[i].IsChecked())
			{
				continue;
			}
			while (true)
			{
				return i;
			}
		}
		while (true)
		{
			return 0;
		}
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (UIFrontEnd.Get().CanMenuEscape())
			{
				if (!UIFrontEnd.Get().m_frontEndChatConsole.EscapeJustPressed())
				{
					CancelClicked(null);
				}
			}
		}
		if (!Input.GetMouseButtonDown(0))
		{
			return;
		}
		bool flag = true;
		bool flag2 = EventSystem.current.currentSelectedGameObject != null && EventSystem.current.currentSelectedGameObject.GetComponentInParent<FriendListBannerMenu>() != null;
		if (EventSystem.current != null)
		{
			if (EventSystem.current.IsPointerOverGameObject(-1))
			{
				StandaloneInputModuleWithEventDataAccess component = EventSystem.current.gameObject.GetComponent<StandaloneInputModuleWithEventDataAccess>();
				if (component != null && component.GetLastPointerEventDataPublic(-1).pointerEnter != null)
				{
					_SelectableBtn componentInParent = component.GetLastPointerEventDataPublic(-1).pointerEnter.GetComponentInParent<_SelectableBtn>();
					bool flag3 = false;
					if (componentInParent == null)
					{
						while (true)
						{
							if (componentInParent != null)
							{
								if (componentInParent == m_gameSubTypeDropdownBtn)
								{
									flag3 = true;
									break;
								}
								componentInParent = componentInParent.transform.parent.GetComponentInParent<_SelectableBtn>();
								continue;
							}
							break;
						}
					}
					if (!(componentInParent != null) && !flag3)
					{
						if (!flag2)
						{
							goto IL_01c6;
						}
					}
					flag = false;
				}
			}
		}
		goto IL_01c6;
		IL_01c6:
		if (!flag)
		{
			return;
		}
		while (true)
		{
			SetGameSubTypeDropdownListVisible(false);
			return;
		}
	}
}

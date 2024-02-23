using LobbyGameClientMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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

	private System.Random m_random = new System.Random();

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
		if (m_maxRoundTime != null)
		{
			if (m_maxRoundTime.transform.parent != null)
			{
				UIManager.SetGameObjectActive(m_maxRoundTime.transform.parent, false);
			}
		}
		_ToggleSwap[] componentsInChildren = m_mapListContainer.transform.GetComponentsInChildren<_ToggleSwap>(true);
		ScrollRect scrollRect = m_mapListContainer.GetComponentInParent<ScrollRect>();
		foreach (_ToggleSwap toggleSwap in componentsInChildren)
		{
			if (scrollRect != null)
			{
				_MouseEventPasser mouseEventPasser = toggleSwap.m_onButton.gameObject.AddComponent<_MouseEventPasser>();
				mouseEventPasser.AddNewHandler(scrollRect);
				_MouseEventPasser mouseEventPasser2 = toggleSwap.m_offButton.gameObject.AddComponent<_MouseEventPasser>();
				mouseEventPasser2.AddNewHandler(scrollRect);
				UIEventTriggerUtils.AddListener(toggleSwap.gameObject, EventTriggerType.Scroll, delegate(BaseEventData data)
				{
					scrollRect.OnScroll((PointerEventData)data);
				});
			}
			toggleSwap.transform.SetParent(m_mapListContainer.transform);
			toggleSwap.transform.localPosition = Vector3.zero;
			toggleSwap.transform.localScale = Vector3.one;
			toggleSwap.changedNotify = MapClicked;
			m_theMapList.Add(new MapSelectButton
			{
				MapConfig = null,
				ToggleBtn = toggleSwap
			});
		}
		for (int j = 0; j < m_teamAPlayersButtons.Length; j++)
		{
			m_teamAPlayersButtons[j].SetChecked(false);
			m_teamAPlayersButtons[j].SetTeam(0);
			m_teamAPlayersButtons[j].SetIndex(j);
			m_teamAPlayersButtons[j].m_callback = TeamSizeButtonClicked;
		}
		for (int k = 0; k < m_teamBPlayersButtons.Length; k++)
		{
			m_teamBPlayersButtons[k].SetChecked(false);
			m_teamBPlayersButtons[k].SetTeam(1);
			m_teamBPlayersButtons[k].SetIndex(k);
			m_teamBPlayersButtons[k].m_callback = TeamSizeButtonClicked;
		}
		while (true)
		{
			for (int l = 0; l < m_spectatorButtons.Length; l++)
			{
				m_spectatorButtons[l].SetChecked(false);
				m_spectatorButtons[l].SetTeam(2);
				m_spectatorButtons[l].SetIndex(l);
				m_spectatorButtons[l].m_callback = TeamSizeButtonClicked;
			}
			while (true)
			{
				for (int m = 0; m < m_teamAMemberEntries.Length; m++)
				{
					m_teamAMemberEntries[m].SetTeamId(Team.TeamA);
				}
				while (true)
				{
					for (int n = 0; n < m_teamBMemberEntries.Length; n++)
					{
						m_teamBMemberEntries[n].SetTeamId(Team.TeamB);
					}
					for (int num = 0; num < m_spectatorMemberEntries.Length; num++)
					{
						m_spectatorMemberEntries[num].SetTeamId(Team.Spectator);
					}
					while (true)
					{
						UIManager.SetGameObjectActive(m_teamBPlayersButtons[0], false);
						UIManager.SetGameObjectActive(m_teamAPlayersButtons[0], false);
						SetVisible(false);
						base.Awake();
						return;
					}
				}
			}
		}
	}

	private void EditGameName(string name)
	{
		if (m_gameNameInputField.text.Length <= UICreateGameScreen.MAX_GAMENAME_SIZE)
		{
			return;
		}
		while (true)
		{
			m_gameNameInputField.text = m_gameNameInputField.text.Substring(0, UICreateGameScreen.MAX_GAMENAME_SIZE);
			return;
		}
	}

	private void EditRoundTime(string name)
	{
		if (m_roundTime.text.IsNullOrEmpty())
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		try
		{
			string text = Mathf.FloorToInt((float)GameSubType.ConformTurnTimeSpanFromSeconds(double.Parse(m_roundTime.text)).TotalSeconds).ToString();
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
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return;
				}
			}
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
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
				{
					for (int i = 0; i < m_teamAPlayersButtons.Length; i++)
					{
						m_teamAPlayersButtons[i].SetChecked(i == 4);
						m_teamAPlayersButtons[i].Clickable = false;
					}
					while (true)
					{
						switch (3)
						{
						case 0:
							break;
						default:
						{
							for (int j = 0; j < m_teamBPlayersButtons.Length; j++)
							{
								m_teamBPlayersButtons[j].SetChecked(j == 4);
								m_teamBPlayersButtons[j].Clickable = false;
							}
							return;
						}
						}
					}
				}
				}
			}
		}
		for (int k = 0; k < m_teamAPlayersButtons.Length; k++)
		{
			m_teamAPlayersButtons[k].Clickable = true;
		}
		while (true)
		{
			for (int l = 0; l < m_teamBPlayersButtons.Length; l++)
			{
				m_teamBPlayersButtons[l].Clickable = true;
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
				for (int i = 0; i < m_theMapList.Count; i++)
				{
					if (m_theMapList[i].MapConfig == null)
					{
						continue;
					}

					bool isActive;
					if (clientGameManager.IsMapInGameType(GameType.Custom, m_theMapList[i].MapConfig.Map, out isActive))
					{
						if (!isActive)
						{
							m_theMapList[i].ToggleBtn.gameObject.GetComponent<TextMeshProUGUI>().fontStyle |= FontStyles.Strikethrough;
							continue;
						}
					}
					m_theMapList[i].ToggleBtn.gameObject.GetComponent<TextMeshProUGUI>().fontStyle &= (FontStyles)(-65);
				}
			}
		}
		m_cancelButton.ResetMouseState();
		m_updateButton.ResetMouseState();
		for (int j = 0; j < m_containers.Length; j++)
		{
			UIManager.SetGameObjectActive(m_containers[j], visible);
		}
		while (true)
		{
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
					if (AppState.GetCurrent() == AppState_CharacterSelect.Get())
					{
						if (!UICharacterSelectScreenController.Get().m_changeFreelancerBtn.gameObject.activeSelf)
						{
							UIManager.SetGameObjectActive(UICharacterSelectScreenController.Get().m_changeFreelancerBtn, true);
						}
					}
					UICharacterSelectScreenController.Get().UpdateReadyCancelButtonStates();
				}
			}
			if (UICharacterScreen.Get() != null)
			{
				while (true)
				{
					UICharacterScreen.Get().DoRefreshFunctions(1);
					return;
				}
			}
			return;
		}
	}

	private void SetupMapButtons(LobbyGameConfig gameConfig)
	{
		GameSubType instanceSubType = gameConfig.InstanceSubType;
		ScrollRect scrollRect = m_mapListContainer.GetComponentInParent<ScrollRect>();
		int num = 0;
		for (int i = 0; i < instanceSubType.GameMapConfigs.Count; i++)
		{
			GameMapConfig gameMapConfig = instanceSubType.GameMapConfigs[i];
			if (!gameMapConfig.IsActive)
			{
				continue;
			}
			GameWideData.Get().GetMapDisplayName(gameMapConfig.Map);
			if (num >= m_theMapList.Count)
			{
				_ToggleSwap toggleSwap = UnityEngine.Object.Instantiate(m_mapItemPrefab);
				if (scrollRect != null)
				{
					_MouseEventPasser mouseEventPasser = toggleSwap.m_onButton.gameObject.AddComponent<_MouseEventPasser>();
					mouseEventPasser.AddNewHandler(scrollRect);
					_MouseEventPasser mouseEventPasser2 = toggleSwap.m_offButton.gameObject.AddComponent<_MouseEventPasser>();
					mouseEventPasser2.AddNewHandler(scrollRect);
					UIEventTriggerUtils.AddListener(toggleSwap.gameObject, EventTriggerType.Scroll, delegate(BaseEventData data)
					{
						scrollRect.OnScroll((PointerEventData)data);
					});
				}
				toggleSwap.transform.SetParent(m_mapListContainer.transform);
				toggleSwap.transform.localPosition = Vector3.zero;
				toggleSwap.transform.localScale = Vector3.one;
				toggleSwap.changedNotify = MapClicked;
				m_theMapList.Add(new MapSelectButton
				{
					MapConfig = gameMapConfig,
					ToggleBtn = toggleSwap
				});
			}
			_ToggleSwap toggleBtn = m_theMapList[num].ToggleBtn;
			m_theMapList[num].MapConfig = gameMapConfig;
			toggleBtn.SetOn(gameConfig.Map == gameMapConfig.Map);
			UIManager.SetGameObjectActive(toggleBtn, true);
			toggleBtn.gameObject.GetComponent<TextMeshProUGUI>().text = GameWideData.Get().GetMapDisplayName(gameMapConfig.Map);
			bool isActive;
			if (ClientGameManager.Get().IsMapInGameType(GameType.Custom, gameMapConfig.Map, out isActive))
			{
				if (!isActive)
				{
					toggleBtn.gameObject.GetComponent<TextMeshProUGUI>().fontStyle |= FontStyles.Strikethrough;
				}
			}
			num++;
		}
		while (true)
		{
			int count = instanceSubType.GameMapConfigs.Count;
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
			for (int j = count; j < m_theMapList.Count; j++)
			{
				UIManager.SetGameObjectActive(m_theMapList[j].ToggleBtn, false);
				m_theMapList[j].MapConfig = null;
			}
			while (true)
			{
				switch (1)
				{
				default:
					return;
				case 0:
					break;
				}
			}
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
			if (initialTimeBankConsumables.HasValue)
			{
				int? initialTimeBankConsumables2 = gameConfig.InstanceSubType.GameOverrides.InitialTimeBankConsumables;
				if (initialTimeBankConsumables2.HasValue && initialTimeBankConsumables2.GetValueOrDefault() <= 0)
				{
					isOn = false;
				}
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
		int @checked = GetChecked(m_teamAPlayersButtons);
		int checked2 = GetChecked(m_teamBPlayersButtons);
		int checked3 = GetChecked(m_spectatorButtons);
		PopulateTeam(@checked, teamInfo.TeamAPlayerInfo, m_teamAMemberEntries);
		PopulateTeam(checked2, teamInfo.TeamBPlayerInfo, m_teamBMemberEntries);
		PopulateTeam(checked3, teamInfo.SpectatorInfo, m_spectatorMemberEntries);
	}

	public void MapClicked(_ToggleSwap btn)
	{
		for (int i = 0; i < m_theMapList.Count; i++)
		{
			if (!(m_theMapList[i].ToggleBtn == btn))
			{
				continue;
			}

			bool isActive;
			if (!ClientGameManager.Get().IsMapInGameType(GameType.Custom, m_theMapList[i].MapConfig.Map, out isActive))
			{
				continue;
			}
			if (isActive)
			{
				continue;
			}
			while (true)
			{
				m_theMapList[i].ToggleBtn.SetOn(false);
				UIFrontEnd.PlaySound(FrontEndButtonSounds.NotifyWarning);
				return;
			}
		}
		UIFrontEnd.PlaySound(FrontEndButtonSounds.OptionsChoice);
		for (int j = 0; j < m_theMapList.Count; j++)
		{
			m_theMapList[j].ToggleBtn.SetOn(m_theMapList[j].ToggleBtn == btn);
		}
	}

	private void UpdateClickedHelper(bool closeSettingsWindow = true)
	{
		LobbyGameConfig lobbyGameConfig = new LobbyGameConfig();
		string map = string.Empty;

		foreach (MapSelectButton btn in m_theMapList)
		{
			if (btn.ToggleBtn.IsChecked())
			{
				map = btn.MapConfig.Map;
				break;
			}
		}
		lobbyGameConfig.Map = map;
		lobbyGameConfig.RoomName = m_gameNameInputField.text;
		lobbyGameConfig.TeamAPlayers = GetChecked(m_teamAPlayersButtons);
		lobbyGameConfig.TeamBPlayers = GetChecked(m_teamBPlayersButtons);
		lobbyGameConfig.Spectators = GetChecked(m_spectatorButtons);
		lobbyGameConfig.GameType = GameType.Custom;
		lobbyGameConfig.SubTypes = new List<GameSubType> { GameManager.Get().GameConfig.InstanceSubType };
		lobbyGameConfig.InstanceSubTypeBit = GameManager.Get().GameConfig.InstanceSubTypeBit;
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
			lobbyGameConfig.GameOptionFlags = lobbyGameConfig.GameOptionFlags.WithGameOption(GameOptionFlag.AllowDuplicateCharacters);
		}
		if (m_allowPausing.isOn)
		{
			lobbyGameConfig.GameOptionFlags = lobbyGameConfig.GameOptionFlags.WithGameOption(GameOptionFlag.AllowPausing);
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
		BalancedTeamRequest request = new BalancedTeamRequest();
		request.Slots = new List<BalanceTeamSlot>();
		IEnumerator<UITeamMemberEntry> enumerator = m_teamAMemberEntries.Union(m_teamBMemberEntries).GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				UITeamMemberEntry current = enumerator.Current;
				LobbyPlayerInfo playerInfo = current.GetPlayerInfo();
				if (playerInfo != null)
				{
					if (!playerInfo.IsSpectator)
					{
						request.Slots.Add(new BalanceTeamSlot
						{
							Team = current.GetTeamId(),
							PlayerId = playerInfo.PlayerId,
							AccountId = playerInfo.AccountId,
							SelectedCharacter = playerInfo.CharacterType,
							BotDifficulty = playerInfo.Difficulty
						});
					}
				}
			}
		}
		finally
		{
			if (enumerator != null)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
						enumerator.Dispose();
						goto end_IL_00fb;
					}
				}
			}
			end_IL_00fb:;
		}
		ClientGameManager.Get().LobbyInterface.RequestBalancedTeam(request, delegate(BalancedTeamResponse response)
		{
			if (!response.Success)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
					{
						UIFrontEnd.PlaySound(FrontEndButtonSounds.OptionsCancel);
						string text;
						if (response.LocalizedFailure != null)
						{
							text = response.LocalizedFailure.ToString();
						}
						else
						{
							text = response.ErrorMessage;
						}
						string text2 = text;
						TextConsole.Get().Write(new TextConsole.Message
						{
							Text = text2,
							MessageType = ConsoleMessageType.SystemMessage
						});
						return;
					}
					}
				}
			}
			UIFrontEnd.PlaySound(FrontEndButtonSounds.OptionsOK);
			List<LobbyPlayerInfo> list = new List<LobbyPlayerInfo>();
			List<LobbyPlayerInfo> list2 = new List<LobbyPlayerInfo>();
			using (List<BalanceTeamSlot>.Enumerator enumerator2 = response.Slots.GetEnumerator())
			{
				BalanceTeamSlot slot = default(BalanceTeamSlot);
				while (enumerator2.MoveNext())
				{
					slot = enumerator2.Current;
					if (request.Slots.Exists((BalanceTeamSlot p) => p.PlayerId == slot.PlayerId && p.Team != slot.Team))
					{
						IEnumerable<UITeamMemberEntry> source = m_teamAMemberEntries.Union(m_teamBMemberEntries);
						UITeamMemberEntry uITeamMemberEntry = source.FirstOrDefault(delegate(UITeamMemberEntry p)
						{
							int result;
							if (p.GetPlayerInfo() != null)
							{
								result = ((p.GetPlayerInfo().PlayerId == slot.PlayerId) ? 1 : 0);
							}
							else
							{
								result = 0;
							}
							return (byte)result != 0;
						});
						if (uITeamMemberEntry != null)
						{
							LobbyPlayerInfo playerInfo2 = uITeamMemberEntry.GetPlayerInfo();
							if (playerInfo2 != null)
							{
								RemovePlayer(playerInfo2);
								if (slot.Team == Team.TeamA)
								{
									list.Add(playerInfo2);
								}
								else
								{
									list2.Add(playerInfo2);
								}
							}
						}
					}
				}
			}
			foreach (LobbyPlayerInfo item in list)
			{
				item.TeamId = Team.TeamA;
				AddPlayer(item);
			}
			foreach (LobbyPlayerInfo item2 in list2)
			{
				item2.TeamId = Team.TeamB;
				AddPlayer(item2);
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
		if (btnClicked.GetTeam() == 0)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					UpdateTeamSize(m_teamAMemberEntries, m_teamAPlayersButtons, btnClicked);
					return;
				}
			}
		}
		if (btnClicked.GetTeam() == 1)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					UpdateTeamSize(m_teamBMemberEntries, m_teamBPlayersButtons, btnClicked);
					return;
				}
			}
		}
		if (btnClicked.GetTeam() == 2)
		{
			UpdateTeamSize(m_spectatorMemberEntries, m_spectatorButtons, btnClicked);
		}
	}

	private void SetChecked(UITeamSizeButton[] buttons, int index)
	{
		for (int i = 0; i < buttons.Length; i++)
		{
			buttons[i].SetChecked(i == index);
		}
		while (true)
		{
			return;
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

	public void PopulateTeam(int teamSize, IEnumerable<LobbyPlayerInfo> teamPlayerInfo, UITeamMemberEntry[] teamMemberEntries)
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
		for (int i = 0; i < m_theMapList.Count; i++)
		{
			m_theMapList[i].ToggleBtn.SetClickable(interactable);
		}
		while (true)
		{
			for (int j = 0; j < m_teamAPlayersButtons.Length; j++)
			{
				m_teamAPlayersButtons[j].m_btnHitBox.interactable = interactable;
			}
			while (true)
			{
				for (int k = 0; k < m_teamBPlayersButtons.Length; k++)
				{
					m_teamBPlayersButtons[k].m_btnHitBox.interactable = interactable;
				}
				while (true)
				{
					for (int l = 0; l < m_spectatorButtons.Length; l++)
					{
						m_spectatorButtons[l].m_btnHitBox.interactable = interactable;
					}
					m_roundTime.interactable = interactable;
					m_maxRoundTime.interactable = interactable;
					return;
				}
			}
		}
	}

	public void AddBot(UITeamMemberEntry teamMemberEntry, CharacterType characterType = CharacterType.None)
	{
		if (characterType == CharacterType.None)
		{
			characterType = GetUnusedBotCharacter(teamMemberEntry.GetTeamId());
		}
		LobbyPlayerInfo lobbyPlayerInfo = new LobbyPlayerInfo();
		lobbyPlayerInfo.IsNPCBot = true;
		lobbyPlayerInfo.Difficulty = BotDifficulty.Hard;
		lobbyPlayerInfo.TeamId = teamMemberEntry.GetTeamId();
		lobbyPlayerInfo.CharacterInfo.CharacterType = characterType;
		CharacterResourceLink characterResourceLink = GameWideData.Get().GetCharacterResourceLink(lobbyPlayerInfo.CharacterType);
		lobbyPlayerInfo.Handle = characterResourceLink.GetDisplayName();
		teamMemberEntry.SetTeamPlayerInfo(lobbyPlayerInfo);
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
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
				{
					LobbyPlayerInfo lobbyPlayerInfo = controllingPlayerInfo.Clone();
					lobbyPlayerInfo.IsGameOwner = false;
					lobbyPlayerInfo.Handle = new StringBuilder().Append(controllingPlayerInfo.GetHandle()).ToString();
					lobbyPlayerInfo.PlayerId = 0;
					lobbyPlayerInfo.CharacterInfo = teamMemberEntry.m_playerInfo.CharacterInfo.Clone();
					lobbyPlayerInfo.ControllingPlayerId = controllingPlayerInfo.PlayerId;
					lobbyPlayerInfo.TeamId = teamMemberEntry.GetTeamId();
					teamMemberEntry.SetTeamPlayerInfo(lobbyPlayerInfo);
					UpdateClickedHelper(false);
					return;
				}
				}
			}
		}
		AddBot(teamMemberEntry, teamMemberEntry.m_playerInfo.CharacterType);
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
		while (true)
		{
			switch (1)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	private UITeamMemberEntry[] GetTeamMemberEntries(Team team)
	{
		if (team == Team.TeamA)
		{
			return m_teamAMemberEntries;
		}
		if (team == Team.TeamB)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return m_teamBMemberEntries;
				}
			}
		}
		if (team == Team.Spectator)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return m_spectatorMemberEntries;
				}
			}
		}
		throw new Exception("unrecognized team");
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
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		UITeamMemberEntry[] teamMemberEntries = GetTeamMemberEntries(playerInfo.TeamId);
		for (int i = 0; i < teamMemberEntries.Length; i++)
		{
			if (teamMemberEntries[i].GetPlayerInfo() != null)
			{
				continue;
			}
			while (true)
			{
				teamMemberEntries[i].SetTeamPlayerInfo(playerInfo);
				return;
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

	public void SwapTeam(UITeamMemberEntry teamMemberEntry)
	{
		LobbyPlayerInfo playerInfo = teamMemberEntry.GetPlayerInfo();
		if (playerInfo == null)
		{
			return;
		}
		int num;
		if (playerInfo.TeamId == Team.TeamA)
		{
			num = 1;
		}
		else
		{
			num = 0;
		}
		Team team = (Team)num;
		int numValidTeamMemberEntries = GetNumValidTeamMemberEntries(team);
		int @checked;
		if (team == Team.TeamA)
		{
			@checked = GetChecked(m_teamAPlayersButtons);
		}
		else
		{
			@checked = GetChecked(m_teamBPlayersButtons);
		}
		int num2 = @checked;
		if (numValidTeamMemberEntries < num2)
		{
			RemovePlayer(playerInfo);
			playerInfo.TeamId = team;
			AddPlayer(playerInfo);
			UpdateClickedHelper(false);
		}
	}

	public void SwapSpectator(UITeamMemberEntry teamMemberEntry)
	{
		LobbyPlayerInfo playerInfo = teamMemberEntry.GetPlayerInfo();
		if (playerInfo == null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		for (int i = 0; i < m_teamAMemberEntries.Length; i++)
		{
			LobbyPlayerInfo playerInfo2 = m_teamAMemberEntries[i].GetPlayerInfo();
			if (playerInfo2 == null)
			{
				continue;
			}
			if (playerInfo2.IsRemoteControlled)
			{
				if (playerInfo2.ControllingPlayerId == playerInfo.PlayerId)
				{
					return;
				}
			}
		}
		for (int j = 0; j < m_teamBMemberEntries.Length; j++)
		{
			LobbyPlayerInfo playerInfo3 = m_teamBMemberEntries[j].GetPlayerInfo();
			if (playerInfo3 == null)
			{
				continue;
			}
			if (!playerInfo3.IsRemoteControlled)
			{
				continue;
			}
			if (playerInfo3.ControllingPlayerId == playerInfo.PlayerId)
			{
				while (true)
				{
					switch (2)
					{
					default:
						return;
					case 0:
						break;
					}
				}
			}
		}
		while (true)
		{
			int numValidTeamMemberEntries = GetNumValidTeamMemberEntries(Team.TeamA);
			int numValidTeamMemberEntries2 = GetNumValidTeamMemberEntries(Team.TeamB);
			int numValidTeamMemberEntries3 = GetNumValidTeamMemberEntries(Team.Spectator);
			int @checked = GetChecked(m_teamAPlayersButtons);
			int checked2 = GetChecked(m_teamBPlayersButtons);
			int checked3 = GetChecked(m_spectatorButtons);
			Team teamId = playerInfo.TeamId;
			if (playerInfo.TeamId == Team.Spectator)
			{
				if (numValidTeamMemberEntries < @checked)
				{
					teamId = Team.TeamA;
				}
				else
				{
					if (numValidTeamMemberEntries2 >= checked2)
					{
						return;
					}
					teamId = Team.TeamB;
				}
			}
			else
			{
				if (numValidTeamMemberEntries3 >= checked3)
				{
					return;
				}
				teamId = Team.Spectator;
			}
			RemovePlayer(playerInfo);
			playerInfo.TeamId = teamId;
			AddPlayer(playerInfo);
			UpdateClickedHelper(false);
			return;
		}
	}

	public void UpdateTeamSize(UITeamMemberEntry[] teamMemberEntries, UITeamSizeButton[] teamPlayersButtons, UITeamSizeButton btnClicked)
	{
		bool flag = true;
		while (flag)
		{
			flag = false;
			for (int num = teamMemberEntries.Length - 1; num > 0; num--)
			{
				if (teamMemberEntries[num].GetPlayerInfo() != null && teamMemberEntries[num - 1].GetPlayerInfo() == null)
				{
					teamMemberEntries[num - 1].SetTeamPlayerInfo(teamMemberEntries[num].GetPlayerInfo());
					teamMemberEntries[num].SetTeamPlayerInfo(null);
					flag = true;
				}
			}
		}
		while (true)
		{
			int num2 = 0;
			for (int i = 0; i < teamMemberEntries.Length; i++)
			{
				if (teamMemberEntries[i].GetPlayerInfo() != null)
				{
					num2++;
				}
			}
			while (true)
			{
				int num3 = Mathf.Max(btnClicked.GetIndex(), num2);
				for (int j = 0; j < teamMemberEntries.Length; j++)
				{
					if (j < num3)
					{
						if (!teamMemberEntries[j].gameObject.activeInHierarchy)
						{
							teamMemberEntries[j].SetTeamPlayerInfo(null);
						}
						UIManager.SetGameObjectActive(teamMemberEntries[j], true);
					}
					else if (teamMemberEntries[j].GetPlayerInfo() == null)
					{
						UIManager.SetGameObjectActive(teamMemberEntries[j], false);
					}
				}
				while (true)
				{
					for (int k = 0; k < teamPlayersButtons.Length; k++)
					{
						teamPlayersButtons[k].SetChecked(k == num3);
					}
					while (true)
					{
						UpdateClickedHelper(false);
						return;
					}
				}
			}
		}
	}

	public CharacterType GetUnusedBotCharacter(Team team)
	{
		GameManager gameManager = GameManager.Get();
		GameType gameType = GameType.Custom;
		List<CharacterType> list = gameManager.GameplayOverrides.GetCharacterTypes().ToList();
		list.Shuffle(m_random);
		using (List<CharacterType>.Enumerator enumerator = list.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				CharacterType current = enumerator.Current;
				CharacterConfig characterConfig = gameManager.GameplayOverrides.GetCharacterConfig(current);
				if (gameManager.GameplayOverrides.IsCharacterAllowedForBots(characterConfig.CharacterType) && gameManager.GameplayOverrides.IsCharacterAllowedForGameType(characterConfig.CharacterType, gameType, null, null))
				{
					if (!IsCharacterTaken(team, characterConfig.CharacterType))
					{
						while (true)
						{
							switch (4)
							{
							case 0:
								break;
							default:
								return characterConfig.CharacterType;
							}
						}
					}
				}
			}
		}
		throw new Exception("Could not find a bot character type");
	}

	public bool IsCharacterTaken(Team team, CharacterType character)
	{
		UITeamMemberEntry[] teamMemberEntries = GetTeamMemberEntries(team);
		UITeamMemberEntry[] array = teamMemberEntries;
		foreach (UITeamMemberEntry uITeamMemberEntry in array)
		{
			if (uITeamMemberEntry.GetPlayerInfo() != null)
			{
				if (uITeamMemberEntry.GetPlayerInfo().CharacterType == character)
				{
					return true;
				}
			}
		}
		while (true)
		{
			return false;
		}
	}
}

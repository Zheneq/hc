using System;
using System.Collections.Generic;
using System.Linq;
using LobbyGameClientMessages;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIGameSettingsPanel : UIScene
{
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

	public List<UIGameSettingsPanel.MapSelectButton> m_theMapList = new List<UIGameSettingsPanel.MapSelectButton>();

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
		return UIGameSettingsPanel.s_instance;
	}

	public override SceneType GetSceneType()
	{
		return SceneType.CustomGameSettings;
	}

	public override void Awake()
	{
		UIGameSettingsPanel.s_instance = this;
		this.m_cancelButton.callback = new _ButtonSwapSprite.ButtonClickCallback(this.CancelClicked);
		this.m_updateButton.callback = new _ButtonSwapSprite.ButtonClickCallback(this.UpdateClicked);
		this.m_balanceTeams.callback = new _ButtonSwapSprite.ButtonClickCallback(this.BalanceTeamsClicked);
		this.m_updateButton.m_soundToPlay = FrontEndButtonSounds.Generic;
		this.m_gameNameInputField.onValueChanged.AddListener(new UnityAction<string>(this.EditGameName));
		this.m_roundTime.onValueChanged.AddListener(new UnityAction<string>(this.EditRoundTime));
		if (this.m_maxRoundTime != null)
		{
			if (this.m_maxRoundTime.transform.parent != null)
			{
				UIManager.SetGameObjectActive(this.m_maxRoundTime.transform.parent, false, null);
			}
		}
		_ToggleSwap[] componentsInChildren = this.m_mapListContainer.transform.GetComponentsInChildren<_ToggleSwap>(true);
		ScrollRect scrollRect = this.m_mapListContainer.GetComponentInParent<ScrollRect>();
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
			toggleSwap.transform.SetParent(this.m_mapListContainer.transform);
			toggleSwap.transform.localPosition = Vector3.zero;
			toggleSwap.transform.localScale = Vector3.one;
			toggleSwap.changedNotify = new _ToggleSwap.NotifyChanged(this.MapClicked);
			this.m_theMapList.Add(new UIGameSettingsPanel.MapSelectButton
			{
				MapConfig = null,
				ToggleBtn = toggleSwap
			});
		}
		for (int j = 0; j < this.m_teamAPlayersButtons.Length; j++)
		{
			this.m_teamAPlayersButtons[j].SetChecked(false);
			this.m_teamAPlayersButtons[j].SetTeam(0);
			this.m_teamAPlayersButtons[j].SetIndex(j);
			this.m_teamAPlayersButtons[j].m_callback = new UITeamSizeButton.NotifyClickDelegate(this.TeamSizeButtonClicked);
		}
		for (int k = 0; k < this.m_teamBPlayersButtons.Length; k++)
		{
			this.m_teamBPlayersButtons[k].SetChecked(false);
			this.m_teamBPlayersButtons[k].SetTeam(1);
			this.m_teamBPlayersButtons[k].SetIndex(k);
			this.m_teamBPlayersButtons[k].m_callback = new UITeamSizeButton.NotifyClickDelegate(this.TeamSizeButtonClicked);
		}
		for (int l = 0; l < this.m_spectatorButtons.Length; l++)
		{
			this.m_spectatorButtons[l].SetChecked(false);
			this.m_spectatorButtons[l].SetTeam(2);
			this.m_spectatorButtons[l].SetIndex(l);
			this.m_spectatorButtons[l].m_callback = new UITeamSizeButton.NotifyClickDelegate(this.TeamSizeButtonClicked);
		}
		for (int m = 0; m < this.m_teamAMemberEntries.Length; m++)
		{
			this.m_teamAMemberEntries[m].SetTeamId(Team.TeamA);
		}
		for (int n = 0; n < this.m_teamBMemberEntries.Length; n++)
		{
			this.m_teamBMemberEntries[n].SetTeamId(Team.TeamB);
		}
		for (int num = 0; num < this.m_spectatorMemberEntries.Length; num++)
		{
			this.m_spectatorMemberEntries[num].SetTeamId(Team.Spectator);
		}
		UIManager.SetGameObjectActive(this.m_teamBPlayersButtons[0], false, null);
		UIManager.SetGameObjectActive(this.m_teamAPlayersButtons[0], false, null);
		this.SetVisible(false);
		base.Awake();
	}

	private void EditGameName(string name)
	{
		if (this.m_gameNameInputField.text.Length > UICreateGameScreen.MAX_GAMENAME_SIZE)
		{
			this.m_gameNameInputField.text = this.m_gameNameInputField.text.Substring(0, UICreateGameScreen.MAX_GAMENAME_SIZE);
		}
	}

	private void EditRoundTime(string name)
	{
		if (this.m_roundTime.text.IsNullOrEmpty())
		{
			return;
		}
		try
		{
			string text = Mathf.FloorToInt((float)GameSubType.ConformTurnTimeSpanFromSeconds(double.Parse(this.m_roundTime.text)).TotalSeconds).ToString();
			this.m_roundTime.text = text;
		}
		catch (FormatException)
		{
			this.m_roundTime.text = GameManager.Get().GameConfig.TurnTime.ToString();
		}
	}

	private void EditMaxRoundTime(string name)
	{
		if (this.m_maxRoundTime.text.IsNullOrEmpty())
		{
			return;
		}
		try
		{
			this.m_maxRoundTime.text = int.Parse(this.m_maxRoundTime.text).ToString();
		}
		catch (FormatException)
		{
			this.m_maxRoundTime.text = string.Empty;
		}
	}

	private void SetupOptionRestrictions(GameSubType GameSubtype)
	{
		if (GameSubtype.HasMod(GameSubType.SubTypeMods.RankedFreelancerSelection))
		{
			for (int i = 0; i < this.m_teamAPlayersButtons.Length; i++)
			{
				this.m_teamAPlayersButtons[i].SetChecked(i == 4);
				this.m_teamAPlayersButtons[i].Clickable = false;
			}
			for (int j = 0; j < this.m_teamBPlayersButtons.Length; j++)
			{
				this.m_teamBPlayersButtons[j].SetChecked(j == 4);
				this.m_teamBPlayersButtons[j].Clickable = false;
			}
		}
		else
		{
			for (int k = 0; k < this.m_teamAPlayersButtons.Length; k++)
			{
				this.m_teamAPlayersButtons[k].Clickable = true;
			}
			for (int l = 0; l < this.m_teamBPlayersButtons.Length; l++)
			{
				this.m_teamBPlayersButtons[l].Clickable = true;
			}
		}
	}

	public void SetVisible(bool visible)
	{
		this.m_lastVisible = visible;
		if (visible)
		{
			ClientGameManager clientGameManager = ClientGameManager.Get();
			this.SetupOptionRestrictions(GameManager.Get().GameConfig.InstanceSubType);
			if (!this.m_theMapList.IsNullOrEmpty<UIGameSettingsPanel.MapSelectButton>())
			{
				for (int i = 0; i < this.m_theMapList.Count; i++)
				{
					if (this.m_theMapList[i].MapConfig != null)
					{
						bool flag;
						if (clientGameManager.IsMapInGameType(GameType.Custom, this.m_theMapList[i].MapConfig.Map, out flag))
						{
							if (!flag)
							{
								this.m_theMapList[i].ToggleBtn.gameObject.GetComponent<TextMeshProUGUI>().fontStyle |= FontStyles.Strikethrough;
								goto IL_10C;
							}
						}
						this.m_theMapList[i].ToggleBtn.gameObject.GetComponent<TextMeshProUGUI>().fontStyle &= (FontStyles)(-0x41);
					}
					IL_10C:;
				}
			}
		}
		this.m_cancelButton.ResetMouseState();
		this.m_updateButton.ResetMouseState();
		for (int j = 0; j < this.m_containers.Length; j++)
		{
			UIManager.SetGameObjectActive(this.m_containers[j], visible, null);
		}
		if (UICharacterSelectScreenController.Get() != null)
		{
			if (visible)
			{
				UICharacterSelectScreenController.Get().UpdateReadyCancelButtonStates();
				if (UICharacterSelectScreenController.Get().m_changeFreelancerBtn.gameObject.activeSelf)
				{
					UIManager.SetGameObjectActive(UICharacterSelectScreenController.Get().m_changeFreelancerBtn, false, null);
				}
				UICharacterSelectScreenController.Get().SetCharacterSelectVisible(false);
				UICharacterSelectScreenController.Get().m_charSettingsPanel.SetVisible(false, UICharacterSelectCharacterSettingsPanel.TabPanel.None);
				if (UIPlayerProgressPanel.Get().IsVisible())
				{
					UIPlayerProgressPanel.Get().SetVisible(false, true);
				}
			}
			else
			{
				if (AppState.GetCurrent() == AppState_CharacterSelect.Get())
				{
					if (!UICharacterSelectScreenController.Get().m_changeFreelancerBtn.gameObject.activeSelf)
					{
						UIManager.SetGameObjectActive(UICharacterSelectScreenController.Get().m_changeFreelancerBtn, true, null);
					}
				}
				UICharacterSelectScreenController.Get().UpdateReadyCancelButtonStates();
			}
		}
		if (UICharacterScreen.Get() != null)
		{
			UICharacterScreen.Get().DoRefreshFunctions(1);
		}
	}

	private void SetupMapButtons(LobbyGameConfig gameConfig)
	{
		GameSubType instanceSubType = gameConfig.InstanceSubType;
		ScrollRect scrollRect = this.m_mapListContainer.GetComponentInParent<ScrollRect>();
		int num = 0;
		for (int i = 0; i < instanceSubType.GameMapConfigs.Count; i++)
		{
			GameMapConfig gameMapConfig = instanceSubType.GameMapConfigs[i];
			if (gameMapConfig.IsActive)
			{
				GameWideData.Get().GetMapDisplayName(gameMapConfig.Map);
				if (num >= this.m_theMapList.Count)
				{
					_ToggleSwap toggleSwap = UnityEngine.Object.Instantiate<_ToggleSwap>(this.m_mapItemPrefab);
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
					toggleSwap.transform.SetParent(this.m_mapListContainer.transform);
					toggleSwap.transform.localPosition = Vector3.zero;
					toggleSwap.transform.localScale = Vector3.one;
					toggleSwap.changedNotify = new _ToggleSwap.NotifyChanged(this.MapClicked);
					this.m_theMapList.Add(new UIGameSettingsPanel.MapSelectButton
					{
						MapConfig = gameMapConfig,
						ToggleBtn = toggleSwap
					});
				}
				_ToggleSwap toggleBtn = this.m_theMapList[num].ToggleBtn;
				this.m_theMapList[num].MapConfig = gameMapConfig;
				toggleBtn.SetOn(gameConfig.Map == gameMapConfig.Map, false);
				UIManager.SetGameObjectActive(toggleBtn, true, null);
				toggleBtn.gameObject.GetComponent<TextMeshProUGUI>().text = GameWideData.Get().GetMapDisplayName(gameMapConfig.Map);
				bool flag;
				if (ClientGameManager.Get().IsMapInGameType(GameType.Custom, gameMapConfig.Map, out flag))
				{
					if (!flag)
					{
						toggleBtn.gameObject.GetComponent<TextMeshProUGUI>().fontStyle |= FontStyles.Strikethrough;
					}
				}
				num++;
			}
		}
		int count = instanceSubType.GameMapConfigs.Count;
		float y = (this.m_mapListContainer.cellSize.y + this.m_mapListContainer.spacing.y) * (float)count * -1f;
		(this.m_mapListContainer.gameObject.transform as RectTransform).offsetMin = new Vector2((this.m_mapListContainer.gameObject.transform as RectTransform).offsetMin.x, y);
		(this.m_mapListContainer.gameObject.transform as RectTransform).offsetMax = new Vector2((this.m_mapListContainer.gameObject.transform as RectTransform).offsetMax.x, 0f);
		for (int j = count; j < this.m_theMapList.Count; j++)
		{
			UIManager.SetGameObjectActive(this.m_theMapList[j].ToggleBtn, false, null);
			this.m_theMapList[j].MapConfig = null;
		}
	}

	public void Setup(LobbyGameConfig gameConfig, LobbyTeamInfo teamInfo, LobbyPlayerInfo playerInfo)
	{
		this.isSetup = false;
		this.m_teamInfo = teamInfo;
		this.m_playerInfo = playerInfo;
		this.SetChecked(this.m_teamAPlayersButtons, gameConfig.TeamAPlayers);
		this.SetChecked(this.m_teamBPlayersButtons, gameConfig.TeamBPlayers);
		this.SetChecked(this.m_spectatorButtons, gameConfig.Spectators);
		this.PopulateTeam(gameConfig.TeamAPlayers, teamInfo.TeamAPlayerInfo, this.m_teamAMemberEntries);
		this.PopulateTeam(gameConfig.TeamBPlayers, teamInfo.TeamBPlayerInfo, this.m_teamBMemberEntries);
		this.PopulateTeam(gameConfig.Spectators, teamInfo.SpectatorInfo, this.m_spectatorMemberEntries);
		InputField gameNameInputField = this.m_gameNameInputField;
		string text;
		if ((text = gameConfig.RoomName) == null)
		{
			text = string.Empty;
		}
		gameNameInputField.text = text;
		this.m_roundTime.text = gameConfig.TurnTime.ToString();
		this.m_allowDuplicateCharacters.isOn = gameConfig.HasGameOption(GameOptionFlag.AllowDuplicateCharacters);
		this.m_allowPausing.isOn = gameConfig.HasGameOption(GameOptionFlag.AllowPausing);
		bool isOn = true;
		if (gameConfig.InstanceSubType.GameOverrides != null)
		{
			int? initialTimeBankConsumables = gameConfig.InstanceSubType.GameOverrides.InitialTimeBankConsumables;
			if (initialTimeBankConsumables != null)
			{
				if (gameConfig.InstanceSubType.GameOverrides.InitialTimeBankConsumables <= 0)
				{
					isOn = false;
				}
			}
		}
		this.m_useTimeBank.isOn = isOn;
		this.SetupMapButtons(gameConfig);
		this.SetInteractable(this.m_playerInfo.IsGameOwner);
		this.isSetup = true;
	}

	public void UpdateCharacterList(LobbyPlayerInfo playerInfo, LobbyTeamInfo teamInfo, LobbyGameInfo gameInfo)
	{
		this.m_teamInfo = teamInfo;
		this.m_playerInfo = playerInfo;
		int @checked = this.GetChecked(this.m_teamAPlayersButtons);
		int checked2 = this.GetChecked(this.m_teamBPlayersButtons);
		int checked3 = this.GetChecked(this.m_spectatorButtons);
		this.PopulateTeam(@checked, teamInfo.TeamAPlayerInfo, this.m_teamAMemberEntries);
		this.PopulateTeam(checked2, teamInfo.TeamBPlayerInfo, this.m_teamBMemberEntries);
		this.PopulateTeam(checked3, teamInfo.SpectatorInfo, this.m_spectatorMemberEntries);
	}

	public void MapClicked(_ToggleSwap btn)
	{
		for (int i = 0; i < this.m_theMapList.Count; i++)
		{
			if (this.m_theMapList[i].ToggleBtn == btn)
			{
				bool flag;
				if (ClientGameManager.Get().IsMapInGameType(GameType.Custom, this.m_theMapList[i].MapConfig.Map, out flag))
				{
					if (!flag)
					{
						this.m_theMapList[i].ToggleBtn.SetOn(false, false);
						UIFrontEnd.PlaySound(FrontEndButtonSounds.NotifyWarning);
						return;
					}
				}
			}
		}
		UIFrontEnd.PlaySound(FrontEndButtonSounds.OptionsChoice);
		for (int j = 0; j < this.m_theMapList.Count; j++)
		{
			this.m_theMapList[j].ToggleBtn.SetOn(this.m_theMapList[j].ToggleBtn == btn, false);
		}
	}

	private void UpdateClickedHelper(bool closeSettingsWindow = true)
	{
		LobbyGameConfig lobbyGameConfig = new LobbyGameConfig();
		string map = string.Empty;
		int count = this.m_theMapList.Count;
		for (int i = 0; i < count; i++)
		{
			if (this.m_theMapList[i].ToggleBtn.IsChecked())
			{
				map = this.m_theMapList[i].MapConfig.Map;
				IL_6E:
				lobbyGameConfig.Map = map;
				int @checked = this.GetChecked(this.m_teamAPlayersButtons);
				int checked2 = this.GetChecked(this.m_teamBPlayersButtons);
				int checked3 = this.GetChecked(this.m_spectatorButtons);
				lobbyGameConfig.RoomName = this.m_gameNameInputField.text;
				lobbyGameConfig.TeamAPlayers = @checked;
				lobbyGameConfig.TeamBPlayers = checked2;
				lobbyGameConfig.Spectators = checked3;
				lobbyGameConfig.GameType = GameType.Custom;
				lobbyGameConfig.SubTypes = new List<GameSubType>();
				lobbyGameConfig.SubTypes.Add(GameManager.Get().GameConfig.InstanceSubType);
				lobbyGameConfig.InstanceSubTypeBit = GameManager.Get().GameConfig.InstanceSubTypeBit;
				if (lobbyGameConfig.InstanceSubType.GameOverrides == null)
				{
					lobbyGameConfig.InstanceSubType.GameOverrides = new GameValueOverrides();
				}
				try
				{
					lobbyGameConfig.InstanceSubType.GameOverrides.SetTimeSpanOverride(GameValueOverrides.OverrideAbleGameValue.TurnTimeSpan, new TimeSpan?(GameSubType.ConformTurnTimeSpanFromSeconds(double.Parse(this.m_roundTime.text))));
				}
				catch (Exception exception)
				{
					Log.Exception(exception);
				}
				if (this.m_allowDuplicateCharacters.isOn)
				{
					lobbyGameConfig.GameOptionFlags = lobbyGameConfig.GameOptionFlags.WithGameOption(GameOptionFlag.AllowDuplicateCharacters);
				}
				if (this.m_allowPausing.isOn)
				{
					lobbyGameConfig.GameOptionFlags = lobbyGameConfig.GameOptionFlags.WithGameOption(GameOptionFlag.AllowPausing);
				}
				try
				{
					if (this.m_useTimeBank.isOn)
					{
						lobbyGameConfig.InstanceSubType.GameOverrides.SetIntOverride(GameValueOverrides.OverrideAbleGameValue.InitialTimeBankConsumables, null);
					}
					else
					{
						lobbyGameConfig.InstanceSubType.GameOverrides.SetIntOverride(GameValueOverrides.OverrideAbleGameValue.InitialTimeBankConsumables, new int?(0));
					}
				}
				catch (Exception exception2)
				{
					Log.Exception(exception2);
				}
				this.m_teamInfo.TeamPlayerInfo.Clear();
				int num = 1;
				foreach (UITeamMemberEntry uiteamMemberEntry in this.m_teamAMemberEntries)
				{
					LobbyPlayerInfo playerInfo = uiteamMemberEntry.GetPlayerInfo();
					if (playerInfo != null)
					{
						playerInfo.CustomGameVisualSlot = num;
						this.m_teamInfo.TeamPlayerInfo.Add(playerInfo);
					}
					num++;
				}
				num = 1;
				foreach (UITeamMemberEntry uiteamMemberEntry2 in this.m_teamBMemberEntries)
				{
					LobbyPlayerInfo playerInfo2 = uiteamMemberEntry2.GetPlayerInfo();
					if (playerInfo2 != null)
					{
						playerInfo2.CustomGameVisualSlot = num;
						this.m_teamInfo.TeamPlayerInfo.Add(playerInfo2);
					}
					num++;
				}
				num = 1;
				foreach (UITeamMemberEntry uiteamMemberEntry3 in this.m_spectatorMemberEntries)
				{
					LobbyPlayerInfo playerInfo3 = uiteamMemberEntry3.GetPlayerInfo();
					if (playerInfo3 != null)
					{
						playerInfo3.CustomGameVisualSlot = num;
						this.m_teamInfo.TeamPlayerInfo.Add(playerInfo3);
					}
					num++;
				}
				AppState_CharacterSelect.Get().OnUpdateGameSettingsClicked(lobbyGameConfig, this.m_teamInfo, closeSettingsWindow);
				return;
			}
		}
		goto IL_6E;
	}

	public void UpdateClicked(BaseEventData data)
	{
		this.UpdateClickedHelper(true);
	}

	public void BalanceTeamsClicked(BaseEventData data)
	{
		BalancedTeamRequest request = new BalancedTeamRequest();
		request.Slots = new List<BalanceTeamSlot>();
		IEnumerator<UITeamMemberEntry> enumerator = this.m_teamAMemberEntries.Union(this.m_teamBMemberEntries).GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				UITeamMemberEntry uiteamMemberEntry = enumerator.Current;
				LobbyPlayerInfo playerInfo = uiteamMemberEntry.GetPlayerInfo();
				if (playerInfo != null)
				{
					if (!playerInfo.IsSpectator)
					{
						request.Slots.Add(new BalanceTeamSlot
						{
							Team = uiteamMemberEntry.GetTeamId(),
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
				enumerator.Dispose();
			}
		}
		ClientGameManager.Get().LobbyInterface.RequestBalancedTeam(request, delegate(BalancedTeamResponse response)
		{
			if (!response.Success)
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
				}, null);
				return;
			}
			UIFrontEnd.PlaySound(FrontEndButtonSounds.OptionsOK);
			List<LobbyPlayerInfo> list = new List<LobbyPlayerInfo>();
			List<LobbyPlayerInfo> list2 = new List<LobbyPlayerInfo>();
			using (List<BalanceTeamSlot>.Enumerator enumerator2 = response.Slots.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					BalanceTeamSlot slot = enumerator2.Current;
					if (request.Slots.Exists((BalanceTeamSlot p) => p.PlayerId == slot.PlayerId && p.Team != slot.Team))
					{
						IEnumerable<UITeamMemberEntry> source = this.m_teamAMemberEntries.Union(this.m_teamBMemberEntries);
						UITeamMemberEntry uiteamMemberEntry2 = source.FirstOrDefault(delegate(UITeamMemberEntry p)
						{
							bool result;
							if (p.GetPlayerInfo() != null)
							{
								result = (p.GetPlayerInfo().PlayerId == slot.PlayerId);
							}
							else
							{
								result = false;
							}
							return result;
						});
						if (uiteamMemberEntry2 != null)
						{
							LobbyPlayerInfo playerInfo2 = uiteamMemberEntry2.GetPlayerInfo();
							if (playerInfo2 != null)
							{
								this.RemovePlayer(playerInfo2);
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
			foreach (LobbyPlayerInfo lobbyPlayerInfo in list)
			{
				lobbyPlayerInfo.TeamId = Team.TeamA;
				this.AddPlayer(lobbyPlayerInfo);
			}
			foreach (LobbyPlayerInfo lobbyPlayerInfo2 in list2)
			{
				lobbyPlayerInfo2.TeamId = Team.TeamB;
				this.AddPlayer(lobbyPlayerInfo2);
			}
			this.UpdateClickedHelper(false);
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
			this.UpdateTeamSize(this.m_teamAMemberEntries, this.m_teamAPlayersButtons, btnClicked);
		}
		else if (btnClicked.GetTeam() == 1)
		{
			this.UpdateTeamSize(this.m_teamBMemberEntries, this.m_teamBPlayersButtons, btnClicked);
		}
		else if (btnClicked.GetTeam() == 2)
		{
			this.UpdateTeamSize(this.m_spectatorMemberEntries, this.m_spectatorButtons, btnClicked);
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

	public void PopulateTeam(int teamSize, IEnumerable<LobbyPlayerInfo> teamPlayerInfo, UITeamMemberEntry[] teamMemberEntries)
	{
		if (teamSize > teamMemberEntries.Length)
		{
			teamSize = teamMemberEntries.Length;
		}
		bool[] array = new bool[teamSize];
		if (teamPlayerInfo != null)
		{
			IEnumerator<LobbyPlayerInfo> enumerator = teamPlayerInfo.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					LobbyPlayerInfo lobbyPlayerInfo = enumerator.Current;
					int num = teamSize;
					for (int i = 0; i < array.Length; i++)
					{
						if (!array[i])
						{
							num = i;
							break;
						}
					}
					if (num < teamSize)
					{
						if (lobbyPlayerInfo.IsGameOwner)
						{
							if (lobbyPlayerInfo.CustomGameVisualSlot != 0)
							{
								if (0 < lobbyPlayerInfo.CustomGameVisualSlot)
								{
									if (lobbyPlayerInfo.CustomGameVisualSlot < array.Length + 1)
									{
										if (!array[lobbyPlayerInfo.CustomGameVisualSlot - 1])
										{
											num = lobbyPlayerInfo.CustomGameVisualSlot - 1;
										}
									}
								}
							}
							teamMemberEntries[num].SetTeamPlayerInfo(lobbyPlayerInfo);
							UIManager.SetGameObjectActive(teamMemberEntries[num], true, null);
							array[num] = true;
							goto IL_127;
						}
					}
				}
			}
			finally
			{
				if (enumerator != null)
				{
					enumerator.Dispose();
				}
			}
			IL_127:
			IEnumerator<LobbyPlayerInfo> enumerator2 = teamPlayerInfo.GetEnumerator();
			try
			{
				IL_226:
				while (enumerator2.MoveNext())
				{
					LobbyPlayerInfo lobbyPlayerInfo2 = enumerator2.Current;
					int num2 = teamSize;
					int j = 0;
					while (j < array.Length)
					{
						if (!array[j])
						{
							num2 = j;
							IL_176:
							if (num2 >= teamSize)
							{
								goto IL_226;
							}
							if (lobbyPlayerInfo2.IsNPCBot)
							{
								goto IL_226;
							}
							if (!lobbyPlayerInfo2.IsGameOwner)
							{
								if (lobbyPlayerInfo2.CustomGameVisualSlot != 0)
								{
									if (0 < lobbyPlayerInfo2.CustomGameVisualSlot)
									{
										if (lobbyPlayerInfo2.CustomGameVisualSlot < array.Length + 1 && !array[lobbyPlayerInfo2.CustomGameVisualSlot - 1])
										{
											num2 = lobbyPlayerInfo2.CustomGameVisualSlot - 1;
										}
									}
								}
								teamMemberEntries[num2].SetTeamPlayerInfo(lobbyPlayerInfo2);
								UIManager.SetGameObjectActive(teamMemberEntries[num2], true, null);
								array[num2] = true;
								goto IL_226;
							}
							goto IL_226;
						}
						else
						{
							j++;
						}
					}
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						goto IL_176;
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
			IEnumerator<LobbyPlayerInfo> enumerator3 = teamPlayerInfo.GetEnumerator();
			try
			{
				IL_33F:
				while (enumerator3.MoveNext())
				{
					LobbyPlayerInfo lobbyPlayerInfo3 = enumerator3.Current;
					int num3 = teamSize;
					int k = 0;
					while (k < array.Length)
					{
						if (!array[k])
						{
							num3 = k;
							IL_2A5:
							if (num3 >= teamSize)
							{
								goto IL_33F;
							}
							if (lobbyPlayerInfo3.IsNPCBot)
							{
								if (lobbyPlayerInfo3.CustomGameVisualSlot != 0)
								{
									if (0 < lobbyPlayerInfo3.CustomGameVisualSlot)
									{
										if (lobbyPlayerInfo3.CustomGameVisualSlot < array.Length + 1)
										{
											if (!array[lobbyPlayerInfo3.CustomGameVisualSlot - 1])
											{
												num3 = lobbyPlayerInfo3.CustomGameVisualSlot - 1;
											}
										}
									}
								}
								teamMemberEntries[num3].SetTeamPlayerInfo(lobbyPlayerInfo3);
								UIManager.SetGameObjectActive(teamMemberEntries[num3], true, null);
								array[num3] = true;
								goto IL_33F;
							}
							goto IL_33F;
						}
						else
						{
							k++;
						}
					}
					for (;;)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						goto IL_2A5;
					}
				}
			}
			finally
			{
				if (enumerator3 != null)
				{
					enumerator3.Dispose();
				}
			}
		}
		for (int l = 0; l < teamMemberEntries.Length; l++)
		{
			if (l < teamSize)
			{
				if (!array[l])
				{
					teamMemberEntries[l].SetTeamPlayerInfo(null);
				}
				UIManager.SetGameObjectActive(teamMemberEntries[l], true, null);
			}
			else
			{
				teamMemberEntries[l].SetEmptyPlayerInfo();
				UIManager.SetGameObjectActive(teamMemberEntries[l], false, null);
			}
		}
	}

	public void SetInteractable(bool interactable)
	{
		for (int i = 0; i < this.m_theMapList.Count; i++)
		{
			this.m_theMapList[i].ToggleBtn.SetClickable(interactable);
		}
		for (int j = 0; j < this.m_teamAPlayersButtons.Length; j++)
		{
			this.m_teamAPlayersButtons[j].m_btnHitBox.interactable = interactable;
		}
		for (int k = 0; k < this.m_teamBPlayersButtons.Length; k++)
		{
			this.m_teamBPlayersButtons[k].m_btnHitBox.interactable = interactable;
		}
		for (int l = 0; l < this.m_spectatorButtons.Length; l++)
		{
			this.m_spectatorButtons[l].m_btnHitBox.interactable = interactable;
		}
		this.m_roundTime.interactable = interactable;
		this.m_maxRoundTime.interactable = interactable;
	}

	public void AddBot(UITeamMemberEntry teamMemberEntry, CharacterType characterType = CharacterType.None)
	{
		if (characterType == CharacterType.None)
		{
			characterType = this.GetUnusedBotCharacter(teamMemberEntry.GetTeamId());
		}
		LobbyPlayerInfo lobbyPlayerInfo = new LobbyPlayerInfo();
		lobbyPlayerInfo.IsNPCBot = true;
		lobbyPlayerInfo.Difficulty = BotDifficulty.Hard;
		lobbyPlayerInfo.TeamId = teamMemberEntry.GetTeamId();
		lobbyPlayerInfo.CharacterInfo.CharacterType = characterType;
		CharacterResourceLink characterResourceLink = GameWideData.Get().GetCharacterResourceLink(lobbyPlayerInfo.CharacterType);
		lobbyPlayerInfo.Handle = characterResourceLink.GetDisplayName();
		teamMemberEntry.SetTeamPlayerInfo(lobbyPlayerInfo);
		this.UpdateClickedHelper(false);
	}

	public void RemoveBot(UITeamMemberEntry teamMemberEntry)
	{
		this.RemovePlayer(teamMemberEntry.GetPlayerInfo());
		this.UpdateClickedHelper(false);
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
			lobbyPlayerInfo.Handle = string.Format("{0}", controllingPlayerInfo.GetHandle());
			lobbyPlayerInfo.PlayerId = 0;
			lobbyPlayerInfo.CharacterInfo = teamMemberEntry.m_playerInfo.CharacterInfo.Clone();
			lobbyPlayerInfo.ControllingPlayerId = controllingPlayerInfo.PlayerId;
			lobbyPlayerInfo.TeamId = teamMemberEntry.GetTeamId();
			teamMemberEntry.SetTeamPlayerInfo(lobbyPlayerInfo);
			this.UpdateClickedHelper(false);
		}
		else
		{
			this.AddBot(teamMemberEntry, teamMemberEntry.m_playerInfo.CharacterType);
		}
	}

	public void KickPlayer(UITeamMemberEntry teamMemberEntry)
	{
		this.RemovePlayer(teamMemberEntry.GetPlayerInfo());
		this.UpdateClickedHelper(false);
	}

	public void RemovePlayer(LobbyPlayerInfo playerInfo)
	{
		if (playerInfo == null)
		{
			return;
		}
		UITeamMemberEntry[] teamMemberEntries = this.GetTeamMemberEntries(playerInfo.TeamId);
		bool flag = false;
		for (int i = 0; i < teamMemberEntries.Length; i++)
		{
			if (teamMemberEntries[i].GetPlayerInfo() == playerInfo)
			{
				flag = true;
			}
			if (flag)
			{
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
	}

	private UITeamMemberEntry[] GetTeamMemberEntries(Team team)
	{
		if (team == Team.TeamA)
		{
			return this.m_teamAMemberEntries;
		}
		if (team == Team.TeamB)
		{
			return this.m_teamBMemberEntries;
		}
		if (team == Team.Spectator)
		{
			return this.m_spectatorMemberEntries;
		}
		throw new Exception("unrecognized team");
	}

	private int GetNumValidTeamMemberEntries(Team team)
	{
		int num = 0;
		foreach (UITeamMemberEntry uiteamMemberEntry in this.GetTeamMemberEntries(team))
		{
			if (uiteamMemberEntry.m_playerInfo != null)
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
		UITeamMemberEntry[] teamMemberEntries = this.GetTeamMemberEntries(playerInfo.TeamId);
		for (int i = 0; i < teamMemberEntries.Length; i++)
		{
			if (teamMemberEntries[i].GetPlayerInfo() == null)
			{
				teamMemberEntries[i].SetTeamPlayerInfo(playerInfo);
				return;
			}
		}
		for (;;)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			return;
		}
	}

	public void SwapTeam(UITeamMemberEntry teamMemberEntry)
	{
		LobbyPlayerInfo playerInfo = teamMemberEntry.GetPlayerInfo();
		if (playerInfo == null)
		{
			return;
		}
		Team team;
		if (playerInfo.TeamId == Team.TeamA)
		{
			team = Team.TeamB;
		}
		else
		{
			team = Team.TeamA;
		}
		Team team2 = team;
		int numValidTeamMemberEntries = this.GetNumValidTeamMemberEntries(team2);
		int @checked;
		if (team2 == Team.TeamA)
		{
			@checked = this.GetChecked(this.m_teamAPlayersButtons);
		}
		else
		{
			@checked = this.GetChecked(this.m_teamBPlayersButtons);
		}
		int num = @checked;
		if (numValidTeamMemberEntries >= num)
		{
			return;
		}
		this.RemovePlayer(playerInfo);
		playerInfo.TeamId = team2;
		this.AddPlayer(playerInfo);
		this.UpdateClickedHelper(false);
	}

	public void SwapSpectator(UITeamMemberEntry teamMemberEntry)
	{
		LobbyPlayerInfo playerInfo = teamMemberEntry.GetPlayerInfo();
		if (playerInfo == null)
		{
			return;
		}
		for (int i = 0; i < this.m_teamAMemberEntries.Length; i++)
		{
			LobbyPlayerInfo playerInfo2 = this.m_teamAMemberEntries[i].GetPlayerInfo();
			if (playerInfo2 != null)
			{
				if (playerInfo2.IsRemoteControlled)
				{
					if (playerInfo2.ControllingPlayerId == playerInfo.PlayerId)
					{
						return;
					}
				}
			}
		}
		for (int j = 0; j < this.m_teamBMemberEntries.Length; j++)
		{
			LobbyPlayerInfo playerInfo3 = this.m_teamBMemberEntries[j].GetPlayerInfo();
			if (playerInfo3 != null)
			{
				if (playerInfo3.IsRemoteControlled)
				{
					if (playerInfo3.ControllingPlayerId == playerInfo.PlayerId)
					{
						return;
					}
				}
			}
		}
		int numValidTeamMemberEntries = this.GetNumValidTeamMemberEntries(Team.TeamA);
		int numValidTeamMemberEntries2 = this.GetNumValidTeamMemberEntries(Team.TeamB);
		int numValidTeamMemberEntries3 = this.GetNumValidTeamMemberEntries(Team.Spectator);
		int @checked = this.GetChecked(this.m_teamAPlayersButtons);
		int checked2 = this.GetChecked(this.m_teamBPlayersButtons);
		int checked3 = this.GetChecked(this.m_spectatorButtons);
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
		this.RemovePlayer(playerInfo);
		playerInfo.TeamId = teamId;
		this.AddPlayer(playerInfo);
		this.UpdateClickedHelper(false);
	}

	public void UpdateTeamSize(UITeamMemberEntry[] teamMemberEntries, UITeamSizeButton[] teamPlayersButtons, UITeamSizeButton btnClicked)
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
		int num = 0;
		for (int j = 0; j < teamMemberEntries.Length; j++)
		{
			if (teamMemberEntries[j].GetPlayerInfo() != null)
			{
				num++;
			}
		}
		int num2 = Mathf.Max(btnClicked.GetIndex(), num);
		for (int k = 0; k < teamMemberEntries.Length; k++)
		{
			if (k < num2)
			{
				if (!teamMemberEntries[k].gameObject.activeInHierarchy)
				{
					teamMemberEntries[k].SetTeamPlayerInfo(null);
				}
				UIManager.SetGameObjectActive(teamMemberEntries[k], true, null);
			}
			else if (teamMemberEntries[k].GetPlayerInfo() == null)
			{
				UIManager.SetGameObjectActive(teamMemberEntries[k], false, null);
			}
		}
		for (int l = 0; l < teamPlayersButtons.Length; l++)
		{
			teamPlayersButtons[l].SetChecked(l == num2);
		}
		this.UpdateClickedHelper(false);
	}

	public CharacterType GetUnusedBotCharacter(Team team)
	{
		GameManager gameManager = GameManager.Get();
		GameType gameType = GameType.Custom;
		List<CharacterType> list = gameManager.GameplayOverrides.GetCharacterTypes().ToList<CharacterType>();
		list.Shuffle(this.m_random);
		using (List<CharacterType>.Enumerator enumerator = list.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				CharacterType characterType = enumerator.Current;
				CharacterConfig characterConfig = gameManager.GameplayOverrides.GetCharacterConfig(characterType);
				if (gameManager.GameplayOverrides.IsCharacterAllowedForBots(characterConfig.CharacterType) && gameManager.GameplayOverrides.IsCharacterAllowedForGameType(characterConfig.CharacterType, gameType, null, null))
				{
					if (!this.IsCharacterTaken(team, characterConfig.CharacterType))
					{
						return characterConfig.CharacterType;
					}
				}
			}
		}
		throw new Exception("Could not find a bot character type");
	}

	public bool IsCharacterTaken(Team team, CharacterType character)
	{
		UITeamMemberEntry[] teamMemberEntries = this.GetTeamMemberEntries(team);
		foreach (UITeamMemberEntry uiteamMemberEntry in teamMemberEntries)
		{
			if (uiteamMemberEntry.GetPlayerInfo() != null)
			{
				if (uiteamMemberEntry.GetPlayerInfo().CharacterType == character)
				{
					return true;
				}
			}
		}
		return false;
	}

	public class MapSelectButton
	{
		public _ToggleSwap ToggleBtn;

		public GameMapConfig MapConfig;
	}
}

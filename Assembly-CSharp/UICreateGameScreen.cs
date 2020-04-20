using System;
using System.Collections.Generic;
using LobbyGameClientMessages;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UICreateGameScreen : UIScene
{
	public static int MAX_GAMENAME_SIZE = 0x15;

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

	public List<UICreateGameScreen.MapSelectButton> m_mapList = new List<UICreateGameScreen.MapSelectButton>();

	public RectTransform[] m_containers;

	public _SelectableBtn m_gameSubTypeDropdownBtn;

	public RectTransform m_gameSubtypeContainer;

	public LayoutGroup m_gameSubTypeItemParent;

	public _SelectableBtn m_gameSubTypeItem;

	private static UICreateGameScreen s_instance;

	private List<UICreateGameScreen.SubTypeButtonSelection> GameSubTypeButtons = new List<UICreateGameScreen.SubTypeButtonSelection>();

	private ushort SelectedSubTypeMask;

	private GameSubType SelectedGameSubtype;

	private GameMapConfig SelectedMapConfig;

	public static UICreateGameScreen Get()
	{
		return UICreateGameScreen.s_instance;
	}

	public override SceneType GetSceneType()
	{
		return SceneType.CreateGameSettings;
	}

	public override void Awake()
	{
		UICreateGameScreen.s_instance = this;
		base.Awake();
		this.m_cancelButton.callback = new _ButtonSwapSprite.ButtonClickCallback(this.CancelClicked);
		this.m_createButton.callback = new _ButtonSwapSprite.ButtonClickCallback(this.CreateClicked);
		this.m_createButton.m_soundToPlay = FrontEndButtonSounds.Generic;
		for (int i = 0; i < this.m_teamAPlayersButtons.Length; i++)
		{
			this.m_teamAPlayersButtons[i].SetChecked(false);
			this.m_teamAPlayersButtons[i].SetTeam(0);
			this.m_teamAPlayersButtons[i].SetIndex(i);
			this.m_teamAPlayersButtons[i].m_callback = new UITeamSizeButton.NotifyClickDelegate(this.TeamSizeButtonClicked);
		}
		for (int j = 0; j < this.m_teamBPlayersButtons.Length; j++)
		{
			this.m_teamBPlayersButtons[j].SetChecked(false);
			this.m_teamBPlayersButtons[j].SetTeam(1);
			this.m_teamBPlayersButtons[j].SetIndex(j);
			this.m_teamBPlayersButtons[j].m_callback = new UITeamSizeButton.NotifyClickDelegate(this.TeamSizeButtonClicked);
		}
		for (int k = 0; k < this.m_spectatorButtons.Length; k++)
		{
			this.m_spectatorButtons[k].SetChecked(false);
			this.m_spectatorButtons[k].SetTeam(2);
			this.m_spectatorButtons[k].SetIndex(k);
			this.m_spectatorButtons[k].m_callback = new UITeamSizeButton.NotifyClickDelegate(this.TeamSizeButtonClicked);
		}
		ScrollRect scrollRect = this.m_mapListContainer.GetComponentInParent<ScrollRect>();
		if (scrollRect != null)
		{
			_MouseEventPasser mouseEventPasser = scrollRect.verticalScrollbar.gameObject.AddComponent<_MouseEventPasser>();
			mouseEventPasser.AddNewHandler(scrollRect);
			scrollRect.scrollSensitivity = 100f;
		}
		foreach (_ToggleSwap toggleSwap in this.m_mapListContainer.transform.GetComponentsInChildren<_ToggleSwap>(true))
		{
			if (scrollRect != null)
			{
				_MouseEventPasser mouseEventPasser2 = toggleSwap.m_onButton.gameObject.AddComponent<_MouseEventPasser>();
				mouseEventPasser2.AddNewHandler(scrollRect);
				_MouseEventPasser mouseEventPasser3 = toggleSwap.m_offButton.gameObject.AddComponent<_MouseEventPasser>();
				mouseEventPasser3.AddNewHandler(scrollRect);
				UIEventTriggerUtils.AddListener(toggleSwap.gameObject, EventTriggerType.Scroll, delegate(BaseEventData data)
				{
					scrollRect.OnScroll((PointerEventData)data);
				});
			}
			toggleSwap.transform.SetParent(this.m_mapListContainer.transform);
			toggleSwap.transform.localPosition = Vector3.zero;
			toggleSwap.transform.localScale = Vector3.one;
			toggleSwap.changedNotify = new _ToggleSwap.NotifyChanged(this.MapClicked);
			this.m_mapList.Add(new UICreateGameScreen.MapSelectButton
			{
				MapConfig = null,
				ToggleBtn = toggleSwap
			});
		}
		this.m_gameSubTypeDropdownBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.DropdownClicked);
		UIManager.SetGameObjectActive(this.m_gameSubtypeContainer, false, null);
		this.m_gameNameInputField.onValueChanged.AddListener(new UnityAction<string>(this.EditGameName));
		this.m_roundTime.onValueChanged.AddListener(new UnityAction<string>(this.EditRoundTime));
		if (this.m_maxRoundTime != null)
		{
			if (this.m_maxRoundTime.transform.parent != null)
			{
				UIManager.SetGameObjectActive(this.m_maxRoundTime.transform.parent, false, null);
			}
		}
		UIManager.SetGameObjectActive(this.m_teamBPlayersButtons[0], false, null);
		UIManager.SetGameObjectActive(this.m_teamAPlayersButtons[0], false, null);
	}

	private void SetGameSubTypeDropdownListVisible(bool visible)
	{
		UIManager.SetGameObjectActive(this.m_gameSubtypeContainer, visible, null);
	}

	public void DropdownClicked(BaseEventData data)
	{
		this.SetGameSubTypeDropdownListVisible(!this.m_gameSubtypeContainer.gameObject.activeSelf);
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

	private void SetupOptionRestrictions(UICreateGameScreen.SubTypeButtonSelection selection)
	{
		if (selection.SubType.HasMod(GameSubType.SubTypeMods.RankedFreelancerSelection))
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

	private void SetupMapButtons(UICreateGameScreen.SubTypeButtonSelection selection)
	{
		ScrollRect scrollRect = this.m_mapListContainer.GetComponentInParent<ScrollRect>();
		int num = -1;
		int num2 = 0;
		for (int i = 0; i < selection.SubType.GameMapConfigs.Count; i++)
		{
			GameMapConfig gameMapConfig = selection.SubType.GameMapConfigs[i];
			if (gameMapConfig.IsActive)
			{
				GameWideData.Get().GetMapDisplayName(gameMapConfig.Map);
				if (num2 >= this.m_mapList.Count)
				{
					_ToggleSwap toggleSwap = UnityEngine.Object.Instantiate<_ToggleSwap>(this.m_mapListEntryPrefab);
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
					this.m_mapList.Add(new UICreateGameScreen.MapSelectButton
					{
						MapConfig = gameMapConfig,
						ToggleBtn = toggleSwap
					});
				}
				_ToggleSwap toggleBtn = this.m_mapList[num2].ToggleBtn;
				this.m_mapList[num2].MapConfig = gameMapConfig;
				UIManager.SetGameObjectActive(toggleBtn, true, null);
				toggleBtn.gameObject.GetComponent<TextMeshProUGUI>().text = GameWideData.Get().GetMapDisplayName(gameMapConfig.Map);
				bool flag;
				if (!ClientGameManager.Get().IsMapInGameType(GameType.Custom, gameMapConfig.Map, out flag))
				{
					goto IL_21A;
				}
				if (flag)
				{
					goto IL_21A;
				}
				toggleBtn.gameObject.GetComponent<TextMeshProUGUI>().fontStyle |= FontStyles.Strikethrough;
				IL_220:
				num2++;
				goto IL_224;
				IL_21A:
				if (num == -1)
				{
					num = num2;
					goto IL_220;
				}
				goto IL_220;
			}
			IL_224:;
		}
		int count = selection.SubType.GameMapConfigs.Count;
		if (num != -1)
		{
			for (int j = 0; j < this.m_mapList.Count; j++)
			{
				this.m_mapList[j].ToggleBtn.SetOn(j == num, false);
			}
			this.SelectedMapConfig = this.m_mapList[num].MapConfig;
		}
		float y = (this.m_mapListContainer.cellSize.y + this.m_mapListContainer.spacing.y) * (float)count * -1f;
		(this.m_mapListContainer.gameObject.transform as RectTransform).offsetMin = new Vector2((this.m_mapListContainer.gameObject.transform as RectTransform).offsetMin.x, y);
		(this.m_mapListContainer.gameObject.transform as RectTransform).offsetMax = new Vector2((this.m_mapListContainer.gameObject.transform as RectTransform).offsetMax.x, 0f);
		for (int k = count; k < this.m_mapList.Count; k++)
		{
			UIManager.SetGameObjectActive(this.m_mapList[k].ToggleBtn, false, null);
			this.m_mapList[k].MapConfig = null;
		}
	}

	private void SubTypeClickedHelper(UICreateGameScreen.SubTypeButtonSelection selected)
	{
		this.SelectedSubTypeMask = 0;
		this.SelectedGameSubtype = null;
		for (int i = 0; i < this.GameSubTypeButtons.Count; i++)
		{
			if (this.GameSubTypeButtons[i] == selected)
			{
				TextMeshProUGUI[] componentsInChildren = this.m_gameSubTypeDropdownBtn.GetComponentsInChildren<TextMeshProUGUI>(true);
				for (int j = 0; j < componentsInChildren.Length; j++)
				{
					componentsInChildren[j].text = selected.SubType.GetNameAsPayload().ToString();
				}
				Dictionary<ushort, GameSubType> gameTypeSubTypes = ClientGameManager.Get().GetGameTypeSubTypes(GameType.Custom);
				foreach (KeyValuePair<ushort, GameSubType> keyValuePair in gameTypeSubTypes)
				{
					if (keyValuePair.Value == selected.SubType)
					{
						this.SelectedSubTypeMask = keyValuePair.Key;
						this.SelectedGameSubtype = keyValuePair.Value;
						break;
					}
				}
				this.GameSubTypeButtons[i].DropdownButton.SetSelected(true, true, string.Empty, string.Empty);
				this.SetupMapButtons(selected);
				this.SetupOptionRestrictions(selected);
			}
			else
			{
				this.GameSubTypeButtons[i].DropdownButton.SetSelected(false, true, string.Empty, string.Empty);
			}
		}
		if (this.SelectedSubTypeMask != 0)
		{
			Debug.Log(string.Format("Selected SubType {0} with Mask {1}", selected.SubType.GetNameAsPayload().Term, this.SelectedSubTypeMask));
		}
	}

	public void SubTypeClicked(BaseEventData data)
	{
		this.SetGameSubTypeDropdownListVisible(false);
		for (int i = 0; i < this.GameSubTypeButtons.Count; i++)
		{
			if (this.GameSubTypeButtons[i].DropdownButton.spriteController.gameObject == (data as PointerEventData).pointerCurrentRaycast.gameObject)
			{
				this.SubTypeClickedHelper(this.GameSubTypeButtons[i]);
				break;
			}
		}
	}

	private void PopulateSubtypes()
	{
		_SelectableBtn[] componentsInChildren = this.m_gameSubTypeItemParent.GetComponentsInChildren<_SelectableBtn>(true);
		int num = 0;
		this.GameSubTypeButtons.Clear();
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
					if (!SubType.Requirements.IsNullOrEmpty<QueueRequirement>())
					{
						if (SubType.Requirements.Exists((QueueRequirement p) => !p.DoesApplicantPass(systemInfo, applicant, GameType.Custom, SubType)))
						{
							continue;
						}
					}
					_SelectableBtn selectableBtn;
					if (num < componentsInChildren.Length)
					{
						selectableBtn = componentsInChildren[num];
						num++;
					}
					else
					{
						selectableBtn = UnityEngine.Object.Instantiate<_SelectableBtn>(this.m_gameSubTypeItem);
						UIManager.ReparentTransform(selectableBtn.transform, this.m_gameSubTypeItemParent.transform);
					}
					this.GameSubTypeButtons.Add(new UICreateGameScreen.SubTypeButtonSelection
					{
						DropdownButton = selectableBtn,
						SubType = SubType
					});
					selectableBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.SubTypeClicked);
					selectableBtn.SetSelected(false, false, string.Empty, string.Empty);
					TextMeshProUGUI[] componentsInChildren2 = selectableBtn.GetComponentsInChildren<TextMeshProUGUI>(true);
					for (int i = 0; i < componentsInChildren2.Length; i++)
					{
						componentsInChildren2[i].text = SubType.GetNameAsPayload().ToString();
					}
				}
			}
			this.m_gameSubtypeContainer.sizeDelta = new Vector2(this.m_gameSubtypeContainer.sizeDelta.x, 16f + 30f * (float)gameTypeAvailability.SubTypes.Count);
		}
		else
		{
			Log.Error("No game sub types for custom!", new object[0]);
		}
		for (int j = num; j < componentsInChildren.Length; j++)
		{
			UIManager.SetGameObjectActive(componentsInChildren[j], false, null);
		}
		if (this.GameSubTypeButtons.Count > 0)
		{
			this.SubTypeClickedHelper(this.GameSubTypeButtons[0]);
		}
	}

	public void SetVisible(bool visible)
	{
		if (visible)
		{
			ClientGameManager clientGameManager = ClientGameManager.Get();
			if (!this.m_mapList.IsNullOrEmpty<UICreateGameScreen.MapSelectButton>())
			{
				for (int i = 0; i < this.m_mapList.Count; i++)
				{
					if (this.m_mapList[i].MapConfig != null)
					{
						bool flag;
						if (clientGameManager.IsMapInGameType(GameType.Custom, this.m_mapList[i].MapConfig.Map, out flag))
						{
							if (!flag)
							{
								this.m_mapList[i].ToggleBtn.gameObject.GetComponent<TextMeshProUGUI>().fontStyle |= FontStyles.Strikethrough;
								goto IL_F4;
							}
						}
						this.m_mapList[i].ToggleBtn.gameObject.GetComponent<TextMeshProUGUI>().fontStyle &= (FontStyles)(-0x41);
					}
					IL_F4:;
				}
			}
			this.SetGameSubTypeDropdownListVisible(false);
		}
		UIManager.Get().SetSceneVisible(this.GetSceneType(), visible, new SceneVisibilityParameters());
		for (int j = 0; j < this.m_containers.Length; j++)
		{
			UIManager.SetGameObjectActive(this.m_containers[j], visible, null);
		}
		this.m_cancelButton.ResetMouseState();
		this.m_createButton.ResetMouseState();
	}

	public void Setup()
	{
		LobbyGameConfig lobbyGameConfig = new LobbyGameConfig();
		int index = MathUtil.Clamp(lobbyGameConfig.TeamAPlayers, 0, this.m_teamAPlayersButtons.Length);
		int index2 = MathUtil.Clamp(lobbyGameConfig.TeamBPlayers, 0, this.m_teamBPlayersButtons.Length);
		int index3 = MathUtil.Clamp(lobbyGameConfig.Spectators, 0, this.m_spectatorButtons.Length);
		this.SetChecked(this.m_teamAPlayersButtons, index);
		this.SetChecked(this.m_teamBPlayersButtons, index2);
		this.SetChecked(this.m_spectatorButtons, index3);
		this.m_timeBankToggle.isOn = true;
		this.m_roundTime.text = lobbyGameConfig.TurnTime.ToString();
		int num = UIFrontEnd.Get().m_playerPanel.m_playerHandle.LastIndexOf('#');
		int num2 = Mathf.Min(UIFrontEnd.Get().m_playerPanel.m_playerHandle.Length, UICreateGameScreen.MAX_GAMENAME_SIZE - 7);
		if (num < num2)
		{
			num2 = num;
		}
		this.m_gameNameInputField.text = string.Format(StringUtil.TR("CreateGameTitle", "Global"), UIFrontEnd.Get().m_playerPanel.m_playerHandle.Substring(0, num2));
		this.PopulateSubtypes();
	}

	public void MapClicked(_ToggleSwap btn)
	{
		for (int i = 0; i < this.m_mapList.Count; i++)
		{
			bool flag;
			if (this.m_mapList[i].ToggleBtn == btn && ClientGameManager.Get().IsMapInGameType(GameType.Custom, this.m_mapList[i].MapConfig.Map, out flag))
			{
				if (!flag)
				{
					this.m_mapList[i].ToggleBtn.SetOn(false, false);
					UIFrontEnd.PlaySound(FrontEndButtonSounds.NotifyWarning);
					return;
				}
			}
		}
		UIFrontEnd.PlaySound(FrontEndButtonSounds.OptionsChoice);
		for (int j = 0; j < this.m_mapList.Count; j++)
		{
			if (this.m_mapList[j].ToggleBtn == btn)
			{
				this.SelectedMapConfig = this.m_mapList[j].MapConfig;
				this.m_mapList[j].ToggleBtn.SetOn(true, false);
			}
			else
			{
				this.m_mapList[j].ToggleBtn.SetOn(false, false);
			}
		}
	}

	public void CreateClicked(BaseEventData data)
	{
		LobbyGameConfig lobbyGameConfig = new LobbyGameConfig();
		List<GameMapConfig> list = new List<GameMapConfig>();
		list.Add(this.SelectedMapConfig);
		lobbyGameConfig.Map = this.SelectedMapConfig.Map;
		int @checked = this.GetChecked(this.m_teamAPlayersButtons);
		int checked2 = this.GetChecked(this.m_teamBPlayersButtons);
		int checked3 = this.GetChecked(this.m_spectatorButtons);
		string text = this.m_gameNameInputField.text;
		if (text.IsNullOrEmpty())
		{
			int length = Mathf.Min(UIFrontEnd.Get().m_playerPanel.m_playerHandle.Length, UICreateGameScreen.MAX_GAMENAME_SIZE - 7);
			text = string.Format(StringUtil.TR("CreateGameTitle", "Global"), UIFrontEnd.Get().m_playerPanel.m_playerHandle.Substring(0, length));
		}
		lobbyGameConfig.RoomName = text;
		lobbyGameConfig.TeamAPlayers = @checked;
		lobbyGameConfig.TeamBPlayers = checked2;
		lobbyGameConfig.Spectators = checked3;
		lobbyGameConfig.GameType = GameType.Custom;
		lobbyGameConfig.SubTypes = new List<GameSubType>();
		lobbyGameConfig.SubTypes.Add(this.SelectedGameSubtype);
		lobbyGameConfig.InstanceSubTypeBit = this.SelectedSubTypeMask;
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
		lobbyGameConfig.SetGameOption(GameOptionFlag.AllowDuplicateCharacters, this.m_allowDuplicateCharacters.isOn);
		lobbyGameConfig.SetGameOption(GameOptionFlag.AllowPausing, this.m_allowPausing.isOn);
		try
		{
			if (this.m_timeBankToggle.isOn)
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
			for (int i = 0; i < this.m_teamAPlayersButtons.Length; i++)
			{
				this.m_teamAPlayersButtons[i].SetChecked(this.m_teamAPlayersButtons[i] == btnClicked);
			}
		}
		else if (btnClicked.GetTeam() == 1)
		{
			for (int j = 0; j < this.m_teamBPlayersButtons.Length; j++)
			{
				this.m_teamBPlayersButtons[j].SetChecked(this.m_teamBPlayersButtons[j] == btnClicked);
			}
		}
		else if (btnClicked.GetTeam() == 2)
		{
			for (int k = 0; k < this.m_spectatorButtons.Length; k++)
			{
				this.m_spectatorButtons[k].SetChecked(this.m_spectatorButtons[k] == btnClicked);
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
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (UIFrontEnd.Get().CanMenuEscape())
			{
				if (!UIFrontEnd.Get().m_frontEndChatConsole.EscapeJustPressed())
				{
					this.CancelClicked(null);
				}
			}
		}
		if (Input.GetMouseButtonDown(0))
		{
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
							while (componentInParent != null)
							{
								if (componentInParent == this.m_gameSubTypeDropdownBtn)
								{
									flag3 = true;
									goto IL_19E;
								}
								componentInParent = componentInParent.transform.parent.GetComponentInParent<_SelectableBtn>();
							}
						}
						IL_19E:
						if (!(componentInParent != null) && !flag3)
						{
							if (!flag2)
							{
								goto IL_1C6;
							}
						}
						flag = false;
					}
				}
			}
			IL_1C6:
			if (flag)
			{
				this.SetGameSubTypeDropdownListVisible(false);
			}
		}
	}

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
}

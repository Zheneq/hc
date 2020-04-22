using LobbyGameClientMessages;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIPlayCategoryMenu : MonoBehaviour
{
	public enum GameTypeButton
	{
		Practice,
		Solo,
		Cooperative,
		Versus,
		Ranked,
		Custom
	}

	public Animator m_playCategoryAnimator;

	public _SelectableBtn m_PracticeBtn;

	public _SelectableBtn m_SoloBtn;

	public _SelectableBtn m_CooperativeBtn;

	public _SelectableBtn m_VersusBtn;

	public _SelectableBtn m_RankedBtn;

	public _SelectableBtn m_CustomBtn;

	public RectTransform m_installDiscordContainer;

	public _SelectableBtn m_installDiscordBtn;

	public RectTransform m_installJoinContainer;

	public _ToggleSwap m_installJoinBtn;

	private List<_SelectableBtn> m_menuList;

	private bool m_visible;

	private bool m_autoJoinDiscord;

	private Dictionary<GameType, GameTypeAvailability> m_validGameTypes;

	private Scheduler m_taskScheduler;

	private Action m_checkDiscordStatusAction;

	private _SelectableBtn m_gamePadHoverBtn;

	public static UIPlayCategoryMenu Get()
	{
		if (UIFrontEnd.Get() != null)
		{
			if (UIFrontEnd.Get().m_frontEndNavPanel != null)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
						return UIFrontEnd.Get().m_frontEndNavPanel.m_playMenuCatgeory;
					}
				}
			}
		}
		return null;
	}

	private void Awake()
	{
		m_menuList = new List<_SelectableBtn>();
		m_PracticeBtn.spriteController.m_soundToPlay = FrontEndButtonSounds.GameModeSelect;
		m_SoloBtn.spriteController.m_soundToPlay = FrontEndButtonSounds.GameModeSelect;
		m_CooperativeBtn.spriteController.m_soundToPlay = FrontEndButtonSounds.GameModeSelect;
		m_VersusBtn.spriteController.m_soundToPlay = FrontEndButtonSounds.GameModeSelect;
		m_RankedBtn.spriteController.m_soundToPlay = FrontEndButtonSounds.GameModeSelect;
		m_CustomBtn.spriteController.m_soundToPlay = FrontEndButtonSounds.GameModeSelect;
		m_menuList.Add(m_PracticeBtn);
		m_menuList.Add(m_SoloBtn);
		m_menuList.Add(m_CooperativeBtn);
		m_menuList.Add(m_VersusBtn);
		m_menuList.Add(m_RankedBtn);
		m_menuList.Add(m_CustomBtn);
		m_CustomBtn.transform.SetAsLastSibling();
		m_installDiscordContainer.transform.SetAsLastSibling();
		m_installJoinContainer.transform.SetAsLastSibling();
		for (int i = 0; i < m_menuList.Count; i++)
		{
			_SelectableBtn btn = m_menuList[i];
			btn.spriteController.callback = GameTypeClicked;
			btn.spriteController.SetForceHovercallback(true);
			btn.spriteController.SetForceExitCallback(true);
			btn.spriteController.GetComponent<UITooltipHoverObject>().Setup(TooltipType.Titled, (UITooltipBase tooltip) => GameTypeTooltipSetup(tooltip, btn));
		}
		m_visible = true;
		SetVisible(false);
		m_installDiscordBtn.spriteController.callback = InstallDiscordBtnClicked;
		if (Options_UI.Get() != null)
		{
			m_autoJoinDiscord = Options_UI.Get().GetEnableAutoJoinDiscord();
		}
		else
		{
			m_autoJoinDiscord = false;
		}
		m_installJoinBtn.SetOn(m_autoJoinDiscord);
		m_installJoinBtn.changedNotify = DiscordAutoJoinToggleClicked;
		m_taskScheduler = new Scheduler();
		m_checkDiscordStatusAction = delegate
		{
			CheckDiscordStatus();
		};
	}

	private void Update()
	{
		bool flag = false;
		if (Options_UI.Get() != null)
		{
			flag = Options_UI.Get().GetEnableAutoJoinDiscord();
		}
		if (flag != m_autoJoinDiscord)
		{
			m_autoJoinDiscord = flag;
			m_installJoinBtn.SetOn(m_autoJoinDiscord);
		}
		if (!(GameManager.Get() != null))
		{
			return;
		}
		while (true)
		{
			if (GameManager.Get().GameplayOverrides.DisableControlPadInput)
			{
				return;
			}
			while (true)
			{
				if (!m_visible)
				{
					return;
				}
				while (true)
				{
					if (Input.GetButtonDown("GamepadButtonLeftShoulder"))
					{
						m_gamePadHoverBtn.SetSelected(false, false, string.Empty, string.Empty);
						if (m_gamePadHoverBtn == m_PracticeBtn)
						{
							m_gamePadHoverBtn = m_CustomBtn;
						}
						else if (m_gamePadHoverBtn == m_CooperativeBtn)
						{
							m_gamePadHoverBtn = m_PracticeBtn;
						}
						else if (m_gamePadHoverBtn == m_VersusBtn)
						{
							m_gamePadHoverBtn = m_CooperativeBtn;
						}
						else if (m_gamePadHoverBtn == m_RankedBtn)
						{
							m_gamePadHoverBtn = m_VersusBtn;
						}
						else if (m_gamePadHoverBtn == m_CustomBtn)
						{
							m_gamePadHoverBtn = m_RankedBtn;
						}
						m_gamePadHoverBtn.SetSelected(true, false, string.Empty, string.Empty);
					}
					else if (Input.GetButtonDown("GamepadButtonRightShoulder"))
					{
						m_gamePadHoverBtn.SetSelected(false, false, string.Empty, string.Empty);
						if (m_gamePadHoverBtn == m_PracticeBtn)
						{
							m_gamePadHoverBtn = m_CooperativeBtn;
						}
						else if (m_gamePadHoverBtn == m_CooperativeBtn)
						{
							m_gamePadHoverBtn = m_VersusBtn;
						}
						else if (m_gamePadHoverBtn == m_VersusBtn)
						{
							m_gamePadHoverBtn = m_RankedBtn;
						}
						else if (m_gamePadHoverBtn == m_RankedBtn)
						{
							m_gamePadHoverBtn = m_CustomBtn;
						}
						else if (m_gamePadHoverBtn == m_CustomBtn)
						{
							m_gamePadHoverBtn = m_PracticeBtn;
						}
						m_gamePadHoverBtn.SetSelected(true, false, string.Empty, string.Empty);
					}
					if (!Input.GetButtonDown("GamepadButtonA"))
					{
						return;
					}
					if (m_gamePadHoverBtn == m_CustomBtn)
					{
						while (true)
						{
							switch (3)
							{
							case 0:
								break;
							default:
								CustomGameTypeClicked();
								return;
							}
						}
					}
					DoSelectGameType(m_gamePadHoverBtn.spriteController.gameObject);
					return;
				}
			}
		}
	}

	public void UITopCategoryDoneAnimOut()
	{
		UIManager.SetGameObjectActive(base.gameObject, false);
	}

	private void SetButtonClickable(_SelectableBtn btn, bool clickable)
	{
		btn.spriteController.SetClickable(clickable);
		UIManager.SetGameObjectActive(btn.spriteController.m_defaultImage, clickable);
		UIManager.SetGameObjectActive(btn.spriteController.m_hoverImage, clickable);
		UIManager.SetGameObjectActive(btn.spriteController.m_pressedImage, clickable);
	}

	public void UpdateGameTypeAvailability(Dictionary<GameType, GameTypeAvailability> validGameTypes)
	{
		m_validGameTypes = validGameTypes;
		using (Dictionary<GameType, GameTypeAvailability>.Enumerator enumerator = validGameTypes.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<GameType, GameTypeAvailability> current = enumerator.Current;
				bool clickable = false;
				ClientGameManager clientGameManager = ClientGameManager.Get();
				GameType key = current.Key;
				GameTypeAvailability value = current.Value;
				if (value.IsActive)
				{
					if (key == GameType.Ranked)
					{
						clickable = true;
						if (value.Requirements != null)
						{
							List<QueueRequirement> list = value.Requirements.ToList();
							int num = 0;
							while (true)
							{
								if (num >= list.Count)
								{
									break;
								}
								QueueRequirement queueRequirement = list[num];
								if (queueRequirement.Requirement == QueueRequirement.RequirementType.AccessLevel && queueRequirement.DoesApplicantPass(clientGameManager.QueueRequirementSystemInfo, clientGameManager.QueueRequirementApplicant, GameType.Ranked, null))
								{
									clickable = true;
									break;
								}
								num++;
							}
						}
					}
					else
					{
						DateTime? penaltyTimeout = value.PenaltyTimeout;
						if (!penaltyTimeout.HasValue || DateTime.Now >= value.PenaltyTimeout.Value.ToLocalTime())
						{
							if (clientGameManager.MeetsAllQueueRequirements(key))
							{
								clickable = true;
							}
						}
					}
				}
				if (key == GameType.Practice)
				{
					SetButtonClickable(m_PracticeBtn, clickable);
				}
				else if (key == GameType.Coop)
				{
					SetButtonClickable(m_CooperativeBtn, clickable);
				}
				else
				{
					switch (key)
					{
					case GameType.PvP:
						SetButtonClickable(m_VersusBtn, clickable);
						break;
					case GameType.Ranked:
						SetButtonClickable(m_RankedBtn, clickable);
						break;
					}
				}
			}
			while (true)
			{
				switch (4)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
	}

	public void SetMenuButtonsClickable(bool clickable)
	{
		if (ClientGameManager.Get().GroupInfo == null)
		{
			return;
		}
		int num;
		if (ClientGameManager.Get().GroupInfo.InAGroup)
		{
			if (ClientGameManager.Get().GroupInfo.IsLeader)
			{
				num = 1;
				goto IL_006f;
			}
		}
		num = ((!ClientGameManager.Get().GroupInfo.InAGroup) ? 1 : 0);
		goto IL_006f;
		IL_006f:
		bool flag = (byte)num != 0;
		bool flag2 = clickable && UIFrontEnd.Get().m_frontEndNavPanel.m_PlayBtn.IsSelected();
		if (flag2 && flag)
		{
			UpdateGameTypeAvailability(ClientGameManager.Get().GameTypeAvailabilies);
		}
		else
		{
			for (int i = 0; i < m_menuList.Count; i++)
			{
				_SelectableBtn btn = m_menuList[i];
				int clickable2;
				if (flag2)
				{
					clickable2 = (flag ? 1 : 0);
				}
				else
				{
					clickable2 = 0;
				}
				SetButtonClickable(btn, (byte)clickable2 != 0);
			}
		}
		if (ClientGameManager.Get().GroupInfo.InAGroup)
		{
			SetButtonClickable(m_SoloBtn, false);
			SetButtonClickable(m_PracticeBtn, false);
			SetButtonClickable(m_CustomBtn, flag2);
		}
		else
		{
			SetButtonClickable(m_CustomBtn, flag2);
		}
		for (int j = 0; j < m_menuList.Count; j++)
		{
			if (m_menuList[j].spriteController.IsClickable())
			{
				continue;
			}
			if (!m_menuList[j].IsSelected())
			{
				continue;
			}
			int num2 = -1;
			int num3 = 0;
			while (true)
			{
				if (num3 < m_menuList.Count)
				{
					if (m_menuList[num3].spriteController.IsClickable())
					{
						if (m_menuList[num3] != m_CustomBtn)
						{
							num2 = num3;
							break;
						}
					}
					num3++;
					continue;
				}
				break;
			}
			if (num2 == -1)
			{
				continue;
			}
			while (true)
			{
				DoSelectGameType(m_menuList[num2].spriteController.gameObject);
				return;
			}
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

	public void UpdateGroupInfo()
	{
		if (ClientGameManager.Get().GroupInfo.InAGroup)
		{
			SelectGroupGameType();
		}
	}

	private void SelectGroupGameType()
	{
		if (ClientGameManager.Get() == null)
		{
			return;
		}
		while (true)
		{
			if (ClientGameManager.Get().GroupInfo == null)
			{
				return;
			}
			while (true)
			{
				if (m_menuList == null)
				{
					while (true)
					{
						switch (4)
						{
						default:
							return;
						case 0:
							break;
						}
					}
				}
				GameTypeButton gameTypeButton = GameTypeToGameTypeButton(ClientGameManager.Get().GroupInfo.SelectedQueueType);
				if (m_menuList[(int)gameTypeButton].IsSelected())
				{
					return;
				}
				GameType selectedQueueType = ClientGameManager.Get().GroupInfo.SelectedQueueType;
				if (UICharacterSelectScreenController.Get() != null)
				{
					int maxWillFillPerTeam = ClientGameManager.Get().GameTypeAvailabilies[selectedQueueType].MaxWillFillPerTeam;
					UIManager.SetGameObjectActive(UICharacterSelectScreenController.Get().m_miscCharSelectButtons, maxWillFillPerTeam > 0);
					if (maxWillFillPerTeam == 0 && UICharacterScreen.GetCurrentSpecificState().CharacterTypeToDisplay.IsWillFill())
					{
						UIManager.Get().HandleNewSceneStateParameter(new UICharacterScreen.CharacterSelectSceneStateParameters
						{
							ClientRequestToServerSelectCharacter = CharacterType.Scoundrel
						});
					}
				}
				UICharacterSelectScreen.Get().SelectedGameMode(ClientGameManager.Get().GroupInfo.SelectedQueueType);
				if (gameTypeButton == GameTypeButton.Solo)
				{
					m_CooperativeBtn.SetSelected(true, false, string.Empty, string.Empty);
					m_gamePadHoverBtn = m_CooperativeBtn;
					UIManager.Get().HandleNewSceneStateParameter(new UICharacterScreen.CharacterSelectSceneStateParameters
					{
						AllyBotTeammatesSelected = true
					});
				}
				else
				{
					for (int i = 0; i < m_menuList.Count; i++)
					{
						if (i == (int)gameTypeButton)
						{
							m_menuList[i].SetSelected(true, false, string.Empty, string.Empty);
							m_gamePadHoverBtn = m_menuList[i];
						}
						else
						{
							m_menuList[i].SetSelected(false, false, string.Empty, string.Empty);
						}
					}
				}
				UICharacterSelectScreenController.Get().SetupForRanked(ClientGameManager.Get().GroupInfo.SelectedQueueType == GameType.Ranked);
				return;
			}
		}
	}

	public void DiscordAutoJoinToggleClicked(_ToggleSwap btn)
	{
		bool autoJoin = btn.IsChecked();
		ClientGameManager.Get().ConfigureDiscord(autoJoin);
	}

	public void InstallDiscordBtnClicked(BaseEventData data)
	{
		Application.OpenURL("https://discordapp.com/download");
		m_taskScheduler.AddTask(m_checkDiscordStatusAction, TimeSpan.FromSeconds(5.0));
	}

	private void CheckDiscordStatus()
	{
		if (DiscordClientInterface.IsEnabled)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					if (!DiscordClientInterface.IsSdkEnabled)
					{
						if (!DiscordClientInterface.IsInstalled)
						{
							UIManager.SetGameObjectActive(m_installDiscordContainer, true);
							UIManager.SetGameObjectActive(m_installJoinContainer, false);
							m_taskScheduler.AddTask(m_checkDiscordStatusAction, TimeSpan.FromSeconds(5.0));
							return;
						}
					}
					UIManager.SetGameObjectActive(m_installDiscordContainer, false);
					UIManager.SetGameObjectActive(m_installJoinContainer, true);
					m_taskScheduler.RemoveTask(m_checkDiscordStatusAction);
					return;
				}
			}
		}
		UIManager.SetGameObjectActive(m_installDiscordContainer, false);
		UIManager.SetGameObjectActive(m_installJoinContainer, false);
	}

	public bool IsVisible()
	{
		return m_visible;
	}

	public void SetVisible(bool visible)
	{
		if (m_visible == visible)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		m_visible = visible;
		UIManager.SetGameObjectActive(base.gameObject, true);
		if (visible)
		{
			m_playCategoryAnimator.Play("UITopCategoryIN", 0, 0f);
			SelectGroupGameType();
			CheckDiscordStatus();
		}
		else
		{
			m_playCategoryAnimator.Play("UITopCategoryOUT", 0, 0f);
		}
		for (int i = 0; i < m_menuList.Count; i++)
		{
			m_menuList[i].spriteController.SetClickable(visible);
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

	private GameTypeButton GameTypeToGameTypeButton(GameType type)
	{
		GameTypeButton result = GameTypeButton.Versus;
		switch (type)
		{
		case GameType.Coop:
			result = GameTypeButton.Cooperative;
			break;
		case GameType.Practice:
			result = GameTypeButton.Practice;
			break;
		case GameType.Solo:
			result = GameTypeButton.Solo;
			break;
		case GameType.PvP:
			result = GameTypeButton.Versus;
			break;
		case GameType.Ranked:
			result = GameTypeButton.Ranked;
			break;
		}
		return result;
	}

	private GameType GameTypeButtonToGameType(GameTypeButton btn)
	{
		GameType result = GameType.None;
		switch (btn)
		{
		case GameTypeButton.Cooperative:
			result = GameType.Coop;
			break;
		case GameTypeButton.Practice:
			result = GameType.Practice;
			break;
		case GameTypeButton.Ranked:
			result = GameType.Ranked;
			break;
		case GameTypeButton.Solo:
			result = GameType.Solo;
			break;
		case GameTypeButton.Versus:
			result = GameType.PvP;
			break;
		}
		return result;
	}

	private bool GameTypeTooltipSetup(UITooltipBase tooltip, _SelectableBtn btn)
	{
		if (!m_visible)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return false;
				}
			}
		}
		if (AppState_GroupCharacterSelect.Get() == AppState.GetCurrent())
		{
			ClientGameManager clientGameManager = ClientGameManager.Get();
			if (!btn.spriteController.IsClickable())
			{
				if (GameManager.Get().QueueInfo == null)
				{
					while (true)
					{
						string text;
						UITitledTooltip uITitledTooltip;
						switch (3)
						{
						case 0:
							break;
						default:
							{
								GameType gameType = GameType.None;
								if (btn == m_PracticeBtn)
								{
									gameType = GameType.Practice;
								}
								else if (btn == m_SoloBtn)
								{
									gameType = GameType.Solo;
								}
								else if (btn == m_CooperativeBtn)
								{
									gameType = GameType.Coop;
								}
								else if (btn == m_VersusBtn)
								{
									gameType = GameType.PvP;
								}
								else if (btn == m_RankedBtn)
								{
									gameType = GameType.Ranked;
								}
								else if (btn == m_CustomBtn)
								{
									gameType = GameType.Custom;
								}
								if (m_validGameTypes == null)
								{
									UpdateGameTypeAvailability(clientGameManager.GameTypeAvailabilies);
								}
								LocalizationPayload blockingQueueRestriction = clientGameManager.GetBlockingQueueRestriction(gameType);
								LobbyPlayerGroupInfo groupInfo = clientGameManager.GroupInfo;
								text = null;
								if (blockingQueueRestriction != null)
								{
									text = blockingQueueRestriction.ToString();
								}
								else if (groupInfo != null && groupInfo.InAGroup && !groupInfo.IsLeader)
								{
									text = StringUtil.TR("OnlyLeaderCanChange", "Global");
								}
								else
								{
									if (clientGameManager.GameTypeAvailabilies.TryGetValue(gameType, out GameTypeAvailability value))
									{
										DateTime? penaltyTimeout = value.PenaltyTimeout;
										if (penaltyTimeout.HasValue)
										{
											if (value.PenaltyTimeout.HasValue)
											{
												text = LocalizationPayload.Create("QueueDodgePenaltyBlocksQueueEntry", "Matchmaking", LocalizationArg_Handle.Create(clientGameManager.Handle)).ToString();
												goto IL_0253;
											}
										}
									}
									text = ((groupInfo == null || !groupInfo.InAGroup || gameType != 0) ? StringUtil.TR("GameModeUnavailable", "Global") : StringUtil.TR("MustLeaveGroupBody", "Global"));
								}
								goto IL_0253;
							}
							IL_0253:
							uITitledTooltip = (tooltip as UITitledTooltip);
							uITitledTooltip.Setup(StringUtil.TR("GameModeDisabled", "Global"), text, string.Empty);
							return true;
						}
					}
				}
			}
		}
		return false;
	}

	private void DoSelectGameType(GameObject btnHit)
	{
		UIFrontEnd.PlaySound(FrontEndButtonSounds.PlayCategorySelect);
		int num = -1;
		for (int i = 0; i < m_menuList.Count; i++)
		{
			if (m_menuList[i].spriteController.gameObject == btnHit)
			{
				num = i;
				m_menuList[i].SetSelected(true, false, string.Empty, string.Empty);
				m_gamePadHoverBtn = m_menuList[i];
			}
			else
			{
				m_menuList[i].SetSelected(false, false, string.Empty, string.Empty);
			}
		}
		while (true)
		{
			if (num == -1)
			{
				return;
			}
			while (true)
			{
				GameType gameType = GameTypeButtonToGameType((GameTypeButton)num);
				AppState_GroupCharacterSelect.Get().SelectedGameMode(gameType);
				UICharacterSelectScreenController.Get().SetupForRanked(gameType == GameType.Ranked);
				if (btnHit == m_PracticeBtn.spriteController.gameObject)
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							break;
						default:
							AnnouncerSounds.GetAnnouncerSounds().PlayAnnouncementByEnum(AnnouncerSounds.AnnouncerEvent.Practice);
							return;
						}
					}
				}
				if (btnHit == m_CooperativeBtn.spriteController.gameObject)
				{
					AnnouncerSounds.GetAnnouncerSounds().PlayAnnouncementByEnum(AnnouncerSounds.AnnouncerEvent.CoOp);
					return;
				}
				if (btnHit == m_VersusBtn.spriteController.gameObject)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							break;
						default:
							AnnouncerSounds.GetAnnouncerSounds().PlayAnnouncementByEnum(AnnouncerSounds.AnnouncerEvent.Pvp);
							return;
						}
					}
				}
				if (btnHit == m_SoloBtn.spriteController.gameObject)
				{
					AnnouncerSounds.GetAnnouncerSounds().PlayAnnouncementByEnum(AnnouncerSounds.AnnouncerEvent.Solo);
					return;
				}
				if (btnHit == m_CustomBtn.spriteController.gameObject)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							break;
						default:
							AnnouncerSounds.GetAnnouncerSounds().PlayAnnouncementByEnum(AnnouncerSounds.AnnouncerEvent.Custom);
							return;
						}
					}
				}
				if (btnHit == m_RankedBtn.spriteController.gameObject)
				{
					AnnouncerSounds.GetAnnouncerSounds().PlayAnnouncementByEnum(AnnouncerSounds.AnnouncerEvent.Ranked);
				}
				return;
			}
		}
	}

	public void GameTypeClicked(BaseEventData data)
	{
		if (UISeasonsPanel.Get().IsVisible())
		{
			return;
		}
		GameObject gameObject = (data as PointerEventData).pointerCurrentRaycast.gameObject;
		if (gameObject == m_CustomBtn.spriteController.gameObject)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					CustomGameTypeClicked();
					return;
				}
			}
		}
		DoSelectGameType(gameObject);
	}

	public void CustomGameTypeClicked()
	{
		ClientGameManager.Get().LeaveGame(true, GameResult.ClientLeft);
		Log.Info("Custom button Clicked, leaving queue");
		ClientGameManager clientGameManager = ClientGameManager.Get();
		
		clientGameManager.LeaveQueue(delegate(LeaveMatchmakingQueueResponse r)
			{
				if (!r.Success)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							break;
						default:
							Log.Warning("Failure to unqueue when entering custom: {0}", r.ErrorMessage);
							return;
						}
					}
				}
			});
		AppState_GameTypeSelect.Get().OnCustomClicked();
		AppState_GroupCharacterSelect.Get().NotifyQueueDrop();
		SetVisible(false);
		UIRankedModeSelectScreen.Get().SetVisible(false);
		UICharacterSelectScreen.Get().SelectedGameMode(GameType.Custom);
	}

	public GameType GetGameTypeForSelectedButton()
	{
		for (int i = 0; i < m_menuList.Count; i++)
		{
			if (m_menuList[i].IsSelected())
			{
				return GameTypeButtonToGameType((GameTypeButton)i);
			}
		}
		while (true)
		{
			return GameType.None;
		}
	}
}

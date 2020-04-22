using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISystemEscapeMenu : UIScene
{
	public RectTransform m_parentObject;

	public _ButtonSwapSprite m_toggleButton;

	public GameObject m_toggleButtonMaster;

	public _ButtonSwapSprite m_returnToGame;

	public _ButtonSwapSprite m_pauseBtn;

	public _ButtonSwapSprite m_playBtn;

	public _ButtonSwapSprite m_reportBug;

	public _ButtonSwapSprite m_options;

	public _ButtonSwapSprite m_leaveGame;

	public _ButtonSwapSprite m_keyBinding;

	public Image m_background;

	private bool m_isOpen;

	private static UISystemEscapeMenu s_instance;

	public void SetParent(bool visible)
	{
		m_isOpen = visible;
		UIManager.SetGameObjectActive(m_parentObject, visible);
		UIManager.SetGameObjectActive(m_background, visible);
		int num;
		if (GameManager.Get() != null && GameManager.Get().GameConfig != null)
		{
			num = (GameManager.Get().IsAllowingPlayerRequestedPause() ? 1 : 0);
		}
		else
		{
			num = 0;
		}
		bool flag = (byte)num != 0;
		bool flag2 = false;
		if (flag)
		{
			if (GameFlowData.Get() != null)
			{
				flag2 = GameFlowData.Get().GetPausedByPlayerRequest();
			}
		}
		_SelectableBtn selectableButton = m_pauseBtn.selectableButton;
		int doActive;
		if (flag)
		{
			doActive = ((!flag2) ? 1 : 0);
		}
		else
		{
			doActive = 0;
		}
		UIManager.SetGameObjectActive(selectableButton, (byte)doActive != 0);
		_SelectableBtn selectableButton2 = m_playBtn.selectableButton;
		int doActive2;
		if (flag)
		{
			doActive2 = (flag2 ? 1 : 0);
		}
		else
		{
			doActive2 = 0;
		}
		UIManager.SetGameObjectActive(selectableButton2, (byte)doActive2 != 0);
	}

	public static UISystemEscapeMenu Get()
	{
		return s_instance;
	}

	public override void Awake()
	{
		s_instance = this;
		base.Awake();
	}

	public override SceneType GetSceneType()
	{
		return SceneType.InGameMenu;
	}

	private void Start()
	{
		m_toggleButton.callback = OnToggleButtonClick;
		m_pauseBtn.callback = OnPauseClick;
		m_playBtn.callback = OnPlayClick;
		m_returnToGame.callback = OnReturnToGameClick;
		m_options.callback = OnOptionsClick;
		m_keyBinding.callback = OnKeyBindingClick;
		m_leaveGame.callback = OnLeaveGameClick;
		m_reportBug.callback = OnReportBugClick;
		m_returnToGame.m_ignoreDialogboxes = true;
		m_options.m_ignoreDialogboxes = true;
		m_keyBinding.m_ignoreDialogboxes = true;
		m_leaveGame.m_ignoreDialogboxes = true;
		m_reportBug.m_ignoreDialogboxes = true;
		m_toggleButton.m_soundToPlay = FrontEndButtonSounds.MenuChoice;
		m_returnToGame.m_soundToPlay = FrontEndButtonSounds.MenuChoice;
		m_options.m_soundToPlay = FrontEndButtonSounds.MenuChoice;
		m_keyBinding.m_soundToPlay = FrontEndButtonSounds.MenuChoice;
		m_leaveGame.m_soundToPlay = FrontEndButtonSounds.MenuChoice;
		m_reportBug.m_soundToPlay = FrontEndButtonSounds.MenuChoice;
		SetParent(false);
	}

	public bool IsOpen()
	{
		return m_isOpen;
	}

	public void OnToggleButtonClick(BaseEventData data)
	{
		SetParent(!m_isOpen);
	}

	private void OnReturnToGameClick(BaseEventData data)
	{
		SetParent(false);
		GameEventManager.Get().FireEvent(GameEventManager.EventType.SystemEscapeMenuOnReturnToGameClick, null);
	}

	private void OnOptionsClick(BaseEventData data)
	{
		SetParent(false);
		Options_UI.Get().ToggleOptions();
	}

	private void OnKeyBindingClick(BaseEventData data)
	{
		SetParent(false);
		KeyBinding_UI.Get().ToggleKeybinds();
	}

	public void OnReportBugClick(BaseEventData data)
	{
		UILandingPageFullScreenMenus.Get().SetFeedbackContainerVisible(true);
		OnReturnToGameClick(data);
	}

	public void OnPauseClick(BaseEventData data)
	{
		PauseGame(true);
		SetParent(false);
	}

	public void OnPlayClick(BaseEventData data)
	{
		PauseGame(false);
		SetParent(false);
	}

	public void OnLeaveGameClick(BaseEventData data)
	{
		SetParent(false);
		LobbyGameInfo gameInfo = GameManager.Get().GameInfo;
		int num;
		if (gameInfo.GameConfig.GameType.AllowsReconnect())
		{
			num = ((gameInfo.GameStatus != GameStatus.Stopped) ? 1 : 0);
		}
		else
		{
			num = 0;
		}
		bool allowsReconnect = (byte)num != 0;
		if (ReplayPlayManager.Get() != null)
		{
			if (ReplayPlayManager.Get().IsPlayback())
			{
				ClientGameManager.Get().LeaveGame(true, GameResult.ClientLeft);
				return;
			}
		}
		string title;
		string description;
		if (gameInfo.GameConfig.GameType != GameType.Tutorial)
		{
			if (gameInfo.GameConfig.GameType != GameType.NewPlayerSolo)
			{
				title = StringUtil.TR("LeaveGame", "Global");
				bool flag = false;
				if (GameManager.Get() != null)
				{
					if (GameManager.Get().GameInfo != null && GameManager.Get().GameInfo.GameConfig != null)
					{
						if (GameManager.Get().GameInfo.GameConfig.InstanceSubType.HasMod(GameSubType.SubTypeMods.AntiSocial))
						{
							flag = true;
						}
					}
				}
				if (ClientGameManager.Get().HasLeavingPenalty(GameManager.Get().GameConfig.GameType))
				{
					if (!flag)
					{
						description = StringUtil.TR("QuitGamePromptWithPenalty", "Global");
						goto IL_01c2;
					}
				}
				description = StringUtil.TR("LeaveGameConfirmation", "Global");
				goto IL_01c2;
			}
		}
		title = StringUtil.TR("LeaveTutorial", "Global");
		description = StringUtil.TR("LeaveTutorialConfirmation", "Global");
		goto IL_01c2;
		IL_01c2:
		UIDialogPopupManager.OpenTwoButtonDialog(title, description, StringUtil.TR("Yes", "Global"), StringUtil.TR("No", "Global"), delegate
		{
			ClientGameManager.Get().LeaveGame(!allowsReconnect, GameResult.ClientLeft);
			if (UITutorialFullscreenPanel.Get() != null)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
						if (UITutorialFullscreenPanel.Get().IsAnyPanelVisible())
						{
							while (true)
							{
								switch (5)
								{
								case 0:
									break;
								default:
									UITutorialFullscreenPanel.Get().ClearAllPanels();
									return;
								}
							}
						}
						return;
					}
				}
			}
		});
	}

	private void PauseGame(bool desiredPause)
	{
		if (GameManager.Get() != null)
		{
			if (GameManager.Get().GameConfig != null)
			{
				if (GameManager.Get().IsAllowingPlayerRequestedPause())
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							break;
						default:
						{
							object obj;
							if (GameFlowData.Get() != null)
							{
								obj = GameFlowData.Get().activeOwnedActorData;
							}
							else
							{
								obj = null;
							}
							ActorData actorData = (ActorData)obj;
							if (actorData != null)
							{
								if (actorData.GetActorController() != null)
								{
									while (true)
									{
										switch (4)
										{
										case 0:
											break;
										default:
											actorData.GetActorController().RequestCustomGamePause(desiredPause, actorData.ActorIndex);
											return;
										}
									}
								}
							}
							TextConsole.Get().Write(StringUtil.TR("PauseError", "Global"));
							return;
						}
						}
					}
				}
			}
		}
		TextConsole.Get().Write(StringUtil.TR("PauseDisabled", "Global"));
	}

	private void OnDestroy()
	{
		s_instance = null;
	}

	private void Update()
	{
		if (GameFlowData.Get() != null)
		{
			while (true)
			{
				bool flag3;
				switch (5)
				{
				case 0:
					break;
				default:
					{
						if (InputManager.Get().IsKeyBindingNewlyHeld(KeyPreference.ToggleSystemMenu))
						{
							ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
							if (activeOwnedActorData == null)
							{
								SetParent(!m_isOpen);
								return;
							}
							bool flag = activeOwnedActorData.HasQueuedMovement();
							AbilityData component = activeOwnedActorData.GetComponent<AbilityData>();
							bool flag2 = component.HasQueuedAbilities();
							if (!flag)
							{
								if (!flag2)
								{
									goto IL_00a7;
								}
							}
							if (activeOwnedActorData.GetActorTurnSM().CurrentState != 0)
							{
								goto IL_00a7;
							}
						}
						goto IL_00b6;
					}
					IL_00b6:
					flag3 = (AppState.GetCurrent() == AppState_InGameEnding.Get());
					UIManager.SetGameObjectActive(m_toggleButtonMaster, !flag3);
					return;
					IL_00a7:
					SetParent(!m_isOpen);
					goto IL_00b6;
				}
			}
		}
		UIManager.SetGameObjectActive(m_toggleButtonMaster, false);
	}
}

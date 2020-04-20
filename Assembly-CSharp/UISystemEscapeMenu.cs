using System;
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
		this.m_isOpen = visible;
		UIManager.SetGameObjectActive(this.m_parentObject, visible, null);
		UIManager.SetGameObjectActive(this.m_background, visible, null);
		bool flag;
		if (GameManager.Get() != null && GameManager.Get().GameConfig != null)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UISystemEscapeMenu.SetParent(bool)).MethodHandle;
			}
			flag = GameManager.Get().IsAllowingPlayerRequestedPause();
		}
		else
		{
			flag = false;
		}
		bool flag2 = flag;
		bool flag3 = false;
		if (flag2)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (GameFlowData.Get() != null)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				flag3 = GameFlowData.Get().GetPausedByPlayerRequest();
			}
		}
		Component selectableButton = this.m_pauseBtn.selectableButton;
		bool doActive;
		if (flag2)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			doActive = !flag3;
		}
		else
		{
			doActive = false;
		}
		UIManager.SetGameObjectActive(selectableButton, doActive, null);
		Component selectableButton2 = this.m_playBtn.selectableButton;
		bool doActive2;
		if (flag2)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			doActive2 = flag3;
		}
		else
		{
			doActive2 = false;
		}
		UIManager.SetGameObjectActive(selectableButton2, doActive2, null);
	}

	public static UISystemEscapeMenu Get()
	{
		return UISystemEscapeMenu.s_instance;
	}

	public override void Awake()
	{
		UISystemEscapeMenu.s_instance = this;
		base.Awake();
	}

	public override SceneType GetSceneType()
	{
		return SceneType.InGameMenu;
	}

	private void Start()
	{
		this.m_toggleButton.callback = new _ButtonSwapSprite.ButtonClickCallback(this.OnToggleButtonClick);
		this.m_pauseBtn.callback = new _ButtonSwapSprite.ButtonClickCallback(this.OnPauseClick);
		this.m_playBtn.callback = new _ButtonSwapSprite.ButtonClickCallback(this.OnPlayClick);
		this.m_returnToGame.callback = new _ButtonSwapSprite.ButtonClickCallback(this.OnReturnToGameClick);
		this.m_options.callback = new _ButtonSwapSprite.ButtonClickCallback(this.OnOptionsClick);
		this.m_keyBinding.callback = new _ButtonSwapSprite.ButtonClickCallback(this.OnKeyBindingClick);
		this.m_leaveGame.callback = new _ButtonSwapSprite.ButtonClickCallback(this.OnLeaveGameClick);
		this.m_reportBug.callback = new _ButtonSwapSprite.ButtonClickCallback(this.OnReportBugClick);
		this.m_returnToGame.m_ignoreDialogboxes = true;
		this.m_options.m_ignoreDialogboxes = true;
		this.m_keyBinding.m_ignoreDialogboxes = true;
		this.m_leaveGame.m_ignoreDialogboxes = true;
		this.m_reportBug.m_ignoreDialogboxes = true;
		this.m_toggleButton.m_soundToPlay = FrontEndButtonSounds.MenuChoice;
		this.m_returnToGame.m_soundToPlay = FrontEndButtonSounds.MenuChoice;
		this.m_options.m_soundToPlay = FrontEndButtonSounds.MenuChoice;
		this.m_keyBinding.m_soundToPlay = FrontEndButtonSounds.MenuChoice;
		this.m_leaveGame.m_soundToPlay = FrontEndButtonSounds.MenuChoice;
		this.m_reportBug.m_soundToPlay = FrontEndButtonSounds.MenuChoice;
		this.SetParent(false);
	}

	public bool IsOpen()
	{
		return this.m_isOpen;
	}

	public void OnToggleButtonClick(BaseEventData data)
	{
		this.SetParent(!this.m_isOpen);
	}

	private void OnReturnToGameClick(BaseEventData data)
	{
		this.SetParent(false);
		GameEventManager.Get().FireEvent(GameEventManager.EventType.SystemEscapeMenuOnReturnToGameClick, null);
	}

	private void OnOptionsClick(BaseEventData data)
	{
		this.SetParent(false);
		Options_UI.Get().ToggleOptions();
	}

	private void OnKeyBindingClick(BaseEventData data)
	{
		this.SetParent(false);
		KeyBinding_UI.Get().ToggleKeybinds();
	}

	public void OnReportBugClick(BaseEventData data)
	{
		UILandingPageFullScreenMenus.Get().SetFeedbackContainerVisible(true);
		this.OnReturnToGameClick(data);
	}

	public void OnPauseClick(BaseEventData data)
	{
		this.PauseGame(true);
		this.SetParent(false);
	}

	public void OnPlayClick(BaseEventData data)
	{
		this.PauseGame(false);
		this.SetParent(false);
	}

	public void OnLeaveGameClick(BaseEventData data)
	{
		UISystemEscapeMenu.<OnLeaveGameClick>c__AnonStorey0 <OnLeaveGameClick>c__AnonStorey = new UISystemEscapeMenu.<OnLeaveGameClick>c__AnonStorey0();
		this.SetParent(false);
		LobbyGameInfo gameInfo = GameManager.Get().GameInfo;
		UISystemEscapeMenu.<OnLeaveGameClick>c__AnonStorey0 <OnLeaveGameClick>c__AnonStorey2 = <OnLeaveGameClick>c__AnonStorey;
		bool allowsReconnect;
		if (gameInfo.GameConfig.GameType.AllowsReconnect())
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UISystemEscapeMenu.OnLeaveGameClick(BaseEventData)).MethodHandle;
			}
			allowsReconnect = (gameInfo.GameStatus != GameStatus.Stopped);
		}
		else
		{
			allowsReconnect = false;
		}
		<OnLeaveGameClick>c__AnonStorey2.allowsReconnect = allowsReconnect;
		if (ReplayPlayManager.Get() != null)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
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
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (gameInfo.GameConfig.GameType != GameType.NewPlayerSolo)
			{
				title = StringUtil.TR("LeaveGame", "Global");
				bool flag = false;
				if (GameManager.Get() != null)
				{
					for (;;)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					if (GameManager.Get().GameInfo != null && GameManager.Get().GameInfo.GameConfig != null)
					{
						for (;;)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							break;
						}
						if (GameManager.Get().GameInfo.GameConfig.InstanceSubType.HasMod(GameSubType.SubTypeMods.AntiSocial))
						{
							for (;;)
							{
								switch (7)
								{
								case 0:
									continue;
								}
								break;
							}
							flag = true;
						}
					}
				}
				if (ClientGameManager.Get().HasLeavingPenalty(GameManager.Get().GameConfig.GameType))
				{
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!flag)
					{
						description = StringUtil.TR("QuitGamePromptWithPenalty", "Global");
						goto IL_1C2;
					}
				}
				description = StringUtil.TR("LeaveGameConfirmation", "Global");
				goto IL_1C2;
			}
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		title = StringUtil.TR("LeaveTutorial", "Global");
		description = StringUtil.TR("LeaveTutorialConfirmation", "Global");
		IL_1C2:
		UIDialogPopupManager.OpenTwoButtonDialog(title, description, StringUtil.TR("Yes", "Global"), StringUtil.TR("No", "Global"), delegate(UIDialogBox dialogReference)
		{
			ClientGameManager.Get().LeaveGame(!<OnLeaveGameClick>c__AnonStorey.allowsReconnect, GameResult.ClientLeft);
			if (UITutorialFullscreenPanel.Get() != null)
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle2 = methodof(UISystemEscapeMenu.<OnLeaveGameClick>c__AnonStorey0.<>m__0(UIDialogBox)).MethodHandle;
				}
				if (UITutorialFullscreenPanel.Get().IsAnyPanelVisible())
				{
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					UITutorialFullscreenPanel.Get().ClearAllPanels();
				}
			}
		}, null, false, false);
	}

	private void PauseGame(bool desiredPause)
	{
		if (GameManager.Get() != null)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UISystemEscapeMenu.PauseGame(bool)).MethodHandle;
			}
			if (GameManager.Get().GameConfig != null)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (GameManager.Get().IsAllowingPlayerRequestedPause())
				{
					for (;;)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					ActorData actorData;
					if (GameFlowData.Get() != null)
					{
						for (;;)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							break;
						}
						actorData = GameFlowData.Get().activeOwnedActorData;
					}
					else
					{
						actorData = null;
					}
					ActorData actorData2 = actorData;
					if (actorData2 != null)
					{
						for (;;)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							break;
						}
						if (actorData2.GetActorController() != null)
						{
							for (;;)
							{
								switch (4)
								{
								case 0:
									continue;
								}
								break;
							}
							actorData2.GetActorController().RequestCustomGamePause(desiredPause, actorData2.ActorIndex);
							goto IL_EF;
						}
					}
					TextConsole.Get().Write(StringUtil.TR("PauseError", "Global"), ConsoleMessageType.SystemMessage);
					IL_EF:
					return;
				}
			}
		}
		TextConsole.Get().Write(StringUtil.TR("PauseDisabled", "Global"), ConsoleMessageType.SystemMessage);
	}

	private void OnDestroy()
	{
		UISystemEscapeMenu.s_instance = null;
	}

	private void Update()
	{
		if (GameFlowData.Get() != null)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UISystemEscapeMenu.Update()).MethodHandle;
			}
			if (InputManager.Get().IsKeyBindingNewlyHeld(KeyPreference.ToggleSystemMenu))
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
				if (activeOwnedActorData == null)
				{
					this.SetParent(!this.m_isOpen);
					return;
				}
				bool flag = activeOwnedActorData.HasQueuedMovement();
				AbilityData component = activeOwnedActorData.GetComponent<AbilityData>();
				bool flag2 = component.HasQueuedAbilities();
				if (!flag)
				{
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!flag2)
					{
						goto IL_A7;
					}
				}
				if (activeOwnedActorData.GetActorTurnSM().CurrentState == TurnStateEnum.DECIDING)
				{
					goto IL_B6;
				}
				IL_A7:
				this.SetParent(!this.m_isOpen);
			}
			IL_B6:
			bool flag3 = AppState.GetCurrent() == AppState_InGameEnding.Get();
			UIManager.SetGameObjectActive(this.m_toggleButtonMaster, !flag3, null);
		}
		else
		{
			UIManager.SetGameObjectActive(this.m_toggleButtonMaster, false, null);
		}
	}
}

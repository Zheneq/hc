using System;

public class AppState_GameTeardown : AppState
{
	private static AppState_GameTeardown s_instance;

	private GameResult m_lastGameResult;

	private string m_lastLobbyErrorMessage;

	private LobbyGameInfo m_lastGameInfo;

	private UIDialogBox m_messageBox;

	public static AppState_GameTeardown Get()
	{
		return AppState_GameTeardown.s_instance;
	}

	public static void Create()
	{
		AppState.Create<AppState_GameTeardown>();
	}

	private void Awake()
	{
		AppState_GameTeardown.s_instance = this;
	}

	public void Enter(GameResult gameResult, string lastLobbyErrorMessage)
	{
		this.m_lastGameResult = gameResult;
		this.m_lastLobbyErrorMessage = lastLobbyErrorMessage;
		base.Enter();
	}

	protected override void OnEnter()
	{
		GameManager.Get().StopGame(GameResult.NoResult);
		this.m_lastGameInfo = GameManager.Get().GameInfo;
		ClientGameManager clientGameManager = ClientGameManager.Get();
		clientGameManager.LeaveGame(false, this.m_lastGameResult);
		if (HUD_UI.Get() != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AppState_GameTeardown.OnEnter()).MethodHandle;
			}
			HUD_UI.Get().m_mainScreenPanel.SetVisible(false);
			UIManager.SetGameObjectActive(HUD_UI.Get().m_textConsole, false, null);
		}
		UILoadingScreenPanel.Get().SetVisible(false);
		GameEventManager.Get().FireEvent(GameEventManager.EventType.GameTeardown, null);
		if (HUD_UI.Get() != null)
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
			HUD_UI.Get().GameTeardown();
		}
		if (ClientGamePrefabInstantiator.Get() != null)
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
			ClientGamePrefabInstantiator.Get().DestroyInstantiations();
		}
		else
		{
			Log.Error("ClientGamePrefabInstantiator reference not set on game teardown", new object[0]);
		}
		if (this.m_lastGameResult == GameResult.ServerCrashed)
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
			this.m_messageBox = UIDialogPopupManager.OpenOneButtonDialog(string.Empty, StringUtil.TR("ServerShutDown", "Global"), StringUtil.TR("Ok", "Global"), delegate(UIDialogBox UIDialogBox)
			{
				this.GoToFrontend();
			}, -1, false);
		}
		else if (this.m_lastGameResult.IsConnectionErrorResult())
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
			if (!this.m_lastLobbyErrorMessage.IsNullOrEmpty())
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
				if (clientGameManager.AllowRelogin)
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
					string empty = string.Empty;
					string description = string.Format(StringUtil.TR("PressOkToReconnect", "Global"), this.m_lastLobbyErrorMessage);
					string leftButtonLabel = StringUtil.TR("Ok", "Global");
					string rightButtonLabel = StringUtil.TR("Cancel", "Global");
					UIDialogBox.DialogButtonCallback leftButtonCallback = delegate(UIDialogBox UIDialogBox)
					{
						this.GoToFrontend();
					};
					if (AppState_GameTeardown.<>f__am$cache0 == null)
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
						AppState_GameTeardown.<>f__am$cache0 = delegate(UIDialogBox UIDialogBox)
						{
							AppState_Shutdown.Get().Enter();
						};
					}
					this.m_messageBox = UIDialogPopupManager.OpenTwoButtonDialog(empty, description, leftButtonLabel, rightButtonLabel, leftButtonCallback, AppState_GameTeardown.<>f__am$cache0, false, false);
				}
				else
				{
					this.m_messageBox = UIDialogPopupManager.OpenOneButtonDialog(string.Empty, string.Format(StringUtil.TR("PressOkToExit", "Global"), this.m_lastLobbyErrorMessage), StringUtil.TR("Ok", "Global"), delegate(UIDialogBox UIDialogBox)
					{
						AppState_Shutdown.Get().Enter();
					}, -1, false);
				}
			}
			else
			{
				this.m_messageBox = UIDialogPopupManager.OpenOneButtonDialog(string.Empty, this.m_lastGameResult.GetErrorMessage(), StringUtil.TR("Ok", "Global"), delegate(UIDialogBox UIDialogBox)
				{
					this.GoToFrontend();
				}, -1, false);
			}
		}
		else
		{
			this.GoToFrontend();
		}
		this.m_lastLobbyErrorMessage = null;
		if (!clientGameManager.PlayerObjectStartedOnClient)
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
			if (!clientGameManager.InGameUIActivated)
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
				if (!clientGameManager.VisualSceneLoaded)
				{
					if (!clientGameManager.DesignSceneStarted)
					{
						return;
					}
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
				}
			}
		}
		string message = string.Format("CGM UI state in a bad state: {0} {1} {2} {3}", new object[]
		{
			clientGameManager.PlayerObjectStartedOnClient,
			clientGameManager.InGameUIActivated,
			clientGameManager.VisualSceneLoaded,
			clientGameManager.DesignSceneStarted
		});
		clientGameManager.PlayerObjectStartedOnClient = false;
		clientGameManager.InGameUIActivated = false;
		clientGameManager.VisualSceneLoaded = false;
		clientGameManager.DesignSceneStarted = false;
		Log.Info(message, new object[0]);
	}

	private void GoToFrontend()
	{
		AppState_FrontendLoadingScreen.NextState nextState;
		if (this.m_lastGameInfo.GameConfig.GameType != GameType.Tutorial)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AppState_GameTeardown.GoToFrontend()).MethodHandle;
			}
			if (this.m_lastGameInfo.GameConfig.GameType != GameType.Custom)
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
				if (this.m_lastGameInfo.GameConfig.GameType != GameType.NewPlayerSolo)
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
					if (this.m_lastGameResult != GameResult.ClientIdleTimeout && this.m_lastGameResult != GameResult.SkippedTurnsIdleTimeout)
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
						nextState = AppState_FrontendLoadingScreen.NextState.GoToCharacterSelect;
						goto IL_87;
					}
				}
			}
		}
		nextState = AppState_FrontendLoadingScreen.NextState.GoToLandingPage;
		IL_87:
		AppState_FrontendLoadingScreen.Get().Enter(this.m_lastLobbyErrorMessage, nextState);
	}

	protected override void OnLeave()
	{
		if (this.m_messageBox != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AppState_GameTeardown.OnLeave()).MethodHandle;
			}
			this.m_messageBox.Close();
			this.m_messageBox = null;
		}
	}

	private void Update()
	{
	}
}

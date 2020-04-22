using Fabric;

public class AppState_InGameStarting : AppStateInGame
{
	private static AppState_InGameStarting s_instance;

	public static AppState_InGameStarting Get()
	{
		return s_instance;
	}

	public static void Create()
	{
		AppState.Create<AppState_InGameStarting>();
	}

	private void Awake()
	{
		s_instance = this;
	}

	protected override void OnEnter()
	{
		if (UIScreenManager.Get() != null)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			UIScreenManager.Get().ClearAllPanels();
		}
		if (GameManager.Get() != null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (GameManager.Get().GameConfig != null)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (GameManager.Get().GameConfig.GameType != GameType.Tutorial)
				{
					UILoadingScreenPanel.Get().SetVisible(true);
					goto IL_0093;
				}
			}
		}
		UILoadingScreenPanel.Get().SetVisible(false);
		goto IL_0093;
		IL_0093:
		if (ClientGameManager.Get().PlayerObjectStartedOnClient && ClientGameManager.Get().DesignSceneStarted)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			UIScreenManager.Get().TryLoadAndSetupInGameUI();
			ClientGameManager.Get().CheckAndSendClientPreparedForGameStartNotification();
		}
		if (QuestListPanel.Get() != null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			QuestListPanel.Get().SetVisible(false);
		}
		Log.Info("AppState_InGameStarting.OnEnter");
		RegisterGameStoppedHandler();
		GameFlowData.s_onGameStateChanged += OnGameStateChanged;
		GameTime.scale = 1f;
	}

	protected override void OnLeave()
	{
		AudioManager.StandardizeAudioLinkages();
		EventManager.Instance.PostEvent("mus_menu", EventAction.SetSwitch, "mus_menu_loop");
		UnregisterGameStoppedHandler();
		GameFlowData.s_onGameStateChanged -= OnGameStateChanged;
	}

	private void OnGameStateChanged(GameState newState)
	{
		if (newState == GameState.Deployment)
		{
			AppState_InGameDeployment.Get().Enter();
			return;
		}
		if (newState == GameState.BothTeams_Decision)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					AppState_InGameDecision.Get().Enter();
					return;
				}
			}
		}
		if (newState == GameState.BothTeams_Resolve)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					AppState_InGameResolve.Get().Enter();
					return;
				}
			}
		}
		if (newState != GameState.EndingGame)
		{
			return;
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			AppState_InGameEnding.Get().Enter();
			return;
		}
	}
}

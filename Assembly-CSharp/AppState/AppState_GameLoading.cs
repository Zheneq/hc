using UnityEngine.SceneManagement;

public class AppState_GameLoading : AppState
{
	private static AppState_GameLoading s_instance;

	private UIOneButtonDialog m_messageBox;

	private bool m_assetLoadStarted;

	public static AppState_GameLoading Get()
	{
		return s_instance;
	}

	public static void Create()
	{
		AppState.Create<AppState_GameLoading>();
	}

	private void Awake()
	{
		s_instance = this;
	}

	public void Enter(GameType gameType)
	{
		if (UIFrontEnd.Get() != null)
		{
			UIFrontEnd.Get().ShowScreen(FrontEndScreenState.None);
		}
		m_assetLoadStarted = false;
		int num;
		if (GameManager.Get().PlayerInfo != null && GameManager.Get().GameInfo != null)
		{
			if (GameManager.Get().TeamInfo == null)
			{
				num = (ClientGameManager.Get().Reconnected ? 1 : 0);
			}
			else
			{
				num = 1;
			}
		}
		else
		{
			num = 0;
		}
		if (num != 0)
		{
			if (gameType != GameType.Tutorial)
			{
				if (gameType != GameType.NewPlayerSolo)
				{
					UILoadingScreenPanel.Get().SetVisible(true);
					UILoadingScreenPanel.Get().ShowTeams();
					goto IL_010e;
				}
			}
		}
		if (!UIFrontendLoadingScreen.Get().gameObject.activeSelf)
		{
			UIFrontendLoadingScreen.Get().SetVisible(true);
		}
		else
		{
			UIFrontendLoadingScreen.Get().StartDisplayLoading();
		}
		goto IL_010e;
		IL_010e:
		if (UIFrontEnd.Get() != null)
		{
			if (UIFrontEnd.Get().IsProgressScreenOpen())
			{
				UIFrontEnd.Get().TogglePlayerProgressScreenVisibility();
			}
		}
		UITextConsole frontEndChatConsole = UIFrontEnd.Get().m_frontEndChatConsole;
		if (frontEndChatConsole != null)
		{
			UIManager.SetGameObjectActive(frontEndChatConsole, false);
		}
		if (UIDialogPopupManager.Get() != null)
		{
			UIDialogPopupManager.Get().HideAllMenus();
		}
		ClientGameManager.Get().DisableFrontEnd();
		if (ClientQualityComponentEnabler.OptimizeForMemory())
		{
			ClientGameManager.Get().CleanupMemory();
		}
		AudioManager.GetMixerSnapshotManager().SetMix_LoadingScreen();
		StartCoroutine(AssetBundleManager.Get().LoadSceneAsync("HUD_UI", "frontend", LoadSceneMode.Additive));
		GameManager.Get().OnGameStopped += HandleGameStopped;
		base.Enter();
	}

	protected override void OnLeave()
	{
		if (m_messageBox != null)
		{
			m_messageBox.Close();
			m_messageBox = null;
		}
		GameManager.Get().OnGameStopped -= HandleGameStopped;
	}

	private void Update()
	{
		if (HUD_UI.Get() != null && GameManager.Get().GameStatus >= GameStatus.Launched)
		{
			if (!m_assetLoadStarted)
			{
				if (ClientGameManager.Get().IsConnectedToGameServer)
				{
					if (!ClientGameManager.Get().IsRegisteredToGameServer)
					{
						if (!ReplayPlayManager.Get().IsPlayback())
						{
							goto IL_009e;
						}
					}
					ClientGameManager.Get().LoadAssets();
					m_assetLoadStarted = true;
				}
			}
		}
		goto IL_009e;
		IL_009e:
		if (!m_assetLoadStarted)
		{
			return;
		}
		while (true)
		{
			if (!ClientGameManager.Get().IsLoading)
			{
				while (true)
				{
					AppState_InGameStarting.Get().Enter();
					return;
				}
			}
			return;
		}
	}

	public void HandleGameStopped(GameResult gameResult)
	{
		UIFrontendLoadingScreen.Get().SetVisible(false);
		string empty = string.Empty;
		if (gameResult == GameResult.ServerCrashed)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					empty = StringUtil.TR("GameServerShutDown", "Global");
					AppState_GameTeardown.Get().Enter(gameResult, empty);
					return;
				}
			}
		}
		if (gameResult.IsConnectionErrorResult())
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					empty = gameResult.GetErrorMessage();
					AppState_GameTeardown.Get().Enter(gameResult, null);
					return;
				}
			}
		}
		if (!(m_messageBox == null))
		{
			return;
		}
		while (true)
		{
			AppState_GameTeardown.Get().Enter(gameResult, empty);
			return;
		}
	}
}

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
			while (true)
			{
				switch (3)
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
			UIFrontEnd.Get().ShowScreen(FrontEndScreenState.None);
		}
		m_assetLoadStarted = false;
		int num;
		if (GameManager.Get().PlayerInfo != null && GameManager.Get().GameInfo != null)
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
			if (GameManager.Get().TeamInfo == null)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
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
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (gameType != GameType.Tutorial)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				if (gameType != GameType.NewPlayerSolo)
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					UILoadingScreenPanel.Get().SetVisible(true);
					UILoadingScreenPanel.Get().ShowTeams();
					goto IL_010e;
				}
			}
		}
		if (!UIFrontendLoadingScreen.Get().gameObject.activeSelf)
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
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (UIFrontEnd.Get().IsProgressScreenOpen())
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				UIFrontEnd.Get().TogglePlayerProgressScreenVisibility();
			}
		}
		UITextConsole frontEndChatConsole = UIFrontEnd.Get().m_frontEndChatConsole;
		if (frontEndChatConsole != null)
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
			UIManager.SetGameObjectActive(frontEndChatConsole, false);
		}
		if (UIDialogPopupManager.Get() != null)
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
			UIDialogPopupManager.Get().HideAllMenus();
		}
		ClientGameManager.Get().DisableFrontEnd();
		if (ClientQualityComponentEnabler.OptimizeForMemory())
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
			if (!m_assetLoadStarted)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				if (ClientGameManager.Get().IsConnectedToGameServer)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!ClientGameManager.Get().IsRegisteredToGameServer)
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
			switch (6)
			{
			case 0:
				continue;
			}
			if (!ClientGameManager.Get().IsLoading)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
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
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
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
			switch (6)
			{
			case 0:
				continue;
			}
			AppState_GameTeardown.Get().Enter(gameResult, empty);
			return;
		}
	}
}

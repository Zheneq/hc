using System;
using UnityEngine.SceneManagement;

public class AppState_GameLoading : AppState
{
	private static AppState_GameLoading s_instance;

	private UIOneButtonDialog m_messageBox;

	private bool m_assetLoadStarted;

	public static AppState_GameLoading Get()
	{
		return AppState_GameLoading.s_instance;
	}

	public static void Create()
	{
		AppState.Create<AppState_GameLoading>();
	}

	private void Awake()
	{
		AppState_GameLoading.s_instance = this;
	}

	public void Enter(GameType gameType)
	{
		if (UIFrontEnd.Get() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AppState_GameLoading.Enter(GameType)).MethodHandle;
			}
			UIFrontEnd.Get().ShowScreen(FrontEndScreenState.None, false);
		}
		this.m_assetLoadStarted = false;
		bool flag;
		if (GameManager.Get().PlayerInfo != null && GameManager.Get().GameInfo != null)
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
			if (GameManager.Get().TeamInfo == null)
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
				flag = ClientGameManager.Get().Reconnected;
			}
			else
			{
				flag = true;
			}
		}
		else
		{
			flag = false;
		}
		bool flag2 = flag;
		if (flag2)
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
			if (gameType != GameType.Tutorial)
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
				if (gameType != GameType.NewPlayerSolo)
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
					UILoadingScreenPanel.Get().SetVisible(true);
					UILoadingScreenPanel.Get().ShowTeams();
					goto IL_10E;
				}
			}
		}
		if (!UIFrontendLoadingScreen.Get().gameObject.activeSelf)
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
			UIFrontendLoadingScreen.Get().SetVisible(true);
		}
		else
		{
			UIFrontendLoadingScreen.Get().StartDisplayLoading(null);
		}
		IL_10E:
		if (UIFrontEnd.Get() != null)
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
			if (UIFrontEnd.Get().IsProgressScreenOpen())
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
				UIFrontEnd.Get().TogglePlayerProgressScreenVisibility(true);
			}
		}
		UITextConsole frontEndChatConsole = UIFrontEnd.Get().m_frontEndChatConsole;
		if (frontEndChatConsole != null)
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
			UIManager.SetGameObjectActive(frontEndChatConsole, false, null);
		}
		if (UIDialogPopupManager.Get() != null)
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
			UIDialogPopupManager.Get().HideAllMenus();
		}
		ClientGameManager.Get().DisableFrontEnd();
		if (ClientQualityComponentEnabler.OptimizeForMemory())
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
			ClientGameManager.Get().CleanupMemory();
		}
		AudioManager.GetMixerSnapshotManager().SetMix_LoadingScreen();
		base.StartCoroutine(AssetBundleManager.Get().LoadSceneAsync("HUD_UI", "frontend", LoadSceneMode.Additive));
		GameManager.Get().OnGameStopped += this.HandleGameStopped;
		base.Enter();
	}

	protected override void OnLeave()
	{
		if (this.m_messageBox != null)
		{
			this.m_messageBox.Close();
			this.m_messageBox = null;
		}
		GameManager.Get().OnGameStopped -= this.HandleGameStopped;
	}

	private void Update()
	{
		if (HUD_UI.Get() != null && GameManager.Get().GameStatus >= GameStatus.Launched)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AppState_GameLoading.Update()).MethodHandle;
			}
			if (!this.m_assetLoadStarted)
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
				if (ClientGameManager.Get().IsConnectedToGameServer)
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
					if (!ClientGameManager.Get().IsRegisteredToGameServer)
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
						if (!ReplayPlayManager.Get().IsPlayback())
						{
							goto IL_9E;
						}
					}
					ClientGameManager.Get().LoadAssets();
					this.m_assetLoadStarted = true;
				}
			}
		}
		IL_9E:
		if (this.m_assetLoadStarted)
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
			if (!ClientGameManager.Get().IsLoading)
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
				AppState_InGameStarting.Get().Enter();
			}
		}
	}

	public void HandleGameStopped(GameResult gameResult)
	{
		UIFrontendLoadingScreen.Get().SetVisible(false);
		string lastLobbyErrorMessage = string.Empty;
		if (gameResult == GameResult.ServerCrashed)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AppState_GameLoading.HandleGameStopped(GameResult)).MethodHandle;
			}
			lastLobbyErrorMessage = StringUtil.TR("GameServerShutDown", "Global");
			AppState_GameTeardown.Get().Enter(gameResult, lastLobbyErrorMessage);
		}
		else if (gameResult.IsConnectionErrorResult())
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
			lastLobbyErrorMessage = gameResult.GetErrorMessage();
			AppState_GameTeardown.Get().Enter(gameResult, null);
		}
		else if (this.m_messageBox == null)
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
			AppState_GameTeardown.Get().Enter(gameResult, lastLobbyErrorMessage);
		}
	}
}

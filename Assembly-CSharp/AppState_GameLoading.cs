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
			UIFrontEnd.Get().ShowScreen(FrontEndScreenState.None, false);
		}
		this.m_assetLoadStarted = false;
		bool flag;
		if (GameManager.Get().PlayerInfo != null && GameManager.Get().GameInfo != null)
		{
			if (GameManager.Get().TeamInfo == null)
			{
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
			if (gameType != GameType.Tutorial)
			{
				if (gameType != GameType.NewPlayerSolo)
				{
					UILoadingScreenPanel.Get().SetVisible(true);
					UILoadingScreenPanel.Get().ShowTeams();
					goto IL_10E;
				}
			}
		}
		if (!UIFrontendLoadingScreen.Get().gameObject.activeSelf)
		{
			UIFrontendLoadingScreen.Get().SetVisible(true);
		}
		else
		{
			UIFrontendLoadingScreen.Get().StartDisplayLoading(null);
		}
		IL_10E:
		if (UIFrontEnd.Get() != null)
		{
			if (UIFrontEnd.Get().IsProgressScreenOpen())
			{
				UIFrontEnd.Get().TogglePlayerProgressScreenVisibility(true);
			}
		}
		UITextConsole frontEndChatConsole = UIFrontEnd.Get().m_frontEndChatConsole;
		if (frontEndChatConsole != null)
		{
			UIManager.SetGameObjectActive(frontEndChatConsole, false, null);
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
			if (!this.m_assetLoadStarted)
			{
				if (ClientGameManager.Get().IsConnectedToGameServer)
				{
					if (!ClientGameManager.Get().IsRegisteredToGameServer)
					{
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
			if (!ClientGameManager.Get().IsLoading)
			{
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
			lastLobbyErrorMessage = StringUtil.TR("GameServerShutDown", "Global");
			AppState_GameTeardown.Get().Enter(gameResult, lastLobbyErrorMessage);
		}
		else if (gameResult.IsConnectionErrorResult())
		{
			lastLobbyErrorMessage = gameResult.GetErrorMessage();
			AppState_GameTeardown.Get().Enter(gameResult, null);
		}
		else if (this.m_messageBox == null)
		{
			AppState_GameTeardown.Get().Enter(gameResult, lastLobbyErrorMessage);
		}
	}
}

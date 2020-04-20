using System;
using Fabric;

public class AppState_InGameStarting : AppStateInGame
{
	private static AppState_InGameStarting s_instance;

	public static AppState_InGameStarting Get()
	{
		return AppState_InGameStarting.s_instance;
	}

	public static void Create()
	{
		AppState.Create<AppState_InGameStarting>();
	}

	private void Awake()
	{
		AppState_InGameStarting.s_instance = this;
	}

	protected override void OnEnter()
	{
		if (UIScreenManager.Get() != null)
		{
			UIScreenManager.Get().ClearAllPanels();
		}
		if (GameManager.Get() != null)
		{
			if (GameManager.Get().GameConfig != null)
			{
				if (GameManager.Get().GameConfig.GameType != GameType.Tutorial)
				{
					UILoadingScreenPanel.Get().SetVisible(true);
					goto IL_93;
				}
			}
		}
		UILoadingScreenPanel.Get().SetVisible(false);
		IL_93:
		if (ClientGameManager.Get().PlayerObjectStartedOnClient && ClientGameManager.Get().DesignSceneStarted)
		{
			UIScreenManager.Get().TryLoadAndSetupInGameUI();
			ClientGameManager.Get().CheckAndSendClientPreparedForGameStartNotification();
		}
		if (QuestListPanel.Get() != null)
		{
			QuestListPanel.Get().SetVisible(false, false, false);
		}
		Log.Info("AppState_InGameStarting.OnEnter", new object[0]);
		base.RegisterGameStoppedHandler();
		GameFlowData.s_onGameStateChanged += this.OnGameStateChanged;
		GameTime.scale = 1f;
	}

	protected override void OnLeave()
	{
		AudioManager.StandardizeAudioLinkages();
		EventManager.Instance.PostEvent("mus_menu", EventAction.SetSwitch, "mus_menu_loop");
		base.UnregisterGameStoppedHandler();
		GameFlowData.s_onGameStateChanged -= this.OnGameStateChanged;
	}

	private void OnGameStateChanged(GameState newState)
	{
		if (newState == GameState.Deployment)
		{
			AppState_InGameDeployment.Get().Enter();
		}
		else if (newState == GameState.BothTeams_Decision)
		{
			AppState_InGameDecision.Get().Enter();
		}
		else if (newState == GameState.BothTeams_Resolve)
		{
			AppState_InGameResolve.Get().Enter();
		}
		else if (newState == GameState.EndingGame)
		{
			AppState_InGameEnding.Get().Enter();
		}
	}
}

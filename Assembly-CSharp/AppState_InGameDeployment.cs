using System.Collections.Generic;

public class AppState_InGameDeployment : AppStateInGame
{
	private static AppState_InGameDeployment s_instance;

	public static AppState_InGameDeployment Get()
	{
		return s_instance;
	}

	public static void Create()
	{
		AppState.Create<AppState_InGameDeployment>();
	}

	private void Awake()
	{
		s_instance = this;
	}

	protected override void OnEnter()
	{
		if ((bool)CameraManager.Get().GetFlyThroughCamera())
		{
			if (!ClientGameManager.Get().IsSpectator)
			{
				CameraManager.Get().GetFlyThroughCamera().StartFlyThroughAnimation();
			}
		}
		WinUtils.FlashWindow();
		ChatterManager.Get().CancelActiveChatter();
		ChatterManager.Get().ForceCancelActiveChatter();
		if (GameManager.Get().GameConfig.GameType != GameType.Tutorial)
		{
			if (!ClientGameManager.Get().Reconnected)
			{
				UIFrontEnd.PlaySound(FrontEndButtonSounds.DeploymentBegin);
			}
		}
		AudioManager.GetMixerSnapshotManager().SetMix_MatchIntro();
		if (UIFrontendLoadingScreen.Get().gameObject.activeSelf)
		{
			UIFrontendLoadingScreen.Get().StartDisplayFadeOut();
		}
		Board.Get().SetLOSVisualEffect(false);
		if (GameManager.Get().GameConfig.GameType == GameType.Tutorial)
		{
			CameraManager.Get().EnableOffFogOfWarEffect(false);
		}
		Log.Info("HEALTHBARCHECK: DEPLOYMENT ENTERED");
		UIScreenManager.Get().SetHUDHide(false, false, true);
		if (HUD_UI.Get() != null)
		{
			UIManager.SetGameObjectActive(HUD_UI.Get().m_textConsole, true);
		}
		if (GameFlowData.Get().activeOwnedActorData != null)
		{
			GameFlowData.Get().activeOwnedActorData.GetActorController().SetMovementDistanceLinesVisible(false);
		}
		RegisterGameStoppedHandler();
		GameFlowData.s_onGameStateChanged += OnGameStateChanged;
	}

	protected override void OnLeave()
	{
		if ((bool)CameraManager.Get().GetFlyThroughCamera())
		{
			CameraManager.Get().GetFlyThroughCamera().StopFlyThroughAnimation();
		}
		if ((bool)BrushCoordinator.Get())
		{
			BrushCoordinator.Get().EnableBrushVisibility();
		}
		Board.Get().SetLOSVisualEffect(true);
		if (GameManager.Get().GameConfig.GameType == GameType.Tutorial)
		{
			CameraManager.Get().EnableOffFogOfWarEffect(true);
		}
		Log.Info("HEALTHBARCHECK: DEPLOYMENT LEAVE");
		UIScreenManager.Get().SetHUDHide(true, true);
		if (GameFlowData.Get().activeOwnedActorData != null)
		{
			using (List<ActorData>.Enumerator enumerator = GameFlowData.Get().m_ownedActorDatas.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ActorData current = enumerator.Current;
					current.GetActorController().SetMovementDistanceLinesVisible(true);
				}
			}
		}
		UnregisterGameStoppedHandler();
		GameFlowData.s_onGameStateChanged -= OnGameStateChanged;
	}

	private void OnGameStateChanged(GameState newState)
	{
		switch (newState)
		{
		default:
			return;
		case GameState.BothTeams_Decision:
			AppState_InGameDecision.Get().Enter();
			return;
		case GameState.EndingGame:
			break;
		}
		while (true)
		{
			AppState_InGameEnding.Get().Enter();
			return;
		}
	}
}

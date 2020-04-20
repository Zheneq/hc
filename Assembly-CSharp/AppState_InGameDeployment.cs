using System;
using System.Collections.Generic;

public class AppState_InGameDeployment : AppStateInGame
{
	private static AppState_InGameDeployment s_instance;

	public static AppState_InGameDeployment Get()
	{
		return AppState_InGameDeployment.s_instance;
	}

	public static void Create()
	{
		AppState.Create<AppState_InGameDeployment>();
	}

	private void Awake()
	{
		AppState_InGameDeployment.s_instance = this;
	}

	protected override void OnEnter()
	{
		if (CameraManager.Get().GetFlyThroughCamera())
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
		Log.Info("HEALTHBARCHECK: DEPLOYMENT ENTERED", new object[0]);
		UIScreenManager.Get().SetHUDHide(false, false, true, false);
		if (HUD_UI.Get() != null)
		{
			UIManager.SetGameObjectActive(HUD_UI.Get().m_textConsole, true, null);
		}
		if (GameFlowData.Get().activeOwnedActorData != null)
		{
			GameFlowData.Get().activeOwnedActorData.GetActorController().SetMovementDistanceLinesVisible(false);
		}
		base.RegisterGameStoppedHandler();
		GameFlowData.s_onGameStateChanged += this.OnGameStateChanged;
	}

	protected override void OnLeave()
	{
		if (CameraManager.Get().GetFlyThroughCamera())
		{
			CameraManager.Get().GetFlyThroughCamera().StopFlyThroughAnimation();
		}
		if (BrushCoordinator.Get())
		{
			BrushCoordinator.Get().EnableBrushVisibility();
		}
		Board.Get().SetLOSVisualEffect(true);
		if (GameManager.Get().GameConfig.GameType == GameType.Tutorial)
		{
			CameraManager.Get().EnableOffFogOfWarEffect(true);
		}
		Log.Info("HEALTHBARCHECK: DEPLOYMENT LEAVE", new object[0]);
		UIScreenManager.Get().SetHUDHide(true, true, false, false);
		if (GameFlowData.Get().activeOwnedActorData != null)
		{
			using (List<ActorData>.Enumerator enumerator = GameFlowData.Get().m_ownedActorDatas.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ActorData actorData = enumerator.Current;
					actorData.GetActorController().SetMovementDistanceLinesVisible(true);
				}
			}
		}
		base.UnregisterGameStoppedHandler();
		GameFlowData.s_onGameStateChanged -= this.OnGameStateChanged;
	}

	private void OnGameStateChanged(GameState newState)
	{
		if (newState == GameState.BothTeams_Decision)
		{
			AppState_InGameDecision.Get().Enter();
		}
		else if (newState == GameState.EndingGame)
		{
			AppState_InGameEnding.Get().Enter();
		}
	}
}

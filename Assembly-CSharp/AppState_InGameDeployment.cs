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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AppState_InGameDeployment.OnEnter()).MethodHandle;
			}
			if (!ClientGameManager.Get().IsSpectator)
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
				CameraManager.Get().GetFlyThroughCamera().StartFlyThroughAnimation();
			}
		}
		WinUtils.FlashWindow();
		ChatterManager.Get().CancelActiveChatter();
		ChatterManager.Get().ForceCancelActiveChatter();
		if (GameManager.Get().GameConfig.GameType != GameType.Tutorial)
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
			if (!ClientGameManager.Get().Reconnected)
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
				UIFrontEnd.PlaySound(FrontEndButtonSounds.DeploymentBegin);
			}
		}
		AudioManager.GetMixerSnapshotManager().SetMix_MatchIntro();
		if (UIFrontendLoadingScreen.Get().gameObject.activeSelf)
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
			UIFrontendLoadingScreen.Get().StartDisplayFadeOut();
		}
		Board.\u000E().SetLOSVisualEffect(false);
		if (GameManager.Get().GameConfig.GameType == GameType.Tutorial)
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
			CameraManager.Get().EnableOffFogOfWarEffect(false);
		}
		Log.Info("HEALTHBARCHECK: DEPLOYMENT ENTERED", new object[0]);
		UIScreenManager.Get().SetHUDHide(false, false, true, false);
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
			UIManager.SetGameObjectActive(HUD_UI.Get().m_textConsole, true, null);
		}
		if (GameFlowData.Get().activeOwnedActorData != null)
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
			GameFlowData.Get().activeOwnedActorData.\u000E().SetMovementDistanceLinesVisible(false);
		}
		base.RegisterGameStoppedHandler();
		GameFlowData.s_onGameStateChanged += this.OnGameStateChanged;
	}

	protected override void OnLeave()
	{
		if (CameraManager.Get().GetFlyThroughCamera())
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AppState_InGameDeployment.OnLeave()).MethodHandle;
			}
			CameraManager.Get().GetFlyThroughCamera().StopFlyThroughAnimation();
		}
		if (BrushCoordinator.Get())
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
			BrushCoordinator.Get().EnableBrushVisibility();
		}
		Board.\u000E().SetLOSVisualEffect(true);
		if (GameManager.Get().GameConfig.GameType == GameType.Tutorial)
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
					actorData.\u000E().SetMovementDistanceLinesVisible(true);
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AppState_InGameDeployment.OnGameStateChanged(GameState)).MethodHandle;
			}
			AppState_InGameEnding.Get().Enter();
		}
	}
}

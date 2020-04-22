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
			while (true)
			{
				switch (5)
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
			if (!ClientGameManager.Get().IsSpectator)
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
				CameraManager.Get().GetFlyThroughCamera().StartFlyThroughAnimation();
			}
		}
		WinUtils.FlashWindow();
		ChatterManager.Get().CancelActiveChatter();
		ChatterManager.Get().ForceCancelActiveChatter();
		if (GameManager.Get().GameConfig.GameType != GameType.Tutorial)
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
			if (!ClientGameManager.Get().Reconnected)
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
				UIFrontEnd.PlaySound(FrontEndButtonSounds.DeploymentBegin);
			}
		}
		AudioManager.GetMixerSnapshotManager().SetMix_MatchIntro();
		if (UIFrontendLoadingScreen.Get().gameObject.activeSelf)
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
			UIFrontendLoadingScreen.Get().StartDisplayFadeOut();
		}
		Board.Get().SetLOSVisualEffect(false);
		if (GameManager.Get().GameConfig.GameType == GameType.Tutorial)
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
			CameraManager.Get().EnableOffFogOfWarEffect(false);
		}
		Log.Info("HEALTHBARCHECK: DEPLOYMENT ENTERED");
		UIScreenManager.Get().SetHUDHide(false, false, true);
		if (HUD_UI.Get() != null)
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
			UIManager.SetGameObjectActive(HUD_UI.Get().m_textConsole, true);
		}
		if (GameFlowData.Get().activeOwnedActorData != null)
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
			GameFlowData.Get().activeOwnedActorData.GetActorController().SetMovementDistanceLinesVisible(false);
		}
		RegisterGameStoppedHandler();
		GameFlowData.s_onGameStateChanged += OnGameStateChanged;
	}

	protected override void OnLeave()
	{
		if ((bool)CameraManager.Get().GetFlyThroughCamera())
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			CameraManager.Get().GetFlyThroughCamera().StopFlyThroughAnimation();
		}
		if ((bool)BrushCoordinator.Get())
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
			BrushCoordinator.Get().EnableBrushVisibility();
		}
		Board.Get().SetLOSVisualEffect(true);
		if (GameManager.Get().GameConfig.GameType == GameType.Tutorial)
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
				while (true)
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
			switch (1)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			AppState_InGameEnding.Get().Enter();
			return;
		}
	}
}

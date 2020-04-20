﻿using System;

public class AppState_InGameEnding : AppStateInGame
{
	private static AppState_InGameEnding s_instance;

	public static AppState_InGameEnding Get()
	{
		return AppState_InGameEnding.s_instance;
	}

	public static void Create()
	{
		AppState.Create<AppState_InGameEnding>();
	}

	private void Awake()
	{
		AppState_InGameEnding.s_instance = this;
	}

	protected override void OnEnter()
	{
		if (HUD_UI.Get() != null)
		{
			HUD_UI.Get().m_mainScreenPanel.SetVisible(false);
		}
		if (UIGameOverScreen.Get() != null)
		{
			UIGameOverScreen.Get().SetVisible(true);
			AudioManager.GetMixerSnapshotManager().SetMix_GameOver();
		}
		base.RegisterGameStoppedHandler();
	}

	protected override void OnLeave()
	{
		base.UnregisterGameStoppedHandler();
	}
}

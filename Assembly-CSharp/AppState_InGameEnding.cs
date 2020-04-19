using System;

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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AppState_InGameEnding.OnEnter()).MethodHandle;
			}
			HUD_UI.Get().m_mainScreenPanel.SetVisible(false);
		}
		if (UIGameOverScreen.Get() != null)
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

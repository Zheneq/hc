using System;

public class AppState_InGameDecision : AppStateInGame
{
	private static AppState_InGameDecision s_instance;

	public static AppState_InGameDecision Get()
	{
		return AppState_InGameDecision.s_instance;
	}

	public static void Create()
	{
		AppState.Create<AppState_InGameDecision>();
	}

	private void Awake()
	{
		AppState_InGameDecision.s_instance = this;
	}

	protected override void OnEnter()
	{
		if (UILoadingScreenPanel.Get() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AppState_InGameDecision.OnEnter()).MethodHandle;
			}
			UILoadingScreenPanel.Get().SetVisible(false);
		}
		base.RegisterGameStoppedHandler();
		GameFlowData.s_onGameStateChanged += this.OnGameStateChanged;
	}

	protected override void OnLeave()
	{
		base.UnregisterGameStoppedHandler();
		GameFlowData.s_onGameStateChanged -= this.OnGameStateChanged;
	}

	private void OnGameStateChanged(GameState newState)
	{
		if (newState == GameState.BothTeams_Resolve)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AppState_InGameDecision.OnGameStateChanged(GameState)).MethodHandle;
			}
			AppState_InGameResolve.Get().Enter();
		}
		else if (newState == GameState.EndingGame)
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
			AppState_InGameEnding.Get().Enter();
		}
	}
}

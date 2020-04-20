using System;

public class AppState_InGameResolve : AppStateInGame
{
	private static AppState_InGameResolve s_instance;

	public static AppState_InGameResolve Get()
	{
		return AppState_InGameResolve.s_instance;
	}

	public static void Create()
	{
		AppState.Create<AppState_InGameResolve>();
	}

	private void Awake()
	{
		AppState_InGameResolve.s_instance = this;
	}

	protected override void OnEnter()
	{
		if (UILoadingScreenPanel.Get() != null)
		{
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

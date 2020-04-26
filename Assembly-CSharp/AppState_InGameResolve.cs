public class AppState_InGameResolve : AppStateInGame
{
	private static AppState_InGameResolve s_instance;

	public static AppState_InGameResolve Get()
	{
		return s_instance;
	}

	public static void Create()
	{
		AppState.Create<AppState_InGameResolve>();
	}

	private void Awake()
	{
		s_instance = this;
	}

	protected override void OnEnter()
	{
		if (UILoadingScreenPanel.Get() != null)
		{
			UILoadingScreenPanel.Get().SetVisible(false);
		}
		RegisterGameStoppedHandler();
		GameFlowData.s_onGameStateChanged += OnGameStateChanged;
	}

	protected override void OnLeave()
	{
		UnregisterGameStoppedHandler();
		GameFlowData.s_onGameStateChanged -= OnGameStateChanged;
	}

	private void OnGameStateChanged(GameState newState)
	{
		if (newState == GameState.BothTeams_Decision)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					AppState_InGameDecision.Get().Enter();
					return;
				}
			}
		}
		if (newState != GameState.EndingGame)
		{
			return;
		}
		while (true)
		{
			AppState_InGameEnding.Get().Enter();
			return;
		}
	}
}

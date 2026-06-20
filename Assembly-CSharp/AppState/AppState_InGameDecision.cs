public class AppState_InGameDecision : AppStateInGame
{
	private static AppState_InGameDecision s_instance;

	public static AppState_InGameDecision Get()
	{
		return s_instance;
	}

	public static void Create()
	{
		AppState.Create<AppState_InGameDecision>();
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
		if (newState == GameState.BothTeams_Resolve)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					AppState_InGameResolve.Get().Enter();
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

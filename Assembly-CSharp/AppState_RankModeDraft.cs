public class AppState_RankModeDraft : AppState
{
	private static AppState_RankModeDraft s_instance;

	public static AppState_RankModeDraft Get()
	{
		return s_instance;
	}

	public static void Create()
	{
		AppState.Create<AppState_RankModeDraft>();
	}

	private void Awake()
	{
		s_instance = this;
	}

	protected override void OnEnter()
	{
		UIRankedModeDraftScreen.Get().SetupRankDraft();
		AppState_GroupCharacterSelect.Get().NotifyEnteredRankModeDraft();
		UIFrontEnd.Get().ShowScreen(FrontEndScreenState.RankedModeSelect);
		ClientGameManager.Get().OnDisconnectedFromLobbyServer += HandleDisconnectedFromLobbyServer;
	}

	private void HandleDisconnectedFromLobbyServer(string lastLobbyErrorMessage)
	{
		AppState_LandingPage.Get().Enter(lastLobbyErrorMessage);
	}

	protected override void OnLeave()
	{
		UIRankedModeDraftScreen.Get().DismantleRankDraft();
		ClientGameManager.Get().OnDisconnectedFromLobbyServer -= HandleDisconnectedFromLobbyServer;
	}

	private void Update()
	{
		GameStatus gameStatus = GameManager.Get().GameStatus;
		if (gameStatus < GameStatus.Launched)
		{
			return;
		}
		while (true)
		{
			if (gameStatus != GameStatus.Stopped)
			{
				while (true)
				{
					AppState_GameLoading.Get().Enter(GameManager.Get().GameInfo.GameConfig.GameType);
					return;
				}
			}
			return;
		}
	}
}

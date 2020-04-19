using System;

public class AppState_RankModeDraft : AppState
{
	private static AppState_RankModeDraft s_instance;

	public static AppState_RankModeDraft Get()
	{
		return AppState_RankModeDraft.s_instance;
	}

	public static void Create()
	{
		AppState.Create<AppState_RankModeDraft>();
	}

	private void Awake()
	{
		AppState_RankModeDraft.s_instance = this;
	}

	protected override void OnEnter()
	{
		UIRankedModeDraftScreen.Get().SetupRankDraft();
		AppState_GroupCharacterSelect.Get().NotifyEnteredRankModeDraft();
		UIFrontEnd.Get().ShowScreen(FrontEndScreenState.RankedModeSelect, false);
		ClientGameManager.Get().OnDisconnectedFromLobbyServer += this.HandleDisconnectedFromLobbyServer;
	}

	private void HandleDisconnectedFromLobbyServer(string lastLobbyErrorMessage)
	{
		AppState_LandingPage.Get().Enter(lastLobbyErrorMessage, false);
	}

	protected override void OnLeave()
	{
		UIRankedModeDraftScreen.Get().DismantleRankDraft();
		ClientGameManager.Get().OnDisconnectedFromLobbyServer -= this.HandleDisconnectedFromLobbyServer;
	}

	private void Update()
	{
		GameStatus gameStatus = GameManager.Get().GameStatus;
		if (gameStatus >= GameStatus.Launched)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AppState_RankModeDraft.Update()).MethodHandle;
			}
			if (gameStatus != GameStatus.Stopped)
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
				AppState_GameLoading.Get().Enter(GameManager.Get().GameInfo.GameConfig.GameType);
			}
		}
	}
}

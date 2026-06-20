public class AppStateInGame : AppState
{
	protected void RegisterGameStoppedHandler()
	{
		ClientGameManager clientGameManager = ClientGameManager.Get();
		clientGameManager.OnDisconnectedFromLobbyServer += HandleDisconnectedFromLobbyServer;
		GameManager.Get().OnGameStopped += HandleGameStopped;
	}

	protected void UnregisterGameStoppedHandler()
	{
		ClientGameManager clientGameManager = ClientGameManager.Get();
		clientGameManager.OnDisconnectedFromLobbyServer -= HandleDisconnectedFromLobbyServer;
		GameManager.Get().OnGameStopped -= HandleGameStopped;
	}

	private void HandleDisconnectedFromLobbyServer(string lastLobbyErrorMessage)
	{
		AppState_GameTeardown.Get().Enter(GameResult.ClientNetworkErrorToLobbyServer, lastLobbyErrorMessage);
	}

	private void HandleGameStopped(GameResult gameResult)
	{
		AppState_GameTeardown.Get().Enter(gameResult, null);
	}
}

using System;

public static class GameResultExtensions
{
	public static bool IsCompletedResult(this GameResult value)
	{
		if (value != GameResult.TieGame)
		{
			if (value != GameResult.TeamAWon)
			{
				return value == GameResult.TeamBWon;
			}
		}
		return true;
	}

	public static bool IsConnectionErrorResult(this GameResult gameResult)
	{
		if (gameResult != GameResult.ClientConnectionFailedToLobbyServer)
		{
			if (gameResult != GameResult.ClientConnectionFailedToGameServer)
			{
				if (gameResult != GameResult.ClientNetworkErrorToLobbyServer && gameResult != GameResult.ClientNetworkErrorToGameServer)
				{
					if (gameResult != GameResult.ClientHeartbeatTimeoutToLobbyServer)
					{
						if (gameResult != GameResult.ClientHeartbeatTimeoutToGameServer)
						{
							if (gameResult != GameResult.ClientLoginFailedToLobbyServer && gameResult != GameResult.ClientLoginFailedToGameServer)
							{
								if (gameResult != GameResult.LobbyServerNetworkErrorToClient)
								{
									if (gameResult != GameResult.LobbyServerHeartbeatTimeoutToClient && gameResult != GameResult.GameServerNetworkErrorToClient)
									{
										return gameResult == GameResult.GameServerHeartbeatTimeoutToClient;
									}
								}
							}
						}
					}
				}
			}
		}
		return true;
	}

	public static string GetErrorMessage(this GameResult gameResult)
	{
		string result = string.Empty;
		switch (gameResult)
		{
		case GameResult.ClientConnectionFailedToLobbyServer:
			result = StringUtil.TR("FailedConnectLobbyServer", "GameResultError");
			break;
		case GameResult.ClientConnectionFailedToGameServer:
			result = StringUtil.TR("FailedConnectGameServer", "GameResultError");
			break;
		case GameResult.ClientNetworkErrorToLobbyServer:
			result = StringUtil.TR("DisconnectedLobbyServer", "GameResultError");
			break;
		case GameResult.ClientNetworkErrorToGameServer:
			result = StringUtil.TR("DisconnectedGameServer", "GameResultError");
			break;
		case GameResult.ClientHeartbeatTimeoutToLobbyServer:
			result = StringUtil.TR("NetworkTimeoutLobbyServer", "GameResultError");
			break;
		case GameResult.ClientHeartbeatTimeoutToGameServer:
			result = StringUtil.TR("NetworkTimeoutGameServer", "GameResultError");
			break;
		case GameResult.ClientLoginFailedToLobbyServer:
			result = StringUtil.TR("LoginFailedLobbyServer", "GameResultError");
			break;
		case GameResult.ClientLoginFailedToGameServer:
			result = StringUtil.TR("LoginFailedGameServer", "GameResultError");
			break;
		case GameResult.ClientLoadingTimeout:
			result = StringUtil.TR("LoadingTimeout", "GameResultError");
			break;
		}
		return result;
	}

	public static GameResult Parse(string gameResultString, Team selfTeam)
	{
		GameResult result = GameResult.NoResult;
		if (gameResultString.EqualsIgnoreCase("win"))
		{
			GameResult gameResult;
			if (selfTeam == Team.TeamA)
			{
				gameResult = GameResult.TeamAWon;
			}
			else
			{
				gameResult = GameResult.TeamBWon;
			}
			result = gameResult;
		}
		else if (gameResultString.EqualsIgnoreCase("loss"))
		{
			GameResult gameResult2;
			if (selfTeam == Team.TeamA)
			{
				gameResult2 = GameResult.TeamBWon;
			}
			else
			{
				gameResult2 = GameResult.TeamAWon;
			}
			result = gameResult2;
		}
		else if (gameResultString.EqualsIgnoreCase("tie"))
		{
			result = GameResult.TieGame;
		}
		return result;
	}
}

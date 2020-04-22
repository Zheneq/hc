public static class GameResultExtensions
{
	public static bool IsCompletedResult(this GameResult value)
	{
		int result;
		if (value != GameResult.TieGame)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (value != GameResult.TeamAWon)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				result = ((value == GameResult.TeamBWon) ? 1 : 0);
				goto IL_002c;
			}
		}
		result = 1;
		goto IL_002c;
		IL_002c:
		return (byte)result != 0;
	}

	public static bool IsConnectionErrorResult(this GameResult gameResult)
	{
		int result;
		if (gameResult != GameResult.ClientConnectionFailedToLobbyServer)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (gameResult != GameResult.ClientConnectionFailedToGameServer)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (gameResult != GameResult.ClientNetworkErrorToLobbyServer && gameResult != GameResult.ClientNetworkErrorToGameServer)
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					if (gameResult != GameResult.ClientHeartbeatTimeoutToLobbyServer)
					{
						while (true)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
						if (gameResult != GameResult.ClientHeartbeatTimeoutToGameServer)
						{
							while (true)
							{
								switch (7)
								{
								case 0:
									continue;
								}
								break;
							}
							if (gameResult != GameResult.ClientLoginFailedToLobbyServer && gameResult != GameResult.ClientLoginFailedToGameServer)
							{
								while (true)
								{
									switch (3)
									{
									case 0:
										continue;
									}
									break;
								}
								if (gameResult != GameResult.LobbyServerNetworkErrorToClient)
								{
									while (true)
									{
										switch (2)
										{
										case 0:
											continue;
										}
										break;
									}
									if (gameResult != GameResult.LobbyServerHeartbeatTimeoutToClient && gameResult != GameResult.GameServerNetworkErrorToClient)
									{
										while (true)
										{
											switch (5)
											{
											case 0:
												continue;
											}
											break;
										}
										result = ((gameResult == GameResult.GameServerHeartbeatTimeoutToClient) ? 1 : 0);
										goto IL_00a1;
									}
								}
							}
						}
					}
				}
			}
		}
		result = 1;
		goto IL_00a1;
		IL_00a1:
		return (byte)result != 0;
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
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			int num;
			if (selfTeam == Team.TeamA)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				num = 2;
			}
			else
			{
				num = 3;
			}
			result = (GameResult)num;
		}
		else if (gameResultString.EqualsIgnoreCase("loss"))
		{
			int num2;
			if (selfTeam == Team.TeamA)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				num2 = 3;
			}
			else
			{
				num2 = 2;
			}
			result = (GameResult)num2;
		}
		else if (gameResultString.EqualsIgnoreCase("tie"))
		{
			result = GameResult.TieGame;
		}
		return result;
	}
}

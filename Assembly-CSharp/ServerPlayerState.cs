// ROGUES
// SERVER
using UnityEngine.Networking;

#if SERVER
public class ServerPlayerState
{
	public ServerPlayerInfo PlayerInfo;
	public LobbySessionInfo SessionInfo;
	public NetworkConnection ConnectionPersistent;
	public int ConnectionId;
	public string ConnectionAddress;
	public bool ConnectionReady;
	public bool LocalClient;
	public GameResult GameResult;
	public GameLoadingState GameLoadingState;

	public LobbyGameInfo GameInfo
	{
		get
		{
            return GameManager.Get()?.GameInfo;
        }
    }

	public bool ReplacedWithBots
	{
		get
		{
			return PlayerInfo.ReplacedWithBots;
		}
	}

	public bool IsNPCBot
	{
		get
		{
			return PlayerInfo.IsNPCBot;
		}
	}

	public bool IsLoadTestBot
	{
		get
		{
			return PlayerInfo.IsLoadTestBot;
		}
	}

	public bool IsAIControlled
	{
		get
		{
			return PlayerInfo.IsAIControlled;
		}
	}

	internal void DisconnectAndReplaceWithBots(GameResult gameResult)
	{
		Log.Info($"ServerPlayerState::DisconnectAndReplaceWithBots: {PlayerInfo.Handle} disconnected: {gameResult}");
		if (ConnectionPersistent != null)
		{
			LogGameExit(gameResult);
			ConnectionPersistent.Disconnect();
			ConnectionPersistent = null;
			ConnectionReady = false;
		}
		OnReplaceWithBots();
	}

	internal void OnReplaceWithBots()
	{
		if (!PlayerInfo.ReplacedWithBots)
		{
			LocalClient = false;
			PlayerInfo.ReplacedWithBots = true;
			for (int i = 0; i < PlayerInfo.ProxyPlayerInfos.Count; i++)
			{
				PlayerInfo.ProxyPlayerInfos[i].ReplacedWithBots = true;
			}
		}
	}

	internal void OnReplaceWithHumans()
	{
		if (PlayerInfo.ReplacedWithBots)
		{
			PlayerInfo.ReplacedWithBots = false;
			for (int i = 0; i < PlayerInfo.ProxyPlayerInfos.Count; i++)
			{
				PlayerInfo.ProxyPlayerInfos[i].ReplacedWithBots = false;
			}
		}
	}

	public void LogGameEnter()
	{
		if (GameResult != GameResult.NoResult)
		{
			return;
		}
		Log.Info("Player {0} entering game", new object[]
		{
			SessionInfo.Name
		});
		long num = 0L;
		if (SessionInfo != null)
		{
			num = SessionInfo.AccountId;
		}
		else if (PlayerInfo != null && PlayerInfo.LobbyPlayerInfo != null)
		{
			num = PlayerInfo.LobbyPlayerInfo.AccountId;
		}
		if (num == 0L)
		{
			return;
		}
		EventLogMessage eventLogMessage = new EventLogMessage("match", "EnterGame");
		eventLogMessage.AddData("AccountId", num);
		eventLogMessage.AddData("ProcessCode", (HydrogenConfig.Get() != null) ? HydrogenConfig.Get().ProcessCode : "?");
		if (PlayerInfo != null)
		{
			eventLogMessage.AddData("Handle", PlayerInfo.Handle);
			eventLogMessage.AddData("Character", PlayerInfo.CharacterType);
			eventLogMessage.AddData("Team", PlayerInfo.TeamId);
			if (PlayerInfo.LobbyPlayerInfo != null)
			{
				eventLogMessage.AddData("GroupId", PlayerInfo.LobbyPlayerInfo.GroupIdAtStartOfMatch);
			}
		}
		if (SessionInfo != null)
		{
			eventLogMessage.AddData("SessionToken", SessionInfo.SessionToken);
		}
		GameManager gameManager = GameManager.Get();
		// custom
		if (gameManager != null && gameManager.GameConfig != null)
		{
			eventLogMessage.AddData("Map", gameManager.GameConfig.Map);
		}
		// rogues
		//if (gameManager != null && gameManager.GameMission != null)
		//{
		//	eventLogMessage.AddData("Map", gameManager.GameMission.Map);
		//	eventLogMessage.AddData("Encounter", gameManager.GameMission.Encounter);
		//}
		eventLogMessage.Write();
	}

	public void LogGameExit(GameResult gameResult)
	{
		if (GameResult != GameResult.NoResult)
		{
			return;
		}
		Log.Info($"Player {SessionInfo.Name} exiting game with result {gameResult} ({(GameInfo != null ? GameInfo.Name : "?")})");
		GameResult = gameResult;
		long accountId = 0L;
		if (SessionInfo != null)
		{
			accountId = SessionInfo.AccountId;
		}
		else if (PlayerInfo != null && PlayerInfo.LobbyPlayerInfo != null)
		{
			accountId = PlayerInfo.LobbyPlayerInfo.AccountId;
		}
		if (accountId == 0L)
		{
			return;
		}
		EventLogMessage eventLogMessage = new EventLogMessage("match", "ExitGame");
		eventLogMessage.AddData("AccountId", accountId);
		eventLogMessage.AddData("LeaveReason", gameResult);
		eventLogMessage.AddData("ProcessCode", (HydrogenConfig.Get() != null) ? HydrogenConfig.Get().ProcessCode : "?");
		if (PlayerInfo != null)
		{
			eventLogMessage.AddData("Handle", PlayerInfo.Handle);
			eventLogMessage.AddData("Character", PlayerInfo.CharacterType);
			eventLogMessage.AddData("Team", PlayerInfo.TeamId);
			if (PlayerInfo.LobbyPlayerInfo != null)
			{
				eventLogMessage.AddData("GroupId", PlayerInfo.LobbyPlayerInfo.GroupIdAtStartOfMatch);
			}
		}
		if (SessionInfo != null)
		{
			eventLogMessage.AddData("SessionToken", SessionInfo.SessionToken);
		}
		bool flag = false;
		GameManager gameManager = GameManager.Get();
		if (gameManager != null && gameManager.GameConfig != null)  // GameConfig -> GameMissionin rogues
		{
			eventLogMessage.AddData("Map", gameManager.GameConfig.Map);
			//eventLogMessage.AddData("Encounter", gameManager.GameMission.Encounter);
			eventLogMessage.AddData("GameStatus", gameManager.GameStatus);
			if (gameManager.GameSummary != null)
			{
				eventLogMessage.AddData("GameTime", gameManager.GameSummary.MatchTime);
				eventLogMessage.AddData("GameTurn", gameManager.GameSummary.NumOfTurns);
				PlayerGameSummary playerGameSummary = gameManager.GameSummary.PlayerGameSummaryList.Find((PlayerGameSummary p) => p.AccountId == accountId);
				if (playerGameSummary != null)
				{
					eventLogMessage.AddData("PlayerKills", playerGameSummary.NumKills);
					eventLogMessage.AddData("PlayerDamage", playerGameSummary.TotalPlayerDamage);
					eventLogMessage.AddData("PlayerAssists", playerGameSummary.NumAssists);
					eventLogMessage.AddData("PlayerDeaths", playerGameSummary.NumDeaths);
					eventLogMessage.AddData("PlayerDamageReceived", playerGameSummary.TotalPlayerDamageReceived);
					eventLogMessage.AddData("PlayerHealing", playerGameSummary.TotalPlayerHealing);
					eventLogMessage.AddData("TimebankUsage", playerGameSummary.TimebankUsage.Count);
					for (int i = 0; i < playerGameSummary.TimebankUsage.Count; i++)
					{
						eventLogMessage.AddData(string.Format("TimebankUsed{0}", i), playerGameSummary.TimebankUsage[i]);
					}
				}
			}
			if (gameManager.GameStatus == GameStatus.Stopped)
			{
				flag = true;
			}
		}
		ObjectivePoints objectivePoints = ObjectivePoints.Get();
		if (objectivePoints != null && objectivePoints.m_matchState == ObjectivePoints.MatchState.MatchEnd)
		{
			flag = true;
		}
		eventLogMessage.AddData("GameOver", flag);
		eventLogMessage.Write();
	}

	public override string ToString()
	{
		return string.Format("{0} - {1}", PlayerInfo, SessionInfo);
	}
}
#endif

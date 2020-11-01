using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Networking;

public class ObjectivePoints : NetworkBehaviour
{
	public enum MatchState
	{
		InMatch,
		MatchEnd
	}

	[HideInInspector]
	public bool m_skipEndOfGameCheck;
	public int m_startingPointsTeamA;
	public int m_startingPointsTeamB;
	public int m_timeLimitTurns;
	public bool m_disablePowerupsAfterTimeLimit = true;
	public string m_victoryCondition;
	public string m_victoryConditionOneTurnLeft;
	public VictoryCondition m_teamAVictoryCondition;
	public VictoryCondition m_teamBVictoryCondition;
	public bool m_allowTies = true;
	public List<PointsForCharacter> m_passivePointsForTeamWithCharacter;
	private bool m_inSuddenDeath;
	public RectTransform m_gameModePanelPrefab;
	private UIObjectivePointsPanel m_objectivePointsPanel;
	private SyncListInt m_points = new SyncListInt();
	private int[] m_displayedPoints = new int[2];
	private HashSet<ActorData> m_respawningPlayers;
	private float m_sendGameSummaryToLobbyServer = -1f;
	[SyncVar]
	private GameResult m_gameResult;
	private float m_gameResultFraction = 0.5f;
	[SyncVar]
	private float m_minutesInMatchOnGameEnd;
	private List<MatchObjective> m_objectives;
	[SyncVar(hook = "HookSetMatchState")]
	[HideInInspector]
	public MatchState m_matchState;
	private float m_gameShutdownTime;
	private List<int> m_clientNumDeathInTurn = new List<int>();
	private static ObjectivePoints s_instance;

	private static int kListm_points = 2045097107;

	public GameResult Networkm_gameResult
	{
		get
		{
			return m_gameResult;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_gameResult, 2u);
		}
	}

	public float Networkm_minutesInMatchOnGameEnd
	{
		get
		{
			return m_minutesInMatchOnGameEnd;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_minutesInMatchOnGameEnd, 4u);
		}
	}

	public MatchState Networkm_matchState
	{
		get
		{
			return m_matchState;
		}
		[param: In]
		set
		{
			if (NetworkServer.localClientActive && !syncVarHookGuard)
			{
				syncVarHookGuard = true;
				HookSetMatchState(value);
				syncVarHookGuard = false;
			}
			SetSyncVar(value, ref m_matchState, 8u);
		}
	}

	static ObjectivePoints()
	{
		RegisterSyncListDelegate(typeof(ObjectivePoints), kListm_points, InvokeSyncListm_points);
		NetworkCRC.RegisterBehaviour("ObjectivePoints", 0);
	}

	public static ObjectivePoints Get()
	{
		return s_instance;
	}

	public bool InSuddenDeath()
	{
		return m_inSuddenDeath;
	}

	private void Awake()
	{
		s_instance = this;
		m_skipEndOfGameCheck = false;
		m_points.InitializeBehaviour(this, kListm_points);
	}

	private void Start()
	{
		if (NetworkServer.active)
		{
			m_points.Add(m_startingPointsTeamA);
			m_points.Add(m_startingPointsTeamB);
		}
		for (int i = 0; i <= 1; i++)
		{
			m_clientNumDeathInTurn.Add(0);
		}
		m_displayedPoints[0] = m_points[0];
		m_displayedPoints[1] = m_points[1];
		m_respawningPlayers = new HashSet<ActorData>();
		m_inSuddenDeath = false;
		if (NetworkServer.active)
		{
			Networkm_matchState = MatchState.InMatch;
		}
		m_objectives = new List<MatchObjective>(gameObject.GetComponentsInChildren<MatchObjective>());
	}

	private void OnDestroy()
	{
		if (m_objectivePointsPanel != null)
		{
			Destroy(m_objectivePointsPanel.gameObject);
			m_objectivePointsPanel = null;
		}
		s_instance = null;
	}

	private void Update()
	{
		if (GameFlowData.Get().IsInDecisionState())
		{
			if (m_displayedPoints[0] != m_points[0])
			{
				m_displayedPoints[0] = m_points[0];
			}
			if (m_displayedPoints[1] != m_points[1])
			{
				m_displayedPoints[1] = m_points[1];
			}
		}
		if (HUD_UI.Get() != null
			&& m_objectivePointsPanel == null
			&& m_gameModePanelPrefab != null)
		{
			RectTransform rectTransform = Instantiate(m_gameModePanelPrefab);
			m_objectivePointsPanel = rectTransform.GetComponent<UIObjectivePointsPanel>();
			m_objectivePointsPanel.transform.SetParent(HUD_UI.Get().m_mainScreenPanel.m_gameSpecificRectDisplay.transform);
			m_objectivePointsPanel.transform.localPosition = new Vector3(
				m_objectivePointsPanel.transform.localPosition.x,
				m_objectivePointsPanel.transform.localPosition.y,
				0f);
			m_objectivePointsPanel.Setup(GetInfoString);
			m_objectivePointsPanel.transform.localScale = Vector3.one;
		}
	}

	private void HookSetMatchState(MatchState state)
	{
		Networkm_matchState = state;
		if (m_matchState != MatchState.MatchEnd)
		{
			return;
		}
		m_gameShutdownTime = Time.time + GameManager.Get().GameConfig.GameServerShutdownTime;
		Team myTeam = Team.TeamA;
		Team enemyTeam = Team.TeamB;
		ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
		if (activeOwnedActorData != null && activeOwnedActorData.GetTeam() == Team.TeamB)
		{
			myTeam = Team.TeamB;
			enemyTeam = Team.TeamA;
		}
		GameEventManager.Get().FireEvent(GameEventManager.EventType.MatchEnded, new GameEventManager.MatchEndedArgs
		{
			result = m_gameResult
		});
		UIGameOverScreen.Get().Setup(GameManager.Get().GameConfig.GameType, m_gameResult, m_points[(int)myTeam], m_points[(int)enemyTeam]);
	}

	public void SetVisible(bool visible)
	{
		if (m_objectivePointsPanel != null)
		{
			m_objectivePointsPanel.gameObject.SetActive(visible);
		}
	}

	public float GetTotalMinutesOnMatchEnd()
	{
		return m_minutesInMatchOnGameEnd;
	}

	public void OnTurnTick()
	{
		m_displayedPoints[0] = m_points[0];
		m_displayedPoints[1] = m_points[1];
		Log.Info($"Score: Team A: {m_displayedPoints[0]} Team B: {m_displayedPoints[1]}");
		for (int i = 0; i < m_clientNumDeathInTurn.Count; i++)
		{
			m_clientNumDeathInTurn[i] = 0;
		}
	}

	public void SetUpGameUI(UIGameModePanel UIPanel)
	{
		UIObjectivePointsPanel uIObjectivePointsPanel = UIPanel as UIObjectivePointsPanel;
		if (uIObjectivePointsPanel == null)
		{
			return;
		}
		ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
		Team team = activeOwnedActorData != null ? activeOwnedActorData.GetTeam() : Team.TeamA;
		Team team2 = activeOwnedActorData != null ? activeOwnedActorData.GetEnemyTeam() : Team.TeamB;
		if (activeOwnedActorData != null && activeOwnedActorData.GetTeam() == Team.Spectator)
		{
			return;
		}
		if (m_matchState == MatchState.InMatch)
		{
			string victoryConditionString = "";
			string myTeamLabel = "";
			string myEnemyTeamLabel = "";
			if (m_timeLimitTurns > 0)
			{
				if (!m_inSuddenDeath && GameFlowData.Get().CurrentTurn < m_timeLimitTurns)
				{
					float turnsLeft = m_timeLimitTurns - GameFlowData.Get().CurrentTurn;
					victoryConditionString = !m_victoryCondition.IsNullOrEmpty()
						? string.Format(StringUtil.TR(m_victoryCondition), turnsLeft)
						: string.Format(StringUtil.TR("TurnsLeft", "GameModes"), turnsLeft);
				}
				else
				{
					victoryConditionString = StringUtil.TR("SuddenDeathCondition", "GameModes");
				}
			}
			else if (team == Team.TeamA)
			{
				victoryConditionString = !m_teamAVictoryCondition.m_conditionString.IsNullOrEmpty()
					? StringUtil.TR(m_teamAVictoryCondition.m_conditionString)
					: "";
				myTeamLabel = StringUtil.TR(m_teamAVictoryCondition.m_PointName);
				myEnemyTeamLabel = StringUtil.TR(m_teamBVictoryCondition.m_PointName);
			}
			else if (team == Team.TeamB)
			{
				victoryConditionString = !m_teamAVictoryCondition.m_conditionString.IsNullOrEmpty()
					? StringUtil.TR(m_teamAVictoryCondition.m_conditionString)
					: "";
				myTeamLabel = StringUtil.TR(m_teamBVictoryCondition.m_PointName);
				myEnemyTeamLabel = StringUtil.TR(m_teamAVictoryCondition.m_PointName);
			}
			string phaseName = "";
			int phaseId = -1;
			if (GameFlowData.Get().gameState == GameState.BothTeams_Decision)
			{
				phaseId = 0;
				phaseName = StringUtil.TR("Decision", "Global");
			}
			else if (ServerClientUtils.GetCurrentActionPhase() == ActionBufferPhase.Abilities)
			{
				UIQueueListPanel.UIPhase uIPhaseFromAbilityPriority = UIQueueListPanel.GetUIPhaseFromAbilityPriority(ServerClientUtils.GetCurrentAbilityPhase());
				switch (uIPhaseFromAbilityPriority)
				{
					case 0:
						phaseId = 1;
						phaseName = StringUtil.TR("Prep", "Global");
						break;
					case UIQueueListPanel.UIPhase.Evasion:
						phaseId = 2;
						phaseName = StringUtil.TR("Dash", "Global");
						break;
					case UIQueueListPanel.UIPhase.Combat:
						phaseId = 3;
						phaseName = StringUtil.TR("Blast", "Global");
						break;
				}
			}
			else if (ServerClientUtils.GetCurrentActionPhase() == ActionBufferPhase.Movement
				|| ServerClientUtils.GetCurrentActionPhase() == ActionBufferPhase.MovementChase)
			{
				phaseId = 4;
				phaseName = StringUtil.TR("Movement", "Global");
			}
			for (int i = 0; i < uIObjectivePointsPanel.PhaseIndicators.Length; i++)
			{
				uIObjectivePointsPanel.SetPhaseIndicatorActive(phaseId == i, i);
			}
			uIObjectivePointsPanel.SetInMatchValues(myTeamLabel, ActorData.s_friendlyPlayerColor, m_displayedPoints[(int)team], myEnemyTeamLabel, ActorData.s_hostilePlayerColor, m_displayedPoints[(int)team2], victoryConditionString, phaseName);
		}
		else if (m_matchState == MatchState.MatchEnd)
		{
			int timeToExit = (int)(m_gameShutdownTime - Time.time);
			bool hasTeamAWon = m_gameResult == GameResult.TeamAWon;
			bool hasTeamBWon = m_gameResult == GameResult.TeamBWon;
			if (hasTeamAWon && team == Team.TeamA || hasTeamBWon && team == Team.TeamB)
			{
				uIObjectivePointsPanel.SetEndMatchValues(StringUtil.TR("YourTeamWins", "GameModes"), ActorData.s_friendlyPlayerColor, StringUtil.TR("ExitingIn", "GameModes"), timeToExit);
			}
			else if (hasTeamAWon && team == Team.TeamB || hasTeamBWon && team == Team.TeamA)
			{
				uIObjectivePointsPanel.SetEndMatchValues(StringUtil.TR("EnemyTeamWins", "GameModes"), ActorData.s_hostilePlayerColor, StringUtil.TR("ExitingIn", "GameModes"), timeToExit);
			}
			else
			{
				uIObjectivePointsPanel.SetEndMatchValues(StringUtil.TR("Draw", "GameModes"), Color.white, StringUtil.TR("ExitingIn", "GameModes"), timeToExit);
			}
		}
	}

	protected string GetInfoString()
	{
		ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
		Team myTeam = activeOwnedActorData == null ? Team.TeamA : activeOwnedActorData.GetTeam();
		Team enemyTeam = activeOwnedActorData == null ? Team.TeamB : activeOwnedActorData.GetEnemyTeam();
		string colorFriendly = UIUtils.ColorToNGUIRichTextTag(ActorData.s_friendlyPlayerColor);
		string colorHostile = UIUtils.ColorToNGUIRichTextTag(ActorData.s_hostilePlayerColor);
		string result = "";
		if (m_matchState == MatchState.InMatch)
		{
			string victoryConditionString;
			switch (myTeam)
			{
				case Team.TeamA:
					victoryConditionString = StringUtil.TR(m_teamAVictoryCondition.m_conditionString);
					break;
				case Team.TeamB:
					victoryConditionString = StringUtil.TR(m_teamAVictoryCondition.m_conditionString);
					break;
				default:
					victoryConditionString = "";
					break;
			}
			result = string.Format(StringUtil.TR("InMatchInfo", "GameModes"), colorFriendly, m_displayedPoints[(int)myTeam], colorHostile, m_displayedPoints[(int)enemyTeam], victoryConditionString);
		}
		else if (m_matchState == MatchState.MatchEnd)
		{
			int timeToExit = (int)(m_gameShutdownTime - Time.time);
			bool hasTeamAWon = m_gameResult == GameResult.TeamAWon;
			bool hasTeamBWon = m_gameResult == GameResult.TeamBWon;
			if (hasTeamAWon && myTeam == Team.TeamA || hasTeamBWon && myTeam == Team.TeamB)
			{
				result = string.Format(StringUtil.TR("MatchEndTeamAWon", "GameModes"), colorFriendly, timeToExit);
			}
			else if (hasTeamAWon && myTeam == Team.TeamB || hasTeamBWon && myTeam == Team.TeamA)
			{
				result = string.Format(StringUtil.TR("MatchEndTeamBWon", "GameModes"), colorHostile, timeToExit);
			}
			else
			{
				result = string.Format(StringUtil.TR("MatchEndDraw", "GameModes"), timeToExit);
			}
		}
		return result;
	}

	public void Server_OnActorDeath(ActorData actor)
	{
		if (NetworkServer.active && m_matchState == MatchState.InMatch)
		{
			if (GameplayUtils.IsPlayerControlled(actor))
			{
				SpawnPointManager spawnPointManager = SpawnPointManager.Get();
				if (spawnPointManager != null)
				{
					actor.NextRespawnTurn = actor.LastDeathTurn + spawnPointManager.m_respawnDelay;
					Log.Info($"ObjectivePoints: OnActorDeath {actor.DisplayName} turn {GameFlowData.Get().CurrentTurn} " +
						$"next respawn turn {actor.NextRespawnTurn} " +
						$"last death turn {actor.LastDeathTurn} " +
						$"respawn delay {spawnPointManager.m_respawnDelay}");
				}
				m_respawningPlayers.Add(actor);
			}
			foreach (MatchObjective objective in m_objectives)
			{
				objective.Server_OnActorDeath(actor);
			}
		}
	}

	public void Client_OnActorDeath(ActorData actor)
	{
		if (NetworkClient.active)
		{
			foreach (MatchObjective objective in m_objectives)
			{
				objective.Client_OnActorDeath(actor);
			}
			if (actor != null && GameplayUtils.IsPlayerControlled(actor))
			{
				int team = (int)actor.GetTeam();
				if (team >= 0 && team < m_clientNumDeathInTurn.Count)
				{
					m_clientNumDeathInTurn[team]++;
				}
			}
		}
	}

	public int Client_GetNumDeathOnTeamForCurrentTurn(Team team)
	{
		if (team >= Team.TeamA && (int)team < m_clientNumDeathInTurn.Count)
		{
			return m_clientNumDeathInTurn[(int)team];
		}
		return -1;
	}

	private bool HasActorRespawningForTeam(Team team)
	{
		foreach (ActorData current in m_respawningPlayers)
		{
			if (current.GetTeam() == team)
			{
				return true;
			}
		}
		return false;
	}

	[Server]
	private void CheckForEndOfGame()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void ObjectivePoints::CheckForEndOfGame()' called on client");
			return;
		}
		if (m_matchState == MatchState.InMatch
			&& !m_skipEndOfGameCheck
			&& !GameManager.Get().GameConfig.HasGameOption(GameOptionFlag.SkipEndOfGameCheck)
			&& (DebugParameters.Get() == null || !DebugParameters.Get().GetParameterAsBool("DisableGameEndCheck")))
		{
			bool isOvertime = m_timeLimitTurns == 0 || GameFlowData.Get().CurrentTurn >= m_timeLimitTurns;
			int teamAPoints = m_points[0];
			int teamBPoints = m_points[1];
			bool hasTeamAWon = m_teamAVictoryCondition.ArePointConditionsMet(teamAPoints, teamBPoints, isOvertime, Team.TeamA);
			bool hasTeamBWon = m_teamBVictoryCondition.ArePointConditionsMet(teamBPoints, teamAPoints, isOvertime, Team.TeamB);
			bool isGameOver;
			if (hasTeamAWon && hasTeamBWon || !hasTeamAWon && !hasTeamBWon && m_timeLimitTurns > 0)
			{
				if (m_allowTies)
				{
					isGameOver = true;
					Networkm_gameResult = GameResult.TieGame;
					Log.Info($"Tie because: {m_teamAVictoryCondition.GetVictoryLogString(teamAPoints, teamBPoints, isOvertime, Team.TeamA)} " +
						$"AND {m_teamBVictoryCondition.GetVictoryLogString(teamBPoints, teamAPoints, isOvertime, Team.TeamB)}");
				}
				else
				{
					if (isOvertime)
					{
						m_inSuddenDeath = true;
						if (m_disablePowerupsAfterTimeLimit)
						{
							PowerUpManager.Get().SetSpawningEnabled(false);
						}
					}
					isGameOver = false;
				}
			}
			else if (hasTeamAWon)
			{
				isGameOver = true;
				Networkm_gameResult = GameResult.TeamAWon;
				if (teamAPoints + teamBPoints > 0)
				{
					m_gameResultFraction = teamAPoints / (float)(teamAPoints + teamBPoints);
				}
				Log.Info($"Team A won because: {m_teamAVictoryCondition.GetVictoryLogString(teamAPoints, teamBPoints, isOvertime, Team.TeamA)}");
			}
			else if (hasTeamBWon)
			{
				isGameOver = true;
				Networkm_gameResult = GameResult.TeamBWon;
				if (teamAPoints + teamBPoints > 0)
				{
					m_gameResultFraction = teamBPoints / (float)(teamAPoints + teamBPoints);
				}
				Log.Info($"Team B won because: {m_teamAVictoryCondition.GetVictoryLogString(teamBPoints, teamAPoints, isOvertime, Team.TeamB)}");
			}
			else
			{
				isGameOver = false;
			}
			if (isGameOver)
			{
				EndGame();
			}
		}
	}

	[Server]
	internal void DebugEndGame(PlayerData playerData, GameResult debugResult, int matchSeconds, int ggPacksUsedCount, bool ggPacksUsedToSelf, bool playWithFriendsBonus, bool playedLastTurn)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void ObjectivePoints::DebugEndGame(PlayerData,GameResult,System.Int32,System.Int32,System.Boolean,System.Boolean,System.Boolean)' called on client");
		}
	}

	[Server]
	internal void EndGame()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void ObjectivePoints::EndGame()' called on client");
			return;
		}
		Networkm_matchState = MatchState.MatchEnd;
		m_gameShutdownTime = Time.time + GameManager.Get().GameConfig.GameServerShutdownTime;
		GameFlowData.Get().gameState = GameState.EndingGame;
	}

	public void AdjustPoints(int adjustAmount, Team teamToAdjust)
	{
		if (adjustAmount != 0)
		{
			if (teamToAdjust == Team.TeamA)
			{
				m_points[0] += adjustAmount;
			}
			else if (teamToAdjust == Team.TeamB)
			{
				m_points[1] += adjustAmount;
			}
			GameEventManager.Get().FireEvent(
				GameEventManager.EventType.MatchObjectiveEvent,
				new GameEventManager.MatchObjectiveEventArgs
				{
					objective = GameEventManager.MatchObjectiveEventArgs.ObjectiveType.ObjectivePointsGained,
					team = teamToAdjust
				});
		}
	}

	public void AdjustUnresolvedPoints(int adjustAmount, Team teamToAdjust)
	{
		if (adjustAmount != 0)
		{
			switch (teamToAdjust)
			{
				case Team.TeamA:
					m_displayedPoints[0] += adjustAmount;
					break;
				case Team.TeamB:
					m_displayedPoints[1] += adjustAmount;
					break;
				default:
					break;
			}
		}
	}

	public void SetPoints(int setAmount, Team teamToAdjust)
	{
		bool flag = false;
		if (teamToAdjust == Team.TeamA)
		{
			flag = m_points[0] != setAmount;
			if (flag)
			{
				m_points[0] = setAmount;
			}
		}
		else if (teamToAdjust == Team.TeamB)
		{
			flag = m_points[1] != setAmount;
			if (flag)
			{
				m_points[1] = setAmount;
			}
		}
		if (flag)
		{
			GameEventManager.Get().FireEvent(
				GameEventManager.EventType.MatchObjectiveEvent,
				new GameEventManager.MatchObjectiveEventArgs
				{
					objective = GameEventManager.MatchObjectiveEventArgs.ObjectiveType.ObjectivePointsGained,
					team = teamToAdjust
				});
		}
	}

	public int GetPointsForTeam(Team t)
	{
		if (m_points != null && m_points.Count != 0 && m_points.Count > (int)t)
		{
			return m_points[(int)t];
		}
		return 0;
	}

	private void UNetVersion()
	{
	}

	protected static void InvokeSyncListm_points(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("SyncList m_points called on server.");
			return;
		}
		((ObjectivePoints)obj).m_points.HandleMsg(reader);
		Log.Info($"[JSON] {{\"points\":{DefaultJsonSerializer.Serialize(((ObjectivePoints)obj).m_points)}}}");
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		if (forceAll)
		{
			SyncListInt.WriteInstance(writer, m_points);
			writer.Write((int)m_gameResult);
			writer.Write(m_minutesInMatchOnGameEnd);
			writer.Write((int)m_matchState);
			return true;
		}
		bool flag = false;
		if ((syncVarDirtyBits & 1) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			SyncListInt.WriteInstance(writer, m_points);
		}
		if ((syncVarDirtyBits & 2) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			writer.Write((int)m_gameResult);
		}
		if ((syncVarDirtyBits & 4) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			writer.Write(m_minutesInMatchOnGameEnd);
		}
		if ((syncVarDirtyBits & 8) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			writer.Write((int)m_matchState);
		}
		if (!flag)
		{
			writer.WritePackedUInt32(syncVarDirtyBits);
		}
		return flag;
	}

	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
		if (initialState)
		{
			SyncListInt.ReadReference(reader, m_points);
			m_gameResult = (GameResult)reader.ReadInt32();
			m_minutesInMatchOnGameEnd = reader.ReadSingle();
			m_matchState = (MatchState)reader.ReadInt32();
			LogJson();
			return;
		}
		int num = (int)reader.ReadPackedUInt32();
		if ((num & 1) != 0)
		{
			SyncListInt.ReadReference(reader, m_points);
		}
		if ((num & 2) != 0)
		{
			m_gameResult = (GameResult)reader.ReadInt32();
		}
		if ((num & 4) != 0)
		{
			m_minutesInMatchOnGameEnd = reader.ReadSingle();
		}
		if ((num & 8) != 0)
		{
			HookSetMatchState((MatchState)reader.ReadInt32());
		}
		LogJson(num);
	}

	private void LogJson(int mask = System.Int32.MaxValue)
	{
		var jsonLog = new List<string>();
		if ((mask & 1) != 0)
		{
			jsonLog.Add($"\"points\":{DefaultJsonSerializer.Serialize(m_points)}");
		}
		if ((mask & 2) != 0)
		{
			jsonLog.Add($"\"gameResult\":{DefaultJsonSerializer.Serialize(m_gameResult)}");
		}
		if ((mask & 4) != 0)
		{
			jsonLog.Add($"\"minutesInMatchOnGameEnd\":{DefaultJsonSerializer.Serialize(m_minutesInMatchOnGameEnd)}");
		}
		if ((mask & 8) != 0)
		{
			jsonLog.Add($"\"matchState\":{DefaultJsonSerializer.Serialize(Networkm_matchState)}");
		}

		Log.Info($"[JSON] {{\"objectivePoints\":{{{System.String.Join(",", jsonLog.ToArray())}}}}}");
	}
}

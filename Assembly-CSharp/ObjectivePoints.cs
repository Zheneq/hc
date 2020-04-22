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

	private static int kListm_points;

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
			ref MatchState matchState = ref m_matchState;
			if (NetworkServer.localClientActive)
			{
				if (!base.syncVarHookGuard)
				{
					base.syncVarHookGuard = true;
					HookSetMatchState(value);
					base.syncVarHookGuard = false;
				}
			}
			SetSyncVar(value, ref matchState, 8u);
		}
	}

	static ObjectivePoints()
	{
		kListm_points = 2045097107;
		NetworkBehaviour.RegisterSyncListDelegate(typeof(ObjectivePoints), kListm_points, InvokeSyncListm_points);
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
		while (true)
		{
			m_displayedPoints[0] = m_points[0];
			m_displayedPoints[1] = m_points[1];
			m_respawningPlayers = new HashSet<ActorData>();
			m_inSuddenDeath = false;
			if (NetworkServer.active)
			{
				Networkm_matchState = MatchState.InMatch;
			}
			m_objectives = new List<MatchObjective>(base.gameObject.GetComponentsInChildren<MatchObjective>());
			return;
		}
	}

	private void OnDestroy()
	{
		if ((bool)m_objectivePointsPanel)
		{
			Object.Destroy(m_objectivePointsPanel.gameObject);
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
		if (!(HUD_UI.Get() != null))
		{
			return;
		}
		while (true)
		{
			if (!(m_objectivePointsPanel == null))
			{
				return;
			}
			while (true)
			{
				if (m_gameModePanelPrefab != null)
				{
					while (true)
					{
						RectTransform rectTransform = Object.Instantiate(m_gameModePanelPrefab);
						m_objectivePointsPanel = rectTransform.GetComponent<UIObjectivePointsPanel>();
						m_objectivePointsPanel.transform.SetParent(HUD_UI.Get().m_mainScreenPanel.m_gameSpecificRectDisplay.transform);
						Transform transform = m_objectivePointsPanel.transform;
						Vector3 localPosition = m_objectivePointsPanel.transform.localPosition;
						float x = localPosition.x;
						Vector3 localPosition2 = m_objectivePointsPanel.transform.localPosition;
						transform.localPosition = new Vector3(x, localPosition2.y, 0f);
						m_objectivePointsPanel.Setup(GetInfoString);
						m_objectivePointsPanel.transform.localScale = Vector3.one;
						return;
					}
				}
				return;
			}
		}
	}

	private void HookSetMatchState(MatchState state)
	{
		Networkm_matchState = state;
		if (m_matchState != MatchState.MatchEnd)
		{
			return;
		}
		while (true)
		{
			m_gameShutdownTime = Time.time + GameManager.Get().GameConfig.GameServerShutdownTime;
			Team i = Team.TeamA;
			Team i2 = Team.TeamB;
			ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
			if (activeOwnedActorData != null)
			{
				if (activeOwnedActorData.GetTeam() == Team.TeamB)
				{
					i = Team.TeamB;
					i2 = Team.TeamA;
				}
			}
			GameEventManager.Get().FireEvent(GameEventManager.EventType.MatchEnded, new GameEventManager.MatchEndedArgs
			{
				result = m_gameResult
			});
			UIGameOverScreen.Get().Setup(GameManager.Get().GameConfig.GameType, m_gameResult, m_points[(int)i], m_points[(int)i2]);
			return;
		}
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
		Log.Info("Score: Team A: {0} Team B: {1}", m_displayedPoints[0], m_displayedPoints[1]);
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
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
		int num;
		if (activeOwnedActorData == null)
		{
			num = 0;
		}
		else
		{
			num = (int)activeOwnedActorData.GetTeam();
		}
		Team team = (Team)num;
		int num2;
		if (activeOwnedActorData == null)
		{
			num2 = 1;
		}
		else
		{
			num2 = (int)activeOwnedActorData.GetOpposingTeam();
		}
		Team team2 = (Team)num2;
		if (activeOwnedActorData != null)
		{
			if (activeOwnedActorData.GetTeam() == Team.Spectator)
			{
				while (true)
				{
					switch (2)
					{
					default:
						return;
					case 0:
						break;
					}
				}
			}
		}
		Color s_friendlyPlayerColor = ActorData.s_friendlyPlayerColor;
		Color s_hostilePlayerColor = ActorData.s_hostilePlayerColor;
		if (m_matchState == MatchState.InMatch)
		{
			while (true)
			{
				string victoryConditionString;
				string myTeamLabel;
				string myEnemyTeamLabel;
				string phaseName;
				int num4;
				switch (4)
				{
				case 0:
					break;
				default:
					{
						victoryConditionString = string.Empty;
						myTeamLabel = string.Empty;
						myEnemyTeamLabel = string.Empty;
						if (m_timeLimitTurns > 0)
						{
							if (!m_inSuddenDeath)
							{
								if (GameFlowData.Get().CurrentTurn < m_timeLimitTurns)
								{
									float num3 = m_timeLimitTurns - GameFlowData.Get().CurrentTurn;
									victoryConditionString = ((!m_victoryCondition.IsNullOrEmpty()) ? string.Format(StringUtil.TR(m_victoryCondition), num3) : string.Format(StringUtil.TR("TurnsLeft", "GameModes"), num3));
									goto IL_025c;
								}
							}
							victoryConditionString = StringUtil.TR("SuddenDeathCondition", "GameModes");
						}
						else if (team == Team.TeamA)
						{
							victoryConditionString = ((!m_teamAVictoryCondition.m_conditionString.IsNullOrEmpty()) ? StringUtil.TR(m_teamAVictoryCondition.m_conditionString) : string.Empty);
							myTeamLabel = StringUtil.TR(m_teamAVictoryCondition.m_PointName);
							myEnemyTeamLabel = StringUtil.TR(m_teamBVictoryCondition.m_PointName);
						}
						else if (team == Team.TeamB)
						{
							string text;
							if (m_teamAVictoryCondition.m_conditionString.IsNullOrEmpty())
							{
								text = string.Empty;
							}
							else
							{
								text = StringUtil.TR(m_teamAVictoryCondition.m_conditionString);
							}
							victoryConditionString = text;
							myTeamLabel = StringUtil.TR(m_teamBVictoryCondition.m_PointName);
							myEnemyTeamLabel = StringUtil.TR(m_teamAVictoryCondition.m_PointName);
						}
						goto IL_025c;
					}
					IL_025c:
					phaseName = string.Empty;
					num4 = -1;
					if (GameFlowData.Get().gameState == GameState.BothTeams_Decision)
					{
						num4 = 0;
						phaseName = StringUtil.TR("Decision", "Global");
					}
					else if (ServerClientUtils.GetCurrentActionPhase() == ActionBufferPhase.Abilities)
					{
						UIQueueListPanel.UIPhase uIPhaseFromAbilityPriority = UIQueueListPanel.GetUIPhaseFromAbilityPriority(ServerClientUtils.GetCurrentAbilityPhase());
						if (uIPhaseFromAbilityPriority != 0)
						{
							if (uIPhaseFromAbilityPriority != UIQueueListPanel.UIPhase.Evasion)
							{
								if (uIPhaseFromAbilityPriority != UIQueueListPanel.UIPhase.Combat)
								{
								}
								else
								{
									num4 = 3;
									phaseName = StringUtil.TR("Blast", "Global");
								}
							}
							else
							{
								num4 = 2;
								phaseName = StringUtil.TR("Dash", "Global");
							}
						}
						else
						{
							num4 = 1;
							phaseName = StringUtil.TR("Prep", "Global");
						}
					}
					else
					{
						if (ServerClientUtils.GetCurrentActionPhase() != ActionBufferPhase.Movement)
						{
							if (ServerClientUtils.GetCurrentActionPhase() != ActionBufferPhase.MovementChase)
							{
								goto IL_0368;
							}
						}
						num4 = 4;
						phaseName = StringUtil.TR("Movement", "Global");
					}
					goto IL_0368;
					IL_0368:
					for (int i = 0; i < uIObjectivePointsPanel.PhaseIndicators.Length; i++)
					{
						if (num4 == i)
						{
							uIObjectivePointsPanel.SetPhaseIndicatorActive(true, i);
						}
						else
						{
							uIObjectivePointsPanel.SetPhaseIndicatorActive(false, i);
						}
					}
					uIObjectivePointsPanel.SetInMatchValues(myTeamLabel, s_friendlyPlayerColor, m_displayedPoints[(int)team], myEnemyTeamLabel, s_hostilePlayerColor, m_displayedPoints[(int)team2], victoryConditionString, phaseName);
					return;
				}
			}
		}
		if (m_matchState != MatchState.MatchEnd)
		{
			return;
		}
		int timeToExit = (int)(m_gameShutdownTime - Time.time);
		bool flag = m_gameResult == GameResult.TeamAWon;
		bool flag2 = m_gameResult == GameResult.TeamBWon;
		if (flag)
		{
			if (team == Team.TeamA)
			{
				goto IL_0428;
			}
		}
		if (flag2)
		{
			if (team == Team.TeamB)
			{
				goto IL_0428;
			}
		}
		if (flag)
		{
			if (team == Team.TeamB)
			{
				goto IL_048e;
			}
		}
		if (flag2)
		{
			if (team == Team.TeamA)
			{
				goto IL_048e;
			}
		}
		uIObjectivePointsPanel.SetEndMatchValues(StringUtil.TR("Draw", "GameModes"), Color.white, StringUtil.TR("ExitingIn", "GameModes"), timeToExit);
		return;
		IL_048e:
		uIObjectivePointsPanel.SetEndMatchValues(StringUtil.TR("EnemyTeamWins", "GameModes"), s_hostilePlayerColor, StringUtil.TR("ExitingIn", "GameModes"), timeToExit);
		return;
		IL_0428:
		uIObjectivePointsPanel.SetEndMatchValues(StringUtil.TR("YourTeamWins", "GameModes"), s_friendlyPlayerColor, StringUtil.TR("ExitingIn", "GameModes"), timeToExit);
	}

	protected string GetInfoString()
	{
		ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
		Team team = (!(activeOwnedActorData == null)) ? activeOwnedActorData.GetTeam() : Team.TeamA;
		Team team2 = (activeOwnedActorData == null) ? Team.TeamB : activeOwnedActorData.GetOpposingTeam();
		string text = UIUtils.ColorToNGUIRichTextTag(ActorData.s_friendlyPlayerColor);
		string text2 = UIUtils.ColorToNGUIRichTextTag(ActorData.s_hostilePlayerColor);
		string result = string.Empty;
		int num;
		if (m_matchState == MatchState.InMatch)
		{
			string text3;
			if (team == Team.TeamA)
			{
				text3 = StringUtil.TR(m_teamAVictoryCondition.m_conditionString);
			}
			else if (team == Team.TeamB)
			{
				text3 = StringUtil.TR(m_teamAVictoryCondition.m_conditionString);
			}
			else
			{
				text3 = string.Empty;
			}
			result = string.Format(StringUtil.TR("InMatchInfo", "GameModes"), text, m_displayedPoints[(int)team], text2, m_displayedPoints[(int)team2], text3);
		}
		else if (m_matchState == MatchState.MatchEnd)
		{
			num = (int)(m_gameShutdownTime - Time.time);
			bool flag = m_gameResult == GameResult.TeamAWon;
			bool flag2 = m_gameResult == GameResult.TeamBWon;
			if (flag)
			{
				if (team == Team.TeamA)
				{
					goto IL_018e;
				}
			}
			if (flag2)
			{
				if (team == Team.TeamB)
				{
					goto IL_018e;
				}
			}
			if (flag)
			{
				if (team == Team.TeamB)
				{
					goto IL_01c7;
				}
			}
			if (flag2 && team == Team.TeamA)
			{
				goto IL_01c7;
			}
			result = string.Format(StringUtil.TR("MatchEndDraw", "GameModes"), num);
		}
		goto IL_020b;
		IL_020b:
		return result;
		IL_018e:
		result = string.Format(StringUtil.TR("MatchEndTeamAWon", "GameModes"), text, num);
		goto IL_020b;
		IL_01c7:
		result = string.Format(StringUtil.TR("MatchEndTeamBWon", "GameModes"), text2, num);
		goto IL_020b;
	}

	public void Server_OnActorDeath(ActorData actor)
	{
		if (!NetworkServer.active)
		{
			return;
		}
		while (true)
		{
			if (m_matchState != 0)
			{
				return;
			}
			while (true)
			{
				if (GameplayUtils.IsPlayerControlled(actor))
				{
					SpawnPointManager spawnPointManager = SpawnPointManager.Get();
					if (spawnPointManager != null)
					{
						actor.NextRespawnTurn = actor.LastDeathTurn + spawnPointManager.m_respawnDelay;
						Log.Info("ObjectivePoints: OnActorDeath " + actor.DisplayName + " turn " + GameFlowData.Get().CurrentTurn + " next respawn turn " + actor.NextRespawnTurn + " last death turn " + actor.LastDeathTurn + " respawn delay " + spawnPointManager.m_respawnDelay);
					}
					m_respawningPlayers.Add(actor);
				}
				using (List<MatchObjective>.Enumerator enumerator = m_objectives.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						MatchObjective current = enumerator.Current;
						current.Server_OnActorDeath(actor);
					}
					while (true)
					{
						switch (5)
						{
						default:
							return;
						case 0:
							break;
						}
					}
				}
			}
		}
	}

	public void Client_OnActorDeath(ActorData actor)
	{
		if (!NetworkClient.active)
		{
			return;
		}
		while (true)
		{
			using (List<MatchObjective>.Enumerator enumerator = m_objectives.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					MatchObjective current = enumerator.Current;
					current.Client_OnActorDeath(actor);
				}
			}
			if (!(actor != null))
			{
				return;
			}
			while (true)
			{
				if (!GameplayUtils.IsPlayerControlled(actor))
				{
					return;
				}
				while (true)
				{
					int team = (int)actor.GetTeam();
					if (team < 0)
					{
						return;
					}
					while (true)
					{
						if (team < m_clientNumDeathInTurn.Count)
						{
							while (true)
							{
								m_clientNumDeathInTurn[team]++;
								return;
							}
						}
						return;
					}
				}
			}
		}
	}

	public int Client_GetNumDeathOnTeamForCurrentTurn(Team team)
	{
		if (team >= Team.TeamA)
		{
			if ((int)team < m_clientNumDeathInTurn.Count)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						return m_clientNumDeathInTurn[(int)team];
					}
				}
			}
		}
		return -1;
	}

	private bool HasActorRespawningForTeam(Team team)
	{
		bool result = false;
		using (HashSet<ActorData>.Enumerator enumerator = m_respawningPlayers.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ActorData current = enumerator.Current;
				if (current.GetTeam() == team)
				{
					return true;
				}
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					if (true)
					{
						return result;
					}
					/*OpCode not supported: LdMemberToken*/;
					return result;
				}
			}
		}
	}

	[Server]
	private void CheckForEndOfGame()
	{
		if (!NetworkServer.active)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					Debug.LogWarning("[Server] function 'System.Void ObjectivePoints::CheckForEndOfGame()' called on client");
					return;
				}
			}
		}
		if (m_matchState != 0)
		{
			return;
		}
		while (true)
		{
			if (m_skipEndOfGameCheck)
			{
				return;
			}
			while (true)
			{
				if (GameManager.Get().GameConfig.HasGameOption(GameOptionFlag.SkipEndOfGameCheck))
				{
					return;
				}
				if (DebugParameters.Get() != null)
				{
					if (DebugParameters.Get().GetParameterAsBool("DisableGameEndCheck"))
					{
						return;
					}
				}
				bool flag = m_timeLimitTurns == 0 || GameFlowData.Get().CurrentTurn >= m_timeLimitTurns;
				int num = m_points[0];
				int num2 = m_points[1];
				bool flag2 = m_teamAVictoryCondition.ArePointConditionsMet(num, num2, flag, Team.TeamA);
				bool flag3 = m_teamBVictoryCondition.ArePointConditionsMet(num2, num, flag, Team.TeamB);
				int num3;
				if (flag2)
				{
					if (flag3)
					{
						num3 = 1;
						goto IL_014a;
					}
				}
				if (!flag2)
				{
					if (!flag3)
					{
						num3 = ((m_timeLimitTurns > 0) ? 1 : 0);
						goto IL_014a;
					}
				}
				num3 = 0;
				goto IL_014a;
				IL_014a:
				bool flag4;
				if (num3 != 0)
				{
					if (m_allowTies)
					{
						flag4 = true;
						Networkm_gameResult = GameResult.TieGame;
						Log.Info("Tie because: " + m_teamAVictoryCondition.GetVictoryLogString(num, num2, flag, Team.TeamA) + " AND " + m_teamBVictoryCondition.GetVictoryLogString(num2, num, flag, Team.TeamB));
					}
					else
					{
						if (flag)
						{
							m_inSuddenDeath = true;
							if (m_disablePowerupsAfterTimeLimit)
							{
								PowerUpManager.Get().SetSpawningEnabled(false);
							}
						}
						flag4 = false;
					}
				}
				else if (flag2)
				{
					flag4 = true;
					Networkm_gameResult = GameResult.TeamAWon;
					if (num + num2 > 0)
					{
						m_gameResultFraction = (float)num / (float)(num + num2);
					}
					Log.Info("Team A won because: " + m_teamAVictoryCondition.GetVictoryLogString(num, num2, flag, Team.TeamA));
				}
				else if (flag3)
				{
					flag4 = true;
					Networkm_gameResult = GameResult.TeamBWon;
					if (num + num2 > 0)
					{
						m_gameResultFraction = (float)num2 / (float)(num + num2);
					}
					Log.Info("Team B won because: " + m_teamAVictoryCondition.GetVictoryLogString(num2, num, flag, Team.TeamB));
				}
				else
				{
					flag4 = false;
				}
				if (flag4)
				{
					while (true)
					{
						EndGame();
						return;
					}
				}
				return;
			}
		}
	}

	[Server]
	internal void _001D(PlayerData _001D, GameResult _000E, int _0012, int _0015, bool _0016, bool _0013, bool _0018)
	{
		if (NetworkServer.active)
		{
			return;
		}
		while (true)
		{
			Debug.LogWarning("[Server] function 'System.Void ObjectivePoints::DebugEndGame(PlayerData,GameResult,System.Int32,System.Int32,System.Boolean,System.Boolean,System.Boolean)' called on client");
			return;
		}
	}

	[Server]
	internal void EndGame()
	{
		if (!NetworkServer.active)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					Debug.LogWarning("[Server] function 'System.Void ObjectivePoints::EndGame()' called on client");
					return;
				}
			}
		}
		Networkm_matchState = MatchState.MatchEnd;
		m_gameShutdownTime = Time.time + GameManager.Get().GameConfig.GameServerShutdownTime;
		GameFlowData.Get().gameState = GameState.EndingGame;
	}

	public void AdjustPoints(int adjustAmount, Team teamToAdjust)
	{
		if (adjustAmount == 0)
		{
			return;
		}
		while (true)
		{
			if (teamToAdjust == Team.TeamA)
			{
				m_points[0] += adjustAmount;
			}
			else if (teamToAdjust == Team.TeamB)
			{
				m_points[1] += adjustAmount;
			}
			GameEventManager.MatchObjectiveEventArgs matchObjectiveEventArgs = new GameEventManager.MatchObjectiveEventArgs();
			matchObjectiveEventArgs.objective = GameEventManager.MatchObjectiveEventArgs.ObjectiveType.ObjectivePointsGained;
			matchObjectiveEventArgs.team = teamToAdjust;
			GameEventManager.Get().FireEvent(GameEventManager.EventType.MatchObjectiveEvent, matchObjectiveEventArgs);
			return;
		}
	}

	public void AdjustUnresolvedPoints(int adjustAmount, Team teamToAdjust)
	{
		if (adjustAmount == 0)
		{
			return;
		}
		while (true)
		{
			switch (teamToAdjust)
			{
			default:
				return;
			case Team.TeamA:
				m_displayedPoints[0] += adjustAmount;
				return;
			case Team.TeamB:
				break;
			}
			while (true)
			{
				m_displayedPoints[1] += adjustAmount;
				return;
			}
		}
	}

	public void SetPoints(int setAmount, Team teamToAdjust)
	{
		bool flag = false;
		if (teamToAdjust == Team.TeamA)
		{
			flag = (m_points[0] != setAmount);
			if (flag)
			{
				m_points[0] = setAmount;
			}
		}
		else if (teamToAdjust == Team.TeamB)
		{
			flag = (m_points[1] != setAmount);
			if (flag)
			{
				m_points[1] = setAmount;
			}
		}
		if (!flag)
		{
			return;
		}
		while (true)
		{
			GameEventManager.MatchObjectiveEventArgs matchObjectiveEventArgs = new GameEventManager.MatchObjectiveEventArgs();
			matchObjectiveEventArgs.objective = GameEventManager.MatchObjectiveEventArgs.ObjectiveType.ObjectivePointsGained;
			matchObjectiveEventArgs.team = teamToAdjust;
			GameEventManager.Get().FireEvent(GameEventManager.EventType.MatchObjectiveEvent, matchObjectiveEventArgs);
			return;
		}
	}

	public int GetPointsForTeam(Team t)
	{
		if (m_points != null && m_points.Count != 0)
		{
			if (m_points.Count > (int)t)
			{
				return m_points[(int)t];
			}
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
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					Debug.LogError("SyncList m_points called on server.");
					return;
				}
			}
		}
		((ObjectivePoints)obj).m_points.HandleMsg(reader);
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		if (forceAll)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					SyncListInt.WriteInstance(writer, m_points);
					writer.Write((int)m_gameResult);
					writer.Write(m_minutesInMatchOnGameEnd);
					writer.Write((int)m_matchState);
					return true;
				}
			}
		}
		bool flag = false;
		if ((base.syncVarDirtyBits & 1) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			SyncListInt.WriteInstance(writer, m_points);
		}
		if ((base.syncVarDirtyBits & 2) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.Write((int)m_gameResult);
		}
		if ((base.syncVarDirtyBits & 4) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.Write(m_minutesInMatchOnGameEnd);
		}
		if ((base.syncVarDirtyBits & 8) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.Write((int)m_matchState);
		}
		if (!flag)
		{
			writer.WritePackedUInt32(base.syncVarDirtyBits);
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
	}
}

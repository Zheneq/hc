using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Networking;

public class ObjectivePoints : NetworkBehaviour
{
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
	public ObjectivePoints.MatchState m_matchState;

	private float m_gameShutdownTime;

	private List<int> m_clientNumDeathInTurn = new List<int>();

	private static ObjectivePoints s_instance;

	private static int kListm_points = 0x79E5B493;

	static ObjectivePoints()
	{
		NetworkBehaviour.RegisterSyncListDelegate(typeof(ObjectivePoints), ObjectivePoints.kListm_points, new NetworkBehaviour.CmdDelegate(ObjectivePoints.InvokeSyncListm_points));
		NetworkCRC.RegisterBehaviour("ObjectivePoints", 0);
	}

	public static ObjectivePoints Get()
	{
		return ObjectivePoints.s_instance;
	}

	public bool InSuddenDeath()
	{
		return this.m_inSuddenDeath;
	}

	private void Awake()
	{
		ObjectivePoints.s_instance = this;
		this.m_skipEndOfGameCheck = false;
		this.m_points.InitializeBehaviour(this, ObjectivePoints.kListm_points);
	}

	private void Start()
	{
		if (NetworkServer.active)
		{
			this.m_points.Add(this.m_startingPointsTeamA);
			this.m_points.Add(this.m_startingPointsTeamB);
		}
		for (int i = 0; i <= 1; i++)
		{
			this.m_clientNumDeathInTurn.Add(0);
		}
		for (;;)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			break;
		}
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(ObjectivePoints.Start()).MethodHandle;
		}
		this.m_displayedPoints[0] = this.m_points[0];
		this.m_displayedPoints[1] = this.m_points[1];
		this.m_respawningPlayers = new HashSet<ActorData>();
		this.m_inSuddenDeath = false;
		if (NetworkServer.active)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			this.Networkm_matchState = ObjectivePoints.MatchState.InMatch;
		}
		this.m_objectives = new List<MatchObjective>(base.gameObject.GetComponentsInChildren<MatchObjective>());
	}

	private void OnDestroy()
	{
		if (this.m_objectivePointsPanel)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ObjectivePoints.OnDestroy()).MethodHandle;
			}
			UnityEngine.Object.Destroy(this.m_objectivePointsPanel.gameObject);
			this.m_objectivePointsPanel = null;
		}
		ObjectivePoints.s_instance = null;
	}

	private void Update()
	{
		if (GameFlowData.Get().IsInDecisionState())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ObjectivePoints.Update()).MethodHandle;
			}
			if (this.m_displayedPoints[0] != this.m_points[0])
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				this.m_displayedPoints[0] = this.m_points[0];
			}
			if (this.m_displayedPoints[1] != this.m_points[1])
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				this.m_displayedPoints[1] = this.m_points[1];
			}
		}
		if (HUD_UI.Get() != null)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (this.m_objectivePointsPanel == null)
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				if (this.m_gameModePanelPrefab != null)
				{
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					RectTransform rectTransform = UnityEngine.Object.Instantiate<RectTransform>(this.m_gameModePanelPrefab);
					this.m_objectivePointsPanel = rectTransform.GetComponent<UIObjectivePointsPanel>();
					this.m_objectivePointsPanel.transform.SetParent(HUD_UI.Get().m_mainScreenPanel.m_gameSpecificRectDisplay.transform);
					this.m_objectivePointsPanel.transform.localPosition = new Vector3(this.m_objectivePointsPanel.transform.localPosition.x, this.m_objectivePointsPanel.transform.localPosition.y, 0f);
					this.m_objectivePointsPanel.Setup(new Func<string>(this.GetInfoString));
					this.m_objectivePointsPanel.transform.localScale = Vector3.one;
				}
			}
		}
	}

	private void HookSetMatchState(ObjectivePoints.MatchState state)
	{
		this.Networkm_matchState = state;
		if (this.m_matchState == ObjectivePoints.MatchState.MatchEnd)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ObjectivePoints.HookSetMatchState(ObjectivePoints.MatchState)).MethodHandle;
			}
			this.m_gameShutdownTime = Time.time + GameManager.Get().GameConfig.GameServerShutdownTime;
			Team i = Team.TeamA;
			Team i2 = Team.TeamB;
			ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
			if (activeOwnedActorData != null)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (activeOwnedActorData.GetTeam() == Team.TeamB)
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
					i = Team.TeamB;
					i2 = Team.TeamA;
				}
			}
			GameEventManager.Get().FireEvent(GameEventManager.EventType.MatchEnded, new GameEventManager.MatchEndedArgs
			{
				result = this.m_gameResult
			});
			UIGameOverScreen.Get().Setup(GameManager.Get().GameConfig.GameType, this.m_gameResult, this.m_points[(int)i], this.m_points[(int)i2]);
		}
	}

	public void SetVisible(bool visible)
	{
		if (this.m_objectivePointsPanel != null)
		{
			this.m_objectivePointsPanel.gameObject.SetActive(visible);
		}
	}

	public float GetTotalMinutesOnMatchEnd()
	{
		return this.m_minutesInMatchOnGameEnd;
	}

	public void OnTurnTick()
	{
		this.m_displayedPoints[0] = this.m_points[0];
		this.m_displayedPoints[1] = this.m_points[1];
		Log.Info("Score: Team A: {0} Team B: {1}", new object[]
		{
			this.m_displayedPoints[0],
			this.m_displayedPoints[1]
		});
		for (int i = 0; i < this.m_clientNumDeathInTurn.Count; i++)
		{
			this.m_clientNumDeathInTurn[i] = 0;
		}
	}

	public void SetUpGameUI(UIGameModePanel UIPanel)
	{
		UIObjectivePointsPanel uiobjectivePointsPanel = UIPanel as UIObjectivePointsPanel;
		if (uiobjectivePointsPanel == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ObjectivePoints.SetUpGameUI(UIGameModePanel)).MethodHandle;
			}
			return;
		}
		ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
		Team team;
		if (activeOwnedActorData == null)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			team = Team.TeamA;
		}
		else
		{
			team = activeOwnedActorData.GetTeam();
		}
		Team team2 = team;
		Team team3;
		if (activeOwnedActorData == null)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			team3 = Team.TeamB;
		}
		else
		{
			team3 = activeOwnedActorData.GetOpposingTeam();
		}
		Team team4 = team3;
		if (activeOwnedActorData != null)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (activeOwnedActorData.GetTeam() == Team.Spectator)
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
				return;
			}
		}
		Color s_friendlyPlayerColor = ActorData.s_friendlyPlayerColor;
		Color s_hostilePlayerColor = ActorData.s_hostilePlayerColor;
		if (this.m_matchState == ObjectivePoints.MatchState.InMatch)
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
			string victoryConditionString = string.Empty;
			string myTeamLabel = string.Empty;
			string myEnemyTeamLabel = string.Empty;
			if (this.m_timeLimitTurns > 0)
			{
				if (!this.m_inSuddenDeath)
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
					if (GameFlowData.Get().CurrentTurn < this.m_timeLimitTurns)
					{
						float num = (float)(this.m_timeLimitTurns - GameFlowData.Get().CurrentTurn);
						if (this.m_victoryCondition.IsNullOrEmpty())
						{
							victoryConditionString = string.Format(StringUtil.TR("TurnsLeft", "GameModes"), num);
							goto IL_184;
						}
						victoryConditionString = string.Format(StringUtil.TR(this.m_victoryCondition), num);
						goto IL_184;
					}
				}
				victoryConditionString = StringUtil.TR("SuddenDeathCondition", "GameModes");
				IL_184:;
			}
			else if (team2 == Team.TeamA)
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				victoryConditionString = ((!this.m_teamAVictoryCondition.m_conditionString.IsNullOrEmpty()) ? StringUtil.TR(this.m_teamAVictoryCondition.m_conditionString) : string.Empty);
				myTeamLabel = StringUtil.TR(this.m_teamAVictoryCondition.m_PointName);
				myEnemyTeamLabel = StringUtil.TR(this.m_teamBVictoryCondition.m_PointName);
			}
			else if (team2 == Team.TeamB)
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				string text;
				if (this.m_teamAVictoryCondition.m_conditionString.IsNullOrEmpty())
				{
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					text = string.Empty;
				}
				else
				{
					text = StringUtil.TR(this.m_teamAVictoryCondition.m_conditionString);
				}
				victoryConditionString = text;
				myTeamLabel = StringUtil.TR(this.m_teamBVictoryCondition.m_PointName);
				myEnemyTeamLabel = StringUtil.TR(this.m_teamAVictoryCondition.m_PointName);
			}
			string phaseName = string.Empty;
			int num2 = -1;
			if (GameFlowData.Get().gameState == GameState.BothTeams_Decision)
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				num2 = 0;
				phaseName = StringUtil.TR("Decision", "Global");
			}
			else if (ServerClientUtils.GetCurrentActionPhase() == ActionBufferPhase.Abilities)
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
				UIQueueListPanel.UIPhase uiphaseFromAbilityPriority = UIQueueListPanel.GetUIPhaseFromAbilityPriority(ServerClientUtils.GetCurrentAbilityPhase());
				if (uiphaseFromAbilityPriority != UIQueueListPanel.UIPhase.Prep)
				{
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					if (uiphaseFromAbilityPriority != UIQueueListPanel.UIPhase.Evasion)
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
						if (uiphaseFromAbilityPriority != UIQueueListPanel.UIPhase.Combat)
						{
							for (;;)
							{
								switch (1)
								{
								case 0:
									continue;
								}
								break;
							}
						}
						else
						{
							num2 = 3;
							phaseName = StringUtil.TR("Blast", "Global");
						}
					}
					else
					{
						num2 = 2;
						phaseName = StringUtil.TR("Dash", "Global");
					}
				}
				else
				{
					num2 = 1;
					phaseName = StringUtil.TR("Prep", "Global");
				}
			}
			else
			{
				if (ServerClientUtils.GetCurrentActionPhase() != ActionBufferPhase.Movement)
				{
					if (ServerClientUtils.GetCurrentActionPhase() != ActionBufferPhase.MovementChase)
					{
						goto IL_368;
					}
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
				}
				num2 = 4;
				phaseName = StringUtil.TR("Movement", "Global");
			}
			IL_368:
			for (int i = 0; i < uiobjectivePointsPanel.PhaseIndicators.Length; i++)
			{
				if (num2 == i)
				{
					uiobjectivePointsPanel.SetPhaseIndicatorActive(true, i);
				}
				else
				{
					uiobjectivePointsPanel.SetPhaseIndicatorActive(false, i);
				}
			}
			uiobjectivePointsPanel.SetInMatchValues(myTeamLabel, s_friendlyPlayerColor, this.m_displayedPoints[(int)team2], myEnemyTeamLabel, s_hostilePlayerColor, this.m_displayedPoints[(int)team4], victoryConditionString, phaseName);
		}
		else if (this.m_matchState == ObjectivePoints.MatchState.MatchEnd)
		{
			int timeToExit = (int)(this.m_gameShutdownTime - Time.time);
			bool flag = this.m_gameResult == GameResult.TeamAWon;
			bool flag2 = this.m_gameResult == GameResult.TeamBWon;
			if (flag)
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				if (team2 == Team.TeamA)
				{
					goto IL_428;
				}
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			if (flag2)
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
				if (team2 == Team.TeamB)
				{
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						goto IL_428;
					}
				}
			}
			if (flag)
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
				if (team2 == Team.TeamB)
				{
					goto IL_48E;
				}
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			if (flag2)
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (team2 == Team.TeamA)
				{
					for (;;)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						goto IL_48E;
					}
				}
			}
			uiobjectivePointsPanel.SetEndMatchValues(StringUtil.TR("Draw", "GameModes"), Color.white, StringUtil.TR("ExitingIn", "GameModes"), timeToExit);
			return;
			IL_48E:
			uiobjectivePointsPanel.SetEndMatchValues(StringUtil.TR("EnemyTeamWins", "GameModes"), s_hostilePlayerColor, StringUtil.TR("ExitingIn", "GameModes"), timeToExit);
			return;
			IL_428:
			uiobjectivePointsPanel.SetEndMatchValues(StringUtil.TR("YourTeamWins", "GameModes"), s_friendlyPlayerColor, StringUtil.TR("ExitingIn", "GameModes"), timeToExit);
		}
	}

	protected string GetInfoString()
	{
		ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
		Team team = (!(activeOwnedActorData == null)) ? activeOwnedActorData.GetTeam() : Team.TeamA;
		Team team2 = (!(activeOwnedActorData == null)) ? activeOwnedActorData.GetOpposingTeam() : Team.TeamB;
		string text = UIUtils.ColorToNGUIRichTextTag(ActorData.s_friendlyPlayerColor);
		string text2 = UIUtils.ColorToNGUIRichTextTag(ActorData.s_hostilePlayerColor);
		string result = string.Empty;
		if (this.m_matchState == ObjectivePoints.MatchState.InMatch)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ObjectivePoints.GetInfoString()).MethodHandle;
			}
			string text3;
			if (team == Team.TeamA)
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				text3 = StringUtil.TR(this.m_teamAVictoryCondition.m_conditionString);
			}
			else if (team == Team.TeamB)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				text3 = StringUtil.TR(this.m_teamAVictoryCondition.m_conditionString);
			}
			else
			{
				text3 = string.Empty;
			}
			result = string.Format(StringUtil.TR("InMatchInfo", "GameModes"), new object[]
			{
				text,
				this.m_displayedPoints[(int)team],
				text2,
				this.m_displayedPoints[(int)team2],
				text3
			});
		}
		else if (this.m_matchState == ObjectivePoints.MatchState.MatchEnd)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			int num = (int)(this.m_gameShutdownTime - Time.time);
			bool flag = this.m_gameResult == GameResult.TeamAWon;
			bool flag2 = this.m_gameResult == GameResult.TeamBWon;
			if (flag)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (team == Team.TeamA)
				{
					goto IL_18E;
				}
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			if (flag2)
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				if (team == Team.TeamB)
				{
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						goto IL_18E;
					}
				}
			}
			if (flag)
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
				if (team == Team.TeamB)
				{
					goto IL_1C7;
				}
			}
			if (!flag2 || team != Team.TeamA)
			{
				return string.Format(StringUtil.TR("MatchEndDraw", "GameModes"), num);
			}
			IL_1C7:
			return string.Format(StringUtil.TR("MatchEndTeamBWon", "GameModes"), text2, num);
			IL_18E:
			result = string.Format(StringUtil.TR("MatchEndTeamAWon", "GameModes"), text, num);
		}
		return result;
	}

	public void Server_OnActorDeath(ActorData actor)
	{
		if (NetworkServer.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ObjectivePoints.Server_OnActorDeath(ActorData)).MethodHandle;
			}
			if (this.m_matchState == ObjectivePoints.MatchState.InMatch)
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (GameplayUtils.IsPlayerControlled(actor))
				{
					for (;;)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					SpawnPointManager spawnPointManager = SpawnPointManager.Get();
					if (spawnPointManager != null)
					{
						for (;;)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							break;
						}
						actor.NextRespawnTurn = actor.LastDeathTurn + spawnPointManager.m_respawnDelay;
						Log.Info(string.Concat(new object[]
						{
							"ObjectivePoints: OnActorDeath ",
							actor.DisplayName,
							" turn ",
							GameFlowData.Get().CurrentTurn,
							" next respawn turn ",
							actor.NextRespawnTurn,
							" last death turn ",
							actor.LastDeathTurn,
							" respawn delay ",
							spawnPointManager.m_respawnDelay
						}), new object[0]);
					}
					this.m_respawningPlayers.Add(actor);
				}
				using (List<MatchObjective>.Enumerator enumerator = this.m_objectives.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						MatchObjective matchObjective = enumerator.Current;
						matchObjective.Server_OnActorDeath(actor);
					}
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
				}
			}
		}
	}

	public void Client_OnActorDeath(ActorData actor)
	{
		if (NetworkClient.active)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ObjectivePoints.Client_OnActorDeath(ActorData)).MethodHandle;
			}
			using (List<MatchObjective>.Enumerator enumerator = this.m_objectives.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					MatchObjective matchObjective = enumerator.Current;
					matchObjective.Client_OnActorDeath(actor);
				}
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			if (actor != null)
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				if (GameplayUtils.IsPlayerControlled(actor))
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
					int team = (int)actor.GetTeam();
					if (team >= 0)
					{
						for (;;)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							break;
						}
						if (team < this.m_clientNumDeathInTurn.Count)
						{
							for (;;)
							{
								switch (5)
								{
								case 0:
									continue;
								}
								break;
							}
							List<int> clientNumDeathInTurn;
							int index;
							(clientNumDeathInTurn = this.m_clientNumDeathInTurn)[index = team] = clientNumDeathInTurn[index] + 1;
						}
					}
				}
			}
		}
	}

	public int Client_GetNumDeathOnTeamForCurrentTurn(Team team)
	{
		if (team >= Team.TeamA)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ObjectivePoints.Client_GetNumDeathOnTeamForCurrentTurn(Team)).MethodHandle;
			}
			if (team < (Team)this.m_clientNumDeathInTurn.Count)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				return this.m_clientNumDeathInTurn[(int)team];
			}
		}
		return -1;
	}

	private bool HasActorRespawningForTeam(Team team)
	{
		bool result = false;
		using (HashSet<ActorData>.Enumerator enumerator = this.m_respawningPlayers.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ActorData actorData = enumerator.Current;
				if (actorData.GetTeam() == team)
				{
					return true;
				}
			}
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ObjectivePoints.HasActorRespawningForTeam(Team)).MethodHandle;
			}
		}
		return result;
	}

	[Server]
	private void CheckForEndOfGame()
	{
		if (!NetworkServer.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ObjectivePoints.CheckForEndOfGame()).MethodHandle;
			}
			Debug.LogWarning("[Server] function 'System.Void ObjectivePoints::CheckForEndOfGame()' called on client");
			return;
		}
		if (this.m_matchState == ObjectivePoints.MatchState.InMatch)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!this.m_skipEndOfGameCheck)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!GameManager.Get().GameConfig.HasGameOption(GameOptionFlag.SkipEndOfGameCheck))
				{
					if (DebugParameters.Get() != null)
					{
						for (;;)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							break;
						}
						if (DebugParameters.Get().GetParameterAsBool("DisableGameEndCheck"))
						{
							return;
						}
						for (;;)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
					}
					bool flag = this.m_timeLimitTurns == 0 || GameFlowData.Get().CurrentTurn >= this.m_timeLimitTurns;
					int num = this.m_points[0];
					int num2 = this.m_points[1];
					bool flag2 = this.m_teamAVictoryCondition.ArePointConditionsMet(num, num2, flag, Team.TeamA);
					bool flag3 = this.m_teamBVictoryCondition.ArePointConditionsMet(num2, num, flag, Team.TeamB);
					bool flag4;
					if (flag2)
					{
						for (;;)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
						if (flag3)
						{
							flag4 = true;
							goto IL_14A;
						}
						for (;;)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							break;
						}
					}
					if (!flag2)
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
						if (!flag3)
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
							flag4 = (this.m_timeLimitTurns > 0);
							goto IL_147;
						}
					}
					flag4 = false;
					IL_147:
					IL_14A:
					bool flag5 = flag4;
					bool flag6;
					if (flag5)
					{
						if (this.m_allowTies)
						{
							for (;;)
							{
								switch (3)
								{
								case 0:
									continue;
								}
								break;
							}
							flag6 = true;
							this.Networkm_gameResult = GameResult.TieGame;
							Log.Info("Tie because: " + this.m_teamAVictoryCondition.GetVictoryLogString(num, num2, flag, Team.TeamA) + " AND " + this.m_teamBVictoryCondition.GetVictoryLogString(num2, num, flag, Team.TeamB), new object[0]);
						}
						else
						{
							if (flag)
							{
								for (;;)
								{
									switch (7)
									{
									case 0:
										continue;
									}
									break;
								}
								this.m_inSuddenDeath = true;
								if (this.m_disablePowerupsAfterTimeLimit)
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
									PowerUpManager.Get().SetSpawningEnabled(false);
								}
							}
							flag6 = false;
						}
					}
					else if (flag2)
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
						flag6 = true;
						this.Networkm_gameResult = GameResult.TeamAWon;
						if (num + num2 > 0)
						{
							for (;;)
							{
								switch (1)
								{
								case 0:
									continue;
								}
								break;
							}
							this.m_gameResultFraction = (float)num / (float)(num + num2);
						}
						Log.Info("Team A won because: " + this.m_teamAVictoryCondition.GetVictoryLogString(num, num2, flag, Team.TeamA), new object[0]);
					}
					else if (flag3)
					{
						flag6 = true;
						this.Networkm_gameResult = GameResult.TeamBWon;
						if (num + num2 > 0)
						{
							for (;;)
							{
								switch (7)
								{
								case 0:
									continue;
								}
								break;
							}
							this.m_gameResultFraction = (float)num2 / (float)(num + num2);
						}
						Log.Info("Team B won because: " + this.m_teamAVictoryCondition.GetVictoryLogString(num2, num, flag, Team.TeamB), new object[0]);
					}
					else
					{
						flag6 = false;
					}
					if (flag6)
					{
						for (;;)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
						this.EndGame();
					}
				}
			}
		}
	}

	[Server]
	internal void \u001D(PlayerData \u001D, GameResult \u000E, int \u0012, int \u0015, bool \u0016, bool \u0013, bool \u0018)
	{
		if (!NetworkServer.active)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ObjectivePoints.\u001D(PlayerData, GameResult, int, int, bool, bool, bool)).MethodHandle;
			}
			Debug.LogWarning("[Server] function 'System.Void ObjectivePoints::DebugEndGame(PlayerData,GameResult,System.Int32,System.Int32,System.Boolean,System.Boolean,System.Boolean)' called on client");
			return;
		}
	}

	[Server]
	internal void EndGame()
	{
		if (!NetworkServer.active)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ObjectivePoints.EndGame()).MethodHandle;
			}
			Debug.LogWarning("[Server] function 'System.Void ObjectivePoints::EndGame()' called on client");
			return;
		}
		this.Networkm_matchState = ObjectivePoints.MatchState.MatchEnd;
		this.m_gameShutdownTime = Time.time + GameManager.Get().GameConfig.GameServerShutdownTime;
		GameFlowData.Get().gameState = GameState.EndingGame;
	}

	public void AdjustPoints(int adjustAmount, Team teamToAdjust)
	{
		if (adjustAmount != 0)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ObjectivePoints.AdjustPoints(int, Team)).MethodHandle;
			}
			if (teamToAdjust == Team.TeamA)
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
				SyncListInt points;
				(points = this.m_points)[0] = points[0] + adjustAmount;
			}
			else if (teamToAdjust == Team.TeamB)
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				SyncListInt points;
				(points = this.m_points)[1] = points[1] + adjustAmount;
			}
			GameEventManager.MatchObjectiveEventArgs matchObjectiveEventArgs = new GameEventManager.MatchObjectiveEventArgs();
			matchObjectiveEventArgs.objective = GameEventManager.MatchObjectiveEventArgs.ObjectiveType.ObjectivePointsGained;
			matchObjectiveEventArgs.team = teamToAdjust;
			GameEventManager.Get().FireEvent(GameEventManager.EventType.MatchObjectiveEvent, matchObjectiveEventArgs);
		}
	}

	public void AdjustUnresolvedPoints(int adjustAmount, Team teamToAdjust)
	{
		if (adjustAmount != 0)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ObjectivePoints.AdjustUnresolvedPoints(int, Team)).MethodHandle;
			}
			if (teamToAdjust == Team.TeamA)
			{
				this.m_displayedPoints[0] += adjustAmount;
			}
			else if (teamToAdjust == Team.TeamB)
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				this.m_displayedPoints[1] += adjustAmount;
			}
		}
	}

	public void SetPoints(int setAmount, Team teamToAdjust)
	{
		bool flag = false;
		if (teamToAdjust == Team.TeamA)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ObjectivePoints.SetPoints(int, Team)).MethodHandle;
			}
			flag = (this.m_points[0] != setAmount);
			if (flag)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				this.m_points[0] = setAmount;
			}
		}
		else if (teamToAdjust == Team.TeamB)
		{
			flag = (this.m_points[1] != setAmount);
			if (flag)
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
				this.m_points[1] = setAmount;
			}
		}
		if (flag)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			GameEventManager.MatchObjectiveEventArgs matchObjectiveEventArgs = new GameEventManager.MatchObjectiveEventArgs();
			matchObjectiveEventArgs.objective = GameEventManager.MatchObjectiveEventArgs.ObjectiveType.ObjectivePointsGained;
			matchObjectiveEventArgs.team = teamToAdjust;
			GameEventManager.Get().FireEvent(GameEventManager.EventType.MatchObjectiveEvent, matchObjectiveEventArgs);
		}
	}

	public int GetPointsForTeam(Team t)
	{
		if (this.m_points != null && this.m_points.Count != 0)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ObjectivePoints.GetPointsForTeam(Team)).MethodHandle;
			}
			if (this.m_points.Count > (int)t)
			{
				return this.m_points[(int)t];
			}
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		return 0;
	}

	private void UNetVersion()
	{
	}

	public GameResult Networkm_gameResult
	{
		get
		{
			return this.m_gameResult;
		}
		[param: In]
		set
		{
			base.SetSyncVar<GameResult>(value, ref this.m_gameResult, 2U);
		}
	}

	public float Networkm_minutesInMatchOnGameEnd
	{
		get
		{
			return this.m_minutesInMatchOnGameEnd;
		}
		[param: In]
		set
		{
			base.SetSyncVar<float>(value, ref this.m_minutesInMatchOnGameEnd, 4U);
		}
	}

	public ObjectivePoints.MatchState Networkm_matchState
	{
		get
		{
			return this.m_matchState;
		}
		[param: In]
		set
		{
			uint dirtyBit = 8U;
			if (NetworkServer.localClientActive)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(ObjectivePoints.set_Networkm_matchState(ObjectivePoints.MatchState)).MethodHandle;
				}
				if (!base.syncVarHookGuard)
				{
					base.syncVarHookGuard = true;
					this.HookSetMatchState(value);
					base.syncVarHookGuard = false;
				}
			}
			base.SetSyncVar<ObjectivePoints.MatchState>(value, ref this.m_matchState, dirtyBit);
		}
	}

	protected static void InvokeSyncListm_points(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ObjectivePoints.InvokeSyncListm_points(NetworkBehaviour, NetworkReader)).MethodHandle;
			}
			Debug.LogError("SyncList m_points called on server.");
			return;
		}
		((ObjectivePoints)obj).m_points.HandleMsg(reader);
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		if (forceAll)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ObjectivePoints.OnSerialize(NetworkWriter, bool)).MethodHandle;
			}
			SyncListInt.WriteInstance(writer, this.m_points);
			writer.Write((int)this.m_gameResult);
			writer.Write(this.m_minutesInMatchOnGameEnd);
			writer.Write((int)this.m_matchState);
			return true;
		}
		bool flag = false;
		if ((base.syncVarDirtyBits & 1U) != 0U)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!flag)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			SyncListInt.WriteInstance(writer, this.m_points);
		}
		if ((base.syncVarDirtyBits & 2U) != 0U)
		{
			if (!flag)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.Write((int)this.m_gameResult);
		}
		if ((base.syncVarDirtyBits & 4U) != 0U)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!flag)
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.Write(this.m_minutesInMatchOnGameEnd);
		}
		if ((base.syncVarDirtyBits & 8U) != 0U)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!flag)
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
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.Write((int)this.m_matchState);
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
			SyncListInt.ReadReference(reader, this.m_points);
			this.m_gameResult = (GameResult)reader.ReadInt32();
			this.m_minutesInMatchOnGameEnd = reader.ReadSingle();
			this.m_matchState = (ObjectivePoints.MatchState)reader.ReadInt32();
			return;
		}
		int num = (int)reader.ReadPackedUInt32();
		if ((num & 1) != 0)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ObjectivePoints.OnDeserialize(NetworkReader, bool)).MethodHandle;
			}
			SyncListInt.ReadReference(reader, this.m_points);
		}
		if ((num & 2) != 0)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			this.m_gameResult = (GameResult)reader.ReadInt32();
		}
		if ((num & 4) != 0)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			this.m_minutesInMatchOnGameEnd = reader.ReadSingle();
		}
		if ((num & 8) != 0)
		{
			this.HookSetMatchState((ObjectivePoints.MatchState)reader.ReadInt32());
		}
	}

	public enum MatchState
	{
		InMatch,
		MatchEnd
	}
}

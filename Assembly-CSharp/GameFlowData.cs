// ROGUES
// SERVER
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
//using Escalation;
//using Mirror;
using UnityEngine;
using UnityEngine.Networking;

public class GameFlowData : NetworkBehaviour, IGameEventListener
{
	private static GameFlowData s_gameFlowData;
	public static float s_loadingScreenTime = 4f;

	[SyncVar]
	private bool m_pause;
	[SyncVar]
	private bool m_pausedForDebugging;
	[SyncVar]
	private bool m_pausedByPlayerRequest;

	private bool m_pausedForDialog;

	[SyncVar]
	private bool m_pausedForSinglePlayer;
	[SyncVar]
	private ResolutionPauseState m_resolutionPauseState;

	internal PlayerData m_localPlayerData;
	private GameObject m_actorRoot;
	private GameObject m_thinCoverRoot;
	private GameObject m_brushRegionBorderRoot;
	// removed in rogues
	private Team m_selectedTeam;

	private List<ActorData> m_teamAPlayerAndBots = new List<ActorData>();
	private List<ActorData> m_teamBPlayerAndBots = new List<ActorData>();
	private List<ActorData> m_teamObjectsPlayerAndBots = new List<ActorData>();
	private List<ActorData> m_teamA = new List<ActorData>();
	private List<ActorData> m_teamB = new List<ActorData>();
	private List<ActorData> m_teamObjects = new List<ActorData>();
	private List<ActorData> m_actors = new List<ActorData>();
	private List<GameObject> m_players = new List<GameObject>();
	public List<ActorData> m_ownedActorDatas = new List<ActorData>();

	// added in rogues
	//private bool m_markedToSwitchToNextActiveActor;

	private ActorData m_activeOwnedActorData;
	public bool m_oneClassOnTeam = true;
	public GameObject[] m_availableCharacterResourceLinkPrefabs;

	[SyncVar(hook = "HookSetStartTime")]
	public float m_startTime = 5f;
	[SyncVar(hook = "HookSetDeploymentTime")]
	public float m_deploymentTime = 7f;
	[SyncVar(hook = "HookSetTurnTime")]
	public float m_turnTime = 10f;
	[SyncVar(hook = "HookSetMaxTurnTime")]
	public float m_maxTurnTime = 20f;

	public float m_resolveTimeoutLimit = 112f; // 160f in rogues

	// rogues
	//[SyncVar]
	//public int m_reviveTokens;

	private float m_matchStartTime;
	private float m_deploymentStartTime;
	private float m_timeRemainingInDecision = 20f;

	[SyncVar]
	private float m_timeRemainingInDecisionOverflow;
	[SyncVar]
	private bool m_willEnterTimebankMode;

	private const float c_timeRemainingUpdateInterval = 1f;
	private const float c_latencyCorrectionTime = 1f;

	// added in rogues
#if SERVER
	private float m_timeForNextTimeRemainingSync = -1f;
#endif

	private float m_timeInState;
	private float m_timeInStateUnscaled;
	private float m_timeInDecision;

	[SyncVar(hook = "HookSetCurrentTurn")]
	private int m_currentTurn;
	[SyncVar(hook = "HookSetGameState")]
	private GameState m_gameState;

	private static int kRpcRpcUpdateTimeRemaining = 0x3800B000;

	// rogues
	//[SyncVar(hook = "HookSetPlayerActionState")]
	//private PlayerActionStateMachine.StateFlag m_actionsFsmState;

	// rogues
	//[SyncVar]
	//private Team m_actingTeam = Team.Invalid;

	// rogues
	//[SyncVar]
	//private bool m_isInCombat;

	// added in rogues
#if SERVER
	private bool m_displayedTeamTurnNotice;
#endif

	// added in rogues
#if SERVER
	private string c_enemyTeamActingString = "Enemy Turn";
	private string c_allyTeamActingString = "Ally Turn";
#endif
	
	// custom
#if SERVER
	public int LastTurnWithAllPlayersConnected { get; private set; } = 0;
	public int NumTurnsWithNotAllPlayersConnected { get; private set; } = 0;
#endif
	
	static GameFlowData()
	{
		RegisterRpcDelegate(typeof(GameFlowData), kRpcRpcUpdateTimeRemaining, new CmdDelegate(InvokeRpcRpcUpdateTimeRemaining));
		NetworkCRC.RegisterBehaviour("GameFlowData", 0);
		//RegisterRpcDelegate(typeof(GameFlowData), "RpcSetActingTeam", new NetworkBehaviour.CmdDelegate(InvokeRpcRpcSetActingTeam));
	}

	internal static event Action<ActorData> s_onAddActor;
	internal static event Action<ActorData> s_onRemoveActor;
	internal static event Action<ActorData> s_onActiveOwnedActorChange;
	internal static event Action<GameState> s_onGameStateChanged;

	// added in rogues
	//internal static event Action<Team> s_onActingTeamChanged;

	// added in rogues
#if SERVER
	internal static event Action<ActorData, BoardSquarePathInfo, BoardSquarePathInfo> s_onServerActorMoved;
#endif

	// added in rogues
#if SERVER
	internal static event Action<ActorData, BoardSquarePathInfo, BoardSquarePathInfo, List<BoardSquare>> s_onServerActorTeleported;
#endif

	public bool Started { get; private set; }

	public PlayerData LocalPlayerData
	{
		get
		{
			return m_localPlayerData;
		}
	}

	private void Awake()
	{
		s_gameFlowData = this;
		if (ClientGamePrefabInstantiator.Get() != null)
		{
			ClientGamePrefabInstantiator.Get().InstantiatePrefabs();
		}
		else
		{
			Log.Error("ClientGamePrefabInstantiator reference not set on game start");
		}
	}

	private void Start()
	{
		Started = true;
		GameEventManager.Get().FireEvent(GameEventManager.EventType.GameFlowDataStarted, null);
		VisualsLoader.FireSceneLoadedEventIfNoVisualLoader();
		ClientGameManager.Get().DesignSceneStarted = true;
		if (ClientGameManager.Get().PlayerObjectStartedOnClient
			&& AppState.GetCurrent() == AppState_InGameStarting.Get())
		{
			UIScreenManager.Get().TryLoadAndSetupInGameUI();
		}
		ClientGameManager.Get().CheckAndSendClientPreparedForGameStartNotification();
		Log.Info("GameFlowData.Start");
		GameEventManager.Get().AddListener(this, GameEventManager.EventType.GameTeardown);
		GameEventManager.Get().AddListener(this, GameEventManager.EventType.TheatricsAbilityAnimationStart);
		GameEventManager.Get().AddListener(this, GameEventManager.EventType.ServerActionBufferPhaseStart);
		GameEventManager.Get().AddListener(this, GameEventManager.EventType.ServerActionBufferActionsDone);
		// rogues
		//if (EscalationProducer.Get() != null)
		//{
		//	this.Networkm_reviveTokens = EscalationProducer.Get().ReviveTokens;
		//}
	}

	public void OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		switch (eventType)
		{
			case GameEventManager.EventType.GameTeardown:
				Destroy(m_actorRoot);
				Destroy(m_thinCoverRoot);
				Destroy(m_brushRegionBorderRoot);
				CharacterResourceLink.DestroyAudioResources();
				break;
#if SERVER
			case GameEventManager.EventType.TheatricsAbilityAnimationStart:
				if (NetworkServer.active
					&& m_resolutionPauseState == ResolutionPauseState.UnpausedUntilNextAbilityOrPhase)
				{
					GameEventManager.TheatricsAbilityAnimationStartArgs theatricsAbilityAnimationStartArgs = (GameEventManager.TheatricsAbilityAnimationStartArgs)args;
					AbilityPriority abilityPhase = ServerActionBuffer.Get().AbilityPhase;
					if (abilityPhase - AbilityPriority.Prep_Offense > 1
						|| !theatricsAbilityAnimationStartArgs.lastInPhase)
					{
						Networkm_resolutionPauseState = ResolutionPauseState.PausedUntilInput;
					}
				}
				break;
			case GameEventManager.EventType.ServerActionBufferPhaseStart:
				if (NetworkServer.active && m_resolutionPauseState == ResolutionPauseState.UnpausedUntilNextAbilityOrPhase)
				{
					AbilityPriority abilityPhase = ServerActionBuffer.Get().AbilityPhase;
					if (abilityPhase == AbilityPriority.Prep_Defense
						|| abilityPhase - AbilityPriority.Evasion <= 1)
					{
						Networkm_resolutionPauseState = ResolutionPauseState.PausedUntilInput;
					}
				}
				break;
			case GameEventManager.EventType.ServerActionBufferActionsDone:
				if (NetworkServer.active
					&& m_resolutionPauseState == ResolutionPauseState.UnpausedUntilNextAbilityOrPhase)
				{
					Networkm_resolutionPauseState = ResolutionPauseState.PausedUntilInput;
				}
				break;
#endif
		}
	}

	public override void OnStartServer()
	{
		// reactor
		LobbyGameConfig gameConfig = GameManager.Get().GameConfig;
		// rogues
		//MissionData gameMission = GameManager.Get().GameMission;
		Networkm_gameState = GameState.Launched;

		// reactor
		Networkm_turnTime = Convert.ToSingle(gameConfig.TurnTime);
		Networkm_maxTurnTime = Convert.ToSingle(gameConfig.TurnTime)
			+ Mathf.Max(GameWideData.Get().m_tbInitial, GameWideData.Get().m_tbRechargeCap)
			+ GameWideData.Get().m_tbConsumableDuration + 1f;
		m_resolveTimeoutLimit = Convert.ToSingle(gameConfig.ResolveTimeoutLimit);
		// rogues
		//this.Networkm_turnTime = 0f;
		//this.Networkm_maxTurnTime = 0f;
		//this.m_resolveTimeoutLimit = 0f;
	}

	public override void OnStartClient()
	{
		if (CurrentTurn > 0)
		{
			NotifyOnTurnTick();
		}
	}

	private void OnDestroy()
	{
		s_onActiveOwnedActorChange = null;
		s_onAddActor = null;
		s_onRemoveActor = null;
		s_onGameStateChanged = null;

		// added in rogues
#if SERVER
		//GameFlowData.s_onActingTeamChanged = null;
		s_onServerActorMoved = null;
		s_onServerActorTeleported = null;
#endif

		if (GameEventManager.Get() != null)
		{
			GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.GameTeardown);
			GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.TheatricsAbilityAnimationStart);
			GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.ServerActionBufferPhaseStart);
			GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.ServerActionBufferActionsDone);
		}
		m_ownedActorDatas.Clear();
		m_activeOwnedActorData = null;
		m_actors.Clear();
		m_teamA.Clear();
		m_teamAPlayerAndBots.Clear();
		m_teamB.Clear();
		m_teamBPlayerAndBots.Clear();
		m_teamObjects.Clear();
		m_teamObjectsPlayerAndBots.Clear();
		s_gameFlowData = null;
	}

	public bool GetPause()
	{
		return m_pause;
	}

	// removed in rogues
	public bool GetPauseForDialog()
	{
		return m_pausedForDialog;
	}

	// added in rogues
#if SERVER
	public bool IsPausedForDialogue()
	{
		return m_pausedForDialog;
	}
#endif

	public bool GetPauseForSinglePlayer()
	{
		return m_pausedForSinglePlayer;
	}

	public bool GetPauseForDebugging()
	{
		return m_pausedForDebugging;
	}

	public bool GetPausedByPlayerRequest()
	{
		return m_pausedByPlayerRequest;
	}

	internal void SetPausedForDialog(bool pause)
	{
		if (NetworkServer.active && m_pausedForDialog != pause)
		{
			m_pausedForDialog = pause;
			UpdatePause();
			// rogues?
			//UIScreenManager uiscreenManager = UIScreenManager.Get();
			//if (uiscreenManager)
			//{
			//	bool flag = !pause;
			//	uiscreenManager.SetHUDHide(flag, flag, pause, false);
			//}
		}
	}

	internal void SetPausedForSinglePlayer(bool pause)
	{
		if (NetworkServer.active && m_pausedForSinglePlayer != pause)
		{
			Networkm_pausedForSinglePlayer = pause;
			UpdatePause();
		}
	}

	internal void SetPausedForDebugging(bool pause)
	{
		// TODO LOW missing code
	}

	internal void SetPausedForCustomGame(bool pause)
	{
		if (NetworkServer.active && m_pausedByPlayerRequest != pause)
		{
			Networkm_pausedByPlayerRequest = pause;
			UpdatePause();
		}
	}

	public void UpdatePause()
	{
		if (NetworkServer.active)
		{
			bool pause = m_pausedForDialog || m_pausedForSinglePlayer || m_pausedForDebugging || m_pausedByPlayerRequest;
			if (m_pause != pause)
			{
				Networkm_pause = pause;
			}
		}
	}

	internal bool IsResolutionPaused()
	{
		return m_resolutionPauseState == ResolutionPauseState.PausedUntilInput;
	}

	internal bool GetResolutionSingleStepping()
	{
		return m_resolutionPauseState == ResolutionPauseState.PausedUntilInput
			|| m_resolutionPauseState == ResolutionPauseState.UnpausedUntilNextAbilityOrPhase;
	}

	internal void SetResolutionSingleStepping(bool singleStepping)
	{
		if (NetworkServer.active)
		{
			HandleSetResolutionSingleStepping(singleStepping);
		}
		else if (Get() != null && Get().activeOwnedActorData != null)
		{
			Get().activeOwnedActorData.CallCmdSetResolutionSingleStepping(singleStepping);
		}
	}

	internal void SetResolutionSingleSteppingAdvance()
	{
		if (NetworkServer.active)
		{
			HandleSetResolutionSingleSteppingAdvance();
		}
		else if (Get() != null && Get().activeOwnedActorData != null)
		{
			Get().activeOwnedActorData.CallCmdSetResolutionSingleSteppingAdvance();
		}
	}

	[Server]
	private void HandleSetResolutionSingleStepping(bool singleStepping)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void GameFlowData::HandleSetResolutionSingleStepping(System.Boolean)' called on client");
			return;
		}

#if SERVER
		if (singleStepping
			&& m_resolutionPauseState == ResolutionPauseState.Unpaused
			&& GameFlow.Get().FindNumHumanPlayers() == 1U)
		{
			Networkm_resolutionPauseState = ResolutionPauseState.PausedUntilInput;
			return;
		}
		if (!singleStepping)
		{
			Networkm_resolutionPauseState = ResolutionPauseState.Unpaused;
		}
#endif
	}

	[Server]
	private void HandleSetResolutionSingleSteppingAdvance()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void GameFlowData::HandleSetResolutionSingleSteppingAdvance()' called on client");
			return;
		}

#if SERVER
		if (m_resolutionPauseState == ResolutionPauseState.PausedUntilInput)
		{
			Networkm_resolutionPauseState = ResolutionPauseState.UnpausedUntilNextAbilityOrPhase;
		}
#endif
	}

	public static GameFlowData Get()
	{
		return s_gameFlowData;
	}

	public static GameObject FindParentBelowRoot(GameObject child)
	{
		GameObject result = child.transform.parent == null ? null : child;
		GameObject it = child;
		while (it.transform.parent != null)
		{
			result = it;
			it = it.transform.parent.gameObject;
		}
		return result;
	}

	public GameObject GetActorRoot()
	{
		if (m_actorRoot == null)
		{
			m_actorRoot = new GameObject("ActorRoot");
			DontDestroyOnLoad(m_actorRoot);
		}
		return m_actorRoot;
	}

	public GameObject GetThinCoverRoot()
	{
		if (m_thinCoverRoot == null)
		{
			m_thinCoverRoot = new GameObject("ThinCoverRoot");
			DontDestroyOnLoad(m_thinCoverRoot);
		}
		return m_thinCoverRoot;
	}

	public GameObject GetBrushBordersRoot()
	{
		if (m_brushRegionBorderRoot == null)
		{
			m_brushRegionBorderRoot = new GameObject("BrushRegionBorderRoot");
			DontDestroyOnLoad(m_brushRegionBorderRoot);
		}
		return m_brushRegionBorderRoot;
	}

	// removed in rogues
	public Team GetSelectedTeam()
	{
		return m_selectedTeam;
	}

	private void HookSetGameState(GameState state)
	{
		if (m_gameState != state && !NetworkServer.active)
		{
			gameState = state;
		}
	}

	private void HookSetStartTime(float startTime)
	{
		if (m_startTime != startTime)
		{
			Networkm_startTime = startTime;
		}
	}

	private void HookSetDeploymentTime(float deploymentTime)
	{
		if (m_deploymentTime != deploymentTime)
		{
			Networkm_deploymentTime = deploymentTime;
		}
	}

	private void HookSetTurnTime(float turnTime)
	{
		if (m_turnTime != turnTime)
		{
			Networkm_turnTime = turnTime;
		}
	}

	private void HookSetMaxTurnTime(float maxTurnTime)
	{
		if (m_maxTurnTime != maxTurnTime)
		{
			Networkm_maxTurnTime = maxTurnTime;
		}
	}

	private void HookSetCurrentTurn(int turn)
	{
		if (!NetworkServer.active)
		{
			while (m_currentTurn < turn)
			{
				IncrementTurn();
			}
			Networkm_currentTurn = turn;
			HUD_UI.Get().m_mainScreenPanel.m_notificationPanel.NotifyTurnCountSet();
		}
	}

	// added in rogues
	//private void HookSetPlayerActionState(PlayerActionStateMachine.StateFlag state)
	//{
	//	if (!NetworkServer.active)
	//	{
	//		this.Networkm_actionsFsmState = state;
	//	}
	//}

	public int GetClassIndexFromName(string className)
	{
		int result = -1;
		for (int i = 0; i < m_availableCharacterResourceLinkPrefabs.Length; i++)
		{
			GameObject gameObject = m_availableCharacterResourceLinkPrefabs[i];
			CharacterResourceLink crl = gameObject?.GetComponent<CharacterResourceLink>();
			if (crl != null && crl.m_displayName == className)
			{
				result = i;
				break;
			}
		}
		return result;
	}

	public void AddOwnedActorData(ActorData actorData)
	{
		if (m_ownedActorDatas.Contains(actorData))
		{
			return;
		}
		// rogues
		//PveLog.DebugLog(string.Concat(new object[]
		//{
		//	"Adding Owned actor ",
		//	actorData.DebugNameString(),
		//	", Team ",
		//	actorData.GetTeam()
		//}), null);
		if (actorData.GetTeam() != Team.TeamA)
		{
			// reactor
			if (m_ownedActorDatas.Count != 0)
			{
				m_ownedActorDatas.Add(actorData);
				Log.Info($"GameFlowData.AddOwnedActorData {m_ownedActorDatas.Count} {actorData}");
			}
			// rogues
			//this.m_ownedActorDatas.Add(actorData);
		}
		else
		{
			// reactor
			m_ownedActorDatas.Insert(0, actorData);
			Log.Info($"GameFlowData.AddOwnedActorData {0} {actorData}");
			// rogues
			//int num = -1;
			//         int num2 = 0;
			//         while (num2 < this.m_ownedActorDatas.Count && this.m_ownedActorDatas[num2].GetTeam() == Team.TeamA)
			//         {
			//             num = num2;
			//             num2++;
			//         }
			//         this.m_ownedActorDatas.Insert(num + 1, actorData);
		}
		if (activeOwnedActorData == null)
		{
			activeOwnedActorData = actorData;
		}
		// added in rogues
		//if (this.m_ownedActorDatas.Count > 1)
		//{
		//	LobbyGameInfo gameInfo = ClientGameManager.Get().GameInfo;
		//}
	}

	public void ResetOwnedActorDataToFirst()
	{
		if (m_ownedActorDatas.Count > 0)
		{
			if (SpawnPointManager.Get() == null || !SpawnPointManager.Get().m_playersSelectRespawn)
			{
				foreach (ActorData actorData in m_ownedActorDatas)
				{
					if (actorData != null && !actorData.IsDead())
					{
						activeOwnedActorData = actorData;
						return;
					}
				}
			}
			activeOwnedActorData = m_ownedActorDatas[0];
		}
	}

	// added in rogues
	//public void ResetOwnedActorDataToFirst_FCFS()
	//{
	//	if (this.m_ownedActorDatas.Count > 0)
	//	{
	//		if (SpawnPointManager.Get() == null || !SpawnPointManager.Get().m_playersSelectRespawn)
	//		{
	//			using (List<ActorData>.Enumerator enumerator = this.m_ownedActorDatas.GetEnumerator())
	//			{
	//				while (enumerator.MoveNext())
	//				{
	//					ActorData actorData = enumerator.Current;
	//					if (actorData != null && !actorData.IsDead() && actorData.GetTeam() == this.ActingTeam)
	//					{
	//						this.activeOwnedActorData = actorData;
	//						break;
	//					}
	//				}
	//				return;
	//			}
	//		}
	//		foreach (ActorData actorData2 in this.m_ownedActorDatas)
	//		{
	//			if (actorData2 != null && actorData2.GetTeam() == this.ActingTeam && actorData2.GetActorTurnSM().CanStillActInDecision())
	//			{
	//				this.activeOwnedActorData = actorData2;
	//				return;
	//			}
	//		}
	//		foreach (ActorData actorData3 in this.m_ownedActorDatas)
	//		{
	//			if (actorData3 != null && actorData3.GetTeam() == this.ActingTeam)
	//			{
	//				this.activeOwnedActorData = actorData3;
	//				break;
	//			}
	//		}
	//	}
	//}

	public bool IsActorDataOwned(ActorData actorData)
	{
		return actorData != null && m_ownedActorDatas.Contains(actorData);
	}

	public ActorData nextOwnedActorData
	{
		get
		{
			if (m_ownedActorDatas.Count > 0)
			{
				if (activeOwnedActorData == null)
				{
					return m_ownedActorDatas[0];
				}
				else
				{
					int num = 0;
					for (int i = 0; i < m_ownedActorDatas.Count; i++)
					{
						if (m_ownedActorDatas[i] == activeOwnedActorData)
						{
							num = i;
							break;
						}
					}
					return m_ownedActorDatas[(num + 1) % m_ownedActorDatas.Count];
				}
			}
			return null;
		}
	}

	public void SetActiveNextNonConfirmedOwnedActorData()
	{
		if (activeOwnedActorData == null)
		{
			activeOwnedActorData = firstOwnedFriendlyActorData;
			return;
		}
		bool isFound = false;
		int num = 0;
		for (int i = 0; i < m_ownedActorDatas.Count; i++)
		{
			if (m_ownedActorDatas[i] == activeOwnedActorData)
			{
				num = i;
				break;
			}
		}
		for (int i = 0; i < m_ownedActorDatas.Count; i++)
		{
			int index = (num + i) % m_ownedActorDatas.Count;
			ActorData actorData = m_ownedActorDatas[index];
			if (actorData != activeOwnedActorData
				&& activeOwnedActorData != null
				&& actorData.GetTeam() == activeOwnedActorData.GetTeam()
				&& actorData.GetComponent<ActorTurnSM>().CurrentState != TurnStateEnum.CONFIRMED)
			{
				isFound = true;
				activeOwnedActorData = actorData;
				break;
			}
		}
		if (!isFound)
		{
			for (int i = 0; i < m_ownedActorDatas.Count; i++)
			{
				int index = (num + i) % m_ownedActorDatas.Count;
				ActorData actorData = m_ownedActorDatas[index];
				if (actorData != activeOwnedActorData
					&& actorData.GetComponent<ActorTurnSM>().CurrentState != TurnStateEnum.CONFIRMED)
				{
					activeOwnedActorData = actorData;
					break;
				}
			}
		}
	}

	// added in rogues
	//public void MarkToSwitchToNextActiveActor()
	//{
	//	this.m_markedToSwitchToNextActiveActor = true;
	//}

	// added in rogues
	//public void SetActiveNextNonConfirmedOwnedActorData_FCFS()
	//{
	//	if (this.activeOwnedActorData == null)
	//	{
	//		this.activeOwnedActorData = this.firstOwnedFriendlyActorData;
	//		return;
	//	}
	//	int num = this.m_ownedActorDatas.IndexOf(this.activeOwnedActorData);
	//	for (int i = 1; i < this.m_ownedActorDatas.Count; i++)
	//	{
	//		int index = (num + i) % this.m_ownedActorDatas.Count;
	//		ActorData actorData = this.m_ownedActorDatas[index];
	//		if (actorData != this.activeOwnedActorData && actorData.GetTeam() == GameFlowData.Get().ActingTeam && actorData.GetActorTurnSM().CanStillActInDecision())
	//		{
	//			this.activeOwnedActorData = actorData;
	//			return;
	//		}
	//	}
	//}

	// added in rogues
	//public bool HasOwnedActorCanStillActInDecision()
	//{
	//	Team actingTeam = GameFlowData.Get().ActingTeam;
	//	for (int i = 0; i < this.m_ownedActorDatas.Count; i++)
	//	{
	//		ActorData actorData = this.m_ownedActorDatas[i];
	//		if (actorData != null && actorData.GetTeam() == actingTeam && actorData.GetActorTurnSM().CanStillActInDecision())
	//		{
	//			return true;
	//		}
	//	}
	//	return false;
	//}

	// added in rogues
	//public bool CheckAndRequestEndTurnIfNeeded()
	//{
	//	ActorData activeOwnedActorData = this.activeOwnedActorData;
	//	if (activeOwnedActorData != null && this.m_ownedActorDatas.Count > 0 && activeOwnedActorData.GetTeam() == GameFlowData.Get().ActingTeam)
	//	{
	//		bool flag = true;
	//		int num = 0;
	//		while (num < this.m_actors.Count && flag)
	//		{
	//			ActorData actorData = this.m_actors[num];
	//			if (actorData != null && actorData.GetTeam() == activeOwnedActorData.GetTeam() && !this.IsActorReadyToAutoEndTurn(actorData))
	//			{
	//				flag = false;
	//			}
	//			num++;
	//		}
	//		if (flag)
	//		{
	//			this.EndTurnForOwnedActors(activeOwnedActorData.GetTeam());
	//			return true;
	//		}
	//	}
	//	return false;
	//}

	// added in rogues
	//public bool IsActorReadyToAutoEndTurn(ActorData actor)
	//{
	//	if (actor != null)
	//	{
	//		ActorTurnSM actorTurnSM = actor.GetActorTurnSM();
	//		bool flag = actorTurnSM.CurrentState == TurnStateEnum.CONFIRMED || actorTurnSM.CurrentState == TurnStateEnum.WAITING;
	//		bool flag2 = !actor.IsDead() && actorTurnSM.UsedAllFullActions();
	//		bool flag3 = actorTurnSM.CurrentState == TurnStateEnum.PICKING_RESPAWN && actorTurnSM.GetNumRespawnPickInputs() > 0;
	//		return flag || flag2 || flag3;
	//	}
	//	return false;
	//}

	// added in rogues
	//public bool HasOwnedActorWithUnusedFullAction(Team team)
	//{
	//	bool flag = false;
	//	for (int i = 0; i < this.m_ownedActorDatas.Count; i++)
	//	{
	//		ActorData actorData = this.m_ownedActorDatas[i];
	//		if (actorData != null && !actorData.IsDead() && actorData.GetTeam() == team)
	//		{
	//			flag |= !actorData.GetActorTurnSM().UsedAllFullActions();
	//		}
	//	}
	//	return flag;
	//}

	// added in rogues
	//public void EndTurnForOwnedActors(Team team)
	//{
	//	// rogues
	//	//PveLog.DebugLog("Trying to end turn for owned actors, team = " + team, null);
	//	for (int i = 0; i < this.m_ownedActorDatas.Count; i++)
	//	{
	//		ActorData actorData = this.m_ownedActorDatas[i];
	//		if (actorData != null && actorData.GetTeam() == team)
	//		{
	//			actorData.GetActorTurnSM().RequestEndTurn(true);
	//		}
	//	}
	//}

	// void in rogues
	public bool SetActiveOwnedActor_FCFS(ActorData actor)
	{
		if (actor != null && IsActorDataOwned(actor) && activeOwnedActorData != actor)
		{
			activeOwnedActorData = actor;
			return true;
		}
		return false;
	}

	public ActorData firstOwnedFriendlyActorData
	{
		get
		{
			if (m_ownedActorDatas.Count > 0 && activeOwnedActorData != null)
			{
				foreach (ActorData actorData in m_ownedActorDatas)
				{
					if (actorData.GetTeam() == activeOwnedActorData.GetTeam())
					{
						return actorData;
					}
				}
			}
			return null;
		}
	}

	public ActorData firstOwnedEnemyActorData
	{
		get
		{
			if (activeOwnedActorData != null)
			{
				foreach (ActorData actorData in m_ownedActorDatas)
				{
					if (actorData.GetTeam() != activeOwnedActorData.GetTeam())
					{
						return actorData;
					}
				}
			}
			return null;
		}
	}

	internal ActorData POVActorData
	{
		get
		{
			return activeOwnedActorData;
		}
	}

	public ActorData activeOwnedActorData
	{
		get
		{
			return m_activeOwnedActorData;
		}
		set
		{
			bool isChange = m_activeOwnedActorData != value;
			bool isTeamChange = false;
			if (m_activeOwnedActorData != null)
			{
				m_activeOwnedActorData.OnDeselectedAsActiveActor();
				isTeamChange = value != null && value.GetEnemyTeam() == m_activeOwnedActorData.GetTeam();
			}
			m_activeOwnedActorData = value;
			if (m_activeOwnedActorData != null)
			{
				m_activeOwnedActorData.OnSelectedAsActiveActor();
			}
			if (isChange)
			{
				s_onActiveOwnedActorChange?.Invoke(value);
				// rogues
				//PveLog.DebugLog("Set active actor to " + ((this.m_activeOwnedActorData != null) ? this.m_activeOwnedActorData.DebugNameString() : "NULL"), null);
			}
			if (isTeamChange)
			{
				GameEventManager.Get().FireEvent(GameEventManager.EventType.ActiveControlChangedToEnemyTeam, null);
			}
		}
	}

	// added in rogues
#if SERVER
	public static ActorData ClientActor
	{
		get
		{
			return Get()?.activeOwnedActorData;
		}
	}
#endif

	// added in rogues
	//public static bool ClientTeamActing
	//{
	//	get
	//	{
	//		GameFlowData gameFlowData = GameFlowData.Get();
	//		return gameFlowData != null && gameFlowData.activeOwnedActorData != null && gameFlowData.activeOwnedActorData.GetTeam() == gameFlowData.ActingTeam;
	//	}
	//}

	public string GetActiveOwnedActorDataDebugNameString()
	{
		if (activeOwnedActorData)
		{
			return activeOwnedActorData.DebugNameString();
		}
		return "(no owned actor)";
	}

	public void RemoveFromTeam(ActorData actorData)
	{
		m_teamA.Remove(actorData);
		m_teamB.Remove(actorData);
		m_teamObjects.Remove(actorData);
		m_teamAPlayerAndBots.Remove(actorData);
		m_teamBPlayerAndBots.Remove(actorData);
		m_teamObjectsPlayerAndBots.Remove(actorData);
	}

	public void AddToTeam(ActorData actorData)
	{
		if (GameplayUtils.IsPlayerControlled(actorData))
		{
			if (actorData.GetTeam() == Team.TeamA && !m_teamAPlayerAndBots.Contains(actorData))
			{
				m_teamBPlayerAndBots.Remove(actorData);
				m_teamObjectsPlayerAndBots.Remove(actorData);
				m_teamAPlayerAndBots.Add(actorData);
			}
			else if (actorData.GetTeam() == Team.TeamB && !m_teamBPlayerAndBots.Contains(actorData))
			{
				m_teamAPlayerAndBots.Remove(actorData);
				m_teamObjectsPlayerAndBots.Remove(actorData);
				m_teamBPlayerAndBots.Add(actorData);
			}
			else if (actorData.GetTeam() == Team.Objects && !m_teamObjectsPlayerAndBots.Contains(actorData))
			{
				m_teamAPlayerAndBots.Remove(actorData);
				m_teamBPlayerAndBots.Remove(actorData);
				m_teamObjectsPlayerAndBots.Add(actorData);
			}
		}
		if (actorData.GetTeam() == Team.TeamA && !m_teamA.Contains(actorData))
		{
			m_teamB.Remove(actorData);
			m_teamObjects.Remove(actorData);
			m_teamA.Add(actorData);
		}
		else if (actorData.GetTeam() == Team.TeamB && !m_teamB.Contains(actorData))
		{
			m_teamA.Remove(actorData);
			m_teamObjects.Remove(actorData);
			m_teamB.Add(actorData);
		}
		else if (actorData.GetTeam() == Team.Objects && !m_teamObjects.Contains(actorData))
		{
			m_teamA.Remove(actorData);
			m_teamB.Remove(actorData);
			m_teamObjects.Add(actorData);
		}
	}

	private List<ActorData> GetAllActorsOnTeam(Team team)  // public in rogues
	{
		switch (team)
		{
			case Team.TeamA:
				return m_teamA;
			case Team.TeamB:
				return m_teamB;
			case Team.Objects:
				return m_teamObjects;
			default:
				return new List<ActorData>();
		}
	}

	private List<ActorData> GetPlayersAndBotsOnTeam(Team team)
	{
		switch (team)
		{
			case Team.TeamA:
				return m_teamAPlayerAndBots;
			case Team.TeamB:
				return m_teamBPlayerAndBots;
			case Team.Objects:
				return m_teamObjectsPlayerAndBots;
			default:
				return new List<ActorData>();
		}
	}

	public List<GameObject> GetPlayers()
	{
		return m_players;
	}

	public void AddPlayer(GameObject player)
	{
		m_players.Add(player);
		SetLocalPlayerData();
	}

	// removed in rogues
	public void RemoveExistingPlayer(GameObject player)
	{
		if (m_players.Contains(player))
		{
			m_players.Remove(player);
		}
	}

	public List<ActorData> GetActors()
	{
		return m_actors;
	}

	public List<ActorData> GetActorsVisibleToActor(ActorData observer, bool targetableOnly = true)
	{
		List<ActorData> list = new List<ActorData>();
		if (observer != null)
		{
			foreach (ActorData actorData in m_actors)
			{
				if (!actorData.IsDead()
					&& actorData.IsActorVisibleToActor(observer, false)
					&& (!targetableOnly || !actorData.IgnoreForAbilityHits))
				{
					list.Add(actorData);
				}
			}
		}
		return list;
	}

	public List<ActorData> GetAllActorsForPlayer(int playerIndex)
	{
		List<ActorData> list = new List<ActorData>();
		for (int i = 0; i < m_actors.Count; i++)
		{
			if (m_actors[i].PlayerIndex == playerIndex)
			{
				list.Add(m_actors[i]);
			}
		}
		return list;
	}

	public void AddActor(ActorData actor)
	{
		Log.Info($"Registering actor {actor}");
		m_actors.Add(actor);
		if (NetworkServer.active)
		{
			s_onAddActor?.Invoke(actor);
		}
	}

	internal ActorData FindActorByActorIndex(int actorIndex)
	{
		ActorData result = null;
		foreach (ActorData actorData in m_actors)
		{
			if (actorData.ActorIndex == actorIndex)
			{
				result = actorData;
				break;
			}
		}

		// removed in rogues
		if (result == null
			&& actorIndex > 0
			&& CurrentTurn > 0
			&& GameManager.Get() != null
			&& GameManager.Get().GameConfig != null
			&& GameManager.Get().GameConfig.GameType != GameType.Tutorial)
		{
			Log.Warning($"Failed to find actor index {actorIndex}");
		}
		return result;
	}

	internal ActorData FindActorByPlayerIndex(int playerIndex)
	{
		for (int i = 0; i < m_players.Count; i++)
		{
			ActorData component = m_players[i].GetComponent<ActorData>();
			if (component != null && component.PlayerIndex == playerIndex)
			{
				return component;
			}
		}
		Log.Warning($"Failed to find player index {playerIndex}");
		return null;
	}

	internal ActorData FindActorByPlayer(Player player)
	{
		for (int i = 0; i < m_actors.Count; i++)
		{
			PlayerData playerData = m_actors[i].PlayerData;
			if (playerData != null && playerData.GetPlayer() == player)
			{
				return m_actors[i];
			}
		}
		return null;
	}

	internal List<ActorData> GetAllTeamMembers(Team team)
	{
		return GetAllActorsOnTeam(team);
	}

	internal List<ActorData> GetPlayerAndBotTeamMembers(Team team)
	{
		return GetPlayersAndBotsOnTeam(team);
	}

	public void RemoveReferencesToDestroyedActor(ActorData actor)
	{
		if (actor == null)
		{
			Log.Error("Trying to destroy a null actor.");
			return;
		}

		if (m_teamAPlayerAndBots.Contains(actor))
		{
			m_teamAPlayerAndBots.Remove(actor);
		}
		if (m_teamBPlayerAndBots.Contains(actor))
		{
			m_teamBPlayerAndBots.Remove(actor);
		}
		if (m_teamObjectsPlayerAndBots.Contains(actor))
		{
			m_teamObjectsPlayerAndBots.Remove(actor);
		}
		if (m_teamA.Contains(actor))
		{
			m_teamA.Remove(actor);
		}
		if (m_teamB.Contains(actor))
		{
			m_teamB.Remove(actor);
		}
		if (m_teamObjects.Contains(actor))
		{
			m_teamObjects.Remove(actor);
		}
		if (m_players.Contains(actor.gameObject))
		{
			m_players.Remove(actor.gameObject);
		}
		if (m_actors.Contains(actor))
		{
			m_actors.Remove(actor);
		}
		SetLocalPlayerData();
		s_onRemoveActor?.Invoke(actor);
	}

	// added in rogues
	//public bool UpdateIsInCombatFlag()
	//{
	//	bool result = false;
	//	if (NetworkServer.active)
	//	{
	//		bool flag = false;
	//		foreach (ActorData actorData in this.m_actors)
	//		{
	//			if (!actorData.IsDead() && actorData.GetTeam() == Team.TeamB && actorData.Alerted)
	//			{
	//				flag = true;
	//			}
	//		}
	//		if (this.IsInCombat != flag)
	//		{
	//			this.IsInCombat = flag;
	//			result = true;
	//		}
	//	}
	//	return result;
	//}

	public int CurrentTurn
	{
		get
		{
			return m_currentTurn;
		}
	}

	internal GameState gameState
	{
		get
		{
			return m_gameState;
		}
		set
		{
			if (m_gameState != value)
			{
				SetGameState(value);
			}
		}
	}

	// added in rogues
	//internal PlayerActionStateMachine.StateFlag PlayerActionState
	//{
	//	get
	//	{
	//		return this.m_actionsFsmState;
	//	}
	//	set
	//	{
	//		if (!NetworkServer.active)
	//		{
	//			Log.Error("Trying to set PlayerActionState without active server");
	//			return;
	//		}
	//		if (this.m_actionsFsmState != value)
	//		{
	//			this.Networkm_actionsFsmState = value;
	//		}
	//	}
	//}

	// added in rogues
	//internal Team ActingTeam
	//{
	//	get
	//	{
	//		return this.m_actingTeam;
	//	}
	//	set
	//	{
	//		if (!NetworkServer.active)
	//		{
	//			Log.Error("Trying to set ActingTeam without active server");
	//			return;
	//		}
	//		if (this.m_actingTeam != value)
	//		{
	//			this.HandleSetActingTeam(value);
	//			this.CallRpcSetActingTeam((int)value);
	//		}
	//	}
	//}

	// added in rogues
	//private void HandleSetActingTeam(Team team)
	//{
	//	// rogues
	//	//PveLog.DebugLog("Setting ActingTeam to " + team.ToString(), "orange");
	//	this.Networkm_actingTeam = team;
	//	if (GameFlowData.s_onActingTeamChanged != null)
	//	{
	//		GameFlowData.s_onActingTeamChanged(this.m_actingTeam);
	//	}
	//}

	// added in rogues
	//internal bool IsInCombat
	//{
	//	get
	//	{
	//		return this.m_isInCombat;
	//	}
	//	set
	//	{
	//		if (NetworkServer.active && this.m_isInCombat != value)
	//		{
	//			// rogues
	//			//PveLog.LogEncMsg("Setting IsInCombat = " + value.ToString(), null);
	//			this.Networkm_isInCombat = value;
	//		}
	//	}
	//}

	// added in rogues
	//[ClientRpc]
	//private void RpcSetActingTeam(int teamInt)
	//{
	//	if (!NetworkServer.active)
	//	{
	//		this.HandleSetActingTeam((Team)teamInt);
	//	}
	//}

	[ClientRpc]
	private void RpcUpdateTimeRemaining(float timeRemaining)
	{
		if (!NetworkServer.active)
		{
			m_timeRemainingInDecision = timeRemaining - 1f;
		}
	}

	public float GetTimeRemainingInDecision()
	{
		return m_timeRemainingInDecision;
	}

	private void SetGameState(GameState value)
	{
		Log.Info($"GameFlowData::SetGameState {value}");
		// rogues
		//PveLog.DebugLog(string.Concat(new object[]
		//{
		//	"GameState change from [ ",
		//	this.m_gameState,
		//	" ] to [ ",
		//	value,
		//	" ]"
		//}), null);
		Networkm_gameState = value;
		m_timeInState = 0f;
		m_timeInStateUnscaled = 0f;
		Log.Info($"Game state: {value}");
		switch (m_gameState)
		{
			case GameState.StartingGame:
				if (HUD_UI.Get() != null)
				{
					SinglePlayerManager.ResetUIActivations();
				}
				break;
			case GameState.Deployment:
				if (SinglePlayerManager.Get() != null)
				{
					SinglePlayerManager.Get().OnTurnTick();
				}
				m_deploymentStartTime = Time.realtimeSinceStartup;
				break;
			case GameState.BothTeams_Decision:
				if (NetworkServer.active)
				{
					IncrementTurn();
				}
				if (CurrentTurn == 1)
				{
					m_matchStartTime = Time.realtimeSinceStartup;
				}
				ResetOwnedActorDataToFirst();
				m_timeInDecision = 0f;
				break;
				// rogues
				//case GameState.PVE_TeamActions:
				//	this.ResetOwnedActorDataToFirst_FCFS();
				//	this.m_displayedTeamTurnNotice = false;
				//	break;
		}
		s_onGameStateChanged?.Invoke(m_gameState);
	}

	public int GetNumAvailableCharacterResourceLinks()
	{
		return m_availableCharacterResourceLinkPrefabs.Length;
	}

	public string GetFirstAvailableCharacterResourceLinkName()
	{
		GameObject gameObject = m_availableCharacterResourceLinkPrefabs[0];
		CharacterResourceLink crl = gameObject?.GetComponent<CharacterResourceLink>();
		if (crl != null)
		{
			return crl.m_displayName;
		}
		return "";
	}

	// server-only
#if SERVER
	public void ServerIncrementTurn()
	{
		if (NetworkServer.active)
		{
			IncrementTurn();
		}
	}
#endif

	private void IncrementTurn()
	{
		m_timeInDecision = 0f;
		Networkm_currentTurn = m_currentTurn + 1;
		NotifyOnTurnTick();
		Log.Info($"Turn: {CurrentTurn}");
		if (Board.Get() != null)
		{
			Board.Get().MarkForUpdateValidSquares(true);
		}
		
		// custom
#if SERVER
		UIFrontendLoadingScreen.Get()?.StartDisplayError($"Turn: {CurrentTurn}");
#endif
	}

	private void NotifyOnTurnTick()
	{
		// TODO check that all these subsystems are created
		if (TeamSensitiveDataMatchmaker.Get() != null)
		{
			TeamSensitiveDataMatchmaker.Get().SetTeamSensitiveDataForUnhandledActors();
		}
		GameEventManager.Get().FireEvent(GameEventManager.EventType.TurnTick, null);

		// removed in rogues
		ShowIntervanStatusNotifications();

		// added in rogues
#if SERVER
		if (ControlPointManager.Get() != null)
		{
			ControlPointManager.Get().OnTurnTick();
		}
		if (ServerEffectManager.Get() != null)
		{
			ServerEffectManager.Get().OnTurnTick();
		}
#endif

		// rogues
		//if (PveScriptManager.Get() != null)
		//{
		//	PveScriptManager.Get().OnTurnTick();
		//}

		if (ClientResolutionManager.Get() != null)
		{
			ClientResolutionManager.Get().OnTurnStart();
		}
		if (ClientClashManager.Get() != null)
		{
			ClientClashManager.Get().OnTurnStart();
		}
		if (SequenceManager.Get() != null)
		{
			SequenceManager.Get().OnTurnStart(m_currentTurn);
		}
#if !SERVER
		// TODO HACK
		if (InterfaceManager.Get() != null)
		{
			InterfaceManager.Get().OnTurnTick();
		}
#endif
		foreach (PowerUp.IPowerUpListener powerUpListener in PowerUpManager.Get().powerUpListeners)
		{
			powerUpListener.OnTurnTick();
		}
		if (TriggerCoordinator.Get() != null)
		{
			TriggerCoordinator.Get().OnTurnTick();
		}
		if (SinglePlayerManager.Get() != null)
		{
			SinglePlayerManager.Get().OnTurnTick();
		}
		if (TheatricsManager.Get() != null)
		{
			TheatricsManager.Get().OnTurnTick();
		}
		if (SequenceManager.Get() != null)
		{
			SequenceManager.Get().ClientOnTurnResolveEnd();
		}
		if (CameraManager.Get() != null)
		{
			CameraManager.Get().OnTurnTick();
		}
		if (FirstTurnMovement.Get() != null)
		{
			FirstTurnMovement.Get().OnTurnTick();
		}
		if (CollectTheCoins.Get() != null)
		{
			CollectTheCoins.Get().OnTurnTick();
		}
		m_timeRemainingInDecision = Get().m_turnTime;

		// added in rogues
#if SERVER
		Networkm_timeRemainingInDecisionOverflow = 0f;

		foreach (ActorData actorData in Get().GetActors())
		{
			if (actorData != null && actorData.IsHumanControlled())
			{
				Networkm_timeRemainingInDecisionOverflow = Mathf.Max(m_timeRemainingInDecisionOverflow, actorData.GetTimeBank().GetPermittedOverflowTime());
			}
		}
		if (NetworkServer.active)
		{
			// custom
			bool allClientsConnected = ServerGameManager.Get() != null && ServerGameManager.Get().AreAllClientsConnected();
			if (allClientsConnected)
			{
				LastTurnWithAllPlayersConnected = m_currentTurn;
			}
			else
			{
				NumTurnsWithNotAllPlayersConnected++;
			}
			// end custom
			
			if (!m_pause)
			{
				// custom
				if (!allClientsConnected && HydrogenConfig.Get().PendingReconnectTurnTime > Get().m_turnTime)
				{
					Log.Info($"Disconnect detected, extending turn time");
					ServerGameManager.Get()?.SendUnlocalizedConsoleMessage(
						"Not all players are connected. Giving disconnected players more time to reconnect.");

					m_timeRemainingInDecision = HydrogenConfig.Get().PendingReconnectTurnTime;
				}
				else
				{
					m_timeRemainingInDecision = Get().m_turnTime;
				}
				// rogues
				// m_timeRemainingInDecision = Get().m_turnTime;
			}
			m_timeRemainingInDecision += 1f;
		}
		m_timeForNextTimeRemainingSync = -1f;
#endif

		foreach (ActorData actorData in GetActors())
		{
			actorData.OnTurnTick();
		}
		if (ObjectivePoints.Get() != null)
		{
			ObjectivePoints.Get().OnTurnTick();
		}
		if (ClientAbilityResults.DebugTraceOn)
		{
			Log.Warning("Turn Start: <color=magenta>" + Get().CurrentTurn + "</color>");
		}
		if (ControlpadGameplay.Get() != null)
		{
			ControlpadGameplay.Get().OnTurnTick();
		}
	}
	
	// custom - don't wait for timebanks if everyone has confirmed
#if SERVER
	public void UpdateTimeRemainingOverflow()
	{
		float overflow = 0f;

		if (m_timeRemainingInDecision <= 0)
		{
			return;
		}
		
		foreach (ActorData actorData in GetActors())
		{
			Log.Debug($"UpdateTimeRemainingOverflow {actorData.m_displayName} " +
			          $"{actorData.GetTimeBank().GetPermittedOverflowTime()} " +
			          $"{actorData.GetActorTurnSM()?.CurrentState}");
			if (actorData != null
			    && actorData.IsHumanControlled()
			    && actorData.GetActorTurnSM()?.CurrentState != TurnStateEnum.CONFIRMED)
			{
				overflow = Mathf.Max(overflow, actorData.GetTimeBank().GetPermittedOverflowTime());
				Log.Debug($"UpdateTimeRemainingOverflow Waiting for {actorData.m_displayName} ");
			}
		}
		Networkm_timeRemainingInDecisionOverflow = overflow;
	}
#endif

	// removed in rogues
	public bool HasPotentialGameMutatorVisibilityChanges(bool onTurnStart)
	{
		GameplayMutators gameplayMutators = GameplayMutators.Get();
		if (gameplayMutators == null || CurrentTurn <= 1)
		{
			return false;
		}
		for (int i = 0; i < gameplayMutators.m_alwaysOnStatuses.Count; i++)
		{
			GameplayMutators.StatusInterval statusInterval = gameplayMutators.m_alwaysOnStatuses[i];
			if (statusInterval.m_statusType == StatusType.Blind
				|| statusInterval.m_statusType == StatusType.InvisibleToEnemies
				|| statusInterval.m_statusType == StatusType.Revealed)
			{
				if (onTurnStart
					&& GameplayMutators.IsStatusActive(statusInterval.m_statusType, CurrentTurn, GameplayMutators.ActionPhaseCheckMode.Default)
					!= GameplayMutators.IsStatusActive(statusInterval.m_statusType, CurrentTurn - 1, GameplayMutators.ActionPhaseCheckMode.Default))
				{
					return true;
				}
				else if (!onTurnStart
					&& GameplayMutators.IsStatusActive(statusInterval.m_statusType, CurrentTurn, GameplayMutators.ActionPhaseCheckMode.Abilities)
					!= GameplayMutators.IsStatusActive(statusInterval.m_statusType, CurrentTurn, GameplayMutators.ActionPhaseCheckMode.Movement))
				{
					return true;
				}
			}
		}

		for (int i = 0; i < gameplayMutators.m_statusSuppression.Count; i++)
		{
			GameplayMutators.StatusInterval statusInterval = gameplayMutators.m_statusSuppression[i];
			if (statusInterval.m_statusType == StatusType.Blind
				|| statusInterval.m_statusType == StatusType.InvisibleToEnemies
				|| statusInterval.m_statusType == StatusType.Revealed)
			{
				if (onTurnStart
					&& GameplayMutators.IsStatusSuppressed(statusInterval.m_statusType, CurrentTurn, GameplayMutators.ActionPhaseCheckMode.Default)
					!= GameplayMutators.IsStatusSuppressed(statusInterval.m_statusType, CurrentTurn - 1, GameplayMutators.ActionPhaseCheckMode.Default))
				{
					return true;
				}
				else if (!onTurnStart
					&& GameplayMutators.IsStatusSuppressed(statusInterval.m_statusType, CurrentTurn, GameplayMutators.ActionPhaseCheckMode.Abilities)
					!= GameplayMutators.IsStatusSuppressed(statusInterval.m_statusType, CurrentTurn, GameplayMutators.ActionPhaseCheckMode.Movement))
				{
					return true;
				}
			}
		}
		return false;
	}

	// removed in rogues
	private void ShowIntervanStatusNotifications()
	{
		if (!NetworkClient.active || HUD_UI.Get() == null)
		{
			return;
		}
		GameplayMutators gameplayMutators = GameplayMutators.Get();
		if (gameplayMutators == null)
		{
			return;
		}
		for (int i = 0; i < gameplayMutators.m_alwaysOnStatuses.Count; i++)
		{
			GameplayMutators.StatusInterval statusInterval = gameplayMutators.m_alwaysOnStatuses[i];
			if (statusInterval.m_delayTillStartOfMovement)
			{
				bool isActiveOnAbilityPhase = GameplayMutators.IsStatusActive(statusInterval.m_statusType, CurrentTurn, GameplayMutators.ActionPhaseCheckMode.Abilities);
				bool isActiveOnMovementPhase = GameplayMutators.IsStatusActive(statusInterval.m_statusType, CurrentTurn, GameplayMutators.ActionPhaseCheckMode.Movement);
				if (!isActiveOnAbilityPhase
					&& isActiveOnMovementPhase
					&& !string.IsNullOrEmpty(statusInterval.m_activateNotificationTurnBefore))
				{
					InterfaceManager.Get().DisplayAlert(StringUtil.TR_IfHasContext(statusInterval.m_activateNotificationTurnBefore), Color.cyan, 5f, true, 1);
				}
				bool isActiveOnNextTurnAbilityPhase = GameplayMutators.IsStatusActive(statusInterval.m_statusType, CurrentTurn + 1, GameplayMutators.ActionPhaseCheckMode.Abilities);
				if (isActiveOnMovementPhase
					&& !isActiveOnNextTurnAbilityPhase
					&& !string.IsNullOrEmpty(statusInterval.m_offNotificationTurnBefore))
				{
					InterfaceManager.Get().DisplayAlert(StringUtil.TR_IfHasContext(statusInterval.m_offNotificationTurnBefore), Color.cyan, 5f, true, 1);
				}
			}
			else
			{
				bool isActiveThisTurn = GameplayMutators.IsStatusActive(statusInterval.m_statusType, CurrentTurn, GameplayMutators.ActionPhaseCheckMode.Any);
				bool isActiveNextTurn = GameplayMutators.IsStatusActive(statusInterval.m_statusType, CurrentTurn + 1, GameplayMutators.ActionPhaseCheckMode.Any);
				if (isActiveThisTurn
					&& !isActiveNextTurn
					&& !string.IsNullOrEmpty(statusInterval.m_offNotificationTurnBefore))
				{
					InterfaceManager.Get().DisplayAlert(StringUtil.TR_IfHasContext(statusInterval.m_offNotificationTurnBefore), Color.cyan, 5f, true, 1);
				}
				else if (!isActiveThisTurn
					&& isActiveNextTurn
					&& !string.IsNullOrEmpty(statusInterval.m_activateNotificationTurnBefore))
				{
					InterfaceManager.Get().DisplayAlert(StringUtil.TR_IfHasContext(statusInterval.m_activateNotificationTurnBefore), Color.cyan, 5f, true, 1);
				}
			}
		}
		return;
	}

	// added in rogues
#if SERVER
	public void OnServerActorMoved(ActorData mover, BoardSquarePathInfo from, BoardSquarePathInfo to)
	{
		if (NetworkServer.active)
		{
			Action<ActorData, BoardSquarePathInfo, BoardSquarePathInfo> action = s_onServerActorMoved;
			if (action == null)
			{
				return;
			}
			action(mover, from, to);
		}
	}
#endif

	// added in rogues
#if SERVER
	public void OnServerActorTeleported(ActorData mover, BoardSquarePathInfo from, BoardSquarePathInfo to, List<BoardSquare> squaresInLine)
	{
		if (NetworkServer.active)
		{
			Action<ActorData, BoardSquarePathInfo, BoardSquarePathInfo, List<BoardSquare>> action = s_onServerActorTeleported;
			if (action == null)
			{
				return;
			}
			action(mover, from, to, squaresInLine);
		}
	}
#endif

	// rogues
	//public void NotifyOnActorRevived(ActorData actor)
	//{
	//	if (ObjectivePoints.Get() != null)
	//	{
	//		ObjectivePoints.Get().Server_OnActorRevived(actor);
	//	}
	//}

	public void NotifyOnActorDeath(ActorData actor)
	{
		if (NetworkServer.active)
		{
#if SERVER
			if (ServerEffectManager.Get() != null)
			{
				ServerEffectManager.Get().OnActorDeath(actor);
			}
			if (ObjectivePoints.Get() != null)
			{
				ObjectivePoints.Get().Server_OnActorDeath(actor);
			}
			// TODO CTF CTC
			//if (CaptureTheFlag.Get() != null)
			//{
			//	CaptureTheFlag.Get().OnActorDeath(actor);
			//}
			if (CoinCarnageManager.Get() != null)
			{
				CoinCarnageManager.Get().OnActorDeath(actor);
			}
			// TODO CTF CTC
			//if (CollectTheCoins.Get() != null)
			//{
			//	CollectTheCoins.Get().OnActorDeath(actor);
			//}
			// TODO NPC
			//if (NPCCoordinator.Get() != null)
			//{
			//	NPCCoordinator.Get().OnActorDeath(actor);
			//}
#endif
		}
		if (SinglePlayerManager.Get() != null)
		{
			SinglePlayerManager.Get().OnActorDeath(actor);
		}

		// removed in rogues
		if (NPCCoordinator.Get() != null)
		{
			NPCCoordinator.Get().OnActorDeath(actor);
		}

		SatelliteController[] components = actor.GetComponents<SatelliteController>();
		foreach (SatelliteController satelliteController in components)
		{
			satelliteController.OnActorDeath();
		}
		GameEventManager.Get().FireEvent(GameEventManager.EventType.PostCharacterDeath, new GameEventManager.CharacterDeathEventArgs
		{
			deadCharacter = actor
		});
	}

	// rogues
	//public static bool PauseForPvEPrototype()
	//{
	//	return GameWideData.Get() != null;
	//}

	public float GetTimeInState()
	{
		return m_timeInState;
	}

	public float GetTimeInDecision()
	{
		return m_timeInDecision;
	}

	public float GetTimeSinceDeployment()
	{
		return Time.realtimeSinceStartup - m_deploymentStartTime;
	}

	public float GetTimeLeftInTurn()
	{
		float num;
		if (IsInDecisionState())
		{
			num = m_turnTime - GetTimeInDecision();
			if (num == 0f)
			{
				num = 0f;
			}
		}
		else
		{
			num = 0f;
		}
		return num;
	}

	public float GetGameTime()
	{
		return Time.realtimeSinceStartup - m_matchStartTime;
	}

	private void Update()
	{
		DEBUG_DisplayGameInfo();
		if (!m_pause)
		{
			m_timeInState += Time.deltaTime;
			m_timeInStateUnscaled += Time.unscaledDeltaTime;
			m_timeInDecision += Time.deltaTime;
		}
		if (AppState.GetCurrent() != AppState_GameTeardown.Get())
		{
			UpdateTimeRemaining();
			// added in rogues
			//if (InterfaceManager.Get() != null && this.activeOwnedActorData != null && this.ActingTeam != Team.Invalid)
			//{
			//	if (Team.TeamA != this.ActingTeam)
			//	{
			//		if (!this.m_displayedTeamTurnNotice)
			//		{
			//			InterfaceManager.Get().DisplayAlert(this.c_enemyTeamActingString, Color.red, 3f, true, 1);
			//			this.m_displayedTeamTurnNotice = true;
			//		}
			//	}
			//	else if (!this.m_displayedTeamTurnNotice)
			//	{
			//		InterfaceManager.Get().DisplayAlert(this.c_allyTeamActingString, HUD_UIResources.Get().m_allyIndicatorBar, 3f, true, 1);
			//		this.m_displayedTeamTurnNotice = true;
			//	}
			//}
			//if (this.m_markedToSwitchToNextActiveActor)
			//{
			//	this.SetActiveNextNonConfirmedOwnedActorData_FCFS();
			//	this.m_markedToSwitchToNextActiveActor = false;
			//}
		}
	}

	private void UpdateTimeRemaining()
	{
		// rogues
		// if (!GameFlowData.PauseForPvEPrototype() && ...
		if (!m_pause
			&& Get().IsInDecisionState()
			&& m_timeRemainingInDecision >= -m_timeRemainingInDecisionOverflow)
		{
			m_timeRemainingInDecision -= Time.deltaTime;
			if (m_timeRemainingInDecision < -m_timeRemainingInDecisionOverflow)
			{
				m_timeRemainingInDecision = -m_timeRemainingInDecisionOverflow;

				// custom
				if (NetworkServer.active)
				{
					foreach (ActorData actor in GetActors())
					{
						var turnSm = actor.gameObject.GetComponent<ActorTurnSM>();
						turnSm.OnMessage(TurnMessage.BEGIN_RESOLVE);
					}
					ServerActionBuffer.Get().ActionPhase = ActionBufferPhase.Abilities;
					gameState = GameState.BothTeams_Resolve;
				}
			}

			// server-only
#if SERVER
			if (NetworkServer.active)
			{
				bool syncRequired = m_timeForNextTimeRemainingSync < 0f || Time.time >= m_timeForNextTimeRemainingSync;
				if (m_timeRemainingInDecision <= 0f)
				{
					if (PreventAutoLockInOnTimeout())
					{
						ServerGameManager serverGameManager = ServerGameManager.Get();
						for (int i = 0; i < m_actors.Count; i++)
						{
							ActorData actorData = m_actors[i];
							if (actorData != null
								&& actorData.IsHumanControlled()
								&& actorData.GetActorTurnSM() != null
								&& actorData.GetActorTurnSM().AmStillDeciding()
								&& -m_timeRemainingInDecision >= actorData.GetTimeBank().GetPermittedOverflowTime()
								&& serverGameManager.IsAccountReconnecting(actorData.GetAccountId()))
							{
								actorData.GetActorTurnSM().OnMessage(TurnMessage.DONE_BUTTON_CLICKED, true);
							}
						}
					}
					else
					{
						foreach (ActorData actorData in Get().GetActors())
						{
							if (actorData != null
								&& actorData.IsHumanControlled()
								&& actorData.GetActorTurnSM() != null
								&& actorData.GetActorTurnSM().AmStillDeciding()
								&& -m_timeRemainingInDecision >= actorData.GetTimeBank().GetPermittedOverflowTime())
							{
								syncRequired = true;
								actorData.GetActorTurnSM().OnMessage(TurnMessage.DONE_BUTTON_CLICKED, true);
							}
						}
					}
				}
				if (syncRequired)
				{
					m_timeForNextTimeRemainingSync = Time.time + 1f;
					Get().CallRpcUpdateTimeRemaining(m_timeRemainingInDecision);
				}
			}
			bool willEnterTimebankMode = false;
			foreach (ActorData actorData in Get().GetActors())
			{
				if ((actorData != null
						&& actorData.IsHumanControlled()
						&& actorData.GetActorTurnSM() != null
						&& actorData.GetTimeBank().GetConsumableUsed())
					|| (actorData.GetActorTurnSM().AmStillDeciding()
						&& actorData.GetTimeBank().HasTimeSaved()))
				{
					willEnterTimebankMode = true;
				}
			}
			if (willEnterTimebankMode != m_willEnterTimebankMode)
			{
				Networkm_willEnterTimebankMode = willEnterTimebankMode;
			}
#endif
		}
	}

	private void DEBUG_DisplayGameInfo()
	{
		// TODO LOW missing debug code
	}

	public bool IsOwnerTargeting()
	{
		bool result = false;
		ActorData activeOwnedActorData = this.activeOwnedActorData;
		if (activeOwnedActorData != null)
		{
			ActorTurnSM component = activeOwnedActorData.GetComponent<ActorTurnSM>();
			if (component != null && component.CurrentState == TurnStateEnum.TARGETING_ACTION)
			{
				result = true;
			}
		}
		return result;
	}

	// added in rogues
#if SERVER
	public static bool IsDecisionStateEnum(GameState state)
	{
		return state == GameState.BothTeams_Decision;  // rogues || state == GameState.PVE_TeamActions;
	}
#endif

	public bool IsInDecisionState()
	{
		// reactor
		return m_gameState == GameState.BothTeams_Decision;
		// rogues
		//return IsDecisionStateEnum(gameState);
	}

	// reactor
	public bool IsTeamsTurn(Team team)
	{
		bool flag = false;
		if (IsTeamADecision() || IsTeamAResolving())
		{
			flag |= (team == Team.TeamA);
		}
		if (IsTeamBDecision() || IsTeamBResolving())
		{
			flag |= (team == Team.TeamB);
		}
		return flag;
	}

	// rogues
	//public bool IsTeamsTurn(Team team)
	//{
	//	return !this.GetPause() && GameFlowData.Get().ActingTeam == team;
	//}

	public bool IsOwnedActorsTurn()
	{
		return activeOwnedActorData != null && IsTeamsTurn(activeOwnedActorData.GetTeam());
	}

	public bool IsInResolveState()
	{
		return m_gameState == GameState.BothTeams_Resolve;
	}

	// removed in rogues
	public bool IsTeamAResolving()
	{
		return IsInResolveState();
	}

	// removed in rogues
	public bool IsTeamBResolving()
	{
		return IsInResolveState();
	}

	// removed in rogues
	public bool IsTeamADecision()
	{
		return m_gameState == GameState.BothTeams_Decision;
	}

	// removed in rogues
	public bool IsTeamBDecision()
	{
		return m_gameState == GameState.BothTeams_Decision;
	}

	// removed in rogues
	public bool IsPhase(int phase)
	{
		return (phase == 1)
			&& (m_gameState == GameState.BothTeams_Decision
				|| m_gameState == GameState.BothTeams_Resolve);
	}

	// removed in rogues
	public void SetSelectedTeam(int team)
	{
		m_selectedTeam = (Team)team;
	}

	public bool ShouldForceResolveTimeout()
	{
		return m_timeInStateUnscaled > m_resolveTimeoutLimit
			&& (DebugParameters.Get() == null || !DebugParameters.Get().GetParameterAsBool("DisableResolveFailsafe"));
	}

	public bool PlayersMustUseTimeBank()
	{
		return IsInDecisionState() && GetTimeInState() > m_turnTime;
	}

	public bool WillEnterTimebankMode()
	{
		return m_willEnterTimebankMode;
	}

	// reactor
	public bool PreventAutoLockInOnTimeout()
	{
		// TODO for now
		return false;

		GameManager gameManager = GameManager.Get();
		if (gameManager == null)
		{
			return false;
		}
		GameType gameType = gameManager.GameConfig.GameType;
		if (gameType == GameType.Practice)
		{
			return true;
		}
		if (!gameManager.GameplayOverrides.SoloGameNoAutoLockinOnTimeout)
		{
			return false;
		}
		if (gameType != GameType.Solo && gameType != GameType.NewPlayerSolo && gameType != GameType.Tutorial && !gameManager.GameConfig.InstanceSubType.HasMod(GameSubType.SubTypeMods.AntiSocial))
		{
			return false;
		}
		return true;
	}

	// rogues
	//public bool PreventAutoLockInOnTimeout()
	//{
	//	GameManager gameManager = GameManager.Get();
	//	return !(gameManager == null) && gameManager.GameplayOverrides.SoloGameNoAutoLockinOnTimeout;
	//}

	public void ClearCooldowns()
	{
		AbilityData abilityData = null;
		GameFlowData gameFlowData = Get();
		if (gameFlowData)
		{
			ActorData activeOwnedActorData = gameFlowData.activeOwnedActorData;
			if (activeOwnedActorData)
			{
				abilityData = activeOwnedActorData.GetComponent<AbilityData>();
			}
		}
		if (NetworkServer.active)
		{
			if (abilityData)
			{
				abilityData.ClearCooldowns();
			}
		}
		else if (abilityData)
		{
			abilityData.CallCmdClearCooldowns();
		}
	}

	public void RefillStocks()
	{
		AbilityData abilityData = null;
		GameFlowData gameFlowData = Get();
		if (gameFlowData)
		{
			ActorData activeOwnedActorData = gameFlowData.activeOwnedActorData;
			if (activeOwnedActorData)
			{
				abilityData = activeOwnedActorData.GetComponent<AbilityData>();
			}
		}
		if (NetworkServer.active)
		{
			if (abilityData)
			{
				abilityData.RefillStocks();
			}
		}
		else if (abilityData)
		{
			abilityData.CallCmdRefillStocks();
		}
	}

	// removed in rogues
	public void DistributeRewardForKill(ActorData killedActor)
	{
		if (!GameplayUtils.IsPlayerControlled(killedActor) && !GameplayUtils.IsMinion(killedActor))
		{
			return;
		}
		int num;
		bool flag;
		if (GameplayUtils.IsMinion(killedActor))
		{
			num = GameplayData.Get().m_creditsPerMinionKill;
			flag = GameplayData.Get().m_minionBountyCountsParticipation;
		}
		else
		{
			num = GameplayData.Get().m_creditsPerPlayerKill;
			flag = GameplayData.Get().m_playerBountyCountsParticipation;
		}
		if (num > 0)
		{
			if (flag)
			{
				List<ActorData> contributorsToKill = GetContributorsToKill(killedActor, false);
				if (contributorsToKill.Count > 0)
				{
					RewardContributorsToKill(contributorsToKill, num);
				}
				else if (GameplayData.Get().m_participationlessBountiesGoToTeam)
				{
					RewardTeam(killedActor.GetEnemyTeam(), num);
				}
			}
			else
			{
				RewardTeam(killedActor.GetEnemyTeam(), num);
			}
		}
	}

	public int GetTotalDeathsOnTurnStart(Team team)
	{
		int num = 0;
		List<ActorData> allTeamMembers = GetAllTeamMembers(team);
		if (allTeamMembers != null)
		{
			foreach (ActorData actorData in allTeamMembers)
			{
				if (actorData.GetActorBehavior() != null)
				{
					num += actorData.GetActorBehavior().totalDeathsOnTurnStart;
				}
			}
		}
		return num;
	}

	public List<ActorData> GetContributorsToKill(ActorData killedActor, bool onlyDirectDamagers = false)
	{
		List<ActorData> result = new List<ActorData>();
		if (NetworkServer.active)
		{
#if SERVER
			List<ActorData> allTeamMembers = GetAllTeamMembers(killedActor.GetEnemyTeam());
			if (allTeamMembers != null)
			{
				ActorBehavior actorBehavior = killedActor.GetActorBehavior();
				int currentTurn = Get().CurrentTurn;
				int num = currentTurn - GameWideData.Get().m_killAssistMemory;
				foreach (ActorData actorData in allTeamMembers)
				{
					if (GameplayUtils.IsPlayerControlled(actorData))
					{
						for (int i = currentTurn; i >= num; i--)
						{
							ActorBehavior.TurnBehavior behaviorOfTurn = actorBehavior.GetBehaviorOfTurn(i);
							if (behaviorOfTurn != null && (behaviorOfTurn.DamagedByActor(actorData) || behaviorOfTurn.DebuffedByActor(actorData)))
							{
								result.Add(actorData);
								break;
							}
						}
					}
				}
				if (!onlyDirectDamagers)
				{
					List<ActorData> list2 = new List<ActorData>();
					foreach (ActorData actorData2 in allTeamMembers)
					{
						if (!result.Contains(actorData2) && GameplayUtils.IsPlayerControlled(actorData2))
						{
							foreach (ActorData actorData3 in result)
							{
								if (list2.Contains(actorData2))
								{
									break;
								}
								ActorBehavior actorBehavior2 = actorData3.GetActorBehavior();
								for (int j = currentTurn; j >= num; j--)
								{
									ActorBehavior.TurnBehavior behaviorOfTurn2 = actorBehavior2.GetBehaviorOfTurn(j);
									if (behaviorOfTurn2 != null && (behaviorOfTurn2.HealedByActor(actorData2) || behaviorOfTurn2.BuffedByActor(actorData2)))
									{
										list2.Add(actorData2);
										break;
									}
								}
							}
						}
					}
					foreach (ActorData item in list2)
					{
						result.Add(item);
					}
				}
			}
#endif
		}
		return result;
	}

	public List<ActorData> GetContributorsToKillOnClient(ActorData killedActor, bool onlyDirectDamagers = false)
	{
		List<ActorData> result = new List<ActorData>();
		List<ActorData> allTeamMembers = GetAllTeamMembers(killedActor.GetEnemyTeam());
		if (allTeamMembers == null)
		{
			return result;
		}
		ActorBehavior killedActorBehavior = killedActor.GetActorBehavior();
		foreach (ActorData actorData in allTeamMembers)
		{
			if (GameplayUtils.IsPlayerControlled(actorData)
				&& killedActorBehavior.Client_ActorDamagedOrDebuffedByActor(actorData))
			{
				result.Add(actorData);
				break;
			}
		}
		if (!onlyDirectDamagers)
		{
			List<ActorData> supports = new List<ActorData>();
			foreach (ActorData actorData in allTeamMembers)
			{
				if (!result.Contains(actorData) && GameplayUtils.IsPlayerControlled(actorData))
				{
					foreach (ActorData killer in result)
					{
						if (supports.Contains(actorData))
						{
							break;
						}
						if (killer.GetActorBehavior().Client_ActorHealedOrBuffedByActor(actorData))
						{
							supports.Add(actorData);
							break;
						}
					}
				}
			}
			foreach (ActorData support in supports)
			{
				result.Add(support);
			}
		}
		return result;
	}

	// removed in rogues
	public void RewardContributorsToKill(List<ActorData> participants, int baseCreditsReward)
	{
		if (participants.Count > 0)
		{
			int num = baseCreditsReward / participants.Count;
			int num2 = Mathf.FloorToInt(GameplayData.Get().m_creditBonusFractionPerExtraPlayer * (participants.Count - 1) * num);
			int numCredits = num + num2;
			foreach (ActorData actorData in participants)
			{
				ItemData component = actorData.GetComponent<ItemData>();
				if (component != null)
				{
					component.GiveCredits(numCredits);
				}
			}
		}
	}

	// removed in rogues
	public void RewardTeam(Team teamToReward, int creditsReward)
	{
		List<ActorData> allTeamMembers = GetAllTeamMembers(teamToReward);
		if (allTeamMembers != null)
		{
			foreach (ActorData actorData in allTeamMembers)
			{
				ItemData component = actorData.GetComponent<ItemData>();
				if (component != null)
				{
					component.GiveCredits(creditsReward);
				}
			}
		}
	}

	public int GetDeathCountOfTeam(Team team)
	{
		int num = 0;
		for (int i = 0; i < m_actors.Count; i++)
		{
			ActorData actorData = m_actors[i];
			if (actorData != null
				&& actorData.GetTeam() == team
				&& actorData.GetActorBehavior() != null)
			{
				num += actorData.GetActorBehavior().totalDeaths;
			}
		}
		return num;
	}

	public int GetTotalTeamDamageReceived(Team team)
	{
		int num = 0;
		for (int i = 0; i < m_actors.Count; i++)
		{
			ActorData actorData = m_actors[i];
			if (actorData != null
				&& actorData.GetTeam() == team
				&& actorData.GetActorBehavior() != null)
			{
				num += actorData.GetActorBehavior().totalPlayerDamageReceived;
			}
		}
		return num;
	}

	public void UpdateCoverFromBarriersForAllActors()
	{
		for (int i = 0; i < m_actors.Count; i++)
		{
			ActorData actorData = m_actors[i];
			if (actorData.GetActorCover() != null)
			{
				actorData.GetActorCover().UpdateCoverFromBarriers();
			}
		}
	}

	public static void SetDebugParamOnServer(string name, bool value)
	{
	}

	public void LogTurnBehaviorsFromTurnsAgo(int numTurnsAgo)
	{
		if (NetworkServer.active)
		{
#if SERVER
			List<ActorData> actors = GetActors();
			string text = "";
			bool flag = false;
			foreach (ActorData actorData in actors)
			{
				if (actorData != null && actorData.GetActorBehavior() != null)
				{
					int currentTurn = Get().CurrentTurn;
					ActorBehavior.TurnBehavior behaviorOfTurn = actorData.GetActorBehavior().GetBehaviorOfTurn(currentTurn - numTurnsAgo);
					if (behaviorOfTurn != null)
					{
						flag = true;
						text = string.Concat(new string[]
						{
							text,
							actorData.DebugNameString(),
							" on Team[",
							actorData.GetTeam().ToString(),
							"]: \n",
							behaviorOfTurn.ToString(),
							"\n"
						});
					}
					else
					{
						flag = true;
						text = string.Concat(new object[]
						{
							text,
							"Did not find TurnBehavior from ",
							numTurnsAgo,
							" turns ago for ",
							actorData.DebugNameString(),
							"\n\n"
						});
					}
				}
			}
			if (flag)
			{
				Debug.LogWarning(text);
			}
#endif
		}
	}

	public void SetLocalPlayerData()
	{
		m_localPlayerData = null;
		if (GameFlow.Get() != null)
		{
			foreach (GameObject gameObject in m_players)
			{
				if (gameObject != null)
				{
					PlayerData component = gameObject.GetComponent<PlayerData>();
					if (component != null
						&& GameFlow.Get().playerDetails.TryGetValue(component.GetPlayer(), out PlayerDetails playerDetails)
						&& playerDetails.IsLocal())
					{
						m_localPlayerData = component;
						break;
					}
				}
			}
		}
	}

	// reactor
	private void UNetVersion()
	{
	}
	// rogues
	//private void MirrorProcessed()
	//{
	//}

	public bool Networkm_pause
	{
		get
		{
			return m_pause;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_pause, 1U);
		}
	}

	public bool Networkm_pausedForDebugging
	{
		get
		{
			return m_pausedForDebugging;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_pausedForDebugging, 2U);
		}
	}

	public bool Networkm_pausedByPlayerRequest
	{
		get
		{
			return m_pausedByPlayerRequest;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_pausedByPlayerRequest, 4U);
		}
	}

	public bool Networkm_pausedForSinglePlayer
	{
		get
		{
			return m_pausedForSinglePlayer;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_pausedForSinglePlayer, 8U);
		}
	}

	public ResolutionPauseState Networkm_resolutionPauseState
	{
		get
		{
			return m_resolutionPauseState;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_resolutionPauseState, 0x10U);
		}
	}

	public float Networkm_startTime
	{
		get
		{
			return m_startTime;
		}
		[param: In]
		set
		{
			if (NetworkServer.localClientActive && !syncVarHookGuard)
			{
				syncVarHookGuard = true;
				HookSetStartTime(value);
				syncVarHookGuard = false;
			}
			SetSyncVar(value, ref m_startTime, 0x20U);
		}
	}

	public float Networkm_deploymentTime
	{
		get
		{
			return m_deploymentTime;
		}
		[param: In]
		set
		{
			if (NetworkServer.localClientActive && !syncVarHookGuard)
			{
				syncVarHookGuard = true;
				HookSetDeploymentTime(value);
				syncVarHookGuard = false;
			}
			SetSyncVar(value, ref m_deploymentTime, 0x40U);
		}
	}

	public float Networkm_turnTime
	{
		get
		{
			return m_turnTime;
		}
		[param: In]
		set
		{
			if (NetworkServer.localClientActive && !syncVarHookGuard)
			{
				syncVarHookGuard = true;
				HookSetTurnTime(value);
				syncVarHookGuard = false;
			}
			SetSyncVar(value, ref m_turnTime, 0x80U);
		}
	}

	public float Networkm_maxTurnTime
	{
		get
		{
			return m_maxTurnTime;
		}
		[param: In]
		set
		{
			if (NetworkServer.localClientActive && !syncVarHookGuard)
			{
				syncVarHookGuard = true;
				HookSetMaxTurnTime(value);
				syncVarHookGuard = false;
			}
			SetSyncVar(value, ref m_maxTurnTime, 0x100U);
		}
	}

	// rogues
	//public int Networkm_reviveTokens
	//{
	//	get
	//	{
	//		return this.m_reviveTokens;
	//	}
	//	[param: In]
	//	set
	//	{
	//		base.SetSyncVar<int>(value, ref this.m_reviveTokens, 512UL);
	//	}
	//}

	public float Networkm_timeRemainingInDecisionOverflow
	{
		get
		{
			return m_timeRemainingInDecisionOverflow;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_timeRemainingInDecisionOverflow, 0x200U);  // 0x400U in rogues
		}
	}

	public bool Networkm_willEnterTimebankMode
	{
		get
		{
			return m_willEnterTimebankMode;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_willEnterTimebankMode, 0x400U);  // 0x800U in rogues
		}
	}

	public int Networkm_currentTurn
	{
		get
		{
			return m_currentTurn;
		}
		[param: In]
		set
		{
			if (NetworkServer.localClientActive && !syncVarHookGuard)
			{
				syncVarHookGuard = true;
				HookSetCurrentTurn(value);
				syncVarHookGuard = false;
			}
			SetSyncVar(value, ref m_currentTurn, 0x800U);  // 0x1000U in rogues
		}
	}

	public GameState Networkm_gameState
	{
		get
		{
			return m_gameState;
		}
		[param: In]
		set
		{
			if (NetworkServer.localClientActive && !syncVarHookGuard)
			{
				syncVarHookGuard = true;
				HookSetGameState(value);
				syncVarHookGuard = false;
			}
			SetSyncVar(value, ref m_gameState, 0x1000U);  // 0x2000U in rogues
		}
	}

	// rogues
	//public PlayerActionStateMachine.StateFlag Networkm_actionsFsmState
	//{
	//	get
	//	{
	//		return this.m_actionsFsmState;
	//	}
	//	[param: In]
	//	set
	//	{
	//		if (NetworkServer.localClientActive && !base.syncVarHookGuard)
	//		{
	//			base.syncVarHookGuard = true;
	//			this.HookSetPlayerActionState(value);
	//			base.syncVarHookGuard = false;
	//		}
	//		base.SetSyncVar<PlayerActionStateMachine.StateFlag>(value, ref this.m_actionsFsmState, 16384UL);
	//	}
	//}

	// rogues
	//public Team Networkm_actingTeam
	//{
	//	get
	//	{
	//		return this.m_actingTeam;
	//	}
	//	[param: In]
	//	set
	//	{
	//		base.SetSyncVar<Team>(value, ref this.m_actingTeam, 32768UL);
	//	}
	//}

	// rogues
	//public bool Networkm_isInCombat
	//{
	//	get
	//	{
	//		return this.m_isInCombat;
	//	}
	//	[param: In]
	//	set
	//	{
	//		base.SetSyncVar<bool>(value, ref this.m_isInCombat, 65536UL);
	//	}
	//}

	// rogues
	//protected static void InvokeRpcRpcSetActingTeam(NetworkBehaviour obj, NetworkReader reader)
	//{
	//	if (!NetworkClient.active)
	//	{
	//		Debug.LogError("RPC RpcSetActingTeam called on server.");
	//		return;
	//	}
	//	((GameFlowData)obj).RpcSetActingTeam(reader.ReadPackedInt32());
	//}

	protected static void InvokeRpcRpcUpdateTimeRemaining(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcUpdateTimeRemaining called on server.");
			return;
		}
		((GameFlowData)obj).RpcUpdateTimeRemaining(reader.ReadSingle());
	}

	// rogues
	//public void CallRpcSetActingTeam(int teamInt)
	//{
	//	NetworkWriter networkWriter = new NetworkWriter();
	//	networkWriter.WritePackedInt32(teamInt);
	//	this.SendRPCInternal(typeof(GameFlowData), "RpcSetActingTeam", networkWriter, 0);
	//}

	// reactor
	public void CallRpcUpdateTimeRemaining(float timeRemaining)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("RPC Function RpcUpdateTimeRemaining called on client.");
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)2);
		networkWriter.WritePackedUInt32((uint)kRpcRpcUpdateTimeRemaining);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		networkWriter.Write(timeRemaining);
		SendRPCInternal(networkWriter, 0, "RpcUpdateTimeRemaining");
	}

	// rogues
	//public void CallRpcUpdateTimeRemaining(float timeRemaining)
	//{
	//	NetworkWriter networkWriter = new NetworkWriter();
	//	networkWriter.Write(timeRemaining);
	//	this.SendRPCInternal(typeof(GameFlowData), "RpcUpdateTimeRemaining", networkWriter, 0);
	//}

	// reactor
	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		if (forceAll)
		{
			writer.Write(m_pause);
			writer.Write(m_pausedForDebugging);
			writer.Write(m_pausedByPlayerRequest);
			writer.Write(m_pausedForSinglePlayer);
			writer.Write((int)m_resolutionPauseState);
			writer.Write(m_startTime);
			writer.Write(m_deploymentTime);
			writer.Write(m_turnTime);
			writer.Write(m_maxTurnTime);
			writer.Write(m_timeRemainingInDecisionOverflow);
			writer.Write(m_willEnterTimebankMode);
			writer.WritePackedUInt32((uint)m_currentTurn);
			writer.Write((int)m_gameState);
			return true;
		}
		bool flag = false;
		if ((syncVarDirtyBits & 1U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			writer.Write(m_pause);
		}
		if ((syncVarDirtyBits & 2U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			writer.Write(m_pausedForDebugging);
		}
		if ((syncVarDirtyBits & 4U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			writer.Write(m_pausedByPlayerRequest);
		}
		if ((syncVarDirtyBits & 8U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			writer.Write(m_pausedForSinglePlayer);
		}
		if ((syncVarDirtyBits & 0x10U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			writer.Write((int)m_resolutionPauseState);
		}
		if ((syncVarDirtyBits & 0x20U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			writer.Write(m_startTime);
		}
		if ((syncVarDirtyBits & 0x40U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			writer.Write(m_deploymentTime);
		}
		if ((syncVarDirtyBits & 0x80U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			writer.Write(m_turnTime);
		}
		if ((syncVarDirtyBits & 0x100U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			writer.Write(m_maxTurnTime);
		}
		if ((syncVarDirtyBits & 0x200U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			writer.Write(m_timeRemainingInDecisionOverflow);
		}
		if ((syncVarDirtyBits & 0x400U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			writer.Write(m_willEnterTimebankMode);
		}
		if ((syncVarDirtyBits & 0x800U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_currentTurn);
		}
		if ((syncVarDirtyBits & 0x1000U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			writer.Write((int)m_gameState);
		}
		if (!flag)
		{
			writer.WritePackedUInt32(syncVarDirtyBits);
		}
		return flag;
	}

	// reactor
	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
		if (initialState)
		{
			m_pause = reader.ReadBoolean();
			m_pausedForDebugging = reader.ReadBoolean();
			m_pausedByPlayerRequest = reader.ReadBoolean();
			m_pausedForSinglePlayer = reader.ReadBoolean();
			m_resolutionPauseState = (ResolutionPauseState)reader.ReadInt32();
			m_startTime = reader.ReadSingle();
			m_deploymentTime = reader.ReadSingle();
			m_turnTime = reader.ReadSingle();
			m_maxTurnTime = reader.ReadSingle();
			m_timeRemainingInDecisionOverflow = reader.ReadSingle();
			m_willEnterTimebankMode = reader.ReadBoolean();
			m_currentTurn = (int)reader.ReadPackedUInt32();
			m_gameState = (GameState)reader.ReadInt32();
			return;
		}
		int num = (int)reader.ReadPackedUInt32();
		if ((num & 1) != 0)
		{
			m_pause = reader.ReadBoolean();
		}
		if ((num & 2) != 0)
		{
			m_pausedForDebugging = reader.ReadBoolean();
		}
		if ((num & 4) != 0)
		{
			m_pausedByPlayerRequest = reader.ReadBoolean();
		}
		if ((num & 8) != 0)
		{
			m_pausedForSinglePlayer = reader.ReadBoolean();
		}
		if ((num & 0x10) != 0)
		{
			m_resolutionPauseState = (ResolutionPauseState)reader.ReadInt32();
		}
		if ((num & 0x20) != 0)
		{
			HookSetStartTime(reader.ReadSingle());
		}
		if ((num & 0x40) != 0)
		{
			HookSetDeploymentTime(reader.ReadSingle());
		}
		if ((num & 0x80) != 0)
		{
			HookSetTurnTime(reader.ReadSingle());
		}
		if ((num & 0x100) != 0)
		{
			HookSetMaxTurnTime(reader.ReadSingle());
		}
		if ((num & 0x200) != 0)
		{
			m_timeRemainingInDecisionOverflow = reader.ReadSingle();
		}
		if ((num & 0x400) != 0)
		{
			m_willEnterTimebankMode = reader.ReadBoolean();
		}
		if ((num & 0x800) != 0)
		{
			HookSetCurrentTurn((int)reader.ReadPackedUInt32());
		}
		if ((num & 0x1000) != 0)
		{
			HookSetGameState((GameState)reader.ReadInt32());
		}
	}
}

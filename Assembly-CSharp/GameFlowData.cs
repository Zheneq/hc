using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;

public class GameFlowData : NetworkBehaviour, IGameEventListener
{
	private static GameFlowData s_gameFlowData;

	public static float s_loadingScreenTime;

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

	public float m_resolveTimeoutLimit = 112f;

	private float m_matchStartTime;

	private float m_deploymentStartTime;

	private float m_timeRemainingInDecision = 20f;

	[SyncVar]
	private float m_timeRemainingInDecisionOverflow;

	[SyncVar]
	private bool m_willEnterTimebankMode;

	private const float c_timeRemainingUpdateInterval = 1f;

	private const float c_latencyCorrectionTime = 1f;

	private float m_timeInState;

	private float m_timeInStateUnscaled;

	private float m_timeInDecision;

	[SyncVar(hook = "HookSetCurrentTurn")]
	private int m_currentTurn;

	[SyncVar(hook = "HookSetGameState")]
	private GameState m_gameState;

	private static int kRpcRpcUpdateTimeRemaining;

	public bool Started
	{
		get;
		private set;
	}

	public PlayerData LocalPlayerData => m_localPlayerData;

	public ActorData nextOwnedActorData
	{
		get
		{
			ActorData result = null;
			if (m_ownedActorDatas.Count > 0)
			{
				if (activeOwnedActorData == null)
				{
					result = m_ownedActorDatas[0];
				}
				else
				{
					int num = 0;
					int num2 = 0;
					while (true)
					{
						if (num2 < m_ownedActorDatas.Count)
						{
							if (m_ownedActorDatas[num2] == activeOwnedActorData)
							{
								num = num2;
								break;
							}
							num2++;
							continue;
						}
						break;
					}
					result = m_ownedActorDatas[(num + 1) % m_ownedActorDatas.Count];
				}
			}
			return result;
		}
	}

	public ActorData firstOwnedFriendlyActorData
	{
		get
		{
			ActorData result = null;
			if (m_ownedActorDatas.Count > 0 && activeOwnedActorData != null)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
						{
							foreach (ActorData ownedActorData in m_ownedActorDatas)
							{
								if (ownedActorData.GetTeam() == activeOwnedActorData.GetTeam())
								{
									while (true)
									{
										switch (7)
										{
										case 0:
											break;
										default:
											return ownedActorData;
										}
									}
								}
							}
							return result;
						}
					}
				}
			}
			return result;
		}
	}

	public ActorData firstOwnedEnemyActorData
	{
		get
		{
			ActorData result = null;
			if (activeOwnedActorData != null)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
					{
						using (List<ActorData>.Enumerator enumerator = m_ownedActorDatas.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								ActorData current = enumerator.Current;
								if (current.GetTeam() != activeOwnedActorData.GetTeam())
								{
									return current;
								}
							}
							while (true)
							{
								switch (3)
								{
								case 0:
									break;
								default:
									return result;
								}
							}
						}
					}
					}
				}
			}
			return result;
		}
	}

	internal ActorData POVActorData => activeOwnedActorData;

	public ActorData activeOwnedActorData
	{
		get
		{
			return m_activeOwnedActorData;
		}
		set
		{
			bool flag = m_activeOwnedActorData != value;
			bool flag2 = false;
			if (m_activeOwnedActorData != null)
			{
				m_activeOwnedActorData.OnDeselect();
				int num;
				if (value != null)
				{
					num = ((value.GetOpposingTeam() == m_activeOwnedActorData.GetTeam()) ? 1 : 0);
				}
				else
				{
					num = 0;
				}
				flag2 = ((byte)num != 0);
			}
			m_activeOwnedActorData = value;
			if (m_activeOwnedActorData != null)
			{
				m_activeOwnedActorData.OnSelect();
			}
			if (flag)
			{
				if (GameFlowData.s_onActiveOwnedActorChangeHolder != null)
				{
					GameFlowData.s_onActiveOwnedActorChangeHolder(value);
				}
			}
			if (!flag2)
			{
				return;
			}
			while (true)
			{
				GameEventManager.Get().FireEvent(GameEventManager.EventType.ActiveControlChangedToEnemyTeam, null);
				return;
			}
		}
	}

	public int CurrentTurn => m_currentTurn;

	internal GameState gameState
	{
		get
		{
			return m_gameState;
		}
		set
		{
			if (m_gameState == value)
			{
				return;
			}
			while (true)
			{
				SetGameState(value);
				return;
			}
		}
	}

	public bool Networkm_pause
	{
		get
		{
			return m_pause;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_pause, 1u);
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
			SetSyncVar(value, ref m_pausedForDebugging, 2u);
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
			SetSyncVar(value, ref m_pausedByPlayerRequest, 4u);
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
			SetSyncVar(value, ref m_pausedForSinglePlayer, 8u);
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
			SetSyncVar(value, ref m_resolutionPauseState, 16u);
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
			ref float startTime = ref m_startTime;
			if (NetworkServer.localClientActive)
			{
				if (!base.syncVarHookGuard)
				{
					base.syncVarHookGuard = true;
					HookSetStartTime(value);
					base.syncVarHookGuard = false;
				}
			}
			SetSyncVar(value, ref startTime, 32u);
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
			ref float deploymentTime = ref m_deploymentTime;
			if (NetworkServer.localClientActive)
			{
				if (!base.syncVarHookGuard)
				{
					base.syncVarHookGuard = true;
					HookSetDeploymentTime(value);
					base.syncVarHookGuard = false;
				}
			}
			SetSyncVar(value, ref deploymentTime, 64u);
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
			ref float turnTime = ref m_turnTime;
			if (NetworkServer.localClientActive)
			{
				if (!base.syncVarHookGuard)
				{
					base.syncVarHookGuard = true;
					HookSetTurnTime(value);
					base.syncVarHookGuard = false;
				}
			}
			SetSyncVar(value, ref turnTime, 128u);
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
			ref float maxTurnTime = ref m_maxTurnTime;
			if (NetworkServer.localClientActive)
			{
				if (!base.syncVarHookGuard)
				{
					base.syncVarHookGuard = true;
					HookSetMaxTurnTime(value);
					base.syncVarHookGuard = false;
				}
			}
			SetSyncVar(value, ref maxTurnTime, 256u);
		}
	}

	public float Networkm_timeRemainingInDecisionOverflow
	{
		get
		{
			return m_timeRemainingInDecisionOverflow;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_timeRemainingInDecisionOverflow, 512u);
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
			SetSyncVar(value, ref m_willEnterTimebankMode, 1024u);
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
			ref int currentTurn = ref m_currentTurn;
			if (NetworkServer.localClientActive)
			{
				if (!base.syncVarHookGuard)
				{
					base.syncVarHookGuard = true;
					HookSetCurrentTurn(value);
					base.syncVarHookGuard = false;
				}
			}
			SetSyncVar(value, ref currentTurn, 2048u);
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
			ref GameState gameState = ref m_gameState;
			if (NetworkServer.localClientActive)
			{
				if (!base.syncVarHookGuard)
				{
					base.syncVarHookGuard = true;
					HookSetGameState(value);
					base.syncVarHookGuard = false;
				}
			}
			SetSyncVar(value, ref gameState, 4096u);
		}
	}

	private static Action<ActorData> s_onAddActorHolder;
	internal static event Action<ActorData> s_onAddActor
	{
		add
		{
			Action<ActorData> action = GameFlowData.s_onAddActorHolder;
			Action<ActorData> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref GameFlowData.s_onAddActorHolder, (Action<ActorData>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
		remove
		{
			Action<ActorData> action = GameFlowData.s_onAddActorHolder;
			Action<ActorData> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref GameFlowData.s_onAddActorHolder, (Action<ActorData>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
		}
	}

	private static Action<ActorData> s_onRemoveActorHolder;
	internal static event Action<ActorData> s_onRemoveActor
	{
		add
		{
			Action<ActorData> action = GameFlowData.s_onRemoveActorHolder;
			Action<ActorData> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref GameFlowData.s_onRemoveActorHolder, (Action<ActorData>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
		remove
		{
			Action<ActorData> action = GameFlowData.s_onRemoveActorHolder;
			Action<ActorData> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref GameFlowData.s_onRemoveActorHolder, (Action<ActorData>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
	}

	private static Action<ActorData> s_onActiveOwnedActorChangeHolder;
	internal static event Action<ActorData> s_onActiveOwnedActorChange
	{
		add
		{
			Action<ActorData> action = GameFlowData.s_onActiveOwnedActorChangeHolder;
			Action<ActorData> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref GameFlowData.s_onActiveOwnedActorChangeHolder, (Action<ActorData>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
		remove
		{
			Action<ActorData> action = GameFlowData.s_onActiveOwnedActorChangeHolder;
			Action<ActorData> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref GameFlowData.s_onActiveOwnedActorChangeHolder, (Action<ActorData>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
	}

	private static Action<GameState> s_onGameStateChangedHolder;
	internal static event Action<GameState> s_onGameStateChanged
	{
		add
		{
			Action<GameState> action = GameFlowData.s_onGameStateChangedHolder;
			Action<GameState> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref GameFlowData.s_onGameStateChangedHolder, (Action<GameState>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
		remove
		{
			Action<GameState> action = GameFlowData.s_onGameStateChangedHolder;
			Action<GameState> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref GameFlowData.s_onGameStateChangedHolder, (Action<GameState>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
	}

	static GameFlowData()
	{
		GameFlowData.s_onAddActorHolder = delegate
		{
		};
		GameFlowData.s_onRemoveActorHolder = delegate
		{
		};
		GameFlowData.s_onActiveOwnedActorChangeHolder = delegate
		{
		};
		GameFlowData.s_onGameStateChangedHolder = delegate
		{
		};
		s_loadingScreenTime = 4f;
		kRpcRpcUpdateTimeRemaining = 939569152;
		NetworkBehaviour.RegisterRpcDelegate(typeof(GameFlowData), kRpcRpcUpdateTimeRemaining, InvokeRpcRpcUpdateTimeRemaining);
		NetworkCRC.RegisterBehaviour("GameFlowData", 0);
	}

	private void Awake()
	{
		s_gameFlowData = this;
		if (ClientGamePrefabInstantiator.Get() != null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					ClientGamePrefabInstantiator.Get().InstantiatePrefabs();
					return;
				}
			}
		}
		Log.Error("ClientGamePrefabInstantiator reference not set on game start");
	}

	private void Start()
	{
		Started = true;
		GameEventManager.Get().FireEvent(GameEventManager.EventType.GameFlowDataStarted, null);
		VisualsLoader.FireSceneLoadedEventIfNoVisualLoader();
		ClientGameManager.Get().DesignSceneStarted = true;
		if (ClientGameManager.Get().PlayerObjectStartedOnClient)
		{
			if (AppState.GetCurrent() == AppState_InGameStarting.Get())
			{
				UIScreenManager.Get().TryLoadAndSetupInGameUI();
			}
		}
		ClientGameManager.Get().CheckAndSendClientPreparedForGameStartNotification();
		Log.Info("GameFlowData.Start");
		GameEventManager.Get().AddListener(this, GameEventManager.EventType.GameTeardown);
		GameEventManager.Get().AddListener(this, GameEventManager.EventType.TheatricsAbilityAnimationStart);
		GameEventManager.Get().AddListener(this, GameEventManager.EventType.ServerActionBufferPhaseStart);
		GameEventManager.Get().AddListener(this, GameEventManager.EventType.ServerActionBufferActionsDone);
	}

	public void OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		if (eventType != GameEventManager.EventType.GameTeardown)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		UnityEngine.Object.Destroy(m_actorRoot);
		UnityEngine.Object.Destroy(m_thinCoverRoot);
		UnityEngine.Object.Destroy(m_brushRegionBorderRoot);
		CharacterResourceLink.DestroyAudioResources();
	}

	public override void OnStartServer()
	{
		LobbyGameConfig gameConfig = GameManager.Get().GameConfig;
		Networkm_gameState = GameState.Launched;
		Networkm_turnTime = Convert.ToSingle(gameConfig.TurnTime);
		Networkm_maxTurnTime = Convert.ToSingle(gameConfig.TurnTime) + Mathf.Max(GameWideData.Get().m_tbInitial, GameWideData.Get().m_tbRechargeCap) + GameWideData.Get().m_tbConsumableDuration + 1f;
		m_resolveTimeoutLimit = Convert.ToSingle(gameConfig.ResolveTimeoutLimit);
	}

	public override void OnStartClient()
	{
		if (CurrentTurn <= 0)
		{
			return;
		}
		while (true)
		{
			NotifyOnTurnTick();
			return;
		}
	}

	private void OnDestroy()
	{
		GameFlowData.s_onActiveOwnedActorChangeHolder = null;
		GameFlowData.s_onAddActorHolder = null;
		GameFlowData.s_onRemoveActorHolder = null;
		GameFlowData.s_onGameStateChangedHolder = null;
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

	public bool GetPauseForDialog()
	{
		return m_pausedForDialog;
	}

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
		if (!NetworkServer.active)
		{
			return;
		}
		while (true)
		{
			if (m_pausedForDialog != pause)
			{
				while (true)
				{
					m_pausedForDialog = pause;
					UpdatePause();
					return;
				}
			}
			return;
		}
	}

	internal void SetPausedForSinglePlayer(bool pause)
	{
		if (!NetworkServer.active)
		{
			return;
		}
		while (true)
		{
			if (m_pausedForSinglePlayer != pause)
			{
				while (true)
				{
					Networkm_pausedForSinglePlayer = pause;
					UpdatePause();
					return;
				}
			}
			return;
		}
	}

	internal void SetPausedForDebugging(bool pause)
	{
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
		if (!NetworkServer.active)
		{
			return;
		}
		while (true)
		{
			int num;
			if (!m_pausedForDialog)
			{
				if (!m_pausedForSinglePlayer)
				{
					if (!m_pausedForDebugging)
					{
						num = (m_pausedByPlayerRequest ? 1 : 0);
						goto IL_005b;
					}
				}
			}
			num = 1;
			goto IL_005b;
			IL_005b:
			bool flag = (byte)num != 0;
			if (m_pause != flag)
			{
				Networkm_pause = flag;
			}
			return;
		}
	}

	internal bool IsResolutionPaused()
	{
		return m_resolutionPauseState == ResolutionPauseState.PausedUntilInput;
	}

	internal bool GetResolutionSingleStepping()
	{
		int result;
		if (m_resolutionPauseState != ResolutionPauseState.PausedUntilInput)
		{
			result = ((m_resolutionPauseState == ResolutionPauseState.UnpausedUntilNextAbilityOrPhase) ? 1 : 0);
		}
		else
		{
			result = 1;
		}
		return (byte)result != 0;
	}

	internal void SetResolutionSingleStepping(bool singleStepping)
	{
		if (NetworkServer.active)
		{
			HandleSetResolutionSingleStepping(singleStepping);
		}
		else
		{
			if (!(Get() != null))
			{
				return;
			}
			while (true)
			{
				if (Get().activeOwnedActorData != null)
				{
					while (true)
					{
						Get().activeOwnedActorData.CallCmdSetResolutionSingleStepping(singleStepping);
						return;
					}
				}
				return;
			}
		}
	}

	internal void SetResolutionSingleSteppingAdvance()
	{
		if (NetworkServer.active)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					HandleSetResolutionSingleSteppingAdvance();
					return;
				}
			}
		}
		if (!(Get() != null))
		{
			return;
		}
		while (true)
		{
			if (Get().activeOwnedActorData != null)
			{
				while (true)
				{
					Get().activeOwnedActorData.CallCmdSetResolutionSingleSteppingAdvance();
					return;
				}
			}
			return;
		}
	}

	[Server]
	private void HandleSetResolutionSingleStepping(bool singleStepping)
	{
		if (NetworkServer.active)
		{
			return;
		}
		while (true)
		{
			Debug.LogWarning("[Server] function 'System.Void GameFlowData::HandleSetResolutionSingleStepping(System.Boolean)' called on client");
			return;
		}
	}

	[Server]
	private void HandleSetResolutionSingleSteppingAdvance()
	{
		if (NetworkServer.active)
		{
			return;
		}
		while (true)
		{
			Debug.LogWarning("[Server] function 'System.Void GameFlowData::HandleSetResolutionSingleSteppingAdvance()' called on client");
			return;
		}
	}

	public static GameFlowData Get()
	{
		return s_gameFlowData;
	}

	public static GameObject FindParentBelowRoot(GameObject child)
	{
		object obj;
		if (child.transform.parent == null)
		{
			obj = null;
		}
		else
		{
			obj = child;
		}
		GameObject result = (GameObject)obj;
		GameObject gameObject = child;
		while (gameObject.transform.parent != null)
		{
			result = gameObject;
			gameObject = gameObject.transform.parent.gameObject;
		}
		return result;
	}

	public GameObject GetActorRoot()
	{
		if (m_actorRoot == null)
		{
			m_actorRoot = new GameObject("ActorRoot");
			UnityEngine.Object.DontDestroyOnLoad(m_actorRoot);
		}
		return m_actorRoot;
	}

	public GameObject GetThinCoverRoot()
	{
		if (m_thinCoverRoot == null)
		{
			m_thinCoverRoot = new GameObject("ThinCoverRoot");
			UnityEngine.Object.DontDestroyOnLoad(m_thinCoverRoot);
		}
		return m_thinCoverRoot;
	}

	public GameObject GetBrushBordersRoot()
	{
		if (m_brushRegionBorderRoot == null)
		{
			m_brushRegionBorderRoot = new GameObject("BrushRegionBorderRoot");
			UnityEngine.Object.DontDestroyOnLoad(m_brushRegionBorderRoot);
		}
		return m_brushRegionBorderRoot;
	}

	public Team GetSelectedTeam()
	{
		return m_selectedTeam;
	}

	private void HookSetGameState(GameState state)
	{
		if (m_gameState == state)
		{
			return;
		}
		while (true)
		{
			if (!NetworkServer.active)
			{
				while (true)
				{
					gameState = state;
					return;
				}
			}
			return;
		}
	}

	private void HookSetStartTime(float startTime)
	{
		if (m_startTime == startTime)
		{
			return;
		}
		while (true)
		{
			Networkm_startTime = startTime;
			return;
		}
	}

	private void HookSetDeploymentTime(float deploymentTime)
	{
		if (m_deploymentTime == deploymentTime)
		{
			return;
		}
		while (true)
		{
			Networkm_deploymentTime = deploymentTime;
			return;
		}
	}

	private void HookSetTurnTime(float turnTime)
	{
		if (m_turnTime == turnTime)
		{
			return;
		}
		while (true)
		{
			Networkm_turnTime = turnTime;
			return;
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
		if (NetworkServer.active)
		{
			return;
		}
		while (true)
		{
			while (m_currentTurn < turn)
			{
				IncrementTurn();
			}
			while (true)
			{
				Networkm_currentTurn = turn;
				HUD_UI.Get().m_mainScreenPanel.m_notificationPanel.NotifyTurnCountSet();
				return;
			}
		}
	}

	public int GetClassIndexFromName(string className)
	{
		int result = -1;
		for (int i = 0; i < m_availableCharacterResourceLinkPrefabs.Length; i++)
		{
			GameObject gameObject = m_availableCharacterResourceLinkPrefabs[i];
			object obj;
			if (gameObject == null)
			{
				obj = null;
			}
			else
			{
				obj = gameObject.GetComponent<CharacterResourceLink>();
			}
			CharacterResourceLink characterResourceLink = (CharacterResourceLink)obj;
			if (characterResourceLink != null)
			{
				if (characterResourceLink.m_displayName == className)
				{
					result = i;
					break;
				}
			}
		}
		return result;
	}

	public void AddOwnedActorData(ActorData actorData)
	{
		if (m_ownedActorDatas.Contains(actorData))
		{
			while (true)
			{
				return;
			}
		}
		if (actorData.GetTeam() != 0)
		{
			if (m_ownedActorDatas.Count != 0)
			{
				m_ownedActorDatas.Add(actorData);
				Log.Info("GameFlowData.AddOwnedActorData {0} {1}", m_ownedActorDatas.Count, actorData);
				goto IL_00a4;
			}
		}
		m_ownedActorDatas.Insert(0, actorData);
		Log.Info("GameFlowData.AddOwnedActorData {0} {1}", 0, actorData);
		goto IL_00a4;
		IL_00a4:
		if (!(activeOwnedActorData == null))
		{
			return;
		}
		while (true)
		{
			activeOwnedActorData = actorData;
			return;
		}
	}

	public void ResetOwnedActorDataToFirst()
	{
		if (m_ownedActorDatas.Count <= 0)
		{
			return;
		}
		while (true)
		{
			if (!(SpawnPointManager.Get() == null))
			{
				if (SpawnPointManager.Get().m_playersSelectRespawn)
				{
					goto IL_00b4;
				}
			}
			foreach (ActorData ownedActorData in m_ownedActorDatas)
			{
				if (ownedActorData != null && !ownedActorData.IsDead())
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							break;
						default:
							activeOwnedActorData = ownedActorData;
							return;
						}
					}
				}
			}
			goto IL_00b4;
			IL_00b4:
			activeOwnedActorData = m_ownedActorDatas[0];
			return;
		}
	}

	public bool IsActorDataOwned(ActorData actorData)
	{
		int result;
		if (actorData != null)
		{
			result = (m_ownedActorDatas.Contains(actorData) ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	public void SetActiveNextNonConfirmedOwnedActorData()
	{
		if (activeOwnedActorData == null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					activeOwnedActorData = firstOwnedFriendlyActorData;
					return;
				}
			}
		}
		bool flag = false;
		int num = 0;
		int num2 = 0;
		while (true)
		{
			if (num2 < m_ownedActorDatas.Count)
			{
				if (m_ownedActorDatas[num2] == activeOwnedActorData)
				{
					num = num2;
					break;
				}
				num2++;
				continue;
			}
			break;
		}
		int num3 = 0;
		while (true)
		{
			if (num3 < m_ownedActorDatas.Count)
			{
				int index = (num + num3) % m_ownedActorDatas.Count;
				ActorData actorData = m_ownedActorDatas[index];
				if (actorData != activeOwnedActorData && activeOwnedActorData != null)
				{
					if (actorData.GetTeam() == activeOwnedActorData.GetTeam())
					{
						ActorTurnSM component = actorData.GetComponent<ActorTurnSM>();
						if (component.CurrentState != TurnStateEnum.CONFIRMED)
						{
							flag = true;
							activeOwnedActorData = actorData;
							break;
						}
					}
				}
				num3++;
				continue;
			}
			break;
		}
		if (flag)
		{
			return;
		}
		while (true)
		{
			int num4 = 0;
			ActorData actorData2;
			while (true)
			{
				if (num4 >= m_ownedActorDatas.Count)
				{
					return;
				}
				int index2 = (num + num4) % m_ownedActorDatas.Count;
				actorData2 = m_ownedActorDatas[index2];
				if (actorData2 != activeOwnedActorData)
				{
					ActorTurnSM component2 = actorData2.GetComponent<ActorTurnSM>();
					if (component2.CurrentState != TurnStateEnum.CONFIRMED)
					{
						break;
					}
				}
				num4++;
			}
			while (true)
			{
				flag = true;
				activeOwnedActorData = actorData2;
				return;
			}
		}
	}

	public bool SetActiveOwnedActor_FCFS(ActorData actor)
	{
		if (actor != null)
		{
			if (IsActorDataOwned(actor))
			{
				if (activeOwnedActorData != actor)
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							break;
						default:
							activeOwnedActorData = actor;
							return true;
						}
					}
				}
			}
		}
		return false;
	}

	public string GetActiveOwnedActorDataDebugNameString()
	{
		if ((bool)activeOwnedActorData)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return activeOwnedActorData.GetDebugName();
				}
			}
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
			if (actorData.GetTeam() == Team.TeamA)
			{
				if (!m_teamAPlayerAndBots.Contains(actorData))
				{
					m_teamBPlayerAndBots.Remove(actorData);
					m_teamObjectsPlayerAndBots.Remove(actorData);
					m_teamAPlayerAndBots.Add(actorData);
					goto IL_0119;
				}
			}
			if (actorData.GetTeam() == Team.TeamB && !m_teamBPlayerAndBots.Contains(actorData))
			{
				m_teamAPlayerAndBots.Remove(actorData);
				m_teamObjectsPlayerAndBots.Remove(actorData);
				m_teamBPlayerAndBots.Add(actorData);
			}
			else if (actorData.GetTeam() == Team.Objects)
			{
				if (!m_teamObjectsPlayerAndBots.Contains(actorData))
				{
					m_teamAPlayerAndBots.Remove(actorData);
					m_teamBPlayerAndBots.Remove(actorData);
					m_teamObjectsPlayerAndBots.Add(actorData);
				}
			}
		}
		goto IL_0119;
		IL_0119:
		if (actorData.GetTeam() == Team.TeamA)
		{
			if (!m_teamA.Contains(actorData))
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						m_teamB.Remove(actorData);
						m_teamObjects.Remove(actorData);
						m_teamA.Add(actorData);
						return;
					}
				}
			}
		}
		if (actorData.GetTeam() == Team.TeamB && !m_teamB.Contains(actorData))
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					m_teamA.Remove(actorData);
					m_teamObjects.Remove(actorData);
					m_teamB.Add(actorData);
					return;
				}
			}
		}
		if (actorData.GetTeam() != Team.Objects)
		{
			return;
		}
		while (true)
		{
			if (!m_teamObjects.Contains(actorData))
			{
				while (true)
				{
					m_teamA.Remove(actorData);
					m_teamB.Remove(actorData);
					m_teamObjects.Add(actorData);
					return;
				}
			}
			return;
		}
	}

	private List<ActorData> GetAllActorsOnTeam(Team team)
	{
		List<ActorData> list = null;
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
		List<ActorData> list = null;
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

	public void RemoveExistingPlayer(GameObject player)
	{
		if (!m_players.Contains(player))
		{
			return;
		}
		while (true)
		{
			m_players.Remove(player);
			return;
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
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
				{
					using (List<ActorData>.Enumerator enumerator = m_actors.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							ActorData current = enumerator.Current;
							if (!current.IsDead())
							{
								if (current.IsActorVisibleToActor(observer))
								{
									if (targetableOnly)
									{
										if (current.IgnoreForAbilityHits)
										{
											continue;
										}
									}
									list.Add(current);
								}
							}
						}
						while (true)
						{
							switch (6)
							{
							case 0:
								break;
							default:
								return list;
							}
						}
					}
				}
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
		while (true)
		{
			return list;
		}
	}

	public void AddActor(ActorData actor)
	{
		Log.Info("Registering actor {0}", actor);
		m_actors.Add(actor);
		if (!NetworkServer.active)
		{
			return;
		}
		while (true)
		{
			if (GameFlowData.s_onAddActorHolder != null)
			{
				while (true)
				{
					GameFlowData.s_onAddActorHolder(actor);
					return;
				}
			}
			return;
		}
	}

	internal ActorData FindActorByActorIndex(int actorIndex)
	{
		ActorData actorData = null;
		int num = 0;
		while (true)
		{
			if (num < m_actors.Count)
			{
				ActorData actorData2 = m_actors[num];
				if (actorData2.ActorIndex == actorIndex)
				{
					actorData = actorData2;
					break;
				}
				num++;
				continue;
			}
			break;
		}
		if (actorData == null)
		{
			if (actorIndex > 0 && CurrentTurn > 0)
			{
				if (GameManager.Get() != null)
				{
					if (GameManager.Get().GameConfig != null)
					{
						GameType gameType = GameManager.Get().GameConfig.GameType;
						if (gameType != GameType.Tutorial)
						{
							Log.Warning("Failed to find actor index {0}", actorIndex);
						}
					}
				}
			}
		}
		return actorData;
	}

	internal ActorData FindActorByPlayerIndex(int playerIndex)
	{
		for (int i = 0; i < m_players.Count; i++)
		{
			ActorData component = m_players[i].GetComponent<ActorData>();
			if (component != null)
			{
				if (component.PlayerIndex == playerIndex)
				{
					return component;
				}
			}
		}
		while (true)
		{
			Log.Warning("Failed to find player index {0}", playerIndex);
			return null;
		}
	}

	internal ActorData FindActorByPlayer(Player player)
	{
		for (int i = 0; i < m_actors.Count; i++)
		{
			PlayerData playerData = m_actors[i].PlayerData;
			if (!(playerData != null) || !(playerData.GetPlayer() == player))
			{
				continue;
			}
			while (true)
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
		if (actor != null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
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
					if (GameFlowData.s_onRemoveActorHolder != null)
					{
						while (true)
						{
							switch (1)
							{
							case 0:
								break;
							default:
								GameFlowData.s_onRemoveActorHolder(actor);
								return;
							}
						}
					}
					return;
				}
			}
		}
		Log.Error("Trying to destroy a null actor.");
	}

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
		Networkm_gameState = value;
		m_timeInState = 0f;
		m_timeInStateUnscaled = 0f;
		Log.Info("Game state: {0}", value.ToString());
		switch (m_gameState)
		{
		case GameState.Deployment:
			if (SinglePlayerManager.Get() != null)
			{
				SinglePlayerManager.Get().OnTurnTick();
			}
			m_deploymentStartTime = Time.realtimeSinceStartup;
			break;
		case GameState.StartingGame:
			if (HUD_UI.Get() != null)
			{
				SinglePlayerManager.ResetUIActivations();
			}
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
		}
		if (GameFlowData.s_onGameStateChangedHolder == null)
		{
			return;
		}
		while (true)
		{
			GameFlowData.s_onGameStateChangedHolder(m_gameState);
			return;
		}
	}

	public int GetNumAvailableCharacterResourceLinks()
	{
		return m_availableCharacterResourceLinkPrefabs.Length;
	}

	public string GetFirstAvailableCharacterResourceLinkName()
	{
		GameObject gameObject = m_availableCharacterResourceLinkPrefabs[0];
		object obj;
		if (gameObject == null)
		{
			obj = null;
		}
		else
		{
			obj = gameObject.GetComponent<CharacterResourceLink>();
		}
		CharacterResourceLink characterResourceLink = (CharacterResourceLink)obj;
		if (characterResourceLink != null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return characterResourceLink.m_displayName;
				}
			}
		}
		return string.Empty;
	}

	private void IncrementTurn()
	{
		m_timeInDecision = 0f;
		Networkm_currentTurn = m_currentTurn + 1;
		NotifyOnTurnTick();
		Log.Info("Turn: {0}", CurrentTurn);
		if (!(Board.Get() != null))
		{
			return;
		}
		while (true)
		{
			Board.Get().MarkForUpdateValidSquares();
			return;
		}
	}

	private void NotifyOnTurnTick()
	{
		if (TeamSensitiveDataMatchmaker.Get() != null)
		{
			TeamSensitiveDataMatchmaker.Get().SetTeamSensitiveDataForUnhandledActors();
		}
		GameEventManager.Get().FireEvent(GameEventManager.EventType.TurnTick, null);
		ShowIntervanStatusNotifications();
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
		if (InterfaceManager.Get() != null)
		{
			InterfaceManager.Get().OnTurnTick();
		}
		List<PowerUp.IPowerUpListener> powerUpListeners = PowerUpManager.Get().powerUpListeners;
		using (List<PowerUp.IPowerUpListener>.Enumerator enumerator = powerUpListeners.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				PowerUp.IPowerUpListener current = enumerator.Current;
				current.OnTurnTick();
			}
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
		foreach (ActorData actor in GetActors())
		{
			actor.OnTurnTick();
		}
		if (ObjectivePoints.Get() != null)
		{
			ObjectivePoints.Get().OnTurnTick();
		}
		if (ClientAbilityResults.LogMissingSequences)
		{
			Log.Warning("Turn Start: <color=magenta>" + Get().CurrentTurn + "</color>");
		}
		if (!(ControlpadGameplay.Get() != null))
		{
			return;
		}
		while (true)
		{
			ControlpadGameplay.Get().OnTurnTick();
			return;
		}
	}

	public bool HasPotentialGameMutatorVisibilityChanges(bool onTurnStart)
	{
		bool flag = false;
		GameplayMutators gameplayMutators = GameplayMutators.Get();
		if (gameplayMutators != null && CurrentTurn > 1)
		{
			for (int i = 0; i < gameplayMutators.m_alwaysOnStatuses.Count; i++)
			{
				if (!flag)
				{
					GameplayMutators.StatusInterval statusInterval = gameplayMutators.m_alwaysOnStatuses[i];
					if (statusInterval.m_statusType != StatusType.Blind)
					{
						if (statusInterval.m_statusType != StatusType.InvisibleToEnemies)
						{
							if (statusInterval.m_statusType != 0)
							{
								continue;
							}
						}
					}
					if (onTurnStart)
					{
						if (GameplayMutators.IsStatusActive(statusInterval.m_statusType, CurrentTurn) != GameplayMutators.IsStatusActive(statusInterval.m_statusType, CurrentTurn - 1))
						{
							flag = true;
							continue;
						}
					}
					if (onTurnStart)
					{
						continue;
					}
					if (GameplayMutators.IsStatusActive(statusInterval.m_statusType, CurrentTurn, GameplayMutators.ActionPhaseCheckMode.Abilities) != GameplayMutators.IsStatusActive(statusInterval.m_statusType, CurrentTurn, GameplayMutators.ActionPhaseCheckMode.Movement))
					{
						flag = true;
					}
					continue;
				}
				break;
			}
			for (int j = 0; j < gameplayMutators.m_statusSuppression.Count; j++)
			{
				if (!flag)
				{
					GameplayMutators.StatusInterval statusInterval2 = gameplayMutators.m_statusSuppression[j];
					if (statusInterval2.m_statusType != StatusType.Blind)
					{
						if (statusInterval2.m_statusType != StatusType.InvisibleToEnemies)
						{
							if (statusInterval2.m_statusType != 0)
							{
								continue;
							}
						}
					}
					if (onTurnStart)
					{
						if (GameplayMutators.IsStatusSuppressed(statusInterval2.m_statusType, CurrentTurn) != GameplayMutators.IsStatusSuppressed(statusInterval2.m_statusType, CurrentTurn - 1))
						{
							flag = true;
							continue;
						}
					}
					if (!onTurnStart)
					{
						if (GameplayMutators.IsStatusSuppressed(statusInterval2.m_statusType, CurrentTurn, GameplayMutators.ActionPhaseCheckMode.Abilities) != GameplayMutators.IsStatusSuppressed(statusInterval2.m_statusType, CurrentTurn, GameplayMutators.ActionPhaseCheckMode.Movement))
						{
							flag = true;
						}
					}
					continue;
				}
				break;
			}
		}
		return flag;
	}

	private void ShowIntervanStatusNotifications()
	{
		if (!NetworkClient.active)
		{
			return;
		}
		if (HUD_UI.Get() == null)
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
		GameplayMutators gameplayMutators = GameplayMutators.Get();
		if (!(gameplayMutators != null))
		{
			return;
		}
		while (true)
		{
			for (int i = 0; i < gameplayMutators.m_alwaysOnStatuses.Count; i++)
			{
				GameplayMutators.StatusInterval statusInterval = gameplayMutators.m_alwaysOnStatuses[i];
				if (statusInterval.m_delayTillStartOfMovement)
				{
					bool flag = GameplayMutators.IsStatusActive(statusInterval.m_statusType, CurrentTurn, GameplayMutators.ActionPhaseCheckMode.Abilities);
					bool flag2 = GameplayMutators.IsStatusActive(statusInterval.m_statusType, CurrentTurn, GameplayMutators.ActionPhaseCheckMode.Movement);
					if (!flag && flag2 && !string.IsNullOrEmpty(statusInterval.m_activateNotificationTurnBefore))
					{
						InterfaceManager.Get().DisplayAlert(StringUtil.TR_IfHasContext(statusInterval.m_activateNotificationTurnBefore), Color.cyan, 5f, true, 1);
					}
					bool flag3 = GameplayMutators.IsStatusActive(statusInterval.m_statusType, CurrentTurn + 1, GameplayMutators.ActionPhaseCheckMode.Abilities);
					if (!flag2 || flag3)
					{
						continue;
					}
					if (!string.IsNullOrEmpty(statusInterval.m_offNotificationTurnBefore))
					{
						InterfaceManager.Get().DisplayAlert(StringUtil.TR_IfHasContext(statusInterval.m_offNotificationTurnBefore), Color.cyan, 5f, true, 1);
					}
					continue;
				}
				bool flag4 = GameplayMutators.IsStatusActive(statusInterval.m_statusType, CurrentTurn, GameplayMutators.ActionPhaseCheckMode.Any);
				bool flag5 = GameplayMutators.IsStatusActive(statusInterval.m_statusType, CurrentTurn + 1, GameplayMutators.ActionPhaseCheckMode.Any);
				if (flag4 && !flag5 && !string.IsNullOrEmpty(statusInterval.m_offNotificationTurnBefore))
				{
					InterfaceManager.Get().DisplayAlert(StringUtil.TR_IfHasContext(statusInterval.m_offNotificationTurnBefore), Color.cyan, 5f, true, 1);
					continue;
				}
				if (flag4)
				{
					continue;
				}
				if (!flag5)
				{
					continue;
				}
				if (!string.IsNullOrEmpty(statusInterval.m_activateNotificationTurnBefore))
				{
					InterfaceManager.Get().DisplayAlert(StringUtil.TR_IfHasContext(statusInterval.m_activateNotificationTurnBefore), Color.cyan, 5f, true, 1);
				}
			}
			return;
		}
	}

	public void NotifyOnActorDeath(ActorData actor)
	{
		if (NetworkServer.active)
		{
		}
		if (SinglePlayerManager.Get() != null)
		{
			SinglePlayerManager.Get().OnActorDeath(actor);
		}
		if (NPCCoordinator.Get() != null)
		{
			NPCCoordinator.Get().OnActorDeath(actor);
		}
		SatelliteController[] components = actor.GetComponents<SatelliteController>();
		SatelliteController[] array = components;
		foreach (SatelliteController satelliteController in array)
		{
			satelliteController.OnActorDeath();
		}
		while (true)
		{
			GameEventManager.Get().FireEvent(GameEventManager.EventType.PostCharacterDeath, new GameEventManager.CharacterDeathEventArgs
			{
				deadCharacter = actor
			});
			return;
		}
	}

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
		float num = 0f;
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
		_000E();
		if (!m_pause)
		{
			m_timeInState += Time.deltaTime;
			m_timeInStateUnscaled += Time.unscaledDeltaTime;
			m_timeInDecision += Time.deltaTime;
		}
		if (!(AppState.GetCurrent() != AppState_GameTeardown.Get()))
		{
			return;
		}
		while (true)
		{
			UpdateTimeRemaining();
			return;
		}
	}

	private void UpdateTimeRemaining()
	{
		bool flag = Get().IsInDecisionState();
		if (m_pause || !flag)
		{
			return;
		}
		while (true)
		{
			if (!(m_timeRemainingInDecision >= 0f - m_timeRemainingInDecisionOverflow))
			{
				return;
			}
			while (true)
			{
				m_timeRemainingInDecision -= Time.deltaTime;
				if (m_timeRemainingInDecision < 0f - m_timeRemainingInDecisionOverflow)
				{
					while (true)
					{
						m_timeRemainingInDecision = 0f - m_timeRemainingInDecisionOverflow;
						return;
					}
				}
				return;
			}
		}
	}

	private void _000E()
	{
	}

	public bool IsOwnerTargeting()
	{
		bool result = false;
		ActorData activeOwnedActorData = this.activeOwnedActorData;
		if (activeOwnedActorData != null)
		{
			ActorTurnSM component = activeOwnedActorData.GetComponent<ActorTurnSM>();
			if ((bool)component)
			{
				if (component.CurrentState == TurnStateEnum.TARGETING_ACTION)
				{
					result = true;
				}
			}
		}
		return result;
	}

	public bool IsInDecisionState()
	{
		GameState gameState = m_gameState;
		if (gameState != GameState.BothTeams_Decision)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return false;
				}
			}
		}
		return true;
	}

	public bool IsTeamsTurn(Team team)
	{
		bool flag = false;
		if (!IsTeamADecision())
		{
			if (!IsTeamAResolving())
			{
				goto IL_003a;
			}
		}
		flag = (flag || team == Team.TeamA);
		goto IL_003a;
		IL_003a:
		if (!IsTeamBDecision())
		{
			if (!IsTeamBResolving())
			{
				goto IL_0067;
			}
		}
		flag = (flag || team == Team.TeamB);
		goto IL_0067;
		IL_0067:
		return flag;
	}

	public bool IsOwnedActorsTurn()
	{
		bool result = false;
		if (activeOwnedActorData != null)
		{
			result = IsTeamsTurn(activeOwnedActorData.GetTeam());
		}
		return result;
	}

	public bool IsInResolveState()
	{
		GameState gameState = m_gameState;
		if (gameState != GameState.BothTeams_Resolve)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return false;
				}
			}
		}
		return true;
	}

	public bool IsTeamAResolving()
	{
		return IsInResolveState();
	}

	public bool IsTeamBResolving()
	{
		return IsInResolveState();
	}

	public bool IsTeamADecision()
	{
		GameState gameState = m_gameState;
		if (gameState != GameState.BothTeams_Decision)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return false;
				}
			}
		}
		return true;
	}

	public bool IsTeamBDecision()
	{
		GameState gameState = m_gameState;
		if (gameState != GameState.BothTeams_Decision)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return false;
				}
			}
		}
		return true;
	}

	public bool IsPhase(int phase)
	{
		GameState gameState = m_gameState;
		if (gameState != GameState.BothTeams_Decision)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					if (gameState == GameState.BothTeams_Resolve)
					{
						return phase == 1;
					}
					return false;
				}
			}
		}
		return phase == 1;
	}

	public void SetSelectedTeam(int team)
	{
		m_selectedTeam = (Team)team;
	}

	public bool ShouldForceResolveTimeout()
	{
		bool flag = m_timeInStateUnscaled > m_resolveTimeoutLimit;
		int num;
		if (DebugParameters.Get() != null)
		{
			num = (DebugParameters.Get().GetParameterAsBool("DisableResolveFailsafe") ? 1 : 0);
		}
		else
		{
			num = 0;
		}
		bool flag2 = (byte)num != 0;
		return flag && !flag2;
	}

	public bool PlayersMustUseTimeBank()
	{
		int result;
		if (IsInDecisionState())
		{
			result = ((GetTimeInState() > m_turnTime) ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	public bool WillEnterTimebankMode()
	{
		return m_willEnterTimebankMode;
	}

	public bool PreventAutoLockInOnTimeout()
	{
		GameManager gameManager = GameManager.Get();
		if (gameManager == null)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return false;
				}
			}
		}
		GameType gameType = gameManager.GameConfig.GameType;
		if (gameType == GameType.Practice)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return true;
				}
			}
		}
		if (!gameManager.GameplayOverrides.SoloGameNoAutoLockinOnTimeout)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return false;
				}
			}
		}
		if (gameType != GameType.Solo)
		{
			if (gameType != GameType.NewPlayerSolo)
			{
				if (gameType != GameType.Tutorial)
				{
					if (!gameManager.GameConfig.InstanceSubType.HasMod(GameSubType.SubTypeMods.AntiSocial))
					{
						return false;
					}
				}
			}
		}
		return true;
	}

	public void ClearCooldowns()
	{
		AbilityData abilityData = null;
		GameFlowData gameFlowData = Get();
		if ((bool)gameFlowData)
		{
			ActorData activeOwnedActorData = gameFlowData.activeOwnedActorData;
			if ((bool)activeOwnedActorData)
			{
				abilityData = activeOwnedActorData.GetComponent<AbilityData>();
			}
		}
		if (NetworkServer.active)
		{
			if (!abilityData)
			{
				return;
			}
			while (true)
			{
				abilityData.ClearCooldowns();
				return;
			}
		}
		if (!abilityData)
		{
			return;
		}
		while (true)
		{
			abilityData.CallCmdClearCooldowns();
			return;
		}
	}

	public void RefillStocks()
	{
		AbilityData abilityData = null;
		GameFlowData gameFlowData = Get();
		if ((bool)gameFlowData)
		{
			ActorData activeOwnedActorData = gameFlowData.activeOwnedActorData;
			if ((bool)activeOwnedActorData)
			{
				abilityData = activeOwnedActorData.GetComponent<AbilityData>();
			}
		}
		if (NetworkServer.active)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					if ((bool)abilityData)
					{
						while (true)
						{
							switch (1)
							{
							case 0:
								break;
							default:
								abilityData.RefillStocks();
								return;
							}
						}
					}
					return;
				}
			}
		}
		if ((bool)abilityData)
		{
			abilityData.CallCmdRefillStocks();
		}
	}

	public void DistributeRewardForKill(ActorData killedActor)
	{
		if (!GameplayUtils.IsPlayerControlled(killedActor))
		{
			if (!GameplayUtils.IsMinion(killedActor))
			{
				while (true)
				{
					switch (4)
					{
					default:
						return;
					case 0:
						break;
					}
				}
			}
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
		if (num <= 0)
		{
			return;
		}
		while (true)
		{
			if (flag)
			{
				List<ActorData> contributorsToKill = GetContributorsToKill(killedActor);
				if (contributorsToKill.Count > 0)
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							break;
						default:
							RewardContributorsToKill(contributorsToKill, num);
							return;
						}
					}
				}
				if (GameplayData.Get().m_participationlessBountiesGoToTeam)
				{
					RewardTeam(killedActor.GetOpposingTeam(), num);
				}
			}
			else
			{
				RewardTeam(killedActor.GetOpposingTeam(), num);
			}
			return;
		}
	}

	public int GetTotalDeathsOnTurnStart(Team team)
	{
		int num = 0;
		List<ActorData> allTeamMembers = GetAllTeamMembers(team);
		if (allTeamMembers != null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					{
						foreach (ActorData item in allTeamMembers)
						{
							if (item.GetActorBehavior() != null)
							{
								num += item.GetActorBehavior().totalDeathsOnTurnStart;
							}
						}
						return num;
					}
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
		}
		return result;
	}

	public List<ActorData> GetContributorsToKillOnClient(ActorData killedActor, bool onlyDirectDamagers = false)
	{
		List<ActorData> list = new List<ActorData>();
		List<ActorData> allTeamMembers = GetAllTeamMembers(killedActor.GetOpposingTeam());
		if (allTeamMembers != null)
		{
			ActorBehavior actorBehavior = killedActor.GetActorBehavior();
			foreach (ActorData item in allTeamMembers)
			{
				if (!GameplayUtils.IsPlayerControlled(item))
				{
				}
				else if (actorBehavior.Client_ActorDamagedOrDebuffedByActor(item))
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							break;
						default:
							list.Add(item);
							goto end_IL_0041;
						}
					}
				}
			}
			if (!onlyDirectDamagers)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
					{
						List<ActorData> list2 = new List<ActorData>();
						using (List<ActorData>.Enumerator enumerator2 = allTeamMembers.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								ActorData current2 = enumerator2.Current;
								if (list.Contains(current2))
								{
								}
								else if (!GameplayUtils.IsPlayerControlled(current2))
								{
								}
								else
								{
									using (List<ActorData>.Enumerator enumerator3 = list.GetEnumerator())
									{
										while (true)
										{
											if (!enumerator3.MoveNext())
											{
												break;
											}
											ActorData current3 = enumerator3.Current;
											if (list2.Contains(current2))
											{
												break;
											}
											ActorBehavior actorBehavior2 = current3.GetActorBehavior();
											if (actorBehavior2.Client_ActorHealedOrBuffedByActor(current2))
											{
												while (true)
												{
													switch (6)
													{
													case 0:
														break;
													default:
														list2.Add(current2);
														goto end_IL_0104;
													}
												}
											}
										}
										end_IL_0104:;
									}
								}
							}
						}
						using (List<ActorData>.Enumerator enumerator4 = list2.GetEnumerator())
						{
							while (enumerator4.MoveNext())
							{
								ActorData current4 = enumerator4.Current;
								list.Add(current4);
							}
							while (true)
							{
								switch (2)
								{
								case 0:
									break;
								default:
									return list;
								}
							}
						}
					}
					}
				}
			}
		}
		return list;
	}

	public void RewardContributorsToKill(List<ActorData> participants, int baseCreditsReward)
	{
		if (participants.Count <= 0)
		{
			return;
		}
		while (true)
		{
			int num = baseCreditsReward / participants.Count;
			int num2 = Mathf.FloorToInt(GameplayData.Get().m_creditBonusFractionPerExtraPlayer * (float)(participants.Count - 1) * (float)num);
			int numCredits = num + num2;
			using (List<ActorData>.Enumerator enumerator = participants.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ActorData current = enumerator.Current;
					ItemData component = current.GetComponent<ItemData>();
					if (component != null)
					{
						component.GiveCredits(numCredits);
					}
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

	public void RewardTeam(Team teamToReward, int creditsReward)
	{
		List<ActorData> allTeamMembers = GetAllTeamMembers(teamToReward);
		if (allTeamMembers != null)
		{
			using (List<ActorData>.Enumerator enumerator = allTeamMembers.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ActorData current = enumerator.Current;
					ItemData component = current.GetComponent<ItemData>();
					if (component != null)
					{
						component.GiveCredits(creditsReward);
					}
				}
				while (true)
				{
					switch (1)
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

	public int GetDeathCountOfTeam(Team team)
	{
		int num = 0;
		for (int i = 0; i < m_actors.Count; i++)
		{
			ActorData actorData = m_actors[i];
			if (!(actorData != null))
			{
				continue;
			}
			if (actorData.GetTeam() != team)
			{
				continue;
			}
			if (actorData.GetActorBehavior() != null)
			{
				num += actorData.GetActorBehavior().totalDeaths;
			}
		}
		while (true)
		{
			return num;
		}
	}

	public int GetTotalTeamDamageReceived(Team team)
	{
		int num = 0;
		for (int i = 0; i < m_actors.Count; i++)
		{
			ActorData actorData = m_actors[i];
			if (actorData != null && actorData.GetTeam() == team && actorData.GetActorBehavior() != null)
			{
				num += actorData.GetActorBehavior().totalPlayerDamageReceived;
			}
		}
		while (true)
		{
			return num;
		}
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
		if (!NetworkServer.active)
		{
			return;
		}
		while (true)
		{
			return;
		}
	}

	public void SetLocalPlayerData()
	{
		m_localPlayerData = null;
		if (GameFlow.Get() != null)
		{
			foreach (GameObject player in m_players)
			{
				if (player != null)
				{
					PlayerData component = player.GetComponent<PlayerData>();
					if (component != null)
					{
						PlayerDetails value = null;
						if (GameFlow.Get().playerDetails.TryGetValue(component.GetPlayer(), out value))
						{
							if (value.IsLocal())
							{
								m_localPlayerData = component;
								break;
							}
						}
					}
				}
			}
		}
	}

	private void UNetVersion()
	{
	}

	protected static void InvokeRpcRpcUpdateTimeRemaining(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					Debug.LogError("RPC RpcUpdateTimeRemaining called on server.");
					return;
				}
			}
		}
		((GameFlowData)obj).RpcUpdateTimeRemaining(reader.ReadSingle());
	}

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
		if ((base.syncVarDirtyBits & 1) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.Write(m_pause);
		}
		if ((base.syncVarDirtyBits & 2) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.Write(m_pausedForDebugging);
		}
		if ((base.syncVarDirtyBits & 4) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.Write(m_pausedByPlayerRequest);
		}
		if ((base.syncVarDirtyBits & 8) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.Write(m_pausedForSinglePlayer);
		}
		if ((base.syncVarDirtyBits & 0x10) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.Write((int)m_resolutionPauseState);
		}
		if ((base.syncVarDirtyBits & 0x20) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.Write(m_startTime);
		}
		if ((base.syncVarDirtyBits & 0x40) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.Write(m_deploymentTime);
		}
		if ((base.syncVarDirtyBits & 0x80) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.Write(m_turnTime);
		}
		if ((base.syncVarDirtyBits & 0x100) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.Write(m_maxTurnTime);
		}
		if ((base.syncVarDirtyBits & 0x200) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.Write(m_timeRemainingInDecisionOverflow);
		}
		if ((base.syncVarDirtyBits & 0x400) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.Write(m_willEnterTimebankMode);
		}
		if ((base.syncVarDirtyBits & 0x800) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_currentTurn);
		}
		if ((base.syncVarDirtyBits & 0x1000) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.Write((int)m_gameState);
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
		if ((num & 0x1000) == 0)
		{
			return;
		}
		while (true)
		{
			HookSetGameState((GameState)reader.ReadInt32());
			return;
		}
	}
}

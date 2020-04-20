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

	static GameFlowData()
	{
		GameFlowData.s_onAddActorHolder = delegate(ActorData A_0)
		{
		};
		GameFlowData.s_onRemoveActorHolder = delegate(ActorData A_0)
		{
		};
		GameFlowData.s_onActiveOwnedActorChangeHolder = delegate(ActorData A_0)
		{
		};
		GameFlowData.s_onGameStateChangedHolder = delegate(GameState A_0)
		{
		};
		GameFlowData.s_loadingScreenTime = 4f;
		GameFlowData.kRpcRpcUpdateTimeRemaining = 0x3800B000;
		NetworkBehaviour.RegisterRpcDelegate(typeof(GameFlowData), GameFlowData.kRpcRpcUpdateTimeRemaining, new NetworkBehaviour.CmdDelegate(GameFlowData.InvokeRpcRpcUpdateTimeRemaining));
		NetworkCRC.RegisterBehaviour("GameFlowData", 0);
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
				action = Interlocked.CompareExchange<Action<ActorData>>(ref GameFlowData.s_onAddActorHolder, (Action<ActorData>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<ActorData> action = GameFlowData.s_onAddActorHolder;
			Action<ActorData> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<ActorData>>(ref GameFlowData.s_onAddActorHolder, (Action<ActorData>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
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
				action = Interlocked.CompareExchange<Action<ActorData>>(ref GameFlowData.s_onRemoveActorHolder, (Action<ActorData>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<ActorData> action = GameFlowData.s_onRemoveActorHolder;
			Action<ActorData> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<ActorData>>(ref GameFlowData.s_onRemoveActorHolder, (Action<ActorData>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
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
				action = Interlocked.CompareExchange<Action<ActorData>>(ref GameFlowData.s_onActiveOwnedActorChangeHolder, (Action<ActorData>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<ActorData> action = GameFlowData.s_onActiveOwnedActorChangeHolder;
			Action<ActorData> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<ActorData>>(ref GameFlowData.s_onActiveOwnedActorChangeHolder, (Action<ActorData>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
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
				action = Interlocked.CompareExchange<Action<GameState>>(ref GameFlowData.s_onGameStateChangedHolder, (Action<GameState>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<GameState> action = GameFlowData.s_onGameStateChangedHolder;
			Action<GameState> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<GameState>>(ref GameFlowData.s_onGameStateChangedHolder, (Action<GameState>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public bool Started { get; private set; }

	public PlayerData LocalPlayerData
	{
		get
		{
			return this.m_localPlayerData;
		}
	}

	private void Awake()
	{
		GameFlowData.s_gameFlowData = this;
		if (ClientGamePrefabInstantiator.Get() != null)
		{
			ClientGamePrefabInstantiator.Get().InstantiatePrefabs();
		}
		else
		{
			Log.Error("ClientGamePrefabInstantiator reference not set on game start", new object[0]);
		}
	}

	private void Start()
	{
		this.Started = true;
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
		Log.Info("GameFlowData.Start", new object[0]);
		GameEventManager.Get().AddListener(this, GameEventManager.EventType.GameTeardown);
		GameEventManager.Get().AddListener(this, GameEventManager.EventType.TheatricsAbilityAnimationStart);
		GameEventManager.Get().AddListener(this, GameEventManager.EventType.ServerActionBufferPhaseStart);
		GameEventManager.Get().AddListener(this, GameEventManager.EventType.ServerActionBufferActionsDone);
	}

	public void OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		if (eventType != GameEventManager.EventType.GameTeardown)
		{
		}
		else
		{
			UnityEngine.Object.Destroy(this.m_actorRoot);
			UnityEngine.Object.Destroy(this.m_thinCoverRoot);
			UnityEngine.Object.Destroy(this.m_brushRegionBorderRoot);
			CharacterResourceLink.DestroyAudioResources();
		}
	}

	public override void OnStartServer()
	{
		LobbyGameConfig gameConfig = GameManager.Get().GameConfig;
		this.Networkm_gameState = GameState.Launched;
		this.Networkm_turnTime = Convert.ToSingle(gameConfig.TurnTime);
		this.Networkm_maxTurnTime = Convert.ToSingle(gameConfig.TurnTime) + Mathf.Max(GameWideData.Get().m_tbInitial, GameWideData.Get().m_tbRechargeCap) + GameWideData.Get().m_tbConsumableDuration + 1f;
		this.m_resolveTimeoutLimit = Convert.ToSingle(gameConfig.ResolveTimeoutLimit);
	}

	public override void OnStartClient()
	{
		if (this.CurrentTurn > 0)
		{
			this.NotifyOnTurnTick();
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
		this.m_ownedActorDatas.Clear();
		this.m_activeOwnedActorData = null;
		this.m_actors.Clear();
		this.m_teamA.Clear();
		this.m_teamAPlayerAndBots.Clear();
		this.m_teamB.Clear();
		this.m_teamBPlayerAndBots.Clear();
		this.m_teamObjects.Clear();
		this.m_teamObjectsPlayerAndBots.Clear();
		GameFlowData.s_gameFlowData = null;
	}

	public bool GetPause()
	{
		return this.m_pause;
	}

	public bool GetPauseForDialog()
	{
		return this.m_pausedForDialog;
	}

	public bool GetPauseForSinglePlayer()
	{
		return this.m_pausedForSinglePlayer;
	}

	public bool GetPauseForDebugging()
	{
		return this.m_pausedForDebugging;
	}

	public bool GetPausedByPlayerRequest()
	{
		return this.m_pausedByPlayerRequest;
	}

	internal void SetPausedForDialog(bool pause)
	{
		if (NetworkServer.active)
		{
			if (this.m_pausedForDialog != pause)
			{
				this.m_pausedForDialog = pause;
				this.UpdatePause();
			}
		}
	}

	internal void SetPausedForSinglePlayer(bool pause)
	{
		if (NetworkServer.active)
		{
			if (this.m_pausedForSinglePlayer != pause)
			{
				this.Networkm_pausedForSinglePlayer = pause;
				this.UpdatePause();
			}
		}
	}

	internal void SetPausedForDebugging(bool pause)
	{
	}

	internal void SetPausedForCustomGame(bool pause)
	{
		if (NetworkServer.active && this.m_pausedByPlayerRequest != pause)
		{
			this.Networkm_pausedByPlayerRequest = pause;
			this.UpdatePause();
		}
	}

	public void UpdatePause()
	{
		if (NetworkServer.active)
		{
			bool flag;
			if (!this.m_pausedForDialog)
			{
				if (!this.m_pausedForSinglePlayer)
				{
					if (!this.m_pausedForDebugging)
					{
						flag = this.m_pausedByPlayerRequest;
						goto IL_5B;
					}
				}
			}
			flag = true;
			IL_5B:
			bool flag2 = flag;
			if (this.m_pause != flag2)
			{
				this.Networkm_pause = flag2;
			}
		}
	}

	internal bool IsResolutionPaused()
	{
		return this.m_resolutionPauseState == ResolutionPauseState.PausedUntilInput;
	}

	internal bool GetResolutionSingleStepping()
	{
		bool result;
		if (this.m_resolutionPauseState != ResolutionPauseState.PausedUntilInput)
		{
			result = (this.m_resolutionPauseState == ResolutionPauseState.UnpausedUntilNextAbilityOrPhase);
		}
		else
		{
			result = true;
		}
		return result;
	}

	internal void SetResolutionSingleStepping(bool singleStepping)
	{
		if (NetworkServer.active)
		{
			this.HandleSetResolutionSingleStepping(singleStepping);
		}
		else if (GameFlowData.Get() != null)
		{
			if (GameFlowData.Get().activeOwnedActorData != null)
			{
				GameFlowData.Get().activeOwnedActorData.CallCmdSetResolutionSingleStepping(singleStepping);
			}
		}
	}

	internal void SetResolutionSingleSteppingAdvance()
	{
		if (NetworkServer.active)
		{
			this.HandleSetResolutionSingleSteppingAdvance();
		}
		else if (GameFlowData.Get() != null)
		{
			if (GameFlowData.Get().activeOwnedActorData != null)
			{
				GameFlowData.Get().activeOwnedActorData.CallCmdSetResolutionSingleSteppingAdvance();
			}
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
	}

	[Server]
	private void HandleSetResolutionSingleSteppingAdvance()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void GameFlowData::HandleSetResolutionSingleSteppingAdvance()' called on client");
			return;
		}
	}

	public static GameFlowData Get()
	{
		return GameFlowData.s_gameFlowData;
	}

	public static GameObject FindParentBelowRoot(GameObject child)
	{
		GameObject gameObject;
		if (child.transform.parent == null)
		{
			gameObject = null;
		}
		else
		{
			gameObject = child;
		}
		GameObject result = gameObject;
		GameObject gameObject2 = child;
		while (gameObject2.transform.parent != null)
		{
			result = gameObject2;
			gameObject2 = gameObject2.transform.parent.gameObject;
		}
		return result;
	}

	public GameObject GetActorRoot()
	{
		if (this.m_actorRoot == null)
		{
			this.m_actorRoot = new GameObject("ActorRoot");
			UnityEngine.Object.DontDestroyOnLoad(this.m_actorRoot);
		}
		return this.m_actorRoot;
	}

	public GameObject GetThinCoverRoot()
	{
		if (this.m_thinCoverRoot == null)
		{
			this.m_thinCoverRoot = new GameObject("ThinCoverRoot");
			UnityEngine.Object.DontDestroyOnLoad(this.m_thinCoverRoot);
		}
		return this.m_thinCoverRoot;
	}

	public GameObject GetBrushBordersRoot()
	{
		if (this.m_brushRegionBorderRoot == null)
		{
			this.m_brushRegionBorderRoot = new GameObject("BrushRegionBorderRoot");
			UnityEngine.Object.DontDestroyOnLoad(this.m_brushRegionBorderRoot);
		}
		return this.m_brushRegionBorderRoot;
	}

	public Team GetSelectedTeam()
	{
		return this.m_selectedTeam;
	}

	private void HookSetGameState(GameState state)
	{
		if (this.m_gameState != state)
		{
			if (!NetworkServer.active)
			{
				this.gameState = state;
			}
		}
	}

	private void HookSetStartTime(float startTime)
	{
		if (this.m_startTime != startTime)
		{
			this.Networkm_startTime = startTime;
		}
	}

	private void HookSetDeploymentTime(float deploymentTime)
	{
		if (this.m_deploymentTime != deploymentTime)
		{
			this.Networkm_deploymentTime = deploymentTime;
		}
	}

	private void HookSetTurnTime(float turnTime)
	{
		if (this.m_turnTime != turnTime)
		{
			this.Networkm_turnTime = turnTime;
		}
	}

	private void HookSetMaxTurnTime(float maxTurnTime)
	{
		if (this.m_maxTurnTime != maxTurnTime)
		{
			this.Networkm_maxTurnTime = maxTurnTime;
		}
	}

	private void HookSetCurrentTurn(int turn)
	{
		if (!NetworkServer.active)
		{
			while (this.m_currentTurn < turn)
			{
				this.IncrementTurn();
			}
			this.Networkm_currentTurn = turn;
			HUD_UI.Get().m_mainScreenPanel.m_notificationPanel.NotifyTurnCountSet();
		}
	}

	public int GetClassIndexFromName(string className)
	{
		int result = -1;
		for (int i = 0; i < this.m_availableCharacterResourceLinkPrefabs.Length; i++)
		{
			GameObject gameObject = this.m_availableCharacterResourceLinkPrefabs[i];
			CharacterResourceLink characterResourceLink;
			if (gameObject == null)
			{
				characterResourceLink = null;
			}
			else
			{
				characterResourceLink = gameObject.GetComponent<CharacterResourceLink>();
			}
			CharacterResourceLink characterResourceLink2 = characterResourceLink;
			if (characterResourceLink2 != null)
			{
				if (characterResourceLink2.m_displayName == className)
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
		if (this.m_ownedActorDatas.Contains(actorData))
		{
			return;
		}
		if (actorData.GetTeam() != Team.TeamA)
		{
			if (this.m_ownedActorDatas.Count != 0)
			{
				this.m_ownedActorDatas.Add(actorData);
				Log.Info("GameFlowData.AddOwnedActorData {0} {1}", new object[]
				{
					this.m_ownedActorDatas.Count,
					actorData
				});
				goto IL_A4;
			}
		}
		this.m_ownedActorDatas.Insert(0, actorData);
		Log.Info("GameFlowData.AddOwnedActorData {0} {1}", new object[]
		{
			0,
			actorData
		});
		IL_A4:
		if (this.activeOwnedActorData == null)
		{
			this.activeOwnedActorData = actorData;
		}
	}

	public void ResetOwnedActorDataToFirst()
	{
		if (this.m_ownedActorDatas.Count > 0)
		{
			if (!(SpawnPointManager.Get() == null))
			{
				if (SpawnPointManager.Get().m_playersSelectRespawn)
				{
					goto IL_B4;
				}
			}
			foreach (ActorData actorData in this.m_ownedActorDatas)
			{
				if (actorData != null && !actorData.IsDead())
				{
					this.activeOwnedActorData = actorData;
					return;
				}
			}
			IL_B4:
			this.activeOwnedActorData = this.m_ownedActorDatas[0];
		}
	}

	public bool IsActorDataOwned(ActorData actorData)
	{
		bool result;
		if (actorData != null)
		{
			result = this.m_ownedActorDatas.Contains(actorData);
		}
		else
		{
			result = false;
		}
		return result;
	}

	public ActorData nextOwnedActorData
	{
		get
		{
			ActorData result = null;
			if (this.m_ownedActorDatas.Count > 0)
			{
				if (this.activeOwnedActorData == null)
				{
					result = this.m_ownedActorDatas[0];
				}
				else
				{
					int num = 0;
					for (int i = 0; i < this.m_ownedActorDatas.Count; i++)
					{
						if (this.m_ownedActorDatas[i] == this.activeOwnedActorData)
						{
							num = i;
							break;
						}
					}
					return this.m_ownedActorDatas[(num + 1) % this.m_ownedActorDatas.Count];
				}
			}
			return result;
		}
	}

	public void SetActiveNextNonConfirmedOwnedActorData()
	{
		if (this.activeOwnedActorData == null)
		{
			this.activeOwnedActorData = this.firstOwnedFriendlyActorData;
		}
		else
		{
			bool flag = false;
			int num = 0;
			int i = 0;
			while (i < this.m_ownedActorDatas.Count)
			{
				if (this.m_ownedActorDatas[i] == this.activeOwnedActorData)
				{
					num = i;
					break;
				}
				else
				{
					i++;
				}
			}
			for (int j = 0; j < this.m_ownedActorDatas.Count; j++)
			{
				int index = (num + j) % this.m_ownedActorDatas.Count;
				ActorData actorData = this.m_ownedActorDatas[index];
				if (actorData != this.activeOwnedActorData && this.activeOwnedActorData != null && actorData.GetTeam() == this.activeOwnedActorData.GetTeam())
				{
					ActorTurnSM component = actorData.GetComponent<ActorTurnSM>();
					if (component.CurrentState != TurnStateEnum.CONFIRMED)
					{
						flag = true;
						this.activeOwnedActorData = actorData;
						break;
					}
				}
			}
			if (!flag)
			{
				for (int k = 0; k < this.m_ownedActorDatas.Count; k++)
				{
					int index2 = (num + k) % this.m_ownedActorDatas.Count;
					ActorData actorData2 = this.m_ownedActorDatas[index2];
					if (actorData2 != this.activeOwnedActorData)
					{
						ActorTurnSM component2 = actorData2.GetComponent<ActorTurnSM>();
						if (component2.CurrentState != TurnStateEnum.CONFIRMED)
						{
							this.activeOwnedActorData = actorData2;
							break;
						}
					}
				}
				return;
			}
			return;
		}
	}

	public bool SetActiveOwnedActor_FCFS(ActorData actor)
	{
		if (actor != null)
		{
			if (this.IsActorDataOwned(actor))
			{
				if (this.activeOwnedActorData != actor)
				{
					this.activeOwnedActorData = actor;
					return true;
				}
			}
		}
		return false;
	}

	public ActorData firstOwnedFriendlyActorData
	{
		get
		{
			ActorData result = null;
			if (this.m_ownedActorDatas.Count > 0 && this.activeOwnedActorData != null)
			{
				foreach (ActorData actorData in this.m_ownedActorDatas)
				{
					if (actorData.GetTeam() == this.activeOwnedActorData.GetTeam())
					{
						result = actorData;
						break;
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
			if (this.activeOwnedActorData != null)
			{
				using (List<ActorData>.Enumerator enumerator = this.m_ownedActorDatas.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ActorData actorData = enumerator.Current;
						if (actorData.GetTeam() != this.activeOwnedActorData.GetTeam())
						{
							return actorData;
						}
					}
				}
			}
			return result;
		}
	}

	internal ActorData POVActorData
	{
		get
		{
			return this.activeOwnedActorData;
		}
	}

	public ActorData activeOwnedActorData
	{
		get
		{
			return this.m_activeOwnedActorData;
		}
		set
		{
			bool flag = this.m_activeOwnedActorData != value;
			bool flag2 = false;
			if (this.m_activeOwnedActorData != null)
			{
				this.m_activeOwnedActorData.OnDeselect();
				bool flag3;
				if (value != null)
				{
					flag3 = (value.GetOpposingTeam() == this.m_activeOwnedActorData.GetTeam());
				}
				else
				{
					flag3 = false;
				}
				flag2 = flag3;
			}
			this.m_activeOwnedActorData = value;
			if (this.m_activeOwnedActorData != null)
			{
				this.m_activeOwnedActorData.OnSelect();
			}
			if (flag)
			{
				if (GameFlowData.s_onActiveOwnedActorChangeHolder != null)
				{
					GameFlowData.s_onActiveOwnedActorChangeHolder(value);
				}
			}
			if (flag2)
			{
				GameEventManager.Get().FireEvent(GameEventManager.EventType.ActiveControlChangedToEnemyTeam, null);
			}
		}
	}

	public string GetActiveOwnedActorDataDebugNameString()
	{
		if (this.activeOwnedActorData)
		{
			return this.activeOwnedActorData.GetDebugName();
		}
		return "(no owned actor)";
	}

	public void RemoveFromTeam(ActorData actorData)
	{
		this.m_teamA.Remove(actorData);
		this.m_teamB.Remove(actorData);
		this.m_teamObjects.Remove(actorData);
		this.m_teamAPlayerAndBots.Remove(actorData);
		this.m_teamBPlayerAndBots.Remove(actorData);
		this.m_teamObjectsPlayerAndBots.Remove(actorData);
	}

	public void AddToTeam(ActorData actorData)
	{
		if (GameplayUtils.IsPlayerControlled(actorData))
		{
			if (actorData.GetTeam() == Team.TeamA)
			{
				if (!this.m_teamAPlayerAndBots.Contains(actorData))
				{
					this.m_teamBPlayerAndBots.Remove(actorData);
					this.m_teamObjectsPlayerAndBots.Remove(actorData);
					this.m_teamAPlayerAndBots.Add(actorData);
					goto IL_119;
				}
			}
			if (actorData.GetTeam() == Team.TeamB && !this.m_teamBPlayerAndBots.Contains(actorData))
			{
				this.m_teamAPlayerAndBots.Remove(actorData);
				this.m_teamObjectsPlayerAndBots.Remove(actorData);
				this.m_teamBPlayerAndBots.Add(actorData);
			}
			else if (actorData.GetTeam() == Team.Objects)
			{
				if (!this.m_teamObjectsPlayerAndBots.Contains(actorData))
				{
					this.m_teamAPlayerAndBots.Remove(actorData);
					this.m_teamBPlayerAndBots.Remove(actorData);
					this.m_teamObjectsPlayerAndBots.Add(actorData);
				}
			}
		}
		IL_119:
		if (actorData.GetTeam() == Team.TeamA)
		{
			if (!this.m_teamA.Contains(actorData))
			{
				this.m_teamB.Remove(actorData);
				this.m_teamObjects.Remove(actorData);
				this.m_teamA.Add(actorData);
				return;
			}
		}
		if (actorData.GetTeam() == Team.TeamB && !this.m_teamB.Contains(actorData))
		{
			this.m_teamA.Remove(actorData);
			this.m_teamObjects.Remove(actorData);
			this.m_teamB.Add(actorData);
		}
		else if (actorData.GetTeam() == Team.Objects)
		{
			if (!this.m_teamObjects.Contains(actorData))
			{
				this.m_teamA.Remove(actorData);
				this.m_teamB.Remove(actorData);
				this.m_teamObjects.Add(actorData);
			}
		}
	}

	private List<ActorData> GetAllActorsOnTeam(Team team)
	{
		List<ActorData> result;
		switch (team)
		{
		case Team.TeamA:
			result = this.m_teamA;
			break;
		case Team.TeamB:
			result = this.m_teamB;
			break;
		case Team.Objects:
			result = this.m_teamObjects;
			break;
		default:
			result = new List<ActorData>();
			break;
		}
		return result;
	}

	private List<ActorData> GetPlayersAndBotsOnTeam(Team team)
	{
		List<ActorData> result;
		switch (team)
		{
		case Team.TeamA:
			result = this.m_teamAPlayerAndBots;
			break;
		case Team.TeamB:
			result = this.m_teamBPlayerAndBots;
			break;
		case Team.Objects:
			result = this.m_teamObjectsPlayerAndBots;
			break;
		default:
			result = new List<ActorData>();
			break;
		}
		return result;
	}

	public List<GameObject> GetPlayers()
	{
		return this.m_players;
	}

	public void AddPlayer(GameObject player)
	{
		this.m_players.Add(player);
		this.SetLocalPlayerData();
	}

	public void RemoveExistingPlayer(GameObject player)
	{
		if (this.m_players.Contains(player))
		{
			this.m_players.Remove(player);
		}
	}

	public List<ActorData> GetActors()
	{
		return this.m_actors;
	}

	public List<ActorData> GetActorsVisibleToActor(ActorData observer, bool targetableOnly = true)
	{
		List<ActorData> list = new List<ActorData>();
		if (observer != null)
		{
			using (List<ActorData>.Enumerator enumerator = this.m_actors.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ActorData actorData = enumerator.Current;
					if (!actorData.IsDead())
					{
						if (actorData.IsActorVisibleToActor(observer, false))
						{
							if (targetableOnly)
							{
								if (actorData.IgnoreForAbilityHits)
								{
									continue;
								}
							}
							list.Add(actorData);
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
		for (int i = 0; i < this.m_actors.Count; i++)
		{
			if (this.m_actors[i].PlayerIndex == playerIndex)
			{
				list.Add(this.m_actors[i]);
			}
		}
		return list;
	}

	public void AddActor(ActorData actor)
	{
		Log.Info("Registering actor {0}", new object[]
		{
			actor
		});
		this.m_actors.Add(actor);
		if (NetworkServer.active)
		{
			if (GameFlowData.s_onAddActorHolder != null)
			{
				GameFlowData.s_onAddActorHolder(actor);
			}
		}
	}

	internal ActorData FindActorByActorIndex(int actorIndex)
	{
		ActorData actorData = null;
		for (int i = 0; i < this.m_actors.Count; i++)
		{
			ActorData actorData2 = this.m_actors[i];
			if (actorData2.ActorIndex == actorIndex)
			{
				actorData = actorData2;
				break;
			}
		}
		if (actorData == null)
		{
			if (actorIndex > 0 && this.CurrentTurn > 0)
			{
				if (GameManager.Get() != null)
				{
					if (GameManager.Get().GameConfig != null)
					{
						GameType gameType = GameManager.Get().GameConfig.GameType;
						if (gameType != GameType.Tutorial)
						{
							Log.Warning("Failed to find actor index {0}", new object[]
							{
										actorIndex
							});
						}
					}
				}
			}
		}
		return actorData;
	}

	internal ActorData FindActorByPlayerIndex(int playerIndex)
	{
		for (int i = 0; i < this.m_players.Count; i++)
		{
			ActorData component = this.m_players[i].GetComponent<ActorData>();
			if (component != null)
			{
				if (component.PlayerIndex == playerIndex)
				{
					return component;
				}
			}
		}
		Log.Warning("Failed to find player index {0}", new object[]
		{
			playerIndex
		});
		return null;
	}

	internal ActorData FindActorByPlayer(Player player)
	{
		for (int i = 0; i < this.m_actors.Count; i++)
		{
			PlayerData playerData = this.m_actors[i].PlayerData;
			if (playerData != null && playerData.GetPlayer() == player)
			{
				return this.m_actors[i];
			}
		}
		return null;
	}

	internal List<ActorData> GetAllTeamMembers(Team team)
	{
		return this.GetAllActorsOnTeam(team);
	}

	internal List<ActorData> GetPlayerAndBotTeamMembers(Team team)
	{
		return this.GetPlayersAndBotsOnTeam(team);
	}

	public void RemoveReferencesToDestroyedActor(ActorData actor)
	{
		if (actor != null)
		{
			if (this.m_teamAPlayerAndBots.Contains(actor))
			{
				this.m_teamAPlayerAndBots.Remove(actor);
			}
			if (this.m_teamBPlayerAndBots.Contains(actor))
			{
				this.m_teamBPlayerAndBots.Remove(actor);
			}
			if (this.m_teamObjectsPlayerAndBots.Contains(actor))
			{
				this.m_teamObjectsPlayerAndBots.Remove(actor);
			}
			if (this.m_teamA.Contains(actor))
			{
				this.m_teamA.Remove(actor);
			}
			if (this.m_teamB.Contains(actor))
			{
				this.m_teamB.Remove(actor);
			}
			if (this.m_teamObjects.Contains(actor))
			{
				this.m_teamObjects.Remove(actor);
			}
			if (this.m_players.Contains(actor.gameObject))
			{
				this.m_players.Remove(actor.gameObject);
			}
			if (this.m_actors.Contains(actor))
			{
				this.m_actors.Remove(actor);
			}
			this.SetLocalPlayerData();
			if (GameFlowData.s_onRemoveActorHolder != null)
			{
				GameFlowData.s_onRemoveActorHolder(actor);
			}
		}
		else
		{
			Log.Error("Trying to destroy a null actor.", new object[0]);
		}
	}

	public int CurrentTurn
	{
		get
		{
			return this.m_currentTurn;
		}
	}

	internal GameState gameState
	{
		get
		{
			return this.m_gameState;
		}
		set
		{
			if (this.m_gameState != value)
			{
				this.SetGameState(value);
			}
		}
	}

	[ClientRpc]
	private void RpcUpdateTimeRemaining(float timeRemaining)
	{
		if (!NetworkServer.active)
		{
			this.m_timeRemainingInDecision = timeRemaining - 1f;
		}
	}

	public float GetTimeRemainingInDecision()
	{
		return this.m_timeRemainingInDecision;
	}

	private void SetGameState(GameState value)
	{
		this.Networkm_gameState = value;
		this.m_timeInState = 0f;
		this.m_timeInStateUnscaled = 0f;
		Log.Info("Game state: {0}", new object[]
		{
			value.ToString()
		});
		switch (this.m_gameState)
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
			this.m_deploymentStartTime = Time.realtimeSinceStartup;
			break;
		case GameState.BothTeams_Decision:
			if (NetworkServer.active)
			{
				this.IncrementTurn();
			}
			if (this.CurrentTurn == 1)
			{
				this.m_matchStartTime = Time.realtimeSinceStartup;
			}
			this.ResetOwnedActorDataToFirst();
			this.m_timeInDecision = 0f;
			break;
		}
		if (GameFlowData.s_onGameStateChangedHolder != null)
		{
			GameFlowData.s_onGameStateChangedHolder(this.m_gameState);
		}
	}

	public int GetNumAvailableCharacterResourceLinks()
	{
		return this.m_availableCharacterResourceLinkPrefabs.Length;
	}

	public string GetFirstAvailableCharacterResourceLinkName()
	{
		GameObject gameObject = this.m_availableCharacterResourceLinkPrefabs[0];
		CharacterResourceLink characterResourceLink;
		if (gameObject == null)
		{
			characterResourceLink = null;
		}
		else
		{
			characterResourceLink = gameObject.GetComponent<CharacterResourceLink>();
		}
		CharacterResourceLink characterResourceLink2 = characterResourceLink;
		if (characterResourceLink2 != null)
		{
			return characterResourceLink2.m_displayName;
		}
		return string.Empty;
	}

	private void IncrementTurn()
	{
		this.m_timeInDecision = 0f;
		this.Networkm_currentTurn = this.m_currentTurn + 1;
		this.NotifyOnTurnTick();
		Log.Info("Turn: {0}", new object[]
		{
			this.CurrentTurn
		});
		if (Board.Get() != null)
		{
			Board.Get().MarkForUpdateValidSquares(true);
		}
	}

	private void NotifyOnTurnTick()
	{
		if (TeamSensitiveDataMatchmaker.Get() != null)
		{
			TeamSensitiveDataMatchmaker.Get().SetTeamSensitiveDataForUnhandledActors();
		}
		GameEventManager.Get().FireEvent(GameEventManager.EventType.TurnTick, null);
		this.ShowIntervanStatusNotifications();
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
			SequenceManager.Get().OnTurnStart(this.m_currentTurn);
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
				PowerUp.IPowerUpListener powerUpListener = enumerator.Current;
				powerUpListener.OnTurnTick();
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
		this.m_timeRemainingInDecision = GameFlowData.Get().m_turnTime;
		foreach (ActorData actorData in this.GetActors())
		{
			actorData.OnTurnTick();
		}
		if (ObjectivePoints.Get() != null)
		{
			ObjectivePoints.Get().OnTurnTick();
		}
		if (ClientAbilityResults.LogMissingSequences)
		{
			Log.Warning("Turn Start: <color=magenta>" + GameFlowData.Get().CurrentTurn + "</color>", new object[0]);
		}
		if (ControlpadGameplay.Get() != null)
		{
			ControlpadGameplay.Get().OnTurnTick();
		}
	}

	public bool HasPotentialGameMutatorVisibilityChanges(bool onTurnStart)
	{
		bool flag = false;
		GameplayMutators gameplayMutators = GameplayMutators.Get();
		if (gameplayMutators != null && this.CurrentTurn > 1)
		{
			int i = 0;
			while (i < gameplayMutators.m_alwaysOnStatuses.Count)
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
						goto IL_13F;
					}
				}
				else
				{
					GameplayMutators.StatusInterval statusInterval = gameplayMutators.m_alwaysOnStatuses[i];
					if (statusInterval.m_statusType == StatusType.Blind)
					{
						goto IL_8B;
					}
					if (statusInterval.m_statusType == StatusType.InvisibleToEnemies)
					{
						goto IL_8B;
					}
					if (statusInterval.m_statusType == StatusType.Revealed)
					{
						for (;;)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							goto IL_8B;
						}
					}
					IL_111:
					i++;
					continue;
					IL_8B:
					if (onTurnStart)
					{
						if (GameplayMutators.IsStatusActive(statusInterval.m_statusType, this.CurrentTurn, GameplayMutators.ActionPhaseCheckMode.Default) != GameplayMutators.IsStatusActive(statusInterval.m_statusType, this.CurrentTurn - 1, GameplayMutators.ActionPhaseCheckMode.Default))
						{
							flag = true;
							goto IL_111;
						}
					}
					if (onTurnStart)
					{
						goto IL_111;
					}
					if (GameplayMutators.IsStatusActive(statusInterval.m_statusType, this.CurrentTurn, GameplayMutators.ActionPhaseCheckMode.Abilities) != GameplayMutators.IsStatusActive(statusInterval.m_statusType, this.CurrentTurn, GameplayMutators.ActionPhaseCheckMode.Movement))
					{
						flag = true;
						goto IL_111;
					}
					goto IL_111;
				}
			}
			IL_13F:
			int j = 0;
			while (j < gameplayMutators.m_statusSuppression.Count)
			{
				if (flag)
				{
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						return flag;
					}
				}
				else
				{
					GameplayMutators.StatusInterval statusInterval2 = gameplayMutators.m_statusSuppression[j];
					if (statusInterval2.m_statusType == StatusType.Blind)
					{
						goto IL_18C;
					}
					if (statusInterval2.m_statusType == StatusType.InvisibleToEnemies)
					{
						goto IL_18C;
					}
					if (statusInterval2.m_statusType == StatusType.Revealed)
					{
						for (;;)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							goto IL_18C;
						}
					}
					IL_212:
					j++;
					continue;
					IL_18C:
					if (onTurnStart)
					{
						if (GameplayMutators.IsStatusSuppressed(statusInterval2.m_statusType, this.CurrentTurn, GameplayMutators.ActionPhaseCheckMode.Default) != GameplayMutators.IsStatusSuppressed(statusInterval2.m_statusType, this.CurrentTurn - 1, GameplayMutators.ActionPhaseCheckMode.Default))
						{
							flag = true;
							goto IL_212;
						}
					}
					if (onTurnStart)
					{
						goto IL_212;
					}
					if (GameplayMutators.IsStatusSuppressed(statusInterval2.m_statusType, this.CurrentTurn, GameplayMutators.ActionPhaseCheckMode.Abilities) != GameplayMutators.IsStatusSuppressed(statusInterval2.m_statusType, this.CurrentTurn, GameplayMutators.ActionPhaseCheckMode.Movement))
					{
						flag = true;
						goto IL_212;
					}
					goto IL_212;
				}
			}
		}
		return flag;
	}

	private void ShowIntervanStatusNotifications()
	{
		if (NetworkClient.active)
		{
			if (!(HUD_UI.Get() == null))
			{
				GameplayMutators gameplayMutators = GameplayMutators.Get();
				if (gameplayMutators != null)
				{
					for (int i = 0; i < gameplayMutators.m_alwaysOnStatuses.Count; i++)
					{
						GameplayMutators.StatusInterval statusInterval = gameplayMutators.m_alwaysOnStatuses[i];
						if (statusInterval.m_delayTillStartOfMovement)
						{
							bool flag = GameplayMutators.IsStatusActive(statusInterval.m_statusType, this.CurrentTurn, GameplayMutators.ActionPhaseCheckMode.Abilities);
							bool flag2 = GameplayMutators.IsStatusActive(statusInterval.m_statusType, this.CurrentTurn, GameplayMutators.ActionPhaseCheckMode.Movement);
							if (!flag && flag2 && !string.IsNullOrEmpty(statusInterval.m_activateNotificationTurnBefore))
							{
								InterfaceManager.Get().DisplayAlert(StringUtil.TR_IfHasContext(statusInterval.m_activateNotificationTurnBefore), Color.cyan, 5f, true, 1);
							}
							bool flag3 = GameplayMutators.IsStatusActive(statusInterval.m_statusType, this.CurrentTurn + 1, GameplayMutators.ActionPhaseCheckMode.Abilities);
							if (flag2 && !flag3)
							{
								if (!string.IsNullOrEmpty(statusInterval.m_offNotificationTurnBefore))
								{
									InterfaceManager.Get().DisplayAlert(StringUtil.TR_IfHasContext(statusInterval.m_offNotificationTurnBefore), Color.cyan, 5f, true, 1);
								}
							}
						}
						else
						{
							bool flag4 = GameplayMutators.IsStatusActive(statusInterval.m_statusType, this.CurrentTurn, GameplayMutators.ActionPhaseCheckMode.Any);
							bool flag5 = GameplayMutators.IsStatusActive(statusInterval.m_statusType, this.CurrentTurn + 1, GameplayMutators.ActionPhaseCheckMode.Any);
							if (flag4 && !flag5 && !string.IsNullOrEmpty(statusInterval.m_offNotificationTurnBefore))
							{
								InterfaceManager.Get().DisplayAlert(StringUtil.TR_IfHasContext(statusInterval.m_offNotificationTurnBefore), Color.cyan, 5f, true, 1);
							}
							else if (!flag4)
							{
								if (flag5)
								{
									if (!string.IsNullOrEmpty(statusInterval.m_activateNotificationTurnBefore))
									{
										InterfaceManager.Get().DisplayAlert(StringUtil.TR_IfHasContext(statusInterval.m_activateNotificationTurnBefore), Color.cyan, 5f, true, 1);
									}
								}
							}
						}
					}
				}
				return;
			}
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
		foreach (SatelliteController satelliteController in components)
		{
			satelliteController.OnActorDeath();
		}
		GameEventManager.Get().FireEvent(GameEventManager.EventType.PostCharacterDeath, new GameEventManager.CharacterDeathEventArgs
		{
			deadCharacter = actor
		});
	}

	public float GetTimeInState()
	{
		return this.m_timeInState;
	}

	public float GetTimeInDecision()
	{
		return this.m_timeInDecision;
	}

	public float GetTimeSinceDeployment()
	{
		return Time.realtimeSinceStartup - this.m_deploymentStartTime;
	}

	public float GetTimeLeftInTurn()
	{
		float num;
		if (this.IsInDecisionState())
		{
			num = this.m_turnTime - this.GetTimeInDecision();
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
		return Time.realtimeSinceStartup - this.m_matchStartTime;
	}

	private void Update()
	{
		this.symbol_000E();
		if (!this.m_pause)
		{
			this.m_timeInState += Time.deltaTime;
			this.m_timeInStateUnscaled += Time.unscaledDeltaTime;
			this.m_timeInDecision += Time.deltaTime;
		}
		if (AppState.GetCurrent() != AppState_GameTeardown.Get())
		{
			this.UpdateTimeRemaining();
		}
	}

	private void UpdateTimeRemaining()
	{
		bool flag = GameFlowData.Get().IsInDecisionState();
		if (!this.m_pause && flag)
		{
			if (this.m_timeRemainingInDecision >= -this.m_timeRemainingInDecisionOverflow)
			{
				this.m_timeRemainingInDecision -= Time.deltaTime;
				if (this.m_timeRemainingInDecision < -this.m_timeRemainingInDecisionOverflow)
				{
					this.m_timeRemainingInDecision = -this.m_timeRemainingInDecisionOverflow;
				}
			}
		}
	}

	private void symbol_000E()
	{
	}

	public bool IsOwnerTargeting()
	{
		bool result = false;
		ActorData activeOwnedActorData = this.activeOwnedActorData;
		if (activeOwnedActorData != null)
		{
			ActorTurnSM component = activeOwnedActorData.GetComponent<ActorTurnSM>();
			if (component)
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
		GameState gameState = this.m_gameState;
		bool result;
		if (gameState != GameState.BothTeams_Decision)
		{
			result = false;
		}
		else
		{
			result = true;
		}
		return result;
	}

	public bool IsTeamsTurn(Team team)
	{
		bool flag = false;
		if (!this.IsTeamADecision())
		{
			if (!this.IsTeamAResolving())
			{
				goto IL_3A;
			}
		}
		flag |= (team == Team.TeamA);
		IL_3A:
		if (!this.IsTeamBDecision())
		{
			if (!this.IsTeamBResolving())
			{
				return flag;
			}
		}
		flag |= (team == Team.TeamB);
		return flag;
	}

	public bool IsOwnedActorsTurn()
	{
		bool result = false;
		if (this.activeOwnedActorData != null)
		{
			result = this.IsTeamsTurn(this.activeOwnedActorData.GetTeam());
		}
		return result;
	}

	public bool IsInResolveState()
	{
		GameState gameState = this.m_gameState;
		bool result;
		if (gameState != GameState.BothTeams_Resolve)
		{
			result = false;
		}
		else
		{
			result = true;
		}
		return result;
	}

	public bool IsTeamAResolving()
	{
		return this.IsInResolveState();
	}

	public bool IsTeamBResolving()
	{
		return this.IsInResolveState();
	}

	public bool IsTeamADecision()
	{
		GameState gameState = this.m_gameState;
		bool result;
		if (gameState != GameState.BothTeams_Decision)
		{
			result = false;
		}
		else
		{
			result = true;
		}
		return result;
	}

	public bool IsTeamBDecision()
	{
		GameState gameState = this.m_gameState;
		bool result;
		if (gameState != GameState.BothTeams_Decision)
		{
			result = false;
		}
		else
		{
			result = true;
		}
		return result;
	}

	public bool IsPhase(int phase)
	{
		GameState gameState = this.m_gameState;
		bool result;
		if (gameState != GameState.BothTeams_Decision)
		{
			result = (gameState == GameState.BothTeams_Resolve && phase == 1);
		}
		else
		{
			result = (phase == 1);
		}
		return result;
	}

	public void SetSelectedTeam(int team)
	{
		this.m_selectedTeam = (Team)team;
	}

	public bool ShouldForceResolveTimeout()
	{
		bool flag = this.m_timeInStateUnscaled > this.m_resolveTimeoutLimit;
		bool flag2;
		if (DebugParameters.Get() != null)
		{
			flag2 = DebugParameters.Get().GetParameterAsBool("DisableResolveFailsafe");
		}
		else
		{
			flag2 = false;
		}
		bool flag3 = flag2;
		return flag && !flag3;
	}

	public bool PlayersMustUseTimeBank()
	{
		bool result;
		if (this.IsInDecisionState())
		{
			result = (this.GetTimeInState() > this.m_turnTime);
		}
		else
		{
			result = false;
		}
		return result;
	}

	public bool WillEnterTimebankMode()
	{
		return this.m_willEnterTimebankMode;
	}

	public bool PreventAutoLockInOnTimeout()
	{
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
		GameFlowData gameFlowData = GameFlowData.Get();
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
		GameFlowData gameFlowData = GameFlowData.Get();
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

	public void DistributeRewardForKill(ActorData killedActor)
	{
		if (!GameplayUtils.IsPlayerControlled(killedActor))
		{
			if (!GameplayUtils.IsMinion(killedActor))
			{
				return;
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
		if (num > 0)
		{
			if (flag)
			{
				List<ActorData> contributorsToKill = this.GetContributorsToKill(killedActor, false);
				if (contributorsToKill.Count > 0)
				{
					this.RewardContributorsToKill(contributorsToKill, num);
				}
				else if (GameplayData.Get().m_participationlessBountiesGoToTeam)
				{
					this.RewardTeam(killedActor.GetOpposingTeam(), num);
				}
			}
			else
			{
				this.RewardTeam(killedActor.GetOpposingTeam(), num);
			}
		}
	}

	public int GetTotalDeathsOnTurnStart(Team team)
	{
		int num = 0;
		List<ActorData> allTeamMembers = this.GetAllTeamMembers(team);
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
		}
		return result;
	}

	public List<ActorData> GetContributorsToKillOnClient(ActorData killedActor, bool onlyDirectDamagers = false)
	{
		List<ActorData> list = new List<ActorData>();
		List<ActorData> allTeamMembers = this.GetAllTeamMembers(killedActor.GetOpposingTeam());
		if (allTeamMembers != null)
		{
			ActorBehavior actorBehavior = killedActor.GetActorBehavior();
			foreach (ActorData actorData in allTeamMembers)
			{
				if (!GameplayUtils.IsPlayerControlled(actorData))
				{
				}
				else if (actorBehavior.Client_ActorDamagedOrDebuffedByActor(actorData))
				{
					list.Add(actorData);
					break;
				}
			}
			if (!onlyDirectDamagers)
			{
				List<ActorData> list2 = new List<ActorData>();
				using (List<ActorData>.Enumerator enumerator2 = allTeamMembers.GetEnumerator())
				{
					IL_17A:
					while (enumerator2.MoveNext())
					{
						ActorData actorData2 = enumerator2.Current;
						if (list.Contains(actorData2))
						{
						}
						else if (!GameplayUtils.IsPlayerControlled(actorData2))
						{
						}
						else
						{
							using (List<ActorData>.Enumerator enumerator3 = list.GetEnumerator())
							{
								while (enumerator3.MoveNext())
								{
									ActorData actorData3 = enumerator3.Current;
									if (list2.Contains(actorData2))
									{
									}
									else
									{
										ActorBehavior actorBehavior2 = actorData3.GetActorBehavior();
										if (!actorBehavior2.Client_ActorHealedOrBuffedByActor(actorData2))
										{
											continue;
										}
										list2.Add(actorData2);
									}
									goto IL_17A;
								}
							}
						}
					}
				}
				using (List<ActorData>.Enumerator enumerator4 = list2.GetEnumerator())
				{
					while (enumerator4.MoveNext())
					{
						ActorData item = enumerator4.Current;
						list.Add(item);
					}
				}
			}
		}
		return list;
	}

	public void RewardContributorsToKill(List<ActorData> participants, int baseCreditsReward)
	{
		if (participants.Count > 0)
		{
			int num = baseCreditsReward / participants.Count;
			int num2 = Mathf.FloorToInt(GameplayData.Get().m_creditBonusFractionPerExtraPlayer * (float)(participants.Count - 1) * (float)num);
			int numCredits = num + num2;
			using (List<ActorData>.Enumerator enumerator = participants.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ActorData actorData = enumerator.Current;
					ItemData component = actorData.GetComponent<ItemData>();
					if (component != null)
					{
						component.GiveCredits(numCredits);
					}
				}
			}
		}
	}

	public void RewardTeam(Team teamToReward, int creditsReward)
	{
		List<ActorData> allTeamMembers = this.GetAllTeamMembers(teamToReward);
		if (allTeamMembers != null)
		{
			using (List<ActorData>.Enumerator enumerator = allTeamMembers.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ActorData actorData = enumerator.Current;
					ItemData component = actorData.GetComponent<ItemData>();
					if (component != null)
					{
						component.GiveCredits(creditsReward);
					}
				}
			}
		}
	}

	public int GetDeathCountOfTeam(Team team)
	{
		int num = 0;
		for (int i = 0; i < this.m_actors.Count; i++)
		{
			ActorData actorData = this.m_actors[i];
			if (actorData != null)
			{
				if (actorData.GetTeam() == team)
				{
					if (actorData.GetActorBehavior() != null)
					{
						num += actorData.GetActorBehavior().totalDeaths;
					}
				}
			}
		}
		return num;
	}

	public int GetTotalTeamDamageReceived(Team team)
	{
		int num = 0;
		for (int i = 0; i < this.m_actors.Count; i++)
		{
			ActorData actorData = this.m_actors[i];
			if (actorData != null && actorData.GetTeam() == team && actorData.GetActorBehavior() != null)
			{
				num += actorData.GetActorBehavior().totalPlayerDamageReceived;
			}
		}
		return num;
	}

	public void UpdateCoverFromBarriersForAllActors()
	{
		for (int i = 0; i < this.m_actors.Count; i++)
		{
			ActorData actorData = this.m_actors[i];
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
		}
	}

	public void SetLocalPlayerData()
	{
		this.m_localPlayerData = null;
		if (GameFlow.Get() != null)
		{
			foreach (GameObject gameObject in this.m_players)
			{
				if (gameObject != null)
				{
					PlayerData component = gameObject.GetComponent<PlayerData>();
					if (component != null)
					{
						PlayerDetails playerDetails = null;
						if (GameFlow.Get().playerDetails.TryGetValue(component.GetPlayer(), out playerDetails))
						{
							if (playerDetails.IsLocal())
							{
								this.m_localPlayerData = component;
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

	public bool Networkm_pause
	{
		get
		{
			return this.m_pause;
		}
		[param: In]
		set
		{
			base.SetSyncVar<bool>(value, ref this.m_pause, 1U);
		}
	}

	public bool Networkm_pausedForDebugging
	{
		get
		{
			return this.m_pausedForDebugging;
		}
		[param: In]
		set
		{
			base.SetSyncVar<bool>(value, ref this.m_pausedForDebugging, 2U);
		}
	}

	public bool Networkm_pausedByPlayerRequest
	{
		get
		{
			return this.m_pausedByPlayerRequest;
		}
		[param: In]
		set
		{
			base.SetSyncVar<bool>(value, ref this.m_pausedByPlayerRequest, 4U);
		}
	}

	public bool Networkm_pausedForSinglePlayer
	{
		get
		{
			return this.m_pausedForSinglePlayer;
		}
		[param: In]
		set
		{
			base.SetSyncVar<bool>(value, ref this.m_pausedForSinglePlayer, 8U);
		}
	}

	public ResolutionPauseState Networkm_resolutionPauseState
	{
		get
		{
			return this.m_resolutionPauseState;
		}
		[param: In]
		set
		{
			base.SetSyncVar<ResolutionPauseState>(value, ref this.m_resolutionPauseState, 0x10U);
		}
	}

	public float Networkm_startTime
	{
		get
		{
			return this.m_startTime;
		}
		[param: In]
		set
		{
			uint dirtyBit = 0x20U;
			if (NetworkServer.localClientActive)
			{
				if (!base.syncVarHookGuard)
				{
					base.syncVarHookGuard = true;
					this.HookSetStartTime(value);
					base.syncVarHookGuard = false;
				}
			}
			base.SetSyncVar<float>(value, ref this.m_startTime, dirtyBit);
		}
	}

	public float Networkm_deploymentTime
	{
		get
		{
			return this.m_deploymentTime;
		}
		[param: In]
		set
		{
			uint dirtyBit = 0x40U;
			if (NetworkServer.localClientActive)
			{
				if (!base.syncVarHookGuard)
				{
					base.syncVarHookGuard = true;
					this.HookSetDeploymentTime(value);
					base.syncVarHookGuard = false;
				}
			}
			base.SetSyncVar<float>(value, ref this.m_deploymentTime, dirtyBit);
		}
	}

	public float Networkm_turnTime
	{
		get
		{
			return this.m_turnTime;
		}
		[param: In]
		set
		{
			uint dirtyBit = 0x80U;
			if (NetworkServer.localClientActive)
			{
				if (!base.syncVarHookGuard)
				{
					base.syncVarHookGuard = true;
					this.HookSetTurnTime(value);
					base.syncVarHookGuard = false;
				}
			}
			base.SetSyncVar<float>(value, ref this.m_turnTime, dirtyBit);
		}
	}

	public float Networkm_maxTurnTime
	{
		get
		{
			return this.m_maxTurnTime;
		}
		[param: In]
		set
		{
			uint dirtyBit = 0x100U;
			if (NetworkServer.localClientActive)
			{
				if (!base.syncVarHookGuard)
				{
					base.syncVarHookGuard = true;
					this.HookSetMaxTurnTime(value);
					base.syncVarHookGuard = false;
				}
			}
			base.SetSyncVar<float>(value, ref this.m_maxTurnTime, dirtyBit);
		}
	}

	public float Networkm_timeRemainingInDecisionOverflow
	{
		get
		{
			return this.m_timeRemainingInDecisionOverflow;
		}
		[param: In]
		set
		{
			base.SetSyncVar<float>(value, ref this.m_timeRemainingInDecisionOverflow, 0x200U);
		}
	}

	public bool Networkm_willEnterTimebankMode
	{
		get
		{
			return this.m_willEnterTimebankMode;
		}
		[param: In]
		set
		{
			base.SetSyncVar<bool>(value, ref this.m_willEnterTimebankMode, 0x400U);
		}
	}

	public int Networkm_currentTurn
	{
		get
		{
			return this.m_currentTurn;
		}
		[param: In]
		set
		{
			uint dirtyBit = 0x800U;
			if (NetworkServer.localClientActive)
			{
				if (!base.syncVarHookGuard)
				{
					base.syncVarHookGuard = true;
					this.HookSetCurrentTurn(value);
					base.syncVarHookGuard = false;
				}
			}
			base.SetSyncVar<int>(value, ref this.m_currentTurn, dirtyBit);
		}
	}

	public GameState Networkm_gameState
	{
		get
		{
			return this.m_gameState;
		}
		[param: In]
		set
		{
			uint dirtyBit = 0x1000U;
			if (NetworkServer.localClientActive)
			{
				if (!base.syncVarHookGuard)
				{
					base.syncVarHookGuard = true;
					this.HookSetGameState(value);
					base.syncVarHookGuard = false;
				}
			}
			base.SetSyncVar<GameState>(value, ref this.m_gameState, dirtyBit);
		}
	}

	protected static void InvokeRpcRpcUpdateTimeRemaining(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcUpdateTimeRemaining called on server.");
			return;
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
		networkWriter.Write(0);
		networkWriter.Write((short)((ushort)2));
		networkWriter.WritePackedUInt32((uint)GameFlowData.kRpcRpcUpdateTimeRemaining);
		networkWriter.Write(base.GetComponent<NetworkIdentity>().netId);
		networkWriter.Write(timeRemaining);
		this.SendRPCInternal(networkWriter, 0, "RpcUpdateTimeRemaining");
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		if (forceAll)
		{
			writer.Write(this.m_pause);
			writer.Write(this.m_pausedForDebugging);
			writer.Write(this.m_pausedByPlayerRequest);
			writer.Write(this.m_pausedForSinglePlayer);
			writer.Write((int)this.m_resolutionPauseState);
			writer.Write(this.m_startTime);
			writer.Write(this.m_deploymentTime);
			writer.Write(this.m_turnTime);
			writer.Write(this.m_maxTurnTime);
			writer.Write(this.m_timeRemainingInDecisionOverflow);
			writer.Write(this.m_willEnterTimebankMode);
			writer.WritePackedUInt32((uint)this.m_currentTurn);
			writer.Write((int)this.m_gameState);
			return true;
		}
		bool flag = false;
		if ((base.syncVarDirtyBits & 1U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.Write(this.m_pause);
		}
		if ((base.syncVarDirtyBits & 2U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.Write(this.m_pausedForDebugging);
		}
		if ((base.syncVarDirtyBits & 4U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.Write(this.m_pausedByPlayerRequest);
		}
		if ((base.syncVarDirtyBits & 8U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.Write(this.m_pausedForSinglePlayer);
		}
		if ((base.syncVarDirtyBits & 0x10U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.Write((int)this.m_resolutionPauseState);
		}
		if ((base.syncVarDirtyBits & 0x20U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.Write(this.m_startTime);
		}
		if ((base.syncVarDirtyBits & 0x40U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.Write(this.m_deploymentTime);
		}
		if ((base.syncVarDirtyBits & 0x80U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.Write(this.m_turnTime);
		}
		if ((base.syncVarDirtyBits & 0x100U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.Write(this.m_maxTurnTime);
		}
		if ((base.syncVarDirtyBits & 0x200U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.Write(this.m_timeRemainingInDecisionOverflow);
		}
		if ((base.syncVarDirtyBits & 0x400U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.Write(this.m_willEnterTimebankMode);
		}
		if ((base.syncVarDirtyBits & 0x800U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)this.m_currentTurn);
		}
		if ((base.syncVarDirtyBits & 0x1000U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.Write((int)this.m_gameState);
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
			this.m_pause = reader.ReadBoolean();
			this.m_pausedForDebugging = reader.ReadBoolean();
			this.m_pausedByPlayerRequest = reader.ReadBoolean();
			this.m_pausedForSinglePlayer = reader.ReadBoolean();
			this.m_resolutionPauseState = (ResolutionPauseState)reader.ReadInt32();
			this.m_startTime = reader.ReadSingle();
			this.m_deploymentTime = reader.ReadSingle();
			this.m_turnTime = reader.ReadSingle();
			this.m_maxTurnTime = reader.ReadSingle();
			this.m_timeRemainingInDecisionOverflow = reader.ReadSingle();
			this.m_willEnterTimebankMode = reader.ReadBoolean();
			this.m_currentTurn = (int)reader.ReadPackedUInt32();
			this.m_gameState = (GameState)reader.ReadInt32();
			return;
		}
		int num = (int)reader.ReadPackedUInt32();
		if ((num & 1) != 0)
		{
			this.m_pause = reader.ReadBoolean();
		}
		if ((num & 2) != 0)
		{
			this.m_pausedForDebugging = reader.ReadBoolean();
		}
		if ((num & 4) != 0)
		{
			this.m_pausedByPlayerRequest = reader.ReadBoolean();
		}
		if ((num & 8) != 0)
		{
			this.m_pausedForSinglePlayer = reader.ReadBoolean();
		}
		if ((num & 0x10) != 0)
		{
			this.m_resolutionPauseState = (ResolutionPauseState)reader.ReadInt32();
		}
		if ((num & 0x20) != 0)
		{
			this.HookSetStartTime(reader.ReadSingle());
		}
		if ((num & 0x40) != 0)
		{
			this.HookSetDeploymentTime(reader.ReadSingle());
		}
		if ((num & 0x80) != 0)
		{
			this.HookSetTurnTime(reader.ReadSingle());
		}
		if ((num & 0x100) != 0)
		{
			this.HookSetMaxTurnTime(reader.ReadSingle());
		}
		if ((num & 0x200) != 0)
		{
			this.m_timeRemainingInDecisionOverflow = reader.ReadSingle();
		}
		if ((num & 0x400) != 0)
		{
			this.m_willEnterTimebankMode = reader.ReadBoolean();
		}
		if ((num & 0x800) != 0)
		{
			this.HookSetCurrentTurn((int)reader.ReadPackedUInt32());
		}
		if ((num & 0x1000) != 0)
		{
			this.HookSetGameState((GameState)reader.ReadInt32());
		}
	}
}

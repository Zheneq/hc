// ROGUES
// SERVER
using System.Collections.Generic;
using System.Linq;
//using Mirror;
using Theatrics;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class TheatricsManager : NetworkBehaviour, IGameEventListener
{
	public bool m_allowAbilityAnimationInterruptHitReaction = true;
	public int m_numClientPhaseTimeoutsUntilForceDisconnect = 3;

	// reactor
	public const float c_timeoutAdvancePhaseSlowClient = 1.3f;
	// rogues
	//public float m_timeoutAdvancePhaseSlowClient = 5f;

	public const float c_maxPhaseAdvanceTimeoutDuration = 45f;

	[Separator("Ragdoll Force Settings", true)]
	public float m_ragdollImpactForce = 15f;

	// removed in rogues
	[Header("-- Whether to apply force only to single joint")]
	public bool m_ragdollApplyForceOnSingleJointOnly;

	private Turn m_turn = new Turn();
	private AbilityPriority m_phaseToUpdate = AbilityPriority.INVALID;
	private int m_turnToUpdate;
	private HashSet<long> m_playerConnectionIdsInUpdatePhase = new HashSet<long>();
	private int m_numConnectionIdsAddedForPhase;
	private float m_phaseStartTime;
	private AbilityPriority m_lastPhaseEnded = AbilityPriority.INVALID;
	
#if SERVER
	// added in rogues
	private float m_timeToTimeoutPhase = float.MaxValue;
	private Dictionary<long, int> m_connectionIdsToNumPhaseTimeouts = new Dictionary<long, int>();
#endif

	private static TheatricsManager s_instance;

	// removed in rogues
	private SerializeHelper m_serializeHelper = new SerializeHelper();
	internal string m_debugSerializationInfoStr = "";

	internal const string c_actorAnimDebugHeader = "<color=cyan>Theatrics: </color>";

    internal static bool DebugTraceExecution => false;
    internal static bool TraceTheatricsSerialization => false;

	internal static TheatricsManager Get()
	{
		return s_instance;
	}

	public static float GetRagdollImpactForce()
	{
		return s_instance != null ? s_instance.m_ragdollImpactForce : 15f;
	}

    // removed in rogues
    public static bool RagdollOnlyApplyForceAtSingleJoint()
	{
		return s_instance != null && s_instance.m_ragdollApplyForceOnSingleJointOnly;
	}

	// added in rogues
#if SERVER
	public int GetSetBoundCount()
	{
		return m_turn.m_cameraBoundSetCount;
	}
#endif

	protected void Awake()
	{
		s_instance = this;
		DontDestroyOnLoad(gameObject);
	}

	private void Start()
	{
		MyNetworkManager.Get().m_OnServerDisconnect += OnServerDisconnect;
		GameEventManager.Get().AddListener(this, GameEventManager.EventType.UIPhaseStartedMovement);
	}

	private void OnDestroy()
	{
		MyNetworkManager.Get().m_OnServerDisconnect -= OnServerDisconnect;
		GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.UIPhaseStartedMovement);
		s_instance = null;
	}

	internal void OnTurnTick()
	{
		m_lastPhaseEnded = AbilityPriority.INVALID;
	}


	// server-only
#if SERVER
	[Server]
	internal void InitTurn()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void TheatricsManager::InitTurn()' called on client");
			return;
		}
		m_turnToUpdate = GameFlowData.Get().CurrentTurn;
		m_turn = new Turn(GameFlowData.Get().CurrentTurn);
	}
#endif

	// server-only
#if SERVER
	[Server]
	internal void SetupTurnAbilityPhase(AbilityPriority phasePriority, List<AbilityRequest> abilityRequests, HashSet<int> hitActorIds, bool hasHitsWithoutAnimEntry)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void TheatricsManager::SetupTurnAbilityPhase(AbilityPriority,System.Collections.Generic.List`1<AbilityRequest>,System.Collections.Generic.HashSet`1<System.Int32>,System.Boolean)' called on client");
			return;
		}
		m_turn.SetupAbilityPhase(phasePriority, abilityRequests, hitActorIds, hasHitsWithoutAnimEntry);
	}
#endif

	// server-only
#if SERVER
	[Server]
	internal void MarkPhasesOnActionsDone()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void TheatricsManager::MarkPhasesOnActionsDone()' called on client");
			return;
		}
		if (m_turn != null && m_turn.m_abilityPhases != null)
		{
			foreach (Phase phase in m_turn.m_abilityPhases)
			{
				if (phase != null)
				{
					phase.SetTurnActionsDone();
				}
			}
		}
	}
#endif

	private void OnEnable()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	private void OnDisable()
	{
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		m_turn = new Turn();
		m_phaseToUpdate = AbilityPriority.INVALID;
		m_lastPhaseEnded = AbilityPriority.INVALID;
	}

	internal bool AbilityPhaseHasNoAnimations()
	{
		return m_turn == null || !m_turn.HasAbilityPhaseAnimation();
	}

	internal AbilityPriority GetPhaseToUpdate()  // public in rogues
	{
		return m_phaseToUpdate;
	}

	[Server]
	internal void PlayPhase(AbilityPriority phaseIndex)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void TheatricsManager::PlayPhase(AbilityPriority)' called on client");
			return;
		}
		m_playerConnectionIdsInUpdatePhase.Clear();
		if (m_turn.HasAbilityPhaseAnimation(phaseIndex))
		{
			for (int i = 0; i < GameFlow.Get().playerDetails.Count; i++)
			{
				Player key = GameFlow.Get().playerDetails.Keys.ElementAt(i);
				PlayerDetails playerDetails = GameFlow.Get().playerDetails[key];
				if (!playerDetails.m_disconnected
					&& playerDetails.m_gameObjects != null
					&& playerDetails.m_gameObjects.Count > 0
					&& playerDetails.IsHumanControlled
					&& !playerDetails.IsSpectator
#if SERVER
					// rogues
					&& playerDetails.m_accountId != 0L
					&& !playerDetails.IsLoadTestBot
					&& !ServerGameManager.Get().IsAccountReconnecting(playerDetails.m_accountId)
#endif
					)
				{
					m_playerConnectionIdsInUpdatePhase.Add(playerDetails.m_accountId);
				}
			}
		}
		else if (GameFlowData.Get().activeOwnedActorData != null)
		{
			m_playerConnectionIdsInUpdatePhase.Add(GameFlowData.Get().activeOwnedActorData.GetPlayerDetails().m_accountId);
		}
		m_numConnectionIdsAddedForPhase = m_playerConnectionIdsInUpdatePhase.Count;
		m_turn.InitPhase(phaseIndex);  //, true);  in rogues
		m_phaseToUpdate = phaseIndex;
		m_phaseStartTime = Time.time;
	}

	// added in rogues
#if SERVER
	internal void InitPhaseClient_FCFS(AbilityPriority phase)
	{
		Log.Warning("InitPhaseClient_FCFS");  // custom
		m_turn.InitPhase(phase); // , false
		m_phaseToUpdate = phase;
		m_phaseStartTime = Time.time;
	}
#endif

	// added in rogues
#if SERVER
	internal void SetTurn_FCFS(Turn turn)
	{
		m_turnToUpdate = GameFlowData.Get().CurrentTurn;
		m_turn = turn;
	}
#endif

	internal void OnSequenceHit(
        Sequence seq,
        ActorData target,
        ActorModelData.ImpulseInfo impulseInfo = null,
        ActorModelData.RagdollActivation ragdollActivation = ActorModelData.RagdollActivation.HealthBased)
	{
		m_turn.OnSequenceHit(seq, target, impulseInfo, ragdollActivation);
	}

	// OnKnockbackMovementHitGathered
	// OnKnockbackMovementHitExecuted
	// NeedToWaitForKnockbackAnimFromActor

	internal bool ClientNeedToWaitBeforeKnockbackMove(ActorData actor)
	{
		int id = (int)AbilityPriority.Combat_Knockback;
		if (m_turn.m_abilityPhases.Count > id && m_turn.m_abilityPhases[id] != null)
		{
			return m_turn.m_abilityPhases[id].ClientNeedToWaitBeforeKnockbackMove(actor);
		}
		return false;
	}

	[Server]
	private void OnServerDisconnect(NetworkConnection conn)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void TheatricsManager::OnServerDisconnect(UnityEngine.Networking.NetworkConnection)' called on client");
			return;
		}
		// server-only
#if SERVER
		ServerGameManager serverGameManager = ServerGameManager.Get();
		long playerAccountIdByConnectionId = serverGameManager.GetPlayerAccountIdByConnectionId(conn.connectionId);
		if (!serverGameManager.IsDisconnectPending(conn))
		{
			StopWaitingForConnectionId(playerAccountIdByConnectionId);
		}
#endif
	}

	[Server]
	internal void OnReplacedWithBots(Player player)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void TheatricsManager::OnReplacedWithBots(Player)' called on client");
			return;
		}
		StopWaitingForConnectionId(player.m_accountId);
	}

	// was empty in reactor
	public void StopWaitingForConnectionId(long accountId)
	{
#if SERVER
		Log.Info($"Theatrics: stop waiting for accountId {accountId}, " +
			$"was waiting from this connection?= {m_playerConnectionIdsInUpdatePhase.Contains(accountId)}");
		m_playerConnectionIdsInUpdatePhase.Remove(accountId);
		Log.Info($"TheatricsManager StopWaitingForConnectionId: Removing {accountId} from m_playerConnectionIdsInUpdatePhase");
		m_connectionIdsToNumPhaseTimeouts.Remove(accountId);
		if (m_playerConnectionIdsInUpdatePhase.Count == 0)
		{
			Log.Info("Theatrics: Marking as not waiting for more clients on disconnect or replace with bots");
			if (ServerActionBuffer.Get() != null)
			{
				ServerActionBuffer.Get().OnPlayPhaseEnded();
			}
		}
#endif
	}

	// removed in rogues
	public override bool OnSerialize(NetworkWriter writer, bool initialState)
	{
		return OnSerializeHelper(new NetworkWriterAdapter(writer), initialState);
	}

	// removed in rogues
	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
		uint num = uint.MaxValue;
		if (!initialState)
		{
			num = reader.ReadPackedUInt32();
		}
		if (num != 0)
		{
			OnSerializeHelper(new NetworkReaderAdapter(reader), initialState);
		}
	}

	// removed in rogues
	private bool OnSerializeHelper(IBitStream stream, bool initialState)
	{
		if (!initialState && m_serializeHelper.ShouldReturnImmediately(ref stream))
		{
			return false;
		}
		int turnToUpdate = m_turnToUpdate;
		stream.Serialize(ref turnToUpdate);
		bool updatedTurn = turnToUpdate != m_turnToUpdate;
		m_turnToUpdate = turnToUpdate;

		int phaseToUpdate = (int)m_phaseToUpdate;
		stream.Serialize(ref phaseToUpdate);
		bool updatedPhase = m_phaseToUpdate != (AbilityPriority)phaseToUpdate;
		m_phaseToUpdate = (AbilityPriority)phaseToUpdate;

		if (updatedTurn || updatedPhase)
		{
			m_phaseStartTime = Time.time;
			if (updatedTurn)
			{
				m_turn = new Turn();
			}
		}
		m_turn.OnSerializeHelper(stream);
		if (updatedPhase)
		{
			m_turn.InitPhase(m_phaseToUpdate);
		}
		return m_serializeHelper.End(initialState, syncVarDirtyBits);
	}

	private void Update()
	{
		// reactor
		//UpdateClient();
		// rogues
		//UpdateServer();

#if SERVER
		// NOTE custom code
		if (NetworkServer.active)
        {
			UpdateServer();
		}
		else if (NetworkClient.active)
		{
			UpdateClient();
		}
#else
		UpdateClient();
#endif
	}

	// removed in rogues
	private void UpdateClient()
	{
		if (GameFlowData.Get() == null)
		{
			return;
		}
		if (m_turnToUpdate != GameFlowData.Get().CurrentTurn)
		{
			if (m_turnToUpdate > GameFlowData.Get().CurrentTurn)
			{
				Debug.LogError("Theatrics: Turn to update is higher than current turn");
			}
			return;
		}
		if (m_phaseToUpdate != AbilityPriority.INVALID && !m_turn.UpdatePhase(m_phaseToUpdate))
		{
			if (m_lastPhaseEnded != m_phaseToUpdate && GameFlowData.Get().LocalPlayerData != null)
			{
				GameFlowData.Get().LocalPlayerData.CallCmdTheatricsManagerUpdatePhaseEnded((int)m_phaseToUpdate, Time.time - m_phaseStartTime, Time.smoothDeltaTime);
				m_lastPhaseEnded = m_phaseToUpdate;
			}
		}
		else if (ServerClientUtils.GetCurrentActionPhase() == ActionBufferPhase.Movement)
		{
			bool anyHiddenMovement = false;
			bool anyVisibleMovement = false;
			List<ActorData> actors = GameFlowData.Get().GetActors();
			FogOfWar clientFog = FogOfWar.GetClientFog();
			if (clientFog != null)
			{
				for (int i = 0; i < actors.Count; i++)
				{
					ActorData actorData = actors[i];
					if (actorData.GetActorMovement().AmMoving())
					{
						bool travelBoardSquareVisible = clientFog.IsVisible(actorData.GetTravelBoardSquare());
						anyHiddenMovement = anyHiddenMovement || !travelBoardSquareVisible;
						anyVisibleMovement = anyVisibleMovement || travelBoardSquareVisible;
					}
				}
				if (anyHiddenMovement
					&& !anyVisibleMovement
					&& (SinglePlayerManager.Get() == null || SinglePlayerManager.Get().EnableHiddenMovementText()))
				{
					InterfaceManager.Get().DisplayAlert(StringUtil.TR("HiddenMovement", "Global"), Color.white);
				}
			}
		}
		if (ServerClientUtils.GetCurrentActionPhase() == ActionBufferPhase.Movement
			|| ServerClientUtils.GetCurrentActionPhase() == ActionBufferPhase.MovementWait)
		{
			SetAnimatorParamOnAllActors("DecisionPhase", true);
		}
	}

	// UpdateServer

	public void OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		if (eventType != GameEventManager.EventType.UIPhaseStartedMovement)
		{
			return;
		}
		PlayerData localPlayerData = GameFlowData.Get().LocalPlayerData;
		if (localPlayerData == null)
		{
			return;
		}
		foreach (ActorData actor in GameFlowData.Get().GetActors())
		{
			if (actor.IsActorInvisibleForRespawn())
			{
				ActorModelData actorModelData = actor.GetActorModelData();
				if (actorModelData != null)
				{
					actor.ShowRespawnFlare(null, true);
					if (localPlayerData.GetTeamViewing() == actor.GetTeam()
						|| FogOfWar.GetClientFog().IsVisible(actor.CurrentBoardSquare))
					{
						actor.GetActorVFX().ShowOnRespawnVfx();
					}
					actorModelData.EnableRendererAndUpdateVisibility();
				}
			}
		}
	}

	internal void SetAnimatorParamOnAllActors(string paramName, bool value)
	{
		List<ActorData> actors = GameFlowData.Get().GetActors();
		for (int i = 0; i < actors.Count; i++)
		{
			ActorData actorData = actors[i];
			if (actorData != null && actorData.GetModelAnimator() != null)
			{
				actorData.GetModelAnimator().SetBool(paramName, value);
			}
		}
	}

	internal void SetAnimatorParamOnAllActors(string paramName, int value)
	{
		List<ActorData> actors = GameFlowData.Get().GetActors();
		for (int i = 0; i < actors.Count; i++)
		{
			ActorData actorData = actors[i];
			if (actorData != null && actorData.GetModelAnimator() != null)
			{
				actorData.GetModelAnimator().SetInteger(paramName, value);
			}
		}
	}

	[Server]
	internal void OnUpdatePhaseEnded(long accountId, int phaseEnded, float phaseSeconds, float phaseDeltaSeconds)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void TheatricsManager::OnUpdatePhaseEnded(System.Int64,System.Int32,System.Single,System.Single)' called on client");
			return;
		}
		// server-only below
#if SERVER
		GameFlow.Get().GetPlayerHandleFromAccountId(accountId);
		if (phaseEnded == (int)m_phaseToUpdate)
		{
			m_playerConnectionIdsInUpdatePhase.Remove(accountId);
			Log.Info($"TheatricsManager OnUpdatePhaseEnded: Removing {accountId} from m_playerConnectionIdsInUpdatePhase, {m_playerConnectionIdsInUpdatePhase.Count} left");
			m_connectionIdsToNumPhaseTimeouts.Remove(accountId);
			if (m_playerConnectionIdsInUpdatePhase.Count == 0)
			{
				ServerActionBuffer.Get().OnPlayPhaseEnded();
				return;
			}
			bool flag = true;
			if (m_numConnectionIdsAddedForPhase > 0)
			{
				int num = m_numConnectionIdsAddedForPhase - m_playerConnectionIdsInUpdatePhase.Count;
				if (num > 0 && ((num < 3 && num / (float)m_numConnectionIdsAddedForPhase < 0.49f) || m_turn.HasUnfinishedActorAnimationInPhase(m_phaseToUpdate)))
				{
					flag = false;
				}
			}
			if (flag)
			{
				// rogues
				//m_timeToTimeoutPhase = Time.time + m_timeoutAdvancePhaseSlowClient;
				// custom
				m_timeToTimeoutPhase = Time.time + c_timeoutAdvancePhaseSlowClient;
			}
			else
			{
				m_timeToTimeoutPhase = Time.time + 45f;
			}
		}
#endif
	}

	// server-only
#if SERVER
	private void OnUpdatePhaseEndedNoServerPlayer(int phaseEnded)
	{
		if (phaseEnded == (int)m_phaseToUpdate && m_playerConnectionIdsInUpdatePhase.Count == 0)
		{
			ServerActionBuffer.Get().OnPlayPhaseEnded();
		}
	}
#endif

	// server-only
#if SERVER
	private void ForcePhaseAdvance()
	{
		foreach (long num in m_playerConnectionIdsInUpdatePhase)
		{
			// rogues
			//int accountId = (int)num;
			// custom
			long accountId = num;
			PlayerDetails playerDetails = GameFlow.Get().FindHumanPlayerInfoByAccount(accountId, out _);
			string playerHandle = (playerDetails == null) ? "NULL" : playerDetails.m_handle;
			int playerConnectionId = ServerGameManager.Get().GetPlayerConnectionId(accountId);
			m_connectionIdsToNumPhaseTimeouts.TryGetValue(accountId, out int timeoutNum);
			timeoutNum++;
			if (timeoutNum >= m_numClientPhaseTimeoutsUntilForceDisconnect)
			{
				Log.Warning($"Forcing client {playerHandle} (connectionId {accountId}) to disconnect " +
					$"after {timeoutNum} phase resolution timeouts of {c_timeoutAdvancePhaseSlowClient} seconds each.");  // m_timeoutAdvancePhaseSlowClient in rogues
				ServerGameManager.Get().DisconnectClient(playerConnectionId);
			}
			else
			{
				m_connectionIdsToNumPhaseTimeouts[accountId] = timeoutNum;
				Log.Warning($"Forcing unresponsive client {playerHandle} (accountId {accountId}) to advance their resolution of phase {m_phaseToUpdate}. " +
					$"{timeoutNum} of {m_numClientPhaseTimeoutsUntilForceDisconnect} until they are forced to disconnect.");
				int latencyWarningThreshold = (int)CommonServerConfig.Get().GameServerClientLatencyWarningThreshold.TotalMilliseconds;
				if (ServerGameManager.Get().GetServerConnectionCurrentRtt(playerConnectionId) > latencyWarningThreshold)
				{
					GameFlow.Get().DisplayConsoleText("PlayerConnectionIssues", "Disconnect", playerHandle, ConsoleMessageType.Error);
				}
				else
				{
					GameFlow.Get().DisplayConsoleText("PlayerNotResponding", "Disconnect", playerHandle, ConsoleMessageType.Error);
				}
			}
		}
		m_playerConnectionIdsInUpdatePhase.Clear();
		Log.Info($"TheatricsManager ForcePhaseAdvance: Clearing m_playerConnectionIdsInUpdatePhase");
		ServerActionBuffer.Get().OnPlayPhaseEnded();
	}
#endif

	public void OnAnimationEvent(ActorData animatedActor, Object eventObject, GameObject sourceObject)
	{
		if (m_turn != null)
		{
			m_turn.OnAnimationEvent(animatedActor, eventObject, sourceObject);
		}
	}

	internal void DebugLog(string str)
	{
	}

	public bool IsCinematicPlaying()
	{
		return m_turn != null && m_turn.IsCinematicPlaying();
	}

	public bool IsCinematicsRequestedInCurrentPhase(ActorData actor, Ability ability)
	{
		int phaseToUpdate = (int)m_phaseToUpdate;
		if (ability == null
			|| actor == null
			|| m_turn == null
			|| m_turn.m_abilityPhases == null
			|| phaseToUpdate <= 0
			|| phaseToUpdate >= m_turn.m_abilityPhases.Count)
		{
			return false;
		}

		Phase phase = m_turn.m_abilityPhases[phaseToUpdate];
		for (int i = 0; i < phase.m_actorAnimations.Count; i++)
		{
			ActorAnimation actorAnimation = phase.m_actorAnimations[i];
			if (actorAnimation.Caster == actor
				&& actorAnimation.GetAbility() != null
				&& actorAnimation.GetAbility().GetType() == ability.GetType()
				&& actorAnimation.IsCinematicRequested())
			{
				return true;
			}
		}
		return false;
	}

	public static void EncapsulatePathInBound(ref Bounds bound, BoardSquarePathInfo path, ActorData mover)
	{
		ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
		FogOfWar clientFog = FogOfWar.GetClientFog();
		if (activeOwnedActorData == null || clientFog == null || mover == null)
		{
			return;
		}
		for (BoardSquarePathInfo boardSquarePathInfo = path; boardSquarePathInfo != null; boardSquarePathInfo = boardSquarePathInfo.next)
		{
			if (activeOwnedActorData == null
				|| activeOwnedActorData.GetTeam() == mover.GetTeam()
				|| clientFog.IsVisible(boardSquarePathInfo.square))
			{
				bound.Encapsulate(boardSquarePathInfo.square.CameraBounds);
			}
		}
	}

	// removed in rogues
	internal int GetPlayOrderOfClientAction(ClientResolutionAction action, AbilityPriority phase)
	{
		if (m_turn == null || (int)phase >= m_turn.m_abilityPhases.Count)
		{
			return -1;
		}
		Phase phase2 = m_turn.m_abilityPhases[(int)phase];
		for (int num = 0; num < phase2.m_actorAnimations.Count; num++)
		{
			ActorAnimation actorAnimation = phase2.m_actorAnimations[num];
			if (action.ContainsSequenceSource(actorAnimation.SeqSource))
			{
				return actorAnimation.m_playOrderIndex;
			}
		}
		return -1;
	}

	// removed in rogues
	internal int GetPlayOrderOfFirstDamagingHitOnActor(ActorData actor, AbilityPriority phase)
	{
		if (m_turn == null || (int)phase >= m_turn.m_abilityPhases.Count)
		{
			return -1;
		}

		int num = -1;
		Phase phase2 = m_turn.m_abilityPhases[(int)phase];
		for (int i = 0; i < phase2.m_actorAnimations.Count; i++)
		{
			ActorAnimation actorAnimation = phase2.m_actorAnimations[i];
			if (actorAnimation.HitActorsToDeltaHP != null
				&& actorAnimation.HitActorsToDeltaHP.ContainsKey(actor)
				&& actorAnimation.HitActorsToDeltaHP[actor] < 0
				&& (num < 0 || actorAnimation.m_playOrderIndex < num))
			{
				num = actorAnimation.m_playOrderIndex;
			}
		}
		return num;
	}

	internal void LogCurrentStateOnTimeout()
	{
		Log.Error(GetTheatricsStateString());
	}

	internal string GetTheatricsStateString()
	{
		string result = "[Theatrics state] Phase to update: " + m_phaseToUpdate.ToString() +
			", time in phase: " + (Time.time - m_phaseStartTime) +
			", lastPhaseEnded: " + m_lastPhaseEnded +
			"\nNum of phases so far: " + m_turn.m_abilityPhases.Count + "\n";
		List<ActorData> actorsNotDone = new List<ActorData>();
		for (int num = 0; num < m_turn.m_abilityPhases.Count; num++)
		{
			string lineActorAnimsNotDone = "";
			Phase phase = m_turn.m_abilityPhases[num];
			for (int i = 0; i < phase.m_actorAnimations.Count; i++)
			{
				ActorAnimation actorAnimation = phase.m_actorAnimations[i];
				if (actorAnimation.PlayState != ActorAnimation.PlaybackState.ReleasedFocus)
				{
					lineActorAnimsNotDone += "\t" + actorAnimation.ToString() + "\n";
					if (!actorsNotDone.Contains(actorAnimation.Caster))
					{
						actorsNotDone.Add(actorAnimation.Caster);
					}
				}
			}
			if (lineActorAnimsNotDone.Length > 0)
			{
				result += "Phase " + phase.Index.ToString() + ", ActorAnims not done:\n" + lineActorAnimsNotDone;
			}
		}
		foreach (ActorData actor in actorsNotDone)
		{
			if (actor != null)
			{
				ActorModelData actorModelData = actor.GetActorModelData();
				Animator modelAnimator = actor.GetModelAnimator();
				if (actorModelData != null && modelAnimator != null)
				{
					result += actor.DebugNameString() +
						" InIdle=" + actorModelData.IsPlayingIdleAnim() +
						", DamageAnim=" + actorModelData.IsPlayingDamageAnim() +
						", AttackParam=" + modelAnimator.GetInteger("Attack") + "\n";
				}
			}
		}
		return result;
	}

	internal static void LogForDebugging(string str)
	{
		Debug.LogWarning("<color=cyan>Theatrics: </color>" + str + "\n@time= " + Time.time);
	}

	// reactor
	private void UNetVersion()
	{
	}
	// rogues
	//private void MirrorProcessed()
	//{
	//}

	// server-only
#if SERVER
	[Server]
	internal void OnKnockbackMovementHitGathered(ActorData mover)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void TheatricsManager::OnKnockbackMovementHitGathered(ActorData)' called on client");
			return;
		}
		int num = 5;
		if (num < m_turn.m_abilityPhases.Count)
		{
			m_turn.m_abilityPhases[num].OnKnockbackMovementHitGathered(mover);
		}
	}
#endif

	// server-only
#if SERVER
	[Server]
	internal void OnKnockbackMovementHitExecuted(ActorData mover)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void TheatricsManager::OnKnockbackMovementHitExecuted(ActorData)' called on client");
			return;
		}
		int index = 5;
		m_turn.m_abilityPhases[index].OnKnockbackMovementHitExecuted(mover);
	}
#endif

	// server-only
#if SERVER
	[Server]
	internal bool NeedToWaitForKnockbackAnimFromActor(ActorData initiator)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Boolean TheatricsManager::NeedToWaitForKnockbackAnimFromActor(ActorData)' called on client");
			return false;
		}
		bool result = false;
		int num = 5;
		if (m_turn.m_abilityPhases.Count > num && m_turn.m_abilityPhases[num] != null)
		{
			result = m_turn.m_abilityPhases[num].NeedToWaitForKnockbackAnimFromActor(initiator);
		}
		return result;
	}
#endif

	// server-only
	// TODO LOW compare with UpdateClient
#if SERVER
	private void UpdateServer()
	{
		if (GameFlowData.Get() == null || m_turnToUpdate != GameFlowData.Get().CurrentTurn)
		{
			return;
		}
		if (m_phaseToUpdate != AbilityPriority.INVALID && !m_turn.UpdatePhase(m_phaseToUpdate))
		{
			if (ClientResolutionManager.Get() != null)
			{
				ClientResolutionManager.Get().OnTheatricsPhaseUpdateFinished(m_phaseToUpdate);
			}
			// rogues
			//if (HUD_UI.Get() != null && HUD_UI.Get().m_mainScreenPanel != null)
			//{
			//	HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.HighlightAllNameplatesForAbility();
			//}
			if (NetworkServer.active)
			{
				if (GameFlowData.Get().activeOwnedActorData == null)
				{
					if (m_playerConnectionIdsInUpdatePhase.Count == 0)
					{
						OnUpdatePhaseEndedNoServerPlayer((int)m_phaseToUpdate);
						m_phaseToUpdate = AbilityPriority.INVALID;
					}
					else if (Time.time > m_timeToTimeoutPhase)
					{
						Log.Warning($"ForcePhaseAdvance 1");
						ForcePhaseAdvance();
						m_phaseToUpdate = AbilityPriority.INVALID;
					}
				}
				else if (m_playerConnectionIdsInUpdatePhase.Count <= 1)
				{
					OnUpdatePhaseEnded(ClientGameManager.Get().AccountId, (int)m_phaseToUpdate, Time.time - m_phaseStartTime, Time.smoothDeltaTime);
					m_phaseToUpdate = AbilityPriority.INVALID;
				}
				else if (Time.time > m_timeToTimeoutPhase)
				{
					Log.Warning($"ForcePhaseAdvance 2");
					ForcePhaseAdvance();
					m_phaseToUpdate = AbilityPriority.INVALID;
				}
				if (m_phaseToUpdate == AbilityPriority.INVALID)
				{
					m_timeToTimeoutPhase = float.MaxValue;
				}
				else if (m_timeToTimeoutPhase == float.MaxValue && ServerResolutionManager.Get().GetCurrentState() == ServerResolutionManager.ServerResolutionManagerState.WaitingForNextPhase)
				{
					m_timeToTimeoutPhase = Time.time + 45f;
				}
			}
			else if (m_lastPhaseEnded != m_phaseToUpdate && GameFlowData.Get().LocalPlayerData != null)
			{
				Log.Warning($"CallCmdTheatricsManagerUpdatePhaseEnded");
				GameFlowData.Get().LocalPlayerData.CallCmdTheatricsManagerUpdatePhaseEnded((int)m_phaseToUpdate, Time.time - m_phaseStartTime, Time.smoothDeltaTime);
				m_lastPhaseEnded = m_phaseToUpdate;
				m_phaseToUpdate = AbilityPriority.INVALID;
			}
		}
		if (NetworkServer.active)
		{
			base.SetDirtyBit(1U);
		}
	}

	// custom
	public void ResetTimeToTimeoutPhase()
	{
		m_timeToTimeoutPhase = float.MaxValue;
	}
#endif
}

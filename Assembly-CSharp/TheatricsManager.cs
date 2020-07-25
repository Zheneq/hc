using System.Collections.Generic;
using System.Linq;
using Theatrics;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class TheatricsManager : NetworkBehaviour, IGameEventListener
{
	public bool m_allowAbilityAnimationInterruptHitReaction = true;

	public int m_numClientPhaseTimeoutsUntilForceDisconnect = 3;

	public const float c_timeoutAdvancePhaseSlowClient = 1.3f;

	public const float c_maxPhaseAdvanceTimeoutDuration = 45f;

	[Separator("Ragdoll Force Settings", true)]
	public float m_ragdollImpactForce = 15f;

	[Header("-- Whether to apply force only to single joint")]
	public bool m_ragdollApplyForceOnSingleJointOnly;

	private Turn m_turn = new Turn();

	private AbilityPriority m_phaseToUpdate = AbilityPriority.INVALID;

	private int m_turnToUpdate;

	private HashSet<long> m_playerConnectionIdsInUpdatePhase = new HashSet<long>();

	private int m_numConnectionIdsAddedForPhase;

	private float m_phaseStartTime;

	private AbilityPriority m_lastPhaseEnded = AbilityPriority.INVALID;

	private static TheatricsManager s_instance;

	private SerializeHelper m_serializeHelper = new SerializeHelper();

	internal string m_debugSerializationInfoStr = string.Empty;

	internal const string c_actorAnimDebugHeader = "<color=cyan>Theatrics: </color>";

	internal static bool DebugLog => false;

	internal static bool TraceTheatricsSerialization => false;

	internal static TheatricsManager Get()
	{
		return s_instance;
	}

	public void UpdateTurn(Turn turn, AbilityPriority phaseToUpdate)
	{
		m_turn = turn;
		m_turnToUpdate = turn.TurnID;
		m_phaseToUpdate = phaseToUpdate;
	}

	public static float GetRagdollImpactForce()
	{
		return s_instance?.m_ragdollImpactForce ?? 15f;
	}

	public static bool RagdollOnlyApplyForceAtSingleJoint()
	{
		return s_instance?.m_ragdollApplyForceOnSingleJointOnly ?? false;
	}

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
		return m_turn == null || !m_turn.HasAnimations();
	}

	internal AbilityPriority GetPhaseToUpdate()
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
		if (m_turn.PhaseHasAnimations(phaseIndex))
		{
			for (int i = 0; i < GameFlow.Get().playerDetails.Count; i++)
			{
				Player key = GameFlow.Get().playerDetails.Keys.ElementAt(i);
				PlayerDetails playerDetails = GameFlow.Get().playerDetails[key];
				if (playerDetails.m_disconnected ||
					playerDetails.m_gameObjects == null ||
					playerDetails.m_gameObjects.Count <= 0 ||
					!playerDetails.IsHumanControlled ||
					playerDetails.IsSpectator)
				{
					continue;
				}

				m_playerConnectionIdsInUpdatePhase.Add(playerDetails.m_accountId);
			}
		}
		else if (GameFlowData.Get().activeOwnedActorData != null)
		{
			m_playerConnectionIdsInUpdatePhase.Add(GameFlowData.Get().activeOwnedActorData.GetPlayerDetails().m_accountId);
		}
		m_numConnectionIdsAddedForPhase = m_playerConnectionIdsInUpdatePhase.Count;
		m_turn.GoToPhase(phaseIndex);
		m_phaseToUpdate = phaseIndex;
		m_phaseStartTime = Time.time;
	}

	internal void OnSequenceHit(
		Sequence seq,
		ActorData target,
		ActorModelData.ImpulseInfo impulseInfo = null,
		ActorModelData.RagdollActivation ragdollActivation = ActorModelData.RagdollActivation.HealthBased)
	{
		m_turn.OnSequenceHit(seq, target, impulseInfo, ragdollActivation);
	}

	internal bool ClientNeedToWaitBeforeKnockbackMove(ActorData actor)
	{
		int id = (int)AbilityPriority.Combat_Knockback;
		if (m_turn.Phases.Count > id && m_turn.Phases[id] != null)
		{
			return m_turn.Phases[id]._001C(actor);
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

	public void StopWaitingForConnectionId(long accountId)
	{
	}

	public override bool OnSerialize(NetworkWriter writer, bool initialState)
	{
		return OnSerializeHelper(new NetworkWriterAdapter(writer), initialState);
	}

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

	private bool OnSerializeHelper(IBitStream stream, bool initialState)
	{
		if (!initialState && m_serializeHelper.ShouldReturnImmediately(ref stream))
		{
			return false;
		}
		int value = m_turnToUpdate;
		stream.Serialize(ref value);
		bool updatedTurn = value != m_turnToUpdate;
		m_turnToUpdate = value;

		int value2 = (int)m_phaseToUpdate;
		stream.Serialize(ref value2);
		bool updatedPhase = m_phaseToUpdate != (AbilityPriority)value2;
		m_phaseToUpdate = (AbilityPriority)value2;

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
			m_turn.GoToPhase(m_phaseToUpdate);
		}
		return m_serializeHelper.End(initialState, syncVarDirtyBits);
	}

	private void Update()
	{
		UpdateClient();
	}

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
		if (m_phaseToUpdate != AbilityPriority.INVALID && !m_turn._001A(m_phaseToUpdate))
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
					if (!actorData.GetActorMovement().AmMoving())
					{
						continue;
					}
					bool travelBoardSquareVisible = clientFog.IsVisible(actorData.GetTravelBoardSquare());
					anyHiddenMovement = anyHiddenMovement || !travelBoardSquareVisible;
					anyVisibleMovement = anyVisibleMovement || travelBoardSquareVisible;
				}
				if (anyHiddenMovement && !anyVisibleMovement)
				{
					if (SinglePlayerManager.Get() == null || SinglePlayerManager.Get().EnableHiddenMovementText())
					{
						InterfaceManager.Get().DisplayAlert(StringUtil.TR("HiddenMovement", "Global"), Color.white);
					}
				}
			}
		}
		if (ServerClientUtils.GetCurrentActionPhase() == ActionBufferPhase.Movement ||
			ServerClientUtils.GetCurrentActionPhase() == ActionBufferPhase.MovementWait)
		{
			SetAnimatorParamOnAllActors("DecisionPhase", true);
		}
	}

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
			if (!actor.IsPickingRespawnSquare())
			{
				continue;
			}
			ActorModelData actorModelData = actor.GetActorModelData();
			if (actorModelData == null)
			{
				continue;
			}
			actor.ShowRespawnFlare(null, true);
			if (localPlayerData.GetTeamViewing() == actor.GetTeam() ||
				FogOfWar.GetClientFog().IsVisible(actor.CurrentBoardSquare))
			{
				actor.GetActorVFX().ShowOnRespawnVfx();
			}
			actorModelData.EnableRendererAndUpdateVisibility();
		}
	}

	internal void SetAnimatorParamOnAllActors(string paramName, bool value)
	{
		List<ActorData> actors = GameFlowData.Get().GetActors();
		for (int i = 0; i < actors.Count; i++)
		{
			ActorData actorData = actors[i];
			if (actorData == null)
			{
				continue;
			}
			if (actorData.GetModelAnimator() != null)
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
	}

	public void OnAnimationEvent(ActorData animatedActor, Object eventObject, GameObject sourceObject)
	{
		m_turn?.OnAnimationEvent(animatedActor, eventObject, sourceObject);
	}

	internal void ServerLog(string _001D)
	{
	}

	public bool IsCinematicPlaying()
	{
		return m_turn?.IsCinematicPlaying() ?? false;
	}

	public bool IsCinematicsRequestedInCurrentPhase(ActorData actor, Ability ability)
	{
		int phaseToUpdate = (int)m_phaseToUpdate;
		if (ability == null ||
			actor == null ||
			m_turn == null ||
			m_turn.Phases == null ||
			phaseToUpdate <= 0 ||
			phaseToUpdate >= m_turn.Phases.Count)
		{
			return false;
		}

		Phase phase = m_turn.Phases[phaseToUpdate];
		for (int i = 0; i < phase.Animations.Count; i++)
		{
			ActorAnimation actorAnimation = phase.Animations[i];
			if (actorAnimation.Actor == actor &&
				actorAnimation.GetAbility() != null &&
				actorAnimation.GetAbility().GetType() == ability.GetType() &&
				actorAnimation.IsTauntActivated())
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
			if (activeOwnedActorData == null ||
				activeOwnedActorData.GetTeam() == mover.GetTeam() ||
				clientFog.IsVisible(boardSquarePathInfo.square))
			{
				bound.Encapsulate(boardSquarePathInfo.square.CameraBounds);
			}
		}
	}

	internal int GetPlayOrderOfClientAction(ClientResolutionAction action, AbilityPriority phase)
	{
		if (m_turn == null || (int)phase >= m_turn.Phases.Count)
		{
			return -1;
		}
		Phase phase2 = m_turn.Phases[(int)phase];
		for (int num = 0; num < phase2.Animations.Count; num++)
		{
			ActorAnimation actorAnimation = phase2.Animations[num];
			if (action.ContainsSequenceSource(actorAnimation.SeqSource))
			{
				return actorAnimation.playOrderIndex;
			}
		}
		return -1;
	}

	internal int GetPlayOrderOfFirstDamagingHitOnActor(ActorData actor, AbilityPriority phase)
	{
		if (m_turn == null || (int)phase >= m_turn.Phases.Count)
		{
			return -1;
		}

		int num = -1;
		Phase phase2 = m_turn.Phases[(int)phase];
		for (int i = 0; i < phase2.Animations.Count; i++)
		{
			ActorAnimation actorAnimation = phase2.Animations[i];
			if (actorAnimation.HitActorsToDeltaHP == null ||
				!actorAnimation.HitActorsToDeltaHP.ContainsKey(actor) ||
				actorAnimation.HitActorsToDeltaHP[actor] >= 0 ||
				num >= 0 && actorAnimation.playOrderIndex >= num)
			{
				continue;
			}

			num = actorAnimation.playOrderIndex;
			// TODO: Could a return or min be missing here?
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
			"\nNum of phases so far: " + m_turn.Phases.Count + "\n";
		List<ActorData> actorsNotDone = new List<ActorData>();
		
		for (int num = 0; num < m_turn.Phases.Count; num++)
		{
			string lineActorAnimsNotDone = string.Empty;
			Phase phase = m_turn.Phases[num];
			for (int i = 0; i < phase.Animations.Count; i++)
			{
				ActorAnimation actorAnimation = phase.Animations[i];
				if (actorAnimation.State == ActorAnimation.PlaybackState.F)
				{
					continue;
				}
				lineActorAnimsNotDone += "\t" + actorAnimation.ToString() + "\n";
				if (!actorsNotDone.Contains(actorAnimation.Actor))
				{
					actorsNotDone.Add(actorAnimation.Actor);
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
					result += actor.GetDebugName() +
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

	private void UNetVersion()
	{
	}
}

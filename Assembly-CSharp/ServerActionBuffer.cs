// ROGUES
// SERVER
using System;
using System.Collections.Generic;
//using Mirror;
using UnityEngine;
using UnityEngine.Networking;

// was empty in reactor
#if SERVER
public class ServerActionBuffer : NetworkBehaviour
{
	private static ServerActionBuffer s_instance;

	private SharedActionBuffer m_sharedActionBuffer;

	private ServerEvadeManager m_evadeManager;

	private ServerKnockbackManager m_knockbackManager;

	private ServerMovementStabilizer m_movementStabilizer;

	// rogues
	//private PlayerActionStateMachine m_playerActionFsm;

	private List<AbilityRequest> m_storedAbilityRequests;

	private List<MovementRequest> m_storedMovementRequests;

	private List<AbilityRequest> m_storedAbilityRequestsForNextTurn;

	private bool m_waitingForPlayPhaseEnded;

	private List<MovementRequest> m_removedMovementRequestsFromForceChase;

	internal bool m_gatheringFakeResults;

	internal List<BoardSquare> m_tempReservedSquaresForAbilitySpoil = new List<BoardSquare>();

	private float m_lastAbilityPhaseSet;

	private AbilityPriority m_abilityPhase;

	private ActionBufferPhase m_actionPhase;

	private List<ActorData> m_actorsVisibleUntilEndOfPhase;

	private ActorData m_combatInitiator;

	private int m_combatInitiatorRecordTurn = -1;

	public static bool c_clientOnlySequences = true;

	private const string c_actionLogSearchMarker = "{act} ";

	public static ServerActionBuffer Get()
	{
		return ServerActionBuffer.s_instance;
	}

	public bool GatheringFakeResults
	{
		get
		{
			return m_gatheringFakeResults;
		}
		private set
		{
			if (m_gatheringFakeResults != value)
			{
				m_gatheringFakeResults = value;
			}
		}
	}

	public AbilityPriority AbilityPhase
	{
		get
		{
			return m_abilityPhase;
		}
		set  // private in rogues
		{
			if (m_abilityPhase != value)
			{
				if (GameplayData.Get().m_resolveDamageBetweenAbilityPhases || (GameplayData.Get().m_resolveDamageAfterEvasion && m_abilityPhase == AbilityPriority.Evasion))
				{
					ServerCombatManager.Get().ResolveHitPoints();
					ServerCombatManager.Get().ResolveTechPoints();
				}
				
				OnAbilityPhaseEnd(m_abilityPhase); // custom
				
				m_abilityPhase = value;
				SynchronizeSharedData();
				
				m_waitingForPlayPhaseEnded = true; // custom
			}
			m_lastAbilityPhaseSet = Time.time;
		}
	}

	// custom
	public ActionBufferPhase ActionPhase
	{
		get
		{
			return m_actionPhase;
		}
		set
		{
			if (m_actionPhase != value)
			{
				m_actionPhase = value;
				SynchronizeSharedData();
			}
		}
	}

	// custom
	public bool IsWaitingForPlayPhaseEnded() => m_waitingForPlayPhaseEnded && AbilityPhase != AbilityPriority.INVALID;

	private void Awake()
	{
		ServerActionBuffer.s_instance = this;
		if (NetworkServer.active)
		{
			m_evadeManager = new ServerEvadeManager();
			m_knockbackManager = new ServerKnockbackManager();
			m_movementStabilizer = new ServerMovementStabilizer();
			GameObject sharedActionBufferPrefab = NetworkedSharedGameplayPrefabs.GetSharedActionBufferPrefab();
			if (sharedActionBufferPrefab != null)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(sharedActionBufferPrefab, Vector3.zero, Quaternion.identity);
				NetworkServer.Spawn(gameObject);
				UnityEngine.Object.DontDestroyOnLoad(gameObject);
				m_sharedActionBuffer = gameObject.GetComponent<SharedActionBuffer>();
			}
			m_actorsVisibleUntilEndOfPhase = new List<ActorData>();
		}
		m_storedAbilityRequests = new List<AbilityRequest>();
		m_storedMovementRequests = new List<MovementRequest>();
		m_storedAbilityRequestsForNextTurn = new List<AbilityRequest>();
		m_removedMovementRequestsFromForceChase = new List<MovementRequest>();

		// rogues
		//m_playerActionFsm = new PlayerActionStateMachine();
	}

	private void OnDestroy()
	{
		if (NetworkServer.active && m_sharedActionBuffer != null)
		{
			NetworkServer.Destroy(m_sharedActionBuffer.gameObject);
		}
		ServerActionBuffer.s_instance = null;
	}

	private void Start()
	{
		//AbilityPhase = AbilityUtils.GetHighestAbilityPriority();
		AbilityPhase = AbilityPriority.INVALID;
	}

	private void Update()
	{
		DebugDisplayBufferState();

		// rogues
		//if (NetworkServer.active && m_playerActionFsm != null)
		//{
		//	m_playerActionFsm.OnUpdate();
		//}
	}

	private void SynchronizeSharedData()
	{
		if (m_sharedActionBuffer != null)
		{
			m_sharedActionBuffer.SetDataFromServer(m_actionPhase, m_abilityPhase);
		}
	}

	public void OnPlayPhaseEnded()
	{
		m_waitingForPlayPhaseEnded = false;
	}

	private bool PhaseSetThisFrame()
	{
		return Time.time - m_lastAbilityPhaseSet <= 0f;
	}

	public float TimeSpentInAbilityPhase
	{
		get
		{
			return Time.time - m_lastAbilityPhaseSet;
		}
		private set
		{
		}
	}

	public void SynchronizePositionsOfActorsParticipatingInPhase(AbilityPriority phase)
	{
		if (phase == AbilityPriority.INVALID)
		{
			Log.Error("Calling SynchronizePositionActorsParticipatingInPhase for the 'INVALID' phase.");
			return;
		}
		Log.Info($"SynchronizePositionsOfActorsParticipatingInPhase {phase} BEGIN"); // custom
		foreach (AbilityRequest abilityRequest in ServerActionBuffer.Get().GetAllStoredAbilityRequests())
		{
			if (abilityRequest.m_ability.RunPriority == phase)
			{
				if (abilityRequest.m_caster != null)
				{
					Log.Info($"Requesting SynchronizeTeamSensitiveData {phase} for {abilityRequest.m_caster.DisplayName} for using ability {abilityRequest.m_ability.m_abilityName}"); // custom
					abilityRequest.m_caster.SynchronizeTeamSensitiveData();
				}
				foreach (ActorData hitActor in abilityRequest.m_additionalData.m_abilityResults.HitActorsArray())
				{
					Log.Info($"Requesting SynchronizeTeamSensitiveData {phase} for {hitActor.DisplayName} for being hit by {abilityRequest.m_caster?.DisplayName}'s ability {abilityRequest.m_ability.m_abilityName}"); // custom
					hitActor.SynchronizeTeamSensitiveData();
				}
			}
		}
		foreach (KeyValuePair<ActorData, List<Effect>> keyValuePair in ServerEffectManager.Get().GetAllActorEffects())
		{
			foreach (Effect effect in keyValuePair.Value)
			{
				if (effect.HasResolutionAction(phase))
				{
					if (effect.Caster != null)
					{
						Log.Info($"Requesting SynchronizeTeamSensitiveData {phase} for {effect.Caster.DisplayName} for using effect {effect.m_effectName}"); // custom
						effect.Caster.SynchronizeTeamSensitiveData();
					}
					if (effect.Target != null)
					{
						Log.Info($"Requesting SynchronizeTeamSensitiveData {phase} for {effect.Target.DisplayName} for being the target of {effect.Caster?.DisplayName}'s effect {effect.m_effectName}"); // custom
						effect.Target.SynchronizeTeamSensitiveData();
					}
					foreach (ActorData hitActor in effect.GetResultsForPhase(phase, true).HitActorsArray())
					{
						Log.Info($"Requesting SynchronizeTeamSensitiveData {phase} for {hitActor.DisplayName} for being hit by {effect.Caster?.DisplayName}'s effect {effect.m_effectName}"); // custom
						hitActor.SynchronizeTeamSensitiveData();
					}
				}
			}
		}
		foreach (Effect effect in ServerEffectManager.Get().GetWorldEffects())
		{
			if (effect.HasResolutionAction(phase))
			{
				if (effect.Caster != null)
				{
					Log.Info($"Requesting SynchronizeTeamSensitiveData {phase} for {effect.Caster.DisplayName} for using world effect {effect.m_effectName}"); // custom
					effect.Caster.SynchronizeTeamSensitiveData();
				}
				foreach (var hitActor in effect.GetResultsForPhase(phase, true).HitActorsArray())
				{
					Log.Info($"Requesting SynchronizeTeamSensitiveData {phase} for {hitActor.DisplayName} for being hit by {effect.Caster?.DisplayName}'s world effect {effect.m_effectName}"); // custom
					hitActor.SynchronizeTeamSensitiveData();
				}
			}
		}
		Log.Info($"SynchronizePositionsOfActorsParticipatingInPhase {phase} END"); // custom
	}

	public void SynchronizePositionsOfActorsThatWillBeSeen(List<ActorData> actorsThatWillBeSeenButArentMoving)
	{
		if (actorsThatWillBeSeenButArentMoving != null)
		{
			foreach (ActorData actor in actorsThatWillBeSeenButArentMoving)
			{
				Log.Info($"Requesting SynchronizeTeamSensitiveData for {actor.DisplayName} that is not moving but will be seen"); // custom
				actor.SynchronizeTeamSensitiveData();
			}
		}
	}

	private void SetSquareAtPhaseStartForActors()
	{
		foreach (ActorData actorData in GameFlowData.Get().GetActors())
		{
			actorData.SetSquareAtPhaseStart(actorData.GetCurrentBoardSquare());
		}
	}

	private void TrackDesiredMovementAmountOnResolve()
	{
		foreach (MovementRequest movementRequest in m_storedMovementRequests)
		{
			ActorData actor = movementRequest.m_actor;
			if (actor != null && actor.GetActorBehavior() != null)
			{
				if (movementRequest.IsChasing())
				{
					if (movementRequest.m_chaseTarget != null && movementRequest.m_chaseTarget.GetCurrentBoardSquare() != null)
					{
						BoardSquare closestMoveableSquareTo = actor.GetActorMovement().GetClosestMoveableSquareTo(movementRequest.m_chaseTarget.GetCurrentBoardSquare(), false);
						if (closestMoveableSquareTo != null)
						{
							BoardSquarePathInfo boardSquarePathInfo = actor.GetActorMovement().BuildPathTo(actor.InitialMoveStartSquare, closestMoveableSquareTo);
							if (boardSquarePathInfo != null)
							{
								actor.GetActorBehavior().TrackDesiredMovementOnResolveStart(boardSquarePathInfo.FindMoveCostToEnd());
							}
						}
					}
				}
				else if (movementRequest.m_path != null)
				{
					actor.GetActorBehavior().TrackDesiredMovementOnResolveStart(movementRequest.m_path.FindMoveCostToEnd());
				}
			}
		}
	}

	private void SetSquareRequestedForMovementMetricsForActors()
	{
		foreach (ActorData actorData in GameFlowData.Get().GetActors())
		{
			bool flag = false;
			if (actorData.IsHumanControlled())
			{
				foreach (MovementRequest movementRequest in m_storedMovementRequests)
				{
					if (movementRequest.m_actor == actorData)
					{
						if (movementRequest.m_chaseTarget != null)
						{
							actorData.SetSquareRequestedForMovementMetrics(movementRequest.m_chaseTarget.GetCurrentBoardSquare());
						}
						else
						{
							actorData.SetSquareRequestedForMovementMetrics(movementRequest.m_targetSquare);
						}
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				actorData.SetSquareRequestedForMovementMetrics(null);
			}
		}
	}

	private void ReInitAbilityInteractions(AbilityPriority newPhase)
	{
		foreach (AbilityRequest abilityRequest in m_storedAbilityRequests)
		{
			if (abilityRequest.m_ability.RunPriority == newPhase)
			{
				abilityRequest.m_caster.GetAbilityData().ReinitAbilityInteractionData(abilityRequest.m_ability);
			}
		}
	}

	private void OnPhaseStartForRequestedAbilities(AbilityPriority gatherAbilityPhase)
	{
		foreach (AbilityRequest abilityRequest in m_storedAbilityRequests)
		{
			if (abilityRequest.m_ability.RunPriority == gatherAbilityPhase)
			{
				abilityRequest.m_ability.OnPhaseStartWhenRequested(abilityRequest.m_targets, abilityRequest.m_caster);
			}
		}
	}

	public void ImmediateUpdateAllFogOfWar()
	{
		foreach (ActorData actorData in GameFlowData.Get().GetActors())
		{
			actorData.GetFogOfWar().ImmediateUpdateVisibilityOfSquares();
		}
	}

	private void OnAbilityPhaseEnd(AbilityPriority oldPhase)
	{
		m_knockbackManager.OnAbilityPhaseEnd(AbilityPhase);
		BarrierManager.Get().OnAbilityPhaseEnd(AbilityPhase);
		foreach (ActorData actorData in GameFlowData.Get().GetActors())
		{
			if (actorData.GetPassiveData() != null)
			{
				actorData.GetPassiveData().OnAbilityPhaseEnd(oldPhase);
			}
			if (GameplayData.Get().m_unsuppressInvisibilityOnEndOfPhase && actorData.GetAbilityData() != null)
			{
				actorData.GetAbilityData().UnsuppressInvisibility();
			}
			if (AbilityPhase == AbilityPriority.Evasion || AbilityPhase == AbilityPriority.Combat_Final)
			{
				actorData.ClearFacingDirectionAfterMovement();
			}
			if (AbilityPhase == AbilityPriority.Combat_Final && actorData.IsActorInvisibleForRespawn())
			{
				actorData.RespawnPickedPositionSquare = null;
				if (SpawnPointManager.Get() != null && SpawnPointManager.Get().m_respawnActorsCanBeHitDuringMovement && actorData.NextRespawnTurn > 0)
				{
					actorData.IgnoreForAbilityHits = false;
				}
			}
		}
		foreach (ActorData actorData2 in m_actorsVisibleUntilEndOfPhase)
		{
			actorData2.VisibleTillEndOfPhase = false;
		}
		m_actorsVisibleUntilEndOfPhase.Clear();
		foreach (AbilityRequest abilityRequest in m_storedAbilityRequests)
		{
			if (abilityRequest.m_ability.GetRunPriority() == oldPhase && abilityRequest.m_ability.GetStatusToApplyWhenRequested().Count > 0)
			{
				abilityRequest.m_caster.GetAbilityData().AddOnRequestStatusForAbility(abilityRequest.m_ability);
			}
		}
	}

	public void MarkVisibleTillEndOfPhase(ActorData actor)
	{
	}

	public void OnTurnStart()
	{
		SetSquareAtPhaseStartForActors();

		// rogues
		//m_playerActionFsm.TransitionToState(PlayerActionStateMachine.StateFlag.WaitingForInput);

		// rogues
		//Team actingTeam = GameFlowData.Get().ActingTeam;

		m_storedAbilityRequests.Clear();
		foreach (AbilityRequest abilityRequest in m_storedAbilityRequestsForNextTurn)
		{
			if (
				// rogues
				//actingTeam == abilityRequest.m_caster.GetTeam() &&
				!abilityRequest.m_caster.IsDead())
			{
				m_storedAbilityRequests.Add(abilityRequest);
				OnAbilityRequestStored(abilityRequest);

				// rogues
				//GetPlayerActionFSM().RunQueuedActionsFromActor(abilityRequest.m_caster);
			}
		}
		// custom
		m_storedAbilityRequestsForNextTurn.Clear();
		// rogues
		//m_storedAbilityRequestsForNextTurn.RemoveAll((AbilityRequest r) => r.m_caster.GetTeam() == actingTeam);
    }

	public void ClearNormalMovementResults()
	{
		ServerEffectManager.Get().ClearAllEffectResultsForNormalMovement();
		BarrierManager.Get().ClearAllBarrierResultsForNormalMovement();
		PowerUpManager.Get().ClearAllPowerupResultsForNormalMovement();
		// TODO CTF CTC
		//if (CaptureTheFlag.Get() != null)
		//{
		//	CaptureTheFlag.Get().ClearNormalMovementResults();
		//}
		//if (CollectTheCoins.Get() != null)
		//{
		//	CollectTheCoins.Get().ClearNormalMovementResults();
		//}
		ClearIgnoreCantSprintFlags();
		ClearMoveRangeCompensation();
	}

	private void ClearIgnoreCantSprintFlags()
	{
		foreach (ActorData actorData in GameFlowData.Get().GetActors())
		{
			actorData.GetActorMovement().IgnoreCantSprintStatus = false;
		}
	}

	private void ClearMoveRangeCompensation()
	{
		foreach (ActorData actorData in GameFlowData.Get().GetActors())
		{
			actorData.GetActorMovement().MoveRangeCompensation = 0f;
		}
	}

	public Bounds GetMovementBoundsForTeam(List<MovementRequest> stabilizedMoveRequests, Team team)
	{
		Bounds result = default(Bounds);
		List<ActorData> actors = GameFlowData.Get().GetActors();
		bool flag = false;
		for (int i = 0; i < actors.Count; i++)
		{
			ActorData actorData = actors[i];
			if (actorData != null && !actorData.IsDead() && actorData.GetCurrentBoardSquare() != null && !actorData.IgnoreForAbilityHits && (actorData.GetTeam() == team || actorData.IsActorVisibleToAnyEnemy()))
			{
				if (!flag)
				{
					result = actorData.GetCurrentBoardSquare().CameraBounds;
					flag = true;
				}
				else
				{
					result.Encapsulate(actorData.GetCurrentBoardSquare().CameraBounds);
				}
			}
		}
		if (!flag)
		{
			BoardSquare squareFromIndex = Board.Get().GetSquareFromIndex(Board.Get().GetMaxX() / 2, Board.Get().GetMaxY() / 2);
			if (squareFromIndex != null)
			{
				result = squareFromIndex.CameraBounds;
			}
			else
			{
				Log.Error("Failed to find camera bound for movement");
			}
		}
		foreach (MovementRequest movementRequest in stabilizedMoveRequests)
		{
			if (movementRequest.m_actor.GetTeam() == team)
			{
				EncapsulateMovementBoundsForAllyPath(movementRequest.m_path, ref result);
			}
			else if (movementRequest.m_path != null)
			{
				EncapsulateMovementBoundsForEnemyPath(movementRequest.m_path, ref result);
			}
		}
		Vector3 center = result.center;
		Vector3 size = result.size;
		center.y = 1.5f + (float)Board.Get().BaselineHeight;
		size.y = 3f;
		result = new Bounds(center, size);
		return result;
	}

	private void EncapsulateMovementBoundsForAllyPath(BoardSquarePathInfo path, ref Bounds bounds)
	{
		if (path != null)
		{
			for (BoardSquarePathInfo boardSquarePathInfo = path; boardSquarePathInfo != null; boardSquarePathInfo = boardSquarePathInfo.next)
			{
				if (boardSquarePathInfo.square != null)
				{
					bounds.Encapsulate(boardSquarePathInfo.square.CameraBounds);
				}
			}
		}
	}

	private void EncapsulateMovementBoundsForEnemyPath(BoardSquarePathInfo path, ref Bounds bounds)
	{
		if (path != null)
		{
			BoardSquarePathInfo boardSquarePathInfo = path;
			BoardSquarePathInfo boardSquarePathInfo2 = null;
			while (boardSquarePathInfo != null)
			{
				if (boardSquarePathInfo.m_visibleToEnemies || boardSquarePathInfo.m_updateLastKnownPos || boardSquarePathInfo.m_moverDiesHere || boardSquarePathInfo.m_moverHasGameplayHitHere)
				{
					boardSquarePathInfo2 = boardSquarePathInfo;
				}
				else if (boardSquarePathInfo.prev != null && (boardSquarePathInfo.prev.m_visibleToEnemies || boardSquarePathInfo.prev.m_moverHasGameplayHitHere))
				{
					boardSquarePathInfo2 = boardSquarePathInfo;
				}
				boardSquarePathInfo = boardSquarePathInfo.next;
			}
			if (boardSquarePathInfo2 != null)
			{
				for (boardSquarePathInfo = path; boardSquarePathInfo != null; boardSquarePathInfo = boardSquarePathInfo.next)
				{
					if (boardSquarePathInfo.square != null)
					{
						bounds.Encapsulate(boardSquarePathInfo.square.CameraBounds);
					}
					if (boardSquarePathInfo == boardSquarePathInfo2)
					{
						break;
					}
				}
			}
		}
	}

	private static string CreateReadableAbilityRequests(List<AbilityRequest> abilityRequests, AbilityRequest.AbilityResolveState matchingState, out int numMatching)
	{
		numMatching = 0;
		List<string> list = new List<string>(abilityRequests.Count);
		foreach (AbilityRequest abilityRequest in abilityRequests)
		{
			if (abilityRequest.m_resolveState == matchingState)
			{
				numMatching++;
				if (abilityRequest.m_targets == null)
				{
				}
				string arg;
				if (abilityRequest.m_targets.Count == 0)
				{
					arg = "nobody";
				}
				else if (abilityRequest.MainTarget == null)
				{
					arg = "missing target";
				}
				else if (abilityRequest.MainTarget.GetCurrentBestActorTarget() == null)
				{
					arg = "(null)";
				}
				else
				{
					arg = abilityRequest.MainTarget.GetCurrentBestActorTarget().DisplayName;
				}
				list.Add(string.Format("     {0}'s {1}\n     @ {2}\n", abilityRequest.m_caster.DisplayName, abilityRequest.m_ability.m_abilityName, arg));
			}
		}
		list.Sort();
		string text = "";
		foreach (string str in list)
		{
			text += str;
		}
		return text;
	}

	private string BuildAbilityRequestStateStr()
	{
		int num;
		string text = ServerActionBuffer.CreateReadableAbilityRequests(m_storedAbilityRequests, AbilityRequest.AbilityResolveState.QUEUED, out num);
		text = string.Format("Queued: {0}\n", num) + text;
		int num2;
		string text2 = ServerActionBuffer.CreateReadableAbilityRequests(m_storedAbilityRequests, AbilityRequest.AbilityResolveState.RESOLVING, out num2);
		text2 = string.Format("Resolving: {0}\n", num2) + text2;
		int num3;
		string text3 = ServerActionBuffer.CreateReadableAbilityRequests(m_storedAbilityRequests, AbilityRequest.AbilityResolveState.RESOLVED, out num3);
		text3 = string.Format("Resolved: {0}\n", num3) + text3;
		return string.Format("Abilities: {0}\n{1}{2}{3}", new object[]
		{
			m_storedAbilityRequests.Count,
			text,
			text2,
			text3
		});
	}

	private string BuildMovementRequestStateStr()
	{
		string text = "";
		int num = 0;
		foreach (MovementRequest movementRequest in m_storedMovementRequests)
		{
			if (movementRequest.m_resolveState == MovementRequest.MovementResolveState.QUEUED)
			{
				num++;
				text += string.Format("     {0}", movementRequest.m_actor.DisplayName);
			}
		}
		text = string.Format("Queued: {0}\n", num) + text;
		string text2 = "";
		int num2 = 0;
		foreach (MovementRequest movementRequest2 in m_storedMovementRequests)
		{
			if (movementRequest2.m_resolveState == MovementRequest.MovementResolveState.RESOLVING)
			{
				num2++;
				text2 += string.Format("     {0}", movementRequest2.m_actor.DisplayName);
			}
		}
		text2 = string.Format("Resolving: {0}\n", num2) + text2;
		string text3 = "";
		int num3 = 0;
		foreach (MovementRequest movementRequest3 in m_storedMovementRequests)
		{
			if (movementRequest3.m_resolveState == MovementRequest.MovementResolveState.RESOLVED)
			{
				num3++;
				text3 += string.Format("     {0}", movementRequest3.m_actor.DisplayName);
			}
		}
		text3 = string.Format("Resolved: {0}\n", num3) + text3;
		return string.Format("Movement: {0}\n{1}{2}{3}", new object[]
		{
			m_storedMovementRequests.Count,
			text,
			text2,
			text3
		});
	}

	public string BuildActionBufferStateString()
	{
		string text = BuildAbilityRequestStateStr();
		string text2 = BuildMovementRequestStateStr();
		return string.Concat(new string[]
		{
			"CurrentPhase: ",
			AbilityPhase.ToString(),
			", waiting for play phase to end: ",
			m_waitingForPlayPhaseEnded.ToString(),
			"\n",
			text,
			"\n\n",
			text2
		});
	}

	private void DebugDisplayBufferState()
	{
	}

	public void StoreMovementRequest(int x, int y, ActorData actor, BoardSquarePathInfo path = null)
	{
		if (HasPendingMovementRequest(actor))
		{
			Log.Error(string.Format("Actor {0} is trying to store a movement request, but a request is already stored.  Replacing old request...", actor.DisplayName));
			CancelMovementRequests(actor, false);
		}
		BoardSquare initialMoveStartSquare = actor.InitialMoveStartSquare;
		if (initialMoveStartSquare.x != x || initialMoveStartSquare.y != y)
		{
			MovementRequest item = new MovementRequest(x, y, actor, path);
			m_storedMovementRequests.Add(item);
			actor.OnMovementChanged(ActorData.MovementChangeType.MoreMovement, false);
		}
	}

	public void AppendToMovementRequest(int x, int y, ActorData actor)
	{
		foreach (MovementRequest movementRequest in m_storedMovementRequests)
		{
			if (movementRequest != null && movementRequest.m_actor == actor)
			{
				movementRequest.AppendMovement(x, y);
				actor.OnMovementChanged(ActorData.MovementChangeType.MoreMovement, false);
				break;
			}
		}
	}

	public void ProcessGroupMoveRequest(BoardSquare meetingSquare, List<ActorData> actors, ActorData requestingActor)
	{
		if (meetingSquare == null || !meetingSquare.IsValidForGameplay())
		{
			return;
		}
		GameFlowData gameFlowData = GameFlowData.Get();
		if (!gameFlowData.IsInDecisionState())
		{
			return;
		}
		TargeterUtils.SortActorsByDistanceToPos(ref actors, meetingSquare.ToVector3());
		Vector3 vector = meetingSquare.ToVector3() - requestingActor.GetFreePos();
		vector.y = 0f;
		if (vector.magnitude > 0.01f)
		{
			vector.Normalize();
		}
		else
		{
			vector = Vector3.forward;
		}
		Vector3 vector2 = new Vector3(1f, 0f, 0f);
		Vector3 zero = Vector3.zero;
		if (Mathf.Abs(vector.x) > Mathf.Abs(vector.z))
		{
			if (vector.x > 0f)
			{
				vector2 = new Vector3(-1f, 0f, 0f);
			}
			float num = 0.49f * Board.SquareSizeStatic;
			if (vector.z > 0f)
			{
				num *= -1f;
			}
			zero = new Vector3(0f, 0f, num);
		}
		else
		{
			if (vector.z >= 0f)
			{
				vector2 = new Vector3(0f, 0f, -1f);
			}
			else
			{
				vector2 = new Vector3(0f, 0f, 1f);
			}
			float num2 = 0.49f * Board.SquareSizeStatic;
			if (vector.x > 0f)
			{
				num2 *= -1f;
			}
			zero = new Vector3(num2, 0f, 0f);
		}
		Vector3 vector3 = meetingSquare.GetOccupantLoSPos() + zero;
		Vector3 endPos = vector3 + Board.SquareSizeStatic * vector2;
		List<BoardSquare> squaresInBoxByActorRadius = AreaEffectUtils.GetSquaresInBoxByActorRadius(vector3, endPos, 1f, false, requestingActor, null);
		AreaEffectUtils.SortSquaresByDistanceToPos(ref squaresInBoxByActorRadius, vector3);
		int num3 = 0;
		List<ActorData> list = new List<ActorData>();
		foreach (ActorData actorData in actors)
		{
			ActorTurnSM actorTurnSM = actorData.GetActorTurnSM();
			if (!actorData.IsDead()
				&& actorTurnSM.AmDecidingMovement()
				//&& actorData.GetTeam() == gameFlowData.ActingTeam  // rogues
				&& actorData.GetActorMovement().SquaresCanMoveTo.Count > 0)
			{
				BoardSquare boardSquare = meetingSquare;
				if (num3 < squaresInBoxByActorRadius.Count)
				{
					boardSquare = squaresInBoxByActorRadius[num3];
				}
				num3++;
				if (!actorData.CanMoveToBoardSquare(boardSquare))
				{
					boardSquare = actorData.GetActorMovement().GetClosestMoveableSquareTo(boardSquare, false);
				}
				if (boardSquare != null)
				{
					if (HasPendingMovementRequest(actorData))
					{
						CancelMovementRequests(actorData, false);
					}
					BoardSquare initialMoveStartSquare = actorData.InitialMoveStartSquare;
					if (initialMoveStartSquare != boardSquare)
					{
						BoardSquarePathInfo boardSquarePathInfo = actorData.GetActorMovement().BuildCompletePathTo(initialMoveStartSquare, boardSquare, false, null);
						if (boardSquarePathInfo != null)
						{
							StoreMovementRequest(boardSquare.x, boardSquare.y, actorData, boardSquarePathInfo);
							list.Add(actorData);
						}
					}
				}
			}
		}

		// rogues
		//if (list.Count > 0)
		//{
		//	GetPlayerActionFSM().RunGroupMovementForActors(list);
		//}
	}

	public void GatherMovementInfo(ActorData actor, out BoardSquare destination, out float queuedMovementAmount, out bool isChasing)
	{
		if (actor == null)
		{
			Debug.LogError("ServerActionBuffer trying to gather movement info for a null actor.");
			queuedMovementAmount = 0f;
			isChasing = false;
			destination = null;
			return;
		}
		queuedMovementAmount = 0f;
		isChasing = false;
		destination = actor.InitialMoveStartSquare;
		foreach (MovementRequest movementRequest in m_storedMovementRequests)
		{
			if (movementRequest != null && movementRequest.m_actor == actor)
			{
				if (movementRequest.IsChasing())
				{
					isChasing = true;
					break;
				}
				destination = movementRequest.m_targetSquare;
				if (movementRequest.m_path != null)
				{
					queuedMovementAmount = movementRequest.m_path.FindMoveCostToEnd();
					break;
				}
				Debug.LogError(string.Format("ServerActionBuffer trying to gather movement info for actor {0}, but the request's path is null.\n\tActor current board square: {1}\n\tRequested square: {2}", actor.DebugNameString(), BoardSquare.DebugString(actor.CurrentBoardSquare, true), BoardSquare.DebugString(movementRequest.m_targetSquare, true)));
				break;
			}
		}
		// rogues
		//float num = ActorMovement.CalcMoveAdjustFromExecutedActions(actor);
		//queuedMovementAmount += num;
	}

	public bool HasNormalMovementRequestOutsideOfRange(ActorData actor, float maxMovement)
	{
		if (actor != null)
		{
			foreach (MovementRequest movementRequest in m_storedMovementRequests)
			{
				if (movementRequest != null && movementRequest.m_actor == actor && !movementRequest.IsChasing() && movementRequest.m_path != null)
				{
					BoardSquarePathInfo pathEndpoint = movementRequest.m_path.GetPathEndpoint();
					float moveCost = pathEndpoint.moveCost;
					float num = 0f;
					if (pathEndpoint.prev != null)
					{
						num = pathEndpoint.prev.moveCost;
					}
					bool flag;
					if (GameplayData.Get() != null && GameplayData.Get().m_movementMaximumType == GameplayData.MovementMaximumType.CannotExceedMax)
					{
						flag = (moveCost <= maxMovement);
					}
					else
					{
						flag = (num < maxMovement);
					}
					return !flag;
				}
			}
			return false;
		}
		return false;
	}

	public BoardSquare GetModifiedMoveStartSquareFromAbilities(ActorData caster)
	{
		BoardSquare result = caster.GetCurrentBoardSquare();
		foreach (AbilityRequest abilityRequest in m_storedAbilityRequests)
		{
			if (abilityRequest != null && abilityRequest.m_caster == caster && abilityRequest.m_ability != null && abilityRequest.m_ability.CanOverrideMoveStartSquare())
			{
				BoardSquare modifiedMoveStartSquare = abilityRequest.m_ability.GetModifiedMoveStartSquare(caster, abilityRequest.m_targets);
				if (modifiedMoveStartSquare != null)
				{
					result = modifiedMoveStartSquare;
					break;
				}
			}
		}
		return result;
	}

	public List<GridPos> GetGridPosPath(ActorData actor, out bool isChasing)
	{
		List<GridPos> result = null;
		isChasing = false;
		foreach (MovementRequest movementRequest in m_storedMovementRequests)
		{
			if (movementRequest != null && movementRequest.m_actor == actor)
			{
				result = movementRequest.ToGridPosPath();
				isChasing = movementRequest.IsChasing();
			}
		}
		return result;
	}

	public void StoreChaseRequest(ActorData target, ActorData chaser, bool forceChased, bool chaserInitiated = false)
	{
		if (HasPendingMovementRequest(chaser))
		{
			CancelMovementRequests(chaser, true);
		}
		if (target != null && target != chaser)
		{
			MovementRequest movementRequest = new MovementRequest(target, chaser, forceChased);
			movementRequest.m_chaserInitiatedForceChase = (forceChased && chaserInitiated);
			m_storedMovementRequests.Add(movementRequest);
			chaser.OnMovementChanged(ActorData.MovementChangeType.MoreMovement, forceChased);
		}
	}

	private void ResolveMovmentOnRequest(MovementRequest request, ActorData actor, BoardSquare destinationSquare)
	{
		if (request != null && request.m_actor == actor && request.m_resolveState == MovementRequest.MovementResolveState.RESOLVING)
		{
			if (!(request.m_targetSquare == destinationSquare) && !actor.IsDead())
			{
				string text = (request.m_targetSquare != null) ? request.m_targetSquare.ToString() : "null";
				string text2 = (destinationSquare != null) ? destinationSquare.ToString() : "null";
				Log.Error("on resolving movement request, living actor {0} has destination square mismatch. Request target square = {1}, destSquare = {2}", new object[]
				{
					actor.DebugNameString(),
					text,
					text2
				});
			}
			request.m_resolveState = MovementRequest.MovementResolveState.RESOLVED;
			if (!actor.IsDead())
			{
				actor.GetActorTurnSM().OnMessage(TurnMessage.MOVEMENT_RESOLVED, true);
				return;
			}
		}
		else if (request != null && request.m_actor == actor && request.m_resolveState == MovementRequest.MovementResolveState.QUEUED)
		{
			Log.Error("on resolve movement request, request from actor " + actor.DebugNameString() + " is still in QUEUED state");
		}
	}

	public void RunMovementOnRequest(MovementRequest moveRequest)
	{
		if (moveRequest != null && moveRequest.m_resolveState == MovementRequest.MovementResolveState.QUEUED)
		{
			if (moveRequest.m_actor.GetCurrentBoardSquare() != moveRequest.m_targetSquare || moveRequest.m_path.next != null)
			{
				moveRequest.m_resolveState = MovementRequest.MovementResolveState.RESOLVING;
				moveRequest.m_actor.BroadcastMoveToBoardSquare(moveRequest.m_targetSquare, ActorData.MovementType.Normal, moveRequest.m_path, ActorData.TeleportType.NotATeleport, GameEventManager.EventType.Invalid, false);
				return;
			}
			moveRequest.m_resolveState = MovementRequest.MovementResolveState.RESOLVED;
			ActorTurnSM actorTurnSM = moveRequest.m_actor.GetActorTurnSM();
			if (actorTurnSM)
			{
				actorTurnSM.OnMessage(TurnMessage.MOVEMENT_RESOLVED, true);
			}
		}
	}

	private void ClearMovementRequests()
	{
		m_storedMovementRequests.Clear();
		m_removedMovementRequestsFromForceChase.Clear();
	}

	public bool HasPendingMovementRequest(ActorData actor)
	{
		bool result = false;
		foreach (MovementRequest movementRequest in m_storedMovementRequests)
		{
			if (movementRequest != null && movementRequest.m_actor == actor)
			{
				result = true;
				break;
			}
		}
		return result;
	}

	public bool HasPendingForcedChaseRequest(ActorData actor)
	{
		bool result = false;
		foreach (MovementRequest movementRequest in m_storedMovementRequests)
		{
			if (movementRequest != null && movementRequest.m_actor == actor && movementRequest.IsForcedChase())
			{
				result = true;
				break;
			}
		}
		return result;
	}

	public void PendingMovementRequestInfo(ActorData actor, out bool isMoving, out bool isChasing, out ActorData chaseTargetActor)
	{
		isMoving = false;
		isChasing = false;
		chaseTargetActor = null;
		foreach (MovementRequest movementRequest in m_storedMovementRequests)
		{
			if (movementRequest != null && movementRequest.m_actor == actor)
			{
				if (movementRequest.WasEverChasing())
				{
					isChasing = true;
					chaseTargetActor = movementRequest.m_preStabilizeChaseTarget;
				}
				else
				{
					isMoving = true;
				}
			}
		}
	}

	public bool IsChasing(ActorData chaser)
	{
		bool result = false;
		foreach (MovementRequest movementRequest in m_storedMovementRequests)
		{
			if (movementRequest != null && movementRequest.IsChasing() && movementRequest.m_actor == chaser)
			{
				result = true;
				break;
			}
		}
		return result;
	}

	public bool IsChasing(ActorData chaser, ActorData target)
	{
		bool result = false;
		foreach (MovementRequest movementRequest in m_storedMovementRequests)
		{
			if (movementRequest != null && movementRequest.IsChasing() && movementRequest.m_actor == chaser && movementRequest.m_chaseTarget == target)
			{
				result = true;
				break;
			}
		}
		return result;
	}

	public bool HasResolvingMovementRequest(ActorData fromMover)
	{
		bool result = false;
		foreach (MovementRequest movementRequest in m_storedMovementRequests)
		{
			if (movementRequest != null && movementRequest.m_actor == fromMover && movementRequest.m_resolveState == MovementRequest.MovementResolveState.RESOLVING)
			{
				result = true;
				break;
			}
		}
		return result;
	}

	public bool HasUnresolvedMovementRequest(ActorData fromMover = null)
	{
		bool result = false;
		foreach (MovementRequest movementRequest in m_storedMovementRequests)
		{
			if (movementRequest != null && (fromMover == null || movementRequest.m_actor == fromMover) && movementRequest.m_resolveState != MovementRequest.MovementResolveState.RESOLVED)
			{
				result = true;
				break;
			}
		}
		return result;
	}

	public void CancelMovementRequests(ActorData actor, bool forceChased = false)
	{
		List<MovementRequest> list = new List<MovementRequest>();
		foreach (MovementRequest movementRequest in m_storedMovementRequests)
		{
			if (movementRequest != null && movementRequest.m_actor == actor)
			{
				list.Add(movementRequest);
			}
		}
		foreach (MovementRequest movementRequest2 in list)
		{
			if (forceChased)
			{
				m_removedMovementRequestsFromForceChase.Add(movementRequest2);
			}
			m_storedMovementRequests.Remove(movementRequest2);
			movementRequest2.m_actor.OnMovementChanged(ActorData.MovementChangeType.LessMovement, forceChased);
		}
	}

	public void RemoveMovementRequestsDueToKnockback(ActorData actor)
	{
		List<MovementRequest> list = new List<MovementRequest>();
		foreach (MovementRequest movementRequest in m_storedMovementRequests)
		{
			if (movementRequest != null && movementRequest.m_actor == actor)
			{
				list.Add(movementRequest);
			}
		}
		foreach (MovementRequest movementRequest2 in list)
		{
			movementRequest2.m_actor.GetActorTurnSM().OnMessage(TurnMessage.MOVEMENT_RESOLVED, true);
			m_storedMovementRequests.Remove(movementRequest2);
			float desiredMovementOnResolve = movementRequest2.m_actor.GetActorBehavior().DesiredMovementOnResolve;
			movementRequest2.m_actor.GetActorBehavior().SetTotalMovementLostThisTurn(desiredMovementOnResolve);
		}
	}

	public List<ActorData> GetStationaryActors()
	{
		List<ActorData> list = new List<ActorData>();
		foreach (ActorData actorData in GameFlowData.Get().GetActors())
		{
			if (actorData != null && !HasPendingMovementRequest(actorData))
			{
				list.Add(actorData);
			}
		}
		return list;
	}

	public List<BoardSquare> GetReservedSquares_PreChaseStabilization()
	{
		if (!NetworkServer.active)
		{
			Log.Error("Client tried to find reserved squares; only server knows details of stabilized movement.");
			return null;
		}
		List<BoardSquare> list = new List<BoardSquare>();
		foreach (ActorData actorData in GetStationaryActors())
		{
			if (!actorData.IsDead())
			{
				list.Add(actorData.GetCurrentBoardSquare());
			}
		}
		foreach (MovementRequest movementRequest in m_storedMovementRequests)
		{
			if (!movementRequest.WasEverChasing())
			{
				list.Add(movementRequest.m_targetSquare);
			}
		}
		return list;
	}

	public List<BoardSquare> GetReservedSquares_PostChaseStabilization(ActorData actorToSkip)
	{
		if (!NetworkServer.active)
		{
			Log.Error("Client tried to find reserved squares; only server knows details of stabilized movement.");
			return null;
		}
		List<BoardSquare> list = new List<BoardSquare>();
		foreach (ActorData actorData in GetStationaryActors())
		{
			if (actorData != actorToSkip && !actorData.IsDead())
			{
				list.Add(actorData.GetCurrentBoardSquare());
			}
		}
		foreach (MovementRequest movementRequest in m_storedMovementRequests)
		{
			if (!movementRequest.IsChasing())
			{
				if (list.Contains(movementRequest.m_targetSquare))
				{
					Log.Error("Trying to find reserved squares, but found duplicates, which shouldn't happen (has movement been fully stabilized?).");
				}
				else if (movementRequest.m_actor == actorToSkip)
				{
					Log.Error("Trying to find reserved squares, but found a move request from the actor to skip, who shouldn't have made a move request yet.");
				}
				else
				{
					list.Add(movementRequest.m_targetSquare);
				}
			}
		}
		return list;
	}

	public void CancelActionRequests(ActorData actor, bool keepFutureTurnRequests)
	{
		// rogues
		//PveLog.DebugLog("Canceling Action Requests for " + actor.DebugNameString(), null);

		CancelMovementRequests(actor, false);
		CancelAbilityRequests(actor, false, keepFutureTurnRequests);
	}

	public void StoreAbilityRequest(Ability ability, AbilityData.ActionType actionType, List<AbilityTarget> targets, ActorData caster, SequenceSource parentAbilitySequenceSource = null, ChainAbilityAdditionalModInfo chainModInfo = null, bool storeForNextTurn = false)
	{
		if (HasPendingAbilityRequest(caster, false) && !ability.IsFreeAction())
		{
			CancelAbilityRequests(caster, true, false);
		}
		if ((!caster.QueuedMovementAllowsAbility && ability.GetAffectsMovement()) || ability.CanOverrideMoveStartSquare())
		{
			CancelMovementRequests(caster, false);
		}
		if (ability != null && caster != null)
		{
			AbilityRequest abilityRequest = new AbilityRequest(ability, actionType, targets, caster);
			abilityRequest.m_additionalData.m_parentAbilitySequenceSource = parentAbilitySequenceSource;
			abilityRequest.m_additionalData.m_chainModInfo = chainModInfo;
			abilityRequest.m_additionalData.m_skipTheatricsAnimEntry = ability.SkipTheatricsAnimationEntry(caster);
			if (storeForNextTurn)
			{
				m_storedAbilityRequestsForNextTurn.Add(abilityRequest);
				return;
			}
			m_storedAbilityRequests.Add(abilityRequest);
			OnAbilityRequestStored(abilityRequest);
			return;
		}

		// TODO LOW some broken code here
		//else
		//{
		//	if (caster != null)
		//	{
		//		return;
		//	}
		//	ability != null;
		//	return;
		//}
	}

	private void OnAbilityRequestStored(AbilityRequest newRequest)
	{
		Ability ability = newRequest.m_ability;
		ActorData caster = newRequest.m_caster;
		List<AbilityTarget> targets = newRequest.m_targets;
		AbilityData.ActionType actionType = newRequest.m_actionType;
		caster.GetAbilityData().SetQueuedAction(actionType, true);
		int moddedCost = ability.GetModdedCost();
		if (moddedCost > 0)
		{
			caster.ReservedTechPoints += moddedCost;
			caster.SetTechPoints(caster.TechPoints - moddedCost, false, null, null);
		}
		ability.OnAbilityQueuedDuringDecision();
		Ability[] chainAbilities = ability.GetChainAbilities();
		for (int i = 0; i < chainAbilities.Length; i++)
		{
			Ability ability2 = chainAbilities[i];
			if (!(ability2 == null))
			{
				AbilityData.ActionType actionTypeOfAbility = caster.GetAbilityData().GetActionTypeOfAbility(ability2);
				ChainAbilityAdditionalModInfo chainModInfo = null;
				if (ability.CurrentAbilityMod != null)
				{
					chainModInfo = ability.CurrentAbilityMod.GetChainModInfoAtIndex(i);
				}
				StoreAbilityRequest(ability2, actionTypeOfAbility, targets, caster, newRequest.m_additionalData.m_sequenceSource, chainModInfo); //, ability.m_runChainAbilitiesTheFollowingTurn); in rogues
			}
		}

		// TODO LOW check taunts
		// rogues?
		//if (caster.GetActorTurnSM().m_tauntRequestedForNextAbility == (int)actionType || (DebugParameters.Get() != null && DebugParameters.Get().GetParameterAsBool("AlwaysTauntAutomatically")))
		//{
		//	List<CameraShotSequence> debugTauntListForActionType = caster.GetAbilityData().GetDebugTauntListForActionType(actionType);
		//	if (!debugTauntListForActionType.IsNullOrEmpty<CameraShotSequence>())
		//	{
		//		caster.GetComponent<ActorCinematicRequests>().SendAbilityCinematicRequest(actionType, true, debugTauntListForActionType[0].m_tauntNumber, debugTauntListForActionType[0].m_uniqueTauntID);
		//	}
		//}
		//caster.GetActorTurnSM().UpdateHasStoredAbilityRequestFlag();
	}

	public static bool ShouldLogActorActions()
	{
		GameManager gameManager = GameManager.Get();
		return gameManager != null
			&& gameManager.GameConfig != null  // gameManager.GameMission != null in rogues
			&& GameFlowData.Get() != null;
	}

	private void LogActorStateForRepro(string context)
	{
		if (ServerActionBuffer.ShouldLogActorActions())
		{
			string text = string.Concat(new object[]
			{
				"{act} Turn ",
				GameFlowData.Get().CurrentTurn,
				" | -- ActorState ",
				context,
				" --\n"
			});
			foreach (ActorData actorData in GameFlowData.Get().GetActors())
			{
				if (actorData != null && actorData.PlayerIndex >= 0)
				{
					BoardSquare currentBoardSquare = actorData.GetCurrentBoardSquare();
					string text2 = (currentBoardSquare != null) ? currentBoardSquare.GetGridPos().ToStringWithCross() : "NULL";
					text = string.Concat(new object[]
					{
						text,
						"{act} ",
						actorData.DebugNameString(),
						" @square= ",
						text2,
						" | HP= ",
						actorData.HitPoints,
						" | Energy= ",
						actorData.TechPoints,
						" | Absorb= ",
						actorData.AbsorbPoints,
						" | MaxMovement= ",
						actorData.GetActorMovement().CalculateMaxHorizontalMovement(false, false),
						"\n"
					});
				}
			}
		}
	}

	private void LogRequestsForRepro(bool logAbilities, bool logMovement, string context)
	{
		if (ServerActionBuffer.ShouldLogActorActions())
		{
			string text = string.Concat(new object[]
			{
				"{act} Turn ",
				GameFlowData.Get().CurrentTurn,
				" | -- Requests ",
				context,
				" --\n"
			});
			if (logAbilities)
			{
				text += "{act} -- Ability Requests --\n";
				if (m_storedAbilityRequests.Count == 0)
				{
					text += "{act} NONE\n";
				}
				foreach (AbilityRequest abilityRequest in m_storedAbilityRequests)
				{
					text = string.Concat(new string[]
					{
						text,
						"{act} ",
						abilityRequest.m_caster.DebugNameString(),
						"\n{act}     ActionType= ",
						abilityRequest.m_actionType.ToString(),
						" | AbilityName= ",
						abilityRequest.m_ability.m_abilityName,
						"\n"
					});
					for (int i = 0; i < abilityRequest.m_targets.Count; i++)
					{
						AbilityTarget abilityTarget = abilityRequest.m_targets[i];
						text = string.Concat(new object[]
						{
							text,
							"{act}     CursorState ",
							i,
							": ",
							abilityTarget.GetDebugString(),
							"\n"
						});
					}
				}
			}
			string str = "\n";
			if (logMovement)
			{
				str += GetMovementRequestsString();
			}
		}
	}

	private string GetMovementRequestsString()
	{
		if (ServerActionBuffer.ShouldLogActorActions())
		{
			string text = "{act} -- Movement Requests --\n";
			if (m_storedMovementRequests.Count == 0)
			{
				text += "{act} NONE\n";
			}
			foreach (MovementRequest movementRequest in m_storedMovementRequests)
			{
				text = text + "{act} " + movementRequest.m_actor.DebugNameString() + "\n";
				if (movementRequest.IsChasing())
				{
					string text2;
					if (movementRequest.m_chaseTarget.GetCurrentBoardSquare() != null)
					{
						text2 = movementRequest.m_chaseTarget.GetCurrentBoardSquare().GetGridPos().ToStringWithCross();
					}
					else
					{
						text2 = "NULL";
					}
					text = string.Concat(new string[]
					{
						text,
						"{act}     Chase | ChaseTarget= ",
						movementRequest.m_chaseTarget.DebugNameString(),
						" @square= ",
						text2,
						" targetAlive= ",
						(!movementRequest.m_chaseTarget.IsDead()).ToString(),
						"\n"
					});
				}
				else
				{
					GridPos gridPos = GridPos.s_invalid;
					if (movementRequest.m_targetSquare != null)
					{
						gridPos = movementRequest.m_targetSquare.GetGridPos();
					}
					else
					{
						Log.Error("Movement Request has null target square");
					}
					text = text + "{act}     Normal | ToSquare= " + gridPos.ToStringWithCross() + "\n";
				}
			}
			return text;
		}
		return "";
	}

	public void LogActionRequests()
	{
		foreach (AbilityRequest abilityRequest in m_storedAbilityRequests)
		{
			string text = string.Format("Cast Player:{0} Ability:{1}", abilityRequest.m_caster.DisplayName, abilityRequest.m_ability.m_abilityName);
			for (int i = 0; i < abilityRequest.m_targets.Count; i++)
			{
				AbilityTarget abilityTarget = abilityRequest.m_targets[i];
				text = string.Concat(new object[]
				{
					text,
					"(",
					abilityTarget.GridPos.x,
					",",
					abilityTarget.GridPos.y,
					")"
				});
			}
			MatchLogger.Get().Log(string.Format(text));
		}
		foreach (MovementRequest movementRequest in m_storedMovementRequests)
		{
			GridPos gridPos = movementRequest.m_actor.GetCurrentBoardSquare().GetGridPos();
			string format;
			if (movementRequest.IsChasing())
			{
				GridPos gridPos2 = movementRequest.m_chaseTarget.GetGridPos();
				format = string.Format("Chase Player:{0} Src:({1},{2}) Target:{3} at ({4},{5})", new object[]
				{
					movementRequest.m_actor.DisplayName,
					gridPos.x,
					gridPos.y,
					movementRequest.m_chaseTarget.DisplayName,
					gridPos2.x,
					gridPos2.y
				});
			}
			else
			{
				GridPos gridPos3 = movementRequest.m_targetSquare.GetGridPos();
				format = string.Format("Move Player:{0} Src:({1},{2}) Dst:({3},{4})", new object[]
				{
					movementRequest.m_actor.DisplayName,
					gridPos.x,
					gridPos.y,
					gridPos3.x,
					gridPos3.y
				});
			}
			MatchLogger.Get().Log(string.Format(format));
		}
	}

	internal bool TryRunAbilityRequest(AbilityRequest request)
	{
		bool result = false;
		if (request != null && request.m_ability != null && request.m_resolveState == AbilityRequest.AbilityResolveState.QUEUED && request.m_ability.RunPriority == AbilityPhase)
		{
			RunAbilityRequest(request);
			result = true;
		}
		return result;
	}

	internal void ResolveAbilityRequest(AbilityRequest request)
	{
		request.m_resolveState = AbilityRequest.AbilityResolveState.RESOLVED;
		PassiveData passiveData = request.m_caster.GetPassiveData();
		if (passiveData != null)
		{
			passiveData.OnAbilityCastResolved(request.m_ability);
		}
	}

	public void RunAbilityRequest_FCFS(AbilityRequest request)
	{
		RunAbilityRequest(request);
	}

	private void RunAbilityRequest(AbilityRequest request)
	{
		request.m_resolveState = AbilityRequest.AbilityResolveState.RESOLVING;
		AbilityData abilityData = request.m_caster.GetAbilityData();
		abilityData.OnAbilityCast(request.m_ability);
		bool flag = request.m_ability.ShouldTriggerCooldownOnCast(request.m_targets, request.m_caster, request.m_additionalData);
		request.m_ability.Run(request.m_targets, request.m_caster, request.m_additionalData);
		if (flag)
		{
			abilityData.TriggerCooldown(request.m_actionType);
		}
		abilityData.ConsumeStock(request.m_actionType);
		request.m_caster.GetActorBehavior().CurrentTurn.RecordActionTaken(request.m_ability);
		if (!AbilityUtils.AbilityHasTag(request.m_ability, AbilityTags.DontBreakCasterInvisibilityOnCast))
		{
			ServerEffectManager.Get().OnBreakInvisibility(request.m_caster);
			if (request.m_caster != null && request.m_caster.GetPassiveData() != null)
			{
				request.m_caster.GetPassiveData().OnBreakInvisibility();
			}
			
			// custom
			request.m_caster.SetServerLastKnownPosSquare(request.m_caster.CurrentBoardSquare, "RunAbilityRequest");
		}
		if (BrushCoordinator.Get() != null)
		{
			BrushCoordinator.Get().OnCast_HandleConcealment(request.m_caster, request.m_ability, request.m_targets);
		}
		GameplayMetricHelper.IncrementAbilityUseCount(request.m_caster, request.m_ability, request.m_cinematicRequested != -1);
		if (request.m_actionType >= AbilityData.ActionType.CARD_0 && request.m_actionType <= AbilityData.ActionType.CARD_2)
		{
			GameplayMetricHelper.RecordCatalystUsed(request.m_caster, request.m_actionType);
		}
		if (request.m_cinematicRequested > 0 && request.m_tauntUniqueId >= 0)
		{
			ActorCinematicRequests actorCinematicRequests = (request.m_caster != null) ? request.m_caster.GetComponent<ActorCinematicRequests>() : null;
			if (actorCinematicRequests != null)
			{
				actorCinematicRequests.AddUsedUniqueTauntId(request.m_tauntUniqueId);
			}

			// rogues
			//request.m_caster.GetActorTurnSM().Networkm_tauntRequestedForNextAbility = -1;
		}
	}

	public bool HasPendingAbilityRequest(ActorData fromCaster, bool includeFreeActions)
	{
		bool result = false;
		foreach (AbilityRequest abilityRequest in m_storedAbilityRequests)
		{
			if (abilityRequest != null && abilityRequest.m_caster == fromCaster && (!abilityRequest.m_ability.IsFreeAction() || includeFreeActions))
			{
				result = true;
				break;
			}
		}
		return result;
	}

	public bool HasResolvingAbilityRequest(ActorData fromCaster)
	{
		bool result = false;
		foreach (AbilityRequest abilityRequest in m_storedAbilityRequests)
		{
			if (abilityRequest != null && abilityRequest.m_resolveState == AbilityRequest.AbilityResolveState.RESOLVING && (abilityRequest.m_caster == fromCaster || fromCaster == null))
			{
				result = true;
				break;
			}
		}
		return result;
	}

	public bool HasUnresolvedAbilityRequest(ActorData fromCaster)
	{
		bool result = false;
		foreach (AbilityRequest abilityRequest in m_storedAbilityRequests)
		{
			if (abilityRequest != null && abilityRequest.m_caster == fromCaster && abilityRequest.m_resolveState != AbilityRequest.AbilityResolveState.RESOLVED)
			{
				result = true;
				break;
			}
		}
		return result;
	}

	public bool HasUnresolvedAbilityRequestOfType(ActorData fromCaster, Type abilityType)
	{
		bool result = false;
		foreach (AbilityRequest abilityRequest in m_storedAbilityRequests)
		{
			if (abilityRequest != null && abilityRequest.m_caster == fromCaster && abilityRequest.m_resolveState != AbilityRequest.AbilityResolveState.RESOLVED && abilityRequest.m_ability != null && abilityRequest.m_ability.GetType() == abilityType)
			{
				result = true;
				break;
			}
		}
		return result;
	}

	public bool HasStoredAbilityRequestOfType(ActorData fromCaster, Type abilityType)
	{
		bool result = false;
		for (int i = 0; i < m_storedAbilityRequests.Count; i++)
		{
			AbilityRequest abilityRequest = m_storedAbilityRequests[i];
			if (abilityRequest != null && abilityRequest.m_caster == fromCaster && abilityRequest.m_ability != null && abilityRequest.m_ability.GetType() == abilityType)
			{
				result = true;
				break;
			}
		}
		return result;
	}

	public bool HasStoredAbilityRequestsFromAnyone()
	{
		return m_storedAbilityRequests.Count > 0;
	}

	public bool HasStoredMovementRequestsFromAnyone()
	{
		return m_storedMovementRequests.Count > 0;
	}

	public MovementRequest GetStoredMovementRequestForActor(ActorData mover)
	{
		for (int i = 0; i < m_storedMovementRequests.Count; i++)
		{
			MovementRequest movementRequest = m_storedMovementRequests[i];
			if (movementRequest.m_actor == mover)
			{
				return movementRequest;
			}
		}
		return null;
	}

	public List<AbilityTarget> GetTargetingDataOfStoredAbility(ActorData fromCaser, Type abilityType)
	{
		foreach (AbilityRequest abilityRequest in m_storedAbilityRequests)
		{
			if (abilityRequest != null && abilityRequest.m_caster == fromCaser && abilityRequest.m_ability != null && abilityRequest.m_ability.GetType() == abilityType)
			{
				return abilityRequest.m_targets;
			}
		}
		return null;
	}

	public List<ActorData> GetGatheredActorsOfStoredAbility(ActorData caster, Type abilityType)
	{
		foreach (AbilityRequest abilityRequest in m_storedAbilityRequests)
		{
			if (abilityRequest != null && abilityRequest.m_caster == caster && abilityRequest.m_ability != null && abilityRequest.m_ability.GetType() == abilityType && abilityRequest.m_additionalData.m_abilityResults.GatheredResults)
			{
				return abilityRequest.m_additionalData.m_abilityResults.HitActorList();
			}
		}
		return new List<ActorData>();
	}

	public List<Dictionary<ActorData, int>> GetGatheredHpDeltas(ActorData caster, AbilityPriority fromPhase, AbilityPriority toPhase)
	{
		List<Dictionary<ActorData, int>> list = new List<Dictionary<ActorData, int>>();
		foreach (AbilityRequest abilityRequest in m_storedAbilityRequests)
		{
			if (abilityRequest != null && abilityRequest.m_caster == caster && abilityRequest.m_ability != null && abilityRequest.m_ability.RunPriority >= fromPhase && abilityRequest.m_ability.RunPriority <= toPhase && abilityRequest.m_additionalData.m_abilityResults.GatheredResults)
			{
				list.Add(abilityRequest.m_additionalData.m_abilityResults.DamageResults);
			}
		}
		return list;
	}

	public void CollectGatheredOutgoingHitsSummary(AbilityPriority phase, ActorData caster, ServerActionBuffer.GatheredOutgoingHitsSummary summary)
	{
		if (summary == null)
		{
			return;
		}
		foreach (AbilityRequest abilityRequest in m_storedAbilityRequests)
		{
			if (abilityRequest != null && abilityRequest.m_ability.RunPriority == phase && abilityRequest.m_caster == caster)
			{
				Dictionary<ActorData, int> damageResults = abilityRequest.m_additionalData.m_abilityResults.DamageResults;
				ActorData[] hitActors = abilityRequest.m_additionalData.m_abilityResults.HitActorsArray();
				summary.UpdateValuesForResult(caster, hitActors, damageResults);
			}
		}
		ServerEffectManager.Get().CollectGatheredOutgoingHitsSummary(phase, caster, summary);
	}

	public void CountDamageAndHealFromGatheredResults(AbilityPriority phase, ActorData target, ref int damage, ref int healing)
	{
		foreach (AbilityRequest abilityRequest in m_storedAbilityRequests)
		{
			if (abilityRequest != null && abilityRequest.m_ability.RunPriority == phase)
			{
				ServerGameplayUtils.CountDamageAndHeal(abilityRequest.m_additionalData.m_abilityResults.DamageResults, target, ref damage, ref healing);
			}
		}
		ServerEffectManager.Get().CountDamageAndHealFromGatheredResults(phase, target, ref damage, ref healing);
	}

	public bool ActorIsEvading(ActorData actor)
	{
		if (GatheringFakeResults)
		{
			return false;
		}
		bool flag = false;
		foreach (AbilityRequest abilityRequest in m_storedAbilityRequests)
		{
			if (abilityRequest != null && abilityRequest.m_caster == actor && abilityRequest.m_ability != null && abilityRequest.m_ability.RunPriority == AbilityPriority.Evasion && abilityRequest.m_ability.GetMovementType() != ActorData.MovementType.None)
			{
				flag = true;
				break;
			}
		}
		if (!flag && actor.PlayerIndex == PlayerData.s_invalidPlayerIndex)
		{
			flag = m_evadeManager.HasProcessedEvadeForActor(actor);
		}
		return flag;
	}

	public int GetNumSquaresInProcessedEvade(ActorData actor)
	{
		if (m_evadeManager != null)
		{
			return m_evadeManager.GetNumSquaresInProcessedEvade(actor);
		}
		return 0;
	}

	public List<BoardSquare> GetSquaresInProcessedEvade(ActorData actor)
	{
		if (m_evadeManager != null)
		{
			return m_evadeManager.GetSquaresInProcessedEvade(actor);
		}
		return new List<BoardSquare>();
	}

	public BoardSquare GetProcessedEvadeDestination(ActorData actor)
	{
		if (m_evadeManager != null)
		{
			return m_evadeManager.GetProcessedEvadeDestination(actor);
		}
		return null;
	}

	public List<ActorTargeting.AbilityRequestData> GetPendingAbilityRequestsForTargeting(ActorData fromCaster)
	{
		List<ActorTargeting.AbilityRequestData> list = new List<ActorTargeting.AbilityRequestData>();
		foreach (AbilityRequest abilityRequest in m_storedAbilityRequests)
		{
			if (abilityRequest != null && abilityRequest.m_caster == fromCaster && !AbilityData.IsChain(abilityRequest.m_actionType))
			{
				list.Add(new ActorTargeting.AbilityRequestData(abilityRequest.m_actionType, abilityRequest.m_targets));
			}
		}
		return list;
	}

	public bool AbilityCinematicRequest(ActorData fromCaster, Ability ability, bool requested, int animTauntIndex, int tauntUniqueId)
	{
		bool result = false;
		foreach (AbilityRequest abilityRequest in m_storedAbilityRequests)
		{
			if (abilityRequest != null && abilityRequest.m_caster == fromCaster && (abilityRequest.m_ability == ability || ability.HasAbilityAsPartOfChain(abilityRequest.m_ability)) && abilityRequest.m_resolveState == AbilityRequest.AbilityResolveState.QUEUED)
			{
				if (requested)
				{
					abilityRequest.RequestCinematic(animTauntIndex, tauntUniqueId);
				}
				else
				{
					abilityRequest.CancelCinematic();
					ability.ClearAbilityMod(abilityRequest.m_caster);
				}
				result = true;
			}
		}
		return result;
	}

	public void CancelAbilityRequest(ActorData fromCaster, Ability ability, bool checkForAdditionalToCancel, bool keepFutureTurnRequests)
	{
		List<Ability> list = new List<Ability>();
		List<Ability> list2 = new List<Ability>();
		foreach (AbilityRequest abilityRequest in m_storedAbilityRequests)
		{
			if (abilityRequest != null && abilityRequest.m_caster == fromCaster && abilityRequest.m_ability == ability)
			{
				if (abilityRequest.m_resolveState == AbilityRequest.AbilityResolveState.QUEUED)
				{
					List<Ability> collection;
					HandleRemoveQueuedAbilityRequestForActor(abilityRequest, fromCaster, checkForAdditionalToCancel, out collection);
					list2.AddRange(collection);
					foreach (Ability item in abilityRequest.m_ability.GetChainAbilities())
					{
						list.Add(item);
					}
					m_storedAbilityRequests.Remove(abilityRequest);
					break;
				}
				break;
			}
		}
		if (!keepFutureTurnRequests)
		{
			foreach (AbilityRequest abilityRequest2 in m_storedAbilityRequestsForNextTurn)
			{
				if (abilityRequest2 != null && abilityRequest2.m_caster == fromCaster && abilityRequest2.m_ability == ability)
				{
					if (abilityRequest2.m_resolveState == AbilityRequest.AbilityResolveState.QUEUED)
					{
						List<Ability> collection2;
						HandleRemoveQueuedAbilityRequestForActor(abilityRequest2, fromCaster, checkForAdditionalToCancel, out collection2);
						list2.AddRange(collection2);
						foreach (Ability item2 in abilityRequest2.m_ability.GetChainAbilities())
						{
							list.Add(item2);
						}
						m_storedAbilityRequestsForNextTurn.Remove(abilityRequest2);
						break;
					}
					break;
				}
			}
		}
		foreach (Ability ability2 in list)
		{
			CancelAbilityRequest(fromCaster, ability2, false, keepFutureTurnRequests);
		}
		foreach (Ability ability3 in list2)
		{
			CancelAbilityRequest(fromCaster, ability3, false, keepFutureTurnRequests);
		}

		// rogues
		//fromCaster.GetActorTurnSM().UpdateHasStoredAbilityRequestFlag();
	}

	public void CancelAbilityRequests(ActorData fromCaster, bool keepFreeActions, bool keepFutureTurnRequests)
	{
		List<AbilityRequest> list = new List<AbilityRequest>();
		List<Ability> list2 = new List<Ability>();
		foreach (AbilityRequest abilityRequest in m_storedAbilityRequests)
		{
			if (abilityRequest != null && abilityRequest.m_caster == fromCaster && (!abilityRequest.m_ability.IsFreeAction() || !keepFreeActions))
			{
				List<Ability> list3;
				HandleRemoveQueuedAbilityRequestForActor(abilityRequest, fromCaster, true, out list3);
				list.Add(abilityRequest);
			}
		}
		if (!keepFutureTurnRequests)
		{
			foreach (AbilityRequest abilityRequest2 in m_storedAbilityRequestsForNextTurn)
			{
				if (abilityRequest2 != null && abilityRequest2.m_caster == fromCaster)
				{
					list.Add(abilityRequest2);
				}
			}
		}
		foreach (AbilityRequest item in list)
		{
			m_storedAbilityRequests.Remove(item);
			if (!keepFutureTurnRequests)
			{
				m_storedAbilityRequestsForNextTurn.Remove(item);
			}
		}
		foreach (Ability ability in list2)
		{
			CancelAbilityRequest(fromCaster, ability, false, keepFutureTurnRequests);
		}

		// rogues
		//fromCaster.GetActorTurnSM().UpdateHasStoredAbilityRequestFlag();
	}

	private void HandleRemoveQueuedAbilityRequestForActor(AbilityRequest request, ActorData fromCaster, bool checkForAdditionalToCancel, out List<Ability> othersToCancel)
	{
		othersToCancel = new List<Ability>();
		if (request != null && request.m_ability != null)
		{
			request.m_caster.GetAbilityData().SetQueuedAction(request.m_actionType, false);
			if (request.m_resolveState == AbilityRequest.AbilityResolveState.QUEUED)
			{
				if (fromCaster.GetAbilityData() != null)
				{
					fromCaster.GetAbilityData().RemoveOnRequestStatusForAbility(request.m_ability);
				}
				request.m_ability.OnAbilityUnqueuedDuringDecision();
				if (checkForAdditionalToCancel)
				{
					List<AbilityData.ActionType> otherActionsToCancelOnAbilityUnqueue = request.m_ability.GetOtherActionsToCancelOnAbilityUnqueue(request.m_caster);
					if (otherActionsToCancelOnAbilityUnqueue != null)
					{
						for (int i = 0; i < otherActionsToCancelOnAbilityUnqueue.Count; i++)
						{
							Ability abilityOfActionType = request.m_caster.GetAbilityData().GetAbilityOfActionType(otherActionsToCancelOnAbilityUnqueue[i]);
							if (abilityOfActionType != null && abilityOfActionType != request.m_ability)
							{
								othersToCancel.Add(abilityOfActionType);
							}
						}
					}
				}
				
				// custom
				int techPointCost = request.m_ability.GetModdedCost();
				if (techPointCost > 0)
				{
					fromCaster.ReservedTechPoints = Math.Max(0, fromCaster.ReservedTechPoints - techPointCost);
					fromCaster.SetTechPoints(fromCaster.TechPoints + techPointCost);
				}
				// end custom
			}
		}
	}

	private void ClearRequestsOfDeadActors()
	{
		List<AbilityRequest> list = new List<AbilityRequest>();
		foreach (AbilityRequest abilityRequest in m_storedAbilityRequests)
		{
			if (abilityRequest.m_caster.IsDead())
			{
				list.Add(abilityRequest);
			}
		}
		foreach (AbilityRequest abilityRequest2 in m_storedAbilityRequestsForNextTurn)
		{
			if (abilityRequest2.m_caster.IsDead())
			{
				list.Add(abilityRequest2);
			}
		}
		foreach (AbilityRequest abilityRequest3 in list)
		{
			abilityRequest3.m_caster.GetComponent<AbilityData>().SetQueuedAction(abilityRequest3.m_actionType, false);
			abilityRequest3.m_caster.InitialMoveStartSquare = abilityRequest3.m_caster.GetCurrentBoardSquare();
			m_storedAbilityRequests.Remove(abilityRequest3);
			m_storedAbilityRequestsForNextTurn.Remove(abilityRequest3);
		}
		List<MovementRequest> list2 = new List<MovementRequest>();
		foreach (MovementRequest movementRequest in m_storedMovementRequests)
		{
			if (movementRequest.m_actor.IsDead())
			{
				list2.Add(movementRequest);
			}
			else if (movementRequest.m_actor.GetCurrentBoardSquare() == null)
			{
				Log.Error("{0} is not dead but has no current board square, on removing requests from dead actors", new object[]
				{
					movementRequest.m_actor.DebugNameString()
				});
				list2.Add(movementRequest);
			}
		}
		foreach (MovementRequest item in list2)
		{
			m_storedMovementRequests.Remove(item);
		}
	}

	public void RestoreMovementForForceChaseImmunity()
	{
		List<MovementRequest> list = new List<MovementRequest>();
		List<MovementRequest> list2 = new List<MovementRequest>();
		foreach (MovementRequest movementRequest in m_storedMovementRequests)
		{
			if (movementRequest.IsForcedChase() && !movementRequest.m_chaserInitiatedForceChase && movementRequest.m_actor != null && movementRequest.m_actor.GetActorStatus().IsImmuneToForcedChase())
			{
				list2.Add(movementRequest);
				MovementRequest movementRequest2 = FindMovementRequestToRestore(movementRequest.m_actor);
				if (movementRequest2 != null)
				{
					list.Add(movementRequest2);
				}
			}
		}
		for (int i = m_storedMovementRequests.Count - 1; i >= 0; i--)
		{
			if (list2.Contains(m_storedMovementRequests[i]))
			{
				m_storedMovementRequests[i].m_actor.OnMovementChanged(ActorData.MovementChangeType.LessMovement, true);
				m_storedMovementRequests.RemoveAt(i);
			}
		}
		foreach (MovementRequest movementRequest3 in list)
		{
			m_storedMovementRequests.Add(movementRequest3);
			movementRequest3.m_actor.OnMovementChanged(ActorData.MovementChangeType.MoreMovement, movementRequest3.m_isForcedChase);
		}
	}

	private MovementRequest FindMovementRequestToRestore(ActorData actor)
	{
		foreach (MovementRequest movementRequest in m_removedMovementRequestsFromForceChase)
		{
			if (movementRequest != null && movementRequest.m_actor == actor)
			{
				return movementRequest;
			}
		}
		return null;
	}

	public void UpdateActorLineDataForMovementStatus(ActorData mover, bool forceRebuildLine)
	{
		bool flag = false;
		foreach (MovementRequest movementRequest in m_storedMovementRequests)
		{
			if (movementRequest != null && movementRequest.m_actor == mover)
			{
				if (movementRequest.IsChasing())
				{
					flag = true;
					mover.GetComponent<LineData>().OnMovementChanged(movementRequest.ToGridPosPath(), null, true, forceRebuildLine);
					break;
				}
				if (movementRequest.m_path != null && mover.GetComponent<LineData>() != null)
				{
					flag = true;
					BoardSquarePathInfo boardSquarePathInfo = movementRequest.m_path.Clone(null);
					m_movementStabilizer.ModifyPathForMaxMovement(movementRequest.m_actor, boardSquarePathInfo, false);
					BoardSquarePathInfo boardSquarePathInfo2 = movementRequest.m_path.Clone(null);
					m_movementStabilizer.ModifyPathForMaxMovement(movementRequest.m_actor, boardSquarePathInfo2, true);
					mover.GetComponent<LineData>().OnMovementChanged(boardSquarePathInfo.ToGridPosPath(), boardSquarePathInfo2.ToGridPosPath(), movementRequest.IsChasing(), forceRebuildLine);
					break;
				}
				break;
			}
		}
		if (!flag)
		{
			mover.GetComponent<LineData>().OnMovementChanged(null, null, false, forceRebuildLine);
		}
	}

	public void ExecuteUnexecutedHits(AbilityPriority forPhase, bool asFailsafe)
	{
		ServerGameplayUtils.AdjustStatsForDamageTakenWithEvades(forPhase, m_storedAbilityRequests);
		HandleExecuteUnexecutedHitsForAbilityRequests(m_storedAbilityRequests, forPhase, asFailsafe);
		ServerEffectManager.Get().ExecuteUnexecutedHitsForAllEffects(forPhase, asFailsafe);
		if (forPhase == AbilityPriority.Evasion)
		{
			ExecuteUnexecutedHitsForMovementStageInDistanceOrder(MovementStage.Evasion, asFailsafe);
			return;
		}
		if (forPhase == AbilityPriority.Combat_Knockback)
		{
			ExecuteUnexecutedHitsForMovementStageInDistanceOrder(MovementStage.Knockback, asFailsafe);
		}
	}

	public void HandleExecuteUnexecutedHitsForAbilityRequests(List<AbilityRequest> requests, AbilityPriority forPhase, bool asFailsafe)
	{
		foreach (AbilityRequest abilityRequest in requests)
		{
			if (abilityRequest != null && abilityRequest.m_ability.RunPriority == forPhase)
			{
				int techPointRewardForInteraction = AbilityUtils.GetTechPointRewardForInteraction(abilityRequest.m_ability, AbilityInteractionType.Cast, true, false, false);
				ActorData caster = abilityRequest.m_caster;
				if (techPointRewardForInteraction > 0)
				{
					ServerCombatManager.Get().TechPointGain(abilityRequest.m_ability, caster, caster, techPointRewardForInteraction, ServerCombatManager.TechPointChangeType.AbilityInteraction);
				}
				if (abilityRequest.m_ability.GetModdedCost() > 0)
				{
					caster.ReservedTechPoints = 0;
				}
				if (!abilityRequest.m_additionalData.m_abilityResults.HitsDoneExecuting())
				{
					abilityRequest.m_additionalData.m_abilityResults.ExecuteUnexecutedAbilityHits(asFailsafe);
				}
				if (caster.GetAbilityData() != null && !caster.HasBotController && !AbilityData.IsChain(caster.GetAbilityData().GetActionTypeOfAbility(abilityRequest.m_ability)))
				{
					abilityRequest.m_additionalData.m_abilityResults.GenerateAbilityEvent();
				}
			}
		}
	}

	public void ExecuteUnexecutedNormalMovementHits(bool asFailsafe)
	{
		ExecuteUnexecutedHitsForMovementStageInDistanceOrder(MovementStage.Normal, asFailsafe);
	}

	private void ExecuteUnexecutedHitsForMovementStageInDistanceOrder(MovementStage stage, bool asFailsafe)
	{
		bool flag = true;
		float distance = 0f;
		while (flag)
		{
			bool flag2;
			float num;
			BarrierManager.Get().ExecuteUnexecutedMovementHitsForAllBarriersForDistance(distance, stage, asFailsafe, out flag2, out num);
			bool flag3;
			float num2;
			// TODO CTF CTC
			//if (CaptureTheFlag.Get() != null)
			//{
			//	CaptureTheFlag.Get().ExecuteUnexecutedMovementResultsForDistance_Ctf(distance, stage, asFailsafe, out flag3, out num2);
			//}
			//else
			//{
				flag3 = false;
				num2 = -1f;
			//}
			bool flag4;
			float num3;
			// TODO CTF CTC
			//if (CollectTheCoins.Get() != null)
			//{
			//	CollectTheCoins.Get().ExecuteUnexecutedMovementResultsForDistance_Ctc(distance, stage, asFailsafe, out flag4, out num3);
			//}
			//else
			//{
				flag4 = false;
				num3 = -1f;
			//}
			bool flag5;
			float num4;
			ServerEffectManager.Get().ExecuteUnexecutedMovementHitsForAllEffectsForDistance(distance, stage, asFailsafe, out flag5, out num4);
			bool flag6;
			float num5;
			PowerUpManager.Get().ExecuteUnexecutedMovementHitsForAllPowerupsForDistance(distance, stage, asFailsafe, out flag6, out num5);
			flag = (flag2 || flag3 || flag4 || flag5 || flag6);
			if (flag)
			{
				float num6 = -1f;
				if (flag2 && (num < num6 || num6 == -1f))
				{
					num6 = num;
				}
				if (flag3 && (num2 < num6 || num6 == -1f))
				{
					num6 = num2;
				}
				if (flag4 && (num3 < num6 || num6 == -1f))
				{
					num6 = num3;
				}
				if (flag5 && (num4 < num6 || num6 == -1f))
				{
					num6 = num4;
				}
				if (flag6 && (num5 < num6 || num6 == -1f))
				{
					num6 = num5;
				}
				distance = num6;
			}
		}
	}

	public List<ActorData> IdentifyActorsDyingBeforeKnockbackMovement()
	{
		Dictionary<ActorData, int> dictionary = new Dictionary<ActorData, int>();
		List<ActorData> actors = GameFlowData.Get().GetActors();
		foreach (ActorData key in actors)
		{
			dictionary.Add(key, 0);
		}
		foreach (AbilityRequest abilityRequest in m_storedAbilityRequests)
		{
			if (abilityRequest != null && abilityRequest.m_ability.RunPriority == AbilityPriority.Combat_Knockback)
			{
				ServerGameplayUtils.IntegrateHpDeltas(abilityRequest.m_additionalData.m_abilityResults.DamageResults, ref dictionary);
			}
		}
		ServerEffectManager.Get().IntegrateHpDeltasForEffects(AbilityPriority.Combat_Knockback, ref dictionary, false);
		List<ActorData> list = new List<ActorData>();
		foreach (ActorData actorData in actors)
		{
			int num = actorData.UnresolvedHealing + actorData.AbsorbPoints - actorData.UnresolvedDamage;
			int num2 = dictionary[actorData];
			if (actorData.HitPoints + num + num2 <= 0)
			{
				list.Add(actorData);
			}
		}
		return list;
	}

	public List<AbilityRequest> GetAllStoredAbilityRequests()
	{
		return m_storedAbilityRequests;
	}

	public List<MovementRequest> GetAllStoredMovementRequests()
	{
		return m_storedMovementRequests;
	}

	public void RecordAbilityAlertInitiator(ActorData actor)
	{
		int currentTurn = GameFlowData.Get().CurrentTurn;
		if (m_combatInitiator == null || currentTurn > m_combatInitiatorRecordTurn)
		{
			m_combatInitiator = actor;
			m_combatInitiatorRecordTurn = currentTurn;
		}
	}

	public void ClearAbilityAlertInitiator()
	{
		m_combatInitiator = null;
		m_combatInitiatorRecordTurn = -1;
	}

	public ActorData GetAbilityAlertInitiator(int turn)
	{
		if (turn == m_combatInitiatorRecordTurn)
		{
			return m_combatInitiator;
		}
		return null;
	}

	// rogues
	//public PlayerActionStateMachine GetPlayerActionFSM()
	//{
	//	return m_playerActionFsm;
	//}

	public ServerEvadeManager GetEvadeManager()
	{
		return m_evadeManager;
	}

	public ServerKnockbackManager GetKnockbackManager()
	{
		return m_knockbackManager;
	}

	public ServerMovementStabilizer GetMoveStabilizer()
	{
		return m_movementStabilizer;
	}

	private void MirrorProcessed()
	{
	}

	// custom
	public ActionBufferPhase GetCurrentActionPhase()
    {
		return m_sharedActionBuffer?.Networkm_actionPhase ?? ActionBufferPhase.Done;
	}

	public class GatheredOutgoingHitsSummary
	{
		public int m_damageTotal;

		public int m_healingTotal;

		public int m_numHitsOnEnemies;

		public int m_numHitsOnAllies;

		public int m_numDamagingHits;

		public int m_numHealingHits;

		public void UpdateValuesForResult(ActorData caster, ActorData[] hitActors, Dictionary<ActorData, int> hpDelta)
		{
			foreach (ActorData actorData in hitActors)
			{
				if (caster.GetTeam() == actorData.GetTeam())
				{
					m_numHitsOnAllies++;
					if (hpDelta.ContainsKey(actorData) && hpDelta[actorData] > 0)
					{
						m_numHealingHits++;
						m_healingTotal += hpDelta[actorData];
					}
				}
				else
				{
					m_numHitsOnEnemies++;
					if (hpDelta.ContainsKey(actorData) && hpDelta[actorData] < 0)
					{
						m_numDamagingHits++;
						m_damageTotal -= hpDelta[actorData];
					}
				}
			}
		}
	}

	// removed in rogues
	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		bool result = default(bool);
		return result;
	}

	// removed in rogues
	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
	}
}
#endif

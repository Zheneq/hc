// ROGUES
// SERVER
using System;
using System.Collections.Generic;
using System.Linq;
//using Mirror;
using UnityEngine;
using UnityEngine.Networking;

// was empty in reactor
public class ServerActionBuffer : NetworkBehaviour
{
#if SERVER
	private static ServerActionBuffer s_instance;
	private SharedActionBuffer m_sharedActionBuffer;
	private ServerEvadeManager m_evadeManager;
	private ServerKnockbackManager m_knockbackManager;
	private ServerMovementStabilizer m_movementStabilizer;
	//private PlayerActionStateMachine m_playerActionFsm; // rogues
	
	private List<AbilityRequest> m_storedAbilityRequests;
	private List<MovementRequest> m_storedMovementRequests;
	// private List<AbilityRequest> m_storedAbilityRequestsForNextTurn; // rogues
	private bool m_waitingForPlayPhaseEnded;
	private List<MovementRequest> m_removedMovementRequestsFromForceChase;
	internal bool m_gatheringFakeResults = true; // no default value in rogues

	// custom
	internal Dictionary<Team, List<BoardSquare>> m_tempReservedSquaresForAbilitySpoil = new Dictionary<Team, List<BoardSquare>>();
	// rogues
	// internal List<BoardSquare> m_tempReservedSquaresForAbilitySpoil = new List<BoardSquare>();

	private float m_lastAbilityPhaseSet;
	private AbilityPriority m_abilityPhase;
	private ActionBufferPhase m_actionPhase;
	private List<ActorData> m_actorsVisibleUntilEndOfPhase; // was never populated in rogues
	// private ActorData m_combatInitiator; // rogues

	// private int m_combatInitiatorRecordTurn = -1; // rogues

	public static bool c_clientOnlySequences = SequenceManager.c_clientOnlySequences;

	private const string c_actionLogSearchMarker = "{act} ";
	
	// custom
	public int LastTurnWithActions { get; private set; }

	public void MarkAction()
	{
		LastTurnWithActions = GameFlowData.Get().CurrentTurn;
	}
	// end custom

	public static ServerActionBuffer Get()
	{
		return s_instance;
	}

	public bool GatheringFakeResults
	{
		get => m_gatheringFakeResults;
		set  // private in rogues
		{
			if (m_gatheringFakeResults != value)
			{
				m_gatheringFakeResults = value;
				Log.Info($"Now gathering {(value ? "fake" : "real")} results"); // custom
			}
		}
	}

	public AbilityPriority AbilityPhase
	{
		get => m_abilityPhase;
		private set
		{
			if (m_abilityPhase != value)
			{
				// TODO SAB - client resets CurrentlyVisibleForAbilityCast = false, MovedForEvade = false for all actors
				if (GameplayData.Get().m_resolveDamageBetweenAbilityPhases
				    || (GameplayData.Get().m_resolveDamageAfterEvasion && m_abilityPhase == AbilityPriority.Evasion))
				{
					ServerCombatManager.Get().ResolveHitPoints();
					ServerCombatManager.Get().ResolveTechPoints();
				}
				
				m_abilityPhase = value;
				SynchronizeSharedData();
			}
			m_lastAbilityPhaseSet = Time.time;
		}
	}
	
	// custom
	public ActionBufferPhase ActionPhase
	{
		get => m_actionPhase;
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
		s_instance = this;
		if (NetworkServer.active)
		{
			m_evadeManager = new ServerEvadeManager();
			m_knockbackManager = new ServerKnockbackManager();
			m_movementStabilizer = new ServerMovementStabilizer();
			GameObject sharedActionBufferPrefab = NetworkedSharedGameplayPrefabs.GetSharedActionBufferPrefab();
			if (sharedActionBufferPrefab != null)
			{
				GameObject sharedBuffer = Instantiate(sharedActionBufferPrefab, Vector3.zero, Quaternion.identity);
				NetworkServer.Spawn(sharedBuffer);
				DontDestroyOnLoad(sharedBuffer);
				m_sharedActionBuffer = sharedBuffer.GetComponent<SharedActionBuffer>();
			}
			m_actorsVisibleUntilEndOfPhase = new List<ActorData>();
		}
		m_storedAbilityRequests = new List<AbilityRequest>();
		m_storedMovementRequests = new List<MovementRequest>();
		// m_storedAbilityRequestsForNextTurn = new List<AbilityRequest>(); // rogues
		m_removedMovementRequestsFromForceChase = new List<MovementRequest>();
		
		//m_playerActionFsm = new PlayerActionStateMachine(); // rogues
	} 

	private void OnDestroy()
	{
		if (NetworkServer.active && m_sharedActionBuffer != null)
		{
			NetworkServer.Destroy(m_sharedActionBuffer.gameObject);
		}
		s_instance = null;
	}

	private void Start()
	{
		// TODO SAB - we should probably keep rogues version. Does it even affect anything?
		// rogues
		// AbilityPhase = AbilityUtils.GetHighestAbilityPriority();
		// custom
		AbilityPhase = AbilityPriority.INVALID;
		
		// TODO SAB - init ActionPhase?
	}

	// private void Update()
	// {
	// 	DebugDisplayBufferState();
	//
	// 	// rogues
	// 	//if (NetworkServer.active && m_playerActionFsm != null)
	// 	//{
	// 	//	m_playerActionFsm.OnUpdate();
	// 	//}
	// }

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
		return TimeSpentInAbilityPhase <= 0f;
	}

	public float TimeSpentInAbilityPhase => Time.time - m_lastAbilityPhaseSet;

	public void SynchronizePositionsOfActorsParticipatingInPhase(AbilityPriority phase)
	{
		if (phase == AbilityPriority.INVALID)
		{
			Log.Error("Calling SynchronizePositionActorsParticipatingInPhase for the 'INVALID' phase.");
			return;
		}
		foreach (AbilityRequest abilityRequest in GetAllStoredAbilityRequests())
		{
			if (abilityRequest.m_ability.RunPriority != phase)
			{
				continue;
			}
			
			if (abilityRequest.m_caster != null)
			{
				abilityRequest.m_caster.SynchronizeTeamSensitiveData();
			}
			
			foreach (ActorData hitActor in abilityRequest.m_additionalData.m_abilityResults.HitActorsArray())
			{
				hitActor.SynchronizeTeamSensitiveData();
			}
		}
		
		foreach (KeyValuePair<ActorData, List<Effect>> keyValuePair in ServerEffectManager.Get().GetAllActorEffects())
		{
			foreach (Effect effect in keyValuePair.Value)
			{
				if (!effect.HasResolutionAction(phase))
				{
					continue;
				}
				
				if (effect.Caster != null)
				{
					effect.Caster.SynchronizeTeamSensitiveData();
				}
				
				if (effect.Target != null)
				{
					effect.Target.SynchronizeTeamSensitiveData();
				}
				
				foreach (ActorData hitActor in effect.GetResultsForPhase(phase, true).HitActorsArray())
				{
					hitActor.SynchronizeTeamSensitiveData();
				}
			}
		}
		
		foreach (Effect effect in ServerEffectManager.Get().GetWorldEffects())
		{
			if (!effect.HasResolutionAction(phase))
			{
				continue;
			}
			
			if (effect.Caster != null)
			{
				effect.Caster.SynchronizeTeamSensitiveData();
			}
			
			foreach (ActorData hitActor in effect.GetResultsForPhase(phase, true).HitActorsArray())
			{
				hitActor.SynchronizeTeamSensitiveData();
			}
		}
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

	// TODO SAB - denied movement stat? - not called
	private void TrackDesiredMovementAmountOnResolve()
	{
		foreach (MovementRequest movementRequest in m_storedMovementRequests)
		{
			ActorData actor = movementRequest.m_actor;
			if (actor == null || actor.GetActorBehavior() == null)
			{
				continue;
			}
			
			if (movementRequest.IsChasing())
			{
				if (movementRequest.m_chaseTarget != null && movementRequest.m_chaseTarget.GetCurrentBoardSquare() != null)
				{
					BoardSquare targetSquare = actor.GetActorMovement()
						.GetClosestMoveableSquareTo(movementRequest.m_chaseTarget.GetCurrentBoardSquare(), false);
					if (targetSquare != null)
					{
						BoardSquarePathInfo path = actor.GetActorMovement().BuildPathTo(actor.InitialMoveStartSquare, targetSquare);
						if (path != null)
						{
							actor.GetActorBehavior().TrackDesiredMovementOnResolveStart(path.FindMoveCostToEnd());
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

	// TODO SAB - denied movement stat? - not called
	private void SetSquareRequestedForMovementMetricsForActors()
	{
		foreach (ActorData actorData in GameFlowData.Get().GetActors())
		{
			bool found = false;
			if (actorData.IsHumanControlled())
			{
				foreach (MovementRequest movementRequest in m_storedMovementRequests)
				{
					if (movementRequest.m_actor == actorData)
					{
						actorData.SetSquareRequestedForMovementMetrics(
							movementRequest.m_chaseTarget != null
								? movementRequest.m_chaseTarget.GetCurrentBoardSquare()
								: movementRequest.m_targetSquare);
						found = true;
						break;
					}
				}
			}
			if (!found)
			{
				actorData.SetSquareRequestedForMovementMetrics(null);
			}
		}
	}

	// TODO LOW SAB - currently inlined in GatherAbilities
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
	
	private void OnAbilityPhaseStart()
	{
		OnPhaseStartForRequestedAbilities(AbilityPhase);
	}

	private void OnAbilityPhaseEnd(AbilityPriority oldPhase)
	{
		// TODO HACK
		// Request is supposed to be resolved on the final Update in ActorAnimation
		// but in some cases it doesn't reach the end.
		// custom
		foreach (AbilityRequest abilityRequest in m_storedAbilityRequests)
		{
			if (abilityRequest.m_resolveState == AbilityRequest.AbilityResolveState.RESOLVING)
			{
				ResolveAbilityRequest(abilityRequest);
			}
		}
		// end custom
			
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
				if (SpawnPointManager.Get() != null
				    && SpawnPointManager.Get().m_respawnActorsCanBeHitDuringMovement
				    && actorData.NextRespawnTurn > 0)
				{
					actorData.IgnoreForAbilityHits = false;
				}
			}
		}
		
		foreach (ActorData actorData in m_actorsVisibleUntilEndOfPhase)
		{
			actorData.VisibleTillEndOfPhase = false;
		}
		m_actorsVisibleUntilEndOfPhase.Clear();
		
		foreach (AbilityRequest abilityRequest in m_storedAbilityRequests)
		{
			if (abilityRequest.m_ability.GetRunPriority() == oldPhase
			    && abilityRequest.m_ability.GetStatusToApplyWhenRequested().Count > 0)
			{
				abilityRequest.m_caster.GetAbilityData().AddOnRequestStatusForAbility(abilityRequest.m_ability);
			}
		}
	}

	public void MarkVisibleTillEndOfPhase(ActorData actor)
	{
		// custom, was empty in rogues
		if (!actor.VisibleTillEndOfPhase)
		{
			actor.VisibleTillEndOfPhase = true;
		}
		if (!m_actorsVisibleUntilEndOfPhase.Contains(actor))
		{
			m_actorsVisibleUntilEndOfPhase.Add(actor);
		}
	}

	public void OnTurnStart()
	{
		SetSquareAtPhaseStartForActors();

		// rogues
		//m_playerActionFsm.TransitionToState(PlayerActionStateMachine.StateFlag.WaitingForInput);
		//Team actingTeam = GameFlowData.Get().ActingTeam;

		m_storedAbilityRequests.Clear();
		// rogues
		// foreach (AbilityRequest abilityRequest in m_storedAbilityRequestsForNextTurn)
		// {
		// 	if (actingTeam == abilityRequest.m_caster.GetTeam() &&
		// 		!abilityRequest.m_caster.IsDead())
		// 	{
		// 		m_storedAbilityRequests.Add(abilityRequest);
		// 		OnAbilityRequestStored(abilityRequest);
		// 		GetPlayerActionFSM().RunQueuedActionsFromActor(abilityRequest.m_caster);
		// 	}
		// }
		ClearMovementRequests();
		// rogues
		//m_storedAbilityRequestsForNextTurn.RemoveAll((AbilityRequest r) => r.m_caster.GetTeam() == actingTeam);
	}
	
	// custom - TODO SAB - fake results - probably needs to be removed?
	public void OnTurnEnd()
	{
		GatheringFakeResults = true;
	}

	public void ClearNormalMovementResults()
	{
		ServerEffectManager.Get().ClearAllEffectResultsForNormalMovement();
		BarrierManager.Get().ClearAllBarrierResultsForNormalMovement();
		PowerUpManager.Get().ClearAllPowerupResultsForNormalMovement();
		if (CaptureTheFlag.Get() != null)
		{
			CaptureTheFlag.Get().ClearNormalMovementResults();
		}
		if (CollectTheCoins.Get() != null)
		{
			CollectTheCoins.Get().ClearNormalMovementResults();
		}
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
		Bounds result = new Bounds();
		bool hasValue = false;
		foreach (ActorData actorData in GameFlowData.Get().GetActors())
		{
			if (actorData != null
			    && !actorData.IsDead()
			    && actorData.GetCurrentBoardSquare() != null
			    && !actorData.IgnoreForAbilityHits
			    && (actorData.GetTeam() == team || actorData.IsActorVisibleToAnyEnemy()))
			{
				if (!hasValue)
				{
					result = actorData.GetCurrentBoardSquare().CameraBounds;
					hasValue = true;
				}
				else
				{
					result.Encapsulate(actorData.GetCurrentBoardSquare().CameraBounds);
				}
			}
		}
		if (!hasValue)
		{
			BoardSquare centerSquare = Board.Get().GetSquareFromIndex(Board.Get().GetMaxX() / 2, Board.Get().GetMaxY() / 2);
			if (centerSquare != null)
			{
				result = centerSquare.CameraBounds;
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
		center.y = 0.5f * Theatrics.ActorAnimation.c_minBoundsHeight + Board.Get().BaselineHeight;
		size.y = Theatrics.ActorAnimation.c_minBoundsHeight;
		result = new Bounds(center, size);
		return result;
	}

	private void EncapsulateMovementBoundsForAllyPath(BoardSquarePathInfo path, ref Bounds bounds)
	{
		if (path == null)
		{
			return;
		}
		
		for (BoardSquarePathInfo step = path; step != null; step = step.next)
		{
			if (step.square != null)
			{
				bounds.Encapsulate(step.square.CameraBounds);
			}
		}
	}

	private void EncapsulateMovementBoundsForEnemyPath(BoardSquarePathInfo path, ref Bounds bounds)
	{
		if (path == null)
		{
			return;
		}
		
		BoardSquarePathInfo step = path;
		BoardSquarePathInfo lastVisibleStep = null;
		while (step != null)
		{
			if (step.m_visibleToEnemies
			    || step.m_updateLastKnownPos
			    || step.m_moverDiesHere
			    || step.m_moverHasGameplayHitHere)
			{
				lastVisibleStep = step;
			}
			else if (step.prev != null && (step.prev.m_visibleToEnemies || step.prev.m_moverHasGameplayHitHere))
			{
				lastVisibleStep = step;
			}
			step = step.next;
		}
		if (lastVisibleStep != null)
		{
			for (step = path; step != null; step = step.next)
			{
				if (step.square != null)
				{
					bounds.Encapsulate(step.square.CameraBounds);
				}
				if (step == lastVisibleStep)
				{
					break;
				}
			}
		}
	}

	private static string CreateReadableAbilityRequests(
		List<AbilityRequest> abilityRequests,
		AbilityRequest.AbilityResolveState matchingState,
		out int numMatching)
	{
		numMatching = 0;
		List<string> list = new List<string>(abilityRequests.Count);
		foreach (AbilityRequest abilityRequest in abilityRequests)
		{
			if (abilityRequest.m_resolveState != matchingState)
			{
				continue;
			}
			
			numMatching++;
			
			// broken code
			// if (abilityRequest.m_targets == null)
			// {
			// }
			
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
			list.Add($"     {abilityRequest.m_caster.DisplayName}'s {abilityRequest.m_ability.m_abilityName}\n     @ {arg}\n");
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
		string textQueued = CreateReadableAbilityRequests(m_storedAbilityRequests, AbilityRequest.AbilityResolveState.QUEUED, out int numQueued);
		textQueued = $"Queued: {numQueued}\n" + textQueued;
		string textResolving = CreateReadableAbilityRequests(m_storedAbilityRequests, AbilityRequest.AbilityResolveState.RESOLVING, out int numResolving);
		textResolving = $"Resolving: {numResolving}\n" + textResolving;
		string textResolved = CreateReadableAbilityRequests(m_storedAbilityRequests, AbilityRequest.AbilityResolveState.RESOLVED, out int numResolved);
		textResolved = $"Resolved: {numResolved}\n" + textResolved;
		return $"Abilities: {m_storedAbilityRequests.Count}\n{textQueued}{textResolving}{textResolved}";
	}

	private string BuildMovementRequestStateStr()
	{
		string textQueued = "";
		int numQueued = 0;
		foreach (MovementRequest movementRequest in m_storedMovementRequests)
		{
			if (movementRequest.m_resolveState == MovementRequest.MovementResolveState.QUEUED)
			{
				numQueued++;
				textQueued += $"     {movementRequest.m_actor.DisplayName}";
			}
		}
		textQueued = $"Queued: {numQueued}\n" + textQueued;
		string textResolving = "";
		int numResolving = 0;
		foreach (MovementRequest movementRequest in m_storedMovementRequests)
		{
			if (movementRequest.m_resolveState == MovementRequest.MovementResolveState.RESOLVING)
			{
				numResolving++;
				textResolving += $"     {movementRequest.m_actor.DisplayName}";
			}
		}
		textResolving = $"Resolving: {numResolving}\n" + textResolving;
		string textResolved = "";
		int numResolved = 0;
		foreach (MovementRequest movementRequest in m_storedMovementRequests)
		{
			if (movementRequest.m_resolveState == MovementRequest.MovementResolveState.RESOLVED)
			{
				numResolved++;
				textResolved += $"     {movementRequest.m_actor.DisplayName}";
			}
		}
		textResolved = $"Resolved: {numResolved}\n" + textResolved;
		return $"Movement: {m_storedMovementRequests.Count}\n{textQueued}{textResolving}{textResolved}";
	}

	public string BuildActionBufferStateString()
	{
		return string.Concat(
			"CurrentPhase: ",
			AbilityPhase.ToString(),
			", waiting for play phase to end: ",
			m_waitingForPlayPhaseEnded.ToString(),
			"\n",
			BuildAbilityRequestStateStr(),
			"\n\n",
			BuildMovementRequestStateStr());
	}

	// private void DebugDisplayBufferState()
	// {
		// empty in rogues; called in update, so if anything were here, it would spam a lot
	// }

	public void StoreMovementRequest(int x, int y, ActorData actor, BoardSquarePathInfo path = null)
	{
		if (HasPendingMovementRequest(actor))
		{
			Log.Error($"Actor {actor.DisplayName} is trying to store a movement request, but a request is already stored.  Replacing old request...");
			CancelMovementRequests(actor, false);
		}
		BoardSquare initialMoveStartSquare = actor.InitialMoveStartSquare;
		if (initialMoveStartSquare.x != x || initialMoveStartSquare.y != y)
		{
			MovementRequest item = new MovementRequest(x, y, actor, path);
			m_storedMovementRequests.Add(item);
			actor.OnMovementChanged(ActorData.MovementChangeType.MoreMovement);
		}
	}

	public void AppendToMovementRequest(int x, int y, ActorData actor)
	{
		foreach (MovementRequest movementRequest in m_storedMovementRequests)
		{
			if (movementRequest != null && movementRequest.m_actor == actor)
			{
				movementRequest.AppendMovement(x, y);
				actor.OnMovementChanged(ActorData.MovementChangeType.MoreMovement);
				break;
			}
		}
	}

	// rogues
	// public void ProcessGroupMoveRequest(BoardSquare meetingSquare, List<ActorData> actors, ActorData requestingActor)
	// {
	// 	if (meetingSquare == null || !meetingSquare.IsValidForGameplay())
	// 	{
	// 		return;
	// 	}
	// 	GameFlowData gameFlowData = GameFlowData.Get();
	// 	if (!gameFlowData.IsInDecisionState())
	// 	{
	// 		return;
	// 	}
	// 	TargeterUtils.SortActorsByDistanceToPos(ref actors, meetingSquare.ToVector3());
	// 	Vector3 vector = meetingSquare.ToVector3() - requestingActor.GetFreePos();
	// 	vector.y = 0f;
	// 	if (vector.magnitude > 0.01f)
	// 	{
	// 		vector.Normalize();
	// 	}
	// 	else
	// 	{
	// 		vector = Vector3.forward;
	// 	}
	// 	Vector3 vector2 = new Vector3(1f, 0f, 0f);
	// 	Vector3 zero = Vector3.zero;
	// 	if (Mathf.Abs(vector.x) > Mathf.Abs(vector.z))
	// 	{
	// 		if (vector.x > 0f)
	// 		{
	// 			vector2 = new Vector3(-1f, 0f, 0f);
	// 		}
	// 		float num = 0.49f * Board.SquareSizeStatic;
	// 		if (vector.z > 0f)
	// 		{
	// 			num *= -1f;
	// 		}
	// 		zero = new Vector3(0f, 0f, num);
	// 	}
	// 	else
	// 	{
	// 		if (vector.z >= 0f)
	// 		{
	// 			vector2 = new Vector3(0f, 0f, -1f);
	// 		}
	// 		else
	// 		{
	// 			vector2 = new Vector3(0f, 0f, 1f);
	// 		}
	// 		float num2 = 0.49f * Board.SquareSizeStatic;
	// 		if (vector.x > 0f)
	// 		{
	// 			num2 *= -1f;
	// 		}
	// 		zero = new Vector3(num2, 0f, 0f);
	// 	}
	// 	Vector3 vector3 = meetingSquare.GetOccupantLoSPos() + zero;
	// 	Vector3 endPos = vector3 + Board.SquareSizeStatic * vector2;
	// 	List<BoardSquare> squaresInBoxByActorRadius = AreaEffectUtils.GetSquaresInBoxByActorRadius(vector3, endPos, 1f, false, requestingActor, null);
	// 	AreaEffectUtils.SortSquaresByDistanceToPos(ref squaresInBoxByActorRadius, vector3);
	// 	int num3 = 0;
	// 	List<ActorData> list = new List<ActorData>();
	// 	foreach (ActorData actorData in actors)
	// 	{
	// 		ActorTurnSM actorTurnSM = actorData.GetActorTurnSM();
	// 		if (!actorData.IsDead()
	// 			&& actorTurnSM.AmDecidingMovement()
	// 			&& actorData.GetTeam() == gameFlowData.ActingTeam
	// 			&& actorData.GetActorMovement().SquaresCanMoveTo.Count > 0)
	// 		{
	// 			BoardSquare boardSquare = meetingSquare;
	// 			if (num3 < squaresInBoxByActorRadius.Count)
	// 			{
	// 				boardSquare = squaresInBoxByActorRadius[num3];
	// 			}
	// 			num3++;
	// 			if (!actorData.CanMoveToBoardSquare(boardSquare))
	// 			{
	// 				boardSquare = actorData.GetActorMovement().GetClosestMoveableSquareTo(boardSquare, false);
	// 			}
	// 			if (boardSquare != null)
	// 			{
	// 				if (HasPendingMovementRequest(actorData))
	// 				{
	// 					CancelMovementRequests(actorData, false);
	// 				}
	// 				BoardSquare initialMoveStartSquare = actorData.InitialMoveStartSquare;
	// 				if (initialMoveStartSquare != boardSquare)
	// 				{
	// 					BoardSquarePathInfo boardSquarePathInfo = actorData.GetActorMovement().BuildCompletePathTo(initialMoveStartSquare, boardSquare, false, null);
	// 					if (boardSquarePathInfo != null)
	// 					{
	// 						StoreMovementRequest(boardSquare.x, boardSquare.y, actorData, boardSquarePathInfo);
	// 						list.Add(actorData);
	// 					}
	// 				}
	// 			}
	// 		}
	// 	}
	//
	// 	if (list.Count > 0)
	// 	{
	// 		GetPlayerActionFSM().RunGroupMovementForActors(list);
	// 	}
	// }

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
			if (movementRequest == null || movementRequest.m_actor != actor)
			{
				continue;
			}
			
			if (movementRequest.IsChasing())
			{
				isChasing = true;
				break;
			}
			destination = movementRequest.m_targetSquare;
			if (movementRequest.m_path != null)
			{
				queuedMovementAmount = movementRequest.m_path.FindMoveCostToEnd();
			}
			else
			{
				Debug.LogError(
					$"ServerActionBuffer trying to gather movement info for actor {actor.DebugNameString()}, but the request's path is null.\n"
					+ $"\tActor current board square: {BoardSquare.DebugString(actor.CurrentBoardSquare, true)}\n"
					+ $"\tRequested square: {BoardSquare.DebugString(movementRequest.m_targetSquare, true)}");
			}
			break;
		}
		// rogues
		//float num = ActorMovement.CalcMoveAdjustFromExecutedActions(actor);
		//queuedMovementAmount += num;
	}

	public bool HasNormalMovementRequestOutsideOfRange(ActorData actor, float maxMovement)
	{
		if (actor == null)
		{
			return false;
		}
		
		foreach (MovementRequest movementRequest in m_storedMovementRequests)
		{
			if (movementRequest == null
			    || movementRequest.m_actor != actor
			    || movementRequest.IsChasing()
			    || movementRequest.m_path == null)
			{
				continue;
			}
			
			BoardSquarePathInfo pathEndpoint = movementRequest.m_path.GetPathEndpoint();
			float moveCost = pathEndpoint.moveCost;
			float prevMoveCost = 0f;
			if (pathEndpoint.prev != null)
			{
				prevMoveCost = pathEndpoint.prev.moveCost;
			}
			bool isValid;
			if (GameplayData.Get() != null
			    && GameplayData.Get().m_movementMaximumType == GameplayData.MovementMaximumType.CannotExceedMax)
			{
				isValid = moveCost <= maxMovement;
			}
			else
			{
				isValid = prevMoveCost < maxMovement;
			}
			return !isValid;
		}
		return false;
	}

	public BoardSquare GetModifiedMoveStartSquareFromAbilities(ActorData caster)
	{
		BoardSquare result = caster.GetCurrentBoardSquare();
		foreach (AbilityRequest abilityRequest in m_storedAbilityRequests)
		{
			if (abilityRequest != null
			    && abilityRequest.m_caster == caster
			    && abilityRequest.m_ability != null
			    && abilityRequest.m_ability.CanOverrideMoveStartSquare())
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
			MovementRequest movementRequest = new MovementRequest(target, chaser, forceChased)
			{
				m_chaserInitiatedForceChase = forceChased && chaserInitiated
			};
			m_storedMovementRequests.Add(movementRequest);
			chaser.OnMovementChanged(ActorData.MovementChangeType.MoreMovement, forceChased);
		}
	}

	// TODO SAB - not called - currently called in HandleUpdateResolve? (OnMessage(TurnMessage.MOVEMENT_RESOLVED))
	private void ResolveMovmentOnRequest(MovementRequest request, ActorData actor, BoardSquare destinationSquare)
	{
		if (request != null
		    && request.m_actor == actor
		    && request.m_resolveState == MovementRequest.MovementResolveState.RESOLVING)
		{
			if (request.m_targetSquare != destinationSquare && !actor.IsDead())
			{
				string textTargetSquare = request.m_targetSquare != null
					? request.m_targetSquare.ToString()
					: "null";
				string textDestSquare = destinationSquare != null
					? destinationSquare.ToString()
					: "null";
				Log.Error(
					"on resolving movement request, living actor {0} has destination square mismatch. Request target square = {1}, destSquare = {2}",
					actor.DebugNameString(),
					textTargetSquare,
					textDestSquare);
			}
			request.m_resolveState = MovementRequest.MovementResolveState.RESOLVED;
			if (!actor.IsDead())
			{
				actor.GetActorTurnSM().OnMessage(TurnMessage.MOVEMENT_RESOLVED);
			}
		}
		else if (request != null
		         && request.m_actor == actor
		         && request.m_resolveState == MovementRequest.MovementResolveState.QUEUED)
		{
			Log.Error("on resolve movement request, request from actor " + actor.DebugNameString() + " is still in QUEUED state");
		}
	}

	public void RunMovementOnRequest(MovementRequest moveRequest)
	{
		if (moveRequest == null
		    || moveRequest.m_resolveState != MovementRequest.MovementResolveState.QUEUED)
		{
			return;
		}
		
		if (moveRequest.m_actor.GetCurrentBoardSquare() != moveRequest.m_targetSquare
		    || moveRequest.m_path.next != null)
		{
			moveRequest.m_resolveState = MovementRequest.MovementResolveState.RESOLVING;
			moveRequest.m_actor.BroadcastMoveToBoardSquare(
				moveRequest.m_targetSquare,
				ActorData.MovementType.Normal,
				moveRequest.m_path,
				ActorData.TeleportType.NotATeleport);
		}
		else
		{
			moveRequest.m_resolveState = MovementRequest.MovementResolveState.RESOLVED;
			ActorTurnSM actorTurnSM = moveRequest.m_actor.GetActorTurnSM();
			if (actorTurnSM)
			{
				actorTurnSM.OnMessage(TurnMessage.MOVEMENT_RESOLVED);
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

	// TODO SAB - never called
	public bool IsChasing(ActorData chaser, ActorData target)
	{
		bool result = false;
		foreach (MovementRequest movementRequest in m_storedMovementRequests)
		{
			if (movementRequest != null
			    && movementRequest.IsChasing()
			    && movementRequest.m_actor == chaser
			    && movementRequest.m_chaseTarget == target)
			{
				result = true;
				break;
			}
		}
		return result;
	}


	// TODO SAB - never called
	public bool HasResolvingMovementRequest(ActorData fromMover)
	{
		bool result = false;
		foreach (MovementRequest movementRequest in m_storedMovementRequests)
		{
			if (movementRequest != null
			    && movementRequest.m_actor == fromMover
			    && movementRequest.m_resolveState == MovementRequest.MovementResolveState.RESOLVING)
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
			if (movementRequest != null
			    && (fromMover == null || movementRequest.m_actor == fromMover)
			    && movementRequest.m_resolveState != MovementRequest.MovementResolveState.RESOLVED)
			{
				result = true;
				break;
			}
		}
		return result;
	}

	public void CancelMovementRequests(ActorData actor, bool forceChased = false)
	{
		List<MovementRequest> requestsToCancel = new List<MovementRequest>();
		foreach (MovementRequest movementRequest in m_storedMovementRequests)
		{
			if (movementRequest != null && movementRequest.m_actor == actor)
			{
				requestsToCancel.Add(movementRequest);
			}
		}
		foreach (MovementRequest movementRequest in requestsToCancel)
		{
			if (forceChased)
			{
				m_removedMovementRequestsFromForceChase.Add(movementRequest);
			}
			m_storedMovementRequests.Remove(movementRequest);
			movementRequest.m_actor.OnMovementChanged(ActorData.MovementChangeType.LessMovement, forceChased);
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
		foreach (MovementRequest movementRequest in list)
		{
			movementRequest.m_actor.GetActorTurnSM().OnMessage(TurnMessage.MOVEMENT_RESOLVED);
			m_storedMovementRequests.Remove(movementRequest);
			float desiredMovementOnResolve = movementRequest.m_actor.GetActorBehavior().DesiredMovementOnResolve;
			movementRequest.m_actor.GetActorBehavior().SetTotalMovementLostThisTurn(desiredMovementOnResolve);
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

	// TODO SAB - never called
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
			if (movementRequest.IsChasing())
			{
				continue;
			}
			
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
		return list;
	}

	public void CancelActionRequests(ActorData actor
		// , bool keepFutureTurnRequests // rogues
		)
	{
		// rogues
		//PveLog.DebugLog("Canceling Action Requests for " + actor.DebugNameString(), null);

		CancelMovementRequests(actor);
		CancelAbilityRequests(actor, false); // , keepFutureTurnRequests in rogues
	}
	
	public void StoreAbilityRequest(
		Ability ability,
		AbilityData.ActionType actionType,
		List<AbilityTarget> targets,
		ActorData caster,
		SequenceSource parentAbilitySequenceSource = null,
		ChainAbilityAdditionalModInfo chainModInfo = null
		// , bool storeForNextTurn = false // rogues
		)
	{
		if (HasPendingAbilityRequest(caster, false) && !ability.IsFreeAction())
		{
			CancelAbilityRequests(caster, true); // , false in rogues
		}
		if ((!caster.QueuedMovementAllowsAbility && ability.GetAffectsMovement()) || ability.CanOverrideMoveStartSquare())
		{
			CancelMovementRequests(caster);
		}
		if (ability != null && caster != null)
		{
			AbilityRequest abilityRequest = new AbilityRequest(ability, actionType, targets, caster)
			{
				m_additionalData =
				{
					m_parentAbilitySequenceSource = parentAbilitySequenceSource,
					m_chainModInfo = chainModInfo,
					m_skipTheatricsAnimEntry = ability.SkipTheatricsAnimationEntry(caster)
				}
			};
			// rogues
			// if (storeForNextTurn)
			// {
			// 	m_storedAbilityRequestsForNextTurn.Add(abilityRequest);
			// }
			// else
			// {
				m_storedAbilityRequests.Add(abilityRequest);
				OnAbilityRequestStored(abilityRequest);
			// }
			// return;
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
			caster.SetTechPoints(caster.TechPoints - moddedCost);
		}
		ability.OnAbilityQueuedDuringDecision();
		Ability[] chainAbilities = ability.GetChainAbilities();
		for (int i = 0; i < chainAbilities.Length; i++)
		{
			Ability chainAbility = chainAbilities[i];
			if (chainAbility != null)
			{
				AbilityData.ActionType actionTypeOfAbility = caster.GetAbilityData().GetActionTypeOfAbility(chainAbility);
				ChainAbilityAdditionalModInfo chainModInfo = null;
				if (ability.CurrentAbilityMod != null)
				{
					chainModInfo = ability.CurrentAbilityMod.GetChainModInfoAtIndex(i);
				}
				StoreAbilityRequest(
					chainAbility,
					actionTypeOfAbility,
					targets,
					caster,
					newRequest.m_additionalData.m_sequenceSource,
					chainModInfo); //, ability.m_runChainAbilitiesTheFollowingTurn); in rogues
			}
		}

		if (
			// caster.GetActorTurnSM().m_tauntRequestedForNextAbility == (int)actionType ||  // rogues
			(DebugParameters.Get() != null && DebugParameters.Get().GetParameterAsBool("AlwaysTauntAutomatically")))
		{
			List<CameraShotSequence> taunts = caster.GetAbilityData().GetDebugTauntListForActionType(actionType);
			if (!taunts.IsNullOrEmpty())
			{
				caster
					.GetComponent<ActorCinematicRequests>()
					.SendAbilityCinematicRequest(
						actionType,
						true,
						taunts[0].m_tauntNumber,
						taunts[0].m_uniqueTauntID);
			}
		}
		// caster.GetActorTurnSM().UpdateHasStoredAbilityRequestFlag();  // rogues
	}

	public static bool ShouldLogActorActions()
	{
		GameManager gameManager = GameManager.Get();
		return gameManager != null
			&& gameManager.GameConfig != null  // gameManager.GameMission != null in rogues
			&& GameFlowData.Get() != null;
	}

	// TODO LOW SAB - debug logging never called
	private void LogActorStateForRepro(string context)
	{
		if (!ShouldLogActorActions())
		{
			return;
		}
		
		string text = string.Concat(
			c_actionLogSearchMarker + "Turn ",
			GameFlowData.Get().CurrentTurn,
			" | -- ActorState ",
			context,
			" --\n");
		foreach (ActorData actorData in GameFlowData.Get().GetActors())
		{
			if (actorData == null || actorData.PlayerIndex < 0)
			{
					
				continue;
			}
			BoardSquare currentBoardSquare = actorData.GetCurrentBoardSquare();
			string textGridPos = currentBoardSquare != null ? currentBoardSquare.GetGridPos().ToStringWithCross() : "NULL";
			text = string.Concat(
				text,
				c_actionLogSearchMarker,
				actorData.DebugNameString(),
				" @square= ",
				textGridPos,
				" | HP= ",
				actorData.HitPoints,
				" | Energy= ",
				actorData.TechPoints,
				" | Absorb= ",
				actorData.AbsorbPoints,
				" | MaxMovement= ",
				actorData.GetActorMovement().CalculateMaxHorizontalMovement(),
				"\n");
		}
		
		// custom
		Log.Debug(text);
	}

	// TODO LOW SAB - debug logging never called
	private void LogRequestsForRepro(bool logAbilities, bool logMovement, string context)
	{
		if (!ShouldLogActorActions())
		{
			return;
		}
		
		string textAbilities = string.Concat(
			c_actionLogSearchMarker + "Turn ",
			GameFlowData.Get().CurrentTurn,
			" | -- Requests ",
			context,
			" --\n");
		if (logAbilities)
		{
			textAbilities += c_actionLogSearchMarker + "-- Ability Requests --\n";
			if (m_storedAbilityRequests.Count == 0)
			{
				textAbilities += c_actionLogSearchMarker + "NONE\n";
			}
			foreach (AbilityRequest abilityRequest in m_storedAbilityRequests)
			{
				textAbilities = string.Concat(
					textAbilities,
					c_actionLogSearchMarker,
					abilityRequest.m_caster.DebugNameString(),
					"\n" + c_actionLogSearchMarker + "    ActionType= ",
					abilityRequest.m_actionType.ToString(),
					" | AbilityName= ",
					abilityRequest.m_ability.m_abilityName,
					"\n");
				for (int i = 0; i < abilityRequest.m_targets.Count; i++)
				{
					AbilityTarget abilityTarget = abilityRequest.m_targets[i];
					textAbilities = string.Concat(
						textAbilities,
						c_actionLogSearchMarker + "    CursorState ",
						i,
						": ",
						abilityTarget.GetDebugString(),
						"\n");
				}
			}
		}
		string textMovement = "\n";
		if (logMovement)
		{
			textMovement += GetMovementRequestsString();
		}
		
		// custom
		Log.Debug(textAbilities + textMovement);
	}

	private string GetMovementRequestsString()
	{
		if (!ShouldLogActorActions())
		{
			return "";
		}
		
		string text = c_actionLogSearchMarker + "-- Movement Requests --\n";
		if (m_storedMovementRequests.Count == 0)
		{
			text += c_actionLogSearchMarker + "NONE\n";
		}
		foreach (MovementRequest movementRequest in m_storedMovementRequests)
		{
			text = text + c_actionLogSearchMarker +  movementRequest.m_actor.DebugNameString() + "\n";
			if (movementRequest.IsChasing())
			{
				string textChaseTargetGridPos = movementRequest.m_chaseTarget.GetCurrentBoardSquare() != null
					? movementRequest.m_chaseTarget.GetCurrentBoardSquare().GetGridPos().ToStringWithCross()
					: "NULL";
				text = string.Concat(
					text,
					c_actionLogSearchMarker + "    Chase | ChaseTarget= ",
					movementRequest.m_chaseTarget.DebugNameString(),
					" @square= ",
					textChaseTargetGridPos,
					" targetAlive= ",
					(!movementRequest.m_chaseTarget.IsDead()).ToString(),
					"\n");
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
				text = text + c_actionLogSearchMarker + "    Normal | ToSquare= " + gridPos.ToStringWithCross() + "\n";
			}
		}
		return text;
	}

	public void LogActionRequests()
	{
		foreach (AbilityRequest abilityRequest in m_storedAbilityRequests)
		{
			string text =
				$"Cast Player:{abilityRequest.m_caster.DisplayName} Ability:{abilityRequest.m_ability.m_abilityName}";
			foreach (AbilityTarget abilityTarget in abilityRequest.m_targets)
			{
				text = string.Concat(
					text,
					"(",
					abilityTarget.GridPos.x,
					",",
					abilityTarget.GridPos.y,
					")");
			}
			MatchLogger.Get().Log(string.Format(text));
		}
		foreach (MovementRequest movementRequest in m_storedMovementRequests)
		{
			GridPos gridPos = movementRequest.m_actor.GetCurrentBoardSquare().GetGridPos();
			string format;
			if (movementRequest.IsChasing())
			{
				GridPos targetPos = movementRequest.m_chaseTarget.GetGridPos();
				format = $"Chase Player:{movementRequest.m_actor.DisplayName} "
				         + $"Src:({gridPos.x},{gridPos.y}) "
				         + $"Target:{movementRequest.m_chaseTarget.DisplayName} at ({targetPos.x},{targetPos.y})";
			}
			else
			{
				GridPos targetPos = movementRequest.m_targetSquare.GetGridPos();
				format = $"Move Player:{movementRequest.m_actor.DisplayName} "
				         + $"Src:({gridPos.x},{gridPos.y}) "
				         + $"Dst:({targetPos.x},{targetPos.y})";
			}
			MatchLogger.Get().Log(string.Format(format));
		}
	}

	internal bool TryRunAbilityRequest(AbilityRequest request)
	{
		bool result = false;
		if (request != null
		    && request.m_ability != null
		    && request.m_resolveState == AbilityRequest.AbilityResolveState.QUEUED
		    && request.m_ability.RunPriority == AbilityPhase)
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

	// rogues
	// public void RunAbilityRequest_FCFS(AbilityRequest request)
	// {
	// 	RunAbilityRequest(request);
	// }

	private void RunAbilityRequest(AbilityRequest request)
	{
		request.m_resolveState = AbilityRequest.AbilityResolveState.RESOLVING;
		AbilityData abilityData = request.m_caster.GetAbilityData();
		abilityData.OnAbilityCast(request.m_ability);
		bool shouldTriggerCooldown = request.m_ability.ShouldTriggerCooldownOnCast(request.m_targets, request.m_caster, request.m_additionalData);
		request.m_ability.Run(request.m_targets, request.m_caster, request.m_additionalData);
		if (shouldTriggerCooldown)
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
		}
		if (BrushCoordinator.Get() != null)
		{
			BrushCoordinator.Get().OnCast_HandleConcealment(request.m_caster, request.m_ability, request.m_targets);
		}
		GameplayMetricHelper.IncrementAbilityUseCount(request.m_caster, request.m_ability, request.m_cinematicRequested != -1);
		if (request.m_actionType >= AbilityData.ActionType.CARD_0
		    && request.m_actionType <= AbilityData.ActionType.CARD_2)
		{
			GameplayMetricHelper.RecordCatalystUsed(request.m_caster, request.m_actionType);
		}
		if (request.m_cinematicRequested > 0 && request.m_tauntUniqueId >= 0)
		{
			ActorCinematicRequests actorCinematicRequests = request.m_caster != null
				? request.m_caster.GetComponent<ActorCinematicRequests>()
				: null;
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
			if (abilityRequest != null
			    && abilityRequest.m_caster == fromCaster
			    && (!abilityRequest.m_ability.IsFreeAction() || includeFreeActions))
			{
				result = true;
				break;
			}
		}
		return result;
	}

	// TODO SAB - never called
	public bool HasResolvingAbilityRequest(ActorData fromCaster)
	{
		bool result = false;
		foreach (AbilityRequest abilityRequest in m_storedAbilityRequests)
		{
			if (abilityRequest != null
			    && abilityRequest.m_resolveState == AbilityRequest.AbilityResolveState.RESOLVING
			    && (abilityRequest.m_caster == fromCaster || fromCaster == null))
			{
				result = true;
				break;
			}
		}
		return result;
	}

	// TODO SAB - never called
	public bool HasUnresolvedAbilityRequest(ActorData fromCaster)
	{
		bool result = false;
		foreach (AbilityRequest abilityRequest in m_storedAbilityRequests)
		{
			if (abilityRequest != null
			    && abilityRequest.m_caster == fromCaster
			    && abilityRequest.m_resolveState != AbilityRequest.AbilityResolveState.RESOLVED)
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
			if (abilityRequest != null
			    && abilityRequest.m_caster == fromCaster
			    && abilityRequest.m_resolveState != AbilityRequest.AbilityResolveState.RESOLVED
			    && abilityRequest.m_ability != null
			    && abilityRequest.m_ability.GetType() == abilityType)
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
		foreach (AbilityRequest abilityRequest in m_storedAbilityRequests)
		{
			if (abilityRequest != null
			    && abilityRequest.m_caster == fromCaster
			    && abilityRequest.m_ability != null
			    && abilityRequest.m_ability.GetType() == abilityType)
			{
				result = true;
				break;
			}
		}
		return result;
	}
	
	// TODO SAB - never called
	public bool HasStoredAbilityRequestsFromAnyone()
	{
		return m_storedAbilityRequests.Count > 0;
	}

	// TODO SAB - never called
	public bool HasStoredMovementRequestsFromAnyone()
	{
		return m_storedMovementRequests.Count > 0;
	}

	// TODO SAB - never called
	public MovementRequest GetStoredMovementRequestForActor(ActorData mover)
	{
		foreach (MovementRequest movementRequest in m_storedMovementRequests)
		{
			if (movementRequest.m_actor == mover)
			{
				return movementRequest;
			}
		}

		return null;
	}

	public List<AbilityTarget> GetTargetingDataOfStoredAbility(ActorData fromCaster, Type abilityType)
	{
		foreach (AbilityRequest abilityRequest in m_storedAbilityRequests)
		{
			if (abilityRequest != null
			    && abilityRequest.m_caster == fromCaster
			    && abilityRequest.m_ability != null
			    && abilityRequest.m_ability.GetType() == abilityType)
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
			if (abilityRequest != null
			    && abilityRequest.m_caster == caster
			    && abilityRequest.m_ability != null
			    && abilityRequest.m_ability.GetType() == abilityType
			    && abilityRequest.m_additionalData.m_abilityResults.GatheredResults)
			{
				return abilityRequest.m_additionalData.m_abilityResults.HitActorList();
			}
		}
		return new List<ActorData>();
	}

	// TODO SAB - never called
	public List<Dictionary<ActorData, int>> GetGatheredHpDeltas(ActorData caster, AbilityPriority fromPhase, AbilityPriority toPhase)
	{
		List<Dictionary<ActorData, int>> list = new List<Dictionary<ActorData, int>>();
		foreach (AbilityRequest abilityRequest in m_storedAbilityRequests)
		{
			if (abilityRequest != null
			    && abilityRequest.m_caster == caster
			    && abilityRequest.m_ability != null
			    && abilityRequest.m_ability.RunPriority >= fromPhase
			    && abilityRequest.m_ability.RunPriority <= toPhase
			    && abilityRequest.m_additionalData.m_abilityResults.GatheredResults)
			{
				list.Add(abilityRequest.m_additionalData.m_abilityResults.DamageResults);
			}
		}
		return list;
	}

	public void CollectGatheredOutgoingHitsSummary(AbilityPriority phase, ActorData caster, GatheredOutgoingHitsSummary summary)
	{
		if (summary == null)
		{
			return;
		}
		foreach (AbilityRequest abilityRequest in m_storedAbilityRequests)
		{
			if (abilityRequest != null
			    && abilityRequest.m_ability.RunPriority == phase
			    && abilityRequest.m_caster == caster)
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
				ServerGameplayUtils.CountDamageAndHeal(
					abilityRequest.m_additionalData.m_abilityResults.DamageResults,
					target,
					ref damage,
					ref healing);
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
		bool result = false;
		foreach (AbilityRequest abilityRequest in m_storedAbilityRequests)
		{
			if (abilityRequest != null
			    && abilityRequest.m_caster == actor
			    && abilityRequest.m_ability != null
			    && abilityRequest.m_ability.RunPriority == AbilityPriority.Evasion
			    && abilityRequest.m_ability.GetMovementType() != ActorData.MovementType.None)
			{
				result = true;
				break;
			}
		}
		if (!result && actor.PlayerIndex == PlayerData.s_invalidPlayerIndex)
		{
			result = m_evadeManager.HasProcessedEvadeForActor(actor);
		}
		return result;
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
			if (abilityRequest != null
			    && abilityRequest.m_caster == fromCaster
			    && !AbilityData.IsChain(abilityRequest.m_actionType))
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
			if (abilityRequest != null
			    && abilityRequest.m_caster == fromCaster
			    && (abilityRequest.m_ability == ability || ability.HasAbilityAsPartOfChain(abilityRequest.m_ability))
			    && abilityRequest.m_resolveState == AbilityRequest.AbilityResolveState.QUEUED)
			{
				if (requested)
				{
					abilityRequest.RequestCinematic(animTauntIndex, tauntUniqueId);
				}
				else
				{
					abilityRequest.CancelCinematic();
					// ability.ClearAbilityMod(abilityRequest.m_caster); // rogues
				}
				result = true;
			}
		}
		return result;
	}

	public void CancelAbilityRequest(ActorData fromCaster, Ability ability, bool checkForAdditionalToCancel) // , bool keepFutureTurnRequests in rogues
	{
		List<Ability> abilitiesToCancel = new List<Ability>();
		List<Ability> chainAbilitiesToCancel = new List<Ability>();
		foreach (AbilityRequest abilityRequest in m_storedAbilityRequests)
		{
			if (abilityRequest == null
			    || abilityRequest.m_caster != fromCaster
			    || abilityRequest.m_ability != ability)
			{
				continue;
			}
			
			if (abilityRequest.m_resolveState == AbilityRequest.AbilityResolveState.QUEUED)
			{
				HandleRemoveQueuedAbilityRequestForActor(
					abilityRequest, fromCaster, checkForAdditionalToCancel, out List<Ability> collection);
				abilitiesToCancel.AddRange(collection);
				foreach (Ability abilityToCancel in abilityRequest.m_ability.GetChainAbilities())
				{
					chainAbilitiesToCancel.Add(abilityToCancel);
				}
				m_storedAbilityRequests.Remove(abilityRequest); // modifying collection is fine as long as we break after
			}
			break;
		}
		
		// rogues
		// if (!keepFutureTurnRequests)
		// {
		// 	foreach (AbilityRequest abilityRequest in m_storedAbilityRequestsForNextTurn)
		// 	{
		// 		if (abilityRequest == null
		// 		    || abilityRequest.m_caster != fromCaster
		// 		    || abilityRequest.m_ability != ability)
		// 		{
		// 			continue;
		// 		}
		// 		
		// 		if (abilityRequest.m_resolveState == AbilityRequest.AbilityResolveState.QUEUED)
		// 		{
		// 			HandleRemoveQueuedAbilityRequestForActor(
		// 				abilityRequest, fromCaster, checkForAdditionalToCancel, out List<Ability> collection2);
		// 			abilitiesToCancel.AddRange(collection2);
		// 			foreach (Ability abilityToCancel in abilityRequest.m_ability.GetChainAbilities())
		// 			{
		// 				chainAbilitiesToCancel.Add(abilityToCancel);
		// 			}
		// 			m_storedAbilityRequestsForNextTurn.Remove(abilityRequest);
		// 		}
		// 		break;
		// 	}
		// }
		foreach (Ability abilityToCancel in chainAbilitiesToCancel)
		{
			CancelAbilityRequest(fromCaster, abilityToCancel, false); // , keepFutureTurnRequests in rogues
		}
		foreach (Ability abilityToCancel in abilitiesToCancel)
		{
			CancelAbilityRequest(fromCaster, abilityToCancel, false); // , keepFutureTurnRequests in rogues
		}

		// rogues
		//fromCaster.GetActorTurnSM().UpdateHasStoredAbilityRequestFlag();
	}

	public void CancelAbilityRequests(ActorData fromCaster, bool keepFreeActions) // , bool keepFutureTurnRequests in rogues
	{
		List<AbilityRequest> requestsToCancel = new List<AbilityRequest>();
		List<Ability> abilitiesToCancel = new List<Ability>();
		foreach (AbilityRequest abilityRequest in m_storedAbilityRequests)
		{
			if (abilityRequest != null
			    && abilityRequest.m_caster == fromCaster
			    && (!abilityRequest.m_ability.IsFreeAction() || !keepFreeActions))
			{
				HandleRemoveQueuedAbilityRequestForActor(abilityRequest, fromCaster, true, out _);
				requestsToCancel.Add(abilityRequest);
			}
		}
		
		// rogues
		// if (!keepFutureTurnRequests)
		// {
		// 	foreach (AbilityRequest abilityRequest in m_storedAbilityRequestsForNextTurn)
		// 	{
		// 		if (abilityRequest != null && abilityRequest.m_caster == fromCaster)
		// 		{
		// 			requestsToCancel.Add(abilityRequest);
		// 		}
		// 	}
		// }
		
		foreach (AbilityRequest abilityRequest in requestsToCancel)
		{
			m_storedAbilityRequests.Remove(abilityRequest);
			
			// rogues
			// if (!keepFutureTurnRequests)
			// {
			// 	m_storedAbilityRequestsForNextTurn.Remove(abilityRequest);
			// }
		}
		foreach (Ability ability in abilitiesToCancel)
		{
			CancelAbilityRequest(fromCaster, ability, false); // , keepFutureTurnRequests in rogues
		}

		// rogues
		//fromCaster.GetActorTurnSM().UpdateHasStoredAbilityRequestFlag();
	}

	private void HandleRemoveQueuedAbilityRequestForActor(
		AbilityRequest request,
		ActorData fromCaster,
		bool checkForAdditionalToCancel,
		out List<Ability> othersToCancel)
	{
		othersToCancel = new List<Ability>();
		if (request == null || request.m_ability == null)
		{
			return;
		}
		
		request.m_caster.GetAbilityData().SetQueuedAction(request.m_actionType, false);
		if (request.m_resolveState != AbilityRequest.AbilityResolveState.QUEUED)
		{
			return;
		}
		
		if (fromCaster.GetAbilityData() != null)
		{
			fromCaster.GetAbilityData().RemoveOnRequestStatusForAbility(request.m_ability);
		}
		request.m_ability.OnAbilityUnqueuedDuringDecision();
		if (checkForAdditionalToCancel)
		{
			List<AbilityData.ActionType> actionsToCancel = request.m_ability.GetOtherActionsToCancelOnAbilityUnqueue(request.m_caster);
			if (actionsToCancel != null)
			{
				foreach (AbilityData.ActionType actionType in actionsToCancel)
				{
					Ability abilityOfActionType = request.m_caster.GetAbilityData().GetAbilityOfActionType(actionType);
					if (abilityOfActionType != null && abilityOfActionType != request.m_ability)
					{
						othersToCancel.Add(abilityOfActionType);
					}
				}
			}
		}
				
		// custom - recover tech points
		int techPointCost = request.m_ability.GetModdedCost();
		if (techPointCost > 0)
		{
			fromCaster.ReservedTechPoints = Math.Max(0, fromCaster.ReservedTechPoints - techPointCost);
			fromCaster.SetTechPoints(fromCaster.TechPoints + techPointCost);
		}
		// end custom
	}

	// TODO SAB - never called - currently called in GatherAbilities/GatherMovement
	private void ClearRequestsOfDeadActors()
	{
		List<AbilityRequest> abilitiesToCancel = new List<AbilityRequest>();
		foreach (AbilityRequest abilityRequest in m_storedAbilityRequests)
		{
			if (abilityRequest.m_caster.IsDead())
			{
				abilitiesToCancel.Add(abilityRequest);
			}
		}
		
		// rogues
		// foreach (AbilityRequest abilityRequest in m_storedAbilityRequestsForNextTurn)
		// {
		// 	if (abilityRequest.m_caster.IsDead())
		// 	{
		// 		abilitiesToCancel.Add(abilityRequest);
		// 	}
		// }
		
		foreach (AbilityRequest abilityRequest in abilitiesToCancel)
		{
			abilityRequest.m_caster.GetComponent<AbilityData>().SetQueuedAction(abilityRequest.m_actionType, false);
			abilityRequest.m_caster.InitialMoveStartSquare = abilityRequest.m_caster.GetCurrentBoardSquare();
			m_storedAbilityRequests.Remove(abilityRequest);
			// m_storedAbilityRequestsForNextTurn.Remove(abilityRequest); // rogues
		}
		
		List<MovementRequest> movementToCancel = new List<MovementRequest>();
		foreach (MovementRequest movementRequest in m_storedMovementRequests)
		{
			if (movementRequest.m_actor.IsDead())
			{
				movementToCancel.Add(movementRequest);
			}
			else if (movementRequest.m_actor.GetCurrentBoardSquare() == null)
			{
				Log.Error(
					"{0} is not dead but has no current board square, on removing requests from dead actors",
					movementRequest.m_actor.DebugNameString());
				movementToCancel.Add(movementRequest);
			}
		}
		foreach (MovementRequest item in movementToCancel)
		{
			m_storedMovementRequests.Remove(item);
		}
	}

	public void RestoreMovementForForceChaseImmunity()
	{
		List<MovementRequest> requestsToRestore = new List<MovementRequest>();
		List<MovementRequest> requestsToRemove = new List<MovementRequest>();
		foreach (MovementRequest movementRequest in m_storedMovementRequests)
		{
			if (movementRequest.IsForcedChase()
			    && !movementRequest.m_chaserInitiatedForceChase
			    && movementRequest.m_actor != null
			    && movementRequest.m_actor.GetActorStatus().IsImmuneToForcedChase())
			{
				requestsToRemove.Add(movementRequest);
				MovementRequest requestToRestore = FindMovementRequestToRestore(movementRequest.m_actor);
				if (requestToRestore != null)
				{
					requestsToRestore.Add(requestToRestore);
				}
			}
		}
		for (int i = m_storedMovementRequests.Count - 1; i >= 0; i--)
		{
			if (requestsToRemove.Contains(m_storedMovementRequests[i]))
			{
				m_storedMovementRequests[i].m_actor.OnMovementChanged(ActorData.MovementChangeType.LessMovement, true);
				m_storedMovementRequests.RemoveAt(i);
			}
		}
		foreach (MovementRequest movementRequest in requestsToRestore)
		{
			m_storedMovementRequests.Add(movementRequest);
			movementRequest.m_actor.OnMovementChanged(ActorData.MovementChangeType.MoreMovement, movementRequest.m_isForcedChase);
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
		bool found = false;
		foreach (MovementRequest movementRequest in m_storedMovementRequests)
		{
			if (movementRequest == null || movementRequest.m_actor != mover)
			{
				continue;
			}
			
			if (movementRequest.IsChasing())
			{
				found = true;
				mover.GetComponent<LineData>().OnMovementChanged(movementRequest.ToGridPosPath(), null, true, forceRebuildLine);
			}
			else if (movementRequest.m_path != null && mover.GetComponent<LineData>() != null)
			{
				found = true;
				BoardSquarePathInfo fullPath = movementRequest.m_path.Clone(null);
				m_movementStabilizer.ModifyPathForMaxMovement(movementRequest.m_actor, fullPath, false);
				BoardSquarePathInfo snaredPath = movementRequest.m_path.Clone(null);
				m_movementStabilizer.ModifyPathForMaxMovement(movementRequest.m_actor, snaredPath, true);
				mover.GetComponent<LineData>().OnMovementChanged(
					fullPath.ToGridPosPath(),
					snaredPath.ToGridPosPath(),
					movementRequest.IsChasing(),
					forceRebuildLine);
			}
			break;
		}
		if (!found)
		{
			mover.GetComponent<LineData>().OnMovementChanged(null, null, false, forceRebuildLine);
		}
	}

	public void ExecuteUnexecutedHits(AbilityPriority forPhase, bool asFailsafe)
	{
		ServerGameplayUtils.AdjustStatsForDamageTakenWithEvades(forPhase, m_storedAbilityRequests);
		HandleExecuteUnexecutedHitsForAbilityRequests(m_storedAbilityRequests, forPhase, asFailsafe);
		ServerEffectManager.Get().ExecuteUnexecutedHitsForAllEffects(forPhase, asFailsafe);
		switch (forPhase)
		{
			case AbilityPriority.Evasion:
				ExecuteUnexecutedHitsForMovementStageInDistanceOrder(MovementStage.Evasion, asFailsafe);
				break;
			case AbilityPriority.Combat_Knockback:
				ExecuteUnexecutedHitsForMovementStageInDistanceOrder(MovementStage.Knockback, asFailsafe);
				break;
		}
	}

	public void HandleExecuteUnexecutedHitsForAbilityRequests(List<AbilityRequest> requests, AbilityPriority forPhase, bool asFailsafe)
	{
		foreach (AbilityRequest abilityRequest in requests)
		{
			if (abilityRequest == null || abilityRequest.m_ability.RunPriority != forPhase)
			{
				continue;
			}
			
			int techPointRewardForInteraction = AbilityUtils.GetTechPointRewardForInteraction(
				abilityRequest.m_ability,
				AbilityInteractionType.Cast,
				true);
			ActorData caster = abilityRequest.m_caster;
			if (techPointRewardForInteraction > 0)
			{
				ServerCombatManager.Get().TechPointGain(
					abilityRequest.m_ability,
					caster,
					caster,
					techPointRewardForInteraction,
					ServerCombatManager.TechPointChangeType.AbilityInteraction);
			}
			if (abilityRequest.m_ability.GetModdedCost() > 0)
			{
				caster.ReservedTechPoints = 0;
			}
			if (!abilityRequest.m_additionalData.m_abilityResults.HitsDoneExecuting())
			{
				abilityRequest.m_additionalData.m_abilityResults.ExecuteUnexecutedAbilityHits(asFailsafe);
			}
			if (caster.GetAbilityData() != null
			    && !caster.HasBotController
			    && !AbilityData.IsChain(caster.GetAbilityData().GetActionTypeOfAbility(abilityRequest.m_ability)))
			{
				abilityRequest.m_additionalData.m_abilityResults.GenerateAbilityEvent();
			}
		}
	}

	public void ExecuteUnexecutedNormalMovementHits(bool asFailsafe)
	{
		ExecuteUnexecutedHitsForMovementStageInDistanceOrder(MovementStage.Normal, asFailsafe);
	}

	private void ExecuteUnexecutedHitsForMovementStageInDistanceOrder(MovementStage stage, bool asFailsafe)
	{
		bool stillHasUnexecutedHits = true;
		float distance = 0f;
		while (stillHasUnexecutedHits)
		{
			Log.Info($"ExecuteUnexecutedHitsForMovementStageInDistanceOrder distance={distance}"); // custom debug
			BarrierManager.Get().ExecuteUnexecutedMovementHitsForAllBarriersForDistance(
				distance,
				stage,
				asFailsafe,
				out bool stillHasUnexecutedHitsBarriers,
				out float nextUnexecutedHitDistanceBarriers);
			
			bool stillHasUnexecutedHitsCTF;
			float nextUnexecutedHitDistanceCTF;
			if (CaptureTheFlag.Get() != null)
			{
				CaptureTheFlag.Get().ExecuteUnexecutedMovementResultsForDistance_Ctf(distance, stage, asFailsafe, out stillHasUnexecutedHitsCTF, out nextUnexecutedHitDistanceCTF);
			}
			else
			{
				stillHasUnexecutedHitsCTF = false;
				nextUnexecutedHitDistanceCTF = -1f;
			}
			
			bool stillHasUnexecutedHitsCTC;
			float nextUnexecutedHitDistanceCTC;
			if (CollectTheCoins.Get() != null)
			{
				CollectTheCoins.Get().ExecuteUnexecutedMovementResultsForDistance_Ctc(distance, stage, asFailsafe, out stillHasUnexecutedHitsCTC, out nextUnexecutedHitDistanceCTC);
			}
			else
			{
				stillHasUnexecutedHitsCTC = false;
				nextUnexecutedHitDistanceCTC = -1f;
			}

			ServerEffectManager.Get().ExecuteUnexecutedMovementHitsForAllEffectsForDistance(
				distance,
				stage,
				asFailsafe,
				out bool stillHasUnexecutedHitsEffects,
				out float nextUnexecutedHitDistanceEffects);
			PowerUpManager.Get().ExecuteUnexecutedMovementHitsForAllPowerupsForDistance(
				distance,
				stage,
				asFailsafe,
				out bool stillHasUnexecutedHitsPowerUps,
				out float nextUnexecutedHitDistancePowerUps);
			
			// custom debug
			if (!stillHasUnexecutedHitsBarriers) Log.Info("ExecuteUnexecutedHitsForMovementStageInDistanceOrder no more barrier hits"); 
			if (!stillHasUnexecutedHitsEffects) Log.Info("ExecuteUnexecutedHitsForMovementStageInDistanceOrder no more effect hits");
			if (!stillHasUnexecutedHitsPowerUps) Log.Info("ExecuteUnexecutedHitsForMovementStageInDistanceOrder no more powerup hits");
			// end custom
			
			stillHasUnexecutedHits = stillHasUnexecutedHitsBarriers
			                         || stillHasUnexecutedHitsCTF
			                         || stillHasUnexecutedHitsCTC
			                         || stillHasUnexecutedHitsEffects
			                         || stillHasUnexecutedHitsPowerUps;
			if (stillHasUnexecutedHits)
			{
				float nextUnexecutedHitDistance = -1f;
				if (stillHasUnexecutedHitsBarriers
				    && (nextUnexecutedHitDistanceBarriers < nextUnexecutedHitDistance || nextUnexecutedHitDistance == -1f))
				{
					nextUnexecutedHitDistance = nextUnexecutedHitDistanceBarriers;
				}
				if (stillHasUnexecutedHitsCTF
				    && (nextUnexecutedHitDistanceCTF < nextUnexecutedHitDistance || nextUnexecutedHitDistance == -1f))
				{
					nextUnexecutedHitDistance = nextUnexecutedHitDistanceCTF;
				}
				if (stillHasUnexecutedHitsCTC
				    && (nextUnexecutedHitDistanceCTC < nextUnexecutedHitDistance || nextUnexecutedHitDistance == -1f))
				{
					nextUnexecutedHitDistance = nextUnexecutedHitDistanceCTC;
				}
				if (stillHasUnexecutedHitsEffects
				    && (nextUnexecutedHitDistanceEffects < nextUnexecutedHitDistance || nextUnexecutedHitDistance == -1f))
				{
					nextUnexecutedHitDistance = nextUnexecutedHitDistanceEffects;
				}
				if (stillHasUnexecutedHitsPowerUps
				    && (nextUnexecutedHitDistancePowerUps < nextUnexecutedHitDistance || nextUnexecutedHitDistance == -1f))
				{
					nextUnexecutedHitDistance = nextUnexecutedHitDistancePowerUps;
				}
				distance = nextUnexecutedHitDistance;
			}
		}
	}

	public List<ActorData> IdentifyActorsDyingBeforeKnockbackMovement()
	{
		Dictionary<ActorData, int> actorToHealthDelta = new Dictionary<ActorData, int>();
		List<ActorData> actors = GameFlowData.Get().GetActors();
		foreach (ActorData actorData in actors)
		{
			actorToHealthDelta.Add(actorData, 0);
		}
		foreach (AbilityRequest abilityRequest in m_storedAbilityRequests)
		{
			if (abilityRequest != null && abilityRequest.m_ability.RunPriority == AbilityPriority.Combat_Knockback)
			{
				ServerGameplayUtils.IntegrateHpDeltas(
					abilityRequest.m_additionalData.m_abilityResults.DamageResults,
					ref actorToHealthDelta);
			}
		}

		// custom - fix for Dino shields on knockback
		Dictionary<ActorData, int> actorToKnockbackPhaseEffectShieldDelta = new Dictionary<ActorData, int>();
		foreach (ActorData actorData in actors)
		{
			actorToKnockbackPhaseEffectShieldDelta.Add(actorData, 0);
		}
		foreach (AbilityRequest abilityRequest in m_storedAbilityRequests)
		{
			foreach (var actorToHitResult in abilityRequest.m_additionalData.m_abilityResults.m_actorToHitResults)
			{
				actorToKnockbackPhaseEffectShieldDelta[actorToHitResult.Key] += actorToHitResult.Value.AppliedAbsorb;
			}
		}
		// end custom

		ServerEffectManager.Get().IntegrateHpDeltasForEffects(
			AbilityPriority.Combat_Knockback,
			ref actorToHealthDelta, 
			false);
		
		List<ActorData> dyingActors = new List<ActorData>();
		foreach (ActorData actorData in actors)
		{
			int prevPhasesDelta = actorData.UnresolvedHealing + actorData.AbsorbPoints - actorData.UnresolvedDamage;
			
			// rogues
			// int knockbackPhaseDelta = actorToHealthDelta[actorData];
			// custom
			int knockbackPhaseDelta = actorToHealthDelta[actorData] + actorToKnockbackPhaseEffectShieldDelta[actorData];
			
			if (actorData.HitPoints + prevPhasesDelta + knockbackPhaseDelta <= 0)
			{
				dyingActors.Add(actorData);
			}
		}
		return dyingActors;
	}

	public List<AbilityRequest> GetAllStoredAbilityRequests()
	{
		return m_storedAbilityRequests;
	}

	public List<MovementRequest> GetAllStoredMovementRequests()
	{
		return m_storedMovementRequests;
	}

	// rogues
	// public void RecordAbilityAlertInitiator(ActorData actor)
	// {
	// 	int currentTurn = GameFlowData.Get().CurrentTurn;
	// 	if (m_combatInitiator == null || currentTurn > m_combatInitiatorRecordTurn)
	// 	{
	// 		m_combatInitiator = actor;
	// 		m_combatInitiatorRecordTurn = currentTurn;
	// 	}
	// }

	// rogues
	// public void ClearAbilityAlertInitiator()
	// {
	// 	m_combatInitiator = null;
	// 	m_combatInitiatorRecordTurn = -1;
	// }

	// rogues
	// public ActorData GetAbilityAlertInitiator(int turn)
	// {
	// 	if (turn == m_combatInitiatorRecordTurn)
	// 	{
	// 		return m_combatInitiator;
	// 	}
	// 	return null;
	// }

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

	// rogues
	// private void MirrorProcessed()
	// {
	// }

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
#endif
	// removed in rogues
	private void UNetVersion()
	{
	}

	// removed in rogues
	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		return false;
	}

	// removed in rogues
	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
	}
	
	// ---------------------------------------------------------------------------------
#if SERVER
	// custom
	
	private ActionBufferPhase actionBufferTimerPhase = ActionBufferPhase.Done;
	private float actionBufferPhaseStartTime;
	private HashSet<AbilityPriority> m_nonEmptyPhases = new HashSet<AbilityPriority>();

	private IEnumerable<AbilityRequest> RequestsInPhase(AbilityPriority phase) =>
		m_storedAbilityRequests.Where(r => r?.m_ability?.RunPriority == phase);
	
	public void HandleUpdateResolve() // TODO SAB private?
	{
		TheatricsManager theatrics = TheatricsManager.Get();

		bool isNewPhase = false;
		if (ActionPhase != actionBufferTimerPhase)
		{
			isNewPhase = true;
			actionBufferTimerPhase = ActionPhase;
			actionBufferPhaseStartTime = GameFlowData.Get().GetGameTime();
		}
		
		switch (ActionPhase)
		{
			case ActionBufferPhase.Abilities:
			{
				if (isNewPhase)
				{
					OnBeginResolve();
				}
				HandleUpdateResolveAbilities();
				break;
			}
			case ActionBufferPhase.AbilitiesWait:
			{
				if (isNewPhase)
				{
					OnEndResolveAbilities();
				}
				HandleUpdateResolveAbilitiesWait(isNewPhase);
				break;
			}
			case ActionBufferPhase.Movement:
			{
				HandleUpdateResolveMovement();
				break;
			}
			case ActionBufferPhase.MovementChase:
			{
				HandleUpdateResolveMovementChase();
				break;
			}
			case ActionBufferPhase.MovementWait:
			{
				HandleUpdateResolveMovementWait(theatrics);
				break;
			}
		}
	}
	
	// custom
	private void OnBeginResolve()
	{
		AbilityPhase = AbilityPriority.INVALID; // TODO SAB is incorrect as we have to branch on it in HandleUpdateResolveAbilities 
	}
	
	// custom
	private void OnEndResolveAbilities()
	{
		AbilityPhase = AbilityPriority.INVALID; // TODO SenseiAppendStatusEffect seems to expect it to be not INVALID on movement
	}
	
	// custom
	private void HandleUpdateResolveAbilities()
	{
		ServerResolutionManager manager = ServerResolutionManager.Get();
		if (!manager.ActionsDoneResolving() || IsWaitingForPlayPhaseEnded())
		{
			return;
		}
		
		TheatricsManager theatrics = TheatricsManager.Get();
		while (true)
		{
			ServerKnockbackManager serverKnockbackManager = GetKnockbackManager();
			if (AbilityPhase == AbilityPriority.Combat_Knockback)
			{
				serverKnockbackManager.ClearStoredData();
			}
			ServerEffectManager.Get().OnAbilityPhaseEnd(AbilityPhase);
			OnAbilityPhaseEnd(AbilityPhase);
			if (AbilityPhase == AbilityUtils.GetLowestAbilityPriority())
			{
				// end ability resolution
				ActionPhase = ActionBufferPhase.AbilitiesWait;
				Log.Info($"Going to next action phase {ActionPhase}");
				return;
			}

			AbilityPhase = AbilityPhase == AbilityPriority.INVALID
				? AbilityUtils.GetHighestAbilityPriority()
				: AbilityUtils.GetNextAbilityPriority(AbilityPhase);
			Log.Info($"Going to next turn ability phase {AbilityPhase}");

			m_waitingForPlayPhaseEnded = true;
			SetSquareAtPhaseStartForActors();

			GatheringFakeResults = false;
			
			if (AbilityPhase < AbilityPriority.Combat_Damage)
			{
				SetupPhase(AbilityPhase);
			}
			else if (AbilityPhase == AbilityPriority.Combat_Damage)
			{
				for (AbilityPriority i = AbilityPriority.Combat_Damage;
				     i < AbilityPriority.NumAbilityPriorities;
				     ++i)
				{
					SetupPhase(i);
				}
			}
			else if (AbilityPhase == AbilityPriority.Combat_Knockback)
			{
				// in case something changed since pre-gathering
				serverKnockbackManager.ClearStoredData();
				serverKnockbackManager.ProcessKnockbacks(m_storedAbilityRequests);
				
				Log.Info("Looking for additional effects in this phase...");
				
				GatherEffects(AbilityPhase, true);

				// we are only gathering responses to knockbacks here
				serverKnockbackManager.GatherGameplayResultsInResponseToKnockbacks(out List<ActorData> actorsThatWillBeSeenButArentMoving);
				SynchronizePositionsOfActorsThatWillBeSeen(actorsThatWillBeSeenButArentMoving);
			}

			// we do not want to disrupt brushes and stuff until effect results are gathered
			foreach (AbilityRequest abilityRequest in RequestsInPhase(AbilityPhase))
			{
				TryRunAbilityRequest(abilityRequest);
			}
				
			// Note: some abilities expect phase results gathered before OnAbilityPhaseStart (e.g. MantaDirtyFightingEffect)
			SynchronizePositionsOfActorsParticipatingInPhase(AbilityPhase); /// check? see PlayerAction_*.ExecuteAction for more resolution stuff gathered from all over ARe
			ServerEffectManager.Get().OnAbilityPhaseStart(AbilityPhase);
			ServerResolutionManager.Get().OnAbilityPhaseStart(AbilityPhase);
			foreach (ActorData actorData in GameFlowData.Get().GetActors())
			{
				if (actorData.GetPassiveData() != null)
				{
					actorData.GetPassiveData().OnAbilityPhaseStart(AbilityPhase);
				}
			}
			OnAbilityPhaseStart();
			if (m_nonEmptyPhases.Contains(AbilityPhase))
			{
				break;
			}
			else
			{
				Log.Info("No requests in this phase, going to the next one");
			}
		}
			
		theatrics.SetDirtyBit(uint.MaxValue);
		theatrics.PlayPhase(AbilityPhase);
	}

	private void SetupPhase(AbilityPriority phase)
	{
		TheatricsManager theatrics = TheatricsManager.Get();
		bool hasActionsThisPhase = GatherActionsInPhase(phase);
		if (phase == AbilityPriority.Combat_Knockback)
		{
			GetKnockbackManager().ProcessKnockbacks(m_storedAbilityRequests);
		}
		if (hasActionsThisPhase)
		{
			m_nonEmptyPhases.Add(phase);
		}

		theatrics.SetupTurnAbilityPhase(
			phase,
			m_storedAbilityRequests,
			new HashSet<int>(),  // TODO SAB (hacked inside)
			false);
	}

	private MovementCollection movementCollection;
	private List<MovementRequest> validRequestsThisPhase;
	
	private void HandleUpdateResolveAbilitiesWait(bool isNewPhase)
	{
		if (isNewPhase)
		{
			foreach (ActorData actor in GameFlowData.Get().GetActors())
			{
				ActorTurnSM turnSm = actor.gameObject.GetComponent<ActorTurnSM>();
				turnSm.OnMessage(TurnMessage.CLIENTS_RESOLVED_ABILITIES);
			}
			if (ServerCombatManager.Get().HasUnresolvedHealthEntries())
			{
				ServerCombatManager.Get().ResolveHitPoints();
			}
			foreach (ActorData actorData in GameFlowData.Get().GetActors())
			{
				if (actorData != null && actorData.GetPassiveData() != null)
				{
					actorData.GetPassiveData().OnAbilitiesDone();
				}
			}
					
			Log.Info($"Running {GetAllStoredMovementRequests().Count(req => !req.IsChasing())} non-chase movement requests");
			movementCollection = null; // TODO SAB
			validRequestsThisPhase = null; // TODO SAB
			GatherMovement(false);
		}

		if (GameFlowData.Get().GetGameTime() - actionBufferPhaseStartTime > 1.5f)
		{
			ExecuteMovement(false);
			ActionPhase = ActionBufferPhase.Movement;
		}
	}

	private void HandleUpdateResolveMovement()
	{
		// CompleteExecutingPlayerActions();
		if (ServerCombatManager.Get().HasUnresolvedHealthEntries())
		{
			ServerCombatManager.Get().ResolveHitPoints();
		}
		if (!ServerMovementManager.Get().WaitingOnClients && ServerResolutionManager.Get().ActionsDoneResolving())
		{
			int numChaseRequests = GetAllStoredMovementRequests().FindAll(req => req.WasEverChasing()).Count;
			if (numChaseRequests > 0)
			{
				Log.Info($"Running {numChaseRequests} chase movement requests");
				movementCollection = null; // TODO SAB
				validRequestsThisPhase = null; // TODO SAB
				GatherMovement(true);
				ExecuteMovement(true);
			}
			else
			{
				Log.Info("No chase requests");
			}
			ActionPhase = ActionBufferPhase.MovementChase;
		}
	}

	private void HandleUpdateResolveMovementChase()
	{
		// CompleteExecutingPlayerActions();
		ServerMovementManager manager = ServerMovementManager.Get();
		if (!manager.WaitingOnClients && ServerResolutionManager.Get().ActionsDoneResolving())
		{
			foreach (ActorData actor in GameFlowData.Get().GetActors())
			{
				ActorTurnSM turnSm = actor.gameObject.GetComponent<ActorTurnSM>();
				turnSm.OnMessage(TurnMessage.MOVEMENT_RESOLVED);
			}
			ActionPhase = ActionBufferPhase.MovementWait;
		}
	}

	private void HandleUpdateResolveMovementWait(TheatricsManager theatrics)
	{
		theatrics.MarkPhasesOnActionsDone();
		ActionPhase = ActionBufferPhase.Done;
				
		if (GameFlowData.Get().gameState == GameState.BothTeams_Resolve)
		{
			GameFlowData.Get().gameState = GameState.EndingTurn;
		}
	}
	
	// custom
	private bool GatherActionsInPhase(AbilityPriority phase)
	{
		bool hasActionsThisPhase = false;
		
		hasActionsThisPhase |= GatherAbilities(phase);
		
		// Some abilities (RageBeastSelfHeal) expect abilities to be resolved before effects results are gathered
		// knockback is gathered separately in HandleUpdateResolveAbilities
		hasActionsThisPhase |= GatherEffects(phase, phase != AbilityPriority.Combat_Knockback); 

		return hasActionsThisPhase;
	}
	
	
	public bool GatherAbilities(AbilityPriority phase)
	{
		List<AbilityRequest> requestsThisPhase = RequestsInPhase(phase).ToList();
		if (requestsThisPhase.Count == 0)
		{
			return false;
		}
		
		Log.Info($"Have {requestsThisPhase.Count} requests in this phase, playing them...");
		
		for (int i = requestsThisPhase.Count - 1; i >= 0; i--)
		{
			AbilityRequest abilityRequest = requestsThisPhase[i];
			if (abilityRequest.m_caster.IsDead())
			{
				abilityRequest.m_resolveState = AbilityRequest.AbilityResolveState.QUEUED;
				CancelAbilityRequest(abilityRequest.m_caster, abilityRequest.m_ability, true); // , false in rogues
				requestsThisPhase.RemoveAt(i);
			}
		}
		
		if (requestsThisPhase.Count == 0)
		{
			return false;
		}

		if (phase == AbilityPriority.Evasion)
		{
			SetupForEvadesPreGathering(requestsThisPhase);
		}
		
		foreach (AbilityRequest abilityRequest in requestsThisPhase)
		{
			if (abilityRequest.m_caster.GetPassiveData())
			{
				abilityRequest.m_caster.GetPassiveData().PreGatherResultsForPlayerAction(abilityRequest.m_ability);
			}
			if (abilityRequest.m_caster != null && abilityRequest.m_caster.GetAbilityData() != null)
			{
				abilityRequest.m_caster.GetAbilityData().ReinitAbilityInteractionData(abilityRequest.m_ability);
			}
			abilityRequest.m_ability.GatherResults_Base(
				phase,
				abilityRequest.m_targets,
				abilityRequest.m_caster,
				abilityRequest.m_additionalData);
		}
		return true;
	}

	private void SetupForEvadesPreGathering(List<AbilityRequest> requests)
	{
		ServerEvadeManager evadeManager = GetEvadeManager();
		evadeManager.ProcessEvades(requests, AbilityPriority.Evasion);
		foreach (ActorData actorData in GameFlowData.Get().GetActors())
		{
			if (actorData.GetPassiveData() != null)
			{
				actorData.GetPassiveData().OnEvadesProcessed();
			}
		}

		evadeManager.GatherGameplayResultsInResponseToEvades(out var actorsThatWillBeSeenButArentMoving);
		SynchronizePositionsOfActorsThatWillBeSeen(actorsThatWillBeSeenButArentMoving);
		evadeManager.SwapEvaderSquaresWithDestinations();
		if (evadeManager.HasEvades())
		{
			ImmediateUpdateAllFogOfWar();
		}
	}
	
	private bool GatherEffects(AbilityPriority phase, bool notify)
	{
		if (notify)
		{
			ServerEffectManager.Get().NotifyBeforeGatherAllEffectResults(phase);
		}
		
		// from QueuedPlayerActionsContainer::InitEffectsForExecution
		List<Effect> executingEffects = new List<Effect>();
		foreach (KeyValuePair<ActorData, List<Effect>> actorAndEffects in ServerEffectManager.Get().GetAllActorEffects())
		{
			if (actorAndEffects.Key.IsDead())
			{
				continue;
			}
			
			foreach (Effect effect in actorAndEffects.Value)
			{
				if (effect.HitPhase != phase)
				{
					continue;
				}
				
				EffectResults resultsForPhase = effect.GetResultsForPhase(phase, true);
				if (effect.HitPhase == phase &&
				    (resultsForPhase == null || !resultsForPhase.GatheredResults))
				{
					effect.Resolve();
					executingEffects.Add(effect);
				}
			}
		}

		foreach (Effect effect in ServerEffectManager.Get().GetWorldEffects())
		{
			if (effect.HitPhase != phase)
			{
				continue;
			}
			
			EffectResults resultsForPhase = effect.GetResultsForPhase(phase, true);
			if (effect.HitPhase == phase &&
			    (resultsForPhase == null || !resultsForPhase.GatheredResults))
			{
				effect.Resolve();
				executingEffects.Add(effect);
			}
		}

		if (executingEffects.Count > 0)
		{
			Log.Info($"Have {executingEffects.Count} effects in this phase, playing them...");
			return true;
		}

		return false;
	}
	
	public void GatherMovement(bool isChase)
	{
		List<MovementRequest> moveRequests = GetAllStoredMovementRequests();
		if (moveRequests == null)
		{
			Log.Error("No movement requests");
			return;
		}

		// TODO SAB call ClearRequestsOfDeadActors
		for (int i = moveRequests.Count - 1; i >= 0; i--)
		{
			MovementRequest movementRequest = moveRequests[i];
			if (movementRequest.m_actor.IsDead())
			{
				Log.Info($"Cancelling ${movementRequest.m_actor.m_displayName}'s movement request because they are dead");
				CancelMovementRequests(movementRequest.m_actor);
			}
		}
		
		if (moveRequests.Count == 0)
		{
			Log.Info("No movement requests");
			return;
		}

		// TODO SAB unite two cancellations?
		foreach (MovementRequest movementRequest in moveRequests)
		{
			BoardSquare targetSquare = movementRequest.m_targetSquare;
			if ((movementRequest.m_path?.next == null || targetSquare == null)
			    && !movementRequest.IsChasing())
			{
				Log.Info($"Cancelling ${movementRequest.m_actor.m_displayName}'s movement request because it is invalid");
				CancelMovementRequests(movementRequest.m_actor);
			}
		}
		Log.Info($"{moveRequests.Count} valid movement requests");
				
		GetMoveStabilizer().AdjustMovementStartsForMoveAfterEvade(moveRequests); // custom
		GetMoveStabilizer().StabilizeMovement(moveRequests, isChase);

		// TODO SAB unite two cancellations?
		foreach (MovementRequest movementRequest in moveRequests)
		{
			if ((isChase || !movementRequest.IsChasing()) // custom
			    && (movementRequest.m_path == null || movementRequest.m_path.next == null))
			{
				Log.Warning($"{movementRequest.m_actor.m_displayName}'s movement path is null after stabilization");
				CancelMovementRequests(movementRequest.m_actor);
			}
		}

		ClearNormalMovementResults();
		
		// custom
		ServerClashUtils.MovementClashCollection clashes = ServerClashUtils.IdentifyClashSegments_Movement(moveRequests, isChase);
		ServerClashUtils.ResolveClashMovement(moveRequests, clashes, isChase);
		// end custom
		
		ServerGameplayUtils.GatherGameplayResultsForNormalMovement(moveRequests, isChase);
		
		validRequestsThisPhase = moveRequests.Where(r => r.WasEverChasing() == isChase).ToList();
		movementCollection = new MovementCollection(validRequestsThisPhase);

		// custom
		foreach (ActorData actorData in GameFlowData.Get().GetActors())
		{
			actorData.TeamSensitiveData_authority.MovementCameraBounds = GetMovementBoundsForTeam(validRequestsThisPhase, actorData.GetTeam());
		}
		// end custom
	}

	// rogues+custom: no chasing in rogues
	public void ExecuteMovement(bool isChase)
	{
		List<MovementRequest> moveRequests = GetAllStoredMovementRequests();
		if (moveRequests == null || moveRequests.Count == 0)
		{
			return;
		}
		
		foreach (ActorData actorData in GameFlowData.Get().GetActors())
		{
			if (actorData.GetPassiveData() != null)
			{
				actorData.GetPassiveData().OnMovementResultsGathered(movementCollection);
			}
			actorData.GetActorMovement().ClearPath();
			actorData.UpdateServerLastVisibleTurn();
		}

		ServerGameplayUtils.SetServerLastKnownPositionsForMovement(
			movementCollection,
			out List<ActorData> seenNonMovers_normal,
			out List<ActorData> seenNonMovers_chase);
		// custom
		List<ActorData> seenNonMovers = seenNonMovers_normal;
		
		foreach (ActorData seenNonMover in seenNonMovers)
		{
			seenNonMover.TeamSensitiveData_hostile.BroadcastMovement(
				GameEventManager.EventType.NormalMovementStart,
				seenNonMover.CurrentBoardSquare.GetGridPos(),
				seenNonMover.CurrentBoardSquare,
				ActorData.MovementType.None,
				ActorData.TeleportType.Reappear,
				null);
		}
		// end custom
		
		ServerResolutionManager.Get().OnNormalMovementStart();
		ServerMovementManager.Get().ServerMovementManager_OnMovementStart(movementCollection, isChase
			? ServerMovementManager.MovementType.NormalMovement_Chase
			: ServerMovementManager.MovementType.NormalMovement_NonChase);
		foreach (MovementRequest movementRequest in validRequestsThisPhase)
		{
			RunMovementOnRequest(movementRequest);
			ActorStatus actorStatus = movementRequest.m_actor.GetActorStatus();
			if (actorStatus != null && actorStatus.HasStatus(StatusType.KnockedBack))
			{
				actorStatus.RemoveStatus(StatusType.KnockedBack);
			}
		}
	}
#endif
}

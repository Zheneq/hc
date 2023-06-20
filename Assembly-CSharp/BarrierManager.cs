// ROGUES
// SERVER
using System;
using System.Collections.Generic;
//using Mirror;
using UnityEngine;
using UnityEngine.Networking;

public class BarrierManager : NetworkBehaviour
{
	// server-only
#if SERVER
	public static int s_nextBarrierGuid;
#endif

	private static BarrierManager s_instance;

	private List<Barrier> m_barriers = new List<Barrier>();
	private Dictionary<Team, int> m_movementStates = new Dictionary<Team, int>();
	private Dictionary<Team, int> m_visionStates = new Dictionary<Team, int>();

	// rogues
	//private SyncListBarrierSpawnData m_syncNonAbilityBarrierSpawnInfo = new SyncListBarrierSpawnData();

	private List<BarrierSerializeInfo> m_clientBarrierInfo = new List<BarrierSerializeInfo>();

	// server-only or rogues-only? replaces BarrierSerializeInfo.m_clientSequenceStartAttempted, lookup by guid
	//#if SERVER
	//	private Dictionary<int, bool> m_clientSequenceAttempted = new Dictionary<int, bool>();
	//#endif

	// rogues
	//private List<PveScriptBarrierSpawner> m_pveBarrierSpawners = new List<PveScriptBarrierSpawner>();

	private SyncListInt m_barrierIdSync = new SyncListInt();
	private SyncListInt m_movementStatesSync = new SyncListInt();
	private SyncListInt m_visionStatesSync = new SyncListInt();

	private bool m_clientNeedMovementUpdate;
	private bool m_suppressingAbilityBlocks;
	private bool m_hasAbilityBlockingBarriers;

	// removed in rogues
	private static int kListm_barrierIdSync = 1647649475;
	// removed in rogues
	private static int kListm_movementStatesSync = -1285657162;
	// removed in rogues
	private static int kListm_visionStatesSync = -1477195729;
	// removed in rogues
	private static int kRpcRpcUpdateBarriers = 73930193;

	private const int c_teamNum = 3;

	static BarrierManager()
	{
		// reactor
		RegisterRpcDelegate(typeof(BarrierManager), kRpcRpcUpdateBarriers, InvokeRpcRpcUpdateBarriers);
		RegisterSyncListDelegate(typeof(BarrierManager), kListm_barrierIdSync, InvokeSyncListm_barrierIdSync);
		RegisterSyncListDelegate(typeof(BarrierManager), kListm_movementStatesSync, InvokeSyncListm_movementStatesSync);
		RegisterSyncListDelegate(typeof(BarrierManager), kListm_visionStatesSync, InvokeSyncListm_visionStatesSync);
		NetworkCRC.RegisterBehaviour("BarrierManager", 0);
		// rogues
		//NetworkBehaviour.RegisterRpcDelegate(typeof(BarrierManager), "RpcUpdateBarriers", new NetworkBehaviour.CmdDelegate(BarrierManager.InvokeRpcRpcUpdateBarriers));
		//NetworkBehaviour.RegisterRpcDelegate(typeof(BarrierManager), "RpcOnPveBlockerStateChange", new NetworkBehaviour.CmdDelegate(BarrierManager.InvokeRpcRpcOnPveBlockerStateChange));
	}

	// added in rogues -- instead of custom serialization?
	//public BarrierManager()
	//{
	//	base.InitSyncObject(m_syncNonAbilityBarrierSpawnInfo);
	//	base.InitSyncObject(m_barrierIdSync);
	//	base.InitSyncObject(m_movementStatesSync);
	//	base.InitSyncObject(m_visionStatesSync);
	//}

	public static BarrierManager Get()
	{
		return s_instance;
	}

	public void SuppressAbilityBlocks_Start()
	{
		if (!m_suppressingAbilityBlocks)
		{
			m_suppressingAbilityBlocks = true;
		}
		else
		{
			Debug.LogError("BarrierManager was told to start suppressing barrier ability-blocks, but it already was.  Ignoring...");
		}
	}

	public void SuppressAbilityBlocks_End()
	{
		if (m_suppressingAbilityBlocks)
		{
			m_suppressingAbilityBlocks = false;
		}
		else
		{
			Debug.LogError("BarrierManager was told to stop suppressing barrier ability-blocks, but it already wasn't.  Ignoring...");
		}
	}

	public bool SuppressingAbilityBlocks()
	{
		return m_suppressingAbilityBlocks;
	}

	// rogues
	//public void RegisterBarrierSpawner(PveScriptBarrierSpawner spawner)
	//{
	//	if (spawner != null && !m_pveBarrierSpawners.Contains(spawner))
	//	{
	//		m_pveBarrierSpawners.Add(spawner);
	//		PveLog.DebugLog("Registered barrier spawner " + spawner.gameObject.name, null);
	//	}
	//}

	// rogues
	//public void UnregisterBarrierSpawner(PveScriptBarrierSpawner spawner)
	//{
	//	if (spawner != null)
	//	{
	//		m_pveBarrierSpawners.Remove(spawner);
	//	}
	//}

	private void Awake()
	{
		s_instance = this;
		m_movementStates.Add(Team.TeamA, 0);
		m_movementStates.Add(Team.TeamB, 0);
		m_movementStates.Add(Team.Objects, 0);
		m_visionStates.Add(Team.TeamA, 0);
		m_visionStates.Add(Team.TeamB, 0);
		m_visionStates.Add(Team.Objects, 0);

		// removed in rogues
		m_barrierIdSync.InitializeBehaviour(this, kListm_barrierIdSync);
		m_movementStatesSync.InitializeBehaviour(this, kListm_movementStatesSync);
		m_visionStatesSync.InitializeBehaviour(this, kListm_visionStatesSync);

		// moved from OnStartServer in rogues
		//if (NetworkServer.active)
		//{
		//	for (int i = 0; i < 3; i++)
		//	{
		//		m_movementStatesSync.Add(0);
		//		m_visionStatesSync.Add(0);
		//	}
		//}
	}

	private void OnDestroy()
	{
		s_instance = null;
	}

	public bool IsTeamSupported(Team team)
	{
		return team == Team.TeamA || team == Team.TeamB || team == Team.Objects;
	}

	public override void OnStartClient()
	{
		// reactor
		m_movementStatesSync.Callback = SyncListCallbackMovementStates;
		m_visionStatesSync.Callback = SyncListCallbackVisionStates;
		// rogues
		//m_movementStatesSync.Callback += new SyncList<int>.SyncListChanged(SyncListCallbackMovementStates);
		//m_visionStatesSync.Callback += new SyncList<int>.SyncListChanged(SyncListCallbackVisionStates);
		//m_syncNonAbilityBarrierSpawnInfo.Callback += new SyncList<BarrierSerializeInfo>.SyncListChanged(SyncListCallbackBarrierSpawnInfo);
	}

	// moved into awake in rogues
	public override void OnStartServer()
	{
		for (int i = 0; i < c_teamNum; i++)
		{
			m_movementStatesSync.Add(0);
			m_visionStatesSync.Add(0);
		}
	}

	private void Update()
	{
		if (!NetworkServer.active && m_clientNeedMovementUpdate)
		{
			for (int i = 0; i < c_teamNum; i++)
			{
				Team teamFromSyncIndex = GetTeamFromSyncIndex(i);
				if (m_movementStates[teamFromSyncIndex] != m_movementStatesSync[i])
				{
					m_movementStates[teamFromSyncIndex] = m_movementStatesSync[i];
					ClientUpdateMovementOnSync(teamFromSyncIndex);
				}
			}
			m_clientNeedMovementUpdate = false;
		}
	}

	public void AddBarrier(Barrier barrierToAdd, bool delayVisionUpdate, out List<ActorData> visionUpdaters)  // , bool addToSpawnDataSyncList = false in rogues
	{
		visionUpdaters = new List<ActorData>();
		if (m_barriers.Contains(barrierToAdd))
		{
			Log.Error("Trying to add a barrier we've already added.");
		}
		else
		{
			m_barriers.Add(barrierToAdd);
			if (NetworkServer.active)
			{
				m_barrierIdSync.Add(barrierToAdd.m_guid);
				// rogues
				//if (addToSpawnDataSyncList)
				//{
				//	BarrierSerializeInfo barrierSerializeInfo = Barrier.BarrierToSerializeInfo(barrierToAdd);
				//	m_syncNonAbilityBarrierSpawnInfo.Add(barrierSerializeInfo);
				//}
				CallRpcUpdateBarriers();
				if (m_barrierIdSync.Count > 120)
				{
					Debug.LogError("More than " + 120 + " barriers active?");
				}
			}
			barrierToAdd.OnStart(delayVisionUpdate, out visionUpdaters);
		}
		UpdateHasAbilityBlockingBarriers();
	}

	public void RemoveBarrier(Barrier barrierToRemove, bool doRpcUpdate = true)
	{
		if (!m_barriers.Contains(barrierToRemove))
		{
			Log.Error("Trying to remove a barrier we don't have.");
		}
		else
		{
			m_barriers.Remove(barrierToRemove);
			if (NetworkServer.active)
			{
				for (int num = m_barrierIdSync.Count - 1; num >= 0; num--)
				{
					if (m_barrierIdSync[num] == barrierToRemove.m_guid)
					{
						m_barrierIdSync.RemoveAt(num);
					}
				}
				// rogues
				//for (int j = 0; j < m_syncNonAbilityBarrierSpawnInfo.Count; j++)
				//{
				//	if (m_syncNonAbilityBarrierSpawnInfo[j].m_guid == barrierToRemove.m_guid)
				//	{
				//		m_syncNonAbilityBarrierSpawnInfo.RemoveAt(j);
				//		break;
				//	}
				//}
				if (doRpcUpdate)
				{
					CallRpcUpdateBarriers();
				}
			}
			barrierToRemove.OnEnd();
		}
		UpdateHasAbilityBlockingBarriers();
	}

	public void AddClientBarrierInfo(BarrierSerializeInfo info)
	{
		m_clientBarrierInfo.Add(info);
		//#if SERVER
		//		m_clientSequenceAttempted[info.m_guid] = false;  // added in rogues
		//#endif
		if (m_clientBarrierInfo.Count > 100)
		{
			Debug.LogError("More than 100 client barrier info");
		}
	}

	// added in rogues
	//#if SERVER
	//	public bool WasClientSequenceAttempted(int guid)
	//	{
	//		return m_clientSequenceAttempted.TryGetValue(guid, out bool value) && value;
	//	}
	//#endif

	// added in rogues
	//#if SERVER
	//	public void NotifyClientSequenceAttempted(int guid)
	//	{
	//		m_clientSequenceAttempted[guid] = true;
	//	}
	//#endif

	public void RemoveClientBarrierInfo(int guid)
	{
		for (int num = m_clientBarrierInfo.Count - 1; num >= 0; num--)
		{
			if (m_clientBarrierInfo[num].m_guid == guid)
			{
				if (!m_clientBarrierInfo[num].m_clientSequenceStartAttempted)
				//if (!WasClientSequenceAttempted(guid)) // rogues
				{
					Log.Error("Client did not attempt to spawn barrier sequences before it is removed");
				}
				m_clientBarrierInfo.RemoveAt(num);
			}
		}
		//m_clientSequenceAttempted.Remove(guid);  // added in rogues
	}

	public bool HasBarrier(Barrier barrierToCheck)
	{
		return m_barriers.Contains(barrierToCheck);
	}

	public bool IsVisionBlocked(ActorData viewer, BoardSquare source, BoardSquare dest)
	{
		bool isBlocking1 = false;
		bool isBlocking2 = false;
		float traceHalfWidthInSquares = 0.3f;

		int i;
		for (i = 0; i < m_barriers.Count; i++)
		{
			Barrier barrier = m_barriers[i];
			if (!barrier.CanBeSeenThroughBy(viewer))
			{
				break;
			}

		}
		if (i >= m_barriers.Count)
		{
			return false;
		}

		Vector3 src = source.ToVector3();
		Vector3 dst = dest.ToVector3();
		Vector3 shift = Mathf.Abs(source.x - dest.x) <= Mathf.Abs(source.y - dest.y)
			? new Vector3(Board.Get().squareSize * traceHalfWidthInSquares, 0f, 0f)
			: new Vector3(0f, 0f, Board.Get().squareSize * traceHalfWidthInSquares);
		Vector3 src1 = src + shift;
		Vector3 dest2 = dst + shift;
		Vector3 src2 = src - shift;
		Vector3 dest3 = dst - shift;
		for (int j = i; j < m_barriers.Count; j++)
		{
			if (isBlocking1 && isBlocking2)
			{
				break;
			}
			Barrier barrier = m_barriers[j];
			if (!barrier.CanBeSeenThroughBy(viewer))
			{
				if (!isBlocking1 && barrier.CrossingBarrierForVision(src1, dest2))
				{
					isBlocking1 = true;
				}
				if (!isBlocking2 && barrier.CrossingBarrierForVision(src2, dest3))
				{
					isBlocking2 = true;
				}
			}
		}
		return isBlocking1 && isBlocking2;
	}

	public int GetVisionStateChangesFor(ActorData actor)
	{
		Team team = actor.GetTeam();
		if (!IsTeamSupported(team))
		{
			return -1;
		}
		return m_visionStatesSync[GetSyncIndexFromTeam(team)];
	}

	[Server]
	public void UpdateVisionStateForTeam(Team team)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void BarrierManager::UpdateVisionStateForTeam(Team)' called on client");
			return;
		}
		if (!IsTeamSupported(team))
		{
			throw new Exception("BarrierManager does not support this team");
		}
		int syncIndexFromTeam = GetSyncIndexFromTeam(team);
		int stateIdx = m_visionStatesSync[syncIndexFromTeam] + 1;
		m_visionStates[team] = stateIdx;
		m_visionStatesSync[syncIndexFromTeam] = stateIdx;
	}

	public bool IsMovementBlocked(ActorData mover, BoardSquare source, BoardSquare dest)
	{
		foreach (Barrier barrier in m_barriers)
		{
			if (!barrier.CanBeMovedThroughBy(mover)
				&& barrier.CrossingBarrier(source.ToVector3(), dest.ToVector3()))
			{
				return true;
			}
		}
		return false;
	}

	public bool IsMovementBlockedOnCrossover(ActorData mover, BoardSquare source, BoardSquare dest)
	{
		if (mover.GetActorStatus() != null
			&& mover.GetActorStatus().HasStatus(StatusType.Unstoppable))
		{
			return false;
		}

		foreach (Barrier barrier in m_barriers)
		{
			if (!barrier.CanMoveThroughAfterCrossoverBy(mover)
				&& barrier.CrossingBarrier(source.ToVector3(), dest.ToVector3()))
			{
				return true;
			}
		}
		return false;
	}

	public int GetMovementStateChangesFor(ActorData mover)
	{
		Team team = mover.GetTeam();
		if (!IsTeamSupported(team))
		{
			return -1;
		}
		return m_movementStates[team];
	}

	[Server]
	public void UpdateMovementStateForTeam(Team team)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void BarrierManager::UpdateMovementStateForTeam(Team)' called on client");
			return;
		}
		if (!IsTeamSupported(team))
		{
			throw new Exception("BarrierManager does not support this team");
		}
		int syncIndexFromTeam = GetSyncIndexFromTeam(team);
		int stateIdx = m_movementStatesSync[syncIndexFromTeam] + 1;
		m_movementStates[team] = stateIdx;
		m_movementStatesSync[syncIndexFromTeam] = stateIdx;
	}

	public bool IsPositionTargetingBlocked(ActorData caster, BoardSquare dest)
	{
		if (caster == null)
		{
			return true;
		}
		BoardSquare casterSquare = caster.GetCurrentBoardSquare();
		if (casterSquare == null)
		{
			return true;
		}
		foreach (Barrier barrier in m_barriers)
		{
			if (barrier.IsPositionTargetingBlockedFor(caster)
				&& barrier.CrossingBarrier(casterSquare.ToVector3(), dest.ToVector3()))
			{
				return true;
			}
		}
		return false;
	}

	private void UpdateHasAbilityBlockingBarriers()
	{
		bool hasAbilityBlockingBarriers = false;
		foreach (Barrier barrier in m_barriers)
		{
			if (barrier != null && barrier.BlocksAbilities != BlockingRules.ForNobody)
			{
				hasAbilityBlockingBarriers = true;
				break;
			}
		}
		m_hasAbilityBlockingBarriers = hasAbilityBlockingBarriers;
	}

	public bool HasAbilityBlockingBarriers()
	{
		return m_hasAbilityBlockingBarriers;
	}

	public bool AreAbilitiesBlocked(ActorData caster, BoardSquare source, BoardSquare dest, List<NonActorTargetInfo> nonActorTargetInfo)
	{
		return AreAbilitiesBlocked(caster, source.ToVector3(), dest.ToVector3(), nonActorTargetInfo);
	}

	public bool AreAbilitiesBlocked(ActorData caster, Vector3 startPos, Vector3 destPos, List<NonActorTargetInfo> nonActorTargetInfo)
	{
		bool flag = nonActorTargetInfo != null && NetworkServer.active;
		Vector3 crossPos = Vector3.zero;
		float dist = 0f;
		Barrier bar = null;
		bool result = false;
		foreach (Barrier barrier in m_barriers)
		{
			// TODO HIGH looks like a bug -- first call, then check for null
			if (!barrier.CanBeShotThroughBy(caster)
				&& barrier != null
				&& barrier.CrossingBarrier(startPos, destPos))
			{
				if (!flag)
				{
					result = true;
					break;
				}
				Vector3 intersectionPoint = barrier.GetIntersectionPoint(startPos, destPos);
				intersectionPoint.y = startPos.y;
				float magnitude = (intersectionPoint - startPos).magnitude;
				if (!result || magnitude < dist)
				{
					dist = magnitude;
#if SERVER
					// added in rogues
					crossPos = intersectionPoint;
					bar = barrier;
#endif
				}
				result = true;
			}
		}
		if (bar != null)
		{
			// added in rogues -- an empty if in reactor
#if SERVER
			if (flag && result)
			{
				nonActorTargetInfo.Add(new NonActorTargetInfo_BarrierBlock(bar, crossPos, startPos));
			}
#endif
		}

		return result;
	}

	public Vector3 GetAbilityLineEndpoint(ActorData caster, Vector3 lineStart, Vector3 currentEnd, out bool collision, out Vector3 collisionNormal, List<NonActorTargetInfo> nonActorTargetInfo = null)
	{
		Vector3 endpoint = currentEnd;
		collisionNormal = Vector3.zero;
		collision = false;

		Barrier hitBarrier = null;
		foreach (Barrier barrier in m_barriers)
		{
			if (!barrier.CanBeShotThroughBy(caster) && barrier.CrossingBarrier(lineStart, endpoint))
			{
				endpoint = barrier.GetIntersectionPoint(lineStart, endpoint);
				collision = true;
				collisionNormal = barrier.GetCollisionNormal(currentEnd - lineStart);
				hitBarrier = barrier;
			}
		}
		if (hitBarrier != null)
		{
			// added in rogues -- an empty if in reactor
#if SERVER
			if (nonActorTargetInfo != null)
			{
				nonActorTargetInfo.Add(new NonActorTargetInfo_BarrierBlock(hitBarrier, endpoint, lineStart));
			}
#endif
		}

		return endpoint;
	}

	public void UpdateCachedCoverDirections(ActorData forActor, BoardSquare centerSquare, ref bool[] cachedBarrierDirs)
	{
		if (centerSquare == null || forActor == null)
		{
			return;
		}
		Vector3 vector = centerSquare.ToVector3();
		foreach (Barrier barrier in m_barriers)
		{
			if (barrier.ConsiderAsCover && barrier.GetBarrierTeam() == forActor.GetTeam())
			{
				for (int i = 0; i < cachedBarrierDirs.Length; i++)
				{
					if (!cachedBarrierDirs[i])
					{
						Vector3 b = 1.5f * ActorCover.GetCoverOffsetStatic((ActorCover.CoverDirections)i);
						bool flag = barrier.CrossingBarrier(vector + b, vector);
						cachedBarrierDirs[i] = flag;
					}
				}
			}
		}
	}

	// added in rogues
#if SERVER
	public void AddNonActorTargetInfoFromPos(ActorData caster, Vector3 lineStart, List<NonActorTargetInfo> nonActorTargetInfo, IPosInsideChecker posChecker, List<Vector3> additionalLosCheckPos = null)
	{
		lineStart.y = (float)Board.Get().BaselineHeight + BoardSquare.s_LoSHeightOffset;
		List<Vector3> list = new List<Vector3>
		{
			lineStart
		};
		if (additionalLosCheckPos != null)
		{
			for (int i = 0; i < additionalLosCheckPos.Count; i++)
			{
				Vector3 item = additionalLosCheckPos[i];
				item.y = lineStart.y;
				list.Add(item);
			}
		}
		for (int j = 0; j < m_barriers.Count; j++)
		{
			Barrier barrier = m_barriers[j];
			if (!barrier.CanBeShotThroughBy(caster))
			{
				Vector3 facingDir = barrier.GetFacingDir();
				List<Vector3> list2 = new List<Vector3>(3);
				Vector3 centerPos = barrier.GetCenterPos();
				list2.Add(centerPos);
				Vector3 vector = barrier.GetEndPos1() - centerPos;
				if (posChecker == null || !posChecker.AddTestPosForBarrier(list2, barrier))
				{
					list2.Add(centerPos + 0.9f * vector);
					list2.Add(centerPos - 0.9f * vector);
				}
				bool flag = false;
				int num = 0;
				while (num < list2.Count && !flag)
				{
					Vector3 vector2 = list2[num];
					foreach (Vector3 vector3 in list)
					{
						Vector3 vector4 = vector2 - vector3;
						vector4.y = 0f;
						vector4.Normalize();
						Vector3 vector5 = 0.1f * facingDir;
						if ((posChecker == null || (posChecker.IsPositionInside(vector2 + vector5) && posChecker.IsPositionInside(vector2 - vector5))) && barrier.CrossingBarrier(vector3, vector2))
						{
							Vector3 dir = vector2 - vector3;
							dir.y = 0f;
							float magnitude = dir.magnitude;
							if (!VectorUtils.RaycastInDirection(vector3, dir, magnitude, out RaycastHit raycastHit))
							{
								if (nonActorTargetInfo != null)
								{
									nonActorTargetInfo.Add(new NonActorTargetInfo_BarrierBlock(barrier, vector2, vector3));
								}
								flag = true;
								break;
							}
						}
					}
					num++;
				}
			}
		}
	}
#endif

	// added in rogues
#if SERVER
	public void OnTurnEnd(Team team = Team.Invalid)
	{
		List<Barrier> list = new List<Barrier>();
		for (int i = 0; i < m_barriers.Count; i++)
		{
			Barrier barrier = m_barriers[i];
			bool flag = false;
			if (team == Team.TeamA)
			{
				if (barrier.Caster != null && barrier.Caster.GetTeam() == team)
				{
					flag = true;
				}
			}
			else if (team == Team.TeamB)
			{
				if (barrier.Caster == null || barrier.Caster.GetTeam() == team)
				{
					flag = true;
				}
			}
			else
			{
				flag = true;
			}
			if (flag)
			{
				barrier.m_time.age++;
				barrier.OnTurnEnd();
				if (barrier.ShouldEnd())
				{
					list.Add(barrier);
				}
			}
		}
		foreach (Barrier barrierToRemove in list)
		{
			RemoveBarrier(barrierToRemove, false);
		}
		if (list.Count > 0)
		{
			CallRpcUpdateBarriers();
		}
	}

	// added in rogues
	public void OnTurnStart()
	{
		List<Barrier> list = new List<Barrier>();
		for (int i = 0; i < m_barriers.Count; i++)
		{
			Barrier barrier = m_barriers[i];
			barrier.OnTurnStart();
			if (barrier.ShouldEnd())
			{
				list.Add(barrier);
			}
		}
		foreach (Barrier barrierToRemove in list)
		{
			RemoveBarrier(barrierToRemove, false);
		}
		// rogues?
		//for (int j = 0; j < m_pveBarrierSpawners.Count; j++)
		//{
		//	m_pveBarrierSpawners[j].OnTurnStart_BarrierSpawner();
		//}
		CallRpcUpdateBarriers();
	}

	// added in rogues
	public void OnAbilityPhaseEnd(AbilityPriority phase)
	{
		bool flag = false;
		for (int i = m_barriers.Count - 1; i >= 0; i--)
		{
			Barrier barrier = m_barriers[i];
			if (barrier.ShouldEndAtEndOfPhase(phase))
			{
				RemoveBarrier(barrier, false);
				flag = true;
			}
		}
		if (flag)
		{
			CallRpcUpdateBarriers();
		}
	}

	// added in rogues
	public void ExecuteUnexecutedMovementHitsForAllBarriers(MovementStage movementStage, bool asFailsafe)
	{
		foreach (Barrier barrier in new List<Barrier>(m_barriers))
		{
			if (barrier != null && HasBarrier(barrier))
			{
				barrier.ExecuteUnexecutedMovementResults_Barrier(movementStage, asFailsafe);
			}
		}
		CallRpcUpdateBarriers();
	}

	// added in rogues
	public void ExecuteUnexecutedMovementHitsForAllBarriersForDistance(float distance, MovementStage movementStage, bool asFailsafe, out bool stillHasUnexecutedHits, out float nextUnexecutedHitDistance)
	{
		stillHasUnexecutedHits = false;
		nextUnexecutedHitDistance = float.MaxValue;
		foreach (Barrier barrier in new List<Barrier>(m_barriers))
		{
			if (barrier != null && HasBarrier(barrier))
			{
				barrier.ExecuteUnexecutedMovementResultsForDistance_Barrier(distance, movementStage, asFailsafe, out bool flag, out float num);
				if (flag)
				{
					stillHasUnexecutedHits |= true;
					if (num < nextUnexecutedHitDistance)
					{
						nextUnexecutedHitDistance = num;
					}
				}
			}
		}
	}
#endif

	private Team GetTeamFromSyncIndex(int index)
	{
		switch (index)
		{
			case 0:
				return Team.TeamA;
			case 1:
				return Team.TeamB;
			case 2:
				return Team.Objects;
			default:
				return Team.Invalid;
		}
	}

	private int GetSyncIndexFromTeam(Team team)
	{
		switch (team)
		{
			case Team.TeamA:
				return 0;
			case Team.TeamB:
				return 1;
			case Team.Objects:
				return 2;
			default:
				Debug.LogError("Invalid team passed to GetSyncIndexFromTeam()");
				return 0;
		}
	}

	[ClientRpc]
	private void RpcUpdateBarriers()
	{
		ClientUpdateBarriers();
	}

	private void ClientUpdateBarriers()
	{
		if (NetworkServer.active)
		{
			return;
		}

		bool hasBarrierCovers = false;
		foreach (Barrier barrier in m_barriers)
		{
			if (barrier.ConsiderAsCover)
			{
				hasBarrierCovers = true;
				break;
			}
		}

		// reactor
		m_barriers.Clear();
		// rogues
		//List<int> list = new List<int>();
		//      for (int j = m_barriers.Count - 1; j >= 0; j--)
		//      {
		//          if (!m_barrierIdSync.Contains(m_barriers[j].m_guid))
		//          {
		//              m_barriers.RemoveAt(j);
		//          }
		//          else
		//          {
		//              list.Add(m_barriers[j].m_guid);
		//          }
		//      }
		// end rogues

		if (m_barrierIdSync.Count > 50)
		{
			Debug.LogError("More than 50 barriers active?");  // LogWarning in rogues
		}

		foreach (int barrierId in m_barrierIdSync)
		{
			//if (!list.Contains(barrierId)) // added in rogues -- unconditional in reactor
			//{
			foreach (BarrierSerializeInfo cached in m_clientBarrierInfo)
			{
				if (cached.m_guid == barrierId)
				{
					Barrier barrier = Barrier.CreateBarrierFromSerializeInfo(cached);
					if (barrier.ConsiderAsCover)
					{
						hasBarrierCovers = true;
					}
					AddBarrier(barrier, false, out List<ActorData> _);
					break;
				}
			}

			// rogues?
			//foreach (BarrierSerializeInfo barrierSerializeInfo2 in m_syncNonAbilityBarrierSpawnInfo)
			//{
			//    if (barrierSerializeInfo2.m_guid == m_barrierIdSync[i])
			//    {
			//        Barrier barrier2 = Barrier.CreateBarrierFromSerializeInfo(barrierSerializeInfo2);
			//        if (barrier2.ConsiderAsCover)
			//        {
			//            hasBarrierCovers = true;
			//        }
			//        List<ActorData> list3;
			//        AddBarrier(barrier2, false, out list3, false);
			//        break;
			//    }
			//}
			//}
		}
		ClientUpdateMovementAndVision();
		UpdateHasAbilityBlockingBarriers();
		if (hasBarrierCovers)
		{
			GameFlowData.Get().UpdateCoverFromBarriersForAllActors();
		}
	}

	// rogues
	//   [ClientRpc]
	//public void RpcOnPveBlockerStateChange(int barrierSpawnerKey, bool activeNow)
	//{
	//	for (int i = 0; i < m_pveBarrierSpawners.Count; i++)
	//	{
	//		if (m_pveBarrierSpawners[i].GetIdentifier() == barrierSpawnerKey)
	//		{
	//			m_pveBarrierSpawners[i].SetVisualObjectState(activeNow);
	//			return;
	//		}
	//	}
	//}

	// reactor
	private void SyncListCallbackMovementStates(SyncList<int>.Operation op, int index)
	{
		m_clientNeedMovementUpdate = true;
	}
	// rogues
	//private void SyncListCallbackMovementStates(SyncList<int>.Operation op, int index, int item)
	//{
	//	m_clientNeedMovementUpdate = true;
	//}

	// reactor
	private void SyncListCallbackVisionStates(SyncList<int>.Operation op, int index)
	{
		ClientUpdateMovementAndVision();
	}
	// rogues
	//private void SyncListCallbackVisionStates(SyncList<int>.Operation op, int index, int item)
	//{
	//	ClientUpdateMovementAndVision();
	//}

	// rogues
	//private void SyncListCallbackBarrierSpawnInfo(SyncList<BarrierSerializeInfo>.Operation op, int index, BarrierSerializeInfo item)
	//{
	//	ClientUpdateBarriers();
	//}

	[Client]
	private void ClientUpdateMovementAndVision()
	{
		if (!NetworkClient.active)
		{
			Debug.LogWarning("[Client] function 'System.Void BarrierManager::ClientUpdateMovementAndVision()' called on server");
			return;
		}
		if (NetworkServer.active)
		{
			return;
		}
		for (int i = 0; i < c_teamNum; i++)
		{
			Team teamFromSyncIndex = GetTeamFromSyncIndex(i);
			if (m_movementStatesSync[i] != m_movementStates[teamFromSyncIndex])
			{
				m_clientNeedMovementUpdate = true;
			}
			if (m_visionStatesSync[i] != m_visionStates[teamFromSyncIndex])
			{
				m_visionStates[teamFromSyncIndex] = m_visionStatesSync[i];
				ClientUpdateVisionOnSync(teamFromSyncIndex);
			}
		}
	}

	[Client]
	private void ClientUpdateMovementOnSync(Team team)
	{
		if (!NetworkClient.active)
		{
			Debug.LogWarning("[Client] function 'System.Void BarrierManager::ClientUpdateMovementOnSync(Team)' called on server");
			return;
		}
		ActorData actorData = GameFlowData.Get()?.activeOwnedActorData;
		if (actorData != null && actorData.GetTeam() == team)
		{
			actorData.GetActorMovement().UpdateSquaresCanMoveTo();
		}
	}

	[Client]
	private void ClientUpdateVisionOnSync(Team team)
	{
		if (!NetworkClient.active)
		{
			Debug.LogWarning("[Client] function 'System.Void BarrierManager::ClientUpdateVisionOnSync(Team)' called on server");
			return;
		}
		ActorData actorData = GameFlowData.Get()?.activeOwnedActorData;
		if (actorData != null && actorData.GetTeam() == team)
		{
			actorData.GetFogOfWar().MarkForRecalculateVisibility();
		}
	}

#if SERVER
	// server-only
	public void LinkBarriers(List<Barrier> barriers, LinkedBarrierData linkData)
	{
		foreach (Barrier barrier in barriers)
		{
			foreach (Barrier otherBarrier in barriers)
			{
				barrier.LinkWithBarrier(otherBarrier, linkData);
			}
		}
	}

	// server-only
	public void RemoveBarrierAndLinkedSiblings(Barrier barrier)
	{
		if (barrier != null)
		{
			foreach (Barrier barrierToRemove in barrier.GetLinkedBarriers())
			{
				RemoveBarrier(barrierToRemove, true);
			}
			RemoveBarrier(barrier, true);
		}
	}

	// server-only
	public void GatherAllBarrierResultsInResponseToEvades(MovementCollection evadeMovementCollection)
	{
		foreach (Barrier barrier in m_barriers)
		{
			barrier.GatherResultsInResponseToEvades(evadeMovementCollection);
		}
	}

	// server-only
	public void GatherAllBarrierResultsInResponseToKnockbacks(MovementCollection knockbackCollection)
	{
		foreach (Barrier barrier in m_barriers)
		{
			barrier.GatherResultsInResponseToKnockbacks(knockbackCollection);
		}
	}

	// server-only
	public void GatherAllBarrierResultsInResponseToMovementSegment(
		ServerGameplayUtils.MovementGameplayData gameplayData,
		MovementStage movementStage,
		ref List<MovementResults> moveResultsForSegment)
	{
		foreach (Barrier barrier in m_barriers)
		{
			List<MovementResults> moveResults = new List<MovementResults>();
			barrier.GatherBarrierResultsInResponseToMovementSegment(gameplayData, movementStage, ref moveResults);
			List<MovementResults> movementResultsForMovementStage = barrier.GetMovementResultsForMovementStage(movementStage);
			foreach (MovementResults moveResult in moveResults)
			{
				if (moveResult.ShouldMovementHitUpdateTargetLastKnownPos(gameplayData.Actor))
				{
					gameplayData.m_currentlyConsideredPath.m_visibleToEnemies = true;
					gameplayData.m_currentlyConsideredPath.m_updateLastKnownPos = true;
				}
				gameplayData.m_currentlyConsideredPath.m_moverHasGameplayHitHere = true;
				movementResultsForMovementStage.Add(moveResult);
				moveResultsForSegment.Add(moveResult);
			}
		}
	}

	// server-only
	public void ClearAllBarrierResultsForNormalMovement()
	{
		foreach (Barrier barrier in m_barriers)
		{
			barrier.ClearNormalMovementResults();
		}
	}

	// server-only
	public void IntegrateMovementDamageResults_Evasion(ref Dictionary<ActorData, int> actorToDeltaHP)
	{
		foreach (Barrier barrier in m_barriers)
		{
			barrier.IntegrateDamageResultsForEvasion(ref actorToDeltaHP);
		}
	}

	// server-only
	public void IntegrateMovementDamageResults_Knockback(ref Dictionary<ActorData, int> actorToDeltaHP)
	{
		foreach (Barrier barrier in m_barriers)
		{
			barrier.IntegrateDamageResultsForKnockback(ref actorToDeltaHP);
		}
	}

	// server-only
	public void GatherGrossDamageResults_Barriers_Evasion(ref Dictionary<ActorData, int> actorToGrossDamage_real, ref Dictionary<ActorData, ServerGameplayUtils.DamageDodgedStats> stats)
	{
		foreach (Barrier barrier in m_barriers)
		{
			barrier.GatherGrossDamageResults_Barrier_Evasion(ref actorToGrossDamage_real, ref stats);
		}
	}

	// server-only
	public List<Barrier> GetAllBarriers()
	{
		return m_barriers;
	}
#endif

	private void OnDrawGizmos()
	{
		if (!CameraManager.ShouldDrawGizmosForCurrentCamera() || m_barriers == null)
		{
			return;
		}
		foreach (Barrier barrier in m_barriers)
		{
			barrier.DrawGizmos();
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

	// removed in rogues
	protected static void InvokeSyncListm_barrierIdSync(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("SyncList m_barrierIdSync called on server.");
			return;
		}
		((BarrierManager)obj).m_barrierIdSync.HandleMsg(reader);
	}

	// removed in rogues
	protected static void InvokeSyncListm_movementStatesSync(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("SyncList m_movementStatesSync called on server.");
			return;
		}
		((BarrierManager)obj).m_movementStatesSync.HandleMsg(reader);
	}

	// removed in rogues
	protected static void InvokeSyncListm_visionStatesSync(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("SyncList m_visionStatesSync called on server.");
			return;
		}
		((BarrierManager)obj).m_visionStatesSync.HandleMsg(reader);
	}

	protected static void InvokeRpcRpcUpdateBarriers(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcUpdateBarriers called on server.");
			return;
		}
		((BarrierManager)obj).RpcUpdateBarriers();
	}

	// rogues
	//protected static void InvokeRpcRpcOnPveBlockerStateChange(NetworkBehaviour obj, NetworkReader reader)
	//{
	//	if (!NetworkClient.active)
	//	{
	//		Debug.LogError("RPC RpcOnPveBlockerStateChange called on server.");
	//		return;
	//	}
	//	((BarrierManager)obj).RpcOnPveBlockerStateChange(reader.ReadPackedInt32(), reader.ReadBoolean());
	//}

	public void CallRpcUpdateBarriers()
	{
		// reactor
		if (!NetworkServer.active)
		{
			Debug.LogError("RPC Function RpcUpdateBarriers called on client.");
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)2);
		networkWriter.WritePackedUInt32((uint)kRpcRpcUpdateBarriers);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		SendRPCInternal(networkWriter, 0, "RpcUpdateBarriers");
		// rogues
		//NetworkWriter networkWriter = new NetworkWriter();
		//SendRPCInternal(typeof(BarrierManager), "RpcUpdateBarriers", networkWriter, 0);
	}

	// rogues
	//public void CallRpcOnPveBlockerStateChange(int barrierSpawnerKey, bool activeNow)
	//{
	//	NetworkWriter networkWriter = new NetworkWriter();
	//	networkWriter.WritePackedInt32(barrierSpawnerKey);
	//	networkWriter.Write(activeNow);
	//	SendRPCInternal(typeof(BarrierManager), "RpcOnPveBlockerStateChange", networkWriter, 0);
	//}

	// removed in rogues
	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		if (forceAll)
		{
			SyncListInt.WriteInstance(writer, m_barrierIdSync);
			SyncListInt.WriteInstance(writer, m_movementStatesSync);
			SyncListInt.WriteInstance(writer, m_visionStatesSync);
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
			SyncListInt.WriteInstance(writer, m_barrierIdSync);
		}
		if ((syncVarDirtyBits & 2) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			SyncListInt.WriteInstance(writer, m_movementStatesSync);
		}
		if ((syncVarDirtyBits & 4) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			SyncListInt.WriteInstance(writer, m_visionStatesSync);
		}
		if (!flag)
		{
			writer.WritePackedUInt32(syncVarDirtyBits);
		}
		return flag;
	}

	// removed in rogues
	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
		if (initialState)
		{
			SyncListInt.ReadReference(reader, m_barrierIdSync);
			SyncListInt.ReadReference(reader, m_movementStatesSync);
			SyncListInt.ReadReference(reader, m_visionStatesSync);
			return;
		}
		int num = (int)reader.ReadPackedUInt32();
		if ((num & 1) != 0)
		{
			SyncListInt.ReadReference(reader, m_barrierIdSync);
		}
		if ((num & 2) != 0)
		{
			SyncListInt.ReadReference(reader, m_movementStatesSync);
		}
		if ((num & 4) != 0)
		{
			SyncListInt.ReadReference(reader, m_visionStatesSync);
		}
	}
}

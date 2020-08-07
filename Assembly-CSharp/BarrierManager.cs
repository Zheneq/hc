using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BarrierManager : NetworkBehaviour
{
	private static BarrierManager s_instance;

	private List<Barrier> m_barriers = new List<Barrier>();
	private Dictionary<Team, int> m_movementStates = new Dictionary<Team, int>();
	private Dictionary<Team, int> m_visionStates = new Dictionary<Team, int>();
	private List<BarrierSerializeInfo> m_clientBarrierInfo = new List<BarrierSerializeInfo>();

	private SyncListInt m_barrierIdSync = new SyncListInt();
	private SyncListInt m_movementStatesSync = new SyncListInt();
	private SyncListInt m_visionStatesSync = new SyncListInt();

	private bool m_clientNeedMovementUpdate;
	private bool m_suppressingAbilityBlocks;
	private bool m_hasAbilityBlockingBarriers;

	private static int kListm_barrierIdSync = 1647649475;
	private static int kListm_movementStatesSync = -1285657162;
	private static int kListm_visionStatesSync = -1477195729;
	private static int kRpcRpcUpdateBarriers = 73930193;

	private const int c_teamNum = 3;

	static BarrierManager()
	{
		RegisterRpcDelegate(typeof(BarrierManager), kRpcRpcUpdateBarriers, InvokeRpcRpcUpdateBarriers);
		RegisterSyncListDelegate(typeof(BarrierManager), kListm_barrierIdSync, InvokeSyncListm_barrierIdSync);
		RegisterSyncListDelegate(typeof(BarrierManager), kListm_movementStatesSync, InvokeSyncListm_movementStatesSync);
		RegisterSyncListDelegate(typeof(BarrierManager), kListm_visionStatesSync, InvokeSyncListm_visionStatesSync);
		NetworkCRC.RegisterBehaviour("BarrierManager", 0);
	}

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

	private void Awake()
	{
		s_instance = this;
		m_movementStates.Add(Team.TeamA, 0);
		m_movementStates.Add(Team.TeamB, 0);
		m_movementStates.Add(Team.Objects, 0);
		m_visionStates.Add(Team.TeamA, 0);
		m_visionStates.Add(Team.TeamB, 0);
		m_visionStates.Add(Team.Objects, 0);
		m_barrierIdSync.InitializeBehaviour(this, kListm_barrierIdSync);
		m_movementStatesSync.InitializeBehaviour(this, kListm_movementStatesSync);
		m_visionStatesSync.InitializeBehaviour(this, kListm_visionStatesSync);
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
		m_movementStatesSync.Callback = SyncListCallbackMovementStates;
		m_visionStatesSync.Callback = SyncListCallbackVisionStates;
	}

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

	public void AddBarrier(Barrier barrierToAdd, bool delayVisionUpdate, out List<ActorData> visionUpdaters)
	{
		visionUpdaters = new List<ActorData>();
		if (m_barriers.Contains(barrierToAdd))
		{
			Log.Error("Trying to add a barrier we've already added.");
		}
		else
		{
			Log.Info($"New barrier: by {barrierToAdd.Caster.DisplayName} at {barrierToAdd.GetCenterPos()} ({barrierToAdd.GetEndPos1()}, {barrierToAdd.GetEndPos2()})");
			m_barriers.Add(barrierToAdd);
			if (NetworkServer.active)
			{
				m_barrierIdSync.Add(barrierToAdd.m_guid);
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
		if (m_clientBarrierInfo.Count > 100)
		{
			Debug.LogError("More than 100 client barrier info");
		}
	}

	public void RemoveClientBarrierInfo(int guid)
	{
		for (int num = m_clientBarrierInfo.Count - 1; num >= 0; num--)
		{
			if (m_clientBarrierInfo[num].m_guid == guid)
			{
				if (!m_clientBarrierInfo[num].m_clientSequenceStartAttempted)
				{
					Log.Error("Client did not attempt to spawn barrier sequences before it is removed");
				}
				m_clientBarrierInfo.RemoveAt(num);
			}
		}
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
		float dist = 0f;
		Barrier bar = null;  // TODO missing code regarding bar
		bool result = false;
		foreach (Barrier barrier in m_barriers)
		{
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
				}
				result = true;
			}
		}
		if (bar != null)
		{
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
		// TODO missing code
		if (hitBarrier != null)
		{
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
		Log.Info("BarrierManager.RpcUpdateBarriers");
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

		m_barriers.Clear();
		if (m_barrierIdSync.Count > 50)
		{
			Debug.LogError("More than 50 barriers active?");
		}

		foreach (int barrierId in m_barrierIdSync)
		{
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
		}
		ClientUpdateMovementAndVision();
		UpdateHasAbilityBlockingBarriers();
		if (hasBarrierCovers)
		{
			GameFlowData.Get().UpdateCoverFromBarriersForAllActors();
		}
	}

	private void SyncListCallbackMovementStates(SyncList<int>.Operation op, int index)
	{
		m_clientNeedMovementUpdate = true;
	}

	private void SyncListCallbackVisionStates(SyncList<int>.Operation op, int index)
	{
		ClientUpdateMovementAndVision();
	}

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

	private void UNetVersion()
	{
	}

	protected static void InvokeSyncListm_barrierIdSync(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("SyncList m_barrierIdSync called on server.");
			return;
		}
		((BarrierManager)obj).m_barrierIdSync.HandleMsg(reader);
	}

	protected static void InvokeSyncListm_movementStatesSync(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("SyncList m_movementStatesSync called on server.");
			return;
		}
		((BarrierManager)obj).m_movementStatesSync.HandleMsg(reader);
	}

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

	public void CallRpcUpdateBarriers()
	{
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
	}

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

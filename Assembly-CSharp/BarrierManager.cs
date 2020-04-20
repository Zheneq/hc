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

	private static int kListm_barrierIdSync;

	private static int kListm_movementStatesSync;

	private static int kListm_visionStatesSync;

	private static int kRpcRpcUpdateBarriers = 0x46815D1;

	static BarrierManager()
	{
		NetworkBehaviour.RegisterRpcDelegate(typeof(BarrierManager), BarrierManager.kRpcRpcUpdateBarriers, new NetworkBehaviour.CmdDelegate(BarrierManager.InvokeRpcRpcUpdateBarriers));
		BarrierManager.kListm_barrierIdSync = 0x623522C3;
		NetworkBehaviour.RegisterSyncListDelegate(typeof(BarrierManager), BarrierManager.kListm_barrierIdSync, new NetworkBehaviour.CmdDelegate(BarrierManager.InvokeSyncListm_barrierIdSync));
		BarrierManager.kListm_movementStatesSync = -0x4CA1924A;
		NetworkBehaviour.RegisterSyncListDelegate(typeof(BarrierManager), BarrierManager.kListm_movementStatesSync, new NetworkBehaviour.CmdDelegate(BarrierManager.InvokeSyncListm_movementStatesSync));
		BarrierManager.kListm_visionStatesSync = -0x580C37D1;
		NetworkBehaviour.RegisterSyncListDelegate(typeof(BarrierManager), BarrierManager.kListm_visionStatesSync, new NetworkBehaviour.CmdDelegate(BarrierManager.InvokeSyncListm_visionStatesSync));
		NetworkCRC.RegisterBehaviour("BarrierManager", 0);
	}

	public static BarrierManager Get()
	{
		return BarrierManager.s_instance;
	}

	public void SuppressAbilityBlocks_Start()
	{
		if (!this.m_suppressingAbilityBlocks)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BarrierManager.SuppressAbilityBlocks_Start()).MethodHandle;
			}
			this.m_suppressingAbilityBlocks = true;
		}
		else
		{
			Debug.LogError("BarrierManager was told to start suppressing barrier ability-blocks, but it already was.  Ignoring...");
		}
	}

	public void SuppressAbilityBlocks_End()
	{
		if (this.m_suppressingAbilityBlocks)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(BarrierManager.SuppressAbilityBlocks_End()).MethodHandle;
			}
			this.m_suppressingAbilityBlocks = false;
		}
		else
		{
			Debug.LogError("BarrierManager was told to stop suppressing barrier ability-blocks, but it already wasn't.  Ignoring...");
		}
	}

	public bool SuppressingAbilityBlocks()
	{
		return this.m_suppressingAbilityBlocks;
	}

	private void Awake()
	{
		BarrierManager.s_instance = this;
		this.m_movementStates.Add(Team.TeamA, 0);
		this.m_movementStates.Add(Team.TeamB, 0);
		this.m_movementStates.Add(Team.Objects, 0);
		this.m_visionStates.Add(Team.TeamA, 0);
		this.m_visionStates.Add(Team.TeamB, 0);
		this.m_visionStates.Add(Team.Objects, 0);
		this.m_barrierIdSync.InitializeBehaviour(this, BarrierManager.kListm_barrierIdSync);
		this.m_movementStatesSync.InitializeBehaviour(this, BarrierManager.kListm_movementStatesSync);
		this.m_visionStatesSync.InitializeBehaviour(this, BarrierManager.kListm_visionStatesSync);
	}

	private void OnDestroy()
	{
		BarrierManager.s_instance = null;
	}

	public bool IsTeamSupported(Team team)
	{
		bool result;
		if (team != Team.TeamA && team != Team.TeamB)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BarrierManager.IsTeamSupported(Team)).MethodHandle;
			}
			result = (team == Team.Objects);
		}
		else
		{
			result = true;
		}
		return result;
	}

	public override void OnStartClient()
	{
		this.m_movementStatesSync.Callback = new SyncList<int>.SyncListChanged(this.SyncListCallbackMovementStates);
		this.m_visionStatesSync.Callback = new SyncList<int>.SyncListChanged(this.SyncListCallbackVisionStates);
	}

	public override void OnStartServer()
	{
		for (int i = 0; i < 3; i++)
		{
			this.m_movementStatesSync.Add(0);
			this.m_visionStatesSync.Add(0);
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
			RuntimeMethodHandle runtimeMethodHandle = methodof(BarrierManager.OnStartServer()).MethodHandle;
		}
	}

	private void Update()
	{
		if (!NetworkServer.active)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(BarrierManager.Update()).MethodHandle;
			}
			if (this.m_clientNeedMovementUpdate)
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
				for (int i = 0; i < 3; i++)
				{
					Team teamFromSyncIndex = this.GetTeamFromSyncIndex(i);
					if (this.m_movementStates[teamFromSyncIndex] != this.m_movementStatesSync[i])
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
						this.m_movementStates[teamFromSyncIndex] = this.m_movementStatesSync[i];
						this.ClientUpdateMovementOnSync(teamFromSyncIndex);
					}
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
				this.m_clientNeedMovementUpdate = false;
			}
		}
	}

	public unsafe void AddBarrier(Barrier barrierToAdd, bool delayVisionUpdate, out List<ActorData> visionUpdaters)
	{
		visionUpdaters = new List<ActorData>();
		if (this.m_barriers.Contains(barrierToAdd))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BarrierManager.AddBarrier(Barrier, bool, List<ActorData>*)).MethodHandle;
			}
			Log.Error("Trying to add a barrier we've already added.", new object[0]);
		}
		else
		{
			this.m_barriers.Add(barrierToAdd);
			if (NetworkServer.active)
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
				this.m_barrierIdSync.Add(barrierToAdd.m_guid);
				this.CallRpcUpdateBarriers();
				if (this.m_barrierIdSync.Count > 0x78)
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
					Debug.LogError("More than " + 0x78 + " barriers active?");
				}
			}
			barrierToAdd.OnStart(delayVisionUpdate, out visionUpdaters);
		}
		this.UpdateHasAbilityBlockingBarriers();
	}

	public void RemoveBarrier(Barrier barrierToRemove, bool doRpcUpdate = true)
	{
		if (!this.m_barriers.Contains(barrierToRemove))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BarrierManager.RemoveBarrier(Barrier, bool)).MethodHandle;
			}
			Log.Error("Trying to remove a barrier we don't have.", new object[0]);
		}
		else
		{
			this.m_barriers.Remove(barrierToRemove);
			if (NetworkServer.active)
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
				for (int i = this.m_barrierIdSync.Count - 1; i >= 0; i--)
				{
					if (this.m_barrierIdSync[i] == barrierToRemove.m_guid)
					{
						this.m_barrierIdSync.RemoveAt(i);
					}
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
				if (doRpcUpdate)
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
					this.CallRpcUpdateBarriers();
				}
			}
			barrierToRemove.OnEnd();
		}
		this.UpdateHasAbilityBlockingBarriers();
	}

	public void AddClientBarrierInfo(BarrierSerializeInfo info)
	{
		this.m_clientBarrierInfo.Add(info);
		if (this.m_clientBarrierInfo.Count > 0x64)
		{
			Debug.LogError("More than 100 client barrier info");
		}
	}

	public void RemoveClientBarrierInfo(int guid)
	{
		for (int i = this.m_clientBarrierInfo.Count - 1; i >= 0; i--)
		{
			if (this.m_clientBarrierInfo[i].m_guid == guid)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(BarrierManager.RemoveClientBarrierInfo(int)).MethodHandle;
				}
				if (!this.m_clientBarrierInfo[i].m_clientSequenceStartAttempted)
				{
					Log.Error("Client did not attempt to spawn barrier sequences before it is removed", new object[0]);
				}
				this.m_clientBarrierInfo.RemoveAt(i);
			}
		}
	}

	public bool HasBarrier(Barrier barrierToCheck)
	{
		return this.m_barriers.Contains(barrierToCheck);
	}

	public bool IsVisionBlocked(ActorData viewer, BoardSquare source, BoardSquare dest)
	{
		bool flag = false;
		bool flag2 = false;
		float num = 0.3f;
		for (int i = 0; i < this.m_barriers.Count; i++)
		{
			Barrier barrier = this.m_barriers[i];
			if (!barrier.CanBeSeenThroughBy(viewer))
			{
				IL_51:
				if (i < this.m_barriers.Count)
				{
					Vector3 a = source.ToVector3();
					Vector3 a2 = dest.ToVector3();
					Vector3 b;
					if (Mathf.Abs(source.x - dest.x) > Mathf.Abs(source.y - dest.y))
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
						b = new Vector3(0f, 0f, Board.Get().squareSize * num);
					}
					else
					{
						b = new Vector3(Board.Get().squareSize * num, 0f, 0f);
					}
					Vector3 src = a + b;
					Vector3 dest2 = a2 + b;
					Vector3 src2 = a - b;
					Vector3 dest3 = a2 - b;
					for (int j = i; j < this.m_barriers.Count; j++)
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
								break;
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
									goto IL_1D4;
								}
							}
						}
						Barrier barrier2 = this.m_barriers[j];
						if (!barrier2.CanBeSeenThroughBy(viewer))
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
								if (barrier2.CrossingBarrierForVision(src, dest2))
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
									flag = true;
								}
							}
							if (!flag2)
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
								if (barrier2.CrossingBarrierForVision(src2, dest3))
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
									flag2 = true;
								}
							}
						}
					}
				}
				IL_1D4:
				bool result;
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
					result = flag2;
				}
				else
				{
					result = false;
				}
				return result;
			}
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
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(BarrierManager.IsVisionBlocked(ActorData, BoardSquare, BoardSquare)).MethodHandle;
			goto IL_51;
		}
		goto IL_51;
	}

	public int GetVisionStateChangesFor(ActorData actor)
	{
		Team team = actor.GetTeam();
		if (!this.IsTeamSupported(team))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BarrierManager.GetVisionStateChangesFor(ActorData)).MethodHandle;
			}
			return -1;
		}
		int syncIndexFromTeam = this.GetSyncIndexFromTeam(team);
		return this.m_visionStatesSync[syncIndexFromTeam];
	}

	[Server]
	public void UpdateVisionStateForTeam(Team team)
	{
		if (!NetworkServer.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BarrierManager.UpdateVisionStateForTeam(Team)).MethodHandle;
			}
			Debug.LogWarning("[Server] function 'System.Void BarrierManager::UpdateVisionStateForTeam(Team)' called on client");
			return;
		}
		if (this.IsTeamSupported(team))
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
			int syncIndexFromTeam = this.GetSyncIndexFromTeam(team);
			int num = this.m_visionStatesSync[syncIndexFromTeam];
			int value = num + 1;
			this.m_visionStates[team] = value;
			this.m_visionStatesSync[syncIndexFromTeam] = value;
			return;
		}
		throw new Exception("BarrierManager does not support this team");
	}

	public bool IsMovementBlocked(ActorData mover, BoardSquare source, BoardSquare dest)
	{
		bool result = false;
		for (int i = 0; i < this.m_barriers.Count; i++)
		{
			Barrier barrier = this.m_barriers[i];
			if (!barrier.CanBeMovedThroughBy(mover) && barrier.CrossingBarrier(source.ToVector3(), dest.ToVector3()))
			{
				result = true;
				break;
			}
		}
		return result;
	}

	public bool IsMovementBlockedOnCrossover(ActorData mover, BoardSquare source, BoardSquare dest)
	{
		bool result = false;
		bool flag;
		if (mover.GetActorStatus() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BarrierManager.IsMovementBlockedOnCrossover(ActorData, BoardSquare, BoardSquare)).MethodHandle;
			}
			flag = mover.GetActorStatus().HasStatus(StatusType.Unstoppable, true);
		}
		else
		{
			flag = false;
		}
		if (!flag)
		{
			for (int i = 0; i < this.m_barriers.Count; i++)
			{
				Barrier barrier = this.m_barriers[i];
				if (!barrier.CanMoveThroughAfterCrossoverBy(mover) && barrier.CrossingBarrier(source.ToVector3(), dest.ToVector3()))
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
					return true;
				}
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
		return result;
	}

	public int GetMovementStateChangesFor(ActorData mover)
	{
		Team team = mover.GetTeam();
		if (!this.IsTeamSupported(team))
		{
			return -1;
		}
		return this.m_movementStates[team];
	}

	[Server]
	public void UpdateMovementStateForTeam(Team team)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BarrierManager.UpdateMovementStateForTeam(Team)).MethodHandle;
			}
			Debug.LogWarning("[Server] function 'System.Void BarrierManager::UpdateMovementStateForTeam(Team)' called on client");
			return;
		}
		if (this.IsTeamSupported(team))
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
			int syncIndexFromTeam = this.GetSyncIndexFromTeam(team);
			int num = this.m_movementStatesSync[syncIndexFromTeam];
			int value = num + 1;
			this.m_movementStates[team] = value;
			this.m_movementStatesSync[syncIndexFromTeam] = value;
			return;
		}
		throw new Exception("BarrierManager does not support this team");
	}

	public bool IsPositionTargetingBlocked(ActorData caster, BoardSquare dest)
	{
		if (caster == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BarrierManager.IsPositionTargetingBlocked(ActorData, BoardSquare)).MethodHandle;
			}
			return true;
		}
		BoardSquare currentBoardSquare = caster.GetCurrentBoardSquare();
		if (currentBoardSquare == null)
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
			return true;
		}
		bool result = false;
		for (int i = 0; i < this.m_barriers.Count; i++)
		{
			Barrier barrier = this.m_barriers[i];
			if (barrier.IsPositionTargetingBlockedFor(caster) && barrier.CrossingBarrier(currentBoardSquare.ToVector3(), dest.ToVector3()))
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
				result = true;
				return result;
			}
		}
		for (;;)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			return result;
		}
	}

	private void UpdateHasAbilityBlockingBarriers()
	{
		bool hasAbilityBlockingBarriers = false;
		for (int i = 0; i < this.m_barriers.Count; i++)
		{
			if (this.m_barriers[i] != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(BarrierManager.UpdateHasAbilityBlockingBarriers()).MethodHandle;
				}
				if (this.m_barriers[i].BlocksAbilities != BlockingRules.ForNobody)
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
					hasAbilityBlockingBarriers = true;
					break;
				}
			}
		}
		this.m_hasAbilityBlockingBarriers = hasAbilityBlockingBarriers;
	}

	public bool HasAbilityBlockingBarriers()
	{
		return this.m_hasAbilityBlockingBarriers;
	}

	public bool AreAbilitiesBlocked(ActorData caster, BoardSquare source, BoardSquare dest, List<NonActorTargetInfo> nonActorTargetInfo)
	{
		Vector3 startPos = source.ToVector3();
		Vector3 destPos = dest.ToVector3();
		return this.AreAbilitiesBlocked(caster, startPos, destPos, nonActorTargetInfo);
	}

	public bool AreAbilitiesBlocked(ActorData caster, Vector3 startPos, Vector3 destPos, List<NonActorTargetInfo> nonActorTargetInfo)
	{
		bool flag;
		if (nonActorTargetInfo != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BarrierManager.AreAbilitiesBlocked(ActorData, Vector3, Vector3, List<NonActorTargetInfo>)).MethodHandle;
			}
			flag = NetworkServer.active;
		}
		else
		{
			flag = false;
		}
		bool flag2 = flag;
		float num = 0f;
		Barrier barrier = null;
		bool flag3 = false;
		for (int i = 0; i < this.m_barriers.Count; i++)
		{
			Barrier barrier2 = this.m_barriers[i];
			if (!barrier2.CanBeShotThroughBy(caster))
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
				if (barrier2 != null)
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
					if (barrier2.CrossingBarrier(startPos, destPos))
					{
						if (!flag2)
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
							flag3 = true;
							break;
						}
						Vector3 intersectionPoint = barrier2.GetIntersectionPoint(startPos, destPos);
						intersectionPoint.y = startPos.y;
						float magnitude = (intersectionPoint - startPos).magnitude;
						if (!flag3)
						{
							goto IL_CF;
						}
						if (magnitude < num)
						{
							for (;;)
							{
								switch (3)
								{
								case 0:
									continue;
								}
								goto IL_CF;
							}
						}
						IL_D2:
						flag3 = true;
						goto IL_D4;
						IL_CF:
						num = magnitude;
						goto IL_D2;
					}
				}
			}
			IL_D4:;
		}
		if (barrier != null)
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
		}
		return flag3;
	}

	public unsafe Vector3 GetAbilityLineEndpoint(ActorData caster, Vector3 lineStart, Vector3 currentEnd, out bool collision, out Vector3 collisionNormal, List<NonActorTargetInfo> nonActorTargetInfo = null)
	{
		Vector3 vector = currentEnd;
		collisionNormal = Vector3.zero;
		collision = false;
		Barrier barrier = null;
		for (int i = 0; i < this.m_barriers.Count; i++)
		{
			Barrier barrier2 = this.m_barriers[i];
			if (!barrier2.CanBeShotThroughBy(caster))
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(BarrierManager.GetAbilityLineEndpoint(ActorData, Vector3, Vector3, bool*, Vector3*, List<NonActorTargetInfo>)).MethodHandle;
				}
				if (barrier2.CrossingBarrier(lineStart, vector))
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
					vector = barrier2.GetIntersectionPoint(lineStart, vector);
					collision = true;
					collisionNormal = barrier2.GetCollisionNormal(currentEnd - lineStart);
					barrier = barrier2;
				}
			}
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
		if (barrier != null)
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
		}
		return vector;
	}

	public unsafe void UpdateCachedCoverDirections(ActorData forActor, BoardSquare centerSquare, ref bool[] cachedBarrierDirs)
	{
		if (centerSquare != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BarrierManager.UpdateCachedCoverDirections(ActorData, BoardSquare, bool[]*)).MethodHandle;
			}
			if (forActor != null)
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
				Vector3 vector = centerSquare.ToVector3();
				for (int i = 0; i < this.m_barriers.Count; i++)
				{
					Barrier barrier = this.m_barriers[i];
					if (barrier.ConsiderAsCover)
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
						if (barrier.GetBarrierTeam() == forActor.GetTeam())
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
							for (int j = 0; j < cachedBarrierDirs.Length; j++)
							{
								if (!cachedBarrierDirs[j])
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
									Vector3 b = 1.5f * ActorCover.GetCoverOffsetStatic((ActorCover.CoverDirections)j);
									bool flag = barrier.CrossingBarrier(vector + b, vector);
									cachedBarrierDirs[j] = flag;
								}
							}
						}
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
		if (!NetworkServer.active)
		{
			bool flag = false;
			int i = 0;
			while (i < this.m_barriers.Count)
			{
				if (this.m_barriers[i].ConsiderAsCover)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(BarrierManager.RpcUpdateBarriers()).MethodHandle;
					}
					flag = true;
					IL_5C:
					this.m_barriers.Clear();
					if (this.m_barrierIdSync.Count > 0x32)
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
						Debug.LogError("More than 50 barriers active?");
					}
					for (int j = 0; j < this.m_barrierIdSync.Count; j++)
					{
						using (List<BarrierSerializeInfo>.Enumerator enumerator = this.m_clientBarrierInfo.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								BarrierSerializeInfo barrierSerializeInfo = enumerator.Current;
								if (barrierSerializeInfo.m_guid == this.m_barrierIdSync[j])
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
									Barrier barrier = Barrier.CreateBarrierFromSerializeInfo(barrierSerializeInfo);
									if (barrier.ConsiderAsCover)
									{
										flag = true;
									}
									List<ActorData> list;
									this.AddBarrier(barrier, false, out list);
									goto IL_115;
								}
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
						IL_115:;
					}
					this.ClientUpdateMovementAndVision();
					this.UpdateHasAbilityBlockingBarriers();
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
						GameFlowData.Get().UpdateCoverFromBarriersForAllActors();
						return;
					}
					return;
				}
				else
				{
					i++;
				}
			}
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				goto IL_5C;
			}
		}
	}

	private void SyncListCallbackMovementStates(SyncList<int>.Operation op, int _incorrectIndexBugIn51And52)
	{
		this.m_clientNeedMovementUpdate = true;
	}

	private void SyncListCallbackVisionStates(SyncList<int>.Operation op, int _incorrectIndexBugIn51And52)
	{
		this.ClientUpdateMovementAndVision();
	}

	[Client]
	private void ClientUpdateMovementAndVision()
	{
		if (!NetworkClient.active)
		{
			Debug.LogWarning("[Client] function 'System.Void BarrierManager::ClientUpdateMovementAndVision()' called on server");
			return;
		}
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BarrierManager.ClientUpdateMovementAndVision()).MethodHandle;
			}
			for (int i = 0; i < 3; i++)
			{
				Team teamFromSyncIndex = this.GetTeamFromSyncIndex(i);
				if (this.m_movementStatesSync[i] != this.m_movementStates[teamFromSyncIndex])
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
					this.m_clientNeedMovementUpdate = true;
				}
				if (this.m_visionStatesSync[i] != this.m_visionStates[teamFromSyncIndex])
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
					this.m_visionStates[teamFromSyncIndex] = this.m_visionStatesSync[i];
					this.ClientUpdateVisionOnSync(teamFromSyncIndex);
				}
			}
		}
	}

	[Client]
	private void ClientUpdateMovementOnSync(Team team)
	{
		if (!NetworkClient.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BarrierManager.ClientUpdateMovementOnSync(Team)).MethodHandle;
			}
			Debug.LogWarning("[Client] function 'System.Void BarrierManager::ClientUpdateMovementOnSync(Team)' called on server");
			return;
		}
		ActorData actorData = (!(GameFlowData.Get() == null)) ? GameFlowData.Get().activeOwnedActorData : null;
		if (actorData != null)
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
			if (actorData.GetTeam() == team)
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
				actorData.GetActorMovement().UpdateSquaresCanMoveTo();
			}
		}
	}

	[Client]
	private void ClientUpdateVisionOnSync(Team team)
	{
		if (!NetworkClient.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BarrierManager.ClientUpdateVisionOnSync(Team)).MethodHandle;
			}
			Debug.LogWarning("[Client] function 'System.Void BarrierManager::ClientUpdateVisionOnSync(Team)' called on server");
			return;
		}
		ActorData actorData;
		if (GameFlowData.Get() == null)
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
			actorData = null;
		}
		else
		{
			actorData = GameFlowData.Get().activeOwnedActorData;
		}
		ActorData actorData2 = actorData;
		if (actorData2 != null)
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
			if (actorData2.GetTeam() == team)
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
				actorData2.GetFogOfWar().MarkForRecalculateVisibility();
			}
		}
	}

	private void OnDrawGizmos()
	{
		if (!CameraManager.ShouldDrawGizmosForCurrentCamera())
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(BarrierManager.OnDrawGizmos()).MethodHandle;
			}
			return;
		}
		if (this.m_barriers != null)
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
			using (List<Barrier>.Enumerator enumerator = this.m_barriers.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Barrier barrier = enumerator.Current;
					barrier.DrawGizmos();
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BarrierManager.InvokeSyncListm_movementStatesSync(NetworkBehaviour, NetworkReader)).MethodHandle;
			}
			Debug.LogError("SyncList m_movementStatesSync called on server.");
			return;
		}
		((BarrierManager)obj).m_movementStatesSync.HandleMsg(reader);
	}

	protected static void InvokeSyncListm_visionStatesSync(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BarrierManager.InvokeSyncListm_visionStatesSync(NetworkBehaviour, NetworkReader)).MethodHandle;
			}
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
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(BarrierManager.CallRpcUpdateBarriers()).MethodHandle;
			}
			Debug.LogError("RPC Function RpcUpdateBarriers called on client.");
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write(0);
		networkWriter.Write((short)((ushort)2));
		networkWriter.WritePackedUInt32((uint)BarrierManager.kRpcRpcUpdateBarriers);
		networkWriter.Write(base.GetComponent<NetworkIdentity>().netId);
		this.SendRPCInternal(networkWriter, 0, "RpcUpdateBarriers");
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		if (forceAll)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BarrierManager.OnSerialize(NetworkWriter, bool)).MethodHandle;
			}
			SyncListInt.WriteInstance(writer, this.m_barrierIdSync);
			SyncListInt.WriteInstance(writer, this.m_movementStatesSync);
			SyncListInt.WriteInstance(writer, this.m_visionStatesSync);
			return true;
		}
		bool flag = false;
		if ((base.syncVarDirtyBits & 1U) != 0U)
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
			SyncListInt.WriteInstance(writer, this.m_barrierIdSync);
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
			SyncListInt.WriteInstance(writer, this.m_movementStatesSync);
		}
		if ((base.syncVarDirtyBits & 4U) != 0U)
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
			if (!flag)
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
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			SyncListInt.WriteInstance(writer, this.m_visionStatesSync);
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
		}
		return flag;
	}

	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
		if (initialState)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BarrierManager.OnDeserialize(NetworkReader, bool)).MethodHandle;
			}
			SyncListInt.ReadReference(reader, this.m_barrierIdSync);
			SyncListInt.ReadReference(reader, this.m_movementStatesSync);
			SyncListInt.ReadReference(reader, this.m_visionStatesSync);
			return;
		}
		int num = (int)reader.ReadPackedUInt32();
		if ((num & 1) != 0)
		{
			SyncListInt.ReadReference(reader, this.m_barrierIdSync);
		}
		if ((num & 2) != 0)
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
			SyncListInt.ReadReference(reader, this.m_movementStatesSync);
		}
		if ((num & 4) != 0)
		{
			SyncListInt.ReadReference(reader, this.m_visionStatesSync);
		}
	}
}

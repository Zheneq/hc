using System;
using System.Collections.Generic;
using Unity;
using UnityEngine;
using UnityEngine.Networking;

public class SparkBeamTrackerComponent : NetworkBehaviour
{
	private SyncListInt m_beamActorIndex = new SyncListInt();

	private SyncListSparkTetherAgeInfo m_actorIndexToTetherAge = new SyncListSparkTetherAgeInfo();

	public GameObject m_tetherRangePrefab;

	private GameObject m_tetherRange;

	private ActorData m_actorData;

	private SyncListUInt m_actorsOutOfRangeOnEvade = new SyncListUInt();

	private static int kListm_beamActorIndex;

	private static int kListm_actorIndexToTetherAge;

	private static int kListm_actorsOutOfRangeOnEvade;

	private static int kRpcRpcSetTetherRadiusPosition = -0x586DECD8;

	static SparkBeamTrackerComponent()
	{
		NetworkBehaviour.RegisterRpcDelegate(typeof(SparkBeamTrackerComponent), SparkBeamTrackerComponent.kRpcRpcSetTetherRadiusPosition, new NetworkBehaviour.CmdDelegate(SparkBeamTrackerComponent.InvokeRpcRpcSetTetherRadiusPosition));
		SparkBeamTrackerComponent.kListm_beamActorIndex = -0x7146C3AF;
		NetworkBehaviour.RegisterSyncListDelegate(typeof(SparkBeamTrackerComponent), SparkBeamTrackerComponent.kListm_beamActorIndex, new NetworkBehaviour.CmdDelegate(SparkBeamTrackerComponent.InvokeSyncListm_beamActorIndex));
		SparkBeamTrackerComponent.kListm_actorIndexToTetherAge = 0x3B0EA430;
		NetworkBehaviour.RegisterSyncListDelegate(typeof(SparkBeamTrackerComponent), SparkBeamTrackerComponent.kListm_actorIndexToTetherAge, new NetworkBehaviour.CmdDelegate(SparkBeamTrackerComponent.InvokeSyncListm_actorIndexToTetherAge));
		SparkBeamTrackerComponent.kListm_actorsOutOfRangeOnEvade = -0x5D87529;
		NetworkBehaviour.RegisterSyncListDelegate(typeof(SparkBeamTrackerComponent), SparkBeamTrackerComponent.kListm_actorsOutOfRangeOnEvade, new NetworkBehaviour.CmdDelegate(SparkBeamTrackerComponent.InvokeSyncListm_actorsOutOfRangeOnEvade));
		NetworkCRC.RegisterBehaviour("SparkBeamTrackerComponent", 0);
	}

	private void Start()
	{
		if (this.m_tetherRangePrefab != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SparkBeamTrackerComponent.Start()).MethodHandle;
			}
			this.m_tetherRange = UnityEngine.Object.Instantiate<GameObject>(this.m_tetherRangePrefab);
			this.m_tetherRange.transform.parent = base.gameObject.transform;
			this.m_tetherRange.transform.localPosition = Vector3.zero;
			this.m_tetherRange.SetActive(false);
		}
		this.m_actorData = base.GetComponent<ActorData>();
	}

	internal void SetTetherRadiusPosition(Vector3 tetherRadiusCenter)
	{
		if (!NetworkClient.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SparkBeamTrackerComponent.SetTetherRadiusPosition(Vector3)).MethodHandle;
			}
			this.CallRpcSetTetherRadiusPosition(tetherRadiusCenter);
		}
		else if (this.m_tetherRange != null)
		{
			this.m_tetherRange.transform.parent = null;
			this.m_tetherRange.transform.position = tetherRadiusCenter;
		}
	}

	[ClientRpc]
	private void RpcSetTetherRadiusPosition(Vector3 tetherRadiusCenter)
	{
		this.SetTetherRadiusPosition(tetherRadiusCenter);
	}

	internal bool IsTrackingActor(int index)
	{
		return this.m_beamActorIndex.Contains(index);
	}

	internal bool BeamIsActive()
	{
		return this.m_beamActorIndex.Count > 0;
	}

	internal int GetNumTethers()
	{
		return this.m_beamActorIndex.Count;
	}

	internal int GetTetherAgeOnActor(int actorIndex)
	{
		for (int i = 0; i < (int)this.m_actorIndexToTetherAge.Count; i++)
		{
			if (this.m_actorIndexToTetherAge[i].m_actorIndex == actorIndex)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(SparkBeamTrackerComponent.GetTetherAgeOnActor(int)).MethodHandle;
				}
				return this.m_actorIndexToTetherAge[i].m_tetherAge;
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
		return 0;
	}

	private bool ShouldShowTetherRadius()
	{
		bool result = false;
		if (NetworkClient.active)
		{
			ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
			if (activeOwnedActorData != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(SparkBeamTrackerComponent.ShouldShowTetherRadius()).MethodHandle;
				}
				bool flag = this.m_actorData == activeOwnedActorData;
				if (flag)
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
					if (this.BeamIsActive())
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
						result = true;
					}
					else
					{
						ActorTurnSM actorTurnSM = this.m_actorData.GetActorTurnSM();
						if (actorTurnSM.CurrentState == TurnStateEnum.TARGETING_ACTION)
						{
							AbilityData abilityData = this.m_actorData.GetAbilityData();
							Ability ability;
							if (abilityData)
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
								ability = abilityData.GetLastSelectedAbility();
							}
							else
							{
								ability = null;
							}
							Ability ability2 = ability;
							if (ability2 != null)
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
								if (!(ability2 is SparkBasicAttack))
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
									if (!(ability2 is SparkHealingBeam))
									{
										goto IL_FB;
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
								result = true;
							}
						}
					}
					IL_FB:;
				}
				else
				{
					result = (this.BeamIsActive() && this.m_beamActorIndex.Contains(activeOwnedActorData.ActorIndex));
				}
			}
		}
		return result;
	}

	internal bool IsActorTracked(ActorData actor)
	{
		bool result;
		if (actor != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SparkBeamTrackerComponent.IsActorTracked(ActorData)).MethodHandle;
			}
			result = this.m_beamActorIndex.Contains(actor.ActorIndex);
		}
		else
		{
			result = false;
		}
		return result;
	}

	internal bool IsActorIndexTracked(int actorIndex)
	{
		return this.m_beamActorIndex.Contains(actorIndex);
	}

	internal List<int> GetBeamActorIndices()
	{
		List<int> list = new List<int>();
		IEnumerator<int> enumerator = this.m_beamActorIndex.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				int item = enumerator.Current;
				list.Add(item);
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SparkBeamTrackerComponent.GetBeamActorIndices()).MethodHandle;
			}
		}
		finally
		{
			if (enumerator != null)
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
				enumerator.Dispose();
			}
		}
		return list;
	}

	internal List<ActorData> GetBeamActors()
	{
		List<ActorData> list = new List<ActorData>();
		foreach (int actorIndex in this.GetBeamActorIndices())
		{
			ActorData actorData = GameFlowData.Get().FindActorByActorIndex(actorIndex);
			if (actorData != null)
			{
				list.Add(actorData);
			}
		}
		return list;
	}

	internal bool HasBothTethers()
	{
		bool flag = false;
		bool flag2 = false;
		foreach (int actorIndex in this.GetBeamActorIndices())
		{
			ActorData actorData = GameFlowData.Get().FindActorByActorIndex(actorIndex);
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
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(SparkBeamTrackerComponent.HasBothTethers()).MethodHandle;
				}
				if (actorData.GetTeam() == this.m_actorData.GetTeam())
				{
					flag = true;
				}
				else
				{
					flag2 = true;
				}
			}
		}
		bool result;
		if (flag)
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
			result = flag2;
		}
		else
		{
			result = false;
		}
		return result;
	}

	private void Update()
	{
		if (this.m_tetherRange != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SparkBeamTrackerComponent.Update()).MethodHandle;
			}
			bool flag = this.ShouldShowTetherRadius();
			FriendlyEnemyVFXSelector component = this.m_tetherRange.GetComponent<FriendlyEnemyVFXSelector>();
			if (component != null)
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
				if (GameFlowData.Get().activeOwnedActorData != null)
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
					component.Setup(GameFlowData.Get().activeOwnedActorData.GetTeam());
				}
			}
			if (flag)
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
				if (!this.m_tetherRange.activeSelf)
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
					this.m_tetherRange.SetActive(true);
					return;
				}
			}
			if (!flag)
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
				if (this.m_tetherRange.activeSelf)
				{
					this.m_tetherRange.SetActive(false);
				}
			}
		}
	}

	internal bool IsActorOutOfRangeForEvade(ActorData actor)
	{
		if (actor != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SparkBeamTrackerComponent.IsActorOutOfRangeForEvade(ActorData)).MethodHandle;
			}
			return this.m_actorsOutOfRangeOnEvade.Contains((uint)actor.ActorIndex);
		}
		return false;
	}

	private void UNetVersion()
	{
	}

	protected static void InvokeSyncListm_beamActorIndex(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SparkBeamTrackerComponent.InvokeSyncListm_beamActorIndex(NetworkBehaviour, NetworkReader)).MethodHandle;
			}
			Debug.LogError("SyncList m_beamActorIndex called on server.");
			return;
		}
		((SparkBeamTrackerComponent)obj).m_beamActorIndex.HandleMsg(reader);
	}

	protected static void InvokeSyncListm_actorIndexToTetherAge(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("SyncList m_actorIndexToTetherAge called on server.");
			return;
		}
		((SparkBeamTrackerComponent)obj).m_actorIndexToTetherAge.HandleMsg(reader);
	}

	protected static void InvokeSyncListm_actorsOutOfRangeOnEvade(NetworkBehaviour obj, NetworkReader reader)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SparkBeamTrackerComponent.InvokeSyncListm_actorsOutOfRangeOnEvade(NetworkBehaviour, NetworkReader)).MethodHandle;
			}
			Debug.LogError("SyncList m_actorsOutOfRangeOnEvade called on server.");
			return;
		}
		((SparkBeamTrackerComponent)obj).m_actorsOutOfRangeOnEvade.HandleMsg(reader);
	}

	protected static void InvokeRpcRpcSetTetherRadiusPosition(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SparkBeamTrackerComponent.InvokeRpcRpcSetTetherRadiusPosition(NetworkBehaviour, NetworkReader)).MethodHandle;
			}
			Debug.LogError("RPC RpcSetTetherRadiusPosition called on server.");
			return;
		}
		((SparkBeamTrackerComponent)obj).RpcSetTetherRadiusPosition(reader.ReadVector3());
	}

	public void CallRpcSetTetherRadiusPosition(Vector3 tetherRadiusCenter)
	{
		if (!NetworkServer.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SparkBeamTrackerComponent.CallRpcSetTetherRadiusPosition(Vector3)).MethodHandle;
			}
			Debug.LogError("RPC Function RpcSetTetherRadiusPosition called on client.");
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write(0);
		networkWriter.Write((short)((ushort)2));
		networkWriter.WritePackedUInt32((uint)SparkBeamTrackerComponent.kRpcRpcSetTetherRadiusPosition);
		networkWriter.Write(base.GetComponent<NetworkIdentity>().netId);
		networkWriter.Write(tetherRadiusCenter);
		this.SendRPCInternal(networkWriter, 0, "RpcSetTetherRadiusPosition");
	}

	private void Awake()
	{
		this.m_beamActorIndex.InitializeBehaviour(this, SparkBeamTrackerComponent.kListm_beamActorIndex);
		this.m_actorIndexToTetherAge.InitializeBehaviour(this, SparkBeamTrackerComponent.kListm_actorIndexToTetherAge);
		this.m_actorsOutOfRangeOnEvade.InitializeBehaviour(this, SparkBeamTrackerComponent.kListm_actorsOutOfRangeOnEvade);
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		if (forceAll)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SparkBeamTrackerComponent.OnSerialize(NetworkWriter, bool)).MethodHandle;
			}
			SyncListInt.WriteInstance(writer, this.m_beamActorIndex);
			GeneratedNetworkCode._WriteStructSyncListSparkTetherAgeInfo_None(writer, this.m_actorIndexToTetherAge);
			SyncListUInt.WriteInstance(writer, this.m_actorsOutOfRangeOnEvade);
			return true;
		}
		bool flag = false;
		if ((base.syncVarDirtyBits & 1U) != 0U)
		{
			if (!flag)
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
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			SyncListInt.WriteInstance(writer, this.m_beamActorIndex);
		}
		if ((base.syncVarDirtyBits & 2U) != 0U)
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
			if (!flag)
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
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			GeneratedNetworkCode._WriteStructSyncListSparkTetherAgeInfo_None(writer, this.m_actorIndexToTetherAge);
		}
		if ((base.syncVarDirtyBits & 4U) != 0U)
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
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			SyncListUInt.WriteInstance(writer, this.m_actorsOutOfRangeOnEvade);
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SparkBeamTrackerComponent.OnDeserialize(NetworkReader, bool)).MethodHandle;
			}
			SyncListInt.ReadReference(reader, this.m_beamActorIndex);
			GeneratedNetworkCode._ReadStructSyncListSparkTetherAgeInfo_None(reader, this.m_actorIndexToTetherAge);
			SyncListUInt.ReadReference(reader, this.m_actorsOutOfRangeOnEvade);
			return;
		}
		int num = (int)reader.ReadPackedUInt32();
		if ((num & 1) != 0)
		{
			SyncListInt.ReadReference(reader, this.m_beamActorIndex);
		}
		if ((num & 2) != 0)
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
			GeneratedNetworkCode._ReadStructSyncListSparkTetherAgeInfo_None(reader, this.m_actorIndexToTetherAge);
		}
		if ((num & 4) != 0)
		{
			SyncListUInt.ReadReference(reader, this.m_actorsOutOfRangeOnEvade);
		}
	}

	public struct ActorIndexToTetherAge
	{
		public int m_actorIndex;

		public int m_tetherAge;
	}
}

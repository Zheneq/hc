// ROGUES
// SERVER
using System.Collections.Generic;
using Unity;
using UnityEngine;
using UnityEngine.Networking;

public class SparkBeamTrackerComponent : NetworkBehaviour
{
	public struct ActorIndexToTetherAge
	{
		public int m_actorIndex;
		public int m_tetherAge;
	}

	private SyncListInt m_beamActorIndex = new SyncListInt();
	private SyncListSparkTetherAgeInfo m_actorIndexToTetherAge = new SyncListSparkTetherAgeInfo();

	public GameObject m_tetherRangePrefab;

	private GameObject m_tetherRange;
	private ActorData m_actorData;
	private SyncListUInt m_actorsOutOfRangeOnEvade = new SyncListUInt();

	// removed in rogues
	private static int kListm_beamActorIndex = -1900463023;
	private static int kListm_actorIndexToTetherAge = 990815280;
	private static int kListm_actorsOutOfRangeOnEvade = -98071849;
	private static int kRpcRpcSetTetherRadiusPosition = -1483599064;

	// removed in rogues
	static SparkBeamTrackerComponent()
	{		
		RegisterRpcDelegate(typeof(SparkBeamTrackerComponent), kRpcRpcSetTetherRadiusPosition, InvokeRpcRpcSetTetherRadiusPosition);		
		RegisterSyncListDelegate(typeof(SparkBeamTrackerComponent), kListm_beamActorIndex, InvokeSyncListm_beamActorIndex);		
		RegisterSyncListDelegate(typeof(SparkBeamTrackerComponent), kListm_actorIndexToTetherAge, InvokeSyncListm_actorIndexToTetherAge);		
		RegisterSyncListDelegate(typeof(SparkBeamTrackerComponent), kListm_actorsOutOfRangeOnEvade, InvokeSyncListm_actorsOutOfRangeOnEvade);
		NetworkCRC.RegisterBehaviour("SparkBeamTrackerComponent", 0);
	}

	private void Start()
	{
		if (m_tetherRangePrefab != null)
		{
			m_tetherRange = Instantiate(m_tetherRangePrefab);
			m_tetherRange.transform.parent = gameObject.transform;
			m_tetherRange.transform.localPosition = Vector3.zero;
			m_tetherRange.SetActive(false);
		}
		m_actorData = GetComponent<ActorData>();
	}

	internal void SetTetherRadiusPosition(Vector3 tetherRadiusCenter)
	{
		if (NetworkClient.active)
		{
			if (m_tetherRange != null)
			{
				m_tetherRange.transform.parent = null;
				m_tetherRange.transform.position = tetherRadiusCenter;
			}
		}
		else
		{
			CallRpcSetTetherRadiusPosition(tetherRadiusCenter);
		}
	}

	[ClientRpc]
	private void RpcSetTetherRadiusPosition(Vector3 tetherRadiusCenter)
	{
		SetTetherRadiusPosition(tetherRadiusCenter);
	}

	internal bool IsTrackingActor(int index)
	{
		return m_beamActorIndex.Contains(index);
	}

	internal bool BeamIsActive()
	{
		return m_beamActorIndex.Count > 0;
	}

	internal int GetNumTethers()
	{
		return m_beamActorIndex.Count;
	}

	internal int GetTetherAgeOnActor(int actorIndex)
	{
		foreach (ActorIndexToTetherAge actorIndexToTetherAge in m_actorIndexToTetherAge)
		{
			if (actorIndexToTetherAge.m_actorIndex == actorIndex)
			{
				return actorIndexToTetherAge.m_tetherAge;
			}
		}
		return 0;
	}

	private bool ShouldShowTetherRadius()
	{
		if (!NetworkClient.active)
		{
			return false;
		}
		ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
		if (activeOwnedActorData == null)
		{
			return false;
		}
		if (m_actorData != activeOwnedActorData)
		{
			return (BeamIsActive() && m_beamActorIndex.Contains(activeOwnedActorData.ActorIndex));
		}
		if (BeamIsActive())
		{
			return true;
		}
		if (m_actorData.GetActorTurnSM().CurrentState == TurnStateEnum.TARGETING_ACTION)
		{
			Ability ability = m_actorData.GetAbilityData()?.GetLastSelectedAbility();
			if (ability != null)
			{
				return ability is SparkBasicAttack || ability is SparkHealingBeam;
			}
		}
		return false;
	}

	internal bool IsActorTracked(ActorData actor)
	{
		return actor != null && m_beamActorIndex.Contains(actor.ActorIndex);
	}

	internal bool IsActorIndexTracked(int actorIndex)
	{
		return m_beamActorIndex.Contains(actorIndex);
	}

	internal List<int> GetBeamActorIndices()
	{
		List<int> list = new List<int>();
		foreach (int beamActorIndex in m_beamActorIndex)
		{
			list.Add(beamActorIndex);
		}
		return list;
	}

	internal List<ActorData> GetBeamActors()
	{
		List<ActorData> list = new List<ActorData>();
		foreach (int beamActorIndex in GetBeamActorIndices())
		{
			ActorData actorData = GameFlowData.Get().FindActorByActorIndex(beamActorIndex);
			if (actorData != null)
			{
				list.Add(actorData);
			}
		}
		return list;
	}

	// removed in rogues
	internal bool HasBothTethers()
	{
		bool hasAlly = false;
		bool hasEnemy = false;
		foreach (int beamActorIndex in GetBeamActorIndices())
		{
			ActorData actorData = GameFlowData.Get().FindActorByActorIndex(beamActorIndex);
			if (actorData != null)
			{
				if (actorData.GetTeam() == m_actorData.GetTeam())
				{
					hasAlly = true;
				}
				else
				{
					hasEnemy = true;
				}
			}
		}
		return hasAlly && hasEnemy;
	}

	private void Update()
	{
		if (m_tetherRange != null)
		{
			bool isShowingTetherRadius = ShouldShowTetherRadius();
			FriendlyEnemyVFXSelector component = m_tetherRange.GetComponent<FriendlyEnemyVFXSelector>();
			if (component != null && GameFlowData.Get().activeOwnedActorData != null)
			{
				component.Setup(GameFlowData.Get().activeOwnedActorData.GetTeam());
			}
			if (isShowingTetherRadius && !m_tetherRange.activeSelf)
			{
				m_tetherRange.SetActive(true);
			}
			else if (!isShowingTetherRadius && m_tetherRange.activeSelf)
			{
				m_tetherRange.SetActive(false);
			}
		}
	}

	internal bool IsActorOutOfRangeForEvade(ActorData actor)
	{
		if (actor != null)
		{
			return m_actorsOutOfRangeOnEvade.Contains((uint)actor.ActorIndex);
		}
		return false;
	}

#if SERVER
	[Server]
	internal void RemoveAllActorBeams()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void SparkBeamTrackerComponent::RemoveAllActorBeams()' called on client");
			return;
		}
		m_beamActorIndex.Clear();
	}
#endif

#if SERVER
	[Server]
	internal void AddBeamActorByIndex(int actorIndex)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void SparkBeamTrackerComponent::AddBeamActorByIndex(System.Int32)' called on client");
			return;
		}
		if (!m_beamActorIndex.Contains(actorIndex))
		{
			m_beamActorIndex.Add(actorIndex);
			for (int i = m_actorIndexToTetherAge.Count - 1; i >= 0; i--)
			{
				if (m_actorIndexToTetherAge[i].m_actorIndex == actorIndex)
				{
					m_actorIndexToTetherAge.RemoveAt(i);
					Debug.LogError("Spark sync component, has unexpected actorIndex-to-tetherAge entry when trying to add new one");
				}
			}
			SparkBeamTrackerComponent.ActorIndexToTetherAge actorIndexToTetherAge;
			actorIndexToTetherAge.m_actorIndex = actorIndex;
			actorIndexToTetherAge.m_tetherAge = 0;
			m_actorIndexToTetherAge.Add(actorIndexToTetherAge);
		}
	}
#endif

#if SERVER
	[Server]
	internal void RemoveBeamActorByIndex(int actorIndex)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void SparkBeamTrackerComponent::RemoveBeamActorByIndex(System.Int32)' called on client");
			return;
		}
		if (m_beamActorIndex.Contains(actorIndex))
		{
			m_beamActorIndex.Remove(actorIndex);
			for (int i = m_actorIndexToTetherAge.Count - 1; i >= 0; i--)
			{
				if (m_actorIndexToTetherAge[i].m_actorIndex == actorIndex)
				{
					m_actorIndexToTetherAge.RemoveAt(i);
				}
			}
		}
	}
#endif

#if SERVER
	[Server]
	internal void UpdateTetherDuration(int actorIndex, int ageNow)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void SparkBeamTrackerComponent::UpdateTetherDuration(System.Int32,System.Int32)' called on client");
			return;
		}
		if (m_beamActorIndex.Contains(actorIndex))
		{
			for (int i = 0; i < m_actorIndexToTetherAge.Count; i++)
			{
				if (m_actorIndexToTetherAge[i].m_actorIndex == actorIndex)
				{
					SparkBeamTrackerComponent.ActorIndexToTetherAge actorIndexToTetherAge;
					actorIndexToTetherAge.m_actorIndex = actorIndex;
					actorIndexToTetherAge.m_tetherAge = ageNow;
					m_actorIndexToTetherAge[i] = actorIndexToTetherAge;
					return;
				}
			}
		}
	}
#endif

#if SERVER
	internal void ClearActorsOutOfRangeOnEvade()
	{
		m_actorsOutOfRangeOnEvade.Clear();
	}
#endif

#if SERVER
	internal void AddActorAsOutOfRangeOnEvade(ActorData actor)
	{
		if (actor != null)
		{
			m_actorsOutOfRangeOnEvade.Add((uint)actor.ActorIndex);
		}
	}
#endif

	////rogues
	//public SparkBeamTrackerComponent()
	//{
	//    base.InitSyncObject(m_beamActorIndex);
	//    base.InitSyncObject(m_actorIndexToTetherAge);
	//    base.InitSyncObject(m_actorsOutOfRangeOnEvade);
	//}


	////rogues
	//private void MirrorProcessed()
	//{
	//}

	// removed in rogues
	private void UNetVersion()
	{
	}

	// removed in rogues
	protected static void InvokeSyncListm_beamActorIndex(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("SyncList m_beamActorIndex called on server.");
			return;
		}
		((SparkBeamTrackerComponent)obj).m_beamActorIndex.HandleMsg(reader);
	}

	// removed in rogues
	protected static void InvokeSyncListm_actorIndexToTetherAge(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("SyncList m_actorIndexToTetherAge called on server.");
			return;
		}
		((SparkBeamTrackerComponent)obj).m_actorIndexToTetherAge.HandleMsg(reader);
	}

	// removed in rogues
	protected static void InvokeSyncListm_actorsOutOfRangeOnEvade(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("SyncList m_actorsOutOfRangeOnEvade called on server.");
			return;
		}
		((SparkBeamTrackerComponent)obj).m_actorsOutOfRangeOnEvade.HandleMsg(reader);
	}

	// removed in rogues
	protected static void InvokeRpcRpcSetTetherRadiusPosition(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcSetTetherRadiusPosition called on server.");
			return;
		}
		((SparkBeamTrackerComponent)obj).RpcSetTetherRadiusPosition(reader.ReadVector3());
	}

	// reactor
	public void CallRpcSetTetherRadiusPosition(Vector3 tetherRadiusCenter)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("RPC Function RpcSetTetherRadiusPosition called on client.");
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)2);
		networkWriter.WritePackedUInt32((uint)kRpcRpcSetTetherRadiusPosition);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		networkWriter.Write(tetherRadiusCenter);
		SendRPCInternal(networkWriter, 0, "RpcSetTetherRadiusPosition");
	}

	// rogues
	//public void CallRpcSetTetherRadiusPosition(Vector3 tetherRadiusCenter)
	//{
	//	NetworkWriter networkWriter = new NetworkWriter();
	//	networkWriter.Write(tetherRadiusCenter);
	//	this.SendRPCInternal(typeof(SparkBeamTrackerComponent), "RpcSetTetherRadiusPosition", networkWriter, 0);
	//}

	// removed in rogues
	private void Awake()
	{
		m_beamActorIndex.InitializeBehaviour(this, kListm_beamActorIndex);
		m_actorIndexToTetherAge.InitializeBehaviour(this, kListm_actorIndexToTetherAge);
		m_actorsOutOfRangeOnEvade.InitializeBehaviour(this, kListm_actorsOutOfRangeOnEvade);
	}

	// removed in rogues
	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		if (forceAll)
		{
			SyncListInt.WriteInstance(writer, m_beamActorIndex);
			GeneratedNetworkCode._WriteStructSyncListSparkTetherAgeInfo_None(writer, m_actorIndexToTetherAge);
			SyncListUInt.WriteInstance(writer, m_actorsOutOfRangeOnEvade);
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
			SyncListInt.WriteInstance(writer, m_beamActorIndex);
		}
		if ((syncVarDirtyBits & 2) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			GeneratedNetworkCode._WriteStructSyncListSparkTetherAgeInfo_None(writer, m_actorIndexToTetherAge);
		}
		if ((syncVarDirtyBits & 4) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			SyncListUInt.WriteInstance(writer, m_actorsOutOfRangeOnEvade);
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
			SyncListInt.ReadReference(reader, m_beamActorIndex);
			GeneratedNetworkCode._ReadStructSyncListSparkTetherAgeInfo_None(reader, m_actorIndexToTetherAge);
			SyncListUInt.ReadReference(reader, m_actorsOutOfRangeOnEvade);
			return;
		}
		int num = (int)reader.ReadPackedUInt32();
		if ((num & 1) != 0)
		{
			SyncListInt.ReadReference(reader, m_beamActorIndex);
		}
		if ((num & 2) != 0)
		{
			GeneratedNetworkCode._ReadStructSyncListSparkTetherAgeInfo_None(reader, m_actorIndexToTetherAge);
		}
		if ((num & 4) != 0)
		{
			SyncListUInt.ReadReference(reader, m_actorsOutOfRangeOnEvade);
		}
	}

	// rogues
	//	static SparkBeamTrackerComponent()
	//	{
	//		NetworkBehaviour.RegisterRpcDelegate(typeof(SparkBeamTrackerComponent), "RpcSetTetherRadiusPosition", new NetworkBehaviour.CmdDelegate(InvokeRpcRpcSetTetherRadiusPosition));
	//	}
}

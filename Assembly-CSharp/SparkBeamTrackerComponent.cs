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

	private static int kListm_beamActorIndex;

	private static int kListm_actorIndexToTetherAge;

	private static int kListm_actorsOutOfRangeOnEvade;

	private static int kRpcRpcSetTetherRadiusPosition;

	static SparkBeamTrackerComponent()
	{
		kRpcRpcSetTetherRadiusPosition = -1483599064;
		NetworkBehaviour.RegisterRpcDelegate(typeof(SparkBeamTrackerComponent), kRpcRpcSetTetherRadiusPosition, InvokeRpcRpcSetTetherRadiusPosition);
		kListm_beamActorIndex = -1900463023;
		NetworkBehaviour.RegisterSyncListDelegate(typeof(SparkBeamTrackerComponent), kListm_beamActorIndex, InvokeSyncListm_beamActorIndex);
		kListm_actorIndexToTetherAge = 990815280;
		NetworkBehaviour.RegisterSyncListDelegate(typeof(SparkBeamTrackerComponent), kListm_actorIndexToTetherAge, InvokeSyncListm_actorIndexToTetherAge);
		kListm_actorsOutOfRangeOnEvade = -98071849;
		NetworkBehaviour.RegisterSyncListDelegate(typeof(SparkBeamTrackerComponent), kListm_actorsOutOfRangeOnEvade, InvokeSyncListm_actorsOutOfRangeOnEvade);
		NetworkCRC.RegisterBehaviour("SparkBeamTrackerComponent", 0);
	}

	private void Start()
	{
		if (m_tetherRangePrefab != null)
		{
			m_tetherRange = Object.Instantiate(m_tetherRangePrefab);
			m_tetherRange.transform.parent = base.gameObject.transform;
			m_tetherRange.transform.localPosition = Vector3.zero;
			m_tetherRange.SetActive(false);
		}
		m_actorData = GetComponent<ActorData>();
	}

	internal void SetTetherRadiusPosition(Vector3 tetherRadiusCenter)
	{
		if (!NetworkClient.active)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					CallRpcSetTetherRadiusPosition(tetherRadiusCenter);
					return;
				}
			}
		}
		if (m_tetherRange != null)
		{
			m_tetherRange.transform.parent = null;
			m_tetherRange.transform.position = tetherRadiusCenter;
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
		for (int i = 0; i < m_actorIndexToTetherAge.Count; i++)
		{
			ActorIndexToTetherAge actorIndexToTetherAge = m_actorIndexToTetherAge[i];
			if (actorIndexToTetherAge.m_actorIndex != actorIndex)
			{
				continue;
			}
			while (true)
			{
				ActorIndexToTetherAge actorIndexToTetherAge2 = m_actorIndexToTetherAge[i];
				return actorIndexToTetherAge2.m_tetherAge;
			}
		}
		while (true)
		{
			return 0;
		}
	}

	private bool ShouldShowTetherRadius()
	{
		bool result = false;
		if (NetworkClient.active)
		{
			ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
			if (activeOwnedActorData != null)
			{
				if (m_actorData == activeOwnedActorData)
				{
					if (BeamIsActive())
					{
						result = true;
					}
					else
					{
						ActorTurnSM actorTurnSM = m_actorData.GetActorTurnSM();
						if (actorTurnSM.CurrentState == TurnStateEnum.TARGETING_ACTION)
						{
							AbilityData abilityData = m_actorData.GetAbilityData();
							object obj;
							if ((bool)abilityData)
							{
								obj = abilityData.GetLastSelectedAbility();
							}
							else
							{
								obj = null;
							}
							Ability ability = (Ability)obj;
							if (ability != null)
							{
								if (!(ability is SparkBasicAttack))
								{
									if (!(ability is SparkHealingBeam))
									{
										goto IL_0120;
									}
								}
								result = true;
							}
						}
					}
				}
				else
				{
					result = (BeamIsActive() && m_beamActorIndex.Contains(activeOwnedActorData.ActorIndex));
				}
			}
		}
		goto IL_0120;
		IL_0120:
		return result;
	}

	internal bool IsActorTracked(ActorData actor)
	{
		int result;
		if (actor != null)
		{
			result = (m_beamActorIndex.Contains(actor.ActorIndex) ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	internal bool IsActorIndexTracked(int actorIndex)
	{
		return m_beamActorIndex.Contains(actorIndex);
	}

	internal List<int> GetBeamActorIndices()
	{
		List<int> list = new List<int>();
		IEnumerator<int> enumerator = m_beamActorIndex.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				int current = enumerator.Current;
				list.Add(current);
			}
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					if (true)
					{
						return list;
					}
					/*OpCode not supported: LdMemberToken*/;
					return list;
				}
			}
		}
		finally
		{
			if (enumerator != null)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						enumerator.Dispose();
						goto end_IL_0045;
					}
				}
			}
			end_IL_0045:;
		}
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

	internal bool HasBothTethers()
	{
		bool flag = false;
		bool flag2 = false;
		foreach (int beamActorIndex in GetBeamActorIndices())
		{
			ActorData actorData = GameFlowData.Get().FindActorByActorIndex(beamActorIndex);
			if (actorData != null)
			{
				if (actorData.GetTeam() == m_actorData.GetTeam())
				{
					flag = true;
				}
				else
				{
					flag2 = true;
				}
			}
		}
		int result;
		if (flag)
		{
			result = (flag2 ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	private void Update()
	{
		if (!(m_tetherRange != null))
		{
			return;
		}
		while (true)
		{
			bool flag = ShouldShowTetherRadius();
			FriendlyEnemyVFXSelector component = m_tetherRange.GetComponent<FriendlyEnemyVFXSelector>();
			if (component != null)
			{
				if (GameFlowData.Get().activeOwnedActorData != null)
				{
					component.Setup(GameFlowData.Get().activeOwnedActorData.GetTeam());
				}
			}
			if (flag)
			{
				if (!m_tetherRange.activeSelf)
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							break;
						default:
							m_tetherRange.SetActive(true);
							return;
						}
					}
				}
			}
			if (flag)
			{
				return;
			}
			while (true)
			{
				if (m_tetherRange.activeSelf)
				{
					m_tetherRange.SetActive(false);
				}
				return;
			}
		}
	}

	internal bool IsActorOutOfRangeForEvade(ActorData actor)
	{
		if (actor != null)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return m_actorsOutOfRangeOnEvade.Contains((uint)actor.ActorIndex);
				}
			}
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
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					Debug.LogError("SyncList m_beamActorIndex called on server.");
					return;
				}
			}
		}
		((SparkBeamTrackerComponent)obj).m_beamActorIndex.HandleMsg(reader);
	}

	protected static void InvokeSyncListm_actorIndexToTetherAge(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("SyncList m_actorIndexToTetherAge called on server.");
		}
		else
		{
			((SparkBeamTrackerComponent)obj).m_actorIndexToTetherAge.HandleMsg(reader);
		}
	}

	protected static void InvokeSyncListm_actorsOutOfRangeOnEvade(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					Debug.LogError("SyncList m_actorsOutOfRangeOnEvade called on server.");
					return;
				}
			}
		}
		((SparkBeamTrackerComponent)obj).m_actorsOutOfRangeOnEvade.HandleMsg(reader);
	}

	protected static void InvokeRpcRpcSetTetherRadiusPosition(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					Debug.LogError("RPC RpcSetTetherRadiusPosition called on server.");
					return;
				}
			}
		}
		((SparkBeamTrackerComponent)obj).RpcSetTetherRadiusPosition(reader.ReadVector3());
	}

	public void CallRpcSetTetherRadiusPosition(Vector3 tetherRadiusCenter)
	{
		if (!NetworkServer.active)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					Debug.LogError("RPC Function RpcSetTetherRadiusPosition called on client.");
					return;
				}
			}
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)2);
		networkWriter.WritePackedUInt32((uint)kRpcRpcSetTetherRadiusPosition);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		networkWriter.Write(tetherRadiusCenter);
		SendRPCInternal(networkWriter, 0, "RpcSetTetherRadiusPosition");
	}

	private void Awake()
	{
		m_beamActorIndex.InitializeBehaviour(this, kListm_beamActorIndex);
		m_actorIndexToTetherAge.InitializeBehaviour(this, kListm_actorIndexToTetherAge);
		m_actorsOutOfRangeOnEvade.InitializeBehaviour(this, kListm_actorsOutOfRangeOnEvade);
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		if (forceAll)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					SyncListInt.WriteInstance(writer, m_beamActorIndex);
					GeneratedNetworkCode._WriteStructSyncListSparkTetherAgeInfo_None(writer, m_actorIndexToTetherAge);
					SyncListUInt.WriteInstance(writer, m_actorsOutOfRangeOnEvade);
					return true;
				}
			}
		}
		bool flag = false;
		if ((base.syncVarDirtyBits & 1) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			SyncListInt.WriteInstance(writer, m_beamActorIndex);
		}
		if ((base.syncVarDirtyBits & 2) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			GeneratedNetworkCode._WriteStructSyncListSparkTetherAgeInfo_None(writer, m_actorIndexToTetherAge);
		}
		if ((base.syncVarDirtyBits & 4) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			SyncListUInt.WriteInstance(writer, m_actorsOutOfRangeOnEvade);
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
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					SyncListInt.ReadReference(reader, m_beamActorIndex);
					GeneratedNetworkCode._ReadStructSyncListSparkTetherAgeInfo_None(reader, m_actorIndexToTetherAge);
					SyncListUInt.ReadReference(reader, m_actorsOutOfRangeOnEvade);
					return;
				}
			}
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
}

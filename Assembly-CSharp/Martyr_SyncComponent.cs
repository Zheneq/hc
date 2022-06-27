// ROGUES
// SERVER
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Networking;

public class Martyr_SyncComponent : NetworkBehaviour
{
	private SyncListUInt m_syncAoeOnReactActors = new SyncListUInt();

	[SyncVar]
	public bool CrystalsSpentThisTurn;
	[SyncVar(hook = "HookSetDamageCrystal")]
	public int DamageCrystals;
	[SyncVar]
	public int m_syncNumTurnsAtFullEnergy;

	internal int m_clientCrystalAdjustment;
	internal int m_clientDamageThisTurn;

	// removed in rogues
	private static int kListm_syncAoeOnReactActors = 1750195272;

	public bool NetworkCrystalsSpentThisTurn
	{
		get
		{
			return CrystalsSpentThisTurn;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref CrystalsSpentThisTurn, 2u);  // 1UL in rogues
		}
	}

	public int NetworkDamageCrystals
	{
		get
		{
			return DamageCrystals;
		}
		[param: In]
		set
		{
			if (NetworkServer.localClientActive)
			{
				if (!syncVarHookGuard)
				{
					syncVarHookGuard = true;
					HookSetDamageCrystal(value);
					syncVarHookGuard = false;
				}
			}
			SetSyncVar(value, ref DamageCrystals, 4u);  // 2UL in rogues
		}
	}

	public int Networkm_syncNumTurnsAtFullEnergy
	{
		get
		{
			return m_syncNumTurnsAtFullEnergy;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_syncNumTurnsAtFullEnergy, 8u);  // 4UL in rogues
		}
	}

	// removed in rogues
	static Martyr_SyncComponent()
	{
		RegisterSyncListDelegate(typeof(Martyr_SyncComponent), kListm_syncAoeOnReactActors, InvokeSyncListm_syncAoeOnReactActors);
		NetworkCRC.RegisterBehaviour("Martyr_SyncComponent", 0);
	}

	private void HookSetDamageCrystal(int value)
	{
		NetworkDamageCrystals = value;
		m_clientCrystalAdjustment = 0;
		m_clientDamageThisTurn = 0;
	}

	public void OnClientCrystalConsumed()
	{
		m_clientCrystalAdjustment = Mathf.Min(0, -DamageCrystals);
	}

	public bool IsBonusActive(ActorData owner)
	{
		return CrystalsSpentThisTurn || owner.GetAbilityData().HasQueuedAbilityOfType(typeof(MartyrSpendCrystals)); // , true in rogues
	}

	public int SpentDamageCrystals(ActorData owner)
	{
		if (IsBonusActive(owner))
		{
			return DamageCrystals;
		}
		return 0;
	}

	public void AddAoeOnReactActor(ActorData actor)
	{
		if (actor != null && actor.ActorIndex >= 0)
		{
			m_syncAoeOnReactActors.Add((uint)actor.ActorIndex);
		}
	}

	public void RemoveAoeOnReactActor(ActorData actor)
	{
		if (actor != null && actor.ActorIndex >= 0)
		{
			m_syncAoeOnReactActors.Remove((uint)actor.ActorIndex);
		}
	}

	public bool ActorHasAoeOnReactEffect(ActorData actor)
	{
		if (actor != null && actor.ActorIndex >= 0)
		{
			return m_syncAoeOnReactActors.Contains((uint)actor.ActorIndex);
		}
		return false;
	}

	// rogues
	//public Martyr_SyncComponent()
	//{
	//	base.InitSyncObject(m_syncAoeOnReactActors);
	//}

	// reactor
	private void UNetVersion()
	{
	}
	// rogues
	//private void MirrorProcessed()
	//{
	//}

	// removed in rogues
	protected static void InvokeSyncListm_syncAoeOnReactActors(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("SyncList m_syncAoeOnReactActors called on server.");
			return;
		}
		((Martyr_SyncComponent)obj).m_syncAoeOnReactActors.HandleMsg(reader);
	}

	// removed in rogues
	private void Awake()
	{
		m_syncAoeOnReactActors.InitializeBehaviour(this, kListm_syncAoeOnReactActors);
	}

	// reactor
	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		if (forceAll)
		{
			SyncListUInt.WriteInstance(writer, m_syncAoeOnReactActors);
			writer.Write(CrystalsSpentThisTurn);
			writer.WritePackedUInt32((uint)DamageCrystals);
			writer.WritePackedUInt32((uint)m_syncNumTurnsAtFullEnergy);
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
			SyncListUInt.WriteInstance(writer, m_syncAoeOnReactActors);
		}
		if ((syncVarDirtyBits & 2) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			writer.Write(CrystalsSpentThisTurn);
		}
		if ((syncVarDirtyBits & 4) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)DamageCrystals);
		}
		if ((syncVarDirtyBits & 8) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_syncNumTurnsAtFullEnergy);
		}
		if (!flag)
		{
			writer.WritePackedUInt32(syncVarDirtyBits);
		}
		return flag;
	}

	// reactor
	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
		if (initialState)
		{
			SyncListUInt.ReadReference(reader, m_syncAoeOnReactActors);
			CrystalsSpentThisTurn = reader.ReadBoolean();
			DamageCrystals = (int)reader.ReadPackedUInt32();
			m_syncNumTurnsAtFullEnergy = (int)reader.ReadPackedUInt32();
			return;
		}
		int num = (int)reader.ReadPackedUInt32();
		if ((num & 1) != 0)
		{
			SyncListUInt.ReadReference(reader, m_syncAoeOnReactActors);
		}
		if ((num & 2) != 0)
		{
			CrystalsSpentThisTurn = reader.ReadBoolean();
		}
		if ((num & 4) != 0)
		{
			HookSetDamageCrystal((int)reader.ReadPackedUInt32());
		}
		if ((num & 8) != 0)
		{
			m_syncNumTurnsAtFullEnergy = (int)reader.ReadPackedUInt32();
		}
	}

	// rogues
	//public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	//{
	//	bool result = base.OnSerialize(writer, forceAll);
	//	if (forceAll)
	//	{
	//		writer.Write(CrystalsSpentThisTurn);
	//		writer.WritePackedInt32(DamageCrystals);
	//		writer.WritePackedInt32(m_syncNumTurnsAtFullEnergy);
	//		return true;
	//	}
	//	writer.WritePackedUInt64(syncVarDirtyBits);
	//	if ((syncVarDirtyBits & 1UL) != 0UL)
	//	{
	//		writer.Write(CrystalsSpentThisTurn);
	//		result = true;
	//	}
	//	if ((syncVarDirtyBits & 2UL) != 0UL)
	//	{
	//		writer.WritePackedInt32(DamageCrystals);
	//		result = true;
	//	}
	//	if ((syncVarDirtyBits & 4UL) != 0UL)
	//	{
	//		writer.WritePackedInt32(m_syncNumTurnsAtFullEnergy);
	//		result = true;
	//	}
	//	return result;
	//}

	// rogues
	//public override void OnDeserialize(NetworkReader reader, bool initialState)
	//{
	//	base.OnDeserialize(reader, initialState);
	//	if (initialState)
	//	{
	//		bool networkCrystalsSpentThisTurn = reader.ReadBoolean();
	//		NetworkCrystalsSpentThisTurn = networkCrystalsSpentThisTurn;
	//		int num = reader.ReadPackedInt32();
	//		HookSetDamageCrystal(num);
	//		NetworkDamageCrystals = num;
	//		int networkm_syncNumTurnsAtFullEnergy = reader.ReadPackedInt32();
	//		Networkm_syncNumTurnsAtFullEnergy = networkm_syncNumTurnsAtFullEnergy;
	//		return;
	//	}
	//	long num2 = (long)reader.ReadPackedUInt64();
	//	if ((num2 & 1L) != 0L)
	//	{
	//		bool networkCrystalsSpentThisTurn2 = reader.ReadBoolean();
	//		NetworkCrystalsSpentThisTurn = networkCrystalsSpentThisTurn2;
	//	}
	//	if ((num2 & 2L) != 0L)
	//	{
	//		int num3 = reader.ReadPackedInt32();
	//		HookSetDamageCrystal(num3);
	//		NetworkDamageCrystals = num3;
	//	}
	//	if ((num2 & 4L) != 0L)
	//	{
	//		int networkm_syncNumTurnsAtFullEnergy2 = reader.ReadPackedInt32();
	//		Networkm_syncNumTurnsAtFullEnergy = networkm_syncNumTurnsAtFullEnergy2;
	//	}
	//}
}

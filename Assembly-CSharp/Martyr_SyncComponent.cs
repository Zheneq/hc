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
			SetSyncVar(value, ref CrystalsSpentThisTurn, 2u);
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
			SetSyncVar(value, ref DamageCrystals, 4u);
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
			SetSyncVar(value, ref m_syncNumTurnsAtFullEnergy, 8u);
		}
	}

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
		return CrystalsSpentThisTurn || owner.GetAbilityData().HasQueuedAbilityOfType(typeof(MartyrSpendCrystals));
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

	private void UNetVersion()
	{
	}

	protected static void InvokeSyncListm_syncAoeOnReactActors(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("SyncList m_syncAoeOnReactActors called on server.");
			return;
		}
		((Martyr_SyncComponent)obj).m_syncAoeOnReactActors.HandleMsg(reader);
	}

	private void Awake()
	{
		m_syncAoeOnReactActors.InitializeBehaviour(this, kListm_syncAoeOnReactActors);
	}

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
}

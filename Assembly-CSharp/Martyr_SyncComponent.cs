using System;
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

	private static int kListm_syncAoeOnReactActors = 0x6851DC48;

	static Martyr_SyncComponent()
	{
		NetworkBehaviour.RegisterSyncListDelegate(typeof(Martyr_SyncComponent), Martyr_SyncComponent.kListm_syncAoeOnReactActors, new NetworkBehaviour.CmdDelegate(Martyr_SyncComponent.InvokeSyncListm_syncAoeOnReactActors));
		NetworkCRC.RegisterBehaviour("Martyr_SyncComponent", 0);
	}

	private void HookSetDamageCrystal(int value)
	{
		this.NetworkDamageCrystals = value;
		this.m_clientCrystalAdjustment = 0;
		this.m_clientDamageThisTurn = 0;
	}

	public void OnClientCrystalConsumed()
	{
		this.m_clientCrystalAdjustment = Mathf.Min(0, -this.DamageCrystals);
	}

	public bool IsBonusActive(ActorData owner)
	{
		return this.CrystalsSpentThisTurn || owner.GetAbilityData().HasQueuedAbilityOfType(typeof(MartyrSpendCrystals));
	}

	public int SpentDamageCrystals(ActorData owner)
	{
		if (this.IsBonusActive(owner))
		{
			return this.DamageCrystals;
		}
		return 0;
	}

	public void AddAoeOnReactActor(ActorData actor)
	{
		if (actor != null && actor.ActorIndex >= 0)
		{
			this.m_syncAoeOnReactActors.Add((uint)actor.ActorIndex);
		}
	}

	public void RemoveAoeOnReactActor(ActorData actor)
	{
		if (actor != null)
		{
			if (actor.ActorIndex >= 0)
			{
				this.m_syncAoeOnReactActors.Remove((uint)actor.ActorIndex);
			}
		}
	}

	public bool ActorHasAoeOnReactEffect(ActorData actor)
	{
		bool result = false;
		if (actor != null)
		{
			if (actor.ActorIndex >= 0)
			{
				result = this.m_syncAoeOnReactActors.Contains((uint)actor.ActorIndex);
			}
		}
		return result;
	}

	private void UNetVersion()
	{
	}

	public bool NetworkCrystalsSpentThisTurn
	{
		get
		{
			return this.CrystalsSpentThisTurn;
		}
		[param: In]
		set
		{
			base.SetSyncVar<bool>(value, ref this.CrystalsSpentThisTurn, 2U);
		}
	}

	public int NetworkDamageCrystals
	{
		get
		{
			return this.DamageCrystals;
		}
		[param: In]
		set
		{
			uint dirtyBit = 4U;
			if (NetworkServer.localClientActive)
			{
				if (!base.syncVarHookGuard)
				{
					base.syncVarHookGuard = true;
					this.HookSetDamageCrystal(value);
					base.syncVarHookGuard = false;
				}
			}
			base.SetSyncVar<int>(value, ref this.DamageCrystals, dirtyBit);
		}
	}

	public int Networkm_syncNumTurnsAtFullEnergy
	{
		get
		{
			return this.m_syncNumTurnsAtFullEnergy;
		}
		[param: In]
		set
		{
			base.SetSyncVar<int>(value, ref this.m_syncNumTurnsAtFullEnergy, 8U);
		}
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
		this.m_syncAoeOnReactActors.InitializeBehaviour(this, Martyr_SyncComponent.kListm_syncAoeOnReactActors);
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		if (forceAll)
		{
			SyncListUInt.WriteInstance(writer, this.m_syncAoeOnReactActors);
			writer.Write(this.CrystalsSpentThisTurn);
			writer.WritePackedUInt32((uint)this.DamageCrystals);
			writer.WritePackedUInt32((uint)this.m_syncNumTurnsAtFullEnergy);
			return true;
		}
		bool flag = false;
		if ((base.syncVarDirtyBits & 1U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			SyncListUInt.WriteInstance(writer, this.m_syncAoeOnReactActors);
		}
		if ((base.syncVarDirtyBits & 2U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.Write(this.CrystalsSpentThisTurn);
		}
		if ((base.syncVarDirtyBits & 4U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)this.DamageCrystals);
		}
		if ((base.syncVarDirtyBits & 8U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)this.m_syncNumTurnsAtFullEnergy);
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
			SyncListUInt.ReadReference(reader, this.m_syncAoeOnReactActors);
			this.CrystalsSpentThisTurn = reader.ReadBoolean();
			this.DamageCrystals = (int)reader.ReadPackedUInt32();
			this.m_syncNumTurnsAtFullEnergy = (int)reader.ReadPackedUInt32();
			return;
		}
		int num = (int)reader.ReadPackedUInt32();
		if ((num & 1) != 0)
		{
			SyncListUInt.ReadReference(reader, this.m_syncAoeOnReactActors);
		}
		if ((num & 2) != 0)
		{
			this.CrystalsSpentThisTurn = reader.ReadBoolean();
		}
		if ((num & 4) != 0)
		{
			this.HookSetDamageCrystal((int)reader.ReadPackedUInt32());
		}
		if ((num & 8) != 0)
		{
			this.m_syncNumTurnsAtFullEnergy = (int)reader.ReadPackedUInt32();
		}
	}
}

using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Networking;

public class Archer_SyncComponent : NetworkBehaviour
{
	[SyncVar]
	[HideInInspector]
	public int m_healReactionTargetActor = -1;

	private SyncListUInt m_usedHealReactionActorIndices = new SyncListUInt();

	private SyncListUInt m_expendedHealReactionActorIndices = new SyncListUInt();

	private SyncListUInt m_vfxChangedHealReactionActorIndices = new SyncListUInt();

	private SyncListUInt m_shieldGeneratorTargetActorIndices = new SyncListUInt();

	[HideInInspector]
	[SyncVar]
	public int m_extraAbsorbForShieldGenerator;

	private static int kListm_usedHealReactionActorIndices = -0x14313426;

	private static int kListm_expendedHealReactionActorIndices;

	private static int kListm_vfxChangedHealReactionActorIndices;

	private static int kListm_shieldGeneratorTargetActorIndices;

	static Archer_SyncComponent()
	{
		NetworkBehaviour.RegisterSyncListDelegate(typeof(Archer_SyncComponent), Archer_SyncComponent.kListm_usedHealReactionActorIndices, new NetworkBehaviour.CmdDelegate(Archer_SyncComponent.InvokeSyncListm_usedHealReactionActorIndices));
		Archer_SyncComponent.kListm_expendedHealReactionActorIndices = 0x3FB03DDA;
		NetworkBehaviour.RegisterSyncListDelegate(typeof(Archer_SyncComponent), Archer_SyncComponent.kListm_expendedHealReactionActorIndices, new NetworkBehaviour.CmdDelegate(Archer_SyncComponent.InvokeSyncListm_expendedHealReactionActorIndices));
		Archer_SyncComponent.kListm_vfxChangedHealReactionActorIndices = 0x35EB3169;
		NetworkBehaviour.RegisterSyncListDelegate(typeof(Archer_SyncComponent), Archer_SyncComponent.kListm_vfxChangedHealReactionActorIndices, new NetworkBehaviour.CmdDelegate(Archer_SyncComponent.InvokeSyncListm_vfxChangedHealReactionActorIndices));
		Archer_SyncComponent.kListm_shieldGeneratorTargetActorIndices = 0x34B33;
		NetworkBehaviour.RegisterSyncListDelegate(typeof(Archer_SyncComponent), Archer_SyncComponent.kListm_shieldGeneratorTargetActorIndices, new NetworkBehaviour.CmdDelegate(Archer_SyncComponent.InvokeSyncListm_shieldGeneratorTargetActorIndices));
		NetworkCRC.RegisterBehaviour("Archer_SyncComponent", 0);
	}

	public bool ActorHasUsedHealReaction(ActorData actor)
	{
		bool result;
		if (actor != null)
		{
			result = this.m_usedHealReactionActorIndices.Contains((uint)actor.ActorIndex);
		}
		else
		{
			result = false;
		}
		return result;
	}

	public bool ActorHasExpendedHealReaction(ActorData actor)
	{
		bool result;
		if (actor != null)
		{
			result = this.m_expendedHealReactionActorIndices.Contains((uint)actor.ActorIndex);
		}
		else
		{
			result = false;
		}
		return result;
	}

	public bool ActorShouldSwapVfxForHealReaction(ActorData actor)
	{
		bool result;
		if (actor != null)
		{
			result = this.m_vfxChangedHealReactionActorIndices.Contains((uint)actor.ActorIndex);
		}
		else
		{
			result = false;
		}
		return result;
	}

	public void AddUsedHealReactionActor(ActorData actor)
	{
		this.m_usedHealReactionActorIndices.Add((uint)actor.ActorIndex);
	}

	public void AddExpendedHealReactionActor(ActorData actor)
	{
		this.m_expendedHealReactionActorIndices.Add((uint)actor.ActorIndex);
	}

	public void ChangeVfxForHealReaction(ActorData actor)
	{
		this.m_vfxChangedHealReactionActorIndices.Add((uint)actor.ActorIndex);
	}

	public void ClearUsedHealReactionActors()
	{
		this.m_usedHealReactionActorIndices.Clear();
		this.m_expendedHealReactionActorIndices.Clear();
		this.m_vfxChangedHealReactionActorIndices.Clear();
	}

	public bool ActorIsShieldGeneratorTarget(ActorData actor)
	{
		return actor != null && this.m_shieldGeneratorTargetActorIndices.Contains((uint)actor.ActorIndex);
	}

	public void AddShieldGeneratorTarget(ActorData actor)
	{
		this.m_shieldGeneratorTargetActorIndices.Add((uint)actor.ActorIndex);
	}

	public void ClearShieldGeneratorTargets()
	{
		this.m_shieldGeneratorTargetActorIndices.Clear();
	}

	private void UNetVersion()
	{
	}

	public int Networkm_healReactionTargetActor
	{
		get
		{
			return this.m_healReactionTargetActor;
		}
		[param: In]
		set
		{
			base.SetSyncVar<int>(value, ref this.m_healReactionTargetActor, 1U);
		}
	}

	public int Networkm_extraAbsorbForShieldGenerator
	{
		get
		{
			return this.m_extraAbsorbForShieldGenerator;
		}
		[param: In]
		set
		{
			base.SetSyncVar<int>(value, ref this.m_extraAbsorbForShieldGenerator, 0x20U);
		}
	}

	protected static void InvokeSyncListm_usedHealReactionActorIndices(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("SyncList m_usedHealReactionActorIndices called on server.");
			return;
		}
		((Archer_SyncComponent)obj).m_usedHealReactionActorIndices.HandleMsg(reader);
	}

	protected static void InvokeSyncListm_expendedHealReactionActorIndices(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("SyncList m_expendedHealReactionActorIndices called on server.");
			return;
		}
		((Archer_SyncComponent)obj).m_expendedHealReactionActorIndices.HandleMsg(reader);
	}

	protected static void InvokeSyncListm_vfxChangedHealReactionActorIndices(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("SyncList m_vfxChangedHealReactionActorIndices called on server.");
			return;
		}
		((Archer_SyncComponent)obj).m_vfxChangedHealReactionActorIndices.HandleMsg(reader);
	}

	protected static void InvokeSyncListm_shieldGeneratorTargetActorIndices(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("SyncList m_shieldGeneratorTargetActorIndices called on server.");
			return;
		}
		((Archer_SyncComponent)obj).m_shieldGeneratorTargetActorIndices.HandleMsg(reader);
	}

	private void Awake()
	{
		this.m_usedHealReactionActorIndices.InitializeBehaviour(this, Archer_SyncComponent.kListm_usedHealReactionActorIndices);
		this.m_expendedHealReactionActorIndices.InitializeBehaviour(this, Archer_SyncComponent.kListm_expendedHealReactionActorIndices);
		this.m_vfxChangedHealReactionActorIndices.InitializeBehaviour(this, Archer_SyncComponent.kListm_vfxChangedHealReactionActorIndices);
		this.m_shieldGeneratorTargetActorIndices.InitializeBehaviour(this, Archer_SyncComponent.kListm_shieldGeneratorTargetActorIndices);
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		if (forceAll)
		{
			writer.WritePackedUInt32((uint)this.m_healReactionTargetActor);
			SyncListUInt.WriteInstance(writer, this.m_usedHealReactionActorIndices);
			SyncListUInt.WriteInstance(writer, this.m_expendedHealReactionActorIndices);
			SyncListUInt.WriteInstance(writer, this.m_vfxChangedHealReactionActorIndices);
			SyncListUInt.WriteInstance(writer, this.m_shieldGeneratorTargetActorIndices);
			writer.WritePackedUInt32((uint)this.m_extraAbsorbForShieldGenerator);
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
			writer.WritePackedUInt32((uint)this.m_healReactionTargetActor);
		}
		if ((base.syncVarDirtyBits & 2U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			SyncListUInt.WriteInstance(writer, this.m_usedHealReactionActorIndices);
		}
		if ((base.syncVarDirtyBits & 4U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			SyncListUInt.WriteInstance(writer, this.m_expendedHealReactionActorIndices);
		}
		if ((base.syncVarDirtyBits & 8U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			SyncListUInt.WriteInstance(writer, this.m_vfxChangedHealReactionActorIndices);
		}
		if ((base.syncVarDirtyBits & 0x10U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			SyncListUInt.WriteInstance(writer, this.m_shieldGeneratorTargetActorIndices);
		}
		if ((base.syncVarDirtyBits & 0x20U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)this.m_extraAbsorbForShieldGenerator);
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
			this.m_healReactionTargetActor = (int)reader.ReadPackedUInt32();
			SyncListUInt.ReadReference(reader, this.m_usedHealReactionActorIndices);
			SyncListUInt.ReadReference(reader, this.m_expendedHealReactionActorIndices);
			SyncListUInt.ReadReference(reader, this.m_vfxChangedHealReactionActorIndices);
			SyncListUInt.ReadReference(reader, this.m_shieldGeneratorTargetActorIndices);
			this.m_extraAbsorbForShieldGenerator = (int)reader.ReadPackedUInt32();
			return;
		}
		int num = (int)reader.ReadPackedUInt32();
		if ((num & 1) != 0)
		{
			this.m_healReactionTargetActor = (int)reader.ReadPackedUInt32();
		}
		if ((num & 2) != 0)
		{
			SyncListUInt.ReadReference(reader, this.m_usedHealReactionActorIndices);
		}
		if ((num & 4) != 0)
		{
			SyncListUInt.ReadReference(reader, this.m_expendedHealReactionActorIndices);
		}
		if ((num & 8) != 0)
		{
			SyncListUInt.ReadReference(reader, this.m_vfxChangedHealReactionActorIndices);
		}
		if ((num & 0x10) != 0)
		{
			SyncListUInt.ReadReference(reader, this.m_shieldGeneratorTargetActorIndices);
		}
		if ((num & 0x20) != 0)
		{
			this.m_extraAbsorbForShieldGenerator = (int)reader.ReadPackedUInt32();
		}
	}
}

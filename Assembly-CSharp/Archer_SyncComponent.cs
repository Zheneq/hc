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
	
	private static int kListm_usedHealReactionActorIndices = -338768934;
	private static int kListm_expendedHealReactionActorIndices = 1068514778;
	private static int kListm_vfxChangedHealReactionActorIndices = 90460605;
	private static int kListm_shieldGeneratorTargetActorIndices = 215859;

	public int Networkm_healReactionTargetActor
	{
		get => m_healReactionTargetActor;
		[param: In]
		set => SetSyncVar(value, ref m_healReactionTargetActor, 1u);
	}

	public int Networkm_extraAbsorbForShieldGenerator
	{
		get => m_extraAbsorbForShieldGenerator;
		[param: In]
		set => SetSyncVar(value, ref m_extraAbsorbForShieldGenerator, 32u);
	}

	static Archer_SyncComponent()
	{
		RegisterSyncListDelegate(typeof(Archer_SyncComponent), kListm_usedHealReactionActorIndices, InvokeSyncListm_usedHealReactionActorIndices);
		RegisterSyncListDelegate(typeof(Archer_SyncComponent), kListm_expendedHealReactionActorIndices, InvokeSyncListm_expendedHealReactionActorIndices);
		RegisterSyncListDelegate(typeof(Archer_SyncComponent), kListm_vfxChangedHealReactionActorIndices, InvokeSyncListm_vfxChangedHealReactionActorIndices);
		RegisterSyncListDelegate(typeof(Archer_SyncComponent), kListm_shieldGeneratorTargetActorIndices, InvokeSyncListm_shieldGeneratorTargetActorIndices);
		NetworkCRC.RegisterBehaviour("Archer_SyncComponent", 0);
	}

	public bool ActorHasUsedHealReaction(ActorData actor)
	{
		return actor != null && m_usedHealReactionActorIndices.Contains((uint)actor.ActorIndex);
	}

	public bool ActorHasExpendedHealReaction(ActorData actor)
	{
		return actor != null && m_expendedHealReactionActorIndices.Contains((uint)actor.ActorIndex);
	}

	public bool ActorShouldSwapVfxForHealReaction(ActorData actor)
	{
		return actor != null && m_vfxChangedHealReactionActorIndices.Contains((uint)actor.ActorIndex);
	}

	public void AddUsedHealReactionActor(ActorData actor)
	{
		m_usedHealReactionActorIndices.Add((uint)actor.ActorIndex);
	}

	public void AddExpendedHealReactionActor(ActorData actor)
	{
		m_expendedHealReactionActorIndices.Add((uint)actor.ActorIndex);
	}

	public void ChangeVfxForHealReaction(ActorData actor)
	{
		m_vfxChangedHealReactionActorIndices.Add((uint)actor.ActorIndex);
	}

	public void ClearUsedHealReactionActors()
	{
		m_usedHealReactionActorIndices.Clear();
		m_expendedHealReactionActorIndices.Clear();
		m_vfxChangedHealReactionActorIndices.Clear();
	}

	public bool ActorIsShieldGeneratorTarget(ActorData actor)
	{
		return actor != null && m_shieldGeneratorTargetActorIndices.Contains((uint)actor.ActorIndex);
	}

	public void AddShieldGeneratorTarget(ActorData actor)
	{
		m_shieldGeneratorTargetActorIndices.Add((uint)actor.ActorIndex);
	}

	public void ClearShieldGeneratorTargets()
	{
		m_shieldGeneratorTargetActorIndices.Clear();
	}

	private void UNetVersion()
	{
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
		m_usedHealReactionActorIndices.InitializeBehaviour(this, kListm_usedHealReactionActorIndices);
		m_expendedHealReactionActorIndices.InitializeBehaviour(this, kListm_expendedHealReactionActorIndices);
		m_vfxChangedHealReactionActorIndices.InitializeBehaviour(this, kListm_vfxChangedHealReactionActorIndices);
		m_shieldGeneratorTargetActorIndices.InitializeBehaviour(this, kListm_shieldGeneratorTargetActorIndices);
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		if (forceAll)
		{
			writer.WritePackedUInt32((uint)m_healReactionTargetActor);
			SyncListUInt.WriteInstance(writer, m_usedHealReactionActorIndices);
			SyncListUInt.WriteInstance(writer, m_expendedHealReactionActorIndices);
			SyncListUInt.WriteInstance(writer, m_vfxChangedHealReactionActorIndices);
			SyncListUInt.WriteInstance(writer, m_shieldGeneratorTargetActorIndices);
			writer.WritePackedUInt32((uint)m_extraAbsorbForShieldGenerator);
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
			writer.WritePackedUInt32((uint)m_healReactionTargetActor);
		}
		if ((syncVarDirtyBits & 2) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			SyncListUInt.WriteInstance(writer, m_usedHealReactionActorIndices);
		}
		if ((syncVarDirtyBits & 4) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			SyncListUInt.WriteInstance(writer, m_expendedHealReactionActorIndices);
		}
		if ((syncVarDirtyBits & 8) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			SyncListUInt.WriteInstance(writer, m_vfxChangedHealReactionActorIndices);
		}
		if ((syncVarDirtyBits & 0x10) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			SyncListUInt.WriteInstance(writer, m_shieldGeneratorTargetActorIndices);
		}
		if ((syncVarDirtyBits & 0x20) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_extraAbsorbForShieldGenerator);
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
			m_healReactionTargetActor = (int)reader.ReadPackedUInt32();
			SyncListUInt.ReadReference(reader, m_usedHealReactionActorIndices);
			SyncListUInt.ReadReference(reader, m_expendedHealReactionActorIndices);
			SyncListUInt.ReadReference(reader, m_vfxChangedHealReactionActorIndices);
			SyncListUInt.ReadReference(reader, m_shieldGeneratorTargetActorIndices);
			m_extraAbsorbForShieldGenerator = (int)reader.ReadPackedUInt32();
			return;
		}
		int num = (int)reader.ReadPackedUInt32();
		if ((num & 1) != 0)
		{
			m_healReactionTargetActor = (int)reader.ReadPackedUInt32();
		}
		if ((num & 2) != 0)
		{
			SyncListUInt.ReadReference(reader, m_usedHealReactionActorIndices);
		}
		if ((num & 4) != 0)
		{
			SyncListUInt.ReadReference(reader, m_expendedHealReactionActorIndices);
		}
		if ((num & 8) != 0)
		{
			SyncListUInt.ReadReference(reader, m_vfxChangedHealReactionActorIndices);
		}
		if ((num & 0x10) != 0)
		{
			SyncListUInt.ReadReference(reader, m_shieldGeneratorTargetActorIndices);
		}
		if ((num & 0x20) != 0)
		{
			m_extraAbsorbForShieldGenerator = (int)reader.ReadPackedUInt32();
		}
	}
}

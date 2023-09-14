// ROGUES
// SERVER

using System.Collections.Generic;
using System.Runtime.InteropServices;
//using Mirror;
using UnityEngine.Networking;

public class SharedActionBuffer : NetworkBehaviour
{
	// removed in rogues
	[SyncVar(hook = "HookSetActionPhase")]
	private ActionBufferPhase m_actionPhase;
	[SyncVar(hook = "HookSetAbilityPhase")]
	private AbilityPriority m_abilityPhase;

	// removed in rogues
	public ActionBufferPhase Networkm_actionPhase
	{
		get
		{
			return m_actionPhase;
		}
		[param: In]
		set
		{
            if (NetworkServer.localClientActive && !syncVarHookGuard)
            {
				syncVarHookGuard = true;
                HookSetActionPhase(value);
				syncVarHookGuard = false;
            }
            SetSyncVar(value, ref m_actionPhase, 1u);
		}
	}

	public AbilityPriority Networkm_abilityPhase
	{
		get
		{
			return m_abilityPhase;
		}
		[param: In]
		set
		{
			if (NetworkServer.localClientActive && !syncVarHookGuard)
			{
				syncVarHookGuard = true;
				HookSetAbilityPhase(value);
				syncVarHookGuard = false;
			}
			SetSyncVar(value, ref m_abilityPhase, 2u); // 1UL in rogues
		}
	}

	// server-only
#if SERVER
	public void SetDataFromServer(ActionBufferPhase actionPhase, AbilityPriority abilityPhase)  // no actionPhase in rogues
	{
		Networkm_actionPhase = actionPhase;
		Networkm_abilityPhase = abilityPhase;
	}
#endif

	// removed in rogues
	private void HookSetActionPhase(ActionBufferPhase value)
	{
		Networkm_actionPhase = value;
		SynchronizeClientData();
	}

	private void HookSetAbilityPhase(AbilityPriority value)
	{
		Networkm_abilityPhase = value;
		SynchronizeClientData();
	}

	private void SynchronizeClientData()
	{
		if (ClientActionBuffer.Get() != null)
		{
			ClientActionBuffer.Get().SetDataFromShared(m_actionPhase, m_abilityPhase);  // no m_actionPhase in rogues
		}
	}

	// reactor
	private void UNetVersion()
	{
	}
	// rogues
	//private void MirrorProcessed()
	//{
	//}

	// reactor
	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		if (forceAll)
		{
			writer.Write((int)m_actionPhase);
			writer.Write((int)m_abilityPhase);
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
			writer.Write((int)m_actionPhase);
		}
		if ((syncVarDirtyBits & 2) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			writer.Write((int)m_abilityPhase);
		}
		if (!flag)
		{
			writer.WritePackedUInt32(syncVarDirtyBits);
		}
		return flag;
	}
	// rogues
	//public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	//{
	//	bool result = base.OnSerialize(writer, forceAll);
	//	if (forceAll)
	//	{
	//		writer.WritePackedInt32((int)this.m_abilityPhase);
	//		return true;
	//	}
	//	writer.WritePackedUInt64(base.syncVarDirtyBits);
	//	if ((base.syncVarDirtyBits & 1UL) != 0UL)
	//	{
	//		writer.WritePackedInt32((int)this.m_abilityPhase);
	//		result = true;
	//	}
	//	return result;
	//}

	// reactor
	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
		if (initialState)
		{
			m_actionPhase = (ActionBufferPhase)reader.ReadInt32();
			m_abilityPhase = (AbilityPriority)reader.ReadInt32();
			return;
		}
		int dirtyBits = (int)reader.ReadPackedUInt32();
		if ((dirtyBits & 1) != 0)
		{
			HookSetActionPhase((ActionBufferPhase)reader.ReadInt32());
		}
		if ((dirtyBits & 2) != 0)
		{
			HookSetAbilityPhase((AbilityPriority)reader.ReadInt32());
		}
	}
	// rogues
	//public override void OnDeserialize(NetworkReader reader, bool initialState)
	//{
	//	base.OnDeserialize(reader, initialState);
	//	if (initialState)
	//	{
	//		AbilityPriority abilityPriority = (AbilityPriority)reader.ReadPackedInt32();
	//		this.HookSetAbilityPhase(abilityPriority);
	//		this.Networkm_abilityPhase = abilityPriority;
	//		return;
	//	}
	//	long num = (long)reader.ReadPackedUInt64();
	//	if ((num & 1L) != 0L)
	//	{
	//		AbilityPriority abilityPriority2 = (AbilityPriority)reader.ReadPackedInt32();
	//		this.HookSetAbilityPhase(abilityPriority2);
	//		this.Networkm_abilityPhase = abilityPriority2;
	//	}
	//}

#if SERVER
	// custom
	public override bool OnRebuildObservers(HashSet<NetworkConnection> observers, bool initialize)
	{
		return TeamSensitiveUtils.OnRebuildObservers_NotForReconnection(observers, this);
	}

	// custom
	public override bool OnCheckObserver(NetworkConnection conn)
	{
		return TeamSensitiveUtils.OnCheckObserver_NotForReconnection(conn, this);
	}
#endif
}

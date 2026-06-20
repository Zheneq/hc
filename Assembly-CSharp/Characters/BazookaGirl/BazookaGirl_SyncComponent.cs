using System.Runtime.InteropServices;
using UnityEngine.Networking;

public class BazookaGirl_SyncComponent : NetworkBehaviour
{
	// TODO ZUKI update sync component
	[SyncVar]
	public int m_basicAttackLastCastTurn = -1;
	[SyncVar]
	public int m_basicAttackConsecutiveTurns;
#if SERVER
	public int m_lastCinematicRequested; // custom
#endif

	public int Networkm_basicAttackLastCastTurn
	{
		get => m_basicAttackLastCastTurn;
		[param: In]
		set => SetSyncVar(value, ref m_basicAttackLastCastTurn, 1u);
	}

	public int Networkm_basicAttackConsecutiveTurns
	{
		get => m_basicAttackConsecutiveTurns;
		[param: In]
		set => SetSyncVar(value, ref m_basicAttackConsecutiveTurns, 2u);
	}

	private void UNetVersion()  // MirrorProcessed in rogues
	{
	}

	// reactor
	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		if (forceAll)
		{
			writer.WritePackedUInt32((uint)m_basicAttackLastCastTurn);
			writer.WritePackedUInt32((uint)m_basicAttackConsecutiveTurns);
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
			writer.WritePackedUInt32((uint)m_basicAttackLastCastTurn);
		}
		if ((syncVarDirtyBits & 2) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_basicAttackConsecutiveTurns);
		}
		if (!flag)
		{
			writer.WritePackedUInt32(syncVarDirtyBits);
		}
		return flag;
	}
	// rogues
	// public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	// {
	// 	bool result = base.OnSerialize(writer, forceAll);
	// 	if (forceAll)
	// 	{
	// 		writer.WritePackedInt32(m_basicAttackLastCastTurn);
	// 		writer.WritePackedInt32(m_basicAttackConsecutiveTurns);
	// 		return true;
	// 	}
	// 	writer.WritePackedUInt64(syncVarDirtyBits);
	// 	if ((syncVarDirtyBits & 1) != 0)
	// 	{
	// 		writer.WritePackedInt32(m_basicAttackLastCastTurn);
	// 		result = true;
	// 	}
	// 	if ((syncVarDirtyBits & 2) != 0)
	// 	{
	// 		writer.WritePackedInt32(m_basicAttackConsecutiveTurns);
	// 		result = true;
	// 	}
	// 	return result;
	// }

	// reactor
	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
		if (initialState)
		{
			m_basicAttackLastCastTurn = (int)reader.ReadPackedUInt32();
			m_basicAttackConsecutiveTurns = (int)reader.ReadPackedUInt32();
			return;
		}
		int num = (int)reader.ReadPackedUInt32();
		if ((num & 1) != 0)
		{
			m_basicAttackLastCastTurn = (int)reader.ReadPackedUInt32();
		}
		if ((num & 2) != 0)
		{
			m_basicAttackConsecutiveTurns = (int)reader.ReadPackedUInt32();
		}
	}
	// rogues
	// public override void OnDeserialize(NetworkReader reader, bool initialState)
	// {
	// 	base.OnDeserialize(reader, initialState);
	// 	if (initialState)
	// 	{
	// 		Networkm_basicAttackLastCastTurn = reader.ReadPackedInt32();
	// 		Networkm_basicAttackConsecutiveTurns = reader.ReadPackedInt32();
	// 		return;
	// 	}
	// 	long num = (long)reader.ReadPackedUInt64();
	// 	if ((num & 1L) != 0L)
	// 	{
	// 		int networkm_basicAttackLastCastTurn2 = reader.ReadPackedInt32();
	// 		Networkm_basicAttackLastCastTurn = networkm_basicAttackLastCastTurn2;
	// 	}
	// 	if ((num & 2L) != 0L)
	// 	{
	// 		int networkm_basicAttackConsecutiveTurns2 = reader.ReadPackedInt32();
	// 		Networkm_basicAttackConsecutiveTurns = networkm_basicAttackConsecutiveTurns2;
	// 	}
	// }
}

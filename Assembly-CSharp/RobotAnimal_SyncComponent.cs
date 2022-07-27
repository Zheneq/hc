// ROGUES
// SERVER
using System.Runtime.InteropServices;
using UnityEngine.Networking;

public class RobotAnimal_SyncComponent : NetworkBehaviour
{
	[SyncVar]
	public int m_biteLastCastTurn = -1;
	[SyncVar]
	public int m_biteLastHitTurn = -1;

	public int Networkm_biteLastCastTurn
	{
		get
		{
			return m_biteLastCastTurn;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_biteLastCastTurn, 1u);  // 1ul in rogues
		}
	}

	public int Networkm_biteLastHitTurn
	{
		get
		{
			return m_biteLastHitTurn;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_biteLastHitTurn, 2u);  // 2ul in rogues
		}
	}

	private void UNetVersion()  // MirrorProcessed in rogues
	{
	}

	// reactor
	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		if (forceAll)
		{
			writer.WritePackedUInt32((uint)m_biteLastCastTurn);
			writer.WritePackedUInt32((uint)m_biteLastHitTurn);
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
			writer.WritePackedUInt32((uint)m_biteLastCastTurn);
		}
		if ((syncVarDirtyBits & 2) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_biteLastHitTurn);
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
	// 		writer.WritePackedInt32(m_biteLastCastTurn);
	// 		writer.WritePackedInt32(m_biteLastHitTurn);
	// 		return true;
	// 	}
	// 	writer.WritePackedUInt64(syncVarDirtyBits);
	// 	if ((syncVarDirtyBits & 1UL) != 0UL)
	// 	{
	// 		writer.WritePackedInt32(m_biteLastCastTurn);
	// 		result = true;
	// 	}
	// 	if ((syncVarDirtyBits & 2UL) != 0UL)
	// 	{
	// 		writer.WritePackedInt32(m_biteLastHitTurn);
	// 		result = true;
	// 	}
	// 	return result;
	// }

	// reactor
	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
		if (initialState)
		{
			m_biteLastCastTurn = (int)reader.ReadPackedUInt32();
			m_biteLastHitTurn = (int)reader.ReadPackedUInt32();
			return;
		}
		int num = (int)reader.ReadPackedUInt32();
		if ((num & 1) != 0)
		{
			m_biteLastCastTurn = (int)reader.ReadPackedUInt32();
		}
		if ((num & 2) != 0)
		{
			m_biteLastHitTurn = (int)reader.ReadPackedUInt32();
		}
	}
	// rogues
	// public override void OnDeserialize(NetworkReader reader, bool initialState)
	// {
	// 	base.OnDeserialize(reader, initialState);
	// 	if (initialState)
	// 	{
	// 		Networkm_biteLastCastTurn = reader.ReadPackedInt32();
	// 		Networkm_biteLastHitTurn = reader.ReadPackedInt32();
	// 		return;
	// 	}
	// 	long num = (long)reader.ReadPackedUInt64();
	// 	if ((num & 1L) != 0L)
	// 	{
	// 		Networkm_biteLastCastTurn = reader.ReadPackedInt32();
	// 	}
	// 	if ((num & 2L) != 0L)
	// 	{
	// 		Networkm_biteLastHitTurn = reader.ReadPackedInt32();
	// 	}
	// }
}

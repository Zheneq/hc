using System.Runtime.InteropServices;
using UnityEngine.Networking;

public class BazookaGirl_SyncComponent : NetworkBehaviour
{
	[SyncVar]
	public int m_basicAttackLastCastTurn = -1;
	[SyncVar]
	public int m_basicAttackConsecutiveTurns;

	public int Networkm_basicAttackLastCastTurn
	{
		get
		{
			return m_basicAttackLastCastTurn;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_basicAttackLastCastTurn, 1u);
		}
	}

	public int Networkm_basicAttackConsecutiveTurns
	{
		get
		{
			return m_basicAttackConsecutiveTurns;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_basicAttackConsecutiveTurns, 2u);
		}
	}

	private void UNetVersion()
	{
	}

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
}

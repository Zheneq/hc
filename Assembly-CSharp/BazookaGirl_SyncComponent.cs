using System;
using System.Runtime.InteropServices;
using UnityEngine.Networking;

public class BazookaGirl_SyncComponent : NetworkBehaviour
{
	[SyncVar]
	public int m_basicAttackLastCastTurn = -1;

	[SyncVar]
	public int m_basicAttackConsecutiveTurns;

	private void UNetVersion()
	{
	}

	public int Networkm_basicAttackLastCastTurn
	{
		get
		{
			return this.m_basicAttackLastCastTurn;
		}
		[param: In]
		set
		{
			base.SetSyncVar<int>(value, ref this.m_basicAttackLastCastTurn, 1U);
		}
	}

	public int Networkm_basicAttackConsecutiveTurns
	{
		get
		{
			return this.m_basicAttackConsecutiveTurns;
		}
		[param: In]
		set
		{
			base.SetSyncVar<int>(value, ref this.m_basicAttackConsecutiveTurns, 2U);
		}
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		if (forceAll)
		{
			writer.WritePackedUInt32((uint)this.m_basicAttackLastCastTurn);
			writer.WritePackedUInt32((uint)this.m_basicAttackConsecutiveTurns);
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
			writer.WritePackedUInt32((uint)this.m_basicAttackLastCastTurn);
		}
		if ((base.syncVarDirtyBits & 2U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)this.m_basicAttackConsecutiveTurns);
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
			this.m_basicAttackLastCastTurn = (int)reader.ReadPackedUInt32();
			this.m_basicAttackConsecutiveTurns = (int)reader.ReadPackedUInt32();
			return;
		}
		int num = (int)reader.ReadPackedUInt32();
		if ((num & 1) != 0)
		{
			this.m_basicAttackLastCastTurn = (int)reader.ReadPackedUInt32();
		}
		if ((num & 2) != 0)
		{
			this.m_basicAttackConsecutiveTurns = (int)reader.ReadPackedUInt32();
		}
	}
}

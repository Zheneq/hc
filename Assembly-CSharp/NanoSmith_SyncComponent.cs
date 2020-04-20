using System;
using System.Runtime.InteropServices;
using UnityEngine.Networking;

public class NanoSmith_SyncComponent : NetworkBehaviour
{
	[SyncVar]
	internal int m_extraAbsorbOnVacuumBomb;

	private void UNetVersion()
	{
	}

	public int Networkm_extraAbsorbOnVacuumBomb
	{
		get
		{
			return this.m_extraAbsorbOnVacuumBomb;
		}
		[param: In]
		set
		{
			base.SetSyncVar<int>(value, ref this.m_extraAbsorbOnVacuumBomb, 1U);
		}
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		if (forceAll)
		{
			writer.WritePackedUInt32((uint)this.m_extraAbsorbOnVacuumBomb);
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
			writer.WritePackedUInt32((uint)this.m_extraAbsorbOnVacuumBomb);
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
			this.m_extraAbsorbOnVacuumBomb = (int)reader.ReadPackedUInt32();
			return;
		}
		int num = (int)reader.ReadPackedUInt32();
		if ((num & 1) != 0)
		{
			this.m_extraAbsorbOnVacuumBomb = (int)reader.ReadPackedUInt32();
		}
	}
}

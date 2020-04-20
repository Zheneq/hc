using System;
using System.Runtime.InteropServices;
using UnityEngine.Networking;

public class Soldier_SyncComponent : NetworkBehaviour
{
	[SyncVar]
	public sbyte m_lastPrimaryUsedMode;

	private void UNetVersion()
	{
	}

	public sbyte Networkm_lastPrimaryUsedMode
	{
		get
		{
			return this.m_lastPrimaryUsedMode;
		}
		[param: In]
		set
		{
			base.SetSyncVar<sbyte>(value, ref this.m_lastPrimaryUsedMode, 1U);
		}
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		if (forceAll)
		{
			writer.WritePackedUInt32((uint)this.m_lastPrimaryUsedMode);
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
			writer.WritePackedUInt32((uint)this.m_lastPrimaryUsedMode);
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
			this.m_lastPrimaryUsedMode = (sbyte)reader.ReadPackedUInt32();
			return;
		}
		int num = (int)reader.ReadPackedUInt32();
		if ((num & 1) != 0)
		{
			this.m_lastPrimaryUsedMode = (sbyte)reader.ReadPackedUInt32();
		}
	}
}

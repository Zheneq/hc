using System.Runtime.InteropServices;
using UnityEngine.Networking;

public class Soldier_SyncComponent : NetworkBehaviour
{
	[SyncVar]
	public sbyte m_lastPrimaryUsedMode;

	public sbyte Networkm_lastPrimaryUsedMode
	{
		get => m_lastPrimaryUsedMode;
		[param: In]
		set => SetSyncVar(value, ref m_lastPrimaryUsedMode, 1u);
	}

	private void UNetVersion()
	{
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		if (forceAll)
		{
			writer.WritePackedUInt32((uint)m_lastPrimaryUsedMode);
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
			writer.WritePackedUInt32((uint)m_lastPrimaryUsedMode);
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
			m_lastPrimaryUsedMode = (sbyte)reader.ReadPackedUInt32();
			return;
		}
		int num = (int)reader.ReadPackedUInt32();
		if ((num & 1) != 0)
		{
			m_lastPrimaryUsedMode = (sbyte)reader.ReadPackedUInt32();
		}
	}
}

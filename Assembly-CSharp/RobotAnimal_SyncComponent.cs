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
			SetSyncVar(value, ref m_biteLastCastTurn, 1u);
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
			SetSyncVar(value, ref m_biteLastHitTurn, 2u);
		}
	}

	private void UNetVersion()
	{
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		if (forceAll)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					writer.WritePackedUInt32((uint)m_biteLastCastTurn);
					writer.WritePackedUInt32((uint)m_biteLastHitTurn);
					return true;
				}
			}
		}
		bool flag = false;
		if ((base.syncVarDirtyBits & 1) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_biteLastCastTurn);
		}
		if ((base.syncVarDirtyBits & 2) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_biteLastHitTurn);
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
}

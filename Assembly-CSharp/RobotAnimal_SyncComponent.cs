using System;
using System.Runtime.InteropServices;
using UnityEngine.Networking;

public class RobotAnimal_SyncComponent : NetworkBehaviour
{
	[SyncVar]
	public int m_biteLastCastTurn = -1;

	[SyncVar]
	public int m_biteLastHitTurn = -1;

	private void UNetVersion()
	{
	}

	public int Networkm_biteLastCastTurn
	{
		get
		{
			return this.m_biteLastCastTurn;
		}
		[param: In]
		set
		{
			base.SetSyncVar<int>(value, ref this.m_biteLastCastTurn, 1U);
		}
	}

	public int Networkm_biteLastHitTurn
	{
		get
		{
			return this.m_biteLastHitTurn;
		}
		[param: In]
		set
		{
			base.SetSyncVar<int>(value, ref this.m_biteLastHitTurn, 2U);
		}
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		if (forceAll)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(RobotAnimal_SyncComponent.OnSerialize(NetworkWriter, bool)).MethodHandle;
			}
			writer.WritePackedUInt32((uint)this.m_biteLastCastTurn);
			writer.WritePackedUInt32((uint)this.m_biteLastHitTurn);
			return true;
		}
		bool flag = false;
		if ((base.syncVarDirtyBits & 1U) != 0U)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!flag)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)this.m_biteLastCastTurn);
		}
		if ((base.syncVarDirtyBits & 2U) != 0U)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)this.m_biteLastHitTurn);
		}
		if (!flag)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			writer.WritePackedUInt32(base.syncVarDirtyBits);
		}
		return flag;
	}

	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
		if (initialState)
		{
			this.m_biteLastCastTurn = (int)reader.ReadPackedUInt32();
			this.m_biteLastHitTurn = (int)reader.ReadPackedUInt32();
			return;
		}
		int num = (int)reader.ReadPackedUInt32();
		if ((num & 1) != 0)
		{
			this.m_biteLastCastTurn = (int)reader.ReadPackedUInt32();
		}
		if ((num & 2) != 0)
		{
			this.m_biteLastHitTurn = (int)reader.ReadPackedUInt32();
		}
	}
}

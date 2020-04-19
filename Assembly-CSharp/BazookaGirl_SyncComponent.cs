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
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(BazookaGirl_SyncComponent.OnSerialize(NetworkWriter, bool)).MethodHandle;
			}
			writer.WritePackedUInt32((uint)this.m_basicAttackLastCastTurn);
			writer.WritePackedUInt32((uint)this.m_basicAttackConsecutiveTurns);
			return true;
		}
		bool flag = false;
		if ((base.syncVarDirtyBits & 1U) != 0U)
		{
			for (;;)
			{
				switch (5)
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
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)this.m_basicAttackLastCastTurn);
		}
		if ((base.syncVarDirtyBits & 2U) != 0U)
		{
			if (!flag)
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)this.m_basicAttackConsecutiveTurns);
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
		}
		return flag;
	}

	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
		if (initialState)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(BazookaGirl_SyncComponent.OnDeserialize(NetworkReader, bool)).MethodHandle;
			}
			this.m_basicAttackLastCastTurn = (int)reader.ReadPackedUInt32();
			this.m_basicAttackConsecutiveTurns = (int)reader.ReadPackedUInt32();
			return;
		}
		int num = (int)reader.ReadPackedUInt32();
		if ((num & 1) != 0)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			this.m_basicAttackLastCastTurn = (int)reader.ReadPackedUInt32();
		}
		if ((num & 2) != 0)
		{
			this.m_basicAttackConsecutiveTurns = (int)reader.ReadPackedUInt32();
		}
	}
}

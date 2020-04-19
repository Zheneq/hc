using System;
using System.Runtime.InteropServices;
using UnityEngine.Networking;

public class Valkyrie_SyncComponent : NetworkBehaviour
{
	[SyncVar]
	internal ActorCover.CoverDirections m_coverDirection = ActorCover.CoverDirections.INVALID;

	[SyncVar]
	internal int m_extraAbsorbForGuard;

	[SyncVar]
	internal bool m_skipDamageReductionForNextStab;

	[SyncVar]
	internal int m_extraDamageNextShieldThrow;

	private void UNetVersion()
	{
	}

	public ActorCover.CoverDirections Networkm_coverDirection
	{
		get
		{
			return this.m_coverDirection;
		}
		[param: In]
		set
		{
			base.SetSyncVar<ActorCover.CoverDirections>(value, ref this.m_coverDirection, 1U);
		}
	}

	public int Networkm_extraAbsorbForGuard
	{
		get
		{
			return this.m_extraAbsorbForGuard;
		}
		[param: In]
		set
		{
			base.SetSyncVar<int>(value, ref this.m_extraAbsorbForGuard, 2U);
		}
	}

	public bool Networkm_skipDamageReductionForNextStab
	{
		get
		{
			return this.m_skipDamageReductionForNextStab;
		}
		[param: In]
		set
		{
			base.SetSyncVar<bool>(value, ref this.m_skipDamageReductionForNextStab, 4U);
		}
	}

	public int Networkm_extraDamageNextShieldThrow
	{
		get
		{
			return this.m_extraDamageNextShieldThrow;
		}
		[param: In]
		set
		{
			base.SetSyncVar<int>(value, ref this.m_extraDamageNextShieldThrow, 8U);
		}
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		if (forceAll)
		{
			writer.Write((int)this.m_coverDirection);
			writer.WritePackedUInt32((uint)this.m_extraAbsorbForGuard);
			writer.Write(this.m_skipDamageReductionForNextStab);
			writer.WritePackedUInt32((uint)this.m_extraDamageNextShieldThrow);
			return true;
		}
		bool flag = false;
		if ((base.syncVarDirtyBits & 1U) != 0U)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(Valkyrie_SyncComponent.OnSerialize(NetworkWriter, bool)).MethodHandle;
			}
			if (!flag)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.Write((int)this.m_coverDirection);
		}
		if ((base.syncVarDirtyBits & 2U) != 0U)
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
				flag = true;
			}
			writer.WritePackedUInt32((uint)this.m_extraAbsorbForGuard);
		}
		if ((base.syncVarDirtyBits & 4U) != 0U)
		{
			for (;;)
			{
				switch (6)
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
			writer.Write(this.m_skipDamageReductionForNextStab);
		}
		if ((base.syncVarDirtyBits & 8U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)this.m_extraDamageNextShieldThrow);
		}
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
		}
		return flag;
	}

	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
		if (initialState)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(Valkyrie_SyncComponent.OnDeserialize(NetworkReader, bool)).MethodHandle;
			}
			this.m_coverDirection = (ActorCover.CoverDirections)reader.ReadInt32();
			this.m_extraAbsorbForGuard = (int)reader.ReadPackedUInt32();
			this.m_skipDamageReductionForNextStab = reader.ReadBoolean();
			this.m_extraDamageNextShieldThrow = (int)reader.ReadPackedUInt32();
			return;
		}
		int num = (int)reader.ReadPackedUInt32();
		if ((num & 1) != 0)
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
			this.m_coverDirection = (ActorCover.CoverDirections)reader.ReadInt32();
		}
		if ((num & 2) != 0)
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
			this.m_extraAbsorbForGuard = (int)reader.ReadPackedUInt32();
		}
		if ((num & 4) != 0)
		{
			this.m_skipDamageReductionForNextStab = reader.ReadBoolean();
		}
		if ((num & 8) != 0)
		{
			this.m_extraDamageNextShieldThrow = (int)reader.ReadPackedUInt32();
		}
	}
}

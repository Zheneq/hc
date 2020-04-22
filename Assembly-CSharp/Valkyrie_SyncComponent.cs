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

	public ActorCover.CoverDirections Networkm_coverDirection
	{
		get
		{
			return m_coverDirection;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_coverDirection, 1u);
		}
	}

	public int Networkm_extraAbsorbForGuard
	{
		get
		{
			return m_extraAbsorbForGuard;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_extraAbsorbForGuard, 2u);
		}
	}

	public bool Networkm_skipDamageReductionForNextStab
	{
		get
		{
			return m_skipDamageReductionForNextStab;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_skipDamageReductionForNextStab, 4u);
		}
	}

	public int Networkm_extraDamageNextShieldThrow
	{
		get
		{
			return m_extraDamageNextShieldThrow;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_extraDamageNextShieldThrow, 8u);
		}
	}

	private void UNetVersion()
	{
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		if (forceAll)
		{
			writer.Write((int)m_coverDirection);
			writer.WritePackedUInt32((uint)m_extraAbsorbForGuard);
			writer.Write(m_skipDamageReductionForNextStab);
			writer.WritePackedUInt32((uint)m_extraDamageNextShieldThrow);
			return true;
		}
		bool flag = false;
		if ((base.syncVarDirtyBits & 1) != 0)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (!flag)
			{
				while (true)
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
			writer.Write((int)m_coverDirection);
		}
		if ((base.syncVarDirtyBits & 2) != 0)
		{
			while (true)
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
				while (true)
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
			writer.WritePackedUInt32((uint)m_extraAbsorbForGuard);
		}
		if ((base.syncVarDirtyBits & 4) != 0)
		{
			while (true)
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
			writer.Write(m_skipDamageReductionForNextStab);
		}
		if ((base.syncVarDirtyBits & 8) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_extraDamageNextShieldThrow);
		}
		if (!flag)
		{
			while (true)
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
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					m_coverDirection = (ActorCover.CoverDirections)reader.ReadInt32();
					m_extraAbsorbForGuard = (int)reader.ReadPackedUInt32();
					m_skipDamageReductionForNextStab = reader.ReadBoolean();
					m_extraDamageNextShieldThrow = (int)reader.ReadPackedUInt32();
					return;
				}
			}
		}
		int num = (int)reader.ReadPackedUInt32();
		if ((num & 1) != 0)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			m_coverDirection = (ActorCover.CoverDirections)reader.ReadInt32();
		}
		if ((num & 2) != 0)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			m_extraAbsorbForGuard = (int)reader.ReadPackedUInt32();
		}
		if ((num & 4) != 0)
		{
			m_skipDamageReductionForNextStab = reader.ReadBoolean();
		}
		if ((num & 8) != 0)
		{
			m_extraDamageNextShieldThrow = (int)reader.ReadPackedUInt32();
		}
	}
}

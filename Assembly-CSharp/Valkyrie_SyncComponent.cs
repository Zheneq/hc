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
		if ((syncVarDirtyBits & 1) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			writer.Write((int)m_coverDirection);
		}
		if ((syncVarDirtyBits & 2) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_extraAbsorbForGuard);
		}
		if ((syncVarDirtyBits & 4) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			writer.Write(m_skipDamageReductionForNextStab);
		}
		if ((syncVarDirtyBits & 8) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_extraDamageNextShieldThrow);
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
			m_coverDirection = (ActorCover.CoverDirections)reader.ReadInt32();
			m_extraAbsorbForGuard = (int)reader.ReadPackedUInt32();
			m_skipDamageReductionForNextStab = reader.ReadBoolean();
			m_extraDamageNextShieldThrow = (int)reader.ReadPackedUInt32();
			LogJson();
			return;
		}
		int num = (int)reader.ReadPackedUInt32();
		if ((num & 1) != 0)
		{
			m_coverDirection = (ActorCover.CoverDirections)reader.ReadInt32();
		}
		if ((num & 2) != 0)
		{
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
		LogJson(num);
	}

	private void LogJson(int mask = System.Int32.MaxValue)
	{
		var jsonLog = new System.Collections.Generic.List<string>();
		if ((mask & 1) != 0)
		{
			jsonLog.Add($"\"coverDirection\":{DefaultJsonSerializer.Serialize(m_coverDirection)}");
		}
		if ((mask & 2) != 0)
		{
			jsonLog.Add($"\"extraAbsorbForGuard\":{DefaultJsonSerializer.Serialize(m_extraAbsorbForGuard)}");
		}
		if ((mask & 4) != 0)
		{
			jsonLog.Add($"\"skipDamageReductionForNextStab\":{DefaultJsonSerializer.Serialize(m_skipDamageReductionForNextStab)}");
		}
		if ((mask & 8) != 0)
		{
			jsonLog.Add($"\"extraDamageNextShieldThrow\":{DefaultJsonSerializer.Serialize(m_extraDamageNextShieldThrow)}");
		}

		Log.Info($"[JSON] {{\"valkyrie_SyncComponent\":{{{System.String.Join(",", jsonLog.ToArray())}}}}}");
	}
}

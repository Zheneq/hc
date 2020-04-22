using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Networking;

public class FishMan_SyncComponent : NetworkBehaviour
{
	[SyncVar]
	public int m_turnRoamingShapeWasCast = -1;

	[SyncVar]
	public int m_lastTurnCanRepositionRoamingShape = -1;

	[SyncVar]
	public sbyte m_lastBasicAttackEnemyHitCount;

	[SyncVar]
	public Vector3 m_roamingShapeCurPos = Vector3.zero;

	public int Networkm_turnRoamingShapeWasCast
	{
		get
		{
			return m_turnRoamingShapeWasCast;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_turnRoamingShapeWasCast, 1u);
		}
	}

	public int Networkm_lastTurnCanRepositionRoamingShape
	{
		get
		{
			return m_lastTurnCanRepositionRoamingShape;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_lastTurnCanRepositionRoamingShape, 2u);
		}
	}

	public sbyte Networkm_lastBasicAttackEnemyHitCount
	{
		get
		{
			return m_lastBasicAttackEnemyHitCount;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_lastBasicAttackEnemyHitCount, 4u);
		}
	}

	public Vector3 Networkm_roamingShapeCurPos
	{
		get
		{
			return m_roamingShapeCurPos;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_roamingShapeCurPos, 8u);
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
				switch (6)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					writer.WritePackedUInt32((uint)m_turnRoamingShapeWasCast);
					writer.WritePackedUInt32((uint)m_lastTurnCanRepositionRoamingShape);
					writer.WritePackedUInt32((uint)m_lastBasicAttackEnemyHitCount);
					writer.Write(m_roamingShapeCurPos);
					return true;
				}
			}
		}
		bool flag = false;
		if ((base.syncVarDirtyBits & 1) != 0)
		{
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
			writer.WritePackedUInt32((uint)m_turnRoamingShapeWasCast);
		}
		if ((base.syncVarDirtyBits & 2) != 0)
		{
			while (true)
			{
				switch (2)
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
			writer.WritePackedUInt32((uint)m_lastTurnCanRepositionRoamingShape);
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
				while (true)
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
			writer.WritePackedUInt32((uint)m_lastBasicAttackEnemyHitCount);
		}
		if ((base.syncVarDirtyBits & 8) != 0)
		{
			while (true)
			{
				switch (2)
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
			writer.Write(m_roamingShapeCurPos);
		}
		if (!flag)
		{
			while (true)
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
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					m_turnRoamingShapeWasCast = (int)reader.ReadPackedUInt32();
					m_lastTurnCanRepositionRoamingShape = (int)reader.ReadPackedUInt32();
					m_lastBasicAttackEnemyHitCount = (sbyte)reader.ReadPackedUInt32();
					m_roamingShapeCurPos = reader.ReadVector3();
					return;
				}
			}
		}
		int num = (int)reader.ReadPackedUInt32();
		if ((num & 1) != 0)
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
			m_turnRoamingShapeWasCast = (int)reader.ReadPackedUInt32();
		}
		if ((num & 2) != 0)
		{
			m_lastTurnCanRepositionRoamingShape = (int)reader.ReadPackedUInt32();
		}
		if ((num & 4) != 0)
		{
			m_lastBasicAttackEnemyHitCount = (sbyte)reader.ReadPackedUInt32();
		}
		if ((num & 8) == 0)
		{
			return;
		}
		while (true)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			m_roamingShapeCurPos = reader.ReadVector3();
			return;
		}
	}
}

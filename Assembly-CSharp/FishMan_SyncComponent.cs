using System;
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

	private void UNetVersion()
	{
	}

	public int Networkm_turnRoamingShapeWasCast
	{
		get
		{
			return this.m_turnRoamingShapeWasCast;
		}
		[param: In]
		set
		{
			base.SetSyncVar<int>(value, ref this.m_turnRoamingShapeWasCast, 1U);
		}
	}

	public int Networkm_lastTurnCanRepositionRoamingShape
	{
		get
		{
			return this.m_lastTurnCanRepositionRoamingShape;
		}
		[param: In]
		set
		{
			base.SetSyncVar<int>(value, ref this.m_lastTurnCanRepositionRoamingShape, 2U);
		}
	}

	public sbyte Networkm_lastBasicAttackEnemyHitCount
	{
		get
		{
			return this.m_lastBasicAttackEnemyHitCount;
		}
		[param: In]
		set
		{
			base.SetSyncVar<sbyte>(value, ref this.m_lastBasicAttackEnemyHitCount, 4U);
		}
	}

	public Vector3 Networkm_roamingShapeCurPos
	{
		get
		{
			return this.m_roamingShapeCurPos;
		}
		[param: In]
		set
		{
			base.SetSyncVar<Vector3>(value, ref this.m_roamingShapeCurPos, 8U);
		}
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		if (forceAll)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(FishMan_SyncComponent.OnSerialize(NetworkWriter, bool)).MethodHandle;
			}
			writer.WritePackedUInt32((uint)this.m_turnRoamingShapeWasCast);
			writer.WritePackedUInt32((uint)this.m_lastTurnCanRepositionRoamingShape);
			writer.WritePackedUInt32((uint)this.m_lastBasicAttackEnemyHitCount);
			writer.Write(this.m_roamingShapeCurPos);
			return true;
		}
		bool flag = false;
		if ((base.syncVarDirtyBits & 1U) != 0U)
		{
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
			writer.WritePackedUInt32((uint)this.m_turnRoamingShapeWasCast);
		}
		if ((base.syncVarDirtyBits & 2U) != 0U)
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
			writer.WritePackedUInt32((uint)this.m_lastTurnCanRepositionRoamingShape);
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
			writer.WritePackedUInt32((uint)this.m_lastBasicAttackEnemyHitCount);
		}
		if ((base.syncVarDirtyBits & 8U) != 0U)
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
			writer.Write(this.m_roamingShapeCurPos);
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
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(FishMan_SyncComponent.OnDeserialize(NetworkReader, bool)).MethodHandle;
			}
			this.m_turnRoamingShapeWasCast = (int)reader.ReadPackedUInt32();
			this.m_lastTurnCanRepositionRoamingShape = (int)reader.ReadPackedUInt32();
			this.m_lastBasicAttackEnemyHitCount = (sbyte)reader.ReadPackedUInt32();
			this.m_roamingShapeCurPos = reader.ReadVector3();
			return;
		}
		int num = (int)reader.ReadPackedUInt32();
		if ((num & 1) != 0)
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
			this.m_turnRoamingShapeWasCast = (int)reader.ReadPackedUInt32();
		}
		if ((num & 2) != 0)
		{
			this.m_lastTurnCanRepositionRoamingShape = (int)reader.ReadPackedUInt32();
		}
		if ((num & 4) != 0)
		{
			this.m_lastBasicAttackEnemyHitCount = (sbyte)reader.ReadPackedUInt32();
		}
		if ((num & 8) != 0)
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
			this.m_roamingShapeCurPos = reader.ReadVector3();
		}
	}
}

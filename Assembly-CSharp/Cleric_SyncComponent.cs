using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Networking;

public class Cleric_SyncComponent : NetworkBehaviour
{
	[SyncVar]
	internal int m_turnsAreaBuffActive;

	[SyncVar(hook = "MeleeKnockbackAnimRangeChanged")]
	internal int m_meleeKnockbackAnimRange;

	private static readonly int animAttackRange = Animator.StringToHash("AttackRange");

	internal void MeleeKnockbackAnimRangeChanged(int value)
	{
		ActorData component = base.GetComponent<ActorData>();
		if (component != null)
		{
			if (component.GetActorModelData() != null)
			{
				if (component.GetActorModelData().HasAnimatorControllerParamater("AttackRange"))
				{
					Animator modelAnimator = component.GetActorModelData().GetModelAnimator();
					modelAnimator.SetInteger(Cleric_SyncComponent.animAttackRange, value);
				}
			}
		}
	}

	private void UNetVersion()
	{
	}

	public int Networkm_turnsAreaBuffActive
	{
		get
		{
			return this.m_turnsAreaBuffActive;
		}
		[param: In]
		set
		{
			base.SetSyncVar<int>(value, ref this.m_turnsAreaBuffActive, 1U);
		}
	}

	public int Networkm_meleeKnockbackAnimRange
	{
		get
		{
			return this.m_meleeKnockbackAnimRange;
		}
		[param: In]
		set
		{
			uint dirtyBit = 2U;
			if (NetworkServer.localClientActive)
			{
				if (!base.syncVarHookGuard)
				{
					base.syncVarHookGuard = true;
					this.MeleeKnockbackAnimRangeChanged(value);
					base.syncVarHookGuard = false;
				}
			}
			base.SetSyncVar<int>(value, ref this.m_meleeKnockbackAnimRange, dirtyBit);
		}
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		if (forceAll)
		{
			writer.WritePackedUInt32((uint)this.m_turnsAreaBuffActive);
			writer.WritePackedUInt32((uint)this.m_meleeKnockbackAnimRange);
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
			writer.WritePackedUInt32((uint)this.m_turnsAreaBuffActive);
		}
		if ((base.syncVarDirtyBits & 2U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)this.m_meleeKnockbackAnimRange);
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
			this.m_turnsAreaBuffActive = (int)reader.ReadPackedUInt32();
			this.m_meleeKnockbackAnimRange = (int)reader.ReadPackedUInt32();
			return;
		}
		int num = (int)reader.ReadPackedUInt32();
		if ((num & 1) != 0)
		{
			this.m_turnsAreaBuffActive = (int)reader.ReadPackedUInt32();
		}
		if ((num & 2) != 0)
		{
			this.MeleeKnockbackAnimRangeChanged((int)reader.ReadPackedUInt32());
		}
	}
}

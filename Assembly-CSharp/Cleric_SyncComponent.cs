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

	public int Networkm_turnsAreaBuffActive
	{
		get
		{
			return m_turnsAreaBuffActive;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_turnsAreaBuffActive, 1u);
		}
	}

	public int Networkm_meleeKnockbackAnimRange
	{
		get
		{
			return m_meleeKnockbackAnimRange;
		}
		[param: In]
		set
		{
			ref int meleeKnockbackAnimRange = ref m_meleeKnockbackAnimRange;
			if (NetworkServer.localClientActive)
			{
				if (!base.syncVarHookGuard)
				{
					base.syncVarHookGuard = true;
					MeleeKnockbackAnimRangeChanged(value);
					base.syncVarHookGuard = false;
				}
			}
			SetSyncVar(value, ref meleeKnockbackAnimRange, 2u);
		}
	}

	internal void MeleeKnockbackAnimRangeChanged(int value)
	{
		ActorData component = GetComponent<ActorData>();
		if (!(component != null))
		{
			return;
		}
		while (true)
		{
			if (!(component.GetActorModelData() != null))
			{
				return;
			}
			while (true)
			{
				if (component.GetActorModelData().HasAnimatorControllerParamater("AttackRange"))
				{
					while (true)
					{
						Animator modelAnimator = component.GetActorModelData().GetModelAnimator();
						modelAnimator.SetInteger(animAttackRange, value);
						return;
					}
				}
				return;
			}
		}
	}

	private void UNetVersion()
	{
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		if (forceAll)
		{
			writer.WritePackedUInt32((uint)m_turnsAreaBuffActive);
			writer.WritePackedUInt32((uint)m_meleeKnockbackAnimRange);
			return true;
		}
		bool flag = false;
		if ((base.syncVarDirtyBits & 1) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_turnsAreaBuffActive);
		}
		if ((base.syncVarDirtyBits & 2) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_meleeKnockbackAnimRange);
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
			m_turnsAreaBuffActive = (int)reader.ReadPackedUInt32();
			m_meleeKnockbackAnimRange = (int)reader.ReadPackedUInt32();
			return;
		}
		int num = (int)reader.ReadPackedUInt32();
		if ((num & 1) != 0)
		{
			m_turnsAreaBuffActive = (int)reader.ReadPackedUInt32();
		}
		if ((num & 2) == 0)
		{
			return;
		}
		while (true)
		{
			MeleeKnockbackAnimRangeChanged((int)reader.ReadPackedUInt32());
			return;
		}
	}
}

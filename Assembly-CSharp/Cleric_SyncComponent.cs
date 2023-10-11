using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Networking;

public class Cleric_SyncComponent : NetworkBehaviour
{
	[SyncVar] internal int m_turnsAreaBuffActive;
	
	// searched through several replays -- it's always zero
	// maybe because there is no knockback?
	[SyncVar(hook = "MeleeKnockbackAnimRangeChanged")]
	internal int m_meleeKnockbackAnimRange;

	private static readonly int animAttackRange = Animator.StringToHash("AttackRange");

	public int Networkm_turnsAreaBuffActive
	{
		get => m_turnsAreaBuffActive;
		[param: In]
		set => SetSyncVar(value, ref m_turnsAreaBuffActive, 1u);
	}

	public int Networkm_meleeKnockbackAnimRange
	{
		get => m_meleeKnockbackAnimRange;
		[param: In]
		set
		{
			if (NetworkServer.localClientActive && !syncVarHookGuard)
			{
				syncVarHookGuard = true;
				MeleeKnockbackAnimRangeChanged(value);
				syncVarHookGuard = false;
			}
			SetSyncVar(value, ref m_meleeKnockbackAnimRange, 2u);
		}
	}

	internal void MeleeKnockbackAnimRangeChanged(int value)
	{
		ActorData actorData = GetComponent<ActorData>();
		if (actorData != null
		    && actorData.GetActorModelData() != null
		    && actorData.GetActorModelData().HasAnimatorControllerParamater("AttackRange"))
		{
			actorData.GetActorModelData().GetModelAnimator().SetInteger(animAttackRange, value);
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
		if ((syncVarDirtyBits & 1) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_turnsAreaBuffActive);
		}
		if ((syncVarDirtyBits & 2) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_meleeKnockbackAnimRange);
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
			m_turnsAreaBuffActive = (int)reader.ReadPackedUInt32();
			m_meleeKnockbackAnimRange = (int)reader.ReadPackedUInt32();
			return;
		}
		int dirtyBits = (int)reader.ReadPackedUInt32();
		if ((dirtyBits & 1) != 0)
		{
			m_turnsAreaBuffActive = (int)reader.ReadPackedUInt32();
		}
		if ((dirtyBits & 2) != 0)
		{
			MeleeKnockbackAnimRangeChanged((int)reader.ReadPackedUInt32());
		}
	}
}

using System;
using UnityEngine;
using UnityEngine.Networking;

public class Manta_SyncComponent : NetworkBehaviour
{
	private SyncListUInt m_dirtyFightingActorIndices = new SyncListUInt();

	private MantaConeDirtyFighting m_dirtyFightingAbility;

	private static int kListm_dirtyFightingActorIndices = -0x24697228;

	static Manta_SyncComponent()
	{
		NetworkBehaviour.RegisterSyncListDelegate(typeof(Manta_SyncComponent), Manta_SyncComponent.kListm_dirtyFightingActorIndices, new NetworkBehaviour.CmdDelegate(Manta_SyncComponent.InvokeSyncListm_dirtyFightingActorIndices));
		NetworkCRC.RegisterBehaviour("Manta_SyncComponent", 0);
	}

	public void Start()
	{
		ActorData component = base.GetComponent<ActorData>();
		if (component != null)
		{
			if (component.GetAbilityData() != null)
			{
				this.m_dirtyFightingAbility = (component.GetAbilityData().GetAbilityOfType(typeof(MantaConeDirtyFighting)) as MantaConeDirtyFighting);
			}
		}
	}

	public void AddDirtyFightingActor(ActorData effectActor)
	{
		this.m_dirtyFightingActorIndices.Add((uint)effectActor.ActorIndex);
	}

	public void RemoveDirtyFightingActor(ActorData effectActor)
	{
		this.m_dirtyFightingActorIndices.Remove((uint)effectActor.ActorIndex);
	}

	public bool HasDirtyFightingEffect(ActorData effectActor)
	{
		return this.m_dirtyFightingActorIndices.Contains((uint)effectActor.ActorIndex);
	}

	public string GetAccessoryStringForDamage(ActorData targetActor, ActorData caster, Ability ability)
	{
		int dirtyFightingExtraDamage = this.GetDirtyFightingExtraDamage(targetActor);
		if (dirtyFightingExtraDamage > 0)
		{
			return "\n+ " + AbilityUtils.CalculateDamageForTargeter(caster, targetActor, ability, dirtyFightingExtraDamage, false).ToString();
		}
		return null;
	}

	public int GetDirtyFightingExtraDamage(ActorData effectActor)
	{
		if (this.HasDirtyFightingEffect(effectActor))
		{
			if (this.m_dirtyFightingAbility != null)
			{
				return this.m_dirtyFightingAbility.GetEffectExplosionDamage();
			}
		}
		return 0;
	}

	public int GetDirtyFightingExtraTP(ActorData effectActor)
	{
		if (this.HasDirtyFightingEffect(effectActor))
		{
			if (this.m_dirtyFightingAbility != null)
			{
				return this.m_dirtyFightingAbility.GetTechPointGainPerExplosion();
			}
		}
		return 0;
	}

	private void UNetVersion()
	{
	}

	protected static void InvokeSyncListm_dirtyFightingActorIndices(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("SyncList m_dirtyFightingActorIndices called on server.");
			return;
		}
		((Manta_SyncComponent)obj).m_dirtyFightingActorIndices.HandleMsg(reader);
	}

	private void Awake()
	{
		this.m_dirtyFightingActorIndices.InitializeBehaviour(this, Manta_SyncComponent.kListm_dirtyFightingActorIndices);
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		if (forceAll)
		{
			SyncListUInt.WriteInstance(writer, this.m_dirtyFightingActorIndices);
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
			SyncListUInt.WriteInstance(writer, this.m_dirtyFightingActorIndices);
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
			SyncListUInt.ReadReference(reader, this.m_dirtyFightingActorIndices);
			return;
		}
		int num = (int)reader.ReadPackedUInt32();
		if ((num & 1) != 0)
		{
			SyncListUInt.ReadReference(reader, this.m_dirtyFightingActorIndices);
		}
	}
}

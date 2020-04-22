using UnityEngine;
using UnityEngine.Networking;

public class Manta_SyncComponent : NetworkBehaviour
{
	private SyncListUInt m_dirtyFightingActorIndices = new SyncListUInt();

	private MantaConeDirtyFighting m_dirtyFightingAbility;

	private static int kListm_dirtyFightingActorIndices;

	static Manta_SyncComponent()
	{
		kListm_dirtyFightingActorIndices = -610890280;
		NetworkBehaviour.RegisterSyncListDelegate(typeof(Manta_SyncComponent), kListm_dirtyFightingActorIndices, InvokeSyncListm_dirtyFightingActorIndices);
		NetworkCRC.RegisterBehaviour("Manta_SyncComponent", 0);
	}

	public void Start()
	{
		ActorData component = GetComponent<ActorData>();
		if (!(component != null))
		{
			return;
		}
		while (true)
		{
			if (component.GetAbilityData() != null)
			{
				while (true)
				{
					m_dirtyFightingAbility = (component.GetAbilityData().GetAbilityOfType(typeof(MantaConeDirtyFighting)) as MantaConeDirtyFighting);
					return;
				}
			}
			return;
		}
	}

	public void AddDirtyFightingActor(ActorData effectActor)
	{
		m_dirtyFightingActorIndices.Add((uint)effectActor.ActorIndex);
	}

	public void RemoveDirtyFightingActor(ActorData effectActor)
	{
		m_dirtyFightingActorIndices.Remove((uint)effectActor.ActorIndex);
	}

	public bool HasDirtyFightingEffect(ActorData effectActor)
	{
		return m_dirtyFightingActorIndices.Contains((uint)effectActor.ActorIndex);
	}

	public string GetAccessoryStringForDamage(ActorData targetActor, ActorData caster, Ability ability)
	{
		int dirtyFightingExtraDamage = GetDirtyFightingExtraDamage(targetActor);
		if (dirtyFightingExtraDamage > 0)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return "\n+ " + AbilityUtils.CalculateDamageForTargeter(caster, targetActor, ability, dirtyFightingExtraDamage, false);
				}
			}
		}
		return null;
	}

	public int GetDirtyFightingExtraDamage(ActorData effectActor)
	{
		if (HasDirtyFightingEffect(effectActor))
		{
			if (m_dirtyFightingAbility != null)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						return m_dirtyFightingAbility.GetEffectExplosionDamage();
					}
				}
			}
		}
		return 0;
	}

	public int GetDirtyFightingExtraTP(ActorData effectActor)
	{
		if (HasDirtyFightingEffect(effectActor))
		{
			if (m_dirtyFightingAbility != null)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						return m_dirtyFightingAbility.GetTechPointGainPerExplosion();
					}
				}
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
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					Debug.LogError("SyncList m_dirtyFightingActorIndices called on server.");
					return;
				}
			}
		}
		((Manta_SyncComponent)obj).m_dirtyFightingActorIndices.HandleMsg(reader);
	}

	private void Awake()
	{
		m_dirtyFightingActorIndices.InitializeBehaviour(this, kListm_dirtyFightingActorIndices);
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		if (forceAll)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					SyncListUInt.WriteInstance(writer, m_dirtyFightingActorIndices);
					return true;
				}
			}
		}
		bool flag = false;
		if ((base.syncVarDirtyBits & 1) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			SyncListUInt.WriteInstance(writer, m_dirtyFightingActorIndices);
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
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					SyncListUInt.ReadReference(reader, m_dirtyFightingActorIndices);
					return;
				}
			}
		}
		int num = (int)reader.ReadPackedUInt32();
		if ((num & 1) == 0)
		{
			return;
		}
		while (true)
		{
			SyncListUInt.ReadReference(reader, m_dirtyFightingActorIndices);
			return;
		}
	}
}

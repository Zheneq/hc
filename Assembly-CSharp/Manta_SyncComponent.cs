// ROGUES
// SERVER
using UnityEngine;
using UnityEngine.Networking;

public class Manta_SyncComponent : NetworkBehaviour
{
    private SyncListUInt m_dirtyFightingActorIndices = new SyncListUInt();
    private MantaConeDirtyFighting m_dirtyFightingAbility;

    // removed in rogues
    private static int kListm_dirtyFightingActorIndices = -610890280;
    
    // reactor
    static Manta_SyncComponent()
    {
        RegisterSyncListDelegate(typeof(Manta_SyncComponent), kListm_dirtyFightingActorIndices, InvokeSyncListm_dirtyFightingActorIndices);
        NetworkCRC.RegisterBehaviour("Manta_SyncComponent", 0);
    }
    // rogues
    // public Manta_SyncComponent()
    // {
    //     base.InitSyncObject(m_dirtyFightingActorIndices);
    // }

    public void Start()
    {
        ActorData actorData = GetComponent<ActorData>();
        if (actorData != null && actorData.GetAbilityData() != null)
        {
            m_dirtyFightingAbility = actorData.GetAbilityData().GetAbilityOfType(typeof(MantaConeDirtyFighting)) as MantaConeDirtyFighting;
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

    // removed in rogues
    public string GetAccessoryStringForDamage(ActorData targetActor, ActorData caster, Ability ability)
    {
        int dirtyFightingExtraDamage = GetDirtyFightingExtraDamage(targetActor);
        if (dirtyFightingExtraDamage > 0)
        {
            return "\n+ " + AbilityUtils.CalculateDamageForTargeter(
                caster, targetActor, ability, dirtyFightingExtraDamage, false);
        }
        return null;
    }

    public int GetDirtyFightingExtraDamage(ActorData effectActor)
    {
        return HasDirtyFightingEffect(effectActor) && m_dirtyFightingAbility != null
            ? m_dirtyFightingAbility.GetEffectExplosionDamage()
            : 0;
    }

    public int GetDirtyFightingExtraTP(ActorData effectActor)
    {
        return HasDirtyFightingEffect(effectActor) && m_dirtyFightingAbility != null
            ? m_dirtyFightingAbility.GetTechPointGainPerExplosion()
            : 0;
    }

    // reactor
    private void UNetVersion()
    {
    }
    // rogues
    // private void MirrorProcessed()
    // {
    // }

    // removed in rogues
    protected static void InvokeSyncListm_dirtyFightingActorIndices(NetworkBehaviour obj, NetworkReader reader)
    {
        if (!NetworkClient.active)
        {
            Debug.LogError("SyncList m_dirtyFightingActorIndices called on server.");
            return;
        }

        ((Manta_SyncComponent)obj).m_dirtyFightingActorIndices.HandleMsg(reader);
    }

    // removed in rogues
    private void Awake()
    {
        m_dirtyFightingActorIndices.InitializeBehaviour(this, kListm_dirtyFightingActorIndices);
    }

    // removed in rogues
    public override bool OnSerialize(NetworkWriter writer, bool forceAll)
    {
        if (forceAll)
        {
            SyncListUInt.WriteInstance(writer, m_dirtyFightingActorIndices);
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

            SyncListUInt.WriteInstance(writer, m_dirtyFightingActorIndices);
        }
        if (!flag)
        {
            writer.WritePackedUInt32(syncVarDirtyBits);
        }
        return flag;
    }

    // removed in rogues
    public override void OnDeserialize(NetworkReader reader, bool initialState)
    {
        if (initialState)
        {
            SyncListUInt.ReadReference(reader, m_dirtyFightingActorIndices);
            return;
        }
        int num = (int)reader.ReadPackedUInt32();
        if ((num & 1) != 0)
        {
            SyncListUInt.ReadReference(reader, m_dirtyFightingActorIndices);
        }
    }
}
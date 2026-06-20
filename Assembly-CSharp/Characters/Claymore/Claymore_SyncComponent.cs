using UnityEngine;
using UnityEngine.Networking;

public class Claymore_SyncComponent : NetworkBehaviour
{
	private SyncListUInt m_dirtyFightingActorIndexList = new SyncListUInt();
	private SyncListUInt m_dirtyFightingDamageList = new SyncListUInt();
	private static int kListm_dirtyFightingActorIndexList = -722191822;
	private static int kListm_dirtyFightingDamageList = 734449732;

	static Claymore_SyncComponent()
	{
		RegisterSyncListDelegate(typeof(Claymore_SyncComponent), kListm_dirtyFightingActorIndexList, InvokeSyncListm_dirtyFightingActorIndexList);
		RegisterSyncListDelegate(typeof(Claymore_SyncComponent), kListm_dirtyFightingDamageList, InvokeSyncListm_dirtyFightingDamageList);
		NetworkCRC.RegisterBehaviour("Claymore_SyncComponent", 0);
	}

	public void ResetDirtyFightingData()
	{
		if (NetworkServer.active)
		{
			m_dirtyFightingActorIndexList.Clear();
			m_dirtyFightingDamageList.Clear();
		}
	}

	public void TrackActorToDamage(ActorData actor, int damage)
	{
		if (NetworkServer.active)
		{
			m_dirtyFightingActorIndexList.Add((uint)actor.ActorIndex);
			m_dirtyFightingDamageList.Add((uint)damage);
		}
	}

	public int GetDirtyFightingDamageOnActor(ActorData target)
	{
		int result = 0;
		uint actorIndex = (uint)target.ActorIndex;
		for (int i = 0; i < m_dirtyFightingActorIndexList.Count; i++)
		{
			if (m_dirtyFightingActorIndexList[i] == actorIndex)
			{
				result = (int)m_dirtyFightingDamageList[i];
				break;
			}
		}
		return result;
	}

	public string GetTargetPreviewAccessoryString(AbilityTooltipSymbol symbolType, Ability ability, ActorData targetActor, ActorData caster)
	{
		if (symbolType == AbilityTooltipSymbol.Damage)
		{
			int dirtyFightingDamageOnActor = GetDirtyFightingDamageOnActor(targetActor);
			if (dirtyFightingDamageOnActor > 0)
			{
				return "\n+ " + AbilityUtils.CalculateDamageForTargeter(caster, targetActor, ability, dirtyFightingDamageOnActor, false);
			}
		}
		return null;
	}

	private void UNetVersion()
	{
	}

	protected static void InvokeSyncListm_dirtyFightingActorIndexList(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("SyncList m_dirtyFightingActorIndexList called on server.");
			return;
		}
		((Claymore_SyncComponent)obj).m_dirtyFightingActorIndexList.HandleMsg(reader);
	}

	protected static void InvokeSyncListm_dirtyFightingDamageList(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("SyncList m_dirtyFightingDamageList called on server.");
			return;
		}
		((Claymore_SyncComponent)obj).m_dirtyFightingDamageList.HandleMsg(reader);
	}

	private void Awake()
	{
		m_dirtyFightingActorIndexList.InitializeBehaviour(this, kListm_dirtyFightingActorIndexList);
		m_dirtyFightingDamageList.InitializeBehaviour(this, kListm_dirtyFightingDamageList);
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		if (forceAll)
		{
			SyncListUInt.WriteInstance(writer, m_dirtyFightingActorIndexList);
			SyncListUInt.WriteInstance(writer, m_dirtyFightingDamageList);
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
			SyncListUInt.WriteInstance(writer, m_dirtyFightingActorIndexList);
		}
		if ((syncVarDirtyBits & 2) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			SyncListUInt.WriteInstance(writer, m_dirtyFightingDamageList);
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
			SyncListUInt.ReadReference(reader, m_dirtyFightingActorIndexList);
			SyncListUInt.ReadReference(reader, m_dirtyFightingDamageList);
			return;
		}
		int num = (int)reader.ReadPackedUInt32();
		if ((num & 1) != 0)
		{
			SyncListUInt.ReadReference(reader, m_dirtyFightingActorIndexList);
		}
		if ((num & 2) == 0)
		{
			return;
		}
		while (true)
		{
			SyncListUInt.ReadReference(reader, m_dirtyFightingDamageList);
			return;
		}
	}
}

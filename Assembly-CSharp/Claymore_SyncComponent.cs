using System;
using UnityEngine;
using UnityEngine.Networking;

public class Claymore_SyncComponent : NetworkBehaviour
{
	private SyncListUInt m_dirtyFightingActorIndexList = new SyncListUInt();

	private SyncListUInt m_dirtyFightingDamageList = new SyncListUInt();

	private static int kListm_dirtyFightingActorIndexList = -0x2B0BC5CE;

	private static int kListm_dirtyFightingDamageList;

	static Claymore_SyncComponent()
	{
		NetworkBehaviour.RegisterSyncListDelegate(typeof(Claymore_SyncComponent), Claymore_SyncComponent.kListm_dirtyFightingActorIndexList, new NetworkBehaviour.CmdDelegate(Claymore_SyncComponent.InvokeSyncListm_dirtyFightingActorIndexList));
		Claymore_SyncComponent.kListm_dirtyFightingDamageList = 0x2BC6D044;
		NetworkBehaviour.RegisterSyncListDelegate(typeof(Claymore_SyncComponent), Claymore_SyncComponent.kListm_dirtyFightingDamageList, new NetworkBehaviour.CmdDelegate(Claymore_SyncComponent.InvokeSyncListm_dirtyFightingDamageList));
		NetworkCRC.RegisterBehaviour("Claymore_SyncComponent", 0);
	}

	public void ResetDirtyFightingData()
	{
		if (NetworkServer.active)
		{
			this.m_dirtyFightingActorIndexList.Clear();
			this.m_dirtyFightingDamageList.Clear();
		}
	}

	public void TrackActorToDamage(ActorData actor, int damage)
	{
		if (NetworkServer.active)
		{
			this.m_dirtyFightingActorIndexList.Add((uint)actor.ActorIndex);
			this.m_dirtyFightingDamageList.Add((uint)damage);
		}
	}

	public int GetDirtyFightingDamageOnActor(ActorData target)
	{
		int result = 0;
		uint actorIndex = (uint)target.ActorIndex;
		for (int i = 0; i < this.m_dirtyFightingActorIndexList.Count; i++)
		{
			if (this.m_dirtyFightingActorIndexList[i] == actorIndex)
			{
				result = (int)this.m_dirtyFightingDamageList[i];
				return result;
			}
		}
		for (;;)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			break;
		}
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(Claymore_SyncComponent.GetDirtyFightingDamageOnActor(ActorData)).MethodHandle;
			return result;
		}
		return result;
	}

	public string GetTargetPreviewAccessoryString(AbilityTooltipSymbol symbolType, Ability ability, ActorData targetActor, ActorData caster)
	{
		if (symbolType == AbilityTooltipSymbol.Damage)
		{
			int dirtyFightingDamageOnActor = this.GetDirtyFightingDamageOnActor(targetActor);
			if (dirtyFightingDamageOnActor > 0)
			{
				return "\n+ " + AbilityUtils.CalculateDamageForTargeter(caster, targetActor, ability, dirtyFightingDamageOnActor, false).ToString();
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Claymore_SyncComponent.InvokeSyncListm_dirtyFightingActorIndexList(NetworkBehaviour, NetworkReader)).MethodHandle;
			}
			Debug.LogError("SyncList m_dirtyFightingActorIndexList called on server.");
			return;
		}
		((Claymore_SyncComponent)obj).m_dirtyFightingActorIndexList.HandleMsg(reader);
	}

	protected static void InvokeSyncListm_dirtyFightingDamageList(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Claymore_SyncComponent.InvokeSyncListm_dirtyFightingDamageList(NetworkBehaviour, NetworkReader)).MethodHandle;
			}
			Debug.LogError("SyncList m_dirtyFightingDamageList called on server.");
			return;
		}
		((Claymore_SyncComponent)obj).m_dirtyFightingDamageList.HandleMsg(reader);
	}

	private void Awake()
	{
		this.m_dirtyFightingActorIndexList.InitializeBehaviour(this, Claymore_SyncComponent.kListm_dirtyFightingActorIndexList);
		this.m_dirtyFightingDamageList.InitializeBehaviour(this, Claymore_SyncComponent.kListm_dirtyFightingDamageList);
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		if (forceAll)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(Claymore_SyncComponent.OnSerialize(NetworkWriter, bool)).MethodHandle;
			}
			SyncListUInt.WriteInstance(writer, this.m_dirtyFightingActorIndexList);
			SyncListUInt.WriteInstance(writer, this.m_dirtyFightingDamageList);
			return true;
		}
		bool flag = false;
		if ((base.syncVarDirtyBits & 1U) != 0U)
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
			SyncListUInt.WriteInstance(writer, this.m_dirtyFightingActorIndexList);
		}
		if ((base.syncVarDirtyBits & 2U) != 0U)
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
				flag = true;
			}
			SyncListUInt.WriteInstance(writer, this.m_dirtyFightingDamageList);
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
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(Claymore_SyncComponent.OnDeserialize(NetworkReader, bool)).MethodHandle;
			}
			SyncListUInt.ReadReference(reader, this.m_dirtyFightingActorIndexList);
			SyncListUInt.ReadReference(reader, this.m_dirtyFightingDamageList);
			return;
		}
		int num = (int)reader.ReadPackedUInt32();
		if ((num & 1) != 0)
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
			SyncListUInt.ReadReference(reader, this.m_dirtyFightingActorIndexList);
		}
		if ((num & 2) != 0)
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
			SyncListUInt.ReadReference(reader, this.m_dirtyFightingDamageList);
		}
	}
}

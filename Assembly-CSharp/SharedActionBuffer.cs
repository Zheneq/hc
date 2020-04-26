using System.Runtime.InteropServices;
using UnityEngine.Networking;

public class SharedActionBuffer : NetworkBehaviour
{
	[SyncVar(hook = "HookSetActionPhase")]
	private ActionBufferPhase m_actionPhase;

	[SyncVar(hook = "HookSetAbilityPhase")]
	private AbilityPriority m_abilityPhase;

	public ActionBufferPhase Networkm_actionPhase
	{
		get
		{
			return m_actionPhase;
		}
		[param: In]
		set
		{
			ref ActionBufferPhase actionPhase = ref m_actionPhase;
			if (NetworkServer.localClientActive)
			{
				if (!base.syncVarHookGuard)
				{
					base.syncVarHookGuard = true;
					HookSetActionPhase(value);
					base.syncVarHookGuard = false;
				}
			}
			SetSyncVar(value, ref actionPhase, 1u);
		}
	}

	public AbilityPriority Networkm_abilityPhase
	{
		get
		{
			return m_abilityPhase;
		}
		[param: In]
		set
		{
			ref AbilityPriority abilityPhase = ref m_abilityPhase;
			if (NetworkServer.localClientActive)
			{
				if (!base.syncVarHookGuard)
				{
					base.syncVarHookGuard = true;
					HookSetAbilityPhase(value);
					base.syncVarHookGuard = false;
				}
			}
			SetSyncVar(value, ref abilityPhase, 2u);
		}
	}

	private void HookSetActionPhase(ActionBufferPhase value)
	{
		Networkm_actionPhase = value;
		SynchronizeClientData();
	}

	private void HookSetAbilityPhase(AbilityPriority value)
	{
		Networkm_abilityPhase = value;
		SynchronizeClientData();
	}

	private void SynchronizeClientData()
	{
		if (ClientActionBuffer.Get() != null)
		{
			ClientActionBuffer.Get().SetDataFromShared(m_actionPhase, m_abilityPhase);
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
				switch (1)
				{
				case 0:
					break;
				default:
					writer.Write((int)m_actionPhase);
					writer.Write((int)m_abilityPhase);
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
			writer.Write((int)m_actionPhase);
		}
		if ((base.syncVarDirtyBits & 2) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.Write((int)m_abilityPhase);
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
				switch (6)
				{
				case 0:
					break;
				default:
					m_actionPhase = (ActionBufferPhase)reader.ReadInt32();
					m_abilityPhase = (AbilityPriority)reader.ReadInt32();
					return;
				}
			}
		}
		int num = (int)reader.ReadPackedUInt32();
		if ((num & 1) != 0)
		{
			HookSetActionPhase((ActionBufferPhase)reader.ReadInt32());
		}
		if ((num & 2) != 0)
		{
			HookSetAbilityPhase((AbilityPriority)reader.ReadInt32());
		}
	}
}

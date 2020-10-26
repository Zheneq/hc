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
			if (NetworkServer.localClientActive && !syncVarHookGuard)
			{
				syncVarHookGuard = true;
				HookSetActionPhase(value);
				syncVarHookGuard = false;
			}
			SetSyncVar(value, ref m_actionPhase, 1u);
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
			if (NetworkServer.localClientActive && !syncVarHookGuard)
			{
				syncVarHookGuard = true;
				HookSetAbilityPhase(value);
				syncVarHookGuard = false;
			}
			SetSyncVar(value, ref m_abilityPhase, 2u);
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
			writer.Write((int)m_actionPhase);
			writer.Write((int)m_abilityPhase);
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
			writer.Write((int)m_actionPhase);
		}
		if ((syncVarDirtyBits & 2) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			writer.Write((int)m_abilityPhase);
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
			m_actionPhase = (ActionBufferPhase)reader.ReadInt32();
			m_abilityPhase = (AbilityPriority)reader.ReadInt32();
			LogJson();
			return;
		}
		int dirtyBits = (int)reader.ReadPackedUInt32();
		if ((dirtyBits & 1) != 0)
		{
			HookSetActionPhase((ActionBufferPhase)reader.ReadInt32());
		}
		if ((dirtyBits & 2) != 0)
		{
			HookSetAbilityPhase((AbilityPriority)reader.ReadInt32());
		}
		LogJson(num);
	}

	private void LogJson(int mask = System.Int32.MaxValue)
	{
		var jsonLog = new System.Collections.Generic.List<string>();
		if ((mask & 1) != 0)
		{
			jsonLog.Add($"\"actionPhase\":\"{Networkm_actionPhase}\"");
		}
		if ((mask & 2) != 0)
		{
			jsonLog.Add($"\"abilityPhase\":\"{Networkm_abilityPhase}\"");
		}

		Log.Info($"[JSON] {{\"sharedActionBuffer\":{{{System.String.Join(",", jsonLog.ToArray())}}}}}");
	}
}

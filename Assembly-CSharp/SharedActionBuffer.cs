using System;
using System.Runtime.InteropServices;
using UnityEngine.Networking;

public class SharedActionBuffer : NetworkBehaviour
{
	[SyncVar(hook = "HookSetActionPhase")]
	private ActionBufferPhase m_actionPhase;

	[SyncVar(hook = "HookSetAbilityPhase")]
	private AbilityPriority m_abilityPhase;

	private void HookSetActionPhase(ActionBufferPhase value)
	{
		this.Networkm_actionPhase = value;
		this.SynchronizeClientData();
	}

	private void HookSetAbilityPhase(AbilityPriority value)
	{
		this.Networkm_abilityPhase = value;
		this.SynchronizeClientData();
	}

	private void SynchronizeClientData()
	{
		if (ClientActionBuffer.Get() != null)
		{
			ClientActionBuffer.Get().SetDataFromShared(this.m_actionPhase, this.m_abilityPhase);
		}
	}

	private void UNetVersion()
	{
	}

	public ActionBufferPhase Networkm_actionPhase
	{
		get
		{
			return this.m_actionPhase;
		}
		[param: In]
		set
		{
			uint dirtyBit = 1U;
			if (NetworkServer.localClientActive)
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
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(SharedActionBuffer.set_Networkm_actionPhase(ActionBufferPhase)).MethodHandle;
				}
				if (!base.syncVarHookGuard)
				{
					base.syncVarHookGuard = true;
					this.HookSetActionPhase(value);
					base.syncVarHookGuard = false;
				}
			}
			base.SetSyncVar<ActionBufferPhase>(value, ref this.m_actionPhase, dirtyBit);
		}
	}

	public AbilityPriority Networkm_abilityPhase
	{
		get
		{
			return this.m_abilityPhase;
		}
		[param: In]
		set
		{
			uint dirtyBit = 2U;
			if (NetworkServer.localClientActive)
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
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(SharedActionBuffer.set_Networkm_abilityPhase(AbilityPriority)).MethodHandle;
				}
				if (!base.syncVarHookGuard)
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
					base.syncVarHookGuard = true;
					this.HookSetAbilityPhase(value);
					base.syncVarHookGuard = false;
				}
			}
			base.SetSyncVar<AbilityPriority>(value, ref this.m_abilityPhase, dirtyBit);
		}
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		if (forceAll)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SharedActionBuffer.OnSerialize(NetworkWriter, bool)).MethodHandle;
			}
			writer.Write((int)this.m_actionPhase);
			writer.Write((int)this.m_abilityPhase);
			return true;
		}
		bool flag = false;
		if ((base.syncVarDirtyBits & 1U) != 0U)
		{
			if (!flag)
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
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.Write((int)this.m_actionPhase);
		}
		if ((base.syncVarDirtyBits & 2U) != 0U)
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
			if (!flag)
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
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.Write((int)this.m_abilityPhase);
		}
		if (!flag)
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
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SharedActionBuffer.OnDeserialize(NetworkReader, bool)).MethodHandle;
			}
			this.m_actionPhase = (ActionBufferPhase)reader.ReadInt32();
			this.m_abilityPhase = (AbilityPriority)reader.ReadInt32();
			return;
		}
		int num = (int)reader.ReadPackedUInt32();
		if ((num & 1) != 0)
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
			this.HookSetActionPhase((ActionBufferPhase)reader.ReadInt32());
		}
		if ((num & 2) != 0)
		{
			this.HookSetAbilityPhase((AbilityPriority)reader.ReadInt32());
		}
	}
}

using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Networking;

public class Dino_SyncComponent : NetworkBehaviour
{
	[SyncVar]
	internal int m_dashOrShieldLastCastTurn = -1;

	[SyncVar]
	internal bool m_dashOrShieldInReadyStance;

	[SyncVar]
	internal short m_layerConePowerLevel;

	private ActorData m_actor;

	private DinoDashOrShield m_dashOrShieldAbility;

	private static readonly int s_aHashIdleType = Animator.StringToHash("IdleType");

	private static readonly int s_aHashForceIdle = Animator.StringToHash("ForceIdle");

	private static int kRpcRpcResetDashOrShieldTargeter = 0x4135406C;

	private static int kRpcRpcSetDashReadyStanceAnimParams;

	static Dino_SyncComponent()
	{
		NetworkBehaviour.RegisterRpcDelegate(typeof(Dino_SyncComponent), Dino_SyncComponent.kRpcRpcResetDashOrShieldTargeter, new NetworkBehaviour.CmdDelegate(Dino_SyncComponent.InvokeRpcRpcResetDashOrShieldTargeter));
		Dino_SyncComponent.kRpcRpcSetDashReadyStanceAnimParams = 0x1936AC35;
		NetworkBehaviour.RegisterRpcDelegate(typeof(Dino_SyncComponent), Dino_SyncComponent.kRpcRpcSetDashReadyStanceAnimParams, new NetworkBehaviour.CmdDelegate(Dino_SyncComponent.InvokeRpcRpcSetDashReadyStanceAnimParams));
		NetworkCRC.RegisterBehaviour("Dino_SyncComponent", 0);
	}

	private void Start()
	{
		this.m_actor = base.GetComponent<ActorData>();
	}

	[ClientRpc]
	public void RpcResetDashOrShieldTargeter(bool inReadyStance)
	{
		if (!NetworkServer.active)
		{
			this.Networkm_dashOrShieldInReadyStance = inReadyStance;
			this.ResetDashOrShieldTargeter();
		}
	}

	public void ResetDashOrShieldTargeter()
	{
		if (this.m_dashOrShieldAbility == null)
		{
			this.m_dashOrShieldAbility = (base.GetComponent<AbilityData>().GetAbilityOfType(typeof(DinoDashOrShield)) as DinoDashOrShield);
		}
		if (this.m_dashOrShieldAbility != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Dino_SyncComponent.ResetDashOrShieldTargeter()).MethodHandle;
			}
			this.m_dashOrShieldAbility.ResetTargetersForStanceChange();
		}
	}

	[ClientRpc]
	public void RpcSetDashReadyStanceAnimParams(int idleType, bool forceIdle)
	{
		this.SetDashReadyStanceAnimParams(idleType, forceIdle);
	}

	public void SetDashReadyStanceAnimParams(int idleType, bool forceIdle)
	{
		Animator modelAnimator = this.m_actor.GetModelAnimator();
		if (NetworkClient.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Dino_SyncComponent.SetDashReadyStanceAnimParams(int, bool)).MethodHandle;
			}
			if (modelAnimator != null)
			{
				modelAnimator.SetInteger(Dino_SyncComponent.s_aHashIdleType, idleType);
				if (forceIdle)
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
					modelAnimator.SetTrigger(Dino_SyncComponent.s_aHashForceIdle);
				}
			}
		}
	}

	private void UNetVersion()
	{
	}

	public int Networkm_dashOrShieldLastCastTurn
	{
		get
		{
			return this.m_dashOrShieldLastCastTurn;
		}
		[param: In]
		set
		{
			base.SetSyncVar<int>(value, ref this.m_dashOrShieldLastCastTurn, 1U);
		}
	}

	public bool Networkm_dashOrShieldInReadyStance
	{
		get
		{
			return this.m_dashOrShieldInReadyStance;
		}
		[param: In]
		set
		{
			base.SetSyncVar<bool>(value, ref this.m_dashOrShieldInReadyStance, 2U);
		}
	}

	public short Networkm_layerConePowerLevel
	{
		get
		{
			return this.m_layerConePowerLevel;
		}
		[param: In]
		set
		{
			base.SetSyncVar<short>(value, ref this.m_layerConePowerLevel, 4U);
		}
	}

	protected static void InvokeRpcRpcResetDashOrShieldTargeter(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(Dino_SyncComponent.InvokeRpcRpcResetDashOrShieldTargeter(NetworkBehaviour, NetworkReader)).MethodHandle;
			}
			Debug.LogError("RPC RpcResetDashOrShieldTargeter called on server.");
			return;
		}
		((Dino_SyncComponent)obj).RpcResetDashOrShieldTargeter(reader.ReadBoolean());
	}

	protected static void InvokeRpcRpcSetDashReadyStanceAnimParams(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Dino_SyncComponent.InvokeRpcRpcSetDashReadyStanceAnimParams(NetworkBehaviour, NetworkReader)).MethodHandle;
			}
			Debug.LogError("RPC RpcSetDashReadyStanceAnimParams called on server.");
			return;
		}
		((Dino_SyncComponent)obj).RpcSetDashReadyStanceAnimParams((int)reader.ReadPackedUInt32(), reader.ReadBoolean());
	}

	public void CallRpcResetDashOrShieldTargeter(bool inReadyStance)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("RPC Function RpcResetDashOrShieldTargeter called on client.");
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write(0);
		networkWriter.Write((short)((ushort)2));
		networkWriter.WritePackedUInt32((uint)Dino_SyncComponent.kRpcRpcResetDashOrShieldTargeter);
		networkWriter.Write(base.GetComponent<NetworkIdentity>().netId);
		networkWriter.Write(inReadyStance);
		this.SendRPCInternal(networkWriter, 0, "RpcResetDashOrShieldTargeter");
	}

	public void CallRpcSetDashReadyStanceAnimParams(int idleType, bool forceIdle)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("RPC Function RpcSetDashReadyStanceAnimParams called on client.");
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write(0);
		networkWriter.Write((short)((ushort)2));
		networkWriter.WritePackedUInt32((uint)Dino_SyncComponent.kRpcRpcSetDashReadyStanceAnimParams);
		networkWriter.Write(base.GetComponent<NetworkIdentity>().netId);
		networkWriter.WritePackedUInt32((uint)idleType);
		networkWriter.Write(forceIdle);
		this.SendRPCInternal(networkWriter, 0, "RpcSetDashReadyStanceAnimParams");
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		if (forceAll)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(Dino_SyncComponent.OnSerialize(NetworkWriter, bool)).MethodHandle;
			}
			writer.WritePackedUInt32((uint)this.m_dashOrShieldLastCastTurn);
			writer.Write(this.m_dashOrShieldInReadyStance);
			writer.WritePackedUInt32((uint)this.m_layerConePowerLevel);
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
			writer.WritePackedUInt32((uint)this.m_dashOrShieldLastCastTurn);
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
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.Write(this.m_dashOrShieldInReadyStance);
		}
		if ((base.syncVarDirtyBits & 4U) != 0U)
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
			if (!flag)
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
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)this.m_layerConePowerLevel);
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Dino_SyncComponent.OnDeserialize(NetworkReader, bool)).MethodHandle;
			}
			this.m_dashOrShieldLastCastTurn = (int)reader.ReadPackedUInt32();
			this.m_dashOrShieldInReadyStance = reader.ReadBoolean();
			this.m_layerConePowerLevel = (short)reader.ReadPackedUInt32();
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
			this.m_dashOrShieldLastCastTurn = (int)reader.ReadPackedUInt32();
		}
		if ((num & 2) != 0)
		{
			this.m_dashOrShieldInReadyStance = reader.ReadBoolean();
		}
		if ((num & 4) != 0)
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
			this.m_layerConePowerLevel = (short)reader.ReadPackedUInt32();
		}
	}
}

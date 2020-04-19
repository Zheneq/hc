using System;
using UnityEngine;
using UnityEngine.Networking;

public class Rampart_SyncComponent : NetworkBehaviour
{
	private BoxCollider m_colliderForShield;

	private static readonly int animIdleType = Animator.StringToHash("IdleType");

	private static readonly int animForceIdle = Animator.StringToHash("ForceIdle");

	private ActorData m_owner;

	private static int kRpcRpcSetIdleType = -0x48504D7A;

	private static int kRpcRpcSetFacingDirection;

	static Rampart_SyncComponent()
	{
		NetworkBehaviour.RegisterRpcDelegate(typeof(Rampart_SyncComponent), Rampart_SyncComponent.kRpcRpcSetIdleType, new NetworkBehaviour.CmdDelegate(Rampart_SyncComponent.InvokeRpcRpcSetIdleType));
		Rampart_SyncComponent.kRpcRpcSetFacingDirection = 0x65DC606D;
		NetworkBehaviour.RegisterRpcDelegate(typeof(Rampart_SyncComponent), Rampart_SyncComponent.kRpcRpcSetFacingDirection, new NetworkBehaviour.CmdDelegate(Rampart_SyncComponent.InvokeRpcRpcSetFacingDirection));
		NetworkCRC.RegisterBehaviour("Rampart_SyncComponent", 0);
	}

	private void Start()
	{
		ActorData owner = this.GetOwner();
		if (owner != null && owner.\u000E() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Rampart_SyncComponent.Start()).MethodHandle;
			}
			this.m_colliderForShield = owner.\u000E().gameObject.AddComponent<BoxCollider>();
			this.m_colliderForShield.center = new Vector3(0f, 0.8f, 1f);
			this.m_colliderForShield.size = new Vector3(3.1f * Board.\u000E().squareSize, 3f, 0.3f);
			this.m_colliderForShield.enabled = false;
		}
	}

	private ActorData GetOwner()
	{
		if (this.m_owner == null)
		{
			this.m_owner = base.GetComponent<ActorData>();
		}
		return this.m_owner;
	}

	[ClientRpc]
	public void RpcSetIdleType(int idleType)
	{
		if (!NetworkServer.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Rampart_SyncComponent.RpcSetIdleType(int)).MethodHandle;
			}
			ActorData owner = this.GetOwner();
			if (owner != null && owner.\u000E() != null)
			{
				int integer = owner.\u000E().GetInteger(Rampart_SyncComponent.animIdleType);
				if (integer != idleType)
				{
					owner.\u000E().SetInteger(Rampart_SyncComponent.animIdleType, idleType);
					if (idleType != 0)
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
						owner.\u000E().SetTrigger(Rampart_SyncComponent.animForceIdle);
					}
				}
			}
		}
		if (NetworkClient.active && this.m_colliderForShield != null)
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
			this.m_colliderForShield.enabled = (idleType != 0);
		}
	}

	[ClientRpc]
	public void RpcSetFacingDirection(Vector3 facing)
	{
		if (!NetworkServer.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Rampart_SyncComponent.RpcSetFacingDirection(Vector3)).MethodHandle;
			}
			ActorData owner = this.GetOwner();
			if (owner != null)
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
				owner.TurnToDirection(facing);
			}
		}
	}

	private void UNetVersion()
	{
	}

	protected static void InvokeRpcRpcSetIdleType(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Rampart_SyncComponent.InvokeRpcRpcSetIdleType(NetworkBehaviour, NetworkReader)).MethodHandle;
			}
			Debug.LogError("RPC RpcSetIdleType called on server.");
			return;
		}
		((Rampart_SyncComponent)obj).RpcSetIdleType((int)reader.ReadPackedUInt32());
	}

	protected static void InvokeRpcRpcSetFacingDirection(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcSetFacingDirection called on server.");
			return;
		}
		((Rampart_SyncComponent)obj).RpcSetFacingDirection(reader.ReadVector3());
	}

	public void CallRpcSetIdleType(int idleType)
	{
		if (!NetworkServer.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Rampart_SyncComponent.CallRpcSetIdleType(int)).MethodHandle;
			}
			Debug.LogError("RPC Function RpcSetIdleType called on client.");
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write(0);
		networkWriter.Write((short)((ushort)2));
		networkWriter.WritePackedUInt32((uint)Rampart_SyncComponent.kRpcRpcSetIdleType);
		networkWriter.Write(base.GetComponent<NetworkIdentity>().netId);
		networkWriter.WritePackedUInt32((uint)idleType);
		this.SendRPCInternal(networkWriter, 0, "RpcSetIdleType");
	}

	public void CallRpcSetFacingDirection(Vector3 facing)
	{
		if (!NetworkServer.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Rampart_SyncComponent.CallRpcSetFacingDirection(Vector3)).MethodHandle;
			}
			Debug.LogError("RPC Function RpcSetFacingDirection called on client.");
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write(0);
		networkWriter.Write((short)((ushort)2));
		networkWriter.WritePackedUInt32((uint)Rampart_SyncComponent.kRpcRpcSetFacingDirection);
		networkWriter.Write(base.GetComponent<NetworkIdentity>().netId);
		networkWriter.Write(facing);
		this.SendRPCInternal(networkWriter, 0, "RpcSetFacingDirection");
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		bool result;
		return result;
	}

	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
	}
}

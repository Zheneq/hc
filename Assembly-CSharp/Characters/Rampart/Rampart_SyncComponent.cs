// ROGUES
// SERVER
using UnityEngine;
using UnityEngine.Networking;

public class Rampart_SyncComponent : NetworkBehaviour
{
	private BoxCollider m_colliderForShield;
	private ActorData m_owner;

	// removed in rogues
	private static readonly int animIdleType = Animator.StringToHash("IdleType");
	private static readonly int animForceIdle = Animator.StringToHash("ForceIdle");
	private static int kRpcRpcSetIdleType = -1213222266;
	private static int kRpcRpcSetFacingDirection = 1708941421;
	// end removed in rogues

	static Rampart_SyncComponent()
	{
		// reactor
		RegisterRpcDelegate(typeof(Rampart_SyncComponent), kRpcRpcSetIdleType, InvokeRpcRpcSetIdleType);
		RegisterRpcDelegate(typeof(Rampart_SyncComponent), kRpcRpcSetFacingDirection, InvokeRpcRpcSetFacingDirection);
		// rogues
		// RegisterRpcDelegate(typeof(Rampart_SyncComponent), "RpcSetIdleType", new CmdDelegate(InvokeRpcRpcSetIdleType));
		// RegisterRpcDelegate(typeof(Rampart_SyncComponent), "RpcSetFacingDirection", new CmdDelegate(InvokeRpcRpcSetFacingDirection));
		
		// removed in rogues
		NetworkCRC.RegisterBehaviour("Rampart_SyncComponent", 0);
	}

	private void Start()
	{
		ActorData owner = GetOwner();
		if (owner != null && owner.GetActorModelData() != null)
		{
			m_colliderForShield = owner.GetActorModelData().gameObject.AddComponent<BoxCollider>();
			m_colliderForShield.center = new Vector3(0f, 0.8f, 1f);
			m_colliderForShield.size = new Vector3(3.1f * Board.Get().squareSize, 3f, 0.3f);
			m_colliderForShield.enabled = false;
		}
	}

	private ActorData GetOwner()
	{
		if (m_owner == null)
		{
			m_owner = GetComponent<ActorData>();
		}
		return m_owner;
	}

	[ClientRpc]
	public void RpcSetIdleType(int idleType)
	{
		if (!NetworkServer.active)
		{
			ActorData owner = GetOwner();
			if (owner != null && owner.GetModelAnimator() != null)
			{
				int currentIdleType = owner.GetModelAnimator().GetInteger(animIdleType);
				if (currentIdleType != idleType)
				{
					owner.GetModelAnimator().SetInteger(animIdleType, idleType);
					if (idleType != 0)
					{
						owner.GetModelAnimator().SetTrigger(animForceIdle);
					}
				}
			}
		}
		if (NetworkClient.active && m_colliderForShield != null)
		{
			m_colliderForShield.enabled = (idleType != 0);
		}
	}

	[ClientRpc]
	public void RpcSetFacingDirection(Vector3 facing)
	{
		if (!NetworkServer.active)
		{
			ActorData owner = GetOwner();
			if (owner != null)
			{
				owner.TurnToDirection(facing);
			}
		}
	}

	private void UNetVersion()  // MirrorProcessed in rogues
	{
	}

	protected static void InvokeRpcRpcSetIdleType(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcSetIdleType called on server.");
			return;
		}
		((Rampart_SyncComponent)obj).RpcSetIdleType((int)reader.ReadPackedUInt32());  // ReadPackedInt32 in rogues
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
		// reactor
		if (!NetworkServer.active)
		{
			Debug.LogError("RPC Function RpcSetIdleType called on client.");
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)2);
		networkWriter.WritePackedUInt32((uint)kRpcRpcSetIdleType);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		networkWriter.WritePackedUInt32((uint)idleType);
		SendRPCInternal(networkWriter, 0, "RpcSetIdleType");
		// rogues
		// NetworkWriter networkWriter = new NetworkWriter();
		// networkWriter.WritePackedInt32(idleType);
		// this.SendRPCInternal(typeof(Rampart_SyncComponent), "RpcSetIdleType", networkWriter, 0);
	}

	public void CallRpcSetFacingDirection(Vector3 facing)
	{
		// reactor
		if (!NetworkServer.active)
		{
			Debug.LogError("RPC Function RpcSetFacingDirection called on client.");
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)2);
		networkWriter.WritePackedUInt32((uint)kRpcRpcSetFacingDirection);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		networkWriter.Write(facing);
		SendRPCInternal(networkWriter, 0, "RpcSetFacingDirection");
		// rogues
		// NetworkWriter networkWriter = new NetworkWriter();
		// networkWriter.Write(facing);
		// this.SendRPCInternal(typeof(Rampart_SyncComponent), "RpcSetFacingDirection", networkWriter, 0);
	}

	// removed in rogues
	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		return false;
	}

	// removed in rogues
	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
	}
}

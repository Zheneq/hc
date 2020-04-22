using UnityEngine;
using UnityEngine.Networking;

public class Rampart_SyncComponent : NetworkBehaviour
{
	private BoxCollider m_colliderForShield;

	private static readonly int animIdleType;

	private static readonly int animForceIdle;

	private ActorData m_owner;

	private static int kRpcRpcSetIdleType;

	private static int kRpcRpcSetFacingDirection;

	static Rampart_SyncComponent()
	{
		animIdleType = Animator.StringToHash("IdleType");
		animForceIdle = Animator.StringToHash("ForceIdle");
		kRpcRpcSetIdleType = -1213222266;
		NetworkBehaviour.RegisterRpcDelegate(typeof(Rampart_SyncComponent), kRpcRpcSetIdleType, InvokeRpcRpcSetIdleType);
		kRpcRpcSetFacingDirection = 1708941421;
		NetworkBehaviour.RegisterRpcDelegate(typeof(Rampart_SyncComponent), kRpcRpcSetFacingDirection, InvokeRpcRpcSetFacingDirection);
		NetworkCRC.RegisterBehaviour("Rampart_SyncComponent", 0);
	}

	private void Start()
	{
		ActorData owner = GetOwner();
		if (!(owner != null) || !(owner.GetActorModelData() != null))
		{
			return;
		}
		while (true)
		{
			m_colliderForShield = owner.GetActorModelData().gameObject.AddComponent<BoxCollider>();
			m_colliderForShield.center = new Vector3(0f, 0.8f, 1f);
			m_colliderForShield.size = new Vector3(3.1f * Board.Get().squareSize, 3f, 0.3f);
			m_colliderForShield.enabled = false;
			return;
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
				int integer = owner.GetModelAnimator().GetInteger(animIdleType);
				if (integer != idleType)
				{
					owner.GetModelAnimator().SetInteger(animIdleType, idleType);
					if (idleType != 0)
					{
						owner.GetModelAnimator().SetTrigger(animForceIdle);
					}
				}
			}
		}
		if (!NetworkClient.active || !(m_colliderForShield != null))
		{
			return;
		}
		while (true)
		{
			m_colliderForShield.enabled = (idleType != 0);
			return;
		}
	}

	[ClientRpc]
	public void RpcSetFacingDirection(Vector3 facing)
	{
		if (NetworkServer.active)
		{
			return;
		}
		while (true)
		{
			ActorData owner = GetOwner();
			if (owner != null)
			{
				while (true)
				{
					owner.TurnToDirection(facing);
					return;
				}
			}
			return;
		}
	}

	private void UNetVersion()
	{
	}

	protected static void InvokeRpcRpcSetIdleType(NetworkBehaviour obj, NetworkReader reader)
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
					Debug.LogError("RPC RpcSetIdleType called on server.");
					return;
				}
			}
		}
		((Rampart_SyncComponent)obj).RpcSetIdleType((int)reader.ReadPackedUInt32());
	}

	protected static void InvokeRpcRpcSetFacingDirection(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcSetFacingDirection called on server.");
		}
		else
		{
			((Rampart_SyncComponent)obj).RpcSetFacingDirection(reader.ReadVector3());
		}
	}

	public void CallRpcSetIdleType(int idleType)
	{
		if (!NetworkServer.active)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					Debug.LogError("RPC Function RpcSetIdleType called on client.");
					return;
				}
			}
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)2);
		networkWriter.WritePackedUInt32((uint)kRpcRpcSetIdleType);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		networkWriter.WritePackedUInt32((uint)idleType);
		SendRPCInternal(networkWriter, 0, "RpcSetIdleType");
	}

	public void CallRpcSetFacingDirection(Vector3 facing)
	{
		if (!NetworkServer.active)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					Debug.LogError("RPC Function RpcSetFacingDirection called on client.");
					return;
				}
			}
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)2);
		networkWriter.WritePackedUInt32((uint)kRpcRpcSetFacingDirection);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		networkWriter.Write(facing);
		SendRPCInternal(networkWriter, 0, "RpcSetFacingDirection");
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		bool result = default(bool);
		return result;
	}

	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
	}
}

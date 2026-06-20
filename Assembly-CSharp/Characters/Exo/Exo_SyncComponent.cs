// ROGUES
// SERVER
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Networking;

public class Exo_SyncComponent : NetworkBehaviour
{
	[SyncVar]
	internal bool m_anchored;
	[SyncVar(hook = "HookSetWasAnchoredOnTurnStart")]
	internal bool m_wasAnchoredOnTurnStart;
	[SyncVar]
	internal bool m_laserBarrierIsUp;
	[SyncVar]
	internal Vector3 m_anchoredLaserAimDirection;
	[SyncVar]
	internal short m_turnsAnchored;
	[SyncVar]
	internal short m_lastBasicAttackUsedTurn = -1;

	private ExoAnchorLaser m_anchorLaserAbility;
	
	// removed in rogues
	private static readonly int animIdleType = Animator.StringToHash("IdleType");
	private static readonly int animExitAnchor = Animator.StringToHash("ExitAnchor");
	// end removed in rogues
	
	private ActorData m_owner;
	
	// removed in rogues
	private static int kRpcRpcSetIdleType = 497885915;
	private static int kRpcRpcSetFacingDirection = -1163602312;
	private static int kRpcRpcSetSweepingRight = 162886841;
	// end removed in rogues

	public bool Networkm_anchored
	{
		get => m_anchored;
		[param: In]
		set => SetSyncVar(value, ref m_anchored, 1u);
	}

	public bool Networkm_wasAnchoredOnTurnStart
	{
		get => m_wasAnchoredOnTurnStart;
		[param: In]
		set
		{
			if (NetworkServer.localClientActive && !syncVarHookGuard)
			{
				syncVarHookGuard = true;
				HookSetWasAnchoredOnTurnStart(value);
				syncVarHookGuard = false;
			}
			SetSyncVar(value, ref m_wasAnchoredOnTurnStart, 2u);
		}
	}

	public bool Networkm_laserBarrierIsUp
	{
		get => m_laserBarrierIsUp;
		[param: In]
		set => SetSyncVar(value, ref m_laserBarrierIsUp, 4u);
	}

	public Vector3 Networkm_anchoredLaserAimDirection
	{
		get => m_anchoredLaserAimDirection;
		[param: In]
		set => SetSyncVar(value, ref m_anchoredLaserAimDirection, 8u);
	}

	public short Networkm_turnsAnchored
	{
		get => m_turnsAnchored;
		[param: In]
		set => SetSyncVar(value, ref m_turnsAnchored, 16u);
	}

	public short Networkm_lastBasicAttackUsedTurn
	{
		get => m_lastBasicAttackUsedTurn;
		[param: In]
		set => SetSyncVar(value, ref m_lastBasicAttackUsedTurn, 32u);
	}

	static Exo_SyncComponent()
	{
		// reactor
		RegisterRpcDelegate(typeof(Exo_SyncComponent), kRpcRpcSetIdleType, InvokeRpcRpcSetIdleType);
		RegisterRpcDelegate(typeof(Exo_SyncComponent), kRpcRpcSetFacingDirection, InvokeRpcRpcSetFacingDirection);
		RegisterRpcDelegate(typeof(Exo_SyncComponent), kRpcRpcSetSweepingRight, InvokeRpcRpcSetSweepingRight);
		NetworkCRC.RegisterBehaviour("Exo_SyncComponent", 0);
		// rogues
		// RegisterRpcDelegate(typeof(Exo_SyncComponent), "RpcSetIdleType", new CmdDelegate(InvokeRpcRpcSetIdleType));
		// RegisterRpcDelegate(typeof(Exo_SyncComponent), "RpcSetFacingDirection", new CmdDelegate(InvokeRpcRpcSetFacingDirection));
		// RegisterRpcDelegate(typeof(Exo_SyncComponent), "RpcSetSweepingRight", new CmdDelegate(InvokeRpcRpcSetSweepingRight));
		// RegisterRpcDelegate(typeof(Exo_SyncComponent), "RpcClearExitAnchorTrigger", new CmdDelegate(InvokeRpcRpcClearExitAnchorTrigger));
	}

	private ActorData GetOwner()
	{
		if (m_owner == null)
		{
			m_owner = GetComponent<ActorData>();
		}
		return m_owner;
	}

	private void Start()
	{
		AbilityData abilityData = GetComponent<AbilityData>();
		if (abilityData != null)
		{
			m_anchorLaserAbility = abilityData.GetAbilityOfType(typeof(ExoAnchorLaser)) as ExoAnchorLaser;
		}
	}

	public bool UsedBasicAttackLastTurn()
	{
		return GameFlowData.Get() != null
		       && m_lastBasicAttackUsedTurn > 0
		       && GameFlowData.Get().CurrentTurn - m_lastBasicAttackUsedTurn == 1;
	}

	private void HookSetWasAnchoredOnTurnStart(bool value)
	{
		Networkm_wasAnchoredOnTurnStart = value;
		if (NetworkClient.active
		    && !NetworkServer.active
		    && m_anchorLaserAbility != null
		    && m_anchorLaserAbility.ShouldUpdateMovementOnAnchorChange())
		{
			GetOwner().GetActorMovement().UpdateSquaresCanMoveTo();
		}
	}

	[ClientRpc]
	public void RpcSetIdleType(int idleType)
	{
		if (NetworkServer.active)
		{
			return;
		}
		ActorData owner = GetOwner();
		if (owner != null && owner.GetModelAnimator() != null)
		{
			if (owner.GetModelAnimator().GetInteger(animIdleType) != idleType)
			{
				owner.GetModelAnimator().SetInteger(animIdleType, idleType);
				if (idleType == 0)
				{
					owner.GetModelAnimator().SetTrigger(animExitAnchor);
				}
			}
		}
	}

	[ClientRpc]
	public void RpcSetFacingDirection(Vector3 facing)
	{
		if (NetworkServer.active)
		{
			return;
		}
		ActorData owner = GetOwner();
		if (owner != null)
		{
			owner.TurnToDirection(facing);
		}
	}

	[ClientRpc]
	public void RpcSetSweepingRight(bool sweepingToTheRight)
	{
		if (NetworkServer.active)
		{
			return;
		}
		ActorData owner = GetOwner();
		if (owner != null && owner.GetModelAnimator() != null)
		{
			owner.GetModelAnimator().SetBool("SweepingRight", sweepingToTheRight);
		}
	}

	// rogues
	// [ClientRpc]
	// public void RpcClearExitAnchorTrigger()
	// {
	// 	if (NetworkServer.active)
	// 	{
	// 		return;
	// 	}
	// 	ActorData owner = GetOwner();
	// 	if (owner != null && owner.GetModelAnimator() != null)
	// 	{
	// 		owner.GetModelAnimator().ResetTrigger("ExitAnchor");
	// 	}
	// }

	// reactor
	private void UNetVersion() // MirrorProcessed in rogues
	{
	}

	protected static void InvokeRpcRpcSetIdleType(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcSetIdleType called on server.");
			return;
		}
		((Exo_SyncComponent)obj).RpcSetIdleType((int)reader.ReadPackedUInt32());  // different type in rogues
	}

	protected static void InvokeRpcRpcSetFacingDirection(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcSetFacingDirection called on server.");
			return;
		}
		((Exo_SyncComponent)obj).RpcSetFacingDirection(reader.ReadVector3());
	}

	protected static void InvokeRpcRpcSetSweepingRight(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcSetSweepingRight called on server.");
			return;
		}
		((Exo_SyncComponent)obj).RpcSetSweepingRight(reader.ReadBoolean());
	}

	// rogues
	// protected static void InvokeRpcRpcClearExitAnchorTrigger(NetworkBehaviour obj, NetworkReader reader)
	// {
	// 	if (!NetworkClient.active)
	// 	{
	// 		Debug.LogError("RPC RpcClearExitAnchorTrigger called on server.");
	// 		return;
	// 	}
	// 	((Exo_SyncComponent)obj).RpcClearExitAnchorTrigger();
	// }

	// changed in rogues
	public void CallRpcSetIdleType(int idleType)
	{
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
	}

	// changed in rogues
	public void CallRpcSetFacingDirection(Vector3 facing)
	{
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
	}

	// changed in rogues
	public void CallRpcSetSweepingRight(bool sweepingToTheRight)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("RPC Function RpcSetSweepingRight called on client.");
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)2);
		networkWriter.WritePackedUInt32((uint)kRpcRpcSetSweepingRight);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		networkWriter.Write(sweepingToTheRight);
		SendRPCInternal(networkWriter, 0, "RpcSetSweepingRight");
	}

	// rogues
	// public void CallRpcClearExitAnchorTrigger()
	// {
	// 	NetworkWriter networkWriter = new NetworkWriter();
	// 	this.SendRPCInternal(typeof(Exo_SyncComponent), "RpcClearExitAnchorTrigger", networkWriter, 0);
	// }

	// changed in rogues
	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		if (forceAll)
		{
			writer.Write(m_anchored);
			writer.Write(m_wasAnchoredOnTurnStart);
			writer.Write(m_laserBarrierIsUp);
			writer.Write(m_anchoredLaserAimDirection);
			writer.WritePackedUInt32((uint)m_turnsAnchored);
			writer.WritePackedUInt32((uint)m_lastBasicAttackUsedTurn);
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
			writer.Write(m_anchored);
		}
		if ((syncVarDirtyBits & 2) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			writer.Write(m_wasAnchoredOnTurnStart);
		}
		if ((syncVarDirtyBits & 4) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			writer.Write(m_laserBarrierIsUp);
		}
		if ((syncVarDirtyBits & 8) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			writer.Write(m_anchoredLaserAimDirection);
		}
		if ((syncVarDirtyBits & 0x10) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_turnsAnchored);
		}
		if ((syncVarDirtyBits & 0x20) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_lastBasicAttackUsedTurn);
		}
		if (!flag)
		{
			writer.WritePackedUInt32(syncVarDirtyBits);
		}
		return flag;
	}

	// changed in rogues
	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
		if (initialState)
		{
			m_anchored = reader.ReadBoolean();
			m_wasAnchoredOnTurnStart = reader.ReadBoolean();
			m_laserBarrierIsUp = reader.ReadBoolean();
			m_anchoredLaserAimDirection = reader.ReadVector3();
			m_turnsAnchored = (short)reader.ReadPackedUInt32();
			m_lastBasicAttackUsedTurn = (short)reader.ReadPackedUInt32();
			return;
		}
		int num = (int)reader.ReadPackedUInt32();
		if ((num & 1) != 0)
		{
			m_anchored = reader.ReadBoolean();
		}
		if ((num & 2) != 0)
		{
			HookSetWasAnchoredOnTurnStart(reader.ReadBoolean());
		}
		if ((num & 4) != 0)
		{
			m_laserBarrierIsUp = reader.ReadBoolean();
		}
		if ((num & 8) != 0)
		{
			m_anchoredLaserAimDirection = reader.ReadVector3();
		}
		if ((num & 0x10) != 0)
		{
			m_turnsAnchored = (short)reader.ReadPackedUInt32();
		}
		if ((num & 0x20) != 0)
		{
			m_lastBasicAttackUsedTurn = (short)reader.ReadPackedUInt32();
		}
	}
}

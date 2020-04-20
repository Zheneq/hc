using System;
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
	internal Vector3 m_anchoredLaserAimDirection = default(Vector3);

	[SyncVar]
	internal short m_turnsAnchored;

	[SyncVar]
	internal short m_lastBasicAttackUsedTurn = -1;

	private ExoAnchorLaser m_anchorLaserAbility;

	private static readonly int animIdleType = Animator.StringToHash("IdleType");

	private static readonly int animExitAnchor = Animator.StringToHash("ExitAnchor");

	private ActorData m_owner;

	private static int kRpcRpcSetIdleType = 0x1DAD22DB;

	private static int kRpcRpcSetFacingDirection;

	private static int kRpcRpcSetSweepingRight;

	static Exo_SyncComponent()
	{
		NetworkBehaviour.RegisterRpcDelegate(typeof(Exo_SyncComponent), Exo_SyncComponent.kRpcRpcSetIdleType, new NetworkBehaviour.CmdDelegate(Exo_SyncComponent.InvokeRpcRpcSetIdleType));
		Exo_SyncComponent.kRpcRpcSetFacingDirection = -0x455B2988;
		NetworkBehaviour.RegisterRpcDelegate(typeof(Exo_SyncComponent), Exo_SyncComponent.kRpcRpcSetFacingDirection, new NetworkBehaviour.CmdDelegate(Exo_SyncComponent.InvokeRpcRpcSetFacingDirection));
		Exo_SyncComponent.kRpcRpcSetSweepingRight = 0x9B574B9;
		NetworkBehaviour.RegisterRpcDelegate(typeof(Exo_SyncComponent), Exo_SyncComponent.kRpcRpcSetSweepingRight, new NetworkBehaviour.CmdDelegate(Exo_SyncComponent.InvokeRpcRpcSetSweepingRight));
		NetworkCRC.RegisterBehaviour("Exo_SyncComponent", 0);
	}

	private ActorData GetOwner()
	{
		if (this.m_owner == null)
		{
			this.m_owner = base.GetComponent<ActorData>();
		}
		return this.m_owner;
	}

	private void Start()
	{
		AbilityData component = base.GetComponent<AbilityData>();
		if (component != null)
		{
			this.m_anchorLaserAbility = (component.GetAbilityOfType(typeof(ExoAnchorLaser)) as ExoAnchorLaser);
		}
	}

	public bool UsedBasicAttackLastTurn()
	{
		if (GameFlowData.Get() != null)
		{
			if (this.m_lastBasicAttackUsedTurn > 0)
			{
				return GameFlowData.Get().CurrentTurn - (int)this.m_lastBasicAttackUsedTurn == 1;
			}
		}
		return false;
	}

	private void HookSetWasAnchoredOnTurnStart(bool value)
	{
		this.Networkm_wasAnchoredOnTurnStart = value;
		if (NetworkClient.active)
		{
			if (!NetworkServer.active && this.m_anchorLaserAbility != null && this.m_anchorLaserAbility.ShouldUpdateMovementOnAnchorChange())
			{
				this.GetOwner().GetActorMovement().UpdateSquaresCanMoveTo();
			}
		}
	}

	[ClientRpc]
	public void RpcSetIdleType(int idleType)
	{
		if (!NetworkServer.active)
		{
			ActorData owner = this.GetOwner();
			if (owner != null && owner.GetModelAnimator() != null)
			{
				int integer = owner.GetModelAnimator().GetInteger(Exo_SyncComponent.animIdleType);
				if (integer != idleType)
				{
					owner.GetModelAnimator().SetInteger(Exo_SyncComponent.animIdleType, idleType);
					if (idleType == 0)
					{
						owner.GetModelAnimator().SetTrigger(Exo_SyncComponent.animExitAnchor);
					}
				}
			}
		}
	}

	[ClientRpc]
	public void RpcSetFacingDirection(Vector3 facing)
	{
		if (!NetworkServer.active)
		{
			ActorData owner = this.GetOwner();
			if (owner != null)
			{
				owner.TurnToDirection(facing);
			}
		}
	}

	[ClientRpc]
	public void RpcSetSweepingRight(bool sweepingToTheRight)
	{
		if (!NetworkServer.active)
		{
			ActorData owner = this.GetOwner();
			if (owner != null && owner.GetModelAnimator() != null)
			{
				owner.GetModelAnimator().SetBool("SweepingRight", sweepingToTheRight);
			}
		}
	}

	private void UNetVersion()
	{
	}

	public bool Networkm_anchored
	{
		get
		{
			return this.m_anchored;
		}
		[param: In]
		set
		{
			base.SetSyncVar<bool>(value, ref this.m_anchored, 1U);
		}
	}

	public bool Networkm_wasAnchoredOnTurnStart
	{
		get
		{
			return this.m_wasAnchoredOnTurnStart;
		}
		[param: In]
		set
		{
			uint dirtyBit = 2U;
			if (NetworkServer.localClientActive)
			{
				if (!base.syncVarHookGuard)
				{
					base.syncVarHookGuard = true;
					this.HookSetWasAnchoredOnTurnStart(value);
					base.syncVarHookGuard = false;
				}
			}
			base.SetSyncVar<bool>(value, ref this.m_wasAnchoredOnTurnStart, dirtyBit);
		}
	}

	public bool Networkm_laserBarrierIsUp
	{
		get
		{
			return this.m_laserBarrierIsUp;
		}
		[param: In]
		set
		{
			base.SetSyncVar<bool>(value, ref this.m_laserBarrierIsUp, 4U);
		}
	}

	public Vector3 Networkm_anchoredLaserAimDirection
	{
		get
		{
			return this.m_anchoredLaserAimDirection;
		}
		[param: In]
		set
		{
			base.SetSyncVar<Vector3>(value, ref this.m_anchoredLaserAimDirection, 8U);
		}
	}

	public short Networkm_turnsAnchored
	{
		get
		{
			return this.m_turnsAnchored;
		}
		[param: In]
		set
		{
			base.SetSyncVar<short>(value, ref this.m_turnsAnchored, 0x10U);
		}
	}

	public short Networkm_lastBasicAttackUsedTurn
	{
		get
		{
			return this.m_lastBasicAttackUsedTurn;
		}
		[param: In]
		set
		{
			base.SetSyncVar<short>(value, ref this.m_lastBasicAttackUsedTurn, 0x20U);
		}
	}

	protected static void InvokeRpcRpcSetIdleType(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcSetIdleType called on server.");
			return;
		}
		((Exo_SyncComponent)obj).RpcSetIdleType((int)reader.ReadPackedUInt32());
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

	public void CallRpcSetIdleType(int idleType)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("RPC Function RpcSetIdleType called on client.");
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write(0);
		networkWriter.Write((short)((ushort)2));
		networkWriter.WritePackedUInt32((uint)Exo_SyncComponent.kRpcRpcSetIdleType);
		networkWriter.Write(base.GetComponent<NetworkIdentity>().netId);
		networkWriter.WritePackedUInt32((uint)idleType);
		this.SendRPCInternal(networkWriter, 0, "RpcSetIdleType");
	}

	public void CallRpcSetFacingDirection(Vector3 facing)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("RPC Function RpcSetFacingDirection called on client.");
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write(0);
		networkWriter.Write((short)((ushort)2));
		networkWriter.WritePackedUInt32((uint)Exo_SyncComponent.kRpcRpcSetFacingDirection);
		networkWriter.Write(base.GetComponent<NetworkIdentity>().netId);
		networkWriter.Write(facing);
		this.SendRPCInternal(networkWriter, 0, "RpcSetFacingDirection");
	}

	public void CallRpcSetSweepingRight(bool sweepingToTheRight)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("RPC Function RpcSetSweepingRight called on client.");
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write(0);
		networkWriter.Write((short)((ushort)2));
		networkWriter.WritePackedUInt32((uint)Exo_SyncComponent.kRpcRpcSetSweepingRight);
		networkWriter.Write(base.GetComponent<NetworkIdentity>().netId);
		networkWriter.Write(sweepingToTheRight);
		this.SendRPCInternal(networkWriter, 0, "RpcSetSweepingRight");
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		if (forceAll)
		{
			writer.Write(this.m_anchored);
			writer.Write(this.m_wasAnchoredOnTurnStart);
			writer.Write(this.m_laserBarrierIsUp);
			writer.Write(this.m_anchoredLaserAimDirection);
			writer.WritePackedUInt32((uint)this.m_turnsAnchored);
			writer.WritePackedUInt32((uint)this.m_lastBasicAttackUsedTurn);
			return true;
		}
		bool flag = false;
		if ((base.syncVarDirtyBits & 1U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.Write(this.m_anchored);
		}
		if ((base.syncVarDirtyBits & 2U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.Write(this.m_wasAnchoredOnTurnStart);
		}
		if ((base.syncVarDirtyBits & 4U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.Write(this.m_laserBarrierIsUp);
		}
		if ((base.syncVarDirtyBits & 8U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.Write(this.m_anchoredLaserAimDirection);
		}
		if ((base.syncVarDirtyBits & 0x10U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)this.m_turnsAnchored);
		}
		if ((base.syncVarDirtyBits & 0x20U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)this.m_lastBasicAttackUsedTurn);
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
			this.m_anchored = reader.ReadBoolean();
			this.m_wasAnchoredOnTurnStart = reader.ReadBoolean();
			this.m_laserBarrierIsUp = reader.ReadBoolean();
			this.m_anchoredLaserAimDirection = reader.ReadVector3();
			this.m_turnsAnchored = (short)reader.ReadPackedUInt32();
			this.m_lastBasicAttackUsedTurn = (short)reader.ReadPackedUInt32();
			return;
		}
		int num = (int)reader.ReadPackedUInt32();
		if ((num & 1) != 0)
		{
			this.m_anchored = reader.ReadBoolean();
		}
		if ((num & 2) != 0)
		{
			this.HookSetWasAnchoredOnTurnStart(reader.ReadBoolean());
		}
		if ((num & 4) != 0)
		{
			this.m_laserBarrierIsUp = reader.ReadBoolean();
		}
		if ((num & 8) != 0)
		{
			this.m_anchoredLaserAimDirection = reader.ReadVector3();
		}
		if ((num & 0x10) != 0)
		{
			this.m_turnsAnchored = (short)reader.ReadPackedUInt32();
		}
		if ((num & 0x20) != 0)
		{
			this.m_lastBasicAttackUsedTurn = (short)reader.ReadPackedUInt32();
		}
	}
}

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

	private static readonly int animIdleType;

	private static readonly int animExitAnchor;

	private ActorData m_owner;

	private static int kRpcRpcSetIdleType;

	private static int kRpcRpcSetFacingDirection;

	private static int kRpcRpcSetSweepingRight;

	public bool Networkm_anchored
	{
		get
		{
			return m_anchored;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_anchored, 1u);
		}
	}

	public bool Networkm_wasAnchoredOnTurnStart
	{
		get
		{
			return m_wasAnchoredOnTurnStart;
		}
		[param: In]
		set
		{
			ref bool wasAnchoredOnTurnStart = ref m_wasAnchoredOnTurnStart;
			if (NetworkServer.localClientActive)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				if (!base.syncVarHookGuard)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					base.syncVarHookGuard = true;
					HookSetWasAnchoredOnTurnStart(value);
					base.syncVarHookGuard = false;
				}
			}
			SetSyncVar(value, ref wasAnchoredOnTurnStart, 2u);
		}
	}

	public bool Networkm_laserBarrierIsUp
	{
		get
		{
			return m_laserBarrierIsUp;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_laserBarrierIsUp, 4u);
		}
	}

	public Vector3 Networkm_anchoredLaserAimDirection
	{
		get
		{
			return m_anchoredLaserAimDirection;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_anchoredLaserAimDirection, 8u);
		}
	}

	public short Networkm_turnsAnchored
	{
		get
		{
			return m_turnsAnchored;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_turnsAnchored, 16u);
		}
	}

	public short Networkm_lastBasicAttackUsedTurn
	{
		get
		{
			return m_lastBasicAttackUsedTurn;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_lastBasicAttackUsedTurn, 32u);
		}
	}

	static Exo_SyncComponent()
	{
		animIdleType = Animator.StringToHash("IdleType");
		animExitAnchor = Animator.StringToHash("ExitAnchor");
		kRpcRpcSetIdleType = 497885915;
		NetworkBehaviour.RegisterRpcDelegate(typeof(Exo_SyncComponent), kRpcRpcSetIdleType, InvokeRpcRpcSetIdleType);
		kRpcRpcSetFacingDirection = -1163602312;
		NetworkBehaviour.RegisterRpcDelegate(typeof(Exo_SyncComponent), kRpcRpcSetFacingDirection, InvokeRpcRpcSetFacingDirection);
		kRpcRpcSetSweepingRight = 162886841;
		NetworkBehaviour.RegisterRpcDelegate(typeof(Exo_SyncComponent), kRpcRpcSetSweepingRight, InvokeRpcRpcSetSweepingRight);
		NetworkCRC.RegisterBehaviour("Exo_SyncComponent", 0);
	}

	private ActorData GetOwner()
	{
		if (m_owner == null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_owner = GetComponent<ActorData>();
		}
		return m_owner;
	}

	private void Start()
	{
		AbilityData component = GetComponent<AbilityData>();
		if (!(component != null))
		{
			return;
		}
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_anchorLaserAbility = (component.GetAbilityOfType(typeof(ExoAnchorLaser)) as ExoAnchorLaser);
			return;
		}
	}

	public bool UsedBasicAttackLastTurn()
	{
		if (GameFlowData.Get() != null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_lastBasicAttackUsedTurn > 0)
			{
				return GameFlowData.Get().CurrentTurn - m_lastBasicAttackUsedTurn == 1;
			}
		}
		return false;
	}

	private void HookSetWasAnchoredOnTurnStart(bool value)
	{
		Networkm_wasAnchoredOnTurnStart = value;
		if (!NetworkClient.active)
		{
			return;
		}
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (!NetworkServer.active && m_anchorLaserAbility != null && m_anchorLaserAbility.ShouldUpdateMovementOnAnchorChange())
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					GetOwner().GetActorMovement().UpdateSquaresCanMoveTo();
					return;
				}
			}
			return;
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
		if (!(owner != null) || !(owner.GetModelAnimator() != null))
		{
			return;
		}
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			int integer = owner.GetModelAnimator().GetInteger(animIdleType);
			if (integer != idleType)
			{
				owner.GetModelAnimator().SetInteger(animIdleType, idleType);
				if (idleType == 0)
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						owner.GetModelAnimator().SetTrigger(animExitAnchor);
						return;
					}
				}
				return;
			}
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
			switch (6)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			ActorData owner = GetOwner();
			if (owner != null)
			{
				owner.TurnToDirection(facing);
			}
			return;
		}
	}

	[ClientRpc]
	public void RpcSetSweepingRight(bool sweepingToTheRight)
	{
		if (NetworkServer.active)
		{
			return;
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			ActorData owner = GetOwner();
			if (owner != null && owner.GetModelAnimator() != null)
			{
				owner.GetModelAnimator().SetBool("SweepingRight", sweepingToTheRight);
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
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					Debug.LogError("RPC RpcSetIdleType called on server.");
					return;
				}
			}
		}
		((Exo_SyncComponent)obj).RpcSetIdleType((int)reader.ReadPackedUInt32());
	}

	protected static void InvokeRpcRpcSetFacingDirection(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					Debug.LogError("RPC RpcSetFacingDirection called on server.");
					return;
				}
			}
		}
		((Exo_SyncComponent)obj).RpcSetFacingDirection(reader.ReadVector3());
	}

	protected static void InvokeRpcRpcSetSweepingRight(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					Debug.LogError("RPC RpcSetSweepingRight called on server.");
					return;
				}
			}
		}
		((Exo_SyncComponent)obj).RpcSetSweepingRight(reader.ReadBoolean());
	}

	public void CallRpcSetIdleType(int idleType)
	{
		if (!NetworkServer.active)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
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
				switch (1)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
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

	public void CallRpcSetSweepingRight(bool sweepingToTheRight)
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
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					Debug.LogError("RPC Function RpcSetSweepingRight called on client.");
					return;
				}
			}
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)2);
		networkWriter.WritePackedUInt32((uint)kRpcRpcSetSweepingRight);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		networkWriter.Write(sweepingToTheRight);
		SendRPCInternal(networkWriter, 0, "RpcSetSweepingRight");
	}

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
		if ((base.syncVarDirtyBits & 1) != 0)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (!flag)
			{
				while (true)
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
			writer.Write(m_anchored);
		}
		if ((base.syncVarDirtyBits & 2) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.Write(m_wasAnchoredOnTurnStart);
		}
		if ((base.syncVarDirtyBits & 4) != 0)
		{
			if (!flag)
			{
				while (true)
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
			writer.Write(m_laserBarrierIsUp);
		}
		if ((base.syncVarDirtyBits & 8) != 0)
		{
			while (true)
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
				while (true)
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
			writer.Write(m_anchoredLaserAimDirection);
		}
		if ((base.syncVarDirtyBits & 0x10) != 0)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!flag)
			{
				while (true)
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
			writer.WritePackedUInt32((uint)m_turnsAnchored);
		}
		if ((base.syncVarDirtyBits & 0x20) != 0)
		{
			while (true)
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
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_lastBasicAttackUsedTurn);
		}
		if (!flag)
		{
			while (true)
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
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					m_anchored = reader.ReadBoolean();
					m_wasAnchoredOnTurnStart = reader.ReadBoolean();
					m_laserBarrierIsUp = reader.ReadBoolean();
					m_anchoredLaserAimDirection = reader.ReadVector3();
					m_turnsAnchored = (short)reader.ReadPackedUInt32();
					m_lastBasicAttackUsedTurn = (short)reader.ReadPackedUInt32();
					return;
				}
			}
		}
		int num = (int)reader.ReadPackedUInt32();
		if ((num & 1) != 0)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
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
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			m_anchoredLaserAimDirection = reader.ReadVector3();
		}
		if ((num & 0x10) != 0)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			m_turnsAnchored = (short)reader.ReadPackedUInt32();
		}
		if ((num & 0x20) != 0)
		{
			m_lastBasicAttackUsedTurn = (short)reader.ReadPackedUInt32();
		}
	}
}

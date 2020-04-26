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

	private static readonly int s_aHashIdleType;

	private static readonly int s_aHashForceIdle;

	private static int kRpcRpcResetDashOrShieldTargeter;

	private static int kRpcRpcSetDashReadyStanceAnimParams;

	public int Networkm_dashOrShieldLastCastTurn
	{
		get
		{
			return m_dashOrShieldLastCastTurn;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_dashOrShieldLastCastTurn, 1u);
		}
	}

	public bool Networkm_dashOrShieldInReadyStance
	{
		get
		{
			return m_dashOrShieldInReadyStance;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_dashOrShieldInReadyStance, 2u);
		}
	}

	public short Networkm_layerConePowerLevel
	{
		get
		{
			return m_layerConePowerLevel;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_layerConePowerLevel, 4u);
		}
	}

	static Dino_SyncComponent()
	{
		s_aHashIdleType = Animator.StringToHash("IdleType");
		s_aHashForceIdle = Animator.StringToHash("ForceIdle");
		kRpcRpcResetDashOrShieldTargeter = 1094008940;
		NetworkBehaviour.RegisterRpcDelegate(typeof(Dino_SyncComponent), kRpcRpcResetDashOrShieldTargeter, InvokeRpcRpcResetDashOrShieldTargeter);
		kRpcRpcSetDashReadyStanceAnimParams = 423013429;
		NetworkBehaviour.RegisterRpcDelegate(typeof(Dino_SyncComponent), kRpcRpcSetDashReadyStanceAnimParams, InvokeRpcRpcSetDashReadyStanceAnimParams);
		NetworkCRC.RegisterBehaviour("Dino_SyncComponent", 0);
	}

	private void Start()
	{
		m_actor = GetComponent<ActorData>();
	}

	[ClientRpc]
	public void RpcResetDashOrShieldTargeter(bool inReadyStance)
	{
		if (!NetworkServer.active)
		{
			Networkm_dashOrShieldInReadyStance = inReadyStance;
			ResetDashOrShieldTargeter();
		}
	}

	public void ResetDashOrShieldTargeter()
	{
		if (m_dashOrShieldAbility == null)
		{
			m_dashOrShieldAbility = (GetComponent<AbilityData>().GetAbilityOfType(typeof(DinoDashOrShield)) as DinoDashOrShield);
		}
		if (!(m_dashOrShieldAbility != null))
		{
			return;
		}
		while (true)
		{
			m_dashOrShieldAbility.ResetTargetersForStanceChange();
			return;
		}
	}

	[ClientRpc]
	public void RpcSetDashReadyStanceAnimParams(int idleType, bool forceIdle)
	{
		SetDashReadyStanceAnimParams(idleType, forceIdle);
	}

	public void SetDashReadyStanceAnimParams(int idleType, bool forceIdle)
	{
		Animator modelAnimator = m_actor.GetModelAnimator();
		if (!NetworkClient.active)
		{
			return;
		}
		while (true)
		{
			if (modelAnimator != null)
			{
				modelAnimator.SetInteger(s_aHashIdleType, idleType);
				if (forceIdle)
				{
					while (true)
					{
						modelAnimator.SetTrigger(s_aHashForceIdle);
						return;
					}
				}
				return;
			}
			return;
		}
	}

	private void UNetVersion()
	{
	}

	protected static void InvokeRpcRpcResetDashOrShieldTargeter(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					Debug.LogError("RPC RpcResetDashOrShieldTargeter called on server.");
					return;
				}
			}
		}
		((Dino_SyncComponent)obj).RpcResetDashOrShieldTargeter(reader.ReadBoolean());
	}

	protected static void InvokeRpcRpcSetDashReadyStanceAnimParams(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					Debug.LogError("RPC RpcSetDashReadyStanceAnimParams called on server.");
					return;
				}
			}
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
		networkWriter.Write((short)0);
		networkWriter.Write((short)2);
		networkWriter.WritePackedUInt32((uint)kRpcRpcResetDashOrShieldTargeter);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		networkWriter.Write(inReadyStance);
		SendRPCInternal(networkWriter, 0, "RpcResetDashOrShieldTargeter");
	}

	public void CallRpcSetDashReadyStanceAnimParams(int idleType, bool forceIdle)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("RPC Function RpcSetDashReadyStanceAnimParams called on client.");
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)2);
		networkWriter.WritePackedUInt32((uint)kRpcRpcSetDashReadyStanceAnimParams);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		networkWriter.WritePackedUInt32((uint)idleType);
		networkWriter.Write(forceIdle);
		SendRPCInternal(networkWriter, 0, "RpcSetDashReadyStanceAnimParams");
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		if (forceAll)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					writer.WritePackedUInt32((uint)m_dashOrShieldLastCastTurn);
					writer.Write(m_dashOrShieldInReadyStance);
					writer.WritePackedUInt32((uint)m_layerConePowerLevel);
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
			writer.WritePackedUInt32((uint)m_dashOrShieldLastCastTurn);
		}
		if ((base.syncVarDirtyBits & 2) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.Write(m_dashOrShieldInReadyStance);
		}
		if ((base.syncVarDirtyBits & 4) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_layerConePowerLevel);
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
				switch (3)
				{
				case 0:
					break;
				default:
					m_dashOrShieldLastCastTurn = (int)reader.ReadPackedUInt32();
					m_dashOrShieldInReadyStance = reader.ReadBoolean();
					m_layerConePowerLevel = (short)reader.ReadPackedUInt32();
					return;
				}
			}
		}
		int num = (int)reader.ReadPackedUInt32();
		if ((num & 1) != 0)
		{
			m_dashOrShieldLastCastTurn = (int)reader.ReadPackedUInt32();
		}
		if ((num & 2) != 0)
		{
			m_dashOrShieldInReadyStance = reader.ReadBoolean();
		}
		if ((num & 4) == 0)
		{
			return;
		}
		while (true)
		{
			m_layerConePowerLevel = (short)reader.ReadPackedUInt32();
			return;
		}
	}
}

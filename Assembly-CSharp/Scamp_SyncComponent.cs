using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Networking;

public class Scamp_SyncComponent : NetworkBehaviour
{
	[Separator("Anim Parameter to set, 1 for when Scamp has suit active, 0 otherwise", true)]
	public string m_suitActiveAnimParamName = "ShieldState";

	[Separator("Shield Remove Anim Index", true)]
	public int m_shieldRemoveAnimIndex = 6;

	[AudioEvent(false)]
	public string m_noSuitChatterEventOverride = string.Empty;

	[SyncVar]
	internal bool m_suitWasActiveOnTurnStart = true;

	[SyncVar]
	internal bool m_suitActive = true;

	[SyncVar]
	internal uint m_suitShieldingOnTurnStart;

	[SyncVar]
	internal uint m_lastSuitLostTurn;

	private ScampSideLaserOrCone m_sideLasersAbility;

	private ScampDualLasers m_meetingLasersAbility;

	private ScampDashAndAoe m_dashAoeAbility;

	private int m_suitActiveAnimHash;

	private ActorData m_actor;

	private ScampVfxController m_vfxController;

	private Scamp_ChatterEventOverrider m_chatterEventOverrider;

	private static readonly int s_aHashIdleType;

	private static readonly int s_aHashAttack;

	private static readonly int s_aHashCinematicCam;

	private static readonly int s_aHashStartAttack;

	private static int kRpcRpcResetTargetersForSuitMode;

	private static int kRpcRpcSetAnimParamForSuit;

	private static int kRpcRpcPlayShieldRemoveAnim;

	private static int kRpcRpcResetAttackParam;

	public bool Networkm_suitWasActiveOnTurnStart
	{
		get
		{
			return m_suitWasActiveOnTurnStart;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_suitWasActiveOnTurnStart, 1u);
		}
	}

	public bool Networkm_suitActive
	{
		get
		{
			return m_suitActive;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_suitActive, 2u);
		}
	}

	public uint Networkm_suitShieldingOnTurnStart
	{
		get
		{
			return m_suitShieldingOnTurnStart;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_suitShieldingOnTurnStart, 4u);
		}
	}

	public uint Networkm_lastSuitLostTurn
	{
		get
		{
			return m_lastSuitLostTurn;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_lastSuitLostTurn, 8u);
		}
	}

	static Scamp_SyncComponent()
	{
		s_aHashIdleType = Animator.StringToHash("IdleType");
		s_aHashAttack = Animator.StringToHash("Attack");
		s_aHashCinematicCam = Animator.StringToHash("CinematicCam");
		s_aHashStartAttack = Animator.StringToHash("StartAttack");
		kRpcRpcResetTargetersForSuitMode = -1873216954;
		NetworkBehaviour.RegisterRpcDelegate(typeof(Scamp_SyncComponent), kRpcRpcResetTargetersForSuitMode, InvokeRpcRpcResetTargetersForSuitMode);
		kRpcRpcSetAnimParamForSuit = -1194507473;
		NetworkBehaviour.RegisterRpcDelegate(typeof(Scamp_SyncComponent), kRpcRpcSetAnimParamForSuit, InvokeRpcRpcSetAnimParamForSuit);
		kRpcRpcPlayShieldRemoveAnim = 1437871839;
		NetworkBehaviour.RegisterRpcDelegate(typeof(Scamp_SyncComponent), kRpcRpcPlayShieldRemoveAnim, InvokeRpcRpcPlayShieldRemoveAnim);
		kRpcRpcResetAttackParam = -1007020189;
		NetworkBehaviour.RegisterRpcDelegate(typeof(Scamp_SyncComponent), kRpcRpcResetAttackParam, InvokeRpcRpcResetAttackParam);
		NetworkCRC.RegisterBehaviour("Scamp_SyncComponent", 0);
	}

	private void Awake()
	{
		m_suitActiveAnimHash = Animator.StringToHash(m_suitActiveAnimParamName);
	}

	private void Start()
	{
		m_actor = GetComponent<ActorData>();
		if (m_actor != null && m_actor.GetModelAnimator() != null)
		{
			m_actor.GetModelAnimator().SetInteger(m_suitActiveAnimHash, 1);
		}
		AbilityData component = GetComponent<AbilityData>();
		if (component != null)
		{
			m_sideLasersAbility = component.GetAbilityOfType<ScampSideLaserOrCone>();
			m_meetingLasersAbility = component.GetAbilityOfType<ScampDualLasers>();
			m_dashAoeAbility = component.GetAbilityOfType<ScampDashAndAoe>();
		}
		m_vfxController = GetComponentInChildren<ScampVfxController>();
		ChatterComponent component2 = GetComponent<ChatterComponent>();
		if (component2 != null)
		{
			m_chatterEventOverrider = new Scamp_ChatterEventOverrider(this);
			component2.SetEventOverrider(m_chatterEventOverrider);
		}
	}

	[ClientRpc]
	public void RpcResetTargetersForSuitMode(bool hasShielding)
	{
		if (!NetworkServer.active)
		{
			Networkm_suitWasActiveOnTurnStart = hasShielding;
			ResetTargeterForSuitMode(hasShielding);
		}
	}

	public void ResetTargeterForSuitMode(bool hasShielding)
	{
		if (m_sideLasersAbility != null)
		{
			m_sideLasersAbility.ResetTargetersForShielding(hasShielding);
		}
		if (m_dashAoeAbility != null)
		{
			m_dashAoeAbility.ResetTargetersForShielding(hasShielding);
		}
		if (m_meetingLasersAbility != null)
		{
			m_meetingLasersAbility.ResetTargetersForShielding(hasShielding);
		}
	}

	[ClientRpc]
	public void RpcSetAnimParamForSuit(bool activeNow)
	{
		SetAnimParamForSuit(activeNow);
	}

	public void SetAnimParamForSuit(bool activeNow)
	{
		if (!NetworkClient.active || !(m_actor != null) || !(m_actor.GetModelAnimator() != null))
		{
			return;
		}
		while (true)
		{
			Animator modelAnimator = m_actor.GetModelAnimator();
			int suitActiveAnimHash = m_suitActiveAnimHash;
			int value;
			if (activeNow)
			{
				value = 1;
			}
			else
			{
				value = 0;
			}
			modelAnimator.SetInteger(suitActiveAnimHash, value);
			return;
		}
	}

	[ClientRpc]
	public void RpcPlayShieldRemoveAnim()
	{
		PlayShieldRemoveAnim();
	}

	public void PlayShieldRemoveAnim()
	{
		Animator modelAnimator = m_actor.GetModelAnimator();
		if (!(modelAnimator != null))
		{
			return;
		}
		while (true)
		{
			if (m_shieldRemoveAnimIndex > 0)
			{
				while (true)
				{
					modelAnimator.SetInteger(s_aHashAttack, m_shieldRemoveAnimIndex);
					modelAnimator.SetInteger(s_aHashIdleType, 0);
					modelAnimator.SetBool(s_aHashCinematicCam, false);
					modelAnimator.SetTrigger(s_aHashStartAttack);
					SetAnimParamForSuit(false);
					return;
				}
			}
			return;
		}
	}

	[ClientRpc]
	public void RpcResetAttackParam()
	{
		ResetAttackParam();
	}

	public void ResetAttackParam()
	{
		Animator modelAnimator = m_actor.GetModelAnimator();
		if (modelAnimator != null)
		{
			modelAnimator.SetInteger(s_aHashAttack, 0);
			modelAnimator.SetBool(s_aHashCinematicCam, false);
		}
	}

	public bool IsSuitModelActive()
	{
		int result;
		if (m_vfxController != null)
		{
			result = (m_vfxController.IsSuitVisuallyShown() ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	private void UNetVersion()
	{
	}

	protected static void InvokeRpcRpcResetTargetersForSuitMode(NetworkBehaviour obj, NetworkReader reader)
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
					Debug.LogError("RPC RpcResetTargetersForSuitMode called on server.");
					return;
				}
			}
		}
		((Scamp_SyncComponent)obj).RpcResetTargetersForSuitMode(reader.ReadBoolean());
	}

	protected static void InvokeRpcRpcSetAnimParamForSuit(NetworkBehaviour obj, NetworkReader reader)
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
					Debug.LogError("RPC RpcSetAnimParamForSuit called on server.");
					return;
				}
			}
		}
		((Scamp_SyncComponent)obj).RpcSetAnimParamForSuit(reader.ReadBoolean());
	}

	protected static void InvokeRpcRpcPlayShieldRemoveAnim(NetworkBehaviour obj, NetworkReader reader)
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
					Debug.LogError("RPC RpcPlayShieldRemoveAnim called on server.");
					return;
				}
			}
		}
		((Scamp_SyncComponent)obj).RpcPlayShieldRemoveAnim();
	}

	protected static void InvokeRpcRpcResetAttackParam(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcResetAttackParam called on server.");
		}
		else
		{
			((Scamp_SyncComponent)obj).RpcResetAttackParam();
		}
	}

	public void CallRpcResetTargetersForSuitMode(bool hasShielding)
	{
		if (!NetworkServer.active)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					Debug.LogError("RPC Function RpcResetTargetersForSuitMode called on client.");
					return;
				}
			}
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)2);
		networkWriter.WritePackedUInt32((uint)kRpcRpcResetTargetersForSuitMode);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		networkWriter.Write(hasShielding);
		SendRPCInternal(networkWriter, 0, "RpcResetTargetersForSuitMode");
	}

	public void CallRpcSetAnimParamForSuit(bool activeNow)
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
					Debug.LogError("RPC Function RpcSetAnimParamForSuit called on client.");
					return;
				}
			}
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)2);
		networkWriter.WritePackedUInt32((uint)kRpcRpcSetAnimParamForSuit);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		networkWriter.Write(activeNow);
		SendRPCInternal(networkWriter, 0, "RpcSetAnimParamForSuit");
	}

	public void CallRpcPlayShieldRemoveAnim()
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("RPC Function RpcPlayShieldRemoveAnim called on client.");
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)2);
		networkWriter.WritePackedUInt32((uint)kRpcRpcPlayShieldRemoveAnim);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		SendRPCInternal(networkWriter, 0, "RpcPlayShieldRemoveAnim");
	}

	public void CallRpcResetAttackParam()
	{
		if (!NetworkServer.active)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					Debug.LogError("RPC Function RpcResetAttackParam called on client.");
					return;
				}
			}
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)2);
		networkWriter.WritePackedUInt32((uint)kRpcRpcResetAttackParam);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		SendRPCInternal(networkWriter, 0, "RpcResetAttackParam");
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		if (forceAll)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					writer.Write(m_suitWasActiveOnTurnStart);
					writer.Write(m_suitActive);
					writer.WritePackedUInt32(m_suitShieldingOnTurnStart);
					writer.WritePackedUInt32(m_lastSuitLostTurn);
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
			writer.Write(m_suitWasActiveOnTurnStart);
		}
		if ((base.syncVarDirtyBits & 2) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.Write(m_suitActive);
		}
		if ((base.syncVarDirtyBits & 4) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32(m_suitShieldingOnTurnStart);
		}
		if ((base.syncVarDirtyBits & 8) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32(m_lastSuitLostTurn);
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
				switch (4)
				{
				case 0:
					break;
				default:
					m_suitWasActiveOnTurnStart = reader.ReadBoolean();
					m_suitActive = reader.ReadBoolean();
					m_suitShieldingOnTurnStart = reader.ReadPackedUInt32();
					m_lastSuitLostTurn = reader.ReadPackedUInt32();
					return;
				}
			}
		}
		int num = (int)reader.ReadPackedUInt32();
		if ((num & 1) != 0)
		{
			m_suitWasActiveOnTurnStart = reader.ReadBoolean();
		}
		if ((num & 2) != 0)
		{
			m_suitActive = reader.ReadBoolean();
		}
		if ((num & 4) != 0)
		{
			m_suitShieldingOnTurnStart = reader.ReadPackedUInt32();
		}
		if ((num & 8) != 0)
		{
			m_lastSuitLostTurn = reader.ReadPackedUInt32();
		}
	}
}

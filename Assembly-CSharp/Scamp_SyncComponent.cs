using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Networking;

public class Scamp_SyncComponent : NetworkBehaviour
{
	[Separator("Anim Parameter to set, 1 for when Scamp has suit active, 0 otherwise")]
	public string m_suitActiveAnimParamName = "ShieldState";
	[Separator("Shield Remove Anim Index")]
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

	private static readonly int s_aHashIdleType = Animator.StringToHash("IdleType");
	private static readonly int s_aHashAttack = Animator.StringToHash("Attack");
	private static readonly int s_aHashCinematicCam = Animator.StringToHash("CinematicCam");
	private static readonly int s_aHashStartAttack = Animator.StringToHash("StartAttack");
	
	private static int kRpcRpcResetTargetersForSuitMode = -1873216954;
	private static int kRpcRpcSetAnimParamForSuit = -1194507473;
	private static int kRpcRpcPlayShieldRemoveAnim = 1437871839;
	private static int kRpcRpcResetAttackParam = -1007020189;

	public bool Networkm_suitWasActiveOnTurnStart
	{
		get => m_suitWasActiveOnTurnStart;
		[param: In]
		set => SetSyncVar(value, ref m_suitWasActiveOnTurnStart, 1u);
	}

	public bool Networkm_suitActive
	{
		get => m_suitActive;
		[param: In]
		set => SetSyncVar(value, ref m_suitActive, 2u);
	}

	public uint Networkm_suitShieldingOnTurnStart
	{
		get => m_suitShieldingOnTurnStart;
		[param: In]
		set => SetSyncVar(value, ref m_suitShieldingOnTurnStart, 4u);
	}

	public uint Networkm_lastSuitLostTurn
	{
		get => m_lastSuitLostTurn;
		[param: In]
		set => SetSyncVar(value, ref m_lastSuitLostTurn, 8u);
	}

	static Scamp_SyncComponent()
	{
		RegisterRpcDelegate(typeof(Scamp_SyncComponent), kRpcRpcResetTargetersForSuitMode, InvokeRpcRpcResetTargetersForSuitMode);
		RegisterRpcDelegate(typeof(Scamp_SyncComponent), kRpcRpcSetAnimParamForSuit, InvokeRpcRpcSetAnimParamForSuit);
		RegisterRpcDelegate(typeof(Scamp_SyncComponent), kRpcRpcPlayShieldRemoveAnim, InvokeRpcRpcPlayShieldRemoveAnim);
		RegisterRpcDelegate(typeof(Scamp_SyncComponent), kRpcRpcResetAttackParam, InvokeRpcRpcResetAttackParam);
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
		AbilityData abilityData = GetComponent<AbilityData>();
		if (abilityData != null)
		{
			m_sideLasersAbility = abilityData.GetAbilityOfType<ScampSideLaserOrCone>();
			m_meetingLasersAbility = abilityData.GetAbilityOfType<ScampDualLasers>();
			m_dashAoeAbility = abilityData.GetAbilityOfType<ScampDashAndAoe>();
		}
		m_vfxController = GetComponentInChildren<ScampVfxController>();
		ChatterComponent chatterComponent = GetComponent<ChatterComponent>();
		if (chatterComponent != null)
		{
			m_chatterEventOverrider = new Scamp_ChatterEventOverrider(this);
			chatterComponent.SetEventOverrider(m_chatterEventOverrider);
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
		if (NetworkClient.active && m_actor != null && m_actor.GetModelAnimator() != null)
		{
			m_actor.GetModelAnimator().SetInteger(m_suitActiveAnimHash, activeNow ? 1 : 0);
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
		if (modelAnimator != null && m_shieldRemoveAnimIndex > 0)
		{
			modelAnimator.SetInteger(s_aHashAttack, m_shieldRemoveAnimIndex);
			modelAnimator.SetInteger(s_aHashIdleType, 0);
			modelAnimator.SetBool(s_aHashCinematicCam, false);
			modelAnimator.SetTrigger(s_aHashStartAttack);
			SetAnimParamForSuit(false);
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
		return m_vfxController != null && m_vfxController.IsSuitVisuallyShown();
	}

	private void UNetVersion()
	{
	}

	protected static void InvokeRpcRpcResetTargetersForSuitMode(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcResetTargetersForSuitMode called on server.");
			return;
		}
		((Scamp_SyncComponent)obj).RpcResetTargetersForSuitMode(reader.ReadBoolean());
	}

	protected static void InvokeRpcRpcSetAnimParamForSuit(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcSetAnimParamForSuit called on server.");
			return;
		}
		((Scamp_SyncComponent)obj).RpcSetAnimParamForSuit(reader.ReadBoolean());
	}

	protected static void InvokeRpcRpcPlayShieldRemoveAnim(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcPlayShieldRemoveAnim called on server.");
			return;
		}
		((Scamp_SyncComponent)obj).RpcPlayShieldRemoveAnim();
	}

	protected static void InvokeRpcRpcResetAttackParam(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcResetAttackParam called on server.");
			return;
		}
		((Scamp_SyncComponent)obj).RpcResetAttackParam();
	}

	public void CallRpcResetTargetersForSuitMode(bool hasShielding)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("RPC Function RpcResetTargetersForSuitMode called on client.");
			return;
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
			Debug.LogError("RPC Function RpcSetAnimParamForSuit called on client.");
			return;
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
			Debug.LogError("RPC Function RpcResetAttackParam called on client.");
			return;
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
			writer.Write(m_suitWasActiveOnTurnStart);
			writer.Write(m_suitActive);
			writer.WritePackedUInt32(m_suitShieldingOnTurnStart);
			writer.WritePackedUInt32(m_lastSuitLostTurn);
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
			writer.Write(m_suitWasActiveOnTurnStart);
		}
		if ((syncVarDirtyBits & 2) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			writer.Write(m_suitActive);
		}
		if ((syncVarDirtyBits & 4) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32(m_suitShieldingOnTurnStart);
		}
		if ((syncVarDirtyBits & 8) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32(m_lastSuitLostTurn);
		}
		if (!flag)
		{
			writer.WritePackedUInt32(syncVarDirtyBits);
		}
		return flag;
	}

	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
		if (initialState)
		{
			m_suitWasActiveOnTurnStart = reader.ReadBoolean();
			m_suitActive = reader.ReadBoolean();
			m_suitShieldingOnTurnStart = reader.ReadPackedUInt32();
			m_lastSuitLostTurn = reader.ReadPackedUInt32();
			return;
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

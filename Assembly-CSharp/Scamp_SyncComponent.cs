using System;
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

	private static readonly int s_aHashIdleType = Animator.StringToHash("IdleType");

	private static readonly int s_aHashAttack = Animator.StringToHash("Attack");

	private static readonly int s_aHashCinematicCam = Animator.StringToHash("CinematicCam");

	private static readonly int s_aHashStartAttack = Animator.StringToHash("StartAttack");

	private static int kRpcRpcResetTargetersForSuitMode = -0x6FA705BA;

	private static int kRpcRpcSetAnimParamForSuit;

	private static int kRpcRpcPlayShieldRemoveAnim;

	private static int kRpcRpcResetAttackParam;

	static Scamp_SyncComponent()
	{
		NetworkBehaviour.RegisterRpcDelegate(typeof(Scamp_SyncComponent), Scamp_SyncComponent.kRpcRpcResetTargetersForSuitMode, new NetworkBehaviour.CmdDelegate(Scamp_SyncComponent.InvokeRpcRpcResetTargetersForSuitMode));
		Scamp_SyncComponent.kRpcRpcSetAnimParamForSuit = -0x4732BCD1;
		NetworkBehaviour.RegisterRpcDelegate(typeof(Scamp_SyncComponent), Scamp_SyncComponent.kRpcRpcSetAnimParamForSuit, new NetworkBehaviour.CmdDelegate(Scamp_SyncComponent.InvokeRpcRpcSetAnimParamForSuit));
		Scamp_SyncComponent.kRpcRpcPlayShieldRemoveAnim = 0x55B42EDF;
		NetworkBehaviour.RegisterRpcDelegate(typeof(Scamp_SyncComponent), Scamp_SyncComponent.kRpcRpcPlayShieldRemoveAnim, new NetworkBehaviour.CmdDelegate(Scamp_SyncComponent.InvokeRpcRpcPlayShieldRemoveAnim));
		Scamp_SyncComponent.kRpcRpcResetAttackParam = -0x3C05E89D;
		NetworkBehaviour.RegisterRpcDelegate(typeof(Scamp_SyncComponent), Scamp_SyncComponent.kRpcRpcResetAttackParam, new NetworkBehaviour.CmdDelegate(Scamp_SyncComponent.InvokeRpcRpcResetAttackParam));
		NetworkCRC.RegisterBehaviour("Scamp_SyncComponent", 0);
	}

	private void Awake()
	{
		this.m_suitActiveAnimHash = Animator.StringToHash(this.m_suitActiveAnimParamName);
	}

	private void Start()
	{
		this.m_actor = base.GetComponent<ActorData>();
		if (this.m_actor != null && this.m_actor.\u000E() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Scamp_SyncComponent.Start()).MethodHandle;
			}
			this.m_actor.\u000E().SetInteger(this.m_suitActiveAnimHash, 1);
		}
		AbilityData component = base.GetComponent<AbilityData>();
		if (component != null)
		{
			this.m_sideLasersAbility = component.GetAbilityOfType<ScampSideLaserOrCone>();
			this.m_meetingLasersAbility = component.GetAbilityOfType<ScampDualLasers>();
			this.m_dashAoeAbility = component.GetAbilityOfType<ScampDashAndAoe>();
		}
		this.m_vfxController = base.GetComponentInChildren<ScampVfxController>();
		ChatterComponent component2 = base.GetComponent<ChatterComponent>();
		if (component2 != null)
		{
			this.m_chatterEventOverrider = new Scamp_ChatterEventOverrider(this);
			component2.SetEventOverrider(this.m_chatterEventOverrider);
		}
	}

	[ClientRpc]
	public void RpcResetTargetersForSuitMode(bool hasShielding)
	{
		if (!NetworkServer.active)
		{
			this.Networkm_suitWasActiveOnTurnStart = hasShielding;
			this.ResetTargeterForSuitMode(hasShielding);
		}
	}

	public void ResetTargeterForSuitMode(bool hasShielding)
	{
		if (this.m_sideLasersAbility != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Scamp_SyncComponent.ResetTargeterForSuitMode(bool)).MethodHandle;
			}
			this.m_sideLasersAbility.ResetTargetersForShielding(hasShielding);
		}
		if (this.m_dashAoeAbility != null)
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
			this.m_dashAoeAbility.ResetTargetersForShielding(hasShielding);
		}
		if (this.m_meetingLasersAbility != null)
		{
			this.m_meetingLasersAbility.ResetTargetersForShielding(hasShielding);
		}
	}

	[ClientRpc]
	public void RpcSetAnimParamForSuit(bool activeNow)
	{
		this.SetAnimParamForSuit(activeNow);
	}

	public void SetAnimParamForSuit(bool activeNow)
	{
		if (NetworkClient.active && this.m_actor != null && this.m_actor.\u000E() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Scamp_SyncComponent.SetAnimParamForSuit(bool)).MethodHandle;
			}
			Animator animator = this.m_actor.\u000E();
			int suitActiveAnimHash = this.m_suitActiveAnimHash;
			int value;
			if (activeNow)
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
				value = 1;
			}
			else
			{
				value = 0;
			}
			animator.SetInteger(suitActiveAnimHash, value);
		}
	}

	[ClientRpc]
	public void RpcPlayShieldRemoveAnim()
	{
		this.PlayShieldRemoveAnim();
	}

	public void PlayShieldRemoveAnim()
	{
		Animator animator = this.m_actor.\u000E();
		if (animator != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Scamp_SyncComponent.PlayShieldRemoveAnim()).MethodHandle;
			}
			if (this.m_shieldRemoveAnimIndex > 0)
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
				animator.SetInteger(Scamp_SyncComponent.s_aHashAttack, this.m_shieldRemoveAnimIndex);
				animator.SetInteger(Scamp_SyncComponent.s_aHashIdleType, 0);
				animator.SetBool(Scamp_SyncComponent.s_aHashCinematicCam, false);
				animator.SetTrigger(Scamp_SyncComponent.s_aHashStartAttack);
				this.SetAnimParamForSuit(false);
			}
		}
	}

	[ClientRpc]
	public void RpcResetAttackParam()
	{
		this.ResetAttackParam();
	}

	public void ResetAttackParam()
	{
		Animator animator = this.m_actor.\u000E();
		if (animator != null)
		{
			animator.SetInteger(Scamp_SyncComponent.s_aHashAttack, 0);
			animator.SetBool(Scamp_SyncComponent.s_aHashCinematicCam, false);
		}
	}

	public bool IsSuitModelActive()
	{
		bool result;
		if (this.m_vfxController != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Scamp_SyncComponent.IsSuitModelActive()).MethodHandle;
			}
			result = this.m_vfxController.IsSuitVisuallyShown();
		}
		else
		{
			result = false;
		}
		return result;
	}

	private void UNetVersion()
	{
	}

	public bool Networkm_suitWasActiveOnTurnStart
	{
		get
		{
			return this.m_suitWasActiveOnTurnStart;
		}
		[param: In]
		set
		{
			base.SetSyncVar<bool>(value, ref this.m_suitWasActiveOnTurnStart, 1U);
		}
	}

	public bool Networkm_suitActive
	{
		get
		{
			return this.m_suitActive;
		}
		[param: In]
		set
		{
			base.SetSyncVar<bool>(value, ref this.m_suitActive, 2U);
		}
	}

	public uint Networkm_suitShieldingOnTurnStart
	{
		get
		{
			return this.m_suitShieldingOnTurnStart;
		}
		[param: In]
		set
		{
			base.SetSyncVar<uint>(value, ref this.m_suitShieldingOnTurnStart, 4U);
		}
	}

	public uint Networkm_lastSuitLostTurn
	{
		get
		{
			return this.m_lastSuitLostTurn;
		}
		[param: In]
		set
		{
			base.SetSyncVar<uint>(value, ref this.m_lastSuitLostTurn, 8U);
		}
	}

	protected static void InvokeRpcRpcResetTargetersForSuitMode(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Scamp_SyncComponent.InvokeRpcRpcResetTargetersForSuitMode(NetworkBehaviour, NetworkReader)).MethodHandle;
			}
			Debug.LogError("RPC RpcResetTargetersForSuitMode called on server.");
			return;
		}
		((Scamp_SyncComponent)obj).RpcResetTargetersForSuitMode(reader.ReadBoolean());
	}

	protected static void InvokeRpcRpcSetAnimParamForSuit(NetworkBehaviour obj, NetworkReader reader)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Scamp_SyncComponent.InvokeRpcRpcSetAnimParamForSuit(NetworkBehaviour, NetworkReader)).MethodHandle;
			}
			Debug.LogError("RPC RpcSetAnimParamForSuit called on server.");
			return;
		}
		((Scamp_SyncComponent)obj).RpcSetAnimParamForSuit(reader.ReadBoolean());
	}

	protected static void InvokeRpcRpcPlayShieldRemoveAnim(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Scamp_SyncComponent.InvokeRpcRpcPlayShieldRemoveAnim(NetworkBehaviour, NetworkReader)).MethodHandle;
			}
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Scamp_SyncComponent.CallRpcResetTargetersForSuitMode(bool)).MethodHandle;
			}
			Debug.LogError("RPC Function RpcResetTargetersForSuitMode called on client.");
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write(0);
		networkWriter.Write((short)((ushort)2));
		networkWriter.WritePackedUInt32((uint)Scamp_SyncComponent.kRpcRpcResetTargetersForSuitMode);
		networkWriter.Write(base.GetComponent<NetworkIdentity>().netId);
		networkWriter.Write(hasShielding);
		this.SendRPCInternal(networkWriter, 0, "RpcResetTargetersForSuitMode");
	}

	public void CallRpcSetAnimParamForSuit(bool activeNow)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Scamp_SyncComponent.CallRpcSetAnimParamForSuit(bool)).MethodHandle;
			}
			Debug.LogError("RPC Function RpcSetAnimParamForSuit called on client.");
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write(0);
		networkWriter.Write((short)((ushort)2));
		networkWriter.WritePackedUInt32((uint)Scamp_SyncComponent.kRpcRpcSetAnimParamForSuit);
		networkWriter.Write(base.GetComponent<NetworkIdentity>().netId);
		networkWriter.Write(activeNow);
		this.SendRPCInternal(networkWriter, 0, "RpcSetAnimParamForSuit");
	}

	public void CallRpcPlayShieldRemoveAnim()
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("RPC Function RpcPlayShieldRemoveAnim called on client.");
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write(0);
		networkWriter.Write((short)((ushort)2));
		networkWriter.WritePackedUInt32((uint)Scamp_SyncComponent.kRpcRpcPlayShieldRemoveAnim);
		networkWriter.Write(base.GetComponent<NetworkIdentity>().netId);
		this.SendRPCInternal(networkWriter, 0, "RpcPlayShieldRemoveAnim");
	}

	public void CallRpcResetAttackParam()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Scamp_SyncComponent.CallRpcResetAttackParam()).MethodHandle;
			}
			Debug.LogError("RPC Function RpcResetAttackParam called on client.");
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write(0);
		networkWriter.Write((short)((ushort)2));
		networkWriter.WritePackedUInt32((uint)Scamp_SyncComponent.kRpcRpcResetAttackParam);
		networkWriter.Write(base.GetComponent<NetworkIdentity>().netId);
		this.SendRPCInternal(networkWriter, 0, "RpcResetAttackParam");
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		if (forceAll)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Scamp_SyncComponent.OnSerialize(NetworkWriter, bool)).MethodHandle;
			}
			writer.Write(this.m_suitWasActiveOnTurnStart);
			writer.Write(this.m_suitActive);
			writer.WritePackedUInt32(this.m_suitShieldingOnTurnStart);
			writer.WritePackedUInt32(this.m_lastSuitLostTurn);
			return true;
		}
		bool flag = false;
		if ((base.syncVarDirtyBits & 1U) != 0U)
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
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.Write(this.m_suitWasActiveOnTurnStart);
		}
		if ((base.syncVarDirtyBits & 2U) != 0U)
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
			if (!flag)
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
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.Write(this.m_suitActive);
		}
		if ((base.syncVarDirtyBits & 4U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32(this.m_suitShieldingOnTurnStart);
		}
		if ((base.syncVarDirtyBits & 8U) != 0U)
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
			if (!flag)
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
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32(this.m_lastSuitLostTurn);
		}
		if (!flag)
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
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(Scamp_SyncComponent.OnDeserialize(NetworkReader, bool)).MethodHandle;
			}
			this.m_suitWasActiveOnTurnStart = reader.ReadBoolean();
			this.m_suitActive = reader.ReadBoolean();
			this.m_suitShieldingOnTurnStart = reader.ReadPackedUInt32();
			this.m_lastSuitLostTurn = reader.ReadPackedUInt32();
			return;
		}
		int num = (int)reader.ReadPackedUInt32();
		if ((num & 1) != 0)
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
			this.m_suitWasActiveOnTurnStart = reader.ReadBoolean();
		}
		if ((num & 2) != 0)
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
			this.m_suitActive = reader.ReadBoolean();
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
			this.m_suitShieldingOnTurnStart = reader.ReadPackedUInt32();
		}
		if ((num & 8) != 0)
		{
			this.m_lastSuitLostTurn = reader.ReadPackedUInt32();
		}
	}
}

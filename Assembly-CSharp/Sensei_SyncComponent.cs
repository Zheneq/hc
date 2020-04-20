using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Networking;

public class Sensei_SyncComponent : NetworkBehaviour
{
	[Header("-- Vfx Prefabs --")]
	public GameObject m_canDashToAllyVfxPrefab;

	[JointPopup("Joint for VFX")]
	public JointPopupProperty m_canDashToAllyVfxJoint;

	public Vector3 m_canDashToAllyVfxLocalOffset;

	[Space(10f)]
	public GameObject m_canDashToEnemyVfxPrefab;

	[JointPopup("Joint for VFX")]
	public JointPopupProperty m_canDashToEnemyVfxJoint;

	public Vector3 m_canDashToEnemyLocalOffset;

	[SyncVar(hook = "HookSetNumOrbs")]
	internal sbyte m_syncCurrentNumOrbs;

	[SyncVar]
	internal sbyte m_syncTurnsForSecondYingYangDash;

	[SyncVar]
	internal bool m_syncLastYingYangDashedToAlly;

	[SyncVar]
	internal float m_syncBideExtraDamagePct;

	[SyncVar]
	public sbyte m_lastPrimaryUsedMode;

	internal int m_clientOrbNumAdjust;

	private ActorData m_owner;

	private List<AttachedActorVFXInfo> m_dashIndicatorsSetA = new List<AttachedActorVFXInfo>();

	private List<AttachedActorVFXInfo> m_dashIndicatorsSetB = new List<AttachedActorVFXInfo>();

	private AbilityData m_abilityData;

	private SenseiYingYangDash m_yingYangDashAbility;

	private AbilityData.ActionType m_yingYangDashActionType;

	private bool m_lastAllyDashAudioSwitchState;

	private bool m_lastEnemyDashAudioSwitchState;

	private void HookSetNumOrbs(sbyte value)
	{
		this.Networkm_syncCurrentNumOrbs = value;
		this.m_clientOrbNumAdjust = 0;
	}

	private void Start()
	{
		this.m_owner = base.GetComponent<ActorData>();
		if (this.m_owner != null)
		{
			this.m_abilityData = this.m_owner.GetComponent<AbilityData>();
			if (this.m_abilityData != null)
			{
				this.m_yingYangDashAbility = (this.m_abilityData.GetAbilityOfType(typeof(SenseiYingYangDash)) as SenseiYingYangDash);
				this.m_yingYangDashActionType = this.m_abilityData.GetActionTypeOfAbility(this.m_yingYangDashAbility);
			}
			if (this.m_yingYangDashAbility != null)
			{
				List<GameObject> prefabs = new List<GameObject>
				{
					this.m_canDashToAllyVfxPrefab,
					this.m_canDashToEnemyVfxPrefab
				};
				this.InstantiateVfxIndicators(this.m_dashIndicatorsSetA, this.m_canDashToAllyVfxJoint, this.m_canDashToAllyVfxLocalOffset, prefabs);
				this.InstantiateVfxIndicators(this.m_dashIndicatorsSetB, this.m_canDashToEnemyVfxJoint, this.m_canDashToEnemyLocalOffset, prefabs);
				this.SetAllyAudioSwitch(false);
				this.SetEnemyAudioSwitch(false);
			}
		}
	}

	private void SetAllyAudioSwitch(bool isOn)
	{
		string eventName = "ablty/general/switch/defend_on_off";
		AudioManager.EventAction eventAction = AudioManager.EventAction.SetSwitch;
		object parameter;
		if (isOn)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Sensei_SyncComponent.SetAllyAudioSwitch(bool)).MethodHandle;
			}
			parameter = "defend_on";
		}
		else
		{
			parameter = "defend_off";
		}
		AudioManager.PostEvent(eventName, eventAction, parameter, this.m_owner.gameObject);
		this.m_lastAllyDashAudioSwitchState = isOn;
	}

	private void SetEnemyAudioSwitch(bool isOn)
	{
		string eventName = "ablty/general/switch/attack_on_off";
		AudioManager.EventAction eventAction = AudioManager.EventAction.SetSwitch;
		object parameter;
		if (isOn)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Sensei_SyncComponent.SetEnemyAudioSwitch(bool)).MethodHandle;
			}
			parameter = "attack_on";
		}
		else
		{
			parameter = "attack_off";
		}
		AudioManager.PostEvent(eventName, eventAction, parameter, this.m_owner.gameObject);
		this.m_lastEnemyDashAudioSwitchState = isOn;
	}

	private void InstantiateVfxIndicators(List<AttachedActorVFXInfo> listToAddTo, JointPopupProperty joint, Vector3 localOffset, List<GameObject> prefabs)
	{
		for (int i = 0; i < prefabs.Count; i++)
		{
			AttachedActorVFXInfo attachedActorVFXInfo = new AttachedActorVFXInfo(prefabs[i], this.m_owner, joint, false, "SenseiDashIndicator_" + i, AttachedActorVFXInfo.FriendOrFoeVisibility.Both);
			if (attachedActorVFXInfo.HasVfxInstance())
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(Sensei_SyncComponent.InstantiateVfxIndicators(List<AttachedActorVFXInfo>, JointPopupProperty, Vector3, List<GameObject>)).MethodHandle;
				}
				attachedActorVFXInfo.SetInstanceLocalPosition(localOffset);
			}
			listToAddTo.Add(attachedActorVFXInfo);
		}
		for (;;)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			break;
		}
	}

	private void SetIndicatorVisibility(List<AttachedActorVFXInfo> vfxInstSet, bool onForAlly, bool onForEnemy)
	{
		vfxInstSet[0].UpdateVisibility(onForAlly, true);
		vfxInstSet[1].UpdateVisibility(onForEnemy, true);
	}

	private void Update()
	{
		if (this.m_owner != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Sensei_SyncComponent.Update()).MethodHandle;
			}
			if (this.m_yingYangDashAbility != null)
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
				bool flag = this.m_owner.IsVisibleToClient();
				if (flag)
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
					if (this.m_owner.GetActorModelData() != null)
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
						flag = this.m_owner.GetActorModelData().IsVisibleToClient();
					}
				}
				bool flag2;
				if (!this.m_owner.IsDead())
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
					flag2 = this.m_owner.IsModelAnimatorDisabled();
				}
				else
				{
					flag2 = true;
				}
				bool flag3 = flag2;
				bool flag4 = this.m_abilityData.GetCooldownRemaining(this.m_yingYangDashActionType) <= 0;
				bool flag5;
				if (!flag3)
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
					flag5 = flag4;
				}
				else
				{
					flag5 = false;
				}
				bool flag6 = flag5;
				bool flag7;
				if (flag)
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
					flag7 = flag6;
				}
				else
				{
					flag7 = false;
				}
				bool flag8 = flag7;
				bool flag9;
				if (flag6)
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
					flag9 = this.m_yingYangDashAbility.CanTargetAlly();
				}
				else
				{
					flag9 = false;
				}
				bool flag10 = flag9;
				bool flag11;
				if (flag6)
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
					flag11 = this.m_yingYangDashAbility.CanTargetEnemy();
				}
				else
				{
					flag11 = false;
				}
				bool flag12 = flag11;
				if (this.m_lastAllyDashAudioSwitchState != flag10)
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
					this.SetAllyAudioSwitch(flag10);
				}
				if (this.m_lastEnemyDashAudioSwitchState != flag12)
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
					this.SetEnemyAudioSwitch(flag12);
				}
				if (flag8)
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
					if (!flag10)
					{
						if (!flag12)
						{
							goto IL_20D;
						}
						for (;;)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							break;
						}
					}
					if (flag10 && flag12)
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
						this.SetIndicatorVisibility(this.m_dashIndicatorsSetA, true, false);
						this.SetIndicatorVisibility(this.m_dashIndicatorsSetB, false, true);
					}
					else if (flag10)
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
						this.SetIndicatorVisibility(this.m_dashIndicatorsSetA, true, false);
						this.SetIndicatorVisibility(this.m_dashIndicatorsSetB, true, false);
					}
					else
					{
						this.SetIndicatorVisibility(this.m_dashIndicatorsSetA, false, true);
						this.SetIndicatorVisibility(this.m_dashIndicatorsSetB, false, true);
					}
					return;
				}
				IL_20D:
				this.SetIndicatorVisibility(this.m_dashIndicatorsSetA, false, false);
				this.SetIndicatorVisibility(this.m_dashIndicatorsSetB, false, false);
			}
		}
	}

	private void UNetVersion()
	{
	}

	public sbyte Networkm_syncCurrentNumOrbs
	{
		get
		{
			return this.m_syncCurrentNumOrbs;
		}
		[param: In]
		set
		{
			uint dirtyBit = 1U;
			if (NetworkServer.localClientActive)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(Sensei_SyncComponent.set_Networkm_syncCurrentNumOrbs(sbyte)).MethodHandle;
				}
				if (!base.syncVarHookGuard)
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
					base.syncVarHookGuard = true;
					this.HookSetNumOrbs(value);
					base.syncVarHookGuard = false;
				}
			}
			base.SetSyncVar<sbyte>(value, ref this.m_syncCurrentNumOrbs, dirtyBit);
		}
	}

	public sbyte Networkm_syncTurnsForSecondYingYangDash
	{
		get
		{
			return this.m_syncTurnsForSecondYingYangDash;
		}
		[param: In]
		set
		{
			base.SetSyncVar<sbyte>(value, ref this.m_syncTurnsForSecondYingYangDash, 2U);
		}
	}

	public bool Networkm_syncLastYingYangDashedToAlly
	{
		get
		{
			return this.m_syncLastYingYangDashedToAlly;
		}
		[param: In]
		set
		{
			base.SetSyncVar<bool>(value, ref this.m_syncLastYingYangDashedToAlly, 4U);
		}
	}

	public float Networkm_syncBideExtraDamagePct
	{
		get
		{
			return this.m_syncBideExtraDamagePct;
		}
		[param: In]
		set
		{
			base.SetSyncVar<float>(value, ref this.m_syncBideExtraDamagePct, 8U);
		}
	}

	public sbyte Networkm_lastPrimaryUsedMode
	{
		get
		{
			return this.m_lastPrimaryUsedMode;
		}
		[param: In]
		set
		{
			base.SetSyncVar<sbyte>(value, ref this.m_lastPrimaryUsedMode, 0x10U);
		}
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Sensei_SyncComponent.OnSerialize(NetworkWriter, bool)).MethodHandle;
			}
			writer.WritePackedUInt32((uint)this.m_syncCurrentNumOrbs);
			writer.WritePackedUInt32((uint)this.m_syncTurnsForSecondYingYangDash);
			writer.Write(this.m_syncLastYingYangDashedToAlly);
			writer.Write(this.m_syncBideExtraDamagePct);
			writer.WritePackedUInt32((uint)this.m_lastPrimaryUsedMode);
			return true;
		}
		bool flag = false;
		if ((base.syncVarDirtyBits & 1U) != 0U)
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
			writer.WritePackedUInt32((uint)this.m_syncCurrentNumOrbs);
		}
		if ((base.syncVarDirtyBits & 2U) != 0U)
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
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)this.m_syncTurnsForSecondYingYangDash);
		}
		if ((base.syncVarDirtyBits & 4U) != 0U)
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
			if (!flag)
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
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.Write(this.m_syncLastYingYangDashedToAlly);
		}
		if ((base.syncVarDirtyBits & 8U) != 0U)
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
			if (!flag)
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
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.Write(this.m_syncBideExtraDamagePct);
		}
		if ((base.syncVarDirtyBits & 0x10U) != 0U)
		{
			if (!flag)
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
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)this.m_lastPrimaryUsedMode);
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Sensei_SyncComponent.OnDeserialize(NetworkReader, bool)).MethodHandle;
			}
			this.m_syncCurrentNumOrbs = (sbyte)reader.ReadPackedUInt32();
			this.m_syncTurnsForSecondYingYangDash = (sbyte)reader.ReadPackedUInt32();
			this.m_syncLastYingYangDashedToAlly = reader.ReadBoolean();
			this.m_syncBideExtraDamagePct = reader.ReadSingle();
			this.m_lastPrimaryUsedMode = (sbyte)reader.ReadPackedUInt32();
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
			this.HookSetNumOrbs((sbyte)reader.ReadPackedUInt32());
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
			this.m_syncTurnsForSecondYingYangDash = (sbyte)reader.ReadPackedUInt32();
		}
		if ((num & 4) != 0)
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
			this.m_syncLastYingYangDashedToAlly = reader.ReadBoolean();
		}
		if ((num & 8) != 0)
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
			this.m_syncBideExtraDamagePct = reader.ReadSingle();
		}
		if ((num & 0x10) != 0)
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
			this.m_lastPrimaryUsedMode = (sbyte)reader.ReadPackedUInt32();
		}
	}
}

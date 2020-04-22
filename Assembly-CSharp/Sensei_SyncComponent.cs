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

	public sbyte Networkm_syncCurrentNumOrbs
	{
		get
		{
			return m_syncCurrentNumOrbs;
		}
		[param: In]
		set
		{
			ref sbyte syncCurrentNumOrbs = ref m_syncCurrentNumOrbs;
			if (NetworkServer.localClientActive)
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
				if (!base.syncVarHookGuard)
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
					base.syncVarHookGuard = true;
					HookSetNumOrbs(value);
					base.syncVarHookGuard = false;
				}
			}
			SetSyncVar(value, ref syncCurrentNumOrbs, 1u);
		}
	}

	public sbyte Networkm_syncTurnsForSecondYingYangDash
	{
		get
		{
			return m_syncTurnsForSecondYingYangDash;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_syncTurnsForSecondYingYangDash, 2u);
		}
	}

	public bool Networkm_syncLastYingYangDashedToAlly
	{
		get
		{
			return m_syncLastYingYangDashedToAlly;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_syncLastYingYangDashedToAlly, 4u);
		}
	}

	public float Networkm_syncBideExtraDamagePct
	{
		get
		{
			return m_syncBideExtraDamagePct;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_syncBideExtraDamagePct, 8u);
		}
	}

	public sbyte Networkm_lastPrimaryUsedMode
	{
		get
		{
			return m_lastPrimaryUsedMode;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_lastPrimaryUsedMode, 16u);
		}
	}

	private void HookSetNumOrbs(sbyte value)
	{
		Networkm_syncCurrentNumOrbs = value;
		m_clientOrbNumAdjust = 0;
	}

	private void Start()
	{
		m_owner = GetComponent<ActorData>();
		if (m_owner != null)
		{
			m_abilityData = m_owner.GetComponent<AbilityData>();
			if (m_abilityData != null)
			{
				m_yingYangDashAbility = (m_abilityData.GetAbilityOfType(typeof(SenseiYingYangDash)) as SenseiYingYangDash);
				m_yingYangDashActionType = m_abilityData.GetActionTypeOfAbility(m_yingYangDashAbility);
			}
			if (m_yingYangDashAbility != null)
			{
				List<GameObject> list = new List<GameObject>();
				list.Add(m_canDashToAllyVfxPrefab);
				list.Add(m_canDashToEnemyVfxPrefab);
				List<GameObject> prefabs = list;
				InstantiateVfxIndicators(m_dashIndicatorsSetA, m_canDashToAllyVfxJoint, m_canDashToAllyVfxLocalOffset, prefabs);
				InstantiateVfxIndicators(m_dashIndicatorsSetB, m_canDashToEnemyVfxJoint, m_canDashToEnemyLocalOffset, prefabs);
				SetAllyAudioSwitch(false);
				SetEnemyAudioSwitch(false);
			}
		}
	}

	private void SetAllyAudioSwitch(bool isOn)
	{
		object parameter;
		if (isOn)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			parameter = "defend_on";
		}
		else
		{
			parameter = "defend_off";
		}
		AudioManager.PostEvent("ablty/general/switch/defend_on_off", AudioManager.EventAction.SetSwitch, parameter, m_owner.gameObject);
		m_lastAllyDashAudioSwitchState = isOn;
	}

	private void SetEnemyAudioSwitch(bool isOn)
	{
		object parameter;
		if (isOn)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			parameter = "attack_on";
		}
		else
		{
			parameter = "attack_off";
		}
		AudioManager.PostEvent("ablty/general/switch/attack_on_off", AudioManager.EventAction.SetSwitch, parameter, m_owner.gameObject);
		m_lastEnemyDashAudioSwitchState = isOn;
	}

	private void InstantiateVfxIndicators(List<AttachedActorVFXInfo> listToAddTo, JointPopupProperty joint, Vector3 localOffset, List<GameObject> prefabs)
	{
		for (int i = 0; i < prefabs.Count; i++)
		{
			AttachedActorVFXInfo attachedActorVFXInfo = new AttachedActorVFXInfo(prefabs[i], m_owner, joint, false, "SenseiDashIndicator_" + i, AttachedActorVFXInfo.FriendOrFoeVisibility.Both);
			if (attachedActorVFXInfo.HasVfxInstance())
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
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				attachedActorVFXInfo.SetInstanceLocalPosition(localOffset);
			}
			listToAddTo.Add(attachedActorVFXInfo);
		}
		while (true)
		{
			switch (7)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	private void SetIndicatorVisibility(List<AttachedActorVFXInfo> vfxInstSet, bool onForAlly, bool onForEnemy)
	{
		vfxInstSet[0].UpdateVisibility(onForAlly, true);
		vfxInstSet[1].UpdateVisibility(onForEnemy, true);
	}

	private void Update()
	{
		if (!(m_owner != null))
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
			if (!(m_yingYangDashAbility != null))
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
				bool flag = m_owner.IsVisibleToClient();
				if (flag)
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
					if (m_owner.GetActorModelData() != null)
					{
						while (true)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							break;
						}
						flag = m_owner.GetActorModelData().IsVisibleToClient();
					}
				}
				int num;
				if (!m_owner.IsDead())
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					num = (m_owner.IsModelAnimatorDisabled() ? 1 : 0);
				}
				else
				{
					num = 1;
				}
				bool flag2 = (byte)num != 0;
				bool flag3 = m_abilityData.GetCooldownRemaining(m_yingYangDashActionType) <= 0;
				int num2;
				if (!flag2)
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
					num2 = (flag3 ? 1 : 0);
				}
				else
				{
					num2 = 0;
				}
				bool flag4 = (byte)num2 != 0;
				int num3;
				if (flag)
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
					num3 = (flag4 ? 1 : 0);
				}
				else
				{
					num3 = 0;
				}
				bool flag5 = (byte)num3 != 0;
				int num4;
				if (flag4)
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					num4 = (m_yingYangDashAbility.CanTargetAlly() ? 1 : 0);
				}
				else
				{
					num4 = 0;
				}
				bool flag6 = (byte)num4 != 0;
				int num5;
				if (flag4)
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
					num5 = (m_yingYangDashAbility.CanTargetEnemy() ? 1 : 0);
				}
				else
				{
					num5 = 0;
				}
				bool flag7 = (byte)num5 != 0;
				if (m_lastAllyDashAudioSwitchState != flag6)
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
					SetAllyAudioSwitch(flag6);
				}
				if (m_lastEnemyDashAudioSwitchState != flag7)
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
					SetEnemyAudioSwitch(flag7);
				}
				if (flag5)
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
					if (!flag6)
					{
						if (!flag7)
						{
							goto IL_020d;
						}
						while (true)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							break;
						}
					}
					if (flag6 && flag7)
					{
						while (true)
						{
							switch (5)
							{
							case 0:
								break;
							default:
								SetIndicatorVisibility(m_dashIndicatorsSetA, true, false);
								SetIndicatorVisibility(m_dashIndicatorsSetB, false, true);
								return;
							}
						}
					}
					if (flag6)
					{
						while (true)
						{
							switch (6)
							{
							case 0:
								break;
							default:
								SetIndicatorVisibility(m_dashIndicatorsSetA, true, false);
								SetIndicatorVisibility(m_dashIndicatorsSetB, true, false);
								return;
							}
						}
					}
					SetIndicatorVisibility(m_dashIndicatorsSetA, false, true);
					SetIndicatorVisibility(m_dashIndicatorsSetB, false, true);
					return;
				}
				goto IL_020d;
				IL_020d:
				SetIndicatorVisibility(m_dashIndicatorsSetA, false, false);
				SetIndicatorVisibility(m_dashIndicatorsSetB, false, false);
				return;
			}
		}
	}

	private void UNetVersion()
	{
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
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					writer.WritePackedUInt32((uint)m_syncCurrentNumOrbs);
					writer.WritePackedUInt32((uint)m_syncTurnsForSecondYingYangDash);
					writer.Write(m_syncLastYingYangDashedToAlly);
					writer.Write(m_syncBideExtraDamagePct);
					writer.WritePackedUInt32((uint)m_lastPrimaryUsedMode);
					return true;
				}
			}
		}
		bool flag = false;
		if ((base.syncVarDirtyBits & 1) != 0)
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
			writer.WritePackedUInt32((uint)m_syncCurrentNumOrbs);
		}
		if ((base.syncVarDirtyBits & 2) != 0)
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
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_syncTurnsForSecondYingYangDash);
		}
		if ((base.syncVarDirtyBits & 4) != 0)
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
			writer.Write(m_syncLastYingYangDashedToAlly);
		}
		if ((base.syncVarDirtyBits & 8) != 0)
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
			writer.Write(m_syncBideExtraDamagePct);
		}
		if ((base.syncVarDirtyBits & 0x10) != 0)
		{
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
			writer.WritePackedUInt32((uint)m_lastPrimaryUsedMode);
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
				switch (1)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					m_syncCurrentNumOrbs = (sbyte)reader.ReadPackedUInt32();
					m_syncTurnsForSecondYingYangDash = (sbyte)reader.ReadPackedUInt32();
					m_syncLastYingYangDashedToAlly = reader.ReadBoolean();
					m_syncBideExtraDamagePct = reader.ReadSingle();
					m_lastPrimaryUsedMode = (sbyte)reader.ReadPackedUInt32();
					return;
				}
			}
		}
		int num = (int)reader.ReadPackedUInt32();
		if ((num & 1) != 0)
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
			HookSetNumOrbs((sbyte)reader.ReadPackedUInt32());
		}
		if ((num & 2) != 0)
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
			m_syncTurnsForSecondYingYangDash = (sbyte)reader.ReadPackedUInt32();
		}
		if ((num & 4) != 0)
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
			m_syncLastYingYangDashedToAlly = reader.ReadBoolean();
		}
		if ((num & 8) != 0)
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
			m_syncBideExtraDamagePct = reader.ReadSingle();
		}
		if ((num & 0x10) == 0)
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
			m_lastPrimaryUsedMode = (sbyte)reader.ReadPackedUInt32();
			return;
		}
	}
}

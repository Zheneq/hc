using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
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
		get { return m_syncCurrentNumOrbs; }
		[param: In]
		set
		{
			if (NetworkServer.localClientActive && !syncVarHookGuard)
			{
				syncVarHookGuard = true;
				HookSetNumOrbs(value);
				syncVarHookGuard = false;
			}
			SetSyncVar(value, ref m_syncCurrentNumOrbs, 1u);
		}
	}

	public sbyte Networkm_syncTurnsForSecondYingYangDash
	{
		get { return m_syncTurnsForSecondYingYangDash; }
		[param: In]
		set { SetSyncVar(value, ref m_syncTurnsForSecondYingYangDash, 2u); }
	}

	public bool Networkm_syncLastYingYangDashedToAlly
	{
		get { return m_syncLastYingYangDashedToAlly; }
		[param: In]
		set { SetSyncVar(value, ref m_syncLastYingYangDashedToAlly, 4u); }
	}

	public float Networkm_syncBideExtraDamagePct
	{
		get { return m_syncBideExtraDamagePct; }
		[param: In]
		set { SetSyncVar(value, ref m_syncBideExtraDamagePct, 8u); }
	}

	public sbyte Networkm_lastPrimaryUsedMode
	{
		get { return m_lastPrimaryUsedMode; }
		[param: In]
		set { SetSyncVar(value, ref m_lastPrimaryUsedMode, 16u); }
	}

	private void HookSetNumOrbs(sbyte value)
	{
		Networkm_syncCurrentNumOrbs = value;
		m_clientOrbNumAdjust = 0;
	}

	private void Start()
	{
		m_owner = GetComponent<ActorData>();
		if (m_owner == null)
		{
			return;
		}
		m_abilityData = m_owner.GetComponent<AbilityData>();
		if (m_abilityData != null)
		{
			m_yingYangDashAbility = m_abilityData.GetAbilityOfType(typeof(SenseiYingYangDash)) as SenseiYingYangDash;
			m_yingYangDashActionType = m_abilityData.GetActionTypeOfAbility(m_yingYangDashAbility);
		}
		if (m_yingYangDashAbility != null)
		{
			List<GameObject> prefabs = new List<GameObject>
			{
				m_canDashToAllyVfxPrefab,
				m_canDashToEnemyVfxPrefab
			};
			InstantiateVfxIndicators(m_dashIndicatorsSetA, m_canDashToAllyVfxJoint, m_canDashToAllyVfxLocalOffset, prefabs);
			InstantiateVfxIndicators(m_dashIndicatorsSetB, m_canDashToEnemyVfxJoint, m_canDashToEnemyLocalOffset, prefabs);
			SetAllyAudioSwitch(false);
			SetEnemyAudioSwitch(false);
		}
	}

	private void SetAllyAudioSwitch(bool isOn)
	{
		object parameter = isOn ? "defend_on" : "defend_off";
		AudioManager.PostEvent("ablty/general/switch/defend_on_off", AudioManager.EventAction.SetSwitch, parameter, m_owner.gameObject);
		m_lastAllyDashAudioSwitchState = isOn;
	}

	private void SetEnemyAudioSwitch(bool isOn)
	{
		object parameter = isOn ? "attack_on" : "attack_off";
		AudioManager.PostEvent("ablty/general/switch/attack_on_off", AudioManager.EventAction.SetSwitch, parameter, m_owner.gameObject);
		m_lastEnemyDashAudioSwitchState = isOn;
	}

	private void InstantiateVfxIndicators(List<AttachedActorVFXInfo> listToAddTo, JointPopupProperty joint, Vector3 localOffset, List<GameObject> prefabs)
	{
		for (int i = 0; i < prefabs.Count; i++)
		{
			AttachedActorVFXInfo attachedActorVFXInfo = new AttachedActorVFXInfo(
				prefabs[i],
				m_owner,
				joint,
				false,
				new StringBuilder().Append("SenseiDashIndicator_").Append(i).ToString(),
				AttachedActorVFXInfo.FriendOrFoeVisibility.Both);
			if (attachedActorVFXInfo.HasVfxInstance())
			{
				attachedActorVFXInfo.SetInstanceLocalPosition(localOffset);
			}
			listToAddTo.Add(attachedActorVFXInfo);
		}
	}

	private void SetIndicatorVisibility(List<AttachedActorVFXInfo> vfxInstSet, bool onForAlly, bool onForEnemy)
	{
		vfxInstSet[0].UpdateVisibility(onForAlly, true);
		vfxInstSet[1].UpdateVisibility(onForEnemy, true);
	}

	private void Update()
	{
		if (m_owner == null || m_yingYangDashAbility == null)
		{
			return;
		}
		bool isVisibleToClient = m_owner.IsActorVisibleToClient();
		if (isVisibleToClient && m_owner.GetActorModelData() != null)
		{
			isVisibleToClient = m_owner.GetActorModelData().IsVisibleToClient();
		}
		bool isDead = m_owner.IsDead() || m_owner.IsInRagdoll();
		bool canCastAbility = !isDead && m_abilityData.GetCooldownRemaining(m_yingYangDashActionType) <= 0;
		bool canTargetAlly = canCastAbility && m_yingYangDashAbility.CanTargetAlly();
		bool canTargetEnemy = canCastAbility && m_yingYangDashAbility.CanTargetEnemy();
		if (m_lastAllyDashAudioSwitchState != canTargetAlly)
		{
			SetAllyAudioSwitch(canTargetAlly);
		}
		if (m_lastEnemyDashAudioSwitchState != canTargetEnemy)
		{
			SetEnemyAudioSwitch(canTargetEnemy);
		}
		if (isVisibleToClient && canCastAbility && (canTargetAlly || canTargetEnemy))
		{
			if (canTargetAlly && canTargetEnemy)
			{
				SetIndicatorVisibility(m_dashIndicatorsSetA, true, false);
				SetIndicatorVisibility(m_dashIndicatorsSetB, false, true);
			}
			else if (canTargetAlly)
			{
				SetIndicatorVisibility(m_dashIndicatorsSetA, true, false);
				SetIndicatorVisibility(m_dashIndicatorsSetB, true, false);
			}
			else
			{
				SetIndicatorVisibility(m_dashIndicatorsSetA, false, true);
				SetIndicatorVisibility(m_dashIndicatorsSetB, false, true);
			}
		}
		else
		{
			SetIndicatorVisibility(m_dashIndicatorsSetA, false, false);
			SetIndicatorVisibility(m_dashIndicatorsSetB, false, false);
		}
	}

	private void UNetVersion()
	{
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		if (forceAll)
		{
			writer.WritePackedUInt32((uint)m_syncCurrentNumOrbs);
			writer.WritePackedUInt32((uint)m_syncTurnsForSecondYingYangDash);
			writer.Write(m_syncLastYingYangDashedToAlly);
			writer.Write(m_syncBideExtraDamagePct);
			writer.WritePackedUInt32((uint)m_lastPrimaryUsedMode);
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
			writer.WritePackedUInt32((uint)m_syncCurrentNumOrbs);
		}
		if ((syncVarDirtyBits & 2) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_syncTurnsForSecondYingYangDash);
		}
		if ((syncVarDirtyBits & 4) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			writer.Write(m_syncLastYingYangDashedToAlly);
		}
		if ((syncVarDirtyBits & 8) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			writer.Write(m_syncBideExtraDamagePct);
		}
		if ((syncVarDirtyBits & 0x10) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_lastPrimaryUsedMode);
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
			m_syncCurrentNumOrbs = (sbyte)reader.ReadPackedUInt32();
			m_syncTurnsForSecondYingYangDash = (sbyte)reader.ReadPackedUInt32();
			m_syncLastYingYangDashedToAlly = reader.ReadBoolean();
			m_syncBideExtraDamagePct = reader.ReadSingle();
			m_lastPrimaryUsedMode = (sbyte)reader.ReadPackedUInt32();
			return;
		}
		int num = (int)reader.ReadPackedUInt32();
		if ((num & 1) != 0)
		{
			HookSetNumOrbs((sbyte)reader.ReadPackedUInt32());
		}
		if ((num & 2) != 0)
		{
			m_syncTurnsForSecondYingYangDash = (sbyte)reader.ReadPackedUInt32();
		}
		if ((num & 4) != 0)
		{
			m_syncLastYingYangDashedToAlly = reader.ReadBoolean();
		}
		if ((num & 8) != 0)
		{
			m_syncBideExtraDamagePct = reader.ReadSingle();
		}

		if ((num & 0x10) != 0)
		{
			m_lastPrimaryUsedMode = (sbyte)reader.ReadPackedUInt32();
		}
	}
}

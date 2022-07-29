// ROGUES
// SERVER
using UnityEngine;

public class Passive_Claymore : Passive
{
	[Header("-- Stack Indicator Ability")]
	public AbilityData.ActionType m_stackIndicatorActionType = AbilityData.ActionType.ABILITY_4;
	[Header("-- Sequence")]
	public GameObject m_selfHealSequencePrefab;
	public GameObject m_ultScaleSequencePrefab;
#if SERVER
	// added in rogues
	private AbilityData m_abilityData;
	private ClaymoreSilenceLaser m_daggerAbility;
	private AbilityData.ActionType m_cdrTargetActionType = AbilityData.ActionType.INVALID_ACTION;
	private int m_pendingCdrForDaggerTrigger;

	// added in rogues
	public void SetPendingCdrDaggerTrigger(int pendingCdr, AbilityData.ActionType onAction)
	{
		m_pendingCdrForDaggerTrigger = pendingCdr;
		m_cdrTargetActionType = onAction;
	}

	// added in rogues
	protected override void OnStartup()
	{
		base.OnStartup();
		m_abilityData = Owner.GetAbilityData();
		if (m_abilityData != null)
		{
			m_daggerAbility = m_abilityData.GetAbilityOfType(typeof(ClaymoreSilenceLaser)) as ClaymoreSilenceLaser;
		}
	}

	// added in rogues
	public override void OnTurnStart()
	{
		base.OnTurnStart();
		if (m_daggerAbility != null && m_abilityData != null && m_pendingCdrForDaggerTrigger > 0)
		{
			int cooldownRemaining = m_abilityData.GetCooldownRemaining(m_cdrTargetActionType);
			int cooldownRemainingOverride = Mathf.Max(0, cooldownRemaining - m_pendingCdrForDaggerTrigger);
			m_abilityData.OverrideCooldown(m_cdrTargetActionType, cooldownRemainingOverride);
		}
		m_pendingCdrForDaggerTrigger = 0;
	}

	// added in rogues
	public AbilityData.ActionType GetHitIndicatorActionType()
	{
		return AbilityData.ActionType.INVALID_ACTION;
	}
#endif
}

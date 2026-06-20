// ROGUES
// SERVER
using UnityEngine;

public class Passive_Archer : Passive
{
	[HideInInspector]
	public int m_turnShieldGenEffectExpires = -1;
	
	private Archer_SyncComponent m_syncComp;
	private ArcherDashAndShoot m_dashAbility;
	private AbilityData.ActionType m_dashAbilityAction;
	
# if SERVER
	// added in rogues
	protected override void OnStartup()
	{
		base.OnStartup();
		m_syncComp = Owner.GetComponent<Archer_SyncComponent>();
		AbilityData abilityData = Owner.GetAbilityData();
		m_dashAbility = abilityData != null
			? abilityData.GetAbilityOfType(typeof(ArcherDashAndShoot)) as ArcherDashAndShoot
			: null;
		m_dashAbilityAction = abilityData != null
			? abilityData.GetActionTypeOfAbility(m_dashAbility)
			: AbilityData.ActionType.INVALID_ACTION;
	}

	// added in rogues
	public override void OnTurnStart()
	{
		base.OnTurnStart();
		if (m_turnShieldGenEffectExpires >= 0 && GameFlowData.Get().CurrentTurn >= m_turnShieldGenEffectExpires)
		{
			if (m_syncComp != null)
			{
				m_syncComp.ClearShieldGeneratorTargets();
			}
			m_turnShieldGenEffectExpires = -1;
		}
		if (m_dashAbility != null)
		{
			int cooldownAdjustmentEachTurnIfUnderHealthThreshold = m_dashAbility.GetCooldownAdjustmentEachTurnIfUnderHealthThreshold();
			float healthThresholdForCooldownOverride = m_dashAbility.GetHealthThresholdForCooldownOverride();
			if (cooldownAdjustmentEachTurnIfUnderHealthThreshold != 0
			    && healthThresholdForCooldownOverride > 0f
			    && Owner.GetHpPortionInServerResolution() <= healthThresholdForCooldownOverride)
			{
				ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(Owner, Owner.GetFreePos()));
				MiscHitEventData_AddToCasterCooldown hitEvent = new MiscHitEventData_AddToCasterCooldown(
					m_dashAbilityAction, cooldownAdjustmentEachTurnIfUnderHealthThreshold);
				actorHitResults.AddMiscHitEvent(hitEvent);
				MovementResults.SetupAndExecuteAbilityResultsOutsideResolution(Owner, Owner, actorHitResults, m_dashAbility);
			}
		}
	}
#endif
}

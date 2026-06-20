// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

// empty in reactor
public class Passive_Sniper : Passive
{
#if SERVER
	// added in rogues
	private AbilityData m_abilityData;
	// added in rogues
	private SniperGhillieSuit m_stealthAbility;
	// added in rogues
	private bool m_hasResetCooldownThisLife;

	// added in rogues
	protected override void OnStartup()
	{
		m_abilityData = Owner.GetAbilityData();
		if (m_abilityData != null)
		{
			m_stealthAbility = m_abilityData.GetAbilityOfType(typeof(SniperGhillieSuit)) as SniperGhillieSuit;
		}
	}

	// added in rogues
	public override void OnTurnEnd()
	{
		base.OnTurnEnd();
		HandleStealthAbilityCooldown();
	}

	// added in rogues
	public override void OnActorRespawn()
	{
		base.OnActorRespawn();
		m_hasResetCooldownThisLife = false;
	}

	// added in rogues
	private void HandleStealthAbilityCooldown()
	{
		if (m_abilityData != null
		    && !m_hasResetCooldownThisLife
		    && m_stealthAbility != null
		    && m_stealthAbility.GetCooldownResetHealthThreshold() > 0f)
		{
			int healthThreshold = Mathf.RoundToInt(Owner.GetMaxHitPoints() * m_stealthAbility.GetCooldownResetHealthThreshold());
			if (Owner.HitPoints < healthThreshold)
			{
				AbilityData.ActionType actionTypeOfAbility = Owner.GetAbilityData().GetActionTypeOfAbility(m_stealthAbility);
				Owner.GetAbilityData().OverrideCooldown(actionTypeOfAbility, 0);
				m_hasResetCooldownThisLife = true;
			}
		}
	}

	// added in rogues
	public override void OnTurnStart()
	{
		if (!Owner.IsDead())
		{
			if (!Owner.IsActorVisibleToAnyEnemy())
			{
				Owner.GetFreelancerStats().IncrementValueOfStat(FreelancerStats.SniperStats.DecisionPhasesNotVisibleToEnemies);
			}
			List<ActorData> actorsInRadius = AreaEffectUtils.GetActorsInRadius(
				Owner.GetLoSCheckPos(), 4f, true, Owner, Owner.GetOtherTeams(), null);
			if (actorsInRadius != null && actorsInRadius.Count == 0)
			{
				Owner.GetFreelancerStats().IncrementValueOfStat(FreelancerStats.SniperStats.DecisionPhasesWithNoNearbyEnemies);
			}
		}
	}
#endif
}

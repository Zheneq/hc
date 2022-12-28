// ROGUES
// SERVER
using UnityEngine;

// empty in reactor
public class Passive_Soldier : Passive
{
#if SERVER
	// added in rogues
	private AbilityData m_abilityData;
	// added in rogues
	private SoldierStimPack m_stimAbility;
	// added in rogues
	private bool m_hasResetCooldownThisLife;

	// added in rogues
	protected override void OnStartup()
	{
		m_abilityData = Owner.GetAbilityData();
		if (m_abilityData != null)
		{
			m_stimAbility = (m_abilityData.GetAbilityOfType(typeof(SoldierStimPack)) as SoldierStimPack);
		}
	}

	// added in rogues
	public override void OnTurnEnd()
	{
		base.OnTurnEnd();
		HandleStimPackCooldown();
	}

	// added in rogues
	public override void OnActorRespawn()
	{
		base.OnActorRespawn();
		m_hasResetCooldownThisLife = false;
	}

	// added in rogues
	private void HandleStimPackCooldown()
	{
		if (m_abilityData != null
		    && !m_hasResetCooldownThisLife
		    && m_stimAbility != null
		    && m_stimAbility.GetCooldownResetHealthThreshold() > 0f)
		{
			int num = Mathf.RoundToInt(Owner.GetMaxHitPoints() * m_stimAbility.GetCooldownResetHealthThreshold());
			if (Owner.HitPoints < num)
			{
				AbilityData.ActionType actionTypeOfAbility = Owner.GetAbilityData().GetActionTypeOfAbility(m_stimAbility);
				Owner.GetAbilityData().OverrideCooldown(actionTypeOfAbility, 0);
				m_hasResetCooldownThisLife = true;
			}
		}
	}
#endif
}

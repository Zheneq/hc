// ROGUES
// SERVER
using UnityEngine;

public class Passive_Scoundrel : Passive
{
	public int m_trapwireLastCastTurn = -1;
	public bool m_trapwireDidDamage;
#if SERVER
	// custom
	public bool m_damagedThisTurn;
	
	// added in rogues
	private ScoundrelTrapWire m_trapwireAbility;
	// custom
	private AbilityData m_abilityData;
	// custom
	private static readonly AbilityData.ActionType s_evasionAbilityType = AbilityData.ActionType.ABILITY_3;

	// added in rogues
	protected override void OnStartup()
	{
		AbilityData abilityData = Owner.GetAbilityData();
		m_abilityData = abilityData; // custom
		if (abilityData != null)
		{
			m_trapwireAbility = abilityData.GetAbilityOfType(typeof(ScoundrelTrapWire)) as ScoundrelTrapWire;
		}
	}

	// added in rogues
	public override void OnDamagedOther(ActorData damageTarget, DamageSource damageSource, int damageAmount)
	{
		base.OnDamagedOther(damageTarget, damageSource, damageAmount);
		if (damageSource.Ability != null && damageSource.Ability.GetType() == typeof(ScoundrelTrapWire))
		{
			m_trapwireDidDamage = true;
		}
	}

	// custom
	public override void OnDamaged(ActorData damageCaster, DamageSource damageSource, int damageAmount)
	{
		base.OnDamaged(damageCaster, damageSource, damageAmount);
		m_damagedThisTurn = true;
	}

	// added in rogues
	public override void OnTurnStart()
	{
		base.OnTurnStart();
		if (m_trapwireAbility != null
		    && !m_trapwireDidDamage
		    && m_trapwireLastCastTurn > 0
		    && GameFlowData.Get().CurrentTurn - m_trapwireLastCastTurn == m_trapwireAbility.ModdedBarrierData().m_maxDuration)
		{
			m_trapwireAbility.TrapwireExpiredWithoutHitting();
		}
		
		// custom 
		if (m_damagedThisTurn)
		{
			int cd = m_abilityData.GetCooldownRemaining(s_evasionAbilityType);
			if (cd > 0)
			{
				cd = Mathf.Max(0, cd - 1);
				m_abilityData.OverrideCooldown(s_evasionAbilityType, cd);
			}
		}
		m_damagedThisTurn = false;
		// end custom
	}
#endif
}

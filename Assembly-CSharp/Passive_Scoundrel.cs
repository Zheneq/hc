// ROGUES
// SERVER
public class Passive_Scoundrel : Passive
{
	public int m_trapwireLastCastTurn = -1;
	public bool m_trapwireDidDamage;
#if SERVER
	// added in rogues
	private ScoundrelTrapWire m_trapwireAbility;

	// added in rogues
	protected override void OnStartup()
	{
		AbilityData abilityData = Owner.GetAbilityData();
		if (abilityData != null)
		{
			m_trapwireAbility = (abilityData.GetAbilityOfType(typeof(ScoundrelTrapWire)) as ScoundrelTrapWire);
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
	}
#endif
}

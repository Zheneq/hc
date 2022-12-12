// ROGUES
// SERVER
public class Passive_SpaceMarineHeatOfBattle : Passive
{
	public StandardActorEffectData m_buffEffectData;
	
#if SERVER
	// added in rogues
	private bool m_addedStatModsThisTurn;
	// added in rogues
	private int m_healthPreviousTurn;

	// added in rogues
	protected override void OnStartup()
	{
		m_healthPreviousTurn = Owner.HitPoints;
	}

	// added in rogues
	public override void OnTurnStart()
	{
		m_addedStatModsThisTurn = false;
		base.OnTurnStart();
		SpaceMarineJetpack component = Owner.GetComponent<SpaceMarineJetpack>();
		if (component != null
		    && component.CooldownResetHealthThreshold() > 0
		    && !Owner.IsDead())
		{
			int healthThreshold = component.CooldownResetHealthThreshold();
			if (m_healthPreviousTurn >= healthThreshold && Owner.HitPoints < healthThreshold)
			{
				AbilityData abilityData = Owner.GetComponent<AbilityData>();
				if (abilityData != null)
				{
					abilityData.SetCooldown(abilityData.GetActionTypeOfAbility(component), 0);
				}
			}
		}
		m_healthPreviousTurn = Owner.HitPoints;
	}

	// added in rogues
	public override void OnDamagedOther(ActorData damageTarget, DamageSource damageSource, int damageAmount)
	{
		base.OnDamagedOther(damageTarget, damageSource, damageAmount);
		if (!m_addedStatModsThisTurn)
		{
			StandardActorEffect effect = new StandardActorEffect(AsEffectSource(), Owner.GetCurrentBoardSquare(), Owner, Owner, m_buffEffectData);
			ServerEffectManager.Get().ApplyEffect(effect);
			m_addedStatModsThisTurn = true;
		}
	}
#endif
}

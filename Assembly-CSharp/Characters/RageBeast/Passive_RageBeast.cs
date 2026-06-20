// ROGUES
// SERVER
public class Passive_RageBeast : Passive
{
	public int m_techPointLossOnRespawnIfFull;
	
#if SERVER
	// added in rogues
	private RageBeastUltimate m_ultAbility;

	// added in rogues
	protected override void OnStartup()
	{
		base.OnStartup();
		AbilityData abilityData = GetComponent<AbilityData>();
		if (abilityData != null)
		{
			m_ultAbility = abilityData.GetAbilityOfType(typeof(RageBeastUltimate)) as RageBeastUltimate;
		}
	}

	// added in rogues
	public override void OnActorRespawn()
	{
		if (m_techPointLossOnRespawnIfFull <= 0
		    || m_ultAbility == null
		    || !m_ultAbility.ShouldAutoQueueIfValid())
		{
			return;
		}
		
		int num = Owner.GetTechPointRegen();
		if (Owner.GetAbilityData())
		{
			num += Owner.GetAbilityData().GetTechPointRegenFromAbilities();
		}
		if (Owner.GetActorStatus() != null && GameWideData.Get().m_useEnergyStatusForPassiveRegen)
		{
			ServerGameplayUtils.EnergyStatAdjustments energyStatAdjustments = new ServerGameplayUtils.EnergyStatAdjustments(Owner, Owner);
			num = ServerCombatManager.Get().CalcTechPointGain(Owner, num, AbilityData.ActionType.INVALID_ACTION, energyStatAdjustments);
			energyStatAdjustments.CalculateAdjustments();
			energyStatAdjustments.ApplyStatAdjustments();
		}
		if (Owner.TechPoints + num >= Owner.GetMaxTechPoints())
		{
			Owner.SetTechPoints(Owner.GetMaxTechPoints() - m_techPointLossOnRespawnIfFull - num);
		}
	}

	// added in rogues
	public override void OnDamaged(ActorData damageCaster, DamageSource damageSource, int damageAmount)
	{
		base.OnDamaged(damageCaster, damageSource, damageAmount);
		int num = Owner.GetActorStats().CalculateEnergyGainOnDamage(damageAmount, null);
		if (num > 0)
		{
			Owner.GetFreelancerStats().AddToValueOfStat(FreelancerStats.RageBeastStats.EnergyGainedFromDamageTaken, num);
		}
	}
#endif
}

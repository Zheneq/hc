// ROGUES
// SERVER
using UnityEngine;

#if SERVER
// added in rogues
public class ExoShieldEffect : StandardActorEffect
{
	private ExoShield m_shieldAbility;
	private int m_maxRefundedAbsorb;
	private int m_cdrIfShieldNotUsed;
	private int m_shieldLostPerEnergyGain;
	private int m_maxShieldLostForEnergyGain;
	private int m_damageAmountSoFar;
	private int m_energyGainSoFar;
	private AbilityData m_abilityData;
	private AbilityData.ActionType m_actionType;

	public ExoShieldEffect(
		EffectSource parent,
		BoardSquare targetSquare,
		ActorData target,
		ActorData caster,
		StandardActorEffectData data,
		ExoShield parentAbility,
		int cdrIfShieldNotUsed,
		int shieldLostPerEnergyGain,
		int maxShieldLostForEnergyGain,
		AbilityData.ActionType actionType)
		: base(parent, targetSquare, target, caster, data)
	{
		m_maxRefundedAbsorb = data.m_absorbAmount;
		m_shieldAbility = parentAbility;
		m_cdrIfShieldNotUsed = cdrIfShieldNotUsed;
		m_shieldLostPerEnergyGain = shieldLostPerEnergyGain;
		m_maxShieldLostForEnergyGain = maxShieldLostForEnergyGain;
		m_abilityData = caster.GetAbilityData();
		m_actionType = actionType;
	}

	public override void OnEnd()
	{
		float num = 0f;
		if (m_shieldAbility != null)
		{
			num = m_shieldAbility.GetAbsorbToTechPointConversionRate();
		}
		if (num > 0f)
		{
			int num2 = Mathf.Min(Caster.AbsorbPoints, m_maxRefundedAbsorb);
			Caster.SetTechPoints(Caster.TechPoints + Mathf.RoundToInt(num2 * num), true, Caster, "ExoShieldEffect_refund");
		}
		base.OnEnd();
		if (Absorbtion.m_absorbAmount > 0)
		{
			if (Absorbtion.m_absorbAmount == Absorbtion.m_absorbRemaining
			    && m_cdrIfShieldNotUsed > 0
			    && m_abilityData != null
			    && m_actionType != AbilityData.ActionType.INVALID_ACTION)
			{
				int num3 = m_abilityData.GetCooldownRemaining(m_actionType);
				if (num3 > 0)
				{
					num3 -= m_cdrIfShieldNotUsed;
					num3 = Mathf.Max(0, num3);
					m_abilityData.OverrideCooldown(m_actionType, num3);
				}
			}
			int num4 = Absorbtion.m_absorbAmount - Absorbtion.m_absorbRemaining;
			if (m_maxShieldLostForEnergyGain > 0)
			{
				num4 = Mathf.Min(m_maxShieldLostForEnergyGain, num4);
			}
			if (num4 > 0 && m_shieldLostPerEnergyGain > 0 && Target != null)
			{
				int num5 = num4 / m_shieldLostPerEnergyGain;
				num5 -= m_energyGainSoFar;
				if (num5 > 0)
				{
					int techPoints = Target.TechPoints;
					int num6 = Mathf.Min(techPoints + num5, Target.GetMaxTechPoints());
					if (techPoints != num6)
					{
						Target.SetTechPoints(num6);
					}
				}
			}
		}
		m_damageAmountSoFar = 0;
		m_energyGainSoFar = 0;
	}

	protected override void OnDamaged(ActorData damageTarget, ActorData damageCaster, DamageSource damageSource, int damageAmount, ActorHitResults actorHitResults)
	{
		base.OnDamaged(damageTarget, damageCaster, damageSource, damageAmount, actorHitResults);
		if (m_shieldLostPerEnergyGain > 0 && damageAmount > m_shieldLostPerEnergyGain && m_damageAmountSoFar < Absorbtion.m_absorbAmount)
		{
			ActorHitResults shieldActorHitResults = new ActorHitResults(new ActorHitParameters(Target, Target.GetFreePos()));
			int energyGain = damageAmount / m_shieldLostPerEnergyGain;
			shieldActorHitResults.AddTechPointGain(energyGain);
			m_energyGainSoFar += energyGain;
			MovementResults.SetupAndExecuteAbilityResultsOutsideResolution(
				Target,
				Caster,
				shieldActorHitResults,
				Caster.GetAbilityData().GetAbilityOfType(typeof(ExoShield)));
		}
		m_damageAmountSoFar += damageAmount;
	}
}
#endif

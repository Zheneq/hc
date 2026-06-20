// ROGUES
// SERVER
using UnityEngine;

public class Passive_Nanosmith : Passive
{
	[Header("-- Shield Regen")]
	public int m_shieldGainPerTurn;
	public int m_shieldGainLimit = 40;
	[Header("-- Set Shield on Game Start, Limit still applies")]
	public bool m_setShieldOnGameStart = true;
	public bool m_setShieldOnRespawn = true;
	public int m_shieldGainOnGameStart;

	private int m_shieldsApplied;
	private int m_shieldsThatAbsorbedDamage;

#if SERVER
	// added in rogues
	public override void OnTurnStart()
	{
		base.OnTurnStart();
		if (!Owner.IsDead())
		{
			int shieldGainAmount = m_shieldGainPerTurn;
			if (GameFlowData.Get().CurrentTurn == 1 && m_setShieldOnGameStart)
			{
				shieldGainAmount = m_shieldGainOnGameStart;
			}
			ApplyEffectForAbsorb(shieldGainAmount);
		}
	}

	// added in rogues
	public override void OnActorRespawn()
	{
		if (m_setShieldOnRespawn)
		{
			ApplyEffectForAbsorb(m_shieldGainOnGameStart);
		}
	}

	// added in rogues
	private void ApplyEffectForAbsorb(int shieldGainAmount)
	{
		if (shieldGainAmount > 0)
		{
			int amount = Mathf.Min(shieldGainAmount, m_shieldGainLimit - Owner.AbsorbPoints);
			if (amount > 0)
			{
				StandardActorEffectData standardActorEffectData = new StandardActorEffectData();
				standardActorEffectData.SetValues(
					"Nanosmith Autogen Shield",
					0,
					0,
					0,
					0,
					ServerCombatManager.HealingType.Effect,
					0,
					amount,
					new AbilityStatMod[0],
					new StatusType[0],
					StandardActorEffectData.StatusDelayMode.DefaultBehavior);
				StandardActorEffect effect = new StandardActorEffect(
					AsEffectSource(),
					Owner.GetCurrentBoardSquare(),
					Owner,
					Owner,
					standardActorEffectData);
				ServerEffectManager.Get().ApplyEffect(effect);
			}
		}
	}

	// added in rogues
	public void OnShieldApplied()
	{
		m_shieldsApplied++;
		ReclacShieldGuessPercentStat();
	}

	// added in rogues
	public void OnShieldAbsorbedDamage()
	{
		m_shieldsThatAbsorbedDamage++;
		ReclacShieldGuessPercentStat();
	}

	// added in rogues
	public void ReclacShieldGuessPercentStat()
	{
		int newVal = m_shieldsApplied != 0
			? Mathf.RoundToInt(m_shieldsThatAbsorbedDamage / (float)(m_shieldsApplied + m_shieldsThatAbsorbedDamage) * 100f)
			: 0;
		Owner.GetFreelancerStats().SetValueOfStat(FreelancerStats.NanoSmithStats.PercentageOfTimesShieldedAllyWasDamaged, newVal);
	}
#endif
}

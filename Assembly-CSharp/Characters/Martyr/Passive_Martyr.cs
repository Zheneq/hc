// ROGUES
// SERVER
using UnityEngine;

public class Passive_Martyr : Passive
{
	public enum CrystalGainMode
	{
		ByEnergy,
		ByDamageTaken
	}

	[Header("-- How to gain crystals --")]
	public CrystalGainMode m_crystalGainMode;
	[Header("-- [ByEnergy] Energy gain on Damage --")]
	public float m_energyGainPerDamageTaken = 1f;
	public bool m_skipEnergyGainIfUsingUlt;
	[Header("-- [ByEnergy] Energy Adjust On Respawn (final = Mult * Current + Adjust) --")]
	public int m_energyAdjustOnRespawn;
	public float m_energyMultOnRespawn = 1f;
	[Header("-- [ByEnergy] energy amount per crystal")]
	public int m_energyToCrystalConversion = 20;
	[Header("-- [ByDamageTaken] Crystals gained at end of turn = damage received this turn / conversion")]
	public float m_damageToCrystalConversion = 20f;
	[Header("-- Energy gained per crystal gained on beginning of turn")]
	public int m_energyGainPerCrystal;
	[Header("-- Crystal Count and Increments --")]
	public int m_maxCrystals;
	[Tooltip("The cap on crystal gain each turn. 0 means no cap.")]
	public int m_maxCrystalsGainedEachTurn;
	[Tooltip("A passive gain of crystals on each turn start. Can be negative.")]
	public int m_automaticCrystalGainedEachTurn;
	[Tooltip("All abilities get the crystal bonus even if SpendCrystals isn't used this turn")]
	public bool m_automaticCrystalBonus = true;

	private Martyr_SyncComponent m_syncComponent;
	
#if SERVER
	// added in rogues
	private AbilityData m_abilityData;
	private MartyrSpendCrystals m_spendCrystalAbility;
	private AbilityData.ActionType m_spendCrystalActionType;
	private MartyrProtectAlly m_protectAllyAbility;
	private int m_damageFromRedirectThisTurn;
	private MartyrAoeOnReactHit m_aoeReactAbility;
	private AbilityData.ActionType m_aoeReactActionType;

	// added in rogues
	internal bool m_aoeReactEndedWithoutTriggering;
#endif

	public int DamageReceivedThisTurn
	{
		get;
		set;
	}

#if SERVER
	// added in rogues
	protected override void OnStartup()
	{
		base.OnStartup();
		m_syncComponent = GetComponent<Martyr_SyncComponent>();
		m_abilityData = GetComponent<AbilityData>();
		if (m_abilityData != null)
		{
			m_spendCrystalAbility = (m_abilityData.GetAbilityOfType(typeof(MartyrSpendCrystals)) as MartyrSpendCrystals);
			m_spendCrystalActionType = m_abilityData.GetActionTypeOfAbility(m_spendCrystalAbility);
			m_protectAllyAbility = (m_abilityData.GetAbilityOfType(typeof(MartyrProtectAlly)) as MartyrProtectAlly);
			m_aoeReactAbility = (m_abilityData.GetAbilityOfType(typeof(MartyrAoeOnReactHit)) as MartyrAoeOnReactHit);
			m_aoeReactActionType = m_abilityData.GetActionTypeOfAbility(m_aoeReactAbility);
		}
	}

	// added in rogues
	public override void OnTurnStart()
	{
		base.OnTurnStart();
		int numCrystalsGainedThisTurn = m_automaticCrystalGainedEachTurn;
		int numCrystals = m_syncComponent.DamageCrystals;
		if (m_crystalGainMode == CrystalGainMode.ByDamageTaken && m_damageToCrystalConversion > 0f)
		{
			numCrystalsGainedThisTurn += Mathf.FloorToInt(DamageReceivedThisTurn / m_damageToCrystalConversion);
			if (m_maxCrystalsGainedEachTurn > 0 && m_maxCrystalsGainedEachTurn < numCrystalsGainedThisTurn)
			{
				numCrystalsGainedThisTurn = m_maxCrystalsGainedEachTurn;
			}
			numCrystals += numCrystalsGainedThisTurn;
		}
		else if (m_crystalGainMode == CrystalGainMode.ByEnergy && m_energyToCrystalConversion > 0)
		{
			numCrystals = Owner.TechPoints / m_energyToCrystalConversion;
		}
		if (m_maxCrystals > 0 && m_maxCrystals < numCrystals)
		{
			numCrystals = m_maxCrystals;
		}
		int numCrystalGained = numCrystals - m_syncComponent.DamageCrystals;
		if (numCrystalGained > 0 && m_energyGainPerCrystal > 0)
		{
			int techPointGain = m_energyGainPerCrystal * numCrystalGained;
			int techPoints = Mathf.Clamp(Owner.TechPoints + techPointGain, 0, Owner.GetMaxTechPoints());
			Owner.SetTechPoints(techPoints);
		}
		m_syncComponent.NetworkDamageCrystals = numCrystals;
		if (Owner.TechPoints >= Owner.GetMaxTechPoints())
		{
			Martyr_SyncComponent syncComponent = m_syncComponent;
			syncComponent.Networkm_syncNumTurnsAtFullEnergy = syncComponent.m_syncNumTurnsAtFullEnergy + 1;
		}
		else
		{
			m_syncComponent.Networkm_syncNumTurnsAtFullEnergy = 0;
		}
		HandleTurnStartForDamageRedirectAbility();
		HandleTurnStartForAoeReactAbility();
		DamageReceivedThisTurn = 0;
		m_damageFromRedirectThisTurn = 0;
		m_syncComponent.NetworkCrystalsSpentThisTurn = m_automaticCrystalBonus;
		if (Owner.TechPoints == Owner.GetMaxTechPoints())
		{
			Owner.GetFreelancerStats().IncrementValueOfStat(FreelancerStats.MartyrStats.TurnsWithMaxEnergy);
		}
	}

	// added in rogues
	private void HandleTurnStartForDamageRedirectAbility()
	{
		if (m_protectAllyAbility != null && m_protectAllyAbility.GetHealOnTurnStartPerRedirectDamage() > 0f && !Owner.IsDead())
		{
			int healing = Mathf.RoundToInt(m_damageFromRedirectThisTurn * m_protectAllyAbility.GetHealOnTurnStartPerRedirectDamage());
			if (healing > 0 && Mathf.Clamp(Owner.HitPoints + healing, 0, Owner.GetMaxHitPoints()) != Owner.HitPoints)
			{
				ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(Owner, Owner.GetFreePos()));
				actorHitResults.SetBaseHealing(healing);
				MovementResults.SetupAndExecuteAbilityResultsOutsideResolution(Owner, Owner, actorHitResults, m_protectAllyAbility, true, null, null);
			}
		}
	}

	// added in rogues
	private void HandleTurnStartForAoeReactAbility()
	{
		if (m_aoeReactAbility != null
			&& m_aoeReactAbility.GetCdrIfNoReactionTriggered() > 0
			&& m_aoeReactEndedWithoutTriggering)
		{
			int cd = m_abilityData.GetCooldownRemaining(m_aoeReactActionType);
			if (cd > 0)
			{
				cd = Mathf.Max(0, cd - m_aoeReactAbility.GetCdrIfNoReactionTriggered());
				m_abilityData.OverrideCooldown(m_aoeReactActionType, cd);
			}
		}
		m_aoeReactEndedWithoutTriggering = false;
	}

	// added in rogues
	public override void OnAbilityPhaseEnd(AbilityPriority phase)
	{
		base.OnAbilityPhaseEnd(phase);
		if (m_abilityData != null
			&& m_spendCrystalAbility != null
			&& m_spendCrystalAbility.GetRunPriority() == phase && m_abilityData.HasQueuedAbilityOfType(typeof(MartyrSpendCrystals)))  // , true in rogues
		{
			if (!m_automaticCrystalBonus && m_syncComponent.CrystalsSpentThisTurn)
			{
				bool flag = false;
				for (int i = 0; i <= AbilityData.ABILITY_4; i++)
				{
					if (i != (int)m_spendCrystalActionType)
					{
						flag = m_abilityData.HasQueuedAction((AbilityData.ActionType)i);  // , true in rogues
					}
				}
				if (flag)
				{
					m_syncComponent.NetworkDamageCrystals = 0;
				}
			}
			if (m_automaticCrystalBonus && m_abilityData.HasQueuedAbilityOfType(typeof(MartyrSpendCrystals)))  // , true in rogues
			{
				m_syncComponent.NetworkDamageCrystals = 0;
			}
		}
	}

	// added in rogues
	public override void OnActorRespawn()
	{
		if (m_crystalGainMode == CrystalGainMode.ByEnergy)
		{
			int techPoints = Owner.TechPoints;
			if (m_energyMultOnRespawn >= 0f)
			{
				techPoints = Mathf.RoundToInt(Owner.TechPoints * m_energyMultOnRespawn);
			}
			techPoints = Mathf.Clamp(techPoints + m_energyAdjustOnRespawn, 0, Owner.GetMaxTechPoints());
			if (techPoints != Owner.TechPoints)
			{
				Owner.SetTechPoints(techPoints);
				if (m_energyToCrystalConversion > 0)
				{
					int num2 = Owner.TechPoints / m_energyToCrystalConversion;
					if (m_maxCrystals > 0 && m_maxCrystals < num2)
					{
						num2 = m_maxCrystals;
					}
					m_syncComponent.NetworkDamageCrystals = num2;
				}
			}
		}
	}

	// added in rogues
	public override void OnDamaged(ActorData damageCaster, DamageSource damageSource, int damageAmount)
	{
		base.OnDamaged(damageCaster, damageSource, damageAmount);
		DamageReceivedThisTurn += damageAmount;
		if (damageSource != null && damageSource.Ability != null && damageSource.Ability is MartyrProtectAlly)
		{
			m_damageFromRedirectThisTurn += damageAmount;
		}
	}

	// added in rogues
	public override int CalculateEnergyGainOnDamage(int damage, ActorData caster, Ability sourceAbility, Effect sourceEffect)
	{
		int energyGain = 0;
		if (m_energyGainPerDamageTaken > 0f
			&& Owner != null
			&& Owner.GetAbilityData() != null
			&& (!m_skipEnergyGainIfUsingUlt || !Owner.GetAbilityData().HasQueuedAbilityOfType(typeof(MartyrSpendCrystals))))  // , true in rogues
		{
			float energyGainPerDamageTaken = m_energyGainPerDamageTaken;
			if (sourceAbility != null && sourceAbility is MartyrProtectAlly)
			{
				MartyrProtectAlly martyrProtectAlly = sourceAbility as MartyrProtectAlly;
				if (martyrProtectAlly.GetExtraEnergyPerRedirectDamage() > 0f)
				{
					energyGainPerDamageTaken += martyrProtectAlly.GetExtraEnergyPerRedirectDamage();
				}
			}
			energyGain = Mathf.RoundToInt(damage * energyGainPerDamageTaken);
			energyGain = Mathf.Max(0, energyGain);
		}
		return energyGain;
	}
#endif
}

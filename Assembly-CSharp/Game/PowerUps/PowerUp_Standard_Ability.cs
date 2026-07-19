// ROGUES
// SERVER
using System;
using System.Collections.Generic;
using AbilityContextNamespace;
using UnityEngine;

public class PowerUp_Standard_Ability : Ability
{
	// removed in rogues
	public enum ExtraEffectApplyCondition
	{
		Ignore,
		EnemyLowestHealthPct
	}

	// removed in rogues
	[Serializable]
	public class ExtraEffectApplyData
	{
		public ExtraEffectApplyCondition m_condition;
		public StandardEffectInfo m_extraEffect;
	}

	public int m_healAmount = 30;
	public int m_techPointsAmount;
	// removed in rogues
	[Tooltip("Credits to give to the actor who ran over the powerup.")]
	public int m_personalCredits;
	// removed in rogues
	[Tooltip("Credits to give to each actor on the team of the actor who ran over the powerup (including the picker-upper).")]
	public int m_teamCredits;
	// TODO POWERUPS unused
	public int m_objectivePointAdjust_casterTeam;
	public int m_objectivePointAdjust_enemyTeam;
	public bool m_applyEffect;
	public StandardActorEffectData m_effect;
	public AbilityStatMod[] m_permanentStatMods;
	public StatusType[] m_permanentStatusChanges;
	public bool m_awardCoins;

	// rogues
	// public OnHitAuthoredData OnHitData;

	// TODO POWERUPS
	// removed in rogues
	[Separator("Extra Effects, for one-off powerups", true)]
	public List<ExtraEffectApplyData> m_extraEffectsToApply;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Standard PowerUp";
		}
	}

	public void SetHealAmount(int amount)
	{
		m_healAmount = amount;
	}

	public void SetTechPointAmount(int amount)
	{
		m_techPointsAmount = amount;
	}

	// added in rogues
#if SERVER
	public virtual ActorHitResults CreateActorHitResults(
		PowerUp powerUp,
		ActorData targetActor,
		Vector3 origin,
		StandardPowerUpAbilityModData powerupMod,
		EffectSource effectSourceOverride,
		bool isDirectActorHit) // non-virtual in rogues
	{
		ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(targetActor, origin)
		{
			AbilityResults = new AbilityResults(null, this, null, true, false)
		});
		AppendToActorHitResults(powerUp, actorHitResults, targetActor, powerupMod, effectSourceOverride, isDirectActorHit);
		
		// rogues
		// ActorHitContext actorContext = new ActorHitContext();
		// ContextVars abilityContext = new ContextVars();
		// NumericHitResultScratch numericHitResultScratch = new NumericHitResultScratch();
		// GenericAbility_Container.SetEffectTemplateFieldsOnHitResults(targetActor, targetActor, actorContext, abilityContext, actorHitResults, OnHitData.m_effectTemplateFields);
		// if (targetActor.GetTeam() == powerUp.PickupTeam || !powerUp.m_restrictPickupByTeam)
		// {
		// 	GenericAbility_Container.CalcIntFieldValues(targetActor, targetActor, actorContext, abilityContext, OnHitData.m_allyHitIntFields, numericHitResultScratch);
		// 	GenericAbility_Container.SetNumericFieldsOnHitResults(actorHitResults, numericHitResultScratch);
		// 	GenericAbility_Container.SetKnockbackFieldsOnHitResults(targetActor, targetActor, actorContext, abilityContext, actorHitResults, OnHitData.m_allyHitKnockbackFields);
		// 	GenericAbility_Container.SetCooldownReductionFieldsOnHitResults(targetActor, targetActor, actorContext, abilityContext, actorHitResults, OnHitData.m_allyHitCooldownReductionFields, 1);
		// 	GenericAbility_Container.SetEffectFieldsOnHitResults(targetActor, targetActor, actorContext, abilityContext, actorHitResults, OnHitData.m_allyHitEffectFields);
		// 	GenericAbility_Container.SetEffectTemplateFieldsOnHitResults(targetActor, targetActor, actorContext, abilityContext, actorHitResults, OnHitData.m_allyHitEffectTemplateFields);
		// }
		// else
		// {
		// 	GenericAbility_Container.CalcIntFieldValues(targetActor, targetActor, actorContext, abilityContext, OnHitData.m_enemyHitIntFields, numericHitResultScratch);
		// 	GenericAbility_Container.SetNumericFieldsOnHitResults(actorHitResults, numericHitResultScratch);
		// 	GenericAbility_Container.SetKnockbackFieldsOnHitResults(targetActor, targetActor, actorContext, abilityContext, actorHitResults, OnHitData.m_enemyHitKnockbackFields);
		// 	GenericAbility_Container.SetCooldownReductionFieldsOnHitResults(targetActor, targetActor, actorContext, abilityContext, actorHitResults, OnHitData.m_enemyHitCooldownReductionFields, 1);
		// 	GenericAbility_Container.SetEffectFieldsOnHitResults(targetActor, targetActor, actorContext, abilityContext, actorHitResults, OnHitData.m_enemyHitEffectFields);
		// 	GenericAbility_Container.SetEffectTemplateFieldsOnHitResults(targetActor, targetActor, actorContext, abilityContext, actorHitResults, OnHitData.m_enemyHitEffectTemplateFields);
		// }
		
		return actorHitResults;
	}
#endif

	// added in rogues
#if SERVER
	public void AppendToActorHitResults(PowerUp powerUp, ActorHitResults results, ActorData targetActor, StandardPowerUpAbilityModData powerupMod, EffectSource effectSourceOverride, bool isDirectActorHit)
	{
		if (m_applyEffect)
		{
			StandardEffectInfo standardEffectInfo = new StandardEffectInfo();
			standardEffectInfo.m_applyEffect = m_applyEffect;
			standardEffectInfo.m_effectData = m_effect;
			EffectSource effectSource;
			if (effectSourceOverride != null)
			{
				effectSource = effectSourceOverride;
			}
			else
			{
				effectSource = base.AsEffectSource();
			}
			if (effectSource != null)
			{
				StandardActorEffect standardActorEffect = standardEffectInfo.CreateEffect(effectSource, targetActor, targetActor);
				if (m_healAmount > 0)
				{
					standardActorEffect.SetPerTurnHitDelay(1);
				}
				if (GameplayMutators.Get() != null)
				{
					int num = standardActorEffect.m_time.duration + GameplayMutators.GetPowerupDurationAdjustment();
					num = Mathf.Max(num, 0);
					standardActorEffect.SetDurationBeforeStart(num);
				}
				results.AddEffect(standardActorEffect);
			}
			else if (Application.isEditor)
			{
				Debug.LogError("Effect source is null for powerup ability");
			}
		}
		int num2 = (powerupMod != null) ? powerupMod.m_healMod.GetModifiedValue(m_healAmount) : m_healAmount;
		if (isDirectActorHit && powerupMod != null && powerupMod.m_extraHealIfDirectHit > 0)
		{
			num2 += powerupMod.m_extraHealIfDirectHit;
		}
		results.AddBaseHealing(num2);
		int num3 = (powerupMod != null) ? powerupMod.m_techPointMod.GetModifiedValue(m_techPointsAmount) : m_techPointsAmount;
		if (isDirectActorHit && powerupMod != null && powerupMod.m_extraTechPointIfDirectHit > 0)
		{
			num3 += powerupMod.m_extraTechPointIfDirectHit;
		}
		if (num3 > 0)
		{
			results.AddTechPointGain(num3);
		}
		results.AddPermanentStatMods(m_permanentStatMods);
		results.AddPermanentStatusChanges(m_permanentStatusChanges);
		if (CollectTheCoins.Get() != null && !powerUp.m_restrictPickupByTeam && !powerUp.m_isSpoil)
		{
			if (CollectTheCoins.Get().m_numCoinsToAwardPerNonCoinPowerup > 0 && !m_awardCoins)
			{
				results.AddGameModeEvent(new GameModeEvent
				{
					m_eventType = GameModeEventType.Ctc_NonCoinPowerupTouched,
					m_primaryActor = targetActor
				});
			}
			if (CollectTheCoins.Get().m_numCoinsToAwardPerCoinPowerup > 0 && m_awardCoins)
			{
				results.AddGameModeEvent(new GameModeEvent
				{
					m_eventType = GameModeEventType.Ctc_CoinPowerupTouched,
					m_primaryActor = targetActor
				});
			}
		}
	}
#endif
}

// ROGUES
// SERVER
using System.Collections.Generic;

public class Passive_BattleMonk : Passive
{
	public StandardEffectInfo m_effectOnOwner_nearAllies;
	public StandardEffectInfo m_effectOnOwner_nearEnemies;
	public StandardEffectInfo m_effectOnAllies;
	public StandardEffectInfo m_effectOnEnemies;
	public AbilityAreaShape m_alliesShape;
	public AbilityAreaShape m_enemiesShape;
	public bool m_penetrateLosForAllies;
	public bool m_penetrateLosForEnemies;
	public BattleMonkSelfBuff m_buffAbility;
	public BattleMonkBoundingLeap m_chargeAbility;
	public int m_chargeLastCastTurn = -1;
	public int m_buffLastCastTurn = -1;
	public int m_damagedThisTurn;
	
#if SERVER
	protected override void OnStartup()
	{
		AbilityData abilityData = Owner.GetAbilityData();
		if (abilityData != null)
		{
			m_chargeAbility = abilityData.GetAbilityOfType(typeof(BattleMonkBoundingLeap)) as BattleMonkBoundingLeap;
			m_buffAbility = abilityData.GetAbilityOfType(typeof(BattleMonkSelfBuff)) as BattleMonkSelfBuff;
		}
	}

	public override void OnDamaged(ActorData damageCaster, DamageSource damageSource, int damageAmount)
	{
		base.OnDamaged(damageCaster, damageSource, damageAmount);
		if (damageAmount > 0)
		{
			m_damagedThisTurn++;
		}
	}

	public override void OnTurnStart()
	{
		if (Owner.IsDead() || Owner.GetCurrentBoardSquare() == null)
		{
			return;
		}
		List<ActorData> alliesNearby = AreaEffectUtils.GetActorsInShape(
			m_alliesShape,
			Owner.GetFreePos(),
			Owner.GetCurrentBoardSquare(),
			m_penetrateLosForAllies,
			Owner,
			Owner.GetTeamAsList(),
			null);
		bool hasAllyNearby = false;
		foreach (ActorData actorData in alliesNearby)
		{
			if (actorData != Owner)
			{
				hasAllyNearby = true;
				break;
			}
		}
		if (hasAllyNearby)
		{
			if (m_effectOnOwner_nearAllies.m_applyEffect)
			{
				StandardActorEffect effect = new StandardActorEffect(
					AsEffectSource(),
					Owner.GetCurrentBoardSquare(),
					Owner,
					Owner,
					m_effectOnOwner_nearAllies.m_effectData);
				ServerEffectManager.Get().ApplyEffect(effect);
			}
			if (m_effectOnAllies.m_applyEffect)
			{
				foreach (ActorData actorData in alliesNearby)
				{
					if (actorData != Owner && m_effectOnOwner_nearAllies.m_applyEffect)
					{
						StandardActorEffect effect = new StandardActorEffect(
							AsEffectSource(),
							actorData.GetCurrentBoardSquare(),
							actorData,
							Owner,
							m_effectOnOwner_nearAllies.m_effectData);
						ServerEffectManager.Get().ApplyEffect(effect);
					}
				}
			}
		}
		List<ActorData> enemiesNearby = AreaEffectUtils.GetActorsInShape(
			m_enemiesShape,
			Owner.GetFreePos(),
			Owner.GetCurrentBoardSquare(),
			m_penetrateLosForEnemies,
			Owner,
			Owner.GetOtherTeams(),
			null);
		if (enemiesNearby.Count > 0)
		{
			if (m_effectOnOwner_nearEnemies.m_applyEffect)
			{
				StandardActorEffect effect = new StandardActorEffect(
					AsEffectSource(),
					Owner.GetCurrentBoardSquare(),
					Owner,
					Owner,
					m_effectOnOwner_nearEnemies.m_effectData);
				ServerEffectManager.Get().ApplyEffect(effect);
			}
			if (m_effectOnEnemies.m_applyEffect)
			{
				foreach (ActorData actorData in enemiesNearby)
				{
					if (actorData != Owner && m_effectOnOwner_nearAllies.m_applyEffect)
					{
						StandardActorEffect effect = new StandardActorEffect(
							AsEffectSource(),
							actorData.GetCurrentBoardSquare(),
							actorData,
							Owner,
							m_effectOnEnemies.m_effectData);
						ServerEffectManager.Get().ApplyEffect(effect);
					}
				}
			}
		}
		if (m_chargeAbility != null && m_damagedThisTurn == 0)
		{
			int healAmountIfNotDamagedThisTurn = m_chargeAbility.GetHealAmountIfNotDamagedThisTurn();
			if (healAmountIfNotDamagedThisTurn > 0
			    && m_chargeLastCastTurn > 0
			    && GameFlowData.Get().CurrentTurn - m_chargeLastCastTurn == 1)
			{
				ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(Owner, Owner.GetFreePos()));
				actorHitResults.AddBaseHealing(healAmountIfNotDamagedThisTurn);
				MovementResults.SetupAndExecuteAbilityResultsOutsideResolution(Owner, Owner, actorHitResults, m_chargeAbility);
			}
		}
		if (m_buffAbility != null && m_buffAbility.HasEffectForStartOfNextTurn())
		{
			int durationOfSelfEffect = m_buffAbility.GetDurationOfSelfEffect(m_damagedThisTurn);
			if (durationOfSelfEffect > 0 && m_buffLastCastTurn > 0 && GameFlowData.Get().CurrentTurn - m_buffLastCastTurn == 1)
			{
				StandardEffectInfo shallowCopy = m_buffAbility.GetSelfEffect().GetShallowCopy();
				ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(Owner, Owner.GetFreePos()));
				shallowCopy.m_effectData.m_duration = durationOfSelfEffect;
				actorHitResults.AddStandardEffectInfo(shallowCopy);
				MovementResults.SetupAndExecuteAbilityResultsOutsideResolution(Owner, Owner, actorHitResults, m_buffAbility);
			}
		}
		m_damagedThisTurn = 0;
	}
#endif
}

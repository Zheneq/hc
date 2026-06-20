// ROGUES
// SERVER
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Passive_RobotAnimal : Passive
{
	public int m_chargeLastHitTurn = -1;
	public List<ActorData> m_chargeHitActors = new List<ActorData>();
	public int m_dragLastCastTurn = -1;
	public List<ActorData> m_dragHitActors = new List<ActorData>();
	public bool m_shouldApplyAdditionalEffectFromStealth;
	public int m_biteLastCastTurn = -1;
	public List<ActorData> m_biteAdjacentEnemies = new List<ActorData>();
	
#if SERVER
	// added in rogues
	private RobotAnimalBite m_biteAbility;
	private RobotAnimalCharge m_chargeAbility;
	private RobotAnimalDrag m_dragAbility;
	private RobotAnimalStealth m_stealthAbility;

	// added in rogues
	protected override void OnStartup()
	{
		AbilityData abilityData = Owner.GetAbilityData();
		if (abilityData != null)
		{
			m_biteAbility = abilityData.GetAbilityOfType(typeof(RobotAnimalBite)) as RobotAnimalBite;
			m_chargeAbility = abilityData.GetAbilityOfType(typeof(RobotAnimalCharge)) as RobotAnimalCharge;
			m_dragAbility = abilityData.GetAbilityOfType(typeof(RobotAnimalDrag)) as RobotAnimalDrag;
			m_stealthAbility = abilityData.GetAbilityOfType(typeof(RobotAnimalStealth)) as RobotAnimalStealth;
		}
	}

	// added in rogues
	public override void OnTurnStart()
	{
		base.OnTurnStart();
		HandleBiteAbilityTurnStart();
		HandleChargeAbilityTurnStart();
		HandleDragAbilityTurnStart();
	}

	// added in rogues
	public override void OnGainingEffect(Effect effect)
	{
		if (effect != null
		    && effect.GetType() == typeof(RobotAnimalStealthEffect)
		    && m_stealthAbility != null
		    && (m_stealthAbility.ShouldApplyEffectOnNextDamageAttack() || m_stealthAbility.GetExtraDamageNextAttack() != 0))
		{
			m_shouldApplyAdditionalEffectFromStealth = true;
		}
	}

	// added in rogues
	public override void OnLosingEffect(Effect effect)
	{
		if (effect != null && effect.GetType() == typeof(RobotAnimalStealthEffect))
		{
			m_shouldApplyAdditionalEffectFromStealth = false;
		}
	}

	// added in rogues
	public bool HasEffectOnNextDamageAttack()
	{
		return m_stealthAbility != null && m_stealthAbility.ShouldApplyEffectOnNextDamageAttack();
	}

	// added in rogues
	public StandardEffectInfo GetEffectOnNextDamageAttack()
	{
		return HasEffectOnNextDamageAttack()
			? m_stealthAbility.GetEffectOnNextDamageAttack()
			: new StandardEffectInfo();
	}

	// added in rogues
	public bool ShouldApplyExtraDamageNextAttack()
	{
		return m_stealthAbility != null
		       && m_stealthAbility.GetExtraDamageNextAttack() != 0
		       && m_shouldApplyAdditionalEffectFromStealth;
	}

	// added in rogues
	public int GetExtraDamageNextAttack()
	{
		return m_stealthAbility != null
			? m_stealthAbility.GetExtraDamageNextAttack()
			: 0;
	}

	// added in rogues
	private void HandleChargeAbilityTurnStart()
	{
		if (m_chargeAbility != null
		    && m_chargeAbility.ModdedHealOnNextTurnStartIfKilledTarget() > 0
		    && m_chargeLastHitTurn > 0
		    && GameFlowData.Get().CurrentTurn - m_chargeLastHitTurn == 1)
		{
			bool flag = false;
			foreach (ActorData actorData in m_chargeHitActors)
			{
				if (actorData != null && actorData.IsDead())
				{
					flag = true;
					break;
				}
			}
			if (flag && !Owner.IsDead())
			{
				ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(Owner, Owner.GetFreePos()));
				actorHitResults.SetBaseHealing(m_chargeAbility.ModdedHealOnNextTurnStartIfKilledTarget());
				MovementResults.SetupAndExecuteAbilityResultsOutsideResolution(Owner, Owner, actorHitResults, m_chargeAbility);
			}
		}
		m_chargeHitActors.Clear();
	}

	// added in rogues
	private void HandleDragAbilityTurnStart()
	{
		if (m_dragAbility != null
		    && m_dragAbility.HasEffectOnNextTurnStart()
		    && m_dragLastCastTurn > 0
		    && GameFlowData.Get().CurrentTurn - m_dragLastCastTurn == 1)
		{
			foreach (ActorData actorData in m_dragHitActors)
			{
				if (actorData != null && !actorData.IsDead())
				{
					ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(actorData, actorData.GetFreePos()));
					actorHitResults.AddStandardEffectInfo(m_dragAbility.EffectInfoOnNextTurnStart());
					MovementResults.SetupAndExecuteAbilityResultsOutsideResolution(actorData, Owner, actorHitResults, m_dragAbility);
				}
			}
			List<PowerUp> moddedPowerUpsToSpawn = m_dragAbility.GetModdedPowerUpsToSpawn();
			if (moddedPowerUpsToSpawn != null && moddedPowerUpsToSpawn.Count() > 0)
			{
				ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(Owner, Owner.GetFreePos()));
				int num = 0;
				foreach (BoardSquare boardSquare in
				         from a in AreaEffectUtils.GetSquaresInShape(
					         m_dragAbility.GetModdedPowerUpsToSpawnShape(),
					         Owner.GetFreePos(),
					         Owner.GetCurrentBoardSquare(),
					         false,
					         Owner) 
				         orderby GameplayRandom.GetUniform() 
				         select a)
				{
					if (boardSquare.OccupantActor == null && boardSquare.IsValidForGameplay())
					{
						if (moddedPowerUpsToSpawn.ElementAt(num) != null)
						{
							SpoilSpawnDataForAbilityHit spoilSpawnData = new SpoilSpawnDataForAbilityHit(boardSquare, Owner.GetTeam(), new List<GameObject>
							{
								moddedPowerUpsToSpawn.ElementAt(num).gameObject
							});
							actorHitResults.AddSpoilSpawnData(spoilSpawnData);
						}
						num++;
						if (num >= moddedPowerUpsToSpawn.Count())
						{
							break;
						}
					}
				}
				MovementResults.SetupAndExecuteAbilityResultsOutsideResolution(Owner, Owner, actorHitResults, m_dragAbility);
			}
		}
		m_dragHitActors.Clear();
	}

	// added in rogues
	private void HandleBiteAbilityTurnStart()
	{
		if (m_biteAbility != null
		    && m_biteAbility.HasEffectOnNextTurnStart()
		    && m_biteLastCastTurn > 0
		    && GameFlowData.Get().CurrentTurn - m_biteLastCastTurn == 1)
		{
			ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(Owner, Owner.GetFreePos()));
			for (int i = 0; i < m_biteAdjacentEnemies.Count; i++)
			{
				actorHitResults.AddStandardEffectInfo(m_biteAbility.EffectInfoOnNextTurnStart());
			}
			MovementResults.SetupAndExecuteAbilityResultsOutsideResolution(Owner, Owner, actorHitResults, m_biteAbility);
		}
		m_biteAdjacentEnemies.Clear();
	}
#endif
}

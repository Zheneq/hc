// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

// server-only, was empty in reactor
public class Passive_Spark : Passive
{
#if SERVER
	private SparkBasicAttack m_damageBeamAbility;
	private SparkHealingBeam m_healBeamAbility;
	private SparkEnergized m_energizedAbility;
	private SparkBeamTrackerComponent m_syncComp;

	private int m_damageBeamPulseAnimIndex;
	private int m_healBeamPulseAnimIndex;

	public int GetDamageBeamPulseAnimIndex()
	{
		return m_damageBeamPulseAnimIndex;
	}

	public int GetHealBeamPulseAnimIndex()
	{
		return m_healBeamPulseAnimIndex;
	}

	protected override void OnStartup()
	{
		base.OnStartup();
		AbilityData component = Owner.GetComponent<AbilityData>();
		if (component != null)
		{
			m_damageBeamAbility = component.GetAbilityOfType(typeof(SparkBasicAttack)) as SparkBasicAttack;
			m_healBeamAbility = component.GetAbilityOfType(typeof(SparkHealingBeam)) as SparkHealingBeam;
			m_energizedAbility = component.GetAbilityOfType(typeof(SparkEnergized)) as SparkEnergized;
		}
		if (m_damageBeamAbility != null)
		{
			m_damageBeamPulseAnimIndex = m_damageBeamAbility.m_pulseAnimIndex;
		}
		if (m_healBeamAbility != null)
		{
			m_healBeamPulseAnimIndex = m_healBeamAbility.m_pulseAnimIndex;
		}
		m_syncComp = Owner.GetComponent<SparkBeamTrackerComponent>();
	}

	public override void OnTurnStart()
	{
		base.OnTurnStart();
		HandleHealTetherOnTurnStart();
		HandleEnergizedOnTurnStart();
		m_syncComp.ClearActorsOutOfRangeOnEvade();
	}

	private void HandleHealTetherOnTurnStart()
	{
		if (m_healBeamAbility == null
		    || !m_healBeamAbility.ShouldApplyTargetEffectForXDamage()
		    || GameFlowData.Get().CurrentTurn <= 1)
		{
			return;
		}

		foreach (int beamActorIndex in m_syncComp.GetBeamActorIndices())
		{
			ActorData beamActor = GameplayUtils.GetActorOfActorIndex(beamActorIndex);
			if (beamActor != null && beamActor.GetActorBehavior() != null)
			{
				ActorBehavior.TurnBehavior behaviorOfTurn = beamActor.GetActorBehavior().GetBehaviorOfTurn(GameFlowData.Get().CurrentTurn - 1);
				if (behaviorOfTurn != null
				    && behaviorOfTurn.DamageTaken >= m_healBeamAbility.GetXDamageThreshold()
				    && ServerEffectManager.Get().GetEffectsOnTargetByCaster(beamActor, Owner, typeof(SparkHealingBeamEffect)).Count > 0)
				{
					ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(beamActor, beamActor.GetFreePos()));
					actorHitResults.AddStandardEffectInfo(m_healBeamAbility.GetTargetEffectForXDamage());
					MovementResults.SetupAndExecuteAbilityResultsOutsideResolution(beamActor, Owner, actorHitResults, m_healBeamAbility);
				}
			}
		}
	}

	private void HandleEnergizedOnTurnStart()
	{
		if (m_energizedAbility == null)
		{
			return;
		}
		if (m_energizedAbility.HasEnemyEffectForTurnStart())
		{
			foreach (ActorData actorData in m_energizedAbility.GetLastHitActors())
			{
				if (actorData != null
				    && !actorData.IsDead()
				    && actorData.GetTeam() != Owner.GetTeam())
				{
					ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(actorData, actorData.GetFreePos()));
					actorHitResults.AddStandardEffectInfo(m_energizedAbility.GetEnemyEffectForTurnStart());
					MovementResults.SetupAndExecuteAbilityResultsOutsideResolution(actorData, Owner, actorHitResults, m_energizedAbility);
				}
			}
		}
		m_energizedAbility.GetLastHitActors().Clear();
	}

	public void SetPulseAnimIndexOnFirstBeams()
	{
		bool processedAllyBeam = false;
		bool processedEnemyBeam = false;
		foreach (int beamActorIndex in m_syncComp.GetBeamActorIndices())
		{
			ActorData beamActor = GameplayUtils.GetActorOfActorIndex(beamActorIndex);
			if (beamActor == null)
			{
				continue;
			}
			List<Effect> allyEffects = ServerEffectManager.Get().GetEffectsOnTargetByCaster(
				beamActor, Owner, typeof(SparkHealingBeamEffect));
			if (allyEffects.Count > 0)
			{
				Log.Info($"SPARK got {allyEffects.Count} ally effects");
				foreach (Effect effect in allyEffects)
				{
					SparkHealingBeamEffect sparkHealingBeamEffect = effect as SparkHealingBeamEffect;
					bool isSkippingAllyBeam = sparkHealingBeamEffect.IsSkippingGatheringResults();
					int animIndex = processedAllyBeam || isSkippingAllyBeam
						? 0
						: m_healBeamAbility.m_pulseAnimIndex;
					Log.Info($"SPARK setting ally effect pulse anim index to {animIndex} " +
					         $"(processedAllyBeam={processedAllyBeam}, isSkippingAllyBeam={isSkippingAllyBeam}");
					sparkHealingBeamEffect.SetPulseAnimIndex(animIndex);
					if (!isSkippingAllyBeam)
					{
						processedAllyBeam = true;
					}
				}
			}
			List<Effect> enemyEffects = ServerEffectManager.Get().GetEffectsOnTargetByCaster(
				beamActor, Owner, typeof(SparkBasicAttackEffect));
			if (enemyEffects.Count > 0)
			{
				foreach (Effect effect in enemyEffects)
				{
					Log.Info($"SPARK got {enemyEffects.Count} enemy effects");
					SparkBasicAttackEffect sparkBasicAttackEffect = effect as SparkBasicAttackEffect;
					bool isSkippingEnemyBeam = sparkBasicAttackEffect.IsSkippingGatheringResults();
					int animIndex = processedEnemyBeam || isSkippingEnemyBeam
						? 0
						: m_damageBeamAbility.m_pulseAnimIndex;
					Log.Info($"SPARK setting enemy effect pulse anim index to {animIndex} " +
					         $"(processedEnemyBeam={processedEnemyBeam}, isSkippingEnemyBeam={isSkippingEnemyBeam}");
					sparkBasicAttackEffect.SetPulseAnimIndex(animIndex);
					if (!isSkippingEnemyBeam)
					{
						processedEnemyBeam = true;
					}
				}
			}
		}
	}

	private void RemoveDuplicateTethers()
	{
		List<int> beamActorIndices = m_syncComp.GetBeamActorIndices();
		float minAllyDist = 100000f;
		int closestAlly = ActorData.s_invalidActorIndex;
		float minEnemyDist = 100000f;
		int closestEnemy = ActorData.s_invalidActorIndex;
		Vector3 freePos = Owner.GetFreePos();
		foreach (int actorIndex in beamActorIndices)
		{
			ActorData beamActor = GameplayUtils.GetActorOfActorIndex(actorIndex);
			if (beamActor != null)
			{
				float dist = (freePos - beamActor.GetFreePos()).magnitude;
				if (ServerEffectManager.Get().HasEffectByCaster(beamActor, Owner, typeof(SparkHealingBeamEffect)))
				{
					if (dist <= minAllyDist)
					{
						minAllyDist = dist;
						closestAlly = beamActor.ActorIndex;
					}
				}
				else if (ServerEffectManager.Get().HasEffectByCaster(beamActor, Owner, typeof(SparkBasicAttackEffect)))
				{
					if (dist <= minEnemyDist)
					{
						minEnemyDist = dist;
						closestEnemy = beamActor.ActorIndex;
					}
				}
			}
		}
		foreach (int actorIndex in beamActorIndices)
		{
			ActorData beamActor = GameplayUtils.GetActorOfActorIndex(actorIndex);
			if (beamActor == null)
			{
				continue;
			}
			if (beamActor.GetTeam() == Owner.GetTeam())
			{
				if (closestAlly != beamActor.ActorIndex)
				{
					List<Effect> healingBeamEffects = ServerEffectManager.Get().GetEffectsOnTargetByCaster(
						beamActor, Owner, typeof(SparkHealingBeamEffect));
					foreach (Effect effectToRemove in healingBeamEffects)
					{
						ServerEffectManager.Get().RemoveEffect(
							effectToRemove,
							ServerEffectManager.Get().GetActorEffects(beamActor));
					}
				}
			}
			else if (closestEnemy != beamActor.ActorIndex)
			{
				foreach (Effect effectToRemove in ServerEffectManager.Get().GetEffectsOnTargetByCaster(
					         beamActor, Owner, typeof(SparkBasicAttackEffect)))
				{
					ServerEffectManager.Get().RemoveEffect(
						effectToRemove, 
						ServerEffectManager.Get().GetActorEffects(beamActor));
				}
			}
		}
	}

	public override void OnEvadesProcessed()
	{
		base.OnEvadesProcessed();
		m_syncComp.ClearActorsOutOfRangeOnEvade();
		if (Owner.IsDead())
		{
			return;
		}
		List<Effect> basicAttackEffects = ServerEffectManager.Get().GetAllActorEffectsByCaster(Owner, typeof(SparkBasicAttackEffect));
		if (basicAttackEffects.Count <= 0)
		{
			return;
		}
		BoardSquare casterBoardSquare = ServerActionBuffer.Get().GetProcessedEvadeDestination(Owner);
		if (casterBoardSquare == null)
		{
			casterBoardSquare = Owner.GetCurrentBoardSquare();
		}
		if (casterBoardSquare == null)
		{
			return;
		}
		foreach (Effect effect in basicAttackEffects)
		{
			SparkBasicAttackEffect sparkBasicAttackEffect = effect as SparkBasicAttackEffect;
			BoardSquare targetActorSquare = ServerActionBuffer.Get().GetProcessedEvadeDestination(effect.Target);
			if (targetActorSquare == null)
			{
				targetActorSquare = effect.Target.GetCurrentBoardSquare();
			}
			if (targetActorSquare != null
			    && casterBoardSquare.HorizontalDistanceInSquaresTo(targetActorSquare) > sparkBasicAttackEffect.GetMaxTetherDist())
			{
				m_syncComp.AddActorAsOutOfRangeOnEvade(effect.Target);
				sparkBasicAttackEffect.RemoveRevealedStatusForEvadeOutOfRange();
			}
		}
	}
#endif
}

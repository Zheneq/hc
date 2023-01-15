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
			m_damageBeamAbility = (component.GetAbilityOfType(typeof(SparkBasicAttack)) as SparkBasicAttack);
			m_healBeamAbility = (component.GetAbilityOfType(typeof(SparkHealingBeam)) as SparkHealingBeam);
			m_energizedAbility = (component.GetAbilityOfType(typeof(SparkEnergized)) as SparkEnergized);
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
		if (m_healBeamAbility != null && m_healBeamAbility.ShouldApplyTargetEffectForXDamage() && GameFlowData.Get().CurrentTurn > 1)
		{
			List<int> beamActorIndices = m_syncComp.GetBeamActorIndices();
			for (int i = 0; i < beamActorIndices.Count; i++)
			{
				ActorData actorOfActorIndex = GameplayUtils.GetActorOfActorIndex(beamActorIndices[i]);
				if (actorOfActorIndex != null && actorOfActorIndex.GetActorBehavior() != null)
				{
					ActorBehavior.TurnBehavior behaviorOfTurn = actorOfActorIndex.GetActorBehavior().GetBehaviorOfTurn(GameFlowData.Get().CurrentTurn - 1);
					if (behaviorOfTurn != null && behaviorOfTurn.DamageTaken >= m_healBeamAbility.GetXDamageThreshold() && ServerEffectManager.Get().GetEffectsOnTargetByCaster(actorOfActorIndex, Owner, typeof(SparkHealingBeamEffect)).Count > 0)
					{
						ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(actorOfActorIndex, actorOfActorIndex.GetFreePos()));
						actorHitResults.AddStandardEffectInfo(m_healBeamAbility.GetTargetEffectForXDamage());
						MovementResults.SetupAndExecuteAbilityResultsOutsideResolution(actorOfActorIndex, Owner, actorHitResults, m_healBeamAbility, true, null, null);
					}
				}
			}
		}
	}

	private void HandleEnergizedOnTurnStart()
	{
		if (m_energizedAbility != null)
		{
			if (m_energizedAbility.HasEnemyEffectForTurnStart())
			{
				foreach (ActorData actorData in m_energizedAbility.GetLastHitActors())
				{
					if (actorData != null && !actorData.IsDead() && actorData.GetTeam() != Owner.GetTeam())
					{
						ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(actorData, actorData.GetFreePos()));
						actorHitResults.AddStandardEffectInfo(m_energizedAbility.GetEnemyEffectForTurnStart());
						MovementResults.SetupAndExecuteAbilityResultsOutsideResolution(actorData, Owner, actorHitResults, m_energizedAbility, true, null, null);
					}
				}
			}
			m_energizedAbility.GetLastHitActors().Clear();
		}
	}

	public void SetPulseAnimIndexOnFirstBeams()
	{
		List<int> beamActorIndices = m_syncComp.GetBeamActorIndices();
		bool processedAllyBeam = false;
		bool processedEnemyBeam = false;
		foreach (int beamActorIndex in beamActorIndices)
		{
			ActorData actorOfActorIndex = GameplayUtils.GetActorOfActorIndex(beamActorIndex);
			if (actorOfActorIndex == null)
			{
				continue;
			}
			List<Effect> allyEffects = ServerEffectManager.Get().GetEffectsOnTargetByCaster(actorOfActorIndex, Owner, typeof(SparkHealingBeamEffect));
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
					Log.Info($"SPARK setting ally effect pulse anim index to {animIndex} (processedAllyBeam={processedAllyBeam}, isSkippingAllyBeam={isSkippingAllyBeam}");
					sparkHealingBeamEffect.SetPulseAnimIndex(animIndex);
					if (!isSkippingAllyBeam)
					{
						processedAllyBeam = true;
					}
				}
			}
			List<Effect> enemyEffects = ServerEffectManager.Get().GetEffectsOnTargetByCaster(actorOfActorIndex, Owner, typeof(SparkBasicAttackEffect));
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
					Log.Info($"SPARK setting enemy effect pulse anim index to {animIndex} (processedEnemyBeam={processedEnemyBeam}, isSkippingEnemyBeam={isSkippingEnemyBeam}");
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
		float num = 100000f;
		int num2 = -1;
		float num3 = 100000f;
		int num4 = -1;
		Vector3 freePos = Owner.GetFreePos();
		foreach (int actorIndex in beamActorIndices)
		{
			ActorData actorOfActorIndex = GameplayUtils.GetActorOfActorIndex(actorIndex);
			if (actorOfActorIndex != null)
			{
				float magnitude = (freePos - actorOfActorIndex.GetFreePos()).magnitude;
				if (ServerEffectManager.Get().HasEffectByCaster(actorOfActorIndex, Owner, typeof(SparkHealingBeamEffect)))
				{
					if (magnitude <= num)
					{
						num = magnitude;
						num2 = actorOfActorIndex.ActorIndex;
					}
				}
				else if (ServerEffectManager.Get().HasEffectByCaster(actorOfActorIndex, Owner, typeof(SparkBasicAttackEffect)) && magnitude <= num3)
				{
					num3 = magnitude;
					num4 = actorOfActorIndex.ActorIndex;
				}
			}
		}
		foreach (int actorIndex2 in beamActorIndices)
		{
			ActorData actorOfActorIndex2 = GameplayUtils.GetActorOfActorIndex(actorIndex2);
			if (actorOfActorIndex2 != null)
			{
				if (actorOfActorIndex2.GetTeam() == Owner.GetTeam())
				{
					if (num2 == actorOfActorIndex2.ActorIndex)
					{
						continue;
					}
					using (List<Effect>.Enumerator enumerator2 = ServerEffectManager.Get().GetEffectsOnTargetByCaster(actorOfActorIndex2, Owner, typeof(SparkHealingBeamEffect)).GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							Effect effectToRemove = enumerator2.Current;
							ServerEffectManager.Get().RemoveEffect(effectToRemove, ServerEffectManager.Get().GetActorEffects(actorOfActorIndex2));
						}
						continue;
					}
				}
				if (num4 != actorOfActorIndex2.ActorIndex)
				{
					foreach (Effect effectToRemove2 in ServerEffectManager.Get().GetEffectsOnTargetByCaster(actorOfActorIndex2, Owner, typeof(SparkBasicAttackEffect)))
					{
						ServerEffectManager.Get().RemoveEffect(effectToRemove2, ServerEffectManager.Get().GetActorEffects(actorOfActorIndex2));
					}
				}
			}
		}
	}

	public override void OnEvadesProcessed()
	{
		base.OnEvadesProcessed();
		m_syncComp.ClearActorsOutOfRangeOnEvade();
		if (!Owner.IsDead())
		{
			List<Effect> allActorEffectsByCaster = ServerEffectManager.Get().GetAllActorEffectsByCaster(Owner, typeof(SparkBasicAttackEffect));
			if (allActorEffectsByCaster.Count > 0)
			{
				BoardSquare boardSquare = ServerActionBuffer.Get().GetProcessedEvadeDestination(Owner);
				if (boardSquare == null)
				{
					boardSquare = Owner.GetCurrentBoardSquare();
				}
				if (boardSquare != null)
				{
					foreach (Effect effect in allActorEffectsByCaster)
					{
						SparkBasicAttackEffect sparkBasicAttackEffect = effect as SparkBasicAttackEffect;
						BoardSquare boardSquare2 = ServerActionBuffer.Get().GetProcessedEvadeDestination(effect.Target);
						if (boardSquare2 == null)
						{
							boardSquare2 = effect.Target.GetCurrentBoardSquare();
						}
						if (boardSquare2 != null && boardSquare.HorizontalDistanceInSquaresTo(boardSquare2) > sparkBasicAttackEffect.GetMaxTetherDist())
						{
							m_syncComp.AddActorAsOutOfRangeOnEvade(effect.Target);
							sparkBasicAttackEffect.RemoveRevealedStatusForEvadeOutOfRange();
						}
					}
				}
			}
		}
	}
#endif
}

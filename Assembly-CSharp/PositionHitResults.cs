// ROGUES
// SERVER
//using System;
using System.Collections.Generic;
//using Mirror;
using UnityEngine;
using UnityEngine.Networking;

// server-only -- missing in reactor
#if SERVER
public class PositionHitResults
{
	public PositionHitParameters m_hitParameters;
	public List<Effect> m_effects;
	private List<ServerAbilityUtils.EffectRemovalData> m_effectsForRemoval;
	private List<ServerAbilityUtils.BarrierRemovalData> m_barriersForRemoval;
	private List<Barrier> m_barriers;
	private List<ServerClientUtils.SequenceEndData> m_sequencesToEnd;
	private Dictionary<BoardSquare, int> m_dynamicGeoForDamage;
	private List<MovementResults> m_reactionsOnPosHit;
	private HitPosDelegate m_hitDelegate;

	private bool m_executed;

	public PositionHitResults(PositionHitParameters hitParams)
	{
		m_hitParameters = hitParams;
	}

	public PositionHitResults(Effect effect, PositionHitParameters hitParams)
	{
		AddEffect(effect);
		m_hitParameters = hitParams;
	}

	public PositionHitResults(Effect effect, HitPosDelegate hitDelegate, PositionHitParameters hitParams)
	{
		AddEffect(effect);
		m_hitDelegate = hitDelegate;
		m_hitParameters = hitParams;
	}

	public PositionHitResults(HitPosDelegate hitDelegate, PositionHitParameters hitParams)
	{
		m_hitDelegate = hitDelegate;
		m_hitParameters = hitParams;
	}

	public void AddEffect(Effect effect)
	{
		if (m_effects == null)
		{
			m_effects = new List<Effect>(1);
		}
		m_effects.Add(effect);
	}

	public void AddEffectForRemoval(Effect effect, List<Effect> listToRemoveFrom)
	{
		if (m_effectsForRemoval == null)
		{
			m_effectsForRemoval = new List<ServerAbilityUtils.EffectRemovalData>(1);
		}
		m_effectsForRemoval.Add(new ServerAbilityUtils.EffectRemovalData(effect, listToRemoveFrom));
	}

	public void AddBarrierForRemoval(Barrier barrier, bool removeLinkedBarriers)
	{
		if (m_barriersForRemoval == null)
		{
			m_barriersForRemoval = new List<ServerAbilityUtils.BarrierRemovalData>(1);
		}
		m_barriersForRemoval.Add(new ServerAbilityUtils.BarrierRemovalData(barrier, removeLinkedBarriers));
	}

	public void AddBarrier(Barrier barrier)
	{
		if (m_barriers == null)
		{
			m_barriers = new List<Barrier>(1);
		}
		m_barriers.Add(barrier);
	}

	public void AddEffectSequenceToEnd(GameObject sequencePrefab, int effectGuid)
	{
		AddEffectSequenceToEnd(sequencePrefab, effectGuid, Vector3.zero);
	}

	public void AddEffectSequenceToEnd(GameObject sequencePrefab, int effectGuid, Vector3 targetPos)
	{
		if (m_sequencesToEnd == null)
		{
			m_sequencesToEnd = new List<ServerClientUtils.SequenceEndData>();
		}
		ServerClientUtils.SequenceEndData item = new ServerClientUtils.SequenceEndData(sequencePrefab, ServerClientUtils.SequenceEndData.AssociationType.EffectGuid, effectGuid, targetPos);
		m_sequencesToEnd.Add(item);
	}

	public void AddBarrierSequenceToEnd(GameObject sequencePrefab, int barrierGuid)
	{
		AddBarrierSequenceToEnd(sequencePrefab, barrierGuid, Vector3.zero);
	}

	public void AddBarrierSequenceToEnd(GameObject sequencePrefab, int barrierGuid, Vector3 targetPos)
	{
		if (m_sequencesToEnd == null)
		{
			m_sequencesToEnd = new List<ServerClientUtils.SequenceEndData>();
		}
		ServerClientUtils.SequenceEndData item = new ServerClientUtils.SequenceEndData(sequencePrefab, ServerClientUtils.SequenceEndData.AssociationType.BarrierGuid, barrierGuid, targetPos);
		m_sequencesToEnd.Add(item);
	}

	public void AddDynamicMissionGeometryDamage(BoardSquare geoSquare, int damage)
	{
		if (m_dynamicGeoForDamage == null)
		{
			m_dynamicGeoForDamage = new Dictionary<BoardSquare, int>();
		}
		if (m_dynamicGeoForDamage.ContainsKey(geoSquare))
		{
			damage += m_dynamicGeoForDamage[geoSquare];
		}
		m_dynamicGeoForDamage[geoSquare] = damage;
	}

	public void AddReactionOnPositionHit(MovementResults results)
	{
		if (m_reactionsOnPosHit == null)
		{
			m_reactionsOnPosHit = new List<MovementResults>();
		}
		m_reactionsOnPosHit.Add(results);
	}

	public void ExecuteResults()
	{
		if (AbilityResults.DebugTraceOn)
		{
			Debug.LogWarning(AbilityResults.s_executePositionHitHeader + ToDebugString());
		}
		if (m_effectsForRemoval != null)
		{
			foreach (ServerAbilityUtils.EffectRemovalData effectRemovalData in m_effectsForRemoval)
			{
				Effect effectToRemove = effectRemovalData.m_effectToRemove;
				List<Effect> effectListToRemoveFrom = effectRemovalData.m_effectListToRemoveFrom;
				if (effectToRemove != null && effectListToRemoveFrom != null && effectListToRemoveFrom.Contains(effectToRemove))
				{
					ServerEffectManager.Get().RemoveEffect(effectToRemove, effectListToRemoveFrom);
				}
			}
		}
		if (m_barriersForRemoval != null)
		{
			foreach (ServerAbilityUtils.BarrierRemovalData barrierRemovalData in m_barriersForRemoval)
			{
				Barrier barrierToRemove = barrierRemovalData.m_barrierToRemove;
				if (barrierToRemove != null && BarrierManager.Get().HasBarrier(barrierToRemove))
				{
					if (barrierRemovalData.m_removeLinkedBarriers)
					{
						List<Barrier> linkedBarriers = barrierToRemove.GetLinkedBarriers();
						if (linkedBarriers != null)
						{
							foreach (Barrier barrier in linkedBarriers)
							{
								if (barrier != null && BarrierManager.Get().HasBarrier(barrier))
								{
									BarrierManager.Get().RemoveBarrier(barrier, true);
								}
							}
						}
					}
					BarrierManager.Get().RemoveBarrier(barrierToRemove, true);
				}
			}
			GameFlowData.Get().UpdateCoverFromBarriersForAllActors();
		}

		// rogues
		//if (m_dynamicGeoForDamage != null && !NetworkClient.active)
		//{
		//	foreach (KeyValuePair<BoardSquare, int> keyValuePair in m_dynamicGeoForDamage)
		//	{
		//		DynamicMissionGeoManager.Get().AddDamageForGeoAtSquare(keyValuePair.Key, keyValuePair.Value);
		//	}
		//}

		if (m_effects != null)
		{
			foreach (Effect effect in m_effects)
			{
				ServerEffectManager.Get().ApplyEffect(effect, 1);
			}
		}
		if (m_barriers != null)
		{
			List<ActorData> list = new List<ActorData>();
			foreach (Barrier barrierToAdd in m_barriers)
			{
				List<ActorData> list2;
				BarrierManager.Get().AddBarrier(barrierToAdd, true, out list2);
				foreach (ActorData item in list2)
				{
					if (!list.Contains(item))
					{
						list.Add(item);
					}
				}
			}
			if (list.Count > 0)
			{
				foreach (ActorData actorData in list)
				{
					actorData.GetFogOfWar().ImmediateUpdateVisibilityOfSquares();
				}
				foreach (ActorData actorData2 in GameFlowData.Get().GetActors())
				{
					if (actorData2.IsActorVisibleToAnyEnemy())
					{
						actorData2.SynchronizeTeamSensitiveData();
					}
				}
			}
			GameFlowData.Get().UpdateCoverFromBarriersForAllActors();
		}
		if (m_hitDelegate != null)
		{
			m_hitDelegate(m_hitParameters);
		}
		if (m_reactionsOnPosHit != null)
		{
			foreach (MovementResults movementResults in m_reactionsOnPosHit)
			{
				movementResults.ExecuteUnexecutedMovementHits(false);
			}
		}
		ExecutedResults = true;
		if (m_hitParameters.GetRelevantAbility() != null)
		{
			if (m_hitParameters.Ability != null)
			{
				m_hitParameters.GetRelevantAbility().OnExecutedPositionHit_Ability(m_hitParameters.Caster, this);
			}
			if (m_hitParameters.Effect != null)
			{
				m_hitParameters.GetRelevantAbility().OnExecutedPositionHit_Effect(m_hitParameters.Caster, this);
			}
			if (m_hitParameters.Barrier != null)
			{
				m_hitParameters.GetRelevantAbility().OnExecutedPositionHit_Barrier(m_hitParameters.Caster, this);
			}
		}
	}

	public bool ExecutedResults
	{
		get
		{
			return m_executed;
		}
		private set
		{
			m_executed = value;
		}
	}

	public static bool HitsInCollectionDoneExecuting(Dictionary<Vector3, PositionHitResults> positionToHitResults)
	{
		foreach (PositionHitResults posToHitResult in positionToHitResults.Values)
		{
			if (!posToHitResult.ExecutedResults)
			{
				return false;
			}
		}
		return true;
	}

	public static void ExecuteUnexecutedPositionHits(Dictionary<Vector3, PositionHitResults> positionHitResults, bool asFailsafe, string logHeader)
	{
		foreach (PositionHitResults positionHitResults2 in positionHitResults.Values)
		{
			if (!positionHitResults2.ExecutedResults)
			{
				if (asFailsafe)
				{
					Debug.LogError(logHeader + " force-executing ability hit on position " + positionHitResults2.m_hitParameters.ToString() + " due to failsafe.");
				}
				positionHitResults2.ExecuteResults();
			}
		}
	}

	public string ToDebugString()
	{
		string text = "Position: " + m_hitParameters.Pos.ToString();
		if (m_effects != null)
		{
			text += "\n";
			foreach (Effect effect in m_effects)
			{
				if (effect != null)
				{
					text += effect.GetInEditorDescription();
				}
			}
		}
		if (m_barriers != null)
		{
			text += "\n";
			foreach (Barrier barrier in m_barriers)
			{
				if (barrier != null)
				{
					text += barrier.ToString();
				}
			}
		}
		text += "\n";
		return text;
	}

	public void PositionHitResults_SerializeToStream(NetworkWriter writer)
	{
		AbilityResultsUtils.SerializeEffectsToStartToStream(m_effects, writer);
		AbilityResultsUtils.SerializeBarriersToStartToStream(m_barriers, writer);
		AbilityResultsUtils.SerializeEffectsForRemovalToStream(m_effectsForRemoval, writer);
		AbilityResultsUtils.SerializeBarriersForRemovalToStream(m_barriersForRemoval, writer);
		AbilityResultsUtils.SerializeSequenceEndDataListToStream(m_sequencesToEnd, writer);
		AbilityResultsUtils.SerializeServerMovementResultsListToStream(m_reactionsOnPosHit, writer);

		// rogues
		//AbilityResultsUtils.SerializeDynamicGeoDamageToStream(m_dynamicGeoForDamage, writer);
	}

	public delegate void HitPosDelegate(PositionHitParameters parameters);
}
#endif

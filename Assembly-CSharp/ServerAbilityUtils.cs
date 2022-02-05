// ROGUES
// SERVER
using System;
using System.Collections.Generic;

// server-only, missing in reactor
#if SERVER
public static class ServerAbilityUtils
{
	public static void ForceChase(ActorData chaser, ActorData target, bool chaserInitiated)
	{
		if (chaser != null && target != null)
		{
			ServerActionBuffer.Get().StoreChaseRequest(target, chaser, true, chaserInitiated);
			chaser.GetActorBehavior().CurrentTurn.ChaseTargetActor = target;
		}
	}

	public static List<ActorData> GetEvaders()
	{
		List<ActorData> list = new List<ActorData>();
		foreach (ActorData actorData in GameFlowData.Get().GetActors())
		{
			if (ServerActionBuffer.Get().ActorIsEvading(actorData))
			{
				list.Add(actorData);
			}
		}
		return list;
	}

	public static void RemoveEvadersFromHitTargets(ref List<ActorData> hitTargets)
	{
		hitTargets.RemoveAll(new Predicate<ActorData>(ServerActionBuffer.Get().ActorIsEvading));
	}

	public static void RemoveEvadersFromHitTargets(ref Dictionary<ActorData, int> hitTargets)
	{
		List<ActorData> list = new List<ActorData>();
		foreach (ActorData actorData in hitTargets.Keys)
		{
			if (ServerActionBuffer.Get().ActorIsEvading(actorData))
			{
				list.Add(actorData);
			}
		}
		foreach (ActorData key in list)
		{
			hitTargets.Remove(key);
		}
	}

	public static void RemoveEvadersFromHitTargets(ref Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo> hitTargets)
	{
		List<ActorData> list = new List<ActorData>();
		foreach (ActorData actorData in hitTargets.Keys)
		{
			if (ServerActionBuffer.Get().ActorIsEvading(actorData))
			{
				list.Add(actorData);
			}
		}
		foreach (ActorData key in list)
		{
			hitTargets.Remove(key);
		}
	}

	public static bool CurrentlyGatheringRealResults()
	{
		return ServerActionBuffer.Get() == null || !ServerActionBuffer.Get().GatheringFakeResults;
	}

	public static ServerAbilityUtils.TriggeringPathInfo FindShortestPathCrossingOverSquare(MovementCollection movement, BoardSquare targetSquare, Team moverTeam)
	{
		BoardSquarePathInfo triggeringPathSegment = null;
		MovementInstance currentShortestMove = null;
		float currentShortestMoveCost = 0f;
		foreach (MovementInstance mi in movement.m_movementInstances)
		{
			if (mi.m_mover.GetTeam() == moverTeam)
			{
				for (BoardSquarePathInfo boardSquarePathInfo = mi.m_path; boardSquarePathInfo != null; boardSquarePathInfo = boardSquarePathInfo.next)
				{
					BoardSquare square = boardSquarePathInfo.square;
					if ((mi.m_groundBased || boardSquarePathInfo.IsPathEndpoint()) &&
						!boardSquarePathInfo.IsPathStartPoint() &&
						square == targetSquare &&
						MovementUtils.IsBetterMovementPathForGameplayThan(mi, boardSquarePathInfo.moveCost, currentShortestMove, currentShortestMoveCost))
					{
						triggeringPathSegment = boardSquarePathInfo;
						currentShortestMove = mi;
						currentShortestMoveCost = boardSquarePathInfo.moveCost;
						break;
					}
				}
			}
		}
		if (currentShortestMove != null)
		{
			return new ServerAbilityUtils.TriggeringPathInfo(currentShortestMove.m_mover, triggeringPathSegment);
		}
		else
		{
			return null;
		}
	}

	public class AbilityRunData
	{
		internal SequenceSource m_sequenceSource;

		internal SequenceSource m_parentAbilitySequenceSource;

		internal ChainAbilityAdditionalModInfo m_chainModInfo;

		internal AbilityResults m_abilityResults;

		internal AbilityResults m_abilityResults_fake;

		internal bool m_skipTheatricsAnimEntry;

		public AbilityRunData(AbilityRequest abilityRequest)
		{
			m_sequenceSource = new SequenceSource(null, null, true, null, null);
			m_sequenceSource.SetWaitForClientEnable(true);
			m_abilityResults = new AbilityResults(abilityRequest.m_caster, abilityRequest.m_ability, m_sequenceSource, true, false);
			m_abilityResults_fake = new AbilityResults(abilityRequest.m_caster, abilityRequest.m_ability, m_sequenceSource, false, false);
		}

		public AbilityRunData(PowerUp pup)
		{
			m_sequenceSource = new SequenceSource(null, null, true, null, null);
		}

		private AbilityRunData()
		{
			Log.Error("Code error: unexpected default construction of ServerAbilityUtils.AbilityRunData");
		}
	}

	public class CustomTargetEffectValidationResult
	{
		public Effect m_effect;

		public bool m_valid;

		public string m_failReason;

		public CustomTargetEffectValidationResult(Effect effect)
		{
			m_effect = effect;
			m_valid = true;
			m_failReason = "";
		}
	}

	public class TriggeringPathInfo
	{
		public ActorData m_mover;

		public BoardSquarePathInfo m_triggeringPathSegment;

		public TriggeringPathInfo(ActorData mover, BoardSquarePathInfo triggeringPathSegment)
		{
			m_mover = mover;
			m_triggeringPathSegment = triggeringPathSegment;
		}
	}

	public class EffectRemovalData
	{
		public Effect m_effectToRemove;

		public List<Effect> m_effectListToRemoveFrom;

		public EffectRemovalData(Effect effectToRemove, List<Effect> effectListToRemoveFrom)
		{
			m_effectToRemove = effectToRemove;
			m_effectListToRemoveFrom = effectListToRemoveFrom;
		}
	}

	public class EffectRefreshData
	{
		public Effect m_effectToRefresh;

		public List<Effect> m_effectListOfEffect;

		public EffectRefreshData(Effect effectToRefresh, List<Effect> effectListOfEffect)
		{
			m_effectToRefresh = effectToRefresh;
			m_effectListOfEffect = effectListOfEffect;
		}
	}

	public class BarrierRemovalData
	{
		public Barrier m_barrierToRemove;

		public bool m_removeLinkedBarriers;

		public BarrierRemovalData(Barrier barrierToRemove, bool removeLinkedBarriers)
		{
			m_barrierToRemove = barrierToRemove;
			m_removeLinkedBarriers = removeLinkedBarriers;
		}
	}

	public class PowerupRemovalData
	{
		public PowerUp m_powerupToRemove;

		public PowerupRemovalData(PowerUp powerupToRemove)
		{
			m_powerupToRemove = powerupToRemove;
		}
	}

	public class PowerUpStealData
	{
		public PowerUp m_powerUp;

		public ActorData m_thief;

		public AbilityResults_Powerup m_results;

		public PowerUpStealData(PowerUp powerUp, ActorData thief)
		{
			m_powerUp = powerUp;
			m_thief = thief;
			m_results = null;
		}
	}
}
#endif

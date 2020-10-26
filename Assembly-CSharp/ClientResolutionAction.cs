using System;
using System.Collections.Generic;
using UnityEngine;

public class ClientResolutionAction : IComparable
{
	private ResolutionActionType m_type;
	private ClientAbilityResults m_abilityResults;
	private ClientEffectResults m_effectResults;
	private ClientMovementResults m_moveResults;

	public ClientResolutionAction()
	{
		m_type = ResolutionActionType.Invalid;
		m_abilityResults = null;
		m_effectResults = null;
	}

	public int CompareTo(object obj)
	{
		if (obj == null)
		{
			return 1;
		}
		ClientResolutionAction clientResolutionAction = obj as ClientResolutionAction;
		if (clientResolutionAction == null)
		{
			throw new ArgumentException("Object is not a ClientResolutionAction");
		}
		if (ReactsToMovement() != clientResolutionAction.ReactsToMovement())
		{
			return ReactsToMovement().CompareTo(clientResolutionAction.ReactsToMovement());
		}
		if (!ReactsToMovement() && !clientResolutionAction.ReactsToMovement())
		{
			return 0;
		}
		float moveCost = m_moveResults.m_triggeringPath.moveCost;
		float moveCost2 = clientResolutionAction.m_moveResults.m_triggeringPath.moveCost;
		if (moveCost != moveCost2)
		{
			return moveCost.CompareTo(moveCost2);
		}
		bool hasBarriers = m_moveResults.HasBarrierHitResults();
		bool hasBarriers2 = clientResolutionAction.m_moveResults.HasBarrierHitResults();
		if (hasBarriers && !hasBarriers2)
		{
			return -1;
		}
		if (!hasBarriers && hasBarriers2)
		{
			return 1;
		}
		bool hasGameMode = m_moveResults.HasGameModeHitResults();
		bool hasGameMode2 = clientResolutionAction.m_moveResults.HasGameModeHitResults();
		if (hasGameMode && !hasGameMode2)
		{
			return -1;
		}
		if (!hasGameMode && hasGameMode2)
		{
			return 1;
		}
		return 0;
	}

	public static ClientResolutionAction ClientResolutionAction_DeSerializeFromStream(ref IBitStream stream)
	{
		ClientResolutionAction clientResolutionAction = new ClientResolutionAction();
		sbyte actionType = -1;
		stream.Serialize(ref actionType);
		clientResolutionAction.m_type = (ResolutionActionType)actionType;

		switch (clientResolutionAction.m_type)
		{
			case ResolutionActionType.AbilityCast:
				clientResolutionAction.m_abilityResults = AbilityResultsUtils.DeSerializeClientAbilityResultsFromStream(ref stream);
				break;
			case ResolutionActionType.EffectAnimation:
			case ResolutionActionType.EffectPulse:
				clientResolutionAction.m_effectResults = AbilityResultsUtils.DeSerializeClientEffectResultsFromStream(ref stream);
				break;
			case ResolutionActionType.EffectOnMove:
			case ResolutionActionType.BarrierOnMove:
			case ResolutionActionType.PowerupOnMove:
			case ResolutionActionType.GameModeOnMove:
				clientResolutionAction.m_moveResults = AbilityResultsUtils.DeSerializeClientMovementResultsFromStream(ref stream);
				break;
		}
		return clientResolutionAction;
	}

	public string json()
	{
		var jsonLog = new List<string>();
		jsonLog.Add($"\"type\":{DefaultJsonSerializer.Serialize(m_type)}");
		switch (m_type)
		{
			case ResolutionActionType.AbilityCast:
				jsonLog.Add($"\"abilityResults\":{DefaultJsonSerializer.Serialize(m_abilityResults)}");
				break;
			case ResolutionActionType.EffectAnimation:
			case ResolutionActionType.EffectPulse:
				jsonLog.Add($"\"effectResults\":{DefaultJsonSerializer.Serialize(m_effectResults)}");
				break;
			case ResolutionActionType.EffectOnMove:
			case ResolutionActionType.BarrierOnMove:
			case ResolutionActionType.PowerupOnMove:
			case ResolutionActionType.GameModeOnMove:
				jsonLog.Add($"\"moveResults\":{DefaultJsonSerializer.Serialize(m_moveResults)}");
				break;
		}

		return $"{{{String.Join(",", jsonLog.ToArray())}}}";
	}

	public ActorData GetCaster()
	{
		return m_abilityResults?.GetCaster()
			?? m_effectResults?.GetCaster()
			?? null;
	}

	public AbilityData.ActionType GetSourceAbilityActionType()
	{
		return m_abilityResults?.GetSourceActionType()
			?? m_effectResults?.GetSourceActionType()
			?? AbilityData.ActionType.INVALID_ACTION;
	}

	public bool IsResolutionActionType(ResolutionActionType testType)
	{
		return m_type == testType;
	}

	public bool HasReactionHitByCaster(ActorData caster)
	{
		return m_abilityResults?.HasReactionByCaster(caster)
			?? m_effectResults?.HasReactionByCaster(caster)
			?? false;
	}

	public void GetHitResults(out Dictionary<ActorData, ClientActorHitResults> actorHitResList, out Dictionary<Vector3, ClientPositionHitResults> posHitResList)
	{
		actorHitResList = null;
		posHitResList = null;
		if (m_abilityResults != null)
		{
			actorHitResList = m_abilityResults.GetActorHitResults();
			posHitResList = m_abilityResults.GetPosHitResults();

		}
		else if (m_effectResults != null)
		{
			actorHitResList = m_effectResults.GetActorHitResults();
			posHitResList = m_effectResults.GetPosHitResults();
		}
	}

	public void GetReactionHitResultsByCaster(ActorData caster, out Dictionary<ActorData, ClientActorHitResults> actorHitResList, out Dictionary<Vector3, ClientPositionHitResults> posHitResList)
	{
		actorHitResList = null;
		posHitResList = null;
		if (m_abilityResults != null)
		{
			m_abilityResults.GetReactionHitResultsByCaster(caster, out actorHitResList, out posHitResList);
		}
		else if (m_effectResults != null)
		{
			m_effectResults.GetReactionHitResultsByCaster(caster, out actorHitResList, out posHitResList);
		}
	}

	public void RunStartSequences()
	{
		if (m_abilityResults != null)
		{
			m_abilityResults.StartSequences();
		}
		if (m_effectResults != null)
		{
			m_effectResults.StartSequences();
		}
	}

	public void Run_OutsideResolution()
	{
		if (m_abilityResults != null)
		{
			m_abilityResults.StartSequences();
		}
		if (m_effectResults != null)
		{
			m_effectResults.StartSequences();
		}
		if (m_moveResults != null)
		{
			m_moveResults.ReactToMovement();
		}
	}

	public bool CompletedAction()
	{
		switch (m_type)
		{
			case ResolutionActionType.AbilityCast:
				return m_abilityResults.DoneHitting();
			case ResolutionActionType.EffectAnimation:
			case ResolutionActionType.EffectPulse:
				return m_effectResults.DoneHitting();
			case ResolutionActionType.EffectOnMove:
			case ResolutionActionType.BarrierOnMove:
			case ResolutionActionType.PowerupOnMove:
			case ResolutionActionType.GameModeOnMove:
				return m_moveResults.DoneHitting();
			default:
				Debug.LogError("ClientResolutionAction has unknown type: " + (int)m_type + ".  Assuming it's complete...");
				return true;
		}
	}

	public void ExecuteUnexecutedClientHitsInAction()
	{
		switch (m_type)
		{
			case ResolutionActionType.AbilityCast:
				m_abilityResults.ExecuteUnexecutedClientHits();
				break;
			case ResolutionActionType.EffectAnimation:
			case ResolutionActionType.EffectPulse:
				m_effectResults.ExecuteUnexecutedClientHits();
				break;
			case ResolutionActionType.EffectOnMove:
			case ResolutionActionType.BarrierOnMove:
			case ResolutionActionType.PowerupOnMove:
			case ResolutionActionType.GameModeOnMove:
				m_moveResults.ExecuteUnexecutedClientHits();
				break;
		}
	}

	public bool HasUnexecutedHitOnActor(ActorData actor)
	{
		switch (m_type)
		{
			case ResolutionActionType.AbilityCast:
				return m_abilityResults.HasUnexecutedHitOnActor(actor);
			case ResolutionActionType.EffectAnimation:
			case ResolutionActionType.EffectPulse:
				return m_effectResults.HasUnexecutedHitOnActor(actor);
			case ResolutionActionType.EffectOnMove:
			case ResolutionActionType.BarrierOnMove:
			case ResolutionActionType.PowerupOnMove:
			case ResolutionActionType.GameModeOnMove:
				return m_moveResults.HasUnexecutedHitOnActor(actor);
			default:
				return false;
		}
	}

	public void ExecuteReactionHitsWithExtraFlagsOnActor(ActorData targetActor, ActorData caster, bool hasDamage, bool hasHealing)
	{
		switch (m_type)
		{
			case ResolutionActionType.AbilityCast:
				m_abilityResults.ExecuteReactionHitsWithExtraFlagsOnActor(targetActor, caster, hasDamage, hasHealing);
				break;
			case ResolutionActionType.EffectAnimation:
			case ResolutionActionType.EffectPulse:
				m_effectResults.ExecuteReactionHitsWithExtraFlagsOnActor(targetActor, caster, hasDamage, hasHealing);
				break;
			case ResolutionActionType.EffectOnMove:
			case ResolutionActionType.BarrierOnMove:
			case ResolutionActionType.PowerupOnMove:
			case ResolutionActionType.GameModeOnMove:
				m_moveResults.ExecuteReactionHitsWithExtraFlagsOnActor(targetActor, caster, hasDamage, hasHealing);
				break;
		}
	}

	public static bool DoneHitting(Dictionary<ActorData, ClientActorHitResults> actorToHitResults, Dictionary<Vector3, ClientPositionHitResults> positionHitResults)
	{
		bool executedAllActorHits = true;
		bool executedAllPositionHits = true;
		foreach (ClientActorHitResults hitResults in actorToHitResults.Values)
		{
			if (!hitResults.ExecutedHit || hitResults.HasUnexecutedReactionHits())
			{
				executedAllActorHits = false;
				break;
			}
		}
		foreach (ClientPositionHitResults hitResults in positionHitResults.Values)
		{
			if (!hitResults.ExecutedHit)
			{
				executedAllPositionHits = false;
				break;
			}
		}
		return executedAllActorHits && executedAllPositionHits;
	}

	public static bool HasUnexecutedHitOnActor(ActorData targetActor, Dictionary<ActorData, ClientActorHitResults> actorToHitResults)
	{
		foreach (KeyValuePair<ActorData, ClientActorHitResults> hitResults in actorToHitResults)
		{
			if (!hitResults.Value.ExecutedHit && hitResults.Key.ActorIndex == targetActor.ActorIndex)
			{
				return true;
			}
			if (hitResults.Value.HasUnexecutedReactionOnActor(targetActor))
			{
				return true;
			}
		}
		return false;
	}

	public static void ExecuteUnexecutedHits(Dictionary<ActorData, ClientActorHitResults> actorToHitResults, Dictionary<Vector3, ClientPositionHitResults> positionHitResults, ActorData caster)
	{
		foreach (KeyValuePair<ActorData, ClientActorHitResults> hitResults in actorToHitResults)
		{
			if (!hitResults.Value.ExecutedHit)
			{
				if (ClientAbilityResults.DebugTraceOn)
				{
					Log.Warning(ClientAbilityResults.s_clientHitResultHeader + "Executing unexecuted actor hit on " + hitResults.Key.DebugNameString() + " from " + caster.DebugNameString());
				}
				hitResults.Value.ExecuteActorHit(hitResults.Key, caster);
			}
		}
		foreach (KeyValuePair<Vector3, ClientPositionHitResults> hitResults in positionHitResults)
		{
			if (!hitResults.Value.ExecutedHit)
			{
				if (ClientAbilityResults.DebugTraceOn)
				{
					Log.Warning(ClientAbilityResults.s_clientHitResultHeader + "Executing unexecuted position hit on " + hitResults.Key.ToString() + " from " + caster.DebugNameString());
				}
				hitResults.Value.ExecutePositionHit();
			}
		}
	}

	public static void ExecuteReactionHitsWithExtraFlagsOnActorAux(Dictionary<ActorData, ClientActorHitResults> actorToHitResults, ActorData targetActor, ActorData caster, bool hasDamage, bool hasHealing)
	{
		foreach (KeyValuePair<ActorData, ClientActorHitResults> hitResults in actorToHitResults)
		{
			if (!hitResults.Value.ExecutedHit && hitResults.Key == targetActor)
			{
				hitResults.Value.ExecuteReactionHitsWithExtraFlagsOnActor(targetActor, caster, hasDamage, hasHealing);
			}
		}
	}

	public static bool HasReactionHitByCaster(ActorData caster, Dictionary<ActorData, ClientActorHitResults> actorHitResults)
	{
		foreach (KeyValuePair<ActorData, ClientActorHitResults> hitResults in actorHitResults)
		{
			if (hitResults.Value.HasReactionHitByCaster(caster))
			{
				return true;
			}
		}
		return false;
	}

	public static void GetReactionHitResultsByCaster(ActorData caster, Dictionary<ActorData, ClientActorHitResults> actorHitResults, out Dictionary<ActorData, ClientActorHitResults> reactionActorHits, out Dictionary<Vector3, ClientPositionHitResults> reactionPosHits)
	{
		reactionActorHits = null;
		reactionPosHits = null;
		foreach (KeyValuePair<ActorData, ClientActorHitResults> hitResults in actorHitResults)
		{
			if (hitResults.Value.HasReactionHitByCaster(caster))
			{
				hitResults.Value.GetReactionHitResultsByCaster(caster, out reactionActorHits, out reactionPosHits);
				return;
			}
		}
	}

	public bool ContainsSequenceSource(SequenceSource sequenceSource)
	{
		return sequenceSource != null && ContainsSequenceSourceID(sequenceSource.RootID);
	}

	public bool ContainsSequenceSourceID(uint id)
	{
		switch (m_type)
		{
			case ResolutionActionType.AbilityCast:
				return m_abilityResults.ContainsSequenceSourceID(id);
			case ResolutionActionType.EffectAnimation:
			case ResolutionActionType.EffectPulse:
				return m_effectResults.ContainsSequenceSourceID(id);
			case ResolutionActionType.EffectOnMove:
			case ResolutionActionType.BarrierOnMove:
			case ResolutionActionType.PowerupOnMove:
			case ResolutionActionType.GameModeOnMove:
				return m_moveResults.ContainsSequenceSourceID(id);
			default:
				Debug.LogError("ClientResolutionAction has unknown type: " + (int)m_type + ".  Assuming it does not have a given SequenceSource...");
				return false;
		}
	}

	public bool ReactsToMovement()
	{
		return m_type == ResolutionActionType.EffectOnMove
			|| m_type == ResolutionActionType.BarrierOnMove
			|| m_type == ResolutionActionType.PowerupOnMove
			|| m_type == ResolutionActionType.GameModeOnMove;
	}

	public ActorData GetTriggeringMovementActor()
	{
		if (m_moveResults != null)
		{
			return m_moveResults.m_triggeringMover;
		}
		return null;
	}

	public void OnActorMoved_ClientResolutionAction(ActorData mover, BoardSquarePathInfo curPath)
	{
		if (m_moveResults.TriggerMatchesMovement(mover, curPath))
		{
			m_moveResults.ReactToMovement();
		}
	}

	public void AdjustKnockbackCounts_ClientResolutionAction(ref Dictionary<ActorData, int> outgoingKnockbacks, ref Dictionary<ActorData, int> incomingKnockbacks)
	{
		switch (m_type)
		{
			case ResolutionActionType.AbilityCast:
				m_abilityResults.AdjustKnockbackCounts_ClientAbilityResults(ref outgoingKnockbacks, ref incomingKnockbacks);
				break;
			case ResolutionActionType.EffectAnimation:
			case ResolutionActionType.EffectPulse:
				m_effectResults.AdjustKnockbackCounts_ClientEffectResults(ref outgoingKnockbacks, ref incomingKnockbacks);
				break;
		}
	}

	public string GetDebugDescription()
	{
		string str = m_type.ToString() + ": ";
		switch (m_type)
		{
			case ResolutionActionType.AbilityCast:
				return str + m_abilityResults.GetDebugDescription();
			case ResolutionActionType.EffectAnimation:
			case ResolutionActionType.EffectPulse:
				return str + m_effectResults.GetDebugDescription();
			case ResolutionActionType.EffectOnMove:
			case ResolutionActionType.BarrierOnMove:
			case ResolutionActionType.PowerupOnMove:
			case ResolutionActionType.GameModeOnMove:
				return str + m_moveResults.GetDebugDescription();
			default:
				return str + "??? (invalid results)";
		}
	}

	public string GetUnexecutedHitsDebugStr(bool logSequenceDataActors = false)
	{
		string text;
		switch (m_type)
		{
			case ResolutionActionType.AbilityCast:
				text = m_abilityResults.UnexecutedHitsDebugStr();
				if (logSequenceDataActors)
				{
					text = text + "\n" + m_abilityResults.GetSequenceStartDataDebugStr() + "\n";
				}
				break;
			case ResolutionActionType.EffectAnimation:
			case ResolutionActionType.EffectPulse:
				text = m_effectResults.UnexecutedHitsDebugStr();
				break;
			case ResolutionActionType.EffectOnMove:
			case ResolutionActionType.BarrierOnMove:
			case ResolutionActionType.PowerupOnMove:
			case ResolutionActionType.GameModeOnMove:
				text = m_moveResults.UnexecutedHitsDebugStr();
				break;
			default:
				text = "";
				break;
		}
		return text;
	}

	public static string AssembleUnexecutedHitsDebugStr(Dictionary<ActorData, ClientActorHitResults> actorToHitResults, Dictionary<Vector3, ClientPositionHitResults> positionToHitResults)
	{
		int numUnexecuted = 0;
		int numExecuted = 0;
		string unexecuted = "";
		string executed = "";
		foreach (KeyValuePair<ActorData, ClientActorHitResults> hitResult in actorToHitResults)
		{
			if (!hitResult.Value.ExecutedHit)
			{
				numUnexecuted++;
				unexecuted += "\n\t\t" + numUnexecuted + ". ActorHit on " + hitResult.Key.DebugNameString();
			}
			else
			{
				numExecuted++;
				executed += "\n\t\t" + numExecuted + ". ActorHit on " + hitResult.Key.DebugNameString();
			}
		}
		foreach (KeyValuePair<Vector3, ClientPositionHitResults> hitResult in positionToHitResults)
		{
			if (!hitResult.Value.ExecutedHit)
			{
				numUnexecuted++;
				unexecuted += "\n\t\t" + numUnexecuted + ". PositionHit on " + hitResult.Key.ToString();
			}
			else
			{
				numExecuted++;
				executed += "\n\t\t" + numExecuted + ". PositionHit on " + hitResult.Key.ToString();
			}
		}
		string str = "\n\tUnexecuted hits: " + numUnexecuted + unexecuted;
		if (numExecuted > 0)
		{
			str += "\n\tExecuted hits: " + numExecuted + executed;
		}
		return str + "\n";
	}
}

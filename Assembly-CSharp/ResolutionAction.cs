// ROGUES
// SERVER
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

// server-only -- was empty in reactor
public class ResolutionAction
{
#if SERVER
	private ResolutionActionType m_type;
	internal AbilityResults m_abilityResults;
	private EffectResults m_effectResults;
	private MovementResults m_moveResults;

	public string m_debugStr;

	public static ResolutionAction ConstructFromAbilityRequest(AbilityRequest request)
	{
		ResolutionAction resolutionAction = new ResolutionAction();
		resolutionAction.m_type = ResolutionActionType.AbilityCast;
		resolutionAction.m_abilityResults = request.m_additionalData.m_abilityResults;
		resolutionAction.m_effectResults = null;
		resolutionAction.m_moveResults = null;
		resolutionAction.m_debugStr = "(" + resolutionAction.m_type.ToString() + ") " + request.m_ability.m_abilityName;
		return resolutionAction;
	}

	public static List<ResolutionAction> ConstructFromEffectResult(EffectResults result)
	{
		List<ResolutionAction> list = new List<ResolutionAction>();
		if (!result.m_actorToHitResults.Any((KeyValuePair<ActorData, ActorHitResults> hitResults) => !hitResults.Value.ExecutedResults))
		{
			if (!result.m_positionToHitResults.Any((KeyValuePair<Vector3, PositionHitResults> hitResults) => !hitResults.Value.ExecutedResults))
			{
				return list;
			}
		}
		ResolutionAction resolutionAction = new ResolutionAction();
		if (result.Effect.GetCasterAnimationIndex(result.Effect.HitPhase) == 0)
		{
			resolutionAction.m_type = ResolutionActionType.EffectPulse;
		}
		else
		{
			resolutionAction.m_type = ResolutionActionType.EffectAnimation;
		}
		resolutionAction.m_abilityResults = null;
		resolutionAction.m_effectResults = result;
		resolutionAction.m_moveResults = null;
		resolutionAction.m_debugStr = "(" + resolutionAction.m_type.ToString() + ") " + result.Effect.m_effectName;
		list.Add(resolutionAction);
		return list;
	}

	// rogues?
	//public static List<ResolutionAction> ConstructFromEffect(EffectSystem.Effect effect)
	//{
	//	List<ResolutionAction> list = new List<ResolutionAction>();
	//	foreach (EffectResults effectResults in effect.pendingEffectResults.Where(delegate(EffectResults result)
	//	{
	//		if (!result.m_actorToHitResults.Any((KeyValuePair<ActorData, ActorHitResults> hitResults) => !hitResults.Value.ExecutedResults))
	//		{
	//			if (!result.m_positionToHitResults.Any((KeyValuePair<Vector3, PositionHitResults> hitResults) => !hitResults.Value.ExecutedResults))
	//			{
	//				return result.m_sequenceStartData.Any<ServerClientUtils.SequenceStartData>();
	//			}
	//		}
	//		return true;
	//	}))
	//	{
	//		ResolutionAction resolutionAction = new ResolutionAction();
	//		if (effect.GetCasterAnimationIndex(effect.HitPhase) == 0)
	//		{
	//			resolutionAction.m_type = ResolutionActionType.EffectPulse;
	//		}
	//		else
	//		{
	//			resolutionAction.m_type = ResolutionActionType.EffectAnimation;
	//		}
	//		resolutionAction.m_abilityResults = null;
	//		resolutionAction.m_effectResults = effectResults;
	//		resolutionAction.m_moveResults = null;
	//		resolutionAction.m_debugStr = "(" + resolutionAction.m_type.ToString() + ") " + effect.m_effectName;
	//		list.Add(resolutionAction);
	//	}
	//	return list;
	//}

	public static List<ResolutionAction> ConstructFromEffect(global::Effect effect)
	{
		ResolutionAction resolutionAction = new ResolutionAction();
		if (effect.GetCasterAnimationIndex(effect.HitPhase) == 0)
		{
			resolutionAction.m_type = ResolutionActionType.EffectPulse;
		}
		else
		{
			resolutionAction.m_type = ResolutionActionType.EffectAnimation;
		}
		resolutionAction.m_abilityResults = null;
		resolutionAction.m_effectResults = effect.GetResultsForPhase(effect.HitPhase, true);
		resolutionAction.m_moveResults = null;
		resolutionAction.m_debugStr = "(" + resolutionAction.m_type.ToString() + ") " + effect.m_effectName;
		return new List<ResolutionAction>
		{
			resolutionAction
		};
	}

	public static ResolutionAction ConstructFromMoveResults(MovementResults results)
	{
		ResolutionAction resolutionAction = new ResolutionAction();
		if (results.m_gameplayResponseType == MovementResults_GameplayResponseType.Effect)
		{
			resolutionAction.m_type = ResolutionActionType.EffectOnMove;
			resolutionAction.m_debugStr = "(" + resolutionAction.m_type.ToString() + ") " + results.GetEffectResults().Effect.m_effectName;
		}
		else if (results.m_gameplayResponseType == MovementResults_GameplayResponseType.Barrier)
		{
			resolutionAction.m_type = ResolutionActionType.BarrierOnMove;
			resolutionAction.m_debugStr = "(" + resolutionAction.m_type.ToString() + ") " + results.GetBarrierResults().Barrier.Name;
		}
		else if (results.m_gameplayResponseType == MovementResults_GameplayResponseType.Powerup)
		{
			resolutionAction.m_type = ResolutionActionType.PowerupOnMove;
			resolutionAction.m_debugStr = "(" + resolutionAction.m_type.ToString() + ") " + results.GetPowerUpResults().Ability.m_abilityName;
		}
		else if (results.m_gameplayResponseType == MovementResults_GameplayResponseType.GameMode)
		{
			resolutionAction.m_type = ResolutionActionType.GameModeOnMove;
			resolutionAction.m_debugStr = "(" + resolutionAction.m_type.ToString() + ")" + results.GetGameModeResults().Ability.m_abilityName;
		}
		resolutionAction.m_abilityResults = null;
		resolutionAction.m_effectResults = null;
		resolutionAction.m_moveResults = results;
		return resolutionAction;
	}

	public AbilityResults AbilityResults
	{
		get
		{
			return m_abilityResults;
		}
	}

	public EffectResults EffectResults
	{
		get
		{
			return m_effectResults;
		}
	}

	public MovementResults MovementResults
	{
		get
		{
			return m_moveResults;
		}
	}

	private ActorData GetAnimatingActor()
	{
		if (m_type == ResolutionActionType.AbilityCast)
		{
			return m_abilityResults.Caster;
		}
		if (m_type == ResolutionActionType.EffectAnimation)
		{
			return m_effectResults.Caster;
		}
		return null;
	}

	public bool ReactToMovement()
	{
		if (m_type != ResolutionActionType.EffectOnMove && m_type != ResolutionActionType.BarrierOnMove && m_type != ResolutionActionType.PowerupOnMove && m_type != ResolutionActionType.GameModeOnMove)
		{
			return false;
		}
		if (m_moveResults != null)
		{
			return true;
		}
		Log.Error("ResolutionAction has type " + m_type.ToString() + " but its move results is null.");
		return false;
	}

	private void OnActorMoved(ActorData mover, BoardSquarePathInfo curPath)
	{
		if (!ReactToMovement())
		{
			return;
		}
		m_moveResults.TriggerMatchesMovement(mover, curPath);
	}

	public void ResolutionAction_SerializeToStream(NetworkWriter writer)
	{
		sbyte b = checked((sbyte)m_type);
		writer.Write(b);
		if (m_type == ResolutionActionType.AbilityCast)
		{
			AbilityResultsUtils.SerializeServerAbilityResultsToStream(m_abilityResults, true, writer);
		}
		else if (m_type == ResolutionActionType.EffectAnimation || m_type == ResolutionActionType.EffectPulse)
		{
			AbilityResultsUtils.SerializeServerEffectResultsToStream(m_effectResults, true, writer);
		}
		else if (m_type == ResolutionActionType.EffectOnMove || m_type == ResolutionActionType.BarrierOnMove || m_type == ResolutionActionType.PowerupOnMove || m_type == ResolutionActionType.GameModeOnMove)
		{
			AbilityResultsUtils.SerializeServerMovementResultsToStream(m_moveResults, writer);
		}
		if (ClientAbilityResults.DebugSerializeSizeOn)
		{
			Debug.LogWarning("\t <color=yellow>Serializing Action:</color> " + m_debugStr);
		}
	}
#endif
}

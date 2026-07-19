// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

// server-only, missing in reactor
#if SERVER
public class MovementResults
{
	private MovementStage m_forMovementStage;
	private List<GameObject> m_sequencePrefabs;
	private BoardSquare m_targetSquare;
	private ActorData m_caster;
	private SequenceSource m_sequenceSource;
	private Sequence.IExtraSequenceParams[] m_extraParams;
	private List<ServerClientUtils.SequenceStartData> m_sequenceStartDataOverrides;
	private EffectResults m_effectResults;
	private BarrierResults m_barrierResults;
	private AbilityResults m_powerupResults;
	private AbilityResults m_gameModeResults;

	public ActorData m_triggeringMover;
	public BoardSquarePathInfo m_triggeringPath;
	public BoardSquarePathInfo m_triggeringPath_endingAtHit;

	private bool m_alreadyReacted;
	private string m_resultsName = "";

	public MovementResults_GameplayResponseType m_gameplayResponseType;

	public MovementResults(
		MovementStage moveStage,
		Effect reactingEffect,
		ActorHitResults reactionHitResults,
		ActorData triggeringMover,
		BoardSquarePathInfo triggeringPathSegment,
		GameObject sequencePrefab,
		BoardSquare sequenceTargetSquare,
		SequenceSource parentSequenceSource,
		Sequence.IExtraSequenceParams[] extraParams = null)
	{
		m_forMovementStage = moveStage;
		SetupTriggerData(triggeringMover, triggeringPathSegment);
		SetupSequenceData(sequencePrefab, sequenceTargetSquare, parentSequenceSource, extraParams);
		SetupGameplayData(reactingEffect, reactionHitResults);
	}

	public MovementResults(MovementStage moveStage)
	{
		m_forMovementStage = moveStage;
	}

	public void SetupTriggerData(ServerAbilityUtils.TriggeringPathInfo triggerInfo)
	{
		m_triggeringMover = triggerInfo.m_mover;
		m_triggeringPath = triggerInfo.m_triggeringPathSegment;
		if (m_triggeringPath != null)
		{
			m_triggeringPath.CheckIsValidTriggeringPath(m_triggeringMover);
		}
	}

	public void SetupTriggerData(ActorData triggeringMover, BoardSquarePathInfo triggeringPath)
	{
		m_triggeringMover = triggeringMover;
		m_triggeringPath = triggeringPath;
		if (m_triggeringPath != null)
		{
			m_triggeringPath.CheckIsValidTriggeringPath(m_triggeringMover);
			m_triggeringPath.m_moverHasGameplayHitHere = true;
		}
	}

	public void SetupSequenceData(
		GameObject sequencePrefab,
		BoardSquare sequenceTargetSquare,
		SequenceSource parentSequenceSource,
		Sequence.IExtraSequenceParams[] extraParams = null,
		bool removeAtEndOfTurn = true)
	{
		m_sequencePrefabs = new List<GameObject> { sequencePrefab };
		m_targetSquare = sequenceTargetSquare;
		m_sequenceSource = new SequenceSource(
			OnMovementReactionHitActor,
			OnMovementReactionHitPosition,
			removeAtEndOfTurn,
			parentSequenceSource);
		m_extraParams = extraParams;
	}

	public void SetupSequenceDataList(
		List<GameObject> sequencePrefabList,
		BoardSquare sequenceTargetSquare,
		SequenceSource parentSequenceSource,
		Sequence.IExtraSequenceParams[] extraParams = null)
	{
		m_sequencePrefabs = new List<GameObject>();
		if (sequencePrefabList != null)
		{
			m_sequencePrefabs.AddRange(sequencePrefabList);
		}
		m_targetSquare = sequenceTargetSquare;
		m_sequenceSource = new SequenceSource(
			OnMovementReactionHitActor,
			OnMovementReactionHitPosition,
			true,
			parentSequenceSource);
		m_extraParams = extraParams;
	}

	public void AddSequenceStartOverride(
		ServerClientUtils.SequenceStartData startData,
		SequenceSource parentSequenceSource,
		bool removeAtEndOfTurn = true)
	{
		if (startData != null)
		{
			if (m_sequenceStartDataOverrides == null)
			{
				m_sequenceStartDataOverrides = new List<ServerClientUtils.SequenceStartData>();
			}
			SequenceSource source = new SequenceSource(
				OnMovementReactionHitActor,
				OnMovementReactionHitPosition,
				removeAtEndOfTurn,
				parentSequenceSource);
			startData.InitSequenceSourceData(source);
			m_sequenceStartDataOverrides.Add(startData);
		}
	}

	public EffectResults SetupGameplayData(Effect reactingEffect, ActorHitResults reactionHitResults)
	{
		m_effectResults = new EffectResults(reactingEffect, null, true);
		// rogues
		// reactionHitResults.CanBeReactedTo = false;
		// reactionHitResults.ForMovementStage = m_forMovementStage;
		if (reactionHitResults != null)
		{
			// custom
			reactionHitResults.CanBeReactedTo = false;
			reactionHitResults.ForMovementStage = m_forMovementStage;
			// end custom
			m_effectResults.StoreActorHit(reactionHitResults);
		}
		m_effectResults.GatheredResults = true;
		m_caster = reactingEffect.Caster;
		m_resultsName = reactingEffect.GetType() + " " + reactingEffect.m_effectName;
		m_gameplayResponseType = MovementResults_GameplayResponseType.Effect;
		return m_effectResults;
	}

	public BarrierResults SetupGameplayData(Barrier reactingBarrier, ActorHitResults reactionHitResults)
	{
		m_barrierResults = new BarrierResults(reactingBarrier);
		// rogues
		// reactionHitResults.CanBeReactedTo = false;
		// reactionHitResults.ForMovementStage = m_forMovementStage;
		if (reactionHitResults != null)
		{
			// custom
			reactionHitResults.CanBeReactedTo = false;
			reactionHitResults.ForMovementStage = m_forMovementStage;
			// end custom
			m_barrierResults.StoreActorHit(reactionHitResults);
		}
		m_barrierResults.GatheredResults = true;
		m_caster = reactingBarrier.Caster != null ? reactingBarrier.Caster : reactionHitResults.m_hitParameters.Target;
		m_resultsName = "Barrier " + reactingBarrier.Name;
		m_gameplayResponseType = MovementResults_GameplayResponseType.Barrier;
		return m_barrierResults;
	}

	public void SetupGameplayData(PowerUp reactingPowerup, ActorHitResults reactionHitResults)
	{
		ActorData actorData = reactingPowerup.GetCreator();
		if (actorData == null)
		{
			actorData = reactionHitResults.m_hitParameters.Target;
		}
		Ability ability = reactingPowerup.GetCreatorAbility();
		if (ability == null)
		{
			ability = reactingPowerup.m_ability;
		}
		m_powerupResults = new AbilityResults(actorData, ability, reactingPowerup.SequenceSource, true);
		reactionHitResults.CanBeReactedTo = false;
		reactionHitResults.ForMovementStage = m_forMovementStage;
		m_powerupResults.StoreActorHit(reactionHitResults);
		m_powerupResults.GatheredResults = true;
		m_caster = actorData;
		m_resultsName = reactingPowerup.m_powerUpName;
		m_gameplayResponseType = MovementResults_GameplayResponseType.Powerup;
	}

	public void SetupGameplayData(Ability gameModeAbility, ActorHitResults reactionHitResults)
	{
		ActorData actorData = reactionHitResults.m_hitParameters.Caster;
		if (actorData == null)
		{
			actorData = reactionHitResults.m_hitParameters.Target;
		}
		m_gameModeResults = new AbilityResults(actorData, gameModeAbility, null, true);
		reactionHitResults.CanBeReactedTo = false;
		reactionHitResults.ForMovementStage = m_forMovementStage;
		m_gameModeResults.StoreActorHit(reactionHitResults);
		m_gameModeResults.GatheredResults = true;
		m_caster = actorData;
		m_resultsName = gameModeAbility.m_abilityName;
		m_gameplayResponseType = MovementResults_GameplayResponseType.GameMode;
	}

	public void SetupGameplayDataForAbility(Ability ability, ActorHitResults hitRes, ActorData caster)
	{
		ActorData target = hitRes.m_hitParameters.Target;
		SequenceSource sequenceSource = new SequenceSource(null, null);
		m_powerupResults = new AbilityResults(target, ability, sequenceSource, true);
		hitRes.CanBeReactedTo = false;
		m_powerupResults.StoreActorHit(hitRes);
		m_powerupResults.GatheredResults = true;
		m_caster = caster;
		m_resultsName = ability.m_abilityName;
		m_gameplayResponseType = MovementResults_GameplayResponseType.Powerup;
	}

	public void SetupGameplayDataForAbility(Ability ability, ActorData caster)
	{
		SequenceSource sequenceSource = new SequenceSource(null, null);
		m_powerupResults = new AbilityResults(caster, ability, sequenceSource, true);
		m_powerupResults.GatheredResults = true;
		m_caster = caster;
		m_resultsName = ability.m_abilityName;
		m_gameplayResponseType = MovementResults_GameplayResponseType.Powerup;
	}

	public void AddActorHitResultsForReaction(ActorHitResults reactionHitResults)
	{
		if (reactionHitResults != null)
		{
			reactionHitResults.CanBeReactedTo = false;
			m_powerupResults.StoreActorHit(reactionHitResults);
		}
	}

	public void SetupForHitOutsideResolution(
		ActorData target,
		ActorData caster,
		ActorHitResults hitRes,
		Ability ability,
		GameObject seqPrefab,
		BoardSquare seqTargetSquare,
		SequenceSource seqSource,
		bool removeSequenceAtEndOfTurn,
		Sequence.IExtraSequenceParams[] extraParams = null)
	{
		SetupTriggerData(target, null);
		SetupGameplayDataForAbility(ability, hitRes, caster);
		SetupSequenceData(seqPrefab, seqTargetSquare, seqSource, extraParams, removeSequenceAtEndOfTurn);
	}

	public static void SetupAndExecuteAbilityResultsOutsideResolution(
		ActorData target,
		ActorData caster,
		ActorHitResults hitRes,
		Ability sourceAbility,
		bool removeSequenceAtEndOfTurn = true,
		GameObject inputSequencePrefab = null,
		Sequence.IExtraSequenceParams[] extraParams = null)
	{
		GameObject gameObject = inputSequencePrefab;
		if (gameObject == null)
		{
			gameObject = SequenceLookup.Get().GetSimpleHitSequencePrefab();
		}
		SequenceSource seqSource = new SequenceSource(null, null, removeSequenceAtEndOfTurn);
		MovementResults movementResults = new MovementResults(MovementStage.INVALID);
		movementResults.SetupForHitOutsideResolution(target, caster, hitRes, sourceAbility, gameObject, target.GetCurrentBoardSquare(), seqSource, removeSequenceAtEndOfTurn, extraParams);
		movementResults.ExecuteUnexecutedMovementHits(false);
		if (ServerResolutionManager.Get() != null)
		{
			ServerResolutionManager.Get().SendNonResolutionActionToClients(movementResults);
		}
	}

	public Dictionary<ActorData, int> GetMovementDamageResults()
	{
		if (m_effectResults != null)
		{
			return m_effectResults.DamageResults;
		}
		if (m_barrierResults != null)
		{
			return m_barrierResults.DamageResults;
		}
		if (m_powerupResults != null)
		{
			return m_powerupResults.DamageResults;
		}
		if (m_gameModeResults != null)
		{
			return m_gameModeResults.DamageResults;
		}
		return null;
	}

	public Dictionary<ActorData, int> GetMovementDamageResults_Gross()
	{
		if (m_effectResults != null)
		{
			return m_effectResults.DamageResults_Gross;
		}
		if (m_barrierResults != null)
		{
			return m_barrierResults.DamageResults_Gross;
		}
		if (m_powerupResults != null)
		{
			return m_powerupResults.DamageResults_Gross;
		}
		if (m_gameModeResults != null)
		{
			return m_gameModeResults.DamageResults_Gross;
		}
		return null;
	}

	public bool ShouldMovementHitUpdateTargetLastKnownPos(ActorData mover)
	{
		bool result;
		if (m_effectResults != null)
		{
			result = m_effectResults.ShouldMovementHitUpdateTargetLastKnownPos(mover);
		}
		else if (m_barrierResults != null)
		{
			result = m_barrierResults.ShouldMovementHitUpdateTargetLastKnownPos(mover);
		}
		else if (m_powerupResults != null)
		{
			result = m_powerupResults.ShouldMovementHitUpdateTargetLastKnownPos(mover);
		}
		else
		{
			result = m_gameModeResults != null && m_gameModeResults.ShouldMovementHitUpdateTargetLastKnownPos(mover);
		}
		return result;
	}

	public virtual List<ServerClientUtils.SequenceStartData> GetTriggerSeqDataList()
	{
		List<ServerClientUtils.SequenceStartData> list = new List<ServerClientUtils.SequenceStartData>();
		ActorData[] targetActorArray;
		if (m_effectResults != null)
		{
			targetActorArray = m_effectResults.HitActorsArray();
		}
		else if (m_barrierResults != null)
		{
			targetActorArray = m_barrierResults.HitActorsArray();
		}
		else if (m_powerupResults != null)
		{
			targetActorArray = m_powerupResults.HitActorsArray();
		}
		else if (m_gameModeResults != null)
		{
			targetActorArray = m_gameModeResults.HitActorsArray();
		}
		else
		{
			targetActorArray = new ActorData[0];
		}
		if (m_sequenceStartDataOverrides != null && m_sequenceStartDataOverrides.Count > 0)
		{
			foreach (ServerClientUtils.SequenceStartData sequenceStartData in m_sequenceStartDataOverrides)
			{
				sequenceStartData.InitTargetActors(targetActorArray);
				list.Add(sequenceStartData);
			}
			return list;
		}
		foreach (GameObject prefab in m_sequencePrefabs)
		{
			list.Add(new ServerClientUtils.SequenceStartData(prefab, m_targetSquare, targetActorArray, m_caster, m_sequenceSource, m_extraParams));
		}
		return list;
	}

	public static void ExecuteUnexecutedHits(List<MovementResults> moveResults, bool asFailsafe)
	{
		foreach (MovementResults movementResults in moveResults)
		{
			if (movementResults.m_alreadyReacted)
			{
				Debug.LogError($"Executing unexecuted movement hits for {movementResults.m_resultsName}, " +
				               $"but it's already reacted.  (Failsafe = {asFailsafe})");
			}
			movementResults.ExecuteUnexecutedMovementHits(asFailsafe);
		}
	}

	public void ExecuteUnexecutedMovementHits(bool asFailsafe)
	{
		Dictionary<ActorData, ActorHitResults> actorToHitResults;
		Dictionary<Vector3, PositionHitResults> posToHitResults;
		if (m_effectResults != null)
		{
			actorToHitResults = m_effectResults.m_actorToHitResults;
			posToHitResults = m_effectResults.m_positionToHitResults;
		}
		else if (m_barrierResults != null)
		{
			actorToHitResults = m_barrierResults.m_actorToHitResults;
			posToHitResults = m_barrierResults.m_positionToHitResults;
		}
		else if (m_powerupResults != null)
		{
			actorToHitResults = m_powerupResults.m_actorToHitResults;
			posToHitResults = m_powerupResults.m_positionToHitResults;
		}
		else if (m_gameModeResults != null)
		{
			actorToHitResults = m_gameModeResults.m_actorToHitResults;
			posToHitResults = m_gameModeResults.m_positionToHitResults;
		}
		else
		{
			actorToHitResults = new Dictionary<ActorData, ActorHitResults>();
			posToHitResults = new Dictionary<Vector3, PositionHitResults>();
		}
		foreach (ActorHitResults actorHitResults in actorToHitResults.Values)
		{
			if (actorHitResults.ExecutedResults)
			{
				continue;
			}
			ActorData caster = actorHitResults.m_hitParameters.Caster;
			ActorData target = actorHitResults.m_hitParameters.Target;
			if (asFailsafe)
			{
				Debug.LogError($"{caster.DebugNameString()}'s {m_resultsName} force-executing movement hit " +
				               $"on actor {target.name} {target.DisplayName} due to failsafe.");
			}
			if (NetworkServer.active && actorHitResults.m_hitParameters.Target.IsDead())
			{
				Log.Error($"Trying to execute movement hit on dead actor: " +
				          $"{actorHitResults.m_hitParameters.Target.DebugNameString()}");
			}
			actorHitResults.ExecuteResults();
			if (actorHitResults.HasReactions)
			{
				actorHitResults.ExecuteUnexecutedReactionHits(asFailsafe);
			}
			if (actorHitResults.ForMovementStage == MovementStage.Knockback)
			{
				List<ActorData> knockbackSourceActorsOnTarget = ServerKnockbackManager.Get().GetKnockbackSourceActorsOnTarget(target);
				if (knockbackSourceActorsOnTarget != null)
				{
					foreach (ActorData actorData in knockbackSourceActorsOnTarget)
					{
						actorData.OnKnockbackHitExecutedOnTarget(target, actorHitResults);
					}
				}
			}
		}
		foreach (PositionHitResults positionHitResults in posToHitResults.Values)
		{
			if (positionHitResults.ExecutedResults)
			{
				continue;
			}
			if (asFailsafe)
			{
				ActorData caster = positionHitResults.m_hitParameters.Caster;
				Debug.LogError($"{caster.DebugNameString()}'s {m_resultsName} force-executing movement hit " +
				               $"on position {positionHitResults.m_hitParameters.Pos} due to failsafe.");
			}
			positionHitResults.ExecuteResults();
		}
		m_alreadyReacted = true;
	}

	public static void ExecuteUnexecutedHitsForDistance(
		List<MovementResults> moveResults,
		float distance,
		bool asFailsafe,
		out bool stillHasUnexecutedHits,
		out float nextUnexecutedHitsDistance)
	{
		stillHasUnexecutedHits = false;
		nextUnexecutedHitsDistance = float.MaxValue;
		foreach (MovementResults movementResults in moveResults)
		{
			if (!movementResults.StillHasUnexecutedHits())
			{
				continue;
			}
			if (movementResults.TriggersAtDistance(distance))
			{
				movementResults.ExecuteUnexecutedMovementHits(asFailsafe);
			}
			else
			{
				stillHasUnexecutedHits = true;
				if (movementResults.GetTriggerDistance() < nextUnexecutedHitsDistance)
				{
					nextUnexecutedHitsDistance = movementResults.GetTriggerDistance();
				}
			}
		}
	}

	public void OnMovementReactionHitActor(ActorData target)
	{
		bool hitsDoneExecuting = false;
		bool executeForActor = false;
		if (m_effectResults != null)
		{
			executeForActor = m_effectResults.ExecuteForActor(target);
			hitsDoneExecuting = m_effectResults.HitsDoneExecuting();
		}
		else if (m_barrierResults != null)
		{
			executeForActor = m_barrierResults.ExecuteForActor(target);
			hitsDoneExecuting = m_barrierResults.HitsDoneExecuting();
		}
		else if (m_powerupResults != null)
		{
			executeForActor = m_powerupResults.ExecuteForActor(target);
			hitsDoneExecuting = m_powerupResults.HitsDoneExecuting();
		}
		else if (m_gameModeResults != null)
		{
			executeForActor = m_gameModeResults.ExecuteForActor(target);
			hitsDoneExecuting = m_gameModeResults.HitsDoneExecuting();
		}
		if (executeForActor
		    && hitsDoneExecuting
		    && ServerClientUtils.GetCurrentAbilityPhase() == AbilityPriority.Combat_Knockback)
		{
			TheatricsManager.Get().OnKnockbackMovementHitExecuted(m_triggeringMover);
		}
	}

	public void OnMovementReactionHitPosition(Vector3 pos)
	{
		bool hitsDoneExecuting = false;
		bool executeForPosition = false;
		if (m_effectResults != null)
		{
			executeForPosition = m_effectResults.ExecuteForPosition(pos);
			hitsDoneExecuting = m_effectResults.HitsDoneExecuting();
		}
		else if (m_barrierResults != null)
		{
			executeForPosition = m_barrierResults.ExecuteForPosition(pos);
			hitsDoneExecuting = m_barrierResults.HitsDoneExecuting();
		}
		else if (m_powerupResults != null)
		{
			executeForPosition = m_powerupResults.ExecuteForPosition(pos);
			hitsDoneExecuting = m_powerupResults.HitsDoneExecuting();
		}
		else if (m_gameModeResults != null)
		{
			executeForPosition = m_gameModeResults.ExecuteForPosition(pos);
			hitsDoneExecuting = m_gameModeResults.HitsDoneExecuting();
		}
		if (executeForPosition
		    && hitsDoneExecuting
		    && ServerClientUtils.GetCurrentAbilityPhase() == AbilityPriority.Combat_Knockback)
		{
			TheatricsManager.Get().OnKnockbackMovementHitExecuted(m_triggeringMover);
		}
	}

	public ActorData GetTriggeringActor()
	{
		return m_triggeringMover;
	}

	public EffectResults GetEffectResults()
	{
		return m_effectResults;
	}

	public BarrierResults GetBarrierResults()
	{
		return m_barrierResults;
	}

	public AbilityResults GetPowerUpResults()
	{
		return m_powerupResults;
	}

	public AbilityResults GetGameModeResults()
	{
		return m_gameModeResults;
	}

	public bool TriggerMatchesMovement(ActorData mover, BoardSquarePathInfo curPath)
	{
		return !m_alreadyReacted
		       && mover == m_triggeringMover
		       && MovementUtils.ArePathSegmentsEquivalent_FromBeginning(m_triggeringPath, curPath);
	}

	public float GetTriggerDistance()
	{
		return m_triggeringPath?.moveCost ?? 0f;
	}

	public bool TriggersAtDistance(float distance)
	{
		return ServerClashUtils.AreMoveCostsEqual(GetTriggerDistance(), distance);
	}

	public bool StillHasUnexecutedHits()
	{
		return !m_alreadyReacted;
	}

	public bool AppliesStatusToMover(StatusType status)
	{
		Dictionary<ActorData, ActorHitResults> actorToHitResults = null;
		if (m_effectResults != null)
		{
			actorToHitResults = m_effectResults.m_actorToHitResults;
		}
		else if (m_barrierResults != null)
		{
			actorToHitResults = m_barrierResults.m_actorToHitResults;
		}
		else if (m_powerupResults != null)
		{
			actorToHitResults = m_powerupResults.m_actorToHitResults;
		}
		else if (m_gameModeResults != null)
		{
			actorToHitResults = m_gameModeResults.m_actorToHitResults;
		}
		return actorToHitResults != null && actorToHitResults[m_triggeringMover].AppliedStatus(status);
	}

	public string GetDebugDescription()
	{
		string text = "Movement Results from ";
		if (m_effectResults != null)
		{
			text += "Effect " + m_effectResults.Effect.GetDebugIdentifier();
		}
		else if (m_barrierResults != null)
		{
			if (m_barrierResults.Barrier.GetSourceAbility() != null)
			{
				text += "Barrier from Ability " + m_barrierResults.Barrier.GetSourceAbility().GetDebugIdentifier();
			}
			else
			{
				text += "Barrier without source ability";
			}
		}
		else if (m_powerupResults != null)
		{
			if (m_powerupResults.Ability != null)
			{
				text += "Powerup ability " + m_powerupResults.Ability.GetDebugIdentifier();
			}
			else
			{
				text += "Powerup";
			}
		}
		else if (m_gameModeResults != null)
		{
			if (m_gameModeResults.Ability != null)
			{
				text += "GameMode ability " + m_gameModeResults.Ability.GetDebugIdentifier();
			}
			else
			{
				text += "GameMode";
			}
		}
		return text;
	}
}
#endif

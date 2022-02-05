// ROGUES
// SERVER
using System.Collections.Generic;
//using Mirror;
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

	public MovementResults(MovementStage moveStage, Effect reactingEffect, ActorHitResults reactionHitResults, ActorData triggeringMover, BoardSquarePathInfo triggeringPathSegment, GameObject sequencePrefab, BoardSquare sequenceTargetSquare, SequenceSource parentSequenceSource, Sequence.IExtraSequenceParams[] extraParams = null)
	{
		this.m_forMovementStage = moveStage;
		this.SetupTriggerData(triggeringMover, triggeringPathSegment);
		this.SetupSequenceData(sequencePrefab, sequenceTargetSquare, parentSequenceSource, extraParams, true);
		this.SetupGameplayData(reactingEffect, reactionHitResults);
	}

	public MovementResults(MovementStage moveStage)
	{
		this.m_forMovementStage = moveStage;
	}

	public void SetupTriggerData(ServerAbilityUtils.TriggeringPathInfo triggerInfo)
	{
		this.m_triggeringMover = triggerInfo.m_mover;
		this.m_triggeringPath = triggerInfo.m_triggeringPathSegment;
		if (this.m_triggeringPath != null)
		{
			this.m_triggeringPath.CheckIsValidTriggeringPath(this.m_triggeringMover);
		}
	}

	public void SetupTriggerData(ActorData triggeringMover, BoardSquarePathInfo triggeringPath)
	{
		this.m_triggeringMover = triggeringMover;
		this.m_triggeringPath = triggeringPath;
		if (this.m_triggeringPath != null)
		{
			this.m_triggeringPath.CheckIsValidTriggeringPath(this.m_triggeringMover);
			this.m_triggeringPath.m_moverHasGameplayHitHere = true;
		}
	}

	public void SetupSequenceData(GameObject sequencePrefab, BoardSquare sequenceTargetSquare, SequenceSource parentSequenceSource, Sequence.IExtraSequenceParams[] extraParams = null, bool removeAtEndOfTurn = true)
	{
		this.m_sequencePrefabs = new List<GameObject>();
		this.m_sequencePrefabs.Add(sequencePrefab);
		this.m_targetSquare = sequenceTargetSquare;
		this.m_sequenceSource = new SequenceSource(new SequenceSource.ActorDelegate(this.OnMovementReactionHitActor), new SequenceSource.Vector3Delegate(this.OnMovementReactionHitPosition), removeAtEndOfTurn, parentSequenceSource, null);
		this.m_extraParams = extraParams;
	}

	public void SetupSequenceDataList(List<GameObject> sequencePrefabList, BoardSquare sequenceTargetSquare, SequenceSource parentSequenceSource, Sequence.IExtraSequenceParams[] extraParams = null)
	{
		this.m_sequencePrefabs = new List<GameObject>();
		if (sequencePrefabList != null)
		{
			this.m_sequencePrefabs.AddRange(sequencePrefabList);
		}
		this.m_targetSquare = sequenceTargetSquare;
		this.m_sequenceSource = new SequenceSource(new SequenceSource.ActorDelegate(this.OnMovementReactionHitActor), new SequenceSource.Vector3Delegate(this.OnMovementReactionHitPosition), true, parentSequenceSource, null);
		this.m_extraParams = extraParams;
	}

	public void AddSequenceStartOverride(ServerClientUtils.SequenceStartData startData, SequenceSource parentSequenceSource, bool removeAtEndOfTurn = true)
	{
		if (startData != null)
		{
			if (this.m_sequenceStartDataOverrides == null)
			{
				this.m_sequenceStartDataOverrides = new List<ServerClientUtils.SequenceStartData>();
			}
			SequenceSource source = new SequenceSource(new SequenceSource.ActorDelegate(this.OnMovementReactionHitActor), new SequenceSource.Vector3Delegate(this.OnMovementReactionHitPosition), removeAtEndOfTurn, parentSequenceSource, null);
			startData.InitSequenceSourceData(source);
			this.m_sequenceStartDataOverrides.Add(startData);
		}
	}

	public EffectResults SetupGameplayData(Effect reactingEffect, ActorHitResults reactionHitResults)
	{
		this.m_effectResults = new EffectResults(reactingEffect, null, true, false);
		reactionHitResults.CanBeReactedTo = false;
		reactionHitResults.ForMovementStage = this.m_forMovementStage;
		if (reactionHitResults != null)
		{
			this.m_effectResults.StoreActorHit(reactionHitResults);
		}
		this.m_effectResults.GatheredResults = true;
		this.m_caster = reactingEffect.Caster;
		this.m_resultsName = reactingEffect.GetType().ToString() + " " + reactingEffect.m_effectName;
		this.m_gameplayResponseType = MovementResults_GameplayResponseType.Effect;
		return this.m_effectResults;
	}

	public BarrierResults SetupGameplayData(Barrier reactingBarrier, ActorHitResults reactionHitResults)
	{
		this.m_barrierResults = new BarrierResults(reactingBarrier);
		reactionHitResults.CanBeReactedTo = false;
		reactionHitResults.ForMovementStage = this.m_forMovementStage;
		if (reactionHitResults != null)
		{
			this.m_barrierResults.StoreActorHit(reactionHitResults);
		}
		this.m_barrierResults.GatheredResults = true;
		this.m_caster = ((reactingBarrier.Caster != null) ? reactingBarrier.Caster : reactionHitResults.m_hitParameters.Target);
		this.m_resultsName = "Barrier " + reactingBarrier.Name;
		this.m_gameplayResponseType = MovementResults_GameplayResponseType.Barrier;
		return this.m_barrierResults;
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
		this.m_powerupResults = new AbilityResults(actorData, ability, reactingPowerup.SequenceSource, true, false);
		reactionHitResults.CanBeReactedTo = false;
		reactionHitResults.ForMovementStage = this.m_forMovementStage;
		this.m_powerupResults.StoreActorHit(reactionHitResults);
		this.m_powerupResults.GatheredResults = true;
		this.m_caster = actorData;
		this.m_resultsName = reactingPowerup.m_powerUpName;
		this.m_gameplayResponseType = MovementResults_GameplayResponseType.Powerup;
	}

	public void SetupGameplayData(Ability gameModeAbility, ActorHitResults reactionHitResults)
	{
		ActorData actorData = reactionHitResults.m_hitParameters.Caster;
		if (actorData == null)
		{
			actorData = reactionHitResults.m_hitParameters.Target;
		}
		this.m_gameModeResults = new AbilityResults(actorData, gameModeAbility, null, true, false);
		reactionHitResults.CanBeReactedTo = false;
		reactionHitResults.ForMovementStage = this.m_forMovementStage;
		this.m_gameModeResults.StoreActorHit(reactionHitResults);
		this.m_gameModeResults.GatheredResults = true;
		this.m_caster = actorData;
		this.m_resultsName = gameModeAbility.m_abilityName;
		this.m_gameplayResponseType = MovementResults_GameplayResponseType.GameMode;
	}

	public void SetupGameplayDataForAbility(Ability ability, ActorHitResults hitRes, ActorData caster)
	{
		ActorData target = hitRes.m_hitParameters.Target;
		SequenceSource sequenceSource = new SequenceSource(null, null, true, null, null);
		this.m_powerupResults = new AbilityResults(target, ability, sequenceSource, true, false);
		hitRes.CanBeReactedTo = false;
		this.m_powerupResults.StoreActorHit(hitRes);
		this.m_powerupResults.GatheredResults = true;
		this.m_caster = caster;
		this.m_resultsName = ability.m_abilityName;
		this.m_gameplayResponseType = MovementResults_GameplayResponseType.Powerup;
	}

	public void SetupGameplayDataForAbility(Ability ability, ActorData caster)
	{
		SequenceSource sequenceSource = new SequenceSource(null, null, true, null, null);
		this.m_powerupResults = new AbilityResults(caster, ability, sequenceSource, true, false);
		this.m_powerupResults.GatheredResults = true;
		this.m_caster = caster;
		this.m_resultsName = ability.m_abilityName;
		this.m_gameplayResponseType = MovementResults_GameplayResponseType.Powerup;
	}

	public void AddActorHitResultsForReaction(ActorHitResults reactionHitResults)
	{
		if (reactionHitResults != null)
		{
			reactionHitResults.CanBeReactedTo = false;
			this.m_powerupResults.StoreActorHit(reactionHitResults);
		}
	}

	public void SetupForHitOutsideResolution(ActorData target, ActorData caster, ActorHitResults hitRes, Ability ability, GameObject seqPrefab, BoardSquare seqTargetSquare, SequenceSource seqSource, bool removeSequenceAtEndOfTurn, Sequence.IExtraSequenceParams[] extraParams = null)
	{
		this.SetupTriggerData(target, null);
		this.SetupGameplayDataForAbility(ability, hitRes, caster);
		this.SetupSequenceData(seqPrefab, seqTargetSquare, seqSource, extraParams, removeSequenceAtEndOfTurn);
	}

	public static void SetupAndExecuteAbilityResultsOutsideResolution(ActorData target, ActorData caster, ActorHitResults hitRes, Ability sourceAbility, bool removeSequenceAtEndOfTurn = true, GameObject inputSequencePrefab = null, Sequence.IExtraSequenceParams[] extraParams = null)
	{
		GameObject gameObject = inputSequencePrefab;
		if (gameObject == null)
		{
			gameObject = SequenceLookup.Get().GetSimpleHitSequencePrefab();
		}
		SequenceSource seqSource = new SequenceSource(null, null, removeSequenceAtEndOfTurn, null, null);
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
		if (this.m_effectResults != null)
		{
			return this.m_effectResults.DamageResults;
		}
		if (this.m_barrierResults != null)
		{
			return this.m_barrierResults.DamageResults;
		}
		if (this.m_powerupResults != null)
		{
			return this.m_powerupResults.DamageResults;
		}
		if (this.m_gameModeResults != null)
		{
			return this.m_gameModeResults.DamageResults;
		}
		return null;
	}

	public Dictionary<ActorData, int> GetMovementDamageResults_Gross()
	{
		if (this.m_effectResults != null)
		{
			return this.m_effectResults.DamageResults_Gross;
		}
		if (this.m_barrierResults != null)
		{
			return this.m_barrierResults.DamageResults_Gross;
		}
		if (this.m_powerupResults != null)
		{
			return this.m_powerupResults.DamageResults_Gross;
		}
		if (this.m_gameModeResults != null)
		{
			return this.m_gameModeResults.DamageResults_Gross;
		}
		return null;
	}

	public bool ShouldMovementHitUpdateTargetLastKnownPos(ActorData mover)
	{
		bool result;
		if (this.m_effectResults != null)
		{
			result = this.m_effectResults.ShouldMovementHitUpdateTargetLastKnownPos(mover);
		}
		else if (this.m_barrierResults != null)
		{
			result = this.m_barrierResults.ShouldMovementHitUpdateTargetLastKnownPos(mover);
		}
		else if (this.m_powerupResults != null)
		{
			result = this.m_powerupResults.ShouldMovementHitUpdateTargetLastKnownPos(mover);
		}
		else
		{
			result = (this.m_gameModeResults != null && this.m_gameModeResults.ShouldMovementHitUpdateTargetLastKnownPos(mover));
		}
		return result;
	}

	public virtual List<ServerClientUtils.SequenceStartData> GetTriggerSeqDataList()
	{
		List<ServerClientUtils.SequenceStartData> list = new List<ServerClientUtils.SequenceStartData>();
		ActorData[] targetActorArray;
		if (this.m_effectResults != null)
		{
			targetActorArray = this.m_effectResults.HitActorsArray();
		}
		else if (this.m_barrierResults != null)
		{
			targetActorArray = this.m_barrierResults.HitActorsArray();
		}
		else if (this.m_powerupResults != null)
		{
			targetActorArray = this.m_powerupResults.HitActorsArray();
		}
		else if (this.m_gameModeResults != null)
		{
			targetActorArray = this.m_gameModeResults.HitActorsArray();
		}
		else
		{
			targetActorArray = new ActorData[0];
		}
		if (this.m_sequenceStartDataOverrides != null && this.m_sequenceStartDataOverrides.Count > 0)
		{
			using (List<ServerClientUtils.SequenceStartData>.Enumerator enumerator = this.m_sequenceStartDataOverrides.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ServerClientUtils.SequenceStartData sequenceStartData = enumerator.Current;
					sequenceStartData.InitTargetActors(targetActorArray);
					list.Add(sequenceStartData);
				}
				return list;
			}
		}
		foreach (GameObject prefab in this.m_sequencePrefabs)
		{
			list.Add(new ServerClientUtils.SequenceStartData(prefab, this.m_targetSquare, targetActorArray, this.m_caster, this.m_sequenceSource, this.m_extraParams));
		}
		return list;
	}

	public static void ExecuteUnexecutedHits(List<MovementResults> moveResults, bool asFailsafe)
	{
		foreach (MovementResults movementResults in moveResults)
		{
			if (movementResults.m_alreadyReacted)
			{
				Debug.LogError(string.Concat(new string[]
				{
					"Executing unexecuted movement hits for ",
					movementResults.m_resultsName,
					", but it's already reacted.  (Failsafe = ",
					asFailsafe.ToString(),
					")"
				}));
			}
			movementResults.ExecuteUnexecutedMovementHits(asFailsafe);
		}
	}

	public void ExecuteUnexecutedMovementHits(bool asFailsafe)
	{
		Dictionary<ActorData, ActorHitResults> dictionary;
		Dictionary<Vector3, PositionHitResults> dictionary2;
		if (this.m_effectResults != null)
		{
			dictionary = this.m_effectResults.m_actorToHitResults;
			dictionary2 = this.m_effectResults.m_positionToHitResults;
		}
		else if (this.m_barrierResults != null)
		{
			dictionary = this.m_barrierResults.m_actorToHitResults;
			dictionary2 = this.m_barrierResults.m_positionToHitResults;
		}
		else if (this.m_powerupResults != null)
		{
			dictionary = this.m_powerupResults.m_actorToHitResults;
			dictionary2 = this.m_powerupResults.m_positionToHitResults;
		}
		else if (this.m_gameModeResults != null)
		{
			dictionary = this.m_gameModeResults.m_actorToHitResults;
			dictionary2 = this.m_gameModeResults.m_positionToHitResults;
		}
		else
		{
			dictionary = new Dictionary<ActorData, ActorHitResults>();
			dictionary2 = new Dictionary<Vector3, PositionHitResults>();
		}
		foreach (ActorHitResults actorHitResults in dictionary.Values)
		{
			if (!actorHitResults.ExecutedResults)
			{
				ActorData caster = actorHitResults.m_hitParameters.Caster;
				ActorData target = actorHitResults.m_hitParameters.Target;
				if (asFailsafe)
				{
					Debug.LogError(string.Concat(new string[]
					{
						caster.DebugNameString(),
						"'s ",
						this.m_resultsName,
						" force-executing movement hit on actor ",
						target.name,
						" ",
						target.DisplayName,
						" due to failsafe."
					}));
				}
				if (NetworkServer.active && actorHitResults.m_hitParameters.Target.IsDead())
				{
					Log.Error("Trying to execute movement hit on dead actor: {0}", new object[]
					{
						actorHitResults.m_hitParameters.Target.DebugNameString()
					});
				}
				actorHitResults.ExecuteResults();
				if (actorHitResults.HasReactions)
				{
					actorHitResults.ExecuteUnexecutedReactionHits(asFailsafe);
				}
				// TODO LOW KNOCKBACK
				// rogues?
				//if (actorHitResults.ForMovementStage == MovementStage.Knockback)
				//{
				//	List<ActorData> knockbackSourceActorsOnTarget = ServerKnockbackManager.Get().GetKnockbackSourceActorsOnTarget(target);
				//	if (knockbackSourceActorsOnTarget != null)
				//	{
				//		foreach (ActorData actorData in knockbackSourceActorsOnTarget)
				//		{
				//			actorData.OnKnockbackHitExecutedOnTarget(target, actorHitResults);
				//		}
				//	}
				//}
			}
		}
		foreach (PositionHitResults positionHitResults in dictionary2.Values)
		{
			if (!positionHitResults.ExecutedResults)
			{
				if (asFailsafe)
				{
					ActorData caster2 = positionHitResults.m_hitParameters.Caster;
					Debug.LogError(string.Concat(new string[]
					{
						caster2.DebugNameString(),
						"'s ",
						this.m_resultsName,
						" force-executing movement hit on position ",
						positionHitResults.m_hitParameters.Pos.ToString(),
						" due to failsafe."
					}));
				}
				positionHitResults.ExecuteResults();
			}
		}
		this.m_alreadyReacted = true;
	}

	public static void ExecuteUnexecutedHitsForDistance(List<MovementResults> moveResults, float distance, bool asFailsafe, out bool stillHasUnexecutedHits, out float nextUnexecutedHitsDistance)
	{
		stillHasUnexecutedHits = false;
		nextUnexecutedHitsDistance = float.MaxValue;
		foreach (MovementResults movementResults in moveResults)
		{
			if (movementResults.StillHasUnexecutedHits())
			{
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
	}

	public void OnMovementReactionHitActor(ActorData target)
	{
		bool flag = false;
		bool flag2 = false;
		if (this.m_effectResults != null)
		{
			flag2 = this.m_effectResults.ExecuteForActor(target);
			flag = this.m_effectResults.HitsDoneExecuting();
		}
		else if (this.m_barrierResults != null)
		{
			flag2 = this.m_barrierResults.ExecuteForActor(target);
			flag = this.m_barrierResults.HitsDoneExecuting();
		}
		else if (this.m_powerupResults != null)
		{
			flag2 = this.m_powerupResults.ExecuteForActor(target);
			flag = this.m_powerupResults.HitsDoneExecuting();
		}
		else if (this.m_gameModeResults != null)
		{
			flag2 = this.m_gameModeResults.ExecuteForActor(target);
			flag = this.m_gameModeResults.HitsDoneExecuting();
		}
		if (flag2 && flag && ServerClientUtils.GetCurrentAbilityPhase() == AbilityPriority.Combat_Knockback)
		{
			TheatricsManager.Get().OnKnockbackMovementHitExecuted(this.m_triggeringMover);
		}
	}

	public void OnMovementReactionHitPosition(Vector3 pos)
	{
		bool flag = false;
		bool flag2 = false;
		if (this.m_effectResults != null)
		{
			flag2 = this.m_effectResults.ExecuteForPosition(pos);
			flag = this.m_effectResults.HitsDoneExecuting();
		}
		else if (this.m_barrierResults != null)
		{
			flag2 = this.m_barrierResults.ExecuteForPosition(pos);
			flag = this.m_barrierResults.HitsDoneExecuting();
		}
		else if (this.m_powerupResults != null)
		{
			flag2 = this.m_powerupResults.ExecuteForPosition(pos);
			flag = this.m_powerupResults.HitsDoneExecuting();
		}
		else if (this.m_gameModeResults != null)
		{
			flag2 = this.m_gameModeResults.ExecuteForPosition(pos);
			flag = this.m_gameModeResults.HitsDoneExecuting();
		}
		if (flag2 && flag && ServerClientUtils.GetCurrentAbilityPhase() == AbilityPriority.Combat_Knockback)
		{
			TheatricsManager.Get().OnKnockbackMovementHitExecuted(this.m_triggeringMover);
		}
	}

	public ActorData GetTriggeringActor()
	{
		return this.m_triggeringMover;
	}

	public EffectResults GetEffectResults()
	{
		return this.m_effectResults;
	}

	public BarrierResults GetBarrierResults()
	{
		return this.m_barrierResults;
	}

	public AbilityResults GetPowerUpResults()
	{
		return this.m_powerupResults;
	}

	public AbilityResults GetGameModeResults()
	{
		return this.m_gameModeResults;
	}

	public bool TriggerMatchesMovement(ActorData mover, BoardSquarePathInfo curPath)
	{
		return !this.m_alreadyReacted && !(mover != this.m_triggeringMover) && MovementUtils.ArePathSegmentsEquivalent_FromBeginning(this.m_triggeringPath, curPath);
	}

	public float GetTriggerDistance()
	{
		if (this.m_triggeringPath == null)
		{
			return 0f;
		}
		return this.m_triggeringPath.moveCost;
	}

	public bool TriggersAtDistance(float distance)
	{
		return ServerClashUtils.AreMoveCostsEqual(this.GetTriggerDistance(), distance, false);
	}

	public bool StillHasUnexecutedHits()
	{
		return !this.m_alreadyReacted;
	}

	public bool AppliesStatusToMover(StatusType status)
	{
		Dictionary<ActorData, ActorHitResults> dictionary = null;
		if (this.m_effectResults != null)
		{
			dictionary = this.m_effectResults.m_actorToHitResults;
		}
		else if (this.m_barrierResults != null)
		{
			dictionary = this.m_barrierResults.m_actorToHitResults;
		}
		else if (this.m_powerupResults != null)
		{
			dictionary = this.m_powerupResults.m_actorToHitResults;
		}
		else if (this.m_gameModeResults != null)
		{
			dictionary = this.m_gameModeResults.m_actorToHitResults;
		}
		return dictionary != null && dictionary[this.m_triggeringMover].AppliedStatus(status);
	}

	public string GetDebugDescription()
	{
		string text = "Movement Results from ";
		if (this.m_effectResults != null)
		{
			text = text + "Effect " + this.m_effectResults.Effect.GetDebugIdentifier();
		}
		else if (this.m_barrierResults != null)
		{
			if (this.m_barrierResults.Barrier.GetSourceAbility() != null)
			{
				text = text + "Barrier from Ability " + this.m_barrierResults.Barrier.GetSourceAbility().GetDebugIdentifier("");
			}
			else
			{
				text += "Barrier without source ability";
			}
		}
		else if (this.m_powerupResults != null)
		{
			if (this.m_powerupResults.Ability != null)
			{
				text = text + "Powerup ability " + this.m_powerupResults.Ability.GetDebugIdentifier("");
			}
			else
			{
				text += "Powerup";
			}
		}
		else if (this.m_gameModeResults != null)
		{
			if (this.m_gameModeResults.Ability != null)
			{
				text = text + "GameMode ability " + this.m_gameModeResults.Ability.GetDebugIdentifier("");
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

// ROGUES
// SERVER
using System.Collections.Generic;
using Theatrics;
using UnityEngine.Networking;

// rogues-only, missing in reactor
#if SERVER
public class PlayerAction_Ability : PlayerAction
{
	private List<AbilityRequest> m_requests;
	private AbilityPriority m_phase;

	public PlayerAction_Ability(List<AbilityRequest> requests, AbilityPriority phase)
	{
		m_requests = new List<AbilityRequest>(requests);
		m_phase = phase;
	}

	public AbilityPriority GetRelevantPhase()
	{
		return m_phase;
	}

	public override bool ExecuteAction()
	{
		if (m_requests == null)
		{
			return false;
		}
		base.ExecuteAction();
		for (int i = m_requests.Count - 1; i >= 0; i--)
		{
			AbilityRequest abilityRequest = m_requests[i];
			if (abilityRequest.m_caster.IsDead())
			{
				abilityRequest.m_resolveState = AbilityRequest.AbilityResolveState.QUEUED;
				ServerActionBuffer.Get().CancelAbilityRequest(abilityRequest.m_caster, abilityRequest.m_ability, true, false);
				m_requests.RemoveAt(i);
			}
		}
		if (m_requests.Count == 0)
		{
			return false;
		}
		AbilityPriority phase = m_phase;
		foreach (AbilityRequest abilityRequest2 in m_requests)
		{
			if (abilityRequest2.m_caster != null)
			{
				abilityRequest2.m_caster.SetSquareAtPhaseStart(abilityRequest2.m_caster.GetCurrentBoardSquare());
			}
		}
		bool isEvasionPhase = phase == AbilityPriority.Evasion;
		bool isKnockbackPhase = phase == AbilityPriority.Combat_Knockback;
		if (isEvasionPhase)
		{
			SetupForEvadesPreGathering(m_requests);
		}
		List<ActorAnimation> list = new List<ActorAnimation>();
		sbyte b = 0;
		foreach (AbilityRequest abilityRequest3 in m_requests)
		{
			if (abilityRequest3.m_caster.GetPassiveData())
			{
				abilityRequest3.m_caster.GetPassiveData().PreGatherResultsForPlayerAction(abilityRequest3.m_ability);
			}
			if (abilityRequest3.m_caster != null && abilityRequest3.m_caster.GetAbilityData() != null)
			{
				abilityRequest3.m_caster.GetAbilityData().ReinitAbilityInteractionData(abilityRequest3.m_ability);
			}
			abilityRequest3.m_ability.GatherResults_Base(phase, abilityRequest3.m_targets, abilityRequest3.m_caster, abilityRequest3.m_additionalData);

			// rogues
			//if (abilityRequest3.m_additionalData.m_abilityResults.GatheredResults)
			//{
			//	foreach (ActorHitResults actorHitResults in abilityRequest3.m_additionalData.m_abilityResults.m_actorToHitResults.Values)
			//	{
			//		actorHitResults.ProcessEffectTemplates();
			//	}
			//}

			ActorAnimation actorAnimation = new ActorAnimation(null, null, abilityRequest3, abilityRequest3.m_additionalData.m_sequenceSource);
			actorAnimation.m_playOrderGroupIndex = 0;
			if (Turn.AnimsStartTogetherInPhase(phase))
			{
				actorAnimation.m_playOrderIndex = 0;
				actorAnimation.m_doCinematicCam = false;
			}
			else
			{
				actorAnimation.m_playOrderIndex = b;
				if (actorAnimation.IsCinematicRequested())
				{
					actorAnimation.m_doCinematicCam = true;
					actorAnimation.m_cinematicCamIndex = actorAnimation.CinematicIndex;
				}
			}
			if (Turn.AnimsStartTogetherInPhase(phase))
			{
				short animIndex = (short)abilityRequest3.m_ability.GetActionAnimType(abilityRequest3.m_targets, abilityRequest3.m_caster);
				if (abilityRequest3.m_cinematicRequested > 0 && CameraManager.Get().DoesAnimIndexTriggerTauntCamera(abilityRequest3.m_caster, (int)animIndex, abilityRequest3.m_cinematicRequested))
				{
					SequenceSource source = abilityRequest3.m_ability.UseAbilitySequenceSourceForEvadeOrKnockbackTaunt() ? abilityRequest3.m_additionalData.m_sequenceSource : new SequenceSource(null, null, true, null, null);
					ActorAnimation actorAnimation2 = new ActorAnimation(null, null, abilityRequest3.m_caster, abilityRequest3.m_actionType, animIndex, abilityRequest3.m_cinematicRequested, abilityRequest3.m_targets, source);
					actorAnimation2.m_doCinematicCam = true;
					actorAnimation2.m_cinematicCamIndex = actorAnimation2.CinematicIndex;
					actorAnimation2.m_playOrderIndex = b;
					b = (actorAnimation.m_playOrderIndex = (sbyte)(b + 1));
					list.Add(actorAnimation2);
				}
			}
			list.Add(actorAnimation);
			// rogues
			//abilityRequest3.m_caster.GetActorTurnSM().OnMessage(TurnMessage.EXECUTE_ACTION_START, true);
			b += 1;
		}
		//ServerActionBuffer.Get().SynchronizePositionsOfActorsParticipatingInPhase(phase);
		//if (!NetworkClient.active)
		//{
		//	PlayerAction_Ability.InitializeTheatricsForPhaseActions(phase, list);
		//}
		// if (isKnockbackPhase)
		// {
		// 	List<ActorData> actorsThatWillBeSeenButArentMoving;
		// 	ServerActionBuffer.Get().GetKnockbackManager().ProcessKnockbacks(m_requests, out actorsThatWillBeSeenButArentMoving);
		// 	ServerActionBuffer.Get().SynchronizePositionsOfActorsThatWillBeSeen(actorsThatWillBeSeenButArentMoving);
		// }
		//ServerResolutionManager.Get().SendActionsToClients_FCFS(m_requests, list, phase);
		//if (isEvasionPhase)
		//{
		//	SetupForEvadesPostGathering();
		//}
		//foreach (AbilityRequest abilityRequest4 in m_requests)
		//{
		//	ServerActionBuffer.Get().RunAbilityRequest_FCFS(abilityRequest4);
		//	// rogues
		//	//abilityRequest4.m_caster.GetActorTurnSM().MarkPveAbilityFlagAtIndex((int)abilityRequest4.m_actionType);
		//}
		return true;
	}

	//custom
	public List<ActorAnimation> PrepareResults()
	{
		if (m_requests == null)
		{
			return new List<ActorAnimation>();
		}
		base.ExecuteAction();
		for (int i = m_requests.Count - 1; i >= 0; i--)
		{
			AbilityRequest abilityRequest = m_requests[i];
			if (abilityRequest.m_caster.IsDead())
			{
				abilityRequest.m_resolveState = AbilityRequest.AbilityResolveState.QUEUED;
				ServerActionBuffer.Get().CancelAbilityRequest(abilityRequest.m_caster, abilityRequest.m_ability, true, false);
				m_requests.RemoveAt(i);
			}
		}
		if (m_requests.Count == 0)
		{
			return new List<ActorAnimation>();
		}
		AbilityPriority phase = m_phase;
		// foreach (AbilityRequest abilityRequest2 in m_requests)
		// {
		// 	if (abilityRequest2.m_caster != null)
		// 	{
		// 		abilityRequest2.m_caster.SetSquareAtPhaseStart(abilityRequest2.m_caster.GetCurrentBoardSquare());
		// 	}
		// }
		bool isEvasionPhase = phase == AbilityPriority.Evasion;
		bool isKnockbackPhase = phase == AbilityPriority.Combat_Knockback;
		if (isEvasionPhase)
		{
			SetupForEvadesPreGathering(m_requests);
		}
		List<ActorAnimation> list = new List<ActorAnimation>();
		sbyte b = 0;

		
		foreach (AbilityRequest abilityRequest3 in m_requests)
		{
			if (abilityRequest3.m_caster.GetPassiveData())
			{
				abilityRequest3.m_caster.GetPassiveData().PreGatherResultsForPlayerAction(abilityRequest3.m_ability);
			}
			if (abilityRequest3.m_caster != null && abilityRequest3.m_caster.GetAbilityData() != null)
			{
				abilityRequest3.m_caster.GetAbilityData().ReinitAbilityInteractionData(abilityRequest3.m_ability);
			}
			abilityRequest3.m_ability.GatherResults_Base(phase, abilityRequest3.m_targets, abilityRequest3.m_caster, abilityRequest3.m_additionalData);

			// rogues
			//if (abilityRequest3.m_additionalData.m_abilityResults.GatheredResults)
			//{
			//	foreach (ActorHitResults actorHitResults in abilityRequest3.m_additionalData.m_abilityResults.m_actorToHitResults.Values)
			//	{
			//		actorHitResults.ProcessEffectTemplates();
			//	}
			//}

			ActorAnimation actorAnimation = new ActorAnimation(null, null, abilityRequest3, abilityRequest3.m_additionalData.m_sequenceSource);
			actorAnimation.m_playOrderGroupIndex = 0;
			if (Turn.AnimsStartTogetherInPhase(phase))
			{
				actorAnimation.m_playOrderIndex = 0;
				actorAnimation.m_doCinematicCam = false;
			}
			else
			{
				actorAnimation.m_playOrderIndex = b;
				if (actorAnimation.IsCinematicRequested())
				{
					actorAnimation.m_doCinematicCam = true;
					actorAnimation.m_cinematicCamIndex = actorAnimation.CinematicIndex;
				}
			}
			if (Turn.AnimsStartTogetherInPhase(phase))
			{
				short animIndex = (short)abilityRequest3.m_ability.GetActionAnimType(abilityRequest3.m_targets, abilityRequest3.m_caster);
				if (abilityRequest3.m_cinematicRequested > 0 && CameraManager.Get().DoesAnimIndexTriggerTauntCamera(abilityRequest3.m_caster, (int)animIndex, abilityRequest3.m_cinematicRequested))
				{
					SequenceSource source = abilityRequest3.m_ability.UseAbilitySequenceSourceForEvadeOrKnockbackTaunt() ? abilityRequest3.m_additionalData.m_sequenceSource : new SequenceSource(null, null, true, null, null);
					ActorAnimation actorAnimation2 = new ActorAnimation(null, null, abilityRequest3.m_caster, abilityRequest3.m_actionType, animIndex, abilityRequest3.m_cinematicRequested, abilityRequest3.m_targets, source);
					actorAnimation2.m_doCinematicCam = true;
					actorAnimation2.m_cinematicCamIndex = actorAnimation2.CinematicIndex;
					actorAnimation2.m_playOrderIndex = b;
					b = (actorAnimation.m_playOrderIndex = (sbyte)(b + 1));
					list.Add(actorAnimation2);
				}
			}
			list.Add(actorAnimation);
			// rogues
			//abilityRequest3.m_caster.GetActorTurnSM().OnMessage(TurnMessage.EXECUTE_ACTION_START, true);
			b += 1;
		}
		//ServerActionBuffer.Get().SynchronizePositionsOfActorsParticipatingInPhase(phase);
		//if (!NetworkClient.active)
		//{
		//	PlayerAction_Ability.InitializeTheatricsForPhaseActions(phase, list);
		//}
		// if (isKnockbackPhase)
		// {
		// 	List<ActorData> actorsThatWillBeSeenButArentMoving;
		// 	ServerActionBuffer.Get().GetKnockbackManager().ProcessKnockbacks(m_requests, out actorsThatWillBeSeenButArentMoving);
		// 	ServerActionBuffer.Get().SynchronizePositionsOfActorsThatWillBeSeen(actorsThatWillBeSeenButArentMoving);
		// }
		//ServerResolutionManager.Get().SendActionsToClients_FCFS(m_requests, list, phase);
		//if (isEvasionPhase)
		//{
		//	SetupForEvadesPostGathering();
		//}
		// foreach (AbilityRequest abilityRequest4 in m_requests)
		// {
		// 	// reactor
		// 	ServerActionBuffer.Get().TryRunAbilityRequest(abilityRequest4);
		// 	// rogues
		// 	// ServerActionBuffer.Get().RunAbilityRequest_FCFS(abilityRequest4);
		// 	//abilityRequest4.m_caster.GetActorTurnSM().MarkPveAbilityFlagAtIndex((int)abilityRequest4.m_actionType);
		// }
		return list;
	}

	// custom
	public void RunAbilityRequests()
	{
		foreach (AbilityRequest request in m_requests)
		{
			ServerActionBuffer.Get().TryRunAbilityRequest(request);
		}
	}

	// public override void OnExecutionComplete(bool isLastAction)
	// {
	// 	if (m_requests != null)
	// 	{
	// 		foreach (AbilityRequest abilityRequest in m_requests)
	// 		{
	// 			// rogues, requests are reset on turn start in reactor
	// 			// abilityRequest.m_resolveState = AbilityRequest.AbilityResolveState.QUEUED;
	// 			// ServerActionBuffer.Get().CancelAbilityRequest(abilityRequest.m_caster, abilityRequest.m_ability, true, true);
	// 			// rogues
	// 			//if (isLastAction && !PlayerActionStateMachine.ConstructPlayerActionOnExecution())
	// 			//{
	// 			//	abilityRequest.m_caster.GetActorTurnSM().OnMessage(TurnMessage.EXECUTE_ACTION_DONE, true);
	// 			//}
	// 		}
	// 		
	// 		// rogues, not to be handled after every action in reactor
	// 		// if (m_phase == AbilityPriority.Combat_Knockback)
	// 		// {
	// 		// 	ServerActionBuffer.Get().GetKnockbackManager().ClearStoredData();
	// 		// }
	// 		// if (ServerCombatManager.Get().HasUnresolvedHealthEntries())
	// 		// {
	// 		// 	ServerCombatManager.Get().ResolveHitPoints();
	// 		// }
	// 		// if (ServerCombatManager.Get().HasUnresolvedTechPointsEntries())
	// 		// {
	// 		// 	ServerCombatManager.Get().ResolveTechPoints();
	// 		// }
	// 	}
	// }

	private void SetupForEvadesPreGathering(List<AbilityRequest> requests)
	{
		ServerEvadeManager evadeManager = ServerActionBuffer.Get().GetEvadeManager();
		evadeManager.ProcessEvades(requests, AbilityPriority.Evasion);
		foreach (ActorData actorData in GameFlowData.Get().GetActors())
		{
			if (actorData.GetPassiveData() != null)
			{
				actorData.GetPassiveData().OnEvadesProcessed();
			}
		}
		List<ActorData> actorsThatWillBeSeenButArentMoving;
		evadeManager.GatherGameplayResultsInResponseToEvades(out actorsThatWillBeSeenButArentMoving);
		ServerActionBuffer.Get().SynchronizePositionsOfActorsThatWillBeSeen(actorsThatWillBeSeenButArentMoving);
		evadeManager.SwapEvaderSquaresWithDestinations();
		if (evadeManager.HasEvades())
		{
			ServerActionBuffer.Get().ImmediateUpdateAllFogOfWar();
		}
	}

	private void SetupForEvadesPostGathering()
	{
		ServerEvadeManager evadeManager = ServerActionBuffer.Get().GetEvadeManager();
		ServerActionBuffer.Get().SynchronizePositionsOfActorsParticipatingInPhase(AbilityPriority.Evasion);
		evadeManager.UndoEvaderDestinationsSwap();
		if (evadeManager.HasEvades())
		{
			ServerActionBuffer.Get().ImmediateUpdateAllFogOfWar();
		}
		evadeManager.RunEvades();
	}

	public static void InitializeTheatricsForPhaseActions(AbilityPriority currentPhase, List<ActorAnimation> animEntries)
	{
		Turn turn = new Turn(GameFlowData.Get().CurrentTurn);
		while (turn.m_abilityPhases.Count <= (int)currentPhase)
		{
			Phase phase = new Phase(turn);
			phase.SetPhaseIndex_FCFS(currentPhase);
			turn.m_abilityPhases.Add(phase);
		}
		foreach (ActorAnimation actorAnimation in animEntries)
		{
			actorAnimation.SetTurn_FCFS(turn);
		}
		turn.m_abilityPhases[(int)currentPhase].m_actorAnimations = new List<ActorAnimation>(animEntries);
		TheatricsManager.Get().SetTurn_FCFS(turn);
		TheatricsManager.Get().InitPhaseClient_FCFS(currentPhase);
	}
}
#endif

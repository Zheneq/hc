// ROGUES
// SERVER
using System.Collections.Generic;
using System.Linq;
using Theatrics;
using UnityEngine;
using UnityEngine.Networking;

// rogues-only, missing in reactor
#if SERVER
public class PlayerAction_Effect : PlayerAction
{
	private List<EffectResults> m_requests;

	private AbilityPriority m_phase;

	public PlayerAction_Effect(List<global::Effect> requests, AbilityPriority phase)
	{
		this.m_requests = requests.SelectMany(delegate (global::Effect e)
		{
			// rogues
			//EffectSystem.Effect effect;
			//if ((effect = (e as EffectSystem.Effect)) != null)
			//{
			//    return effect.pendingEffectResults;
			//}
			return new List<EffectResults>
				{
					e.GetResultsForPhase(phase, true)
				};
		}).ToList<EffectResults>();
		this.m_phase = phase;
	}

	public AbilityPriority GetRelevantPhase()
	{
		return this.m_phase;
	}

	public override bool ExecuteAction()
	{
		if (this.m_requests != null)
		{
			base.ExecuteAction();
			AbilityPriority phase = this.m_phase;
			bool flag = phase == AbilityPriority.Evasion;
			bool flag2 = phase == AbilityPriority.Combat_Knockback;
			List<ActorAnimation> list = new List<ActorAnimation>();
			this.m_requests = (from effectResults in this.m_requests
							   where effectResults.m_actorToHitResults.Any<KeyValuePair<ActorData, ActorHitResults>>() || effectResults.m_positionToHitResults.Any<KeyValuePair<Vector3, PositionHitResults>>() || effectResults.m_sequenceStartData.Any<ServerClientUtils.SequenceStartData>()
							   select effectResults).ToList<EffectResults>();
			sbyte b = 0;
			foreach (EffectResults effectResults2 in this.m_requests)
			{
				if (effectResults2.Effect.AddActorAnimEntryIfHasHits(phase) || effectResults2.Effect.GetCasterAnimationIndex(phase) > 0)
				{
					ActorAnimation actorAnimation = new ActorAnimation(null, null, effectResults2);
					actorAnimation.m_playOrderGroupIndex = 0;
					if (flag || flag2)
					{
						actorAnimation.m_playOrderIndex = 0;
					}
					else
					{
						actorAnimation.m_playOrderIndex = b;
					}
					list.Add(actorAnimation);
				}
				b += 1;
			}
			ServerActionBuffer.Get().SynchronizePositionsOfActorsParticipatingInPhase(phase);
			if (!NetworkClient.active)
			{
				PlayerAction_Ability.InitializeTheatricsForPhaseActions(phase, list);
			}
			if (flag2)
			{
				List<ActorData> actorsThatWillBeSeenButArentMoving;
				ServerActionBuffer.Get().GetKnockbackManager().ProcessKnockbacks(new List<AbilityRequest>(), out actorsThatWillBeSeenButArentMoving);
				ServerActionBuffer.Get().SynchronizePositionsOfActorsThatWillBeSeen(actorsThatWillBeSeenButArentMoving);
			}
			ServerResolutionManager.Get().SendEffectActionsToClients_FCFS(this.m_requests, list, phase);
			return true;
		}
		return false;
	}

	public override void OnExecutionComplete(bool isLastAction)
	{
		foreach (EffectResults effectResults in this.m_requests)
		{
			// rogues
			//if (isLastAction && !PlayerActionStateMachine.ConstructPlayerActionOnExecution())
			//{
			//	effectResults.Caster.GetActorTurnSM().OnMessage(TurnMessage.EXECUTE_ACTION_DONE, true);
			//}
			effectResults.OnExecutionComplete();
		}
		if (this.m_phase == AbilityPriority.Combat_Knockback)
		{
			ServerActionBuffer.Get().GetKnockbackManager().ClearStoredData();
		}
		if (ServerCombatManager.Get().HasUnresolvedHealthEntries())
		{
			ServerCombatManager.Get().ResolveHitPoints();
		}
		if (ServerCombatManager.Get().HasUnresolvedTechPointsEntries())
		{
			ServerCombatManager.Get().ResolveTechPoints();
		}
	}
}
#endif

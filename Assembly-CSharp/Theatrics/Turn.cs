using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Theatrics
{
	public class Turn
	{
		internal List<Phase> m_abilityPhases = new List<Phase>((int)AbilityPriority.NumAbilityPriorities);
		private int m_phaseIndex = -1;
		internal Bounds m_lastSetBoundInTurn;
		internal int m_cameraBoundSetCount;
		internal bool m_cameraBoundSetForEvade;
		private HashSet<int> m_clientAbilitiesEndEventFiredPhases = new HashSet<int>();

		internal int TurnID { get; private set; }
		internal float TimeInPhase { get; private set; }
		internal float TimeInResolve { get; private set; }

		internal Turn()
		{
		}

		internal static bool IsEvasionOrKnockback(AbilityPriority priority)
		{
			return priority == AbilityPriority.Evasion || priority == AbilityPriority.Combat_Knockback;
		}
		
		public string Json()
		{
			string phases = "";
			if (!Phases.IsNullOrEmpty())
			{
				foreach (var e in Phases)
				{
					phases += (phases.Length == 0 ? "" : ",\n") + e.Json();
				}
			}
			return $"{{" +
				$"\"turnID\": {TurnID}," +
				$"\"phases\": [{phases}]" +
				$"}}";
		}
		
		internal void OnSerializeHelper(IBitStream stream)
		{
			int turnID = TurnID;
			stream.Serialize(ref turnID);
			TurnID = turnID;
			sbyte numPhases = (sbyte)m_abilityPhases.Count;
			stream.Serialize(ref numPhases);
			for (int num = 0; num < numPhases; num++)
			{
				while (num >= m_abilityPhases.Count)
				{
					m_abilityPhases.Add(new Phase(this));
				}
				m_abilityPhases[num].OnSerializeHelper(stream);
			}
		}

		internal void InitPhase(AbilityPriority phaseIndex)
		{
			if (phaseIndex == (AbilityPriority)m_phaseIndex)
			{
				return;
			}

			TimeInPhase = 0f;
			m_phaseIndex = (int)phaseIndex;
			if (m_phaseIndex >= 0 && m_phaseIndex < m_abilityPhases.Count)
			{
				TheatricsManager.Get().SetAnimatorParamOnAllActors("DecisionPhase", false);
				m_abilityPhases[m_phaseIndex].Init();
			}
			List<ActorData> actors = GameFlowData.Get().GetActors();
			if (actors != null)
			{
				for (int i = 0; i < actors.Count; i++)
				{
					ActorData actorData = actors[i];
					if (actorData != null
						&& actorData.GetHitPointsToDisplay() <= 0
						&& !actorData.IsInRagdoll())
					{
						if (GameplayData.Get().m_resolveDamageBetweenAbilityPhases || IsReadyToRagdoll(actorData))
						{
							actorData.DoVisualDeath(Sequence.CreateImpulseInfoWithActorForward(actorData));
						}
					}
				}
			}
			if (NetworkClient.active
				&& phaseIndex == AbilityPriority.Combat_Damage
				&& ClientResolutionManager.Get() != null
				&& !ClientResolutionManager.Get().IsWaitingForActionMessages(phaseIndex))
			{
				ClientResolutionManager.Get().OnCombatPhasePlayDataReceived();
			}
		}

		internal bool UpdatePhase(AbilityPriority phaseIndex)
		{
			TimeInResolve += GameTime.deltaTime;
			if (m_phaseIndex >= (int)AbilityPriority.NumAbilityPriorities)
			{
				return false;
			}
			bool hiddenAction = false;
			bool nonHiddenAction = false;
			if (m_phaseIndex < 0)
			{
				Log.Error("Phase index is negative! Code error.");
				return true;
			}
			bool flag3 = m_phaseIndex < m_abilityPhases.Count
				&& !m_abilityPhases[m_phaseIndex].Update(this, ref hiddenAction, ref nonHiddenAction);
			bool resolutionTimedOut = TimeInPhase >= GameFlowData.Get().m_resolveTimeoutLimit * 0.8f;
			if (resolutionTimedOut)
			{
				Log.Error("Theatrics: phase: " + ServerClientUtils.GetCurrentActionPhase().ToString() + " timed out for turn " + TurnID + ",  timeline index " + m_phaseIndex);
			}
			bool flag4 = !flag3 && !resolutionTimedOut;
			if (flag4)
			{
				if (hiddenAction && !nonHiddenAction)
				{
					if (GameFlowData.Get().activeOwnedActorData != null
						&& !GameFlowData.Get().activeOwnedActorData.IsDead())
					{
						InterfaceManager.Get().DisplayAlert(StringUtil.TR("HiddenAction", "Global"), Color.white);
					}
				}
				else
				{
					InterfaceManager.Get().CancelAlert(StringUtil.TR("HiddenAction", "Global"));
				}

				if (GameFlowData.Get() == null || !GameFlowData.Get().IsResolutionPaused())
				{
					TimeInPhase += GameTime.deltaTime;
				}
			}
			else
			{
				TheatricsManager.Get().DebugLog("Theatrics: finished timeline index " + m_phaseIndex + " with duration " + TimeInPhase + " @absolute time " + GameTime.time);
				if (TheatricsManager.DebugTraceExecution)
				{
					TheatricsManager.LogForDebugging("Phase Finished: " + m_phaseIndex);
				}
			}
			if (!flag4
				&& !HasAbilityPhaseAnimationAfter(phaseIndex)
				&& NetworkClient.active
				&& !m_clientAbilitiesEndEventFiredPhases.Contains(m_phaseIndex))
			{
				m_clientAbilitiesEndEventFiredPhases.Add(m_phaseIndex);
				GameEventManager.Get().FireEvent(GameEventManager.EventType.TheatricsAbilitiesEnd, null);
			}
			return flag4;
		}

		internal void OnAnimationEvent(ActorData animatedActor, Object eventObject, GameObject sourceObject)
		{
			if (m_phaseIndex < 0 || m_phaseIndex >= (int)AbilityPriority.NumAbilityPriorities)
			{
				return;
			}
			Phase phase = m_abilityPhases[m_phaseIndex];
			if (phase == null)
			{
				return;
			}
			for (int i = 0; i < phase.m_actorAnimations.Count; i++)
			{
				ActorAnimation actorAnimation = phase.m_actorAnimations[i];

				if (actorAnimation.Caster == animatedActor
					&& actorAnimation.Played
					&& actorAnimation.UpdateNotFinished())
				{
					actorAnimation.OnAnimationEvent(animatedActor, eventObject, sourceObject);
					if (actorAnimation.ParentAbilitySeqSource != null)
					{
						actorAnimation.OnAnimationEventFromChild(animatedActor, eventObject, sourceObject);
					}
					break;
				}
			}
		}

		internal void OnSequenceHit(
			Sequence seq,
			ActorData target,
			ActorModelData.ImpulseInfo impulseInfo,
			ActorModelData.RagdollActivation ragdollActivation = ActorModelData.RagdollActivation.HealthBased)
		{
			if (m_phaseIndex >= (int)AbilityPriority.NumAbilityPriorities)
			{
				return;
			}
			bool flag = false;
			bool flag2 = false;
			if (m_phaseIndex >= 0)
			{
				Phase phase = m_abilityPhases[m_phaseIndex];
				if (phase != null)
				{
					foreach (ActorAnimation actorAnimation in phase.m_actorAnimations)
					{
						if (actorAnimation.Caster == seq.Caster && actorAnimation.TriggeredSequence(seq))
						{
							if (actorAnimation.OnSequenceHit(seq, target, impulseInfo, ragdollActivation))
							{
								flag = true;
								break;
							}
						}
						else if (actorAnimation.Caster == target && !actorAnimation.UpdateNotFinished() && actorAnimation.Played)
						{
							flag2 = true;
						}
					}
				}
			}
			if (!flag && seq.RequestsHitAnimation(target))
			{
				ActorModelData actorModelData = target.GetActorModelData();
				if (actorModelData != null
					&& (!flag2 || actorModelData.CanPlayDamageReactAnim())
					&& IsReadyForDamageReaction(target))
				{
					target.PlayDamageReactionAnim(seq.m_customHitReactTriggerName);
				}
				if (ragdollActivation != ActorModelData.RagdollActivation.None
					&& IsReadyToRagdoll(target))
				{
					target.DoVisualDeath(impulseInfo);
				}
			}
		}

		internal Bounds CalcAbilitiesBounds(Phase phase, int playOrderIndex, out bool isDefault)
		{
			isDefault = true;
			bool flag = phase == null;
			ActorData actorData = GameFlowData.Get().activeOwnedActorData;
			if (actorData == null)
			{
				List<ActorData> actors = GameFlowData.Get().GetActors();
				if (actors == null || actors.Count == 0)
				{
					Log.Error("No actors found to create Abilities Bounds.");
					return default(Bounds);
				}
				actorData = actors[0];
			}
			Bounds cameraBounds = Board.Get().GetSquareFromVec3(actorData.transform.position).CameraBounds;
			Vector3 center = cameraBounds.center;
			center.y = 0f;
			Bounds result = cameraBounds;
			bool flag2 = true;
			for (int i = 0; i < m_abilityPhases.Count; i++)
			{
				Phase phase2 = m_abilityPhases[i];
				if (phase == null || phase == phase2)
				{
					for (int j = 0; j < phase2.m_actorAnimations.Count; j++)
					{
						ActorAnimation actorAnimation = phase2.m_actorAnimations[j];
						if ((playOrderIndex < 0 || actorAnimation.m_playOrderIndex == playOrderIndex)
							&& actorAnimation.UpdateNotFinished()
							&& !actorAnimation.NotInLoS()
							&& !actorAnimation.ShouldIgnoreCameraFraming())
						{
							Bounds bound = actorAnimation.m_bounds;
							if (phase.Index == AbilityPriority.Evasion && actorAnimation.Caster != null)
							{
								ActorTeamSensitiveData teamSensitiveData_authority = actorAnimation.Caster.TeamSensitiveData_authority;
								if (teamSensitiveData_authority != null)
								{
									teamSensitiveData_authority.EncapsulateVisiblePathBound(ref bound);
								}
							}
							if (flag2)
							{
								result = bound;
								flag2 = false;
							}
							else
							{
								result.Encapsulate(bound);
							}
							isDefault = false;
						}
					}
					if (flag && !flag2)
					{
						break;
					}
				}
			}
			return result;
		}

		internal bool HasAbilityPhaseAnimation()
		{
			return HasAbilityPhaseAnimationAfter(AbilityPriority.INVALID);
		}

		internal bool HasAbilityPhaseAnimationAfter(AbilityPriority phaseIndex)
		{
			for (int i = (int)(phaseIndex + 1); i < m_abilityPhases.Count; i++)
			{
				if (HasAbilityPhaseAnimation((AbilityPriority)i))
				{
					return true;
				}
			}
			return false;
		}

		internal bool HasAbilityPhaseAnimation(AbilityPriority phaseIndex)
		{
			return phaseIndex >= AbilityPriority.Prep_Defense
				&& (int)phaseIndex < m_abilityPhases.Count
				&& m_abilityPhases[(int)phaseIndex].m_actorAnimations.Count > 0;
		}

		internal bool HasUnfinishedActorAnimationInPhase(AbilityPriority phaseIndex)
		{
			return phaseIndex >= AbilityPriority.Prep_Defense
				&& phaseIndex < (AbilityPriority)m_abilityPhases.Count
				&& m_abilityPhases[(int)phaseIndex].HasUnfinishedActorAnimation();
		}

		private bool DeadAtEndOfCurrentTimelineIndex(ActorData actor)
		{
			return DeadAtEndOfTimelineIndex(actor, m_phaseIndex);
		}

		internal bool DeadAtEndOfTimelineIndex(ActorData actor, int phaseIndex)
		{
			if (actor.HitPoints <= 0)
			{
				return true;
			}
			if (phaseIndex >= (int)AbilityPriority.NumAbilityPriorities)
			{
				return actor.GetHitPointsToDisplay() <= 0;
			}
			int num = 0;
			for (int i = 0; i <= phaseIndex; i++)
			{
				Dictionary<int, int> hitActorIndexToDeltaHP = m_abilityPhases[i].HitActorIndexToDeltaHP;
				if (hitActorIndexToDeltaHP != null && hitActorIndexToDeltaHP.ContainsKey(actor.ActorIndex))
				{
					num += hitActorIndexToDeltaHP[actor.ActorIndex];
				}
			}
			return actor.HitPoints + actor.AbsorbPoints + num <= 0;
		}

		internal bool IsReadyForDamageReaction(ActorData actor)
		{
			if (actor.IsInRagdoll())
			{
				return false;
			}
			if (m_phaseIndex > 0 && m_phaseIndex < m_abilityPhases.Count)
			{
				List<ActorAnimation> animations = m_abilityPhases[m_phaseIndex].m_actorAnimations;
				foreach (ActorAnimation actorAnimation in animations)
				{
					if (actorAnimation.Caster == actor
						&& actorAnimation.Played
						&& !actorAnimation.AnimationFinished)
					{
						return false;
					}
				}
			}
			return true;
		}

		internal bool IsReadyToRagdoll(ActorData actor, int pendingDeltaHP = 0, int sequenceSourceIdToIgnore = -1)
		{
			if (actor.GetHitPointsToDisplay() + pendingDeltaHP > 0
				|| actor.IsInRagdoll()
				|| !DeadAtEndOfCurrentTimelineIndex(actor)
				|| m_phaseIndex < 3)
			{
				return false;
			}
			int num = m_phaseIndex;
			int phaseIndex = m_phaseIndex;
			while (true)
			{
				if (num >= 0 && num < m_abilityPhases.Count)
				{
					if (num == (int)AbilityPriority.Combat_Knockback
						&& m_abilityPhases[num].HasKnockbackMovementHitsRemaining(actor))
					{
						return false;
					}
					List<ActorAnimation> animations = m_abilityPhases[num].m_actorAnimations;
					for (int i = 0; i < animations.Count; i++)
					{
						ActorAnimation actorAnimation = animations[i];
						if ((sequenceSourceIdToIgnore < 0 || actorAnimation.SeqSource.RootID != sequenceSourceIdToIgnore)
							&& ((actorAnimation.Caster == actor && actorAnimation.UpdateNotFinished()) || actorAnimation.DeltaHPPending(actor)))
						{
							return false;
						}
					}
					if (num > phaseIndex && m_abilityPhases[num].HasHitOnActor(actor))
					{
						return false;
					}
				}
				num++;
				if (num >= (int)AbilityPriority.NumAbilityPriorities || GameplayData.Get().m_resolveDamageBetweenAbilityPhases)
				{
					return !(ClientResolutionManager.Get() != null)
						|| !ClientResolutionManager.Get().HasUnexecutedHitsOnActor(actor, sequenceSourceIdToIgnore);
				}
			}
		}

		public bool IsCinematicPlaying()
		{
			if (m_phaseIndex >= 0 && m_phaseIndex < m_abilityPhases.Count)
			{
				List<ActorAnimation> animations = m_abilityPhases[m_phaseIndex].m_actorAnimations;
				for (int i = 0; i < animations.Count; i++)
				{
					if (animations[i].m_doCinematicCam
						&& (animations[i].PlayState == ActorAnimation.PlaybackState.PlayRequested
							|| animations[i].PlayState == ActorAnimation.PlaybackState.PlayingAnimation
							|| animations[i].PlayState == ActorAnimation.PlaybackState.WaitingForTargetHits))
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}

// ROGUES
// SERVER
using System.Collections.Generic;
//using System.Diagnostics;
//using System.Runtime.CompilerServices;
using System.Linq;
//using Mirror;
using UnityEngine;
using UnityEngine.Networking;

namespace Theatrics
{
	internal class Turn
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

		// server-only
#if SERVER
		internal static bool AnimsStartTogetherInPhase(AbilityPriority phaseIndex)
		{
			return phaseIndex == AbilityPriority.Evasion || phaseIndex == AbilityPriority.Combat_Knockback;
		}
#endif

		internal Turn()
		{
		}

		// server-only
#if SERVER
		internal Turn(int turnID)
		{
			TurnID = turnID;
		}
#endif

		// removed in rogues?
		internal static bool IsEvasionOrKnockback(AbilityPriority priority)
		{
			return priority == AbilityPriority.Evasion || priority == AbilityPriority.Combat_Knockback;
		}


		// removed in rogues
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

		internal void InitPhase(AbilityPriority phaseIndex)  // , bool doServersideInit = true in rogues
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
				Phase phase = m_abilityPhases[m_phaseIndex];

				// server-only
#if SERVER
				if (NetworkServer.active)  //  && doServersideInit in rogues
				{
					List<EffectResults> effectsWithAnimations = ServerEffectManager.Get().FindEffectsWithCasterAnimations(phaseIndex);
					int m;
					int i;
					for (i = 0; i < effectsWithAnimations.Count; i = m)
					{
						if (!phase.m_actorAnimations.Exists((ActorAnimation x) => x.Effect == effectsWithAnimations[i].Effect))
						{
							phase.m_actorAnimations.Add(new ActorAnimation(this, phase, effectsWithAnimations[i]));
						}
						m = i + 1;
					}
					for (int j = m_phaseIndex; j < m_abilityPhases.Count; j++)
					{
						Phase phase2 = m_abilityPhases[j];
						for (int k = 0; k < phase2.m_actorAnimations.Count; k++)
						{
							phase2.m_actorAnimations[k].InitHitActorsToDeltaHP((AbilityPriority)j);
						}
						phase2.InitHitActorsToDeltaHP();
					}
					UnassignAnimationPlayOrder();
					AssignAnimationPlayOrderAndCinematicsForPhase(phaseIndex, true, true);
				}
#endif

				phase.Init();
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
							// reactor
							actorData.DoVisualDeath(Sequence.CreateImpulseInfoWithActorForward(actorData));
							// rogues
							//actorData.DoVisualDeath(null);
						}
					}
				}
			}

			// removed in rogues
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
			// TODO ROGUES
			// reactor
			bool resolutionTimedOut = TimeInPhase >= GameFlowData.Get().m_resolveTimeoutLimit * 0.8f;
			// rogues
			//bool resolutionTimedOut = GameFlowData.Get().m_resolveTimeoutLimit > 0
			//	&& TimeInPhase >= GameFlowData.Get().m_resolveTimeoutLimit * 0.8f;
			if (resolutionTimedOut)
			{
				// reactor
				Log.Error("Theatrics: phase: " + ServerClientUtils.GetCurrentActionPhase().ToString() + " timed out for turn " + TurnID + ",  timeline index " + m_phaseIndex);
				// rogues
				//Log.Error("Theatrics timed out for turn ", TurnID, ",  timeline index ", m_phaseIndex, ", after ", TimeInPhase));
				//if (num < m_abilityPhases.Count)
				//{
				//	Phase phase = m_abilityPhases[num];
				//	if (phase != null && phase.m_actorAnimations != null)
				//	{
				//		for (int i = 0; i < phase.m_actorAnimations.Count; i++)
				//		{
				//			ActorAnimation actorAnimation = phase.m_actorAnimations[i];
				//			if (actorAnimation != null && actorAnimation.PlayState != ActorAnimation.PlaybackState.ReleasedFocus && actorAnimation.PlayState != ActorAnimation.PlaybackState.CantBeStarted)
				//			{
				//				actorAnimation.PlayState = ActorAnimation.PlaybackState.ReleasedFocus;
				//			}
				//		}
				//	}
				//}
			}
			bool flag4 = !flag3 && !resolutionTimedOut;
			if (flag4)
			{
				if (hiddenAction && !nonHiddenAction)
				{
					if (GameFlowData.Get().activeOwnedActorData != null
						&& !GameFlowData.Get().activeOwnedActorData.IsDead())
					{
						InterfaceManager.Get().DisplayAlert(StringUtil.TR("HiddenAction", "Global"), Color.white);  //  Color.red in rogues
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
				// rogues
				// if (NetworkServer.active && NetworkClient.active)
				// {
				TheatricsManager.Get().DebugLog("Theatrics: finished timeline index " + m_phaseIndex + " with duration " + TimeInPhase + " @absolute time " + GameTime.time);
				if (TheatricsManager.DebugTraceExecution)
				{
					TheatricsManager.LogForDebugging("Phase Finished: " + m_phaseIndex);
				}
				// rogues
				// }
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
					// reactor
					&& (!flag2 || actorModelData.CanPlayDamageReactAnim())
					// rogues
					//&& (!flag2 || actorModelData.IsPlayingIdleAnim(false) || actorModelData.IsPlayingDamageAnim())
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

		// server-only
#if SERVER
		internal bool HasActionFromOwnedActor(Phase phase, int playOrderIndex)
		{
			for (int i = 0; i < m_abilityPhases.Count; i++)
			{
				Phase phase2 = m_abilityPhases[i];
				if (phase == null || phase == phase2)
				{
					for (int j = 0; j < phase2.m_actorAnimations.Count; j++)
					{
						ActorAnimation actorAnimation = phase2.m_actorAnimations[j];
						if ((playOrderIndex < 0 || actorAnimation.m_playOrderIndex == playOrderIndex)
							&& actorAnimation.Caster != null
							&& GameFlowData.Get().IsActorDataOwned(actorAnimation.Caster))
						{
							return true;
						}
					}
				}
			}
			return false;
		}
#endif

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

		private bool DeadAtEndOfCurrentTimelineIndex(ActorData actor)  // IsDeadAtm_phaseIndex
		{
			return DeadAtEndOfTimelineIndex(actor, m_phaseIndex);
		}

		internal bool DeadAtEndOfTimelineIndex(ActorData actor, int phaseIndex)  // IsDeadAtPhase
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
				|| !DeadAtEndOfCurrentTimelineIndex(actor)  // removed in rogues
				|| m_phaseIndex < 3)  // m_phaseIndex < 2 in rogues
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

		// server-only
#if SERVER
		internal void SetupAbilityPhase(AbilityPriority phasePriority, List<AbilityRequest> abilityRequests, HashSet<int> hitActorIds, bool hasHitsWithoutAnimEntry)
		{
			foreach (Phase oldPhase in m_abilityPhases)
			{
				if (oldPhase.Index == phasePriority)
				{
					Debug.LogError("Setting up ability phase " + phasePriority + ", but it's already been set up.");
					return;
				}
			}
			Phase phase = new Phase(this, phasePriority, abilityRequests);
			
			// TODO HACK
			// custom
			if (hitActorIds.IsNullOrEmpty())
			{
				hitActorIds = new HashSet<int>(phase.HitActorIndexToDeltaHP.Keys);
			}
			// end custom
			
			phase.SetHitActorIds(hitActorIds);
			m_abilityPhases.Add(phase);
			AssignAnimationPlayOrderAndCinematicsForPhase(phasePriority, false, false);
		}
#endif

		// server-only
#if SERVER
		private void AssignAnimationPlayOrderAndCinematicsForPhase(AbilityPriority phasePriority, bool mergeBounds, bool showDebugInfo)
		{
			Dictionary<int, ServerPlayOrderGroup> groupByOnlyEnemyTarget = new Dictionary<int, ServerPlayOrderGroup>(GameFlowData.Get().GetActors().Count);
			Dictionary<int, ServerPlayOrderGroup> groupByCaster = new Dictionary<int, ServerPlayOrderGroup>();
			List<ServerPlayOrderGroup> allGroups = new List<ServerPlayOrderGroup>(GameFlowData.Get().GetActors().Count);
			Dictionary<Team, ServerPlayOrderGroup> groupsByBotTeam = new Dictionary<Team, ServerPlayOrderGroup>(2);
			int num = (int)phasePriority;
			List<ActorAnimation> actorAnimations = m_abilityPhases[num].m_actorAnimations;
			groupByOnlyEnemyTarget.Clear();
			allGroups.Clear();
			groupsByBotTeam.Clear();
			int count = actorAnimations.Count;
			if (Turn.AnimsStartTogetherInPhase(phasePriority))
			{
				sbyte b = 0;
				for (int i = 0; i < actorAnimations.Count; i++)
				{
					ActorAnimation actorAnimation = actorAnimations[i];
					if (actorAnimation.IsTauntForEvadeOrKnockback())
					{
						actorAnimation.m_doCinematicCam = true;
						actorAnimation.m_playOrderIndex = b;
						b += 1;
					}
				}
				for (int j = 0; j < actorAnimations.Count; j++)
				{
					ActorAnimation actorAnimation2 = actorAnimations[j];
					if (actorAnimation2.m_playOrderIndex == -1)
					{
						actorAnimation2.m_playOrderIndex = b;
					}
				}
			}
			else
			{
				foreach (ActorAnimation actorAnimation in actorAnimations)
				{
					if (actorAnimation.IsCinematicRequested())
					{
						actorAnimation.m_doCinematicCam = true;
						actorAnimation.m_cinematicCamIndex = actorAnimation.CinematicIndex;
					}
				}
				IEnumerable<int> actorsWithFreeActions = from a in actorAnimations
					where a.HasFreeActionAbility()
					select a.Caster.ActorIndex;
				foreach (ActorAnimation actorAnimation in actorAnimations)
				{
					if (actorAnimation.m_playOrderIndex != -1)
					{
						continue;
					}
					actorAnimation.m_playOrderIndex = -2;
					ServerPlayOrderGroup serverPlayOrderGroup = null;
					ActorData caster = actorAnimation.Caster;
					int onlyEnemyTargetActorIndex = actorAnimation.GetOnlyEnemyTargetActorIndex();
					bool forceGroupByCaster = actorsWithFreeActions.Contains(caster.ActorIndex)
					                          || actorAnimation.GetTheatricsSortPriority() > 0;
					if (forceGroupByCaster && groupByCaster.ContainsKey(caster.ActorIndex))
					{
						serverPlayOrderGroup = groupByCaster[caster.ActorIndex];
					}
					if (!forceGroupByCaster && onlyEnemyTargetActorIndex != ActorData.s_invalidActorIndex)
					{
						groupByOnlyEnemyTarget.TryGetValue(onlyEnemyTargetActorIndex, out serverPlayOrderGroup);
					}
					if (!GameplayUtils.IsPlayerControlled(caster))
					{
						if (!groupsByBotTeam.ContainsKey(caster.GetTeam()))
						{
							groupsByBotTeam[caster.GetTeam()] = new ServerPlayOrderGroup(this, num);
							allGroups.Add(groupsByBotTeam[caster.GetTeam()]);
						}
						groupsByBotTeam[caster.GetTeam()].Add(actorAnimation);
					}
					else
					{
						if (serverPlayOrderGroup == null)
						{
							serverPlayOrderGroup = new ServerPlayOrderGroup(this, num);
							if (forceGroupByCaster)
							{
								groupByCaster[caster.ActorIndex] = serverPlayOrderGroup;
							}
							allGroups.Add(serverPlayOrderGroup);
						}
						if (!forceGroupByCaster && onlyEnemyTargetActorIndex != ActorData.s_invalidActorIndex)
						{
							groupByOnlyEnemyTarget[onlyEnemyTargetActorIndex] = serverPlayOrderGroup;
						}
						serverPlayOrderGroup.Add(actorAnimation);
					}
				}
				foreach (ServerPlayOrderGroup playOrderGroup in allGroups)
				{
					playOrderGroup.InitSortData();
				}
				allGroups.Sort();
				sbyte startIndex = 0;
				foreach (ServerPlayOrderGroup playOrderGroup in allGroups)
				{
					startIndex = playOrderGroup.AssignPlayOrderIndexes(startIndex);
				}
				if (mergeBounds && CameraManager.Get() != null && CameraManager.Get().GetAbilitiesCamera() != null)
				{
					float boundMergeSideDistThreshold = CameraManager.Get().GetAbilitiesCamera().m_boundMergeSideDistThreshold;
					List<ActorAnimation> list2 = new List<ActorAnimation>();
					Bounds bounds = default(Bounds);
					Bounds bounds2 = default(Bounds);
					int num2 = 0;
					int num3 = 0;
					foreach (ServerPlayOrderGroup playOrderGroup in allGroups)
					{
						List<ActorAnimation> actorAnimationsInGroup = playOrderGroup.GetActorAnimationsInGroup();
						for (int num5 = 0; num5 < actorAnimationsInGroup.Count; num5++)
						{
							ActorAnimation actorAnimation5 = actorAnimationsInGroup[num5];
							list2.Add(actorAnimation5);
							if (num2 == 0 || num3 == num2)
							{
								bounds = actorAnimation5.m_bounds;
								bounds2 = actorAnimation5.m_bounds;
							}
							else
							{
								Vector3 vector = actorAnimation5.m_bounds.center - bounds.center;
								vector.y = 0f;
								float magnitude = vector.magnitude;
								Vector3 vector2;
								Vector3 vector3;
								bool flag2 = CameraManager.BoundSidesWithinDistance(actorAnimation5.m_bounds, bounds, boundMergeSideDistThreshold, out vector2, out vector3);
								if (CameraManager.CamDebugTraceOn)
								{
									CameraManager.LogForDebugging(string.Concat(new object[]
									{
										"Index [",
										num2,
										"] can merge: ",
										flag2.ToString(),
										" | ",
										actorAnimation5.DebugShortName("white"),
										" | centerDist = ",
										magnitude,
										" | minBoundsDiff: ",
										vector3,
										" | maxBoundsDiff: ",
										vector2,
										"\nCurrent Bound: ",
										actorAnimation5.m_bounds,
										"\nCompare to Bound: ",
										bounds
									}), CameraManager.CameraLogType.MergeBounds);
								}
								if (flag2)
								{
									bounds2.Encapsulate(actorAnimation5.m_bounds);
								}
								bool flag3 = num2 == count - 1;
								if (!flag2 || flag3)
								{
									int num6 = (flag3 && flag2) ? num2 : (num2 - 1);
									if (num6 - num3 > 0)
									{
										for (int num7 = num3; num7 <= num6; num7++)
										{
											if (CameraManager.CamDebugTraceOn)
											{
												CameraManager.LogForDebugging(string.Concat(new object[]
												{
													"Merging Bound for entry index [",
													num7,
													"] | ",
													list2[num7].DebugShortName("white"),
													"\nFROM [",
													list2[num7].m_bounds,
													"] TO [",
													bounds2,
													"]"
												}), CameraManager.CameraLogType.MergeBounds);
											}
											list2[num7].m_bounds = bounds2;
										}
									}
									if (!flag3)
									{
										bounds = actorAnimation5.m_bounds;
										bounds2 = actorAnimation5.m_bounds;
										num3 = num2;
									}
								}
							}
							num2++;
						}
					}
				}
				ActorDebugUtils actorDebugUtils = ActorDebugUtils.Get();
				if (showDebugInfo && actorDebugUtils != null && actorDebugUtils.ShowingCategory(ActorDebugUtils.DebugCategory.TheatricsOrder, false))
				{
					ActorDebugUtils.DebugCategoryInfo debugCategoryInfo = actorDebugUtils.GetDebugCategoryInfo(ActorDebugUtils.DebugCategory.TheatricsOrder);
					string text = "Phase: " + phasePriority.ToString() + "\n";
					int num8 = 0;
					Vector3 vector4 = Vector3.zero;
					float y = (float)Board.Get().BaselineHeight;
					foreach (ServerPlayOrderGroup playOrderGroup in allGroups)
					{
						List<ActorAnimation> actorAnimationsInGroup2 = playOrderGroup.GetActorAnimationsInGroup();
						for (int num10 = 0; num10 < actorAnimationsInGroup2.Count; num10++)
						{
							ActorAnimation actorAnimation6 = actorAnimationsInGroup2[num10];
							text = text + actorAnimation6.ToString() + "\n";
							if (playOrderGroup.CasterDied() || playOrderGroup.TargetDied())
							{
								text = string.Concat(new string[]
								{
									text,
									"\tCasterDied=",
									playOrderGroup.CasterDied().ToString(),
									" | TargetDied=",
									playOrderGroup.TargetDied().ToString(),
									"\n"
								});
							}
							Vector3 center = actorAnimation6.m_bounds.center;
							center.y = y;
							Debug.DrawRay(center, 2.5f * Vector3.up, (num8 == 0) ? Color.cyan : Color.white, 25f);
							float num11 = (float)num8 * 0.2f;
							float num12 = (float)(num8 + 1) * 0.2f;
							if (num8 > 0)
							{
								float num13 = Mathf.Clamp(1f - (float)(num8 - 1) * 0.2f, 0f, 1f);
								Color color = num13 * Color.red + (1f - num13) * Color.yellow;
								Debug.DrawLine(vector4 + num11 * Vector3.up, center + num12 * Vector3.up, color, 25f);
							}
							vector4 = center;
							num8++;
						}
					}
					if (num8 > 0)
					{
						debugCategoryInfo.m_stringToDisplay = text;
						Debug.LogWarning(text);
					}
				}
			}
			m_abilityPhases[num].PostProcessSortedActorAnimations();
		}
#endif

		// server-only
#if SERVER
		private void UnassignAnimationPlayOrder()
		{
			for (int i = m_phaseIndex; i < m_abilityPhases.Count; i++)
			{
				Phase phase = m_abilityPhases[i];
				for (int j = 0; j < phase.m_actorAnimations.Count; j++)
				{
					phase.m_actorAnimations[j].m_playOrderIndex = -1;
				}
			}
		}
#endif
	}
}

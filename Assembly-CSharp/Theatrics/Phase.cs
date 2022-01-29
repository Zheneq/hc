using CameraManagerInternal;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Networking;

namespace Theatrics
{
	internal class Phase
	{
		internal List<ActorAnimation> m_actorAnimations = new List<ActorAnimation>();
		private Dictionary<int, int> m_hitActorIndexToDeltaHP = new Dictionary<int, int>();
		private Dictionary<int, int> m_actorIndexToKnockbackHitsRemaining = new Dictionary<int, int>(); // only matters if it is zero or positive
		private List<int> m_hitActorIds = new List<int>();

		private int m_playOrderIndex = -1;
		private int m_playOrderGroupIndex = -1;
		private bool m_playOrderGroupChanged = true;
		private int m_maxPlayOrderIndex = -1;
		private int m_firstNonCinematicPlayOrderIndex = -1;
		private float m_maxCamStartDelay;
		private int m_cameraTargetPlayOrderIndex = -1;
		private float m_cameraTargetPlayOrderIndexTime;
		private float m_firstNonCinematicEvadeAbilityPlayedTime;

		private Turn m_turn;

		private float m_timeSinceActorAnimationPlayed;
		private float m_timeSinceUpdateStart;
		private bool m_inCinematicCamLastUpdate;
		private bool m_firedMoveStartEvent;
		private float m_evasionMoveStartDesiredTime = -1f;

		private bool m_turnActionsDone;
		private bool m_displayedHungErrorForCurrentActorAnim;
		private bool m_loggedWarningForInKnockdownAnim;
		private bool m_cameraBoundsSameAsLast;
		private bool m_cinematicCamera;
		private bool _001B;
		private int _001E = -1;
		private bool m_highlightingActionEntriesNow;

		private const float c_firstEvadeWaitTimeInTurn = 0.7f;
		private const float c_firstNonEvadeWaitTimeInTurn = 0.3f;
		private const float c_animlessPlayTimeBeforeEaseInEnd = 0.35f;

		internal AbilityPriority Index { get; private set; }

		internal Dictionary<int, int> HitActorIndexToDeltaHP
		{
			get
			{
				return m_hitActorIndexToDeltaHP;
			}
			private set
			{
				m_hitActorIndexToDeltaHP = value;
			}
		}

		internal Phase(Turn turn)
		{
			m_turn = turn;
		}

		public void DoNotSendAnimations()
		{
			m_turnActionsDone = true;
		}

		internal void Init()
		{
			foreach (ActorAnimation actorAnimation in m_actorAnimations)
			{
				if (!actorAnimation.IsTauntForEvadeOrKnockback())
				{
					float camStartEventDelay = actorAnimation.GetCamStartEventDelay(Index == AbilityPriority.Evasion && actorAnimation.IsCinematicRequested());
					m_maxCamStartDelay = Mathf.Max(m_maxCamStartDelay, camStartEventDelay);
				}
				if (m_firstNonCinematicPlayOrderIndex == -1 && !actorAnimation.m_doCinematicCam)
				{
					m_firstNonCinematicPlayOrderIndex = actorAnimation.m_playOrderIndex;
				}
			}
		}

		internal bool ClientNeedToWaitBeforeKnockbackMove(ActorData actor)
		{
			if (m_actorAnimations != null)
			{
				for (int i = 0; i < m_actorAnimations.Count; i++)
				{
					ActorAnimation actorAnimation = m_actorAnimations[i];
					if (actorAnimation.Caster == actor
						&& !actorAnimation.IsTauntForEvadeOrKnockback()
						&& (!ClientResolutionManager.Get().HitsDoneExecuting(actorAnimation.SeqSource)
							|| actorAnimation.PlayState < ActorAnimation.PlaybackState.WaitingForTargetHits))
					{
						return true;
					}
				}
			}
			return false;
		}

		internal bool HasKnockbackMovementHitsRemaining(ActorData actor)
		{
			return m_actorIndexToKnockbackHitsRemaining != null
				&& m_actorIndexToKnockbackHitsRemaining.ContainsKey(actor.ActorIndex)
				&& m_actorIndexToKnockbackHitsRemaining[actor.ActorIndex] > 0;
		}

		internal bool HasHitOnActor(ActorData actor)
		{
			return actor != null
				&& m_hitActorIds != null
				&& m_hitActorIds.Contains(actor.ActorIndex);
		}

		internal bool HasUnfinishedActorAnimation()
		{
			for (int i = 0; i < m_actorAnimations.Count; i++)
			{
				ActorAnimation actorAnimation = m_actorAnimations[i];
				if (actorAnimation.PlayState != ActorAnimation.PlaybackState.CantBeStarted
					&& actorAnimation.PlayState != ActorAnimation.PlaybackState.ReleasedFocus)
				{
					return true;
				}
			}
			return false;
		}

		internal void OnSerializeHelper(IBitStream stream)
		{
			sbyte phaseIndex = (sbyte)Index;
			stream.Serialize(ref phaseIndex);
			Index = (AbilityPriority)phaseIndex;
			sbyte numAnimations = (sbyte)m_actorAnimations.Count;
			bool dropAnimations = m_turnActionsDone || phaseIndex < (sbyte)ServerClientUtils.GetCurrentAbilityPhase();
			if (stream.isWriting && dropAnimations)
			{
				numAnimations = 0;
			}
			stream.Serialize(ref numAnimations);
			for (int i = 0; i < numAnimations; i++)
			{
				while (i >= m_actorAnimations.Count)
				{
					m_actorAnimations.Add(new ActorAnimation(m_turn));
				}
				m_actorAnimations[i].OnSerializeHelper(stream);
			}
			if (stream.isWriting)
			{
				sbyte numActorIndexToDeltaHP = (sbyte)m_hitActorIndexToDeltaHP.Count;
				stream.Serialize(ref numActorIndexToDeltaHP);
				foreach (var actorIndexAndDeltaHP in m_hitActorIndexToDeltaHP)
				{
					sbyte actorIndex = (sbyte)actorIndexAndDeltaHP.Key;
					short deltaHP = (short)actorIndexAndDeltaHP.Value;
					stream.Serialize(ref actorIndex);
					stream.Serialize(ref deltaHP);
				}
			}
			else
			{
				sbyte numActorIndexToDeltaHP = -1;
				stream.Serialize(ref numActorIndexToDeltaHP);
				m_hitActorIndexToDeltaHP = new Dictionary<int, int>();
				for (int j = 0; j < numActorIndexToDeltaHP; j++)
				{
					sbyte actorIndex = (sbyte)ActorData.s_invalidActorIndex;
					short deltaHP = -1;
					stream.Serialize(ref actorIndex);
					stream.Serialize(ref deltaHP);
					m_hitActorIndexToDeltaHP.Add(actorIndex, deltaHP);
				}
			}
			if (phaseIndex == (sbyte)AbilityPriority.Combat_Knockback)
			{
				if (stream.isWriting)
				{
					sbyte value9 = (sbyte)m_actorIndexToKnockbackHitsRemaining.Count;
					if (dropAnimations)
					{
						value9 = 0;
						stream.Serialize(ref value9);
					}
					else
					{
						stream.Serialize(ref value9);
						foreach (KeyValuePair<int, int> item in m_actorIndexToKnockbackHitsRemaining)
						{
							sbyte value10 = (sbyte)item.Key;
							sbyte value11 = (sbyte)item.Value;
							stream.Serialize(ref value10);
							stream.Serialize(ref value11);
						}
					}
				}
				else
				{
					sbyte value12 = -1;
					stream.Serialize(ref value12);
					m_actorIndexToKnockbackHitsRemaining = new Dictionary<int, int>();
					for (int k = 0; k < value12; k++)
					{
						sbyte value13 = (sbyte)ActorData.s_invalidActorIndex;
						sbyte value14 = -1;
						stream.Serialize(ref value13);
						stream.Serialize(ref value14);
						m_actorIndexToKnockbackHitsRemaining.Add(value13, value14);
					}
				}
			}
			if (stream.isWriting)
			{
				sbyte numParticipants = (sbyte)m_hitActorIds.Count;
				stream.Serialize(ref numParticipants);
				for (sbyte b = 0; b < numParticipants; b = (sbyte)(b + 1))
				{
					sbyte participant = (sbyte)m_hitActorIds[b];
					stream.Serialize(ref participant);
				}
			}
			else
			{
				sbyte numParticipants = -1;
				stream.Serialize(ref numParticipants);
				m_hitActorIds = new List<int>();
				for (sbyte b = 0; b < numParticipants; b = (sbyte)(b + 1))
				{
					sbyte participant = -1;
					stream.Serialize(ref participant);
					m_hitActorIds.Add(participant);
				}
			}
		}

		private void StartAbilityHighlightForAnimEntries(List<ActorAnimation> animations)
		{
			if (animations == null || animations.Count <= 0)
			{
				return;
			}
			GameEventManager.TheatricsAbilityHighlightStartArgs theatricsAbilityHighlightStartArgs = new GameEventManager.TheatricsAbilityHighlightStartArgs();
			for (int i = 0; i < animations.Count; i++)
			{
				ActorAnimation actorAnimation = animations[i];
				theatricsAbilityHighlightStartArgs.m_casters.Add(actorAnimation.Caster);
				if (actorAnimation.HitActorsToDeltaHP != null)
				{
					for (int j = 0; j < actorAnimation.HitActorsToDeltaHP.Keys.Count; j++)
					{
						theatricsAbilityHighlightStartArgs.m_targets.Add(actorAnimation.HitActorsToDeltaHP.Keys.ElementAt(j));
					}
				}
			}
			List<ActorData> actorsWithMovementHits = ClientResolutionManager.Get().GetActorsWithMovementHits();
			for (int k = 0; k < actorsWithMovementHits.Count; k++)
			{
				if (!theatricsAbilityHighlightStartArgs.m_targets.Contains(actorsWithMovementHits[k]))
				{
					theatricsAbilityHighlightStartArgs.m_targets.Add(actorsWithMovementHits[k]);
				}
			}
			GameEventManager.Get().FireEvent(GameEventManager.EventType.TheatricsAbilityHighlightStart, theatricsAbilityHighlightStartArgs);
			m_highlightingActionEntriesNow = true;
		}

		private void ClearAbilityHighlightForAnimEntries()
		{
			GameEventManager.TheatricsAbilityHighlightStartArgs args = new GameEventManager.TheatricsAbilityHighlightStartArgs();
			GameEventManager.Get().FireEvent(GameEventManager.EventType.TheatricsAbilityHighlightStart, args);
			m_highlightingActionEntriesNow = false;
		}

		private ActorData GetActorAtPlayOrderIndex(int playOrderIndex, out bool cinematicCamera)
		{
			cinematicCamera = false;
			for (int i = 0; i < m_actorAnimations.Count; i++)
			{
				if (m_actorAnimations[i].m_playOrderIndex == playOrderIndex)
				{
					cinematicCamera = m_actorAnimations[i].m_doCinematicCam;
					return m_actorAnimations[i].Caster;
				}
			}
			return null;
		}

		private bool ActorAnimContainsKillOrSave(int playOrderIndex)
		{
			bool result = false;
			for (int num = 0; num < m_actorAnimations.Count; num++)
			{
				ActorAnimation actorAnimation = m_actorAnimations[num];
				if (actorAnimation != null && actorAnimation.HitActorsToDeltaHP != null && actorAnimation.m_playOrderIndex == playOrderIndex)
				{
					if (actorAnimation.IsCinematicRequested())
					{
						break;
					}
					foreach (var current in actorAnimation.HitActorsToDeltaHP)
					{
						ActorData hitActor = current.Key;
						int deltaHP = current.Value;
						int hitPointsAfterResolution = hitActor.GetHitPointsToDisplay();
						int hitPointsAfterResolutionWithDelta = hitActor.GetExpectedClientHpForDisplay(deltaHP);
						if (deltaHP < 0 && hitPointsAfterResolution > 0 && hitPointsAfterResolutionWithDelta <= 0)
						{
							result = true;
						}
						else if (deltaHP > 0 && hitPointsAfterResolution <= 0 && hitPointsAfterResolutionWithDelta > 0)
						{
							result = true;
						}
						else if (m_turn.IsReadyToRagdoll(hitActor, deltaHP, (int)actorAnimation.SeqSource.RootID))
						{
							result = true;
							if (CameraManager.CamDebugTraceOn)
							{
								CameraManager.LogForDebugging($"Ragdolling hit on {hitActor} when HP is already 0");
							}
						}
						if (result && CameraManager.CamDebugTraceOn)
						{
							CameraManager.LogForDebugging("Using Low Position for " + actorAnimation.ToString() +
								"\nhpDelta: " + deltaHP +
								" | hpForDisplay: " + hitPointsAfterResolution +
								" | expectedHpAfterHit: " + hitPointsAfterResolutionWithDelta);
						}
					}
					break;
				}
			}
			return result;
		}

		private bool CanUseSameBounds(Bounds prev, Bounds bounds, float maxCenterDistDiff, float maxSideDiff, bool canUseInflatedBounds)
		{
			float centerDist = VectorUtils.HorizontalPlaneDistInWorld(prev.center, bounds.center);
			if (centerDist > maxCenterDistDiff)
			{
				if (CameraManager.CamDebugTraceOn)
				{
					CameraManager.LogForDebugging("Not merging bounds, centerDist too far: " + centerDist, CameraManager.CameraLogType.SimilarBounds);
				}
				return false;
			}
			bool usedInflatedBounds = false;
			bool boundSidesWithinDistance = CameraManager.BoundSidesWithinDistance(prev, bounds, maxSideDiff, out Vector3 maxBoundDiff, out Vector3 minBoundDiff);
			if (canUseInflatedBounds && !boundSidesWithinDistance)
			{
				Bounds inflatedBounds = prev;
				inflatedBounds.Expand(new Vector3(1.5f, 100f, 1.5f));
				if (inflatedBounds.Contains(bounds.center + bounds.extents) && inflatedBounds.Contains(bounds.center - bounds.extents))
				{
					boundSidesWithinDistance = true;
					usedInflatedBounds = true;
				}
			}
			if (CameraManager.CamDebugTraceOn)
			{
				CameraManager.LogForDebugging(
					$"Considering bounds as similar, " +
					$"result = <color=yellow>{boundSidesWithinDistance}</color> " +
					$"| centerDist = {centerDist} " +
					$"| minBoundsDiff: {minBoundDiff} " +
					$"| maxBoundsDiff: {maxBoundDiff} " +
					$"| used inflated bounds: {usedInflatedBounds}" +
					$"\nPrev Bound: {prev}" +
					$"\nCompare to Bound: {bounds}",
					CameraManager.CameraLogType.SimilarBounds);
			}
			return boundSidesWithinDistance;
		}

		internal bool Update(Turn turn, ref bool hiddenAction, ref bool nonHiddenAction)
		{
			m_timeSinceActorAnimationPlayed += GameTime.deltaTime;
			m_timeSinceUpdateStart += GameTime.deltaTime;
			bool flag = m_actorAnimations.Count != 0
				&& Index == AbilityPriority.Evasion
				&& !m_firedMoveStartEvent;
			bool flag2 = true;
			foreach (ActorAnimation actorAnimation in m_actorAnimations)
			{
				if (actorAnimation.Update(turn))
				{
					flag = true;
					if (actorAnimation.m_playOrderIndex <= m_playOrderIndex)
					{
						flag2 = flag2 && actorAnimation.ShouldAutoCameraReleaseFocus();
					}
					if (actorAnimation.m_playOrderIndex == m_playOrderIndex)
					{
						bool isNotInLoS = actorAnimation.NotInLoS();
						hiddenAction = hiddenAction || isNotInLoS;
						nonHiddenAction = nonHiddenAction || !isNotInLoS;
					}
				}
			}
			if (CameraManager.Get().IsPlayingShotSequence())
			{
				m_inCinematicCamLastUpdate = true;
				return true;
			}
			if (m_inCinematicCamLastUpdate)
			{
				m_timeSinceActorAnimationPlayed = 0f;
				m_inCinematicCamLastUpdate = false;
			}
			if (!flag)
			{
				return false;
			}
			int minPlayOrderIndex = int.MaxValue;
			int minPlayOrderGroupIndex = int.MaxValue;
			for (int j = 0; j < m_actorAnimations.Count; j++)
			{
				ActorAnimation actorAnimation2 = m_actorAnimations[j];
				if (actorAnimation2 != null && actorAnimation2.PlayState == 0 && actorAnimation2.m_playOrderIndex < minPlayOrderIndex)
				{
					minPlayOrderIndex = actorAnimation2.m_playOrderIndex;
					minPlayOrderGroupIndex = actorAnimation2.m_playOrderGroupIndex;
				}
			}
			AbilitiesCamera abilitiesCamera = AbilitiesCamera.Get();
			float num7 = Index == AbilityPriority.Evasion ? 0.7f : 0.3f;
			bool flag4 = turn.TimeInResolve >= num7 || m_timeSinceActorAnimationPlayed >= num7;
			bool flag5 = flag4 && flag2 && minPlayOrderIndex != m_playOrderIndex && minPlayOrderIndex != int.MaxValue;
			bool isResolutionPaused = GameFlowData.Get() == null || GameFlowData.Get().IsResolutionPaused();
			flag5 = flag5 && !isResolutionPaused;
			if (!flag5 && m_timeSinceActorAnimationPlayed > 20f && !isResolutionPaused)
			{
				Log.Error("Stuck when trying to advance to next actor anim entry, \nplay order release focus: " + flag2.ToString() + "\npast waiting for first action: " + flag4 + "\nminNotStartedPLayOrderIndex: " + minPlayOrderIndex + "\nplayOrderIndex: " + m_playOrderIndex);
				flag5 = true;
			}
			if (flag5)
			{
				if (m_highlightingActionEntriesNow)
				{
					ClearAbilityHighlightForAnimEntries();
				}
				List<ActorAnimation> list = new List<ActorAnimation>();
				bool flag7 = true;
				foreach (ActorAnimation actorAnimation3 in m_actorAnimations)
				{
					ActorModelData actorModelData = actorAnimation3?.Caster?.GetActorModelData();
					Animator animator = actorModelData?.GetModelAnimator();
					bool isCasterAlreadyInRagdoll = actorAnimation3.Caster != null
						&& actorAnimation3.GetAnimationIndex() <= 0
						&& actorAnimation3.Caster.IsInRagdoll();
					if (actorAnimation3 == null
						|| actorAnimation3.PlayState != 0
						|| actorAnimation3.m_playOrderIndex != minPlayOrderIndex)
					{
						continue;
					}

					bool flag9 = false;
					if (NetworkClient.active)
					{
						flag9 = !actorAnimation3.FindIfSequencesReadyToPlay(Index)
							|| animator == null
							|| animator.layerCount < 1
							|| (!isCasterAlreadyInRagdoll
								&& !actorModelData.IsPlayingIdleAnim()
								&& (!actorModelData.IsPlayingDamageAnim() || !TheatricsManager.Get().m_allowAbilityAnimationInterruptHitReaction))
							|| animator.GetInteger("Attack") != 0;
					}
					if (flag9)
					{
						if (m_timeSinceActorAnimationPlayed > 1f && actorModelData.IsPlayingKnockdownAnim())
						{
							if (!m_loggedWarningForInKnockdownAnim)
							{
								string message = actorAnimation3 + " is stuck in knockdown when trying to play animation for ability, forcing idle";
								if (Application.isEditor)
								{
									Log.Error(message);
								}
								else
								{
									Log.Warning(message);
								}
								m_loggedWarningForInKnockdownAnim = true;
							}
							animator.SetBool("TurnStart", true);
							animator.SetTrigger("ForceIdle");
						}
						if (m_timeSinceActorAnimationPlayed > 5f)
						{
							if (!m_displayedHungErrorForCurrentActorAnim)
							{
								m_displayedHungErrorForCurrentActorAnim = true;
								bool isPlayingDamageReaction = animator.GetCurrentAnimatorStateInfo(0).IsName("Damage");
								int attackParam = animator.GetInteger("Attack");
								string hitActors = string.Empty;
								if (actorAnimation3.HitActorsToDeltaHP != null)
								{
									foreach (var hitActorsToDeltaHP in actorAnimation3.HitActorsToDeltaHP)
									{
										hitActors += hitActorsToDeltaHP.Key.ToString();
										hitActors += ", ";
									}
								}

								Log.Error($"{actorAnimation3} is not ready to play. " +
									$"Current animation state: {(actorModelData != null ? actorModelData.GetCurrentAnimatorStateName() : "NULL actor model data")}, " +
									$"playing idle animation: {(actorModelData != null ? actorModelData.IsPlayingIdleAnim().ToString() : "NULL actor model data")}, " +
									$"to hit({hitActors}), " +
									$"playing damage reaction: {isPlayingDamageReaction}, " +
									$"attack animation parameter: {attackParam}, " +
									$"animator layer count: {(animator != null ? animator.layerCount.ToString() : "NULL")}" +
									$"{(actorAnimation3.PlayState == ActorAnimation.PlaybackState.NotStarted ? (", sequences ready: " + actorAnimation3.FindIfSequencesReadyToPlay(Index, true)) : string.Empty)}");

							}
							if (m_timeSinceActorAnimationPlayed > 8f)
							{
								Log.Error($"{actorAnimation3} timed out, skipping");
								m_displayedHungErrorForCurrentActorAnim = false;
								if (ClientResolutionManager.Get() != null)
								{
									ClientResolutionManager.Get().UpdateLastEventTime();
								}
								continue;
							}
						}
						flag7 = false;
					}
					list.Add(actorAnimation3);
				}
				m_playOrderGroupChanged = m_playOrderGroupIndex != minPlayOrderGroupIndex;
				if (m_cameraTargetPlayOrderIndex != minPlayOrderIndex && flag2)
				{
					Bounds bounds = turn.CalcAbilitiesBounds(this, minPlayOrderIndex, out bool isDefaultBounds);
					bool isEvasionOrKnockback = Index == AbilityPriority.Evasion || Index == AbilityPriority.Combat_Knockback;
					_001B = false;
					m_cinematicCamera = false;
					bool useLowPosition = false;
					if (!isEvasionOrKnockback)
					{
						useLowPosition = ActorAnimContainsKillOrSave(minPlayOrderIndex);
						ActorData actorData = GetActorAtPlayOrderIndex(minPlayOrderIndex, out m_cinematicCamera);
						int actorIndex = actorData != null ? actorData.ActorIndex : -1;
						if (_001E > 0
							&& actorIndex == _001E
							&& !m_cinematicCamera)
						{
							_001B = true;
						}
					}
					if (!isDefaultBounds)
					{
						bool flag12 = m_turn.m_cameraBoundSetCount > 0 && m_turn.m_lastSetBoundInTurn == bounds;
						if (m_turn.m_cameraBoundSetCount > 0
							&& !flag12
							&& !isEvasionOrKnockback
							&& CanUseSameBounds(m_turn.m_lastSetBoundInTurn, bounds, abilitiesCamera.m_similarCenterDistThreshold, abilitiesCamera.m_similarBoundSideMaxDiff, abilitiesCamera.m_considerFramingSimilarIfInsidePrevious))
						{
							bounds = m_turn.m_lastSetBoundInTurn;
						}
						bool quickerTransition = Index == AbilityPriority.Evasion || !m_playOrderGroupChanged;
						CameraManager.Get().SetTarget(bounds, quickerTransition, useLowPosition);
						m_turn.m_cameraBoundSetForEvade = true;
						m_cameraBoundsSameAsLast = m_turn.m_lastSetBoundInTurn == bounds;
						m_turn.m_lastSetBoundInTurn = bounds;
						if (isEvasionOrKnockback)
						{
							m_turn.m_cameraBoundSetCount = 0;
						}
						else
						{
							m_turn.m_cameraBoundSetCount++;
						}
					}
					if (minPlayOrderIndex == 0)
					{
						CameraManager.Get().OnActionPhaseChange(ActionBufferPhase.Abilities, true);
					}
					m_cameraTargetPlayOrderIndex = minPlayOrderIndex;
					m_cameraTargetPlayOrderIndexTime = GameTime.time;
					if (isEvasionOrKnockback)
					{
						StartAbilityHighlightForAnimEntries(list);
					}
					else
					{
						ClearAbilityHighlightForAnimEntries();
					}
					if (TheatricsManager.DebugTraceExecution)
					{
						TheatricsManager.LogForDebugging("Cam set target for player order index " + minPlayOrderIndex + " group " + minPlayOrderGroupIndex + " group changed " + m_playOrderGroupChanged + " timeInResolve = " + m_turn.TimeInResolve + " anticipating CamStartEvent...");
					}
				}
				if (flag7)
				{
					m_playOrderIndex = minPlayOrderIndex;
					m_playOrderGroupIndex = minPlayOrderGroupIndex;
					flag2 = false;
					GameEventManager.TheatricsAbilityAnimationStartArgs theatricsAbilityAnimationStartArgs = new GameEventManager.TheatricsAbilityAnimationStartArgs();
					theatricsAbilityAnimationStartArgs.lastInPhase = (m_playOrderIndex >= m_maxPlayOrderIndex);
					GameEventManager.Get().FireEvent(GameEventManager.EventType.TheatricsAbilityAnimationStart, theatricsAbilityAnimationStartArgs);
				}
			}
			AbilityPriority index = Index;
			float easeInTime;
			if (index != AbilityPriority.Evasion)
			{
				easeInTime = abilitiesCamera.m_easeInTime;
				if (!m_cinematicCamera)
				{
					if (_001B && m_cameraBoundsSameAsLast)
					{
						easeInTime = 0f;
					}
					else if (m_cameraBoundsSameAsLast)
					{
						easeInTime = abilitiesCamera.m_easeInTimeForSimilarBounds;
					}
					else if (!m_playOrderGroupChanged)
					{
						easeInTime = abilitiesCamera.m_easeInTimeWithinGroup;
					}
				}
				float num23 = m_cameraTargetPlayOrderIndexTime <= 0f ? 0f : (m_cameraTargetPlayOrderIndexTime + easeInTime);
				foreach (ActorAnimation actorAnimation7 in m_actorAnimations)
				{
					if (actorAnimation7 != null
						&& actorAnimation7.PlayState == 0
						&& actorAnimation7.m_playOrderIndex == m_playOrderIndex)
					{
						float num25 = Mathf.Max(0f, num23 - GameTime.time);
						if (actorAnimation7.ShouldIgnoreCameraFraming())
						{
							num25 = 0f;
						}
						float camStartEventDelay = actorAnimation7.GetCamStartEventDelay(false);
						if (actorAnimation7.GetAnimationIndex() == 0)
						{
							camStartEventDelay = 0.35f;
						}
						if (camStartEventDelay < 0f)
						{
							Log.Error("Camera start event delay is negative");
							camStartEventDelay = 0f;
						}
						if (num25 <= camStartEventDelay)
						{
							if (TheatricsManager.DebugTraceExecution)
							{
								TheatricsManager.LogForDebugging(
									"Queued " + actorAnimation7
									+ "\ngroup " + actorAnimation7.m_playOrderGroupIndex
									+ " camStartEventDelay: " + camStartEventDelay
									+ " easeInTime: " + easeInTime
									+ " camera bounds similar as last: " + m_cameraBoundsSameAsLast
									+ " phase " + Index.ToString());
							}
							actorAnimation7.Play(turn);
							m_timeSinceActorAnimationPlayed = 0f;
							m_loggedWarningForInKnockdownAnim = false;
							_001E = ((!(actorAnimation7.Caster != null)) ? (-1) : actorAnimation7.Caster.ActorIndex);
							if (Index != AbilityPriority.Evasion
								&& Index != AbilityPriority.Combat_Knockback
								&& !actorAnimation7.ShouldIgnoreCameraFraming())
							{
								StartAbilityHighlightForAnimEntries(new List<ActorAnimation>
								{
									actorAnimation7
								});
							}
						}
					}
				}
			}
			else if (m_playOrderIndex < m_firstNonCinematicPlayOrderIndex)
			{
				foreach (ActorAnimation actorAnimation4 in m_actorAnimations)
				{
					if (actorAnimation4 != null
						&& actorAnimation4.PlayState == ActorAnimation.PlaybackState.NotStarted
						&& actorAnimation4.m_playOrderIndex == m_playOrderIndex)
					{
						actorAnimation4.Play(turn);
						m_timeSinceActorAnimationPlayed = 0f;
						m_loggedWarningForInKnockdownAnim = false;
					}
				}
			}
			else if (!m_firedMoveStartEvent)
			{
				float maxEvadeStartDelay = Mathf.Max(0.8f, m_maxCamStartDelay);
				if (m_evasionMoveStartDesiredTime < 0f)
				{
					m_evasionMoveStartDesiredTime = GameTime.time + maxEvadeStartDelay;
					if (TheatricsManager.DebugTraceExecution)
					{
						TheatricsManager.LogForDebugging("Setting evade start time: " + m_evasionMoveStartDesiredTime + " maxEvadeStartDelay: " + maxEvadeStartDelay);
					}
				}
				foreach (ActorAnimation actorAnimation5 in m_actorAnimations)
				{
					if (actorAnimation5 != null
						&& actorAnimation5.PlayState == 0
						&& actorAnimation5.m_playOrderIndex == m_playOrderIndex)
					{
						bool useTauntCamAltTime = Index == AbilityPriority.Evasion && actorAnimation5.IsCinematicRequested();
						if (m_evasionMoveStartDesiredTime <= GameTime.time + Mathf.Max(float.Epsilon, GameTime.smoothDeltaTime) + actorAnimation5.GetCamStartEventDelay(useTauntCamAltTime))
						{
							actorAnimation5.Play(turn);
							m_timeSinceActorAnimationPlayed = 0f;
							m_loggedWarningForInKnockdownAnim = false;
							if (m_firstNonCinematicEvadeAbilityPlayedTime == 0f)
							{
								m_firstNonCinematicEvadeAbilityPlayedTime = GameTime.time;
							}
						}
						if (actorAnimation5.ForceActorVisibleForAbilityCast())
						{
							actorAnimation5.Caster.CurrentlyVisibleForAbilityCast = true;
						}
					}
				}
				if (m_evasionMoveStartDesiredTime > 0f && m_evasionMoveStartDesiredTime <= GameTime.time)
				{
					foreach (ActorAnimation actorAnimation6 in m_actorAnimations)
					{
						if (actorAnimation6.GetAbility() != null)
						{
							actorAnimation6.GetAbility().OnEvasionMoveStartEvent(actorAnimation6.Caster);
						}
					}
					List<ActorData> actors = GameFlowData.Get().GetActors();
					foreach (ActorData actor in actors)
					{
						actor.CurrentlyVisibleForAbilityCast = false;
					}
					foreach (ActorData actor in actors)
					{
						actor.ForceUpdateIsVisibleToClientCache();
					}
					m_firedMoveStartEvent = true;
					GameEventManager.Get().FireEvent(GameEventManager.EventType.TheatricsEvasionMoveStart, null);
					foreach (ActorData actor in actors)
					{
						actor.ForceUpdateIsVisibleToClientCache();
					}
					if (TheatricsManager.DebugTraceExecution)
					{
						TheatricsManager.LogForDebugging("Evasion Move Start, MaxCamStartDelay= " + m_maxCamStartDelay);
					}
				}
			}
			return true;
		}
	}
}

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
		private bool _0005;
		private bool _001B;
		private int _001E = -1;
		private bool m_highlightingActionEntriesNow;

		private const float c_firstEvadeWaitTimeInTurn = 0.7f;
		private const float c_firstNonEvadeWaitTimeInTurn = 0.3f;
		private const float c_animlessPlayTimeBeforeEaseInEnd = 0.35f;

		internal AbilityPriority Index { get; private set; }

		internal Dictionary<int, int> ActorIndexToDeltaHP
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
			this.m_turn = turn;
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
			bool flag = false;
			
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
							flag = true;
						}
						else
						{
							if (deltaHP > 0 && hitPointsAfterResolution <= 0 && hitPointsAfterResolutionWithDelta > 0)
							{
								flag = true;
							}
							else
							{
								if (m_turn.IsReadyToRagdoll(hitActor, deltaHP, (int)actorAnimation.SeqSource.RootID))
								{
									flag = true;
									if (CameraManager.CamDebugTraceOn)
									{
										CameraManager.LogForDebugging(string.Concat("Ragdolling hit on ", hitActor, " when HP is already 0"));
									}
								}
							}
						}
						if (flag && CameraManager.CamDebugTraceOn)
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
			return flag;
		}

		private bool CanUseSameBounds(Bounds prev, Bounds bounds, float maxCenterDistDiff, float maxSideDiff, bool canUseInflatedBounds)
		{
			float centerDist = VectorUtils.HorizontalPlaneDistInWorld(prev.center, bounds.center);
			if (centerDist <= maxCenterDistDiff)
			{
				bool usedInflatedBounds = false;
				bool boundSidesWithinDistance = CameraManager.BoundSidesWithinDistance(prev, bounds, maxSideDiff, out Vector3 maxBoundDiff, out Vector3 minBoundDiff);
				if (canUseInflatedBounds && !boundSidesWithinDistance)
				{
					Bounds bounds2 = prev;
					bounds2.Expand(new Vector3(1.5f, 100f, 1.5f));
					if (bounds2.Contains(bounds.center + bounds.extents) && bounds2.Contains(bounds.center - bounds.extents))
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
			if (CameraManager.CamDebugTraceOn)
			{
				CameraManager.LogForDebugging("Not merging bounds, centerDist too far: " + centerDist, CameraManager.CameraLogType.SimilarBounds);
			}
			return false;
		}

		internal bool Update(Turn turn, ref bool hiddenAction, ref bool nonHiddenAction)
		{
			m_timeSinceActorAnimationPlayed += GameTime.deltaTime;
			m_timeSinceUpdateStart += GameTime.deltaTime;
			bool flag = ((m_actorAnimations.Count != 0) & (Index == AbilityPriority.Evasion)) && !m_firedMoveStartEvent;
			bool flag2 = true;
			for (int i = 0; i < m_actorAnimations.Count; i++)
			{
				ActorAnimation actorAnimation = m_actorAnimations[i];
				if (actorAnimation.Update(turn))
				{
					flag = true;
					if (actorAnimation.m_playOrderIndex <= m_playOrderIndex)
					{
						flag2 = flag2 && actorAnimation.ShouldAutoCameraReleaseFocus();
					}
					if (actorAnimation.m_playOrderIndex == m_playOrderIndex)
					{
						bool flag3 = actorAnimation.NotInLoS();
						hiddenAction = hiddenAction || flag3;
						nonHiddenAction = nonHiddenAction || !flag3;
					}
				}
			}
			while (true)
			{
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
				int num4 = int.MaxValue;
				int num5 = int.MaxValue;
				for (int j = 0; j < m_actorAnimations.Count; j++)
				{
					ActorAnimation actorAnimation2 = m_actorAnimations[j];
					if (actorAnimation2 != null && actorAnimation2.PlayState == 0 && actorAnimation2.m_playOrderIndex < num4)
					{
						num4 = actorAnimation2.m_playOrderIndex;
						num5 = actorAnimation2.m_playOrderGroupIndex;
					}
				}
				while (true)
				{
					AbilitiesCamera abilitiesCamera = AbilitiesCamera.Get();
					List<ActorAnimation> list = null;
					float num7 = Index == AbilityPriority.Evasion ? 0.7f : 0.3f;
					bool flag4 = turn.TimeInResolve >= num7 || m_timeSinceActorAnimationPlayed >= num7;
					bool flag5 = flag4 && flag2 && num4 != m_playOrderIndex && num4 != int.MaxValue;
					bool isResolutionPaused = GameFlowData.Get() == null || GameFlowData.Get().IsResolutionPaused();
					flag5 = flag5 && !isResolutionPaused;
					if (!flag5 && m_timeSinceActorAnimationPlayed > 20f && !isResolutionPaused)
					{
						Log.Error("Stuck when trying to advance to next actor anim entry, \nplay order release focus: " + flag2.ToString() + "\npast waiting for first action: " + flag4 + "\nminNotStartedPLayOrderIndex: " + num4 + "\nplayOrderIndex: " + m_playOrderIndex);
						flag5 = true;
					}
					if (flag5)
					{
						if (m_highlightingActionEntriesNow)
						{
							ClearAbilityHighlightForAnimEntries();
						}
						list = new List<ActorAnimation>();
						bool flag7 = true;
						for (int k = 0; k < m_actorAnimations.Count; k++)
						{
							ActorAnimation actorAnimation3 = m_actorAnimations[k];
							object obj;
							if (actorAnimation3 != null)
							{
								if (!(actorAnimation3.Caster == null))
								{
									obj = actorAnimation3.Caster.GetActorModelData();
									goto IL_03fc;
								}
							}
							obj = null;
							goto IL_03fc;
							IL_03fc:
							ActorModelData actorModelData = (ActorModelData)obj;
							object obj2;
							if (actorModelData == null)
							{
								obj2 = null;
							}
							else
							{
								obj2 = actorModelData.GetModelAnimator();
							}
							Animator animator = (Animator)obj2;
							bool flag8 = false;
							if (actorAnimation3.Caster != null)
							{
								if (actorAnimation3.GetAnimationIndex() <= 0)
								{
									flag8 = actorAnimation3.Caster.IsInRagdoll();
								}
							}
							if (actorAnimation3 == null)
							{
								continue;
							}
							if (actorAnimation3.PlayState != 0)
							{
								continue;
							}
							if (actorAnimation3.m_playOrderIndex != num4)
							{
								continue;
							}
							bool flag9 = false;
							int num12;
							if (NetworkClient.active)
							{
								if (actorAnimation3.FindIfSequencesReadyToPlay(Index))
								{
									if (!(animator == null) && animator.layerCount >= 1)
									{
										if (flag8 || actorModelData.IsPlayingIdleAnim())
										{
											goto IL_053a;
										}
										if (actorModelData.IsPlayingDamageAnim())
										{
											if (TheatricsManager.Get().m_allowAbilityAnimationInterruptHitReaction)
											{
												goto IL_053a;
											}
										}
									}
								}
								num12 = 1;
								goto IL_0551;
							}
							goto IL_0553;
							IL_0553:
							if (flag9)
							{
								if (m_timeSinceActorAnimationPlayed > 1f)
								{
									if (actorModelData.IsPlayingKnockdownAnim())
									{
										if (!m_loggedWarningForInKnockdownAnim)
										{
											string message = string.Concat(actorAnimation3, " is stuck in knockdown when trying to play animation for ability, forcing idle");
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
										Log.Error(string.Concat(actorAnimation3, " timed out, skipping"));
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
							continue;
							IL_0551:
							flag9 = ((byte)num12 != 0);
							goto IL_0553;
							IL_053a:
							num12 = ((animator.GetInteger("Attack") != 0) ? 1 : 0);
							goto IL_0551;
						}
						m_playOrderGroupChanged = (m_playOrderGroupIndex != num5);
						if (m_cameraTargetPlayOrderIndex != num4)
						{
							if (flag2)
							{
								bool _00122;
								Bounds bounds = turn.CalcAbilitiesBounds(this, num4, out _00122);
								int num13;
								if (Index != AbilityPriority.Evasion)
								{
									num13 = ((Index == AbilityPriority.Combat_Knockback) ? 1 : 0);
								}
								else
								{
									num13 = 1;
								}
								bool flag11 = (byte)num13 != 0;
								_001B = false;
								_0005 = false;
								bool useLowPosition = false;
								int num14 = -1;
								if (!flag11)
								{
									useLowPosition = ActorAnimContainsKillOrSave(num4);
									ActorData actorData = GetActorAtPlayOrderIndex(num4, out _0005);
									num14 = ((!(actorData != null)) ? (-1) : actorData.ActorIndex);
									if (_001E > 0)
									{
										if (num14 == _001E)
										{
											if (!_0005)
											{
												_001B = true;
											}
										}
									}
								}
								if (!_00122)
								{
									bool flag12 = m_turn.m_cameraBoundSetCount > 0 && m_turn.m_lastSetBoundInTurn == bounds;
									if (m_turn.m_cameraBoundSetCount > 0)
									{
										if (!flag12)
										{
											if (!flag11)
											{
												if (CanUseSameBounds(m_turn.m_lastSetBoundInTurn, bounds, abilitiesCamera.m_similarCenterDistThreshold, abilitiesCamera.m_similarBoundSideMaxDiff, abilitiesCamera.m_considerFramingSimilarIfInsidePrevious))
												{
													bounds = m_turn.m_lastSetBoundInTurn;
												}
											}
										}
									}
									int num15;
									if (Index != AbilityPriority.Evasion)
									{
										num15 = ((!m_playOrderGroupChanged) ? 1 : 0);
									}
									else
									{
										num15 = 1;
									}
									bool quickerTransition = (byte)num15 != 0;
									CameraManager.Get().SetTarget(bounds, quickerTransition, useLowPosition);
									m_turn.m_cameraBoundSetForEvade = true;
									if (m_turn.m_lastSetBoundInTurn == bounds)
									{
										m_cameraBoundsSameAsLast = true;
									}
									else
									{
										m_cameraBoundsSameAsLast = false;
									}
									m_turn.m_lastSetBoundInTurn = bounds;
									if (flag11)
									{
										m_turn.m_cameraBoundSetCount = 0;
									}
									else
									{
										m_turn.m_cameraBoundSetCount++;
									}
								}
								if (num4 == 0)
								{
									CameraManager.Get().OnActionPhaseChange(ActionBufferPhase.Abilities, true);
								}
								m_cameraTargetPlayOrderIndex = num4;
								m_cameraTargetPlayOrderIndexTime = GameTime.time;
								if (flag11)
								{
									StartAbilityHighlightForAnimEntries(list);
								}
								else
								{
									ClearAbilityHighlightForAnimEntries();
								}
								if (TheatricsManager.DebugLog)
								{
									TheatricsManager.LogForDebugging("Cam set target for player order index " + num4 + " group " + num5 + " group changed " + m_playOrderGroupChanged + " timeInResolve = " + m_turn.TimeInResolve + " anticipating CamStartEvent...");
								}
							}
						}
						if (flag7)
						{
							m_playOrderIndex = num4;
							m_playOrderGroupIndex = num5;
							flag2 = false;
							GameEventManager.TheatricsAbilityAnimationStartArgs theatricsAbilityAnimationStartArgs = new GameEventManager.TheatricsAbilityAnimationStartArgs();
							theatricsAbilityAnimationStartArgs.lastInPhase = (m_playOrderIndex >= m_maxPlayOrderIndex);
							GameEventManager.Get().FireEvent(GameEventManager.EventType.TheatricsAbilityAnimationStart, theatricsAbilityAnimationStartArgs);
						}
					}
					AbilityPriority index = Index;
					float num16;
					if (index != AbilityPriority.Evasion)
					{
						num16 = abilitiesCamera.m_easeInTime;
						if (!_0005)
						{
							if (_001B)
							{
								if (m_cameraBoundsSameAsLast)
								{
									num16 = 0f;
									goto IL_106f;
								}
							}
							if (m_cameraBoundsSameAsLast)
							{
								num16 = abilitiesCamera.m_easeInTimeForSimilarBounds;
							}
							else if (!m_playOrderGroupChanged)
							{
								num16 = abilitiesCamera.m_easeInTimeWithinGroup;
							}
						}
						goto IL_106f;
					}
					if (m_playOrderIndex < m_firstNonCinematicPlayOrderIndex)
					{
						for (int l = 0; l < m_actorAnimations.Count; l++)
						{
							ActorAnimation actorAnimation4 = m_actorAnimations[l];
							if (actorAnimation4 == null)
							{
								continue;
							}
							if (actorAnimation4.PlayState == ActorAnimation.PlaybackState.NotStarted)
							{
								if (actorAnimation4.m_playOrderIndex == m_playOrderIndex)
								{
									actorAnimation4.Play(turn);
									m_timeSinceActorAnimationPlayed = 0f;
									m_loggedWarningForInKnockdownAnim = false;
								}
							}
						}
					}
					else if (!m_firedMoveStartEvent)
					{
						float num17 = Mathf.Max(0.8f, m_maxCamStartDelay);
						if (m_evasionMoveStartDesiredTime < 0f)
						{
							m_evasionMoveStartDesiredTime = GameTime.time + num17;
							if (TheatricsManager.DebugLog)
							{
								TheatricsManager.LogForDebugging("Setting evade start time: " + m_evasionMoveStartDesiredTime + " maxEvadeStartDelay: " + num17);
							}
						}
						float num18 = m_evasionMoveStartDesiredTime;
						for (int m = 0; m < m_actorAnimations.Count; m++)
						{
							ActorAnimation actorAnimation5 = m_actorAnimations[m];
							if (actorAnimation5 == null)
							{
								continue;
							}
							if (actorAnimation5.PlayState != 0)
							{
								continue;
							}
							if (actorAnimation5.m_playOrderIndex != m_playOrderIndex)
							{
								continue;
							}
							int num19;
							if (Index == AbilityPriority.Evasion)
							{
								num19 = (actorAnimation5.IsCinematicRequested() ? 1 : 0);
							}
							else
							{
								num19 = 0;
							}
							bool flag13 = (byte)num19 != 0;
							if (num18 <= GameTime.time + Mathf.Max(float.Epsilon, GameTime.smoothDeltaTime) + actorAnimation5.GetCamStartEventDelay(flag13))
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
						if (num18 > 0f && num18 <= GameTime.time)
						{
							for (int n = 0; n < m_actorAnimations.Count; n++)
							{
								ActorAnimation actorAnimation6 = m_actorAnimations[n];
								if (actorAnimation6.GetAbility() != null)
								{
									actorAnimation6.GetAbility().OnEvasionMoveStartEvent(actorAnimation6.Caster);
								}
							}
							List<ActorData> actors = GameFlowData.Get().GetActors();
							for (int num20 = 0; num20 < actors.Count; num20++)
							{
								actors[num20].CurrentlyVisibleForAbilityCast = false;
							}
							for (int num21 = 0; num21 < actors.Count; num21++)
							{
								actors[num21].ForceUpdateIsVisibleToClientCache();
							}
							m_firedMoveStartEvent = true;
							GameEventManager.Get().FireEvent(GameEventManager.EventType.TheatricsEvasionMoveStart, null);
							for (int num22 = 0; num22 < actors.Count; num22++)
							{
								actors[num22].ForceUpdateIsVisibleToClientCache();
							}
							if (TheatricsManager.DebugLog)
							{
								TheatricsManager.LogForDebugging("Evasion Move Start, MaxCamStartDelay= " + m_maxCamStartDelay);
							}
						}
					}
					goto IL_12cd;
					IL_12cd:
					return true;
					IL_106f:
					float num23 = (!(m_cameraTargetPlayOrderIndexTime <= 0f)) ? (m_cameraTargetPlayOrderIndexTime + num16) : 0f;
					for (int num24 = 0; num24 < m_actorAnimations.Count; num24++)
					{
						ActorAnimation actorAnimation7 = m_actorAnimations[num24];
						if (actorAnimation7 == null)
						{
							continue;
						}
						if (actorAnimation7.PlayState != 0 || actorAnimation7.m_playOrderIndex != m_playOrderIndex)
						{
							continue;
						}
						float num25 = Mathf.Max(0f, num23 - GameTime.time);
						if (actorAnimation7.ShouldIgnoreCameraFraming())
						{
							num25 = 0f;
						}
						float num26 = actorAnimation7.GetCamStartEventDelay(false);
						if (actorAnimation7.GetAnimationIndex() == 0)
						{
							num26 = 0.35f;
						}
						if (num26 < 0f)
						{
							Log.Error("Camera start event delay is negative");
							num26 = 0f;
						}
						if (!(num25 <= num26))
						{
							continue;
						}
						if (TheatricsManager.DebugLog)
						{
							TheatricsManager.LogForDebugging(string.Concat("Queued ", actorAnimation7, "\ngroup ", actorAnimation7.m_playOrderGroupIndex, " camStartEventDelay: ", num26, " easeInTime: ", num16, " camera bounds similar as last: ", m_cameraBoundsSameAsLast, " phase ", Index.ToString()));
						}
						actorAnimation7.Play(turn);
						m_timeSinceActorAnimationPlayed = 0f;
						m_loggedWarningForInKnockdownAnim = false;
						_001E = ((!(actorAnimation7.Caster != null)) ? (-1) : actorAnimation7.Caster.ActorIndex);
						if (Index == AbilityPriority.Evasion)
						{
							continue;
						}
						if (Index != AbilityPriority.Combat_Knockback)
						{
							if (!actorAnimation7.ShouldIgnoreCameraFraming())
							{
								StartAbilityHighlightForAnimEntries(new List<ActorAnimation>
								{
									actorAnimation7
								});
							}
						}
					}
					goto IL_12cd;
				}
			}
		}
	}
}

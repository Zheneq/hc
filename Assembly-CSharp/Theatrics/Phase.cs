using CameraManagerInternal;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Networking;

namespace Theatrics
{
	public class Phase
	{
		public List<ActorAnimation> Animations = new List<ActorAnimation>();
		private Dictionary<int, int> actorIndexToDeltaHP = new Dictionary<int, int>();
		public Dictionary<int, int> ActorIndexToKnockback = new Dictionary<int, int>(); // only matters if it is zero or positive
		public List<int> Participants = new List<int>();
		private int playOrderIndex = -1;
		private int _0018 = -1;
		private bool _0009 = true;
		private int _0019 = -1;
		private int _0011_somePlayOrderIndex = -1;
		private float maxCamStartDelay;
		private int _0004 = -1;
		private float _000B;
		private float _0003;
		public Turn Turn;
		private float _0017;
		private float _000D;
		private bool _0008;
		private bool _0002;
		private float evadeStartTime = -1f;
		private bool IsNotSendingAnimations;
		private bool _0020;
		private bool _000C;
		private bool _0014;
		private bool _0005;
		private bool _001B;
		private int _001E = -1;
		private bool _0001_HasAbilitiesToHighlight;
		private const float _001F = 0.7f;
		private const float _0010 = 0.3f;
		private const float _0007 = 0.35f;

		public AbilityPriority Index
		{
			get;
			set;
		}

		public Dictionary<int, int> ActorIndexToDeltaHP
		{
			get
			{
				return actorIndexToDeltaHP;
			}
			set
			{
				actorIndexToDeltaHP = value;
			}
		}

		internal Phase(Turn turn)
		{
			this.Turn = turn;
		}

		public void DoNotSendAnimations()
		{
			IsNotSendingAnimations = true;
		}

		internal void _001D_000E()
		{
			for (int i = 0; i < Animations.Count; i++)
			{
				ActorAnimation actorAnimation = Animations[i];
				if (!actorAnimation._0006_000E())
				{
					float delay = actorAnimation.GetCamStartEventDelay(Index == AbilityPriority.Evasion && actorAnimation.IsTauntActivated());
					maxCamStartDelay = Mathf.Max(maxCamStartDelay, delay);
				}
				if (_0011_somePlayOrderIndex == -1 && !actorAnimation.cinematicCamera)
				{
					_0011_somePlayOrderIndex = actorAnimation.playOrderIndex;
				}
			}
		}

		internal bool _001C(ActorData actor)
		{
			bool result = false;
			if (Animations != null)
			{
				for (int i = 0; i < Animations.Count; i++)
				{
					ActorAnimation actorAnimation = Animations[i];
					if (actorAnimation.Actor == actor && !actorAnimation._0006_000E())
					{
						if (!ClientResolutionManager.Get().HitsDoneExecuting(actorAnimation.SeqSource) ||
							actorAnimation.State < ActorAnimation.PlaybackState.ANIMATION_FINISHED)
						{
							result = true;
							break;
						}
					}
				}
			}
			return result;
		}

		internal bool _001D_000E_IsKnockedBack(ActorData actor)
		{
			if (ActorIndexToKnockback != null && ActorIndexToKnockback.ContainsKey(actor.ActorIndex))
			{
				return ActorIndexToKnockback[actor.ActorIndex] > 0;
			}
			return false;
		}

		internal bool IsParticipant(ActorData actor) // _000E_000E
		{
			if (actor != null && Participants != null)
			{
				return Participants.Contains(actor.ActorIndex);
			}
			return false;
		}

		internal bool _001C_HasUnfinishedAnimations()
		{
			for (int i = 0; i < Animations.Count; i++)
			{
				ActorAnimation actorAnimation = Animations[i];
				if (actorAnimation.State != ActorAnimation.PlaybackState.FINISHED && actorAnimation.State != ActorAnimation.PlaybackState.F)
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
			sbyte numAnimations = (sbyte)Animations.Count;
			bool dropAnimations = IsNotSendingAnimations || phaseIndex < (sbyte)ServerClientUtils.GetCurrentAbilityPhase();
			if (stream.isWriting && dropAnimations)
			{
				numAnimations = 0;
			}
			stream.Serialize(ref numAnimations);
			for (int i = 0; i < numAnimations; i++)
			{
				while (i >= Animations.Count)
				{
					Animations.Add(new ActorAnimation(Turn));
				}
				Animations[i].OnSerializeHelper(stream);
			}
			if (stream.isWriting)
			{
				sbyte numActorIndexToDeltaHP = (sbyte)actorIndexToDeltaHP.Count;
				stream.Serialize(ref numActorIndexToDeltaHP);
				foreach (var actorIndexAndDeltaHP in actorIndexToDeltaHP)
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
				actorIndexToDeltaHP = new Dictionary<int, int>();
				for (int j = 0; j < numActorIndexToDeltaHP; j++)
				{
					sbyte actorIndex = (sbyte)ActorData.s_invalidActorIndex;
					short deltaHP = -1;
					stream.Serialize(ref actorIndex);
					stream.Serialize(ref deltaHP);
					actorIndexToDeltaHP.Add(actorIndex, deltaHP);
				}
			}
			if (phaseIndex == (sbyte)AbilityPriority.Combat_Knockback)
			{
				if (stream.isWriting)
				{
					sbyte value9 = (sbyte)ActorIndexToKnockback.Count;
					if (dropAnimations)
					{
						value9 = 0;
						stream.Serialize(ref value9);
					}
					else
					{
						stream.Serialize(ref value9);
						foreach (KeyValuePair<int, int> item in ActorIndexToKnockback)
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
					ActorIndexToKnockback = new Dictionary<int, int>();
					for (int k = 0; k < value12; k++)
					{
						sbyte value13 = (sbyte)ActorData.s_invalidActorIndex;
						sbyte value14 = -1;
						stream.Serialize(ref value13);
						stream.Serialize(ref value14);
						ActorIndexToKnockback.Add(value13, value14);
					}
				}
			}
			if (stream.isWriting)
			{
				sbyte numParticipants = (sbyte)Participants.Count;
				stream.Serialize(ref numParticipants);
				for (sbyte b = 0; b < numParticipants; b = (sbyte)(b + 1))
				{
					sbyte participant = (sbyte)Participants[b];
					stream.Serialize(ref participant);
				}
			}
			else
			{
				sbyte numParticipants = -1;
				stream.Serialize(ref numParticipants);
				Participants = new List<int>();
				for (sbyte b = 0; b < numParticipants; b = (sbyte)(b + 1))
				{
					sbyte participant = -1;
					stream.Serialize(ref participant);
					Participants.Add(participant);
				}
			}
		}

		private void FireEventTheatricsAbilityHighlightStart(List<ActorAnimation> animations)
		{
			if (animations == null || animations.Count <= 0)
			{
				return;
			}
			GameEventManager.TheatricsAbilityHighlightStartArgs theatricsAbilityHighlightStartArgs = new GameEventManager.TheatricsAbilityHighlightStartArgs();
			for (int i = 0; i < animations.Count; i++)
			{
				ActorAnimation actorAnimation = animations[i];
				theatricsAbilityHighlightStartArgs.m_casters.Add(actorAnimation.Actor);
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
			_0001_HasAbilitiesToHighlight = true;

		}

		private void FireEventTheatricsAbilityHighlightStart()
		{
			GameEventManager.TheatricsAbilityHighlightStartArgs args = new GameEventManager.TheatricsAbilityHighlightStartArgs();
			GameEventManager.Get().FireEvent(GameEventManager.EventType.TheatricsAbilityHighlightStart, args);
			_0001_HasAbilitiesToHighlight = false;
		}

		private ActorData GetActorAtPlayOrderIndex(int playOrderIndex, out bool cinematicCamera)
		{
			cinematicCamera = false;
			for (int i = 0; i < Animations.Count; i++)
			{
				if (Animations[i].playOrderIndex == playOrderIndex)
				{
					cinematicCamera = Animations[i].cinematicCamera;
					return Animations[i].Actor;
				}
			}
			return null;
		}

		private bool _001C(int playOrderIndex)
		{
			bool flag = false;
			
			for (int num = 0; num < Animations.Count; num++)
			{
				ActorAnimation actorAnimation = Animations[num];
				if (actorAnimation != null && actorAnimation.HitActorsToDeltaHP != null && actorAnimation.playOrderIndex == playOrderIndex)
				{
					if (actorAnimation.IsTauntActivated())
					{
						break;
					}
					foreach (var current in actorAnimation.HitActorsToDeltaHP)
					{
						ActorData hitActor = current.Key;
						int deltaHP = current.Value;
						int hitPointsAfterResolution = hitActor.GetHitPointsToDisplay();
						int hitPointsAfterResolutionWithDelta = hitActor.GetHitPointsToDisplayWithDelta(deltaHP);
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
								if (Turn._0004_FinishedTheatrics(hitActor, deltaHP, (int)actorAnimation.SeqSource.RootID))
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

		private bool AreBoundsMergeable(Bounds prevBound, Bounds compareToBound, float maxCenterDistToMerge, float mergeSizeThresh, bool allowInflatedBounds) // _001C
		{
			float centerDist = VectorUtils.HorizontalPlaneDistInWorld(prevBound.center, compareToBound.center);
			if (centerDist <= maxCenterDistToMerge)
			{
				bool usedInflatedBounds = false;
				bool boundSidesWithinDistance = CameraManager.BoundSidesWithinDistance(prevBound, compareToBound, mergeSizeThresh, out Vector3 maxBoundDiff, out Vector3 minBoundDiff);
				if (allowInflatedBounds && !boundSidesWithinDistance)
				{
					Bounds bounds = prevBound;
					bounds.Expand(new Vector3(1.5f, 100f, 1.5f));
					if (bounds.Contains(compareToBound.center + compareToBound.extents) && bounds.Contains(compareToBound.center - compareToBound.extents))
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
						$"\nPrev Bound: {prevBound}" +
						$"\nCompare to Bound: {compareToBound}",
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

		// Play
		internal bool _001C(Turn _001D, ref bool _000E, ref bool _0012)
		{
			_0017 += GameTime.deltaTime;
			_000D += GameTime.deltaTime;
			bool flag = ((Animations.Count != 0) & (Index == AbilityPriority.Evasion)) && !_0002;
			bool flag2 = true;
			for (int i = 0; i < Animations.Count; i++)
			{
				ActorAnimation actorAnimation = Animations[i];
				if (!actorAnimation._000D_000E_Tick(_001D))
				{
					continue;
				}
				flag = true;
				if (actorAnimation.playOrderIndex <= playOrderIndex)
				{
					flag2 = flag2 && actorAnimation._0005_000E();
				}
				if (actorAnimation.playOrderIndex != playOrderIndex)
				{
					continue;
				}
				bool flag3 = actorAnimation._000C_000E();
				_000E = _000E || flag3;
				_0012 = _0012 || !flag3;
			}
			while (true)
			{
				if (CameraManager.Get().IsPlayingShotSequence())
				{
					_0008 = true;
					return true;
				}
				if (_0008)
				{
					_0017 = 0f;
					_0008 = false;
				}
				if (!flag)
				{
					return false;
				}
				int num4 = int.MaxValue;
				int num5 = int.MaxValue;
				for (int j = 0; j < Animations.Count; j++)
				{
					ActorAnimation actorAnimation2 = Animations[j];
					if (actorAnimation2 != null && actorAnimation2.State == 0 && actorAnimation2.playOrderIndex < num4)
					{
						num4 = actorAnimation2.playOrderIndex;
						num5 = actorAnimation2.groupIndex;
					}
				}
				while (true)
				{
					AbilitiesCamera abilitiesCamera = AbilitiesCamera.Get();
					List<ActorAnimation> list = null;
					float num7 = Index == AbilityPriority.Evasion ? 0.7f : 0.3f;
					bool flag4 = _001D.TimeInResolve >= num7 || _0017 >= num7;
					bool flag5 = flag4 && flag2 && num4 != playOrderIndex && num4 != int.MaxValue;
					bool isResolutionPaused = GameFlowData.Get() == null || GameFlowData.Get().IsResolutionPaused();
					flag5 = flag5 && !isResolutionPaused;
					if (!flag5 && _0017 > 20f && !isResolutionPaused)
					{
						Log.Error("Stuck when trying to advance to next actor anim entry, \nplay order release focus: " + flag2.ToString() + "\npast waiting for first action: " + flag4 + "\nminNotStartedPLayOrderIndex: " + num4 + "\nplayOrderIndex: " + playOrderIndex);
						flag5 = true;
					}
					if (flag5)
					{
						if (_0001_HasAbilitiesToHighlight)
						{
							FireEventTheatricsAbilityHighlightStart();
						}
						list = new List<ActorAnimation>();
						bool flag7 = true;
						for (int k = 0; k < Animations.Count; k++)
						{
							ActorAnimation actorAnimation3 = Animations[k];
							object obj;
							if (actorAnimation3 != null)
							{
								if (!(actorAnimation3.Actor == null))
								{
									obj = actorAnimation3.Actor.GetActorModelData();
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
							if (actorAnimation3.Actor != null)
							{
								if (actorAnimation3.GetAnimationIndex() <= 0)
								{
									flag8 = actorAnimation3.Actor.IsModelAnimatorDisabled();
								}
							}
							if (actorAnimation3 == null)
							{
								continue;
							}
							if (actorAnimation3.State != 0)
							{
								continue;
							}
							if (actorAnimation3.playOrderIndex != num4)
							{
								continue;
							}
							bool flag9 = false;
							int num12;
							if (NetworkClient.active)
							{
								if (actorAnimation3.IsReadyToPlay_zq(Index))
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
								if (_0017 > 1f)
								{
									if (actorModelData.IsPlayingKnockdownAnim())
									{
										if (!_000C)
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
											_000C = true;
										}
										animator.SetBool("TurnStart", true);
										animator.SetTrigger("ForceIdle");
									}
								}
								if (_0017 > 5f)
								{
									if (!_0020)
									{
										_0020 = true;
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
											$"{(actorAnimation3.State == ActorAnimation.PlaybackState.A ? (", sequences ready: " + actorAnimation3.IsReadyToPlay_zq(Index, true)) : string.Empty)}");

									}
									if (_0017 > 8f)
									{
										Log.Error(string.Concat(actorAnimation3, " timed out, skipping"));
										_0020 = false;
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
						_0009 = (_0018 != num5);
						if (_0004 != num4)
						{
							if (flag2)
							{
								bool _00122;
								Bounds bounds = _001D._0011_CreateAbilitiesBounds(this, num4, out _00122);
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
									useLowPosition = _001C(num4);
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
									bool flag12 = Turn._0018 > 0 && Turn._0013 == bounds;
									if (Turn._0018 > 0)
									{
										if (!flag12)
										{
											if (!flag11)
											{
												if (AreBoundsMergeable(Turn._0013, bounds, abilitiesCamera.m_similarCenterDistThreshold, abilitiesCamera.m_similarBoundSideMaxDiff, abilitiesCamera.m_considerFramingSimilarIfInsidePrevious))
												{
													bounds = Turn._0013;
												}
											}
										}
									}
									int num15;
									if (Index != AbilityPriority.Evasion)
									{
										num15 = ((!_0009) ? 1 : 0);
									}
									else
									{
										num15 = 1;
									}
									bool quickerTransition = (byte)num15 != 0;
									CameraManager.Get().SetTarget(bounds, quickerTransition, useLowPosition);
									Turn._0009_HasFocusedAction = true;
									if (Turn._0013 == bounds)
									{
										_0014 = true;
									}
									else
									{
										_0014 = false;
									}
									Turn._0013 = bounds;
									if (flag11)
									{
										Turn._0018 = 0;
									}
									else
									{
										Turn._0018++;
									}
								}
								if (num4 == 0)
								{
									CameraManager.Get().OnActionPhaseChange(ActionBufferPhase.Abilities, true);
								}
								_0004 = num4;
								_000B = GameTime.time;
								if (flag11)
								{
									FireEventTheatricsAbilityHighlightStart(list);
								}
								else
								{
									FireEventTheatricsAbilityHighlightStart();
								}
								if (TheatricsManager.DebugLog)
								{
									TheatricsManager.LogForDebugging("Cam set target for player order index " + num4 + " group " + num5 + " group changed " + _0009 + " timeInResolve = " + Turn.TimeInResolve + " anticipating CamStartEvent...");
								}
							}
						}
						if (flag7)
						{
							playOrderIndex = num4;
							_0018 = num5;
							flag2 = false;
							GameEventManager.TheatricsAbilityAnimationStartArgs theatricsAbilityAnimationStartArgs = new GameEventManager.TheatricsAbilityAnimationStartArgs();
							theatricsAbilityAnimationStartArgs.lastInPhase = (playOrderIndex >= _0019);
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
								if (_0014)
								{
									num16 = 0f;
									goto IL_106f;
								}
							}
							if (_0014)
							{
								num16 = abilitiesCamera.m_easeInTimeForSimilarBounds;
							}
							else if (!_0009)
							{
								num16 = abilitiesCamera.m_easeInTimeWithinGroup;
							}
						}
						goto IL_106f;
					}
					if (playOrderIndex < _0011_somePlayOrderIndex)
					{
						for (int l = 0; l < Animations.Count; l++)
						{
							ActorAnimation actorAnimation4 = Animations[l];
							if (actorAnimation4 == null)
							{
								continue;
							}
							if (actorAnimation4.State == ActorAnimation.PlaybackState.A)
							{
								if (actorAnimation4.playOrderIndex == playOrderIndex)
								{
									actorAnimation4.Play(_001D);
									_0017 = 0f;
									_000C = false;
								}
							}
						}
					}
					else if (!_0002)
					{
						float num17 = Mathf.Max(0.8f, maxCamStartDelay);
						if (evadeStartTime < 0f)
						{
							evadeStartTime = GameTime.time + num17;
							if (TheatricsManager.DebugLog)
							{
								TheatricsManager.LogForDebugging("Setting evade start time: " + evadeStartTime + " maxEvadeStartDelay: " + num17);
							}
						}
						float num18 = evadeStartTime;
						for (int m = 0; m < Animations.Count; m++)
						{
							ActorAnimation actorAnimation5 = Animations[m];
							if (actorAnimation5 == null)
							{
								continue;
							}
							if (actorAnimation5.State != 0)
							{
								continue;
							}
							if (actorAnimation5.playOrderIndex != playOrderIndex)
							{
								continue;
							}
							int num19;
							if (Index == AbilityPriority.Evasion)
							{
								num19 = (actorAnimation5.IsTauntActivated() ? 1 : 0);
							}
							else
							{
								num19 = 0;
							}
							bool flag13 = (byte)num19 != 0;
							if (num18 <= GameTime.time + Mathf.Max(float.Epsilon, GameTime.smoothDeltaTime) + actorAnimation5.GetCamStartEventDelay(flag13))
							{
								actorAnimation5.Play(_001D);
								_0017 = 0f;
								_000C = false;
								if (_0003 == 0f)
								{
									_0003 = GameTime.time;
								}
							}
							if (actorAnimation5._0020_000E_IsActorVisibleForAbilityCast())
							{
								actorAnimation5.Actor.CurrentlyVisibleForAbilityCast = true;
							}
						}
						if (num18 > 0f && num18 <= GameTime.time)
						{
							for (int n = 0; n < Animations.Count; n++)
							{
								ActorAnimation actorAnimation6 = Animations[n];
								if (actorAnimation6.GetAbility() != null)
								{
									actorAnimation6.GetAbility().OnEvasionMoveStartEvent(actorAnimation6.Actor);
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
							_0002 = true;
							GameEventManager.Get().FireEvent(GameEventManager.EventType.TheatricsEvasionMoveStart, null);
							for (int num22 = 0; num22 < actors.Count; num22++)
							{
								actors[num22].ForceUpdateIsVisibleToClientCache();
							}
							if (TheatricsManager.DebugLog)
							{
								TheatricsManager.LogForDebugging("Evasion Move Start, MaxCamStartDelay= " + maxCamStartDelay);
							}
						}
					}
					goto IL_12cd;
					IL_12cd:
					return true;
					IL_106f:
					float num23 = (!(_000B <= 0f)) ? (_000B + num16) : 0f;
					for (int num24 = 0; num24 < Animations.Count; num24++)
					{
						ActorAnimation actorAnimation7 = Animations[num24];
						if (actorAnimation7 == null)
						{
							continue;
						}
						if (actorAnimation7.State != 0 || actorAnimation7.playOrderIndex != playOrderIndex)
						{
							continue;
						}
						float num25 = Mathf.Max(0f, num23 - GameTime.time);
						if (actorAnimation7.GetSymbol0013())
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
							TheatricsManager.LogForDebugging(string.Concat("Queued ", actorAnimation7, "\ngroup ", actorAnimation7.groupIndex, " camStartEventDelay: ", num26, " easeInTime: ", num16, " camera bounds similar as last: ", _0014, " phase ", Index.ToString()));
						}
						actorAnimation7.Play(_001D);
						_0017 = 0f;
						_000C = false;
						_001E = ((!(actorAnimation7.Actor != null)) ? (-1) : actorAnimation7.Actor.ActorIndex);
						if (Index == AbilityPriority.Evasion)
						{
							continue;
						}
						if (Index != AbilityPriority.Combat_Knockback)
						{
							if (!actorAnimation7.GetSymbol0013())
							{
								FireEventTheatricsAbilityHighlightStart(new List<ActorAnimation>
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

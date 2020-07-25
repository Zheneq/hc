using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Networking;

namespace Theatrics
{
	public class Turn
	{
		public List<Phase> Phases = new List<Phase>(7);
		private int CurrentPhase = -1;
		internal Bounds _0013;
		internal int _0018;
		internal bool _0009_HasFocusedAction;
		private HashSet<int> CompletedPhases = new HashSet<int>();

		public int TurnID
		{
			get;
			set;
		}

		internal float TimeInPhase
		{
			get;
			private set;
		}

		internal float TimeInResolve
		{
			get;
			private set;
		}

		internal Turn()
		{
		}

		internal static bool IsEvasionOrKnockback(AbilityPriority priority)
		{
			return priority == AbilityPriority.Evasion || priority == AbilityPriority.Combat_Knockback;
		}

		internal void OnSerializeHelper(IBitStream stream) // _0011
		{
			int turnID = TurnID;
			stream.Serialize(ref turnID);
			TurnID = turnID;
			sbyte numPhases = (sbyte)Phases.Count;
			stream.Serialize(ref numPhases);
			
			for (int num = 0; num < numPhases; num++)
			{
				while (num >= Phases.Count)
				{
					Phases.Add(new Phase(this));
				}
				Phases[num].OnSerializeHelper(stream);
			}
		}

		internal void GoToPhase(AbilityPriority priority)
		{
			if (priority == (AbilityPriority)CurrentPhase)
			{
				return;
			}

			TimeInPhase = 0f;
			CurrentPhase = (int)priority;
			if (CurrentPhase >= 0 && CurrentPhase < Phases.Count)
			{
				TheatricsManager.Get().SetAnimatorParamOnAllActors("DecisionPhase", false);
				Phases[CurrentPhase]._001D_000E();
					
			}
			List<ActorData> actors = GameFlowData.Get().GetActors();
			if (actors != null)
			{
				for (int i = 0; i < actors.Count; i++)
				{
					ActorData actorData = actors[i];
					if (actorData != null &&
						actorData.GetHitPointsToDisplay() <= 0 &&
						!actorData.IsModelAnimatorDisabled())
					{
						if (GameplayData.Get().m_resolveDamageBetweenAbilityPhases || _0004_FinishedTheatrics(actorData))
						{
							actorData.DoVisualDeath(Sequence.CreateImpulseInfoWithActorForward(actorData));
						}
					}
				}
			}
			if (NetworkClient.active &&
				priority == AbilityPriority.Combat_Damage &&
				ClientResolutionManager.Get() != null &&
				!ClientResolutionManager.Get().IsWaitingForActionMessages(priority))
			{
				ClientResolutionManager.Get().OnCombatPhasePlayDataReceived();
			}
		}

		internal bool _001A(AbilityPriority phase)
		{
			TimeInResolve += GameTime.deltaTime;
			if (CurrentPhase >= 7)
			{
				return false;
			}
			bool flag = false;
			bool flag2 = false;
			bool flag3;
			int num = CurrentPhase;
			if (CurrentPhase < 0)
			{
				Log.Error("Phase index is negative! Code error.");
				return true;
			}
			flag3 = num < Phases.Count && !Phases[num]._001C(this, ref flag, ref flag2);
			bool resolutionTimedOut = TimeInPhase >= GameFlowData.Get().m_resolveTimeoutLimit * 0.8f;
			if (resolutionTimedOut)
			{
				Log.Error("Theatrics: phase: " + ServerClientUtils.GetCurrentActionPhase().ToString() + " timed out for turn " + TurnID + ",  timeline index " + CurrentPhase);
			}
			bool flag4 = !flag3 && !resolutionTimedOut;
			if (flag4)
			{
				if (flag && !flag2 &&
					GameFlowData.Get().activeOwnedActorData != null &&
					!GameFlowData.Get().activeOwnedActorData.IsDead())
				{
					InterfaceManager.Get().DisplayAlert(StringUtil.TR("HiddenAction", "Global"), Color.white);
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
				TheatricsManager.Get().ServerLog("Theatrics: finished timeline index " + CurrentPhase + " with duration " + TimeInPhase + " @absolute time " + GameTime.time);
				if (TheatricsManager.DebugLog)
				{
					TheatricsManager.LogForDebugging("Phase Finished: " + CurrentPhase);
				}
			}
			if (!flag4)
			{
				if (!HasAnimationsAfterPhase(phase) && NetworkClient.active && !CompletedPhases.Contains(CurrentPhase))
				{
					CompletedPhases.Add(CurrentPhase);
					GameEventManager.Get().FireEvent(GameEventManager.EventType.TheatricsAbilitiesEnd, null);
				}
			}
			return flag4;
		}

		internal void OnAnimationEvent(ActorData animatedActor, Object eventObject, GameObject sourceObject) // _0011
		{
			if (CurrentPhase < 0 || CurrentPhase >= 7)
			{
				return;
			}
			Phase phase = this.Phases[this.CurrentPhase];
			if (phase == null)
			{
				return;
			}
			for (int i = 0; i < phase.Animations.Count; i++)
			{
				ActorAnimation actorAnimation = phase.Animations[i];

				if (actorAnimation.Actor == animatedActor &&
					actorAnimation.PlaybackState2OrLater_zq &&
					actorAnimation._0014_000E_NotFinished())
				{
					actorAnimation._000D_000E(animatedActor, eventObject, sourceObject);
					if (actorAnimation.ParentAbilitySeqSource != null)
					{
						actorAnimation._0008_000E(animatedActor, eventObject, sourceObject);
					}
					break;
				}
			}
		}

		internal void OnSequenceHit(
			Sequence seq,
			ActorData target,
			ActorModelData.ImpulseInfo impulseInfo,
			ActorModelData.RagdollActivation ragdollActivation = ActorModelData.RagdollActivation.HealthBased) // _0011
		{
			if (this.CurrentPhase >= 7)
			{
				return;
			}
			bool flag = false;
			bool flag2 = false;
			if (this.CurrentPhase >= 0)
			{
				Phase phase = this.Phases[this.CurrentPhase];
				if (phase != null)
				{
					int num = 0;
					while (true)
					{
						if (num < phase.Animations.Count)
						{
							ActorAnimation actorAnimation = phase.Animations[num];
							if (actorAnimation.Actor == seq.Caster)
							{
								if (actorAnimation.HasSameSequenceSource(seq))
								{
									if (actorAnimation._000D_000E(seq, target, impulseInfo, ragdollActivation))
									{
										flag = true;
										break;
									}
									goto IL_00e7;
								}
							}
							if (actorAnimation.Actor == target && !actorAnimation._0014_000E_NotFinished() && actorAnimation.PlaybackState2OrLater_zq)
							{
								flag2 = true;
							}
							goto IL_00e7;
						}
						break;
						IL_00e7:
						num++;
					}
				}
			}
			if (flag)
			{
				return;
			}
			while (true)
			{
				if (!seq.RequestsHitAnimation(target))
				{
					return;
				}
				while (true)
				{
					ActorModelData actorModelData = target.GetActorModelData();
					if (actorModelData != null)
					{
						if (flag2)
						{
							if (!actorModelData.CanPlayDamageReactAnim())
							{
								goto IL_0177;
							}
						}
						if (_001A_AreAnimationsFinishedFor(target))
						{
							target.PlayDamageReactionAnim(seq.m_customHitReactTriggerName);
						}
					}
					goto IL_0177;
					IL_0177:
					if (ragdollActivation == ActorModelData.RagdollActivation.None)
					{
						return;
					}
					while (true)
					{
						if (_0004_FinishedTheatrics(target))
						{
							while (true)
							{
								target.DoVisualDeath(impulseInfo);
								return;
							}
						}
						return;
					}
				}
			}
		}

		internal Bounds _0011_CreateAbilitiesBounds(Phase _001D, int _000E, out bool _0012)
		{
			_0012 = true;
			bool flag = _001D == null;
			ActorData actorData = GameFlowData.Get().activeOwnedActorData;
			if (actorData == null)
			{
				List<ActorData> actors = GameFlowData.Get().GetActors();
				if (actors != null)
				{
					if (actors.Count != 0)
					{
						actorData = actors[0];
						goto IL_0087;
					}
				}
				Log.Error("No actors found to create Abilities Bounds.");
				return default(Bounds);
			}
			goto IL_0087;
			IL_0087:
			BoardSquare boardSquare = Board.Get().GetSquare(actorData.transform.position);
			Bounds cameraBounds = boardSquare.CameraBounds;
			Vector3 center = cameraBounds.center;
			center.y = 0f;
			Bounds result = cameraBounds;
			bool flag2 = true;
			for (int i = 0; i < this.Phases.Count; i++)
			{
				Phase phase = this.Phases[i];
				if (_001D != null)
				{
					if (_001D != phase)
					{
						continue;
					}
				}
				for (int j = 0; j < phase.Animations.Count; j++)
				{
					ActorAnimation actorAnimation = phase.Animations[j];
					if (_000E >= 0)
					{
						if (actorAnimation.playOrderIndex != _000E)
						{
							continue;
						}
					}
					if (!actorAnimation._0014_000E_NotFinished())
					{
						continue;
					}
					if (actorAnimation._000C_000E())
					{
						continue;
					}
					if (actorAnimation.GetSymbol0013())
					{
						continue;
					}
					Bounds bound = actorAnimation.bounds;
					if (_001D.Index == AbilityPriority.Evasion && actorAnimation.Actor != null)
					{
						ActorTeamSensitiveData teamSensitiveData_authority = actorAnimation.Actor.TeamSensitiveData_authority;
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
					_0012 = false;
				}
				if (!flag)
				{
					continue;
				}
				if (!flag2)
				{
					break;
				}
			}
			return result;
		}

		internal bool HasAnimations() // _0011
		{
			return HasAnimationsAfterPhase(AbilityPriority.INVALID);
		}

		internal bool HasAnimationsAfterPhase(AbilityPriority phase) // _0004
		{
			for (int i = (int)(phase + 1); i < Phases.Count; i++)
			{
				if (PhaseHasAnimations((AbilityPriority)i))
				{
					return true;
				}
			}
			return false;
		}

		internal bool PhaseHasAnimations(AbilityPriority phase) // _000B
		{
			if (phase >= AbilityPriority.Prep_Defense && (int)phase < Phases.Count)
			{
				return Phases[(int)phase].Animations.Count > 0;
			}
			return false;
		}

		internal bool _0003_PhaseHasUnfinishedAnimations(AbilityPriority phase)
		{
			if (phase >= AbilityPriority.Prep_Defense && (int)phase < Phases.Count)
			{
				return Phases[(int)phase]._001C_HasUnfinishedAnimations();
			}
			return false;
		}

		private bool IsDeadAtCurrentPhase(ActorData actor)
		{
			return IsDeadAtPhase(actor, CurrentPhase);
		}

		internal bool IsDeadAtPhase(ActorData actor, int phase)
		{
			if (actor.HitPoints <= 0)
			{
				return true;
			}
			if (phase >= (int)AbilityPriority.NumAbilityPriorities)
			{
				return actor.GetHitPointsToDisplay() <= 0;
			}
			int num = 0;
			for (int i = 0; i <= phase; i++)
			{
				Dictionary<int, int> dictionary = this.Phases[i].ActorIndexToDeltaHP;
				if (dictionary != null && dictionary.ContainsKey(actor.ActorIndex))
				{
					num += dictionary[actor.ActorIndex];
				}
			}
			return actor.HitPoints + actor.AbsorbPoints + num <= 0;
		}

		internal bool _001A_AreAnimationsFinishedFor(ActorData actor)
		{
			if (actor.IsModelAnimatorDisabled())
			{
				return false;
			}
			if (CurrentPhase > 0 && CurrentPhase < Phases.Count)
			{
				List<ActorAnimation> animations = Phases[CurrentPhase].Animations;
				for (int i = 0; i < animations.Count; i++)
				{
					ActorAnimation actorAnimation = animations[i];
					if (actorAnimation.Actor == actor &&
						actorAnimation.PlaybackState2OrLater_zq &&
						!actorAnimation.AnimationFinished)
					{
						return false;
					}
				}
			}
			return true;
		}

		internal bool _0004_FinishedTheatrics(ActorData actor, int deltaHP = 0, int seqSourceRootID = -1)
		{
			if (actor.GetHitPointsToDisplay() + deltaHP <= 0 &&
				!actor.IsModelAnimatorDisabled() &&
				IsDeadAtCurrentPhase(actor) &&
				this.CurrentPhase >= 3)
			{
				int num = this.CurrentPhase;
				int num2 = this.CurrentPhase;
				while (true)
				{
					if (num >= 0 && num < this.Phases.Count)
					{
						if (num == 5 && this.Phases[num]._001D_000E_IsKnockedBack(actor))
						{
							return false;
						}
						List<ActorAnimation> animations = this.Phases[num].Animations;
						for (int i = 0; i < animations.Count; i++)
						{
							ActorAnimation actorAnimation = animations[i];
							if (seqSourceRootID < 0 || actorAnimation.SeqSource.RootID != seqSourceRootID)
							{
								if (actorAnimation.Actor == actor && actorAnimation._0014_000E_NotFinished() || actorAnimation.IsPendingHitOn(actor))
								{
									return false;
								}
							}
						}
						if (num > num2 && this.Phases[num].IsParticipant(actor))
						{
							return false;
						}
						
					}
					num++;
					if (num >= 7 || GameplayData.Get().m_resolveDamageBetweenAbilityPhases)
					{
						break;
					}
				}
				if (ClientResolutionManager.Get() != null && ClientResolutionManager.Get().HasUnexecutedHitsOnActor(actor, seqSourceRootID))
				{
					return false;
				}
				return true;
			}
			return false;
		}

		public bool IsCinematicPlaying() // _001A
		{
			if (CurrentPhase >= 0 && CurrentPhase < Phases.Count)
			{
				List<ActorAnimation> animations = Phases[CurrentPhase].Animations;
				for (int i = 0; i < animations.Count; i++)
				{
					if (animations[i].cinematicCamera &&
						(animations[i].State == ActorAnimation.PlaybackState.C ||
						animations[i].State == ActorAnimation.PlaybackState.D ||
						animations[i].State == ActorAnimation.PlaybackState.ANIMATION_FINISHED))
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}

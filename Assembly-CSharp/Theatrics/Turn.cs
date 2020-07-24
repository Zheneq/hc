using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Networking;

namespace Theatrics
{
	internal class Turn
	{
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private int _001D;

		internal List<Phase> Phases = new List<Phase>(7);

		private int CurrentPhase = -1;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private float _0015;

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private float _0016;

		internal Bounds _0013;

		internal int _0018;

		internal bool _0009;

		private HashSet<int> _0019 = new HashSet<int>();

		internal int TurnID
		{
			get;
			private set;
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

		internal void _0011(IBitStream stream) { OnSerializeHelper(stream); }
		internal void OnSerializeHelper(IBitStream stream) // _0011
		{
			int value = TurnID;
			stream.Serialize(ref value);
			TurnID = value;
			sbyte value2 = (sbyte)Phases.Count;
			stream.Serialize(ref value2);
			
			for (int num = 0; num < value2; num++)
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
				Phase phase = Phases[CurrentPhase];
				phase._001D_000E();
					
			}
			List<ActorData> actors = GameFlowData.Get().GetActors();
			if (actors != null)
			{
				for (int i = 0; i < actors.Count; i++)
				{
					ActorData actorData = actors[i];
					if (actorData == null ||
						actorData.GetHitPointsAfterResolution() > 0 ||
						actorData.IsModelAnimatorDisabled())
					{
						continue;
					}
					if (GameplayData.Get().m_resolveDamageBetweenAbilityPhases || _0004(actorData))
					{
						actorData.DoVisualDeath(Sequence.CreateImpulseInfoWithActorForward(actorData));
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

		internal bool _001A(AbilityPriority priority)
		{
			TimeInResolve += GameTime.deltaTime;
			if (CurrentPhase >= 7)
			{
				return false;
			}
			bool flag = false;
			bool flag2 = false;
			bool flag3;
			bool flag4;
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
			flag4 = true;
			if (!flag3 && !resolutionTimedOut)
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
				flag4 = false;
				TheatricsManager.Get().ServerLog("Theatrics: finished timeline index " + CurrentPhase + " with duration " + TimeInPhase + " @absolute time " + GameTime.time);
				if (TheatricsManager.DebugLog)
				{
					TheatricsManager.LogForDebugging("Phase Finished: " + CurrentPhase);
				}
			}
			if (!flag4)
			{
				if (!HasAnimationsAfterPhase(priority) && NetworkClient.active && !_0019.Contains(CurrentPhase))
				{
					_0019.Add(CurrentPhase);
					GameEventManager.Get().FireEvent(GameEventManager.EventType.TheatricsAbilitiesEnd, null);
				}
			}
			return flag4;
		}

		internal void _0011(ActorData animatedActor, Object eventObject, GameObject sourceObject)
		{
			OnAnimationEvent(animatedActor, eventObject, sourceObject);
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
			for (int i = 0; i < phase.animations.Count; i++)
			{
				ActorAnimation actorAnimation = phase.animations[i];

				if (actorAnimation.Actor == animatedActor &&
					actorAnimation.PlaybackState2OrLater_zq &&
					actorAnimation._0014_000E())
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

		internal void _0011(
			Sequence _001D,
			ActorData _000E,
			ActorModelData.ImpulseInfo _0012,
			ActorModelData.RagdollActivation _0015 = ActorModelData.RagdollActivation.HealthBased)
		{
			OnSequenceHit(_001D, _000E, _0012, _0015);
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
						if (num < phase.animations.Count)
						{
							ActorAnimation actorAnimation = phase.animations[num];
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
							if (actorAnimation.Actor == target && !actorAnimation._0014_000E() && actorAnimation.PlaybackState2OrLater_zq)
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
						if (_001A(target))
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
						if (_0004(target))
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

		internal Bounds _0011(Phase _001D, int _000E, out bool _0012)
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
				for (int j = 0; j < phase.animations.Count; j++)
				{
					ActorAnimation actorAnimation = phase.animations[j];
					if (_000E >= 0)
					{
						if (actorAnimation.playOrderIndex != _000E)
						{
							continue;
						}
					}
					if (!actorAnimation._0014_000E())
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
					Bounds bound = actorAnimation._0020;
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

		internal bool _0011() { return HasAnimations();  }
		internal bool HasAnimations() // _0011
		{
			return HasAnimationsAfterPhase(AbilityPriority.INVALID);
		}

		internal bool _0004(AbilityPriority priority) { return HasAnimationsAfterPhase(priority); }
		internal bool HasAnimationsAfterPhase(AbilityPriority priority) // _0004
		{
			for (int i = (int)(priority + 1); i < Phases.Count; i++)
			{
				if (PhaseHasAnimations((AbilityPriority)i))
				{
					return true;
				}
			}
			return false;
		}

		internal bool _000B(AbilityPriority priority) { return PhaseHasAnimations(priority); }
		internal bool PhaseHasAnimations(AbilityPriority priority) // _000B
		{
			if (priority >= AbilityPriority.Prep_Defense && (int)priority < Phases.Count)
			{
				return Phases[(int)priority].animations.Count > 0;
			}
			return false;
		}

		internal bool _0003(AbilityPriority _001D)
		{
			if (_001D >= AbilityPriority.Prep_Defense)
			{
				if ((int)_001D < Phases.Count)
				{
					return Phases[(int)_001D]._001C();
				}
			}
			return false;
		}

		private bool _0011(ActorData _001D)
		{
			return _0011(_001D, CurrentPhase);
		}

		internal bool _0011(ActorData _001D, int _000E)
		{
			if (_001D.HitPoints <= 0)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
						return true;
					}
				}
			}
			if (_000E >= 7)
			{
				return _001D.GetHitPointsAfterResolution() <= 0;
			}
			int num = 0;
			for (int i = 0; i <= _000E; i++)
			{
				Dictionary<int, int> dictionary = this.Phases[i].ActorIndexToDeltaHP;
				if (dictionary == null)
				{
					continue;
				}
				if (dictionary.ContainsKey(_001D.ActorIndex))
				{
					num += dictionary[_001D.ActorIndex];
				}
			}
			while (true)
			{
				return _001D.HitPoints + _001D.AbsorbPoints + num <= 0;
			}
		}

		internal bool _001A(ActorData _001D)
		{
			if (_001D.IsModelAnimatorDisabled())
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						return false;
					}
				}
			}
			if (CurrentPhase > 0)
			{
				if (CurrentPhase < Phases.Count)
				{
					List<ActorAnimation> animations = Phases[CurrentPhase].animations;
					for (int i = 0; i < animations.Count; i++)
					{
						ActorAnimation actorAnimation = animations[i];
						if (!(actorAnimation.Actor == _001D))
						{
							continue;
						}
						if (!actorAnimation.PlaybackState2OrLater_zq)
						{
							continue;
						}
						if (actorAnimation.AnimationFinished)
						{
							continue;
						}
						while (true)
						{
							return false;
						}
					}
				}
			}
			return true;
		}

		// FinishedThetrics
		internal bool _0004(ActorData _001D, int _000E = 0, int _0012 = -1)
		{
			if (_001D.GetHitPointsAfterResolution() + _000E <= 0)
			{
				if (!_001D.IsModelAnimatorDisabled())
				{
					if (_0011(_001D))
					{
						if (this.CurrentPhase >= 3)
						{
							int num = this.CurrentPhase;
							int num2 = this.CurrentPhase;
							while (true)
							{
								if (num < this.Phases.Count)
								{
									if (num >= 0)
									{
										if (num == 5 && this.Phases[num]._001D_000E(_001D))
										{
											while (true)
											{
												switch (2)
												{
												case 0:
													break;
												default:
													return false;
												}
											}
										}
										List<ActorAnimation> animations = this.Phases[num].animations;
										for (int i = 0; i < animations.Count; i++)
										{
											ActorAnimation actorAnimation = animations[i];
											if (_0012 >= 0)
											{
												if (actorAnimation.SeqSource.RootID == _0012)
												{
													continue;
												}
											}
											if (actorAnimation.Actor == _001D)
											{
												if (actorAnimation._0014_000E())
												{
													goto IL_0130;
												}
											}
											if (!actorAnimation._0008_000E(_001D))
											{
												continue;
											}
											goto IL_0130;
											IL_0130:
											return false;
										}
										if (num > num2)
										{
											if (this.Phases[num]._000E_000E(_001D))
											{
												while (true)
												{
													switch (2)
													{
													case 0:
														break;
													default:
														return false;
													}
												}
											}
										}
									}
								}
								num++;
								if (num >= 7)
								{
									break;
								}
								while (true)
								{
									switch (1)
									{
									case 0:
										break;
									default:
										goto end_IL_0185;
									}
									continue;
									end_IL_0185:
									break;
								}
								if (GameplayData.Get().m_resolveDamageBetweenAbilityPhases)
								{
									break;
								}
							}
							if (ClientResolutionManager.Get() != null && ClientResolutionManager.Get().HasUnexecutedHitsOnActor(_001D, _0012))
							{
								while (true)
								{
									switch (4)
									{
									case 0:
										break;
									default:
										return false;
									}
								}
							}
							return true;
						}
					}
				}
			}
			return false;
		}

		public bool _001A()
		{
			return IsCinematicPlaying();
		}
		public bool IsCinematicPlaying() // _001A
		{
			if (CurrentPhase < Phases.Count)
			{
				if (CurrentPhase >= 0)
				{
					List<ActorAnimation> animations = Phases[CurrentPhase].animations;
					for (int i = 0; i < animations.Count; i++)
					{
						if (!animations[i].cinematicCamera)
						{
							continue;
						}
						if (animations[i].State != ActorAnimation.PlaybackState.C)
						{
							if (animations[i].State != ActorAnimation.PlaybackState.D)
							{
								if (animations[i].State != ActorAnimation.PlaybackState.E)
								{
									continue;
								}
							}
						}
						return true;
					}
				}
			}
			return false;
		}
	}
}

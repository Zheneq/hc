using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using CameraManagerInternal;
using UnityEngine;
using UnityEngine.Networking;

namespace Theatrics
{
	public class ActorAnimation : IComparable<ActorAnimation>
	{
		public const float symbol_001D = 3f;

		private short animationIndex;

		private Vector3 symbol_0012;

		private bool symbol_0015;

		private int tauntNumber;

		private bool symbol_0013;

		private bool symbol_0018;

		private bool symbol_0009;

		private AbilityData.ActionType symbol_0019 = AbilityData.ActionType.INVALID_ACTION;

		private bool symbol_0011;

		private bool symbol_001A;

		private bool symbol_0004;

		private Ability ability;

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private SequenceSource symbol_0003;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private SequenceSource symbol_000F;

		internal int actorIndex = ActorData.s_invalidActorIndex;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private Dictionary<ActorData, int> symbol_000D;

		internal bool cinematicCamera;

		internal int tauntAnimIndex;

		internal sbyte playOrderIndex;

		internal sbyte groupIndex;

		internal Bounds symbol_0020;

		private List<byte> symbol_000C = new List<byte>();

		private List<byte> symbol_0014 = new List<byte>();

		private bool symbol_0005;

		private AbilityRequest symbol_001B;

		private Turn turn;

		private bool symbol_0001;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private bool symbol_001F;

		private bool symbol_0010;

		private float symbol_0007;

		private float symbol_001C;

		private float symbol_001Dsymbol_000E;

		private float symbol_000Esymbol_000E;

		private bool symbol_0012symbol_000E;

		private float symbol_0015symbol_000E;

		private bool symbol_0016symbol_000E;

		internal Bounds symbol_0013symbol_000E;

		private List<string> symbol_0018symbol_000E = new List<string>();

		private ActorAnimation.PlaybackState playbackSate;

		private static readonly int DistToGoalHash = Animator.StringToHash("DistToGoal");

		private static readonly int StartDamageReactionHash = Animator.StringToHash("StartDamageReaction");

		private static readonly int AttackHash = Animator.StringToHash("Attack");

		private static readonly int CinematicCamHash = Animator.StringToHash("CinematicCam");

		private static readonly int TauntNumberHash = Animator.StringToHash("TauntNumber");

		private static readonly int TauntAnimIndexHash = Animator.StringToHash("TauntAnimIndex");

		private static readonly int StartAttackHash = Animator.StringToHash("StartAttack");

		private const float symbol_0017symbol_000E = 1f;

		internal ActorAnimation(Turn turn)
		{
			this.turn = turn;
		}

		internal SequenceSource SeqSource { get; private set; }

		internal SequenceSource ParentAbilitySeqSource { get; private set; }

		internal ActorData Actor
		{
			get
			{
				if (this.actorIndex != ActorData.s_invalidActorIndex)
				{
					if (!(GameFlowData.Get() == null))
					{
						return GameFlowData.Get().FindActorByActorIndex(this.actorIndex);
					}
				}
				return null;
			}
			set
			{
				if (value == null)
				{
					if (this.actorIndex != ActorData.s_invalidActorIndex)
					{
						this.actorIndex = ActorData.s_invalidActorIndex;
						return;
					}
				}
				if (value != null)
				{
					if (value.ActorIndex != this.actorIndex)
					{
						this.actorIndex = value.ActorIndex;
					}
				}
			}
		}

		internal Dictionary<ActorData, int> HitActorsToDeltaHP { get; private set; }

		internal Ability GetAbility()
		{
			return this.ability;
		}

		public int TauntNumber
		{
			get
			{
				return this.tauntNumber;
			}
			private set
			{
			}
		}

		internal bool AnimationFinished { get; private set; }

		internal ActorAnimation.PlaybackState State
		{
			get
			{
				return this.playbackSate;
			}
			set
			{
				if (value != this.playbackSate)
				{
					if (this.ability != null)
					{
						if (value == ActorAnimation.PlaybackState.symbol_0012)
						{
							int num = AbilityUtils.GetTechPointRewardForInteraction(this.ability, AbilityInteractionType.Cast, true, false, false);
							num = AbilityUtils.CalculateTechPointsForTargeter(this.Actor, this.ability, num);
							if (num > 0)
							{
								this.Actor.AddCombatText(num.ToString(), string.Empty, CombatTextCategory.TP_Recovery, BuffIconToDisplay.None);
								bool flag = ClientResolutionManager.Get().IsInResolutionState();
								if (flag)
								{
									this.Actor.ClientUnresolvedTechPointGain += num;
								}
							}
							if (this.ability.GetModdedCost() > 0)
							{
								if (this.Actor.ReservedTechPoints > 0)
								{
									int num2 = this.Actor.ClientReservedTechPoints - this.ability.GetModdedCost();
									num2 = Mathf.Max(num2, -this.Actor.ReservedTechPoints);
									this.Actor.ClientReservedTechPoints = num2;
								}
							}
						}
					}
					if (TheatricsManager.DebugLog)
					{
						TheatricsManager.LogForDebugging(string.Concat(new object[]
						{
							this.ToString(),
							" PlayState: <color=cyan>",
							this.playbackSate,
							"</color> -> <color=cyan>",
							value,
							"</color>"
						}));
					}
					if (value != ActorAnimation.PlaybackState.symbol_0013)
					{
						if (value != ActorAnimation.PlaybackState.symbol_0018)
						{
							goto IL_1C8;
						}
					}
					if (this.Actor != null)
					{
						this.Actor.CurrentlyVisibleForAbilityCast = false;
					}
					IL_1C8:
					this.playbackSate = value;
				}
			}
		}

		internal bool PlaybackState2OrLater_zq
		{
			get
			{
				return this.State >= ActorAnimation.PlaybackState.symbol_0012;
			}
		}

		private bool StartFinalPlaybackState()
		{
			if (this.Actor == null)
			{
				Log.Error("Theatrics: can't start {0} since the actor can no longer be found. Was the actor destroyed during resolution?", new object[]
				{
					this
				});
				this.State = ActorAnimation.PlaybackState.symbol_0018;
				return false;
			}
			return this.State == ActorAnimation.PlaybackState.symbol_0018;
		}

		internal void OnSerializeHelper(IBitStream symbol_001D)
		{
			sbyte b = (sbyte)this.animationIndex;
			sbyte b2 = (sbyte)this.symbol_0019;
			float x = this.symbol_0012.x;
			float z = this.symbol_0012.z;
			sbyte s_invalidActorIndex;
			if (this.Actor == null)
			{
				s_invalidActorIndex = (sbyte)ActorData.s_invalidActorIndex;
			}
			else
			{
				s_invalidActorIndex = (sbyte)this.Actor.ActorIndex;
			}
			sbyte b3 = s_invalidActorIndex;
			bool flag = this.cinematicCamera;
			sbyte b4 = (sbyte)this.tauntNumber;
			bool u = this.symbol_0013;
			bool u2 = this.symbol_0018;
			bool u3 = this.symbol_0009;
			bool u4 = this.symbol_0015;
			sbyte b5 = this.playOrderIndex;
			sbyte b6 = this.groupIndex;
			Vector3 center = this.symbol_0020.center;
			Vector3 size = this.symbol_0020.size;
			byte b7 = checked((byte)this.symbol_000C.Count);
			symbol_001D.Serialize(ref b);
			symbol_001D.Serialize(ref b2);
			symbol_001D.Serialize(ref x);
			symbol_001D.Serialize(ref z);
			symbol_001D.Serialize(ref b3);
			symbol_001D.Serialize(ref flag);
			symbol_001D.Serialize(ref b4);
			symbol_001D.Serialize(ref u);
			symbol_001D.Serialize(ref u2);
			symbol_001D.Serialize(ref u3);
			symbol_001D.Serialize(ref u4);
			symbol_001D.Serialize(ref b5);
			symbol_001D.Serialize(ref b6);
			short num = (short)Mathf.RoundToInt(center.x);
			short num2 = (short)Mathf.RoundToInt(center.z);
			symbol_001D.Serialize(ref num);
			symbol_001D.Serialize(ref num2);
			if (symbol_001D.isReading)
			{
				center.x = (float)num;
				center.y = 1.5f + (float)Board.Get().BaselineHeight;
				center.z = (float)num2;
			}
			short num3 = (short)Mathf.CeilToInt(size.x + 0.5f);
			short num4 = (short)Mathf.CeilToInt(size.z + 0.5f);
			symbol_001D.Serialize(ref num3);
			symbol_001D.Serialize(ref num4);
			if (symbol_001D.isReading)
			{
				size.x = (float)num3;
				size.y = 3f;
				size.z = (float)num4;
			}
			symbol_001D.Serialize(ref b7);
			if (symbol_001D.isReading)
			{
				for (int i = 0; i < (int)b7; i++)
				{
					byte item = 0;
					byte item2 = 0;
					symbol_001D.Serialize(ref item);
					symbol_001D.Serialize(ref item2);
					this.symbol_000C.Add(item);
					this.symbol_0014.Add(item2);
				}
			}
			else
			{
				for (int j = 0; j < (int)b7; j++)
				{
					byte b8 = this.symbol_000C[j];
					byte b9 = this.symbol_0014[j];
					symbol_001D.Serialize(ref b8);
					symbol_001D.Serialize(ref b9);
				}
			}
			this.animationIndex = (short)b;
			if (symbol_001D.isReading)
			{
				this.symbol_0012 = new Vector3(x, (float)Board.Get().BaselineHeight, z);
			}
			this.actorIndex = (int)b3;
			this.cinematicCamera = flag;
			this.tauntNumber = (int)b4;
			this.symbol_0013 = u;
			this.symbol_0018 = u2;
			this.symbol_0009 = u3;
			this.symbol_0015 = u4;
			this.playOrderIndex = b5;
			this.groupIndex = b6;
			this.symbol_0020 = new Bounds(center, size);
			this.symbol_0019 = (AbilityData.ActionType)b2;
			this.ability = ((!(this.Actor == null)) ? this.Actor.GetAbilityData().GetAbilityOfActionType(this.symbol_0019) : null);
			if (this.SeqSource == null)
			{
				this.SeqSource = new SequenceSource();
			}
			this.SeqSource.OnSerializeHelper(symbol_001D);
			if (symbol_001D.isWriting)
			{
				bool flag2 = this.ParentAbilitySeqSource != null;
				symbol_001D.Serialize(ref flag2);
				if (flag2)
				{
					this.ParentAbilitySeqSource.OnSerializeHelper(symbol_001D);
				}
			}
			if (symbol_001D.isReading)
			{
				bool flag3 = false;
				symbol_001D.Serialize(ref flag3);
				if (flag3)
				{
					if (this.ParentAbilitySeqSource == null)
					{
						this.ParentAbilitySeqSource = new SequenceSource();
					}
					this.ParentAbilitySeqSource.OnSerializeHelper(symbol_001D);
				}
				else
				{
					this.ParentAbilitySeqSource = null;
				}
			}
			sbyte b10;
			if (this.HitActorsToDeltaHP == null)
			{
				b10 = 0;
			}
			else
			{
				b10 = (sbyte)this.HitActorsToDeltaHP.Count;
			}
			sbyte b11 = b10;
			symbol_001D.Serialize(ref b11);
			if ((int)b11 > 0)
			{
				if (this.HitActorsToDeltaHP == null)
				{
					this.HitActorsToDeltaHP = new Dictionary<ActorData, int>();
				}
			}
			if (symbol_001D.isWriting)
			{
				if ((int)b11 > 0)
				{
					using (Dictionary<ActorData, int>.Enumerator enumerator = this.HitActorsToDeltaHP.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							KeyValuePair<ActorData, int> keyValuePair = enumerator.Current;
							sbyte s_invalidActorIndex2;
							if (keyValuePair.Key == null)
							{
								s_invalidActorIndex2 = (sbyte)ActorData.s_invalidActorIndex;
							}
							else
							{
								s_invalidActorIndex2 = (sbyte)keyValuePair.Key.ActorIndex;
							}
							sbyte b12 = s_invalidActorIndex2;
							if ((int)b12 != ActorData.s_invalidActorIndex)
							{
								sbyte b13 = 0;
								if (keyValuePair.Value > 0)
								{
									b13 = 1;
								}
								else if (keyValuePair.Value < 0)
								{
									b13 = -1;
								}
								symbol_001D.Serialize(ref b12);
								symbol_001D.Serialize(ref b13);
							}
						}
					}
					goto IL_64D;
				}
			}
			if (symbol_001D.isReading)
			{
				if (this.HitActorsToDeltaHP != null)
				{
					this.HitActorsToDeltaHP.Clear();
				}
				for (int k = 0; k < (int)b11; k++)
				{
					sbyte b14 = (sbyte)ActorData.s_invalidActorIndex;
					sbyte b15 = 0;
					symbol_001D.Serialize(ref b14);
					symbol_001D.Serialize(ref b15);
					ActorData actorData = GameFlowData.Get().FindActorByActorIndex((int)b14);
					if (actorData != null)
					{
						this.HitActorsToDeltaHP[actorData] = (int)b15;
					}
				}
			}
			IL_64D:
			this.StartFinalPlaybackState();
		}

		internal bool symbol_0002symbol_000E()
		{
			return this.tauntNumber > 0;
		}

		internal bool GetSymbol0013()
		{
			return this.symbol_0013;
		}

		internal bool symbol_0006symbol_000E()
		{
			return this.symbol_0015;
		}

		internal bool IsActorDamaged(ActorData actor)
		{
			if (this.HitActorsToDeltaHP != null)
			{
				if (this.HitActorsToDeltaHP.ContainsKey(actor))
				{
					return this.HitActorsToDeltaHP[actor] < 0;
				}
			}
			return false;
		}

		internal bool symbol_0020symbol_000E()
		{
			bool result;
			if (!this.Actor.IsDead())
			{
				if (!this.symbol_0009)
				{
					result = this.cinematicCamera;
				}
				else
				{
					result = true;
				}
			}
			else
			{
				result = false;
			}
			return result;
		}

		internal bool symbol_000Csymbol_000E()
		{
			FogOfWar clientFog = FogOfWar.GetClientFog();
			ActorStatus actorStatus = this.Actor.GetActorStatus();
			bool flag = this.symbol_0020symbol_000E();
			if (actorStatus != null)
			{
				if (actorStatus.HasStatus(StatusType.Revealed, true))
				{
					goto IL_78;
				}
			}
			bool flag2;
			if (!this.Actor.VisibleTillEndOfPhase)
			{
				if (!this.Actor.CurrentlyVisibleForAbilityCast)
				{
					flag2 = flag;
					goto IL_79;
				}
			}
			IL_78:
			flag2 = true;
			IL_79:
			if (!flag2)
			{
				if (clientFog == null)
				{
				}
				else
				{
					if (this.symbol_0018)
					{
						return false;
					}
					if (NetworkClient.active && GameFlowData.Get() != null)
					{
						if (GameFlowData.Get().LocalPlayerData != null)
						{
							if (this.Actor.SomeVisibilityCheckB_zq(GameFlowData.Get().LocalPlayerData, true, false))
							{
								return true;
							}
						}
					}
					for (int i = 0; i < this.symbol_000C.Count; i++)
					{
						BoardSquare boardSquare = Board.Get().GetBoardSquare((int)this.symbol_000C[i], (int)this.symbol_0014[i]);
						if (boardSquare != null && clientFog.IsVisible(boardSquare))
						{
							return false;
						}
					}
					ActorMovement actorMovement = this.Actor.GetActorMovement();
					if (actorMovement && actorMovement.FindIsVisibleToClient())
					{
						return false;
					}
					if (this.HitActorsToDeltaHP != null)
					{
						if (Board.Get() != null)
						{
							using (Dictionary<ActorData, int>.Enumerator enumerator = this.HitActorsToDeltaHP.GetEnumerator())
							{
								while (enumerator.MoveNext())
								{
									KeyValuePair<ActorData, int> keyValuePair = enumerator.Current;
									ActorData key = keyValuePair.Key;
									if (key == null)
									{
									}
									else
									{
										BoardSquare boardSquare = Board.Get().GetBoardSquare(key.transform.position);
										if (clientFog.IsVisible(boardSquare))
										{
											return false;
										}
									}
								}
							}
							return true;
						}
					}
					return true;
				}
			}
			return false;
		}

		internal bool HasSameSequenceSource(Sequence sequence)
		{
			bool result;
			if (sequence != null)
			{
				result = (sequence.Source == this.SeqSource);
			}
			else
			{
				result = false;
			}
			return result;
		}

		internal bool IsReadyToPlay_zq(AbilityPriority abilityPriority, bool logErrorIfNotReady = false)
		{
			if (this.State != ActorAnimation.PlaybackState.symbol_001D)
			{
				return false;
			}
			bool flag = !ClientResolutionManager.Get().IsWaitingForActionMessages(abilityPriority);
			if (!flag)
			{
				if (logErrorIfNotReady)
				{
					Log.Error("sequences not ready, current client resolution state = {0}", new object[]
					{
						ClientResolutionManager.Get().GetCurrentStateName()
					});
				}
			}
			return flag;
		}

		internal bool symbol_0014symbol_000E()
		{
			return this.State != ActorAnimation.PlaybackState.symbol_0018 && this.State != ActorAnimation.PlaybackState.symbol_0013;
		}

		internal bool symbol_0005symbol_000E()
		{
			int result;
			if (this.State >= ActorAnimation.PlaybackState.symbol_0015)
			{
				if (!this.symbol_0001)
				{
					if (!this.AnimationFinished)
					{
						goto IL_D7;
					}
				}
				if (this.symbol_0007 > 0f)
				{
					if (NetworkClient.active)
					{
						if (GameTime.time < this.symbol_0007 + this.CalcFrameTimeAfterAllHitsButMine())
						{
							goto IL_D7;
						}
					}
					if (NetworkClient.active)
					{
						if (this.symbol_000Esymbol_000E > 0f)
						{
							result = ((GameTime.time >= this.symbol_000Esymbol_000E + 1f) ? 1 : 0);
						}
						else
						{
							result = 0;
						}
					}
					else
					{
						result = 1;
					}
					return result != 0;
				}
			}
			IL_D7:
			result = 0;
			return result != 0;
		}

		internal bool symbol_0008symbol_000E(ActorData symbol_001D)
		{
			if (this.HitActorsToDeltaHP != null && this.HitActorsToDeltaHP.ContainsKey(symbol_001D))
			{
				if (this.HitActorsToDeltaHP[symbol_001D] != 0)
				{
					if (!SequenceSource.DidSequenceHit(this.SeqSource, symbol_001D))
					{
						return true;
					}
				}
			}
			return false;
		}

		private int OtherActorsInHitActorsToDeltaHPNum()
		{
			int result;
			if (this.HitActorsToDeltaHP == null)
			{
				result = 0;
			}
			else
			{
				int count = this.HitActorsToDeltaHP.Count;
				int num;
				if (this.HitActorsToDeltaHP.ContainsKey(this.Actor))
				{
					num = 1;
				}
				else
				{
					num = 0;
				}
				result = count - num;
			}
			return result;
		}

		private float CalcFrameTimeAfterAllHitsButMine()
		{
			return AbilitiesCamera.Get().CalcFrameTimeAfterHit(this.OtherActorsInHitActorsToDeltaHPNum());
		}

		internal void method000D000E(Turn symbol_001D)
		{
			if (ClientAbilityResults.LogMissingSequences || TheatricsManager.DebugLog)
			{
				Log.Warning(string.Concat(new object[]
				{
					"<color=cyan>ActorAnimation</color> Play for: ",
					this.ToString(),
					" @time= ",
					GameTime.time
				}), new object[0]);
			}
			this.symbol_000Esymbol_000E = GameTime.time;
			if (this.State == ActorAnimation.PlaybackState.symbol_0018)
			{
				return;
			}
			bool flag;
			if (this.ability != null)
			{
				flag = this.ability.ShouldRotateToTargetPos();
			}
			else
			{
				flag = (this.animationIndex > 0);
			}
			bool flag2 = flag;
			if (flag2)
			{
				if (this.cinematicCamera)
				{
					this.Actor.TurnToPositionInstant(this.symbol_0012);
				}
				else
				{
					this.Actor.TurnToPosition(this.symbol_0012, 0.2f);
				}
			}
			Animator modelAnimator = this.Actor.GetModelAnimator();
			if (modelAnimator != null)
			{
				float num;
				if (this.ability == null)
				{
					num = 0f;
				}
				else
				{
					if (this.ability.GetMovementType() != ActorData.MovementType.None)
					{
						if (this.ability.GetMovementType() != ActorData.MovementType.Knockback)
						{
							num = 10f;
							goto IL_177;
						}
					}
					num = this.Actor.GetActorMovement().FindDistanceToEnd();
				}
				IL_177:
				float value = num;
				modelAnimator.SetFloat(ActorAnimation.DistToGoalHash, value);
				modelAnimator.ResetTrigger(ActorAnimation.StartDamageReactionHash);
			}
			AbilityData.ActionType actionTypeOfAbility = this.Actor.GetAbilityData().GetActionTypeOfAbility(this.ability);
			if (AbilityData.IsCard(actionTypeOfAbility))
			{
				GameEventManager.Get().FireEvent(GameEventManager.EventType.CardUsed, new GameEventManager.CardUsedArgs
				{
					userActor = this.Actor
				});
				if (HUD_UI.Get() != null)
				{
					HUD_UI.Get().m_mainScreenPanel.m_abilityBar.m_theTimer.m_abilityUsedTracker.AddNewAbility(this.GetAbility(), this.Actor);
				}
			}
			else if (!this.symbol_0015)
			{
				GameEventManager.Get().FireEvent(GameEventManager.EventType.AbilityUsed, new GameEventManager.AbilityUseArgs
				{
					ability = this.ability,
					userActor = this.Actor
				});
			}
			if (this.animationIndex <= 0)
			{
				this.State = ActorAnimation.PlaybackState.symbol_0016;
				this.symbol_001A = true;
				this.AnimationFinished = true;
			}
			if (NetworkServer.active)
			{
				if (NetworkClient.active)
				{
					SequenceManager.Get().DoClientEnable(this.SeqSource);
				}
			}
			else
			{
				SequenceManager.Get().DoClientEnable(this.SeqSource);
			}
			bool flag3 = this.symbol_0020symbol_000E();
			if (flag3)
			{
				this.Actor.CurrentlyVisibleForAbilityCast = true;
			}
			if (this.animationIndex <= 0)
			{
				this.no_op_2();
				this.UpdateLastEventTime();
			}
			else if (!NetworkClient.active)
			{
				this.State = ActorAnimation.PlaybackState.symbol_0016;
				this.symbol_001A = true;
				this.AnimationFinished = true;
				this.no_op_2();
				this.UpdateLastEventTime();
			}
			else
			{
				modelAnimator.SetInteger(ActorAnimation.AttackHash, (int)this.animationIndex);
				modelAnimator.SetBool(ActorAnimation.CinematicCamHash, this.cinematicCamera);
				if (this.symbol_000Dsymbol_000E(modelAnimator, "TauntNumber"))
				{
					modelAnimator.SetInteger(ActorAnimation.TauntNumberHash, this.tauntNumber);
				}
				modelAnimator.SetTrigger(ActorAnimation.StartAttackHash);
				if (this.Actor.GetActorModelData().HasAnimatorControllerParamater("TauntAnimIndex"))
				{
					modelAnimator.SetInteger(ActorAnimation.TauntAnimIndexHash, this.tauntAnimIndex);
				}
				if (this.ability != null)
				{
					this.ability.OnAbilityAnimationRequest(this.Actor, (int)this.animationIndex, this.cinematicCamera, this.symbol_0012);
				}
				if (HUD_UI.Get() != null)
				{
					HUD_UI.Get().m_mainScreenPanel.m_abilityBar.m_theTimer.m_abilityUsedTracker.AddNewAbility(this.GetAbility(), this.Actor);
				}
				if (this.symbol_0002symbol_000E())
				{
					ChatterManager.Get().CancelActiveChatter();
				}
				CameraManager.Get().OnAbilityAnimationStart(this.Actor, (int)this.animationIndex, this.symbol_0012, this.cinematicCamera, this.tauntNumber);
				if (this.Actor != null && this.cinematicCamera)
				{
					if (NetworkClient.active)
					{
						this.Actor.ForceUpdateIsVisibleToClientCache();
					}
				}
				if (this.cinematicCamera)
				{
					if (this.tauntNumber > 0)
					{
					}
				}
				this.no_op_1();
				this.UpdateLastEventTime();
				this.State = ActorAnimation.PlaybackState.symbol_0012;
			}
			if (Application.isEditor)
			{
				if (!CameraManager.CamDebugTraceOn)
				{
					if (!TheatricsManager.DebugLog)
					{
						return;
					}
				}
				ActorDebugUtils.symbol_001D(this.symbol_0013symbol_000E, Color.green, 3f);
			}
		}

		internal bool symbol_000Dsymbol_000E(Animator symbol_001D, string symbol_000E)
		{
			if (symbol_001D != null)
			{
				if (symbol_001D.parameters != null)
				{
					for (int i = 0; i < symbol_001D.parameterCount; i++)
					{
						if (symbol_001D.parameters[i].name == symbol_000E)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		internal bool symbol_000Dsymbol_000E(Turn symbol_001D)
		{
			if (NetworkClient.active)
			{
				if (!(this.Actor == null))
				{
					if (!(this.Actor.GetActorModelData() == null))
					{
						goto IL_54;
					}
				}
				this.State = ActorAnimation.PlaybackState.symbol_0018;
			}
			IL_54:
			Animator animator = null;
			if (NetworkClient.active)
			{
				if (this.State != ActorAnimation.PlaybackState.symbol_0018)
				{
					animator = this.Actor.GetModelAnimator();
					if (animator == null)
					{
						this.State = ActorAnimation.PlaybackState.symbol_0018;
					}
					else if (!animator.enabled && this.animationIndex > 0)
					{
						this.State = ActorAnimation.PlaybackState.symbol_0018;
					}
				}
			}
			if (this.State == ActorAnimation.PlaybackState.symbol_0018)
			{
				return false;
			}
			ActorMovement actorMovement = this.Actor.GetActorMovement();
			bool flag = !actorMovement.AmMoving();
			if (this.State >= ActorAnimation.PlaybackState.symbol_0012 && this.State < ActorAnimation.PlaybackState.symbol_0013)
			{
				bool flag2 = this.symbol_001C > 12f;
				if (flag2)
				{
					if (!this.symbol_0011)
					{
						this.symbol_0011 = true;
						this.DebugLogHung(animator, flag);
					}
				}
				this.symbol_001C += GameTime.deltaTime;
				if (NetworkClient.active)
				{
					if (this.State >= ActorAnimation.PlaybackState.symbol_0016)
					{
						if (!this.symbol_0012symbol_000E)
						{
							if (this.symbol_001Dsymbol_000E < 7f)
							{
								if (!flag2)
								{
									goto IL_1C5;
								}
							}
							this.symbol_000Dsymbol_000E(animator, flag);
						}
						IL_1C5:
						this.symbol_001Dsymbol_000E += GameTime.deltaTime;
					}
				}
			}
			bool flag3 = this.symbol_001C > 15f;
			if (flag3)
			{
				if (!this.symbol_0004)
				{
					this.symbol_0004 = true;
					Log.Error("Theatrics: animation timed out for {0} {1} after {2} seconds.", new object[]
					{
						this.Actor.DisplayName,
						(!(this.ability != null)) ? (" animation index " + this.animationIndex.ToString()) : this.ability.ToString(),
						this.symbol_001C
					});
				}
			}
			bool flag4 = false;
			bool flag5 = false;
			if (animator)
			{
				flag4 = this.Actor.GetActorModelData().IsPlayingAttackAnim(out flag5);
			}
			if (flag4)
			{
				this.symbol_001A = true;
				this.symbol_0015symbol_000E += GameTime.deltaTime;
			}
			bool animationFinished;
			if (this.symbol_001A)
			{
				animationFinished = !flag4;
			}
			else
			{
				animationFinished = false;
			}
			this.AnimationFinished = animationFinished;
			if (this.State >= ActorAnimation.PlaybackState.symbol_0012)
			{
				if (this.State < ActorAnimation.PlaybackState.symbol_0013)
				{
					if (this.symbol_0007 == 0f)
					{
						if (!this.symbol_0015)
						{
							if (!ClientResolutionManager.Get().HitsDoneExecuting(this.SeqSource))
							{
								goto IL_398;
							}
						}
						this.symbol_0007 = GameTime.time;
						if (TheatricsManager.DebugLog)
						{
							TheatricsManager.LogForDebugging(this.ToString() + " hits done");
						}
					}
				}
			}
			IL_398:
			bool flag6;
			if (!this.symbol_0012symbol_000E)
			{
				if (this.symbol_0007 > 0f)
				{
					flag6 = (GameTime.time - this.symbol_0007 >= this.CalcFrameTimeAfterAllHitsButMine());
				}
				else
				{
					flag6 = false;
				}
			}
			else
			{
				flag6 = true;
			}
			bool flag7 = flag6;
			switch (this.State)
			{
			case ActorAnimation.PlaybackState.symbol_001D:
				return true;
			case ActorAnimation.PlaybackState.symbol_0012:
				if (!flag4)
				{
					if (this.symbol_001C < 5f)
					{
						return true;
					}
					this.State = ActorAnimation.PlaybackState.symbol_0016;
					this.AnimationFinished = true;
					this.symbol_000Dsymbol_000E(animator, flag);
				}
				if (animator != null)
				{
					animator.SetInteger(ActorAnimation.AttackHash, 0);
					animator.SetBool(ActorAnimation.CinematicCamHash, false);
				}
				if (this.ability != null)
				{
					this.ability.OnAbilityAnimationRequestProcessed(this.Actor);
				}
				if (this.State < ActorAnimation.PlaybackState.symbol_0016)
				{
					this.State = ActorAnimation.PlaybackState.symbol_0015;
				}
				this.no_op_1();
				this.no_op_2();
				if (ClientResolutionManager.Get() != null)
				{
					ClientResolutionManager.Get().OnAbilityCast(this.Actor, this.ability);
					ClientResolutionManager.Get().UpdateLastEventTime();
				}
				break;
			case ActorAnimation.PlaybackState.symbol_0015:
				if (flag4)
				{
					if (!this.symbol_0001)
					{
						break;
					}
				}
				this.State = ActorAnimation.PlaybackState.symbol_0016;
				this.UpdateLastEventTime();
				break;
			case ActorAnimation.PlaybackState.symbol_0013:
				return false;
			}
			if (!flag)
			{
				if (!this.symbol_0010)
				{
					if (!actorMovement.OnPathType(BoardSquarePathInfo.ConnectionType.Charge))
					{
						if (!actorMovement.OnPathType(BoardSquarePathInfo.ConnectionType.Knockback))
						{
							if (!actorMovement.OnPathType(BoardSquarePathInfo.ConnectionType.Flight))
							{
								goto IL_64B;
							}
						}
					}
					bool u;
					if (FogOfWar.GetClientFog() != null)
					{
						u = FogOfWar.GetClientFog().IsVisible(this.Actor.GetTravelBoardSquare());
					}
					else
					{
						u = false;
					}
					this.symbol_0010 = u;
					if (this.symbol_0010)
					{
						Bounds bounds = CameraManager.Get().GetTarget();
						if (symbol_001D.symbol_0009)
						{
							bounds.Encapsulate(this.symbol_0020);
						}
						else
						{
							bounds = this.symbol_0020;
						}
						if (this.Actor.GetActorMovement() != null)
						{
							this.Actor.GetActorMovement().EncapsulatePathInBound(ref bounds);
						}
						CameraManager.Get().SetTarget(bounds, false, false);
						symbol_001D.symbol_0009 = true;
					}
				}
			}
			IL_64B:
			if (!this.symbol_0016symbol_000E && ServerClientUtils.GetCurrentAbilityPhase() == AbilityPriority.Combat_Knockback)
			{
				if (ClientKnockbackManager.Get() != null)
				{
					if (flag7 && this.State >= ActorAnimation.PlaybackState.symbol_0016)
					{
						ClientKnockbackManager.Get().NotifyOnActorAnimHitsDone(this.Actor);
						this.symbol_0016symbol_000E = true;
					}
				}
			}
			bool flag8;
			if (NetworkClient.active)
			{
				if (this.symbol_000Esymbol_000E > 0f)
				{
					if (!this.GetSymbol0013())
					{
						flag8 = (GameTime.time > this.symbol_000Esymbol_000E + 1f);
						goto IL_708;
					}
				}
			}
			flag8 = true;
			IL_708:
			bool flag9 = flag8;
			if (flag4)
			{
				if (!this.symbol_0001)
				{
					if (this.animationIndex > 0)
					{
						goto IL_77E;
					}
				}
			}
			if (CameraManager.Get().ShotSequence == null)
			{
				if (flag)
				{
					if (flag7)
					{
						if (flag9)
						{
							goto IL_78F;
						}
					}
				}
			}
			IL_77E:
			if (!flag3)
			{
				return this.symbol_0014symbol_000E();
			}
			IL_78F:
			this.AnimationFinished = true;
			this.State = ActorAnimation.PlaybackState.symbol_0013;
			this.no_op_1();
			this.UpdateLastEventTime();
			ActorData u000D_u000E = this.Actor;
			if (this.ability != null)
			{
				this.ability.OnAbilityAnimationReleaseFocus(u000D_u000E);
			}
			if (symbol_001D.symbol_0004(u000D_u000E, 0, -1))
			{
				u000D_u000E.DoVisualDeath(new ActorModelData.ImpulseInfo(u000D_u000E.GetTravelBoardSquareWorldPositionForLos(), Vector3.up));
			}
			return false;
		}

		internal void symbol_000Dsymbol_000E(ActorData symbol_001D, UnityEngine.Object symbol_000E, GameObject symbol_0012)
		{
			if (symbol_0012 != null)
			{
				if (symbol_000E.name != null)
				{
					if (symbol_000E.name == "CamEndEvent")
					{
						this.symbol_0001 = true;
						if (TheatricsManager.DebugLog)
						{
							TheatricsManager.LogForDebugging("CamEndEvent received for " + this.symbol_000Dsymbol_000E(string.Empty));
						}
						goto IL_98;
					}
				}
			}
			SequenceManager.Get().OnAnimationEvent(symbol_001D, symbol_000E, symbol_0012, this.SeqSource);
			IL_98:
			this.symbol_0018symbol_000E.Add(symbol_000E.name);
		}

		internal void symbol_0008symbol_000E(ActorData symbol_001D, UnityEngine.Object symbol_000E, GameObject symbol_0012)
		{
			if (this.ParentAbilitySeqSource != null)
			{
				SequenceManager.Get().OnAnimationEvent(symbol_001D, symbol_000E, symbol_0012, this.ParentAbilitySeqSource);
			}
		}

		internal bool symbol_000Dsymbol_000E(Sequence symbol_001D, ActorData symbol_000E, ActorModelData.ImpulseInfo symbol_0012, ActorModelData.RagdollActivation symbol_0015)
		{
			if (!this.HasSameSequenceSource(symbol_001D))
			{
				return false;
			}
			if (symbol_001D.RequestsHitAnimation(symbol_000E))
			{
				if (this.HitActorsToDeltaHP == null)
				{
					Log.Warning(string.Concat(new object[]
					{
						this,
						" has sequence ",
						symbol_001D,
						" marked Target Hit Animtion, but the ability did not return anything from GatherResults, skipping hit reaction and ragdoll"
					}), new object[0]);
					return true;
				}
				if (!this.HitActorsToDeltaHP.ContainsKey(symbol_000E))
				{
					Log.Warning(string.Concat(new object[]
					{
						this,
						" has sequence ",
						symbol_001D,
						" with target ",
						symbol_000E,
						" but the ability did not return that target from GatherResults, skipping hit reaction and ragdoll"
					}), new object[0]);
					return true;
				}
				ActorModelData actorModelData = symbol_000E.GetActorModelData();
				if (actorModelData != null)
				{
					if (actorModelData.CanPlayDamageReactAnim())
					{
						if (this.turn.symbol_001A(symbol_000E))
						{
							symbol_000E.PlayDamageReactionAnim(symbol_001D.m_customHitReactTriggerName);
						}
					}
				}
				if (symbol_0015 != ActorModelData.RagdollActivation.None)
				{
					if (this.turn.symbol_0004(symbol_000E, 0, -1))
					{
						symbol_000E.DoVisualDeath(symbol_0012);
						if (symbol_001D.Caster != null)
						{
							if (symbol_001D.Caster != symbol_000E)
							{
								if (!symbol_001D.Caster.IsModelAnimatorDisabled())
								{
									if (symbol_001D.Caster.GetTeam() != symbol_000E.GetTeam())
									{
										GameEventManager.CharacterRagdollHitEventArgs args = new GameEventManager.CharacterRagdollHitEventArgs
										{
											m_ragdollingActor = symbol_000E,
											m_triggeringActor = symbol_001D.Caster
										};
										GameEventManager.Get().FireEvent(GameEventManager.EventType.ClientRagdollTriggerHit, args);
									}
								}
							}
						}
					}
				}
			}
			return true;
		}

		private void symbol_000Dsymbol_000E(Animator symbol_001D, bool symbol_000E)
		{
			if (!this.symbol_0012symbol_000E && !ClientResolutionManager.Get().HitsDoneExecuting(this.SeqSource))
			{
				string[] array = new string[7];
				int num = 0;
				string text;
				if (this.ability != null)
				{
					if (this.ability.CurrentAbilityMod != null)
					{
						text = "Mod Id: [" + this.ability.CurrentAbilityMod.m_abilityScopeId + "]\n";
						goto IL_9B;
					}
				}
				text = string.Empty;
				IL_9B:
				array[num] = text;
				array[1] = "Theatrics Entry: ";
				array[2] = this.ToString();
				array[3] = "\n";
				array[4] = this.GetDebugStringAnimationEventsSeen();
				array[5] = this.GetDebugStringDetails(symbol_001D, symbol_000E);
				array[6] = "\n";
				string extraInfo = string.Concat(array);
				ClientResolutionManager.Get().ExecuteUnexecutedActions(this.SeqSource, extraInfo);
				ClientResolutionManager.Get().UpdateLastEventTime();
				this.symbol_0012symbol_000E = true;
			}
		}

		private void no_op_1()
		{
		}

		private void no_op_2()
		{
		}

		private void UpdateLastEventTime()
		{
			if (ClientResolutionManager.Get() != null)
			{
				ClientResolutionManager.Get().UpdateLastEventTime();
			}
		}

		internal float symbol_000Dsymbol_000E(bool symbol_001D)
		{
			ActorData u000D_u000E = this.Actor;
			if (u000D_u000E == null || u000D_u000E.GetActorModelData() == null)
			{
				return 0f;
			}
			return u000D_u000E.GetActorModelData().GetCamStartEventDelay((int)this.animationIndex, symbol_001D);
		}

		internal int GetAnimationIndex()
		{
			return (int)this.animationIndex;
		}

		internal int symbol_000Asymbol_000E()
		{
			int s_invalidActorIndex = ActorData.s_invalidActorIndex;
			if (this.HitActorsToDeltaHP != null)
			{
				if (this.HitActorsToDeltaHP.Count >= 1)
				{
					if (this.HitActorsToDeltaHP.Count <= 2)
					{
						for (int i = 0; i < this.HitActorsToDeltaHP.Count; i++)
						{
							ActorData actorData = this.HitActorsToDeltaHP.Keys.ElementAt(i);
							if (actorData != null)
							{
								if (this.Actor != null)
								{
									if (this.Actor.GetTeam() != actorData.GetTeam())
									{
										if (s_invalidActorIndex != ActorData.s_invalidActorIndex)
										{
											s_invalidActorIndex = ActorData.s_invalidActorIndex;
											break;
										}
										s_invalidActorIndex = actorData.ActorIndex;
									}
									else if (actorData != this.Actor)
									{
										s_invalidActorIndex = ActorData.s_invalidActorIndex;
										break;
									}
								}
							}
						}
					}
				}
			}
			return s_invalidActorIndex;
		}

		public int CompareTo(ActorAnimation rhs)
		{
			if (rhs == null)
			{
				return 1;
			}
			if (object.ReferenceEquals(this, rhs))
			{
				return 0;
			}
			if (!(this.ability == null))
			{
				if (rhs.ability == null)
				{
				}
				else
				{
					if (this.ability.RunPriority != rhs.ability.RunPriority)
					{
						return this.ability.RunPriority.CompareTo(rhs.ability.RunPriority);
					}
					if ((int)this.playOrderIndex != (int)rhs.playOrderIndex)
					{
						return this.playOrderIndex.CompareTo(rhs.playOrderIndex);
					}
					bool flag = GameFlowData.Get().IsActorDataOwned(this.Actor);
					bool flag2 = GameFlowData.Get().IsActorDataOwned(rhs.Actor);
					if (!this.ability.IsFreeAction())
					{
						if (rhs.ability.IsFreeAction())
						{
						}
						else
						{
							if (flag != flag2)
							{
								return flag.CompareTo(flag2);
							}
							if (this.Actor.ActorIndex != rhs.Actor.ActorIndex)
							{
								return this.Actor.ActorIndex.CompareTo(rhs.Actor.ActorIndex);
							}
							if (this.animationIndex != rhs.animationIndex)
							{
								return this.animationIndex.CompareTo(rhs.animationIndex);
							}
							return 0;
						}
					}
					return -1 * this.ability.IsFreeAction().CompareTo(rhs.ability.IsFreeAction());
				}
			}
			if (this.ability == null)
			{
				if (rhs.ability == null)
				{
					return 0;
				}
			}
			if (this.ability != null)
			{
				if (this.ability.IsFreeAction())
				{
					return -1;
				}
			}
			if (rhs.ability != null)
			{
				if (rhs.ability.IsFreeAction())
				{
					return 1;
				}
			}
			return (!(this.ability == null)) ? 1 : -1;
		}

		private void DebugLogHung(Animator animator, bool movementPathDone)
		{
			Log.Error("Theatrics: {0} {1} is hung", new object[]
			{
				this.Actor.DisplayName,
				this.GetDebugStringDetails(animator, movementPathDone)
			});
		}

		public string GetDebugStringDetails(Animator animator, bool movementPathDone)
		{
			string result = string.Empty;
			if (animator != null && this.Actor.GetActorModelData() != null)
			{
				int integer = animator.GetInteger("Attack");
				bool @bool = animator.GetBool("Cover");
				float @float = animator.GetFloat("DistToGoal");
				int integer2 = animator.GetInteger("NextLinkType");
				int integer3 = animator.GetInteger("CurLinkType");
				bool bool2 = animator.GetBool("CinematicCam");
				bool bool3 = animator.GetBool("DecisionPhase");
				bool flag = animator.GetCurrentAnimatorStateInfo(0).IsName("Damage");
				if (flag)
				{
					object[] array = new object[9];
					array[0] = "\nIn ability animation state for ";
					array[1] = this.Actor.GetDebugName();
					array[2] = " while Damage flag is set (hit react.). Code error, show Chris. debug info: (state: ";
					array[3] = this.State.ToString();
					array[4] = ", Attack: ";
					array[5] = integer;
					array[6] = ", ability: ";
					int num = 7;
					object obj;
					if (this.ability == null)
					{
						obj = "NULL";
					}
					else
					{
						obj = this.ability.GetActionAnimType().ToString();
					}
					array[num] = obj;
					array[8] = ")";
					result = string.Concat(array);
				}
				else
				{
					object[] array2 = new object[0x23];
					array2[0] = "\nIn animation state ";
					array2[1] = this.Actor.GetActorModelData().GetCurrentAnimatorStateName();
					array2[2] = " for ";
					array2[3] = this.symbol_001C;
					array2[4] = " sec.\nAfter a request for ability ";
					int num2 = 5;
					object obj2;
					if (this.ability == null)
					{
						obj2 = "NULL";
					}
					else
					{
						obj2 = this.ability.m_abilityName;
					}
					array2[num2] = obj2;
					array2[6] = ".\nParameters [Attack: ";
					array2[7] = integer;
					array2[8] = ", Cover: ";
					array2[9] = @bool;
					array2[0xA] = ", DistToGoal: ";
					array2[0xB] = @float;
					array2[0xC] = ", NextLinkType: ";
					array2[0xD] = integer2;
					array2[0xE] = ", CurLinkType: ";
					array2[0xF] = integer3;
					array2[0x10] = ", CinematicCam: ";
					array2[0x11] = bool2;
					array2[0x12] = ", DecisionPhase: ";
					array2[0x13] = bool3;
					array2[0x14] = "].\nDetails [state: ";
					array2[0x15] = this.State.ToString();
					array2[0x16] = ", actor state: ";
					array2[0x17] = this.Actor.GetActorTurnSM().CurrentState.ToString();
					array2[0x18] = ", movement path done: ";
					array2[0x19] = movementPathDone;
					array2[0x1A] = ", ability anim: ";
					int num3 = 0x1B;
					object obj3;
					if (this.ability == null)
					{
						obj3 = "NULL";
					}
					else
					{
						obj3 = this.ability.GetActionAnimType().ToString();
					}
					array2[num3] = obj3;
					array2[0x1C] = ", ability anim played: ";
					array2[0x1D] = this.symbol_001A;
					array2[0x1E] = ", time: ";
					array2[0x1F] = GameTime.time;
					array2[0x20] = ", turn: ";
					array2[0x21] = GameFlowData.Get().CurrentTurn;
					array2[0x22] = "]";
					result = string.Concat(array2);
				}
			}
			else if (NetworkServer.active)
			{
				if (!NetworkClient.active)
				{
					object[] array3 = new object[8];
					array3[0] = "\nIn ability animation state for ";
					array3[1] = this.Actor.GetDebugName();
					array3[2] = ", ability: ";
					int num4 = 3;
					object obj4;
					if (this.ability == null)
					{
						obj4 = "NULL";
					}
					else
					{
						obj4 = this.ability.GetActionAnimType().ToString();
					}
					array3[num4] = obj4;
					array3[4] = ", time: ";
					array3[5] = GameTime.time;
					array3[6] = ", turn: ";
					array3[7] = GameFlowData.Get().CurrentTurn;
					result = string.Concat(array3);
				}
			}
			return result;
		}

		public string GetDebugStringAnimationEventsSeen()
		{
			string text = "Animation Events Seen:\n";
			for (int i = 0; i < this.symbol_0018symbol_000E.Count; i++)
			{
				if (this.symbol_0018symbol_000E[i] != null)
				{
					text = text + "    [ " + this.symbol_0018symbol_000E[i] + " ]\n";
				}
			}
			return text;
		}

		public override string ToString()
		{
			object[] array = new object[0xD];
			array[0] = "[ActorAnimation: ";
			array[1] = ((!(this.Actor == null)) ? this.Actor.GetDebugName() : "(NULL caster)");
			array[2] = " ";
			int num = 3;
			object obj;
			if (this.ability == null)
			{
				obj = "(NULL ability)";
			}
			else
			{
				obj = this.ability.m_abilityName;
			}
			array[num] = obj;
			array[4] = ", animation index: ";
			array[5] = this.animationIndex;
			array[6] = ", play order index: ";
			array[7] = this.playOrderIndex;
			array[8] = ", group index: ";
			array[9] = this.groupIndex;
			array[0xA] = ", state: ";
			array[0xB] = this.State;
			array[0xC] = "]";
			return string.Concat(array);
		}

		public string symbol_000Dsymbol_000E(string symbol_001D = "")
		{
			string[] array = new string[5];
			array[0] = "[ActorAnimation: ";
			int num = 1;
			string text;
			if (this.Actor == null)
			{
				text = "(NULL caster)";
			}
			else
			{
				text = this.Actor.GetDebugName();
			}
			array[num] = text;
			array[2] = ", ";
			array[3] = ((!(this.ability == null)) ? this.ability.m_abilityName : "(NULL ability)");
			array[4] = "]";
			string text2 = string.Concat(array);
			if (symbol_001D.Length > 0)
			{
				text2 = string.Concat(new string[]
				{
					"<color=",
					symbol_001D,
					">",
					text2,
					"</color>"
				});
			}
			return text2;
		}

		internal enum PlaybackState
		{
			symbol_001D,
			symbol_000E,
			symbol_0012,
			symbol_0015,
			symbol_0016,
			symbol_0013,
			symbol_0018
		}
	}
}

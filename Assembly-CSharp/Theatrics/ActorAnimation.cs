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
		public const float \u001D = 3f;

		private short animationIndex;

		private Vector3 \u0012;

		private bool \u0015;

		private int tauntNumber;

		private bool \u0013;

		private bool \u0018;

		private bool \u0009;

		private AbilityData.ActionType \u0019 = AbilityData.ActionType.INVALID_ACTION;

		private bool \u0011;

		private bool \u001A;

		private bool \u0004;

		private Ability ability;

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private SequenceSource \u0003;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private SequenceSource \u000F;

		internal int actorIndex = ActorData.s_invalidActorIndex;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private Dictionary<ActorData, int> \u000D;

		internal bool cinematicCamera;

		internal int tauntAnimIndex;

		internal sbyte playOrderIndex;

		internal sbyte groupIndex;

		internal Bounds \u0020;

		private List<byte> \u000C = new List<byte>();

		private List<byte> \u0014 = new List<byte>();

		private bool \u0005;

		private AbilityRequest \u001B;

		private Turn turn;

		private bool \u0001;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private bool \u001F;

		private bool \u0010;

		private float \u0007;

		private float \u001C;

		private float \u001D\u000E;

		private float \u000E\u000E;

		private bool \u0012\u000E;

		private float \u0015\u000E;

		private bool \u0016\u000E;

		internal Bounds \u0013\u000E;

		private List<string> \u0018\u000E = new List<string>();

		private ActorAnimation.PlaybackState playbackSate;

		private static readonly int DistToGoalHash = Animator.StringToHash("DistToGoal");

		private static readonly int StartDamageReactionHash = Animator.StringToHash("StartDamageReaction");

		private static readonly int AttackHash = Animator.StringToHash("Attack");

		private static readonly int CinematicCamHash = Animator.StringToHash("CinematicCam");

		private static readonly int TauntNumberHash = Animator.StringToHash("TauntNumber");

		private static readonly int TauntAnimIndexHash = Animator.StringToHash("TauntAnimIndex");

		private static readonly int StartAttackHash = Animator.StringToHash("StartAttack");

		private const float \u0017\u000E = 1f;

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
					for (;;)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!true)
					{
						RuntimeMethodHandle runtimeMethodHandle = methodof(ActorAnimation.get_\u000D\u000E()).MethodHandle;
					}
				}
				return null;
			}
			set
			{
				if (value == null)
				{
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!true)
					{
						RuntimeMethodHandle runtimeMethodHandle = methodof(ActorAnimation.set_\u000D\u000E(ActorData)).MethodHandle;
					}
					if (this.actorIndex != ActorData.s_invalidActorIndex)
					{
						for (;;)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							break;
						}
						this.actorIndex = ActorData.s_invalidActorIndex;
						return;
					}
				}
				if (value != null)
				{
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
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
						for (;;)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
						if (!true)
						{
							RuntimeMethodHandle runtimeMethodHandle = methodof(ActorAnimation.set_\u000D\u000E(ActorAnimation.PlaybackState)).MethodHandle;
						}
						if (value == ActorAnimation.PlaybackState.\u0012)
						{
							for (;;)
							{
								switch (5)
								{
								case 0:
									continue;
								}
								break;
							}
							int num = AbilityUtils.GetTechPointRewardForInteraction(this.ability, AbilityInteractionType.Cast, true, false, false);
							num = AbilityUtils.CalculateTechPointsForTargeter(this.Actor, this.ability, num);
							if (num > 0)
							{
								this.Actor.AddCombatText(num.ToString(), string.Empty, CombatTextCategory.TP_Recovery, BuffIconToDisplay.None);
								bool flag = ClientResolutionManager.Get().IsInResolutionState();
								if (flag)
								{
									for (;;)
									{
										switch (6)
										{
										case 0:
											continue;
										}
										break;
									}
									this.Actor.ClientUnresolvedTechPointGain += num;
								}
							}
							if (this.ability.GetModdedCost() > 0)
							{
								for (;;)
								{
									switch (2)
									{
									case 0:
										continue;
									}
									break;
								}
								if (this.Actor.ReservedTechPoints > 0)
								{
									for (;;)
									{
										switch (3)
										{
										case 0:
											continue;
										}
										break;
									}
									int num2 = this.Actor.ClientReservedTechPoints - this.ability.GetModdedCost();
									num2 = Mathf.Max(num2, -this.Actor.ReservedTechPoints);
									this.Actor.ClientReservedTechPoints = num2;
								}
							}
						}
					}
					if (TheatricsManager.DebugLog)
					{
						for (;;)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
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
					if (value != ActorAnimation.PlaybackState.\u0013)
					{
						for (;;)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
						if (value != ActorAnimation.PlaybackState.\u0018)
						{
							goto IL_1C8;
						}
						for (;;)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
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
				return this.State >= ActorAnimation.PlaybackState.\u0012;
			}
		}

		private bool StartFinalPlaybackState()
		{
			if (this.Actor == null)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(ActorAnimation.StartFinalPlaybackState()).MethodHandle;
				}
				Log.Error("Theatrics: can't start {0} since the actor can no longer be found. Was the actor destroyed during resolution?", new object[]
				{
					this
				});
				this.State = ActorAnimation.PlaybackState.\u0018;
				return false;
			}
			return this.State == ActorAnimation.PlaybackState.\u0018;
		}

		internal void OnSerializeHelper(IBitStream \u001D)
		{
			sbyte b = (sbyte)this.animationIndex;
			sbyte b2 = (sbyte)this.\u0019;
			float x = this.\u0012.x;
			float z = this.\u0012.z;
			sbyte s_invalidActorIndex;
			if (this.Actor == null)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(ActorAnimation.OnSerializeHelper(IBitStream)).MethodHandle;
				}
				s_invalidActorIndex = (sbyte)ActorData.s_invalidActorIndex;
			}
			else
			{
				s_invalidActorIndex = (sbyte)this.Actor.ActorIndex;
			}
			sbyte b3 = s_invalidActorIndex;
			bool flag = this.cinematicCamera;
			sbyte b4 = (sbyte)this.tauntNumber;
			bool u = this.\u0013;
			bool u2 = this.\u0018;
			bool u3 = this.\u0009;
			bool u4 = this.\u0015;
			sbyte b5 = this.playOrderIndex;
			sbyte b6 = this.groupIndex;
			Vector3 center = this.\u0020.center;
			Vector3 size = this.\u0020.size;
			byte b7 = checked((byte)this.\u000C.Count);
			\u001D.Serialize(ref b);
			\u001D.Serialize(ref b2);
			\u001D.Serialize(ref x);
			\u001D.Serialize(ref z);
			\u001D.Serialize(ref b3);
			\u001D.Serialize(ref flag);
			\u001D.Serialize(ref b4);
			\u001D.Serialize(ref u);
			\u001D.Serialize(ref u2);
			\u001D.Serialize(ref u3);
			\u001D.Serialize(ref u4);
			\u001D.Serialize(ref b5);
			\u001D.Serialize(ref b6);
			short num = (short)Mathf.RoundToInt(center.x);
			short num2 = (short)Mathf.RoundToInt(center.z);
			\u001D.Serialize(ref num);
			\u001D.Serialize(ref num2);
			if (\u001D.isReading)
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				center.x = (float)num;
				center.y = 1.5f + (float)Board.Get().BaselineHeight;
				center.z = (float)num2;
			}
			short num3 = (short)Mathf.CeilToInt(size.x + 0.5f);
			short num4 = (short)Mathf.CeilToInt(size.z + 0.5f);
			\u001D.Serialize(ref num3);
			\u001D.Serialize(ref num4);
			if (\u001D.isReading)
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				size.x = (float)num3;
				size.y = 3f;
				size.z = (float)num4;
			}
			\u001D.Serialize(ref b7);
			if (\u001D.isReading)
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				for (int i = 0; i < (int)b7; i++)
				{
					byte item = 0;
					byte item2 = 0;
					\u001D.Serialize(ref item);
					\u001D.Serialize(ref item2);
					this.\u000C.Add(item);
					this.\u0014.Add(item2);
				}
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			else
			{
				for (int j = 0; j < (int)b7; j++)
				{
					byte b8 = this.\u000C[j];
					byte b9 = this.\u0014[j];
					\u001D.Serialize(ref b8);
					\u001D.Serialize(ref b9);
				}
			}
			this.animationIndex = (short)b;
			if (\u001D.isReading)
			{
				this.\u0012 = new Vector3(x, (float)Board.Get().BaselineHeight, z);
			}
			this.actorIndex = (int)b3;
			this.cinematicCamera = flag;
			this.tauntNumber = (int)b4;
			this.\u0013 = u;
			this.\u0018 = u2;
			this.\u0009 = u3;
			this.\u0015 = u4;
			this.playOrderIndex = b5;
			this.groupIndex = b6;
			this.\u0020 = new Bounds(center, size);
			this.\u0019 = (AbilityData.ActionType)b2;
			this.ability = ((!(this.Actor == null)) ? this.Actor.GetAbilityData().GetAbilityOfActionType(this.\u0019) : null);
			if (this.SeqSource == null)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				this.SeqSource = new SequenceSource();
			}
			this.SeqSource.OnSerializeHelper(\u001D);
			if (\u001D.isWriting)
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				bool flag2 = this.ParentAbilitySeqSource != null;
				\u001D.Serialize(ref flag2);
				if (flag2)
				{
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					this.ParentAbilitySeqSource.OnSerializeHelper(\u001D);
				}
			}
			if (\u001D.isReading)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				bool flag3 = false;
				\u001D.Serialize(ref flag3);
				if (flag3)
				{
					for (;;)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					if (this.ParentAbilitySeqSource == null)
					{
						this.ParentAbilitySeqSource = new SequenceSource();
					}
					this.ParentAbilitySeqSource.OnSerializeHelper(\u001D);
				}
				else
				{
					this.ParentAbilitySeqSource = null;
				}
			}
			sbyte b10;
			if (this.HitActorsToDeltaHP == null)
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				b10 = 0;
			}
			else
			{
				b10 = (sbyte)this.HitActorsToDeltaHP.Count;
			}
			sbyte b11 = b10;
			\u001D.Serialize(ref b11);
			if ((int)b11 > 0)
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				if (this.HitActorsToDeltaHP == null)
				{
					this.HitActorsToDeltaHP = new Dictionary<ActorData, int>();
				}
			}
			if (\u001D.isWriting)
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
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
								for (;;)
								{
									switch (4)
									{
									case 0:
										continue;
									}
									break;
								}
								s_invalidActorIndex2 = (sbyte)ActorData.s_invalidActorIndex;
							}
							else
							{
								s_invalidActorIndex2 = (sbyte)keyValuePair.Key.ActorIndex;
							}
							sbyte b12 = s_invalidActorIndex2;
							if ((int)b12 != ActorData.s_invalidActorIndex)
							{
								for (;;)
								{
									switch (3)
									{
									case 0:
										continue;
									}
									break;
								}
								sbyte b13 = 0;
								if (keyValuePair.Value > 0)
								{
									for (;;)
									{
										switch (1)
										{
										case 0:
											continue;
										}
										break;
									}
									b13 = 1;
								}
								else if (keyValuePair.Value < 0)
								{
									for (;;)
									{
										switch (3)
										{
										case 0:
											continue;
										}
										break;
									}
									b13 = -1;
								}
								\u001D.Serialize(ref b12);
								\u001D.Serialize(ref b13);
							}
						}
						for (;;)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							break;
						}
					}
					goto IL_64D;
				}
			}
			if (\u001D.isReading)
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				if (this.HitActorsToDeltaHP != null)
				{
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					this.HitActorsToDeltaHP.Clear();
				}
				for (int k = 0; k < (int)b11; k++)
				{
					sbyte b14 = (sbyte)ActorData.s_invalidActorIndex;
					sbyte b15 = 0;
					\u001D.Serialize(ref b14);
					\u001D.Serialize(ref b15);
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

		internal bool \u0002\u000E()
		{
			return this.tauntNumber > 0;
		}

		internal bool GetSymbol0013()
		{
			return this.\u0013;
		}

		internal bool \u0006\u000E()
		{
			return this.\u0015;
		}

		internal bool IsActorDamaged(ActorData actor)
		{
			if (this.HitActorsToDeltaHP != null)
			{
				if (this.HitActorsToDeltaHP.ContainsKey(actor))
				{
					return this.HitActorsToDeltaHP[actor] < 0;
				}
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(ActorAnimation.IsActorDamaged(ActorData)).MethodHandle;
				}
			}
			return false;
		}

		internal bool \u0020\u000E()
		{
			bool result;
			if (!this.Actor.IsDead())
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(ActorAnimation.\u0020\u000E()).MethodHandle;
				}
				if (!this.\u0009)
				{
					for (;;)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
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

		internal bool \u000C\u000E()
		{
			FogOfWar clientFog = FogOfWar.GetClientFog();
			ActorStatus actorStatus = this.Actor.GetActorStatus();
			bool flag = this.\u0020\u000E();
			if (actorStatus != null)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(ActorAnimation.\u000C\u000E()).MethodHandle;
				}
				if (actorStatus.HasStatus(StatusType.Revealed, true))
				{
					goto IL_78;
				}
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			bool flag2;
			if (!this.Actor.VisibleTillEndOfPhase)
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
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
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (clientFog == null)
				{
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
				}
				else
				{
					if (this.\u0018)
					{
						for (;;)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
						return false;
					}
					if (NetworkClient.active && GameFlowData.Get() != null)
					{
						for (;;)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
						if (GameFlowData.Get().LocalPlayerData != null)
						{
							for (;;)
							{
								switch (1)
								{
								case 0:
									continue;
								}
								break;
							}
							if (this.Actor.SomeVisibilityCheckB_zq(GameFlowData.Get().LocalPlayerData, true, false))
							{
								for (;;)
								{
									switch (7)
									{
									case 0:
										continue;
									}
									break;
								}
								return true;
							}
						}
					}
					for (int i = 0; i < this.\u000C.Count; i++)
					{
						BoardSquare boardSquare = Board.Get().GetBoardSquare((int)this.\u000C[i], (int)this.\u0014[i]);
						if (boardSquare != null && clientFog.IsVisible(boardSquare))
						{
							for (;;)
							{
								switch (6)
								{
								case 0:
									continue;
								}
								break;
							}
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
						for (;;)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
						if (Board.Get() != null)
						{
							for (;;)
							{
								switch (5)
								{
								case 0:
									continue;
								}
								break;
							}
							using (Dictionary<ActorData, int>.Enumerator enumerator = this.HitActorsToDeltaHP.GetEnumerator())
							{
								while (enumerator.MoveNext())
								{
									KeyValuePair<ActorData, int> keyValuePair = enumerator.Current;
									ActorData key = keyValuePair.Key;
									if (key == null)
									{
										for (;;)
										{
											switch (5)
											{
											case 0:
												continue;
											}
											break;
										}
									}
									else
									{
										BoardSquare boardSquare = Board.Get().GetBoardSquare(key.transform.position);
										if (clientFog.IsVisible(boardSquare))
										{
											for (;;)
											{
												switch (3)
												{
												case 0:
													continue;
												}
												break;
											}
											return false;
										}
									}
								}
								for (;;)
								{
									switch (5)
									{
									case 0:
										continue;
									}
									break;
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
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(ActorAnimation.HasSameSequenceSource(Sequence)).MethodHandle;
				}
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
			if (this.State != ActorAnimation.PlaybackState.\u001D)
			{
				return false;
			}
			bool flag = !ClientResolutionManager.Get().IsWaitingForActionMessages(abilityPriority);
			if (!flag)
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(ActorAnimation.IsReadyToPlay_zq(AbilityPriority, bool)).MethodHandle;
				}
				if (logErrorIfNotReady)
				{
					for (;;)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					Log.Error("sequences not ready, current client resolution state = {0}", new object[]
					{
						ClientResolutionManager.Get().GetCurrentStateName()
					});
				}
			}
			return flag;
		}

		internal bool \u0014\u000E()
		{
			return this.State != ActorAnimation.PlaybackState.\u0018 && this.State != ActorAnimation.PlaybackState.\u0013;
		}

		internal bool \u0005\u000E()
		{
			int result;
			if (this.State >= ActorAnimation.PlaybackState.\u0015)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(ActorAnimation.\u0005\u000E()).MethodHandle;
				}
				if (!this.\u0001)
				{
					for (;;)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!this.AnimationFinished)
					{
						goto IL_D7;
					}
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
				}
				if (this.\u0007 > 0f)
				{
					for (;;)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					if (NetworkClient.active)
					{
						for (;;)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							break;
						}
						if (GameTime.time < this.\u0007 + this.CalcFrameTimeAfterAllHitsButMine())
						{
							goto IL_D7;
						}
						for (;;)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
					}
					if (NetworkClient.active)
					{
						if (this.\u000E\u000E > 0f)
						{
							for (;;)
							{
								switch (7)
								{
								case 0:
									continue;
								}
								break;
							}
							result = ((GameTime.time >= this.\u000E\u000E + 1f) ? 1 : 0);
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

		internal bool \u0008\u000E(ActorData \u001D)
		{
			if (this.HitActorsToDeltaHP != null && this.HitActorsToDeltaHP.ContainsKey(\u001D))
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(ActorAnimation.\u0008\u000E(ActorData)).MethodHandle;
				}
				if (this.HitActorsToDeltaHP[\u001D] != 0)
				{
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!SequenceSource.DidSequenceHit(this.SeqSource, \u001D))
					{
						for (;;)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
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
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(ActorAnimation.OtherActorsInHitActorsToDeltaHPNum()).MethodHandle;
				}
				result = 0;
			}
			else
			{
				int count = this.HitActorsToDeltaHP.Count;
				int num;
				if (this.HitActorsToDeltaHP.ContainsKey(this.Actor))
				{
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
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

		internal void method000D000E(Turn \u001D)
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
			this.\u000E\u000E = GameTime.time;
			if (this.State == ActorAnimation.PlaybackState.\u0018)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(ActorAnimation.method000D000E(Turn)).MethodHandle;
				}
				return;
			}
			bool flag;
			if (this.ability != null)
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
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
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					this.Actor.TurnToPositionInstant(this.\u0012);
				}
				else
				{
					this.Actor.TurnToPosition(this.\u0012, 0.2f);
				}
			}
			Animator modelAnimator = this.Actor.GetModelAnimator();
			if (modelAnimator != null)
			{
				float num;
				if (this.ability == null)
				{
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					num = 0f;
				}
				else
				{
					if (this.ability.GetMovementType() != ActorData.MovementType.None)
					{
						for (;;)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
						if (this.ability.GetMovementType() != ActorData.MovementType.Knockback)
						{
							for (;;)
							{
								switch (5)
								{
								case 0:
									continue;
								}
								break;
							}
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
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				GameEventManager.Get().FireEvent(GameEventManager.EventType.CardUsed, new GameEventManager.CardUsedArgs
				{
					userActor = this.Actor
				});
				if (HUD_UI.Get() != null)
				{
					HUD_UI.Get().m_mainScreenPanel.m_abilityBar.m_theTimer.m_abilityUsedTracker.AddNewAbility(this.GetAbility(), this.Actor);
				}
			}
			else if (!this.\u0015)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				GameEventManager.Get().FireEvent(GameEventManager.EventType.AbilityUsed, new GameEventManager.AbilityUseArgs
				{
					ability = this.ability,
					userActor = this.Actor
				});
			}
			if (this.animationIndex <= 0)
			{
				this.State = ActorAnimation.PlaybackState.\u0016;
				this.\u001A = true;
				this.AnimationFinished = true;
			}
			if (NetworkServer.active)
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (NetworkClient.active)
				{
					for (;;)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					SequenceManager.Get().DoClientEnable(this.SeqSource);
				}
			}
			else
			{
				SequenceManager.Get().DoClientEnable(this.SeqSource);
			}
			bool flag3 = this.\u0020\u000E();
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
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				this.State = ActorAnimation.PlaybackState.\u0016;
				this.\u001A = true;
				this.AnimationFinished = true;
				this.no_op_2();
				this.UpdateLastEventTime();
			}
			else
			{
				modelAnimator.SetInteger(ActorAnimation.AttackHash, (int)this.animationIndex);
				modelAnimator.SetBool(ActorAnimation.CinematicCamHash, this.cinematicCamera);
				if (this.\u000D\u000E(modelAnimator, "TauntNumber"))
				{
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					modelAnimator.SetInteger(ActorAnimation.TauntNumberHash, this.tauntNumber);
				}
				modelAnimator.SetTrigger(ActorAnimation.StartAttackHash);
				if (this.Actor.GetActorModelData().HasAnimatorControllerParamater("TauntAnimIndex"))
				{
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					modelAnimator.SetInteger(ActorAnimation.TauntAnimIndexHash, this.tauntAnimIndex);
				}
				if (this.ability != null)
				{
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					this.ability.OnAbilityAnimationRequest(this.Actor, (int)this.animationIndex, this.cinematicCamera, this.\u0012);
				}
				if (HUD_UI.Get() != null)
				{
					for (;;)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					HUD_UI.Get().m_mainScreenPanel.m_abilityBar.m_theTimer.m_abilityUsedTracker.AddNewAbility(this.GetAbility(), this.Actor);
				}
				if (this.\u0002\u000E())
				{
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					ChatterManager.Get().CancelActiveChatter();
				}
				CameraManager.Get().OnAbilityAnimationStart(this.Actor, (int)this.animationIndex, this.\u0012, this.cinematicCamera, this.tauntNumber);
				if (this.Actor != null && this.cinematicCamera)
				{
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					if (NetworkClient.active)
					{
						for (;;)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							break;
						}
						this.Actor.ForceUpdateIsVisibleToClientCache();
					}
				}
				if (this.cinematicCamera)
				{
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					if (this.tauntNumber > 0)
					{
					}
				}
				this.no_op_1();
				this.UpdateLastEventTime();
				this.State = ActorAnimation.PlaybackState.\u0012;
			}
			if (Application.isEditor)
			{
				if (!CameraManager.CamDebugTraceOn)
				{
					for (;;)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!TheatricsManager.DebugLog)
					{
						return;
					}
				}
				ActorDebugUtils.\u001D(this.\u0013\u000E, Color.green, 3f);
			}
		}

		internal bool \u000D\u000E(Animator \u001D, string \u000E)
		{
			if (\u001D != null)
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(ActorAnimation.\u000D\u000E(Animator, string)).MethodHandle;
				}
				if (\u001D.parameters != null)
				{
					for (int i = 0; i < \u001D.parameterCount; i++)
					{
						if (\u001D.parameters[i].name == \u000E)
						{
							for (;;)
							{
								switch (7)
								{
								case 0:
									continue;
								}
								break;
							}
							return true;
						}
					}
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
				}
			}
			return false;
		}

		internal bool \u000D\u000E(Turn \u001D)
		{
			if (NetworkClient.active)
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(ActorAnimation.\u000D\u000E(Turn)).MethodHandle;
				}
				if (!(this.Actor == null))
				{
					if (!(this.Actor.GetActorModelData() == null))
					{
						goto IL_54;
					}
					for (;;)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
				}
				this.State = ActorAnimation.PlaybackState.\u0018;
			}
			IL_54:
			Animator animator = null;
			if (NetworkClient.active)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (this.State != ActorAnimation.PlaybackState.\u0018)
				{
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					animator = this.Actor.GetModelAnimator();
					if (animator == null)
					{
						this.State = ActorAnimation.PlaybackState.\u0018;
					}
					else if (!animator.enabled && this.animationIndex > 0)
					{
						for (;;)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							break;
						}
						this.State = ActorAnimation.PlaybackState.\u0018;
					}
				}
			}
			if (this.State == ActorAnimation.PlaybackState.\u0018)
			{
				return false;
			}
			ActorMovement actorMovement = this.Actor.GetActorMovement();
			bool flag = !actorMovement.AmMoving();
			if (this.State >= ActorAnimation.PlaybackState.\u0012 && this.State < ActorAnimation.PlaybackState.\u0013)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				bool flag2 = this.\u001C > 12f;
				if (flag2)
				{
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!this.\u0011)
					{
						for (;;)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							break;
						}
						this.\u0011 = true;
						this.DebugLogHung(animator, flag);
					}
				}
				this.\u001C += GameTime.deltaTime;
				if (NetworkClient.active)
				{
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					if (this.State >= ActorAnimation.PlaybackState.\u0016)
					{
						for (;;)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
						if (!this.\u0012\u000E)
						{
							for (;;)
							{
								switch (3)
								{
								case 0:
									continue;
								}
								break;
							}
							if (this.\u001D\u000E < 7f)
							{
								for (;;)
								{
									switch (2)
									{
									case 0:
										continue;
									}
									break;
								}
								if (!flag2)
								{
									goto IL_1C5;
								}
								for (;;)
								{
									switch (1)
									{
									case 0:
										continue;
									}
									break;
								}
							}
							this.\u000D\u000E(animator, flag);
						}
						IL_1C5:
						this.\u001D\u000E += GameTime.deltaTime;
					}
				}
			}
			bool flag3 = this.\u001C > 15f;
			if (flag3)
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!this.\u0004)
				{
					this.\u0004 = true;
					Log.Error("Theatrics: animation timed out for {0} {1} after {2} seconds.", new object[]
					{
						this.Actor.DisplayName,
						(!(this.ability != null)) ? (" animation index " + this.animationIndex.ToString()) : this.ability.ToString(),
						this.\u001C
					});
				}
			}
			bool flag4 = false;
			bool flag5 = false;
			if (animator)
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				flag4 = this.Actor.GetActorModelData().IsPlayingAttackAnim(out flag5);
			}
			if (flag4)
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				this.\u001A = true;
				this.\u0015\u000E += GameTime.deltaTime;
			}
			bool animationFinished;
			if (this.\u001A)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				animationFinished = !flag4;
			}
			else
			{
				animationFinished = false;
			}
			this.AnimationFinished = animationFinished;
			if (this.State >= ActorAnimation.PlaybackState.\u0012)
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				if (this.State < ActorAnimation.PlaybackState.\u0013)
				{
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					if (this.\u0007 == 0f)
					{
						if (!this.\u0015)
						{
							for (;;)
							{
								switch (5)
								{
								case 0:
									continue;
								}
								break;
							}
							if (!ClientResolutionManager.Get().HitsDoneExecuting(this.SeqSource))
							{
								goto IL_398;
							}
							for (;;)
							{
								switch (3)
								{
								case 0:
									continue;
								}
								break;
							}
						}
						this.\u0007 = GameTime.time;
						if (TheatricsManager.DebugLog)
						{
							for (;;)
							{
								switch (7)
								{
								case 0:
									continue;
								}
								break;
							}
							TheatricsManager.LogForDebugging(this.ToString() + " hits done");
						}
					}
				}
			}
			IL_398:
			bool flag6;
			if (!this.\u0012\u000E)
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (this.\u0007 > 0f)
				{
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					flag6 = (GameTime.time - this.\u0007 >= this.CalcFrameTimeAfterAllHitsButMine());
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
			case ActorAnimation.PlaybackState.\u001D:
				return true;
			case ActorAnimation.PlaybackState.\u0012:
				if (!flag4)
				{
					if (this.\u001C < 5f)
					{
						for (;;)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
						return true;
					}
					this.State = ActorAnimation.PlaybackState.\u0016;
					this.AnimationFinished = true;
					this.\u000D\u000E(animator, flag);
				}
				if (animator != null)
				{
					for (;;)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					animator.SetInteger(ActorAnimation.AttackHash, 0);
					animator.SetBool(ActorAnimation.CinematicCamHash, false);
				}
				if (this.ability != null)
				{
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					this.ability.OnAbilityAnimationRequestProcessed(this.Actor);
				}
				if (this.State < ActorAnimation.PlaybackState.\u0016)
				{
					for (;;)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					this.State = ActorAnimation.PlaybackState.\u0015;
				}
				this.no_op_1();
				this.no_op_2();
				if (ClientResolutionManager.Get() != null)
				{
					ClientResolutionManager.Get().OnAbilityCast(this.Actor, this.ability);
					ClientResolutionManager.Get().UpdateLastEventTime();
				}
				break;
			case ActorAnimation.PlaybackState.\u0015:
				if (flag4)
				{
					if (!this.\u0001)
					{
						break;
					}
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
				}
				this.State = ActorAnimation.PlaybackState.\u0016;
				this.UpdateLastEventTime();
				break;
			case ActorAnimation.PlaybackState.\u0013:
				return false;
			}
			if (!flag)
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!this.\u0010)
				{
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!actorMovement.OnPathType(BoardSquarePathInfo.ConnectionType.Charge))
					{
						for (;;)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
						if (!actorMovement.OnPathType(BoardSquarePathInfo.ConnectionType.Knockback))
						{
							if (!actorMovement.OnPathType(BoardSquarePathInfo.ConnectionType.Flight))
							{
								goto IL_64B;
							}
							for (;;)
							{
								switch (1)
								{
								case 0:
									continue;
								}
								break;
							}
						}
					}
					bool u;
					if (FogOfWar.GetClientFog() != null)
					{
						for (;;)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
						u = FogOfWar.GetClientFog().IsVisible(this.Actor.GetTravelBoardSquare());
					}
					else
					{
						u = false;
					}
					this.\u0010 = u;
					if (this.\u0010)
					{
						Bounds bounds = CameraManager.Get().GetTarget();
						if (\u001D.\u0009)
						{
							for (;;)
							{
								switch (6)
								{
								case 0:
									continue;
								}
								break;
							}
							bounds.Encapsulate(this.\u0020);
						}
						else
						{
							bounds = this.\u0020;
						}
						if (this.Actor.GetActorMovement() != null)
						{
							for (;;)
							{
								switch (7)
								{
								case 0:
									continue;
								}
								break;
							}
							this.Actor.GetActorMovement().EncapsulatePathInBound(ref bounds);
						}
						CameraManager.Get().SetTarget(bounds, false, false);
						\u001D.\u0009 = true;
					}
				}
			}
			IL_64B:
			if (!this.\u0016\u000E && ServerClientUtils.GetCurrentAbilityPhase() == AbilityPriority.Combat_Knockback)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (ClientKnockbackManager.Get() != null)
				{
					for (;;)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					if (flag7 && this.State >= ActorAnimation.PlaybackState.\u0016)
					{
						for (;;)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
						ClientKnockbackManager.Get().NotifyOnActorAnimHitsDone(this.Actor);
						this.\u0016\u000E = true;
					}
				}
			}
			bool flag8;
			if (NetworkClient.active)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (this.\u000E\u000E > 0f)
				{
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!this.GetSymbol0013())
					{
						for (;;)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							break;
						}
						flag8 = (GameTime.time > this.\u000E\u000E + 1f);
						goto IL_708;
					}
				}
			}
			flag8 = true;
			IL_708:
			bool flag9 = flag8;
			if (flag4)
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!this.\u0001)
				{
					if (this.animationIndex > 0)
					{
						goto IL_77E;
					}
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
				}
			}
			if (CameraManager.Get().ShotSequence == null)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (flag)
				{
					for (;;)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					if (flag7)
					{
						for (;;)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							break;
						}
						if (flag9)
						{
							goto IL_78F;
						}
						for (;;)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
					}
				}
			}
			IL_77E:
			if (!flag3)
			{
				return this.\u0014\u000E();
			}
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			IL_78F:
			this.AnimationFinished = true;
			this.State = ActorAnimation.PlaybackState.\u0013;
			this.no_op_1();
			this.UpdateLastEventTime();
			ActorData u000D_u000E = this.Actor;
			if (this.ability != null)
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				this.ability.OnAbilityAnimationReleaseFocus(u000D_u000E);
			}
			if (\u001D.\u0004(u000D_u000E, 0, -1))
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				u000D_u000E.DoVisualDeath(new ActorModelData.ImpulseInfo(u000D_u000E.GetTravelBoardSquareWorldPositionForLos(), Vector3.up));
			}
			return false;
		}

		internal void \u000D\u000E(ActorData \u001D, UnityEngine.Object \u000E, GameObject \u0012)
		{
			if (\u0012 != null)
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(ActorAnimation.\u000D\u000E(ActorData, UnityEngine.Object, GameObject)).MethodHandle;
				}
				if (\u000E.name != null)
				{
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					if (\u000E.name == "CamEndEvent")
					{
						for (;;)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							break;
						}
						this.\u0001 = true;
						if (TheatricsManager.DebugLog)
						{
							TheatricsManager.LogForDebugging("CamEndEvent received for " + this.\u000D\u000E(string.Empty));
						}
						goto IL_98;
					}
				}
			}
			SequenceManager.Get().OnAnimationEvent(\u001D, \u000E, \u0012, this.SeqSource);
			IL_98:
			this.\u0018\u000E.Add(\u000E.name);
		}

		internal void \u0008\u000E(ActorData \u001D, UnityEngine.Object \u000E, GameObject \u0012)
		{
			if (this.ParentAbilitySeqSource != null)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(ActorAnimation.\u0008\u000E(ActorData, UnityEngine.Object, GameObject)).MethodHandle;
				}
				SequenceManager.Get().OnAnimationEvent(\u001D, \u000E, \u0012, this.ParentAbilitySeqSource);
			}
		}

		internal bool \u000D\u000E(Sequence \u001D, ActorData \u000E, ActorModelData.ImpulseInfo \u0012, ActorModelData.RagdollActivation \u0015)
		{
			if (!this.HasSameSequenceSource(\u001D))
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(ActorAnimation.\u000D\u000E(Sequence, ActorData, ActorModelData.ImpulseInfo, ActorModelData.RagdollActivation)).MethodHandle;
				}
				return false;
			}
			if (\u001D.RequestsHitAnimation(\u000E))
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (this.HitActorsToDeltaHP == null)
				{
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					Log.Warning(string.Concat(new object[]
					{
						this,
						" has sequence ",
						\u001D,
						" marked Target Hit Animtion, but the ability did not return anything from GatherResults, skipping hit reaction and ragdoll"
					}), new object[0]);
					return true;
				}
				if (!this.HitActorsToDeltaHP.ContainsKey(\u000E))
				{
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					Log.Warning(string.Concat(new object[]
					{
						this,
						" has sequence ",
						\u001D,
						" with target ",
						\u000E,
						" but the ability did not return that target from GatherResults, skipping hit reaction and ragdoll"
					}), new object[0]);
					return true;
				}
				ActorModelData actorModelData = \u000E.GetActorModelData();
				if (actorModelData != null)
				{
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					if (actorModelData.CanPlayDamageReactAnim())
					{
						for (;;)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
						if (this.turn.\u001A(\u000E))
						{
							for (;;)
							{
								switch (1)
								{
								case 0:
									continue;
								}
								break;
							}
							\u000E.PlayDamageReactionAnim(\u001D.m_customHitReactTriggerName);
						}
					}
				}
				if (\u0015 != ActorModelData.RagdollActivation.None)
				{
					for (;;)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					if (this.turn.\u0004(\u000E, 0, -1))
					{
						for (;;)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
						\u000E.DoVisualDeath(\u0012);
						if (\u001D.Caster != null)
						{
							for (;;)
							{
								switch (4)
								{
								case 0:
									continue;
								}
								break;
							}
							if (\u001D.Caster != \u000E)
							{
								for (;;)
								{
									switch (2)
									{
									case 0:
										continue;
									}
									break;
								}
								if (!\u001D.Caster.IsModelAnimatorDisabled())
								{
									for (;;)
									{
										switch (1)
										{
										case 0:
											continue;
										}
										break;
									}
									if (\u001D.Caster.GetTeam() != \u000E.GetTeam())
									{
										for (;;)
										{
											switch (2)
											{
											case 0:
												continue;
											}
											break;
										}
										GameEventManager.CharacterRagdollHitEventArgs args = new GameEventManager.CharacterRagdollHitEventArgs
										{
											m_ragdollingActor = \u000E,
											m_triggeringActor = \u001D.Caster
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

		private void \u000D\u000E(Animator \u001D, bool \u000E)
		{
			if (!this.\u0012\u000E && !ClientResolutionManager.Get().HitsDoneExecuting(this.SeqSource))
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(ActorAnimation.\u000D\u000E(Animator, bool)).MethodHandle;
				}
				string[] array = new string[7];
				int num = 0;
				string text;
				if (this.ability != null)
				{
					for (;;)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
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
				array[5] = this.GetDebugStringDetails(\u001D, \u000E);
				array[6] = "\n";
				string extraInfo = string.Concat(array);
				ClientResolutionManager.Get().ExecuteUnexecutedActions(this.SeqSource, extraInfo);
				ClientResolutionManager.Get().UpdateLastEventTime();
				this.\u0012\u000E = true;
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
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(ActorAnimation.UpdateLastEventTime()).MethodHandle;
				}
				ClientResolutionManager.Get().UpdateLastEventTime();
			}
		}

		internal float \u000D\u000E(bool \u001D)
		{
			ActorData u000D_u000E = this.Actor;
			if (u000D_u000E == null || u000D_u000E.GetActorModelData() == null)
			{
				return 0f;
			}
			return u000D_u000E.GetActorModelData().GetCamStartEventDelay((int)this.animationIndex, \u001D);
		}

		internal int GetAnimationIndex()
		{
			return (int)this.animationIndex;
		}

		internal int \u000A\u000E()
		{
			int s_invalidActorIndex = ActorData.s_invalidActorIndex;
			if (this.HitActorsToDeltaHP != null)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(ActorAnimation.\u000A\u000E()).MethodHandle;
				}
				if (this.HitActorsToDeltaHP.Count >= 1)
				{
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					if (this.HitActorsToDeltaHP.Count <= 2)
					{
						for (;;)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
						for (int i = 0; i < this.HitActorsToDeltaHP.Count; i++)
						{
							ActorData actorData = this.HitActorsToDeltaHP.Keys.ElementAt(i);
							if (actorData != null)
							{
								for (;;)
								{
									switch (5)
									{
									case 0:
										continue;
									}
									break;
								}
								if (this.Actor != null)
								{
									for (;;)
									{
										switch (2)
										{
										case 0:
											continue;
										}
										break;
									}
									if (this.Actor.GetTeam() != actorData.GetTeam())
									{
										for (;;)
										{
											switch (2)
											{
											case 0:
												continue;
											}
											break;
										}
										if (s_invalidActorIndex != ActorData.s_invalidActorIndex)
										{
											s_invalidActorIndex = ActorData.s_invalidActorIndex;
											break;
										}
										for (;;)
										{
											switch (2)
											{
											case 0:
												continue;
											}
											break;
										}
										s_invalidActorIndex = actorData.ActorIndex;
									}
									else if (actorData != this.Actor)
									{
										for (;;)
										{
											switch (3)
											{
											case 0:
												continue;
											}
											break;
										}
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
					for (;;)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!true)
					{
						RuntimeMethodHandle runtimeMethodHandle = methodof(ActorAnimation.CompareTo(ActorAnimation)).MethodHandle;
					}
				}
				else
				{
					if (this.ability.RunPriority != rhs.ability.RunPriority)
					{
						return this.ability.RunPriority.CompareTo(rhs.ability.RunPriority);
					}
					if ((int)this.playOrderIndex != (int)rhs.playOrderIndex)
					{
						for (;;)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							break;
						}
						return this.playOrderIndex.CompareTo(rhs.playOrderIndex);
					}
					bool flag = GameFlowData.Get().IsActorDataOwned(this.Actor);
					bool flag2 = GameFlowData.Get().IsActorDataOwned(rhs.Actor);
					if (!this.ability.IsFreeAction())
					{
						for (;;)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							break;
						}
						if (rhs.ability.IsFreeAction())
						{
							for (;;)
							{
								switch (5)
								{
								case 0:
									continue;
								}
								break;
							}
						}
						else
						{
							if (flag != flag2)
							{
								for (;;)
								{
									switch (1)
									{
									case 0:
										continue;
									}
									break;
								}
								return flag.CompareTo(flag2);
							}
							if (this.Actor.ActorIndex != rhs.Actor.ActorIndex)
							{
								return this.Actor.ActorIndex.CompareTo(rhs.Actor.ActorIndex);
							}
							if (this.animationIndex != rhs.animationIndex)
							{
								for (;;)
								{
									switch (7)
									{
									case 0:
										continue;
									}
									break;
								}
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
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				if (rhs.ability == null)
				{
					for (;;)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					return 0;
				}
			}
			if (this.ability != null)
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				if (this.ability.IsFreeAction())
				{
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					return -1;
				}
			}
			if (rhs.ability != null)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
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
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(ActorAnimation.GetDebugStringDetails(Animator, bool)).MethodHandle;
				}
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
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
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
						for (;;)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							break;
						}
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
					array2[3] = this.\u001C;
					array2[4] = " sec.\nAfter a request for ability ";
					int num2 = 5;
					object obj2;
					if (this.ability == null)
					{
						for (;;)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
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
						for (;;)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							break;
						}
						obj3 = "NULL";
					}
					else
					{
						obj3 = this.ability.GetActionAnimType().ToString();
					}
					array2[num3] = obj3;
					array2[0x1C] = ", ability anim played: ";
					array2[0x1D] = this.\u001A;
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
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!NetworkClient.active)
				{
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					object[] array3 = new object[8];
					array3[0] = "\nIn ability animation state for ";
					array3[1] = this.Actor.GetDebugName();
					array3[2] = ", ability: ";
					int num4 = 3;
					object obj4;
					if (this.ability == null)
					{
						for (;;)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							break;
						}
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
			for (int i = 0; i < this.\u0018\u000E.Count; i++)
			{
				if (this.\u0018\u000E[i] != null)
				{
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!true)
					{
						RuntimeMethodHandle runtimeMethodHandle = methodof(ActorAnimation.GetDebugStringAnimationEventsSeen()).MethodHandle;
					}
					text = text + "    [ " + this.\u0018\u000E[i] + " ]\n";
				}
			}
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
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
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(ActorAnimation.ToString()).MethodHandle;
				}
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

		public string \u000D\u000E(string \u001D = "")
		{
			string[] array = new string[5];
			array[0] = "[ActorAnimation: ";
			int num = 1;
			string text;
			if (this.Actor == null)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(ActorAnimation.\u000D\u000E(string)).MethodHandle;
				}
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
			if (\u001D.Length > 0)
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				text2 = string.Concat(new string[]
				{
					"<color=",
					\u001D,
					">",
					text2,
					"</color>"
				});
			}
			return text2;
		}

		internal enum PlaybackState
		{
			\u001D,
			\u000E,
			\u0012,
			\u0015,
			\u0016,
			\u0013,
			\u0018
		}
	}
}

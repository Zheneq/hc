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

		private short \u000E;

		private Vector3 \u0012;

		private bool \u0015;

		private int \u0016;

		private bool \u0013;

		private bool \u0018;

		private bool \u0009;

		private AbilityData.ActionType \u0019 = AbilityData.ActionType.INVALID_ACTION;

		private bool \u0011;

		private bool \u001A;

		private bool \u0004;

		private Ability \u000B;

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private SequenceSource \u0003;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private SequenceSource \u000F;

		internal int \u0017 = ActorData.s_invalidActorIndex;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private Dictionary<ActorData, int> \u000D;

		internal bool \u0008;

		internal int \u0002;

		internal sbyte \u000A;

		internal sbyte \u0006;

		internal Bounds \u0020;

		private List<byte> \u000C = new List<byte>();

		private List<byte> \u0014 = new List<byte>();

		private bool \u0005;

		private AbilityRequest \u001B;

		private Turn \u001E;

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

		private ActorAnimation.PlaybackState \u0009\u000E;

		private static readonly int \u0019\u000E = Animator.StringToHash("DistToGoal");

		private static readonly int \u0011\u000E = Animator.StringToHash("StartDamageReaction");

		private static readonly int \u001A\u000E = Animator.StringToHash("Attack");

		private static readonly int \u0004\u000E = Animator.StringToHash("CinematicCam");

		private static readonly int \u000B\u000E = Animator.StringToHash("TauntNumber");

		private static readonly int \u0003\u000E = Animator.StringToHash("TauntAnimIndex");

		private static readonly int \u000F\u000E = Animator.StringToHash("StartAttack");

		private const float \u0017\u000E = 1f;

		internal ActorAnimation(Turn \u001D)
		{
			this.\u001E = \u001D;
		}

		internal SequenceSource SeqSource { get; private set; }

		internal SequenceSource ParentAbilitySeqSource { get; private set; }

		internal ActorData \u000D\u000E
		{
			get
			{
				if (this.\u0017 != ActorData.s_invalidActorIndex)
				{
					if (!(GameFlowData.Get() == null))
					{
						return GameFlowData.Get().FindActorByActorIndex(this.\u0017);
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
					if (this.\u0017 != ActorData.s_invalidActorIndex)
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
						this.\u0017 = ActorData.s_invalidActorIndex;
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
					if (value.ActorIndex != this.\u0017)
					{
						this.\u0017 = value.ActorIndex;
					}
				}
			}
		}

		internal Dictionary<ActorData, int> HitActorsToDeltaHP { get; private set; }

		internal Ability \u000D\u000E()
		{
			return this.\u000B;
		}

		public int \u000D\u000E
		{
			get
			{
				return this.\u0016;
			}
			private set
			{
			}
		}

		internal bool AnimationFinished { get; private set; }

		internal ActorAnimation.PlaybackState \u000D\u000E
		{
			get
			{
				return this.\u0009\u000E;
			}
			set
			{
				if (value != this.\u0009\u000E)
				{
					if (this.\u000B != null)
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
							int num = AbilityUtils.GetTechPointRewardForInteraction(this.\u000B, AbilityInteractionType.Cast, true, false, false);
							num = AbilityUtils.CalculateTechPointsForTargeter(this.\u000D\u000E, this.\u000B, num);
							if (num > 0)
							{
								this.\u000D\u000E.AddCombatText(num.ToString(), string.Empty, CombatTextCategory.TP_Recovery, BuffIconToDisplay.None);
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
									this.\u000D\u000E.ClientUnresolvedTechPointGain += num;
								}
							}
							if (this.\u000B.GetModdedCost() > 0)
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
								if (this.\u000D\u000E.ReservedTechPoints > 0)
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
									int num2 = this.\u000D\u000E.ClientReservedTechPoints - this.\u000B.GetModdedCost();
									num2 = Mathf.Max(num2, -this.\u000D\u000E.ReservedTechPoints);
									this.\u000D\u000E.ClientReservedTechPoints = num2;
								}
							}
						}
					}
					if (TheatricsManager.\u000E)
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
							this.\u0009\u000E,
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
					if (this.\u000D\u000E != null)
					{
						this.\u000D\u000E.CurrentlyVisibleForAbilityCast = false;
					}
					IL_1C8:
					this.\u0009\u000E = value;
				}
			}
		}

		internal bool \u000D\u000E
		{
			get
			{
				return this.\u000D\u000E >= ActorAnimation.PlaybackState.\u0012;
			}
		}

		private bool \u0008\u000E()
		{
			if (this.\u000D\u000E == null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ActorAnimation.\u0008\u000E()).MethodHandle;
				}
				Log.Error("Theatrics: can't start {0} since the actor can no longer be found. Was the actor destroyed during resolution?", new object[]
				{
					this
				});
				this.\u000D\u000E = ActorAnimation.PlaybackState.\u0018;
				return false;
			}
			return this.\u000D\u000E == ActorAnimation.PlaybackState.\u0018;
		}

		internal void \u000D\u000E(IBitStream \u001D)
		{
			sbyte b = (sbyte)this.\u000E;
			sbyte b2 = (sbyte)this.\u0019;
			float x = this.\u0012.x;
			float z = this.\u0012.z;
			sbyte b3;
			if (this.\u000D\u000E == null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ActorAnimation.\u000D\u000E(IBitStream)).MethodHandle;
				}
				b3 = (sbyte)ActorData.s_invalidActorIndex;
			}
			else
			{
				b3 = (sbyte)this.\u000D\u000E.ActorIndex;
			}
			sbyte b4 = b3;
			bool u = this.\u0008;
			sbyte b5 = (sbyte)this.\u0016;
			bool u2 = this.\u0013;
			bool u3 = this.\u0018;
			bool u4 = this.\u0009;
			bool u5 = this.\u0015;
			sbyte u000A = this.\u000A;
			sbyte u6 = this.\u0006;
			Vector3 center = this.\u0020.center;
			Vector3 size = this.\u0020.size;
			byte b6 = checked((byte)this.\u000C.Count);
			\u001D.Serialize(ref b);
			\u001D.Serialize(ref b2);
			\u001D.Serialize(ref x);
			\u001D.Serialize(ref z);
			\u001D.Serialize(ref b4);
			\u001D.Serialize(ref u);
			\u001D.Serialize(ref b5);
			\u001D.Serialize(ref u2);
			\u001D.Serialize(ref u3);
			\u001D.Serialize(ref u4);
			\u001D.Serialize(ref u5);
			\u001D.Serialize(ref u000A);
			\u001D.Serialize(ref u6);
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
				center.y = 1.5f + (float)Board.\u000E().BaselineHeight;
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
			\u001D.Serialize(ref b6);
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
				for (int i = 0; i < (int)b6; i++)
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
				for (int j = 0; j < (int)b6; j++)
				{
					byte b7 = this.\u000C[j];
					byte b8 = this.\u0014[j];
					\u001D.Serialize(ref b7);
					\u001D.Serialize(ref b8);
				}
			}
			this.\u000E = (short)b;
			if (\u001D.isReading)
			{
				this.\u0012 = new Vector3(x, (float)Board.\u000E().BaselineHeight, z);
			}
			this.\u0017 = (int)b4;
			this.\u0008 = u;
			this.\u0016 = (int)b5;
			this.\u0013 = u2;
			this.\u0018 = u3;
			this.\u0009 = u4;
			this.\u0015 = u5;
			this.\u000A = u000A;
			this.\u0006 = u6;
			this.\u0020 = new Bounds(center, size);
			this.\u0019 = (AbilityData.ActionType)b2;
			this.\u000B = ((!(this.\u000D\u000E == null)) ? this.\u000D\u000E.\u000E().GetAbilityOfActionType(this.\u0019) : null);
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
				bool flag = this.ParentAbilitySeqSource != null;
				\u001D.Serialize(ref flag);
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
				bool flag2 = false;
				\u001D.Serialize(ref flag2);
				if (flag2)
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
			sbyte b9;
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
				b9 = 0;
			}
			else
			{
				b9 = (sbyte)this.HitActorsToDeltaHP.Count;
			}
			sbyte b10 = b9;
			\u001D.Serialize(ref b10);
			if ((int)b10 > 0)
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
				if ((int)b10 > 0)
				{
					using (Dictionary<ActorData, int>.Enumerator enumerator = this.HitActorsToDeltaHP.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							KeyValuePair<ActorData, int> keyValuePair = enumerator.Current;
							sbyte b11;
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
								b11 = (sbyte)ActorData.s_invalidActorIndex;
							}
							else
							{
								b11 = (sbyte)keyValuePair.Key.ActorIndex;
							}
							sbyte b12 = b11;
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
				for (int k = 0; k < (int)b10; k++)
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
			this.\u0008\u000E();
		}

		internal bool \u0002\u000E()
		{
			return this.\u0016 > 0;
		}

		internal bool \u000A\u000E()
		{
			return this.\u0013;
		}

		internal bool \u0006\u000E()
		{
			return this.\u0015;
		}

		internal bool \u000D\u000E(ActorData \u001D)
		{
			if (this.HitActorsToDeltaHP != null)
			{
				if (this.HitActorsToDeltaHP.ContainsKey(\u001D))
				{
					return this.HitActorsToDeltaHP[\u001D] < 0;
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ActorAnimation.\u000D\u000E(ActorData)).MethodHandle;
				}
			}
			return false;
		}

		internal bool \u0020\u000E()
		{
			bool result;
			if (!this.\u000D\u000E.\u000E())
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
					result = this.\u0008;
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
			ActorStatus actorStatus = this.\u000D\u000E.\u000E();
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
			if (!this.\u000D\u000E.VisibleTillEndOfPhase)
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
				if (!this.\u000D\u000E.CurrentlyVisibleForAbilityCast)
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
							if (this.\u000D\u000E.\u000E(GameFlowData.Get().LocalPlayerData, true, false))
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
						BoardSquare boardSquare = Board.\u000E().\u0016((int)this.\u000C[i], (int)this.\u0014[i]);
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
					ActorMovement actorMovement = this.\u000D\u000E.\u000E();
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
						if (Board.\u000E() != null)
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
										BoardSquare boardSquare = Board.\u000E().\u000E(key.transform.position);
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

		internal bool \u000D\u000E(Sequence \u001D)
		{
			bool result;
			if (\u001D != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ActorAnimation.\u000D\u000E(Sequence)).MethodHandle;
				}
				result = (\u001D.Source == this.SeqSource);
			}
			else
			{
				result = false;
			}
			return result;
		}

		internal bool \u000D\u000E(AbilityPriority \u001D, bool \u000E = false)
		{
			if (this.\u000D\u000E != ActorAnimation.PlaybackState.\u001D)
			{
				return false;
			}
			bool flag = !ClientResolutionManager.Get().IsWaitingForActionMessages(\u001D);
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ActorAnimation.\u000D\u000E(AbilityPriority, bool)).MethodHandle;
				}
				if (\u000E)
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
			return this.\u000D\u000E != ActorAnimation.PlaybackState.\u0018 && this.\u000D\u000E != ActorAnimation.PlaybackState.\u0013;
		}

		internal bool \u0005\u000E()
		{
			int result;
			if (this.\u000D\u000E >= ActorAnimation.PlaybackState.\u0015)
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
						if (GameTime.time < this.\u0007 + this.\u000D\u000E())
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

		private int \u0008\u000E()
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ActorAnimation.\u0008\u000E()).MethodHandle;
				}
				result = 0;
			}
			else
			{
				int count = this.HitActorsToDeltaHP.Count;
				int num;
				if (this.HitActorsToDeltaHP.ContainsKey(this.\u000D\u000E))
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

		private float \u000D\u000E()
		{
			return AbilitiesCamera.Get().CalcFrameTimeAfterHit(this.\u0008\u000E());
		}

		internal void \u000D\u000E(Turn \u001D)
		{
			if (ClientAbilityResults.\u001D || TheatricsManager.\u000E)
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
			if (this.\u000D\u000E == ActorAnimation.PlaybackState.\u0018)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ActorAnimation.\u000D\u000E(Turn)).MethodHandle;
				}
				return;
			}
			bool flag;
			if (this.\u000B != null)
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
				flag = this.\u000B.ShouldRotateToTargetPos();
			}
			else
			{
				flag = (this.\u000E > 0);
			}
			bool flag2 = flag;
			if (flag2)
			{
				if (this.\u0008)
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
					this.\u000D\u000E.TurnToPositionInstant(this.\u0012);
				}
				else
				{
					this.\u000D\u000E.TurnToPosition(this.\u0012, 0.2f);
				}
			}
			Animator animator = this.\u000D\u000E.\u000E();
			if (animator != null)
			{
				float num;
				if (this.\u000B == null)
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
					if (this.\u000B.GetMovementType() != ActorData.MovementType.None)
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
						if (this.\u000B.GetMovementType() != ActorData.MovementType.Knockback)
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
					num = this.\u000D\u000E.\u000E().FindDistanceToEnd();
				}
				IL_177:
				float value = num;
				animator.SetFloat(ActorAnimation.\u0019\u000E, value);
				animator.ResetTrigger(ActorAnimation.\u0011\u000E);
			}
			AbilityData.ActionType actionTypeOfAbility = this.\u000D\u000E.\u000E().GetActionTypeOfAbility(this.\u000B);
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
					userActor = this.\u000D\u000E
				});
				if (HUD_UI.Get() != null)
				{
					HUD_UI.Get().m_mainScreenPanel.m_abilityBar.m_theTimer.m_abilityUsedTracker.AddNewAbility(this.\u000D\u000E(), this.\u000D\u000E);
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
					ability = this.\u000B,
					userActor = this.\u000D\u000E
				});
			}
			if (this.\u000E <= 0)
			{
				this.\u000D\u000E = ActorAnimation.PlaybackState.\u0016;
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
				this.\u000D\u000E.CurrentlyVisibleForAbilityCast = true;
			}
			if (this.\u000E <= 0)
			{
				this.\u0008\u000E();
				this.\u0002\u000E();
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
				this.\u000D\u000E = ActorAnimation.PlaybackState.\u0016;
				this.\u001A = true;
				this.AnimationFinished = true;
				this.\u0008\u000E();
				this.\u0002\u000E();
			}
			else
			{
				animator.SetInteger(ActorAnimation.\u001A\u000E, (int)this.\u000E);
				animator.SetBool(ActorAnimation.\u0004\u000E, this.\u0008);
				if (this.\u000D\u000E(animator, "TauntNumber"))
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
					animator.SetInteger(ActorAnimation.\u000B\u000E, this.\u0016);
				}
				animator.SetTrigger(ActorAnimation.\u000F\u000E);
				if (this.\u000D\u000E.\u000E().HasAnimatorControllerParamater("TauntAnimIndex"))
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
					animator.SetInteger(ActorAnimation.\u0003\u000E, this.\u0002);
				}
				if (this.\u000B != null)
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
					this.\u000B.OnAbilityAnimationRequest(this.\u000D\u000E, (int)this.\u000E, this.\u0008, this.\u0012);
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
					HUD_UI.Get().m_mainScreenPanel.m_abilityBar.m_theTimer.m_abilityUsedTracker.AddNewAbility(this.\u000D\u000E(), this.\u000D\u000E);
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
				CameraManager.Get().OnAbilityAnimationStart(this.\u000D\u000E, (int)this.\u000E, this.\u0012, this.\u0008, this.\u0016);
				if (this.\u000D\u000E != null && this.\u0008)
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
						this.\u000D\u000E.ForceUpdateIsVisibleToClientCache();
					}
				}
				if (this.\u0008)
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
					if (this.\u0016 > 0)
					{
					}
				}
				this.\u000D\u000E();
				this.\u0002\u000E();
				this.\u000D\u000E = ActorAnimation.PlaybackState.\u0012;
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
					if (!TheatricsManager.\u000E)
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
				if (!(this.\u000D\u000E == null))
				{
					if (!(this.\u000D\u000E.\u000E() == null))
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
				this.\u000D\u000E = ActorAnimation.PlaybackState.\u0018;
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
				if (this.\u000D\u000E != ActorAnimation.PlaybackState.\u0018)
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
					animator = this.\u000D\u000E.\u000E();
					if (animator == null)
					{
						this.\u000D\u000E = ActorAnimation.PlaybackState.\u0018;
					}
					else if (!animator.enabled && this.\u000E > 0)
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
						this.\u000D\u000E = ActorAnimation.PlaybackState.\u0018;
					}
				}
			}
			if (this.\u000D\u000E == ActorAnimation.PlaybackState.\u0018)
			{
				return false;
			}
			ActorMovement actorMovement = this.\u000D\u000E.\u000E();
			bool flag = !actorMovement.AmMoving();
			if (this.\u000D\u000E >= ActorAnimation.PlaybackState.\u0012 && this.\u000D\u000E < ActorAnimation.PlaybackState.\u0013)
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
						this.\u0008\u000E(animator, flag);
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
					if (this.\u000D\u000E >= ActorAnimation.PlaybackState.\u0016)
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
						this.\u000D\u000E.DisplayName,
						(!(this.\u000B != null)) ? (" animation index " + this.\u000E.ToString()) : this.\u000B.ToString(),
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
				flag4 = this.\u000D\u000E.\u000E().IsPlayingAttackAnim(out flag5);
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
			if (this.\u000D\u000E >= ActorAnimation.PlaybackState.\u0012)
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
				if (this.\u000D\u000E < ActorAnimation.PlaybackState.\u0013)
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
						if (TheatricsManager.\u000E)
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
					flag6 = (GameTime.time - this.\u0007 >= this.\u000D\u000E());
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
			switch (this.\u000D\u000E)
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
					this.\u000D\u000E = ActorAnimation.PlaybackState.\u0016;
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
					animator.SetInteger(ActorAnimation.\u001A\u000E, 0);
					animator.SetBool(ActorAnimation.\u0004\u000E, false);
				}
				if (this.\u000B != null)
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
					this.\u000B.OnAbilityAnimationRequestProcessed(this.\u000D\u000E);
				}
				if (this.\u000D\u000E < ActorAnimation.PlaybackState.\u0016)
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
					this.\u000D\u000E = ActorAnimation.PlaybackState.\u0015;
				}
				this.\u000D\u000E();
				this.\u0008\u000E();
				if (ClientResolutionManager.Get() != null)
				{
					ClientResolutionManager.Get().OnAbilityCast(this.\u000D\u000E, this.\u000B);
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
				this.\u000D\u000E = ActorAnimation.PlaybackState.\u0016;
				this.\u0002\u000E();
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
						u = FogOfWar.GetClientFog().IsVisible(this.\u000D\u000E.\u000E());
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
						if (this.\u000D\u000E.\u000E() != null)
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
							this.\u000D\u000E.\u000E().EncapsulatePathInBound(ref bounds);
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
					if (flag7 && this.\u000D\u000E >= ActorAnimation.PlaybackState.\u0016)
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
						ClientKnockbackManager.Get().NotifyOnActorAnimHitsDone(this.\u000D\u000E);
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
					if (!this.\u000A\u000E())
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
					if (this.\u000E > 0)
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
			this.\u000D\u000E = ActorAnimation.PlaybackState.\u0013;
			this.\u000D\u000E();
			this.\u0002\u000E();
			ActorData u000D_u000E = this.\u000D\u000E;
			if (this.\u000B != null)
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
				this.\u000B.OnAbilityAnimationReleaseFocus(u000D_u000E);
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
				u000D_u000E.DoVisualDeath(new ActorModelData.ImpulseInfo(u000D_u000E.\u0015(), Vector3.up));
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
						if (TheatricsManager.\u000E)
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
			if (!this.\u000D\u000E(\u001D))
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
				ActorModelData actorModelData = \u000E.\u000E();
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
						if (this.\u001E.\u001A(\u000E))
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
					if (this.\u001E.\u0004(\u000E, 0, -1))
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
								if (!\u001D.Caster.\u0012())
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
									if (\u001D.Caster.\u000E() != \u000E.\u000E())
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
				if (this.\u000B != null)
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
					if (this.\u000B.CurrentAbilityMod != null)
					{
						text = "Mod Id: [" + this.\u000B.CurrentAbilityMod.m_abilityScopeId + "]\n";
						goto IL_9B;
					}
				}
				text = string.Empty;
				IL_9B:
				array[num] = text;
				array[1] = "Theatrics Entry: ";
				array[2] = this.ToString();
				array[3] = "\n";
				array[4] = this.\u000D\u000E();
				array[5] = this.\u0008\u000E(\u001D, \u000E);
				array[6] = "\n";
				string extraInfo = string.Concat(array);
				ClientResolutionManager.Get().ExecuteUnexecutedActions(this.SeqSource, extraInfo);
				ClientResolutionManager.Get().UpdateLastEventTime();
				this.\u0012\u000E = true;
			}
		}

		private void \u000D\u000E()
		{
		}

		private void \u0008\u000E()
		{
		}

		private void \u0002\u000E()
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ActorAnimation.\u0002\u000E()).MethodHandle;
				}
				ClientResolutionManager.Get().UpdateLastEventTime();
			}
		}

		internal float \u000D\u000E(bool \u001D)
		{
			ActorData u000D_u000E = this.\u000D\u000E;
			if (u000D_u000E == null || u000D_u000E.\u000E() == null)
			{
				return 0f;
			}
			return u000D_u000E.\u000E().GetCamStartEventDelay((int)this.\u000E, \u001D);
		}

		internal int \u0002\u000E()
		{
			return (int)this.\u000E;
		}

		internal int \u000A\u000E()
		{
			int num = ActorData.s_invalidActorIndex;
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
								if (this.\u000D\u000E != null)
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
									if (this.\u000D\u000E.\u000E() != actorData.\u000E())
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
										if (num != ActorData.s_invalidActorIndex)
										{
											num = ActorData.s_invalidActorIndex;
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
										num = actorData.ActorIndex;
									}
									else if (actorData != this.\u000D\u000E)
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
										num = ActorData.s_invalidActorIndex;
										break;
									}
								}
							}
						}
					}
				}
			}
			return num;
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
			if (!(this.\u000B == null))
			{
				if (rhs.\u000B == null)
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
					if (this.\u000B.RunPriority != rhs.\u000B.RunPriority)
					{
						return this.\u000B.RunPriority.CompareTo(rhs.\u000B.RunPriority);
					}
					if ((int)this.\u000A != (int)rhs.\u000A)
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
						return this.\u000A.CompareTo(rhs.\u000A);
					}
					bool flag = GameFlowData.Get().IsActorDataOwned(this.\u000D\u000E);
					bool flag2 = GameFlowData.Get().IsActorDataOwned(rhs.\u000D\u000E);
					if (!this.\u000B.IsFreeAction())
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
						if (rhs.\u000B.IsFreeAction())
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
							if (this.\u000D\u000E.ActorIndex != rhs.\u000D\u000E.ActorIndex)
							{
								return this.\u000D\u000E.ActorIndex.CompareTo(rhs.\u000D\u000E.ActorIndex);
							}
							if (this.\u000E != rhs.\u000E)
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
								return this.\u000E.CompareTo(rhs.\u000E);
							}
							return 0;
						}
					}
					return -1 * this.\u000B.IsFreeAction().CompareTo(rhs.\u000B.IsFreeAction());
				}
			}
			if (this.\u000B == null)
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
				if (rhs.\u000B == null)
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
			if (this.\u000B != null)
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
				if (this.\u000B.IsFreeAction())
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
			if (rhs.\u000B != null)
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
				if (rhs.\u000B.IsFreeAction())
				{
					return 1;
				}
			}
			return (!(this.\u000B == null)) ? 1 : -1;
		}

		private void \u0008\u000E(Animator \u001D, bool \u000E)
		{
			Log.Error("Theatrics: {0} {1} is hung", new object[]
			{
				this.\u000D\u000E.DisplayName,
				this.\u0008\u000E(\u001D, \u000E)
			});
		}

		public string \u0008\u000E(Animator \u001D, bool \u000E)
		{
			string result = string.Empty;
			if (\u001D != null && this.\u000D\u000E.\u000E() != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ActorAnimation.\u0008\u000E(Animator, bool)).MethodHandle;
				}
				int integer = \u001D.GetInteger("Attack");
				bool @bool = \u001D.GetBool("Cover");
				float @float = \u001D.GetFloat("DistToGoal");
				int integer2 = \u001D.GetInteger("NextLinkType");
				int integer3 = \u001D.GetInteger("CurLinkType");
				bool bool2 = \u001D.GetBool("CinematicCam");
				bool bool3 = \u001D.GetBool("DecisionPhase");
				bool flag = \u001D.GetCurrentAnimatorStateInfo(0).IsName("Damage");
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
					array[1] = this.\u000D\u000E.\u0018();
					array[2] = " while Damage flag is set (hit react.). Code error, show Chris. debug info: (state: ";
					array[3] = this.\u000D\u000E.ToString();
					array[4] = ", Attack: ";
					array[5] = integer;
					array[6] = ", ability: ";
					int num = 7;
					object obj;
					if (this.\u000B == null)
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
						obj = this.\u000B.GetActionAnimType().ToString();
					}
					array[num] = obj;
					array[8] = ")";
					result = string.Concat(array);
				}
				else
				{
					object[] array2 = new object[0x23];
					array2[0] = "\nIn animation state ";
					array2[1] = this.\u000D\u000E.\u000E().GetCurrentAnimatorStateName();
					array2[2] = " for ";
					array2[3] = this.\u001C;
					array2[4] = " sec.\nAfter a request for ability ";
					int num2 = 5;
					object obj2;
					if (this.\u000B == null)
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
						obj2 = this.\u000B.m_abilityName;
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
					array2[0x15] = this.\u000D\u000E.ToString();
					array2[0x16] = ", actor state: ";
					array2[0x17] = this.\u000D\u000E.\u000E().CurrentState.ToString();
					array2[0x18] = ", movement path done: ";
					array2[0x19] = \u000E;
					array2[0x1A] = ", ability anim: ";
					int num3 = 0x1B;
					object obj3;
					if (this.\u000B == null)
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
						obj3 = this.\u000B.GetActionAnimType().ToString();
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
					array3[1] = this.\u000D\u000E.\u0018();
					array3[2] = ", ability: ";
					int num4 = 3;
					object obj4;
					if (this.\u000B == null)
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
						obj4 = this.\u000B.GetActionAnimType().ToString();
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

		public string \u000D\u000E()
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(ActorAnimation.\u000D\u000E()).MethodHandle;
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
			array[1] = ((!(this.\u000D\u000E == null)) ? this.\u000D\u000E.\u0018() : "(NULL caster)");
			array[2] = " ";
			int num = 3;
			object obj;
			if (this.\u000B == null)
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
				obj = this.\u000B.m_abilityName;
			}
			array[num] = obj;
			array[4] = ", animation index: ";
			array[5] = this.\u000E;
			array[6] = ", play order index: ";
			array[7] = this.\u000A;
			array[8] = ", group index: ";
			array[9] = this.\u0006;
			array[0xA] = ", state: ";
			array[0xB] = this.\u000D\u000E;
			array[0xC] = "]";
			return string.Concat(array);
		}

		public string \u000D\u000E(string \u001D = "")
		{
			string[] array = new string[5];
			array[0] = "[ActorAnimation: ";
			int num = 1;
			string text;
			if (this.\u000D\u000E == null)
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
				text = this.\u000D\u000E.\u0018();
			}
			array[num] = text;
			array[2] = ", ";
			array[3] = ((!(this.\u000B == null)) ? this.\u000B.m_abilityName : "(NULL ability)");
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

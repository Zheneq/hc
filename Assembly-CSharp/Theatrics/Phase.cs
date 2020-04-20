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
	internal class Phase
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private AbilityPriority abilityPriority;

		internal List<ActorAnimation> animations = new List<ActorAnimation>();

		private Dictionary<int, int> actorIndexToDeltaHP = new Dictionary<int, int>();

		private Dictionary<int, int> symbol_0015 = new Dictionary<int, int>();

		private List<int> participants = new List<int>();

		private int playOrderIndex = -1;

		private int symbol_0018 = -1;

		private bool symbol_0009 = true;

		private int symbol_0019 = -1;

		private int symbol_0011 = -1;

		private float maxCamStartDelay;

		private int symbol_0004 = -1;

		private float symbol_000B;

		private float symbol_0003;

		private Turn symbol_000F;

		private float symbol_0017;

		private float symbol_000D;

		private bool symbol_0008;

		private bool symbol_0002;

		private float evadeStartTime = -1f;

		private bool symbol_0006;

		private bool symbol_0020;

		private bool symbol_000C;

		private bool symbol_0014;

		private bool symbol_0005;

		private bool symbol_001B;

		private int symbol_001E = -1;

		private bool symbol_0001;

		private const float symbol_001F = 0.7f;

		private const float symbol_0010 = 0.3f;

		private const float symbol_0007 = 0.35f;

		internal Phase(Turn symbol_001D)
		{
			this.symbol_000F = symbol_001D;
		}

		internal AbilityPriority Index { get; private set; }

		internal Dictionary<int, int> ActorIndexToDeltaHP
		{
			get
			{
				return this.actorIndexToDeltaHP;
			}
			private set
			{
				this.actorIndexToDeltaHP = value;
			}
		}

		public void SetSymbol0006ToTrue()
		{
			this.symbol_0006 = true;
		}

		internal void symbol_001Dsymbol_000E()
		{
			for (int i = 0; i < this.animations.Count; i++)
			{
				ActorAnimation actorAnimation = this.animations[i];
				if (!actorAnimation.symbol_0006symbol_000E())
				{
					float b = actorAnimation.symbol_000Dsymbol_000E(this.Index == AbilityPriority.Evasion && actorAnimation.symbol_0002symbol_000E());
					this.maxCamStartDelay = Mathf.Max(this.maxCamStartDelay, b);
				}
				if (this.symbol_0011 == -1)
				{
					if (!actorAnimation.cinematicCamera)
					{
						this.symbol_0011 = (int)actorAnimation.playOrderIndex;
					}
				}
			}
		}

		internal bool symbol_001C(ActorData symbol_001D)
		{
			bool result = false;
			if (this.animations != null)
			{
				for (int i = 0; i < this.animations.Count; i++)
				{
					ActorAnimation actorAnimation = this.animations[i];
					if (actorAnimation.Actor == symbol_001D)
					{
						if (!actorAnimation.symbol_0006symbol_000E())
						{
							bool flag = actorAnimation.State >= ActorAnimation.PlaybackState.symbol_0016;
							bool flag2 = ClientResolutionManager.Get().HitsDoneExecuting(actorAnimation.SeqSource);
							if (flag2)
							{
								if (flag)
								{
									goto IL_AA;
								}
							}
							result = true;
							break;
						}
					}
					IL_AA:;
				}
			}
			return result;
		}

		internal bool symbol_001Dsymbol_000E(ActorData symbol_001D)
		{
			if (this.symbol_0015 != null)
			{
				if (this.symbol_0015.ContainsKey(symbol_001D.ActorIndex))
				{
					return this.symbol_0015[symbol_001D.ActorIndex] > 0;
				}
			}
			return false;
		}

		internal bool symbol_000Esymbol_000E(ActorData symbol_001D)
		{
			if (symbol_001D != null)
			{
				if (this.participants != null)
				{
					return this.participants.Contains(symbol_001D.ActorIndex);
				}
			}
			return false;
		}

		internal bool symbol_001C()
		{
			bool result = false;
			for (int i = 0; i < this.animations.Count; i++)
			{
				ActorAnimation actorAnimation = this.animations[i];
				if (actorAnimation.State != ActorAnimation.PlaybackState.symbol_0018 && actorAnimation.State != ActorAnimation.PlaybackState.symbol_0013)
				{
					result = true;
					break;
				}
			}
			return result;
		}

		internal void OnSerializeHelper(IBitStream stream)
		{
			sbyte b = (sbyte)this.Index;
			stream.Serialize(ref b);
			this.Index = (AbilityPriority)b;
			sbyte b2 = (sbyte)this.animations.Count;
			bool flag;
			if (stream.isWriting)
			{
				if (!this.symbol_0006)
				{
					flag = ((int)b < (int)((sbyte)ServerClientUtils.GetCurrentAbilityPhase()));
				}
				else
				{
					flag = true;
				}
			}
			else
			{
				flag = false;
			}
			bool flag2 = flag;
			if (stream.isWriting)
			{
				if (flag2)
				{
					b2 = 0;
				}
			}
			stream.Serialize(ref b2);
			for (int i = 0; i < (int)b2; i++)
			{
				while (i >= this.animations.Count)
				{
					this.animations.Add(new ActorAnimation(this.symbol_000F));
				}
				this.animations[i].OnSerializeHelper(stream);
			}
			if (stream.isWriting)
			{
				sbyte b3 = (sbyte)this.actorIndexToDeltaHP.Count;
				stream.Serialize(ref b3);
				using (Dictionary<int, int>.Enumerator enumerator = this.actorIndexToDeltaHP.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<int, int> keyValuePair = enumerator.Current;
						sbyte b4 = (sbyte)keyValuePair.Key;
						short num = (short)keyValuePair.Value;
						stream.Serialize(ref b4);
						stream.Serialize(ref num);
					}
				}
			}
			else
			{
				sbyte b5 = -1;
				stream.Serialize(ref b5);
				this.actorIndexToDeltaHP = new Dictionary<int, int>();
				for (int j = 0; j < (int)b5; j++)
				{
					sbyte b6 = (sbyte)ActorData.s_invalidActorIndex;
					short value = -1;
					stream.Serialize(ref b6);
					stream.Serialize(ref value);
					this.actorIndexToDeltaHP.Add((int)b6, (int)value);
				}
			}
			if ((int)b == 5)
			{
				if (stream.isWriting)
				{
					sbyte b7 = (sbyte)this.symbol_0015.Count;
					if (flag2)
					{
						b7 = 0;
						stream.Serialize(ref b7);
					}
					else
					{
						stream.Serialize(ref b7);
						foreach (KeyValuePair<int, int> keyValuePair2 in this.symbol_0015)
						{
							sbyte b8 = (sbyte)keyValuePair2.Key;
							sbyte b9 = (sbyte)keyValuePair2.Value;
							stream.Serialize(ref b8);
							stream.Serialize(ref b9);
						}
					}
				}
				else
				{
					sbyte b10 = -1;
					stream.Serialize(ref b10);
					this.symbol_0015 = new Dictionary<int, int>();
					for (int k = 0; k < (int)b10; k++)
					{
						sbyte b11 = (sbyte)ActorData.s_invalidActorIndex;
						sbyte b12 = -1;
						stream.Serialize(ref b11);
						stream.Serialize(ref b12);
						this.symbol_0015.Add((int)b11, (int)b12);
					}
				}
			}
			if (stream.isWriting)
			{
				sbyte b13 = (sbyte)this.participants.Count;
				stream.Serialize(ref b13);
				sbyte b14 = 0;
				while ((int)b14 < (int)b13)
				{
					sbyte b15 = (sbyte)this.participants[(int)b14];
					stream.Serialize(ref b15);
					b14 = (sbyte)((int)b14 + 1);
				}
			}
			else
			{
				sbyte b16 = -1;
				stream.Serialize(ref b16);
				this.participants = new List<int>();
				sbyte b17 = 0;
				while ((int)b17 < (int)b16)
				{
					sbyte b18 = -1;
					stream.Serialize(ref b18);
					this.participants.Add((int)b18);
					b17 = (sbyte)((int)b17 + 1);
				}
			}
		}

		private void symbol_001C(List<ActorAnimation> symbol_001D)
		{
			if (symbol_001D != null)
			{
				if (symbol_001D.Count > 0)
				{
					GameEventManager.TheatricsAbilityHighlightStartArgs theatricsAbilityHighlightStartArgs = new GameEventManager.TheatricsAbilityHighlightStartArgs();
					for (int i = 0; i < symbol_001D.Count; i++)
					{
						ActorAnimation actorAnimation = symbol_001D[i];
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
					this.symbol_0001 = true;
				}
			}
		}

		private void symbol_000Esymbol_000E()
		{
			GameEventManager.TheatricsAbilityHighlightStartArgs args = new GameEventManager.TheatricsAbilityHighlightStartArgs();
			GameEventManager.Get().FireEvent(GameEventManager.EventType.TheatricsAbilityHighlightStart, args);
			this.symbol_0001 = false;
		}

		private unsafe ActorData symbol_001C(int symbol_001D, out bool symbol_000E)
		{
			symbol_000E = false;
			for (int i = 0; i < this.animations.Count; i++)
			{
				if ((int)this.animations[i].playOrderIndex == symbol_001D)
				{
					symbol_000E = this.animations[i].cinematicCamera;
					return this.animations[i].Actor;
				}
			}
			return null;
		}

		private bool symbol_001C(int symbol_001D)
		{
			bool flag = false;
			for (int i = 0; i < this.animations.Count; i++)
			{
				ActorAnimation actorAnimation = this.animations[i];
				if (actorAnimation != null)
				{
					if (actorAnimation.HitActorsToDeltaHP != null && (int)actorAnimation.playOrderIndex == symbol_001D)
					{
						if (!actorAnimation.symbol_0002symbol_000E())
						{
							using (Dictionary<ActorData, int>.Enumerator enumerator = actorAnimation.HitActorsToDeltaHP.GetEnumerator())
							{
								while (enumerator.MoveNext())
								{
									KeyValuePair<ActorData, int> keyValuePair = enumerator.Current;
									ActorData key = keyValuePair.Key;
									int value = keyValuePair.Value;
									int hitPointsAfterResolution = key.GetHitPointsAfterResolution();
									int hitPointsAfterResolutionWithDelta = key.GetHitPointsAfterResolutionWithDelta(value);
									bool flag2;
									if (value < 0 && hitPointsAfterResolution > 0)
									{
										flag2 = (hitPointsAfterResolutionWithDelta <= 0);
									}
									else
									{
										flag2 = false;
									}
									bool flag3 = flag2;
									if (flag3)
									{
										flag = true;
									}
									else
									{
										if (value > 0 && hitPointsAfterResolution <= 0)
										{
											if (hitPointsAfterResolutionWithDelta > 0)
											{
												flag = true;
												goto IL_15A;
											}
										}
										if (this.symbol_000F.symbol_0004(key, value, (int)actorAnimation.SeqSource.RootID))
										{
											flag = true;
											if (CameraManager.CamDebugTraceOn)
											{
												CameraManager.LogForDebugging("Ragdolling hit on " + key + " when HP is already 0", CameraManager.CameraLogType.None);
											}
										}
									}
									IL_15A:
									if (flag)
									{
										if (CameraManager.CamDebugTraceOn)
										{
											CameraManager.LogForDebugging(string.Concat(new object[]
											{
												"Using Low Position for ",
												actorAnimation.ToString(),
												"\nhpDelta: ",
												value,
												" | hpForDisplay: ",
												hitPointsAfterResolution,
												" | expectedHpAfterHit: ",
												hitPointsAfterResolutionWithDelta
											}), CameraManager.CameraLogType.None);
										}
									}
								}
							}
						}
						return flag;
					}
				}
			}
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				return flag;
			}
		}

		private bool symbol_001C(Bounds symbol_001D, Bounds symbol_000E, float symbol_0012, float symbol_0015, bool symbol_0016)
		{
			Vector3 center = symbol_001D.center;
			Vector3 center2 = symbol_000E.center;
			Vector3 vector = center2 - center;
			vector.y = 0f;
			float magnitude = vector.magnitude;
			if (magnitude <= symbol_0012)
			{
				bool flag = false;
				Vector3 vector2;
				Vector3 vector3;
				bool flag2 = CameraManager.BoundSidesWithinDistance(symbol_001D, symbol_000E, symbol_0015, out vector2, out vector3);
				if (symbol_0016)
				{
					if (!flag2)
					{
						Bounds bounds = symbol_001D;
						bounds.Expand(new Vector3(1.5f, 100f, 1.5f));
						if (bounds.Contains(symbol_000E.center + symbol_000E.extents))
						{
							if (bounds.Contains(symbol_000E.center - symbol_000E.extents))
							{
								flag2 = true;
								flag = true;
							}
						}
					}
				}
				if (CameraManager.CamDebugTraceOn)
				{
					CameraManager.LogForDebugging(string.Concat(new object[]
					{
						"Considering bounds as similar, result = <color=yellow>",
						flag2,
						"</color> | centerDist = ",
						magnitude,
						" | minBoundsDiff: ",
						vector3,
						" | maxBoundsDiff: ",
						vector2,
						" | used inflated bounds: ",
						flag,
						"\nPrev Bound: ",
						symbol_001D,
						"\nCompare to Bound: ",
						symbol_000E
					}), CameraManager.CameraLogType.SimilarBounds);
				}
				return flag2;
			}
			if (CameraManager.CamDebugTraceOn)
			{
				CameraManager.LogForDebugging("Not merging bounds, centerDist too far: " + magnitude, CameraManager.CameraLogType.SimilarBounds);
			}
			return false;
		}

		internal unsafe bool symbol_001C(Turn symbol_001D, ref bool symbol_000E, ref bool symbol_0012)
		{
			this.symbol_0017 += GameTime.deltaTime;
			this.symbol_000D += GameTime.deltaTime;
			bool flag = (this.animations.Count != 0 & this.Index == AbilityPriority.Evasion) && !this.symbol_0002;
			bool flag2 = true;
			for (int i = 0; i < this.animations.Count; i++)
			{
				ActorAnimation actorAnimation = this.animations[i];
				if (actorAnimation.symbol_000Dsymbol_000E(symbol_001D))
				{
					flag = true;
					if ((int)actorAnimation.playOrderIndex <= this.playOrderIndex)
					{
						bool flag3;
						if (flag2)
						{
							flag3 = actorAnimation.symbol_0005symbol_000E();
						}
						else
						{
							flag3 = false;
						}
						flag2 = flag3;
					}
					if ((int)actorAnimation.playOrderIndex == this.playOrderIndex)
					{
						bool flag4 = actorAnimation.symbol_000Csymbol_000E();
						bool flag5;
						if (!symbol_000E)
						{
							flag5 = flag4;
						}
						else
						{
							flag5 = true;
						}
						symbol_000E = flag5;
						bool flag6;
						if (!symbol_0012)
						{
							flag6 = !flag4;
						}
						else
						{
							flag6 = true;
						}
						symbol_0012 = flag6;
					}
				}
			}
			if (CameraManager.Get().IsPlayingShotSequence())
			{
				this.symbol_0008 = true;
				return true;
			}
			if (this.symbol_0008)
			{
				this.symbol_0017 = 0f;
				this.symbol_0008 = false;
			}
			if (!flag)
			{
				return false;
			}
			int num = int.MaxValue;
			int num2 = int.MaxValue;
			for (int j = 0; j < this.animations.Count; j++)
			{
				ActorAnimation actorAnimation2 = this.animations[j];
				if (actorAnimation2 != null)
				{
					if (actorAnimation2.State == ActorAnimation.PlaybackState.symbol_001D)
					{
						if ((int)actorAnimation2.playOrderIndex < num)
						{
							num = (int)actorAnimation2.playOrderIndex;
							num2 = (int)actorAnimation2.groupIndex;
						}
					}
				}
			}
			AbilitiesCamera abilitiesCamera = AbilitiesCamera.Get();
			List<ActorAnimation> list = null;
			float num3;
			if (this.Index == AbilityPriority.Evasion)
			{
				num3 = 0.7f;
			}
			else
			{
				num3 = 0.3f;
			}
			float num4 = num3;
			bool flag7;
			if (symbol_001D.TimeInResolve < num4)
			{
				flag7 = (this.symbol_0017 >= num4);
			}
			else
			{
				flag7 = true;
			}
			bool flag8 = flag7;
			bool flag9;
			if (flag8)
			{
				if (flag2)
				{
					if (num != this.playOrderIndex)
					{
						flag9 = (num != int.MaxValue);
					}
					else
					{
						flag9 = false;
					}
					goto IL_298;
				}
			}
			flag9 = false;
			IL_298:
			bool flag10 = flag9;
			bool flag11;
			if (!(GameFlowData.Get() == null))
			{
				flag11 = GameFlowData.Get().IsResolutionPaused();
			}
			else
			{
				flag11 = true;
			}
			bool flag12 = flag11;
			bool flag13;
			if (flag10)
			{
				flag13 = !flag12;
			}
			else
			{
				flag13 = false;
			}
			flag10 = flag13;
			if (!flag10 && this.symbol_0017 > 20f)
			{
				if (!flag12)
				{
					Log.Error(string.Concat(new object[]
					{
						"Stuck when trying to advance to next actor anim entry, \nplay order release focus: ",
						flag2.ToString(),
						"\npast waiting for first action: ",
						flag8,
						"\nminNotStartedPLayOrderIndex: ",
						num,
						"\nplayOrderIndex: ",
						this.playOrderIndex
					}), new object[0]);
					flag10 = true;
				}
			}
			if (flag10)
			{
				if (this.symbol_0001)
				{
					this.symbol_000Esymbol_000E();
				}
				list = new List<ActorAnimation>();
				bool flag14 = true;
				int k = 0;
				while (k < this.animations.Count)
				{
					ActorAnimation actorAnimation3 = this.animations[k];
					if (actorAnimation3 == null)
					{
						goto IL_3EB;
					}
					ActorModelData actorModelData;
					if (actorAnimation3.Actor == null)
					{
						for (;;)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							goto IL_3EB;
						}
					}
					else
					{
						actorModelData = actorAnimation3.Actor.GetActorModelData();
					}
					IL_3FC:
					ActorModelData actorModelData2 = actorModelData;
					Animator animator;
					if (actorModelData2 == null)
					{
						animator = null;
					}
					else
					{
						animator = actorModelData2.GetModelAnimator();
					}
					Animator animator2 = animator;
					bool flag15 = false;
					if (actorAnimation3.Actor != null)
					{
						if (actorAnimation3.GetAnimationIndex() <= 0)
						{
							flag15 = actorAnimation3.Actor.IsModelAnimatorDisabled();
						}
					}
					if (actorAnimation3 != null)
					{
						if (actorAnimation3.State == ActorAnimation.PlaybackState.symbol_001D)
						{
							if ((int)actorAnimation3.playOrderIndex == num)
							{
								bool flag16 = false;
								if (NetworkClient.active)
								{
									if (!actorAnimation3.IsReadyToPlay_zq(this.Index, false))
									{
										goto IL_550;
									}
									if (animator2 == null || animator2.layerCount < 1)
									{
										goto IL_550;
									}
									if (!flag15 && !actorModelData2.IsPlayingIdleAnim(false))
									{
										if (!actorModelData2.IsPlayingDamageAnim())
										{
											goto IL_550;
										}
										if (!TheatricsManager.Get().m_allowAbilityAnimationInterruptHitReaction)
										{
											goto IL_550;
										}
									}
									bool flag17 = animator2.GetInteger("Attack") != 0;
									IL_551:
									flag16 = flag17;
									goto IL_553;
									IL_550:
									flag17 = true;
									goto IL_551;
								}
								IL_553:
								if (flag16)
								{
									if (this.symbol_0017 > 1f)
									{
										if (actorModelData2.IsPlayingKnockdownAnim())
										{
											if (!this.symbol_000C)
											{
												string message = actorAnimation3 + " is stuck in knockdown when trying to play animation for ability, forcing idle";
												if (Application.isEditor)
												{
													Log.Error(message, new object[0]);
												}
												else
												{
													Log.Warning(message, new object[0]);
												}
												this.symbol_000C = true;
											}
											animator2.SetBool("TurnStart", true);
											animator2.SetTrigger("ForceIdle");
										}
									}
									if (this.symbol_0017 > 5f)
									{
										if (!this.symbol_0020)
										{
											this.symbol_0020 = true;
											bool flag18 = animator2.GetCurrentAnimatorStateInfo(0).IsName("Damage");
											int integer = animator2.GetInteger("Attack");
											string text = string.Empty;
											if (actorAnimation3.HitActorsToDeltaHP != null)
											{
												using (Dictionary<ActorData, int>.Enumerator enumerator = actorAnimation3.HitActorsToDeltaHP.GetEnumerator())
												{
													while (enumerator.MoveNext())
													{
														KeyValuePair<ActorData, int> keyValuePair = enumerator.Current;
														text += keyValuePair.Key.ToString();
														text += ", ";
													}
												}
											}
											object[] array = new object[0xE];
											array[0] = actorAnimation3;
											array[1] = " is not ready to play. Current animation state: ";
											int num5 = 2;
											object obj;
											if (actorModelData2 == null)
											{
												obj = "NULL actor model data";
											}
											else
											{
												obj = actorModelData2.GetCurrentAnimatorStateName();
											}
											array[num5] = obj;
											array[3] = ", playing idle animation: ";
											array[4] = ((!(actorModelData2 == null)) ? actorModelData2.IsPlayingIdleAnim(false).ToString() : "NULL actor model data");
											array[5] = ", to hit(";
											array[6] = text;
											array[7] = "), playing damage reaction: ";
											array[8] = flag18;
											array[9] = ", attack animation parameter: ";
											array[0xA] = integer;
											array[0xB] = ", animator layer count: ";
											array[0xC] = ((!(animator2 == null)) ? animator2.layerCount.ToString() : "NULL");
											array[0xD] = ((actorAnimation3.State == ActorAnimation.PlaybackState.symbol_001D) ? (", sequences ready: " + actorAnimation3.IsReadyToPlay_zq(this.Index, true)) : string.Empty);
											Log.Error(string.Concat(array), new object[0]);
										}
										if (this.symbol_0017 > 8f)
										{
											Log.Error(actorAnimation3 + " timed out, skipping", new object[0]);
											this.symbol_0020 = false;
											if (ClientResolutionManager.Get() != null)
											{
												ClientResolutionManager.Get().UpdateLastEventTime();
											}
											goto IL_885;
										}
									}
									flag14 = false;
								}
								list.Add(actorAnimation3);
							}
						}
					}
					IL_885:
					k++;
					continue;
					IL_3EB:
					actorModelData = null;
					goto IL_3FC;
				}
				this.symbol_0009 = (this.symbol_0018 != num2);
				if (this.symbol_0004 != num)
				{
					if (flag2)
					{
						bool flag19;
						Bounds bounds = symbol_001D.symbol_0011(this, num, out flag19);
						bool flag20;
						if (this.Index != AbilityPriority.Evasion)
						{
							flag20 = (this.Index == AbilityPriority.Combat_Knockback);
						}
						else
						{
							flag20 = true;
						}
						bool flag21 = flag20;
						this.symbol_001B = false;
						this.symbol_0005 = false;
						bool useLowPosition = false;
						if (!flag21)
						{
							useLowPosition = this.symbol_001C(num);
							ActorData actorData = this.symbol_001C(num, out this.symbol_0005);
							int num6 = (!(actorData != null)) ? -1 : actorData.ActorIndex;
							if (this.symbol_001E > 0)
							{
								if (num6 == this.symbol_001E)
								{
									if (!this.symbol_0005)
									{
										this.symbol_001B = true;
									}
								}
							}
						}
						if (!flag19)
						{
							bool flag22 = this.symbol_000F.symbol_0018 > 0 && this.symbol_000F.symbol_0013 == bounds;
							if (this.symbol_000F.symbol_0018 > 0)
							{
								if (!flag22)
								{
									if (!flag21)
									{
										bool flag23 = this.symbol_001C(this.symbol_000F.symbol_0013, bounds, abilitiesCamera.m_similarCenterDistThreshold, abilitiesCamera.m_similarBoundSideMaxDiff, abilitiesCamera.m_considerFramingSimilarIfInsidePrevious);
										if (flag23)
										{
											bounds = this.symbol_000F.symbol_0013;
										}
									}
								}
							}
							bool flag24;
							if (this.Index != AbilityPriority.Evasion)
							{
								flag24 = !this.symbol_0009;
							}
							else
							{
								flag24 = true;
							}
							bool quickerTransition = flag24;
							CameraManager.Get().SetTarget(bounds, quickerTransition, useLowPosition);
							this.symbol_000F.symbol_0009 = true;
							if (this.symbol_000F.symbol_0013 == bounds)
							{
								this.symbol_0014 = true;
							}
							else
							{
								this.symbol_0014 = false;
							}
							this.symbol_000F.symbol_0013 = bounds;
							if (flag21)
							{
								this.symbol_000F.symbol_0018 = 0;
							}
							else
							{
								this.symbol_000F.symbol_0018++;
							}
						}
						if (num == 0)
						{
							CameraManager.Get().OnActionPhaseChange(ActionBufferPhase.Abilities, true);
						}
						this.symbol_0004 = num;
						this.symbol_000B = GameTime.time;
						if (flag21)
						{
							this.symbol_001C(list);
						}
						else
						{
							this.symbol_000Esymbol_000E();
						}
						if (TheatricsManager.DebugLog)
						{
							TheatricsManager.LogForDebugging(string.Concat(new object[]
							{
								"Cam set target for player order index ",
								num,
								" group ",
								num2,
								" group changed ",
								this.symbol_0009,
								" timeInResolve = ",
								this.symbol_000F.TimeInResolve,
								" anticipating CamStartEvent..."
							}));
						}
					}
				}
				if (flag14)
				{
					this.playOrderIndex = num;
					this.symbol_0018 = num2;
					flag2 = false;
					GameEventManager.TheatricsAbilityAnimationStartArgs theatricsAbilityAnimationStartArgs = new GameEventManager.TheatricsAbilityAnimationStartArgs();
					theatricsAbilityAnimationStartArgs.lastInPhase = (this.playOrderIndex >= this.symbol_0019);
					GameEventManager.Get().FireEvent(GameEventManager.EventType.TheatricsAbilityAnimationStart, theatricsAbilityAnimationStartArgs);
				}
			}
			AbilityPriority index = this.Index;
			if (index != AbilityPriority.Evasion)
			{
				float num7 = abilitiesCamera.m_easeInTime;
				if (!this.symbol_0005)
				{
					if (this.symbol_001B)
					{
						if (this.symbol_0014)
						{
							num7 = 0f;
							goto IL_106F;
						}
					}
					if (this.symbol_0014)
					{
						num7 = abilitiesCamera.m_easeInTimeForSimilarBounds;
					}
					else if (!this.symbol_0009)
					{
						num7 = abilitiesCamera.m_easeInTimeWithinGroup;
					}
				}
				IL_106F:
				float num8 = (this.symbol_000B > 0f) ? (this.symbol_000B + num7) : 0f;
				for (int l = 0; l < this.animations.Count; l++)
				{
					ActorAnimation actorAnimation4 = this.animations[l];
					if (actorAnimation4 != null)
					{
						if (actorAnimation4.State == ActorAnimation.PlaybackState.symbol_001D && (int)actorAnimation4.playOrderIndex == this.playOrderIndex)
						{
							float num9 = Mathf.Max(0f, num8 - GameTime.time);
							if (actorAnimation4.GetSymbol0013())
							{
								num9 = 0f;
							}
							float num10 = actorAnimation4.symbol_000Dsymbol_000E(false);
							if (actorAnimation4.GetAnimationIndex() == 0)
							{
								num10 = 0.35f;
							}
							if (num10 < 0f)
							{
								Log.Error("Camera start event delay is negative", new object[0]);
								num10 = 0f;
							}
							if (num9 <= num10)
							{
								if (TheatricsManager.DebugLog)
								{
									TheatricsManager.LogForDebugging(string.Concat(new object[]
									{
										"Queued ",
										actorAnimation4,
										"\ngroup ",
										actorAnimation4.groupIndex,
										" camStartEventDelay: ",
										num10,
										" easeInTime: ",
										num7,
										" camera bounds similar as last: ",
										this.symbol_0014,
										" phase ",
										this.Index.ToString()
									}));
								}
								actorAnimation4.method000D000E(symbol_001D);
								this.symbol_0017 = 0f;
								this.symbol_000C = false;
								this.symbol_001E = ((!(actorAnimation4.Actor != null)) ? -1 : actorAnimation4.Actor.ActorIndex);
								if (this.Index != AbilityPriority.Evasion)
								{
									if (this.Index != AbilityPriority.Combat_Knockback)
									{
										if (!actorAnimation4.GetSymbol0013())
										{
											this.symbol_001C(new List<ActorAnimation>
											{
												actorAnimation4
											});
										}
									}
								}
							}
						}
					}
				}
			}
			else if (this.playOrderIndex < this.symbol_0011)
			{
				for (int m = 0; m < this.animations.Count; m++)
				{
					ActorAnimation actorAnimation5 = this.animations[m];
					if (actorAnimation5 != null)
					{
						if (actorAnimation5.State == ActorAnimation.PlaybackState.symbol_001D)
						{
							if ((int)actorAnimation5.playOrderIndex == this.playOrderIndex)
							{
								actorAnimation5.method000D000E(symbol_001D);
								this.symbol_0017 = 0f;
								this.symbol_000C = false;
							}
						}
					}
				}
			}
			else if (!this.symbol_0002)
			{
				float num11 = Mathf.Max(0.8f, this.maxCamStartDelay);
				if (this.evadeStartTime < 0f)
				{
					this.evadeStartTime = GameTime.time + num11;
					if (TheatricsManager.DebugLog)
					{
						TheatricsManager.LogForDebugging(string.Concat(new object[]
						{
							"Setting evade start time: ",
							this.evadeStartTime,
							" maxEvadeStartDelay: ",
							num11
						}));
					}
				}
				float num12 = this.evadeStartTime;
				for (int n = 0; n < this.animations.Count; n++)
				{
					ActorAnimation actorAnimation6 = this.animations[n];
					if (actorAnimation6 != null)
					{
						if (actorAnimation6.State == ActorAnimation.PlaybackState.symbol_001D)
						{
							if ((int)actorAnimation6.playOrderIndex == this.playOrderIndex)
							{
								bool flag25;
								if (this.Index == AbilityPriority.Evasion)
								{
									flag25 = actorAnimation6.symbol_0002symbol_000E();
								}
								else
								{
									flag25 = false;
								}
								bool u001D = flag25;
								if (num12 <= GameTime.time + Mathf.Max(1.401298E-45f, GameTime.smoothDeltaTime) + actorAnimation6.symbol_000Dsymbol_000E(u001D))
								{
									actorAnimation6.method000D000E(symbol_001D);
									this.symbol_0017 = 0f;
									this.symbol_000C = false;
									if (this.symbol_0003 == 0f)
									{
										this.symbol_0003 = GameTime.time;
									}
								}
								if (actorAnimation6.symbol_0020symbol_000E())
								{
									actorAnimation6.Actor.CurrentlyVisibleForAbilityCast = true;
								}
							}
						}
					}
				}
				if (num12 > 0f && num12 <= GameTime.time)
				{
					for (int num13 = 0; num13 < this.animations.Count; num13++)
					{
						ActorAnimation actorAnimation7 = this.animations[num13];
						if (actorAnimation7.GetAbility() != null)
						{
							actorAnimation7.GetAbility().OnEvasionMoveStartEvent(actorAnimation7.Actor);
						}
					}
					List<ActorData> actors = GameFlowData.Get().GetActors();
					for (int num14 = 0; num14 < actors.Count; num14++)
					{
						actors[num14].CurrentlyVisibleForAbilityCast = false;
					}
					for (int num15 = 0; num15 < actors.Count; num15++)
					{
						actors[num15].ForceUpdateIsVisibleToClientCache();
					}
					this.symbol_0002 = true;
					GameEventManager.Get().FireEvent(GameEventManager.EventType.TheatricsEvasionMoveStart, null);
					for (int num16 = 0; num16 < actors.Count; num16++)
					{
						actors[num16].ForceUpdateIsVisibleToClientCache();
					}
					if (TheatricsManager.DebugLog)
					{
						TheatricsManager.LogForDebugging("Evasion Move Start, MaxCamStartDelay= " + this.maxCamStartDelay);
					}
				}
			}
			return true;
		}
	}
}

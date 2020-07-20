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
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private AbilityPriority abilityPriority;

		internal List<ActorAnimation> animations = new List<ActorAnimation>();

		private Dictionary<int, int> actorIndexToDeltaHP = new Dictionary<int, int>();

		private Dictionary<int, int> _0015 = new Dictionary<int, int>();

		private List<int> participants = new List<int>();

		private int playOrderIndex = -1;

		private int _0018 = -1;

		private bool _0009 = true;

		private int _0019 = -1;

		private int _0011 = -1;

		private float maxCamStartDelay;

		private int _0004 = -1;

		private float _000B;

		private float _0003;

		private Turn _000F;

		private float _0017;

		private float _000D;

		private bool _0008;

		private bool _0002;

		private float evadeStartTime = -1f;

		private bool _0006;

		private bool _0020;

		private bool _000C;

		private bool _0014;

		private bool _0005;

		private bool _001B;

		private int _001E = -1;

		private bool _0001;

		private const float _001F = 0.7f;

		private const float _0010 = 0.3f;

		private const float _0007 = 0.35f;

		internal AbilityPriority Index
		{
			get;
			private set;
		}

		internal Dictionary<int, int> ActorIndexToDeltaHP
		{
			get
			{
				return actorIndexToDeltaHP;
			}
			private set
			{
				actorIndexToDeltaHP = value;
			}
		}

		internal Phase(Turn _001D)
		{
			_000F = _001D;
		}

		public void SetSymbol0006ToTrue()
		{
			_0006 = true;
		}

		internal void _001D_000E()
		{
			for (int i = 0; i < animations.Count; i++)
			{
				ActorAnimation actorAnimation = animations[i];
				if (!actorAnimation._0006_000E())
				{
					float b = actorAnimation._000D_000E(Index == AbilityPriority.Evasion && actorAnimation.IsTauntActivated());
					maxCamStartDelay = Mathf.Max(maxCamStartDelay, b);
				}
				if (_0011 != -1)
				{
					continue;
				}
				if (!actorAnimation.cinematicCamera)
				{
					_0011 = actorAnimation.playOrderIndex;
				}
			}
		}

		internal bool _001C(ActorData _001D)
		{
			bool result = false;
			if (animations != null)
			{
				for (int i = 0; i < animations.Count; i++)
				{
					ActorAnimation actorAnimation = animations[i];
					if (!(actorAnimation.Actor == _001D))
					{
						continue;
					}
					if (actorAnimation._0006_000E())
					{
						continue;
					}
					bool flag = actorAnimation.State >= ActorAnimation.PlaybackState._0016;
					if (ClientResolutionManager.Get().HitsDoneExecuting(actorAnimation.SeqSource))
					{
						if (flag)
						{
							continue;
						}
					}
					result = true;
					break;
				}
			}
			return result;
		}

		internal bool _001D_000E(ActorData _001D)
		{
			if (_0015 != null)
			{
				if (_0015.ContainsKey(_001D.ActorIndex))
				{
					return _0015[_001D.ActorIndex] > 0;
				}
			}
			return false;
		}

		internal bool _000E_000E(ActorData _001D)
		{
			if (_001D != null)
			{
				if (participants != null)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							break;
						default:
							return participants.Contains(_001D.ActorIndex);
						}
					}
				}
			}
			return false;
		}

		internal bool _001C()
		{
			bool result = false;
			for (int i = 0; i < animations.Count; i++)
			{
				ActorAnimation actorAnimation = animations[i];
				if (actorAnimation.State != ActorAnimation.PlaybackState._0018 && actorAnimation.State != ActorAnimation.PlaybackState._0013)
				{
					result = true;
					break;
				}
			}
			return result;
		}

		internal void OnSerializeHelper(IBitStream stream)
		{
			sbyte value = (sbyte)Index;
			stream.Serialize(ref value);
			Index = (AbilityPriority)value;
			sbyte value2 = (sbyte)animations.Count;
			int num;
			if (stream.isWriting)
			{
				if (!_0006)
				{
					num = ((value < (sbyte)ServerClientUtils.GetCurrentAbilityPhase()) ? 1 : 0);
				}
				else
				{
					num = 1;
				}
			}
			else
			{
				num = 0;
			}
			bool flag = (byte)num != 0;
			if (stream.isWriting)
			{
				if (flag)
				{
					value2 = 0;
				}
			}
			stream.Serialize(ref value2);
			for (int i = 0; i < value2; i++)
			{
				while (i >= animations.Count)
				{
					animations.Add(new ActorAnimation(_000F));
				}
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						goto end_IL_00b7;
					}
					continue;
					end_IL_00b7:
					break;
				}
				animations[i].OnSerializeHelper(stream);
			}
			while (true)
			{
				if (stream.isWriting)
				{
					sbyte value3 = (sbyte)actorIndexToDeltaHP.Count;
					stream.Serialize(ref value3);
					using (Dictionary<int, int>.Enumerator enumerator = actorIndexToDeltaHP.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							KeyValuePair<int, int> current = enumerator.Current;
							sbyte value4 = (sbyte)current.Key;
							short value5 = (short)current.Value;
							stream.Serialize(ref value4);
							stream.Serialize(ref value5);
						}
					}
				}
				else
				{
					sbyte value6 = -1;
					stream.Serialize(ref value6);
					actorIndexToDeltaHP = new Dictionary<int, int>();
					for (int j = 0; j < value6; j++)
					{
						sbyte value7 = (sbyte)ActorData.s_invalidActorIndex;
						short value8 = -1;
						stream.Serialize(ref value7);
						stream.Serialize(ref value8);
						actorIndexToDeltaHP.Add(value7, value8);
					}
				}
				if (value == 5)
				{
					if (stream.isWriting)
					{
						sbyte value9 = (sbyte)_0015.Count;
						if (flag)
						{
							value9 = 0;
							stream.Serialize(ref value9);
						}
						else
						{
							stream.Serialize(ref value9);
							foreach (KeyValuePair<int, int> item in _0015)
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
						_0015 = new Dictionary<int, int>();
						for (int k = 0; k < value12; k++)
						{
							sbyte value13 = (sbyte)ActorData.s_invalidActorIndex;
							sbyte value14 = -1;
							stream.Serialize(ref value13);
							stream.Serialize(ref value14);
							_0015.Add(value13, value14);
						}
					}
				}
				if (stream.isWriting)
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							break;
						default:
						{
							sbyte value15 = (sbyte)participants.Count;
							stream.Serialize(ref value15);
							for (sbyte b = 0; b < value15; b = (sbyte)(b + 1))
							{
								sbyte value16 = (sbyte)participants[b];
								stream.Serialize(ref value16);
							}
							while (true)
							{
								switch (4)
								{
								default:
									return;
								case 0:
									break;
								}
							}
						}
						}
					}
				}
				sbyte value17 = -1;
				stream.Serialize(ref value17);
				participants = new List<int>();
				for (sbyte b2 = 0; b2 < value17; b2 = (sbyte)(b2 + 1))
				{
					sbyte value18 = -1;
					stream.Serialize(ref value18);
					participants.Add(value18);
				}
				while (true)
				{
					switch (3)
					{
					default:
						return;
					case 0:
						break;
					}
				}
			}
		}

		private void _001C(List<ActorAnimation> _001D)
		{
			if (_001D == null)
			{
				return;
			}
			while (true)
			{
				if (_001D.Count <= 0)
				{
					return;
				}
				GameEventManager.TheatricsAbilityHighlightStartArgs theatricsAbilityHighlightStartArgs = new GameEventManager.TheatricsAbilityHighlightStartArgs();
				for (int i = 0; i < _001D.Count; i++)
				{
					ActorAnimation actorAnimation = _001D[i];
					theatricsAbilityHighlightStartArgs.m_casters.Add(actorAnimation.Actor);
					if (actorAnimation.HitActorsToDeltaHP != null)
					{
						for (int j = 0; j < actorAnimation.HitActorsToDeltaHP.Keys.Count; j++)
						{
							theatricsAbilityHighlightStartArgs.m_targets.Add(actorAnimation.HitActorsToDeltaHP.Keys.ElementAt(j));
						}
					}
				}
				while (true)
				{
					List<ActorData> actorsWithMovementHits = ClientResolutionManager.Get().GetActorsWithMovementHits();
					for (int k = 0; k < actorsWithMovementHits.Count; k++)
					{
						if (!theatricsAbilityHighlightStartArgs.m_targets.Contains(actorsWithMovementHits[k]))
						{
							theatricsAbilityHighlightStartArgs.m_targets.Add(actorsWithMovementHits[k]);
						}
					}
					while (true)
					{
						GameEventManager.Get().FireEvent(GameEventManager.EventType.TheatricsAbilityHighlightStart, theatricsAbilityHighlightStartArgs);
						_0001 = true;
						return;
					}
				}
			}
		}

		private void _000E_000E()
		{
			GameEventManager.TheatricsAbilityHighlightStartArgs args = new GameEventManager.TheatricsAbilityHighlightStartArgs();
			GameEventManager.Get().FireEvent(GameEventManager.EventType.TheatricsAbilityHighlightStart, args);
			_0001 = false;
		}

		private ActorData _001C(int _001D, out bool _000E)
		{
			_000E = false;
			for (int i = 0; i < animations.Count; i++)
			{
				if (animations[i].playOrderIndex != _001D)
				{
					continue;
				}
				while (true)
				{
					_000E = animations[i].cinematicCamera;
					return animations[i].Actor;
				}
			}
			return null;
		}

		private bool _001C(int _001D)
		{
			bool flag = false;
			int num = 0;
			while (true)
			{
				if (num < animations.Count)
				{
					ActorAnimation actorAnimation = animations[num];
					if (actorAnimation != null)
					{
						if (actorAnimation.HitActorsToDeltaHP != null && actorAnimation.playOrderIndex == _001D)
						{
							if (actorAnimation.IsTauntActivated())
							{
								break;
							}
							while (true)
							{
								using (Dictionary<ActorData, int>.Enumerator enumerator = actorAnimation.HitActorsToDeltaHP.GetEnumerator())
								{
									while (enumerator.MoveNext())
									{
										KeyValuePair<ActorData, int> current = enumerator.Current;
										ActorData key = current.Key;
										int value = current.Value;
										int hitPointsAfterResolution = key.GetHitPointsAfterResolution();
										int hitPointsAfterResolutionWithDelta = key.GetHitPointsAfterResolutionWithDelta(value);
										int num2;
										if (value < 0 && hitPointsAfterResolution > 0)
										{
											num2 = ((hitPointsAfterResolutionWithDelta <= 0) ? 1 : 0);
										}
										else
										{
											num2 = 0;
										}
										if (num2 != 0)
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
													goto IL_015a;
												}
											}
											if (_000F._0004(key, value, (int)actorAnimation.SeqSource.RootID))
											{
												flag = true;
												if (CameraManager.CamDebugTraceOn)
												{
													CameraManager.LogForDebugging(string.Concat("Ragdolling hit on ", key, " when HP is already 0"));
												}
											}
										}
										goto IL_015a;
										IL_015a:
										if (flag)
										{
											if (CameraManager.CamDebugTraceOn)
											{
												CameraManager.LogForDebugging("Using Low Position for " + actorAnimation.ToString() + "\nhpDelta: " + value + " | hpForDisplay: " + hitPointsAfterResolution + " | expectedHpAfterHit: " + hitPointsAfterResolutionWithDelta);
											}
										}
									}
									while (true)
									{
										switch (4)
										{
										case 0:
											break;
										default:
											return flag;
										}
									}
								}
							}
						}
					}
					num++;
					continue;
				}
				break;
			}
			return flag;
		}

		private bool _001C(Bounds _001D, Bounds _000E, float _0012, float _0015, bool _0016)
		{
			Vector3 center = _001D.center;
			Vector3 center2 = _000E.center;
			Vector3 vector = center2 - center;
			vector.y = 0f;
			float magnitude = vector.magnitude;
			if (magnitude <= _0012)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
					{
						bool flag = false;
						Vector3 maxBoundDiff;
						Vector3 minBoundDiff;
						bool flag2 = CameraManager.BoundSidesWithinDistance(_001D, _000E, _0015, out maxBoundDiff, out minBoundDiff);
						if (_0016)
						{
							if (!flag2)
							{
								Bounds bounds = _001D;
								bounds.Expand(new Vector3(1.5f, 100f, 1.5f));
								if (bounds.Contains(_000E.center + _000E.extents))
								{
									if (bounds.Contains(_000E.center - _000E.extents))
									{
										flag2 = true;
										flag = true;
									}
								}
							}
						}
						if (CameraManager.CamDebugTraceOn)
						{
							CameraManager.LogForDebugging(string.Concat("Considering bounds as similar, result = <color=yellow>", flag2, "</color> | centerDist = ", magnitude, " | minBoundsDiff: ", minBoundDiff, " | maxBoundsDiff: ", maxBoundDiff, " | used inflated bounds: ", flag, "\nPrev Bound: ", _001D, "\nCompare to Bound: ", _000E), CameraManager.CameraLogType.SimilarBounds);
						}
						return flag2;
					}
					}
				}
			}
			if (CameraManager.CamDebugTraceOn)
			{
				CameraManager.LogForDebugging("Not merging bounds, centerDist too far: " + magnitude, CameraManager.CameraLogType.SimilarBounds);
			}
			return false;
		}

		// Play
		internal bool _001C(Turn _001D, ref bool _000E, ref bool _0012)
		{
			_0017 += GameTime.deltaTime;
			_000D += GameTime.deltaTime;
			bool flag = ((animations.Count != 0) & (Index == AbilityPriority.Evasion)) && !_0002;
			bool flag2 = true;
			for (int i = 0; i < animations.Count; i++)
			{
				ActorAnimation actorAnimation = animations[i];
				if (!actorAnimation._000D_000E(_001D))
				{
					continue;
				}
				flag = true;
				if (actorAnimation.playOrderIndex <= playOrderIndex)
				{
					int num;
					if (flag2)
					{
						num = (actorAnimation._0005_000E() ? 1 : 0);
					}
					else
					{
						num = 0;
					}
					flag2 = ((byte)num != 0);
				}
				if (actorAnimation.playOrderIndex != playOrderIndex)
				{
					continue;
				}
				bool flag3 = actorAnimation._000C_000E();
				int num2;
				if (!_000E)
				{
					num2 = (flag3 ? 1 : 0);
				}
				else
				{
					num2 = 1;
				}
				_000E = ((byte)num2 != 0);
				int num3;
				if (!_0012)
				{
					num3 = ((!flag3) ? 1 : 0);
				}
				else
				{
					num3 = 1;
				}
				_0012 = ((byte)num3 != 0);
			}
			while (true)
			{
				if (CameraManager.Get().IsPlayingShotSequence())
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							break;
						default:
							_0008 = true;
							return true;
						}
					}
				}
				if (_0008)
				{
					_0017 = 0f;
					_0008 = false;
				}
				if (!flag)
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
				int num4 = int.MaxValue;
				int num5 = int.MaxValue;
				for (int j = 0; j < animations.Count; j++)
				{
					ActorAnimation actorAnimation2 = animations[j];
					if (actorAnimation2 == null)
					{
						continue;
					}
					if (actorAnimation2.State != 0)
					{
						continue;
					}
					if (actorAnimation2.playOrderIndex < num4)
					{
						num4 = actorAnimation2.playOrderIndex;
						num5 = actorAnimation2.groupIndex;
					}
				}
				while (true)
				{
					AbilitiesCamera abilitiesCamera = AbilitiesCamera.Get();
					List<ActorAnimation> list = null;
					float num6;
					if (Index == AbilityPriority.Evasion)
					{
						num6 = 0.7f;
					}
					else
					{
						num6 = 0.3f;
					}
					float num7 = num6;
					int num8;
					if (!(_001D.TimeInResolve >= num7))
					{
						num8 = ((_0017 >= num7) ? 1 : 0);
					}
					else
					{
						num8 = 1;
					}
					bool flag4 = (byte)num8 != 0;
					int num9;
					if (flag4)
					{
						if (flag2)
						{
							if (num4 != playOrderIndex)
							{
								num9 = ((num4 != int.MaxValue) ? 1 : 0);
							}
							else
							{
								num9 = 0;
							}
							goto IL_0298;
						}
					}
					num9 = 0;
					goto IL_0298;
					IL_0298:
					bool flag5 = (byte)num9 != 0;
					int num10;
					if (!(GameFlowData.Get() == null))
					{
						num10 = (GameFlowData.Get().IsResolutionPaused() ? 1 : 0);
					}
					else
					{
						num10 = 1;
					}
					bool flag6 = (byte)num10 != 0;
					int num11;
					if (flag5)
					{
						num11 = ((!flag6) ? 1 : 0);
					}
					else
					{
						num11 = 0;
					}
					flag5 = ((byte)num11 != 0);
					if (!flag5 && _0017 > 20f)
					{
						if (!flag6)
						{
							Log.Error("Stuck when trying to advance to next actor anim entry, \nplay order release focus: " + flag2.ToString() + "\npast waiting for first action: " + flag4 + "\nminNotStartedPLayOrderIndex: " + num4 + "\nplayOrderIndex: " + playOrderIndex);
							flag5 = true;
						}
					}
					if (flag5)
					{
						if (_0001)
						{
							_000E_000E();
						}
						list = new List<ActorAnimation>();
						bool flag7 = true;
						for (int k = 0; k < animations.Count; k++)
						{
							ActorAnimation actorAnimation3 = animations[k];
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
										bool flag10 = animator.GetCurrentAnimatorStateInfo(0).IsName("Damage");
										int integer = animator.GetInteger("Attack");
										string text = string.Empty;
										if (actorAnimation3.HitActorsToDeltaHP != null)
										{
											using (Dictionary<ActorData, int>.Enumerator enumerator = actorAnimation3.HitActorsToDeltaHP.GetEnumerator())
											{
												while (enumerator.MoveNext())
												{
													text += enumerator.Current.Key.ToString();
													text += ", ";
												}
											}
										}
										object[] obj3 = new object[14]
										{
											actorAnimation3,
											" is not ready to play. Current animation state: ",
											null,
											null,
											null,
											null,
											null,
											null,
											null,
											null,
											null,
											null,
											null,
											null
										};
										object obj4;
										if (actorModelData == null)
										{
											obj4 = "NULL actor model data";
										}
										else
										{
											obj4 = actorModelData.GetCurrentAnimatorStateName();
										}
										obj3[2] = obj4;
										obj3[3] = ", playing idle animation: ";
										obj3[4] = ((!(actorModelData == null)) ? actorModelData.IsPlayingIdleAnim().ToString() : "NULL actor model data");
										obj3[5] = ", to hit(";
										obj3[6] = text;
										obj3[7] = "), playing damage reaction: ";
										obj3[8] = flag10;
										obj3[9] = ", attack animation parameter: ";
										obj3[10] = integer;
										obj3[11] = ", animator layer count: ";
										obj3[12] = ((!(animator == null)) ? animator.layerCount.ToString() : "NULL");
										obj3[13] = ((actorAnimation3.State == ActorAnimation.PlaybackState._001D) ? (", sequences ready: " + actorAnimation3.IsReadyToPlay_zq(Index, true)) : string.Empty);
										Log.Error(string.Concat(obj3));
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
								Bounds bounds = _001D._0011(this, num4, out _00122);
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
									ActorData actorData = _001C(num4, out _0005);
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
									bool flag12 = _000F._0018 > 0 && _000F._0013 == bounds;
									if (_000F._0018 > 0)
									{
										if (!flag12)
										{
											if (!flag11)
											{
												if (_001C(_000F._0013, bounds, abilitiesCamera.m_similarCenterDistThreshold, abilitiesCamera.m_similarBoundSideMaxDiff, abilitiesCamera.m_considerFramingSimilarIfInsidePrevious))
												{
													bounds = _000F._0013;
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
									_000F._0009 = true;
									if (_000F._0013 == bounds)
									{
										_0014 = true;
									}
									else
									{
										_0014 = false;
									}
									_000F._0013 = bounds;
									if (flag11)
									{
										_000F._0018 = 0;
									}
									else
									{
										_000F._0018++;
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
									_001C(list);
								}
								else
								{
									_000E_000E();
								}
								if (TheatricsManager.DebugLog)
								{
									TheatricsManager.LogForDebugging("Cam set target for player order index " + num4 + " group " + num5 + " group changed " + _0009 + " timeInResolve = " + _000F.TimeInResolve + " anticipating CamStartEvent...");
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
					if (playOrderIndex < _0011)
					{
						for (int l = 0; l < animations.Count; l++)
						{
							ActorAnimation actorAnimation4 = animations[l];
							if (actorAnimation4 == null)
							{
								continue;
							}
							if (actorAnimation4.State == ActorAnimation.PlaybackState._001D)
							{
								if (actorAnimation4.playOrderIndex == playOrderIndex)
								{
									actorAnimation4.method000D000E(_001D);
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
						for (int m = 0; m < animations.Count; m++)
						{
							ActorAnimation actorAnimation5 = animations[m];
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
							if (num18 <= GameTime.time + Mathf.Max(float.Epsilon, GameTime.smoothDeltaTime) + actorAnimation5._000D_000E(flag13))
							{
								actorAnimation5.method000D000E(_001D);
								_0017 = 0f;
								_000C = false;
								if (_0003 == 0f)
								{
									_0003 = GameTime.time;
								}
							}
							if (actorAnimation5._0020_000E())
							{
								actorAnimation5.Actor.CurrentlyVisibleForAbilityCast = true;
							}
						}
						if (num18 > 0f && num18 <= GameTime.time)
						{
							for (int n = 0; n < animations.Count; n++)
							{
								ActorAnimation actorAnimation6 = animations[n];
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
					for (int num24 = 0; num24 < animations.Count; num24++)
					{
						ActorAnimation actorAnimation7 = animations[num24];
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
						float num26 = actorAnimation7._000D_000E(false);
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
						actorAnimation7.method000D000E(_001D);
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
								_001C(new List<ActorAnimation>
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

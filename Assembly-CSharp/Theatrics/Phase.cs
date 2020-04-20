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

		private Dictionary<int, int> \u0015 = new Dictionary<int, int>();

		private List<int> participants = new List<int>();

		private int playOrderIndex = -1;

		private int \u0018 = -1;

		private bool \u0009 = true;

		private int \u0019 = -1;

		private int \u0011 = -1;

		private float maxCamStartDelay;

		private int \u0004 = -1;

		private float \u000B;

		private float \u0003;

		private Turn \u000F;

		private float \u0017;

		private float \u000D;

		private bool \u0008;

		private bool \u0002;

		private float evadeStartTime = -1f;

		private bool \u0006;

		private bool \u0020;

		private bool \u000C;

		private bool \u0014;

		private bool \u0005;

		private bool \u001B;

		private int \u001E = -1;

		private bool \u0001;

		private const float \u001F = 0.7f;

		private const float \u0010 = 0.3f;

		private const float \u0007 = 0.35f;

		internal Phase(Turn \u001D)
		{
			this.\u000F = \u001D;
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
			this.\u0006 = true;
		}

		internal void \u001D\u000E()
		{
			for (int i = 0; i < this.animations.Count; i++)
			{
				ActorAnimation actorAnimation = this.animations[i];
				if (!actorAnimation.\u0006\u000E())
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(Phase.\u001D\u000E()).MethodHandle;
					}
					float b = actorAnimation.\u000D\u000E(this.Index == AbilityPriority.Evasion && actorAnimation.\u0002\u000E());
					this.maxCamStartDelay = Mathf.Max(this.maxCamStartDelay, b);
				}
				if (this.\u0011 == -1)
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
					if (!actorAnimation.cinematicCamera)
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
						this.\u0011 = (int)actorAnimation.playOrderIndex;
					}
				}
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

		internal bool \u001C(ActorData \u001D)
		{
			bool result = false;
			if (this.animations != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(Phase.\u001C(ActorData)).MethodHandle;
				}
				for (int i = 0; i < this.animations.Count; i++)
				{
					ActorAnimation actorAnimation = this.animations[i];
					if (actorAnimation.Actor == \u001D)
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
						if (!actorAnimation.\u0006\u000E())
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
							bool flag = actorAnimation.State >= ActorAnimation.PlaybackState.\u0016;
							bool flag2 = ClientResolutionManager.Get().HitsDoneExecuting(actorAnimation.SeqSource);
							if (flag2)
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
								if (flag)
								{
									goto IL_AA;
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
							result = true;
							break;
						}
					}
					IL_AA:;
				}
			}
			return result;
		}

		internal bool \u001D\u000E(ActorData \u001D)
		{
			if (this.\u0015 != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(Phase.\u001D\u000E(ActorData)).MethodHandle;
				}
				if (this.\u0015.ContainsKey(\u001D.ActorIndex))
				{
					return this.\u0015[\u001D.ActorIndex] > 0;
				}
			}
			return false;
		}

		internal bool \u000E\u000E(ActorData \u001D)
		{
			if (\u001D != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(Phase.\u000E\u000E(ActorData)).MethodHandle;
				}
				if (this.participants != null)
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
					return this.participants.Contains(\u001D.ActorIndex);
				}
			}
			return false;
		}

		internal bool \u001C()
		{
			bool result = false;
			for (int i = 0; i < this.animations.Count; i++)
			{
				ActorAnimation actorAnimation = this.animations[i];
				if (actorAnimation.State != ActorAnimation.PlaybackState.\u0018 && actorAnimation.State != ActorAnimation.PlaybackState.\u0013)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(Phase.OnSerializeHelper(IBitStream)).MethodHandle;
				}
				if (!this.\u0006)
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
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
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
					this.animations.Add(new ActorAnimation(this.\u000F));
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
				this.animations[i].OnSerializeHelper(stream);
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
			if (stream.isWriting)
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
			if ((int)b == 5)
			{
				if (stream.isWriting)
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
					sbyte b7 = (sbyte)this.\u0015.Count;
					if (flag2)
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
						b7 = 0;
						stream.Serialize(ref b7);
					}
					else
					{
						stream.Serialize(ref b7);
						foreach (KeyValuePair<int, int> keyValuePair2 in this.\u0015)
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
					this.\u0015 = new Dictionary<int, int>();
					for (int k = 0; k < (int)b10; k++)
					{
						sbyte b11 = (sbyte)ActorData.s_invalidActorIndex;
						sbyte b12 = -1;
						stream.Serialize(ref b11);
						stream.Serialize(ref b12);
						this.\u0015.Add((int)b11, (int)b12);
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
			if (stream.isWriting)
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
				sbyte b13 = (sbyte)this.participants.Count;
				stream.Serialize(ref b13);
				sbyte b14 = 0;
				while ((int)b14 < (int)b13)
				{
					sbyte b15 = (sbyte)this.participants[(int)b14];
					stream.Serialize(ref b15);
					b14 = (sbyte)((int)b14 + 1);
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
		}

		private void \u001C(List<ActorAnimation> \u001D)
		{
			if (\u001D != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(Phase.\u001C(List<ActorAnimation>)).MethodHandle;
				}
				if (\u001D.Count > 0)
				{
					GameEventManager.TheatricsAbilityHighlightStartArgs theatricsAbilityHighlightStartArgs = new GameEventManager.TheatricsAbilityHighlightStartArgs();
					for (int i = 0; i < \u001D.Count; i++)
					{
						ActorAnimation actorAnimation = \u001D[i];
						theatricsAbilityHighlightStartArgs.m_casters.Add(actorAnimation.Actor);
						if (actorAnimation.HitActorsToDeltaHP != null)
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
							for (int j = 0; j < actorAnimation.HitActorsToDeltaHP.Keys.Count; j++)
							{
								theatricsAbilityHighlightStartArgs.m_targets.Add(actorAnimation.HitActorsToDeltaHP.Keys.ElementAt(j));
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
					List<ActorData> actorsWithMovementHits = ClientResolutionManager.Get().GetActorsWithMovementHits();
					for (int k = 0; k < actorsWithMovementHits.Count; k++)
					{
						if (!theatricsAbilityHighlightStartArgs.m_targets.Contains(actorsWithMovementHits[k]))
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
							theatricsAbilityHighlightStartArgs.m_targets.Add(actorsWithMovementHits[k]);
						}
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
					GameEventManager.Get().FireEvent(GameEventManager.EventType.TheatricsAbilityHighlightStart, theatricsAbilityHighlightStartArgs);
					this.\u0001 = true;
				}
			}
		}

		private void \u000E\u000E()
		{
			GameEventManager.TheatricsAbilityHighlightStartArgs args = new GameEventManager.TheatricsAbilityHighlightStartArgs();
			GameEventManager.Get().FireEvent(GameEventManager.EventType.TheatricsAbilityHighlightStart, args);
			this.\u0001 = false;
		}

		private unsafe ActorData \u001C(int \u001D, out bool \u000E)
		{
			\u000E = false;
			for (int i = 0; i < this.animations.Count; i++)
			{
				if ((int)this.animations[i].playOrderIndex == \u001D)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(Phase.\u001C(int, bool*)).MethodHandle;
					}
					\u000E = this.animations[i].cinematicCamera;
					return this.animations[i].Actor;
				}
			}
			return null;
		}

		private bool \u001C(int \u001D)
		{
			bool flag = false;
			for (int i = 0; i < this.animations.Count; i++)
			{
				ActorAnimation actorAnimation = this.animations[i];
				if (actorAnimation != null)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(Phase.\u001C(int)).MethodHandle;
					}
					if (actorAnimation.HitActorsToDeltaHP != null && (int)actorAnimation.playOrderIndex == \u001D)
					{
						if (!actorAnimation.\u0002\u000E())
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
										for (;;)
										{
											switch (4)
											{
											case 0:
												continue;
											}
											break;
										}
										flag2 = (hitPointsAfterResolutionWithDelta <= 0);
									}
									else
									{
										flag2 = false;
									}
									bool flag3 = flag2;
									if (flag3)
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
										flag = true;
									}
									else
									{
										if (value > 0 && hitPointsAfterResolution <= 0)
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
											if (hitPointsAfterResolutionWithDelta > 0)
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
												flag = true;
												goto IL_15A;
											}
										}
										if (this.\u000F.\u0004(key, value, (int)actorAnimation.SeqSource.RootID))
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
											flag = true;
											if (CameraManager.CamDebugTraceOn)
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
												CameraManager.LogForDebugging("Ragdolling hit on " + key + " when HP is already 0", CameraManager.CameraLogType.None);
											}
										}
									}
									IL_15A:
									if (flag)
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
										if (CameraManager.CamDebugTraceOn)
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

		private bool \u001C(Bounds \u001D, Bounds \u000E, float \u0012, float \u0015, bool \u0016)
		{
			Vector3 center = \u001D.center;
			Vector3 center2 = \u000E.center;
			Vector3 vector = center2 - center;
			vector.y = 0f;
			float magnitude = vector.magnitude;
			if (magnitude <= \u0012)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(Phase.\u001C(Bounds, Bounds, float, float, bool)).MethodHandle;
				}
				bool flag = false;
				Vector3 vector2;
				Vector3 vector3;
				bool flag2 = CameraManager.BoundSidesWithinDistance(\u001D, \u000E, \u0015, out vector2, out vector3);
				if (\u0016)
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
					if (!flag2)
					{
						Bounds bounds = \u001D;
						bounds.Expand(new Vector3(1.5f, 100f, 1.5f));
						if (bounds.Contains(\u000E.center + \u000E.extents))
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
							if (bounds.Contains(\u000E.center - \u000E.extents))
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
						\u001D,
						"\nCompare to Bound: ",
						\u000E
					}), CameraManager.CameraLogType.SimilarBounds);
				}
				return flag2;
			}
			if (CameraManager.CamDebugTraceOn)
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
				CameraManager.LogForDebugging("Not merging bounds, centerDist too far: " + magnitude, CameraManager.CameraLogType.SimilarBounds);
			}
			return false;
		}

		internal unsafe bool \u001C(Turn \u001D, ref bool \u000E, ref bool \u0012)
		{
			this.\u0017 += GameTime.deltaTime;
			this.\u000D += GameTime.deltaTime;
			bool flag = (this.animations.Count != 0 & this.Index == AbilityPriority.Evasion) && !this.\u0002;
			bool flag2 = true;
			for (int i = 0; i < this.animations.Count; i++)
			{
				ActorAnimation actorAnimation = this.animations[i];
				if (actorAnimation.\u000D\u000E(\u001D))
				{
					flag = true;
					if ((int)actorAnimation.playOrderIndex <= this.playOrderIndex)
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
							RuntimeMethodHandle runtimeMethodHandle = methodof(Phase.\u001C(Turn, bool*, bool*)).MethodHandle;
						}
						bool flag3;
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
							flag3 = actorAnimation.\u0005\u000E();
						}
						else
						{
							flag3 = false;
						}
						flag2 = flag3;
					}
					if ((int)actorAnimation.playOrderIndex == this.playOrderIndex)
					{
						bool flag4 = actorAnimation.\u000C\u000E();
						bool flag5;
						if (!\u000E)
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
							flag5 = flag4;
						}
						else
						{
							flag5 = true;
						}
						\u000E = flag5;
						bool flag6;
						if (!\u0012)
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
							flag6 = !flag4;
						}
						else
						{
							flag6 = true;
						}
						\u0012 = flag6;
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
				break;
			}
			if (CameraManager.Get().IsPlayingShotSequence())
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
				this.\u0008 = true;
				return true;
			}
			if (this.\u0008)
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
				this.\u0017 = 0f;
				this.\u0008 = false;
			}
			if (!flag)
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
				return false;
			}
			int num = int.MaxValue;
			int num2 = int.MaxValue;
			for (int j = 0; j < this.animations.Count; j++)
			{
				ActorAnimation actorAnimation2 = this.animations[j];
				if (actorAnimation2 != null)
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
					if (actorAnimation2.State == ActorAnimation.PlaybackState.\u001D)
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
						if ((int)actorAnimation2.playOrderIndex < num)
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
							num = (int)actorAnimation2.playOrderIndex;
							num2 = (int)actorAnimation2.groupIndex;
						}
					}
				}
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
			AbilitiesCamera abilitiesCamera = AbilitiesCamera.Get();
			List<ActorAnimation> list = null;
			float num3;
			if (this.Index == AbilityPriority.Evasion)
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
				num3 = 0.7f;
			}
			else
			{
				num3 = 0.3f;
			}
			float num4 = num3;
			bool flag7;
			if (\u001D.TimeInResolve < num4)
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
				flag7 = (this.\u0017 >= num4);
			}
			else
			{
				flag7 = true;
			}
			bool flag8 = flag7;
			bool flag9;
			if (flag8)
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
				if (flag2)
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
					if (num != this.playOrderIndex)
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
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
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
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				flag13 = !flag12;
			}
			else
			{
				flag13 = false;
			}
			flag10 = flag13;
			if (!flag10 && this.\u0017 > 20f)
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
				if (!flag12)
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
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (this.\u0001)
				{
					this.\u000E\u000E();
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
					for (;;)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
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
						for (;;)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							break;
						}
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
						for (;;)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
						if (actorAnimation3.GetAnimationIndex() <= 0)
						{
							flag15 = actorAnimation3.Actor.IsModelAnimatorDisabled();
						}
					}
					if (actorAnimation3 != null)
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
						if (actorAnimation3.State == ActorAnimation.PlaybackState.\u001D)
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
							if ((int)actorAnimation3.playOrderIndex == num)
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
								bool flag16 = false;
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
									if (!actorAnimation3.IsReadyToPlay_zq(this.Index, false))
									{
										goto IL_550;
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
									if (animator2 == null || animator2.layerCount < 1)
									{
										goto IL_550;
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
									if (!flag15 && !actorModelData2.IsPlayingIdleAnim(false))
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
										if (!actorModelData2.IsPlayingDamageAnim())
										{
											goto IL_550;
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
										if (!TheatricsManager.Get().m_allowAbilityAnimationInterruptHitReaction)
										{
											goto IL_550;
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
									for (;;)
									{
										switch (3)
										{
										case 0:
											continue;
										}
										break;
									}
									if (this.\u0017 > 1f)
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
										if (actorModelData2.IsPlayingKnockdownAnim())
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
											if (!this.\u000C)
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
												string message = actorAnimation3 + " is stuck in knockdown when trying to play animation for ability, forcing idle";
												if (Application.isEditor)
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
													Log.Error(message, new object[0]);
												}
												else
												{
													Log.Warning(message, new object[0]);
												}
												this.\u000C = true;
											}
											animator2.SetBool("TurnStart", true);
											animator2.SetTrigger("ForceIdle");
										}
									}
									if (this.\u0017 > 5f)
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
										if (!this.\u0020)
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
											this.\u0020 = true;
											bool flag18 = animator2.GetCurrentAnimatorStateInfo(0).IsName("Damage");
											int integer = animator2.GetInteger("Attack");
											string text = string.Empty;
											if (actorAnimation3.HitActorsToDeltaHP != null)
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
												using (Dictionary<ActorData, int>.Enumerator enumerator = actorAnimation3.HitActorsToDeltaHP.GetEnumerator())
												{
													while (enumerator.MoveNext())
													{
														KeyValuePair<ActorData, int> keyValuePair = enumerator.Current;
														text += keyValuePair.Key.ToString();
														text += ", ";
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
											}
											object[] array = new object[0xE];
											array[0] = actorAnimation3;
											array[1] = " is not ready to play. Current animation state: ";
											int num5 = 2;
											object obj;
											if (actorModelData2 == null)
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
											array[0xD] = ((actorAnimation3.State == ActorAnimation.PlaybackState.\u001D) ? (", sequences ready: " + actorAnimation3.IsReadyToPlay_zq(this.Index, true)) : string.Empty);
											Log.Error(string.Concat(array), new object[0]);
										}
										if (this.\u0017 > 8f)
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
											Log.Error(actorAnimation3 + " timed out, skipping", new object[0]);
											this.\u0020 = false;
											if (ClientResolutionManager.Get() != null)
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
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				this.\u0009 = (this.\u0018 != num2);
				if (this.\u0004 != num)
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
					if (flag2)
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
						bool flag19;
						Bounds bounds = \u001D.\u0011(this, num, out flag19);
						bool flag20;
						if (this.Index != AbilityPriority.Evasion)
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
							flag20 = (this.Index == AbilityPriority.Combat_Knockback);
						}
						else
						{
							flag20 = true;
						}
						bool flag21 = flag20;
						this.\u001B = false;
						this.\u0005 = false;
						bool useLowPosition = false;
						if (!flag21)
						{
							useLowPosition = this.\u001C(num);
							ActorData actorData = this.\u001C(num, out this.\u0005);
							int num6 = (!(actorData != null)) ? -1 : actorData.ActorIndex;
							if (this.\u001E > 0)
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
								if (num6 == this.\u001E)
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
									if (!this.\u0005)
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
										this.\u001B = true;
									}
								}
							}
						}
						if (!flag19)
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
							bool flag22 = this.\u000F.\u0018 > 0 && this.\u000F.\u0013 == bounds;
							if (this.\u000F.\u0018 > 0)
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
								if (!flag22)
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
									if (!flag21)
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
										bool flag23 = this.\u001C(this.\u000F.\u0013, bounds, abilitiesCamera.m_similarCenterDistThreshold, abilitiesCamera.m_similarBoundSideMaxDiff, abilitiesCamera.m_considerFramingSimilarIfInsidePrevious);
										if (flag23)
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
											bounds = this.\u000F.\u0013;
										}
									}
								}
							}
							bool flag24;
							if (this.Index != AbilityPriority.Evasion)
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
								flag24 = !this.\u0009;
							}
							else
							{
								flag24 = true;
							}
							bool quickerTransition = flag24;
							CameraManager.Get().SetTarget(bounds, quickerTransition, useLowPosition);
							this.\u000F.\u0009 = true;
							if (this.\u000F.\u0013 == bounds)
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
								this.\u0014 = true;
							}
							else
							{
								this.\u0014 = false;
							}
							this.\u000F.\u0013 = bounds;
							if (flag21)
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
								this.\u000F.\u0018 = 0;
							}
							else
							{
								this.\u000F.\u0018++;
							}
						}
						if (num == 0)
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
							CameraManager.Get().OnActionPhaseChange(ActionBufferPhase.Abilities, true);
						}
						this.\u0004 = num;
						this.\u000B = GameTime.time;
						if (flag21)
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
							this.\u001C(list);
						}
						else
						{
							this.\u000E\u000E();
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
								"Cam set target for player order index ",
								num,
								" group ",
								num2,
								" group changed ",
								this.\u0009,
								" timeInResolve = ",
								this.\u000F.TimeInResolve,
								" anticipating CamStartEvent..."
							}));
						}
					}
				}
				if (flag14)
				{
					this.playOrderIndex = num;
					this.\u0018 = num2;
					flag2 = false;
					GameEventManager.TheatricsAbilityAnimationStartArgs theatricsAbilityAnimationStartArgs = new GameEventManager.TheatricsAbilityAnimationStartArgs();
					theatricsAbilityAnimationStartArgs.lastInPhase = (this.playOrderIndex >= this.\u0019);
					GameEventManager.Get().FireEvent(GameEventManager.EventType.TheatricsAbilityAnimationStart, theatricsAbilityAnimationStartArgs);
				}
			}
			AbilityPriority index = this.Index;
			if (index != AbilityPriority.Evasion)
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
				float num7 = abilitiesCamera.m_easeInTime;
				if (!this.\u0005)
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
					if (this.\u001B)
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
						if (this.\u0014)
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
							num7 = 0f;
							goto IL_106F;
						}
					}
					if (this.\u0014)
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
						num7 = abilitiesCamera.m_easeInTimeForSimilarBounds;
					}
					else if (!this.\u0009)
					{
						num7 = abilitiesCamera.m_easeInTimeWithinGroup;
					}
				}
				IL_106F:
				float num8 = (this.\u000B > 0f) ? (this.\u000B + num7) : 0f;
				for (int l = 0; l < this.animations.Count; l++)
				{
					ActorAnimation actorAnimation4 = this.animations[l];
					if (actorAnimation4 != null)
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
						if (actorAnimation4.State == ActorAnimation.PlaybackState.\u001D && (int)actorAnimation4.playOrderIndex == this.playOrderIndex)
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
							float num9 = Mathf.Max(0f, num8 - GameTime.time);
							if (actorAnimation4.GetSymbol0013())
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
								num9 = 0f;
							}
							float num10 = actorAnimation4.\u000D\u000E(false);
							if (actorAnimation4.GetAnimationIndex() == 0)
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
								num10 = 0.35f;
							}
							if (num10 < 0f)
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
								Log.Error("Camera start event delay is negative", new object[0]);
								num10 = 0f;
							}
							if (num9 <= num10)
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
										"Queued ",
										actorAnimation4,
										"\ngroup ",
										actorAnimation4.groupIndex,
										" camStartEventDelay: ",
										num10,
										" easeInTime: ",
										num7,
										" camera bounds similar as last: ",
										this.\u0014,
										" phase ",
										this.Index.ToString()
									}));
								}
								actorAnimation4.method000D000E(\u001D);
								this.\u0017 = 0f;
								this.\u000C = false;
								this.\u001E = ((!(actorAnimation4.Actor != null)) ? -1 : actorAnimation4.Actor.ActorIndex);
								if (this.Index != AbilityPriority.Evasion)
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
									if (this.Index != AbilityPriority.Combat_Knockback)
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
										if (!actorAnimation4.GetSymbol0013())
										{
											this.\u001C(new List<ActorAnimation>
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
			else if (this.playOrderIndex < this.\u0011)
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
				for (int m = 0; m < this.animations.Count; m++)
				{
					ActorAnimation actorAnimation5 = this.animations[m];
					if (actorAnimation5 != null)
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
						if (actorAnimation5.State == ActorAnimation.PlaybackState.\u001D)
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
							if ((int)actorAnimation5.playOrderIndex == this.playOrderIndex)
							{
								actorAnimation5.method000D000E(\u001D);
								this.\u0017 = 0f;
								this.\u000C = false;
							}
						}
					}
				}
			}
			else if (!this.\u0002)
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
				float num11 = Mathf.Max(0.8f, this.maxCamStartDelay);
				if (this.evadeStartTime < 0f)
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
						for (;;)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							break;
						}
						if (actorAnimation6.State == ActorAnimation.PlaybackState.\u001D)
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
							if ((int)actorAnimation6.playOrderIndex == this.playOrderIndex)
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
								bool flag25;
								if (this.Index == AbilityPriority.Evasion)
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
									flag25 = actorAnimation6.\u0002\u000E();
								}
								else
								{
									flag25 = false;
								}
								bool u001D = flag25;
								if (num12 <= GameTime.time + Mathf.Max(1.401298E-45f, GameTime.smoothDeltaTime) + actorAnimation6.\u000D\u000E(u001D))
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
									actorAnimation6.method000D000E(\u001D);
									this.\u0017 = 0f;
									this.\u000C = false;
									if (this.\u0003 == 0f)
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
										this.\u0003 = GameTime.time;
									}
								}
								if (actorAnimation6.\u0020\u000E())
								{
									actorAnimation6.Actor.CurrentlyVisibleForAbilityCast = true;
								}
							}
						}
					}
				}
				if (num12 > 0f && num12 <= GameTime.time)
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
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					for (int num15 = 0; num15 < actors.Count; num15++)
					{
						actors[num15].ForceUpdateIsVisibleToClientCache();
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
					this.\u0002 = true;
					GameEventManager.Get().FireEvent(GameEventManager.EventType.TheatricsEvasionMoveStart, null);
					for (int num16 = 0; num16 < actors.Count; num16++)
					{
						actors[num16].ForceUpdateIsVisibleToClientCache();
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
					if (TheatricsManager.DebugLog)
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
						TheatricsManager.LogForDebugging("Evasion Move Start, MaxCamStartDelay= " + this.maxCamStartDelay);
					}
				}
			}
			return true;
		}
	}
}

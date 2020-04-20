using System;
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
		private int \u001D;

		internal List<Phase> \u000E = new List<Phase>(7);

		private int \u0012 = -1;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private float \u0015;

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private float \u0016;

		internal Bounds \u0013;

		internal int \u0018;

		internal bool \u0009;

		private HashSet<int> \u0019 = new HashSet<int>();

		internal Turn()
		{
		}

		internal int TurnID { get; private set; }

		internal float TimeInPhase { get; private set; }

		internal float TimeInResolve { get; private set; }

		internal static bool IsEvasionOrKnockback(AbilityPriority priority)
		{
			return priority == AbilityPriority.Evasion || priority == AbilityPriority.Combat_Knockback;
		}

		internal void \u0011(IBitStream \u001D)
		{
			int turnID = this.TurnID;
			\u001D.Serialize(ref turnID);
			this.TurnID = turnID;
			sbyte b = (sbyte)this.\u000E.Count;
			\u001D.Serialize(ref b);
			for (int i = 0; i < (int)b; i++)
			{
				while (i >= this.\u000E.Count)
				{
					this.\u000E.Add(new Phase(this));
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
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(Turn.\u0011(IBitStream)).MethodHandle;
				}
				this.\u000E[i].OnSerializeHelper(\u001D);
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

		internal void \u0011(AbilityPriority \u001D)
		{
			if (\u001D != (AbilityPriority)this.\u0012)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(Turn.\u0011(AbilityPriority)).MethodHandle;
				}
				this.TimeInPhase = 0f;
				this.\u0012 = (int)\u001D;
				if (this.\u0012 >= 0)
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
					if (this.\u0012 < this.\u000E.Count)
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
						TheatricsManager.Get().SetAnimatorParamOnAllActors("DecisionPhase", false);
						Phase phase = this.\u000E[this.\u0012];
						phase.\u001D\u000E();
					}
				}
				List<ActorData> actors = GameFlowData.Get().GetActors();
				if (actors != null)
				{
					for (int i = 0; i < actors.Count; i++)
					{
						ActorData actorData = actors[i];
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
							if (actorData.GetHitPointsAfterResolution() <= 0)
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
								if (!actorData.IsModelAnimatorDisabled())
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
									if (!GameplayData.Get().m_resolveDamageBetweenAbilityPhases)
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
										if (!this.\u0004(actorData, 0, -1))
										{
											goto IL_11C;
										}
									}
									actorData.DoVisualDeath(Sequence.CreateImpulseInfoWithActorForward(actorData));
								}
							}
						}
						IL_11C:;
					}
				}
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
					if (\u001D == AbilityPriority.Combat_Damage)
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
							if (!ClientResolutionManager.Get().IsWaitingForActionMessages(\u001D))
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
								ClientResolutionManager.Get().OnCombatPhasePlayDataReceived();
							}
						}
					}
				}
			}
		}

		internal bool \u001A(AbilityPriority \u001D)
		{
			this.TimeInResolve += GameTime.deltaTime;
			if (this.\u0012 >= 7)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(Turn.\u001A(AbilityPriority)).MethodHandle;
				}
				return false;
			}
			bool flag = false;
			bool flag2 = false;
			bool flag3 = true;
			int u = this.\u0012;
			if (u < 0)
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
				Log.Error("Phase index is negative! Code error.", new object[0]);
				return true;
			}
			if (u < this.\u000E.Count)
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
				if (!this.\u000E[u].\u001C(this, ref flag, ref flag2))
				{
					goto IL_99;
				}
			}
			flag3 = false;
			IL_99:
			bool flag4 = this.TimeInPhase >= GameFlowData.Get().m_resolveTimeoutLimit * 0.8f;
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
				string text = ServerClientUtils.GetCurrentActionPhase().ToString();
				Log.Error(string.Concat(new object[]
				{
					"Theatrics: phase: ",
					text,
					" timed out for turn ",
					this.TurnID,
					",  timeline index ",
					this.\u0012
				}), new object[0]);
			}
			bool flag5 = true;
			if (!flag3)
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
				if (!flag4)
				{
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
						if (!flag2)
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
							if (GameFlowData.Get().activeOwnedActorData != null)
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
								if (!GameFlowData.Get().activeOwnedActorData.IsDead())
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
									InterfaceManager.Get().DisplayAlert(StringUtil.TR("HiddenAction", "Global"), Color.white, 2f, false, 0);
								}
							}
							goto IL_282;
						}
					}
					InterfaceManager.Get().CancelAlert(StringUtil.TR("HiddenAction", "Global"));
					IL_282:
					if (!(GameFlowData.Get() == null))
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
						if (GameFlowData.Get().IsResolutionPaused())
						{
							goto IL_2CB;
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
					this.TimeInPhase += GameTime.deltaTime;
					goto IL_2CB;
				}
			}
			flag5 = false;
			TheatricsManager.Get().no_op(string.Concat(new object[]
			{
				"Theatrics: finished timeline index ",
				this.\u0012,
				" with duration ",
				this.TimeInPhase,
				" @absolute time ",
				GameTime.time
			}));
			if (TheatricsManager.DebugLog)
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
				TheatricsManager.LogForDebugging("Phase Finished: " + this.\u0012);
			}
			IL_2CB:
			if (!flag5)
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
				if (!this.\u0004(\u001D) && NetworkClient.active && !this.\u0019.Contains(this.\u0012))
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
					this.\u0019.Add(this.\u0012);
					GameEventManager.Get().FireEvent(GameEventManager.EventType.TheatricsAbilitiesEnd, null);
				}
			}
			return flag5;
		}

		internal void \u0011(ActorData \u001D, UnityEngine.Object \u000E, GameObject \u0012)
		{
			if (this.\u0012 < 0)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(Turn.\u0011(ActorData, UnityEngine.Object, GameObject)).MethodHandle;
				}
				return;
			}
			if (this.\u0012 >= 7)
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
				return;
			}
			Phase phase = this.\u000E[this.\u0012];
			if (phase != null)
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
				for (int i = 0; i < phase.animations.Count; i++)
				{
					ActorAnimation actorAnimation = phase.animations[i];
					if (actorAnimation.Actor == \u001D)
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
						if (actorAnimation.PlaybackState2OrLater_zq)
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
							if (actorAnimation.\u0014\u000E())
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
								actorAnimation.\u000D\u000E(\u001D, \u000E, \u0012);
								if (actorAnimation.ParentAbilitySeqSource != null)
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
									actorAnimation.\u0008\u000E(\u001D, \u000E, \u0012);
								}
								return;
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
			}
		}

		internal void \u0011(Sequence \u001D, ActorData \u000E, ActorModelData.ImpulseInfo \u0012, ActorModelData.RagdollActivation \u0015 = ActorModelData.RagdollActivation.HealthBased)
		{
			if (this.\u0012 >= 7)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(Turn.\u0011(Sequence, ActorData, ActorModelData.ImpulseInfo, ActorModelData.RagdollActivation)).MethodHandle;
				}
				return;
			}
			bool flag = false;
			bool flag2 = false;
			if (this.\u0012 >= 0)
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
				Phase phase = this.\u000E[this.\u0012];
				if (phase != null)
				{
					int i = 0;
					while (i < phase.animations.Count)
					{
						ActorAnimation actorAnimation = phase.animations[i];
						if (!(actorAnimation.Actor == \u001D.Caster))
						{
							goto IL_BC;
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
						if (!actorAnimation.HasSameSequenceSource(\u001D))
						{
							goto IL_BC;
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
						if (actorAnimation.\u000D\u000E(\u001D, \u000E, \u0012, \u0015))
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
							goto IL_106;
						}
						IL_E7:
						i++;
						continue;
						IL_BC:
						if (actorAnimation.Actor == \u000E && !actorAnimation.\u0014\u000E() && actorAnimation.PlaybackState2OrLater_zq)
						{
							flag2 = true;
							goto IL_E7;
						}
						goto IL_E7;
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
			IL_106:
			if (!flag)
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
				if (\u001D.RequestsHitAnimation(\u000E))
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
					ActorModelData actorModelData = \u000E.GetActorModelData();
					if (actorModelData != null)
					{
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
							if (!actorModelData.CanPlayDamageReactAnim())
							{
								goto IL_177;
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
						if (this.\u001A(\u000E))
						{
							\u000E.PlayDamageReactionAnim(\u001D.m_customHitReactTriggerName);
						}
					}
					IL_177:
					if (\u0015 != ActorModelData.RagdollActivation.None)
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
						if (this.\u0004(\u000E, 0, -1))
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
							\u000E.DoVisualDeath(\u0012);
						}
					}
				}
			}
		}

		internal unsafe Bounds \u0011(Phase \u001D, int \u000E, out bool \u0012)
		{
			\u0012 = true;
			bool flag = \u001D == null;
			ActorData actorData = GameFlowData.Get().activeOwnedActorData;
			if (actorData == null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(Turn.\u0011(Phase, int, bool*)).MethodHandle;
				}
				List<ActorData> actors = GameFlowData.Get().GetActors();
				if (actors != null)
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
					if (actors.Count != 0)
					{
						actorData = actors[0];
						goto IL_87;
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
				Log.Error("No actors found to create Abilities Bounds.", new object[0]);
				return default(Bounds);
			}
			IL_87:
			BoardSquare boardSquare = Board.Get().GetBoardSquare(actorData.transform.position);
			Bounds cameraBounds = boardSquare.CameraBounds;
			cameraBounds.center.y = 0f;
			Bounds result = cameraBounds;
			bool flag2 = true;
			int i = 0;
			while (i < this.\u000E.Count)
			{
				Phase phase = this.\u000E[i];
				if (\u001D == null)
				{
					goto IL_103;
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
				if (\u001D == phase)
				{
					for (;;)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						goto IL_103;
					}
				}
				IL_236:
				i++;
				continue;
				IL_103:
				int j = 0;
				while (j < phase.animations.Count)
				{
					ActorAnimation actorAnimation = phase.animations[j];
					if (\u000E < 0)
					{
						goto IL_137;
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
					if ((int)actorAnimation.playOrderIndex == \u000E)
					{
						goto IL_137;
					}
					IL_1F4:
					j++;
					continue;
					IL_137:
					if (!actorAnimation.\u0014\u000E())
					{
						goto IL_1F4;
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
					if (actorAnimation.\u000C\u000E())
					{
						goto IL_1F4;
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
					if (!actorAnimation.GetSymbol0013())
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
						Bounds u = actorAnimation.\u0020;
						if (\u001D.Index == AbilityPriority.Evasion && actorAnimation.Actor != null)
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
							ActorTeamSensitiveData teamSensitiveData_authority = actorAnimation.Actor.TeamSensitiveData_authority;
							if (teamSensitiveData_authority != null)
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
								teamSensitiveData_authority.EncapsulateVisiblePathBound(ref u);
							}
						}
						if (flag2)
						{
							result = u;
							flag2 = false;
						}
						else
						{
							result.Encapsulate(u);
						}
						\u0012 = false;
						goto IL_1F4;
					}
					goto IL_1F4;
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
				if (!flag)
				{
					goto IL_236;
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
				if (!flag2)
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
					break;
				}
				goto IL_236;
			}
			return result;
		}

		internal bool \u0011()
		{
			return this.\u0004(AbilityPriority.INVALID);
		}

		internal bool \u0004(AbilityPriority \u001D)
		{
			for (int i = (int)(\u001D + 1); i < this.\u000E.Count; i++)
			{
				if (this.\u000B((AbilityPriority)i))
				{
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(Turn.\u0004(AbilityPriority)).MethodHandle;
			}
			return false;
		}

		internal bool \u000B(AbilityPriority \u001D)
		{
			bool result;
			if (\u001D >= AbilityPriority.Prep_Defense && \u001D < (AbilityPriority)this.\u000E.Count)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(Turn.\u000B(AbilityPriority)).MethodHandle;
				}
				result = (this.\u000E[(int)\u001D].animations.Count > 0);
			}
			else
			{
				result = false;
			}
			return result;
		}

		internal bool \u0003(AbilityPriority \u001D)
		{
			if (\u001D >= AbilityPriority.Prep_Defense)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(Turn.\u0003(AbilityPriority)).MethodHandle;
				}
				if (\u001D < (AbilityPriority)this.\u000E.Count)
				{
					return this.\u000E[(int)\u001D].\u001C();
				}
			}
			return false;
		}

		private bool \u0011(ActorData \u001D)
		{
			return this.\u0011(\u001D, this.\u0012);
		}

		internal bool \u0011(ActorData \u001D, int \u000E)
		{
			if (\u001D.HitPoints <= 0)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(Turn.\u0011(ActorData, int)).MethodHandle;
				}
				return true;
			}
			if (\u000E >= 7)
			{
				return \u001D.GetHitPointsAfterResolution() <= 0;
			}
			int num = 0;
			for (int i = 0; i <= \u000E; i++)
			{
				Dictionary<int, int> u001C = this.\u000E[i].ActorIndexToDeltaHP;
				if (u001C != null)
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
					if (u001C.ContainsKey(\u001D.ActorIndex))
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
						num += u001C[\u001D.ActorIndex];
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
			return \u001D.HitPoints + \u001D.AbsorbPoints + num <= 0;
		}

		internal bool \u001A(ActorData \u001D)
		{
			if (\u001D.IsModelAnimatorDisabled())
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(Turn.\u001A(ActorData)).MethodHandle;
				}
				return false;
			}
			if (this.\u0012 > 0)
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
				if (this.\u0012 < this.\u000E.Count)
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
					List<ActorAnimation> animations = this.\u000E[this.\u0012].animations;
					for (int i = 0; i < animations.Count; i++)
					{
						ActorAnimation actorAnimation = animations[i];
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
							if (actorAnimation.PlaybackState2OrLater_zq)
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
								if (!actorAnimation.AnimationFinished)
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
									return false;
								}
							}
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
			}
			return true;
		}

		internal bool \u0004(ActorData \u001D, int \u000E = 0, int \u0012 = -1)
		{
			if (\u001D.GetHitPointsAfterResolution() + \u000E <= 0)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(Turn.\u0004(ActorData, int, int)).MethodHandle;
				}
				if (!\u001D.IsModelAnimatorDisabled())
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
					if (this.\u0011(\u001D))
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
						if (this.\u0012 >= 3)
						{
							int num = this.\u0012;
							int u = this.\u0012;
							do
							{
								if (num < this.\u000E.Count)
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
									if (num >= 0)
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
										if (num == 5 && this.\u000E[num].\u001D\u000E(\u001D))
										{
											goto IL_A8;
										}
										List<ActorAnimation> animations = this.\u000E[num].animations;
										int i = 0;
										while (i < animations.Count)
										{
											ActorAnimation actorAnimation = animations[i];
											if (\u0012 < 0)
											{
												goto IL_F2;
											}
											if ((ulong)actorAnimation.SeqSource.RootID != (ulong)((long)\u0012))
											{
												for (;;)
												{
													switch (2)
													{
													case 0:
														continue;
													}
													goto IL_F2;
												}
											}
											IL_132:
											i++;
											continue;
											IL_F2:
											if (actorAnimation.Actor == \u001D)
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
												if (actorAnimation.\u0014\u000E())
												{
													return false;
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
											if (actorAnimation.\u0008\u000E(\u001D))
											{
												return false;
											}
											goto IL_132;
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
										if (num > u)
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
											if (this.\u000E[num].\u000E\u000E(\u001D))
											{
												goto IL_171;
											}
										}
									}
								}
								num++;
								if (num >= 7)
								{
									goto IL_1A8;
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
							while (!GameplayData.Get().m_resolveDamageBetweenAbilityPhases);
							for (;;)
							{
								switch (3)
								{
								case 0:
									continue;
								}
								goto IL_1A8;
							}
							for (;;)
							{
								IL_A8:
								switch (2)
								{
								case 0:
									continue;
								}
								break;
							}
							return false;
							for (;;)
							{
								IL_171:
								switch (2)
								{
								case 0:
									continue;
								}
								break;
							}
							return false;
							IL_1A8:
							if (ClientResolutionManager.Get() != null)
							{
								bool flag = ClientResolutionManager.Get().HasUnexecutedHitsOnActor(\u001D, \u0012);
								if (flag)
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
							}
							return true;
						}
					}
				}
			}
			return false;
		}

		public bool \u001A()
		{
			if (this.\u0012 < this.\u000E.Count)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(Turn.\u001A()).MethodHandle;
				}
				if (this.\u0012 >= 0)
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
					List<ActorAnimation> animations = this.\u000E[this.\u0012].animations;
					for (int i = 0; i < animations.Count; i++)
					{
						if (animations[i].cinematicCamera)
						{
							if (animations[i].State != ActorAnimation.PlaybackState.\u0012)
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
								if (animations[i].State != ActorAnimation.PlaybackState.\u0015)
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
									if (animations[i].State != ActorAnimation.PlaybackState.\u0016)
									{
										goto IL_C3;
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
							return true;
						}
						IL_C3:;
					}
				}
			}
			return false;
		}
	}
}

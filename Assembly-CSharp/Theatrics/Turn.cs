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
		private int symbol_001D;

		internal List<Phase> symbol_000E = new List<Phase>(7);

		private int symbol_0012 = -1;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private float symbol_0015;

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private float symbol_0016;

		internal Bounds symbol_0013;

		internal int symbol_0018;

		internal bool symbol_0009;

		private HashSet<int> symbol_0019 = new HashSet<int>();

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

		internal void symbol_0011(IBitStream symbol_001D)
		{
			int turnID = this.TurnID;
			symbol_001D.Serialize(ref turnID);
			this.TurnID = turnID;
			sbyte b = (sbyte)this.symbol_000E.Count;
			symbol_001D.Serialize(ref b);
			for (int i = 0; i < (int)b; i++)
			{
				while (i >= this.symbol_000E.Count)
				{
					this.symbol_000E.Add(new Phase(this));
				}
				this.symbol_000E[i].OnSerializeHelper(symbol_001D);
			}
		}

		internal void symbol_0011(AbilityPriority symbol_001D)
		{
			if (symbol_001D != (AbilityPriority)this.symbol_0012)
			{
				this.TimeInPhase = 0f;
				this.symbol_0012 = (int)symbol_001D;
				if (this.symbol_0012 >= 0)
				{
					if (this.symbol_0012 < this.symbol_000E.Count)
					{
						TheatricsManager.Get().SetAnimatorParamOnAllActors("DecisionPhase", false);
						Phase phase = this.symbol_000E[this.symbol_0012];
						phase.symbol_001Dsymbol_000E();
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
							if (actorData.GetHitPointsAfterResolution() <= 0)
							{
								if (!actorData.IsModelAnimatorDisabled())
								{
									if (!GameplayData.Get().m_resolveDamageBetweenAbilityPhases)
									{
										if (!this.symbol_0004(actorData, 0, -1))
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
					if (symbol_001D == AbilityPriority.Combat_Damage)
					{
						if (ClientResolutionManager.Get() != null)
						{
							if (!ClientResolutionManager.Get().IsWaitingForActionMessages(symbol_001D))
							{
								ClientResolutionManager.Get().OnCombatPhasePlayDataReceived();
							}
						}
					}
				}
			}
		}

		internal bool symbol_001A(AbilityPriority symbol_001D)
		{
			this.TimeInResolve += GameTime.deltaTime;
			if (this.symbol_0012 >= 7)
			{
				return false;
			}
			bool flag = false;
			bool flag2 = false;
			bool flag3 = true;
			int u = this.symbol_0012;
			if (u < 0)
			{
				Log.Error("Phase index is negative! Code error.", new object[0]);
				return true;
			}
			if (u < this.symbol_000E.Count)
			{
				if (!this.symbol_000E[u].symbol_001C(this, ref flag, ref flag2))
				{
					goto IL_99;
				}
			}
			flag3 = false;
			IL_99:
			bool flag4 = this.TimeInPhase >= GameFlowData.Get().m_resolveTimeoutLimit * 0.8f;
			if (flag4)
			{
				string text = ServerClientUtils.GetCurrentActionPhase().ToString();
				Log.Error(string.Concat(new object[]
				{
					"Theatrics: phase: ",
					text,
					" timed out for turn ",
					this.TurnID,
					",  timeline index ",
					this.symbol_0012
				}), new object[0]);
			}
			bool flag5 = true;
			if (!flag3)
			{
				if (!flag4)
				{
					if (flag)
					{
						if (!flag2)
						{
							if (GameFlowData.Get().activeOwnedActorData != null)
							{
								if (!GameFlowData.Get().activeOwnedActorData.IsDead())
								{
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
						if (GameFlowData.Get().IsResolutionPaused())
						{
							goto IL_2CB;
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
				this.symbol_0012,
				" with duration ",
				this.TimeInPhase,
				" @absolute time ",
				GameTime.time
			}));
			if (TheatricsManager.DebugLog)
			{
				TheatricsManager.LogForDebugging("Phase Finished: " + this.symbol_0012);
			}
			IL_2CB:
			if (!flag5)
			{
				if (!this.symbol_0004(symbol_001D) && NetworkClient.active && !this.symbol_0019.Contains(this.symbol_0012))
				{
					this.symbol_0019.Add(this.symbol_0012);
					GameEventManager.Get().FireEvent(GameEventManager.EventType.TheatricsAbilitiesEnd, null);
				}
			}
			return flag5;
		}

		internal void symbol_0011(ActorData symbol_001D, UnityEngine.Object symbol_000E, GameObject symbol_0012)
		{
			if (this.symbol_0012 < 0)
			{
				return;
			}
			if (this.symbol_0012 >= 7)
			{
				return;
			}
			Phase phase = this.symbol_000E[this.symbol_0012];
			if (phase != null)
			{
				for (int i = 0; i < phase.animations.Count; i++)
				{
					ActorAnimation actorAnimation = phase.animations[i];
					if (actorAnimation.Actor == symbol_001D)
					{
						if (actorAnimation.PlaybackState2OrLater_zq)
						{
							if (actorAnimation.symbol_0014symbol_000E())
							{
								actorAnimation.symbol_000Dsymbol_000E(symbol_001D, symbol_000E, symbol_0012);
								if (actorAnimation.ParentAbilitySeqSource != null)
								{
									actorAnimation.symbol_0008symbol_000E(symbol_001D, symbol_000E, symbol_0012);
								}
								return;
							}
						}
					}
				}
			}
		}

		internal void symbol_0011(Sequence symbol_001D, ActorData symbol_000E, ActorModelData.ImpulseInfo symbol_0012, ActorModelData.RagdollActivation symbol_0015 = ActorModelData.RagdollActivation.HealthBased)
		{
			if (this.symbol_0012 >= 7)
			{
				return;
			}
			bool flag = false;
			bool flag2 = false;
			if (this.symbol_0012 >= 0)
			{
				Phase phase = this.symbol_000E[this.symbol_0012];
				if (phase != null)
				{
					int i = 0;
					while (i < phase.animations.Count)
					{
						ActorAnimation actorAnimation = phase.animations[i];
						if (!(actorAnimation.Actor == symbol_001D.Caster))
						{
							goto IL_BC;
						}
						if (!actorAnimation.HasSameSequenceSource(symbol_001D))
						{
							goto IL_BC;
						}
						if (actorAnimation.symbol_000Dsymbol_000E(symbol_001D, symbol_000E, symbol_0012, symbol_0015))
						{
							flag = true;
							goto IL_106;
						}
						IL_E7:
						i++;
						continue;
						IL_BC:
						if (actorAnimation.Actor == symbol_000E && !actorAnimation.symbol_0014symbol_000E() && actorAnimation.PlaybackState2OrLater_zq)
						{
							flag2 = true;
							goto IL_E7;
						}
						goto IL_E7;
					}
				}
			}
			IL_106:
			if (!flag)
			{
				if (symbol_001D.RequestsHitAnimation(symbol_000E))
				{
					ActorModelData actorModelData = symbol_000E.GetActorModelData();
					if (actorModelData != null)
					{
						if (flag2)
						{
							if (!actorModelData.CanPlayDamageReactAnim())
							{
								goto IL_177;
							}
						}
						if (this.symbol_001A(symbol_000E))
						{
							symbol_000E.PlayDamageReactionAnim(symbol_001D.m_customHitReactTriggerName);
						}
					}
					IL_177:
					if (symbol_0015 != ActorModelData.RagdollActivation.None)
					{
						if (this.symbol_0004(symbol_000E, 0, -1))
						{
							symbol_000E.DoVisualDeath(symbol_0012);
						}
					}
				}
			}
		}

		internal unsafe Bounds symbol_0011(Phase symbol_001D, int symbol_000E, out bool symbol_0012)
		{
			symbol_0012 = true;
			bool flag = symbol_001D == null;
			ActorData actorData = GameFlowData.Get().activeOwnedActorData;
			if (actorData == null)
			{
				List<ActorData> actors = GameFlowData.Get().GetActors();
				if (actors != null)
				{
					if (actors.Count != 0)
					{
						actorData = actors[0];
						goto IL_87;
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
			while (i < this.symbol_000E.Count)
			{
				Phase phase = this.symbol_000E[i];
				if (symbol_001D == null)
				{
					goto IL_103;
				}
				if (symbol_001D == phase)
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
					if (symbol_000E < 0)
					{
						goto IL_137;
					}
					if ((int)actorAnimation.playOrderIndex == symbol_000E)
					{
						goto IL_137;
					}
					IL_1F4:
					j++;
					continue;
					IL_137:
					if (!actorAnimation.symbol_0014symbol_000E())
					{
						goto IL_1F4;
					}
					if (actorAnimation.symbol_000Csymbol_000E())
					{
						goto IL_1F4;
					}
					if (!actorAnimation.GetSymbol0013())
					{
						Bounds u = actorAnimation.symbol_0020;
						if (symbol_001D.Index == AbilityPriority.Evasion && actorAnimation.Actor != null)
						{
							ActorTeamSensitiveData teamSensitiveData_authority = actorAnimation.Actor.TeamSensitiveData_authority;
							if (teamSensitiveData_authority != null)
							{
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
						symbol_0012 = false;
						goto IL_1F4;
					}
					goto IL_1F4;
				}
				if (!flag)
				{
					goto IL_236;
				}
				if (!flag2)
				{
					break;
				}
				goto IL_236;
			}
			return result;
		}

		internal bool symbol_0011()
		{
			return this.symbol_0004(AbilityPriority.INVALID);
		}

		internal bool symbol_0004(AbilityPriority symbol_001D)
		{
			for (int i = (int)(symbol_001D + 1); i < this.symbol_000E.Count; i++)
			{
				if (this.symbol_000B((AbilityPriority)i))
				{
					return true;
				}
			}
			return false;
		}

		internal bool symbol_000B(AbilityPriority symbol_001D)
		{
			bool result;
			if (symbol_001D >= AbilityPriority.Prep_Defense && symbol_001D < (AbilityPriority)this.symbol_000E.Count)
			{
				result = (this.symbol_000E[(int)symbol_001D].animations.Count > 0);
			}
			else
			{
				result = false;
			}
			return result;
		}

		internal bool symbol_0003(AbilityPriority symbol_001D)
		{
			if (symbol_001D >= AbilityPriority.Prep_Defense)
			{
				if (symbol_001D < (AbilityPriority)this.symbol_000E.Count)
				{
					return this.symbol_000E[(int)symbol_001D].symbol_001C();
				}
			}
			return false;
		}

		private bool symbol_0011(ActorData symbol_001D)
		{
			return this.symbol_0011(symbol_001D, this.symbol_0012);
		}

		internal bool symbol_0011(ActorData symbol_001D, int symbol_000E)
		{
			if (symbol_001D.HitPoints <= 0)
			{
				return true;
			}
			if (symbol_000E >= 7)
			{
				return symbol_001D.GetHitPointsAfterResolution() <= 0;
			}
			int num = 0;
			for (int i = 0; i <= symbol_000E; i++)
			{
				Dictionary<int, int> u001C = this.symbol_000E[i].ActorIndexToDeltaHP;
				if (u001C != null)
				{
					if (u001C.ContainsKey(symbol_001D.ActorIndex))
					{
						num += u001C[symbol_001D.ActorIndex];
					}
				}
			}
			return symbol_001D.HitPoints + symbol_001D.AbsorbPoints + num <= 0;
		}

		internal bool symbol_001A(ActorData symbol_001D)
		{
			if (symbol_001D.IsModelAnimatorDisabled())
			{
				return false;
			}
			if (this.symbol_0012 > 0)
			{
				if (this.symbol_0012 < this.symbol_000E.Count)
				{
					List<ActorAnimation> animations = this.symbol_000E[this.symbol_0012].animations;
					for (int i = 0; i < animations.Count; i++)
					{
						ActorAnimation actorAnimation = animations[i];
						if (actorAnimation.Actor == symbol_001D)
						{
							if (actorAnimation.PlaybackState2OrLater_zq)
							{
								if (!actorAnimation.AnimationFinished)
								{
									return false;
								}
							}
						}
					}
				}
			}
			return true;
		}

		internal bool symbol_0004(ActorData symbol_001D, int symbol_000E = 0, int symbol_0012 = -1)
		{
			if (symbol_001D.GetHitPointsAfterResolution() + symbol_000E <= 0)
			{
				if (!symbol_001D.IsModelAnimatorDisabled())
				{
					if (this.symbol_0011(symbol_001D))
					{
						if (this.symbol_0012 >= 3)
						{
							int num = this.symbol_0012;
							int u = this.symbol_0012;
							do
							{
								if (num < this.symbol_000E.Count)
								{
									if (num >= 0)
									{
										if (num == 5 && this.symbol_000E[num].symbol_001Dsymbol_000E(symbol_001D))
										{
											goto IL_A8;
										}
										List<ActorAnimation> animations = this.symbol_000E[num].animations;
										int i = 0;
										while (i < animations.Count)
										{
											ActorAnimation actorAnimation = animations[i];
											if (symbol_0012 < 0)
											{
												goto IL_F2;
											}
											if ((ulong)actorAnimation.SeqSource.RootID != (ulong)((long)symbol_0012))
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
											if (actorAnimation.Actor == symbol_001D)
											{
												if (actorAnimation.symbol_0014symbol_000E())
												{
													return false;
												}
											}
											if (actorAnimation.symbol_0008symbol_000E(symbol_001D))
											{
												return false;
											}
											goto IL_132;
										}
										if (num > u)
										{
											if (this.symbol_000E[num].symbol_000Esymbol_000E(symbol_001D))
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
								bool flag = ClientResolutionManager.Get().HasUnexecutedHitsOnActor(symbol_001D, symbol_0012);
								if (flag)
								{
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

		public bool symbol_001A()
		{
			if (this.symbol_0012 < this.symbol_000E.Count)
			{
				if (this.symbol_0012 >= 0)
				{
					List<ActorAnimation> animations = this.symbol_000E[this.symbol_0012].animations;
					for (int i = 0; i < animations.Count; i++)
					{
						if (animations[i].cinematicCamera)
						{
							if (animations[i].State != ActorAnimation.PlaybackState.symbol_0012)
							{
								if (animations[i].State != ActorAnimation.PlaybackState.symbol_0015)
								{
									if (animations[i].State != ActorAnimation.PlaybackState.symbol_0016)
									{
										goto IL_C3;
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

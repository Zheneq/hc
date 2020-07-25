using CameraManagerInternal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Networking;

namespace Theatrics
{
	public class ActorAnimation : IComparable<ActorAnimation>
	{
		internal enum PlaybackState
		{
			A,
			B,
			C,
			D,
			ANIMATION_FINISHED,  // ANIM_HITS_DONE?
			F,
			FINISHED
		}

		public const float _001D = 3f;

		public short animationIndex;

		public Vector3 targetPos;

		public bool _0015;  // knockback?

		public int tauntNumber;

		public bool _0013;

		public bool _0018;

		public bool reveal;

		public AbilityData.ActionType actionType = AbilityData.ActionType.INVALID_ACTION;

		private bool IsHanging;

		private bool StartedPlayingAbilityAnim;

		private bool TimedOut;

		private Ability ability;

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private SequenceSource _0003;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private SequenceSource _000F;

		public int actorIndex = ActorData.s_invalidActorIndex;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private Dictionary<ActorData, int> _000D;

		public bool cinematicCamera;

		internal int tauntAnimIndex;

		public sbyte playOrderIndex;

		public sbyte groupIndex;

		public Bounds bounds;

		public List<byte> _000C_X = new List<byte>();

		public List<byte> _0014_Z = new List<byte>();

		private bool _0005;

		private AbilityRequest _001B;

		private Turn turn;

		private bool _0001;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private bool _001F;

		private bool TravelBoardSquareVisible;

		private float _0007;

		private float CurrentTimePlaying;

		private float _001D_000E;

		private float _000E_000E;

		private bool _0012_000E;

		private float TimePlayingAbilityAnim;

		private bool AnimHitsDone;

		internal Bounds _0013_000E;

		private List<string> AnimationEventsSeen = new List<string>();

		private PlaybackState playbackSate;

		private static readonly int DistToGoalHash = Animator.StringToHash("DistToGoal");

		private static readonly int StartDamageReactionHash = Animator.StringToHash("StartDamageReaction");

		private static readonly int AttackHash = Animator.StringToHash("Attack");

		private static readonly int CinematicCamHash = Animator.StringToHash("CinematicCam");

		private static readonly int TauntNumberHash = Animator.StringToHash("TauntNumber");

		private static readonly int TauntAnimIndexHash = Animator.StringToHash("TauntAnimIndex");

		private static readonly int StartAttackHash = Animator.StringToHash("StartAttack");

		private const float _0017_000E = 1f;

		public SequenceSource SeqSource
		{
			get;
			set;
		}

		public SequenceSource ParentAbilitySeqSource
		{
			get;
			set;
		}

		public ActorData Actor
		{
			get
			{
				if (actorIndex != ActorData.s_invalidActorIndex)
				{
					if (!(GameFlowData.Get() == null))
					{
						return GameFlowData.Get().FindActorByActorIndex(actorIndex);
					}
				}
				return null;
			}
			set
			{
				if (value == null)
				{
					if (actorIndex != ActorData.s_invalidActorIndex)
					{
						while (true)
						{
							switch (6)
							{
							case 0:
								break;
							default:
								actorIndex = ActorData.s_invalidActorIndex;
								return;
							}
						}
					}
				}
				if (!(value != null))
				{
					return;
				}
				while (true)
				{
					if (value.ActorIndex != actorIndex)
					{
						actorIndex = value.ActorIndex;
					}
					return;
				}
			}
		}

		public Dictionary<ActorData, int> HitActorsToDeltaHP
		{
			get;
			private set;
		}

		public int TauntNumber
		{
			get
			{
				return tauntNumber;
			}
			private set
			{
			}
		}

		internal bool AnimationFinished
		{
			get;
			private set;
		}

		internal PlaybackState State
		{
			get
			{
				return playbackSate;
			}
			set
			{
				if (value == playbackSate)
				{
					return;
				}
				if (ability != null)
				{
					if (value == PlaybackState.C)
					{
						int techPointRewardForInteraction = AbilityUtils.GetTechPointRewardForInteraction(ability, AbilityInteractionType.Cast, true);
						techPointRewardForInteraction = AbilityUtils.CalculateTechPointsForTargeter(Actor, ability, techPointRewardForInteraction);
						if (techPointRewardForInteraction > 0)
						{
							Actor.AddCombatText(techPointRewardForInteraction.ToString(), string.Empty, CombatTextCategory.TP_Recovery, BuffIconToDisplay.None);
							if (ClientResolutionManager.Get().IsInResolutionState())
							{
								Actor.ClientUnresolvedTechPointGain += techPointRewardForInteraction;
							}
						}
						if (ability.GetModdedCost() > 0)
						{
							if (Actor.ReservedTechPoints > 0)
							{
								int a = Actor.ClientReservedTechPoints - ability.GetModdedCost();
								a = Mathf.Max(a, -Actor.ReservedTechPoints);
								Actor.ClientReservedTechPoints = a;
							}
						}
					}
				}
				if (TheatricsManager.DebugLog)
				{
					TheatricsManager.LogForDebugging(string.Concat(ToString(), " PlayState: <color=cyan>", playbackSate, "</color> -> <color=cyan>", value, "</color>"));
				}
				if (value != PlaybackState.F)
				{
					if (value != PlaybackState.FINISHED)
					{
						goto IL_01c8;
					}
				}
				if (Actor != null)
				{
					Actor.CurrentlyVisibleForAbilityCast = false;
				}
				goto IL_01c8;
				IL_01c8:
				playbackSate = value;
			}
		}

		internal bool PlaybackState2OrLater_zq => State >= PlaybackState.C;

		public ActorAnimation(Turn turn)
		{
			this.turn = turn;
		}

		internal Ability GetAbility()
		{
			return ability;
		}

		private bool StartFinalPlaybackState()
		{
			if (Actor == null)
			{
				Log.Error("Theatrics: can't start {0} since the actor can no longer be found. Was the actor destroyed during resolution?", this);
				State = PlaybackState.FINISHED;
				return false;
			}
			return State == PlaybackState.FINISHED;
		}

		internal void OnSerializeHelper(IBitStream stream)
		{
			sbyte _animationIndex = (sbyte)animationIndex;
			sbyte _actionType = (sbyte)actionType;
			float _targetPosX = targetPos.x;
			float _targetPosZ = targetPos.z;
			sbyte _actorIndex = (sbyte)(Actor?.ActorIndex ?? ActorData.s_invalidActorIndex);
			bool _cinematicCamera = cinematicCamera;
			sbyte _tauntNumber = (sbyte)tauntNumber;
			bool value8 = _0013;
			bool value9 = _0018;
			bool _reveal = reveal;
			bool value11 = _0015;
			sbyte _playOrderIndex = playOrderIndex;
			sbyte _groupIndex = groupIndex;
			Vector3 center = bounds.center;
			Vector3 size = bounds.size;
			byte value14 = checked((byte)_000C_X.Count);
			stream.Serialize(ref _animationIndex);
			stream.Serialize(ref _actionType);
			stream.Serialize(ref _targetPosX);
			stream.Serialize(ref _targetPosZ);
			stream.Serialize(ref _actorIndex);
			stream.Serialize(ref _cinematicCamera);
			stream.Serialize(ref _tauntNumber);
			stream.Serialize(ref value8);
			stream.Serialize(ref value9);
			stream.Serialize(ref _reveal);
			stream.Serialize(ref value11);
			stream.Serialize(ref _playOrderIndex);
			stream.Serialize(ref _groupIndex);
			short _centerX = (short)Mathf.RoundToInt(center.x);
			short _centerZ = (short)Mathf.RoundToInt(center.z);
			stream.Serialize(ref _centerX);
			stream.Serialize(ref _centerZ);
			if (stream.isReading)
			{
				center.x = _centerX;
				center.y = 1.5f + (float)Board.Get().BaselineHeight;
				center.z = _centerZ;
			}
			short _sizeX = (short)Mathf.CeilToInt(size.x + 0.5f);
			short _sizeZ = (short)Mathf.CeilToInt(size.z + 0.5f);
			stream.Serialize(ref _sizeX);
			stream.Serialize(ref _sizeZ);
			if (stream.isReading)
			{
				size.x = _sizeX;
				size.y = 3f;
				size.z = _sizeZ;
			}
			stream.Serialize(ref value14);
			if (stream.isReading)
			{
				for (int i = 0; i < value14; i++)
				{
					byte value19 = 0;
					byte value20 = 0;
					stream.Serialize(ref value19);
					stream.Serialize(ref value20);
					_000C_X.Add(value19);
					_0014_Z.Add(value20);
				}
			}
			else
			{
				for (int j = 0; j < value14; j++)
				{
					byte value21 = _000C_X[j];
					byte value22 = _0014_Z[j];
					stream.Serialize(ref value21);
					stream.Serialize(ref value22);
				}
			}
			animationIndex = _animationIndex;
			if (stream.isReading)
			{
				targetPos = new Vector3(_targetPosX, Board.Get().BaselineHeight, _targetPosZ);
			}
			this.actorIndex = _actorIndex;
			cinematicCamera = _cinematicCamera;
			tauntNumber = _tauntNumber;
			_0013 = value8;
			_0018 = value9;
			reveal = _reveal;
			_0015 = value11;
			playOrderIndex = _playOrderIndex;
			groupIndex = _groupIndex;
			bounds = new Bounds(center, size);
			actionType = (AbilityData.ActionType)_actionType;
			ability = ((!(Actor == null)) ? Actor.GetAbilityData().GetAbilityOfActionType(actionType) : null);
			if (SeqSource == null)
			{
				SeqSource = new SequenceSource();
			}
			SeqSource.OnSerializeHelper(stream);
			if (stream.isWriting)
			{
				bool value23 = ParentAbilitySeqSource != null;
				stream.Serialize(ref value23);
				if (value23)
				{
					ParentAbilitySeqSource.OnSerializeHelper(stream);
				}
			}
			if (stream.isReading)
			{
				bool value24 = false;
				stream.Serialize(ref value24);
				if (value24)
				{
					if (ParentAbilitySeqSource == null)
					{
						ParentAbilitySeqSource = new SequenceSource();
					}
					ParentAbilitySeqSource.OnSerializeHelper(stream);
				}
				else
				{
					ParentAbilitySeqSource = null;
				}
			}
			int num = HitActorsToDeltaHP?.Count ?? 0;
			sbyte value25 = checked((sbyte)num);
			stream.Serialize(ref value25);
			if (value25 > 0)
			{
				if (HitActorsToDeltaHP == null)
				{
					HitActorsToDeltaHP = new Dictionary<ActorData, int>();
				}
			}
			if (stream.isWriting)
			{
				if (value25 > 0)
				{
					using (Dictionary<ActorData, int>.Enumerator enumerator = HitActorsToDeltaHP.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							KeyValuePair<ActorData, int> current = enumerator.Current;
							int s_invalidActorIndex2;
							if (current.Key == null)
							{
								s_invalidActorIndex2 = ActorData.s_invalidActorIndex;
							}
							else
							{
								s_invalidActorIndex2 = current.Key.ActorIndex;
							}
							sbyte value26 = (sbyte)s_invalidActorIndex2;
							if (value26 != ActorData.s_invalidActorIndex)
							{
								sbyte value27 = 0;
								if (current.Value > 0)
								{
									value27 = 1;
								}
								else if (current.Value < 0)
								{
									value27 = -1;
								}
								stream.Serialize(ref value26);
								stream.Serialize(ref value27);
							}
						}
					}
					goto IL_064d;
				}
			}
			if (stream.isReading)
			{
				if (HitActorsToDeltaHP != null)
				{
					HitActorsToDeltaHP.Clear();
				}
				for (int k = 0; k < value25; k++)
				{
					sbyte value28 = (sbyte)ActorData.s_invalidActorIndex;
					sbyte value29 = 0;
					stream.Serialize(ref value28);
					stream.Serialize(ref value29);
					ActorData actorData = GameFlowData.Get().FindActorByActorIndex(value28);
					if (actorData != null)
					{
						HitActorsToDeltaHP[actorData] = value29;
					}
				}
			}
			goto IL_064d;
			IL_064d:
			StartFinalPlaybackState();
		}

		internal bool IsTauntActivated() // _0002_000E
		{
			return tauntNumber > 0;
		}

		internal bool GetSymbol0013()
		{
			return _0013;
		}

		internal bool _0006_000E()
		{
			return _0015;
		}

		internal bool IsActorDamaged(ActorData actor)
		{
			int result;
			if (HitActorsToDeltaHP != null)
			{
				if (HitActorsToDeltaHP.ContainsKey(actor))
				{
					result = ((HitActorsToDeltaHP[actor] < 0) ? 1 : 0);
					goto IL_0043;
				}
			}
			result = 0;
			goto IL_0043;
			IL_0043:
			return (byte)result != 0;
		}

		internal bool _0020_000E_IsActorVisibleForAbilityCast()
		{
			return !Actor.IsDead() && (reveal || cinematicCamera);
		}

		internal bool _000C_000E()
		{
			FogOfWar clientFog = FogOfWar.GetClientFog();
			ActorStatus actorStatus = Actor.GetActorStatus();
			bool flag;
			if (actorStatus != null && actorStatus.HasStatus(StatusType.Revealed))
			{
				flag = true;
			}
			else if (!Actor.VisibleTillEndOfPhase && !Actor.CurrentlyVisibleForAbilityCast)
			{
				flag = _0020_000E_IsActorVisibleForAbilityCast();
			}
			else
			{
				flag = true;
			}
			if (!flag && clientFog != null)
			{
				if (_0018)
				{
					return false;
				}
				if (NetworkClient.active &&
					GameFlowData.Get() != null &&
					GameFlowData.Get().LocalPlayerData != null &&
					Actor.IsHidden(GameFlowData.Get().LocalPlayerData))
				{
					return true;
				}
				for (int i = 0; i < _000C_X.Count; i++)
				{
					BoardSquare boardSquare = Board.Get().GetSquare(_000C_X[i], _0014_Z[i]);
					if (boardSquare != null && clientFog.IsVisible(boardSquare))
					{
						return false;
					}
				}
				ActorMovement actorMovement = Actor.GetActorMovement();
				if ((bool)actorMovement && actorMovement.FindIsVisibleToClient())
				{
					return false;
				}
				if (HitActorsToDeltaHP != null && Board.Get() != null)
				{
					foreach (ActorData key in HitActorsToDeltaHP.Keys)
					{
						if (key != null)
						{
							BoardSquare boardSquare = Board.Get().GetSquare(key.transform.position);
							if (clientFog.IsVisible(boardSquare))
							{
								return false;
							}
						}
					}
				}
				return true;
			}
			return false;
		}

		internal bool HasSameSequenceSource(Sequence sequence)
		{
			return sequence != null && sequence.Source == SeqSource;
		}

		internal bool IsReadyToPlay_zq(AbilityPriority abilityPriority, bool logErrorIfNotReady = false)
		{
			if (State != PlaybackState.A)
			{
				return false;
			}
			bool flag = !ClientResolutionManager.Get().IsWaitingForActionMessages(abilityPriority);
			if (!flag)
			{
				if (logErrorIfNotReady)
				{
					Log.Error("sequences not ready, current client resolution state = {0}", ClientResolutionManager.Get().GetCurrentStateName());
				}
			}
			return flag;
		}

		internal bool _0014_000E_NotFinished()
		{
			return State != PlaybackState.FINISHED && State != PlaybackState.F;
		}

		internal bool _0005_000E()
		{
			int result;
			if (State >= PlaybackState.D)
			{
				if (!_0001)
				{
					if (!AnimationFinished)
					{
						goto IL_00d7;
					}
				}
				if (_0007 > 0f)
				{
					if (NetworkClient.active)
					{
						if (!(GameTime.time >= _0007 + CalcFrameTimeAfterAllHitsButMine()))
						{
							goto IL_00d7;
						}
					}
					if (NetworkClient.active)
					{
						if (_000E_000E > 0f)
						{
							result = ((GameTime.time >= _000E_000E + 1f) ? 1 : 0);
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
					goto IL_00d8;
				}
			}
			goto IL_00d7;
			IL_00d7:
			result = 0;
			goto IL_00d8;
			IL_00d8:
			return (byte)result != 0;
		}

		internal bool IsPendingHitOn(ActorData actor)
		{
			return HitActorsToDeltaHP != null &&
				HitActorsToDeltaHP.ContainsKey(actor) &&
				HitActorsToDeltaHP[actor] != 0 &&
				!SequenceSource.DidSequenceHit(SeqSource, actor);
		}

		private int OtherActorsInHitActorsToDeltaHPNum()
		{
			int result;
			if (HitActorsToDeltaHP == null)
			{
				result = 0;
			}
			else
			{
				int count = HitActorsToDeltaHP.Count;
				int num;
				if (HitActorsToDeltaHP.ContainsKey(Actor))
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
			return AbilitiesCamera.Get().CalcFrameTimeAfterHit(OtherActorsInHitActorsToDeltaHPNum());
		}

		internal void Play(Turn _001D)
		{
			if (ClientAbilityResults.WarningEnabled || TheatricsManager.DebugLog)
			{
				Log.Warning("<color=cyan>ActorAnimation</color> Play for: " + ToString() + " @time= " + GameTime.time);
			}
			_000E_000E = GameTime.time;
			if (State == PlaybackState.FINISHED)
			{
				return;
			}
			bool num;
			if (ability != null)
			{
				num = ability.ShouldRotateToTargetPos();
			}
			else
			{
				num = (animationIndex > 0);
			}
			if (num)
			{
				if (cinematicCamera)
				{
					Actor.TurnToPositionInstant(targetPos);
				}
				else
				{
					Actor.TurnToPosition(targetPos);
				}
			}
			Animator modelAnimator = Actor.GetModelAnimator();
			float num2;
			if (modelAnimator != null)
			{
				if (ability == null)
				{
					num2 = 0f;
				}
				else
				{
					if (ability.GetMovementType() != ActorData.MovementType.None)
					{
						if (ability.GetMovementType() != ActorData.MovementType.Knockback)
						{
							num2 = 10f;
							goto IL_0177;
						}
					}
					num2 = Actor.GetActorMovement().FindDistanceToEnd();
				}
				goto IL_0177;
			}
			goto IL_018f;
			IL_0177:
			float value = num2;
			modelAnimator.SetFloat(DistToGoalHash, value);
			modelAnimator.ResetTrigger(StartDamageReactionHash);
			goto IL_018f;
			IL_018f:
			AbilityData.ActionType actionTypeOfAbility = Actor.GetAbilityData().GetActionTypeOfAbility(ability);
			if (AbilityData.IsCard(actionTypeOfAbility))
			{
				GameEventManager.Get().FireEvent(GameEventManager.EventType.CardUsed, new GameEventManager.CardUsedArgs
				{
					userActor = Actor
				});
				if (HUD_UI.Get() != null)
				{
					HUD_UI.Get().m_mainScreenPanel.m_abilityBar.m_theTimer.m_abilityUsedTracker.AddNewAbility(GetAbility(), Actor);
				}
			}
			else if (!_0015)
			{
				GameEventManager.Get().FireEvent(GameEventManager.EventType.AbilityUsed, new GameEventManager.AbilityUseArgs
				{
					ability = ability,
					userActor = Actor
				});
			}
			if (animationIndex <= 0)
			{
				State = PlaybackState.ANIMATION_FINISHED;
				StartedPlayingAbilityAnim = true;
				AnimationFinished = true;
			}
			if (NetworkServer.active)
			{
				if (NetworkClient.active)
				{
					SequenceManager.Get().DoClientEnable(SeqSource);
				}
			}
			else
			{
				SequenceManager.Get().DoClientEnable(SeqSource);
			}
			if (_0020_000E_IsActorVisibleForAbilityCast())
			{
				Actor.CurrentlyVisibleForAbilityCast = true;
			}
			if (animationIndex <= 0)
			{
				no_op_2();
				UpdateLastEventTime();
			}
			else if (!NetworkClient.active)
			{
				State = PlaybackState.ANIMATION_FINISHED;
				StartedPlayingAbilityAnim = true;
				AnimationFinished = true;
				no_op_2();
				UpdateLastEventTime();
			}
			else
			{
				modelAnimator.SetInteger(AttackHash, animationIndex);
				modelAnimator.SetBool(CinematicCamHash, cinematicCamera);
				if (AnimatorHasParameterName(modelAnimator, "TauntNumber"))
				{
					modelAnimator.SetInteger(TauntNumberHash, tauntNumber);
				}
				modelAnimator.SetTrigger(StartAttackHash);
				if (Actor.GetActorModelData().HasAnimatorControllerParamater("TauntAnimIndex"))
				{
					modelAnimator.SetInteger(TauntAnimIndexHash, tauntAnimIndex);
				}
				if (ability != null)
				{
					ability.OnAbilityAnimationRequest(Actor, animationIndex, cinematicCamera, targetPos);
				}
				if (HUD_UI.Get() != null)
				{
					HUD_UI.Get().m_mainScreenPanel.m_abilityBar.m_theTimer.m_abilityUsedTracker.AddNewAbility(GetAbility(), Actor);
				}
				if (IsTauntActivated())
				{
					ChatterManager.Get().CancelActiveChatter();
				}
				CameraManager.Get().OnAbilityAnimationStart(Actor, animationIndex, targetPos, cinematicCamera, tauntNumber);
				if (Actor != null && cinematicCamera)
				{
					if (NetworkClient.active)
					{
						Actor.ForceUpdateIsVisibleToClientCache();
					}
				}
				if (cinematicCamera)
				{
					if (tauntNumber <= 0)
					{
					}
				}
				no_op_1();
				UpdateLastEventTime();
				State = PlaybackState.C;
			}
			if (!Application.isEditor)
			{
				return;
			}
			if (!CameraManager.CamDebugTraceOn)
			{
				if (!TheatricsManager.DebugLog)
				{
					return;
				}
			}
			ActorDebugUtils._001D(_0013_000E, Color.green, 3f);
		}

		internal bool AnimatorHasParameterName(Animator animator, string parameterName)
		{
			if (animator != null && animator.parameters != null)
			{
				for (int i = 0; i < animator.parameterCount; i++)
				{
					if (animator.parameters[i].name == parameterName)
					{
						return true;
					}
				}
			}
			return false;
		}

		internal bool _000D_000E_Tick(Turn turn)
		{
			Animator animator = null;
			if (NetworkClient.active)
			{
				if (Actor == null || Actor.GetActorModelData() == null)
				{
					State = PlaybackState.FINISHED;
				}
				if (State != PlaybackState.FINISHED)
				{
					animator = Actor.GetModelAnimator();
					if (animator == null || !animator.enabled && animationIndex > 0)
					{
						State = PlaybackState.FINISHED;
					}
				}
			}
			if (State == PlaybackState.FINISHED)
			{
				return false;
			}
			ActorMovement actorMovement = Actor.GetActorMovement();
			bool actorIsNotMoving = !actorMovement.AmMoving();
			if (State >= PlaybackState.C && State < PlaybackState.F)
			{
				bool isHanging = CurrentTimePlaying > 12f;
				if (isHanging && !IsHanging)
				{
					IsHanging = true;
					DebugLogHung(animator, actorIsNotMoving);
				}
				CurrentTimePlaying += GameTime.deltaTime;
				if (NetworkClient.active && State >= PlaybackState.ANIMATION_FINISHED)
				{
					if (!_0012_000E && (_001D_000E >= 7f || isHanging))
					{
						_000D_000E(animator, actorIsNotMoving);
					}
					_001D_000E += GameTime.deltaTime;
				}
			}
			bool timedOut = CurrentTimePlaying > 15f;
			if (timedOut && !TimedOut)
			{
				TimedOut = true;
				Log.Error("Theatrics: animation timed out for {0} {1} after {2} seconds.",
					Actor.DisplayName,
					ability == null ? " animation index " + animationIndex : ability.ToString(),
					CurrentTimePlaying);
			}
			bool isPlayingAttackAnim = animator && Actor.GetActorModelData().IsPlayingAttackAnim(out bool endingAttack);
			if (isPlayingAttackAnim)
			{
				StartedPlayingAbilityAnim = true;
				TimePlayingAbilityAnim += GameTime.deltaTime;
			}
			AnimationFinished = StartedPlayingAbilityAnim && !isPlayingAttackAnim;
			if (_0007 == 0f && State >= PlaybackState.C && State < PlaybackState.F)
			{
				if (_0015 || ClientResolutionManager.Get().HitsDoneExecuting(SeqSource))
				{
					_0007 = GameTime.time;
					if (TheatricsManager.DebugLog)
					{
						TheatricsManager.LogForDebugging(ToString() + " hits done");
					}
				}
			}
			bool flag5 = _0012_000E || _0007 > 0f && GameTime.time - _0007 >= CalcFrameTimeAfterAllHitsButMine();
			switch (State)
			{
			case PlaybackState.A:
				return true;
			case PlaybackState.C:
				if (!isPlayingAttackAnim)
				{
					if (CurrentTimePlaying < 5f)
					{
						return true;
					}
					State = PlaybackState.ANIMATION_FINISHED;
					AnimationFinished = true;
					_000D_000E(animator, actorIsNotMoving);
				}
				if (animator != null)
				{
					animator.SetInteger(AttackHash, 0);
					animator.SetBool(CinematicCamHash, false);
				}
				if (ability != null)
				{
					ability.OnAbilityAnimationRequestProcessed(Actor);
				}
				if (State < PlaybackState.ANIMATION_FINISHED)
				{
					State = PlaybackState.D;
				}
				no_op_1();
				no_op_2();
				if (ClientResolutionManager.Get() != null)
				{
					ClientResolutionManager.Get().OnAbilityCast(Actor, ability);
					ClientResolutionManager.Get().UpdateLastEventTime();
				}
				break;
			case PlaybackState.D:
				if (isPlayingAttackAnim && !_0001)
				{
					break;
				}
				State = PlaybackState.ANIMATION_FINISHED;
				UpdateLastEventTime();
				break;
			case PlaybackState.F:
				return false;
			}
			if (!actorIsNotMoving && !TravelBoardSquareVisible)
			{
				if (actorMovement.OnPathType(BoardSquarePathInfo.ConnectionType.Charge) ||
					actorMovement.OnPathType(BoardSquarePathInfo.ConnectionType.Knockback) ||
					actorMovement.OnPathType(BoardSquarePathInfo.ConnectionType.Flight))
				{
					TravelBoardSquareVisible = FogOfWar.GetClientFog()?.IsVisible(Actor.GetTravelBoardSquare()) ?? false;
					if (TravelBoardSquareVisible)
					{
						Bounds bound = CameraManager.Get().GetTarget();
						if (turn._0009_HasFocusedAction)
						{
							bound.Encapsulate(bounds);
						}
						else
						{
							bound = bounds;
						}
						Actor.GetActorMovement()?.EncapsulatePathInBound(ref bound);
						CameraManager.Get().SetTarget(bound);
						turn._0009_HasFocusedAction = true;
					}
				}
			}
			if (!AnimHitsDone &&
				ServerClientUtils.GetCurrentAbilityPhase() == AbilityPriority.Combat_Knockback &&
				ClientKnockbackManager.Get() != null &&
				flag5 &&
				State >= PlaybackState.ANIMATION_FINISHED)
			{
				ClientKnockbackManager.Get().NotifyOnActorAnimHitsDone(Actor);
				AnimHitsDone = true;
			}
			bool flag3 = !NetworkClient.active || _000E_000E <= 0f || GetSymbol0013() || GameTime.time > _000E_000E + 1f;
			bool flag42 = !isPlayingAttackAnim || _0001 || animationIndex <= 0;
			if ((!flag42 || CameraManager.Get().ShotSequence != null || !actorIsNotMoving || !flag5 || !flag3) && !timedOut)
			{
				return _0014_000E_NotFinished();
			}
			AnimationFinished = true;
			State = PlaybackState.F;
			no_op_1();
			UpdateLastEventTime();
			if (ability != null)
			{
				ability.OnAbilityAnimationReleaseFocus(Actor);
			}
			if (turn._0004_FinishedTheatrics(Actor))
			{
				Actor.DoVisualDeath(new ActorModelData.ImpulseInfo(Actor.GetTravelBoardSquareWorldPositionForLos(), Vector3.up));
			}
			return false;
		}

		internal void _000D_000E(ActorData _001D, UnityEngine.Object _000E, GameObject _0012)
		{
			if (_0012 != null)
			{
				if (_000E.name != null)
				{
					if (_000E.name == "CamEndEvent")
					{
						_0001 = true;
						if (TheatricsManager.DebugLog)
						{
							TheatricsManager.LogForDebugging("CamEndEvent received for " + _000D_000E(string.Empty));
						}
						goto IL_0098;
					}
				}
			}
			SequenceManager.Get().OnAnimationEvent(_001D, _000E, _0012, SeqSource);
			goto IL_0098;
			IL_0098:
			AnimationEventsSeen.Add(_000E.name);
		}

		internal void _0008_000E(ActorData _001D, UnityEngine.Object _000E, GameObject _0012)
		{
			if (!(ParentAbilitySeqSource != null))
			{
				return;
			}
			while (true)
			{
				SequenceManager.Get().OnAnimationEvent(_001D, _000E, _0012, ParentAbilitySeqSource);
				return;
			}
		}

		internal bool _000D_000E(Sequence _001D, ActorData _000E, ActorModelData.ImpulseInfo _0012, ActorModelData.RagdollActivation _0015)
		{
			if (!HasSameSequenceSource(_001D))
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						return false;
					}
				}
			}
			if (_001D.RequestsHitAnimation(_000E))
			{
				if (HitActorsToDeltaHP == null)
				{
					while (true)
					{
						Log.Warning(string.Concat(this, " has sequence ", _001D, " marked Target Hit Animtion, but the ability did not return anything from GatherResults, skipping hit reaction and ragdoll"));
						return true;
					}
				}
				if (!HitActorsToDeltaHP.ContainsKey(_000E))
				{
					while (true)
					{
						Log.Warning(string.Concat(this, " has sequence ", _001D, " with target ", _000E, " but the ability did not return that target from GatherResults, skipping hit reaction and ragdoll"));
						return true;
					}
				}
				ActorModelData actorModelData = _000E.GetActorModelData();
				if (actorModelData != null)
				{
					if (actorModelData.CanPlayDamageReactAnim())
					{
						if (turn._001A_AreAnimationsFinishedFor(_000E))
						{
							_000E.PlayDamageReactionAnim(_001D.m_customHitReactTriggerName);
						}
					}
				}
				if (_0015 != 0)
				{
					if (turn._0004_FinishedTheatrics(_000E))
					{
						_000E.DoVisualDeath(_0012);
						if (_001D.Caster != null)
						{
							if (_001D.Caster != _000E)
							{
								if (!_001D.Caster.IsModelAnimatorDisabled())
								{
									if (_001D.Caster.GetTeam() != _000E.GetTeam())
									{
										GameEventManager.CharacterRagdollHitEventArgs characterRagdollHitEventArgs = new GameEventManager.CharacterRagdollHitEventArgs();
										characterRagdollHitEventArgs.m_ragdollingActor = _000E;
										characterRagdollHitEventArgs.m_triggeringActor = _001D.Caster;
										GameEventManager.CharacterRagdollHitEventArgs args = characterRagdollHitEventArgs;
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

		private void _000D_000E(Animator animator, bool movementPathDone)
		{
			if (_0012_000E || ClientResolutionManager.Get().HitsDoneExecuting(SeqSource))
			{
				return;
			}
			string modIdString;
			if (ability != null && ability.CurrentAbilityMod != null)
			{
				modIdString = "Mod Id: [" + ability.CurrentAbilityMod.m_abilityScopeId + "]\n";
			}
			else
			{
				modIdString = string.Empty;
			}
			string extraInfo = string.Concat(
				modIdString, "Theatrics Entry: ", ToString(), "\n",
				GetDebugStringAnimationEventsSeen(), GetDebugStringDetails(animator, movementPathDone), "\n");
			ClientResolutionManager.Get().ExecuteUnexecutedActions(SeqSource, extraInfo);
			ClientResolutionManager.Get().UpdateLastEventTime();
			_0012_000E = true;

		}

		private void no_op_1()
		{
		}

		private void no_op_2()
		{
		}

		private void UpdateLastEventTime()
		{
			ClientResolutionManager.Get()?.UpdateLastEventTime();
		}

		internal float GetCamStartEventDelay(bool useTauntCamAltTime)
		{
			ActorData actorData = Actor;
			if (actorData == null || actorData.GetActorModelData() == null)
			{
				return 0f;
			}
			return actorData.GetActorModelData().GetCamStartEventDelay(animationIndex, useTauntCamAltTime);
		}

		internal int GetAnimationIndex()
		{
			return animationIndex;
		}

		internal int _000A_000E()
		{
			int s_invalidActorIndex = ActorData.s_invalidActorIndex;
			if (HitActorsToDeltaHP != null)
			{
				if (HitActorsToDeltaHP.Count >= 1)
				{
					if (HitActorsToDeltaHP.Count <= 2)
					{
						for (int i = 0; i < HitActorsToDeltaHP.Count; i++)
						{
							ActorData actorData = HitActorsToDeltaHP.Keys.ElementAt(i);
							if (!(actorData != null))
							{
								continue;
							}
							if (!(Actor != null))
							{
								continue;
							}
							if (Actor.GetTeam() != actorData.GetTeam())
							{
								if (s_invalidActorIndex != ActorData.s_invalidActorIndex)
								{
									s_invalidActorIndex = ActorData.s_invalidActorIndex;
									break;
								}
								s_invalidActorIndex = actorData.ActorIndex;
							}
							else if (actorData != Actor)
							{
								s_invalidActorIndex = ActorData.s_invalidActorIndex;
								break;
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
			if (!(ability == null))
			{
				if (!(rhs.ability == null))
				{
					if (ability.RunPriority != rhs.ability.RunPriority)
					{
						return ability.RunPriority.CompareTo(rhs.ability.RunPriority);
					}
					if (playOrderIndex != rhs.playOrderIndex)
					{
						while (true)
						{
							switch (1)
							{
							case 0:
								break;
							default:
								return playOrderIndex.CompareTo(rhs.playOrderIndex);
							}
						}
					}
					bool flag = GameFlowData.Get().IsActorDataOwned(Actor);
					bool flag2 = GameFlowData.Get().IsActorDataOwned(rhs.Actor);
					if (!ability.IsFreeAction())
					{
						if (!rhs.ability.IsFreeAction())
						{
							if (flag != flag2)
							{
								while (true)
								{
									switch (1)
									{
									case 0:
										break;
									default:
										return flag.CompareTo(flag2);
									}
								}
							}
							if (Actor.ActorIndex != rhs.Actor.ActorIndex)
							{
								return Actor.ActorIndex.CompareTo(rhs.Actor.ActorIndex);
							}
							if (animationIndex != rhs.animationIndex)
							{
								while (true)
								{
									switch (7)
									{
									case 0:
										break;
									default:
										return animationIndex.CompareTo(rhs.animationIndex);
									}
								}
							}
							return 0;
						}
					}
					return -1 * ability.IsFreeAction().CompareTo(rhs.ability.IsFreeAction());
				}
			}
			if (ability == null)
			{
				if (rhs.ability == null)
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							break;
						default:
							return 0;
						}
					}
				}
			}
			if (ability != null)
			{
				if (ability.IsFreeAction())
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							break;
						default:
							return -1;
						}
					}
				}
			}
			if (rhs.ability != null)
			{
				if (rhs.ability.IsFreeAction())
				{
					return 1;
				}
			}
			return (!(ability == null)) ? 1 : (-1);
		}

		private void DebugLogHung(Animator animator, bool movementPathDone)
		{
			Log.Error("Theatrics: {0} {1} is hung", Actor.DisplayName, GetDebugStringDetails(animator, movementPathDone));
		}

		public string GetDebugStringDetails(Animator animator, bool movementPathDone)
		{
			string result = string.Empty;
			if (animator != null && Actor.GetActorModelData() != null)
			{
				int attack = animator.GetInteger("Attack");
				bool cover = animator.GetBool("Cover");
				float distToGoal = animator.GetFloat("DistToGoal");
				int nextLinkType = animator.GetInteger("NextLinkType");
				int curLinkType = animator.GetInteger("CurLinkType");
				bool cinematicCam = animator.GetBool("CinematicCam");
				bool isDecisionPhase = animator.GetBool("DecisionPhase");
				if (animator.GetCurrentAnimatorStateInfo(0).IsName("Damage"))
				{
					object[] obj = new object[9]
					{
						"\nIn ability animation state for ",
						Actor.GetDebugName(),
						" while Damage flag is set (hit react.). Code error, show Chris. debug info: (state: ",
						State.ToString(),
						", Attack: ",
						attack,
						", ability: ",
						null,
						null
					};
					object obj2;
					if (ability == null)
					{
						obj2 = "NULL";
					}
					else
					{
						obj2 = ability.GetActionAnimType().ToString();
					}
					obj[7] = obj2;
					obj[8] = ")";
					result = string.Concat(obj);
				}
				else
				{
					object[] array = new object[35];
					array[0] = "\nIn animation state ";
					array[1] = Actor.GetActorModelData().GetCurrentAnimatorStateName();
					array[2] = " for ";
					array[3] = CurrentTimePlaying;
					array[4] = " sec.\nAfter a request for ability ";
					object obj3;
					if (ability == null)
					{
						obj3 = "NULL";
					}
					else
					{
						obj3 = ability.m_abilityName;
					}
					array[5] = obj3;
					array[6] = ".\nParameters [Attack: ";
					array[7] = attack;
					array[8] = ", Cover: ";
					array[9] = cover;
					array[10] = ", DistToGoal: ";
					array[11] = distToGoal;
					array[12] = ", NextLinkType: ";
					array[13] = nextLinkType;
					array[14] = ", CurLinkType: ";
					array[15] = curLinkType;
					array[16] = ", CinematicCam: ";
					array[17] = cinematicCam;
					array[18] = ", DecisionPhase: ";
					array[19] = isDecisionPhase;
					array[20] = "].\nDetails [state: ";
					array[21] = State.ToString();
					array[22] = ", actor state: ";
					array[23] = Actor.GetActorTurnSM().CurrentState.ToString();
					array[24] = ", movement path done: ";
					array[25] = movementPathDone;
					array[26] = ", ability anim: ";
					object obj4;
					if (ability == null)
					{
						obj4 = "NULL";
					}
					else
					{
						obj4 = ability.GetActionAnimType().ToString();
					}
					array[27] = obj4;
					array[28] = ", ability anim played: ";
					array[29] = StartedPlayingAbilityAnim;
					array[30] = ", time: ";
					array[31] = GameTime.time;
					array[32] = ", turn: ";
					array[33] = GameFlowData.Get().CurrentTurn;
					array[34] = "]";
					result = string.Concat(array);
				}
			}
			else if (NetworkServer.active)
			{
				if (!NetworkClient.active)
				{
					object[] obj5 = new object[8]
					{
						"\nIn ability animation state for ",
						Actor.GetDebugName(),
						", ability: ",
						null,
						null,
						null,
						null,
						null
					};
					object obj6;
					if (ability == null)
					{
						obj6 = "NULL";
					}
					else
					{
						obj6 = ability.GetActionAnimType().ToString();
					}
					obj5[3] = obj6;
					obj5[4] = ", time: ";
					obj5[5] = GameTime.time;
					obj5[6] = ", turn: ";
					obj5[7] = GameFlowData.Get().CurrentTurn;
					result = string.Concat(obj5);
				}
			}
			return result;
		}

		public string GetDebugStringAnimationEventsSeen()
		{
			string text = "Animation Events Seen:\n";
			for (int i = 0; i < AnimationEventsSeen.Count; i++)
			{
				if (AnimationEventsSeen[i] != null)
				{
					text = text + "    [ " + AnimationEventsSeen[i] + " ]\n";
				}
			}
			return text;
		}

		public override string ToString()
		{
			object[] obj = new object[13]
			{
				"[ActorAnimation: ",
				(!(Actor == null)) ? Actor.GetDebugName() : "(NULL caster)",
				" ",
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
			object obj2;
			if (ability == null)
			{
				obj2 = "(NULL ability)";
			}
			else
			{
				obj2 = ability.m_abilityName;
			}
			obj[3] = obj2;
			obj[4] = ", animation index: ";
			obj[5] = animationIndex;
			obj[6] = ", play order index: ";
			obj[7] = playOrderIndex;
			obj[8] = ", group index: ";
			obj[9] = groupIndex;
			obj[10] = ", state: ";
			obj[11] = State;
			obj[12] = "]";
			return string.Concat(obj);
		}

		public string _000D_000E(string _001D = "")
		{
			string[] obj = new string[5]
			{
				"[ActorAnimation: ",
				null,
				null,
				null,
				null
			};
			object obj2;
			if (Actor == null)
			{
				obj2 = "(NULL caster)";
			}
			else
			{
				obj2 = Actor.GetDebugName();
			}
			obj[1] = (string)obj2;
			obj[2] = ", ";
			obj[3] = ((!(ability == null)) ? ability.m_abilityName : "(NULL ability)");
			obj[4] = "]";
			string text = string.Concat(obj);
			if (_001D.Length > 0)
			{
				text = "<color=" + _001D + ">" + text + "</color>";
			}
			return text;
		}
	}
}

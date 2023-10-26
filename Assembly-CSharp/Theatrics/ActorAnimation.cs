// ROGUES
// SERVER
using CameraManagerInternal;
using System;
using System.Collections.Generic;
//using System.Diagnostics;
using System.Linq;
//using System.Runtime.CompilerServices;
//using Mirror;
using UnityEngine;
using UnityEngine.Networking;

namespace Theatrics
{
	public class ActorAnimation : IComparable<ActorAnimation>
	{
		internal enum PlaybackState
		{
			NotStarted,
			NotStartedSkipCasterAnim,
			PlayRequested,
			PlayingAnimation,
			WaitingForTargetHits,
			ReleasedFocus,
			CantBeStarted
		}

		public const float c_minBoundsHeight = 3f;

		private short m_animationIndex;
		private Vector3 m_targetPos;
		private bool m_isTauntForEvadeOrKnockback;
		private int m_cinematicRequested;
		private bool m_ignoreForCameraFraming;
		// removed in rogues
		private bool m_alwaysInLoS;
		// removed in rogues
		private bool m_revealOnCast;
		private AbilityData.ActionType m_abilityActionType = AbilityData.ActionType.INVALID_ACTION;
		private bool m_displayedHungError;
		private bool m_animationPlayed;
		private bool m_displayedTimeoutError;
		private Ability m_ability;
		internal int m_casterIndex = ActorData.s_invalidActorIndex;

		internal bool m_doCinematicCam;
		internal int m_cinematicCamIndex;
		internal sbyte m_playOrderIndex;
		internal sbyte m_playOrderGroupIndex;
		internal Bounds m_bounds;
		private List<byte> m_squaresOfInterestX = new List<byte>();
		private List<byte> m_squaresOfInterestY = new List<byte>();
		private bool m_isAbilityOrItem;
		private AbilityRequest m_abilityRequest;
		private Turn m_turn;
		private bool m_camEndEventReceived;

		private bool m_visibleToClient;
		private float m_hitsDoneExecutingTime;
		private float m_timeSincePlay;
		private float m_timeSinceWaitingForHits;
		private float m_playRequestedTime;
		private bool m_executedUnexecutedHits;
		private float m_timeAnimating;
		private bool m_notifiedClientKnockbackOnHitsDone;

		// added in rogues
#if SERVER
		private float m_clientKnockbackNotifyTime = -1f;
#endif

		internal Bounds m_originalBounds;
		private List<string> m_animEventsSeen = new List<string>();
		private PlaybackState _playState;

		// added in rogues
#if SERVER
		internal EffectResults m_effectResults;
#endif

		private static readonly int DistToGoalHash = Animator.StringToHash("DistToGoal");
		private static readonly int StartDamageReactionHash = Animator.StringToHash("StartDamageReaction");
		private static readonly int AttackHash = Animator.StringToHash("Attack");
		private static readonly int CinematicCamHash = Animator.StringToHash("CinematicCam");
		private static readonly int TauntNumberHash = Animator.StringToHash("TauntNumber");
		private static readonly int TauntAnimIndexHash = Animator.StringToHash("TauntAnimIndex");
		private static readonly int StartAttackHash = Animator.StringToHash("StartAttack");

		private const float c_minTimeBeforeCameraFocusRelease = 1f;

		internal SequenceSource SeqSource { get; private set; }
		internal SequenceSource ParentAbilitySeqSource { get; private set; }

		internal ActorData Caster
		{
			get
			{
				if (m_casterIndex != ActorData.s_invalidActorIndex && GameFlowData.Get() != null)
				{
					return GameFlowData.Get().FindActorByActorIndex(m_casterIndex);
				}
				return null;
			}
			set
			{
				if (value == null && m_casterIndex != ActorData.s_invalidActorIndex)
				{
					m_casterIndex = ActorData.s_invalidActorIndex;
				}
				else if (value != null && value.ActorIndex != m_casterIndex)
				{
					m_casterIndex = value.ActorIndex;
				}
			}
		}

		internal Dictionary<ActorData, int> HitActorsToDeltaHP { get; private set; }
		public int CinematicIndex
		{
			get
			{
				return m_cinematicRequested;
			}
			private set
			{
			}
		}

		internal bool AnimationFinished { get; private set; }
		internal PlaybackState PlayState
		{
			get
			{
				return _playState;
			}
			set
			{
				if (value == _playState)
				{
					return;
				}

#if SERVER
				if (m_abilityRequest != null
					&& m_abilityRequest.m_resolveState != AbilityRequest.AbilityResolveState.RESOLVED)
				{
					if (value != PlaybackState.ReleasedFocus)
					{
						if (value == PlaybackState.CantBeStarted)
						{
							Log.Error($"Can't start, cancelling: {this}, request resolve state was: {m_abilityRequest.m_resolveState}");
							ServerActionBuffer.Get().CancelAbilityRequest(Caster, m_abilityRequest.m_ability, false, true);
						}
					}
					else
					{
						if (m_abilityRequest.m_resolveState == AbilityRequest.AbilityResolveState.QUEUED)
						{
							ServerActionBuffer.Get().TryRunAbilityRequest(m_abilityRequest);
						}
						ServerActionBuffer.Get().ResolveAbilityRequest(m_abilityRequest);
					}
				}
#endif

				if (m_ability != null && value == PlaybackState.PlayRequested)
				{
					int techPointRewardForInteraction = AbilityUtils.GetTechPointRewardForInteraction(m_ability, AbilityInteractionType.Cast, true);
					techPointRewardForInteraction = AbilityUtils.CalculateTechPointsForTargeter(Caster, m_ability, techPointRewardForInteraction);
					if (techPointRewardForInteraction > 0)
					{
						Caster.AddCombatText(techPointRewardForInteraction.ToString(), "", CombatTextCategory.TP_Recovery, BuffIconToDisplay.None);  // , HitChanceBracket.HitType.Normal in rogues
						if (ClientResolutionManager.Get().IsInResolutionState())
						{
							Caster.ClientUnresolvedTechPointGain += techPointRewardForInteraction;
						}
					}
					if (m_ability.GetModdedCost() > 0 && Caster.ReservedTechPoints > 0)
					{
						int a = Caster.ClientReservedTechPoints - m_ability.GetModdedCost();
						a = Mathf.Max(a, -Caster.ReservedTechPoints);
						Caster.ClientReservedTechPoints = a;
					}
				}
				if (TheatricsManager.DebugTraceExecution)
				{
					TheatricsManager.LogForDebugging(string.Concat(ToString(), " PlayState: <color=cyan>", _playState, "</color> -> <color=cyan>", value, "</color>"));
				}

				// removed in rogues
				if ((value == PlaybackState.ReleasedFocus || value == PlaybackState.CantBeStarted)
					&& Caster != null)
				{
					Caster.CurrentlyVisibleForAbilityCast = false;
				}
				// end removed
				_playState = value;
			}
		}

		internal bool Played => PlayState >= PlaybackState.PlayRequested;

		// server-only or rogues-only?
#if SERVER
		internal Effect Effect
		{
			get
			{
				return m_effectResults?.Effect;
			}
		}
#endif

		internal ActorAnimation(Turn turn)
		{
			m_turn = turn;
		}

		internal Ability GetAbility()
		{
			return m_ability;
		}

		private bool InitNonSerializedData()
		{
			if (Caster == null)
			{
				Log.Error("Theatrics: can't start {0} since the actor can no longer be found. Was the actor destroyed during resolution?", this);
				PlayState = PlaybackState.CantBeStarted;
				return false;
			}
			return PlayState == PlaybackState.CantBeStarted;
		}

		internal void OnSerializeHelper(IBitStream stream)
		{
			sbyte _animationIndex = (sbyte)m_animationIndex;
			sbyte _actionType = (sbyte)m_abilityActionType;
			float _targetPosX = m_targetPos.x;
			float _targetPosZ = m_targetPos.z;
			sbyte _actorIndex = (sbyte)(Caster?.ActorIndex ?? ActorData.s_invalidActorIndex);
			bool _cinematicCamera = m_doCinematicCam;
			sbyte _tauntNumber = (sbyte)m_cinematicRequested;
			bool value8 = m_ignoreForCameraFraming;
			// removed in rogues
			bool value9 = m_alwaysInLoS;
			// removed in rogues
			bool _reveal = m_revealOnCast;
			bool value11 = m_isTauntForEvadeOrKnockback;
			sbyte _playOrderIndex = m_playOrderIndex;
			sbyte _groupIndex = m_playOrderGroupIndex;
			Vector3 center = m_bounds.center;
			Vector3 size = m_bounds.size;
			byte _squaresOfInterestNum = checked((byte)m_squaresOfInterestX.Count);
			stream.Serialize(ref _animationIndex);
			stream.Serialize(ref _actionType);
			stream.Serialize(ref _targetPosX);
			stream.Serialize(ref _targetPosZ);
			stream.Serialize(ref _actorIndex);
			stream.Serialize(ref _cinematicCamera);
			stream.Serialize(ref _tauntNumber);
			stream.Serialize(ref value8);
			// removed in rogues
			stream.Serialize(ref value9);
			// removed in rogues
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
			stream.Serialize(ref _squaresOfInterestNum);
			if (stream.isReading)
			{
				for (int i = 0; i < _squaresOfInterestNum; i++)
				{
					byte value19 = 0;
					byte value20 = 0;
					stream.Serialize(ref value19);
					stream.Serialize(ref value20);
					m_squaresOfInterestX.Add(value19);
					m_squaresOfInterestY.Add(value20);
				}
			}
			else
			{
				for (int j = 0; j < _squaresOfInterestNum; j++)
				{
					byte value21 = m_squaresOfInterestX[j];
					byte value22 = m_squaresOfInterestY[j];
					stream.Serialize(ref value21);
					stream.Serialize(ref value22);
				}
			}
			m_animationIndex = _animationIndex;
			if (stream.isReading)
			{
				m_targetPos = new Vector3(_targetPosX, Board.Get().BaselineHeight, _targetPosZ);
			}
			m_casterIndex = _actorIndex;
			m_doCinematicCam = _cinematicCamera;
			m_cinematicRequested = _tauntNumber;
			m_ignoreForCameraFraming = value8;
			m_alwaysInLoS = value9;
			m_revealOnCast = _reveal;
			m_isTauntForEvadeOrKnockback = value11;
			m_playOrderIndex = _playOrderIndex;
			m_playOrderGroupIndex = _groupIndex;
			m_bounds = new Bounds(center, size);
			m_abilityActionType = (AbilityData.ActionType)_actionType;
			m_ability = Caster?.GetAbilityData().GetAbilityOfActionType(m_abilityActionType);

			// server-only?
#if SERVER
			if (m_ability != null)
			{
				m_isAbilityOrItem = true;
			}
#endif

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
			sbyte value25 = checked((sbyte)(HitActorsToDeltaHP?.Count ?? 0));
			stream.Serialize(ref value25);
			if (value25 > 0 && HitActorsToDeltaHP == null)
			{
				HitActorsToDeltaHP = new Dictionary<ActorData, int>();
			}
			if (stream.isWriting && value25 > 0)
			{
				foreach (KeyValuePair<ActorData, int> current in HitActorsToDeltaHP)
				{
					sbyte value26 = (sbyte)((current.Key == null)
						? ActorData.s_invalidActorIndex
						: current.Key.ActorIndex);
					if ((int)value26 != ActorData.s_invalidActorIndex)
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
			else if (stream.isReading)
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
			InitNonSerializedData();
		}

		internal bool IsCinematicRequested()
		{
			return m_cinematicRequested > 0;
		}


		internal bool ShouldIgnoreCameraFraming()
		{
			return m_ignoreForCameraFraming;
		}

		internal bool IsTauntForEvadeOrKnockback()
		{
			return m_isTauntForEvadeOrKnockback;
		}

		internal bool DoesDamage(ActorData actor)
		{
			return HitActorsToDeltaHP != null
				&& HitActorsToDeltaHP.ContainsKey(actor)
				&& HitActorsToDeltaHP[actor] < 0;
		}

		// removed in rogues
		internal bool ForceActorVisibleForAbilityCast()
		{
			return !Caster.IsDead() && (m_revealOnCast || m_doCinematicCam);
		}

		internal bool NotInLoS()
		{
			FogOfWar clientFog = FogOfWar.GetClientFog();
			ActorStatus actorStatus = Caster.GetActorStatus();
			if (actorStatus != null && actorStatus.HasStatus(StatusType.Revealed))
			{
				return false;
			}

			// reactor
			if (Caster.VisibleTillEndOfPhase
				|| Caster.CurrentlyVisibleForAbilityCast
				|| ForceActorVisibleForAbilityCast())
			{
				return false;
			}
			//rogues
			//if (Caster.VisibleTillEndOfPhase)
			//{
			//	return false;
			//}
			if (clientFog == null)
			{
				return false;
			}

			// removed in rogues
			if (m_alwaysInLoS)
			{
				return false;
			}

			if (NetworkClient.active
				&& GameFlowData.Get() != null
				&& GameFlowData.Get().LocalPlayerData != null
				&& Caster.IsNeverVisibleTo(GameFlowData.Get().LocalPlayerData))
			{
				return true;
			}
			for (int i = 0; i < m_squaresOfInterestX.Count; i++)
			{
				BoardSquare boardSquare = Board.Get().GetSquareFromIndex(m_squaresOfInterestX[i], m_squaresOfInterestY[i]);
				if (boardSquare != null && clientFog.IsVisible(boardSquare))
				{
					return false;
				}
			}
			ActorMovement actorMovement = Caster.GetActorMovement();
			if (actorMovement != null && actorMovement.FindIsVisibleToClient())
			{
				return false;
			}
			if (HitActorsToDeltaHP != null && Board.Get() != null)
			{
				foreach (ActorData key in HitActorsToDeltaHP.Keys)
				{
					if (key != null)
					{
						BoardSquare boardSquare = Board.Get().GetSquareFromVec3(key.transform.position);
						if (clientFog.IsVisible(boardSquare))
						{
							return false;
						}
					}
				}
			}
			return true;
		}

		internal bool TriggeredSequence(Sequence sequence)
		{
			return sequence != null && sequence.Source == SeqSource;
		}

		internal bool FindIfSequencesReadyToPlay(AbilityPriority phase, bool log = false)
		{
			if (PlayState != PlaybackState.NotStarted)
			{
				return false;
			}
			bool isReady = !ClientResolutionManager.Get().IsWaitingForActionMessages(phase);
			if (!isReady && log)
			{
				Log.Error("sequences not ready, current client resolution state = {0}", ClientResolutionManager.Get().GetCurrentStateName());
			}
			return isReady;
		}

		internal bool UpdateNotFinished()
		{
			return PlayState != PlaybackState.CantBeStarted
				&& PlayState != PlaybackState.ReleasedFocus;
		}

		internal bool ShouldAutoCameraReleaseFocus()
		{
			return PlayState >= PlaybackState.PlayingAnimation
				&& (m_camEndEventReceived || AnimationFinished)
				&& m_hitsDoneExecutingTime > 0f
				&& (!NetworkClient.active || GameTime.time >= m_hitsDoneExecutingTime + GetTimeToWaitAfterAllHits())
				&& (!NetworkClient.active || m_playRequestedTime > 0f && GameTime.time >= m_playRequestedTime + 1f);
		}

		internal bool DeltaHPPending(ActorData actor)
		{
			return HitActorsToDeltaHP != null
				&& HitActorsToDeltaHP.ContainsKey(actor)
				&& HitActorsToDeltaHP[actor] != 0
				&& !SequenceSource.DidSequenceHit(SeqSource, actor);
		}

		private int GetNumOtherActorsHit()
		{
			if (HitActorsToDeltaHP != null)
			{
				return HitActorsToDeltaHP.Count - (HitActorsToDeltaHP.ContainsKey(Caster) ? 1 : 0);
			}
			return 0;
		}

		private float GetTimeToWaitAfterAllHits()
		{
			// reactor
			return AbilitiesCamera.Get().CalcFrameTimeAfterHit(GetNumOtherActorsHit());
			// rogues
			//return 0f;
		}

		internal void Play(Turn turn)
		{
			if (ClientAbilityResults.DebugTraceOn || TheatricsManager.DebugTraceExecution)
			{
				Log.Warning("<color=cyan>ActorAnimation</color> Play for: " + ToString() + " @time= " + GameTime.time);
			}
			m_playRequestedTime = GameTime.time;
			if (PlayState == PlaybackState.CantBeStarted)
			{
				return;
			}

			// server-only
#if SERVER
			if (Effect != null)
			{
				Effect.OnActorAnimEntryPlay();
			}
#endif
			// reactor
			bool shouldTurnToPosition = m_ability != null ? m_ability.ShouldRotateToTargetPos() : m_animationIndex > 0;
			// rogues
			//bool shouldTurnToPosition = (this.m_ability == null || this.m_ability.ShouldRotateToTargetPos()) && this.m_animationIndex > 0;
			if (shouldTurnToPosition)
			{
				if (m_doCinematicCam)
				{
					Caster.TurnToPositionInstant(m_targetPos);
				}
				else
				{
					Caster.TurnToPosition(m_targetPos);
				}
			}
			Animator modelAnimator = Caster.GetModelAnimator();
			if (modelAnimator != null)
			{
				float distToGoal;
				if (m_ability == null)
				{
					distToGoal = 0f;
				}
				else
				{
					if (m_ability.GetMovementType() != ActorData.MovementType.None && m_ability.GetMovementType() != ActorData.MovementType.Knockback)
					{
						distToGoal = 10f;
					}
					else
					{
						distToGoal = Caster.GetActorMovement().FindDistanceToEnd();
					}
				}
				// reactor
				modelAnimator.SetFloat(DistToGoalHash, distToGoal);
				modelAnimator.ResetTrigger(StartDamageReactionHash);
				// rogues
				//modelAnimator.SetFloat("DistToGoal", distToGoal);
				//modelAnimator.ResetTrigger("StartDamageReaction");
			}
			if (AbilityData.IsCard(Caster.GetAbilityData().GetActionTypeOfAbility(m_ability)))
			{
				GameEventManager.Get().FireEvent(GameEventManager.EventType.CardUsed, new GameEventManager.CardUsedArgs
				{
					userActor = Caster
				});
				if (HUD_UI.Get() != null)
				{
					// reactor
					HUD_UI.Get().m_mainScreenPanel.m_abilityBar.m_theTimer.m_abilityUsedTracker.AddNewAbility(GetAbility(), Caster);
					// rogues
					//HUD_UI.Get().m_mainScreenPanel.m_sideNotificationsPanel.AddUsedActionToNotification(this.GetAbility(), this.Caster);
				}
			}
			else if (!m_isTauntForEvadeOrKnockback)
			{
				GameEventManager.Get().FireEvent(GameEventManager.EventType.AbilityUsed, new GameEventManager.AbilityUseArgs
				{
					ability = m_ability,
					userActor = Caster
				});
			}
			if (m_animationIndex <= 0)
			{
				PlayState = PlaybackState.WaitingForTargetHits;
				m_animationPlayed = true;
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

			// removed in rogues
			if (ForceActorVisibleForAbilityCast())
			{
				Caster.CurrentlyVisibleForAbilityCast = true;
			}

			// server-only
#if SERVER
			if (NetworkServer.active && m_doCinematicCam && Caster.GetActorStatus().IsInvisibleToEnemies())
			{
				ServerActionBuffer.Get().MarkVisibleTillEndOfPhase(Caster);
			}
#endif

			if (m_animationIndex <= 0)
			{
				NotifyKnockbackManagerOnAnimStarted();
				UpdateLastEventTimeForClientResolution();
			}
			else if (!NetworkClient.active)
			{
				PlayState = PlaybackState.WaitingForTargetHits;
				m_animationPlayed = true;
				AnimationFinished = true;
				NotifyKnockbackManagerOnAnimStarted();
				UpdateLastEventTimeForClientResolution();
			}
			else
			{
				modelAnimator.SetInteger(AttackHash, m_animationIndex);
				modelAnimator.SetBool(CinematicCamHash, m_doCinematicCam);
				//modelAnimator.SetInteger("Attack", (int)this.m_animationIndex);
				//modelAnimator.SetBool("CinematicCam", this.m_doCinematicCam);

				if (AnimatorContainsParameter(modelAnimator, "TauntNumber"))
				{
					modelAnimator.SetInteger(TauntNumberHash, m_cinematicRequested);
					//modelAnimator.SetInteger("TauntNumber", m_cinematicRequested);
				}
				modelAnimator.SetTrigger(StartAttackHash);
				//modelAnimator.SetTrigger("StartAttack");
				if (Caster.GetActorModelData().HasAnimatorControllerParamater("TauntAnimIndex"))
				{
					modelAnimator.SetInteger(TauntAnimIndexHash, m_cinematicCamIndex);
					//modelAnimator.SetInteger("TauntAnimIndex", this.m_cinematicCamIndex);
				}
				if (m_ability != null)
				{
					m_ability.OnAbilityAnimationRequest(Caster, m_animationIndex, m_doCinematicCam, m_targetPos);
				}
				if (HUD_UI.Get() != null)
				{
					// reactor
					HUD_UI.Get().m_mainScreenPanel.m_abilityBar.m_theTimer.m_abilityUsedTracker.AddNewAbility(GetAbility(), Caster);
					// rogues
					//HUD_UI.Get().m_mainScreenPanel.m_sideNotificationsPanel.AddUsedActionToNotification(this.GetAbility(), this.Caster);
				}
				if (IsCinematicRequested())
				{
					ChatterManager.Get().CancelActiveChatter();
				}
				CameraManager.Get().OnAbilityAnimationStart(Caster, m_animationIndex, m_targetPos, m_doCinematicCam, m_cinematicRequested);
				if (Caster != null && m_doCinematicCam && NetworkClient.active)
				{
					Caster.ForceUpdateIsVisibleToClientCache();
				}
				// TODO some broken code
				//if (m_doCinematicCam && m_cinematicRequested <= 0)
				//{
				//}
				NotifyKnockbackManagerForTauntsProgress();
				UpdateLastEventTimeForClientResolution();
				PlayState = PlaybackState.PlayRequested;
			}
			if (Application.isEditor && (CameraManager.CamDebugTraceOn || TheatricsManager.DebugTraceExecution))
			{
				ActorDebugUtils.DebugDrawBoundBase(m_originalBounds, Color.green, 3f);
			}
		}

		internal bool AnimatorContainsParameter(Animator animator, string parameterName)
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

		internal bool Update(Turn turn)
		{
			Animator animator = null;
			if (NetworkClient.active)
			{
				if (Caster == null || Caster.GetActorModelData() == null)
				{
					PlayState = PlaybackState.CantBeStarted;
				}
				if (PlayState != PlaybackState.CantBeStarted)
				{
					animator = Caster.GetModelAnimator();
					if (animator == null || !animator.enabled && m_animationIndex > 0)
					{
						PlayState = PlaybackState.CantBeStarted;
					}
				}
			}
			if (PlayState == PlaybackState.CantBeStarted)
			{
				return false;
			}
			ActorMovement actorMovement = Caster.GetActorMovement();
			bool pathDone = !actorMovement.AmMoving();
			if (PlayState >= PlaybackState.PlayRequested && PlayState < PlaybackState.ReleasedFocus)
			{
				bool isHanging = m_timeSincePlay > 12f;
				if (isHanging && !m_displayedHungError)
				{
					m_displayedHungError = true;
					DisplayHungError(animator, pathDone);
				}
				m_timeSincePlay += GameTime.deltaTime;
				if (NetworkClient.active && PlayState >= PlaybackState.WaitingForTargetHits)
				{
					if (!m_executedUnexecutedHits && (m_timeSinceWaitingForHits >= 7f || isHanging))
					{
						ExecuteUnexecutedHitsForClient(animator, pathDone);
					}
					m_timeSinceWaitingForHits += GameTime.deltaTime;
				}
			}
			bool timedOut = m_timeSincePlay > 15f;
			if (timedOut && !m_displayedTimeoutError)
			{
				m_displayedTimeoutError = true;
				Log.Error("Theatrics: animation timed out for {0} {1} after {2} seconds.",
					Caster.DisplayName,
					m_ability == null ? " animation index " + m_animationIndex : m_ability.ToString(),
					m_timeSincePlay);
			}
			bool isPlayingAttackAnim = animator && Caster.GetActorModelData().IsPlayingAttackAnim(out bool endingAttack);
			if (isPlayingAttackAnim)
			{
				m_animationPlayed = true;
				m_timeAnimating += GameTime.deltaTime;
			}
			AnimationFinished = m_animationPlayed && !isPlayingAttackAnim;
			if (m_hitsDoneExecutingTime == 0f
				&& PlayState >= PlaybackState.PlayRequested
				&& PlayState < PlaybackState.ReleasedFocus
				&& (m_isTauntForEvadeOrKnockback || ClientResolutionManager.Get().HitsDoneExecuting(SeqSource)))
			{
				m_hitsDoneExecutingTime = GameTime.time;
				if (TheatricsManager.DebugTraceExecution)
				{
					TheatricsManager.LogForDebugging(ToString() + " hits done");
				}
			}
			bool amNotWaitingForHits = m_executedUnexecutedHits
				|| m_hitsDoneExecutingTime > 0f && GameTime.time - m_hitsDoneExecutingTime >= GetTimeToWaitAfterAllHits();
			switch (PlayState)
			{
				case PlaybackState.NotStarted:
					return true;
				case PlaybackState.PlayRequested:
					if (!isPlayingAttackAnim)
					{
						if (m_timeSincePlay < 5f)
						{
							return true;
						}
						PlayState = PlaybackState.WaitingForTargetHits;
						AnimationFinished = true;
						ExecuteUnexecutedHitsForClient(animator, pathDone);
					}
					if (animator != null)
					{
						animator.SetInteger(AttackHash, 0);
						animator.SetBool(CinematicCamHash, false);
						//	animator.SetInteger("Attack", 0);
						//animator.SetBool("CinematicCam", false);
					}
					if (m_ability != null)
					{
						m_ability.OnAbilityAnimationRequestProcessed(Caster);
					}
					if (PlayState < PlaybackState.WaitingForTargetHits)
					{
						PlayState = PlaybackState.PlayingAnimation;
					}
					NotifyKnockbackManagerForTauntsProgress();
					NotifyKnockbackManagerOnAnimStarted();
					if (ClientResolutionManager.Get() != null)
					{
						ClientResolutionManager.Get().OnAbilityCast(Caster, m_ability);
						ClientResolutionManager.Get().UpdateLastEventTime();
					}
					break;
				case PlaybackState.PlayingAnimation:
					if (!isPlayingAttackAnim || m_camEndEventReceived)
					{
						PlayState = PlaybackState.WaitingForTargetHits;
						UpdateLastEventTimeForClientResolution();
					}
					break;
				case PlaybackState.ReleasedFocus:
					return false;
			}
			if (!pathDone
				&& !m_visibleToClient
				&& (actorMovement.OnPathType(BoardSquarePathInfo.ConnectionType.Charge)
					|| actorMovement.OnPathType(BoardSquarePathInfo.ConnectionType.Knockback)
					|| actorMovement.OnPathType(BoardSquarePathInfo.ConnectionType.Flight)))
			{
				m_visibleToClient = FogOfWar.GetClientFog() != null
					&& FogOfWar.GetClientFog().IsVisible(Caster.GetTravelBoardSquare());
				if (m_visibleToClient)
				{
					Bounds bound = CameraManager.Get().GetTarget();
					if (turn.m_cameraBoundSetForEvade)
					{
						bound.Encapsulate(m_bounds);
					}
					else
					{
						bound = m_bounds;
					}
					if (Caster.GetActorMovement() != null)
					{
						Caster.GetActorMovement().EncapsulatePathInBound(ref bound);
					}
					CameraManager.Get().SetTarget(bound);
					turn.m_cameraBoundSetForEvade = true;
				}
			}
			if (!m_notifiedClientKnockbackOnHitsDone
				&& ServerClientUtils.GetCurrentAbilityPhase() == AbilityPriority.Combat_Knockback // in rogues: TheatricsManager.Get().GetPhaseToUpdate() == AbilityPriority.Combat_Knockback
				&& ClientKnockbackManager.Get() != null
				&& amNotWaitingForHits
				&& PlayState >= PlaybackState.WaitingForTargetHits)
			{
				// added in rogues
				//bool flag7 =
				ClientKnockbackManager.Get().NotifyOnActorAnimHitsDone(Caster);
				m_notifiedClientKnockbackOnHitsDone = true;

				// added in rogues
				//if (flag7)
				//{
				//	m_clientKnockbackNotifyTime = GameTime.time;
				//}
			}
			bool flag3 = !NetworkClient.active
				|| m_playRequestedTime <= 0f
				|| ShouldIgnoreCameraFraming()
				|| GameTime.time > m_playRequestedTime + 1f;
			bool flag42 = !isPlayingAttackAnim
				|| m_camEndEventReceived
				|| m_animationIndex <= 0;
			// reactor
			bool flag43 = true;
			// rogues
			//bool flag42 = m_clientKnockbackNotifyTime < 0f || GameTime.time - m_clientKnockbackNotifyTime > 1.5f;
			if ((!flag42 || CameraManager.Get().ShotSequence != null || !pathDone || !amNotWaitingForHits || !flag43 || !flag3) && !timedOut)
			{
				return UpdateNotFinished();
			}
			AnimationFinished = true;
			PlayState = PlaybackState.ReleasedFocus;
			NotifyKnockbackManagerForTauntsProgress();
			UpdateLastEventTimeForClientResolution();
			if (m_ability != null)
			{
				m_ability.OnAbilityAnimationReleaseFocus(Caster);
			}
			if (turn.IsReadyToRagdoll(Caster))
			{
				Caster.DoVisualDeath(new ActorModelData.ImpulseInfo(Caster.GetLoSCheckPos(), Vector3.up));
			}
			return false;
		}

		internal void OnAnimationEvent(ActorData animatedActor, UnityEngine.Object eventObject, GameObject sourceObject)
		{
			if (sourceObject != null
				&& eventObject.name != null
				&& eventObject.name == "CamEndEvent")
			{
				m_camEndEventReceived = true;
				if (TheatricsManager.DebugTraceExecution)
				{
					TheatricsManager.LogForDebugging("CamEndEvent received for " + DebugShortName(""));
				}
			}
			else
			{
				SequenceManager.Get().OnAnimationEvent(animatedActor, eventObject, sourceObject, SeqSource);
			}
			m_animEventsSeen.Add(eventObject.name);
		}

		internal void OnAnimationEventFromChild(ActorData animatedActor, UnityEngine.Object eventObject, GameObject sourceObject)
		{
			if (ParentAbilitySeqSource != null)
			{
				SequenceManager.Get().OnAnimationEvent(animatedActor, eventObject, sourceObject, ParentAbilitySeqSource);
			}
		}

		internal bool OnSequenceHit(Sequence seq, ActorData target, ActorModelData.ImpulseInfo impulseInfo, ActorModelData.RagdollActivation ragdollActivation)
		{
			if (!TriggeredSequence(seq))
			{
				return false;
			}
			if (seq.RequestsHitAnimation(target))
			{
				if (HitActorsToDeltaHP == null)
				{
					Log.Warning(this + " has sequence " + seq + " marked Target Hit Animtion, but the ability did not return anything from GatherResults, skipping hit reaction and ragdoll");
					return true;
				}
				if (!HitActorsToDeltaHP.ContainsKey(target))
				{
					Log.Warning(this + " has sequence " + seq + " with target " + target + " but the ability did not return that target from GatherResults, skipping hit reaction and ragdoll");
					return true;
				}

				// rogues
				//HitChanceBracket.HitType hitAccuType = ClientResolutionManager.Get().GetHitAccuType(seq.Source, target);

				ActorModelData actorModelData = target.GetActorModelData();

				// reactor
				if (actorModelData != null
					&& actorModelData.CanPlayDamageReactAnim()
					&& m_turn.IsReadyForDamageReaction(target))
				{
					target.PlayDamageReactionAnim(seq.m_customHitReactTriggerName);
				}
				// rogues
				//if (actorModelData != null
				//	// rogues
				//	//&& hitAccuType != HitChanceBracket.HitType.Miss
				//	&& (actorModelData.IsPlayingIdleAnim(false) || actorModelData.IsPlayingDamageAnim())
				//	&& m_turn.IsReadyForDamageReaction(target))
				//{
				//	target.PlayDamageReactionAnim(seq.m_customHitReactTriggerName);
				//}

				if (ragdollActivation != ActorModelData.RagdollActivation.None
					&& m_turn.IsReadyToRagdoll(target))
				{
					target.DoVisualDeath(impulseInfo);
					if (seq.Caster != null
						&& seq.Caster != target
						&& !seq.Caster.IsInRagdoll()
						&& seq.Caster.GetTeam() != target.GetTeam())
					{
						GameEventManager.CharacterRagdollHitEventArgs args = new GameEventManager.CharacterRagdollHitEventArgs
						{
							m_ragdollingActor = target,
							m_triggeringActor = seq.Caster
						};
						GameEventManager.Get().FireEvent(GameEventManager.EventType.ClientRagdollTriggerHit, args);
					}
				}
			}
			return true;
		}

		private void ExecuteUnexecutedHitsForClient(Animator animator, bool movementPathDone)
		{
			if (m_executedUnexecutedHits || ClientResolutionManager.Get().HitsDoneExecuting(SeqSource))
			{
				return;
			}
			string modIdString;
			if (m_ability != null && m_ability.CurrentAbilityMod != null)
			{
				modIdString = "Mod Id: [" + m_ability.CurrentAbilityMod.m_abilityScopeId + "]\n";
			}
			else
			{
				modIdString = "";
			}
			string extraInfo = modIdString + "Theatrics Entry: " + ToString() + "\n" +
				GetAnimEventsSeenString() + GetCurrentStateString(animator, movementPathDone) + "\n";
			ClientResolutionManager.Get().ExecuteUnexecutedActions(SeqSource, extraInfo);
			ClientResolutionManager.Get().UpdateLastEventTime();
			m_executedUnexecutedHits = true;
		}

		private void NotifyKnockbackManagerForTauntsProgress()
		{
#if SERVER
			if (NetworkServer.active && m_isTauntForEvadeOrKnockback && TheatricsManager.Get().GetPhaseToUpdate() == AbilityPriority.Combat_Knockback)
			{
				ServerKnockbackManager.Get().OnKnockbackTauntProgressed();
				UpdateLastEventTimeForClientResolution();
			}
#endif
		}

		private void NotifyKnockbackManagerOnAnimStarted()
		{
#if SERVER
			if (NetworkServer.active && TheatricsManager.Get().GetPhaseToUpdate() == AbilityPriority.Combat_Knockback)
			{
				ServerKnockbackManager.Get().OnKnockbackAnimStarted(Caster);
			}
#endif
		}

		private void UpdateLastEventTimeForClientResolution()
		{
			if (ClientResolutionManager.Get() != null)
			{
				ClientResolutionManager.Get().UpdateLastEventTime();
			}
		}


		// server-only
#if SERVER
		private void InitBounds()
		{
			if (PlayState == ActorAnimation.PlaybackState.CantBeStarted)
			{
				return;
			}
			bool flag = m_ability != null && m_ability.GetRunPriority() == AbilityPriority.Evasion;
			ActorData caster = Caster;
			BoardSquare squareFromVec = Board.Get().GetSquareFromVec3(GetCurrentPositionForBounds(caster));
			m_squaresOfInterestX.Clear();
			m_squaresOfInterestY.Clear();
			AddToSquaresOfInterest(squareFromVec);
			m_bounds = squareFromVec.CameraBounds;
			Bounds bounds = m_bounds;
			if (m_abilityRequest != null && m_abilityRequest.m_targets != null && m_abilityRequest.m_targets.Count > 0 && m_ability != null && m_ability.UseTargeterGridPosForCameraBounds() && !flag)
			{
				for (int i = 0; i < m_abilityRequest.m_targets.Count; i++)
				{
					BoardSquare square = Board.Get().GetSquare(m_abilityRequest.m_targets[i].GridPos);
					if (square != null)
					{
						if (!m_bounds.Contains(square.ToVector3()))
						{
							AddToSquaresOfInterest(square);
						}
						m_bounds.Encapsulate(square.CameraBounds);
					}
				}
			}
			if (m_ability != null)
			{
				List<AbilityTarget> targets = (m_abilityRequest != null && m_abilityRequest.m_targets != null) ? m_abilityRequest.m_targets : new List<AbilityTarget>();
				Bounds bounds2;
				if (m_ability.CalcBoundsOfInterestForCamera(out bounds2, targets, Caster))
				{
					m_bounds.Encapsulate(bounds2);
				}
			}
			if (Effect != null)
			{
				List<Vector3> list = Effect.CalcPointsOfInterestForCamera();
				if (list != null && list.Count > 0)
				{
					Bounds bounds3 = default(Bounds);
					bounds3.center = list[0];
					for (int j = 1; j < list.Count; j++)
					{
						BoardSquare squareFromVec2 = Board.Get().GetSquareFromVec3(list[j]);
						if (squareFromVec2 != null && !m_bounds.Contains(squareFromVec2.ToVector3()))
						{
							AddToSquaresOfInterest(squareFromVec2);
						}
						bounds3.Encapsulate(list[j]);
					}
					m_bounds.Encapsulate(bounds3);
				}
			}
			if (HitActorsToDeltaHP != null && Board.Get() != null)
			{
				foreach (KeyValuePair<ActorData, int> keyValuePair in HitActorsToDeltaHP)
				{
					ActorData key = keyValuePair.Key;
					if (key == null)
					{
						Log.Warning("Theatrics: null target submitted for ability {0}", new object[]
						{
							this
						});
					}
					else if (!(key == caster))
					{
						Vector3 currentPositionForBounds = GetCurrentPositionForBounds(key);
						squareFromVec = Board.Get().GetSquareFromVec3(currentPositionForBounds);
						if (squareFromVec != null)
						{
							if (!m_bounds.Contains(currentPositionForBounds))
							{
								AddToSquaresOfInterest(squareFromVec);
							}
							m_bounds.Encapsulate(squareFromVec.CameraBounds);
						}
						BoardSquare incomingKnockbackEndSquare = ServerKnockbackManager.Get().GetIncomingKnockbackEndSquare(key);
						if (incomingKnockbackEndSquare != null)
						{
							if (!m_bounds.Contains(incomingKnockbackEndSquare.ToVector3()))
							{
								AddToSquaresOfInterest(incomingKnockbackEndSquare);
							}
							m_bounds.Encapsulate(incomingKnockbackEndSquare.CameraBounds);
						}
					}
				}
			}
			if (bounds == m_bounds && !flag)
			{
				BoardSquare squareFromVec3 = Board.Get().GetSquareFromVec3(m_targetPos);
				if (squareFromVec3 != null && !m_bounds.Contains(squareFromVec3.ToVector3()))
				{
					AddToSquaresOfInterest(squareFromVec3);
				}
				m_bounds.Encapsulate(m_targetPos);
			}
			float num = 3f;
			if (m_ability != null && m_ability.m_cameraBoundsMinHeight > num)
			{
				num = m_ability.m_cameraBoundsMinHeight;
			}
			m_bounds.SetMinMax(new Vector3(m_bounds.min.x, m_bounds.min.y, m_bounds.min.z), new Vector3(m_bounds.max.x, m_bounds.min.y, m_bounds.max.z));
			m_bounds.center = new Vector3(m_bounds.center.x, 0.5f * num + (float)Board.Get().BaselineHeight, m_bounds.center.z);
			m_bounds.Expand(new Vector3(0f, num, 0f));
			m_bounds.Expand(new Vector3(0.75f, 0f, 0.75f));
			m_originalBounds = new Bounds(m_bounds.center, m_bounds.size);
		}
#endif

		// server-only
#if SERVER
		private void AddToSquaresOfInterest(BoardSquare square)
		{
			if (square != null)
			{
				if (square.x <= 255 && square.y <= 255)
				{
					if (m_squaresOfInterestX.Count < 255)
					{
						m_squaresOfInterestX.Add((byte)square.x);
						m_squaresOfInterestY.Add((byte)square.y);
						return;
					}
				}
				else if (Application.isEditor)
				{
					Debug.LogWarning("Square index too big for current theatrics serialization code (using byte), please update serialization code to handle larger indices");
				}
			}
		}
#endif

		// server-only
#if SERVER
		private Vector3 GetCurrentPositionForBounds(ActorData actor)
		{
			if (!(actor != null))
			{
				return Vector3.zero;
			}
			BoardSquare travelBoardSquare = actor.GetTravelBoardSquare();
			if (travelBoardSquare != null)
			{
				return travelBoardSquare.ToVector3();
			}
			return actor.transform.position;
		}
#endif

		// removed in rogues
#if SERVER
		private void UpdateLastEventTime()
		{
			ClientResolutionManager.Get()?.UpdateLastEventTime();
		}
#endif

		internal float GetCamStartEventDelay(bool useTauntCamAltTime)
		{
			if (Caster == null || Caster.GetActorModelData() == null)
			{
				return 0f;
			}
			return Caster.GetActorModelData().GetCamStartEventDelay(m_animationIndex, useTauntCamAltTime);
		}

		internal int GetAnimationIndex()
		{
			return m_animationIndex;
		}

		internal int GetOnlyEnemyTargetActorIndex()
		{
			int actorIndex = ActorData.s_invalidActorIndex;
			if (HitActorsToDeltaHP != null
				&& HitActorsToDeltaHP.Count >= 1
				&& HitActorsToDeltaHP.Count <= 2)
			{
				for (int i = 0; i < HitActorsToDeltaHP.Count; i++)
				{
					ActorData actorData = HitActorsToDeltaHP.Keys.ElementAt(i);
					if (actorData != null && Caster != null)
					{
						if (Caster.GetTeam() == actorData.GetTeam())
						{
							if (actorData != Caster)
							{
								actorIndex = ActorData.s_invalidActorIndex;
								break;
							}
						}
						else
						{
							if (actorIndex != ActorData.s_invalidActorIndex)
							{
								actorIndex = ActorData.s_invalidActorIndex;
								break;
							}
							actorIndex = actorData.ActorIndex;
						}
					}
				}
			}
			return actorIndex;
		}

		// server-only
#if SERVER
		internal bool HasFreeActionAbility()
		{
			return m_ability != null && m_ability.IsFreeAction();
		}
#endif

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
			if (m_ability == null || rhs.m_ability == null)
			{
				if (m_ability == null && rhs.m_ability == null)
				{
					return 0;
				}
				if (m_ability != null && m_ability.IsFreeAction())
				{
					return -1;
				}
				if (rhs.m_ability != null && rhs.m_ability.IsFreeAction())
				{
					return 1;
				}
				return m_ability == null ? -1 : 1;
			}

			if (m_ability.RunPriority != rhs.m_ability.RunPriority)
			{
				return m_ability.RunPriority.CompareTo(rhs.m_ability.RunPriority);
			}
			if (m_playOrderIndex != rhs.m_playOrderIndex)
			{
				return m_playOrderIndex.CompareTo(rhs.m_playOrderIndex);
			}
			bool isLeftOwned = GameFlowData.Get().IsActorDataOwned(Caster);
			bool isRightOwned = GameFlowData.Get().IsActorDataOwned(rhs.Caster);
			if (m_ability.IsFreeAction() || rhs.m_ability.IsFreeAction())
			{
				return -1 * m_ability.IsFreeAction().CompareTo(rhs.m_ability.IsFreeAction());
			}
			if (isLeftOwned != isRightOwned)
			{
				return isLeftOwned.CompareTo(isRightOwned);
			}
			if (Caster.ActorIndex != rhs.Caster.ActorIndex)
			{
				return Caster.ActorIndex.CompareTo(rhs.Caster.ActorIndex);
			}
			if (m_animationIndex != rhs.m_animationIndex)
			{
				return m_animationIndex.CompareTo(rhs.m_animationIndex);
			}
			
			// custom TODO test what happens with it and without
			int leftSortPriority = m_ability.GetTheatricsSortPriority(m_abilityActionType);
			int rightSortPriority = rhs.m_ability.GetTheatricsSortPriority(rhs.m_abilityActionType);
			if (leftSortPriority != rightSortPriority)
			{
				if (rightSortPriority == 0)
				{
					return 1;
				}
				if (leftSortPriority == 0)
				{
					return -1;
				}

				return leftSortPriority.CompareTo(rightSortPriority);
			}
			// end custom
			
			return 0;
		}

		private void DisplayHungError(Animator modelAnimator, bool pathDone)
		{
			Log.Error("Theatrics: {0} {1} is hung", Caster.DisplayName, GetCurrentStateString(modelAnimator, pathDone));
		}

		public string GetCurrentStateString(Animator modelAnimator, bool pathDone)
		{
			if (modelAnimator == null || Caster.GetActorModelData() == null)
			{
				if (NetworkServer.active && !NetworkClient.active)
				{
					return string.Concat(new object[]
					{
						"\nIn ability animation state for ",
						Caster.DebugNameString(),
						", ability: ",
						m_ability == null ? "NULL" : m_ability.GetActionAnimType().ToString(),
						", time: ",
						GameTime.time,
						", turn: ",
						GameFlowData.Get().CurrentTurn
					});
				}
				return "";
			}

			int attack = modelAnimator.GetInteger("Attack");
			bool cover = modelAnimator.GetBool("Cover");
			float distToGoal = modelAnimator.GetFloat("DistToGoal");
			int nextLinkType = modelAnimator.GetInteger("NextLinkType");
			int curLinkType = modelAnimator.GetInteger("CurLinkType");
			bool cinematicCam = modelAnimator.GetBool("CinematicCam");
			bool isDecisionPhase = modelAnimator.GetBool("DecisionPhase");

			if (modelAnimator.GetCurrentAnimatorStateInfo(0).IsName("Damage"))
			{
				return string.Concat(new object[]
				{
						"\nIn ability animation state for ",
						Caster.DebugNameString(),
						" while Damage flag is set (hit react.). Code error, show Chris. debug info: (state: ",
						PlayState.ToString(),
						", Attack: ",
						attack,
						", ability: ",
						m_ability == null ? "NULL" : m_ability.GetActionAnimType().ToString(),
						")"
				});
			}
			else
			{
				return string.Concat(new object[]
				{
						"\nIn animation state ",
						Caster.GetActorModelData().GetCurrentAnimatorStateName(),
						" for ",
						m_timeSincePlay,
						" sec.\nAfter a request for ability ",
						m_ability == null ? "NULL" : m_ability.m_abilityName,
						".\nParameters [Attack: ",
						attack,
						", Cover: ",
						cover,
						", DistToGoal: ",
						distToGoal,
						", NextLinkType: ",
						nextLinkType,
						", CurLinkType: ",
						curLinkType,
						", CinematicCam: ",
						cinematicCam,
						", DecisionPhase: ",
						isDecisionPhase,
						"].\nDetails [state: ",
						PlayState.ToString(),
						", actor state: ",
						Caster.GetActorTurnSM().CurrentState.ToString(),
						", movement path done: ",
						pathDone,
						", ability anim: ",
						m_ability == null ? "NULL" : m_ability.GetActionAnimType().ToString(),
						", ability anim played: ",
						m_animationPlayed,
						", time: ",
						GameTime.time,
						", turn: ",
						GameFlowData.Get().CurrentTurn,
						"]"
				});
			}
		}

		public string GetAnimEventsSeenString()
		{
			string text = "Animation Events Seen:\n";
			for (int i = 0; i < m_animEventsSeen.Count; i++)
			{
				if (m_animEventsSeen[i] != null)
				{
					text = text + "    [ " + m_animEventsSeen[i] + " ]\n";
				}
			}
			return text;
		}

		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"[ActorAnimation: ",
				Caster == null ? "(NULL caster)" : Caster.DebugNameString(),
				" ",
				m_ability == null ? "(NULL ability)" : m_ability.m_abilityName,
				", animation index: ",
				m_animationIndex,
				", play order index: ",
				m_playOrderIndex,
				", group index: ",
				m_playOrderGroupIndex,
				", state: ",
				PlayState,
				"]"
			});
		}

		public string DebugShortName(string colorStr = "")
		{
			string text = string.Concat(new string[]
			{
				"[ActorAnimation: ",
				Caster == null ? "(NULL caster)" : Caster.DebugNameString(),
				", ",
				m_ability == null ? "(NULL ability)" : m_ability.m_abilityName,
				"]"
			});
			if (colorStr.Length > 0)
			{
				text = "<color=" + colorStr + ">" + text + "</color>";
			}
			return text;
		}

#if SERVER
		internal ActorAnimation(Turn turn, Phase phase, AbilityRequest abilityRequest, SequenceSource source)
		{
			m_turn = turn;
			m_isAbilityOrItem = true;
			m_ability = abilityRequest.m_ability;
			m_abilityRequest = abilityRequest;
			m_abilityActionType = abilityRequest.m_actionType;
			if (m_abilityRequest == null)
			{
				Log.Error($"NULL ability request for {this}");
				PlayState = PlaybackState.CantBeStarted;
				m_cinematicRequested = -1;
			}
			else if (!Turn.AnimsStartTogetherInPhase(abilityRequest.m_ability.m_runPriority))
			{
				m_cinematicRequested = abilityRequest.m_cinematicRequested;
			}
			if (m_ability == null)
			{
				Log.Error($"NULL ability for {this}");
				PlayState = PlaybackState.CantBeStarted;
			}
			SeqSource = source;
			Caster = abilityRequest.m_caster;
			m_animationIndex = (short)m_ability.GetActionAnimType(abilityRequest.m_targets, abilityRequest.m_caster);
			if (abilityRequest.m_targets != null && abilityRequest.m_targets.Count > 0 && m_ability.GetTargetData() != null && m_ability.GetTargetData().Length != 0)
			{
				m_targetPos = m_ability.GetRotateToTargetPos(abilityRequest.m_targets, Caster);
			}
			else
			{
				m_targetPos = abilityRequest.m_caster.GetFreePos();
			}
			ParentAbilitySeqSource = m_abilityRequest.m_additionalData.m_parentAbilitySequenceSource;
			InitHitActorsToDeltaHP(m_ability.GetRunPriority());
		}

		internal ActorAnimation(Turn turn, Phase phase, EffectResults effectResults)
		{
			m_effectResults = effectResults;
			AbilityPriority phaseIndex = (phase != null) ? phase.Index : Effect.HitPhase;
			m_turn = turn;
			m_isAbilityOrItem = false;
			m_ability = null;
			m_abilityRequest = null;
			SeqSource = Effect.SequenceSource;
			Caster = Effect.GetActorAnimationActor();
			m_animationIndex = (short)Effect.GetCasterAnimationIndex(phaseIndex);
			m_cinematicRequested = Effect.GetCinematicRequested(phaseIndex);
			m_targetPos = Effect.GetRotationTargetPos(phaseIndex);
			m_ignoreForCameraFraming = Effect.IgnoreCameraFraming();
			InitHitActorsToDeltaHP(phaseIndex);
			
			// custom
			m_alwaysInLoS = true;
		}

		internal ActorAnimation(Turn turn, Phase phase, ActorData caster, AbilityData.ActionType actionType, short animIndex, int tauntNum, List<AbilityTarget> targeterTargets, SequenceSource source)
		{
			m_isTauntForEvadeOrKnockback = true;
			Caster = caster;
			m_turn = turn;
			m_cinematicRequested = tauntNum;
			
			// custom
			m_revealOnCast = caster?
				.GetAbilityData()
				.GetAbilityOfActionType(actionType)?
				.m_tags
				.Contains(AbilityTags.DontBreakCasterInvisibilityOnCast) != true;
			
			m_isAbilityOrItem = false;
			m_ability = null;
			m_abilityRequest = null;
			m_abilityActionType = AbilityData.ActionType.INVALID_ACTION;
			m_animationIndex = animIndex;
			if (targeterTargets != null && targeterTargets.Count > 0)
			{
				m_targetPos = targeterTargets[0].FreePos;
			}
			else
			{
				m_targetPos = Caster.GetFreePos();
			}
			HitActorsToDeltaHP = new Dictionary<ActorData, int>();
			SeqSource = source;
			InitNonSerializedData();
			InitBounds();
		}

		internal void SetTurn_FCFS(Turn turn)
		{
			m_turn = turn;
		}

		internal void InitHitActorsToDeltaHP(AbilityPriority phaseIndex)
		{
			if (m_isAbilityOrItem)
			{
				if (m_abilityRequest == null)
				{
					Log.Error("NULL ability request for {0}", new object[]
					{
						this
					});
					PlayState = ActorAnimation.PlaybackState.CantBeStarted;
				}
				else if (m_ability == null)
				{
					Log.Error("NULL ability for {0}", new object[]
					{
						this
					});
					PlayState = ActorAnimation.PlaybackState.CantBeStarted;
				}
				else
				{
					HitActorsToDeltaHP = m_ability.GatherResults_Base(phaseIndex, m_abilityRequest.m_targets, m_abilityRequest.m_caster, m_abilityRequest.m_additionalData);
				}
			}
			else if (m_effectResults != null)
			{
				if (m_effectResults.GatheredResults)
				{
					HitActorsToDeltaHP = m_effectResults.DamageResults;
				}
				else
				{
					HitActorsToDeltaHP = new Dictionary<ActorData, int>();
				}
			}
			else if (m_isTauntForEvadeOrKnockback)
			{
				HitActorsToDeltaHP = new Dictionary<ActorData, int>();
			}
			else
			{
				HitActorsToDeltaHP = new Dictionary<ActorData, int>();
			}
			InitNonSerializedData();
			InitBounds();
		}
#endif
	}
}


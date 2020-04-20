using System;
using System.Collections.Generic;
using System.Linq;
using Theatrics;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class TheatricsManager : NetworkBehaviour, IGameEventListener
{
	public bool m_allowAbilityAnimationInterruptHitReaction = true;

	public int m_numClientPhaseTimeoutsUntilForceDisconnect = 3;

	public const float c_timeoutAdvancePhaseSlowClient = 1.3f;

	public const float c_maxPhaseAdvanceTimeoutDuration = 45f;

	[Separator("Ragdoll Force Settings", true)]
	public float m_ragdollImpactForce = 15f;

	[Header("-- Whether to apply force only to single joint")]
	public bool m_ragdollApplyForceOnSingleJointOnly;

	private Turn m_turn = new Turn();

	private AbilityPriority m_phaseToUpdate = AbilityPriority.INVALID;

	private int m_turnToUpdate;

	private HashSet<long> m_playerConnectionIdsInUpdatePhase = new HashSet<long>();

	private int m_numConnectionIdsAddedForPhase;

	private float m_phaseStartTime;

	private AbilityPriority m_lastPhaseEnded = AbilityPriority.INVALID;

	private static TheatricsManager s_instance;

	private SerializeHelper m_serializeHelper = new SerializeHelper();

	internal string m_debugSerializationInfoStr = string.Empty;

	internal const string c_actorAnimDebugHeader = "<color=cyan>Theatrics: </color>";

	internal static TheatricsManager Get()
	{
		return TheatricsManager.s_instance;
	}

	public static float GetRagdollImpactForce()
	{
		if (TheatricsManager.s_instance != null)
		{
			return TheatricsManager.s_instance.m_ragdollImpactForce;
		}
		return 15f;
	}

	public static bool RagdollOnlyApplyForceAtSingleJoint()
	{
		if (TheatricsManager.s_instance != null)
		{
			return TheatricsManager.s_instance.m_ragdollApplyForceOnSingleJointOnly;
		}
		return false;
	}

	protected void Awake()
	{
		TheatricsManager.s_instance = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	private void Start()
	{
		MyNetworkManager.Get().m_OnServerDisconnect += this.OnServerDisconnect;
		GameEventManager.Get().AddListener(this, GameEventManager.EventType.UIPhaseStartedMovement);
	}

	private void OnDestroy()
	{
		MyNetworkManager.Get().m_OnServerDisconnect -= this.OnServerDisconnect;
		GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.UIPhaseStartedMovement);
		TheatricsManager.s_instance = null;
	}

	internal void OnTurnTick()
	{
		this.m_lastPhaseEnded = AbilityPriority.INVALID;
	}

	private void OnEnable()
	{
		SceneManager.sceneLoaded += this.OnSceneLoaded;
	}

	private void OnDisable()
	{
		SceneManager.sceneLoaded -= this.OnSceneLoaded;
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		this.m_turn = new Turn();
		this.m_phaseToUpdate = AbilityPriority.INVALID;
		this.m_lastPhaseEnded = AbilityPriority.INVALID;
	}

	internal bool AbilityPhaseHasNoAnimations()
	{
		return this.m_turn == null || !this.m_turn.symbol_0011();
	}

	internal AbilityPriority GetPhaseToUpdate()
	{
		return this.m_phaseToUpdate;
	}

	[Server]
	internal void PlayPhase(AbilityPriority phaseIndex)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void TheatricsManager::PlayPhase(AbilityPriority)' called on client");
			return;
		}
		this.m_playerConnectionIdsInUpdatePhase.Clear();
		if (this.m_turn.symbol_000B(phaseIndex))
		{
			for (int i = 0; i < GameFlow.Get().playerDetails.Count; i++)
			{
				Player key = GameFlow.Get().playerDetails.Keys.ElementAt(i);
				PlayerDetails playerDetails = GameFlow.Get().playerDetails[key];
				if (!playerDetails.m_disconnected && playerDetails.m_gameObjects != null && playerDetails.m_gameObjects.Count > 0)
				{
					if (playerDetails.IsHumanControlled)
					{
						if (!playerDetails.IsSpectator)
						{
							this.m_playerConnectionIdsInUpdatePhase.Add(playerDetails.m_accountId);
						}
					}
				}
			}
		}
		else if (GameFlowData.Get().activeOwnedActorData != null)
		{
			this.m_playerConnectionIdsInUpdatePhase.Add(GameFlowData.Get().activeOwnedActorData.GetPlayerDetails().m_accountId);
		}
		this.m_numConnectionIdsAddedForPhase = this.m_playerConnectionIdsInUpdatePhase.Count;
		this.m_turn.symbol_0011(phaseIndex);
		this.m_phaseToUpdate = phaseIndex;
		this.m_phaseStartTime = Time.time;
	}

	internal void OnSequenceHit(Sequence seq, ActorData target, ActorModelData.ImpulseInfo impulseInfo = null, ActorModelData.RagdollActivation ragdollActivation = ActorModelData.RagdollActivation.HealthBased)
	{
		this.m_turn.symbol_0011(seq, target, impulseInfo, ragdollActivation);
	}

	internal bool ClientNeedToWaitBeforeKnockbackMove(ActorData actor)
	{
		bool result = false;
		int num = 5;
		if (this.m_turn.symbol_000E.Count > num)
		{
			if (this.m_turn.symbol_000E[num] != null)
			{
				result = this.m_turn.symbol_000E[num].symbol_001C(actor);
			}
		}
		return result;
	}

	[Server]
	private void OnServerDisconnect(NetworkConnection conn)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void TheatricsManager::OnServerDisconnect(UnityEngine.Networking.NetworkConnection)' called on client");
			return;
		}
	}

	[Server]
	internal void OnReplacedWithBots(Player player)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void TheatricsManager::OnReplacedWithBots(Player)' called on client");
			return;
		}
		this.StopWaitingForConnectionId(player.m_accountId);
	}

	public void StopWaitingForConnectionId(long accountId)
	{
	}

	public override bool OnSerialize(NetworkWriter writer, bool initialState)
	{
		return this.OnSerializeHelper(new NetworkWriterAdapter(writer), initialState);
	}

	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
		uint num = uint.MaxValue;
		if (!initialState)
		{
			num = reader.ReadPackedUInt32();
		}
		if (num != 0U)
		{
			this.OnSerializeHelper(new NetworkReaderAdapter(reader), initialState);
		}
	}

	private bool OnSerializeHelper(IBitStream stream, bool initialState)
	{
		if (!initialState)
		{
			if (this.m_serializeHelper.ShouldReturnImmediately(ref stream))
			{
				return false;
			}
		}
		int turnToUpdate = this.m_turnToUpdate;
		stream.Serialize(ref turnToUpdate);
		bool flag = turnToUpdate != this.m_turnToUpdate;
		this.m_turnToUpdate = turnToUpdate;
		int phaseToUpdate = (int)this.m_phaseToUpdate;
		stream.Serialize(ref phaseToUpdate);
		bool flag2 = this.m_phaseToUpdate != (AbilityPriority)phaseToUpdate;
		if (!flag2)
		{
			if (!flag)
			{
				goto IL_B8;
			}
		}
		this.m_phaseToUpdate = (AbilityPriority)phaseToUpdate;
		this.m_phaseStartTime = Time.time;
		if (flag)
		{
			this.m_turn = new Turn();
		}
		IL_B8:
		this.m_turn.symbol_0011(stream);
		if (flag2)
		{
			this.m_turn.symbol_0011(this.m_phaseToUpdate);
		}
		return this.m_serializeHelper.End(initialState, base.syncVarDirtyBits);
	}

	private void Update()
	{
		this.UpdateClient();
	}

	private void UpdateClient()
	{
		if (GameFlowData.Get() == null)
		{
			return;
		}
		if (this.m_turnToUpdate != GameFlowData.Get().CurrentTurn)
		{
			if (this.m_turnToUpdate > GameFlowData.Get().CurrentTurn)
			{
				Debug.LogError("Theatrics: Turn to update is higher than current turn");
			}
			return;
		}
		if (this.m_phaseToUpdate != AbilityPriority.INVALID && !this.m_turn.symbol_001A(this.m_phaseToUpdate))
		{
			if (this.m_lastPhaseEnded != this.m_phaseToUpdate)
			{
				if (GameFlowData.Get().LocalPlayerData != null)
				{
					GameFlowData.Get().LocalPlayerData.CallCmdTheatricsManagerUpdatePhaseEnded((int)this.m_phaseToUpdate, Time.time - this.m_phaseStartTime, Time.smoothDeltaTime);
					this.m_lastPhaseEnded = this.m_phaseToUpdate;
				}
			}
		}
		else if (ServerClientUtils.GetCurrentActionPhase() == ActionBufferPhase.Movement)
		{
			bool flag = false;
			bool flag2 = false;
			List<ActorData> actors = GameFlowData.Get().GetActors();
			FogOfWar clientFog = FogOfWar.GetClientFog();
			if (clientFog != null)
			{
				for (int i = 0; i < actors.Count; i++)
				{
					ActorData actorData = actors[i];
					if (actorData.GetActorMovement().AmMoving())
					{
						bool flag3 = clientFog.IsVisible(actorData.GetTravelBoardSquare());
						bool flag4;
						if (!flag)
						{
							flag4 = !flag3;
						}
						else
						{
							flag4 = true;
						}
						flag = flag4;
						bool flag5;
						if (!flag2)
						{
							flag5 = flag3;
						}
						else
						{
							flag5 = true;
						}
						flag2 = flag5;
					}
				}
				if (flag && !flag2)
				{
					if (!(SinglePlayerManager.Get() == null))
					{
						if (!SinglePlayerManager.Get().EnableHiddenMovementText())
						{
							goto IL_230;
						}
					}
					InterfaceManager.Get().DisplayAlert(StringUtil.TR("HiddenMovement", "Global"), Color.white, 2f, false, 0);
				}
			}
		}
		IL_230:
		if (ServerClientUtils.GetCurrentActionPhase() != ActionBufferPhase.Movement)
		{
			if (ServerClientUtils.GetCurrentActionPhase() != ActionBufferPhase.MovementWait)
			{
				return;
			}
		}
		this.SetAnimatorParamOnAllActors("DecisionPhase", true);
	}

	public void OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		if (eventType == GameEventManager.EventType.UIPhaseStartedMovement)
		{
			PlayerData localPlayerData = GameFlowData.Get().LocalPlayerData;
			if (localPlayerData != null)
			{
				List<ActorData> actors = GameFlowData.Get().GetActors();
				using (List<ActorData>.Enumerator enumerator = actors.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ActorData actorData = enumerator.Current;
						if (actorData.ShouldPickRespawn_zq())
						{
							ActorModelData actorModelData = actorData.GetActorModelData();
							if (actorModelData != null)
							{
								actorData.ShowRespawnFlare(null, true);
								if (localPlayerData.GetTeamViewing() == actorData.GetTeam())
								{
									goto IL_C7;
								}
								if (FogOfWar.GetClientFog().IsVisible(actorData.CurrentBoardSquare))
								{
									for (;;)
									{
										switch (3)
										{
										case 0:
											continue;
										}
										goto IL_C7;
									}
								}
								IL_D4:
								actorModelData.EnableRendererAndUpdateVisibility();
								continue;
								IL_C7:
								actorData.GetActorVFX().ShowOnRespawnVfx();
								goto IL_D4;
							}
						}
					}
				}
			}
		}
	}

	internal void SetAnimatorParamOnAllActors(string paramName, bool value)
	{
		List<ActorData> actors = GameFlowData.Get().GetActors();
		for (int i = 0; i < actors.Count; i++)
		{
			ActorData actorData = actors[i];
			if (actorData != null)
			{
				if (actorData.GetModelAnimator() != null)
				{
					actorData.GetModelAnimator().SetBool(paramName, value);
				}
			}
		}
	}

	internal void SetAnimatorParamOnAllActors(string paramName, int value)
	{
		List<ActorData> actors = GameFlowData.Get().GetActors();
		for (int i = 0; i < actors.Count; i++)
		{
			ActorData actorData = actors[i];
			if (actorData != null && actorData.GetModelAnimator() != null)
			{
				actorData.GetModelAnimator().SetInteger(paramName, value);
			}
		}
	}

	[Server]
	internal void OnUpdatePhaseEnded(long accountId, int phaseEnded, float phaseSeconds, float phaseDeltaSeconds)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void TheatricsManager::OnUpdatePhaseEnded(System.Int64,System.Int32,System.Single,System.Single)' called on client");
			return;
		}
	}

	public void OnAnimationEvent(ActorData animatedActor, UnityEngine.Object eventObject, GameObject sourceObject)
	{
		if (this.m_turn != null)
		{
			this.m_turn.symbol_0011(animatedActor, eventObject, sourceObject);
		}
	}

	internal void no_op(string symbol_001D)
	{
	}

	public bool IsCinematicPlaying()
	{
		bool result;
		if (this.m_turn != null)
		{
			result = this.m_turn.symbol_001A();
		}
		else
		{
			result = false;
		}
		return result;
	}

	public bool IsCinematicsRequestedInCurrentPhase(ActorData actor, Ability ability)
	{
		int phaseToUpdate = (int)this.m_phaseToUpdate;
		if (ability != null)
		{
			if (actor != null)
			{
				if (this.m_turn != null)
				{
					if (this.m_turn.symbol_000E != null)
					{
						if (phaseToUpdate > 0)
						{
							if (phaseToUpdate < this.m_turn.symbol_000E.Count)
							{
								Phase phase = this.m_turn.symbol_000E[phaseToUpdate];
								for (int i = 0; i < phase.animations.Count; i++)
								{
									ActorAnimation actorAnimation = phase.animations[i];
									if (actorAnimation.Actor == actor && actorAnimation.GetAbility() != null)
									{
										if (actorAnimation.GetAbility().GetType() == ability.GetType())
										{
											if (actorAnimation.symbol_0002symbol_000E())
											{
												return true;
											}
										}
									}
								}
							}
						}
					}
				}
			}
		}
		return false;
	}

	public unsafe static void EncapsulatePathInBound(ref Bounds bound, BoardSquarePathInfo path, ActorData mover)
	{
		ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
		FogOfWar clientFog = FogOfWar.GetClientFog();
		if (activeOwnedActorData != null)
		{
			if (clientFog != null)
			{
				if (mover != null)
				{
					BoardSquarePathInfo boardSquarePathInfo = path;
					while (boardSquarePathInfo != null)
					{
						if (activeOwnedActorData == null)
						{
							goto IL_B1;
						}
						if (activeOwnedActorData.GetTeam() == mover.GetTeam())
						{
							goto IL_B1;
						}
						if (clientFog.IsVisible(boardSquarePathInfo.square))
						{
							for (;;)
							{
								switch (7)
								{
								case 0:
									continue;
								}
								goto IL_B1;
							}
						}
						IL_C4:
						boardSquarePathInfo = boardSquarePathInfo.next;
						continue;
						IL_B1:
						bound.Encapsulate(boardSquarePathInfo.square.CameraBounds);
						goto IL_C4;
					}
				}
			}
		}
	}

	internal int GetPlayOrderOfClientAction(ClientResolutionAction action, AbilityPriority phase)
	{
		int result = -1;
		if (this.m_turn != null)
		{
			if (phase < (AbilityPriority)this.m_turn.symbol_000E.Count)
			{
				Phase phase2 = this.m_turn.symbol_000E[(int)phase];
				for (int i = 0; i < phase2.animations.Count; i++)
				{
					ActorAnimation actorAnimation = phase2.animations[i];
					if (action.ContainsSequenceSource(actorAnimation.SeqSource))
					{
						return (int)actorAnimation.playOrderIndex;
					}
				}
			}
		}
		return result;
	}

	internal int GetPlayOrderOfFirstDamagingHitOnActor(ActorData actor, AbilityPriority phase)
	{
		int num = -1;
		if (this.m_turn != null)
		{
			if (phase < (AbilityPriority)this.m_turn.symbol_000E.Count)
			{
				Phase phase2 = this.m_turn.symbol_000E[(int)phase];
				for (int i = 0; i < phase2.animations.Count; i++)
				{
					ActorAnimation actorAnimation = phase2.animations[i];
					if (actorAnimation.HitActorsToDeltaHP != null)
					{
						if (actorAnimation.HitActorsToDeltaHP.ContainsKey(actor) && actorAnimation.HitActorsToDeltaHP[actor] < 0)
						{
							if (num >= 0)
							{
								if ((int)actorAnimation.playOrderIndex >= num)
								{
									goto IL_D1;
								}
							}
							num = (int)actorAnimation.playOrderIndex;
						}
					}
					IL_D1:;
				}
			}
		}
		return num;
	}

	internal void LogCurrentStateOnTimeout()
	{
		string theatricsStateString = this.GetTheatricsStateString();
		Log.Error(theatricsStateString, new object[0]);
	}

	internal string GetTheatricsStateString()
	{
		string text = string.Concat(new object[]
		{
			"[Theatrics state] Phase to update: ",
			this.m_phaseToUpdate.ToString(),
			", time in phase: ",
			Time.time - this.m_phaseStartTime
		});
		text = text + ", lastPhaseEnded: " + this.m_lastPhaseEnded;
		string text2 = text;
		text = string.Concat(new object[]
		{
			text2,
			"\nNum of phases so far: ",
			this.m_turn.symbol_000E.Count,
			"\n"
		});
		List<ActorData> list = new List<ActorData>();
		for (int i = 0; i < this.m_turn.symbol_000E.Count; i++)
		{
			string text3 = string.Empty;
			Phase phase = this.m_turn.symbol_000E[i];
			for (int j = 0; j < phase.animations.Count; j++)
			{
				ActorAnimation actorAnimation = phase.animations[j];
				if (actorAnimation.State != ActorAnimation.PlaybackState.symbol_0013)
				{
					text3 = text3 + "\t" + actorAnimation.ToString() + "\n";
					if (!list.Contains(actorAnimation.Actor))
					{
						list.Add(actorAnimation.Actor);
					}
				}
			}
			if (text3.Length > 0)
			{
				text2 = text;
				text = string.Concat(new string[]
				{
					text2,
					"Phase ",
					phase.Index.ToString(),
					", ActorAnims not done:\n",
					text3
				});
			}
		}
		using (List<ActorData>.Enumerator enumerator = list.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ActorData actorData = enumerator.Current;
				if (actorData != null)
				{
					ActorModelData actorModelData = actorData.GetActorModelData();
					Animator modelAnimator = actorData.GetModelAnimator();
					if (actorModelData != null)
					{
						if (modelAnimator != null)
						{
							text2 = text;
							text = string.Concat(new object[]
							{
								text2,
								actorData.GetDebugName(),
								" InIdle=",
								actorModelData.IsPlayingIdleAnim(false),
								", DamageAnim=",
								actorModelData.IsPlayingDamageAnim(),
								", AttackParam=",
								modelAnimator.GetInteger("Attack"),
								"\n"
							});
						}
					}
				}
			}
		}
		return text;
	}

	internal static bool DebugLog
	{
		get
		{
			return false;
		}
	}

	internal static bool TraceTheatricsSerialization
	{
		get
		{
			return false;
		}
	}

	internal static void LogForDebugging(string str)
	{
		Debug.LogWarning(string.Concat(new object[]
		{
			"<color=cyan>Theatrics: </color>",
			str,
			"\n@time= ",
			Time.time
		}));
	}

	private void UNetVersion()
	{
	}
}

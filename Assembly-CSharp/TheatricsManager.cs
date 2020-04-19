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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TheatricsManager.GetRagdollImpactForce()).MethodHandle;
			}
			return TheatricsManager.s_instance.m_ragdollImpactForce;
		}
		return 15f;
	}

	public static bool RagdollOnlyApplyForceAtSingleJoint()
	{
		if (TheatricsManager.s_instance != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TheatricsManager.RagdollOnlyApplyForceAtSingleJoint()).MethodHandle;
			}
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
		return this.m_turn == null || !this.m_turn.\u0011();
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
		if (this.m_turn.\u000B(phaseIndex))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TheatricsManager.PlayPhase(AbilityPriority)).MethodHandle;
			}
			for (int i = 0; i < GameFlow.Get().playerDetails.Count; i++)
			{
				Player key = GameFlow.Get().playerDetails.Keys.ElementAt(i);
				PlayerDetails playerDetails = GameFlow.Get().playerDetails[key];
				if (!playerDetails.m_disconnected && playerDetails.m_gameObjects != null && playerDetails.m_gameObjects.Count > 0)
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
					if (playerDetails.IsHumanControlled)
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
						if (!playerDetails.IsSpectator)
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
							this.m_playerConnectionIdsInUpdatePhase.Add(playerDetails.m_accountId);
						}
					}
				}
			}
		}
		else if (GameFlowData.Get().activeOwnedActorData != null)
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
			this.m_playerConnectionIdsInUpdatePhase.Add(GameFlowData.Get().activeOwnedActorData.\u000E().m_accountId);
		}
		this.m_numConnectionIdsAddedForPhase = this.m_playerConnectionIdsInUpdatePhase.Count;
		this.m_turn.\u0011(phaseIndex);
		this.m_phaseToUpdate = phaseIndex;
		this.m_phaseStartTime = Time.time;
	}

	internal void OnSequenceHit(Sequence seq, ActorData target, ActorModelData.ImpulseInfo impulseInfo = null, ActorModelData.RagdollActivation ragdollActivation = ActorModelData.RagdollActivation.HealthBased)
	{
		this.m_turn.\u0011(seq, target, impulseInfo, ragdollActivation);
	}

	internal bool ClientNeedToWaitBeforeKnockbackMove(ActorData actor)
	{
		bool result = false;
		int num = 5;
		if (this.m_turn.\u000E.Count > num)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TheatricsManager.ClientNeedToWaitBeforeKnockbackMove(ActorData)).MethodHandle;
			}
			if (this.m_turn.\u000E[num] != null)
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
				result = this.m_turn.\u000E[num].\u001C(actor);
			}
		}
		return result;
	}

	[Server]
	private void OnServerDisconnect(NetworkConnection conn)
	{
		if (!NetworkServer.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TheatricsManager.OnServerDisconnect(NetworkConnection)).MethodHandle;
			}
			Debug.LogWarning("[Server] function 'System.Void TheatricsManager::OnServerDisconnect(UnityEngine.Networking.NetworkConnection)' called on client");
			return;
		}
	}

	[Server]
	internal void OnReplacedWithBots(Player player)
	{
		if (!NetworkServer.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TheatricsManager.OnReplacedWithBots(Player)).MethodHandle;
			}
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TheatricsManager.OnSerializeHelper(IBitStream, bool)).MethodHandle;
			}
			if (this.m_serializeHelper.ShouldReturnImmediately(ref stream))
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
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!flag)
			{
				goto IL_B8;
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
		this.m_phaseToUpdate = (AbilityPriority)phaseToUpdate;
		this.m_phaseStartTime = Time.time;
		if (flag)
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
			this.m_turn = new Turn();
		}
		IL_B8:
		this.m_turn.\u0011(stream);
		if (flag2)
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
			this.m_turn.\u0011(this.m_phaseToUpdate);
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TheatricsManager.UpdateClient()).MethodHandle;
			}
			return;
		}
		if (this.m_turnToUpdate != GameFlowData.Get().CurrentTurn)
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
			if (this.m_turnToUpdate > GameFlowData.Get().CurrentTurn)
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
				Debug.LogError("Theatrics: Turn to update is higher than current turn");
			}
			return;
		}
		if (this.m_phaseToUpdate != AbilityPriority.INVALID && !this.m_turn.\u001A(this.m_phaseToUpdate))
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
			if (this.m_lastPhaseEnded != this.m_phaseToUpdate)
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
				if (GameFlowData.Get().LocalPlayerData != null)
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
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				for (int i = 0; i < actors.Count; i++)
				{
					ActorData actorData = actors[i];
					if (actorData.\u000E().AmMoving())
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
						bool flag3 = clientFog.IsVisible(actorData.\u000E());
						bool flag4;
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
							for (;;)
							{
								switch (7)
								{
								case 0:
									continue;
								}
								break;
							}
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
					for (;;)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!(SinglePlayerManager.Get() == null))
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
						if (!SinglePlayerManager.Get().EnableHiddenMovementText())
						{
							goto IL_230;
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
					InterfaceManager.Get().DisplayAlert(StringUtil.TR("HiddenMovement", "Global"), Color.white, 2f, false, 0);
				}
			}
		}
		IL_230:
		if (ServerClientUtils.GetCurrentActionPhase() != ActionBufferPhase.Movement)
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
			if (ServerClientUtils.GetCurrentActionPhase() != ActionBufferPhase.MovementWait)
			{
				return;
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
		this.SetAnimatorParamOnAllActors("DecisionPhase", true);
	}

	public void OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		if (eventType == GameEventManager.EventType.UIPhaseStartedMovement)
		{
			PlayerData localPlayerData = GameFlowData.Get().LocalPlayerData;
			if (localPlayerData != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(TheatricsManager.OnGameEvent(GameEventManager.EventType, GameEventManager.GameEventArgs)).MethodHandle;
				}
				List<ActorData> actors = GameFlowData.Get().GetActors();
				using (List<ActorData>.Enumerator enumerator = actors.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ActorData actorData = enumerator.Current;
						if (actorData.\u0015())
						{
							ActorModelData actorModelData = actorData.\u000E();
							if (actorModelData != null)
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
								actorData.ShowRespawnFlare(null, true);
								if (localPlayerData.GetTeamViewing() == actorData.\u000E())
								{
									goto IL_C7;
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
								actorData.\u000E().ShowOnRespawnVfx();
								goto IL_D4;
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(TheatricsManager.SetAnimatorParamOnAllActors(string, bool)).MethodHandle;
				}
				if (actorData.\u000E() != null)
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
					actorData.\u000E().SetBool(paramName, value);
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

	internal void SetAnimatorParamOnAllActors(string paramName, int value)
	{
		List<ActorData> actors = GameFlowData.Get().GetActors();
		for (int i = 0; i < actors.Count; i++)
		{
			ActorData actorData = actors[i];
			if (actorData != null && actorData.\u000E() != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(TheatricsManager.SetAnimatorParamOnAllActors(string, int)).MethodHandle;
				}
				actorData.\u000E().SetInteger(paramName, value);
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TheatricsManager.OnAnimationEvent(ActorData, UnityEngine.Object, GameObject)).MethodHandle;
			}
			this.m_turn.\u0011(animatedActor, eventObject, sourceObject);
		}
	}

	internal void \u000E(string \u001D)
	{
	}

	public bool IsCinematicPlaying()
	{
		bool result;
		if (this.m_turn != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TheatricsManager.IsCinematicPlaying()).MethodHandle;
			}
			result = this.m_turn.\u001A();
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TheatricsManager.IsCinematicsRequestedInCurrentPhase(ActorData, Ability)).MethodHandle;
			}
			if (actor != null)
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
				if (this.m_turn != null)
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
					if (this.m_turn.\u000E != null)
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
						if (phaseToUpdate > 0)
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
							if (phaseToUpdate < this.m_turn.\u000E.Count)
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
								Phase phase = this.m_turn.\u000E[phaseToUpdate];
								for (int i = 0; i < phase.\u000E.Count; i++)
								{
									ActorAnimation actorAnimation = phase.\u000E[i];
									if (actorAnimation.\u000D\u000E == actor && actorAnimation.\u000D\u000E() != null)
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
										if (actorAnimation.\u000D\u000E().GetType() == ability.GetType())
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
											if (actorAnimation.\u0002\u000E())
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
												return true;
											}
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TheatricsManager.EncapsulatePathInBound(Bounds*, BoardSquarePathInfo, ActorData)).MethodHandle;
			}
			if (clientFog != null)
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
				if (mover != null)
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
					BoardSquarePathInfo boardSquarePathInfo = path;
					while (boardSquarePathInfo != null)
					{
						if (activeOwnedActorData == null)
						{
							goto IL_B1;
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
						if (activeOwnedActorData.\u000E() == mover.\u000E())
						{
							goto IL_B1;
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
			}
		}
	}

	internal int GetPlayOrderOfClientAction(ClientResolutionAction action, AbilityPriority phase)
	{
		int result = -1;
		if (this.m_turn != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TheatricsManager.GetPlayOrderOfClientAction(ClientResolutionAction, AbilityPriority)).MethodHandle;
			}
			if (phase < (AbilityPriority)this.m_turn.\u000E.Count)
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
				Phase phase2 = this.m_turn.\u000E[(int)phase];
				for (int i = 0; i < phase2.\u000E.Count; i++)
				{
					ActorAnimation actorAnimation = phase2.\u000E[i];
					if (action.ContainsSequenceSource(actorAnimation.SeqSource))
					{
						return (int)actorAnimation.\u000A;
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
		return result;
	}

	internal int GetPlayOrderOfFirstDamagingHitOnActor(ActorData actor, AbilityPriority phase)
	{
		int num = -1;
		if (this.m_turn != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TheatricsManager.GetPlayOrderOfFirstDamagingHitOnActor(ActorData, AbilityPriority)).MethodHandle;
			}
			if (phase < (AbilityPriority)this.m_turn.\u000E.Count)
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
				Phase phase2 = this.m_turn.\u000E[(int)phase];
				for (int i = 0; i < phase2.\u000E.Count; i++)
				{
					ActorAnimation actorAnimation = phase2.\u000E[i];
					if (actorAnimation.HitActorsToDeltaHP != null)
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
						if (actorAnimation.HitActorsToDeltaHP.ContainsKey(actor) && actorAnimation.HitActorsToDeltaHP[actor] < 0)
						{
							if (num >= 0)
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
								if ((int)actorAnimation.\u000A >= num)
								{
									goto IL_D1;
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
							num = (int)actorAnimation.\u000A;
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
			this.m_turn.\u000E.Count,
			"\n"
		});
		List<ActorData> list = new List<ActorData>();
		for (int i = 0; i < this.m_turn.\u000E.Count; i++)
		{
			string text3 = string.Empty;
			Phase phase = this.m_turn.\u000E[i];
			for (int j = 0; j < phase.\u000E.Count; j++)
			{
				ActorAnimation actorAnimation = phase.\u000E[j];
				if (actorAnimation.\u000D\u000E != ActorAnimation.PlaybackState.\u0013)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(TheatricsManager.GetTheatricsStateString()).MethodHandle;
					}
					text3 = text3 + "\t" + actorAnimation.ToString() + "\n";
					if (!list.Contains(actorAnimation.\u000D\u000E))
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
						list.Add(actorAnimation.\u000D\u000E);
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
		for (;;)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			break;
		}
		using (List<ActorData>.Enumerator enumerator = list.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ActorData actorData = enumerator.Current;
				if (actorData != null)
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
					ActorModelData actorModelData = actorData.\u000E();
					Animator animator = actorData.\u000E();
					if (actorModelData != null)
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
						if (animator != null)
						{
							text2 = text;
							text = string.Concat(new object[]
							{
								text2,
								actorData.\u0018(),
								" InIdle=",
								actorModelData.IsPlayingIdleAnim(false),
								", DamageAnim=",
								actorModelData.IsPlayingDamageAnim(),
								", AttackParam=",
								animator.GetInteger("Attack"),
								"\n"
							});
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
		return text;
	}

	internal static bool \u000E
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

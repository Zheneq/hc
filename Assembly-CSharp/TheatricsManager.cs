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

	internal static bool DebugLog => false;

	internal static bool TraceTheatricsSerialization => false;

	internal static TheatricsManager Get()
	{
		return s_instance;
	}

	public static float GetRagdollImpactForce()
	{
		if (s_instance != null)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return s_instance.m_ragdollImpactForce;
				}
			}
		}
		return 15f;
	}

	public static bool RagdollOnlyApplyForceAtSingleJoint()
	{
		if (s_instance != null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return s_instance.m_ragdollApplyForceOnSingleJointOnly;
				}
			}
		}
		return false;
	}

	protected void Awake()
	{
		s_instance = this;
		Object.DontDestroyOnLoad(base.gameObject);
	}

	private void Start()
	{
		MyNetworkManager.Get().m_OnServerDisconnect += OnServerDisconnect;
		GameEventManager.Get().AddListener(this, GameEventManager.EventType.UIPhaseStartedMovement);
	}

	private void OnDestroy()
	{
		MyNetworkManager.Get().m_OnServerDisconnect -= OnServerDisconnect;
		GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.UIPhaseStartedMovement);
		s_instance = null;
	}

	internal void OnTurnTick()
	{
		m_lastPhaseEnded = AbilityPriority.INVALID;
	}

	private void OnEnable()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	private void OnDisable()
	{
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		m_turn = new Turn();
		m_phaseToUpdate = AbilityPriority.INVALID;
		m_lastPhaseEnded = AbilityPriority.INVALID;
	}

	internal bool AbilityPhaseHasNoAnimations()
	{
		return m_turn == null || !m_turn._0011();
	}

	internal AbilityPriority GetPhaseToUpdate()
	{
		return m_phaseToUpdate;
	}

	[Server]
	internal void PlayPhase(AbilityPriority phaseIndex)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void TheatricsManager::PlayPhase(AbilityPriority)' called on client");
			return;
		}
		m_playerConnectionIdsInUpdatePhase.Clear();
		if (m_turn._000B(phaseIndex))
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			for (int i = 0; i < GameFlow.Get().playerDetails.Count; i++)
			{
				Player key = GameFlow.Get().playerDetails.Keys.ElementAt(i);
				PlayerDetails playerDetails = GameFlow.Get().playerDetails[key];
				if (playerDetails.m_disconnected || playerDetails.m_gameObjects == null || playerDetails.m_gameObjects.Count <= 0)
				{
					continue;
				}
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!playerDetails.IsHumanControlled)
				{
					continue;
				}
				while (true)
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
					while (true)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					m_playerConnectionIdsInUpdatePhase.Add(playerDetails.m_accountId);
				}
			}
		}
		else if (GameFlowData.Get().activeOwnedActorData != null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			m_playerConnectionIdsInUpdatePhase.Add(GameFlowData.Get().activeOwnedActorData.GetPlayerDetails().m_accountId);
		}
		m_numConnectionIdsAddedForPhase = m_playerConnectionIdsInUpdatePhase.Count;
		m_turn._0011(phaseIndex);
		m_phaseToUpdate = phaseIndex;
		m_phaseStartTime = Time.time;
	}

	internal void OnSequenceHit(Sequence seq, ActorData target, ActorModelData.ImpulseInfo impulseInfo = null, ActorModelData.RagdollActivation ragdollActivation = ActorModelData.RagdollActivation.HealthBased)
	{
		m_turn._0011(seq, target, impulseInfo, ragdollActivation);
	}

	internal bool ClientNeedToWaitBeforeKnockbackMove(ActorData actor)
	{
		bool result = false;
		int num = 5;
		if (m_turn._000E.Count > num)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_turn._000E[num] != null)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				result = m_turn._000E[num]._001C(actor);
			}
		}
		return result;
	}

	[Server]
	private void OnServerDisconnect(NetworkConnection conn)
	{
		if (NetworkServer.active)
		{
			return;
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
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
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					Debug.LogWarning("[Server] function 'System.Void TheatricsManager::OnReplacedWithBots(Player)' called on client");
					return;
				}
			}
		}
		StopWaitingForConnectionId(player.m_accountId);
	}

	public void StopWaitingForConnectionId(long accountId)
	{
	}

	public override bool OnSerialize(NetworkWriter writer, bool initialState)
	{
		return OnSerializeHelper(new NetworkWriterAdapter(writer), initialState);
	}

	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
		uint num = uint.MaxValue;
		if (!initialState)
		{
			num = reader.ReadPackedUInt32();
		}
		if (num != 0)
		{
			OnSerializeHelper(new NetworkReaderAdapter(reader), initialState);
		}
	}

	private bool OnSerializeHelper(IBitStream stream, bool initialState)
	{
		if (!initialState)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_serializeHelper.ShouldReturnImmediately(ref stream))
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						return false;
					}
				}
			}
		}
		int value = m_turnToUpdate;
		stream.Serialize(ref value);
		bool flag = value != m_turnToUpdate;
		m_turnToUpdate = value;
		int value2 = (int)m_phaseToUpdate;
		stream.Serialize(ref value2);
		bool flag2 = m_phaseToUpdate != (AbilityPriority)value2;
		if (!flag2)
		{
			while (true)
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
				goto IL_00b8;
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		m_phaseToUpdate = (AbilityPriority)value2;
		m_phaseStartTime = Time.time;
		if (flag)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			m_turn = new Turn();
		}
		goto IL_00b8;
		IL_00b8:
		m_turn._0011(stream);
		if (flag2)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			m_turn._0011(m_phaseToUpdate);
		}
		return m_serializeHelper.End(initialState, base.syncVarDirtyBits);
	}

	private void Update()
	{
		UpdateClient();
	}

	private void UpdateClient()
	{
		if (GameFlowData.Get() == null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				return;
			}
		}
		if (m_turnToUpdate != GameFlowData.Get().CurrentTurn)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				if (m_turnToUpdate > GameFlowData.Get().CurrentTurn)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						Debug.LogError("Theatrics: Turn to update is higher than current turn");
						return;
					}
				}
				return;
			}
		}
		if (m_phaseToUpdate != AbilityPriority.INVALID && !m_turn._001A(m_phaseToUpdate))
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (m_lastPhaseEnded != m_phaseToUpdate)
			{
				while (true)
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
					while (true)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					GameFlowData.Get().LocalPlayerData.CallCmdTheatricsManagerUpdatePhaseEnded((int)m_phaseToUpdate, Time.time - m_phaseStartTime, Time.smoothDeltaTime);
					m_lastPhaseEnded = m_phaseToUpdate;
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
				while (true)
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
					if (!actorData.GetActorMovement().AmMoving())
					{
						continue;
					}
					while (true)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					bool flag3 = clientFog.IsVisible(actorData.GetTravelBoardSquare());
					int num;
					if (!flag)
					{
						while (true)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
						num = ((!flag3) ? 1 : 0);
					}
					else
					{
						num = 1;
					}
					flag = ((byte)num != 0);
					int num2;
					if (!flag2)
					{
						while (true)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
						num2 = (flag3 ? 1 : 0);
					}
					else
					{
						num2 = 1;
					}
					flag2 = ((byte)num2 != 0);
				}
				if (flag && !flag2)
				{
					while (true)
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
						while (true)
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
							goto IL_0230;
						}
						while (true)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
					}
					InterfaceManager.Get().DisplayAlert(StringUtil.TR("HiddenMovement", "Global"), Color.white);
				}
			}
		}
		goto IL_0230;
		IL_0230:
		if (ServerClientUtils.GetCurrentActionPhase() != ActionBufferPhase.Movement)
		{
			while (true)
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
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		SetAnimatorParamOnAllActors("DecisionPhase", true);
	}

	public void OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		if (eventType != GameEventManager.EventType.UIPhaseStartedMovement)
		{
			return;
		}
		PlayerData localPlayerData = GameFlowData.Get().LocalPlayerData;
		if (!(localPlayerData != null))
		{
			return;
		}
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			List<ActorData> actors = GameFlowData.Get().GetActors();
			using (List<ActorData>.Enumerator enumerator = actors.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ActorData current = enumerator.Current;
					if (!current.ShouldPickRespawn_zq())
					{
						continue;
					}
					ActorModelData actorModelData = current.GetActorModelData();
					if (!(actorModelData != null))
					{
						continue;
					}
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					current.ShowRespawnFlare(null, true);
					if (localPlayerData.GetTeamViewing() != current.GetTeam())
					{
						while (true)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
						if (!FogOfWar.GetClientFog().IsVisible(current.CurrentBoardSquare))
						{
							goto IL_00d4;
						}
						while (true)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							break;
						}
					}
					current.GetActorVFX().ShowOnRespawnVfx();
					goto IL_00d4;
					IL_00d4:
					actorModelData.EnableRendererAndUpdateVisibility();
				}
				while (true)
				{
					switch (2)
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

	internal void SetAnimatorParamOnAllActors(string paramName, bool value)
	{
		List<ActorData> actors = GameFlowData.Get().GetActors();
		for (int i = 0; i < actors.Count; i++)
		{
			ActorData actorData = actors[i];
			if (!(actorData != null))
			{
				continue;
			}
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (actorData.GetModelAnimator() != null)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				actorData.GetModelAnimator().SetBool(paramName, value);
			}
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

	internal void SetAnimatorParamOnAllActors(string paramName, int value)
	{
		List<ActorData> actors = GameFlowData.Get().GetActors();
		for (int i = 0; i < actors.Count; i++)
		{
			ActorData actorData = actors[i];
			if (actorData != null && actorData.GetModelAnimator() != null)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				actorData.GetModelAnimator().SetInteger(paramName, value);
			}
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

	[Server]
	internal void OnUpdatePhaseEnded(long accountId, int phaseEnded, float phaseSeconds, float phaseDeltaSeconds)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void TheatricsManager::OnUpdatePhaseEnded(System.Int64,System.Int32,System.Single,System.Single)' called on client");
		}
	}

	public void OnAnimationEvent(ActorData animatedActor, Object eventObject, GameObject sourceObject)
	{
		if (m_turn == null)
		{
			return;
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_turn._0011(animatedActor, eventObject, sourceObject);
			return;
		}
	}

	internal void no_op(string _001D)
	{
	}

	public bool IsCinematicPlaying()
	{
		int result;
		if (m_turn != null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = (m_turn._001A() ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	public bool IsCinematicsRequestedInCurrentPhase(ActorData actor, Ability ability)
	{
		int phaseToUpdate = (int)m_phaseToUpdate;
		if (ability != null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (actor != null)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (m_turn != null)
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					if (m_turn._000E != null)
					{
						while (true)
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
							while (true)
							{
								switch (2)
								{
								case 0:
									continue;
								}
								break;
							}
							if (phaseToUpdate < m_turn._000E.Count)
							{
								while (true)
								{
									switch (7)
									{
									case 0:
										continue;
									}
									break;
								}
								Phase phase = m_turn._000E[phaseToUpdate];
								for (int i = 0; i < phase.animations.Count; i++)
								{
									ActorAnimation actorAnimation = phase.animations[i];
									if (!(actorAnimation.Actor == actor) || !(actorAnimation.GetAbility() != null))
									{
										continue;
									}
									while (true)
									{
										switch (2)
										{
										case 0:
											continue;
										}
										break;
									}
									if (actorAnimation.GetAbility().GetType() != ability.GetType())
									{
										continue;
									}
									while (true)
									{
										switch (6)
										{
										case 0:
											continue;
										}
										break;
									}
									if (!actorAnimation._0002_000E())
									{
										continue;
									}
									while (true)
									{
										switch (4)
										{
										case 0:
											continue;
										}
										return true;
									}
								}
								while (true)
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

	public static void EncapsulatePathInBound(ref Bounds bound, BoardSquarePathInfo path, ActorData mover)
	{
		ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
		FogOfWar clientFog = FogOfWar.GetClientFog();
		if (!(activeOwnedActorData != null))
		{
			return;
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (!(clientFog != null))
			{
				return;
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				if (!(mover != null))
				{
					return;
				}
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					for (BoardSquarePathInfo boardSquarePathInfo = path; boardSquarePathInfo != null; boardSquarePathInfo = boardSquarePathInfo.next)
					{
						if (!(activeOwnedActorData == null))
						{
							while (true)
							{
								switch (3)
								{
								case 0:
									continue;
								}
								break;
							}
							if (activeOwnedActorData.GetTeam() != mover.GetTeam())
							{
								while (true)
								{
									switch (1)
									{
									case 0:
										continue;
									}
									break;
								}
								if (!clientFog.IsVisible(boardSquarePathInfo.square))
								{
									continue;
								}
								while (true)
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
						bound.Encapsulate(boardSquarePathInfo.square.CameraBounds);
					}
					while (true)
					{
						switch (6)
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

	internal int GetPlayOrderOfClientAction(ClientResolutionAction action, AbilityPriority phase)
	{
		int result = -1;
		if (m_turn != null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if ((int)phase < m_turn._000E.Count)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				Phase phase2 = m_turn._000E[(int)phase];
				int num = 0;
				while (true)
				{
					if (num < phase2.animations.Count)
					{
						ActorAnimation actorAnimation = phase2.animations[num];
						if (action.ContainsSequenceSource(actorAnimation.SeqSource))
						{
							result = actorAnimation.playOrderIndex;
							break;
						}
						num++;
						continue;
					}
					while (true)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
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
		if (m_turn != null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if ((int)phase < m_turn._000E.Count)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				Phase phase2 = m_turn._000E[(int)phase];
				for (int i = 0; i < phase2.animations.Count; i++)
				{
					ActorAnimation actorAnimation = phase2.animations[i];
					if (actorAnimation.HitActorsToDeltaHP == null)
					{
						continue;
					}
					while (true)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!actorAnimation.HitActorsToDeltaHP.ContainsKey(actor) || actorAnimation.HitActorsToDeltaHP[actor] >= 0)
					{
						continue;
					}
					if (num >= 0)
					{
						while (true)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
						if (actorAnimation.playOrderIndex >= num)
						{
							continue;
						}
						while (true)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
					}
					num = actorAnimation.playOrderIndex;
				}
			}
		}
		return num;
	}

	internal void LogCurrentStateOnTimeout()
	{
		string theatricsStateString = GetTheatricsStateString();
		Log.Error(theatricsStateString);
	}

	internal string GetTheatricsStateString()
	{
		string arg = "[Theatrics state] Phase to update: " + m_phaseToUpdate.ToString() + ", time in phase: " + (Time.time - m_phaseStartTime);
		arg = arg + ", lastPhaseEnded: " + m_lastPhaseEnded;
		string text = arg;
		arg = text + "\nNum of phases so far: " + m_turn._000E.Count + "\n";
		List<ActorData> list = new List<ActorData>();
		int num = 0;
		while (num < m_turn._000E.Count)
		{
			string text2 = string.Empty;
			Phase phase = m_turn._000E[num];
			for (int i = 0; i < phase.animations.Count; i++)
			{
				ActorAnimation actorAnimation = phase.animations[i];
				if (actorAnimation.State == ActorAnimation.PlaybackState._0013)
				{
					continue;
				}
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				text2 = text2 + "\t" + actorAnimation.ToString() + "\n";
				if (!list.Contains(actorAnimation.Actor))
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					list.Add(actorAnimation.Actor);
				}
			}
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				if (text2.Length > 0)
				{
					text = arg;
					arg = text + "Phase " + phase.Index.ToString() + ", ActorAnims not done:\n" + text2;
				}
				num++;
				goto IL_01c2;
			}
			IL_01c2:;
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			using (List<ActorData>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ActorData current = enumerator.Current;
					if (current != null)
					{
						while (true)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							break;
						}
						ActorModelData actorModelData = current.GetActorModelData();
						Animator modelAnimator = current.GetModelAnimator();
						if (actorModelData != null)
						{
							while (true)
							{
								switch (2)
								{
								case 0:
									continue;
								}
								break;
							}
							if (modelAnimator != null)
							{
								text = arg;
								arg = text + current.GetDebugName() + " InIdle=" + actorModelData.IsPlayingIdleAnim() + ", DamageAnim=" + actorModelData.IsPlayingDamageAnim() + ", AttackParam=" + modelAnimator.GetInteger("Attack") + "\n";
							}
						}
					}
				}
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						return arg;
					}
				}
			}
		}
	}

	internal static void LogForDebugging(string str)
	{
		Debug.LogWarning("<color=cyan>Theatrics: </color>" + str + "\n@time= " + Time.time);
	}

	private void UNetVersion()
	{
	}
}

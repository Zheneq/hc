using System.Collections.Generic;
using Unity;
using UnityEngine;
using UnityEngine.Networking;

public class ActorTeamSensitiveData : NetworkBehaviour, IGameEventListener
{
	public enum DirtyBit : uint
	{
		FacingDirection = 1u,
		MoveFromBoardSquare = 2u,
		LineData = 4u,
		MovementCameraBound = 8u,
		Respawn = 0x10u,
		InitialMoveStartSquare = 0x20u,
		QueuedAbilities = 0x40u,
		ToggledOnAbilities = 0x80u,
		AbilityRequestDataForTargeter = 0x100u,
		All = uint.MaxValue
	}

	public enum ObservedBy
	{
		Friendlies,
		Hostiles
	}

	public ObservedBy m_typeObservingMe;

	private int m_actorIndex = ActorData.s_invalidActorIndex;

	private ActorData m_associatedActor;

	private Vector3 m_facingDirAfterMovement;

	private BoardSquare m_lastMovementDestination;

	private BoardSquarePathInfo m_lastMovementPath;

	private GameEventManager.EventType m_lastMovementWaitForEvent;

	private ActorData.MovementType m_lastMovementType;

	private BoardSquare m_moveFromBoardSquare;

	private BoardSquare m_initialMoveStartSquare;

	private LineData.LineInstance m_movementLine;

	private sbyte m_numNodesInSnaredLine;

	private Bounds m_movementCameraBounds;

	private List<BoardSquare> m_respawnAvailableSquares = new List<BoardSquare>();

	private BoardSquare m_respawnPickedSquare;

	private List<bool> m_queuedAbilities = new List<bool>();

	private List<bool> m_abilityToggledOn = new List<bool>();

	private bool m_disappearingAfterMovement;

	private bool m_assignedInitialBoardSquare;

	private List<GameObject> m_oldPings = new List<GameObject>();

	private float m_lastPingChatTime;

	private List<ActorTargeting.AbilityRequestData> m_abilityRequestData = new List<ActorTargeting.AbilityRequestData>();

	private static int kRpcRpcMovement;

	private static int kRpcRpcReceivedPingInfo;

	private static int kRpcRpcReceivedAbilityPingInfo;

	public Team ActorsTeam
	{
		get
		{
			if (Actor != null)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						return Actor.GetTeam();
					}
				}
			}
			return Team.Invalid;
		}
	}

	public ActorData Actor
	{
		get
		{
			if (m_associatedActor == null && m_actorIndex != ActorData.s_invalidActorIndex)
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
				if (GameFlowData.Get() != null)
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
					ActorData actorData = GameFlowData.Get().FindActorByActorIndex(m_actorIndex);
					if (actorData != null)
					{
						m_associatedActor = actorData;
					}
				}
			}
			return m_associatedActor;
		}
	}

	public int ActorIndex => m_actorIndex;

	public Vector3 FacingDirAfterMovement
	{
		get
		{
			return m_facingDirAfterMovement;
		}
		set
		{
			if (!(m_facingDirAfterMovement != value))
			{
				return;
			}
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				m_facingDirAfterMovement = value;
				MarkAsDirty(DirtyBit.FacingDirection);
				return;
			}
		}
	}

	public BoardSquare MoveFromBoardSquare
	{
		get
		{
			return m_moveFromBoardSquare;
		}
		set
		{
			if (!(m_moveFromBoardSquare != value))
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
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				m_moveFromBoardSquare = value;
				if (NetworkServer.active)
				{
					MarkAsDirty(DirtyBit.MoveFromBoardSquare);
				}
				return;
			}
		}
	}

	public BoardSquare InitialMoveStartSquare
	{
		get
		{
			return m_initialMoveStartSquare;
		}
		set
		{
			if (!(m_initialMoveStartSquare != value))
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
				m_initialMoveStartSquare = value;
				if (NetworkServer.active)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						MarkAsDirty(DirtyBit.InitialMoveStartSquare);
						return;
					}
				}
				return;
			}
		}
	}

	public Bounds MovementCameraBounds
	{
		get
		{
			return m_movementCameraBounds;
		}
		set
		{
			if (m_movementCameraBounds != value)
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
				m_movementCameraBounds = value;
			}
			if (NetworkServer.active)
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
				MarkAsDirty(DirtyBit.MovementCameraBound);
			}
			if (!NetworkClient.active)
			{
				return;
			}
			ActionBufferPhase currentActionPhase = ClientActionBuffer.Get().CurrentActionPhase;
			if (GameFlowData.Get().gameState != GameState.BothTeams_Resolve || GameManager.Get().GameConfig.GameType == GameType.Tutorial)
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
				if (!(ClientActionBuffer.Get() != null) || !(CameraManager.Get() != null))
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
					if (!ClientGameManager.Get().IsSpectator)
					{
						object obj;
						if (GameFlowData.Get() != null)
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
							obj = GameFlowData.Get().activeOwnedActorData;
						}
						else
						{
							obj = null;
						}
						ActorData actorData = (ActorData)obj;
						if (!(m_associatedActor != null) || !(actorData != null) || m_associatedActor.GetTeam() != actorData.GetTeam())
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
							if (currentActionPhase != ActionBufferPhase.AbilitiesWait)
							{
								if (currentActionPhase != ActionBufferPhase.Movement)
								{
									if (currentActionPhase == ActionBufferPhase.Abilities)
									{
										while (true)
										{
											switch (5)
											{
											case 0:
												continue;
											}
											CameraManager.Get().SaveMovementCameraBound(m_movementCameraBounds);
											return;
										}
									}
									return;
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
							CameraManager.Get().SetTarget(m_movementCameraBounds);
							return;
						}
					}
					if (!(GameFlowData.Get().LocalPlayerData != null) || !(m_associatedActor != null))
					{
						return;
					}
					Team teamViewing = GameFlowData.Get().LocalPlayerData.GetTeamViewing();
					if (teamViewing != m_associatedActor.GetTeam())
					{
						if (teamViewing != Team.Invalid)
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
							break;
						}
					}
					if (currentActionPhase != ActionBufferPhase.AbilitiesWait)
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
						if (currentActionPhase != ActionBufferPhase.Movement)
						{
							if (currentActionPhase == ActionBufferPhase.Abilities)
							{
								while (true)
								{
									switch (6)
									{
									case 0:
										continue;
									}
									CameraManager.Get().SaveMovementCameraBoundForSpectator(m_movementCameraBounds);
									return;
								}
							}
							return;
						}
					}
					CameraManager.Get().SaveMovementCameraBoundForSpectator(m_movementCameraBounds);
					CameraManager.Get().SetTargetForMovementIfNeeded();
					return;
				}
			}
		}
	}

	public BoardSquare RespawnPickedSquare
	{
		get
		{
			return m_respawnPickedSquare;
		}
		set
		{
			m_respawnPickedSquare = value;
			if (!NetworkServer.active)
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
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				MarkAsDirty(DirtyBit.Respawn);
				return;
			}
		}
	}

	public List<BoardSquare> RespawnAvailableSquares
	{
		get
		{
			return m_respawnAvailableSquares;
		}
		set
		{
			m_respawnAvailableSquares = value;
			if (!NetworkServer.active)
			{
				return;
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				MarkAsDirty(DirtyBit.Respawn);
				return;
			}
		}
	}

	static ActorTeamSensitiveData()
	{
		kRpcRpcMovement = 1638998675;
		NetworkBehaviour.RegisterRpcDelegate(typeof(ActorTeamSensitiveData), kRpcRpcMovement, InvokeRpcRpcMovement);
		kRpcRpcReceivedPingInfo = 1349069861;
		NetworkBehaviour.RegisterRpcDelegate(typeof(ActorTeamSensitiveData), kRpcRpcReceivedPingInfo, InvokeRpcRpcReceivedPingInfo);
		kRpcRpcReceivedAbilityPingInfo = 315009541;
		NetworkBehaviour.RegisterRpcDelegate(typeof(ActorTeamSensitiveData), kRpcRpcReceivedAbilityPingInfo, InvokeRpcRpcReceivedAbilityPingInfo);
		NetworkCRC.RegisterBehaviour("ActorTeamSensitiveData", 0);
	}

	private void SetActorIndex(int actorIndex)
	{
		if (m_actorIndex == actorIndex && !(m_associatedActor == null))
		{
			return;
		}
		m_actorIndex = actorIndex;
		object associatedActor;
		if (GameFlowData.Get() != null)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			associatedActor = GameFlowData.Get().FindActorByActorIndex(m_actorIndex);
		}
		else
		{
			associatedActor = null;
		}
		m_associatedActor = (ActorData)associatedActor;
		if (NetworkServer.active)
		{
			return;
		}
		if (m_associatedActor != null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					if (m_typeObservingMe == ObservedBy.Friendlies)
					{
						while (true)
						{
							switch (4)
							{
							case 0:
								break;
							default:
								m_associatedActor.SetClientFriendlyTeamSensitiveData(this);
								return;
							}
						}
					}
					if (m_typeObservingMe == ObservedBy.Hostiles)
					{
						while (true)
						{
							switch (3)
							{
							case 0:
								break;
							default:
								m_associatedActor.SetClientHostileTeamSensitiveData(this);
								return;
							}
						}
					}
					return;
				}
			}
		}
		if (m_actorIndex == ActorData.s_invalidActorIndex)
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
			TeamSensitiveDataMatchmaker.Get().OnTeamSensitiveDataStarted(this);
			return;
		}
	}

	public string GetDebugString()
	{
		string text = (!(Actor == null)) ? Actor.GetDebugName() : ("[null] (actor index = " + m_actorIndex + ")");
		return "ActorTeamSensitiveData-- team = " + ActorsTeam.ToString() + ", actor = " + text + ", observed by = " + m_typeObservingMe;
	}

	private void Awake()
	{
		for (int i = 0; i < 14; i++)
		{
			m_queuedAbilities.Add(false);
			m_abilityToggledOn.Add(false);
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
			return;
		}
	}

	private void Start()
	{
		GameEventManager.Get().AddListener(this, GameEventManager.EventType.TheatricsEvasionMoveStart);
	}

	private void OnDestroy()
	{
		if (GameEventManager.Get() == null)
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
			GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.TheatricsEvasionMoveStart);
			return;
		}
	}

	private void Update()
	{
		if (!NetworkClient.active)
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
			if (NetworkServer.active || !(Actor == null))
			{
				return;
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				if (m_actorIndex == ActorData.s_invalidActorIndex)
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
					object associatedActor;
					if (GameFlowData.Get() != null)
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
						associatedActor = GameFlowData.Get().FindActorByActorIndex(m_actorIndex);
					}
					else
					{
						associatedActor = null;
					}
					m_associatedActor = (ActorData)associatedActor;
					if (!(Actor != null))
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
						if (m_typeObservingMe == ObservedBy.Friendlies)
						{
							while (true)
							{
								switch (7)
								{
								case 0:
									break;
								default:
									Actor.SetClientFriendlyTeamSensitiveData(this);
									return;
								}
							}
						}
						if (m_typeObservingMe == ObservedBy.Hostiles)
						{
							while (true)
							{
								switch (2)
								{
								case 0:
									continue;
								}
								Actor.SetClientHostileTeamSensitiveData(this);
								return;
							}
						}
						return;
					}
				}
			}
		}
	}

	public void MarkAsDirty(DirtyBit bit)
	{
	}

	private bool IsBitDirty(uint setBits, DirtyBit bitToTest)
	{
		return ((int)setBits & (int)bitToTest) != 0;
	}

	public void MarkAsRespawning()
	{
	}

	[ClientRpc]
	private void RpcMovement(GameEventManager.EventType wait, GridPosProp start, GridPosProp end_grid, byte[] pathBytes, ActorData.MovementType type, bool disappearAfterMovement, bool respawning)
	{
		if (NetworkServer.active)
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
			ProcessMovement(wait, GridPos.FromGridPosProp(start), Board.Get().GetBoardSquareSafe(GridPos.FromGridPosProp(end_grid)), MovementUtils.DeSerializePath(pathBytes), type, disappearAfterMovement, respawning);
			return;
		}
	}

	private void ProcessMovement(GameEventManager.EventType wait, GridPos start, BoardSquare end, BoardSquarePathInfo path, ActorData.MovementType type, bool disappearAfterMovement, bool respawning)
	{
		FlushQueuedMovement();
		bool flag = end != null;
		int num;
		if (flag)
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
			num = ((m_lastMovementDestination != end) ? 1 : 0);
		}
		else
		{
			num = 0;
		}
		bool flag2 = (byte)num != 0;
		int num2;
		if (!(Actor == null))
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
			num2 = ((Actor.CurrentBoardSquare == null) ? 1 : 0);
		}
		else
		{
			num2 = 1;
		}
		bool flag3 = (byte)num2 != 0;
		int num3;
		if (flag)
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
			if (!flag2)
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
				if (!flag3)
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
					if (path != null)
					{
						num3 = ((path.GetPathEndpoint().square == Actor.CurrentBoardSquare) ? 1 : 0);
						goto IL_00b7;
					}
				}
			}
		}
		num3 = 0;
		goto IL_00b7;
		IL_02a0:
		if (m_assignedInitialBoardSquare)
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
			Actor.gameObject.SendMessage("OnAssignedToInitialBoardSquare", SendMessageOptions.DontRequireReceiver);
			m_assignedInitialBoardSquare = true;
			return;
		}
		IL_03b5:
		if (!(Actor != null))
		{
			return;
		}
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			if (respawning)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					HandleRespawnCharacterVisibility(Actor);
					return;
				}
			}
			return;
		}
		IL_00b7:
		bool flag4 = (byte)num3 != 0;
		m_lastMovementDestination = end;
		m_lastMovementPath = path;
		m_lastMovementWaitForEvent = wait;
		m_lastMovementType = type;
		m_disappearingAfterMovement = disappearAfterMovement;
		bool flag5 = false;
		if (Actor != null)
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
			if (Actor.GetActorMovement() != null)
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
				flag5 = Actor.GetActorMovement().AmMoving();
			}
		}
		int num4 = 0;
		if (GameFlowData.Get() != null)
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
			num4 = GameFlowData.Get().CurrentTurn;
		}
		if (!flag5)
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
			if (wait == GameEventManager.EventType.Invalid)
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
				if (Actor != null)
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
					if (Actor.LastDeathTurn != num4)
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
						if (Actor.IsDead())
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
							if (!respawning)
							{
								goto IL_02d6;
							}
							while (true)
							{
								switch (1)
								{
								case 0:
									continue;
								}
								break;
							}
						}
						if (!flag2)
						{
							if (flag3)
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
								if (flag)
								{
									goto IL_01ff;
								}
							}
							if (!flag4)
							{
								if (!flag)
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
									if (disappearAfterMovement)
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
										Actor.OnMovementWhileDisappeared(type);
									}
								}
								goto IL_02a0;
							}
						}
						goto IL_01ff;
					}
				}
			}
		}
		goto IL_02d6;
		IL_02d6:
		if (!flag5)
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
			if (wait != 0)
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
				if (Actor != null)
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
					if (Actor.LastDeathTurn != num4)
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
						if (Actor.IsDead())
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
							if (!respawning)
							{
								goto IL_03b5;
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
						BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(start);
						if (!(boardSquareSafe != null))
						{
							return;
						}
						while (true)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							if (boardSquareSafe != Actor.CurrentBoardSquare)
							{
								while (true)
								{
									switch (1)
									{
									case 0:
										continue;
									}
									Actor.AppearAtBoardSquare(boardSquareSafe);
									return;
								}
							}
							return;
						}
					}
				}
			}
		}
		goto IL_03b5;
		IL_01ff:
		if (path == null)
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
			if (type != ActorData.MovementType.Teleport)
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
				Actor.MoveToBoardSquareLocal(end, ActorData.MovementType.Teleport, path, disappearAfterMovement);
				goto IL_0243;
			}
		}
		Actor.MoveToBoardSquareLocal(end, type, path, disappearAfterMovement);
		goto IL_0243;
		IL_0243:
		if (respawning)
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
			if (end != null)
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
				HandleRespawnCharacterVisibility(Actor);
			}
		}
		goto IL_02a0;
	}

	private void HandleRespawnCharacterVisibility(ActorData actor)
	{
		if (!(FogOfWar.GetClientFog() != null))
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
			if (!(Actor.GetActorVFX() != null))
			{
				return;
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				Actor.OnRespawnTeleport();
				Actor.ForceUpdateIsVisibleToClientCache();
				PlayerData localPlayerData = GameFlowData.Get().LocalPlayerData;
				if (!(localPlayerData != null) || !(SpawnPointManager.Get() != null))
				{
					return;
				}
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					if (SpawnPointManager.Get().m_spawnInDuringMovement)
					{
						ActorModelData actorModelData = Actor.GetActorModelData();
						if (actorModelData != null)
						{
							actorModelData.DisableAndHideRenderers();
						}
						if (HighlightUtils.Get().m_recentlySpawnedShader != null)
						{
							TricksterAfterImageNetworkBehaviour.InitializeAfterImageMaterial(Actor.GetActorModelData(), localPlayerData.GetTeamViewing() == Actor.GetTeam(), 0.5f, HighlightUtils.Get().m_recentlySpawnedShader, false);
						}
					}
					return;
				}
			}
		}
	}

	public void EncapsulateVisiblePathBound(ref Bounds bound)
	{
		if (m_lastMovementWaitForEvent == GameEventManager.EventType.Invalid || m_lastMovementPath == null || !(Actor != null))
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			TheatricsManager.EncapsulatePathInBound(ref bound, m_lastMovementPath, Actor);
			return;
		}
	}

	public void ClearPreviousMovementInfo()
	{
		if (!NetworkServer.active)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_lastMovementDestination = null;
			m_lastMovementPath = null;
			m_lastMovementType = ActorData.MovementType.None;
			m_lastMovementWaitForEvent = GameEventManager.EventType.Invalid;
			return;
		}
	}

	public void FlushQueuedMovement()
	{
		if (!NetworkClient.active)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (Actor != null)
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
				bool flag = !Actor.IsDead();
				bool flag2 = m_lastMovementDestination != null;
				bool flag3 = Actor.CurrentBoardSquare == m_lastMovementDestination;
				int num;
				if (Actor.CurrentBoardSquare == null)
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
					num = (Actor.DisappearingAfterCurrentMovement ? 1 : 0);
				}
				else
				{
					num = 0;
				}
				bool flag4 = (byte)num != 0;
				if (flag && flag2)
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
					if (!flag3)
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
						if (!flag4)
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
							Actor.MoveToBoardSquareLocal(m_lastMovementDestination, ActorData.MovementType.Teleport, null, m_disappearingAfterMovement);
						}
					}
				}
			}
			m_lastMovementPath = null;
			m_lastMovementWaitForEvent = GameEventManager.EventType.Invalid;
			return;
		}
	}

	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
		uint setBits = uint.MaxValue;
		if (!initialState)
		{
			setBits = reader.ReadPackedUInt32();
		}
		sbyte b = 0;
		b = reader.ReadSByte();
		SetActorIndex(b);
		if (IsBitDirty(setBits, DirtyBit.FacingDirection))
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
			short num = reader.ReadInt16();
			if (num < 0)
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
				m_facingDirAfterMovement = Vector3.zero;
			}
			else
			{
				m_facingDirAfterMovement = VectorUtils.AngleDegreesToVector(num);
			}
			if (Actor != null)
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
				Actor.SetFacingDirectionAfterMovement(m_facingDirAfterMovement);
			}
		}
		if (IsBitDirty(setBits, DirtyBit.MoveFromBoardSquare))
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
			short x = reader.ReadInt16();
			short y = reader.ReadInt16();
			BoardSquare boardSquare = Board.Get().GetBoardSquare(x, y);
			if (boardSquare != MoveFromBoardSquare)
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
				MoveFromBoardSquare = boardSquare;
				if (Actor != null)
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
					if (Actor.GetActorMovement() != null)
					{
						Actor.GetActorMovement().UpdateSquaresCanMoveTo();
					}
				}
			}
		}
		if (IsBitDirty(setBits, DirtyBit.InitialMoveStartSquare))
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
			short x2 = reader.ReadInt16();
			short y2 = reader.ReadInt16();
			BoardSquare boardSquare2 = Board.Get().GetBoardSquare(x2, y2);
			if (InitialMoveStartSquare != boardSquare2)
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
				InitialMoveStartSquare = boardSquare2;
				if (Actor != null)
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
					if (Actor.GetActorMovement() != null)
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
						Actor.GetActorMovement().UpdateSquaresCanMoveTo();
					}
				}
			}
		}
		if (IsBitDirty(setBits, DirtyBit.LineData))
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
			byte bitField = reader.ReadByte();
			ServerClientUtils.GetBoolsFromBitfield(bitField, out bool @out, out bool out2);
			if (@out)
			{
				m_movementLine = LineData.DeSerializeLine(reader);
			}
			else
			{
				m_movementLine = null;
			}
			if (out2)
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
				m_numNodesInSnaredLine = reader.ReadSByte();
			}
			else
			{
				m_numNodesInSnaredLine = 0;
			}
			if (Actor != null)
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
				LineData component = Actor.GetComponent<LineData>();
				if (component != null)
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
					component.OnDeserializedData(m_movementLine, m_numNodesInSnaredLine);
				}
			}
		}
		if (IsBitDirty(setBits, DirtyBit.MovementCameraBound))
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
			short num2 = reader.ReadInt16();
			short num3 = reader.ReadInt16();
			short num4 = reader.ReadInt16();
			short num5 = reader.ReadInt16();
			Vector3 center = new Vector3(num2, 1.5f + (float)Board.Get().BaselineHeight, num3);
			Vector3 size = new Vector3(num4, 3f, num5);
			MovementCameraBounds = new Bounds(center, size);
		}
		if (IsBitDirty(setBits, DirtyBit.Respawn))
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
			short x3 = reader.ReadInt16();
			short y3 = reader.ReadInt16();
			BoardSquare boardSquare4 = RespawnPickedSquare = Board.Get().GetBoardSquare(x3, y3);
			bool flag = reader.ReadBoolean();
			if (Actor != null)
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
				if (!(RespawnPickedSquare != null))
				{
					if (flag)
					{
						goto IL_03d8;
					}
					while (true)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
				}
				Actor.ShowRespawnFlare(RespawnPickedSquare, flag);
			}
			goto IL_03d8;
		}
		goto IL_0532;
		IL_03d8:
		short num6 = reader.ReadInt16();
		m_respawnAvailableSquares.Clear();
		for (int i = 0; i < num6; i++)
		{
			short x3 = reader.ReadInt16();
			short y3 = reader.ReadInt16();
			BoardSquare boardSquare4 = Board.Get().GetBoardSquare(x3, y3);
			if (boardSquare4 != null)
			{
				m_respawnAvailableSquares.Add(boardSquare4);
			}
			else
			{
				Log.Error("Invalid square received for respawn choices {0}, {1}", x3, y3);
			}
		}
		if (m_respawnAvailableSquares.Count > 0 && RespawnPickedSquare == null)
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
			if (Actor != null)
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
				if (GameFlowData.Get() != null)
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
					if (Actor == GameFlowData.Get().activeOwnedActorData)
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
						if (Actor.GetActorTurnSM().AmStillDeciding())
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
							Actor.ShowRespawnFlare(m_respawnAvailableSquares[0], false);
						}
					}
				}
			}
		}
		goto IL_0532;
		IL_0564:
		bool flag2;
		if (flag2)
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
			bool flag3 = false;
			short num7 = reader.ReadInt16();
			for (int j = 0; j < 14; j++)
			{
				short num8 = (short)(1 << j);
				bool flag4 = (num7 & num8) != 0;
				if (m_queuedAbilities[j] != flag4)
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
					m_queuedAbilities[j] = flag4;
					flag3 = true;
				}
			}
			if (flag3)
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
				if (Actor != null)
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
					if (Actor.GetAbilityData() != null)
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
						Actor.GetAbilityData().OnQueuedAbilitiesChanged();
					}
				}
			}
		}
		if (!IsBitDirty(setBits, DirtyBit.ToggledOnAbilities))
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
			short num9 = reader.ReadInt16();
			for (int k = 0; k < 14; k++)
			{
				short num10 = (short)(1 << k);
				bool flag5 = (num9 & num10) != 0;
				if (m_abilityToggledOn[k] != flag5)
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
					m_abilityToggledOn[k] = flag5;
				}
			}
			while (true)
			{
				switch (5)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
		IL_0532:
		flag2 = IsBitDirty(setBits, DirtyBit.QueuedAbilities);
		if (!flag2)
		{
			if (!IsBitDirty(setBits, DirtyBit.AbilityRequestDataForTargeter))
			{
				goto IL_0564;
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
		DeSerializeAbilityRequestData(reader);
		goto IL_0564;
	}

	public void OnClientAssociatedWithActor(ActorData actor)
	{
		if (NetworkServer.active)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return;
				}
			}
		}
		m_associatedActor = actor;
		if (m_lastMovementDestination != null)
		{
			if (m_lastMovementPath == null && m_lastMovementType != ActorData.MovementType.Teleport)
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
				Actor.MoveToBoardSquareLocal(m_lastMovementDestination, ActorData.MovementType.Teleport, m_lastMovementPath, m_disappearingAfterMovement);
			}
			else
			{
				Actor.MoveToBoardSquareLocal(m_lastMovementDestination, m_lastMovementType, m_lastMovementPath, m_disappearingAfterMovement);
			}
			if (!m_assignedInitialBoardSquare)
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
				Actor.gameObject.SendMessage("OnAssignedToInitialBoardSquare", SendMessageOptions.DontRequireReceiver);
				m_assignedInitialBoardSquare = true;
			}
		}
		Actor.GetActorMovement().UpdateSquaresCanMoveTo();
		if (m_typeObservingMe != 0)
		{
			return;
		}
		LineData component = Actor.GetComponent<LineData>();
		if (!(component != null))
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
			component.OnDeserializedData(m_movementLine, m_numNodesInSnaredLine);
			return;
		}
	}

	public void OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		if (eventType != m_lastMovementWaitForEvent)
		{
			return;
		}
		while (true)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (this == Actor.TeamSensitiveData_authority)
			{
				Actor.MoveToBoardSquareLocal(m_lastMovementDestination, m_lastMovementType, m_lastMovementPath, m_disappearingAfterMovement);
				m_lastMovementPath = null;
				m_lastMovementWaitForEvent = GameEventManager.EventType.Invalid;
			}
			return;
		}
	}

	public void OnTurnTick()
	{
		FlushQueuedMovement();
	}

	public bool HasToggledAction(AbilityData.ActionType actionType)
	{
		bool result = false;
		if (actionType != AbilityData.ActionType.INVALID_ACTION)
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
			if (actionType >= AbilityData.ActionType.ABILITY_0)
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
				if ((int)actionType < m_abilityToggledOn.Count)
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
					result = m_abilityToggledOn[(int)actionType];
				}
			}
		}
		return result;
	}

	[Server]
	public void SetToggledAction(AbilityData.ActionType actionType, bool toggledOn)
	{
		if (!NetworkServer.active)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					Debug.LogWarning("[Server] function 'System.Void ActorTeamSensitiveData::SetToggledAction(AbilityData/ActionType,System.Boolean)' called on client");
					return;
				}
			}
		}
		if (actionType == AbilityData.ActionType.INVALID_ACTION || AbilityData.IsChain(actionType))
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
			if (m_abilityToggledOn[(int)actionType] != toggledOn)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					m_abilityToggledOn[(int)actionType] = toggledOn;
					MarkAsDirty(DirtyBit.ToggledOnAbilities);
					return;
				}
			}
			return;
		}
	}

	public bool HasQueuedAction(AbilityData.ActionType actionType)
	{
		bool result = false;
		int num;
		if (actionType != AbilityData.ActionType.INVALID_ACTION)
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
			if (actionType >= AbilityData.ActionType.ABILITY_0)
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
				if ((int)actionType < m_queuedAbilities.Count)
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
					num = (m_queuedAbilities[(int)actionType] ? 1 : 0);
					goto IL_0054;
				}
			}
			num = 0;
			goto IL_0054;
		}
		goto IL_0055;
		IL_0054:
		result = ((byte)num != 0);
		goto IL_0055;
		IL_0055:
		return result;
	}

	public bool HasQueuedAction(int actionTypeInt)
	{
		return HasQueuedAction((AbilityData.ActionType)actionTypeInt);
	}

	public bool HasQueuedAbilityInPhase(AbilityPriority phase)
	{
		for (int i = 0; i < m_queuedAbilities.Count; i++)
		{
			if (!m_queuedAbilities[i])
			{
				continue;
			}
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
			Ability abilityOfActionType = Actor.GetAbilityData().GetAbilityOfActionType((AbilityData.ActionType)i);
			if (!(abilityOfActionType != null) || abilityOfActionType.RunPriority != phase)
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
				return true;
			}
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			return false;
		}
	}

	[Server]
	internal void SetQueuedAction(AbilityData.ActionType actionType, bool queued)
	{
		if (!NetworkServer.active)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					Debug.LogWarning("[Server] function 'System.Void ActorTeamSensitiveData::SetQueuedAction(AbilityData/ActionType,System.Boolean)' called on client");
					return;
				}
			}
		}
		if (actionType == AbilityData.ActionType.INVALID_ACTION || AbilityData.IsChain(actionType))
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
			if (HasQueuedAction(actionType) != queued)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					m_queuedAbilities[(int)actionType] = queued;
					MarkAsDirty(DirtyBit.QueuedAbilities);
					return;
				}
			}
			return;
		}
	}

	[Server]
	internal void UnqueueActions()
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
					Debug.LogWarning("[Server] function 'System.Void ActorTeamSensitiveData::UnqueueActions()' called on client");
					return;
				}
			}
		}
		for (int i = 0; i < m_queuedAbilities.Count; i++)
		{
			if (m_queuedAbilities[i])
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
				m_queuedAbilities[i] = false;
				MarkAsDirty(DirtyBit.QueuedAbilities);
			}
		}
	}

	[ClientRpc]
	internal void RpcReceivedPingInfo(int teamIndex, Vector3 worldPosition, ActorController.PingType pingType, bool spam)
	{
		if (spam)
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
					if (GameFlowData.Get() != null)
					{
						while (true)
						{
							switch (3)
							{
							case 0:
								break;
							default:
								if (GameFlowData.Get().activeOwnedActorData == Actor)
								{
									while (true)
									{
										switch (5)
										{
										case 0:
											break;
										default:
											TextConsole.Get().Write(new TextConsole.Message
											{
												Text = StringUtil.TR("TooManyPings", "Ping"),
												MessageType = ConsoleMessageType.SystemMessage
											});
											return;
										}
									}
								}
								return;
							}
						}
					}
					return;
				}
			}
		}
		if (!(GameFlowData.Get() != null) || !(GameFlowData.Get().activeOwnedActorData != null) || !(Actor != null))
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
			if (GameFlowData.Get().activeOwnedActorData.GetTeam() != (Team)teamIndex || !(HUD_UI.Get() != null))
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
				if (!(HUD_UI.Get().m_mainScreenPanel != null) || !(HUD_UI.Get().m_mainScreenPanel.m_minimap != null))
				{
					return;
				}
				Vector3 vector = new Vector3(worldPosition.x, Board.Get().BaselineHeight, worldPosition.z);
				ActorData actor = Actor;
				string empty = string.Empty;
				UIWorldPing uIWorldPing;
				string eventName;
				if (pingType == ActorController.PingType.Assist)
				{
					uIWorldPing = Object.Instantiate(HUD_UI.Get().m_mainScreenPanel.m_minimap.m_worldPingAssistPrefab);
					eventName = "ui/ingame/ping/assist";
					BoardSquare boardSquareUnsafe = Board.Get().GetBoardSquareUnsafe(vector.x, vector.z);
					if (boardSquareUnsafe.OccupantActor != null && boardSquareUnsafe.OccupantActor.IsVisibleToClient())
					{
						if (boardSquareUnsafe.OccupantActor.GetTeam() != actor.GetTeam())
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
							string arg = $"<size=36><sprite=\"CharacterSprites\" index={2 * (int)boardSquareUnsafe.OccupantActor.m_characterType + 1}>\u200b</size>";
							empty = string.Format(StringUtil.TR("AssistEnemy", "Ping"), actor.GetFancyDisplayName(), arg, boardSquareUnsafe.OccupantActor.GetFancyDisplayName());
						}
						else if (boardSquareUnsafe.OccupantActor != actor)
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
							string arg2 = $"<size=36><sprite=\"CharacterSprites\" index={2 * (int)boardSquareUnsafe.OccupantActor.m_characterType}>\u200b</size>";
							empty = string.Format(StringUtil.TR("AssistAlly", "Ping"), actor.GetFancyDisplayName(), arg2, boardSquareUnsafe.OccupantActor.GetFancyDisplayName());
						}
						else
						{
							empty = string.Format(StringUtil.TR("Assist", "Ping"), actor.GetFancyDisplayName());
						}
					}
					else
					{
						empty = string.Format(StringUtil.TR("Assist", "Ping"), actor.GetFancyDisplayName());
					}
				}
				else if (pingType == ActorController.PingType.Defend)
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
					uIWorldPing = Object.Instantiate(HUD_UI.Get().m_mainScreenPanel.m_minimap.m_worldPingDefendPrefab);
					eventName = "ui/ingame/ping/anger";
					BoardSquare boardSquareUnsafe2 = Board.Get().GetBoardSquareUnsafe(vector.x, vector.z);
					if (boardSquareUnsafe2.OccupantActor != null)
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
						if (boardSquareUnsafe2.OccupantActor.IsVisibleToClient())
						{
							if (boardSquareUnsafe2.OccupantActor.GetTeam() != actor.GetTeam())
							{
								string arg3 = $"<size=36><sprite=\"CharacterSprites\" index={2 * (int)boardSquareUnsafe2.OccupantActor.m_characterType + 1}>\u200b</size>";
								empty = string.Format(StringUtil.TR("DangerEnemy", "Ping"), actor.GetFancyDisplayName(), arg3, boardSquareUnsafe2.OccupantActor.GetFancyDisplayName());
							}
							else if (boardSquareUnsafe2.OccupantActor != actor)
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
								string arg4 = $"<size=36><sprite=\"CharacterSprites\" index={2 * (int)boardSquareUnsafe2.OccupantActor.m_characterType}>\u200b</size>";
								empty = string.Format(StringUtil.TR("DangerAlly", "Ping"), actor.GetFancyDisplayName(), arg4, boardSquareUnsafe2.OccupantActor.GetFancyDisplayName());
							}
							else
							{
								empty = string.Format(StringUtil.TR("Danger", "Ping"), actor.GetFancyDisplayName());
							}
							goto IL_07f5;
						}
					}
					empty = string.Format(StringUtil.TR("Danger", "Ping"), actor.GetFancyDisplayName());
				}
				else if (pingType == ActorController.PingType.Enemy)
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
					uIWorldPing = Object.Instantiate(HUD_UI.Get().m_mainScreenPanel.m_minimap.m_worldPingEnemyPrefab);
					eventName = "ui/ingame/ping/attack";
					BoardSquare boardSquareUnsafe3 = Board.Get().GetBoardSquareUnsafe(vector.x, vector.z);
					if (boardSquareUnsafe3.OccupantActor != null)
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
						if (boardSquareUnsafe3.OccupantActor.IsVisibleToClient())
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
							if (boardSquareUnsafe3.OccupantActor.GetTeam() != actor.GetTeam())
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
								string arg5 = $"<size=36><sprite=\"CharacterSprites\" index={2 * (int)boardSquareUnsafe3.OccupantActor.m_characterType + 1}>\u200b</size>";
								empty = string.Format(StringUtil.TR("AttackEnemy", "Ping"), actor.GetFancyDisplayName(), arg5, boardSquareUnsafe3.OccupantActor.GetFancyDisplayName());
							}
							else
							{
								empty = string.Format(StringUtil.TR("Attack", "Ping"), actor.GetFancyDisplayName());
							}
							goto IL_07f5;
						}
					}
					empty = string.Format(StringUtil.TR("Attack", "Ping"), actor.GetFancyDisplayName());
				}
				else if (pingType == ActorController.PingType.Move)
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
					uIWorldPing = Object.Instantiate(HUD_UI.Get().m_mainScreenPanel.m_minimap.m_worldPingMovePrefab);
					eventName = "ui/ingame/ping/move";
					BoardSquare boardSquareUnsafe4 = Board.Get().GetBoardSquareUnsafe(vector.x, vector.z);
					if (boardSquareUnsafe4.OccupantActor != null)
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
						if (boardSquareUnsafe4.OccupantActor.IsVisibleToClient())
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
							if (boardSquareUnsafe4.OccupantActor.GetTeam() != actor.GetTeam())
							{
								string arg6 = $"<size=36><sprite=\"CharacterSprites\" index={2 * (int)boardSquareUnsafe4.OccupantActor.m_characterType + 1}>\u200b</size>";
								empty = string.Format(StringUtil.TR("MoveEnemy", "Ping"), actor.GetFancyDisplayName(), arg6, boardSquareUnsafe4.OccupantActor.GetFancyDisplayName());
							}
							else if (boardSquareUnsafe4.OccupantActor != actor)
							{
								string arg7 = $"<size=36><sprite=\"CharacterSprites\" index={2 * (int)boardSquareUnsafe4.OccupantActor.m_characterType}>\u200b</size>";
								empty = string.Format(StringUtil.TR("MoveAlly", "Ping"), actor.GetFancyDisplayName(), arg7, boardSquareUnsafe4.OccupantActor.GetFancyDisplayName());
							}
							else
							{
								empty = string.Format(StringUtil.TR("Move", "Ping"), actor.GetFancyDisplayName());
							}
							goto IL_07f5;
						}
					}
					empty = string.Format(StringUtil.TR("Move", "Ping"), actor.GetFancyDisplayName());
				}
				else
				{
					uIWorldPing = Object.Instantiate(HUD_UI.Get().m_mainScreenPanel.m_minimap.m_worldPingPrefab);
					eventName = "ui/ingame/ping/generic";
					empty = string.Empty;
				}
				goto IL_07f5;
				IL_07f5:
				uIWorldPing.transform.position = vector;
				int num = 0;
				while (num < m_oldPings.Count)
				{
					if (m_oldPings[num] == null)
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
						m_oldPings.RemoveAt(num);
					}
					else if (m_oldPings[num].transform.position == vector)
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
						HUD_UI.Get().m_mainScreenPanel.m_offscreenIndicatorPanel.RemovePing(m_oldPings[num].GetComponent<UIWorldPing>());
						Object.Destroy(m_oldPings[num]);
						m_oldPings.RemoveAt(num);
					}
					else
					{
						num++;
					}
				}
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					m_oldPings.Add(uIWorldPing.gameObject);
					AudioManager.PostEvent(eventName, uIWorldPing.gameObject);
					HUD_UI.Get().m_mainScreenPanel.m_offscreenIndicatorPanel.AddPing(uIWorldPing, pingType, actor);
					GameEventManager.ActorPingEventArgs actorPingEventArgs = new GameEventManager.ActorPingEventArgs();
					actorPingEventArgs.byActor = Actor;
					actorPingEventArgs.pingType = pingType;
					GameEventManager.Get().FireEvent(GameEventManager.EventType.ActorPing, actorPingEventArgs);
					if (!(empty != string.Empty))
					{
						return;
					}
					while (true)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						if (m_lastPingChatTime + 2f < Time.time)
						{
							TextConsole.Get().Write(new TextConsole.Message
							{
								Text = empty,
								MessageType = ConsoleMessageType.PingChat,
								CharacterType = actor.m_characterType,
								SenderTeam = actor.GetTeam()
							});
							m_lastPingChatTime = Time.time;
						}
						return;
					}
				}
			}
		}
	}

	[ClientRpc]
	internal void RpcReceivedAbilityPingInfo(int teamIndex, LocalizationArg_AbilityPing localizedPing, bool spam)
	{
		if (spam)
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
					if (GameFlowData.Get() != null)
					{
						while (true)
						{
							switch (6)
							{
							case 0:
								break;
							default:
								if (GameFlowData.Get().activeOwnedActorData == Actor)
								{
									while (true)
									{
										switch (4)
										{
										case 0:
											break;
										default:
											TextConsole.Get().Write(new TextConsole.Message
											{
												Text = StringUtil.TR("TooManyPings", "Ping"),
												MessageType = ConsoleMessageType.SystemMessage
											});
											return;
										}
									}
								}
								return;
							}
						}
					}
					return;
				}
			}
		}
		if (ClientGameManager.Get().FriendList.Friends.TryGetValue(Actor.GetAccountIdWithSomeConditionA_zq(), out FriendInfo value))
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
			if (value.FriendStatus == FriendStatus.Blocked)
			{
				return;
			}
		}
		TextConsole.Get().Write(new TextConsole.Message
		{
			Text = localizedPing.TR(),
			MessageType = ConsoleMessageType.TeamChat
		});
	}

	public List<ActorTargeting.AbilityRequestData> GetAbilityRequestData()
	{
		return m_abilityRequestData;
	}

	private void SerializeAbilityRequestData(NetworkWriter writer)
	{
		byte b = (byte)m_abilityRequestData.Count;
		writer.Write(b);
		for (int i = 0; i < b; i++)
		{
			sbyte value = (sbyte)m_abilityRequestData[i].m_actionType;
			writer.Write(value);
			AbilityTarget.SerializeAbilityTargetList(m_abilityRequestData[i].m_targets, writer);
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
			return;
		}
	}

	private void DeSerializeAbilityRequestData(NetworkReader reader)
	{
		m_abilityRequestData.Clear();
		byte b = reader.ReadByte();
		for (int i = 0; i < b; i++)
		{
			sbyte b2 = reader.ReadSByte();
			List<AbilityTarget> targets = AbilityTarget.DeSerializeAbilityTargetList(reader);
			AbilityData.ActionType actionType = (AbilityData.ActionType)b2;
			m_abilityRequestData.Add(new ActorTargeting.AbilityRequestData(actionType, targets));
		}
		if (!(Actor != null))
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
			if (Actor.GetActorTargeting() != null)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					Actor.GetActorTargeting().OnRequestDataDeserialized();
					Actor.OnClientQueuedActionChanged();
					return;
				}
			}
			return;
		}
	}

	private void UNetVersion()
	{
	}

	protected static void InvokeRpcRpcMovement(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcMovement called on server.");
		}
		else
		{
			((ActorTeamSensitiveData)obj).RpcMovement((GameEventManager.EventType)reader.ReadInt32(), GeneratedNetworkCode._ReadGridPosProp_None(reader), GeneratedNetworkCode._ReadGridPosProp_None(reader), reader.ReadBytesAndSize(), (ActorData.MovementType)reader.ReadInt32(), reader.ReadBoolean(), reader.ReadBoolean());
		}
	}

	protected static void InvokeRpcRpcReceivedPingInfo(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcReceivedPingInfo called on server.");
		}
		else
		{
			((ActorTeamSensitiveData)obj).RpcReceivedPingInfo((int)reader.ReadPackedUInt32(), reader.ReadVector3(), (ActorController.PingType)reader.ReadInt32(), reader.ReadBoolean());
		}
	}

	protected static void InvokeRpcRpcReceivedAbilityPingInfo(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
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
					Debug.LogError("RPC RpcReceivedAbilityPingInfo called on server.");
					return;
				}
			}
		}
		((ActorTeamSensitiveData)obj).RpcReceivedAbilityPingInfo((int)reader.ReadPackedUInt32(), GeneratedNetworkCode._ReadLocalizationArg_AbilityPing_None(reader), reader.ReadBoolean());
	}

	public void CallRpcMovement(GameEventManager.EventType wait, GridPosProp start, GridPosProp end_grid, byte[] pathBytes, ActorData.MovementType type, bool disappearAfterMovement, bool respawning)
	{
		if (!NetworkServer.active)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					Debug.LogError("RPC Function RpcMovement called on client.");
					return;
				}
			}
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)2);
		networkWriter.WritePackedUInt32((uint)kRpcRpcMovement);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		networkWriter.Write((int)wait);
		GeneratedNetworkCode._WriteGridPosProp_None(networkWriter, start);
		GeneratedNetworkCode._WriteGridPosProp_None(networkWriter, end_grid);
		networkWriter.WriteBytesFull(pathBytes);
		networkWriter.Write((int)type);
		networkWriter.Write(disappearAfterMovement);
		networkWriter.Write(respawning);
		SendRPCInternal(networkWriter, 0, "RpcMovement");
	}

	public void CallRpcReceivedPingInfo(int teamIndex, Vector3 worldPosition, ActorController.PingType pingType, bool spam)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("RPC Function RpcReceivedPingInfo called on client.");
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)2);
		networkWriter.WritePackedUInt32((uint)kRpcRpcReceivedPingInfo);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		networkWriter.WritePackedUInt32((uint)teamIndex);
		networkWriter.Write(worldPosition);
		networkWriter.Write((int)pingType);
		networkWriter.Write(spam);
		SendRPCInternal(networkWriter, 0, "RpcReceivedPingInfo");
	}

	public void CallRpcReceivedAbilityPingInfo(int teamIndex, LocalizationArg_AbilityPing localizedPing, bool spam)
	{
		if (!NetworkServer.active)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					Debug.LogError("RPC Function RpcReceivedAbilityPingInfo called on client.");
					return;
				}
			}
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)2);
		networkWriter.WritePackedUInt32((uint)kRpcRpcReceivedAbilityPingInfo);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		networkWriter.WritePackedUInt32((uint)teamIndex);
		GeneratedNetworkCode._WriteLocalizationArg_AbilityPing_None(networkWriter, localizedPing);
		networkWriter.Write(spam);
		SendRPCInternal(networkWriter, 0, "RpcReceivedAbilityPingInfo");
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		bool result = default(bool);
		return result;
	}
}

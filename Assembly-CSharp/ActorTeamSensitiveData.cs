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

	private static int kRpcRpcMovement = 1638998675;
	private static int kRpcRpcReceivedPingInfo = 1349069861;
	private static int kRpcRpcReceivedAbilityPingInfo = 315009541;

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

	static ActorTeamSensitiveData()
	{
		RegisterRpcDelegate(typeof(ActorTeamSensitiveData), kRpcRpcMovement, InvokeRpcRpcMovement);
		RegisterRpcDelegate(typeof(ActorTeamSensitiveData), kRpcRpcReceivedPingInfo, InvokeRpcRpcReceivedPingInfo);
		RegisterRpcDelegate(typeof(ActorTeamSensitiveData), kRpcRpcReceivedAbilityPingInfo, InvokeRpcRpcReceivedAbilityPingInfo);
		NetworkCRC.RegisterBehaviour("ActorTeamSensitiveData", 0);
	}
	public Team ActorsTeam => Actor != null ? Actor.GetTeam() : Team.Invalid;

	public ActorData Actor
	{
		get
		{
			if (m_associatedActor == null
				&& m_actorIndex != ActorData.s_invalidActorIndex
				&& GameFlowData.Get() != null)
			{
				ActorData actorData = GameFlowData.Get().FindActorByActorIndex(m_actorIndex);
				if (actorData != null)
				{
					m_associatedActor = actorData;
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
			if (m_facingDirAfterMovement != value)
			{
				m_facingDirAfterMovement = value;
				MarkAsDirty(DirtyBit.FacingDirection);
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
			if (m_moveFromBoardSquare != value)
			{
				m_moveFromBoardSquare = value;
				if (NetworkServer.active)
				{
					MarkAsDirty(DirtyBit.MoveFromBoardSquare);
				}
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
			if (m_initialMoveStartSquare != value)
			{
				m_initialMoveStartSquare = value;
				if (NetworkServer.active)
				{
					MarkAsDirty(DirtyBit.InitialMoveStartSquare);
				}
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
				m_movementCameraBounds = value;
			}
			if (NetworkServer.active)
			{
				MarkAsDirty(DirtyBit.MovementCameraBound);
			}
			if (!NetworkClient.active)
			{
				return;
			}
			ActionBufferPhase currentActionPhase = ClientActionBuffer.Get().CurrentActionPhase;
			if (GameFlowData.Get().gameState != GameState.BothTeams_Resolve
				|| GameManager.Get().GameConfig.GameType == GameType.Tutorial)
			{
				return;
			}
			if (ClientActionBuffer.Get() == null || CameraManager.Get() == null)
			{
				return;
			}
			if (!ClientGameManager.Get().IsSpectator)
			{
				ActorData actorData = GameFlowData.Get()?.activeOwnedActorData;
				if (m_associatedActor == null || actorData == null || m_associatedActor.GetTeam() != actorData.GetTeam())
				{
					return;
				}
				if (currentActionPhase != ActionBufferPhase.AbilitiesWait && currentActionPhase != ActionBufferPhase.Movement)
				{
					if (currentActionPhase == ActionBufferPhase.Abilities)
					{
						CameraManager.Get().SaveMovementCameraBound(m_movementCameraBounds);
					}
					return;
				}
				CameraManager.Get().SetTarget(m_movementCameraBounds);
			}
			if (GameFlowData.Get().LocalPlayerData == null || m_associatedActor == null)
			{
				return;
			}
			Team teamViewing = GameFlowData.Get().LocalPlayerData.GetTeamViewing();
			if (teamViewing != m_associatedActor.GetTeam() && teamViewing != Team.Invalid)
			{
				return;
			}
			if (currentActionPhase == ActionBufferPhase.AbilitiesWait
				|| currentActionPhase == ActionBufferPhase.Movement)
			{
				CameraManager.Get().SaveMovementCameraBoundForSpectator(m_movementCameraBounds);
				CameraManager.Get().SetTargetForMovementIfNeeded();
			}
			else if (currentActionPhase == ActionBufferPhase.Abilities)
			{
				CameraManager.Get().SaveMovementCameraBoundForSpectator(m_movementCameraBounds);
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
			if (NetworkServer.active)
			{
				MarkAsDirty(DirtyBit.Respawn);
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
			if (NetworkServer.active)
			{
				MarkAsDirty(DirtyBit.Respawn);
			}
		}
	}

	private void SetActorIndex(int actorIndex)
	{
		if (m_actorIndex == actorIndex && m_associatedActor != null)
		{
			return;
		}
		m_actorIndex = actorIndex;
		m_associatedActor = GameFlowData.Get()?.FindActorByActorIndex(m_actorIndex);
		if (!NetworkServer.active)
		{
			if (m_associatedActor != null)
			{
				if (m_typeObservingMe == ObservedBy.Friendlies)
				{
					m_associatedActor.SetClientFriendlyTeamSensitiveData(this);
				}
				else if (m_typeObservingMe == ObservedBy.Hostiles)
				{
					m_associatedActor.SetClientHostileTeamSensitiveData(this);
				}
			}
			else if (m_actorIndex != ActorData.s_invalidActorIndex)
			{
				TeamSensitiveDataMatchmaker.Get().OnTeamSensitiveDataStarted(this);
			}
		}
	}

	public string GetDebugString()
	{
		string actorDebugName = Actor != null
			? Actor.DebugNameString()
			: "[null] (actor index = " + m_actorIndex + ")";
		return "ActorTeamSensitiveData-- team = " + ActorsTeam.ToString() + ", actor = " + actorDebugName + ", observed by = " + m_typeObservingMe;
	}

	private void Awake()
	{
		for (int i = 0; i < AbilityData.NUM_ACTIONS; i++)
		{
			m_queuedAbilities.Add(false);
			m_abilityToggledOn.Add(false);
		}
	}

	private void Start()
	{
		GameEventManager.Get().AddListener(this, GameEventManager.EventType.TheatricsEvasionMoveStart);
	}

	private void OnDestroy()
	{
		if (GameEventManager.Get() != null)
		{
			GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.TheatricsEvasionMoveStart);
		}
	}

	private void Update()
	{
		if (NetworkClient.active
			&& !NetworkServer.active
			&& Actor == null
			&& m_actorIndex != ActorData.s_invalidActorIndex)
		{
			m_associatedActor = GameFlowData.Get()?.FindActorByActorIndex(m_actorIndex);
			if (Actor != null)
			{
				if (m_typeObservingMe == ObservedBy.Friendlies)
				{
					Actor.SetClientFriendlyTeamSensitiveData(this);
				}
				else if (m_typeObservingMe == ObservedBy.Hostiles)
				{
					Actor.SetClientHostileTeamSensitiveData(this);
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
		Log.Info($"[JSON] {{\"RpcMovement\":{{" +
			$"\"wait\":{DefaultJsonSerializer.Serialize(wait)}," +
			$"\"start\":{DefaultJsonSerializer.Serialize(start)}," +
			$"\"end\":{DefaultJsonSerializer.Serialize(end_grid)}," +
			$"\"pathBytes\":{DefaultJsonSerializer.Serialize(MovementUtils.DeSerializePath(pathBytes))}," +
			$"\"type\":{DefaultJsonSerializer.Serialize(type)}," +
			$"\"disappearAfterMovement\":{DefaultJsonSerializer.Serialize(disappearAfterMovement)}," +
			$"\"respawning\":{DefaultJsonSerializer.Serialize(respawning)}" +
			$"}}}}");
		ProcessMovement(
			wait,
			GridPos.FromGridPosProp(start),
			Board.Get().GetSquare(GridPos.FromGridPosProp(end_grid)),
			MovementUtils.DeSerializePath(pathBytes),
			type,
			disappearAfterMovement,
			respawning);
	}

	private void ProcessMovement(GameEventManager.EventType wait, GridPos start, BoardSquare end, BoardSquarePathInfo path, ActorData.MovementType type, bool disappearAfterMovement, bool respawning)
	{
		FlushQueuedMovement();
		bool doesDestExist = end != null;
		bool isDestChanged = doesDestExist && m_lastMovementDestination != end;
		bool isOnValidSquare = Actor == null || Actor.CurrentBoardSquare == null;
		bool flag4 = doesDestExist && !isDestChanged && !isOnValidSquare && path != null && path.GetPathEndpoint().square == Actor.CurrentBoardSquare;
		m_lastMovementDestination = end;
		m_lastMovementPath = path;
		m_lastMovementWaitForEvent = wait;
		m_lastMovementType = type;
		m_disappearingAfterMovement = disappearAfterMovement;
		bool amMoving = Actor != null && Actor.GetActorMovement() != null && Actor.GetActorMovement().AmMoving();
		int currentTurn = 0;
		if (GameFlowData.Get() != null)
		{
			currentTurn = GameFlowData.Get().CurrentTurn;
		}
		if (!amMoving
			&& wait == GameEventManager.EventType.Invalid
			&& Actor != null
			&& Actor.LastDeathTurn != currentTurn
			&& (!Actor.IsDead() || respawning))
		{
			if (!isDestChanged && (!isOnValidSquare || !doesDestExist) && !flag4)
			{
				if (!doesDestExist && disappearAfterMovement)
				{
					Actor.OnMovementWhileDisappeared(type);
				}
			}
			else
			{
				if (path == null && type != ActorData.MovementType.Teleport)
				{
					Actor.MoveToBoardSquareLocal(end, ActorData.MovementType.Teleport, path, disappearAfterMovement);
				}
				else
				{
					Actor.MoveToBoardSquareLocal(end, type, path, disappearAfterMovement);
				}
				if (respawning && end != null)
				{
					HandleRespawnCharacterVisibility(Actor);
				}
			}
			if (!m_assignedInitialBoardSquare)
			{
				Actor.gameObject.SendMessage("OnAssignedToInitialBoardSquare", SendMessageOptions.DontRequireReceiver);
				m_assignedInitialBoardSquare = true;
			}
		}
		else if (!amMoving
			&& wait != GameEventManager.EventType.Invalid
			&& Actor != null
			&& Actor.LastDeathTurn != currentTurn
			&& (!Actor.IsDead() || respawning))
		{
			BoardSquare square = Board.Get().GetSquare(start);
			if (square != null && square != Actor.CurrentBoardSquare)
			{
				Actor.AppearAtBoardSquare(square);
			}
		}
		else if (Actor != null && respawning)
		{
			HandleRespawnCharacterVisibility(Actor);
		}
	}

	private void HandleRespawnCharacterVisibility(ActorData actor)
	{
		if (FogOfWar.GetClientFog() != null && Actor.GetActorVFX() != null)
		{
			Actor.OnRespawnTeleport();
			Actor.ForceUpdateIsVisibleToClientCache();
			PlayerData localPlayerData = GameFlowData.Get().LocalPlayerData;
			if (localPlayerData != null
				&& SpawnPointManager.Get() != null
				&& SpawnPointManager.Get().m_spawnInDuringMovement)
			{
				ActorModelData actorModelData = Actor.GetActorModelData();
				if (actorModelData != null)
				{
					actorModelData.DisableAndHideRenderers();
				}
				if (HighlightUtils.Get().m_recentlySpawnedShader != null)
				{
					TricksterAfterImageNetworkBehaviour.InitializeAfterImageMaterial(
						Actor.GetActorModelData(),
						localPlayerData.GetTeamViewing() == Actor.GetTeam(),
						0.5f,
						HighlightUtils.Get().m_recentlySpawnedShader,
						false);
				}
			}
		}
	}

	public void EncapsulateVisiblePathBound(ref Bounds bound)
	{
		if (m_lastMovementWaitForEvent != GameEventManager.EventType.Invalid
			&& m_lastMovementPath != null
			&& Actor != null)
		{
			TheatricsManager.EncapsulatePathInBound(ref bound, m_lastMovementPath, Actor);
		}
	}

	public void ClearPreviousMovementInfo()
	{
		if (NetworkServer.active)
		{
			m_lastMovementDestination = null;
			m_lastMovementPath = null;
			m_lastMovementType = ActorData.MovementType.None;
			m_lastMovementWaitForEvent = GameEventManager.EventType.Invalid;
		}
	}

	public void FlushQueuedMovement()
	{
		if (NetworkClient.active)
		{
			if (Actor != null
				&& !Actor.IsDead()
				&& m_lastMovementDestination != null
				&& Actor.CurrentBoardSquare != m_lastMovementDestination
				&& (Actor.CurrentBoardSquare != null || !Actor.DisappearingAfterCurrentMovement))
			{
				Actor.MoveToBoardSquareLocal(m_lastMovementDestination, ActorData.MovementType.Teleport, null, m_disappearingAfterMovement);
			}
			m_lastMovementPath = null;
			m_lastMovementWaitForEvent = GameEventManager.EventType.Invalid;
		}
	}

	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
		string jsonLog = "[JSON] {\"actorTeamSensitiveData\": {";
		uint setBits = uint.MaxValue;
		if (!initialState)
		{
			setBits = reader.ReadPackedUInt32();
		}
		sbyte actorIndex = reader.ReadSByte();
		SetActorIndex(actorIndex);
		jsonLog += $"\"actorIndex\": {actorIndex}," +
			$"\"initialState\": {DefaultJsonSerializer.Serialize(initialState)}," +
			"\"dirtyBits\":{" +
			$"\"FacingDirection\": {DefaultJsonSerializer.Serialize(IsBitDirty(setBits, DirtyBit.FacingDirection))}," +
			$"\"MoveFromBoardSquare\": {DefaultJsonSerializer.Serialize(IsBitDirty(setBits, DirtyBit.MoveFromBoardSquare))}," +
			$"\"InitialMoveStartSquare\": {DefaultJsonSerializer.Serialize(IsBitDirty(setBits, DirtyBit.InitialMoveStartSquare))}," +
			$"\"LineData\": {DefaultJsonSerializer.Serialize(IsBitDirty(setBits, DirtyBit.LineData))}," +
			$"\"MovementCameraBound\": {DefaultJsonSerializer.Serialize(IsBitDirty(setBits, DirtyBit.MovementCameraBound))}," +
			$"\"Respawn\": {DefaultJsonSerializer.Serialize(IsBitDirty(setBits, DirtyBit.Respawn))}," +
			$"\"QueuedAbilities\": {DefaultJsonSerializer.Serialize(IsBitDirty(setBits, DirtyBit.QueuedAbilities))}," +
			$"\"AbilityRequestDataForTargeter\": {DefaultJsonSerializer.Serialize(IsBitDirty(setBits, DirtyBit.AbilityRequestDataForTargeter))}," +
			$"\"ToggledOnAbilities\": {DefaultJsonSerializer.Serialize(IsBitDirty(setBits, DirtyBit.ToggledOnAbilities))}" +
			"}";
		if (IsBitDirty(setBits, DirtyBit.FacingDirection))
		{
			short angle = reader.ReadInt16();
			if (angle < 0)
			{
				m_facingDirAfterMovement = Vector3.zero;
			}
			else
			{
				m_facingDirAfterMovement = VectorUtils.AngleDegreesToVector(angle);
			}
			if (Actor != null)
			{
				Actor.SetFacingDirectionAfterMovement(m_facingDirAfterMovement);
			}
			jsonLog += $",\"angle\": {angle}";
		}
		if (IsBitDirty(setBits, DirtyBit.MoveFromBoardSquare))
		{
			short x = reader.ReadInt16();
			short y = reader.ReadInt16();
			BoardSquare boardSquare = Board.Get().GetSquareFromIndex(x, y);
			if (boardSquare != MoveFromBoardSquare)
			{
				MoveFromBoardSquare = boardSquare;
				if (Actor != null && Actor.GetActorMovement() != null)
				{
					Actor.GetActorMovement().UpdateSquaresCanMoveTo();
				}
			}
			jsonLog += $",\"MoveFromBoardSquare\": {DefaultJsonSerializer.Serialize(MoveFromBoardSquare)}";
		}
		if (IsBitDirty(setBits, DirtyBit.InitialMoveStartSquare))
		{
			short x2 = reader.ReadInt16();
			short y2 = reader.ReadInt16();
			BoardSquare boardSquare = Board.Get().GetSquareFromIndex(x2, y2);
			if (InitialMoveStartSquare != boardSquare)
			{
				InitialMoveStartSquare = boardSquare;
				if (Actor != null && Actor.GetActorMovement() != null)
				{
					Actor.GetActorMovement().UpdateSquaresCanMoveTo();
				}
			}
			jsonLog += $",\"InitialMoveStartSquare\": {DefaultJsonSerializer.Serialize(InitialMoveStartSquare)}";
		}
		if (IsBitDirty(setBits, DirtyBit.LineData))
		{
			byte bitField = reader.ReadByte();
			ServerClientUtils.GetBoolsFromBitfield(bitField, out bool movementLineFlag, out bool numNodesInSnaredFlag);
			if (movementLineFlag)
			{
				m_movementLine = LineData.DeSerializeLine(reader);
			}
			else
			{
				m_movementLine = null;
			}
			if (numNodesInSnaredFlag)
			{
				m_numNodesInSnaredLine = reader.ReadSByte();
			}
			else
			{
				m_numNodesInSnaredLine = 0;
			}
			if (Actor != null)
			{
				LineData component = Actor.GetComponent<LineData>();
				if (component != null)
				{
					component.OnDeserializedData(m_movementLine, m_numNodesInSnaredLine);
				}
			}
			jsonLog += $",\"movementLine\": {DefaultJsonSerializer.Serialize(m_movementLine)}";
			jsonLog += $",\"numNodesInSnaredLine\": {m_numNodesInSnaredLine}";
		}
		if (IsBitDirty(setBits, DirtyBit.MovementCameraBound))
		{
			short x = reader.ReadInt16();
			short z = reader.ReadInt16();
			short w = reader.ReadInt16();
			short h = reader.ReadInt16();
			Vector3 center = new Vector3(x, 1.5f + Board.Get().BaselineHeight, z);
			Vector3 size = new Vector3(w, 3f, h);
			MovementCameraBounds = new Bounds(center, size);
			jsonLog += $",\"MovementCameraBounds\": {DefaultJsonSerializer.Serialize(MovementCameraBounds)}";
		}
		if (IsBitDirty(setBits, DirtyBit.Respawn))
		{
			short x = reader.ReadInt16();
			short y = reader.ReadInt16();
			RespawnPickedSquare = Board.Get().GetSquareFromIndex(x, y);
			bool respawningThisTurn = reader.ReadBoolean();
			if (Actor != null && (RespawnPickedSquare != null || !respawningThisTurn))
			{
				Actor.ShowRespawnFlare(RespawnPickedSquare, respawningThisTurn);
			}

			short respawnAvailableSquaresNum = reader.ReadInt16();

			jsonLog += $",\"RespawnPickedSquare\": {DefaultJsonSerializer.Serialize(RespawnPickedSquare)}";
			jsonLog += $",\"respawningThisTurn\": {respawningThisTurn}";
			jsonLog += $",\"respawnAvailableSquaresNum\": {respawnAvailableSquaresNum}";
			m_respawnAvailableSquares.Clear();
			for (int i = 0; i < respawnAvailableSquaresNum; i++)
			{
				short x3 = reader.ReadInt16();
				short y3 = reader.ReadInt16();
				BoardSquare respawnAvailableSquare = Board.Get().GetSquareFromIndex(x3, y3);
				if (respawnAvailableSquare != null)
				{
					m_respawnAvailableSquares.Add(respawnAvailableSquare);
				}
				else
				{
					Log.Error("Invalid square received for respawn choices {0}, {1}", x3, y3);
				}
			}
			if (m_respawnAvailableSquares.Count > 0
				&& RespawnPickedSquare == null
				&& Actor != null
				&& GameFlowData.Get() != null
				&& Actor == GameFlowData.Get().activeOwnedActorData
				&& Actor.GetActorTurnSM().AmStillDeciding())
			{
				Actor.ShowRespawnFlare(m_respawnAvailableSquares[0], false);
			}
			jsonLog += $",\"respawnAvailableSquares\": {DefaultJsonSerializer.Serialize(m_respawnAvailableSquares)}";
		}
		if (IsBitDirty(setBits, DirtyBit.QueuedAbilities) || IsBitDirty(setBits, DirtyBit.AbilityRequestDataForTargeter))
		{
			DeSerializeAbilityRequestData(reader);
			jsonLog += $",\"abilityRequestData\": {DefaultJsonSerializer.Serialize(m_abilityRequestData)}";
		}
		if (IsBitDirty(setBits, DirtyBit.QueuedAbilities))
		{
			bool changed = false;
			short queuedAbilitiesBitmask = reader.ReadInt16();
			jsonLog += $",\"queuedAbilitiesBitmask\": {System.Convert.ToString(queuedAbilitiesBitmask, 2)}";
			for (int j = 0; j < AbilityData.NUM_ACTIONS; j++)
			{
				short flag = (short)(1 << j);
				bool isAbilityQueued = (queuedAbilitiesBitmask & flag) != 0;
				if (m_queuedAbilities[j] != isAbilityQueued)
				{
					m_queuedAbilities[j] = isAbilityQueued;
					changed = true;
				}
			}
			if (changed && Actor != null && Actor.GetAbilityData() != null)
			{
				Actor.GetAbilityData().OnQueuedAbilitiesChanged();
			}
		}
		if (IsBitDirty(setBits, DirtyBit.ToggledOnAbilities))
		{
			short toggledOnAbilitiesBitmask = reader.ReadInt16();
			jsonLog += $",\"toggledOnAbilitiesBitmask\": {System.Convert.ToString(toggledOnAbilitiesBitmask, 2)}";
			for (int k = 0; k < AbilityData.NUM_ACTIONS; k++)
			{
				short flag = (short)(1 << k);
				bool isAbilityToggledOn = (toggledOnAbilitiesBitmask & flag) != 0;
				if (m_abilityToggledOn[k] != isAbilityToggledOn)
				{
					m_abilityToggledOn[k] = isAbilityToggledOn;
				}
			}
		}
		jsonLog += "}}";
		Log.Info(jsonLog);
	}

	public void OnClientAssociatedWithActor(ActorData actor)
	{
		if (!NetworkServer.active)
		{
			m_associatedActor = actor;
			if (m_lastMovementDestination != null)
			{
				if (m_lastMovementPath == null && m_lastMovementType != ActorData.MovementType.Teleport)
				{
					Actor.MoveToBoardSquareLocal(m_lastMovementDestination, ActorData.MovementType.Teleport, m_lastMovementPath, m_disappearingAfterMovement);
				}
				else
				{
					Actor.MoveToBoardSquareLocal(m_lastMovementDestination, m_lastMovementType, m_lastMovementPath, m_disappearingAfterMovement);
				}
				if (!m_assignedInitialBoardSquare)
				{
					Actor.gameObject.SendMessage("OnAssignedToInitialBoardSquare", SendMessageOptions.DontRequireReceiver);
					m_assignedInitialBoardSquare = true;
				}
			}
			Actor.GetActorMovement().UpdateSquaresCanMoveTo();
			if (m_typeObservingMe == ObservedBy.Friendlies)
			{
				LineData component = Actor.GetComponent<LineData>();
				if (component != null)
				{
					component.OnDeserializedData(m_movementLine, m_numNodesInSnaredLine);
				}
			}
		}
	}

	public void OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		if (eventType == m_lastMovementWaitForEvent && this == Actor.TeamSensitiveData_authority)
		{
			Actor.MoveToBoardSquareLocal(m_lastMovementDestination, m_lastMovementType, m_lastMovementPath, m_disappearingAfterMovement);
			m_lastMovementPath = null;
			m_lastMovementWaitForEvent = GameEventManager.EventType.Invalid;
		}
	}

	public void OnTurnTick()
	{
		FlushQueuedMovement();
	}

	public bool HasToggledAction(AbilityData.ActionType actionType)
	{
		return actionType != AbilityData.ActionType.INVALID_ACTION
			&& actionType >= AbilityData.ActionType.ABILITY_0
			&& (int)actionType < m_abilityToggledOn.Count
			&& m_abilityToggledOn[(int)actionType];
	}

	[Server]
	public void SetToggledAction(AbilityData.ActionType actionType, bool toggledOn)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void ActorTeamSensitiveData::SetToggledAction(AbilityData/ActionType,System.Boolean)' called on client");
			return;
		}
		if (actionType != AbilityData.ActionType.INVALID_ACTION && !AbilityData.IsChain(actionType) && m_abilityToggledOn[(int)actionType] != toggledOn)
		{
			m_abilityToggledOn[(int)actionType] = toggledOn;
			MarkAsDirty(DirtyBit.ToggledOnAbilities);
		}
	}

	public bool HasQueuedAction(AbilityData.ActionType actionType)
	{
		return actionType != AbilityData.ActionType.INVALID_ACTION
			&& actionType >= AbilityData.ActionType.ABILITY_0
			&& (int)actionType < m_queuedAbilities.Count
			&& m_queuedAbilities[(int)actionType];
	}

	public bool HasQueuedAction(int actionTypeInt)
	{
		return HasQueuedAction((AbilityData.ActionType)actionTypeInt);
	}

	public bool HasQueuedAbilityInPhase(AbilityPriority phase)
	{
		for (int i = 0; i < m_queuedAbilities.Count; i++)
		{
			if (m_queuedAbilities[i])
			{
				Ability abilityOfActionType = Actor.GetAbilityData().GetAbilityOfActionType((AbilityData.ActionType)i);
				if (abilityOfActionType != null && abilityOfActionType.RunPriority == phase)
				{
					return true;
				}
			}
		}
		return false;
	}

	[Server]
	internal void SetQueuedAction(AbilityData.ActionType actionType, bool queued)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void ActorTeamSensitiveData::SetQueuedAction(AbilityData/ActionType,System.Boolean)' called on client");
			return;
		}
		if (actionType != AbilityData.ActionType.INVALID_ACTION && !AbilityData.IsChain(actionType) && HasQueuedAction(actionType) != queued)
		{
			m_queuedAbilities[(int)actionType] = queued;
			MarkAsDirty(DirtyBit.QueuedAbilities);
		}
	}

	[Server]
	internal void UnqueueActions()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void ActorTeamSensitiveData::UnqueueActions()' called on client");
			return;
		}
		for (int i = 0; i < m_queuedAbilities.Count; i++)
		{
			if (m_queuedAbilities[i])
			{
				m_queuedAbilities[i] = false;
				MarkAsDirty(DirtyBit.QueuedAbilities);
			}
		}
	}

	[ClientRpc]
	internal void RpcReceivedPingInfo(int teamIndex, Vector3 worldPosition, ActorController.PingType pingType, bool spam)
	{
		Log.Info($"[JSON] {{\"RpcReceivedPingInfo\":{{" +
			$"\"teamIndex\":{DefaultJsonSerializer.Serialize(teamIndex)}," +
			$"\"worldPosition\":{DefaultJsonSerializer.Serialize(worldPosition)}," +
			$"\"pingType\":{DefaultJsonSerializer.Serialize(pingType)}," +
			$"\"spam\":{DefaultJsonSerializer.Serialize(spam)}" +
			$"}}}}");
		if (spam)
		{
			if (GameFlowData.Get() != null && GameFlowData.Get().activeOwnedActorData == Actor)
			{
				TextConsole.Get().Write(new TextConsole.Message
				{
					Text = StringUtil.TR("TooManyPings", "Ping"),
					MessageType = ConsoleMessageType.SystemMessage
				});
			}
			return;
		}
		if (GameFlowData.Get() != null
			&& GameFlowData.Get().activeOwnedActorData != null
			&& Actor != null
			&& GameFlowData.Get().activeOwnedActorData.GetTeam() == (Team)teamIndex
			&& HUD_UI.Get() != null
			&& HUD_UI.Get().m_mainScreenPanel != null
			&& HUD_UI.Get().m_mainScreenPanel.m_minimap != null)
		{
			Vector3 vector = new Vector3(worldPosition.x, Board.Get().BaselineHeight, worldPosition.z);
			ActorData actor = Actor;
			string text = "";
			UIWorldPing uIWorldPing;
			string eventName;
			if (pingType == ActorController.PingType.Assist)
			{
				uIWorldPing = Object.Instantiate(HUD_UI.Get().m_mainScreenPanel.m_minimap.m_worldPingAssistPrefab);
				eventName = "ui/ingame/ping/assist";
				BoardSquare closestSquare = Board.Get().GetSquareClosestToPos(vector.x, vector.z);
				if (closestSquare.OccupantActor != null && closestSquare.OccupantActor.IsActorVisibleToClient())
				{
					if (closestSquare.OccupantActor.GetTeam() != actor.GetTeam())
					{
						string arg = $"<size=36><sprite=\"CharacterSprites\" index={2 * (int)closestSquare.OccupantActor.m_characterType + 1}>\u200b</size>";
						text = string.Format(StringUtil.TR("AssistEnemy", "Ping"), actor.GetDisplayName(), arg, closestSquare.OccupantActor.GetDisplayName());
					}
					else if (closestSquare.OccupantActor != actor)
					{
						string arg2 = $"<size=36><sprite=\"CharacterSprites\" index={2 * (int)closestSquare.OccupantActor.m_characterType}>\u200b</size>";
						text = string.Format(StringUtil.TR("AssistAlly", "Ping"), actor.GetDisplayName(), arg2, closestSquare.OccupantActor.GetDisplayName());
					}
					else
					{
						text = string.Format(StringUtil.TR("Assist", "Ping"), actor.GetDisplayName());
					}
				}
				else
				{
					text = string.Format(StringUtil.TR("Assist", "Ping"), actor.GetDisplayName());
				}
			}
			else if (pingType == ActorController.PingType.Defend)
			{
				uIWorldPing = Object.Instantiate(HUD_UI.Get().m_mainScreenPanel.m_minimap.m_worldPingDefendPrefab);
				eventName = "ui/ingame/ping/anger";
				BoardSquare closestSquare = Board.Get().GetSquareClosestToPos(vector.x, vector.z);
				if (closestSquare.OccupantActor != null && closestSquare.OccupantActor.IsActorVisibleToClient())
				{
					if (closestSquare.OccupantActor.GetTeam() != actor.GetTeam())
					{
						text = string.Format(
							StringUtil.TR("DangerEnemy", "Ping"),
							actor.GetDisplayName(),
							$"<size=36><sprite=\"CharacterSprites\" index={2 * (int)closestSquare.OccupantActor.m_characterType + 1}>\u200b</size>",
							closestSquare.OccupantActor.GetDisplayName());
					}
					else if (closestSquare.OccupantActor != actor)
					{
						text = string.Format(
							StringUtil.TR("DangerAlly", "Ping"),
							actor.GetDisplayName(),
							$"<size=36><sprite=\"CharacterSprites\" index={2 * (int)closestSquare.OccupantActor.m_characterType}>\u200b</size>",
							closestSquare.OccupantActor.GetDisplayName());
					}
					else
					{
						text = string.Format(StringUtil.TR("Danger", "Ping"), actor.GetDisplayName());
					}
				}
				else
				{
					text = string.Format(StringUtil.TR("Danger", "Ping"), actor.GetDisplayName());
				}
			}
			else if (pingType == ActorController.PingType.Enemy)
			{
				uIWorldPing = Object.Instantiate(HUD_UI.Get().m_mainScreenPanel.m_minimap.m_worldPingEnemyPrefab);
				eventName = "ui/ingame/ping/attack";
				BoardSquare closestSquare = Board.Get().GetSquareClosestToPos(vector.x, vector.z);
				if (closestSquare.OccupantActor != null && closestSquare.OccupantActor.IsActorVisibleToClient())
				{
					if (closestSquare.OccupantActor.GetTeam() != actor.GetTeam())
					{
						text = string.Format(
							StringUtil.TR("AttackEnemy", "Ping"),
							actor.GetDisplayName(),
							$"<size=36><sprite=\"CharacterSprites\" index={2 * (int)closestSquare.OccupantActor.m_characterType + 1}>\u200b</size>",
							closestSquare.OccupantActor.GetDisplayName());
					}
					else
					{
						text = string.Format(StringUtil.TR("Attack", "Ping"), actor.GetDisplayName());
					}
				}
				else
				{
					text = string.Format(StringUtil.TR("Attack", "Ping"), actor.GetDisplayName());
				}
			}
			else if (pingType == ActorController.PingType.Move)
			{
				uIWorldPing = Object.Instantiate(HUD_UI.Get().m_mainScreenPanel.m_minimap.m_worldPingMovePrefab);
				eventName = "ui/ingame/ping/move";
				BoardSquare closestSquare = Board.Get().GetSquareClosestToPos(vector.x, vector.z);
				if (closestSquare.OccupantActor != null && closestSquare.OccupantActor.IsActorVisibleToClient())
				{
					if (closestSquare.OccupantActor.GetTeam() != actor.GetTeam())
					{
						text = string.Format(
							StringUtil.TR("MoveEnemy", "Ping"),
							actor.GetDisplayName(),
							$"<size=36><sprite=\"CharacterSprites\" index={2 * (int)closestSquare.OccupantActor.m_characterType + 1}>\u200b</size>",
							closestSquare.OccupantActor.GetDisplayName());
					}
					else if (closestSquare.OccupantActor != actor)
					{
						text = string.Format(StringUtil.TR("MoveAlly", "Ping"),
							actor.GetDisplayName(),
							$"<size=36><sprite=\"CharacterSprites\" index={2 * (int)closestSquare.OccupantActor.m_characterType}>\u200b</size>",
							closestSquare.OccupantActor.GetDisplayName());
					}
					else
					{
						text = string.Format(StringUtil.TR("Move", "Ping"), actor.GetDisplayName());
					}
				}
				else
				{
					text = string.Format(StringUtil.TR("Move", "Ping"), actor.GetDisplayName());
				}
			}
			else
			{
				uIWorldPing = Object.Instantiate(HUD_UI.Get().m_mainScreenPanel.m_minimap.m_worldPingPrefab);
				eventName = "ui/ingame/ping/generic";
				text = "";
			}
			uIWorldPing.transform.position = vector;
			int num = 0;
			while (num < m_oldPings.Count)
			{
				if (m_oldPings[num] == null)
				{
					m_oldPings.RemoveAt(num);
				}
				else if (m_oldPings[num].transform.position == vector)
				{
					HUD_UI.Get().m_mainScreenPanel.m_offscreenIndicatorPanel.RemovePing(m_oldPings[num].GetComponent<UIWorldPing>());
					Object.Destroy(m_oldPings[num]);
					m_oldPings.RemoveAt(num);
				}
				else
				{
					num++;
				}
			}
			m_oldPings.Add(uIWorldPing.gameObject);
			AudioManager.PostEvent(eventName, uIWorldPing.gameObject);
			HUD_UI.Get().m_mainScreenPanel.m_offscreenIndicatorPanel.AddPing(uIWorldPing, pingType, actor);
			GameEventManager.ActorPingEventArgs actorPingEventArgs = new GameEventManager.ActorPingEventArgs();
			actorPingEventArgs.byActor = Actor;
			actorPingEventArgs.pingType = pingType;
			GameEventManager.Get().FireEvent(GameEventManager.EventType.ActorPing, actorPingEventArgs);
			if (text != "" && m_lastPingChatTime + 2f < Time.time)
			{
				TextConsole.Get().Write(new TextConsole.Message
				{
					Text = text,
					MessageType = ConsoleMessageType.PingChat,
					CharacterType = actor.m_characterType,
					SenderTeam = actor.GetTeam()
				});
				m_lastPingChatTime = Time.time;
			}
		}
	}

	[ClientRpc]
	internal void RpcReceivedAbilityPingInfo(int teamIndex, LocalizationArg_AbilityPing localizedPing, bool spam)
	{
		Log.Info($"[JSON] {{\"RpcReceivedAbilityPingInfo\":{{" +
			$"\"teamIndex\":{DefaultJsonSerializer.Serialize(teamIndex)}," +
			$"\"localizedPing\":{DefaultJsonSerializer.Serialize(localizedPing)}," +
			$"\"spam\":{DefaultJsonSerializer.Serialize(spam)}" +
			$"}}}}");
		if (spam)
		{
			if (GameFlowData.Get() != null && GameFlowData.Get().activeOwnedActorData == Actor)
			{
				TextConsole.Get().Write(new TextConsole.Message
				{
					Text = StringUtil.TR("TooManyPings", "Ping"),
					MessageType = ConsoleMessageType.SystemMessage
				});
			}
		}
		else if (!ClientGameManager.Get().FriendList.Friends.TryGetValue(Actor.GetAccountId(), out FriendInfo value) || value.FriendStatus != FriendStatus.Blocked)
		{
			TextConsole.Get().Write(new TextConsole.Message
			{
				Text = localizedPing.TR(),
				MessageType = ConsoleMessageType.TeamChat
			});
		}
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
	}

	private void DeSerializeAbilityRequestData(NetworkReader reader)
	{
		m_abilityRequestData.Clear();
		byte b = reader.ReadByte();
		for (int i = 0; i < b; i++)
		{
			AbilityData.ActionType actionType = (AbilityData.ActionType)reader.ReadSByte();
			List<AbilityTarget> targets = AbilityTarget.DeSerializeAbilityTargetList(reader);
			m_abilityRequestData.Add(new ActorTargeting.AbilityRequestData(actionType, targets));
		}
		if (Actor != null && Actor.GetActorTargeting() != null)
		{
			Actor.GetActorTargeting().OnRequestDataDeserialized();
			Actor.OnClientQueuedActionChanged();
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
			return;
		}
		((ActorTeamSensitiveData)obj).RpcMovement((GameEventManager.EventType)reader.ReadInt32(), GeneratedNetworkCode._ReadGridPosProp_None(reader), GeneratedNetworkCode._ReadGridPosProp_None(reader), reader.ReadBytesAndSize(), (ActorData.MovementType)reader.ReadInt32(), reader.ReadBoolean(), reader.ReadBoolean());
	}

	protected static void InvokeRpcRpcReceivedPingInfo(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcReceivedPingInfo called on server.");
			return;
		}
		((ActorTeamSensitiveData)obj).RpcReceivedPingInfo((int)reader.ReadPackedUInt32(), reader.ReadVector3(), (ActorController.PingType)reader.ReadInt32(), reader.ReadBoolean());
	}

	protected static void InvokeRpcRpcReceivedAbilityPingInfo(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcReceivedAbilityPingInfo called on server.");
			return;
		}
		((ActorTeamSensitiveData)obj).RpcReceivedAbilityPingInfo((int)reader.ReadPackedUInt32(), GeneratedNetworkCode._ReadLocalizationArg_AbilityPing_None(reader), reader.ReadBoolean());
	}

	public void CallRpcMovement(GameEventManager.EventType wait, GridPosProp start, GridPosProp end_grid, byte[] pathBytes, ActorData.MovementType type, bool disappearAfterMovement, bool respawning)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("RPC Function RpcMovement called on client.");
			return;
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
			Debug.LogError("RPC Function RpcReceivedAbilityPingInfo called on client.");
			return;
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
		return false;
	}
}

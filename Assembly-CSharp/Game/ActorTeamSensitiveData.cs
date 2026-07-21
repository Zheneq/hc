// ROGUES
// SERVER
using System.Collections.Generic;
using System.Linq;
using Theatrics;
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

	// removed in rogues
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

	// added in rogues
#if SERVER
	private bool m_respawning;
#endif

	private bool m_disappearingAfterMovement;
	private bool m_assignedInitialBoardSquare;
	private List<GameObject> m_oldPings = new List<GameObject>();
	private float m_lastPingChatTime;
	private List<ActorTargeting.AbilityRequestData> m_abilityRequestData = new List<ActorTargeting.AbilityRequestData>();

	static ActorTeamSensitiveData()
	{
		// reactor
		RegisterRpcDelegate(typeof(ActorTeamSensitiveData), kRpcRpcMovement, InvokeRpcRpcMovement);
		RegisterRpcDelegate(typeof(ActorTeamSensitiveData), kRpcRpcReceivedPingInfo, InvokeRpcRpcReceivedPingInfo);
		RegisterRpcDelegate(typeof(ActorTeamSensitiveData), kRpcRpcReceivedAbilityPingInfo, InvokeRpcRpcReceivedAbilityPingInfo);
		NetworkCRC.RegisterBehaviour("ActorTeamSensitiveData", 0);
		// rogues
		//RegisterRpcDelegate(typeof(ActorTeamSensitiveData), "RpcMovement", new NetworkBehaviour.CmdDelegate(ActorTeamSensitiveData.InvokeRpcRpcMovement));
		//RegisterRpcDelegate(typeof(ActorTeamSensitiveData), "RpcReceivedPingInfo", new NetworkBehaviour.CmdDelegate(ActorTeamSensitiveData.InvokeRpcRpcReceivedPingInfo));
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
		get => m_facingDirAfterMovement;
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
		get => m_moveFromBoardSquare;
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
		get => m_initialMoveStartSquare;
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
		get => m_movementCameraBounds;
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

			// below removed in rogues
			if (!NetworkClient.active)
			{
				return;
			}
			
			ActionBufferPhase currentActionPhase = ClientActionBuffer.Get().CurrentActionPhase;
			if (GameFlowData.Get().gameState != GameState.BothTeams_Resolve
				|| GameManager.Get().GameConfig.GameType == GameType.Tutorial 
				|| ClientActionBuffer.Get() == null
				|| CameraManager.Get() == null)
			{
				return;
			}
			
			if (!ClientGameManager.Get().IsSpectator)
			{
				ActorData actorData = GameFlowData.Get()?.activeOwnedActorData;
				if (m_associatedActor == null
				    || actorData == null
				    || m_associatedActor.GetTeam() != actorData.GetTeam())
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
		get => m_respawnPickedSquare;
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
		get => m_respawnAvailableSquares;
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

				// removed in rogues
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
			? Actor.DebugNameString() // + " netId:" + Actor.GetComponent<NetworkIdentity>().netId   in rogues
			: "[null] (actor index = " + m_actorIndex + ")";
		return "ActorTeamSensitiveData-- team = " + ActorsTeam + ", actor = " + actorDebugName + ", observed by = " + m_typeObservingMe;
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

				// removed in rogues
				else if (m_typeObservingMe == ObservedBy.Hostiles)
				{
					Actor.SetClientHostileTeamSensitiveData(this);
				}
			}
		}
	}

	// added in rogues
#if SERVER
	public void InitToActor(ActorData actor)
	{
		m_actorIndex = actor.ActorIndex;
		m_associatedActor = actor;
	}
#endif

	// added in rogues
#if SERVER
	private PlayerDetails GetPlayerDetailsOfObserver(NetworkConnection observer)
	{
		PlayerDetails result = null;
		foreach (Player player in GameFlow.Get().playerDetails.Keys)
		{
			if (player.m_connectionId == observer.connectionId)
			{
				result = GameFlow.Get().playerDetails[player];
				break;
			}
		}
		return result;
	}
#endif

	public void MarkAsDirty(DirtyBit bit)
	{
#if SERVER
		// server-only
		if (NetworkServer.active)
		{
			SetDirtyBit((uint)bit);  // ulong in rogues
		}
#endif
	}

	private bool IsBitDirty(uint setBits, DirtyBit bitToTest)  // ulong in rogues
	{
		return ((int)setBits & (int)bitToTest) != 0;  // ulong in rogues
	}

	public void MarkAsRespawning()
	{
#if SERVER
		// server-only
		if (NetworkServer.active)
		{
			m_respawning = true;
		}
#endif
	}

	// added in rogues
#if SERVER
	public bool ShouldAdjustMovementToVisibility(
		ActorData.MovementType movementType,
		ActorData.TeleportType teleportType,
		BoardSquarePathInfo path)
	{
		return NetworkServer.active
		       && m_typeObservingMe == ObservedBy.Hostiles
		       && (movementType == ActorData.MovementType.Normal || movementType == ActorData.MovementType.Teleport)
		       && (movementType != ActorData.MovementType.Teleport || teleportType == ActorData.TeleportType.Evasion_AdjustToVision)
		       && path != null
		       && (ServerEffectManager.Get() == null || !ServerEffectManager.Get().HasEffectRequiringAccuratePositionOnClients(Actor));
	}
#endif

	// added in rogues
#if SERVER
	public void BroadcastMovement(
		GameEventManager.EventType eventType,
		GridPos start,
		BoardSquare dest,
		ActorData.MovementType movementType,
		ActorData.TeleportType teleportType,
		BoardSquarePathInfo path)
	{
		if (Actor.TeamSensitiveData_authority == this && eventType == GameEventManager.EventType.Invalid)
		{
			Actor.MoveToBoardSquareLocal(dest, movementType, path, false);
		}
		
		bool adjustMovementToVisibility = ShouldAdjustMovementToVisibility(movementType, teleportType, path);
		m_lastMovementDestination = dest;
		m_lastMovementPath = path;
		m_lastMovementWaitForEvent = eventType;
		m_lastMovementType = movementType;
		
		if (adjustMovementToVisibility)
		{
			bool isObviousMovement = movementType == ActorData.MovementType.Normal
			             || movementType == ActorData.MovementType.Charge
			             || movementType == ActorData.MovementType.Knockback;
			BoardSquarePathInfo pathCopy = path.Clone(null);
			BoardSquarePathInfo step = pathCopy;
			BoardSquarePathInfo lastVisibleStep = null;
			while (step != null)
			{
				if (step.m_visibleToEnemies
				    || step.m_updateLastKnownPos
				    || step.m_moverDiesHere
				    || step.m_moverHasGameplayHitHere)
				{
					lastVisibleStep = step;
				}
				else if (step.prev != null
				         && isObviousMovement
				         && (step.prev.m_visibleToEnemies || step.prev.m_moverHasGameplayHitHere))
				{
					lastVisibleStep = step;
				}
				if (step.next == null)
				{
					break;
				}
				step = step.next;
			}
			if (lastVisibleStep != step)
			{
				FacingDirAfterMovement = Vector3.zero;
				m_disappearingAfterMovement = true;
				if (lastVisibleStep != null)
				{
					lastVisibleStep.next = null;
					Log.Info($"BroadcastMovement {Actor.m_displayName} {m_typeObservingMe} {movementType} {teleportType}"
					         + $" & disappear {start} -> {lastVisibleStep.square?.GetGridPos()}"
					         + (m_typeObservingMe == ObservedBy.Hostiles ? $" (in fact to {dest})" : "")); // custom debug
					PackageRpcMovement(eventType, start, lastVisibleStep.square, pathCopy, movementType, true, m_respawning);
					m_respawning = false;
				}
				else
				{
					Log.Info($"BroadcastMovement {Actor.m_displayName} {m_typeObservingMe} {movementType} {teleportType}"
					         + " & disappear null -> null"
					         + (m_typeObservingMe == ObservedBy.Hostiles ? $" (in fact to {dest})" : "")); // custom debug
					PackageRpcMovement(eventType, GridPos.s_invalid, null, null, movementType, true, m_respawning);
					m_respawning = false;
				}
				return;
			}
		}
		Log.Info($"BroadcastMovement {Actor.m_displayName} {m_typeObservingMe} {movementType} {teleportType} {start} -> {dest}"); // custom debug
		PackageRpcMovement(eventType, start, dest, path, movementType, false, m_respawning);
		m_respawning = false;
	}
#endif

#if SERVER
	private void PackageRpcMovement(
		GameEventManager.EventType wait,
		GridPos start,
		BoardSquare end,
		BoardSquarePathInfo path,
		ActorData.MovementType type,
		bool disappearAfterMovement,
		bool respawning)
	{
		CallRpcMovement(
			wait,
			GridPosProp.FromGridPos(start),
			GridPosProp.FromGridPos(
				end != null
					? end.GetGridPos()
					: GridPos.s_invalid),
			MovementUtils.SerializePath(path),
			type,
			disappearAfterMovement,
			respawning);
	}
#endif

	[ClientRpc]
	private void RpcMovement(
		GameEventManager.EventType wait,
		GridPosProp start,
		GridPosProp end_grid,
		byte[] pathBytes,
		ActorData.MovementType type,
		bool disappearAfterMovement,
		bool respawning)
	{
		if (NetworkServer.active)
		{
			return;
		}
		
		ProcessMovement(
			wait,
			GridPos.FromGridPosProp(start),
			Board.Get().GetSquare(GridPos.FromGridPosProp(end_grid)),
			MovementUtils.DeSerializePath(pathBytes),
			type,
			disappearAfterMovement,
			respawning);
	}

	private void ProcessMovement(
		GameEventManager.EventType wait,
		GridPos start,
		BoardSquare end,
		BoardSquarePathInfo path,
		ActorData.MovementType type,
		bool disappearAfterMovement,
		bool respawning)
	{
		FlushQueuedMovement();
		
		bool doesDestExist = end != null;
		bool isDestChanged = doesDestExist && m_lastMovementDestination != end;
		bool isOnInvalidSquare = Actor == null || Actor.CurrentBoardSquare == null;
		bool isDestCurrent = doesDestExist
		             && !isDestChanged
		             && !isOnInvalidSquare
		             && path != null
		             && path.GetPathEndpoint().square == Actor.CurrentBoardSquare;
		
		m_lastMovementDestination = end;
		m_lastMovementPath = path;
		m_lastMovementWaitForEvent = wait;
		m_lastMovementType = type;
		m_disappearingAfterMovement = disappearAfterMovement;
		
		bool amMoving = Actor != null
		                && Actor.GetActorMovement() != null
		                && Actor.GetActorMovement().AmMoving();
		
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
			if (!isDestChanged && (!isOnInvalidSquare || !doesDestExist) && !isDestCurrent)
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
		if (FogOfWar.GetClientFog() == null || Actor.GetActorVFX() == null)
		{
			return;
		}
		
		Actor.OnRespawnTeleport();
		Actor.ForceUpdateIsVisibleToClientCache();
		
		PlayerData localPlayerData = GameFlowData.Get().LocalPlayerData;
		if (localPlayerData != null
		    && SpawnPointManager.Get() != null
		    // reactor
		    && SpawnPointManager.Get().m_spawnInDuringMovement)
			// rogues
			//&& SpawnPointManager.Get().SpawnInDuringMovement())
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
		if (!NetworkServer.active)
		{
			return;
		}
		
		m_lastMovementDestination = null;
		m_lastMovementPath = null;
		m_lastMovementType = ActorData.MovementType.None;
		m_lastMovementWaitForEvent = GameEventManager.EventType.Invalid;
	}

	public void FlushQueuedMovement()
	{
		if (!NetworkClient.active)
		{
			return;
		}
		
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

	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
		uint setBits = uint.MaxValue;  // ulong in rogues
		if (!initialState)
		{
			setBits = reader.ReadPackedUInt32();  // ReadPackedUInt64 in rogues
		}
		sbyte actorIndex = reader.ReadSByte();
		SetActorIndex(actorIndex);
		
		if (IsBitDirty(setBits, DirtyBit.FacingDirection))
		{
			short angle = reader.ReadInt16();
			m_facingDirAfterMovement = angle < 0
				? Vector3.zero
				: VectorUtils.AngleDegreesToVector(angle);
			
			if (Actor != null)
			{
				Actor.SetFacingDirectionAfterMovement(m_facingDirAfterMovement);
			}
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
		}
		
		if (IsBitDirty(setBits, DirtyBit.InitialMoveStartSquare))
		{
			short x = reader.ReadInt16();
			short y = reader.ReadInt16();
			BoardSquare boardSquare = Board.Get().GetSquareFromIndex(x, y);
			if (InitialMoveStartSquare != boardSquare)
			{
				InitialMoveStartSquare = boardSquare;
				if (Actor != null && Actor.GetActorMovement() != null)
				{
					Actor.GetActorMovement().UpdateSquaresCanMoveTo();
				}
			}
		}
		
		if (IsBitDirty(setBits, DirtyBit.LineData))
		{
			byte bitField = reader.ReadByte();
			ServerClientUtils.GetBoolsFromBitfield(bitField, out bool movementLineFlag, out bool numNodesInSnaredFlag);
			m_movementLine = movementLineFlag
				? LineData.DeSerializeLine(reader)
				: null;
			m_numNodesInSnaredLine = numNodesInSnaredFlag
				? reader.ReadSByte()
				: (sbyte)0;
			
			if (Actor != null)
			{
				LineData component = Actor.GetComponent<LineData>();
				if (component != null)
				{
					component.OnDeserializedData(m_movementLine, m_numNodesInSnaredLine);
				}
			}
		}
		
		if (IsBitDirty(setBits, DirtyBit.MovementCameraBound))
		{
			short x = reader.ReadInt16();
			short z = reader.ReadInt16();
			short w = reader.ReadInt16();
			short h = reader.ReadInt16();
			Vector3 center = new Vector3(x, 0.5f * ActorAnimation.c_minBoundsHeight + Board.Get().BaselineHeight, z);
			Vector3 size = new Vector3(w, ActorAnimation.c_minBoundsHeight, h);
			MovementCameraBounds = new Bounds(center, size);
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
			m_respawnAvailableSquares.Clear();
			for (int i = 0; i < respawnAvailableSquaresNum; i++)
			{
				short x2 = reader.ReadInt16();
				short y2 = reader.ReadInt16();
				BoardSquare respawnAvailableSquare = Board.Get().GetSquareFromIndex(x2, y2);
				if (respawnAvailableSquare != null)
				{
					m_respawnAvailableSquares.Add(respawnAvailableSquare);
				}
				else
				{
					Log.Error("Invalid square received for respawn choices {0}, {1}", x2, y2);
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
		}

		// removed in rogues
		if (IsBitDirty(setBits, DirtyBit.QueuedAbilities) || IsBitDirty(setBits, DirtyBit.AbilityRequestDataForTargeter))
		{
			DeSerializeAbilityRequestData(reader);
		}

		if (IsBitDirty(setBits, DirtyBit.QueuedAbilities))
		{
			bool changed = false;
			short queuedAbilitiesBitmask = reader.ReadInt16();
			for (int i = 0; i < AbilityData.NUM_ACTIONS; i++)
			{
				short abilityMask = (short)(1 << i);
				bool isAbilityQueued = (queuedAbilitiesBitmask & abilityMask) != 0;
				if (m_queuedAbilities[i] != isAbilityQueued)
				{
					m_queuedAbilities[i] = isAbilityQueued;
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
			for (int i = 0; i < AbilityData.NUM_ACTIONS; i++)
			{
				short abilityMask = (short)(1 << i);
				bool isAbilityToggledOn = (toggledOnAbilitiesBitmask & abilityMask) != 0;
				if (m_abilityToggledOn[i] != isAbilityToggledOn)
				{
					m_abilityToggledOn[i] = isAbilityToggledOn;
				}
			}
		}

		// added in rogues
		//if (IsBitDirty(setBits, DirtyBit.AbilityRequestDataForTargeter))
		//{
		//	DeSerializeAbilityRequestData(reader);
		//}
	}

	public void OnClientAssociatedWithActor(ActorData actor)
	{
		if (NetworkServer.active)
		{
			return;
		}
		
		m_associatedActor = actor;
		if (m_lastMovementDestination != null)
		{
			if (m_lastMovementPath == null && m_lastMovementType != ActorData.MovementType.Teleport)
			{
				Actor.MoveToBoardSquareLocal(
					m_lastMovementDestination,
					ActorData.MovementType.Teleport,
					m_lastMovementPath,
					m_disappearingAfterMovement);
			}
			else
			{
				Actor.MoveToBoardSquareLocal(
					m_lastMovementDestination,
					m_lastMovementType,
					m_lastMovementPath,
					m_disappearingAfterMovement);
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

	public void OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		if (eventType == m_lastMovementWaitForEvent && this == Actor.TeamSensitiveData_authority)
		{
			Actor.MoveToBoardSquareLocal(
				m_lastMovementDestination,
				m_lastMovementType,
				m_lastMovementPath,
				m_disappearingAfterMovement);
			m_lastMovementPath = null;
			m_lastMovementWaitForEvent = GameEventManager.EventType.Invalid;
		}
	}

	public void OnTurnTick()
	{
		FlushQueuedMovement();
	}

	// added in rogues
#if SERVER
	public void SynchronizeMovementDataTo(ActorTeamSensitiveData other)
	{
		if (m_disappearingAfterMovement)
		{
			GridPos start = m_lastMovementDestination != null
				? m_lastMovementDestination.GetGridPos()
				: GridPos.s_invalid;
			m_lastMovementDestination = other.m_lastMovementDestination;
			FacingDirAfterMovement = other.FacingDirAfterMovement;
			m_lastMovementType = ActorData.MovementType.Teleport;
			m_lastMovementPath = null;
			m_lastMovementWaitForEvent = GameEventManager.EventType.Invalid;
			m_disappearingAfterMovement = false;
			BroadcastMovement(
				m_lastMovementWaitForEvent,
				start,
				m_lastMovementDestination,
				m_lastMovementType,
				ActorData.TeleportType.Reappear,
				m_lastMovementPath);
		}
	}
#endif

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
		
		if (actionType != AbilityData.ActionType.INVALID_ACTION
		    && !AbilityData.IsChain(actionType)
		    && m_abilityToggledOn[(int)actionType] != toggledOn)
		{
			m_abilityToggledOn[(int)actionType] = toggledOn;
			MarkAsDirty(DirtyBit.ToggledOnAbilities);
		}
	}

	// reactor
	public bool HasQueuedAction(AbilityData.ActionType actionType)
	{
		return actionType != AbilityData.ActionType.INVALID_ACTION
			&& actionType >= AbilityData.ActionType.ABILITY_0
			&& (int)actionType < m_queuedAbilities.Count
			&& m_queuedAbilities[(int)actionType];
	}
	// rogues
	//public bool HasQueuedAction(AbilityData.ActionType actionType, bool checkExecutedForPve)
	//{
	//	bool flag = false;
	//	if (actionType != AbilityData.ActionType.INVALID_ACTION)
	//	{
	//		flag = actionType >= AbilityData.ActionType.ABILITY_0
	//			&& actionType < (AbilityData.ActionType)m_queuedAbilities.Count
	//			&& m_queuedAbilities[(int)actionType];
	//		if (!flag && checkExecutedForPve && this.Actor != null && this.Actor.GetActorTurnSM() != null)
	//		{
	//			flag = this.Actor.GetActorTurnSM().PveIsAbilityAtIndexUsed((int)actionType);
	//		}
	//	}
	//	return flag;
	//}

	public bool HasQueuedAction(int actionTypeInt)  // , bool checkExecutedForPve in rogues
	{
		return HasQueuedAction((AbilityData.ActionType)actionTypeInt);  // , checkExecutedForPve in rogues
	}

	public bool HasQueuedAbilityInPhase(AbilityPriority phase)
	{
		for (int i = 0; i < m_queuedAbilities.Count; i++)
		{
			if (m_queuedAbilities[i])
			{
				Ability ability = Actor.GetAbilityData().GetAbilityOfActionType((AbilityData.ActionType)i);
				if (ability != null && ability.RunPriority == phase)
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
		
		if (actionType != AbilityData.ActionType.INVALID_ACTION
		    && !AbilityData.IsChain(actionType)
		    && HasQueuedAction(actionType) != queued)  // HasQueuedAction(actionType, false) != queued in rogues
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
			string text;
			UIWorldPing uIWorldPing;
			string eventName;
			switch (pingType)
			{
				case ActorController.PingType.Assist:
				{
					uIWorldPing = Instantiate(HUD_UI.Get().m_mainScreenPanel.m_minimap.m_worldPingAssistPrefab);
					eventName = "ui/ingame/ping/assist";
					BoardSquare closestSquare = Board.Get().GetSquareClosestToPos(vector.x, vector.z);
					if (closestSquare.OccupantActor != null && closestSquare.OccupantActor.IsActorVisibleToClient())
					{
						if (closestSquare.OccupantActor.GetTeam() != actor.GetTeam())
						{
							text = string.Format(
								StringUtil.TR("AssistEnemy", "Ping"),
								actor.GetDisplayName(),
								$"<size=36><sprite=\"CharacterSprites\" index={2 * (int)closestSquare.OccupantActor.m_characterType + 1}>\u200b</size>",
								closestSquare.OccupantActor.GetDisplayName());
						}
						else if (closestSquare.OccupantActor != actor)
						{
							text = string.Format(
								StringUtil.TR("AssistAlly", "Ping"),
								actor.GetDisplayName(),
								$"<size=36><sprite=\"CharacterSprites\" index={2 * (int)closestSquare.OccupantActor.m_characterType}>\u200b</size>",
								closestSquare.OccupantActor.GetDisplayName());
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

					break;
				}
				case ActorController.PingType.Defend:
				{
					uIWorldPing = Instantiate(HUD_UI.Get().m_mainScreenPanel.m_minimap.m_worldPingDefendPrefab);
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

					break;
				}
				case ActorController.PingType.Enemy:
				{
					uIWorldPing = Instantiate(HUD_UI.Get().m_mainScreenPanel.m_minimap.m_worldPingEnemyPrefab);
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

					break;
				}
				case ActorController.PingType.Move:
				{
					uIWorldPing = Instantiate(HUD_UI.Get().m_mainScreenPanel.m_minimap.m_worldPingMovePrefab);
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

					break;
				}
				default:
					uIWorldPing = Instantiate(HUD_UI.Get().m_mainScreenPanel.m_minimap.m_worldPingPrefab);
					eventName = "ui/ingame/ping/generic";
					text = "";
					break;
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
					// removed in rogues
					HUD_UI.Get().m_mainScreenPanel.m_offscreenIndicatorPanel.RemovePing(m_oldPings[num].GetComponent<UIWorldPing>());

					Destroy(m_oldPings[num]);
					m_oldPings.RemoveAt(num);
				}
				else
				{
					num++;
				}
			}
			m_oldPings.Add(uIWorldPing.gameObject);
			AudioManager.PostEvent(eventName, uIWorldPing.gameObject);

			// removed in rogues
			HUD_UI.Get().m_mainScreenPanel.m_offscreenIndicatorPanel.AddPing(uIWorldPing, pingType, actor);

			GameEventManager.ActorPingEventArgs actorPingEventArgs = new GameEventManager.ActorPingEventArgs
			{
				byActor = Actor,
				pingType = pingType
			};
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

	// removed in rogues
	[ClientRpc]
	internal void RpcReceivedAbilityPingInfo(int teamIndex, LocalizationArg_AbilityPing localizedPing, bool spam)
	{
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
		else if (!ClientGameManager.Get().FriendList.Friends.TryGetValue(Actor.GetAccountId(), out FriendInfo value)
		         || value.FriendStatus != FriendStatus.Blocked)
		{
			TextConsole.Get().Write(new TextConsole.Message
			{
				Text = localizedPing.TR(),
				MessageType = ConsoleMessageType.TeamChat
			});
		}
	}

	// added in rogues
#if SERVER
	public void SetAbilityRequestData(List<ActorTargeting.AbilityRequestData> requestData)
	{
		m_abilityRequestData = requestData;
	}
#endif

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

			// removed in rogues
			Actor.OnClientQueuedActionChanged();
		}
	}

	// reactor
	private void UNetVersion()
	{
	}
	// rogues
	//private void MirrorProcessed()
	//{
	//}

	protected static void InvokeRpcRpcMovement(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcMovement called on server.");
			return;
		}
		// reactor
		((ActorTeamSensitiveData)obj).RpcMovement(
			(GameEventManager.EventType)reader.ReadInt32(),
			GeneratedNetworkCode._ReadGridPosProp_None(reader),
			GeneratedNetworkCode._ReadGridPosProp_None(reader),
			reader.ReadBytesAndSize(),
			(ActorData.MovementType)reader.ReadInt32(),
			reader.ReadBoolean(),
			reader.ReadBoolean());
		// rogues
		// ((ActorTeamSensitiveData)obj).RpcMovement(
		// 	(GameEventManager.EventType)reader.ReadPackedInt32(),
		// 	GeneratedNetworkCode._ReadGridPosProp_None(reader),
		// 	GeneratedNetworkCode._ReadGridPosProp_None(reader),
		// 	reader.ReadBytesAndSize(),
		// 	(ActorData.MovementType)reader.ReadPackedInt32(),
		// 	reader.ReadBoolean(),
		// 	reader.ReadBoolean());
	}

	protected static void InvokeRpcRpcReceivedPingInfo(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcReceivedPingInfo called on server.");
			return;
		}
		// reactor
		((ActorTeamSensitiveData)obj).RpcReceivedPingInfo(
			(int)reader.ReadPackedUInt32(),
			reader.ReadVector3(),
			(ActorController.PingType)reader.ReadInt32(),
			reader.ReadBoolean());
		// rogues
		// ((ActorTeamSensitiveData)obj).RpcReceivedPingInfo(
		// 	reader.ReadPackedInt32(),
		// 	reader.ReadVector3(),
		// 	(ActorController.PingType)reader.ReadPackedInt32(),
		// 	reader.ReadBoolean());
	}

	// removed in rogues
	protected static void InvokeRpcRpcReceivedAbilityPingInfo(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcReceivedAbilityPingInfo called on server.");
			return;
		}
		((ActorTeamSensitiveData)obj).RpcReceivedAbilityPingInfo(
			(int)reader.ReadPackedUInt32(),
			GeneratedNetworkCode._ReadLocalizationArg_AbilityPing_None(reader),
			reader.ReadBoolean());
	}

	public void CallRpcMovement(
		GameEventManager.EventType wait,
		GridPosProp start,
		GridPosProp end_grid,
		byte[] pathBytes,
		ActorData.MovementType type,
		bool disappearAfterMovement,
		bool respawning)
	{
		// reactor
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
		// rogues
		//NetworkWriter networkWriter = new NetworkWriter();
		//networkWriter.WritePackedInt32((int)wait);
		//GeneratedNetworkCode._WriteGridPosProp_None(networkWriter, start);
		//GeneratedNetworkCode._WriteGridPosProp_None(networkWriter, end_grid);
		//networkWriter.WriteBytesAndSize(pathBytes);
		//networkWriter.WritePackedInt32((int)type);
		//networkWriter.Write(disappearAfterMovement);
		//networkWriter.Write(respawning);
		//this.SendRPCInternal(typeof(ActorTeamSensitiveData), "RpcMovement", networkWriter, 0);
	}

	public void CallRpcReceivedPingInfo(int teamIndex, Vector3 worldPosition, ActorController.PingType pingType, bool spam)
	{
		// reactor
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
		// rogues
		//NetworkWriter networkWriter = new NetworkWriter();
		//networkWriter.WritePackedInt32(teamIndex);
		//networkWriter.Write(worldPosition);
		//networkWriter.WritePackedInt32((int)pingType);
		//networkWriter.Write(spam);
		//this.SendRPCInternal(typeof(ActorTeamSensitiveData), "RpcReceivedPingInfo", networkWriter, 0);
	}

	// removed in rogues
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

	// was empty in reactor
	public override bool OnSerialize(NetworkWriter writer, bool initialState)
	{
#if SERVER
		if (!initialState)
		{
			writer.WritePackedUInt32(syncVarDirtyBits);  // WritePackedUInt64 in rogues
		}
		
		uint dirtyBits = initialState ? uint.MaxValue : syncVarDirtyBits;  // ulong in rogues
		
		writer.Write((sbyte)m_actorIndex);
		
		short facingDirAngle = m_facingDirAfterMovement.magnitude > 0f
			? (short)Mathf.RoundToInt(VectorUtils.HorizontalAngle_Deg(m_facingDirAfterMovement))
			: (short)-1;
		if (IsBitDirty(dirtyBits, DirtyBit.FacingDirection))
		{
			writer.Write(facingDirAngle);
		}
		
		if (IsBitDirty(dirtyBits, DirtyBit.MoveFromBoardSquare))
		{
			short x;
			short y;
			if (MoveFromBoardSquare == null)
			{
				x = -1;
				y = -1;
			}
			else
			{
				x = (short)MoveFromBoardSquare.x;
				y = (short)MoveFromBoardSquare.y;
			}
			writer.Write(x);
			writer.Write(y);
		}
		
		if (IsBitDirty(dirtyBits, DirtyBit.InitialMoveStartSquare))
		{
			short x;
			short y;
			if (InitialMoveStartSquare == null)
			{
				x = -1;
				y = -1;
			}
			else
			{
				x = (short)InitialMoveStartSquare.x;
				y = (short)InitialMoveStartSquare.y;
			}
			writer.Write(x);
			writer.Write(y);
		}
		
		if (IsBitDirty(dirtyBits, DirtyBit.LineData))
		{
			LineData lineData = Actor.GetComponent<LineData>();
			bool hasFullPath = lineData.MovementLine != null;
			bool hasSnaredPath = lineData.MovementSnaredLine != null;
			byte bitMask = ServerClientUtils.CreateBitfieldFromBools(hasFullPath, hasSnaredPath, false, false, false, false, false, false);
			writer.Write(bitMask);
			if (hasFullPath)
			{
				LineData.SerializeLine(lineData.MovementLine, writer);
			}
			if (hasSnaredPath)
			{
				writer.Write((sbyte)lineData.MovementSnaredLine.m_positions.Count);
			}
		}
		
		if (IsBitDirty(dirtyBits, DirtyBit.MovementCameraBound))
		{
			Vector3 center = m_movementCameraBounds.center;
			Vector3 size = m_movementCameraBounds.size;
			writer.Write((short)Mathf.RoundToInt(center.x));
			writer.Write((short)Mathf.RoundToInt(center.z));
			writer.Write((short)Mathf.CeilToInt(size.x > 0f ? size.x + 0.5f : 0f));
			writer.Write((short)Mathf.CeilToInt(size.z > 0f ? size.z + 0.5f : 0f));
		}
		if (IsBitDirty(dirtyBits, DirtyBit.Respawn))
		{
			short x;
			short y;
			if (m_respawnPickedSquare == null)
			{
				x = -1;
				y = -1;
			}
			else
			{
				x = (short)m_respawnPickedSquare.x;
				y = (short)m_respawnPickedSquare.y;
			}
			writer.Write(x);
			writer.Write(y);

			writer.Write(Actor.IsActorInvisibleForRespawn());
			
			short numRespawnSquares = m_respawnAvailableSquares != null
				? (short)m_respawnAvailableSquares.Count
				: (short)0;
			writer.Write(numRespawnSquares);
			for (int i = 0; i < numRespawnSquares; i++)
			{
				writer.Write((short)m_respawnAvailableSquares[i].x);
				writer.Write((short)m_respawnAvailableSquares[i].y);
			}
		}

		// custom
		if (IsBitDirty(dirtyBits, DirtyBit.QueuedAbilities) || IsBitDirty(dirtyBits, DirtyBit.AbilityRequestDataForTargeter))
		{
			SerializeAbilityRequestData(writer);
		}

		if (IsBitDirty(dirtyBits, DirtyBit.QueuedAbilities))
		{
			writer.Write(ServerClientUtils.CreateBitfieldFromBoolsList_16bit(m_queuedAbilities));
		}
		
		if (IsBitDirty(dirtyBits, DirtyBit.ToggledOnAbilities))
		{
			writer.Write(ServerClientUtils.CreateBitfieldFromBoolsList_16bit(m_abilityToggledOn));
		}

		// rogues
		//if (IsBitDirty(num, ActorTeamSensitiveData.DirtyBit.AbilityRequestDataForTargeter))
		//{
		//	SerializeAbilityRequestData(writer);
		//}

		return dirtyBits > 0UL;
#else
		return false;
#endif
	}

#if SERVER
	// custom
	public override bool OnRebuildObservers(HashSet<NetworkConnection> observers, bool initialize)
	{
		foreach (NetworkConnection connection in NetworkServer.connections)
		{
			if (connection != null && connection.isReady && OnCheckObserver(connection))
			{
				observers.Add(connection);
			}
		}
		foreach (NetworkConnection localConnection in NetworkServer.localConnections)
		{
			if (localConnection != null && localConnection.isReady && OnCheckObserver(localConnection))
			{
				observers.Add(localConnection);
			}
		}
		return true;
	}

	// custom
	public override bool OnCheckObserver(NetworkConnection conn)
	{
		PlayerDetails details = GetPlayerDetailsOfObserver(conn);
		if (details == null || Actor == null)
		{
			Log.Error($"OnCheckObserver {m_typeObservingMe} {Actor?.m_displayName} by {details?.m_handle} {details?.m_accountId}");
			return false;
		}
		
		bool isForFriendlies = m_typeObservingMe == ObservedBy.Friendlies;

		if (details.m_team == Team.Spectator)
		{
			return isForFriendlies;
		}

		HashSet<Team> observingTeams = new HashSet<Team>(details.AllServerPlayerInfos.Select(spi => spi.TeamId));
		if (observingTeams.IsNullOrEmpty())
		{
			return false;
		}
		if (observingTeams.Contains(Team.TeamA) && observingTeams.Contains(Team.TeamB))
		{
			return isForFriendlies;
		}

		Team observingTeam = observingTeams.First();

		Team replayRecorderTeam = ServerGameManager.GetReplayRecorderTeam(details.m_accountId);
		if (replayRecorderTeam != Team.Invalid)
		{
			observingTeam = replayRecorderTeam;
		}
		
		bool isAlly = observingTeam == ActorsTeam || observingTeam == Team.Spectator;
		return isAlly == isForFriendlies;
	}
#endif
}

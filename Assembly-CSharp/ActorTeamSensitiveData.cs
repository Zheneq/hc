using System;
using System.Collections.Generic;
using Unity;
using UnityEngine;
using UnityEngine.Networking;

public class ActorTeamSensitiveData : NetworkBehaviour, IGameEventListener
{
	public ActorTeamSensitiveData.ObservedBy m_typeObservingMe;

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

	private static int kRpcRpcMovement = 0x61B12293;

	private static int kRpcRpcReceivedPingInfo;

	private static int kRpcRpcReceivedAbilityPingInfo;

	static ActorTeamSensitiveData()
	{
		NetworkBehaviour.RegisterRpcDelegate(typeof(ActorTeamSensitiveData), ActorTeamSensitiveData.kRpcRpcMovement, new NetworkBehaviour.CmdDelegate(ActorTeamSensitiveData.InvokeRpcRpcMovement));
		ActorTeamSensitiveData.kRpcRpcReceivedPingInfo = 0x50692C25;
		NetworkBehaviour.RegisterRpcDelegate(typeof(ActorTeamSensitiveData), ActorTeamSensitiveData.kRpcRpcReceivedPingInfo, new NetworkBehaviour.CmdDelegate(ActorTeamSensitiveData.InvokeRpcRpcReceivedPingInfo));
		ActorTeamSensitiveData.kRpcRpcReceivedAbilityPingInfo = 0x12C6AA05;
		NetworkBehaviour.RegisterRpcDelegate(typeof(ActorTeamSensitiveData), ActorTeamSensitiveData.kRpcRpcReceivedAbilityPingInfo, new NetworkBehaviour.CmdDelegate(ActorTeamSensitiveData.InvokeRpcRpcReceivedAbilityPingInfo));
		NetworkCRC.RegisterBehaviour("ActorTeamSensitiveData", 0);
	}

	public Team ActorsTeam
	{
		get
		{
			if (this.Actor != null)
			{
				return this.Actor.GetTeam();
			}
			return Team.Invalid;
		}
	}

	public ActorData Actor
	{
		get
		{
			if (this.m_associatedActor == null && this.m_actorIndex != ActorData.s_invalidActorIndex)
			{
				if (GameFlowData.Get() != null)
				{
					ActorData actorData = GameFlowData.Get().FindActorByActorIndex(this.m_actorIndex);
					if (actorData != null)
					{
						this.m_associatedActor = actorData;
					}
				}
			}
			return this.m_associatedActor;
		}
	}

	public int ActorIndex
	{
		get
		{
			return this.m_actorIndex;
		}
	}

	private void SetActorIndex(int actorIndex)
	{
		if (this.m_actorIndex != actorIndex || this.m_associatedActor == null)
		{
			this.m_actorIndex = actorIndex;
			ActorData associatedActor;
			if (GameFlowData.Get() != null)
			{
				associatedActor = GameFlowData.Get().FindActorByActorIndex(this.m_actorIndex);
			}
			else
			{
				associatedActor = null;
			}
			this.m_associatedActor = associatedActor;
			if (!NetworkServer.active)
			{
				if (this.m_associatedActor != null)
				{
					if (this.m_typeObservingMe == ActorTeamSensitiveData.ObservedBy.Friendlies)
					{
						this.m_associatedActor.SetClientFriendlyTeamSensitiveData(this);
					}
					else if (this.m_typeObservingMe == ActorTeamSensitiveData.ObservedBy.Hostiles)
					{
						this.m_associatedActor.SetClientHostileTeamSensitiveData(this);
					}
				}
				else if (this.m_actorIndex != ActorData.s_invalidActorIndex)
				{
					TeamSensitiveDataMatchmaker.Get().OnTeamSensitiveDataStarted(this);
				}
			}
		}
	}

	public string GetDebugString()
	{
		string text;
		if (this.Actor == null)
		{
			text = "[null] (actor index = " + this.m_actorIndex.ToString() + ")";
		}
		else
		{
			text = this.Actor.GetDebugName();
		}
		return string.Concat(new string[]
		{
			"ActorTeamSensitiveData-- team = ",
			this.ActorsTeam.ToString(),
			", actor = ",
			text,
			", observed by = ",
			this.m_typeObservingMe.ToString()
		});
	}

	private void Awake()
	{
		for (int i = 0; i < 0xE; i++)
		{
			this.m_queuedAbilities.Add(false);
			this.m_abilityToggledOn.Add(false);
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
		if (NetworkClient.active)
		{
			if (!NetworkServer.active && this.Actor == null)
			{
				if (this.m_actorIndex != ActorData.s_invalidActorIndex)
				{
					ActorData associatedActor;
					if (GameFlowData.Get() != null)
					{
						associatedActor = GameFlowData.Get().FindActorByActorIndex(this.m_actorIndex);
					}
					else
					{
						associatedActor = null;
					}
					this.m_associatedActor = associatedActor;
					if (this.Actor != null)
					{
						if (this.m_typeObservingMe == ActorTeamSensitiveData.ObservedBy.Friendlies)
						{
							this.Actor.SetClientFriendlyTeamSensitiveData(this);
						}
						else if (this.m_typeObservingMe == ActorTeamSensitiveData.ObservedBy.Hostiles)
						{
							this.Actor.SetClientHostileTeamSensitiveData(this);
						}
					}
				}
			}
		}
	}

	public void MarkAsDirty(ActorTeamSensitiveData.DirtyBit bit)
	{
	}

	private bool IsBitDirty(uint setBits, ActorTeamSensitiveData.DirtyBit bitToTest)
	{
		return (setBits & (uint)bitToTest) != 0U;
	}

	public void MarkAsRespawning()
	{
	}

	[ClientRpc]
	private void RpcMovement(GameEventManager.EventType wait, GridPosProp start, GridPosProp end_grid, byte[] pathBytes, ActorData.MovementType type, bool disappearAfterMovement, bool respawning)
	{
		if (!NetworkServer.active)
		{
			this.ProcessMovement(wait, GridPos.FromGridPosProp(start), Board.Get().GetBoardSquareSafe(GridPos.FromGridPosProp(end_grid)), MovementUtils.DeSerializePath(pathBytes), type, disappearAfterMovement, respawning);
		}
	}

	private void ProcessMovement(GameEventManager.EventType wait, GridPos start, BoardSquare end, BoardSquarePathInfo path, ActorData.MovementType type, bool disappearAfterMovement, bool respawning)
	{
		this.FlushQueuedMovement();
		bool flag = end != null;
		bool flag2;
		if (flag)
		{
			flag2 = (this.m_lastMovementDestination != end);
		}
		else
		{
			flag2 = false;
		}
		bool flag3 = flag2;
		bool flag4;
		if (!(this.Actor == null))
		{
			flag4 = (this.Actor.CurrentBoardSquare == null);
		}
		else
		{
			flag4 = true;
		}
		bool flag5 = flag4;
		bool flag6;
		if (flag)
		{
			if (!flag3)
			{
				if (!flag5)
				{
					if (path != null)
					{
						flag6 = (path.GetPathEndpoint().square == this.Actor.CurrentBoardSquare);
						goto IL_B7;
					}
				}
			}
		}
		flag6 = false;
		IL_B7:
		bool flag7 = flag6;
		this.m_lastMovementDestination = end;
		this.m_lastMovementPath = path;
		this.m_lastMovementWaitForEvent = wait;
		this.m_lastMovementType = type;
		this.m_disappearingAfterMovement = disappearAfterMovement;
		bool flag8 = false;
		if (this.Actor != null)
		{
			if (this.Actor.GetActorMovement() != null)
			{
				flag8 = this.Actor.GetActorMovement().AmMoving();
			}
		}
		int num = 0;
		if (GameFlowData.Get() != null)
		{
			num = GameFlowData.Get().CurrentTurn;
		}
		if (!flag8)
		{
			if (wait == GameEventManager.EventType.Invalid)
			{
				if (this.Actor != null)
				{
					if (this.Actor.LastDeathTurn != num)
					{
						if (this.Actor.IsDead())
						{
							if (!respawning)
							{
								goto IL_2D6;
							}
						}
						if (!flag3)
						{
							if (flag5)
							{
								if (flag)
								{
									goto IL_1FF;
								}
							}
							if (!flag7)
							{
								if (flag)
								{
									goto IL_2A0;
								}
								if (disappearAfterMovement)
								{
									this.Actor.OnMovementWhileDisappeared(type);
									goto IL_2A0;
								}
								goto IL_2A0;
							}
						}
						IL_1FF:
						if (path == null)
						{
							if (type != ActorData.MovementType.Teleport)
							{
								this.Actor.MoveToBoardSquareLocal(end, ActorData.MovementType.Teleport, path, disappearAfterMovement);
								goto IL_243;
							}
						}
						this.Actor.MoveToBoardSquareLocal(end, type, path, disappearAfterMovement);
						IL_243:
						if (respawning)
						{
							if (end != null)
							{
								this.HandleRespawnCharacterVisibility(this.Actor);
							}
						}
						IL_2A0:
						if (!this.m_assignedInitialBoardSquare)
						{
							this.Actor.gameObject.SendMessage("OnAssignedToInitialBoardSquare", SendMessageOptions.DontRequireReceiver);
							this.m_assignedInitialBoardSquare = true;
						}
						return;
					}
				}
			}
		}
		IL_2D6:
		if (!flag8)
		{
			if (wait != GameEventManager.EventType.Invalid)
			{
				if (this.Actor != null)
				{
					if (this.Actor.LastDeathTurn != num)
					{
						if (this.Actor.IsDead())
						{
							if (!respawning)
							{
								goto IL_3B5;
							}
						}
						BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(start);
						if (boardSquareSafe != null)
						{
							if (boardSquareSafe != this.Actor.CurrentBoardSquare)
							{
								this.Actor.AppearAtBoardSquare(boardSquareSafe);
							}
						}
						return;
					}
				}
			}
		}
		IL_3B5:
		if (this.Actor != null)
		{
			if (respawning)
			{
				this.HandleRespawnCharacterVisibility(this.Actor);
			}
		}
	}

	private void HandleRespawnCharacterVisibility(ActorData actor)
	{
		if (FogOfWar.GetClientFog() != null)
		{
			if (this.Actor.GetActorVFX() != null)
			{
				this.Actor.OnRespawnTeleport();
				this.Actor.ForceUpdateIsVisibleToClientCache();
				PlayerData localPlayerData = GameFlowData.Get().LocalPlayerData;
				if (localPlayerData != null && SpawnPointManager.Get() != null)
				{
					if (SpawnPointManager.Get().m_spawnInDuringMovement)
					{
						ActorModelData actorModelData = this.Actor.GetActorModelData();
						if (actorModelData != null)
						{
							actorModelData.DisableAndHideRenderers();
						}
						if (HighlightUtils.Get().m_recentlySpawnedShader != null)
						{
							TricksterAfterImageNetworkBehaviour.InitializeAfterImageMaterial(this.Actor.GetActorModelData(), localPlayerData.GetTeamViewing() == this.Actor.GetTeam(), 0.5f, HighlightUtils.Get().m_recentlySpawnedShader, false);
						}
					}
				}
			}
		}
	}

	public unsafe void EncapsulateVisiblePathBound(ref Bounds bound)
	{
		if (this.m_lastMovementWaitForEvent != GameEventManager.EventType.Invalid && this.m_lastMovementPath != null && this.Actor != null)
		{
			TheatricsManager.EncapsulatePathInBound(ref bound, this.m_lastMovementPath, this.Actor);
		}
	}

	public void ClearPreviousMovementInfo()
	{
		if (NetworkServer.active)
		{
			this.m_lastMovementDestination = null;
			this.m_lastMovementPath = null;
			this.m_lastMovementType = ActorData.MovementType.None;
			this.m_lastMovementWaitForEvent = GameEventManager.EventType.Invalid;
		}
	}

	public void FlushQueuedMovement()
	{
		if (NetworkClient.active)
		{
			if (this.Actor != null)
			{
				bool flag = !this.Actor.IsDead();
				bool flag2 = this.m_lastMovementDestination != null;
				bool flag3 = this.Actor.CurrentBoardSquare == this.m_lastMovementDestination;
				bool flag4;
				if (this.Actor.CurrentBoardSquare == null)
				{
					flag4 = this.Actor.DisappearingAfterCurrentMovement;
				}
				else
				{
					flag4 = false;
				}
				bool flag5 = flag4;
				if (flag && flag2)
				{
					if (!flag3)
					{
						if (!flag5)
						{
							this.Actor.MoveToBoardSquareLocal(this.m_lastMovementDestination, ActorData.MovementType.Teleport, null, this.m_disappearingAfterMovement);
						}
					}
				}
			}
			this.m_lastMovementPath = null;
			this.m_lastMovementWaitForEvent = GameEventManager.EventType.Invalid;
		}
	}

	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
		uint setBits = uint.MaxValue;
		if (!initialState)
		{
			setBits = reader.ReadPackedUInt32();
		}
		sbyte b = reader.ReadSByte();
		this.SetActorIndex((int)b);
		if (this.IsBitDirty(setBits, ActorTeamSensitiveData.DirtyBit.FacingDirection))
		{
			short num = reader.ReadInt16();
			if (num < 0)
			{
				this.m_facingDirAfterMovement = Vector3.zero;
			}
			else
			{
				this.m_facingDirAfterMovement = VectorUtils.AngleDegreesToVector((float)num);
			}
			if (this.Actor != null)
			{
				this.Actor.SetFacingDirectionAfterMovement(this.m_facingDirAfterMovement);
			}
		}
		if (this.IsBitDirty(setBits, ActorTeamSensitiveData.DirtyBit.MoveFromBoardSquare))
		{
			short x = reader.ReadInt16();
			short y = reader.ReadInt16();
			BoardSquare boardSquare = Board.Get().GetBoardSquare((int)x, (int)y);
			if (boardSquare != this.MoveFromBoardSquare)
			{
				this.MoveFromBoardSquare = boardSquare;
				if (this.Actor != null)
				{
					if (this.Actor.GetActorMovement() != null)
					{
						this.Actor.GetActorMovement().UpdateSquaresCanMoveTo();
					}
				}
			}
		}
		if (this.IsBitDirty(setBits, ActorTeamSensitiveData.DirtyBit.InitialMoveStartSquare))
		{
			short x2 = reader.ReadInt16();
			short y2 = reader.ReadInt16();
			BoardSquare boardSquare2 = Board.Get().GetBoardSquare((int)x2, (int)y2);
			if (this.InitialMoveStartSquare != boardSquare2)
			{
				this.InitialMoveStartSquare = boardSquare2;
				if (this.Actor != null)
				{
					if (this.Actor.GetActorMovement() != null)
					{
						this.Actor.GetActorMovement().UpdateSquaresCanMoveTo();
					}
				}
			}
		}
		if (this.IsBitDirty(setBits, ActorTeamSensitiveData.DirtyBit.LineData))
		{
			byte bitField = reader.ReadByte();
			bool flag;
			bool flag2;
			ServerClientUtils.GetBoolsFromBitfield(bitField, out flag, out flag2);
			if (flag)
			{
				this.m_movementLine = LineData.DeSerializeLine(reader);
			}
			else
			{
				this.m_movementLine = null;
			}
			if (flag2)
			{
				this.m_numNodesInSnaredLine = reader.ReadSByte();
			}
			else
			{
				this.m_numNodesInSnaredLine = 0;
			}
			if (this.Actor != null)
			{
				LineData component = this.Actor.GetComponent<LineData>();
				if (component != null)
				{
					component.OnDeserializedData(this.m_movementLine, this.m_numNodesInSnaredLine);
				}
			}
		}
		if (this.IsBitDirty(setBits, ActorTeamSensitiveData.DirtyBit.MovementCameraBound))
		{
			short num2 = reader.ReadInt16();
			short num3 = reader.ReadInt16();
			short num4 = reader.ReadInt16();
			short num5 = reader.ReadInt16();
			Vector3 center = new Vector3((float)num2, 1.5f + (float)Board.Get().BaselineHeight, (float)num3);
			Vector3 size = new Vector3((float)num4, 3f, (float)num5);
			this.MovementCameraBounds = new Bounds(center, size);
		}
		if (this.IsBitDirty(setBits, ActorTeamSensitiveData.DirtyBit.Respawn))
		{
			short num6 = reader.ReadInt16();
			short num7 = reader.ReadInt16();
			BoardSquare boardSquare3 = Board.Get().GetBoardSquare((int)num6, (int)num7);
			this.RespawnPickedSquare = boardSquare3;
			bool flag3 = reader.ReadBoolean();
			if (this.Actor != null)
			{
				if (!(this.RespawnPickedSquare != null))
				{
					if (flag3)
					{
						goto IL_3D8;
					}
				}
				this.Actor.ShowRespawnFlare(this.RespawnPickedSquare, flag3);
			}
			IL_3D8:
			short num8 = reader.ReadInt16();
			this.m_respawnAvailableSquares.Clear();
			for (int i = 0; i < (int)num8; i++)
			{
				num6 = reader.ReadInt16();
				num7 = reader.ReadInt16();
				boardSquare3 = Board.Get().GetBoardSquare((int)num6, (int)num7);
				if (boardSquare3 != null)
				{
					this.m_respawnAvailableSquares.Add(boardSquare3);
				}
				else
				{
					Log.Error("Invalid square received for respawn choices {0}, {1}", new object[]
					{
						num6,
						num7
					});
				}
			}
			if (this.m_respawnAvailableSquares.Count > 0 && this.RespawnPickedSquare == null)
			{
				if (this.Actor != null)
				{
					if (GameFlowData.Get() != null)
					{
						if (this.Actor == GameFlowData.Get().activeOwnedActorData)
						{
							if (this.Actor.GetActorTurnSM().AmStillDeciding())
							{
								this.Actor.ShowRespawnFlare(this.m_respawnAvailableSquares[0], false);
							}
						}
					}
				}
			}
		}
		bool flag4 = this.IsBitDirty(setBits, ActorTeamSensitiveData.DirtyBit.QueuedAbilities);
		if (!flag4)
		{
			if (!this.IsBitDirty(setBits, ActorTeamSensitiveData.DirtyBit.AbilityRequestDataForTargeter))
			{
				goto IL_564;
			}
		}
		this.DeSerializeAbilityRequestData(reader);
		IL_564:
		if (flag4)
		{
			bool flag5 = false;
			short num9 = reader.ReadInt16();
			for (int j = 0; j < 0xE; j++)
			{
				short num10 = (short)(1 << j);
				bool flag6 = (num9 & num10) != 0;
				if (this.m_queuedAbilities[j] != flag6)
				{
					this.m_queuedAbilities[j] = flag6;
					flag5 = true;
				}
			}
			if (flag5)
			{
				if (this.Actor != null)
				{
					if (this.Actor.GetAbilityData() != null)
					{
						this.Actor.GetAbilityData().OnQueuedAbilitiesChanged();
					}
				}
			}
		}
		if (this.IsBitDirty(setBits, ActorTeamSensitiveData.DirtyBit.ToggledOnAbilities))
		{
			short num11 = reader.ReadInt16();
			for (int k = 0; k < 0xE; k++)
			{
				short num12 = (short)(1 << k);
				bool flag7 = (num11 & num12) != 0;
				if (this.m_abilityToggledOn[k] != flag7)
				{
					this.m_abilityToggledOn[k] = flag7;
				}
			}
		}
	}

	public void OnClientAssociatedWithActor(ActorData actor)
	{
		if (NetworkServer.active)
		{
			return;
		}
		this.m_associatedActor = actor;
		if (this.m_lastMovementDestination != null)
		{
			if (this.m_lastMovementPath == null && this.m_lastMovementType != ActorData.MovementType.Teleport)
			{
				this.Actor.MoveToBoardSquareLocal(this.m_lastMovementDestination, ActorData.MovementType.Teleport, this.m_lastMovementPath, this.m_disappearingAfterMovement);
			}
			else
			{
				this.Actor.MoveToBoardSquareLocal(this.m_lastMovementDestination, this.m_lastMovementType, this.m_lastMovementPath, this.m_disappearingAfterMovement);
			}
			if (!this.m_assignedInitialBoardSquare)
			{
				this.Actor.gameObject.SendMessage("OnAssignedToInitialBoardSquare", SendMessageOptions.DontRequireReceiver);
				this.m_assignedInitialBoardSquare = true;
			}
		}
		this.Actor.GetActorMovement().UpdateSquaresCanMoveTo();
		if (this.m_typeObservingMe == ActorTeamSensitiveData.ObservedBy.Friendlies)
		{
			LineData component = this.Actor.GetComponent<LineData>();
			if (component != null)
			{
				component.OnDeserializedData(this.m_movementLine, this.m_numNodesInSnaredLine);
			}
		}
	}

	public void OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		if (eventType == this.m_lastMovementWaitForEvent)
		{
			if (this == this.Actor.TeamSensitiveData_authority)
			{
				this.Actor.MoveToBoardSquareLocal(this.m_lastMovementDestination, this.m_lastMovementType, this.m_lastMovementPath, this.m_disappearingAfterMovement);
				this.m_lastMovementPath = null;
				this.m_lastMovementWaitForEvent = GameEventManager.EventType.Invalid;
			}
		}
	}

	public void OnTurnTick()
	{
		this.FlushQueuedMovement();
	}

	public Vector3 FacingDirAfterMovement
	{
		get
		{
			return this.m_facingDirAfterMovement;
		}
		set
		{
			if (this.m_facingDirAfterMovement != value)
			{
				this.m_facingDirAfterMovement = value;
				this.MarkAsDirty(ActorTeamSensitiveData.DirtyBit.FacingDirection);
			}
		}
	}

	public BoardSquare MoveFromBoardSquare
	{
		get
		{
			return this.m_moveFromBoardSquare;
		}
		set
		{
			if (this.m_moveFromBoardSquare != value)
			{
				this.m_moveFromBoardSquare = value;
				if (NetworkServer.active)
				{
					this.MarkAsDirty(ActorTeamSensitiveData.DirtyBit.MoveFromBoardSquare);
				}
			}
		}
	}

	public BoardSquare InitialMoveStartSquare
	{
		get
		{
			return this.m_initialMoveStartSquare;
		}
		set
		{
			if (this.m_initialMoveStartSquare != value)
			{
				this.m_initialMoveStartSquare = value;
				if (NetworkServer.active)
				{
					this.MarkAsDirty(ActorTeamSensitiveData.DirtyBit.InitialMoveStartSquare);
				}
			}
		}
	}

	public Bounds MovementCameraBounds
	{
		get
		{
			return this.m_movementCameraBounds;
		}
		set
		{
			if (this.m_movementCameraBounds != value)
			{
				this.m_movementCameraBounds = value;
			}
			if (NetworkServer.active)
			{
				this.MarkAsDirty(ActorTeamSensitiveData.DirtyBit.MovementCameraBound);
			}
			if (NetworkClient.active)
			{
				ActionBufferPhase currentActionPhase = ClientActionBuffer.Get().CurrentActionPhase;
				if (GameFlowData.Get().gameState == GameState.BothTeams_Resolve && GameManager.Get().GameConfig.GameType != GameType.Tutorial)
				{
					if (ClientActionBuffer.Get() != null && CameraManager.Get() != null)
					{
						if (!ClientGameManager.Get().IsSpectator)
						{
							ActorData actorData;
							if (GameFlowData.Get() != null)
							{
								actorData = GameFlowData.Get().activeOwnedActorData;
							}
							else
							{
								actorData = null;
							}
							ActorData actorData2 = actorData;
							bool flag = this.m_associatedActor != null && actorData2 != null && this.m_associatedActor.GetTeam() == actorData2.GetTeam();
							if (flag)
							{
								if (currentActionPhase != ActionBufferPhase.AbilitiesWait)
								{
									if (currentActionPhase == ActionBufferPhase.Movement)
									{
									}
									else
									{
										if (currentActionPhase == ActionBufferPhase.Abilities)
										{
											CameraManager.Get().SaveMovementCameraBound(this.m_movementCameraBounds);
											goto IL_18A;
										}
										goto IL_18A;
									}
								}
								CameraManager.Get().SetTarget(this.m_movementCameraBounds, false, false);
							}
							IL_18A:;
						}
						else if (GameFlowData.Get().LocalPlayerData != null && this.m_associatedActor != null)
						{
							Team teamViewing = GameFlowData.Get().LocalPlayerData.GetTeamViewing();
							if (teamViewing != this.m_associatedActor.GetTeam())
							{
								if (teamViewing != Team.Invalid)
								{
									return;
								}
							}
							if (currentActionPhase != ActionBufferPhase.AbilitiesWait)
							{
								if (currentActionPhase != ActionBufferPhase.Movement)
								{
									if (currentActionPhase == ActionBufferPhase.Abilities)
									{
										CameraManager.Get().SaveMovementCameraBoundForSpectator(this.m_movementCameraBounds);
										return;
									}
									return;
								}
							}
							CameraManager.Get().SaveMovementCameraBoundForSpectator(this.m_movementCameraBounds);
							CameraManager.Get().SetTargetForMovementIfNeeded();
						}
					}
				}
			}
		}
	}

	public BoardSquare RespawnPickedSquare
	{
		get
		{
			return this.m_respawnPickedSquare;
		}
		set
		{
			this.m_respawnPickedSquare = value;
			if (NetworkServer.active)
			{
				this.MarkAsDirty(ActorTeamSensitiveData.DirtyBit.Respawn);
			}
		}
	}

	public List<BoardSquare> RespawnAvailableSquares
	{
		get
		{
			return this.m_respawnAvailableSquares;
		}
		set
		{
			this.m_respawnAvailableSquares = value;
			if (NetworkServer.active)
			{
				this.MarkAsDirty(ActorTeamSensitiveData.DirtyBit.Respawn);
			}
		}
	}

	public bool HasToggledAction(AbilityData.ActionType actionType)
	{
		bool result = false;
		if (actionType != AbilityData.ActionType.INVALID_ACTION)
		{
			if (actionType >= AbilityData.ActionType.ABILITY_0)
			{
				if (actionType < (AbilityData.ActionType)this.m_abilityToggledOn.Count)
				{
					result = this.m_abilityToggledOn[(int)actionType];
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
			Debug.LogWarning("[Server] function 'System.Void ActorTeamSensitiveData::SetToggledAction(AbilityData/ActionType,System.Boolean)' called on client");
			return;
		}
		if (actionType != AbilityData.ActionType.INVALID_ACTION && !AbilityData.IsChain(actionType))
		{
			if (this.m_abilityToggledOn[(int)actionType] != toggledOn)
			{
				this.m_abilityToggledOn[(int)actionType] = toggledOn;
				this.MarkAsDirty(ActorTeamSensitiveData.DirtyBit.ToggledOnAbilities);
			}
		}
	}

	public bool HasQueuedAction(AbilityData.ActionType actionType)
	{
		bool result = false;
		if (actionType != AbilityData.ActionType.INVALID_ACTION)
		{
			bool flag;
			if (actionType >= AbilityData.ActionType.ABILITY_0)
			{
				if (actionType < (AbilityData.ActionType)this.m_queuedAbilities.Count)
				{
					flag = this.m_queuedAbilities[(int)actionType];
					goto IL_54;
				}
			}
			flag = false;
			IL_54:
			result = flag;
		}
		return result;
	}

	public bool HasQueuedAction(int actionTypeInt)
	{
		return this.HasQueuedAction((AbilityData.ActionType)actionTypeInt);
	}

	public bool HasQueuedAbilityInPhase(AbilityPriority phase)
	{
		for (int i = 0; i < this.m_queuedAbilities.Count; i++)
		{
			if (this.m_queuedAbilities[i])
			{
				Ability abilityOfActionType = this.Actor.GetAbilityData().GetAbilityOfActionType((AbilityData.ActionType)i);
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
		if (actionType != AbilityData.ActionType.INVALID_ACTION && !AbilityData.IsChain(actionType))
		{
			if (this.HasQueuedAction(actionType) != queued)
			{
				this.m_queuedAbilities[(int)actionType] = queued;
				this.MarkAsDirty(ActorTeamSensitiveData.DirtyBit.QueuedAbilities);
			}
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
		for (int i = 0; i < this.m_queuedAbilities.Count; i++)
		{
			if (this.m_queuedAbilities[i])
			{
				this.m_queuedAbilities[i] = false;
				this.MarkAsDirty(ActorTeamSensitiveData.DirtyBit.QueuedAbilities);
			}
		}
	}

	[ClientRpc]
	internal void RpcReceivedPingInfo(int teamIndex, Vector3 worldPosition, ActorController.PingType pingType, bool spam)
	{
		if (spam)
		{
			if (GameFlowData.Get() != null)
			{
				if (GameFlowData.Get().activeOwnedActorData == this.Actor)
				{
					TextConsole.Get().Write(new TextConsole.Message
					{
						Text = StringUtil.TR("TooManyPings", "Ping"),
						MessageType = ConsoleMessageType.SystemMessage
					}, null);
				}
			}
			return;
		}
		if (GameFlowData.Get() != null && GameFlowData.Get().activeOwnedActorData != null && this.Actor != null)
		{
			if (GameFlowData.Get().activeOwnedActorData.GetTeam() == (Team)teamIndex && HUD_UI.Get() != null)
			{
				if (HUD_UI.Get().m_mainScreenPanel != null && HUD_UI.Get().m_mainScreenPanel.m_minimap != null)
				{
					Vector3 vector = new Vector3(worldPosition.x, (float)Board.Get().BaselineHeight, worldPosition.z);
					ActorData actor = this.Actor;
					string text = string.Empty;
					UIWorldPing uiworldPing;
					string eventName;
					if (pingType == ActorController.PingType.Assist)
					{
						uiworldPing = UnityEngine.Object.Instantiate<UIWorldPing>(HUD_UI.Get().m_mainScreenPanel.m_minimap.m_worldPingAssistPrefab);
						eventName = "ui/ingame/ping/assist";
						BoardSquare boardSquareUnsafe = Board.Get().GetBoardSquareUnsafe(vector.x, vector.z);
						if (boardSquareUnsafe.OccupantActor != null && boardSquareUnsafe.OccupantActor.IsVisibleToClient())
						{
							if (boardSquareUnsafe.OccupantActor.GetTeam() != actor.GetTeam())
							{
								string arg = string.Format("<size=36><sprite=\"CharacterSprites\" index={0}>​</size>", (int)(CharacterType.BazookaGirl * boardSquareUnsafe.OccupantActor.m_characterType + 1));
								text = string.Format(StringUtil.TR("AssistEnemy", "Ping"), actor.GetFancyDisplayName(), arg, boardSquareUnsafe.OccupantActor.GetFancyDisplayName());
							}
							else if (boardSquareUnsafe.OccupantActor != actor)
							{
								string arg2 = string.Format("<size=36><sprite=\"CharacterSprites\" index={0}>​</size>", (int)(CharacterType.BazookaGirl * boardSquareUnsafe.OccupantActor.m_characterType));
								text = string.Format(StringUtil.TR("AssistAlly", "Ping"), actor.GetFancyDisplayName(), arg2, boardSquareUnsafe.OccupantActor.GetFancyDisplayName());
							}
							else
							{
								text = string.Format(StringUtil.TR("Assist", "Ping"), actor.GetFancyDisplayName());
							}
						}
						else
						{
							text = string.Format(StringUtil.TR("Assist", "Ping"), actor.GetFancyDisplayName());
						}
					}
					else if (pingType == ActorController.PingType.Defend)
					{
						uiworldPing = UnityEngine.Object.Instantiate<UIWorldPing>(HUD_UI.Get().m_mainScreenPanel.m_minimap.m_worldPingDefendPrefab);
						eventName = "ui/ingame/ping/anger";
						BoardSquare boardSquareUnsafe2 = Board.Get().GetBoardSquareUnsafe(vector.x, vector.z);
						if (boardSquareUnsafe2.OccupantActor != null)
						{
							if (boardSquareUnsafe2.OccupantActor.IsVisibleToClient())
							{
								if (boardSquareUnsafe2.OccupantActor.GetTeam() != actor.GetTeam())
								{
									string arg3 = string.Format("<size=36><sprite=\"CharacterSprites\" index={0}>​</size>", (int)(CharacterType.BazookaGirl * boardSquareUnsafe2.OccupantActor.m_characterType + 1));
									text = string.Format(StringUtil.TR("DangerEnemy", "Ping"), actor.GetFancyDisplayName(), arg3, boardSquareUnsafe2.OccupantActor.GetFancyDisplayName());
								}
								else if (boardSquareUnsafe2.OccupantActor != actor)
								{
									string arg4 = string.Format("<size=36><sprite=\"CharacterSprites\" index={0}>​</size>", (int)(CharacterType.BazookaGirl * boardSquareUnsafe2.OccupantActor.m_characterType));
									text = string.Format(StringUtil.TR("DangerAlly", "Ping"), actor.GetFancyDisplayName(), arg4, boardSquareUnsafe2.OccupantActor.GetFancyDisplayName());
								}
								else
								{
									text = string.Format(StringUtil.TR("Danger", "Ping"), actor.GetFancyDisplayName());
								}
								goto IL_4D0;
							}
						}
						text = string.Format(StringUtil.TR("Danger", "Ping"), actor.GetFancyDisplayName());
						IL_4D0:;
					}
					else if (pingType == ActorController.PingType.Enemy)
					{
						uiworldPing = UnityEngine.Object.Instantiate<UIWorldPing>(HUD_UI.Get().m_mainScreenPanel.m_minimap.m_worldPingEnemyPrefab);
						eventName = "ui/ingame/ping/attack";
						BoardSquare boardSquareUnsafe3 = Board.Get().GetBoardSquareUnsafe(vector.x, vector.z);
						if (boardSquareUnsafe3.OccupantActor != null)
						{
							if (boardSquareUnsafe3.OccupantActor.IsVisibleToClient())
							{
								if (boardSquareUnsafe3.OccupantActor.GetTeam() != actor.GetTeam())
								{
									string arg5 = string.Format("<size=36><sprite=\"CharacterSprites\" index={0}>​</size>", (int)(CharacterType.BazookaGirl * boardSquareUnsafe3.OccupantActor.m_characterType + 1));
									text = string.Format(StringUtil.TR("AttackEnemy", "Ping"), actor.GetFancyDisplayName(), arg5, boardSquareUnsafe3.OccupantActor.GetFancyDisplayName());
								}
								else
								{
									text = string.Format(StringUtil.TR("Attack", "Ping"), actor.GetFancyDisplayName());
								}
								goto IL_61B;
							}
						}
						text = string.Format(StringUtil.TR("Attack", "Ping"), actor.GetFancyDisplayName());
						IL_61B:;
					}
					else if (pingType == ActorController.PingType.Move)
					{
						uiworldPing = UnityEngine.Object.Instantiate<UIWorldPing>(HUD_UI.Get().m_mainScreenPanel.m_minimap.m_worldPingMovePrefab);
						eventName = "ui/ingame/ping/move";
						BoardSquare boardSquareUnsafe4 = Board.Get().GetBoardSquareUnsafe(vector.x, vector.z);
						if (boardSquareUnsafe4.OccupantActor != null)
						{
							if (boardSquareUnsafe4.OccupantActor.IsVisibleToClient())
							{
								if (boardSquareUnsafe4.OccupantActor.GetTeam() != actor.GetTeam())
								{
									string arg6 = string.Format("<size=36><sprite=\"CharacterSprites\" index={0}>​</size>", (int)(CharacterType.BazookaGirl * boardSquareUnsafe4.OccupantActor.m_characterType + 1));
									text = string.Format(StringUtil.TR("MoveEnemy", "Ping"), actor.GetFancyDisplayName(), arg6, boardSquareUnsafe4.OccupantActor.GetFancyDisplayName());
								}
								else if (boardSquareUnsafe4.OccupantActor != actor)
								{
									string arg7 = string.Format("<size=36><sprite=\"CharacterSprites\" index={0}>​</size>", (int)(CharacterType.BazookaGirl * boardSquareUnsafe4.OccupantActor.m_characterType));
									text = string.Format(StringUtil.TR("MoveAlly", "Ping"), actor.GetFancyDisplayName(), arg7, boardSquareUnsafe4.OccupantActor.GetFancyDisplayName());
								}
								else
								{
									text = string.Format(StringUtil.TR("Move", "Ping"), actor.GetFancyDisplayName());
								}
								goto IL_7C9;
							}
						}
						text = string.Format(StringUtil.TR("Move", "Ping"), actor.GetFancyDisplayName());
						IL_7C9:;
					}
					else
					{
						uiworldPing = UnityEngine.Object.Instantiate<UIWorldPing>(HUD_UI.Get().m_mainScreenPanel.m_minimap.m_worldPingPrefab);
						eventName = "ui/ingame/ping/generic";
						text = string.Empty;
					}
					uiworldPing.transform.position = vector;
					int i = 0;
					while (i < this.m_oldPings.Count)
					{
						if (this.m_oldPings[i] == null)
						{
							this.m_oldPings.RemoveAt(i);
						}
						else if (this.m_oldPings[i].transform.position == vector)
						{
							HUD_UI.Get().m_mainScreenPanel.m_offscreenIndicatorPanel.RemovePing(this.m_oldPings[i].GetComponent<UIWorldPing>());
							UnityEngine.Object.Destroy(this.m_oldPings[i]);
							this.m_oldPings.RemoveAt(i);
						}
						else
						{
							i++;
						}
					}
					this.m_oldPings.Add(uiworldPing.gameObject);
					AudioManager.PostEvent(eventName, uiworldPing.gameObject);
					HUD_UI.Get().m_mainScreenPanel.m_offscreenIndicatorPanel.AddPing(uiworldPing, pingType, actor);
					GameEventManager.ActorPingEventArgs actorPingEventArgs = new GameEventManager.ActorPingEventArgs();
					actorPingEventArgs.byActor = this.Actor;
					actorPingEventArgs.pingType = pingType;
					GameEventManager.Get().FireEvent(GameEventManager.EventType.ActorPing, actorPingEventArgs);
					if (text != string.Empty)
					{
						if (this.m_lastPingChatTime + 2f < Time.time)
						{
							TextConsole.Get().Write(new TextConsole.Message
							{
								Text = text,
								MessageType = ConsoleMessageType.PingChat,
								CharacterType = actor.m_characterType,
								SenderTeam = actor.GetTeam()
							}, null);
							this.m_lastPingChatTime = Time.time;
						}
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
			if (GameFlowData.Get() != null)
			{
				if (GameFlowData.Get().activeOwnedActorData == this.Actor)
				{
					TextConsole.Get().Write(new TextConsole.Message
					{
						Text = StringUtil.TR("TooManyPings", "Ping"),
						MessageType = ConsoleMessageType.SystemMessage
					}, null);
				}
			}
			return;
		}
		FriendInfo friendInfo;
		if (ClientGameManager.Get().FriendList.Friends.TryGetValue(this.Actor.GetAccountIdWithSomeConditionA_zq(), out friendInfo))
		{
			if (friendInfo.FriendStatus == FriendStatus.Blocked)
			{
				return;
			}
		}
		TextConsole.Get().Write(new TextConsole.Message
		{
			Text = localizedPing.TR(),
			MessageType = ConsoleMessageType.TeamChat
		}, null);
	}

	public List<ActorTargeting.AbilityRequestData> GetAbilityRequestData()
	{
		return this.m_abilityRequestData;
	}

	private void SerializeAbilityRequestData(NetworkWriter writer)
	{
		byte b = (byte)this.m_abilityRequestData.Count;
		writer.Write(b);
		for (int i = 0; i < (int)b; i++)
		{
			sbyte value = (sbyte)this.m_abilityRequestData[i].m_actionType;
			writer.Write(value);
			AbilityTarget.SerializeAbilityTargetList(this.m_abilityRequestData[i].m_targets, writer);
		}
	}

	private void DeSerializeAbilityRequestData(NetworkReader reader)
	{
		this.m_abilityRequestData.Clear();
		byte b = reader.ReadByte();
		for (int i = 0; i < (int)b; i++)
		{
			sbyte b2 = reader.ReadSByte();
			List<AbilityTarget> targets = AbilityTarget.DeSerializeAbilityTargetList(reader);
			AbilityData.ActionType actionType = (AbilityData.ActionType)b2;
			this.m_abilityRequestData.Add(new ActorTargeting.AbilityRequestData(actionType, targets));
		}
		if (this.Actor != null)
		{
			if (this.Actor.GetActorTargeting() != null)
			{
				this.Actor.GetActorTargeting().OnRequestDataDeserialized();
				this.Actor.OnClientQueuedActionChanged();
			}
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
		networkWriter.Write(0);
		networkWriter.Write((short)((ushort)2));
		networkWriter.WritePackedUInt32((uint)ActorTeamSensitiveData.kRpcRpcMovement);
		networkWriter.Write(base.GetComponent<NetworkIdentity>().netId);
		networkWriter.Write((int)wait);
		GeneratedNetworkCode._WriteGridPosProp_None(networkWriter, start);
		GeneratedNetworkCode._WriteGridPosProp_None(networkWriter, end_grid);
		networkWriter.WriteBytesFull(pathBytes);
		networkWriter.Write((int)type);
		networkWriter.Write(disappearAfterMovement);
		networkWriter.Write(respawning);
		this.SendRPCInternal(networkWriter, 0, "RpcMovement");
	}

	public void CallRpcReceivedPingInfo(int teamIndex, Vector3 worldPosition, ActorController.PingType pingType, bool spam)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("RPC Function RpcReceivedPingInfo called on client.");
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write(0);
		networkWriter.Write((short)((ushort)2));
		networkWriter.WritePackedUInt32((uint)ActorTeamSensitiveData.kRpcRpcReceivedPingInfo);
		networkWriter.Write(base.GetComponent<NetworkIdentity>().netId);
		networkWriter.WritePackedUInt32((uint)teamIndex);
		networkWriter.Write(worldPosition);
		networkWriter.Write((int)pingType);
		networkWriter.Write(spam);
		this.SendRPCInternal(networkWriter, 0, "RpcReceivedPingInfo");
	}

	public void CallRpcReceivedAbilityPingInfo(int teamIndex, LocalizationArg_AbilityPing localizedPing, bool spam)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("RPC Function RpcReceivedAbilityPingInfo called on client.");
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write(0);
		networkWriter.Write((short)((ushort)2));
		networkWriter.WritePackedUInt32((uint)ActorTeamSensitiveData.kRpcRpcReceivedAbilityPingInfo);
		networkWriter.Write(base.GetComponent<NetworkIdentity>().netId);
		networkWriter.WritePackedUInt32((uint)teamIndex);
		GeneratedNetworkCode._WriteLocalizationArg_AbilityPing_None(networkWriter, localizedPing);
		networkWriter.Write(spam);
		this.SendRPCInternal(networkWriter, 0, "RpcReceivedAbilityPingInfo");
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		return false;
	}

	public enum DirtyBit : uint
	{
		FacingDirection = 1U,
		MoveFromBoardSquare,
		LineData = 4U,
		MovementCameraBound = 8U,
		Respawn = 0x10U,
		InitialMoveStartSquare = 0x20U,
		QueuedAbilities = 0x40U,
		ToggledOnAbilities = 0x80U,
		AbilityRequestDataForTargeter = 0x100U,
		All = 0xFFFFFFFFU
	}

	public enum ObservedBy
	{
		Friendlies,
		Hostiles
	}
}

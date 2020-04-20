using System;
using System.Collections.Generic;
using Unity;
using UnityEngine;
using UnityEngine.Networking;

public class ActorController : NetworkBehaviour
{
	private HashSet<BoardSquare> m_currentCanMoveToSquares = new HashSet<BoardSquare>();

	private HashSet<BoardSquare> m_currentCanMoveToWithAbilitySquares = new HashSet<BoardSquare>();

	private HashSet<BoardSquare> m_currentTargetingSquares = new HashSet<BoardSquare>();

	private HashSet<BoardSquare> m_currentRespawnSquares = new HashSet<BoardSquare>();

	private GameObject m_canMoveToHighlight;

	private GameObject m_canMoveToWithAbilityHighlight;

	private GameObject m_targetingHighlight;

	private GameObject m_respawnHighlight;

	private Ability m_lastTargetedAbility;

	private int m_lastTargetIndex = -1;

	private ActorData m_actor;

	private bool m_movementLinesVisible = true;

	private HashSet<BoardSquare> m_canMoveToSquaresScratch = new HashSet<BoardSquare>();

	private HashSet<BoardSquare> m_canMoveToWithQueuedAbilityScratch = new HashSet<BoardSquare>();

	private HashSet<BoardSquare> m_targetingSquaresScratch = new HashSet<BoardSquare>();

	private static int kCmdCmdDebugTeleportRequest = -0x5E5EA0BE;

	private static int kCmdCmdPickedRespawnRequest;

	private static int kCmdCmdSendMinimapPing;

	private static int kCmdCmdSendAbilityPing;

	private static int kCmdCmdSelectAbilityRequest;

	private static int kCmdCmdQueueSimpleActionRequest;

	private static int kCmdCmdCustomGamePause;

	private static int kRpcRpcUpdateRemainingMovement;

	static ActorController()
	{
		NetworkBehaviour.RegisterCommandDelegate(typeof(ActorController), ActorController.kCmdCmdDebugTeleportRequest, new NetworkBehaviour.CmdDelegate(ActorController.InvokeCmdCmdDebugTeleportRequest));
		ActorController.kCmdCmdPickedRespawnRequest = 0x6919E618;
		NetworkBehaviour.RegisterCommandDelegate(typeof(ActorController), ActorController.kCmdCmdPickedRespawnRequest, new NetworkBehaviour.CmdDelegate(ActorController.InvokeCmdCmdPickedRespawnRequest));
		ActorController.kCmdCmdSendMinimapPing = -0x30510FC2;
		NetworkBehaviour.RegisterCommandDelegate(typeof(ActorController), ActorController.kCmdCmdSendMinimapPing, new NetworkBehaviour.CmdDelegate(ActorController.InvokeCmdCmdSendMinimapPing));
		ActorController.kCmdCmdSendAbilityPing = -0x396C32BD;
		NetworkBehaviour.RegisterCommandDelegate(typeof(ActorController), ActorController.kCmdCmdSendAbilityPing, new NetworkBehaviour.CmdDelegate(ActorController.InvokeCmdCmdSendAbilityPing));
		ActorController.kCmdCmdSelectAbilityRequest = -0x468D04AE;
		NetworkBehaviour.RegisterCommandDelegate(typeof(ActorController), ActorController.kCmdCmdSelectAbilityRequest, new NetworkBehaviour.CmdDelegate(ActorController.InvokeCmdCmdSelectAbilityRequest));
		ActorController.kCmdCmdQueueSimpleActionRequest = -0x2F8E5139;
		NetworkBehaviour.RegisterCommandDelegate(typeof(ActorController), ActorController.kCmdCmdQueueSimpleActionRequest, new NetworkBehaviour.CmdDelegate(ActorController.InvokeCmdCmdQueueSimpleActionRequest));
		ActorController.kCmdCmdCustomGamePause = 0x3AA5E8E2;
		NetworkBehaviour.RegisterCommandDelegate(typeof(ActorController), ActorController.kCmdCmdCustomGamePause, new NetworkBehaviour.CmdDelegate(ActorController.InvokeCmdCmdCustomGamePause));
		ActorController.kRpcRpcUpdateRemainingMovement = 0x3D70F95;
		NetworkBehaviour.RegisterRpcDelegate(typeof(ActorController), ActorController.kRpcRpcUpdateRemainingMovement, new NetworkBehaviour.CmdDelegate(ActorController.InvokeRpcRpcUpdateRemainingMovement));
		NetworkCRC.RegisterBehaviour("ActorController", 0);
	}

	private void Awake()
	{
		this.m_actor = base.GetComponent<ActorData>();
	}

	public Ability GetLastTargetedAbility(ref int lastTargetIndex)
	{
		lastTargetIndex = this.m_lastTargetIndex;
		return this.m_lastTargetedAbility;
	}

	private void OnRespawn()
	{
		ActorData actor = this.m_actor;
		if (Camera.main)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorController.OnRespawn()).MethodHandle;
			}
			if (actor == GameFlowData.Get().activeOwnedActorData)
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
				CameraManager.Get().SetTargetObject(base.gameObject, CameraManager.CameraTargetReason.ClientActorRespawned);
			}
		}
	}

	private void HandlePickRespawnInput()
	{
		ActorData actor = this.m_actor;
		if (actor == GameFlowData.Get().activeOwnedActorData)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorController.HandlePickRespawnInput()).MethodHandle;
			}
			if (!Input.GetMouseButtonUp(0))
			{
				if (!Input.GetMouseButtonUp(1))
				{
					return;
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
			}
			if (InterfaceManager.Get().ShouldHandleMouseClick())
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
				BoardSquare playerClampedSquare = Board.Get().PlayerClampedSquare;
				bool flag = actor.respawnSquares.Contains(playerClampedSquare);
				if (playerClampedSquare != null)
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
					if (flag)
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
						this.CallCmdPickedRespawnRequest(playerClampedSquare.x, playerClampedSquare.y);
						actor.ShowRespawnFlare(playerClampedSquare, false);
					}
				}
			}
		}
	}

	private void HandleDebugTeleport()
	{
		ActorData actor = this.m_actor;
		if (actor == GameFlowData.Get().activeOwnedActorData)
		{
			BoardSquare playerFreeSquare = Board.Get().PlayerFreeSquare;
			if (playerFreeSquare != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ActorController.HandleDebugTeleport()).MethodHandle;
				}
				if (playerFreeSquare.IsBaselineHeight())
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
					bool flag = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
					bool flag2 = Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt);
					if (flag)
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
							if (Input.GetMouseButtonUp(2))
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
								if (InterfaceManager.Get().ShouldHandleMouseClick())
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
									this.CallCmdDebugTeleportRequest(playerFreeSquare.x, playerFreeSquare.y);
								}
							}
						}
					}
				}
			}
		}
	}

	[Command]
	private void CmdDebugTeleportRequest(int x, int y)
	{
	}

	[Command]
	private void CmdPickedRespawnRequest(int x, int y)
	{
	}

	[Command]
	internal void CmdSendMinimapPing(int teamIndex, Vector3 worldPosition, ActorController.PingType pingType)
	{
	}

	[Command]
	internal void CmdSendAbilityPing(int teamIndex, LocalizationArg_AbilityPing localizedPing)
	{
	}

	public void ClearHighlights()
	{
		this.ClearMovementHighlights();
		this.ClearTargetingHighlights();
		this.ClearRespawnHighlights();
	}

	private void ClearMovementHighlights()
	{
		this.m_currentCanMoveToSquares.Clear();
		this.m_currentCanMoveToWithAbilitySquares.Clear();
		if (this.m_canMoveToHighlight)
		{
			HighlightUtils.DestroyBoundaryHighlightObject(this.m_canMoveToHighlight);
		}
		if (this.m_canMoveToWithAbilityHighlight)
		{
			HighlightUtils.DestroyBoundaryHighlightObject(this.m_canMoveToWithAbilityHighlight);
		}
	}

	private void ClearTargetingHighlights()
	{
		this.m_currentTargetingSquares.Clear();
		if (this.m_targetingHighlight)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorController.ClearTargetingHighlights()).MethodHandle;
			}
			HighlightUtils.DestroyBoundaryHighlightObject(this.m_targetingHighlight);
			this.m_targetingHighlight = null;
		}
		this.m_lastTargetedAbility = null;
		this.m_lastTargetIndex = -1;
	}

	private void ClearRespawnHighlights()
	{
		this.m_currentRespawnSquares.Clear();
		if (this.m_respawnHighlight)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorController.ClearRespawnHighlights()).MethodHandle;
			}
			HighlightUtils.DestroyBoundaryHighlightObject(this.m_respawnHighlight);
			this.m_respawnHighlight = null;
		}
	}

	private void Update()
	{
		ActorData actor = this.m_actor;
		if (actor == GameFlowData.Get().activeOwnedActorData)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorController.Update()).MethodHandle;
			}
			ActorTurnSM actorTurnSM = actor.GetActorTurnSM();
			if (actorTurnSM.CanPickRespawnLocation())
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
				this.HandlePickRespawnInput();
			}
			else if (actorTurnSM.CurrentState == TurnStateEnum.DECIDING)
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
				this.HandleDebugTeleport();
			}
		}
	}

	public void SetMovementDistanceLinesVisible(bool visible)
	{
		this.m_movementLinesVisible = visible;
		if (this.m_canMoveToHighlight != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorController.SetMovementDistanceLinesVisible(bool)).MethodHandle;
			}
			this.m_canMoveToHighlight.gameObject.SetActive(this.m_movementLinesVisible);
		}
		if (this.m_canMoveToWithAbilityHighlight != null)
		{
			this.m_canMoveToWithAbilityHighlight.gameObject.SetActive(this.m_movementLinesVisible);
		}
	}

	public HashSet<BoardSquare> GetSquaresToClampTo()
	{
		ActorTurnSM actorTurnSM = this.m_actor.GetActorTurnSM();
		if (actorTurnSM.AmDecidingMovement())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorController.GetSquaresToClampTo()).MethodHandle;
			}
			return this.m_actor.GetActorMovement().SquaresCanMoveTo;
		}
		if (actorTurnSM.AmTargetingAction())
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
			return this.m_currentTargetingSquares;
		}
		if (actorTurnSM.CurrentState == TurnStateEnum.PICKING_RESPAWN)
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
			return this.m_currentRespawnSquares;
		}
		return null;
	}

	public void RecalcAndHighlightValidSquares()
	{
		Board board = Board.Get();
		ActorData actor = this.m_actor;
		AbilityData abilityData = actor.GetAbilityData();
		ActorMovement actorMovement = actor.GetActorMovement();
		this.m_canMoveToSquaresScratch.Clear();
		this.m_canMoveToWithQueuedAbilityScratch.Clear();
		this.m_targetingSquaresScratch.Clear();
		bool flag = false;
		bool flag2 = false;
		bool flag3 = this.m_actor.GetActorTurnSM().AmDecidingMovement();
		bool flag4 = this.m_actor.GetActorTurnSM().CurrentState == TurnStateEnum.TARGETING_ACTION;
		bool markedForUpdateValidSquares = Board.Get().MarkedForUpdateValidSquares;
		bool flag5 = actor.GetPostAbilityHorizontalMovementChange() > 0f;
		bool flag6 = abilityData.GetQueuedAbilitiesMovementAdjust() < 0f;
		if (flag3)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorController.RecalcAndHighlightValidSquares()).MethodHandle;
			}
			if (markedForUpdateValidSquares)
			{
				if (flag5)
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
					if (!flag6)
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
						using (HashSet<BoardSquare>.Enumerator enumerator = actorMovement.SquaresCanMoveTo.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								BoardSquare item = enumerator.Current;
								this.m_canMoveToSquaresScratch.Add(item);
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
						}
					}
				}
				if (actorMovement.SquaresCanMoveToWithQueuedAbility.SetEquals(actorMovement.SquaresCanMoveTo))
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
					if (flag5)
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
						if (!flag6)
						{
							goto IL_1B1;
						}
					}
				}
				using (HashSet<BoardSquare>.Enumerator enumerator2 = actorMovement.SquaresCanMoveToWithQueuedAbility.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						BoardSquare item2 = enumerator2.Current;
						this.m_canMoveToWithQueuedAbilityScratch.Add(item2);
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
				IL_1B1:
				Board.Get().MarkForUpdateValidSquares(false);
			}
			else
			{
				flag = true;
			}
			this.ClearTargetingHighlights();
		}
		else if (flag4)
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
			Ability selectedAbility = abilityData.GetSelectedAbility();
			int targetSelectionIndex = this.m_actor.GetActorTurnSM().GetTargetSelectionIndex();
			if (!(selectedAbility != this.m_lastTargetedAbility))
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
				if (targetSelectionIndex == this.m_lastTargetIndex)
				{
					flag2 = true;
					goto IL_241;
				}
			}
			this.m_targetingSquaresScratch = AbilityUtils.GetTargetableSquaresForAbility(selectedAbility, abilityData, actor, targetSelectionIndex);
			this.m_lastTargetedAbility = selectedAbility;
			this.m_lastTargetIndex = targetSelectionIndex;
		}
		IL_241:
		if (!flag)
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
			if (this.m_currentCanMoveToSquares != this.m_canMoveToSquaresScratch)
			{
				if (!this.m_currentCanMoveToSquares.SetEquals(this.m_canMoveToSquaresScratch))
				{
					goto IL_2FC;
				}
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
			}
		}
		if (!flag)
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
			if (this.m_currentCanMoveToWithAbilitySquares != this.m_canMoveToWithQueuedAbilityScratch)
			{
				if (!this.m_currentCanMoveToWithAbilitySquares.SetEquals(this.m_canMoveToWithQueuedAbilityScratch))
				{
					goto IL_2FC;
				}
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
			}
		}
		if (flag2)
		{
			goto IL_58A;
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
		if (this.m_currentTargetingSquares == this.m_targetingSquaresScratch || this.m_currentTargetingSquares.SetEquals(this.m_targetingSquaresScratch))
		{
			goto IL_58A;
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
		IL_2FC:
		if (flag3)
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
			if (this.m_canMoveToWithAbilityHighlight)
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
				HighlightUtils.DestroyBoundaryHighlightObject(this.m_canMoveToWithAbilityHighlight);
			}
			this.m_canMoveToWithAbilityHighlight = HighlightUtils.Get().CreateBoundaryHighlight(this.m_canMoveToWithQueuedAbilityScratch, BoardSquare.s_moveableHighlightColor, true, null, false);
			if (this.m_canMoveToWithAbilityHighlight)
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
				this.m_canMoveToWithAbilityHighlight.AddComponent<HighlightParent>();
			}
			if (this.m_canMoveToHighlight)
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
				HighlightUtils.DestroyBoundaryHighlightObject(this.m_canMoveToHighlight);
			}
			this.m_canMoveToHighlight = HighlightUtils.Get().CreateBoundaryHighlight(this.m_canMoveToSquaresScratch, BoardSquare.s_moveableHighlightColor, false, null, false);
			if (this.m_canMoveToHighlight)
			{
				this.m_canMoveToHighlight.AddComponent<HighlightParent>();
			}
		}
		else
		{
			HashSet<BoardSquare> hashSet = new HashSet<BoardSquare>();
			if (actor.GetCurrentBoardSquare() != null)
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
				hashSet.Add(actor.GetCurrentBoardSquare());
				if (this.m_lastTargetedAbility != null)
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
					if (this.m_lastTargetedAbility.Targeters != null)
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
						using (List<AbilityUtil_Targeter>.Enumerator enumerator3 = this.m_lastTargetedAbility.Targeters.GetEnumerator())
						{
							while (enumerator3.MoveNext())
							{
								AbilityUtil_Targeter abilityUtil_Targeter = enumerator3.Current;
								if (abilityUtil_Targeter != null)
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
									BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(abilityUtil_Targeter.LastUpdatingGridPos);
									if (boardSquareSafe != null)
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
										hashSet.Add(boardSquareSafe);
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
			if (this.m_targetingHighlight)
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
				HighlightUtils.DestroyBoundaryHighlightObject(this.m_targetingHighlight);
			}
			this.m_targetingHighlight = HighlightUtils.Get().CreateBoundaryHighlight(this.m_targetingSquaresScratch, BoardSquare.s_targetableByAbilityHighlightColor, false, hashSet, false);
			if (this.m_targetingHighlight)
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
				this.m_targetingHighlight.AddComponent<HighlightParent>();
			}
		}
		if (!flag)
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
			this.CopyOverHashsetValues(this.m_currentCanMoveToSquares, this.m_canMoveToSquaresScratch);
			this.CopyOverHashsetValues(this.m_currentCanMoveToWithAbilitySquares, this.m_canMoveToWithQueuedAbilityScratch);
		}
		if (!flag2)
		{
			this.CopyOverHashsetValues(this.m_currentTargetingSquares, this.m_targetingSquaresScratch);
		}
		IL_58A:
		bool flag7;
		if (GameFlowData.Get().IsInDecisionState() && this.m_actor.IsDead() && SpawnPointManager.Get() != null)
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
			flag7 = SpawnPointManager.Get().m_playersSelectRespawn;
		}
		else
		{
			flag7 = false;
		}
		bool flag8 = flag7;
		List<BoardSquare> respawnSquares = actor.respawnSquares;
		if (flag8)
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
			if (actor.IsDead() && !respawnSquares.IsNullOrEmpty<BoardSquare>())
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
				if (this.m_currentRespawnSquares.Count == respawnSquares.Count)
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
					if (respawnSquares.TrueForAll((BoardSquare s) => this.m_currentRespawnSquares.Contains(s)))
					{
						goto IL_6B1;
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
				this.ClearRespawnHighlights();
				this.m_respawnHighlight = HighlightUtils.Get().CreateBoundaryHighlight(respawnSquares, BoardSquare.s_respawnOptionHighlightColor, true);
				if (this.m_respawnHighlight)
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
					this.m_respawnHighlight.AddComponent<HighlightParent>();
				}
				this.m_currentRespawnSquares.Clear();
				this.m_currentRespawnSquares = new HashSet<BoardSquare>(respawnSquares);
				IL_6B1:
				goto IL_6B9;
			}
		}
		this.ClearRespawnHighlights();
		IL_6B9:
		if (this.m_canMoveToWithAbilityHighlight)
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
			bool flag9 = flag3 && this.m_currentCanMoveToWithAbilitySquares.Count > 0 && this.m_movementLinesVisible;
			if (this.m_canMoveToWithAbilityHighlight.gameObject.activeSelf != flag9)
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
				this.m_canMoveToWithAbilityHighlight.gameObject.SetActive(flag9);
			}
		}
		if (this.m_canMoveToHighlight)
		{
			bool flag10;
			if (flag3 && this.m_currentCanMoveToSquares.Count > 0)
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
				flag10 = this.m_movementLinesVisible;
			}
			else
			{
				flag10 = false;
			}
			bool flag11 = flag10;
			if (flag11 && !FirstTurnMovement.ForceShowSprintRange(actor))
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
				Vector3 position = HighlightUtils.Get().MovementMouseOverCursor.transform.position;
				BoardSquare boardSquareSafe2 = board.GetBoardSquareSafe(position.x, position.z);
				if (this.m_canMoveToWithQueuedAbilityScratch.Contains(boardSquareSafe2))
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
					flag11 = false;
				}
			}
			if (this.m_canMoveToHighlight.gameObject.activeSelf != flag11)
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
				this.m_canMoveToHighlight.gameObject.SetActive(flag11);
			}
		}
		if (this.m_targetingHighlight)
		{
			bool flag12;
			if (!flag3)
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
				flag12 = (this.m_currentTargetingSquares.Count > 0);
			}
			else
			{
				flag12 = false;
			}
			bool flag13 = flag12;
			if (this.m_targetingHighlight.gameObject.activeSelf != flag13)
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
				this.m_targetingHighlight.gameObject.SetActive(flag13);
			}
		}
	}

	private void CopyOverHashsetValues(HashSet<BoardSquare> toSet, HashSet<BoardSquare> fromSet)
	{
		toSet.Clear();
		using (HashSet<BoardSquare>.Enumerator enumerator = fromSet.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				BoardSquare item = enumerator.Current;
				toSet.Add(item);
			}
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorController.CopyOverHashsetValues(HashSet<BoardSquare>, HashSet<BoardSquare>)).MethodHandle;
			}
		}
	}

	public void SendSelectAbilityRequest()
	{
		AbilityData.ActionType actionTypeInt = AbilityData.ActionType.INVALID_ACTION;
		AbilityData component = base.GetComponent<AbilityData>();
		UISounds.GetUISounds().Play("ui/ingame/v1/hud/ability_select");
		if (component)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorController.SendSelectAbilityRequest()).MethodHandle;
			}
			actionTypeInt = component.GetSelectedActionType();
		}
		this.CallCmdSelectAbilityRequest((int)actionTypeInt);
	}

	internal void ShowOvercon(int overconId, bool allyOnly)
	{
		ActorData actorData;
		if (GameFlowData.Get() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorController.ShowOvercon(int, bool)).MethodHandle;
			}
			actorData = GameFlowData.Get().activeOwnedActorData;
		}
		else
		{
			actorData = null;
		}
		ActorData x = actorData;
		if (this.m_actor != null)
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
			if (x != null && this.m_actor.IsVisibleToClient())
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
				if (HUD_UI.Get() != null)
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
					if (UIOverconData.Get() != null)
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
						UIOverconData.NameToOverconEntry overconEntryById = UIOverconData.Get().GetOverconEntryById(overconId);
						if (overconEntryById != null)
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
							if (!overconEntryById.m_isHidden)
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
								HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.SpawnOverconForActor(this.m_actor, overconEntryById, false);
							}
						}
					}
				}
			}
		}
	}

	[Command]
	protected void CmdSelectAbilityRequest(int actionTypeInt)
	{
	}

	public void SendQueueSimpleActionRequest(AbilityData.ActionType actionType)
	{
		UISounds.GetUISounds().Play("ui/ingame/v1/hud/catalyst_select");
		ActorData actor = this.m_actor;
		if (actor != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorController.SendQueueSimpleActionRequest(AbilityData.ActionType)).MethodHandle;
			}
			if (actor.GetAbilityData() != null)
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
				actor.GetAbilityData().SetLastSelectedAbility(actor.GetAbilityData().GetAbilityOfActionType(actionType));
			}
		}
		this.CallCmdQueueSimpleActionRequest((int)actionType);
	}

	[Command]
	protected void CmdQueueSimpleActionRequest(int actionTypeInt)
	{
	}

	public void RequestCustomGamePause(bool desiredPause, int requestActorIndex)
	{
		if (NetworkServer.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorController.RequestCustomGamePause(bool, int)).MethodHandle;
			}
			this.HandleCustomGamePauseOnServer(desiredPause, requestActorIndex);
		}
		else
		{
			this.CallCmdCustomGamePause(desiredPause, requestActorIndex);
		}
	}

	private void HandleCustomGamePauseOnServer(bool desiredPause, int requestActorIndex)
	{
	}

	[Command]
	private void CmdCustomGamePause(bool desiredPause, int requestActorIndex)
	{
		this.HandleCustomGamePauseOnServer(desiredPause, requestActorIndex);
	}

	[ClientRpc]
	internal void RpcUpdateRemainingMovement(float remainingMovement, float remainingMovementWithQueuedAbility)
	{
		if (this.m_actor != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorController.RpcUpdateRemainingMovement(float, float)).MethodHandle;
			}
			if (GameFlowData.Get() != null && GameFlowData.Get().activeOwnedActorData == this.m_actor)
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
				bool flag = false;
				if (this.m_actor.RemainingHorizontalMovement != remainingMovement)
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
					this.m_actor.RemainingHorizontalMovement = remainingMovement;
					flag = true;
				}
				if (this.m_actor.RemainingMovementWithQueuedAbility != remainingMovementWithQueuedAbility)
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
					this.m_actor.RemainingMovementWithQueuedAbility = remainingMovementWithQueuedAbility;
					flag = true;
				}
				if (flag)
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
					this.m_actor.GetActorMovement().UpdateSquaresCanMoveTo();
				}
			}
		}
	}

	private void UNetVersion()
	{
	}

	protected static void InvokeCmdCmdDebugTeleportRequest(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorController.InvokeCmdCmdDebugTeleportRequest(NetworkBehaviour, NetworkReader)).MethodHandle;
			}
			Debug.LogError("Command CmdDebugTeleportRequest called on client.");
			return;
		}
		((ActorController)obj).CmdDebugTeleportRequest((int)reader.ReadPackedUInt32(), (int)reader.ReadPackedUInt32());
	}

	protected static void InvokeCmdCmdPickedRespawnRequest(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorController.InvokeCmdCmdPickedRespawnRequest(NetworkBehaviour, NetworkReader)).MethodHandle;
			}
			Debug.LogError("Command CmdPickedRespawnRequest called on client.");
			return;
		}
		((ActorController)obj).CmdPickedRespawnRequest((int)reader.ReadPackedUInt32(), (int)reader.ReadPackedUInt32());
	}

	protected static void InvokeCmdCmdSendMinimapPing(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorController.InvokeCmdCmdSendMinimapPing(NetworkBehaviour, NetworkReader)).MethodHandle;
			}
			Debug.LogError("Command CmdSendMinimapPing called on client.");
			return;
		}
		((ActorController)obj).CmdSendMinimapPing((int)reader.ReadPackedUInt32(), reader.ReadVector3(), (ActorController.PingType)reader.ReadInt32());
	}

	protected static void InvokeCmdCmdSendAbilityPing(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdSendAbilityPing called on client.");
			return;
		}
		((ActorController)obj).CmdSendAbilityPing((int)reader.ReadPackedUInt32(), GeneratedNetworkCode._ReadLocalizationArg_AbilityPing_None(reader));
	}

	protected static void InvokeCmdCmdSelectAbilityRequest(NetworkBehaviour obj, NetworkReader reader)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorController.InvokeCmdCmdSelectAbilityRequest(NetworkBehaviour, NetworkReader)).MethodHandle;
			}
			Debug.LogError("Command CmdSelectAbilityRequest called on client.");
			return;
		}
		((ActorController)obj).CmdSelectAbilityRequest((int)reader.ReadPackedUInt32());
	}

	protected static void InvokeCmdCmdQueueSimpleActionRequest(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorController.InvokeCmdCmdQueueSimpleActionRequest(NetworkBehaviour, NetworkReader)).MethodHandle;
			}
			Debug.LogError("Command CmdQueueSimpleActionRequest called on client.");
			return;
		}
		((ActorController)obj).CmdQueueSimpleActionRequest((int)reader.ReadPackedUInt32());
	}

	protected static void InvokeCmdCmdCustomGamePause(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdCustomGamePause called on client.");
			return;
		}
		((ActorController)obj).CmdCustomGamePause(reader.ReadBoolean(), (int)reader.ReadPackedUInt32());
	}

	public void CallCmdDebugTeleportRequest(int x, int y)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("Command function CmdDebugTeleportRequest called on server.");
			return;
		}
		if (base.isServer)
		{
			this.CmdDebugTeleportRequest(x, y);
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write(0);
		networkWriter.Write((short)((ushort)5));
		networkWriter.WritePackedUInt32((uint)ActorController.kCmdCmdDebugTeleportRequest);
		networkWriter.Write(base.GetComponent<NetworkIdentity>().netId);
		networkWriter.WritePackedUInt32((uint)x);
		networkWriter.WritePackedUInt32((uint)y);
		base.SendCommandInternal(networkWriter, 0, "CmdDebugTeleportRequest");
	}

	public void CallCmdPickedRespawnRequest(int x, int y)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("Command function CmdPickedRespawnRequest called on server.");
			return;
		}
		if (base.isServer)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorController.CallCmdPickedRespawnRequest(int, int)).MethodHandle;
			}
			this.CmdPickedRespawnRequest(x, y);
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write(0);
		networkWriter.Write((short)((ushort)5));
		networkWriter.WritePackedUInt32((uint)ActorController.kCmdCmdPickedRespawnRequest);
		networkWriter.Write(base.GetComponent<NetworkIdentity>().netId);
		networkWriter.WritePackedUInt32((uint)x);
		networkWriter.WritePackedUInt32((uint)y);
		base.SendCommandInternal(networkWriter, 0, "CmdPickedRespawnRequest");
	}

	public void CallCmdSendMinimapPing(int teamIndex, Vector3 worldPosition, ActorController.PingType pingType)
	{
		if (!NetworkClient.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorController.CallCmdSendMinimapPing(int, Vector3, ActorController.PingType)).MethodHandle;
			}
			Debug.LogError("Command function CmdSendMinimapPing called on server.");
			return;
		}
		if (base.isServer)
		{
			this.CmdSendMinimapPing(teamIndex, worldPosition, pingType);
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write(0);
		networkWriter.Write((short)((ushort)5));
		networkWriter.WritePackedUInt32((uint)ActorController.kCmdCmdSendMinimapPing);
		networkWriter.Write(base.GetComponent<NetworkIdentity>().netId);
		networkWriter.WritePackedUInt32((uint)teamIndex);
		networkWriter.Write(worldPosition);
		networkWriter.Write((int)pingType);
		base.SendCommandInternal(networkWriter, 0, "CmdSendMinimapPing");
	}

	public void CallCmdSendAbilityPing(int teamIndex, LocalizationArg_AbilityPing localizedPing)
	{
		if (!NetworkClient.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorController.CallCmdSendAbilityPing(int, LocalizationArg_AbilityPing)).MethodHandle;
			}
			Debug.LogError("Command function CmdSendAbilityPing called on server.");
			return;
		}
		if (base.isServer)
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
			this.CmdSendAbilityPing(teamIndex, localizedPing);
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write(0);
		networkWriter.Write((short)((ushort)5));
		networkWriter.WritePackedUInt32((uint)ActorController.kCmdCmdSendAbilityPing);
		networkWriter.Write(base.GetComponent<NetworkIdentity>().netId);
		networkWriter.WritePackedUInt32((uint)teamIndex);
		GeneratedNetworkCode._WriteLocalizationArg_AbilityPing_None(networkWriter, localizedPing);
		base.SendCommandInternal(networkWriter, 0, "CmdSendAbilityPing");
	}

	public void CallCmdSelectAbilityRequest(int actionTypeInt)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("Command function CmdSelectAbilityRequest called on server.");
			return;
		}
		if (base.isServer)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorController.CallCmdSelectAbilityRequest(int)).MethodHandle;
			}
			this.CmdSelectAbilityRequest(actionTypeInt);
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write(0);
		networkWriter.Write((short)((ushort)5));
		networkWriter.WritePackedUInt32((uint)ActorController.kCmdCmdSelectAbilityRequest);
		networkWriter.Write(base.GetComponent<NetworkIdentity>().netId);
		networkWriter.WritePackedUInt32((uint)actionTypeInt);
		base.SendCommandInternal(networkWriter, 0, "CmdSelectAbilityRequest");
	}

	public void CallCmdQueueSimpleActionRequest(int actionTypeInt)
	{
		if (!NetworkClient.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorController.CallCmdQueueSimpleActionRequest(int)).MethodHandle;
			}
			Debug.LogError("Command function CmdQueueSimpleActionRequest called on server.");
			return;
		}
		if (base.isServer)
		{
			this.CmdQueueSimpleActionRequest(actionTypeInt);
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write(0);
		networkWriter.Write((short)((ushort)5));
		networkWriter.WritePackedUInt32((uint)ActorController.kCmdCmdQueueSimpleActionRequest);
		networkWriter.Write(base.GetComponent<NetworkIdentity>().netId);
		networkWriter.WritePackedUInt32((uint)actionTypeInt);
		base.SendCommandInternal(networkWriter, 0, "CmdQueueSimpleActionRequest");
	}

	public void CallCmdCustomGamePause(bool desiredPause, int requestActorIndex)
	{
		if (!NetworkClient.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorController.CallCmdCustomGamePause(bool, int)).MethodHandle;
			}
			Debug.LogError("Command function CmdCustomGamePause called on server.");
			return;
		}
		if (base.isServer)
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
			this.CmdCustomGamePause(desiredPause, requestActorIndex);
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write(0);
		networkWriter.Write((short)((ushort)5));
		networkWriter.WritePackedUInt32((uint)ActorController.kCmdCmdCustomGamePause);
		networkWriter.Write(base.GetComponent<NetworkIdentity>().netId);
		networkWriter.Write(desiredPause);
		networkWriter.WritePackedUInt32((uint)requestActorIndex);
		base.SendCommandInternal(networkWriter, 0, "CmdCustomGamePause");
	}

	protected static void InvokeRpcRpcUpdateRemainingMovement(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcUpdateRemainingMovement called on server.");
			return;
		}
		((ActorController)obj).RpcUpdateRemainingMovement(reader.ReadSingle(), reader.ReadSingle());
	}

	public void CallRpcUpdateRemainingMovement(float remainingMovement, float remainingMovementWithQueuedAbility)
	{
		if (!NetworkServer.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorController.CallRpcUpdateRemainingMovement(float, float)).MethodHandle;
			}
			Debug.LogError("RPC Function RpcUpdateRemainingMovement called on client.");
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write(0);
		networkWriter.Write((short)((ushort)2));
		networkWriter.WritePackedUInt32((uint)ActorController.kRpcRpcUpdateRemainingMovement);
		networkWriter.Write(base.GetComponent<NetworkIdentity>().netId);
		networkWriter.Write(remainingMovement);
		networkWriter.Write(remainingMovementWithQueuedAbility);
		this.SendRPCInternal(networkWriter, 0, "RpcUpdateRemainingMovement");
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		bool result;
		return result;
	}

	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
	}

	public enum PingType
	{
		Default,
		Assist,
		Defend,
		Enemy,
		Move
	}
}

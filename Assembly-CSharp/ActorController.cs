using System.Collections.Generic;
using Unity;
using UnityEngine;
using UnityEngine.Networking;

public class ActorController : NetworkBehaviour
{
	public enum PingType
	{
		Default,
		Assist,
		Defend,
		Enemy,
		Move
	}

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

	private static int kCmdCmdDebugTeleportRequest;

	private static int kCmdCmdPickedRespawnRequest;

	private static int kCmdCmdSendMinimapPing;

	private static int kCmdCmdSendAbilityPing;

	private static int kCmdCmdSelectAbilityRequest;

	private static int kCmdCmdQueueSimpleActionRequest;

	private static int kCmdCmdCustomGamePause;

	private static int kRpcRpcUpdateRemainingMovement;

	static ActorController()
	{
		kCmdCmdDebugTeleportRequest = -1583259838;
		NetworkBehaviour.RegisterCommandDelegate(typeof(ActorController), kCmdCmdDebugTeleportRequest, InvokeCmdCmdDebugTeleportRequest);
		kCmdCmdPickedRespawnRequest = 1763304984;
		NetworkBehaviour.RegisterCommandDelegate(typeof(ActorController), kCmdCmdPickedRespawnRequest, InvokeCmdCmdPickedRespawnRequest);
		kCmdCmdSendMinimapPing = -810618818;
		NetworkBehaviour.RegisterCommandDelegate(typeof(ActorController), kCmdCmdSendMinimapPing, InvokeCmdCmdSendMinimapPing);
		kCmdCmdSendAbilityPing = -963392189;
		NetworkBehaviour.RegisterCommandDelegate(typeof(ActorController), kCmdCmdSendAbilityPing, InvokeCmdCmdSendAbilityPing);
		kCmdCmdSelectAbilityRequest = -1183646894;
		NetworkBehaviour.RegisterCommandDelegate(typeof(ActorController), kCmdCmdSelectAbilityRequest, InvokeCmdCmdSelectAbilityRequest);
		kCmdCmdQueueSimpleActionRequest = -797856057;
		NetworkBehaviour.RegisterCommandDelegate(typeof(ActorController), kCmdCmdQueueSimpleActionRequest, InvokeCmdCmdQueueSimpleActionRequest);
		kCmdCmdCustomGamePause = 983951586;
		NetworkBehaviour.RegisterCommandDelegate(typeof(ActorController), kCmdCmdCustomGamePause, InvokeCmdCmdCustomGamePause);
		kRpcRpcUpdateRemainingMovement = 64425877;
		NetworkBehaviour.RegisterRpcDelegate(typeof(ActorController), kRpcRpcUpdateRemainingMovement, InvokeRpcRpcUpdateRemainingMovement);
		NetworkCRC.RegisterBehaviour("ActorController", 0);
	}

	private void Awake()
	{
		m_actor = GetComponent<ActorData>();
	}

	public Ability GetLastTargetedAbility(ref int lastTargetIndex)
	{
		lastTargetIndex = m_lastTargetIndex;
		return m_lastTargetedAbility;
	}

	private void OnRespawn()
	{
		ActorData actor = m_actor;
		if (!Camera.main)
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
			if (actor == GameFlowData.Get().activeOwnedActorData)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					CameraManager.Get().SetTargetObject(base.gameObject, CameraManager.CameraTargetReason.ClientActorRespawned);
					return;
				}
			}
			return;
		}
	}

	private void HandlePickRespawnInput()
	{
		ActorData actor = m_actor;
		if (!(actor == GameFlowData.Get().activeOwnedActorData))
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
			if (!Input.GetMouseButtonUp(0))
			{
				if (!Input.GetMouseButtonUp(1))
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
					break;
				}
			}
			if (!InterfaceManager.Get().ShouldHandleMouseClick())
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
				BoardSquare playerClampedSquare = Board.Get().PlayerClampedSquare;
				bool flag = actor.respawnSquares.Contains(playerClampedSquare);
				if (!(playerClampedSquare != null))
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
					if (flag)
					{
						while (true)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							CallCmdPickedRespawnRequest(playerClampedSquare.x, playerClampedSquare.y);
							actor.ShowRespawnFlare(playerClampedSquare, false);
							return;
						}
					}
					return;
				}
			}
		}
	}

	private void HandleDebugTeleport()
	{
		ActorData actor = m_actor;
		if (!(actor == GameFlowData.Get().activeOwnedActorData))
		{
			return;
		}
		BoardSquare playerFreeSquare = Board.Get().PlayerFreeSquare;
		if (!(playerFreeSquare != null))
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
			if (!playerFreeSquare.IsBaselineHeight())
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
				bool flag = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
				bool flag2 = Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt);
				if (!flag)
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
					if (!flag2)
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
						if (!Input.GetMouseButtonUp(2))
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
							if (InterfaceManager.Get().ShouldHandleMouseClick())
							{
								while (true)
								{
									switch (5)
									{
									case 0:
										continue;
									}
									CallCmdDebugTeleportRequest(playerFreeSquare.x, playerFreeSquare.y);
									return;
								}
							}
							return;
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
	internal void CmdSendMinimapPing(int teamIndex, Vector3 worldPosition, PingType pingType)
	{
	}

	[Command]
	internal void CmdSendAbilityPing(int teamIndex, LocalizationArg_AbilityPing localizedPing)
	{
	}

	public void ClearHighlights()
	{
		ClearMovementHighlights();
		ClearTargetingHighlights();
		ClearRespawnHighlights();
	}

	private void ClearMovementHighlights()
	{
		m_currentCanMoveToSquares.Clear();
		m_currentCanMoveToWithAbilitySquares.Clear();
		if ((bool)m_canMoveToHighlight)
		{
			HighlightUtils.DestroyBoundaryHighlightObject(m_canMoveToHighlight);
		}
		if ((bool)m_canMoveToWithAbilityHighlight)
		{
			HighlightUtils.DestroyBoundaryHighlightObject(m_canMoveToWithAbilityHighlight);
		}
	}

	private void ClearTargetingHighlights()
	{
		m_currentTargetingSquares.Clear();
		if ((bool)m_targetingHighlight)
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
			HighlightUtils.DestroyBoundaryHighlightObject(m_targetingHighlight);
			m_targetingHighlight = null;
		}
		m_lastTargetedAbility = null;
		m_lastTargetIndex = -1;
	}

	private void ClearRespawnHighlights()
	{
		m_currentRespawnSquares.Clear();
		if (!m_respawnHighlight)
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
			HighlightUtils.DestroyBoundaryHighlightObject(m_respawnHighlight);
			m_respawnHighlight = null;
			return;
		}
	}

	private void Update()
	{
		ActorData actor = m_actor;
		if (!(actor == GameFlowData.Get().activeOwnedActorData))
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
			ActorTurnSM actorTurnSM = actor.GetActorTurnSM();
			if (actorTurnSM.CanPickRespawnLocation())
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
						HandlePickRespawnInput();
						return;
					}
				}
			}
			if (actorTurnSM.CurrentState == TurnStateEnum.DECIDING)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					HandleDebugTeleport();
					return;
				}
			}
			return;
		}
	}

	public void SetMovementDistanceLinesVisible(bool visible)
	{
		m_movementLinesVisible = visible;
		if (m_canMoveToHighlight != null)
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
			m_canMoveToHighlight.gameObject.SetActive(m_movementLinesVisible);
		}
		if (m_canMoveToWithAbilityHighlight != null)
		{
			m_canMoveToWithAbilityHighlight.gameObject.SetActive(m_movementLinesVisible);
		}
	}

	public HashSet<BoardSquare> GetSquaresToClampTo()
	{
		ActorTurnSM actorTurnSM = m_actor.GetActorTurnSM();
		if (actorTurnSM.AmDecidingMovement())
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
					return m_actor.GetActorMovement().SquaresCanMoveTo;
				}
			}
		}
		if (actorTurnSM.AmTargetingAction())
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return m_currentTargetingSquares;
				}
			}
		}
		if (actorTurnSM.CurrentState == TurnStateEnum.PICKING_RESPAWN)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return m_currentRespawnSquares;
				}
			}
		}
		return null;
	}

	public void RecalcAndHighlightValidSquares()
	{
		Board board = Board.Get();
		ActorData actor = m_actor;
		AbilityData abilityData = actor.GetAbilityData();
		ActorMovement actorMovement = actor.GetActorMovement();
		m_canMoveToSquaresScratch.Clear();
		m_canMoveToWithQueuedAbilityScratch.Clear();
		m_targetingSquaresScratch.Clear();
		bool flag = false;
		bool flag2 = false;
		bool flag3 = m_actor.GetActorTurnSM().AmDecidingMovement();
		bool flag4 = m_actor.GetActorTurnSM().CurrentState == TurnStateEnum.TARGETING_ACTION;
		bool markedForUpdateValidSquares = Board.Get().MarkedForUpdateValidSquares;
		bool flag5 = actor.GetPostAbilityHorizontalMovementChange() > 0f;
		bool flag6 = abilityData.GetQueuedAbilitiesMovementAdjust() < 0f;
		if (flag3)
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
			if (markedForUpdateValidSquares)
			{
				if (flag5)
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
					if (!flag6)
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
						using (HashSet<BoardSquare>.Enumerator enumerator = actorMovement.SquaresCanMoveTo.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								BoardSquare current = enumerator.Current;
								m_canMoveToSquaresScratch.Add(current);
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
					}
				}
				if (actorMovement.SquaresCanMoveToWithQueuedAbility.SetEquals(actorMovement.SquaresCanMoveTo))
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
					if (flag5)
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
						if (!flag6)
						{
							goto IL_01b1;
						}
					}
				}
				using (HashSet<BoardSquare>.Enumerator enumerator2 = actorMovement.SquaresCanMoveToWithQueuedAbility.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						BoardSquare current2 = enumerator2.Current;
						m_canMoveToWithQueuedAbilityScratch.Add(current2);
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
				goto IL_01b1;
			}
			flag = true;
			goto IL_01c1;
		}
		if (flag4)
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
			Ability selectedAbility = abilityData.GetSelectedAbility();
			int targetSelectionIndex = m_actor.GetActorTurnSM().GetTargetSelectionIndex();
			if (!(selectedAbility != m_lastTargetedAbility))
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
				if (targetSelectionIndex == m_lastTargetIndex)
				{
					flag2 = true;
					goto IL_0241;
				}
			}
			m_targetingSquaresScratch = AbilityUtils.GetTargetableSquaresForAbility(selectedAbility, abilityData, actor, targetSelectionIndex);
			m_lastTargetedAbility = selectedAbility;
			m_lastTargetIndex = targetSelectionIndex;
		}
		goto IL_0241;
		IL_01b1:
		Board.Get().MarkForUpdateValidSquares(false);
		goto IL_01c1;
		IL_01c1:
		ClearTargetingHighlights();
		goto IL_0241;
		IL_058a:
		int num;
		if (GameFlowData.Get().IsInDecisionState() && m_actor.IsDead() && SpawnPointManager.Get() != null)
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
			num = (SpawnPointManager.Get().m_playersSelectRespawn ? 1 : 0);
		}
		else
		{
			num = 0;
		}
		bool flag7 = (byte)num != 0;
		List<BoardSquare> respawnSquares = actor.respawnSquares;
		if (flag7)
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
			if (actor.IsDead() && !respawnSquares.IsNullOrEmpty())
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
				if (m_currentRespawnSquares.Count == respawnSquares.Count)
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
					if (respawnSquares.TrueForAll((BoardSquare s) => m_currentRespawnSquares.Contains(s)))
					{
						goto IL_06b9;
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
				ClearRespawnHighlights();
				m_respawnHighlight = HighlightUtils.Get().CreateBoundaryHighlight(respawnSquares, BoardSquare.s_respawnOptionHighlightColor, true);
				if ((bool)m_respawnHighlight)
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
					m_respawnHighlight.AddComponent<HighlightParent>();
				}
				m_currentRespawnSquares.Clear();
				m_currentRespawnSquares = new HashSet<BoardSquare>(respawnSquares);
				goto IL_06b9;
			}
		}
		ClearRespawnHighlights();
		goto IL_06b9;
		IL_06b9:
		if ((bool)m_canMoveToWithAbilityHighlight)
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
			bool flag8 = flag3 && m_currentCanMoveToWithAbilitySquares.Count > 0 && m_movementLinesVisible;
			if (m_canMoveToWithAbilityHighlight.gameObject.activeSelf != flag8)
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
				m_canMoveToWithAbilityHighlight.gameObject.SetActive(flag8);
			}
		}
		if ((bool)m_canMoveToHighlight)
		{
			int num2;
			if (flag3 && m_currentCanMoveToSquares.Count > 0)
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
				num2 = (m_movementLinesVisible ? 1 : 0);
			}
			else
			{
				num2 = 0;
			}
			bool flag9 = (byte)num2 != 0;
			if (flag9 && !FirstTurnMovement.ForceShowSprintRange(actor))
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
				Vector3 position = HighlightUtils.Get().MovementMouseOverCursor.transform.position;
				BoardSquare boardSquareSafe = board.GetBoardSquareSafe(position.x, position.z);
				if (m_canMoveToWithQueuedAbilityScratch.Contains(boardSquareSafe))
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
					flag9 = false;
				}
			}
			if (m_canMoveToHighlight.gameObject.activeSelf != flag9)
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
				m_canMoveToHighlight.gameObject.SetActive(flag9);
			}
		}
		if (!m_targetingHighlight)
		{
			return;
		}
		int num3;
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
			num3 = ((m_currentTargetingSquares.Count > 0) ? 1 : 0);
		}
		else
		{
			num3 = 0;
		}
		bool flag10 = (byte)num3 != 0;
		if (m_targetingHighlight.gameObject.activeSelf == flag10)
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
			m_targetingHighlight.gameObject.SetActive(flag10);
			return;
		}
		IL_0241:
		if (!flag)
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
			if (m_currentCanMoveToSquares != m_canMoveToSquaresScratch)
			{
				if (!m_currentCanMoveToSquares.SetEquals(m_canMoveToSquaresScratch))
				{
					goto IL_02fc;
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
		}
		if (!flag)
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
			if (m_currentCanMoveToWithAbilitySquares != m_canMoveToWithQueuedAbilityScratch)
			{
				if (!m_currentCanMoveToWithAbilitySquares.SetEquals(m_canMoveToWithQueuedAbilityScratch))
				{
					goto IL_02fc;
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
		}
		if (!flag2)
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
			if (m_currentTargetingSquares != m_targetingSquaresScratch && !m_currentTargetingSquares.SetEquals(m_targetingSquaresScratch))
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
				goto IL_02fc;
			}
		}
		goto IL_058a;
		IL_02fc:
		if (flag3)
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
			if ((bool)m_canMoveToWithAbilityHighlight)
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
				HighlightUtils.DestroyBoundaryHighlightObject(m_canMoveToWithAbilityHighlight);
			}
			m_canMoveToWithAbilityHighlight = HighlightUtils.Get().CreateBoundaryHighlight(m_canMoveToWithQueuedAbilityScratch, BoardSquare.s_moveableHighlightColor, true);
			if ((bool)m_canMoveToWithAbilityHighlight)
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
				m_canMoveToWithAbilityHighlight.AddComponent<HighlightParent>();
			}
			if ((bool)m_canMoveToHighlight)
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
				HighlightUtils.DestroyBoundaryHighlightObject(m_canMoveToHighlight);
			}
			m_canMoveToHighlight = HighlightUtils.Get().CreateBoundaryHighlight(m_canMoveToSquaresScratch, BoardSquare.s_moveableHighlightColor);
			if ((bool)m_canMoveToHighlight)
			{
				m_canMoveToHighlight.AddComponent<HighlightParent>();
			}
		}
		else
		{
			HashSet<BoardSquare> hashSet = new HashSet<BoardSquare>();
			if (actor.GetCurrentBoardSquare() != null)
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
				hashSet.Add(actor.GetCurrentBoardSquare());
				if (m_lastTargetedAbility != null)
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
					if (m_lastTargetedAbility.Targeters != null)
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
						using (List<AbilityUtil_Targeter>.Enumerator enumerator3 = m_lastTargetedAbility.Targeters.GetEnumerator())
						{
							while (enumerator3.MoveNext())
							{
								AbilityUtil_Targeter current3 = enumerator3.Current;
								if (current3 != null)
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
									BoardSquare boardSquareSafe2 = Board.Get().GetBoardSquareSafe(current3.LastUpdatingGridPos);
									if (boardSquareSafe2 != null)
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
										hashSet.Add(boardSquareSafe2);
									}
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
			if ((bool)m_targetingHighlight)
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
				HighlightUtils.DestroyBoundaryHighlightObject(m_targetingHighlight);
			}
			m_targetingHighlight = HighlightUtils.Get().CreateBoundaryHighlight(m_targetingSquaresScratch, BoardSquare.s_targetableByAbilityHighlightColor, false, hashSet);
			if ((bool)m_targetingHighlight)
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
				m_targetingHighlight.AddComponent<HighlightParent>();
			}
		}
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
			CopyOverHashsetValues(m_currentCanMoveToSquares, m_canMoveToSquaresScratch);
			CopyOverHashsetValues(m_currentCanMoveToWithAbilitySquares, m_canMoveToWithQueuedAbilityScratch);
		}
		if (!flag2)
		{
			CopyOverHashsetValues(m_currentTargetingSquares, m_targetingSquaresScratch);
		}
		goto IL_058a;
	}

	private void CopyOverHashsetValues(HashSet<BoardSquare> toSet, HashSet<BoardSquare> fromSet)
	{
		toSet.Clear();
		using (HashSet<BoardSquare>.Enumerator enumerator = fromSet.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				BoardSquare current = enumerator.Current;
				toSet.Add(current);
			}
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
					return;
				}
			}
		}
	}

	public void SendSelectAbilityRequest()
	{
		AbilityData.ActionType actionTypeInt = AbilityData.ActionType.INVALID_ACTION;
		AbilityData component = GetComponent<AbilityData>();
		UISounds.GetUISounds().Play("ui/ingame/v1/hud/ability_select");
		if ((bool)component)
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
			actionTypeInt = component.GetSelectedActionType();
		}
		CallCmdSelectAbilityRequest((int)actionTypeInt);
	}

	internal void ShowOvercon(int overconId, bool allyOnly)
	{
		object obj;
		if (GameFlowData.Get() != null)
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
			obj = GameFlowData.Get().activeOwnedActorData;
		}
		else
		{
			obj = null;
		}
		ActorData x = (ActorData)obj;
		if (!(m_actor != null))
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
			if (!(x != null) || !m_actor.IsVisibleToClient())
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
				if (!(HUD_UI.Get() != null))
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
					if (!(UIOverconData.Get() != null))
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
						UIOverconData.NameToOverconEntry overconEntryById = UIOverconData.Get().GetOverconEntryById(overconId);
						if (overconEntryById == null)
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
							if (!overconEntryById.m_isHidden)
							{
								while (true)
								{
									switch (5)
									{
									case 0:
										continue;
									}
									HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.SpawnOverconForActor(m_actor, overconEntryById, false);
									return;
								}
							}
							return;
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
		ActorData actor = m_actor;
		if (actor != null)
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
			if (actor.GetAbilityData() != null)
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
				actor.GetAbilityData().SetLastSelectedAbility(actor.GetAbilityData().GetAbilityOfActionType(actionType));
			}
		}
		CallCmdQueueSimpleActionRequest((int)actionType);
	}

	[Command]
	protected void CmdQueueSimpleActionRequest(int actionTypeInt)
	{
	}

	public void RequestCustomGamePause(bool desiredPause, int requestActorIndex)
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
					HandleCustomGamePauseOnServer(desiredPause, requestActorIndex);
					return;
				}
			}
		}
		CallCmdCustomGamePause(desiredPause, requestActorIndex);
	}

	private void HandleCustomGamePauseOnServer(bool desiredPause, int requestActorIndex)
	{
	}

	[Command]
	private void CmdCustomGamePause(bool desiredPause, int requestActorIndex)
	{
		HandleCustomGamePauseOnServer(desiredPause, requestActorIndex);
	}

	[ClientRpc]
	internal void RpcUpdateRemainingMovement(float remainingMovement, float remainingMovementWithQueuedAbility)
	{
		if (!(m_actor != null))
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
			if (!(GameFlowData.Get() != null) || !(GameFlowData.Get().activeOwnedActorData == m_actor))
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
				bool flag = false;
				if (m_actor.RemainingHorizontalMovement != remainingMovement)
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
					m_actor.RemainingHorizontalMovement = remainingMovement;
					flag = true;
				}
				if (m_actor.RemainingMovementWithQueuedAbility != remainingMovementWithQueuedAbility)
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
					m_actor.RemainingMovementWithQueuedAbility = remainingMovementWithQueuedAbility;
					flag = true;
				}
				if (flag)
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						m_actor.GetActorMovement().UpdateSquaresCanMoveTo();
						return;
					}
				}
				return;
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
					Debug.LogError("Command CmdDebugTeleportRequest called on client.");
					return;
				}
			}
		}
		((ActorController)obj).CmdDebugTeleportRequest((int)reader.ReadPackedUInt32(), (int)reader.ReadPackedUInt32());
	}

	protected static void InvokeCmdCmdPickedRespawnRequest(NetworkBehaviour obj, NetworkReader reader)
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
					Debug.LogError("Command CmdPickedRespawnRequest called on client.");
					return;
				}
			}
		}
		((ActorController)obj).CmdPickedRespawnRequest((int)reader.ReadPackedUInt32(), (int)reader.ReadPackedUInt32());
	}

	protected static void InvokeCmdCmdSendMinimapPing(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
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
					Debug.LogError("Command CmdSendMinimapPing called on client.");
					return;
				}
			}
		}
		((ActorController)obj).CmdSendMinimapPing((int)reader.ReadPackedUInt32(), reader.ReadVector3(), (PingType)reader.ReadInt32());
	}

	protected static void InvokeCmdCmdSendAbilityPing(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdSendAbilityPing called on client.");
		}
		else
		{
			((ActorController)obj).CmdSendAbilityPing((int)reader.ReadPackedUInt32(), GeneratedNetworkCode._ReadLocalizationArg_AbilityPing_None(reader));
		}
	}

	protected static void InvokeCmdCmdSelectAbilityRequest(NetworkBehaviour obj, NetworkReader reader)
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
					Debug.LogError("Command CmdSelectAbilityRequest called on client.");
					return;
				}
			}
		}
		((ActorController)obj).CmdSelectAbilityRequest((int)reader.ReadPackedUInt32());
	}

	protected static void InvokeCmdCmdQueueSimpleActionRequest(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
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
					Debug.LogError("Command CmdQueueSimpleActionRequest called on client.");
					return;
				}
			}
		}
		((ActorController)obj).CmdQueueSimpleActionRequest((int)reader.ReadPackedUInt32());
	}

	protected static void InvokeCmdCmdCustomGamePause(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdCustomGamePause called on client.");
		}
		else
		{
			((ActorController)obj).CmdCustomGamePause(reader.ReadBoolean(), (int)reader.ReadPackedUInt32());
		}
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
			CmdDebugTeleportRequest(x, y);
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)5);
		networkWriter.WritePackedUInt32((uint)kCmdCmdDebugTeleportRequest);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		networkWriter.WritePackedUInt32((uint)x);
		networkWriter.WritePackedUInt32((uint)y);
		SendCommandInternal(networkWriter, 0, "CmdDebugTeleportRequest");
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
					CmdPickedRespawnRequest(x, y);
					return;
				}
			}
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)5);
		networkWriter.WritePackedUInt32((uint)kCmdCmdPickedRespawnRequest);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		networkWriter.WritePackedUInt32((uint)x);
		networkWriter.WritePackedUInt32((uint)y);
		SendCommandInternal(networkWriter, 0, "CmdPickedRespawnRequest");
	}

	public void CallCmdSendMinimapPing(int teamIndex, Vector3 worldPosition, PingType pingType)
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
					Debug.LogError("Command function CmdSendMinimapPing called on server.");
					return;
				}
			}
		}
		if (base.isServer)
		{
			CmdSendMinimapPing(teamIndex, worldPosition, pingType);
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)5);
		networkWriter.WritePackedUInt32((uint)kCmdCmdSendMinimapPing);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		networkWriter.WritePackedUInt32((uint)teamIndex);
		networkWriter.Write(worldPosition);
		networkWriter.Write((int)pingType);
		SendCommandInternal(networkWriter, 0, "CmdSendMinimapPing");
	}

	public void CallCmdSendAbilityPing(int teamIndex, LocalizationArg_AbilityPing localizedPing)
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
					Debug.LogError("Command function CmdSendAbilityPing called on server.");
					return;
				}
			}
		}
		if (base.isServer)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					CmdSendAbilityPing(teamIndex, localizedPing);
					return;
				}
			}
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)5);
		networkWriter.WritePackedUInt32((uint)kCmdCmdSendAbilityPing);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		networkWriter.WritePackedUInt32((uint)teamIndex);
		GeneratedNetworkCode._WriteLocalizationArg_AbilityPing_None(networkWriter, localizedPing);
		SendCommandInternal(networkWriter, 0, "CmdSendAbilityPing");
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
					CmdSelectAbilityRequest(actionTypeInt);
					return;
				}
			}
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)5);
		networkWriter.WritePackedUInt32((uint)kCmdCmdSelectAbilityRequest);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		networkWriter.WritePackedUInt32((uint)actionTypeInt);
		SendCommandInternal(networkWriter, 0, "CmdSelectAbilityRequest");
	}

	public void CallCmdQueueSimpleActionRequest(int actionTypeInt)
	{
		if (!NetworkClient.active)
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
					Debug.LogError("Command function CmdQueueSimpleActionRequest called on server.");
					return;
				}
			}
		}
		if (base.isServer)
		{
			CmdQueueSimpleActionRequest(actionTypeInt);
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)5);
		networkWriter.WritePackedUInt32((uint)kCmdCmdQueueSimpleActionRequest);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		networkWriter.WritePackedUInt32((uint)actionTypeInt);
		SendCommandInternal(networkWriter, 0, "CmdQueueSimpleActionRequest");
	}

	public void CallCmdCustomGamePause(bool desiredPause, int requestActorIndex)
	{
		if (!NetworkClient.active)
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
					Debug.LogError("Command function CmdCustomGamePause called on server.");
					return;
				}
			}
		}
		if (base.isServer)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					CmdCustomGamePause(desiredPause, requestActorIndex);
					return;
				}
			}
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)5);
		networkWriter.WritePackedUInt32((uint)kCmdCmdCustomGamePause);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		networkWriter.Write(desiredPause);
		networkWriter.WritePackedUInt32((uint)requestActorIndex);
		SendCommandInternal(networkWriter, 0, "CmdCustomGamePause");
	}

	protected static void InvokeRpcRpcUpdateRemainingMovement(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcUpdateRemainingMovement called on server.");
		}
		else
		{
			((ActorController)obj).RpcUpdateRemainingMovement(reader.ReadSingle(), reader.ReadSingle());
		}
	}

	public void CallRpcUpdateRemainingMovement(float remainingMovement, float remainingMovementWithQueuedAbility)
	{
		if (!NetworkServer.active)
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
					Debug.LogError("RPC Function RpcUpdateRemainingMovement called on client.");
					return;
				}
			}
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)2);
		networkWriter.WritePackedUInt32((uint)kRpcRpcUpdateRemainingMovement);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		networkWriter.Write(remainingMovement);
		networkWriter.Write(remainingMovementWithQueuedAbility);
		SendRPCInternal(networkWriter, 0, "RpcUpdateRemainingMovement");
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		bool result = default(bool);
		return result;
	}

	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
	}
}

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

	private static int kCmdCmdDebugTeleportRequest = -1583259838;
	private static int kCmdCmdPickedRespawnRequest = 1763304984;
	private static int kCmdCmdSendMinimapPing = -810618818;
	private static int kCmdCmdSendAbilityPing = -963392189;
	private static int kCmdCmdSelectAbilityRequest = -1183646894;
	private static int kCmdCmdQueueSimpleActionRequest = -797856057;
	private static int kCmdCmdCustomGamePause = 983951586;
	private static int kRpcRpcUpdateRemainingMovement = 64425877;

	static ActorController()
	{
		RegisterCommandDelegate(typeof(ActorController), kCmdCmdDebugTeleportRequest, InvokeCmdCmdDebugTeleportRequest);
		RegisterCommandDelegate(typeof(ActorController), kCmdCmdPickedRespawnRequest, InvokeCmdCmdPickedRespawnRequest);
		RegisterCommandDelegate(typeof(ActorController), kCmdCmdSendMinimapPing, InvokeCmdCmdSendMinimapPing);
		RegisterCommandDelegate(typeof(ActorController), kCmdCmdSendAbilityPing, InvokeCmdCmdSendAbilityPing);
		RegisterCommandDelegate(typeof(ActorController), kCmdCmdSelectAbilityRequest, InvokeCmdCmdSelectAbilityRequest);
		RegisterCommandDelegate(typeof(ActorController), kCmdCmdQueueSimpleActionRequest, InvokeCmdCmdQueueSimpleActionRequest);
		RegisterCommandDelegate(typeof(ActorController), kCmdCmdCustomGamePause, InvokeCmdCmdCustomGamePause);
		RegisterRpcDelegate(typeof(ActorController), kRpcRpcUpdateRemainingMovement, InvokeRpcRpcUpdateRemainingMovement);
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
		if (Camera.main && m_actor == GameFlowData.Get().activeOwnedActorData)
		{
			CameraManager.Get().SetTargetObject(gameObject, CameraManager.CameraTargetReason.ClientActorRespawned);
		}
	}

	private void HandlePickRespawnInput()
	{
		ActorData actor = m_actor;
		if (actor == GameFlowData.Get().activeOwnedActorData
			&& (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
			&& InterfaceManager.Get().ShouldHandleMouseClick())
		{
			BoardSquare playerClampedSquare = Board.Get().PlayerClampedSquare;
			bool flag = actor.respawnSquares.Contains(playerClampedSquare);
			if (playerClampedSquare != null && flag)
			{
				CallCmdPickedRespawnRequest(playerClampedSquare.x, playerClampedSquare.y);
				actor.ShowRespawnFlare(playerClampedSquare, false);
			}
		}
	}

	private void HandleDebugTeleport()
	{
		ActorData actor = m_actor;
		if (actor == GameFlowData.Get().activeOwnedActorData)
		{
			BoardSquare playerFreeSquare = Board.Get().PlayerFreeSquare;
			if (playerFreeSquare != null && playerFreeSquare.IsValidForGameplay())
			{
				bool flag = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
				bool flag2 = Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt);
				if (flag && flag2 && Input.GetMouseButtonUp(2))
				{
					if (InterfaceManager.Get().ShouldHandleMouseClick())
					{
						CallCmdDebugTeleportRequest(playerFreeSquare.x, playerFreeSquare.y);
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
		if (m_canMoveToHighlight)
		{
			HighlightUtils.DestroyBoundaryHighlightObject(m_canMoveToHighlight);
		}
		if (m_canMoveToWithAbilityHighlight)
		{
			HighlightUtils.DestroyBoundaryHighlightObject(m_canMoveToWithAbilityHighlight);
		}
	}

	private void ClearTargetingHighlights()
	{
		m_currentTargetingSquares.Clear();
		if (m_targetingHighlight)
		{
			HighlightUtils.DestroyBoundaryHighlightObject(m_targetingHighlight);
			m_targetingHighlight = null;
		}
		m_lastTargetedAbility = null;
		m_lastTargetIndex = -1;
	}

	private void ClearRespawnHighlights()
	{
		m_currentRespawnSquares.Clear();
		if (m_respawnHighlight)
		{
			HighlightUtils.DestroyBoundaryHighlightObject(m_respawnHighlight);
			m_respawnHighlight = null;
		}
	}

	private void Update()
	{
		ActorData actor = m_actor;
		if (actor == GameFlowData.Get().activeOwnedActorData)
		{
			ActorTurnSM actorTurnSM = actor.GetActorTurnSM();
			if (actorTurnSM.CanPickRespawnLocation())
			{
				HandlePickRespawnInput();
			}
			else if (actorTurnSM.CurrentState == TurnStateEnum.DECIDING)
			{
				HandleDebugTeleport();
			}
		}
	}

	public void SetMovementDistanceLinesVisible(bool visible)
	{
		m_movementLinesVisible = visible;
		if (m_canMoveToHighlight != null)
		{
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
			return m_actor.GetActorMovement().SquaresCanMoveTo;
		}
		if (actorTurnSM.AmTargetingAction())
		{
			return m_currentTargetingSquares;
		}
		if (actorTurnSM.CurrentState == TurnStateEnum.PICKING_RESPAWN)
		{
			return m_currentRespawnSquares;
		}
		return null;
	}

	public void RecalcAndHighlightValidSquares()
	{
		AbilityData abilityData = m_actor.GetAbilityData();
		ActorMovement actorMovement = m_actor.GetActorMovement();
		m_canMoveToSquaresScratch.Clear();
		m_canMoveToWithQueuedAbilityScratch.Clear();
		m_targetingSquaresScratch.Clear();
		bool flag = false;
		bool flag2 = false;
		bool amDecidingMovement = m_actor.GetActorTurnSM().AmDecidingMovement();
		bool isTargetingAction = m_actor.GetActorTurnSM().CurrentState == TurnStateEnum.TARGETING_ACTION;
		bool markedForUpdateValidSquares = Board.Get().MarkedForUpdateValidSquares;
		bool hasPostAbilityMovementChange = m_actor.GetAbilityMovementCost() > 0f;
		bool hasQueuedMovementAdjust = abilityData.GetQueuedAbilitiesMovementAdjust() < 0f;
		if (amDecidingMovement)
		{
			if (markedForUpdateValidSquares)
			{
				if (hasPostAbilityMovementChange && !hasQueuedMovementAdjust)
				{
					foreach (BoardSquare current in actorMovement.SquaresCanMoveTo)
					{
						m_canMoveToSquaresScratch.Add(current);
					}
				}
				if (!actorMovement.SquaresCanMoveToWithQueuedAbility.SetEquals(actorMovement.SquaresCanMoveTo)
					|| !hasPostAbilityMovementChange
					|| hasQueuedMovementAdjust)
				{
					foreach (BoardSquare current2 in actorMovement.SquaresCanMoveToWithQueuedAbility)
					{
						m_canMoveToWithQueuedAbilityScratch.Add(current2);
					}
				}
				Board.Get().MarkForUpdateValidSquares(false);
			}
			else
			{
				flag = true;
			}
			ClearTargetingHighlights();
		}
		else if (isTargetingAction)
		{
			Ability selectedAbility = abilityData.GetSelectedAbility();
			int targetSelectionIndex = m_actor.GetActorTurnSM().GetTargetSelectionIndex();
			if (selectedAbility == m_lastTargetedAbility && targetSelectionIndex == m_lastTargetIndex)
			{
				flag2 = true;
			}
			else
			{
				m_targetingSquaresScratch = AbilityUtils.GetTargetableSquaresForAbility(selectedAbility, abilityData, m_actor, targetSelectionIndex);
				m_lastTargetedAbility = selectedAbility;
				m_lastTargetIndex = targetSelectionIndex;
			}
		}

		if (!flag && m_currentCanMoveToSquares != m_canMoveToSquaresScratch && !m_currentCanMoveToSquares.SetEquals(m_canMoveToSquaresScratch)
			|| !flag && m_currentCanMoveToWithAbilitySquares != m_canMoveToWithQueuedAbilityScratch && !m_currentCanMoveToWithAbilitySquares.SetEquals(m_canMoveToWithQueuedAbilityScratch)
			|| !flag2 && m_currentTargetingSquares != m_targetingSquaresScratch && !m_currentTargetingSquares.SetEquals(m_targetingSquaresScratch))
		{
			if (amDecidingMovement)
			{
				if (m_canMoveToWithAbilityHighlight)
				{
					HighlightUtils.DestroyBoundaryHighlightObject(m_canMoveToWithAbilityHighlight);
				}
				m_canMoveToWithAbilityHighlight = HighlightUtils.Get().CreateBoundaryHighlight(m_canMoveToWithQueuedAbilityScratch, BoardSquare.s_moveableHighlightColor, true);
				if (m_canMoveToWithAbilityHighlight)
				{
					m_canMoveToWithAbilityHighlight.AddComponent<HighlightParent>();
				}
				if (m_canMoveToHighlight)
				{
					HighlightUtils.DestroyBoundaryHighlightObject(m_canMoveToHighlight);
				}
				m_canMoveToHighlight = HighlightUtils.Get().CreateBoundaryHighlight(m_canMoveToSquaresScratch, BoardSquare.s_moveableHighlightColor);
				if (m_canMoveToHighlight)
				{
					m_canMoveToHighlight.AddComponent<HighlightParent>();
				}
			}
			else
			{
				HashSet<BoardSquare> hashSet = new HashSet<BoardSquare>();
				if (m_actor.GetCurrentBoardSquare() != null)
				{
					hashSet.Add(m_actor.GetCurrentBoardSquare());
					if (m_lastTargetedAbility != null && m_lastTargetedAbility.Targeters != null)
					{
						foreach (AbilityUtil_Targeter current3 in m_lastTargetedAbility.Targeters)
						{
							if (current3 != null)
							{
								BoardSquare boardSquareSafe2 = Board.Get().GetSquare(current3.LastUpdatingGridPos);
								if (boardSquareSafe2 != null)
								{
									hashSet.Add(boardSquareSafe2);
								}
							}
						}
					}
				}
				if (m_targetingHighlight)
				{
					HighlightUtils.DestroyBoundaryHighlightObject(m_targetingHighlight);
				}
				m_targetingHighlight = HighlightUtils.Get().CreateBoundaryHighlight(m_targetingSquaresScratch, BoardSquare.s_targetableByAbilityHighlightColor, false, hashSet);
				if (m_targetingHighlight)
				{
					m_targetingHighlight.AddComponent<HighlightParent>();
				}
			}
			if (!flag)
			{
				CopyOverHashsetValues(m_currentCanMoveToSquares, m_canMoveToSquaresScratch);
				CopyOverHashsetValues(m_currentCanMoveToWithAbilitySquares, m_canMoveToWithQueuedAbilityScratch);
			}
			if (!flag2)
			{
				CopyOverHashsetValues(m_currentTargetingSquares, m_targetingSquaresScratch);
			}
		}
		bool flag7 = GameFlowData.Get().IsInDecisionState()
			&& m_actor.IsDead()
			&& SpawnPointManager.Get() != null
			&& SpawnPointManager.Get().m_playersSelectRespawn;
		List<BoardSquare> respawnSquares = m_actor.respawnSquares;
		if (flag7 && m_actor.IsDead() && !respawnSquares.IsNullOrEmpty())
		{
			if (m_currentRespawnSquares.Count != respawnSquares.Count || !respawnSquares.TrueForAll((BoardSquare s) => m_currentRespawnSquares.Contains(s)))
			{
				ClearRespawnHighlights();
				m_respawnHighlight = HighlightUtils.Get().CreateBoundaryHighlight(respawnSquares, BoardSquare.s_respawnOptionHighlightColor, true);
				if (m_respawnHighlight)
				{
					m_respawnHighlight.AddComponent<HighlightParent>();
				}
				m_currentRespawnSquares.Clear();
				m_currentRespawnSquares = new HashSet<BoardSquare>(respawnSquares);
			}
		}
		else
		{
			ClearRespawnHighlights();
		}
		if (m_canMoveToWithAbilityHighlight)
		{
			bool flag8 = amDecidingMovement && m_currentCanMoveToWithAbilitySquares.Count > 0 && m_movementLinesVisible;
			if (m_canMoveToWithAbilityHighlight.gameObject.activeSelf != flag8)
			{
				m_canMoveToWithAbilityHighlight.gameObject.SetActive(flag8);
			}
		}
		if (m_canMoveToHighlight)
		{
			bool flag9 = amDecidingMovement && m_currentCanMoveToSquares.Count > 0 && m_movementLinesVisible;
			if (flag9 && !FirstTurnMovement.ForceShowSprintRange(m_actor))
			{
				Vector3 position = HighlightUtils.Get().MovementMouseOverCursor.transform.position;
				BoardSquare boardSquareSafe = Board.Get().GetSquareFromPos(position.x, position.z);
				if (m_canMoveToWithQueuedAbilityScratch.Contains(boardSquareSafe))
				{
					flag9 = false;
				}
			}
			if (m_canMoveToHighlight.gameObject.activeSelf != flag9)
			{
				m_canMoveToHighlight.gameObject.SetActive(flag9);
			}
		}
		if (m_targetingHighlight)
		{
			bool flag10 = !amDecidingMovement && m_currentTargetingSquares.Count > 0;
			if (m_targetingHighlight.gameObject.activeSelf != flag10)
			{
				m_targetingHighlight.gameObject.SetActive(flag10);
			}
		}
	}

	private void CopyOverHashsetValues(HashSet<BoardSquare> toSet, HashSet<BoardSquare> fromSet)
	{
		toSet.Clear();
		foreach (BoardSquare square in fromSet)
		{
			toSet.Add(square);
		}
	}

	public void SendSelectAbilityRequest()
	{
		AbilityData.ActionType actionTypeInt = AbilityData.ActionType.INVALID_ACTION;
		AbilityData component = GetComponent<AbilityData>();
		UISounds.GetUISounds().Play("ui/ingame/v1/hud/ability_select");
		if (component)
		{
			actionTypeInt = component.GetSelectedActionType();
		}
		CallCmdSelectAbilityRequest((int)actionTypeInt);
	}

	internal void ShowOvercon(int overconId, bool allyOnly)
	{
		GameFlowData gameFlowData = GameFlowData.Get();
		ActorData activeOwnedActorData = gameFlowData != null ? gameFlowData.activeOwnedActorData : null;
		if (m_actor != null
			&& activeOwnedActorData != null
			&& m_actor.IsActorVisibleToClient()
			&& HUD_UI.Get() != null
			&& UIOverconData.Get() != null)
		{
			UIOverconData.NameToOverconEntry overconEntryById = UIOverconData.Get().GetOverconEntryById(overconId);
			if (overconEntryById != null && !overconEntryById.m_isHidden)
			{
				HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.SpawnOverconForActor(m_actor, overconEntryById, false);
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
		if (actor != null && actor.GetAbilityData() != null)
		{
			actor.GetAbilityData().SetLastSelectedAbility(actor.GetAbilityData().GetAbilityOfActionType(actionType));
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
			HandleCustomGamePauseOnServer(desiredPause, requestActorIndex);
			return;
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
		if (m_actor != null
			&& GameFlowData.Get() != null
			&& GameFlowData.Get().activeOwnedActorData == m_actor)
		{
			bool flag = false;
			if (m_actor.RemainingHorizontalMovement != remainingMovement)
			{
				m_actor.RemainingHorizontalMovement = remainingMovement;
				flag = true;
			}
			if (m_actor.RemainingMovementWithQueuedAbility != remainingMovementWithQueuedAbility)
			{
				m_actor.RemainingMovementWithQueuedAbility = remainingMovementWithQueuedAbility;
				flag = true;
			}
			if (flag)
			{
				m_actor.GetActorMovement().UpdateSquaresCanMoveTo();
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
			Debug.LogError("Command CmdDebugTeleportRequest called on client.");
			return;
		}
		((ActorController)obj).CmdDebugTeleportRequest((int)reader.ReadPackedUInt32(), (int)reader.ReadPackedUInt32());
	}

	protected static void InvokeCmdCmdPickedRespawnRequest(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdPickedRespawnRequest called on client.");
			return;
		}
		((ActorController)obj).CmdPickedRespawnRequest((int)reader.ReadPackedUInt32(), (int)reader.ReadPackedUInt32());
	}

	protected static void InvokeCmdCmdSendMinimapPing(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdSendMinimapPing called on client.");
			return;
		}
		((ActorController)obj).CmdSendMinimapPing((int)reader.ReadPackedUInt32(), reader.ReadVector3(), (PingType)reader.ReadInt32());
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
			Debug.LogError("Command CmdSelectAbilityRequest called on client.");
			return;
		}
		((ActorController)obj).CmdSelectAbilityRequest((int)reader.ReadPackedUInt32());
	}

	protected static void InvokeCmdCmdQueueSimpleActionRequest(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
		{
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
		if (isServer)
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
		if (isServer)
		{
			CmdPickedRespawnRequest(x, y);
			return;
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
			Debug.LogError("Command function CmdSendMinimapPing called on server.");
			return;
		}
		if (isServer)
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
			Debug.LogError("Command function CmdSendAbilityPing called on server.");
			return;
		}
		if (isServer)
		{
			CmdSendAbilityPing(teamIndex, localizedPing);
			return;
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
		if (isServer)
		{
			CmdSelectAbilityRequest(actionTypeInt);
			return;
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
			Debug.LogError("Command function CmdQueueSimpleActionRequest called on server.");
			return;
		}
		if (isServer)
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
			Debug.LogError("Command function CmdCustomGamePause called on server.");
			return;
		}
		if (isServer)
		{
			CmdCustomGamePause(desiredPause, requestActorIndex);
			return;
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
			return;
		}
		((ActorController)obj).RpcUpdateRemainingMovement(reader.ReadSingle(), reader.ReadSingle());
	}

	public void CallRpcUpdateRemainingMovement(float remainingMovement, float remainingMovementWithQueuedAbility)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("RPC Function RpcUpdateRemainingMovement called on client.");
			return;
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
		return false;
	}

	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
	}
}

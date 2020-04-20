using System;
using UnityEngine;
using UnityEngine.Networking;

public class PickingRespawnState : TurnState
{
	public bool focusedCameraYet;

	public PickingRespawnState(ActorTurnSM masterSM) : base(masterSM)
	{
	}

	public override void OnEnter()
	{
		ActorData component = this.m_SM.GetComponent<ActorData>();
		if (component == GameFlowData.Get().activeOwnedActorData)
		{
			GameObject gameObject = null;
			if (component.RespawnPickedPositionSquare != null)
			{
				gameObject = component.RespawnPickedPositionSquare.gameObject;
			}
			else if (!component.respawnSquares.IsNullOrEmpty<BoardSquare>())
			{
				gameObject = component.respawnSquares[0].gameObject;
			}
			this.focusedCameraYet = false;
			if (gameObject != null)
			{
				CameraManager.Get().SetTargetObject(gameObject, CameraManager.CameraTargetReason.MustSelectRespawnLoc);
				this.focusedCameraYet = true;
				InterfaceManager.Get().DisplayAlert(StringUtil.TR("PickRespawnLocation", "Global"), BoardSquare.s_respawnOptionHighlightColor, 60f, true, 0);
			}
		}
	}

	public override void OnExit()
	{
		if (this.m_SM.GetComponent<ActorData>() == GameFlowData.Get().activeOwnedActorData)
		{
			InterfaceManager.Get().CancelAlert(StringUtil.TR("PickRespawnLocation", "Global"));
		}
	}

	public override void OnMsg(TurnMessage msg, int extraData)
	{
		ActorData component = this.m_SM.GetComponent<ActorData>();
		switch (msg)
		{
		case TurnMessage.BEGIN_RESOLVE:
			break;
		case TurnMessage.MOVEMENT_RESOLVED:
			Log.Error("Received a 'Movement Resolved' message in the PickingRespawn state, which is unexpected.", new object[0]);
			this.m_SM.NextState = TurnStateEnum.WAITING;
			return;
		case TurnMessage.CLIENTS_RESOLVED_ABILITIES:
			if (NetworkServer.active)
			{
				Log.Error("Received a 'CLIENTS_RESOLVED_ABILITIES' message in the PickingRespawn state, which is unexpected.", new object[0]);
			}
			else
			{
				Log.Warning("Received a 'CLIENTS_RESOLVED_ABILITIES' message in the PickingRespawn state, which is unexpected.", new object[0]);
			}
			this.m_SM.NextState = TurnStateEnum.WAITING;
			return;
		case TurnMessage.CANCEL_BUTTON_CLICKED:
			component.RespawnPickedPositionSquare = null;
			if (this.m_SM.GetComponent<ActorData>() == GameFlowData.Get().activeOwnedActorData)
			{
				InterfaceManager.Get().DisplayAlert(StringUtil.TR("PickRespawnLocation", "Global"), BoardSquare.s_respawnOptionHighlightColor, 60f, true, 0);
			}
			return;
		default:
			if (msg != TurnMessage.DISCONNECTED)
			{
				return;
			}
			break;
		case TurnMessage.RESPAWN:
			this.m_SM.NextState = TurnStateEnum.DECIDING;
			return;
		case TurnMessage.PICK_RESPAWN:
			if (this.m_SM.GetComponent<ActorData>() == GameFlowData.Get().activeOwnedActorData)
			{
				InterfaceManager.Get().CancelAlert(StringUtil.TR("PickRespawnLocation", "Global"));
			}
			return;
		case TurnMessage.DONE_BUTTON_CLICKED:
			this.m_SM.NextState = TurnStateEnum.CONFIRMED;
			if (component.RespawnPickedPositionSquare == null && !component.respawnSquares.IsNullOrEmpty<BoardSquare>())
			{
				component.RespawnPickedPositionSquare = component.respawnSquares[0];
			}
			return;
		}
		this.m_SM.NextState = TurnStateEnum.WAITING;
		if (component.RespawnPickedPositionSquare == null && !component.respawnSquares.IsNullOrEmpty<BoardSquare>())
		{
			component.RespawnPickedPositionSquare = component.respawnSquares[0];
		}
	}

	public override void Update()
	{
		ActorData component = this.m_SM.GetComponent<ActorData>();
		if (!this.focusedCameraYet)
		{
			if (component == GameFlowData.Get().activeOwnedActorData)
			{
				GameObject gameObject = null;
				if (component.RespawnPickedPositionSquare != null)
				{
					gameObject = component.RespawnPickedPositionSquare.gameObject;
				}
				else if (!component.respawnSquares.IsNullOrEmpty<BoardSquare>())
				{
					gameObject = component.respawnSquares[0].gameObject;
				}
				if (gameObject != null)
				{
					CameraManager.Get().SetTargetObject(gameObject, CameraManager.CameraTargetReason.MustSelectRespawnLoc);
					this.focusedCameraYet = true;
					InterfaceManager.Get().DisplayAlert(StringUtil.TR("PickRespawnLocation", "Global"), BoardSquare.s_respawnOptionHighlightColor, 60f, true, 0);
				}
			}
		}
		if (component.NextRespawnTurn <= GameFlowData.Get().CurrentTurn)
		{
			this.m_SM.NextState = TurnStateEnum.DECIDING;
		}
		this.m_SM.UpdateEndTurnKey();
	}
}

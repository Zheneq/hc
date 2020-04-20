using System;
using UnityEngine;
using UnityEngine.Networking;

public class ConfirmedState : TurnState
{
	public ConfirmedState(ActorTurnSM masterSM) : base(masterSM)
	{
	}

	public override void OnEnter()
	{
		if (HUD_UI.Get() != null)
		{
			if (this.m_SM.GetComponent<ActorData>() == GameFlowData.Get().activeOwnedActorData)
			{
				HUD_UI.Get().m_mainScreenPanel.m_notificationPanel.DisplayNotification(UINotificationPanel.GamePhaseDisplay.LockedIn);
			}
		}
		if (this.m_SM)
		{
			this.m_SM.OnActionsConfirmed();
		}
	}

	public override void OnMsg(TurnMessage msg, int extraData)
	{
		if (msg == TurnMessage.TURN_START)
		{
			ActorData component = this.m_SM.GetComponent<ActorData>();
			if (component != null && !component.IsDead())
			{
				Log.Error(component.GetDebugName() + " handling TURN_START message in Confirmed state", new object[0]);
				this.m_SM.SetupForNewTurn();
				this.m_SM.NextState = TurnStateEnum.DECIDING;
			}
		}
		else if (msg == TurnMessage.BEGIN_RESOLVE)
		{
			this.m_SM.NextState = TurnStateEnum.RESOLVING;
		}
		else if (msg == TurnMessage.CANCEL_BUTTON_CLICKED)
		{
			if (this.m_SM)
			{
				this.m_SM.OnActionsUnconfirmed();
			}
			ActorData component2 = this.m_SM.GetComponent<ActorData>();
			if (component2.RespawnPickedPositionSquare != null)
			{
				if (!component2.ShouldPickRespawn_zq())
				{
					component2.RespawnPickedPositionSquare = null;
					this.m_SM.NextState = TurnStateEnum.PICKING_RESPAWN;
					goto IL_131;
				}
			}
			this.m_SM.NextState = TurnStateEnum.DECIDING;
			IL_131:;
		}
		else if (msg == TurnMessage.MOVEMENT_ACCEPTED)
		{
			ActorData component3 = this.m_SM.GetComponent<ActorData>();
			if (component3.GetTimeBank().AllowUnconfirm())
			{
				this.m_SM.OnActionsUnconfirmed();
				this.m_SM.NextState = TurnStateEnum.DECIDING;
			}
		}
		else if (msg == TurnMessage.PICK_RESPAWN)
		{
			this.m_SM.NextState = TurnStateEnum.PICKING_RESPAWN;
		}
		else if (msg == TurnMessage.RESPAWN)
		{
			this.m_SM.NextState = TurnStateEnum.RESPAWNING;
		}
		else if (msg == TurnMessage.MOVEMENT_RESOLVED)
		{
			Log.Error(this.m_SM.GetComponent<ActorData>().DisplayName + "Received a 'Movement Resolved' message in the Confirmed state, which is unexpected.", new object[0]);
			this.m_SM.NextState = TurnStateEnum.WAITING;
		}
		else if (msg == TurnMessage.CLIENTS_RESOLVED_ABILITIES)
		{
			if (NetworkServer.active)
			{
				Log.Error(this.m_SM.GetComponent<ActorData>().DisplayName + "Received a 'CLIENTS_RESOLVED_ABILITIES' message in the Confirmed state, which is unexpected.", new object[0]);
			}
			else
			{
				Log.Warning(this.m_SM.GetComponent<ActorData>().DisplayName + "Received a 'CLIENTS_RESOLVED_ABILITIES' message in the Confirmed state, which is unexpected.", new object[0]);
			}
			this.m_SM.NextState = TurnStateEnum.WAITING;
		}
	}

	public override void OnExit()
	{
	}

	public override void Update()
	{
		ActorData component = this.m_SM.GetComponent<ActorData>();
		if (GameFlowData.Get().activeOwnedActorData == component && GameFlowData.Get().gameState != GameState.EndingGame)
		{
			if (Input.GetMouseButtonUp(1))
			{
				if (InterfaceManager.Get().ShouldHandleMouseClick() && !this.m_SM.HandledMouseInput)
				{
					if (component.GetTimeBank().AllowUnconfirm())
					{
						this.m_SM.OnActionsUnconfirmed();
						this.m_SM.HandledMouseInput = true;
						this.m_SM.SelectMovementSquare();
					}
					return;
				}
			}
			if (InputManager.Get().IsKeyBindingNewlyHeld(KeyPreference.LockIn))
			{
				if (!this.m_SM.HandledSpaceInput)
				{
					this.m_SM.HandledSpaceInput = true;
					this.m_SM.RequestCancel(true);
				}
			}
		}
	}
}

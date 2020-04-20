using System;
using UnityEngine;
using UnityEngine.Networking;

public class DecidingMovementState : TurnState
{
	public DecidingMovementState(ActorTurnSM masterSM) : base(masterSM)
	{
	}

	public override void OnEnter()
	{
		AbilityData component = this.m_SM.GetComponent<AbilityData>();
		if (component)
		{
			component.ClearSelectedAbility();
		}
		ActorMovement component2 = this.m_SM.GetComponent<ActorMovement>();
		if (component2)
		{
			component2.UpdateSquaresCanMoveTo();
		}
	}

	public override void OnMsg(TurnMessage msg, int extraData)
	{
		switch (msg)
		{
		case TurnMessage.BEGIN_RESOLVE:
			this.m_SM.NextState = TurnStateEnum.RESOLVING;
			break;
		case TurnMessage.MOVEMENT_RESOLVED:
			Log.Error(this.m_SM.GetComponent<ActorData>().DisplayName + "Received a 'Movement Resolved' message in the DecidingMovement state, which is unexpected.", new object[0]);
			this.m_SM.NextState = TurnStateEnum.WAITING;
			break;
		case TurnMessage.CLIENTS_RESOLVED_ABILITIES:
			if (NetworkServer.active)
			{
				Log.Error(this.m_SM.GetComponent<ActorData>().DisplayName + "Received a 'CLIENTS_RESOLVED_ABILITIES' message in the DecidingMovement state, which is unexpected.", new object[0]);
			}
			else
			{
				Log.Warning(this.m_SM.GetComponent<ActorData>().DisplayName + "Received a 'CLIENTS_RESOLVED_ABILITIES' message in the DecidingMovement state, which is unexpected.", new object[0]);
			}
			this.m_SM.NextState = TurnStateEnum.WAITING;
			break;
		case TurnMessage.SELECTED_ABILITY:
			this.m_SM.NextState = TurnStateEnum.TARGETING_ACTION;
			break;
		case TurnMessage.RESPAWN:
			this.m_SM.NextState = TurnStateEnum.RESPAWNING;
			break;
		case TurnMessage.MOVE_BUTTON_CLICKED:
			this.m_SM.NextState = TurnStateEnum.DECIDING;
			break;
		case TurnMessage.DONE_BUTTON_CLICKED:
			this.m_SM.NextState = TurnStateEnum.CONFIRMED;
			if (SinglePlayerManager.Get())
			{
				SinglePlayerManager.Get().OnActorLockInEntered(this.m_SM.GetComponent<ActorData>());
			}
			break;
		case TurnMessage.DISCONNECTED:
			this.m_SM.NextState = TurnStateEnum.CONFIRMED;
			break;
		}
	}

	public override void Update()
	{
		if (GameFlowData.Get().activeOwnedActorData != null)
		{
			if (GameFlowData.Get().activeOwnedActorData.GetComponent<ActorTurnSM>() == this.m_SM)
			{
				if (!Input.GetMouseButtonUp(0))
				{
					if (!Input.GetMouseButtonUp(1))
					{
						goto IL_B4;
					}
				}
				if (InterfaceManager.Get().ShouldHandleMouseClick() && !this.m_SM.HandledMouseInput)
				{
					this.m_SM.HandledMouseInput = true;
					this.m_SM.SelectMovementSquare();
				}
			}
		}
		IL_B4:
		this.m_SM.UpdateEndTurnKey();
	}
}

using UnityEngine;
using UnityEngine.Networking;

public class DecidingMovementState : TurnState
{
	public DecidingMovementState(ActorTurnSM masterSM)
		: base(masterSM)
	{
	}

	public override void OnEnter()
	{
		AbilityData component = m_SM.GetComponent<AbilityData>();
		if ((bool)component)
		{
			component.ClearSelectedAbility();
		}
		ActorMovement component2 = m_SM.GetComponent<ActorMovement>();
		if ((bool)component2)
		{
			component2.UpdateSquaresCanMoveTo();
		}
	}

	public override void OnMsg(TurnMessage msg, int extraData)
	{
		switch (msg)
		{
		case TurnMessage.CANCEL_BUTTON_CLICKED:
		case TurnMessage.ABILITY_REQUEST_REJECTED:
		case TurnMessage.MOVEMENT_ACCEPTED:
		case TurnMessage.MOVEMENT_REJECTED:
		case TurnMessage.PICK_RESPAWN:
		case TurnMessage.PICKED_RESPAWN:
		case TurnMessage.CANCEL_SINGLE_ABILITY:
		case TurnMessage.CANCEL_MOVEMENT:
			break;
		case TurnMessage.ABILITY_REQUEST_ACCEPTED:
			break;
		case TurnMessage.BEGIN_RESOLVE:
			m_SM.NextState = TurnStateEnum.RESOLVING;
			break;
		case TurnMessage.SELECTED_ABILITY:
			m_SM.NextState = TurnStateEnum.TARGETING_ACTION;
			break;
		case TurnMessage.RESPAWN:
			m_SM.NextState = TurnStateEnum.RESPAWNING;
			break;
		case TurnMessage.MOVEMENT_RESOLVED:
			Log.Error(m_SM.GetComponent<ActorData>().DisplayName + "Received a 'Movement Resolved' message in the DecidingMovement state, which is unexpected.");
			m_SM.NextState = TurnStateEnum.WAITING;
			break;
		case TurnMessage.CLIENTS_RESOLVED_ABILITIES:
			if (NetworkServer.active)
			{
				Log.Error(m_SM.GetComponent<ActorData>().DisplayName + "Received a 'CLIENTS_RESOLVED_ABILITIES' message in the DecidingMovement state, which is unexpected.");
			}
			else
			{
				Log.Warning(m_SM.GetComponent<ActorData>().DisplayName + "Received a 'CLIENTS_RESOLVED_ABILITIES' message in the DecidingMovement state, which is unexpected.");
			}
			m_SM.NextState = TurnStateEnum.WAITING;
			break;
		case TurnMessage.DISCONNECTED:
			m_SM.NextState = TurnStateEnum.CONFIRMED;
			break;
		case TurnMessage.MOVE_BUTTON_CLICKED:
			m_SM.NextState = TurnStateEnum.DECIDING;
			break;
		case TurnMessage.DONE_BUTTON_CLICKED:
			m_SM.NextState = TurnStateEnum.CONFIRMED;
			if (!SinglePlayerManager.Get())
			{
				break;
			}
			while (true)
			{
				SinglePlayerManager.Get().OnActorLockInEntered(m_SM.GetComponent<ActorData>());
				return;
			}
		}
	}

	public override void Update()
	{
		if (GameFlowData.Get().activeOwnedActorData != null)
		{
			if (GameFlowData.Get().activeOwnedActorData.GetComponent<ActorTurnSM>() == m_SM)
			{
				if (!Input.GetMouseButtonUp(0))
				{
					if (!Input.GetMouseButtonUp(1))
					{
						goto IL_00b4;
					}
				}
				if (InterfaceManager.Get().ShouldHandleMouseClick() && !m_SM.HandledMouseInput)
				{
					m_SM.HandledMouseInput = true;
					m_SM.SelectMovementSquare();
				}
			}
		}
		goto IL_00b4;
		IL_00b4:
		m_SM.UpdateEndTurnKey();
	}
}

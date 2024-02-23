using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class DecidingState : TurnState
{
	public DecidingState(ActorTurnSM masterSM)
		: base(masterSM)
	{
	}

	public override void OnEnter()
	{
		ActorMovement component = m_SM.GetComponent<ActorMovement>();
		component.OnGameStateChange(true);
		component.ResetChargeCycleFlag();
		AbilityData component2 = m_SM.GetComponent<AbilityData>();
		if ((bool)component2)
		{
			component2.ClearSelectedAbility();
		}
		ActorData component3 = m_SM.GetComponent<ActorData>();
		if (!component3.IsDead())
		{
			return;
		}
		if (SpawnPointManager.Get().m_playersSelectRespawn && component3.NextRespawnTurn > GameFlowData.Get().CurrentTurn)
		{
			if (component3.RespawnPickedPositionSquare == null)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
						m_SM.NextState = TurnStateEnum.PICKING_RESPAWN;
						return;
					}
				}
			}
		}
		m_SM.NextState = TurnStateEnum.CONFIRMED;
	}

	public override void OnMsg(TurnMessage msg, int extraData)
	{
		AbilityData component = m_SM.GetComponent<AbilityData>();
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
			if (!(component != null))
			{
				break;
			}
			while (true)
			{
				if (component.GetSelectedAbility() != null)
				{
					m_SM.NextState = TurnStateEnum.TARGETING_ACTION;
				}
				return;
			}
		case TurnMessage.RESPAWN:
			m_SM.NextState = TurnStateEnum.RESPAWNING;
			break;
		case TurnMessage.MOVEMENT_RESOLVED:
			Log.Error(new StringBuilder().Append(m_SM.GetComponent<ActorData>().DisplayName).Append("Received a 'Movement Resolved' message in the Deciding state, which is unexpected.").ToString());
			break;
		case TurnMessage.CLIENTS_RESOLVED_ABILITIES:
			if (NetworkServer.active)
			{
				Log.Error(new StringBuilder().Append(m_SM.GetComponent<ActorData>().DisplayName).Append("Received a 'CLIENTS_RESOLVED_ABILITIES' message in the Deciding state, which is unexpected.").ToString());
			}
			else
			{
				Log.Error(new StringBuilder().Append(m_SM.GetComponent<ActorData>().DisplayName).Append("Received a 'CLIENTS_RESOLVED_ABILITIES' message in the Deciding state, which is unexpected.").ToString());
			}
			m_SM.NextState = TurnStateEnum.WAITING;
			break;
		case TurnMessage.DISCONNECTED:
			m_SM.NextState = TurnStateEnum.CONFIRMED;
			break;
		case TurnMessage.MOVE_BUTTON_CLICKED:
			m_SM.NextState = TurnStateEnum.DECIDING_MOVEMENT;
			break;
		case TurnMessage.DONE_BUTTON_CLICKED:
			if (NetworkServer.active)
			{
				if (SinglePlayerManager.Get() != null)
				{
					SinglePlayerManager.Get().RecalcCanEndTurn();
				}
			}
			if (!SinglePlayerManager.CanEndTurn(m_SM.GetComponent<ActorData>()))
			{
				break;
			}
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
		ActorData component = m_SM.GetComponent<ActorData>();
		if (GameFlowData.Get().activeOwnedActorData == component)
		{
			if (GameFlowData.Get().gameState != GameState.EndingGame)
			{
				if (!Input.GetMouseButtonUp(1))
				{
					if (!InputManager.Get().GetAcceptButtonDown())
					{
						goto IL_00b7;
					}
				}
				if (InterfaceManager.Get().ShouldHandleMouseClick())
				{
					if (!m_SM.HandledMouseInput)
					{
						m_SM.HandledMouseInput = true;
						m_SM.SelectMovementSquare();
					}
				}
			}
		}
		goto IL_00b7;
		IL_00b7:
		m_SM.UpdateEndTurnKey();
	}
}

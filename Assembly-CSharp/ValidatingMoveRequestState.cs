using UnityEngine.Networking;

public class ValidatingMoveRequestState : TurnState
{
	public ValidatingMoveRequestState(ActorTurnSM masterSM)
		: base(masterSM)
	{
	}

	public override void Update()
	{
		m_SM.UpdateEndTurnKey();
	}

	public override void OnMsg(TurnMessage msg, int extraData)
	{
		switch (msg)
		{
		case TurnMessage.CANCEL_BUTTON_CLICKED:
		case TurnMessage.SELECTED_ABILITY:
		case TurnMessage.ABILITY_REQUEST_ACCEPTED:
		case TurnMessage.ABILITY_REQUEST_REJECTED:
			break;
		case TurnMessage.BEGIN_RESOLVE:
			m_SM.NextState = TurnStateEnum.RESOLVING;
			break;
		case TurnMessage.MOVEMENT_ACCEPTED:
			if (m_SM.PreviousState == TurnStateEnum.DECIDING_MOVEMENT)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						m_SM.NextState = TurnStateEnum.DECIDING_MOVEMENT;
						return;
					}
				}
			}
			m_SM.NextState = TurnStateEnum.DECIDING;
			break;
		case TurnMessage.MOVEMENT_REJECTED:
			m_SM.NextState = TurnStateEnum.DECIDING;
			break;
		case TurnMessage.MOVEMENT_RESOLVED:
			Log.Error(m_SM.GetComponent<ActorData>().DisplayName + "Received a 'Movement Resolved' message in the ValidatingMoveRequest state, which is unexpected.");
			m_SM.NextState = TurnStateEnum.WAITING;
			break;
		case TurnMessage.CLIENTS_RESOLVED_ABILITIES:
			if (NetworkServer.active)
			{
				Log.Error(m_SM.GetComponent<ActorData>().DisplayName + "Received a 'CLIENTS_RESOLVED_ABILITIES' message in the ValidatingMoveRequest state, which is unexpected.");
			}
			else
			{
				Log.Warning(m_SM.GetComponent<ActorData>().DisplayName + "Received a 'CLIENTS_RESOLVED_ABILITIES' message in the ValidatingMoveRequest state, which is unexpected.");
			}
			m_SM.NextState = TurnStateEnum.WAITING;
			break;
		}
	}
}

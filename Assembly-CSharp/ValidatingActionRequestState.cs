using System.Text;
using UnityEngine.Networking;

public class ValidatingActionRequestState : TurnState
{
	public ValidatingActionRequestState(ActorTurnSM masterSM)
		: base(masterSM)
	{
	}

	public override void Update()
	{
		m_SM.UpdateEndTurnKey();
	}

	public override void OnMsg(TurnMessage msg, int extraData)
	{
		AbilityData component = m_SM.GetComponent<AbilityData>();
		switch (msg)
		{
		case TurnMessage.CANCEL_BUTTON_CLICKED:
		case TurnMessage.SELECTED_ABILITY:
		case TurnMessage.MOVEMENT_ACCEPTED:
		case TurnMessage.MOVEMENT_REJECTED:
			break;
		case TurnMessage.ABILITY_REQUEST_ACCEPTED:
			m_SM.NextState = TurnStateEnum.DECIDING;
			break;
		case TurnMessage.ABILITY_REQUEST_REJECTED:
			component.ClearSelectedAbility();
			m_SM.NextState = TurnStateEnum.DECIDING;
			break;
		case TurnMessage.BEGIN_RESOLVE:
			m_SM.NextState = TurnStateEnum.WAITING;
			break;
		case TurnMessage.RESPAWN:
			m_SM.NextState = TurnStateEnum.RESPAWNING;
			break;
		case TurnMessage.MOVEMENT_RESOLVED:
			Log.Error(new StringBuilder().Append(m_SM.GetComponent<ActorData>().DisplayName).Append("Received a 'Movement Resolved' message in the ValidatingActionRequest state, which is unexpected.").ToString());
			m_SM.NextState = TurnStateEnum.WAITING;
			break;
		case TurnMessage.CLIENTS_RESOLVED_ABILITIES:
			if (NetworkServer.active)
			{
				Log.Error(new StringBuilder().Append(m_SM.GetComponent<ActorData>().DisplayName).Append("Received a 'CLIENTS_RESOLVED_ABILITIES' message in the ValidatingActionRequest state, which is unexpected.").ToString());
			}
			else
			{
				Log.Warning(new StringBuilder().Append(m_SM.GetComponent<ActorData>().DisplayName).Append("Received a 'CLIENTS_RESOLVED_ABILITIES' message in the ValidatingActionRequest state, which is unexpected.").ToString());
			}
			m_SM.NextState = TurnStateEnum.WAITING;
			break;
		}
	}

	public override void OnExit()
	{
		AbilityData component = m_SM.GetComponent<AbilityData>();
		component.ClearActionsToCancelOnTargetingComplete();
	}
}

using System;
using UnityEngine.Networking;

public class ValidatingActionRequestState : TurnState
{
	public ValidatingActionRequestState(ActorTurnSM masterSM) : base(masterSM)
	{
	}

	public override void Update()
	{
		this.m_SM.UpdateEndTurnKey();
	}

	public override void OnMsg(TurnMessage msg, int extraData)
	{
		AbilityData component = this.m_SM.GetComponent<AbilityData>();
		switch (msg)
		{
		case TurnMessage.BEGIN_RESOLVE:
			this.m_SM.NextState = TurnStateEnum.WAITING;
			break;
		case TurnMessage.MOVEMENT_RESOLVED:
			Log.Error(this.m_SM.GetComponent<ActorData>().DisplayName + "Received a 'Movement Resolved' message in the ValidatingActionRequest state, which is unexpected.", new object[0]);
			this.m_SM.NextState = TurnStateEnum.WAITING;
			break;
		case TurnMessage.CLIENTS_RESOLVED_ABILITIES:
			if (NetworkServer.active)
			{
				Log.Error(this.m_SM.GetComponent<ActorData>().DisplayName + "Received a 'CLIENTS_RESOLVED_ABILITIES' message in the ValidatingActionRequest state, which is unexpected.", new object[0]);
			}
			else
			{
				Log.Warning(this.m_SM.GetComponent<ActorData>().DisplayName + "Received a 'CLIENTS_RESOLVED_ABILITIES' message in the ValidatingActionRequest state, which is unexpected.", new object[0]);
			}
			this.m_SM.NextState = TurnStateEnum.WAITING;
			break;
		case TurnMessage.ABILITY_REQUEST_ACCEPTED:
			this.m_SM.NextState = TurnStateEnum.DECIDING;
			break;
		case TurnMessage.ABILITY_REQUEST_REJECTED:
			component.ClearSelectedAbility();
			this.m_SM.NextState = TurnStateEnum.DECIDING;
			break;
		case TurnMessage.RESPAWN:
			this.m_SM.NextState = TurnStateEnum.RESPAWNING;
			break;
		}
	}

	public override void OnExit()
	{
		AbilityData component = this.m_SM.GetComponent<AbilityData>();
		component.ClearActionsToCancelOnTargetingComplete();
	}
}

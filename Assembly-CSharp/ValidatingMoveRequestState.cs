using System;
using UnityEngine.Networking;

public class ValidatingMoveRequestState : TurnState
{
	public ValidatingMoveRequestState(ActorTurnSM masterSM) : base(masterSM)
	{
	}

	public override void Update()
	{
		this.m_SM.UpdateEndTurnKey();
	}

	public override void OnMsg(TurnMessage msg, int extraData)
	{
		switch (msg)
		{
		case TurnMessage.BEGIN_RESOLVE:
			this.m_SM.NextState = TurnStateEnum.RESOLVING;
			break;
		case TurnMessage.MOVEMENT_RESOLVED:
			Log.Error(this.m_SM.GetComponent<ActorData>().DisplayName + "Received a 'Movement Resolved' message in the ValidatingMoveRequest state, which is unexpected.", new object[0]);
			this.m_SM.NextState = TurnStateEnum.WAITING;
			break;
		case TurnMessage.CLIENTS_RESOLVED_ABILITIES:
			if (NetworkServer.active)
			{
				Log.Error(this.m_SM.GetComponent<ActorData>().DisplayName + "Received a 'CLIENTS_RESOLVED_ABILITIES' message in the ValidatingMoveRequest state, which is unexpected.", new object[0]);
			}
			else
			{
				Log.Warning(this.m_SM.GetComponent<ActorData>().DisplayName + "Received a 'CLIENTS_RESOLVED_ABILITIES' message in the ValidatingMoveRequest state, which is unexpected.", new object[0]);
			}
			this.m_SM.NextState = TurnStateEnum.WAITING;
			break;
		case TurnMessage.MOVEMENT_ACCEPTED:
			if (this.m_SM.PreviousState == TurnStateEnum.DECIDING_MOVEMENT)
			{
				this.m_SM.NextState = TurnStateEnum.DECIDING_MOVEMENT;
			}
			else
			{
				this.m_SM.NextState = TurnStateEnum.DECIDING;
			}
			break;
		case TurnMessage.MOVEMENT_REJECTED:
			this.m_SM.NextState = TurnStateEnum.DECIDING;
			break;
		}
	}
}

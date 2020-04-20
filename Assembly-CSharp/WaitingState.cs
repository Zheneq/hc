using System;

public class WaitingState : TurnState
{
	public WaitingState(ActorTurnSM masterSM) : base(masterSM)
	{
	}

	public override void OnEnter()
	{
		AbilityData component = this.m_SM.GetComponent<AbilityData>();
		if (component)
		{
			component.ClearSelectedAbility();
		}
	}

	public override void OnMsg(TurnMessage msg, int extraData)
	{
		if (msg == TurnMessage.TURN_START)
		{
			this.m_SM.SetupForNewTurn();
			this.m_SM.NextState = TurnStateEnum.DECIDING;
		}
		else if (msg == TurnMessage.RESPAWN)
		{
			this.m_SM.NextState = TurnStateEnum.RESPAWNING;
		}
	}
}

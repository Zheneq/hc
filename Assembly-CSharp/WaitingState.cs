public class WaitingState : TurnState
{
	public WaitingState(ActorTurnSM masterSM)
		: base(masterSM)
	{
	}

	public override void OnEnter()
	{
		AbilityData component = m_SM.GetComponent<AbilityData>();
		if (!component)
		{
			return;
		}
		while (true)
		{
			component.ClearSelectedAbility();
			return;
		}
	}

	public override void OnMsg(TurnMessage msg, int extraData)
	{
		if (msg == TurnMessage.TURN_START)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					m_SM.SetupForNewTurn();
					m_SM.NextState = TurnStateEnum.DECIDING;
					return;
				}
			}
		}
		if (msg != TurnMessage.RESPAWN)
		{
			return;
		}
		while (true)
		{
			m_SM.NextState = TurnStateEnum.RESPAWNING;
			return;
		}
	}
}

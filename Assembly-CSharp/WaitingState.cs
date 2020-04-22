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
			switch (7)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
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
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
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
			switch (6)
			{
			case 0:
				continue;
			}
			m_SM.NextState = TurnStateEnum.RESPAWNING;
			return;
		}
	}
}

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
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(WaitingState.OnEnter()).MethodHandle;
			}
			component.ClearSelectedAbility();
		}
	}

	public override void OnMsg(TurnMessage msg, int extraData)
	{
		if (msg == TurnMessage.TURN_START)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(WaitingState.OnMsg(TurnMessage, int)).MethodHandle;
			}
			this.m_SM.SetupForNewTurn();
			this.m_SM.NextState = TurnStateEnum.DECIDING;
		}
		else if (msg == TurnMessage.RESPAWN)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			this.m_SM.NextState = TurnStateEnum.RESPAWNING;
		}
	}
}

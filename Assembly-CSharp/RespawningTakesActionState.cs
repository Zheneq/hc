using System;

public class RespawningTakesActionState : TurnState
{
	public RespawningTakesActionState(ActorTurnSM masterSM) : base(masterSM)
	{
	}

	public override void Update()
	{
		ActorData component = this.m_SM.GetComponent<ActorData>();
		if (component != null)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(RespawningTakesActionState.Update()).MethodHandle;
			}
			if (!component.\u000E())
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
				this.m_SM.NextState = TurnStateEnum.WAITING;
			}
		}
	}
}

public class RespawningTakesActionState : TurnState
{
	public RespawningTakesActionState(ActorTurnSM masterSM)
		: base(masterSM)
	{
	}

	public override void Update()
	{
		ActorData component = m_SM.GetComponent<ActorData>();
		if (!(component != null))
		{
			return;
		}
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (!component.IsDead())
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					m_SM.NextState = TurnStateEnum.WAITING;
					return;
				}
			}
			return;
		}
	}
}

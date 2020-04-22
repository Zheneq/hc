public class RespawningState : TurnState
{
	private float m_timeToWait;

	public RespawningState(ActorTurnSM masterSM)
		: base(masterSM)
	{
	}

	public override void OnEnter()
	{
		m_timeToWait = 2f;
	}

	public override void Update()
	{
		ActorData component = m_SM.GetComponent<ActorData>();
		if (component != null && !component.IsDead())
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (GameFlowData.Get().IsTeamsTurn(component.GetTeam()))
			{
				if (GameFlowData.Get().IsInDecisionState())
				{
					m_SM.SetupForNewTurn();
					m_SM.NextState = TurnStateEnum.DECIDING;
				}
				else if (GameFlowData.Get().IsInResolveState())
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					if (m_timeToWait <= 0f)
					{
						while (true)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
						m_SM.NextState = TurnStateEnum.RESOLVING;
					}
				}
				else
				{
					m_SM.NextState = TurnStateEnum.CONFIRMED;
				}
			}
			else if (m_timeToWait <= 0f)
			{
				m_SM.NextState = TurnStateEnum.WAITING;
			}
		}
		m_timeToWait -= GameTime.deltaTime;
	}
}

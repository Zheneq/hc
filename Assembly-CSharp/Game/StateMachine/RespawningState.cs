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
			if (GameFlowData.Get().IsTeamsTurn(component.GetTeam()))
			{
				if (GameFlowData.Get().IsInDecisionState())
				{
					m_SM.SetupForNewTurn();
					m_SM.NextState = TurnStateEnum.DECIDING;
				}
				else if (GameFlowData.Get().IsInResolveState())
				{
					if (m_timeToWait <= 0f)
					{
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

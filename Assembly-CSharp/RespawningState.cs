using System;

public class RespawningState : TurnState
{
	private float m_timeToWait;

	public RespawningState(ActorTurnSM masterSM) : base(masterSM)
	{
	}

	public override void OnEnter()
	{
		this.m_timeToWait = 2f;
	}

	public override void Update()
	{
		ActorData component = this.m_SM.GetComponent<ActorData>();
		if (component != null && !component.IsDead())
		{
			if (GameFlowData.Get().IsTeamsTurn(component.GetTeam()))
			{
				if (GameFlowData.Get().IsInDecisionState())
				{
					this.m_SM.SetupForNewTurn();
					this.m_SM.NextState = TurnStateEnum.DECIDING;
				}
				else if (GameFlowData.Get().IsInResolveState())
				{
					if (this.m_timeToWait <= 0f)
					{
						this.m_SM.NextState = TurnStateEnum.RESOLVING;
					}
				}
				else
				{
					this.m_SM.NextState = TurnStateEnum.CONFIRMED;
				}
			}
			else if (this.m_timeToWait <= 0f)
			{
				this.m_SM.NextState = TurnStateEnum.WAITING;
			}
		}
		this.m_timeToWait -= GameTime.deltaTime;
	}
}

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
			if (!component.IsDead())
			{
				this.m_SM.NextState = TurnStateEnum.WAITING;
			}
		}
	}
}

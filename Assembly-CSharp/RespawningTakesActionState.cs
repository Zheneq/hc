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
			if (!component.IsDead())
			{
				while (true)
				{
					m_SM.NextState = TurnStateEnum.WAITING;
					return;
				}
			}
			return;
		}
	}
}

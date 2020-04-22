public class TurnState
{
	protected ActorTurnSM m_SM;

	public TurnState(ActorTurnSM masterSM)
	{
		m_SM = masterSM;
	}

	public virtual void OnEnter()
	{
	}

	public virtual void OnExit()
	{
	}

	public virtual void OnMsg(TurnMessage msg, int extraData)
	{
	}

	public virtual void Update()
	{
	}

	public virtual void OnSelectedAbilityChanged()
	{
	}
}

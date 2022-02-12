// ROGUES
// SERVER
// rogues-only, missing in reactor
#if SERVER
public abstract class PlayerAction
{
	public bool Executed { get; private set; }

	public PlayerAction()
	{
	}

	public virtual bool ExecuteAction()
	{
		Executed = true;
		return true;
	}

	public virtual void OnExecutionComplete(bool isLastAction)
	{
	}

	public virtual void OnUpdate()
	{
	}
}
#endif

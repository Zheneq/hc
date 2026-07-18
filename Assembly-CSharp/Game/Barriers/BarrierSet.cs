// ROGUES
// SERVER

// server-only
#if SERVER
public abstract class BarrierSet
{
	public virtual bool ShouldAddGameplayHit(Barrier barrier, ActorData mover)
	{
		return true;
	}

	public virtual void OnBarrierEnd(Barrier endingBarrier)
	{
	}
}
#endif

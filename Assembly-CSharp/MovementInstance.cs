// ROGUES
// SERVER
#if SERVER
public class MovementInstance
{
	public ActorData m_mover;
	public BoardSquarePathInfo m_path;
	public bool m_groundBased;
	public bool m_canCrossBarriers;
	public bool m_wasChase;
	public bool m_willBeStealthed;

	public MovementInstance(ActorData mover, BoardSquarePathInfo path, bool groundBased, bool bWasChase, bool willBeStealthed)
	{
		m_mover = mover;
		m_path = path;
		m_groundBased = groundBased;
		m_canCrossBarriers = true;
		m_wasChase = bWasChase;
		m_willBeStealthed = willBeStealthed;
	}
}
#endif

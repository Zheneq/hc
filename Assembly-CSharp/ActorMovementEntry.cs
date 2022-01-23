public class ActorMovementEntry
{
	public enum MovementProgressState
	{
		NotStartedMovement,
		CurrentlyMoving,
		FinishedMovement
	}

	public ActorData m_actor;

	public bool m_doomed;

	public MovementProgressState m_progressState;

	public ActorMovementEntry(ActorData actor, bool doomed)
	{
		m_actor = actor;
		m_doomed = doomed;
		m_progressState = MovementProgressState.NotStartedMovement;
	}

	public override string ToString()
	{
		string debugName = m_actor.DebugNameString();
		string str = m_progressState.ToString();
		string str2 = (!m_doomed) ? string.Empty : " (doomed)";
		return debugName + ": " + str + str2;
	}
}

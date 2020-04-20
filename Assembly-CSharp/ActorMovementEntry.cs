using System;

public class ActorMovementEntry
{
	public ActorData m_actor;

	public bool m_doomed;

	public ActorMovementEntry.MovementProgressState m_progressState;

	public ActorMovementEntry(ActorData actor, bool doomed)
	{
		this.m_actor = actor;
		this.m_doomed = doomed;
		this.m_progressState = ActorMovementEntry.MovementProgressState.NotStartedMovement;
	}

	public override string ToString()
	{
		string debugName = this.m_actor.GetDebugName();
		string str = this.m_progressState.ToString();
		string str2 = (!this.m_doomed) ? string.Empty : " (doomed)";
		return debugName + ": " + str + str2;
	}

	public enum MovementProgressState
	{
		NotStartedMovement,
		CurrentlyMoving,
		FinishedMovement
	}
}

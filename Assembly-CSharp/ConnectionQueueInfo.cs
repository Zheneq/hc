using System;

[Serializable]
public class ConnectionQueueInfo
{
	public ClientAccessLevel QueueType;

	public int QueuePosition;

	public int QueueSize;

	public int QueueMultiplier;

	public int QueueEstimatedSeconds;
}

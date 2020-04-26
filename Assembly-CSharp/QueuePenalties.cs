using System;

[Serializable]
public class QueuePenalties
{
	public int QueueDodgeCount;

	public DateTime QueueDodgeBlockTimeout;

	public DateTime QueueDodgeParoleTimeout;

	public void ResetQueueDodge()
	{
		QueueDodgeCount = 0;
		QueueDodgeBlockTimeout = DateTime.MinValue;
		QueueDodgeParoleTimeout = DateTime.MinValue;
	}
}

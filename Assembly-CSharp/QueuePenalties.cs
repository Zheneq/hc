using System;

[Serializable]
public class QueuePenalties
{
	public int QueueDodgeCount;

	public DateTime QueueDodgeBlockTimeout;

	public DateTime QueueDodgeParoleTimeout;

	public void ResetQueueDodge()
	{
		this.QueueDodgeCount = 0;
		this.QueueDodgeBlockTimeout = DateTime.MinValue;
		this.QueueDodgeParoleTimeout = DateTime.MinValue;
	}
}

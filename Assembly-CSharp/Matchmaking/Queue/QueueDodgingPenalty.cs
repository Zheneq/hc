using System;
using System.Collections.Generic;

[Serializable]
public class QueueDodgingPenalty
{
	public TimeSpan ParoleTime = TimeSpan.FromHours(20.0);

	public List<TimeSpan> TimeoutPerPenalty;
}

using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

[Serializable]
public class QuestMetaData
{
	public int CompletedCount;

	public int RejectedCount;

	public int AbandonedCount;

	public int Weight;

	public List<DateTime> UtcCompletedTimes;

	public DateTime UtcCompleted;

	public DateTime? PstAbandonDate;

	[JsonIgnore]
	public DateTime UtcFirstCompleted
	{
		get
		{
			DateTime result;
			if (this.UtcCompletedTimes.IsNullOrEmpty<DateTime>())
			{
				result = DateTime.MinValue;
			}
			else
			{
				result = this.UtcCompletedTimes.FirstOrDefault<DateTime>();
			}
			return result;
		}
	}

	[JsonIgnore]
	public DateTime UtcLastCompleted
	{
		get
		{
			DateTime result;
			if (this.UtcCompletedTimes.IsNullOrEmpty<DateTime>())
			{
				result = DateTime.MinValue;
			}
			else
			{
				result = this.UtcCompletedTimes.LastOrDefault<DateTime>();
			}
			return result;
		}
	}
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

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
			if (UtcCompletedTimes.IsNullOrEmpty())
			{
				result = DateTime.MinValue;
			}
			else
			{
				result = UtcCompletedTimes.FirstOrDefault();
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
			if (UtcCompletedTimes.IsNullOrEmpty())
			{
				result = DateTime.MinValue;
			}
			else
			{
				result = UtcCompletedTimes.LastOrDefault();
			}
			return result;
		}
	}
}

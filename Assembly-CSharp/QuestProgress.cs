using System;
using System.Collections.Generic;
using Newtonsoft.Json;

[Serializable]
public class QuestProgress
{
	public int Id { get; set; }

	public Dictionary<int, int> ObjectiveProgress { get; set; }

	public Dictionary<int, DateTime> ObjectiveProgressLastDate { get; set; }

	public override string ToString()
	{
		return JsonConvert.SerializeObject(this);
	}
}

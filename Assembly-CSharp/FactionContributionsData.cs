using System;
using System.Collections.Generic;

[Serializable]
public class FactionContributionsData
{
	public string StartTime;

	public string EndTime;

	public List<FactionContributionsGameTypeData> GameTypes;
}

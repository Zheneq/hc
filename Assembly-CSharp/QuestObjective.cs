using System;
using System.Collections.Generic;

[Serializable]
public class QuestObjective
{
	public string ObjectiveText;

	public int MaxCount;

	public List<QuestTrigger> Triggers;

	public bool Hidden;

	public bool SuperHidden;
}

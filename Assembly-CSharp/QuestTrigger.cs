using System;
using System.Collections.Generic;

[Serializable]
public class QuestTrigger
{
	public QuestTriggerType TriggerType;

	public List<QuestCondition> Conditions;

	public string LogicStatement;

	public int ProgressPerActivation;

	public int RequiredSeason;

	public int RequiredChapter;

	public int MaxActivationCount;
}

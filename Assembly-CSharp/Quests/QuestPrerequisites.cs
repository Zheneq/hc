using System;
using System.Collections.Generic;

[Serializable]
public class QuestPrerequisites
{
	public int RequiredSeason;

	public int RequiredChapter;

	public List<QuestCondition> Conditions;

	public string LogicStatement;
}

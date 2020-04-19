using System;
using System.Collections.Generic;

[Serializable]
public class SeasonItemReward : SeasonReward
{
	public QuestItemReward ItemReward;

	public List<QuestCondition> Conditions;

	public string LogicStatement;
}

using System;
using System.Collections.Generic;

public class NegateLogicOpClass : LogicOpClass
{
	public LogicOpClass m_target;

	public override bool GetValue(List<bool> constantVals)
	{
		return !this.m_target.GetValue(constantVals);
	}
}

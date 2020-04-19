using System;
using System.Collections.Generic;

public class AndLogicOpClass : LogicOpClass
{
	public LogicOpClass m_left;

	public LogicOpClass m_right;

	public override bool GetValue(List<bool> constantVals)
	{
		return this.m_left.GetValue(constantVals) && this.m_right.GetValue(constantVals);
	}
}

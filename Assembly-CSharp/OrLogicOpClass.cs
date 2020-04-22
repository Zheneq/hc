using System.Collections.Generic;

public class OrLogicOpClass : LogicOpClass
{
	public LogicOpClass m_left;

	public LogicOpClass m_right;

	public override bool GetValue(List<bool> constantVals)
	{
		return m_left.GetValue(constantVals) || m_right.GetValue(constantVals);
	}
}

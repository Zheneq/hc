using System;
using System.Collections.Generic;

public class ConstantLogicOpClass : LogicOpClass
{
	public int myIndex;

	public override bool GetValue(List<bool> constantVals)
	{
		if (this.myIndex >= 0 && this.myIndex < constantVals.Count)
		{
			return constantVals[this.myIndex];
		}
		return false;
	}
}

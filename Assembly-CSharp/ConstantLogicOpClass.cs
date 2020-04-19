using System;
using System.Collections.Generic;

public class ConstantLogicOpClass : LogicOpClass
{
	public int myIndex;

	public override bool GetValue(List<bool> constantVals)
	{
		if (this.myIndex >= 0 && this.myIndex < constantVals.Count)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ConstantLogicOpClass.GetValue(List<bool>)).MethodHandle;
			}
			return constantVals[this.myIndex];
		}
		return false;
	}
}

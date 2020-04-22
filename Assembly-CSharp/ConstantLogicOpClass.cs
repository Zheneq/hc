using System.Collections.Generic;

public class ConstantLogicOpClass : LogicOpClass
{
	public int myIndex;

	public override bool GetValue(List<bool> constantVals)
	{
		if (myIndex >= 0 && myIndex < constantVals.Count)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return constantVals[myIndex];
				}
			}
		}
		return false;
	}
}

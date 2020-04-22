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
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return constantVals[myIndex];
				}
			}
		}
		return false;
	}
}

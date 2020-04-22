using System;

[Serializable]
public class AbilityModPropertyBlockingRules
{
	public enum ModOp
	{
		Ignore,
		Override
	}

	public ModOp operation;

	public BlockingRules value;

	public BlockingRules GetModifiedValue(BlockingRules input)
	{
		if (operation == ModOp.Override)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return value;
				}
			}
		}
		return input;
	}
}

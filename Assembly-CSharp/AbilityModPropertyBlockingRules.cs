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
					return value;
				}
			}
		}
		return input;
	}
}

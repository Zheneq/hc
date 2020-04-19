using System;

[Serializable]
public class AbilityModPropertyBlockingRules
{
	public AbilityModPropertyBlockingRules.ModOp operation;

	public BlockingRules value;

	public BlockingRules GetModifiedValue(BlockingRules input)
	{
		if (this.operation == AbilityModPropertyBlockingRules.ModOp.Override)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityModPropertyBlockingRules.GetModifiedValue(BlockingRules)).MethodHandle;
			}
			return this.value;
		}
		return input;
	}

	public enum ModOp
	{
		Ignore,
		Override
	}
}

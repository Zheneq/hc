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

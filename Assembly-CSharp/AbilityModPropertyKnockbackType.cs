using System;

[Serializable]
public class AbilityModPropertyKnockbackType
{
	public KnockbackType value = KnockbackType.AwayFromSource;

	public AbilityModPropertyKnockbackType.ModOp operation;

	public KnockbackType GetModifiedValue(KnockbackType input)
	{
		if (this.operation == AbilityModPropertyKnockbackType.ModOp.Override)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityModPropertyKnockbackType.GetModifiedValue(KnockbackType)).MethodHandle;
			}
			return this.value;
		}
		return input;
	}

	public void CopyValuesFrom(AbilityModPropertyKnockbackType other)
	{
		this.value = other.value;
		this.operation = other.operation;
	}

	public enum ModOp
	{
		Ignore,
		Override
	}
}

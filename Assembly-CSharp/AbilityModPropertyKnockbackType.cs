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

using System;

[Serializable]
public class AbilityModPropertyShape
{
	public AbilityAreaShape value;

	public AbilityModPropertyShape.ModOp operation;

	public AbilityAreaShape GetModifiedValue(AbilityAreaShape input)
	{
		if (this.operation == AbilityModPropertyShape.ModOp.Override)
		{
			return this.value;
		}
		return input;
	}

	public void CopyValuesFrom(AbilityModPropertyShape other)
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

using System;

[Serializable]
public class AbilityModPropertyFloat
{
	public float value;

	public AbilityModPropertyFloat.ModOp operation;

	public float GetModifiedValue(float input)
	{
		if (this.operation == AbilityModPropertyFloat.ModOp.Add)
		{
			return input + this.value;
		}
		if (this.operation == AbilityModPropertyFloat.ModOp.Override)
		{
			return this.value;
		}
		if (this.operation == AbilityModPropertyFloat.ModOp.Multiply)
		{
			return input * this.value;
		}
		return input;
	}

	public void CopyValuesFrom(AbilityModPropertyFloat other)
	{
		this.value = other.value;
		this.operation = other.operation;
	}

	public enum ModOp
	{
		Ignore,
		Add,
		Override,
		Multiply
	}
}

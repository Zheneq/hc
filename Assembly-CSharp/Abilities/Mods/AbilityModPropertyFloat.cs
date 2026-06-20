// ROGUES
// SERVER
using System;

[Serializable]
public class AbilityModPropertyFloat
{
	public enum ModOp
	{
		Ignore,
		Add,
		Override,
		Multiply
	}

	public float value;
	public ModOp operation;

	public float GetModifiedValue(float input)
	{
		switch (operation)
		{
			case ModOp.Add:
				return input + value;
			case ModOp.Override:
				return value;
			case ModOp.Multiply:
				return input * value;
			default:
				return input;
		}
	}

	public void CopyValuesFrom(AbilityModPropertyFloat other)
	{
		value = other.value;
		operation = other.operation;
	}

#if SERVER
	// custom
	public override string ToString()
	{
		switch (operation)
		{
			case ModOp.Add:
				return $"+{value}";
			case ModOp.Multiply:
				return $"*{value}";
			case ModOp.Override:
				return $"*{value}";
			default:
				return "Ignore";
		}
	}
#endif
}

using System;

[Serializable]
public class AbilityModPropertyBool
{
	public enum ModOp
	{
		Ignore,
		Override
	}

	public bool value;

	public ModOp operation;

	public bool GetModifiedValue(bool input)
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

	public void CopyValuesFrom(AbilityModPropertyBool other)
	{
		value = other.value;
		operation = other.operation;
	}
}

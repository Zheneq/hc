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
		if (operation == ModOp.Add)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return input + value;
				}
			}
		}
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
		if (operation == ModOp.Multiply)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return input * value;
				}
			}
		}
		return input;
	}

	public void CopyValuesFrom(AbilityModPropertyFloat other)
	{
		value = other.value;
		operation = other.operation;
	}
}

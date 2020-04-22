using System;

[Serializable]
public class AbilityModPropertyShape
{
	public enum ModOp
	{
		Ignore,
		Override
	}

	public AbilityAreaShape value;

	public ModOp operation;

	public AbilityAreaShape GetModifiedValue(AbilityAreaShape input)
	{
		if (operation == ModOp.Override)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return value;
				}
			}
		}
		return input;
	}

	public void CopyValuesFrom(AbilityModPropertyShape other)
	{
		value = other.value;
		operation = other.operation;
	}
}

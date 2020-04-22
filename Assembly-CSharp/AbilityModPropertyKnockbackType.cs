using System;

[Serializable]
public class AbilityModPropertyKnockbackType
{
	public enum ModOp
	{
		Ignore,
		Override
	}

	public KnockbackType value = KnockbackType.AwayFromSource;

	public ModOp operation;

	public KnockbackType GetModifiedValue(KnockbackType input)
	{
		if (operation == ModOp.Override)
		{
			while (true)
			{
				switch (7)
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

	public void CopyValuesFrom(AbilityModPropertyKnockbackType other)
	{
		value = other.value;
		operation = other.operation;
	}
}

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
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityModPropertyFloat.GetModifiedValue(float)).MethodHandle;
			}
			return input + this.value;
		}
		if (this.operation == AbilityModPropertyFloat.ModOp.Override)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			return this.value;
		}
		if (this.operation == AbilityModPropertyFloat.ModOp.Multiply)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
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

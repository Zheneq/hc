using System;
using UnityEngine;

[Serializable]
public class AbilityModPropertyInt
{
	public float value;

	public AbilityModPropertyInt.ModOp operation;

	public int GetModifiedValue(int input)
	{
		if (this.operation == AbilityModPropertyInt.ModOp.Add)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityModPropertyInt.GetModifiedValue(int)).MethodHandle;
			}
			return input + Mathf.RoundToInt(this.value);
		}
		if (this.operation == AbilityModPropertyInt.ModOp.Override)
		{
			return Mathf.RoundToInt(this.value);
		}
		if (this.operation == AbilityModPropertyInt.ModOp.MultiplyAndFloor)
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
			return Mathf.FloorToInt((float)input * this.value);
		}
		if (this.operation == AbilityModPropertyInt.ModOp.MultiplyAndCeil)
		{
			return Mathf.CeilToInt((float)input * this.value);
		}
		if (this.operation == AbilityModPropertyInt.ModOp.MultiplyAndRound)
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
			return MathUtil.RoundToIntPadded((float)input * this.value);
		}
		return input;
	}

	public void CopyValuesFrom(AbilityModPropertyInt other)
	{
		this.value = other.value;
		this.operation = other.operation;
	}

	public enum ModOp
	{
		Ignore,
		Add,
		Override,
		MultiplyAndFloor,
		MultiplyAndCeil,
		MultiplyAndRound
	}
}

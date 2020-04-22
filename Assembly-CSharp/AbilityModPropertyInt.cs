using System;
using UnityEngine;

[Serializable]
public class AbilityModPropertyInt
{
	public enum ModOp
	{
		Ignore,
		Add,
		Override,
		MultiplyAndFloor,
		MultiplyAndCeil,
		MultiplyAndRound
	}

	public float value;

	public ModOp operation;

	public int GetModifiedValue(int input)
	{
		if (operation == ModOp.Add)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return input + Mathf.RoundToInt(value);
				}
			}
		}
		if (operation == ModOp.Override)
		{
			return Mathf.RoundToInt(value);
		}
		if (operation == ModOp.MultiplyAndFloor)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return Mathf.FloorToInt((float)input * value);
				}
			}
		}
		if (operation == ModOp.MultiplyAndCeil)
		{
			return Mathf.CeilToInt((float)input * value);
		}
		if (operation == ModOp.MultiplyAndRound)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return MathUtil.RoundToIntPadded((float)input * value);
				}
			}
		}
		return input;
	}

	public void CopyValuesFrom(AbilityModPropertyInt other)
	{
		value = other.value;
		operation = other.operation;
	}
}

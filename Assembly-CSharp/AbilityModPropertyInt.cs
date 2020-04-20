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
			return input + Mathf.RoundToInt(this.value);
		}
		if (this.operation == AbilityModPropertyInt.ModOp.Override)
		{
			return Mathf.RoundToInt(this.value);
		}
		if (this.operation == AbilityModPropertyInt.ModOp.MultiplyAndFloor)
		{
			return Mathf.FloorToInt((float)input * this.value);
		}
		if (this.operation == AbilityModPropertyInt.ModOp.MultiplyAndCeil)
		{
			return Mathf.CeilToInt((float)input * this.value);
		}
		if (this.operation == AbilityModPropertyInt.ModOp.MultiplyAndRound)
		{
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

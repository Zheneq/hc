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
		switch (operation)
		{
			case ModOp.Add:
				return input + Mathf.RoundToInt(value);
			case ModOp.Override:
				return Mathf.RoundToInt(value);
			case ModOp.MultiplyAndFloor:
				return Mathf.FloorToInt(input * value);
			case ModOp.MultiplyAndCeil:
				return Mathf.CeilToInt(input * value);
			case ModOp.MultiplyAndRound:
				return MathUtil.RoundToIntPadded(input * value);
			default:
				return input;
		}
	}

	public void CopyValuesFrom(AbilityModPropertyInt other)
	{
		value = other.value;
		operation = other.operation;
	}
}

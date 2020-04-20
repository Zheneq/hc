using System;
using UnityEngine;

[Serializable]
public class AbilityModPropertySequenceOverride
{
	public AbilityModPropertySequenceOverride.ModOp operation;

	public GameObject value;

	public GameObject GetModifiedValue(GameObject input)
	{
		if (this.operation == AbilityModPropertySequenceOverride.ModOp.Override)
		{
			return this.value;
		}
		return input;
	}

	public enum ModOp
	{
		Ignore,
		Override
	}
}

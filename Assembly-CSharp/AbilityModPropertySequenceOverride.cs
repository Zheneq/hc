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
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityModPropertySequenceOverride.GetModifiedValue(GameObject)).MethodHandle;
			}
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

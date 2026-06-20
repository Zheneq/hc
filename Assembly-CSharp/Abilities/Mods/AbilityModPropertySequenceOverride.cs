using System;
using UnityEngine;

[Serializable]
public class AbilityModPropertySequenceOverride
{
	public enum ModOp
	{
		Ignore,
		Override
	}

	public ModOp operation;

	public GameObject value;

	public GameObject GetModifiedValue(GameObject input)
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
					return value;
				}
			}
		}
		return input;
	}
}

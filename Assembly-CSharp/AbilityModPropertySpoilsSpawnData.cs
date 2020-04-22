using System;

[Serializable]
public class AbilityModPropertySpoilsSpawnData
{
	public enum ModOp
	{
		Ignore,
		Override
	}

	public ModOp operation;

	public SpoilsSpawnData spoilsSpawnDataOverride;

	public SpoilsSpawnData GetModifiedValue(SpoilsSpawnData input)
	{
		if (operation == ModOp.Override)
		{
			return spoilsSpawnDataOverride;
		}
		return input;
	}
}

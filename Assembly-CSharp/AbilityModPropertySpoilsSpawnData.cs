using System;

[Serializable]
public class AbilityModPropertySpoilsSpawnData
{
	public AbilityModPropertySpoilsSpawnData.ModOp operation;

	public SpoilsSpawnData spoilsSpawnDataOverride;

	public SpoilsSpawnData GetModifiedValue(SpoilsSpawnData input)
	{
		if (this.operation == AbilityModPropertySpoilsSpawnData.ModOp.Override)
		{
			return this.spoilsSpawnDataOverride;
		}
		return input;
	}

	public enum ModOp
	{
		Ignore,
		Override
	}
}

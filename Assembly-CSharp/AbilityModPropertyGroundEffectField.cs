using System;

[Serializable]
public class AbilityModPropertyGroundEffectField
{
	public enum ModOp
	{
		Ignore,
		UseMods
	}

	public ModOp operation;

	public GroundEffectFieldModData groundFieldModData;

	public GroundEffectField GetModifiedValue(GroundEffectField input)
	{
		if (operation == ModOp.UseMods)
		{
			return groundFieldModData.GetModifiedCopy(input);
		}
		return input;
	}
}

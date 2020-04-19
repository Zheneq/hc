using System;

[Serializable]
public class AbilityModPropertyGroundEffectField
{
	public AbilityModPropertyGroundEffectField.ModOp operation;

	public GroundEffectFieldModData groundFieldModData;

	public GroundEffectField GetModifiedValue(GroundEffectField input)
	{
		if (this.operation == AbilityModPropertyGroundEffectField.ModOp.UseMods)
		{
			return this.groundFieldModData.GetModifiedCopy(input);
		}
		return input;
	}

	public enum ModOp
	{
		Ignore,
		UseMods
	}
}

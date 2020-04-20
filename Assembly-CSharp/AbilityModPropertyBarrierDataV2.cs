using System;

[Serializable]
public class AbilityModPropertyBarrierDataV2
{
	public AbilityModPropertyBarrierDataV2.ModOp operation;

	public BarrierModData barrierModData;

	public StandardBarrierData GetModifiedValue(StandardBarrierData input)
	{
		if (this.operation == AbilityModPropertyBarrierDataV2.ModOp.UseMods)
		{
			return this.barrierModData.GetModifiedCopy(input);
		}
		return input;
	}

	public enum ModOp
	{
		Ignore,
		UseMods
	}
}

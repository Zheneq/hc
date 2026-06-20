using System;

[Serializable]
public class AbilityModPropertyBarrierDataV2
{
	public enum ModOp
	{
		Ignore,
		UseMods
	}

	public ModOp operation;

	public BarrierModData barrierModData;

	public StandardBarrierData GetModifiedValue(StandardBarrierData input)
	{
		if (operation == ModOp.UseMods)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return barrierModData.GetModifiedCopy(input);
				}
			}
		}
		return input;
	}
}

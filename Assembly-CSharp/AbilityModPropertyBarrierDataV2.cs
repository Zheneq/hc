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
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityModPropertyBarrierDataV2.GetModifiedValue(StandardBarrierData)).MethodHandle;
			}
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

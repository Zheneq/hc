using System;
using System.Collections.Generic;

[Serializable]
public class CheckKarma
{
	public int KarmaTemplateId;

	public LootTableEntry KarmaRewardEntry;

	public List<CheckKarmaInteropValue> InteropValues;

	public float GetChance(int quantity)
	{
		if (this.InteropValues.IsNullOrEmpty<CheckKarmaInteropValue>())
		{
			return 0f;
		}
		float result = 0f;
		CheckKarmaInteropValue checkKarmaInteropValue = null;
		CheckKarmaInteropValue checkKarmaInteropValue2 = null;
		using (List<CheckKarmaInteropValue>.Enumerator enumerator = this.InteropValues.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				CheckKarmaInteropValue checkKarmaInteropValue3 = enumerator.Current;
				if (checkKarmaInteropValue3.KarmaValue <= quantity)
				{
					checkKarmaInteropValue = checkKarmaInteropValue3;
				}
				if (quantity <= checkKarmaInteropValue3.KarmaValue)
				{
					checkKarmaInteropValue2 = checkKarmaInteropValue3;
				}
			}
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(CheckKarma.GetChance(int)).MethodHandle;
			}
		}
		if (checkKarmaInteropValue2 == null)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (checkKarmaInteropValue != null)
			{
				result = checkKarmaInteropValue.Ratio;
			}
		}
		if (checkKarmaInteropValue != null)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (checkKarmaInteropValue2 != null)
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				result = (float)(quantity - checkKarmaInteropValue.KarmaValue) * (checkKarmaInteropValue2.Ratio - checkKarmaInteropValue.Ratio) / (float)(checkKarmaInteropValue2.KarmaValue - checkKarmaInteropValue.KarmaValue) + checkKarmaInteropValue.Ratio;
			}
		}
		return result;
	}

	public bool Roll(int quantity)
	{
		float chance = this.GetChance(quantity);
		return this.RollChance(chance);
	}

	private bool RollChance(float chance)
	{
		return (double)chance > InventoryUtility.GetRandomNumber(0.0, 100.0);
	}

	public override string ToString()
	{
		return string.Format("KarmaTemplateId {0}, KarmaRewardEntry ({1}), InteropValues (count {2})", this.KarmaTemplateId, this.KarmaRewardEntry, this.InteropValues.Count);
	}
}

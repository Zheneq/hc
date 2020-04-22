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
		if (InteropValues.IsNullOrEmpty())
		{
			return 0f;
		}
		float result = 0f;
		CheckKarmaInteropValue checkKarmaInteropValue = null;
		CheckKarmaInteropValue checkKarmaInteropValue2 = null;
		using (List<CheckKarmaInteropValue>.Enumerator enumerator = InteropValues.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				CheckKarmaInteropValue current = enumerator.Current;
				if (current.KarmaValue <= quantity)
				{
					checkKarmaInteropValue = current;
				}
				if (quantity <= current.KarmaValue)
				{
					checkKarmaInteropValue2 = current;
				}
			}
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					goto end_IL_002c;
				}
			}
			end_IL_002c:;
		}
		if (checkKarmaInteropValue2 == null)
		{
			while (true)
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
			while (true)
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
				while (true)
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
		float chance = GetChance(quantity);
		return RollChance(chance);
	}

	private bool RollChance(float chance)
	{
		return (double)chance > InventoryUtility.GetRandomNumber(0.0, 100.0);
	}

	public override string ToString()
	{
		return $"KarmaTemplateId {KarmaTemplateId}, KarmaRewardEntry ({KarmaRewardEntry}), InteropValues (count {InteropValues.Count})";
	}
}

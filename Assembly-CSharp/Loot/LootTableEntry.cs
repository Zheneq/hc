using System;

[Serializable]
public class LootTableEntry
{
	public LootTableEntryType Type;

	public int Index;

	public float Weight;

	public int MinValue;

	public int MaxValue;

	public bool InvalidIfOwned;

	public QuestPrerequisites Prerequisites;

	public bool IsValid()
	{
		int result;
		if (Index > 0)
		{
			result = ((Type != LootTableEntryType.None) ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	public bool Roll(float chance)
	{
		if (chance <= 0f)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return false;
				}
			}
		}
		if (chance >= 100f)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return true;
				}
			}
		}
		return RollChance(chance);
	}

	private bool RollChance(float chance)
	{
		return (double)chance > InventoryUtility.GetRandomNumber(0.0, 100.0);
	}

	public override string ToString()
	{
		return $"{Type}, Index={Index}, Weight={Weight}, Amount=({MinValue} ~ {MaxValue})";
	}
}

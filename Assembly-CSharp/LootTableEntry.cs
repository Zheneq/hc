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
		bool result;
		if (this.Index > 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(LootTableEntry.IsValid()).MethodHandle;
			}
			result = (this.Type != LootTableEntryType.None);
		}
		else
		{
			result = false;
		}
		return result;
	}

	public bool Roll(float chance)
	{
		if (chance <= 0f)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(LootTableEntry.Roll(float)).MethodHandle;
			}
			return false;
		}
		if (chance >= 100f)
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
			return true;
		}
		return this.RollChance(chance);
	}

	private bool RollChance(float chance)
	{
		return (double)chance > InventoryUtility.GetRandomNumber(0.0, 100.0);
	}

	public override string ToString()
	{
		return string.Format("{0}, Index={1}, Weight={2}, Amount=({3} ~ {4})", new object[]
		{
			this.Type,
			this.Index,
			this.Weight,
			this.MinValue,
			this.MaxValue
		});
	}
}

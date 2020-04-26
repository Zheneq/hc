using System;
using System.Collections.Generic;

[Serializable]
public class LootTable
{
	public int Index;

	public string Name;

	public string Description;

	public LootTableEntrySelectionRule SelectionRule;

	public List<LootTableEntry> Entries;

	public LootTableEntry FallbackEntry;

	public int[] PreviewTemplateIds;

	public int[] ChaseTemplateIds;

	public List<GrantKarma> GrantKarmas;

	public List<CheckKarma> CheckKarmas;

	public bool NoDuplicates;

	public bool NoDuplicatesOnNextRoll;

	public bool EnableCharacterItemDropBalance;

	public bool EnableKarmaBalance;

	public bool Enabled;

	public LootTable()
	{
		Index = -1;
		Enabled = true;
	}

	public bool IsValid()
	{
		return Index > 0;
	}

	public bool IsRandomSelection()
	{
		int result;
		if (SelectionRule == LootTableEntrySelectionRule.PickFromOneEntry)
		{
			result = ((Entries.Count > 1) ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	public string GetName()
	{
		return StringUtil.TR_LootTableName(Index);
	}

	public string GetDescription()
	{
		return StringUtil.TR_LootTableDescription(Index);
	}

	public override string ToString()
	{
		return $"[{Index}] {Name}, {SelectionRule}, {Entries.Count}";
	}
}

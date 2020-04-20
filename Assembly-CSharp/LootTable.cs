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
		this.Index = -1;
		this.Enabled = true;
	}

	public bool IsValid()
	{
		return this.Index > 0;
	}

	public bool IsRandomSelection()
	{
		bool result;
		if (this.SelectionRule == LootTableEntrySelectionRule.PickFromOneEntry)
		{
			result = (this.Entries.Count > 1);
		}
		else
		{
			result = false;
		}
		return result;
	}

	public string GetName()
	{
		return StringUtil.TR_LootTableName(this.Index);
	}

	public string GetDescription()
	{
		return StringUtil.TR_LootTableDescription(this.Index);
	}

	public override string ToString()
	{
		return string.Format("[{0}] {1}, {2}, {3}", new object[]
		{
			this.Index,
			this.Name,
			this.SelectionRule,
			this.Entries.Count
		});
	}
}

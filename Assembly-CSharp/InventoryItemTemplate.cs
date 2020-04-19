using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class InventoryItemTemplate
{
	public int Index;

	public string DisplayName;

	public string Description;

	public string ObtainDescription;

	public string FlavorText;

	public string IconPath;

	public InventoryItemType Type;

	public int[] TypeSpecificData;

	public string TypeSpecificString;

	public InventoryItemRarity Rarity;

	public int Value;

	public bool NoDuplicates;

	public bool Enabled;

	public bool IsStackable;

	public bool IsAutoConsumable;

	public int ISOPrice;

	public int SeasonCollectionSortOrder;

	public CharacterType AssociatedCharacter;

	public InventoryItemTemplate()
	{
		this.Index = -1;
		this.Enabled = true;
	}

	public string GetDisplayName()
	{
		if (this.Index == -1)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(InventoryItemTemplate.GetDisplayName()).MethodHandle;
			}
			return this.DisplayName;
		}
		return StringUtil.TR_InventoryItemName(this.Index);
	}

	public string GetDescription()
	{
		return StringUtil.TR_InventoryItemDescription(this.Index);
	}

	public string GetObtainDescription()
	{
		return StringUtil.TR_InventoryObtainedDescription(this.Index);
	}

	public string GetFlavorText()
	{
		return StringUtil.TR_InventoryFlavorText(this.Index);
	}

	public InventoryItem Process()
	{
		return new InventoryItem(this.Index, 1, 0);
	}

	public string GetProductCode()
	{
		return "INVENTORY_ITEM_" + this.Index;
	}

	public List<InventoryItem> Process(int count)
	{
		List<InventoryItem> list = new List<InventoryItem>();
		if (this.IsStackable)
		{
			InventoryItem item = new InventoryItem(this.Index, count, 0);
			list.Add(item);
		}
		else
		{
			for (int i = 0; i < count; i++)
			{
				InventoryItem item2 = this.Process();
				list.Add(item2);
			}
		}
		return list;
	}

	public override string ToString()
	{
		if (this.TypeSpecificData.IsNullOrEmpty<int>())
		{
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(InventoryItemTemplate.ToString()).MethodHandle;
			}
			return string.Format("[{0}] {1}, {2}", this.Index, this.DisplayName, this.Type);
		}
		string format = "[{0}] {1}, {2}, ({3})";
		object[] array = new object[4];
		array[0] = this.Index;
		array[1] = this.DisplayName;
		array[2] = this.Type;
		array[3] = string.Join(",", (from p in this.TypeSpecificData
		select p.ToString()).ToArray<string>());
		return string.Format(format, array);
	}
}

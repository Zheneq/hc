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
		Index = -1;
		Enabled = true;
	}

	public string GetDisplayName()
	{
		if (Index == -1)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return DisplayName;
				}
			}
		}
		return StringUtil.TR_InventoryItemName(Index);
	}

	public string GetDescription()
	{
		return StringUtil.TR_InventoryItemDescription(Index);
	}

	public string GetObtainDescription()
	{
		return StringUtil.TR_InventoryObtainedDescription(Index);
	}

	public string GetFlavorText()
	{
		return StringUtil.TR_InventoryFlavorText(Index);
	}

	public InventoryItem Process()
	{
		return new InventoryItem(Index);
	}

	public string GetProductCode()
	{
		return "INVENTORY_ITEM_" + Index;
	}

	public List<InventoryItem> Process(int count)
	{
		List<InventoryItem> list = new List<InventoryItem>();
		if (IsStackable)
		{
			InventoryItem item = new InventoryItem(Index, count);
			list.Add(item);
		}
		else
		{
			for (int i = 0; i < count; i++)
			{
				InventoryItem item2 = Process();
				list.Add(item2);
			}
		}
		return list;
	}

	public override string ToString()
	{
		if (TypeSpecificData.IsNullOrEmpty())
		{
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
					return $"[{Index}] {DisplayName}, {Type}";
				}
			}
		}
		return string.Format("[{0}] {1}, {2}, ({3})", Index, DisplayName, Type, string.Join(",", TypeSpecificData.Select((int p) => p.ToString()).ToArray()));
	}
}

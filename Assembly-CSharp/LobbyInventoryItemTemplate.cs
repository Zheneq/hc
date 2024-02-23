using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[Serializable]
public class LobbyInventoryItemTemplate
{
	public int Index;

	public string DisplayName;

	public string ProductCode;

	public InventoryItemType Type;

	public int[] TypeSpecificData;

	public string TypeSpecificString;

	public InventoryItemRarity Rarity;

	public int Value;

	public bool Enabled;

	public bool IsStackable;

	public bool IsAutoConsumable;

	public int ISOPrice;

	public LobbyInventoryItemTemplate()
	{
		Index = -1;
		Enabled = true;
	}

	public InventoryItem Process()
	{
		return new InventoryItem(Index);
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
				switch (1)
				{
				case 0:
					break;
				default:
					return new StringBuilder().Append("[").Append(Index).Append("] ").Append(DisplayName).Append(", ").Append(Type).ToString();
				}
			}
		}
		object[] obj = new object[4]
		{
			Index,
			DisplayName,
			Type,
			null
		};
		int[] typeSpecificData = TypeSpecificData;
		
		obj[3] = string.Join(",", typeSpecificData.Select(((int p) => p.ToString())).ToArray());
		return string.Format("[{0}] {1}, {2}, ({3})", obj);
	}
}

using System;
using System.Collections.Generic;
using System.Linq;

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
		this.Index = -1;
		this.Enabled = true;
	}

	public InventoryItem Process()
	{
		return new InventoryItem(this.Index, 1, 0);
	}

	public List<InventoryItem> Process(int count)
	{
		List<InventoryItem> list = new List<InventoryItem>();
		if (this.IsStackable)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(LobbyInventoryItemTemplate.Process(int)).MethodHandle;
			}
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
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
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
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(LobbyInventoryItemTemplate.ToString()).MethodHandle;
			}
			return string.Format("[{0}] {1}, {2}", this.Index, this.DisplayName, this.Type);
		}
		string format = "[{0}] {1}, {2}, ({3})";
		object[] array = new object[4];
		array[0] = this.Index;
		array[1] = this.DisplayName;
		array[2] = this.Type;
		int num = 3;
		string separator = ",";
		IEnumerable<int> typeSpecificData = this.TypeSpecificData;
		if (LobbyInventoryItemTemplate.<>f__am$cache0 == null)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			LobbyInventoryItemTemplate.<>f__am$cache0 = ((int p) => p.ToString());
		}
		array[num] = string.Join(separator, typeSpecificData.Select(LobbyInventoryItemTemplate.<>f__am$cache0).ToArray<string>());
		return string.Format(format, array);
	}
}

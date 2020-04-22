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
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
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
			while (true)
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
		if (TypeSpecificData.IsNullOrEmpty())
		{
			while (true)
			{
				switch (1)
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
		object[] obj = new object[4]
		{
			Index,
			DisplayName,
			Type,
			null
		};
		int[] typeSpecificData = TypeSpecificData;
		if (_003C_003Ef__am_0024cache0 == null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			_003C_003Ef__am_0024cache0 = ((int p) => p.ToString());
		}
		obj[3] = string.Join(",", typeSpecificData.Select(_003C_003Ef__am_0024cache0).ToArray());
		return string.Format("[{0}] {1}, {2}, ({3})", obj);
	}
}

using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class Loot
{
	public List<InventoryItem> Items;

	public Dictionary<int, Karma> Karmas;

	public Loot()
	{
		this.Karmas = new Dictionary<int, Karma>();
		this.Items = new List<InventoryItem>();
	}

	public void AddItem(InventoryItem item)
	{
		this.Items.Add(item);
	}

	public void AddItems(List<InventoryItem> items)
	{
		this.Items.AddRange(items);
	}

	public bool HasItem(int itemTemplateId)
	{
		return this.Items.Exists((InventoryItem i) => i.TemplateId == itemTemplateId);
	}

	public IEnumerable<int> GetItemTemplateIds()
	{
		IEnumerable<InventoryItem> items = this.Items;
		if (Loot.<>f__am$cache0 == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Loot.GetItemTemplateIds()).MethodHandle;
			}
			Loot.<>f__am$cache0 = ((InventoryItem i) => i.TemplateId);
		}
		return items.Select(Loot.<>f__am$cache0);
	}

	public void AddKarma(Karma karma)
	{
		if (this.Karmas.ContainsKey(karma.TemplateId))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Loot.AddKarma(Karma)).MethodHandle;
			}
			this.Karmas[karma.TemplateId].Quantity += karma.Quantity;
		}
		else
		{
			this.Karmas[karma.TemplateId] = new Karma
			{
				TemplateId = karma.TemplateId,
				Quantity = karma.Quantity
			};
		}
	}

	public Karma GetKarma(int karmaTemplateId)
	{
		Karma result;
		this.Karmas.TryGetValue(karmaTemplateId, out result);
		return result;
	}

	public int GetKarmaQuantity(int karmaTemplateId)
	{
		int result = 0;
		Karma karma = this.GetKarma(karmaTemplateId);
		if (karma != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Loot.GetKarmaQuantity(int)).MethodHandle;
			}
			result = karma.Quantity;
		}
		return result;
	}

	public void MergeItems(Loot loot)
	{
		this.AddItems(loot.Items);
	}
}

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
		Karmas = new Dictionary<int, Karma>();
		Items = new List<InventoryItem>();
	}

	public void AddItem(InventoryItem item)
	{
		Items.Add(item);
	}

	public void AddItems(List<InventoryItem> items)
	{
		Items.AddRange(items);
	}

	public bool HasItem(int itemTemplateId)
	{
		return Items.Exists((InventoryItem i) => i.TemplateId == itemTemplateId);
	}

	public IEnumerable<int> GetItemTemplateIds()
	{
		List<InventoryItem> items = Items;
		
		return items.Select(((InventoryItem i) => i.TemplateId));
	}

	public void AddKarma(Karma karma)
	{
		if (Karmas.ContainsKey(karma.TemplateId))
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					Karmas[karma.TemplateId].Quantity += karma.Quantity;
					return;
				}
			}
		}
		Karmas[karma.TemplateId] = new Karma
		{
			TemplateId = karma.TemplateId,
			Quantity = karma.Quantity
		};
	}

	public Karma GetKarma(int karmaTemplateId)
	{
		Karma value;
		Karmas.TryGetValue(karmaTemplateId, out value);
		return value;
	}

	public int GetKarmaQuantity(int karmaTemplateId)
	{
		int result = 0;
		Karma karma = GetKarma(karmaTemplateId);
		if (karma != null)
		{
			result = karma.Quantity;
		}
		return result;
	}

	public void MergeItems(Loot loot)
	{
		AddItems(loot.Items);
	}
}

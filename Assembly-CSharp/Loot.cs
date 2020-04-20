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
		
		return items.Select(((InventoryItem i) => i.TemplateId));
	}

	public void AddKarma(Karma karma)
	{
		if (this.Karmas.ContainsKey(karma.TemplateId))
		{
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
			result = karma.Quantity;
		}
		return result;
	}

	public void MergeItems(Loot loot)
	{
		this.AddItems(loot.Items);
	}
}

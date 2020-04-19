using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

[Serializable]
public class InventoryComponent
{
	[JsonIgnore]
	[NonSerialized]
	private Dictionary<int, InventoryItem> m_itemByIdCache;

	[JsonIgnore]
	[NonSerialized]
	private Dictionary<int, InventoryComponent.InventoryItemListCache> m_itemsByTemplateIdCache;

	[JsonIgnore]
	[NonSerialized]
	private Dictionary<InventoryItemType, InventoryComponent.InventoryItemListCache> m_itemsByTypeCache;

	public InventoryComponent()
	{
		this.NextItemId = 1;
		this.Items = new List<InventoryItem>();
		this.Karmas = new Dictionary<int, Karma>();
		this.Loots = new Dictionary<int, Loot>();
		this.CharacterItemDropBalanceValues = new Dictionary<CharacterType, int>();
		this.LastLockboxOpenTime = DateTime.MinValue;
		this.m_itemByIdCache = null;
		this.m_itemsByTemplateIdCache = null;
		this.m_itemsByTypeCache = null;
	}

	public InventoryComponent(List<InventoryItem> items, Dictionary<int, Karma> karmas, Dictionary<int, Loot> loots)
	{
		this.Items = items;
		this.Karmas = karmas;
		this.Loots = loots;
	}

	public int NextItemId { get; set; }

	public List<InventoryItem> Items { get; set; }

	public Dictionary<int, Karma> Karmas { get; set; }

	public Dictionary<int, Loot> Loots { get; set; }

	public Dictionary<CharacterType, int> CharacterItemDropBalanceValues { get; set; }

	public DateTime LastLockboxOpenTime { get; set; }

	public int AddItem(InventoryItem itemToAdd)
	{
		InventoryItem inventoryItem = this.GetItem(itemToAdd);
		if (inventoryItem != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(InventoryComponent.AddItem(InventoryItem)).MethodHandle;
			}
			if (inventoryItem.IsStackable())
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				inventoryItem.Count += itemToAdd.Count;
				this.OnItemAdded(inventoryItem);
				goto IL_E6;
			}
		}
		if (itemToAdd.IsStackable())
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			inventoryItem = new InventoryItem(itemToAdd, this.NextItemId++);
			this.Items.Add(inventoryItem);
			this.OnItemAdded(inventoryItem);
		}
		else
		{
			for (int i = 0; i < itemToAdd.Count; i++)
			{
				inventoryItem = new InventoryItem(itemToAdd.TemplateId, 1, this.NextItemId++);
				this.Items.Add(inventoryItem);
				this.OnItemAdded(inventoryItem);
			}
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		IL_E6:
		return inventoryItem.Id;
	}

	public void AddItems(List<InventoryItem> itemsToAdd)
	{
		itemsToAdd.ForEach(delegate(InventoryItem i)
		{
			this.AddItem(i);
		});
	}

	public bool RemoveItem(InventoryItem itemToRemove)
	{
		InventoryItem item = this.GetItem(itemToRemove);
		if (item != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(InventoryComponent.RemoveItem(InventoryItem)).MethodHandle;
			}
			if (item.IsStackable())
			{
				item.Count -= itemToRemove.Count;
				if (item.Count <= 0)
				{
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					this.RemoveItemInternal(item);
				}
			}
			else
			{
				this.RemoveItemInternal(item);
			}
			return true;
		}
		return false;
	}

	public void RemoveItems(List<InventoryItem> itemsToRemove)
	{
		itemsToRemove.ForEach(delegate(InventoryItem i)
		{
			this.RemoveItem(i);
		});
	}

	public bool RemoveItemByTemplateId(int itemTemplateId, int itemCount = 1)
	{
		InventoryItem itemByTemplateId = this.GetItemByTemplateId(itemTemplateId);
		if (itemByTemplateId != null)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(InventoryComponent.RemoveItemByTemplateId(int, int)).MethodHandle;
			}
			if (itemByTemplateId.IsStackable())
			{
				itemByTemplateId.Count -= itemCount;
				if (itemByTemplateId.Count <= 0)
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
					this.RemoveItemInternal(itemByTemplateId);
				}
			}
			else
			{
				this.RemoveItemInternal(itemByTemplateId);
			}
			return true;
		}
		return false;
	}

	public bool RemoveItem(int itemId, int itemCount = 1)
	{
		InventoryItem item = this.GetItem(itemId);
		if (item != null)
		{
			if (item.IsStackable())
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(InventoryComponent.RemoveItem(int, int)).MethodHandle;
				}
				item.Count -= itemCount;
				if (item.Count <= 0)
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
					this.RemoveItemInternal(item);
				}
			}
			else
			{
				this.RemoveItemInternal(item);
			}
			return true;
		}
		return false;
	}

	public void RemoveItems(List<int> itemIds)
	{
		itemIds.ForEach(delegate(int i)
		{
			this.RemoveItem(i, 1);
		});
	}

	private void RemoveItemInternal(InventoryItem item)
	{
		this.Items.Remove(item);
		this.Loots.Remove(item.Id);
		this.OnItemRemoved(item);
	}

	public void UpdateItem(InventoryItem itemToUpdate)
	{
		int num = 0;
		if (itemToUpdate.Id > 0)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(InventoryComponent.UpdateItem(InventoryItem)).MethodHandle;
			}
			InventoryItem item = this.GetItem(itemToUpdate.Id);
			if (item != null)
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
				num = item.Count;
			}
		}
		else
		{
			num = this.GetItemCountByTemplateId(itemToUpdate.TemplateId);
		}
		int num2 = itemToUpdate.Count - num;
		if (num2 > 0)
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
			itemToUpdate.Count = num2;
			this.AddItem(itemToUpdate);
		}
		else if (num2 < 0)
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
			itemToUpdate.Count = -num2;
			this.RemoveItem(itemToUpdate);
		}
	}

	public void UpdateItems(List<InventoryItem> itemsToUpdate)
	{
		itemsToUpdate.ForEach(delegate(InventoryItem i)
		{
			this.UpdateItem(i);
		});
	}

	public InventoryItem GetItem(InventoryItem itemToSearch)
	{
		if (itemToSearch.Id > 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(InventoryComponent.GetItem(InventoryItem)).MethodHandle;
			}
			return this.GetItem(itemToSearch.Id);
		}
		if (itemToSearch.TemplateId > 0)
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
			return this.GetItemByTemplateId(itemToSearch.TemplateId);
		}
		return null;
	}

	public InventoryItem GetItem(int itemId)
	{
		if (this.m_itemByIdCache == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(InventoryComponent.GetItem(int)).MethodHandle;
			}
			this.m_itemByIdCache = new Dictionary<int, InventoryItem>();
			using (List<InventoryItem>.Enumerator enumerator = this.Items.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					InventoryItem inventoryItem = enumerator.Current;
					this.m_itemByIdCache[inventoryItem.Id] = inventoryItem;
				}
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
			}
		}
		return this.m_itemByIdCache.TryGetValue(itemId);
	}

	public InventoryItem GetItemByTemplateId(int itemTemplateId)
	{
		InventoryComponent.InventoryItemListCache allItemsByTemplateId = this.GetAllItemsByTemplateId(itemTemplateId);
		if (allItemsByTemplateId != null)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(InventoryComponent.GetItemByTemplateId(int)).MethodHandle;
			}
			return allItemsByTemplateId.Items.FirstOrDefault<InventoryItem>();
		}
		return null;
	}

	private InventoryComponent.InventoryItemListCache GetAllItemsByTemplateId(int itemTemplateId)
	{
		if (this.m_itemsByTemplateIdCache == null)
		{
			this.m_itemsByTemplateIdCache = new Dictionary<int, InventoryComponent.InventoryItemListCache>();
			foreach (InventoryItem inventoryItem in this.Items)
			{
				InventoryItemTemplate template = inventoryItem.GetTemplate();
				InventoryComponent.InventoryItemListCache inventoryItemListCache = this.m_itemsByTemplateIdCache.TryGetValue(template.Index);
				if (inventoryItemListCache == null)
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
					if (!true)
					{
						RuntimeMethodHandle runtimeMethodHandle = methodof(InventoryComponent.GetAllItemsByTemplateId(int)).MethodHandle;
					}
					inventoryItemListCache = new InventoryComponent.InventoryItemListCache();
					inventoryItemListCache.Count += inventoryItem.Count;
					inventoryItemListCache.Items.Add(inventoryItem);
					this.m_itemsByTemplateIdCache.Add(template.Index, inventoryItemListCache);
				}
				else
				{
					inventoryItemListCache.Count += inventoryItem.Count;
					inventoryItemListCache.Items.Add(inventoryItem);
				}
			}
		}
		return this.m_itemsByTemplateIdCache.TryGetValue(itemTemplateId);
	}

	private InventoryComponent.InventoryItemListCache GetAllItemsByType(InventoryItemType itemType)
	{
		if (this.m_itemsByTypeCache == null)
		{
			this.m_itemsByTypeCache = new Dictionary<InventoryItemType, InventoryComponent.InventoryItemListCache>();
			using (List<InventoryItem>.Enumerator enumerator = this.Items.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					InventoryItem inventoryItem = enumerator.Current;
					InventoryItemTemplate template = inventoryItem.GetTemplate();
					InventoryComponent.InventoryItemListCache inventoryItemListCache = this.m_itemsByTypeCache.TryGetValue(template.Type);
					if (inventoryItemListCache == null)
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
							RuntimeMethodHandle runtimeMethodHandle = methodof(InventoryComponent.GetAllItemsByType(InventoryItemType)).MethodHandle;
						}
						inventoryItemListCache = new InventoryComponent.InventoryItemListCache();
						inventoryItemListCache.Count += inventoryItem.Count;
						inventoryItemListCache.Items.Add(inventoryItem);
						this.m_itemsByTypeCache.Add(template.Type, inventoryItemListCache);
					}
					else
					{
						inventoryItemListCache.Items.Add(inventoryItem);
					}
				}
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
			}
		}
		return this.m_itemsByTypeCache.TryGetValue(itemType);
	}

	public int GetItemCountByTemplateId(int itemTemplateId)
	{
		int result = 0;
		InventoryComponent.InventoryItemListCache allItemsByTemplateId = this.GetAllItemsByTemplateId(itemTemplateId);
		if (allItemsByTemplateId != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(InventoryComponent.GetItemCountByTemplateId(int)).MethodHandle;
			}
			result = allItemsByTemplateId.Count;
		}
		return result;
	}

	public int GetItemCountByType(InventoryItemType itemType)
	{
		int result = 0;
		InventoryComponent.InventoryItemListCache allItemsByType = this.GetAllItemsByType(itemType);
		if (allItemsByType != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(InventoryComponent.GetItemCountByType(InventoryItemType)).MethodHandle;
			}
			result = allItemsByType.Count;
		}
		return result;
	}

	public bool HasItem(int itemTemplateId)
	{
		return this.GetItemByTemplateId(itemTemplateId) != null;
	}

	public void AddKarma(int karmaTemplateId)
	{
		if (!this.Karmas.ContainsKey(karmaTemplateId))
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(InventoryComponent.AddKarma(int)).MethodHandle;
			}
			Karma value = new Karma
			{
				TemplateId = karmaTemplateId
			};
			this.Karmas[karmaTemplateId] = value;
		}
	}

	public void AddKarma(Karma karma)
	{
		if (this.Karmas.ContainsKey(karma.TemplateId))
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(InventoryComponent.AddKarma(Karma)).MethodHandle;
			}
			this.Karmas[karma.TemplateId].Quantity += karma.Quantity;
		}
		else
		{
			this.Karmas[karma.TemplateId] = karma;
		}
	}

	public void AddKarmas(IDictionary<int, Karma> karmas)
	{
		IEnumerator<KeyValuePair<int, Karma>> enumerator = karmas.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<int, Karma> keyValuePair = enumerator.Current;
				if (this.Karmas.ContainsKey(keyValuePair.Key))
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
					if (!true)
					{
						RuntimeMethodHandle runtimeMethodHandle = methodof(InventoryComponent.AddKarmas(IDictionary<int, Karma>)).MethodHandle;
					}
					this.Karmas[keyValuePair.Key].Quantity += keyValuePair.Value.Quantity;
				}
				else
				{
					this.Karmas[keyValuePair.Key] = keyValuePair.Value;
				}
			}
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		finally
		{
			if (enumerator != null)
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
				enumerator.Dispose();
			}
		}
	}

	public void RemoveKarma(int karmaTemplateId)
	{
		this.Karmas.Remove(karmaTemplateId);
	}

	public void RemoveKarmas(List<int> karmaTemplateIds)
	{
		using (List<int>.Enumerator enumerator = karmaTemplateIds.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				int key = enumerator.Current;
				this.Karmas.Remove(key);
			}
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(InventoryComponent.RemoveKarmas(List<int>)).MethodHandle;
			}
		}
	}

	public Karma GetKarma(int karmaTemplateId)
	{
		Karma result;
		this.Karmas.TryGetValue(karmaTemplateId, out result);
		return result;
	}

	public void AddLoot(int itemId, Loot loot)
	{
		if (!this.Loots.ContainsKey(itemId))
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(InventoryComponent.AddLoot(int, Loot)).MethodHandle;
			}
			this.Loots.Add(itemId, loot);
		}
	}

	public void RemoveLoot(int itemId)
	{
		this.Loots.Remove(itemId);
	}

	public Loot GetLoot(int itemId)
	{
		return this.Loots.TryGetValue(itemId);
	}

	public bool HasPendingItem(int itemTemplateId)
	{
		using (Dictionary<int, Loot>.ValueCollection.Enumerator enumerator = this.Loots.Values.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				Loot loot = enumerator.Current;
				if (loot.HasItem(itemTemplateId))
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(InventoryComponent.HasPendingItem(int)).MethodHandle;
					}
					return true;
				}
			}
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		return false;
	}

	public void AddCharacterItemDropBalanceValues(Loot loot)
	{
		using (List<InventoryItem>.Enumerator enumerator = loot.Items.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				InventoryItem item = enumerator.Current;
				InventoryItemTemplate template = item.GetTemplate();
				if (template.Type.IsCharacterBound())
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(InventoryComponent.AddCharacterItemDropBalanceValues(Loot)).MethodHandle;
					}
					CharacterType bindingCharacterType = template.GetBindingCharacterType();
					this.AddCharacterItemDropBalanceValue(bindingCharacterType, 1);
				}
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
	}

	public void AddCharacterItemDropBalanceValue(CharacterType bindingCharacterType, int delta = 1)
	{
		int characterItemDropBalance = LobbyGameplayData.Get().GetCharacterItemDropBalance();
		int num;
		this.CharacterItemDropBalanceValues.TryGetValue(bindingCharacterType, out num);
		this.CharacterItemDropBalanceValues[bindingCharacterType] = MathUtil.Clamp(num + delta, 0, characterItemDropBalance);
		bool flag = true;
		IEnumerator enumerator = Enum.GetValues(typeof(CharacterType)).GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				CharacterType key = (CharacterType)obj;
				int num2;
				this.CharacterItemDropBalanceValues.TryGetValue(key, out num2);
				if (num2 == 0)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(InventoryComponent.AddCharacterItemDropBalanceValue(CharacterType, int)).MethodHandle;
					}
					flag = false;
					goto IL_C8;
				}
			}
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
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
				disposable.Dispose();
			}
		}
		IL_C8:
		if (flag)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			IEnumerator enumerator2 = Enum.GetValues(typeof(CharacterType)).GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					object obj2 = enumerator2.Current;
					CharacterType key2 = (CharacterType)obj2;
					int num3;
					this.CharacterItemDropBalanceValues.TryGetValue(key2, out num3);
					this.CharacterItemDropBalanceValues[key2] = MathUtil.Clamp(num3 - 1, 0, characterItemDropBalance);
				}
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			finally
			{
				IDisposable disposable2;
				if ((disposable2 = (enumerator2 as IDisposable)) != null)
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
					disposable2.Dispose();
				}
			}
		}
	}

	public int GetBalancedCharacterItemDropWeight(CharacterType characterType)
	{
		int characterItemDropBalance = LobbyGameplayData.Get().GetCharacterItemDropBalance();
		int num;
		this.CharacterItemDropBalanceValues.TryGetValue(characterType, out num);
		return MathUtil.Clamp(characterItemDropBalance - num, 0, characterItemDropBalance);
	}

	public object ShallowCopy()
	{
		return base.MemberwiseClone();
	}

	public InventoryComponent Clone()
	{
		string value = JsonConvert.SerializeObject(this);
		return JsonConvert.DeserializeObject<InventoryComponent>(value);
	}

	public InventoryComponent CloneForClient()
	{
		return new InventoryComponent
		{
			Items = this.Items,
			Loots = this.Loots,
			Karmas = this.Karmas,
			LastLockboxOpenTime = this.LastLockboxOpenTime
		};
	}

	public void FlushCaches()
	{
		this.m_itemByIdCache = null;
		this.m_itemsByTemplateIdCache = null;
		this.m_itemsByTypeCache = null;
	}

	private void OnItemAdded(InventoryItem item)
	{
		if (this.m_itemByIdCache != null)
		{
			this.m_itemByIdCache[item.Id] = item;
		}
		if (this.m_itemsByTemplateIdCache != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(InventoryComponent.OnItemAdded(InventoryItem)).MethodHandle;
			}
			InventoryComponent.InventoryItemListCache inventoryItemListCache = this.m_itemsByTemplateIdCache.TryGetValue(item.TemplateId);
			if (inventoryItemListCache == null)
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
				inventoryItemListCache = new InventoryComponent.InventoryItemListCache();
				inventoryItemListCache.Count += item.Count;
				inventoryItemListCache.Items.Add(item);
				this.m_itemsByTemplateIdCache.Add(item.TemplateId, inventoryItemListCache);
			}
			else
			{
				inventoryItemListCache.Count += item.Count;
				inventoryItemListCache.Items.Add(item);
			}
		}
		if (this.m_itemsByTypeCache != null)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			InventoryItemType type = item.GetTemplate().Type;
			InventoryComponent.InventoryItemListCache inventoryItemListCache2 = this.m_itemsByTypeCache.TryGetValue(type);
			if (inventoryItemListCache2 == null)
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
				inventoryItemListCache2 = new InventoryComponent.InventoryItemListCache();
				inventoryItemListCache2.Count += item.Count;
				inventoryItemListCache2.Items.Add(item);
				this.m_itemsByTypeCache.Add(type, inventoryItemListCache2);
			}
			else
			{
				inventoryItemListCache2.Items.Add(item);
			}
		}
	}

	private void OnItemRemoved(InventoryItem item)
	{
		if (this.m_itemByIdCache != null)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(InventoryComponent.OnItemRemoved(InventoryItem)).MethodHandle;
			}
			this.m_itemByIdCache.Remove(item.Id);
		}
		if (this.m_itemsByTemplateIdCache != null)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			InventoryComponent.InventoryItemListCache inventoryItemListCache = this.m_itemsByTemplateIdCache.TryGetValue(item.TemplateId);
			if (inventoryItemListCache != null)
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
				inventoryItemListCache.Items.Remove(item);
				if (inventoryItemListCache.Items.Count == 0)
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
					this.m_itemsByTemplateIdCache.Remove(item.TemplateId);
				}
			}
		}
		if (this.m_itemsByTypeCache != null)
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
			InventoryItemType type = item.GetTemplate().Type;
			InventoryComponent.InventoryItemListCache inventoryItemListCache2 = this.m_itemsByTypeCache.TryGetValue(type);
			if (inventoryItemListCache2 == null)
			{
				inventoryItemListCache2.Items.Remove(item);
				if (inventoryItemListCache2.Items.Count == 0)
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
					this.m_itemsByTypeCache.Remove(type);
				}
			}
		}
	}

	private class InventoryItemListCache
	{
		public int Count;

		public List<InventoryItem> Items;

		public InventoryItemListCache()
		{
			this.Count = 0;
			this.Items = new List<InventoryItem>();
		}
	}
}

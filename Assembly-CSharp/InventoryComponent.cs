using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class InventoryComponent
{
	private class InventoryItemListCache
	{
		public int Count;

		public List<InventoryItem> Items;

		public InventoryItemListCache()
		{
			Count = 0;
			Items = new List<InventoryItem>();
		}
	}

	[NonSerialized]
	[JsonIgnore]
	private Dictionary<int, InventoryItem> m_itemByIdCache;

	[NonSerialized]
	[JsonIgnore]
	private Dictionary<int, InventoryItemListCache> m_itemsByTemplateIdCache;

	[NonSerialized]
	[JsonIgnore]
	private Dictionary<InventoryItemType, InventoryItemListCache> m_itemsByTypeCache;

	public int NextItemId
	{
		get;
		set;
	}

	public List<InventoryItem> Items
	{
		get;
		set;
	}

	public Dictionary<int, Karma> Karmas
	{
		get;
		set;
	}

	public Dictionary<int, Loot> Loots
	{
		get;
		set;
	}

	public Dictionary<CharacterType, int> CharacterItemDropBalanceValues
	{
		get;
		set;
	}

	public DateTime LastLockboxOpenTime
	{
		get;
		set;
	}

	public InventoryComponent()
	{
		NextItemId = 1;
		Items = new List<InventoryItem>();
		Karmas = new Dictionary<int, Karma>();
		Loots = new Dictionary<int, Loot>();
		CharacterItemDropBalanceValues = new Dictionary<CharacterType, int>();
		LastLockboxOpenTime = DateTime.MinValue;
		m_itemByIdCache = null;
		m_itemsByTemplateIdCache = null;
		m_itemsByTypeCache = null;
	}

	public InventoryComponent(List<InventoryItem> items, Dictionary<int, Karma> karmas, Dictionary<int, Loot> loots)
	{
		Items = items;
		Karmas = karmas;
		Loots = loots;
	}

	public int AddItem(InventoryItem itemToAdd)
	{
		InventoryItem inventoryItem = GetItem(itemToAdd);
		if (inventoryItem != null)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (inventoryItem.IsStackable())
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				inventoryItem.Count += itemToAdd.Count;
				OnItemAdded(inventoryItem);
				goto IL_00e6;
			}
		}
		if (itemToAdd.IsStackable())
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			inventoryItem = new InventoryItem(itemToAdd, NextItemId++);
			Items.Add(inventoryItem);
			OnItemAdded(inventoryItem);
		}
		else
		{
			for (int i = 0; i < itemToAdd.Count; i++)
			{
				inventoryItem = new InventoryItem(itemToAdd.TemplateId, 1, NextItemId++);
				Items.Add(inventoryItem);
				OnItemAdded(inventoryItem);
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		goto IL_00e6;
		IL_00e6:
		return inventoryItem.Id;
	}

	public void AddItems(List<InventoryItem> itemsToAdd)
	{
		itemsToAdd.ForEach(delegate(InventoryItem i)
		{
			AddItem(i);
		});
	}

	public bool RemoveItem(InventoryItem itemToRemove)
	{
		InventoryItem item = GetItem(itemToRemove);
		if (item != null)
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
					if (item.IsStackable())
					{
						item.Count -= itemToRemove.Count;
						if (item.Count <= 0)
						{
							while (true)
							{
								switch (3)
								{
								case 0:
									continue;
								}
								break;
							}
							RemoveItemInternal(item);
						}
					}
					else
					{
						RemoveItemInternal(item);
					}
					return true;
				}
			}
		}
		return false;
	}

	public void RemoveItems(List<InventoryItem> itemsToRemove)
	{
		itemsToRemove.ForEach(delegate(InventoryItem i)
		{
			RemoveItem(i);
		});
	}

	public bool RemoveItemByTemplateId(int itemTemplateId, int itemCount = 1)
	{
		InventoryItem itemByTemplateId = GetItemByTemplateId(itemTemplateId);
		if (itemByTemplateId != null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					if (itemByTemplateId.IsStackable())
					{
						itemByTemplateId.Count -= itemCount;
						if (itemByTemplateId.Count <= 0)
						{
							while (true)
							{
								switch (4)
								{
								case 0:
									continue;
								}
								break;
							}
							RemoveItemInternal(itemByTemplateId);
						}
					}
					else
					{
						RemoveItemInternal(itemByTemplateId);
					}
					return true;
				}
			}
		}
		return false;
	}

	public bool RemoveItem(int itemId, int itemCount = 1)
	{
		InventoryItem item = GetItem(itemId);
		if (item != null)
		{
			if (item.IsStackable())
			{
				while (true)
				{
					switch (2)
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
				item.Count -= itemCount;
				if (item.Count <= 0)
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					RemoveItemInternal(item);
				}
			}
			else
			{
				RemoveItemInternal(item);
			}
			return true;
		}
		return false;
	}

	public void RemoveItems(List<int> itemIds)
	{
		itemIds.ForEach(delegate(int i)
		{
			RemoveItem(i);
		});
	}

	private void RemoveItemInternal(InventoryItem item)
	{
		Items.Remove(item);
		Loots.Remove(item.Id);
		OnItemRemoved(item);
	}

	public void UpdateItem(InventoryItem itemToUpdate)
	{
		int num = 0;
		if (itemToUpdate.Id > 0)
		{
			while (true)
			{
				switch (2)
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
			InventoryItem item = GetItem(itemToUpdate.Id);
			if (item != null)
			{
				while (true)
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
			num = GetItemCountByTemplateId(itemToUpdate.TemplateId);
		}
		int num2 = itemToUpdate.Count - num;
		if (num2 > 0)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					itemToUpdate.Count = num2;
					AddItem(itemToUpdate);
					return;
				}
			}
		}
		if (num2 >= 0)
		{
			return;
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			itemToUpdate.Count = -num2;
			RemoveItem(itemToUpdate);
			return;
		}
	}

	public void UpdateItems(List<InventoryItem> itemsToUpdate)
	{
		itemsToUpdate.ForEach(delegate(InventoryItem i)
		{
			UpdateItem(i);
		});
	}

	public InventoryItem GetItem(InventoryItem itemToSearch)
	{
		if (itemToSearch.Id > 0)
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
					return GetItem(itemToSearch.Id);
				}
			}
		}
		if (itemToSearch.TemplateId > 0)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return GetItemByTemplateId(itemToSearch.TemplateId);
				}
			}
		}
		return null;
	}

	public InventoryItem GetItem(int itemId)
	{
		if (m_itemByIdCache == null)
		{
			while (true)
			{
				switch (5)
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
			m_itemByIdCache = new Dictionary<int, InventoryItem>();
			using (List<InventoryItem>.Enumerator enumerator = Items.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					InventoryItem current = enumerator.Current;
					m_itemByIdCache[current.Id] = current;
				}
				while (true)
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
		return m_itemByIdCache.TryGetValue(itemId);
	}

	public InventoryItem GetItemByTemplateId(int itemTemplateId)
	{
		InventoryItemListCache allItemsByTemplateId = GetAllItemsByTemplateId(itemTemplateId);
		if (allItemsByTemplateId != null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return allItemsByTemplateId.Items.FirstOrDefault();
				}
			}
		}
		return null;
	}

	private InventoryItemListCache GetAllItemsByTemplateId(int itemTemplateId)
	{
		if (m_itemsByTemplateIdCache == null)
		{
			m_itemsByTemplateIdCache = new Dictionary<int, InventoryItemListCache>();
			foreach (InventoryItem item in Items)
			{
				InventoryItemTemplate template = item.GetTemplate();
				InventoryItemListCache inventoryItemListCache = m_itemsByTemplateIdCache.TryGetValue(template.Index);
				if (inventoryItemListCache == null)
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
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					inventoryItemListCache = new InventoryItemListCache();
					inventoryItemListCache.Count += item.Count;
					inventoryItemListCache.Items.Add(item);
					m_itemsByTemplateIdCache.Add(template.Index, inventoryItemListCache);
				}
				else
				{
					inventoryItemListCache.Count += item.Count;
					inventoryItemListCache.Items.Add(item);
				}
			}
		}
		return m_itemsByTemplateIdCache.TryGetValue(itemTemplateId);
	}

	private InventoryItemListCache GetAllItemsByType(InventoryItemType itemType)
	{
		if (m_itemsByTypeCache == null)
		{
			m_itemsByTypeCache = new Dictionary<InventoryItemType, InventoryItemListCache>();
			using (List<InventoryItem>.Enumerator enumerator = Items.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					InventoryItem current = enumerator.Current;
					InventoryItemTemplate template = current.GetTemplate();
					InventoryItemListCache inventoryItemListCache = m_itemsByTypeCache.TryGetValue(template.Type);
					if (inventoryItemListCache == null)
					{
						while (true)
						{
							switch (4)
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
						inventoryItemListCache = new InventoryItemListCache();
						inventoryItemListCache.Count += current.Count;
						inventoryItemListCache.Items.Add(current);
						m_itemsByTypeCache.Add(template.Type, inventoryItemListCache);
					}
					else
					{
						inventoryItemListCache.Items.Add(current);
					}
				}
				while (true)
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
		return m_itemsByTypeCache.TryGetValue(itemType);
	}

	public int GetItemCountByTemplateId(int itemTemplateId)
	{
		int result = 0;
		InventoryItemListCache allItemsByTemplateId = GetAllItemsByTemplateId(itemTemplateId);
		if (allItemsByTemplateId != null)
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
			result = allItemsByTemplateId.Count;
		}
		return result;
	}

	public int GetItemCountByType(InventoryItemType itemType)
	{
		int result = 0;
		InventoryItemListCache allItemsByType = GetAllItemsByType(itemType);
		if (allItemsByType != null)
		{
			while (true)
			{
				switch (5)
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
			result = allItemsByType.Count;
		}
		return result;
	}

	public bool HasItem(int itemTemplateId)
	{
		return GetItemByTemplateId(itemTemplateId) != null;
	}

	public void AddKarma(int karmaTemplateId)
	{
		if (Karmas.ContainsKey(karmaTemplateId))
		{
			return;
		}
		while (true)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			Karma karma = new Karma();
			karma.TemplateId = karmaTemplateId;
			Karma value = karma;
			Karmas[karmaTemplateId] = value;
			return;
		}
	}

	public void AddKarma(Karma karma)
	{
		if (Karmas.ContainsKey(karma.TemplateId))
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					Karmas[karma.TemplateId].Quantity += karma.Quantity;
					return;
				}
			}
		}
		Karmas[karma.TemplateId] = karma;
	}

	public void AddKarmas(IDictionary<int, Karma> karmas)
	{
		IEnumerator<KeyValuePair<int, Karma>> enumerator = karmas.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<int, Karma> current = enumerator.Current;
				if (Karmas.ContainsKey(current.Key))
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
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					Karmas[current.Key].Quantity += current.Value.Quantity;
				}
				else
				{
					Karmas[current.Key] = current.Value;
				}
			}
			while (true)
			{
				switch (5)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
		finally
		{
			if (enumerator != null)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						enumerator.Dispose();
						goto end_IL_00a6;
					}
				}
			}
			end_IL_00a6:;
		}
	}

	public void RemoveKarma(int karmaTemplateId)
	{
		Karmas.Remove(karmaTemplateId);
	}

	public void RemoveKarmas(List<int> karmaTemplateIds)
	{
		using (List<int>.Enumerator enumerator = karmaTemplateIds.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				int current = enumerator.Current;
				Karmas.Remove(current);
			}
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return;
				}
			}
		}
	}

	public Karma GetKarma(int karmaTemplateId)
	{
		Karmas.TryGetValue(karmaTemplateId, out Karma value);
		return value;
	}

	public void AddLoot(int itemId, Loot loot)
	{
		if (Loots.ContainsKey(itemId))
		{
			return;
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			Loots.Add(itemId, loot);
			return;
		}
	}

	public void RemoveLoot(int itemId)
	{
		Loots.Remove(itemId);
	}

	public Loot GetLoot(int itemId)
	{
		return Loots.TryGetValue(itemId);
	}

	public bool HasPendingItem(int itemTemplateId)
	{
		using (Dictionary<int, Loot>.ValueCollection.Enumerator enumerator = Loots.Values.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				Loot current = enumerator.Current;
				if (current.HasItem(itemTemplateId))
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
							return true;
						}
					}
				}
			}
			while (true)
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
				InventoryItem current = enumerator.Current;
				InventoryItemTemplate template = current.GetTemplate();
				if (template.Type.IsCharacterBound())
				{
					while (true)
					{
						switch (5)
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
					CharacterType bindingCharacterType = template.GetBindingCharacterType();
					AddCharacterItemDropBalanceValue(bindingCharacterType);
				}
			}
			while (true)
			{
				switch (7)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
	}

	public void AddCharacterItemDropBalanceValue(CharacterType bindingCharacterType, int delta = 1)
	{
		int characterItemDropBalance = LobbyGameplayData.Get().GetCharacterItemDropBalance();
		CharacterItemDropBalanceValues.TryGetValue(bindingCharacterType, out int value);
		CharacterItemDropBalanceValues[bindingCharacterType] = MathUtil.Clamp(value + delta, 0, characterItemDropBalance);
		bool flag = true;
		IEnumerator enumerator = Enum.GetValues(typeof(CharacterType)).GetEnumerator();
		try
		{
			while (true)
			{
				if (!enumerator.MoveNext())
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					break;
				}
				CharacterType key = (CharacterType)enumerator.Current;
				CharacterItemDropBalanceValues.TryGetValue(key, out int value2);
				if (value2 == 0)
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
							flag = false;
							goto end_IL_0056;
						}
					}
				}
			}
			end_IL_0056:;
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
						disposable.Dispose();
						goto end_IL_00aa;
					}
				}
			}
			end_IL_00aa:;
		}
		if (!flag)
		{
			return;
		}
		while (true)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			IEnumerator enumerator2 = Enum.GetValues(typeof(CharacterType)).GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					CharacterType key2 = (CharacterType)enumerator2.Current;
					CharacterItemDropBalanceValues.TryGetValue(key2, out int value3);
					CharacterItemDropBalanceValues[key2] = MathUtil.Clamp(value3 - 1, 0, characterItemDropBalance);
				}
				while (true)
				{
					switch (1)
					{
					default:
						return;
					case 0:
						break;
					}
				}
			}
			finally
			{
				IDisposable disposable2;
				if ((disposable2 = (enumerator2 as IDisposable)) != null)
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							break;
						default:
							disposable2.Dispose();
							goto end_IL_0145;
						}
					}
				}
				end_IL_0145:;
			}
		}
	}

	public int GetBalancedCharacterItemDropWeight(CharacterType characterType)
	{
		int characterItemDropBalance = LobbyGameplayData.Get().GetCharacterItemDropBalance();
		CharacterItemDropBalanceValues.TryGetValue(characterType, out int value);
		return MathUtil.Clamp(characterItemDropBalance - value, 0, characterItemDropBalance);
	}

	public object ShallowCopy()
	{
		return MemberwiseClone();
	}

	public InventoryComponent Clone()
	{
		string value = JsonConvert.SerializeObject(this);
		return JsonConvert.DeserializeObject<InventoryComponent>(value);
	}

	public InventoryComponent CloneForClient()
	{
		InventoryComponent inventoryComponent = new InventoryComponent();
		inventoryComponent.Items = Items;
		inventoryComponent.Loots = Loots;
		inventoryComponent.Karmas = Karmas;
		inventoryComponent.LastLockboxOpenTime = LastLockboxOpenTime;
		return inventoryComponent;
	}

	public void FlushCaches()
	{
		m_itemByIdCache = null;
		m_itemsByTemplateIdCache = null;
		m_itemsByTypeCache = null;
	}

	private void OnItemAdded(InventoryItem item)
	{
		if (m_itemByIdCache != null)
		{
			m_itemByIdCache[item.Id] = item;
		}
		if (m_itemsByTemplateIdCache != null)
		{
			while (true)
			{
				switch (5)
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
			InventoryItemListCache inventoryItemListCache = m_itemsByTemplateIdCache.TryGetValue(item.TemplateId);
			if (inventoryItemListCache == null)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				inventoryItemListCache = new InventoryItemListCache();
				inventoryItemListCache.Count += item.Count;
				inventoryItemListCache.Items.Add(item);
				m_itemsByTemplateIdCache.Add(item.TemplateId, inventoryItemListCache);
			}
			else
			{
				inventoryItemListCache.Count += item.Count;
				inventoryItemListCache.Items.Add(item);
			}
		}
		if (m_itemsByTypeCache == null)
		{
			return;
		}
		while (true)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			InventoryItemType type = item.GetTemplate().Type;
			InventoryItemListCache inventoryItemListCache2 = m_itemsByTypeCache.TryGetValue(type);
			if (inventoryItemListCache2 == null)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						inventoryItemListCache2 = new InventoryItemListCache();
						inventoryItemListCache2.Count += item.Count;
						inventoryItemListCache2.Items.Add(item);
						m_itemsByTypeCache.Add(type, inventoryItemListCache2);
						return;
					}
				}
			}
			inventoryItemListCache2.Items.Add(item);
			return;
		}
	}

	private void OnItemRemoved(InventoryItem item)
	{
		if (m_itemByIdCache != null)
		{
			while (true)
			{
				switch (2)
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
			m_itemByIdCache.Remove(item.Id);
		}
		if (m_itemsByTemplateIdCache != null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			InventoryItemListCache inventoryItemListCache = m_itemsByTemplateIdCache.TryGetValue(item.TemplateId);
			if (inventoryItemListCache != null)
			{
				while (true)
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
					while (true)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					m_itemsByTemplateIdCache.Remove(item.TemplateId);
				}
			}
		}
		if (m_itemsByTypeCache == null)
		{
			return;
		}
		while (true)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			InventoryItemType type = item.GetTemplate().Type;
			InventoryItemListCache inventoryItemListCache2 = m_itemsByTypeCache.TryGetValue(type);
			if (inventoryItemListCache2 == null)
			{
				inventoryItemListCache2.Items.Remove(item);
				if (inventoryItemListCache2.Items.Count == 0)
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						m_itemsByTypeCache.Remove(type);
						return;
					}
				}
				return;
			}
			return;
		}
	}
}

using System;

public static class InventoryWideDataExtensions
{
	public static bool IsDefaultStackableType(InventoryItemType itemType)
	{
		if (itemType != InventoryItemType.Material)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(InventoryWideDataExtensions.IsDefaultStackableType(InventoryItemType)).MethodHandle;
			}
			if (itemType != InventoryItemType.Currency && itemType != InventoryItemType.Experience)
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
				return itemType == InventoryItemType.FreelancerExpBonus;
			}
		}
		return true;
	}

	public static bool IsStackable(this InventoryItem item)
	{
		return InventoryWideData.Get().GetItemTemplate(item.TemplateId).IsStackable;
	}

	public static bool IsAutoConsumable(this InventoryItem item)
	{
		return InventoryWideData.Get().GetItemTemplate(item.TemplateId).IsAutoConsumable;
	}

	public static bool IsCollectable(this InventoryItem item)
	{
		return InventoryWideData.Get().GetItemTemplate(item.TemplateId).IsCollectable();
	}

	public static bool IsCollectable(this InventoryItemTemplate itemTemplate)
	{
		if (itemTemplate.Type != InventoryItemType.Material)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(InventoryItemTemplate.IsCollectable()).MethodHandle;
			}
			if (itemTemplate.Type != InventoryItemType.Currency)
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
				if (itemTemplate.Type != InventoryItemType.Experience)
				{
					return itemTemplate.Type != InventoryItemType.FreelancerExpBonus;
				}
			}
		}
		return false;
	}

	public static InventoryItemTemplate GetTemplate(this InventoryItem item)
	{
		return InventoryWideData.Get().GetItemTemplate(item.TemplateId);
	}

	public static CharacterType GetBindingCharacterType(this InventoryItemTemplate itemTemplate)
	{
		if (itemTemplate.Type == InventoryItemType.BannerID)
		{
			int num = itemTemplate.TypeSpecificData[0];
			GameBalanceVars.PlayerBanner playerBanner = GameBalanceVars.Get().PlayerBanners[num];
			return playerBanner.m_relatedCharacter;
		}
		if (itemTemplate.Type == InventoryItemType.TitleID)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(InventoryItemTemplate.GetBindingCharacterType()).MethodHandle;
			}
			int num2 = itemTemplate.TypeSpecificData[0];
			GameBalanceVars.PlayerTitle playerTitle = GameBalanceVars.Get().PlayerTitles[num2];
			return playerTitle.m_relatedCharacter;
		}
		if (itemTemplate.Type != InventoryItemType.Skin)
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
			if (itemTemplate.Type != InventoryItemType.Texture)
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
				if (itemTemplate.Type != InventoryItemType.Style && itemTemplate.Type != InventoryItemType.Taunt && itemTemplate.Type != InventoryItemType.Mod)
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
					if (itemTemplate.Type != InventoryItemType.AbilityVfxSwap)
					{
						return CharacterType.None;
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
			}
		}
		return (CharacterType)itemTemplate.TypeSpecificData[0];
	}
}

public static class InventoryWideDataExtensions
{
	public static bool IsDefaultStackableType(InventoryItemType itemType)
	{
		int result;
		if (itemType != InventoryItemType.Material)
		{
			if (itemType != InventoryItemType.Currency && itemType != InventoryItemType.Experience)
			{
				result = ((itemType == InventoryItemType.FreelancerExpBonus) ? 1 : 0);
				goto IL_0034;
			}
		}
		result = 1;
		goto IL_0034;
		IL_0034:
		return (byte)result != 0;
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
		int result;
		if (itemTemplate.Type != InventoryItemType.Material)
		{
			if (itemTemplate.Type != InventoryItemType.Currency)
			{
				if (itemTemplate.Type != InventoryItemType.Experience)
				{
					result = ((itemTemplate.Type != InventoryItemType.FreelancerExpBonus) ? 1 : 0);
					goto IL_004b;
				}
			}
		}
		result = 0;
		goto IL_004b;
		IL_004b:
		return (byte)result != 0;
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
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
				{
					int num2 = itemTemplate.TypeSpecificData[0];
					GameBalanceVars.PlayerTitle playerTitle = GameBalanceVars.Get().PlayerTitles[num2];
					return playerTitle.m_relatedCharacter;
				}
				}
			}
		}
		if (itemTemplate.Type != InventoryItemType.Skin)
		{
			if (itemTemplate.Type != InventoryItemType.Texture)
			{
				if (itemTemplate.Type != InventoryItemType.Style && itemTemplate.Type != InventoryItemType.Taunt && itemTemplate.Type != InventoryItemType.Mod)
				{
					if (itemTemplate.Type != InventoryItemType.AbilityVfxSwap)
					{
						return CharacterType.None;
					}
				}
			}
		}
		return (CharacterType)itemTemplate.TypeSpecificData[0];
	}
}

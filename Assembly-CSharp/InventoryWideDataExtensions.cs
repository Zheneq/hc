public static class InventoryWideDataExtensions
{
	public static bool IsDefaultStackableType(InventoryItemType itemType)
	{
		int result;
		if (itemType != InventoryItemType.Material)
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
			if (itemType != InventoryItemType.Currency && itemType != InventoryItemType.Experience)
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
			if (itemTemplate.Type != InventoryItemType.Currency)
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
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					int num2 = itemTemplate.TypeSpecificData[0];
					GameBalanceVars.PlayerTitle playerTitle = GameBalanceVars.Get().PlayerTitles[num2];
					return playerTitle.m_relatedCharacter;
				}
				}
			}
		}
		if (itemTemplate.Type != InventoryItemType.Skin)
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
			if (itemTemplate.Type != InventoryItemType.Texture)
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
				if (itemTemplate.Type != InventoryItemType.Style && itemTemplate.Type != InventoryItemType.Taunt && itemTemplate.Type != InventoryItemType.Mod)
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
					if (itemTemplate.Type != InventoryItemType.AbilityVfxSwap)
					{
						return CharacterType.None;
					}
					while (true)
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

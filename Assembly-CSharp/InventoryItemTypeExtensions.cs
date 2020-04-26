public static class InventoryItemTypeExtensions
{
	public static bool IsCharacterBound(this InventoryItemType type)
	{
		int result;
		if (type != InventoryItemType.TitleID)
		{
			if (type != InventoryItemType.BannerID)
			{
				if (type != InventoryItemType.Skin)
				{
					if (type != InventoryItemType.Texture)
					{
						if (type != InventoryItemType.Style)
						{
							if (type != InventoryItemType.Taunt)
							{
								if (type != InventoryItemType.Mod)
								{
									result = ((type == InventoryItemType.AbilityVfxSwap) ? 1 : 0);
									goto IL_0076;
								}
							}
						}
					}
				}
			}
		}
		result = 1;
		goto IL_0076;
		IL_0076:
		return (byte)result != 0;
	}
}

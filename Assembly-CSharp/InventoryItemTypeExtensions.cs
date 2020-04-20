using System;

public static class InventoryItemTypeExtensions
{
	public static bool IsCharacterBound(this InventoryItemType type)
	{
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
									return type == InventoryItemType.AbilityVfxSwap;
								}
							}
						}
					}
				}
			}
		}
		return true;
	}
}

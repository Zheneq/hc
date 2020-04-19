using System;

public static class InventoryItemTypeExtensions
{
	public static bool IsCharacterBound(this InventoryItemType type)
	{
		if (type != InventoryItemType.TitleID)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(InventoryItemType.IsCharacterBound()).MethodHandle;
			}
			if (type != InventoryItemType.BannerID)
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
				if (type != InventoryItemType.Skin)
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
					if (type != InventoryItemType.Texture)
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
						if (type != InventoryItemType.Style)
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
							if (type != InventoryItemType.Taunt)
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
								if (type != InventoryItemType.Mod)
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

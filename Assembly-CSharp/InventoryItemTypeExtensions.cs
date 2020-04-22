public static class InventoryItemTypeExtensions
{
	public static bool IsCharacterBound(this InventoryItemType type)
	{
		int result;
		if (type != InventoryItemType.TitleID)
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
			if (type != InventoryItemType.BannerID)
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
				if (type != InventoryItemType.Skin)
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
					if (type != InventoryItemType.Texture)
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
						if (type != InventoryItemType.Style)
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
							if (type != InventoryItemType.Taunt)
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
								if (type != InventoryItemType.Mod)
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

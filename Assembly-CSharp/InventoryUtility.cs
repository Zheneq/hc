using System;
using System.Collections.Generic;
using System.Security.Cryptography;

public static class InventoryUtility
{
	public static double GetRandomNumber(double minimum, double maximum)
	{
		RNGCryptoServiceProvider rNGCryptoServiceProvider = new RNGCryptoServiceProvider();
		byte[] array = new byte[8];
		rNGCryptoServiceProvider.GetNonZeroBytes(array);
		double num = (double)BitConverter.ToUInt64(array, 0) / 1.8446744073709552E+19;
		return num * (maximum - minimum) + minimum;
	}

	public static int GetRandomNumber(int minimum, int maximum)
	{
		return (int)GetRandomNumber((double)minimum, (double)maximum);
	}

	public static int RollWeights(float[] weights)
	{
		float num = 0f;
		float[] array = new float[weights.Length];
		for (int i = 0; i < weights.Length; i++)
		{
			array[i] = num + weights[i];
			num += weights[i];
		}
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			float num2 = (float)GetRandomNumber(0.0, num);
			int result = 0;
			int num3 = 0;
			while (true)
			{
				if (num3 < array.Length)
				{
					if (num2 <= array[num3])
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
						result = num3;
						break;
					}
					num3++;
					continue;
				}
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				break;
			}
			return result;
		}
	}

	public static string DisplayString(this InventoryItemType me)
	{
		switch (me)
		{
		case InventoryItemType.TitleID:
			return StringUtil.TR("Title", "Rewards");
		case InventoryItemType.BannerID:
			return StringUtil.TR("BannerEmblem", "Inventory");
		case InventoryItemType.Skin:
			return StringUtil.TR("Skin", "Rewards");
		case InventoryItemType.Texture:
			return StringUtil.TR("Texture", "Inventory");
		case InventoryItemType.Style:
			return StringUtil.TR("Skin", "Rewards");
		case InventoryItemType.Taunt:
			return StringUtil.TR("Taunt", "Rewards");
		case InventoryItemType.Mod:
			return StringUtil.TR("Mod", "Rewards");
		case InventoryItemType.Lockbox:
			return StringUtil.TR("LootMatrices", "Rewards");
		case InventoryItemType.Currency:
			return StringUtil.TR("Currency", "Inventory");
		case InventoryItemType.ChatEmoji:
			return StringUtil.TR("Emoticons", "Inventory");
		case InventoryItemType.Material:
			return StringUtil.TR("Material", "Rewards");
		case InventoryItemType.AbilityVfxSwap:
			return StringUtil.TR("AbilityEffect", "Rewards");
		case InventoryItemType.Experience:
			return StringUtil.TR("Experience", "Inventory");
		case InventoryItemType.FreelancerExpBonus:
			return StringUtil.TR("FreelancerExpBonus", "Global");
		case InventoryItemType.Overcon:
			return StringUtil.TR("Overcon", "Inventory");
		case InventoryItemType.Faction:
			return StringUtil.TR("Faction", "Inventory");
		case InventoryItemType.Unlock:
			return StringUtil.TR("Unlock", "Inventory");
		case InventoryItemType.Conveyance:
			return StringUtil.TR("Unlock", "Inventory");
		case InventoryItemType.LoadingScreenBackground:
			return StringUtil.TR("LoadingScreenBackground", "Inventory");
		default:
			return me.ToString() + "#NotLocalized";
		}
	}

	public static string GetColorHexString(this InventoryItemRarity itemRarity)
	{
		switch (itemRarity)
		{
		case InventoryItemRarity.Common:
			return "#c6c6c6";
		case InventoryItemRarity.Uncommon:
			return "#10cf10";
		case InventoryItemRarity.Rare:
			return "#008fff";
		case InventoryItemRarity.Epic:
			return "#9936ff";
		case InventoryItemRarity.Legendary:
			return "#ff8004";
		default:
			return string.Empty;
		}
	}

	public static string GetRarityString(this InventoryItemRarity itemRarity)
	{
		switch (itemRarity)
		{
		case InventoryItemRarity.Common:
			return StringUtil.TR("Common", "Rarity");
		case InventoryItemRarity.Uncommon:
			return StringUtil.TR("Uncommon", "Rarity");
		case InventoryItemRarity.Rare:
			return StringUtil.TR("Rare", "Rarity");
		case InventoryItemRarity.Epic:
			return StringUtil.TR("Epic", "Rarity");
		case InventoryItemRarity.Legendary:
			return StringUtil.TR("Legendary", "Rarity");
		default:
			return string.Empty;
		}
	}

	public static void PlaySound(this InventoryItemRarity itemRarity)
	{
		switch (itemRarity)
		{
		case InventoryItemRarity.Uncommon:
			UIFrontEnd.PlaySound(FrontEndButtonSounds.LockboxUnlockUncommon);
			break;
		case InventoryItemRarity.Rare:
			UIFrontEnd.PlaySound(FrontEndButtonSounds.LockboxUnlockRare);
			break;
		case InventoryItemRarity.Epic:
			UIFrontEnd.PlaySound(FrontEndButtonSounds.LockboxUnlockEpic);
			break;
		case InventoryItemRarity.Legendary:
			UIFrontEnd.PlaySound(FrontEndButtonSounds.LockboxUnlockLegendary);
			break;
		default:
			UIFrontEnd.PlaySound(FrontEndButtonSounds.LockboxUnlock);
			break;
		}
	}

	public static List<InventoryItem> MergeByTemplateId(this List<InventoryItem> targetItems, List<InventoryItem> sourceItems, bool forceStack = false)
	{
		using (List<InventoryItem>.Enumerator enumerator = sourceItems.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				InventoryItem sourceItem = enumerator.Current;
				InventoryItem inventoryItem = targetItems.Find((InventoryItem i) => i.TemplateId == sourceItem.TemplateId);
				if (inventoryItem != null)
				{
					if (!forceStack)
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
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						if (!inventoryItem.IsStackable())
						{
							goto IL_007b;
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
					inventoryItem.Count += sourceItem.Count;
					continue;
				}
				goto IL_007b;
				IL_007b:
				targetItems.Add((InventoryItem)sourceItem.Clone());
			}
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return targetItems;
				}
			}
		}
	}

	public static List<InventoryItemWithData> MergeByTemplateId(this List<InventoryItemWithData> targetItems, List<InventoryItemWithData> sourceItems, bool forceStack = false)
	{
		foreach (InventoryItemWithData sourceItem in sourceItems)
		{
			InventoryItemWithData inventoryItemWithData = targetItems.Find((InventoryItemWithData i) => i.Item.TemplateId == sourceItem.Item.TemplateId);
			if (inventoryItemWithData != null)
			{
				if (!forceStack)
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
					if (!inventoryItemWithData.Item.IsStackable())
					{
						goto IL_00a4;
					}
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
				}
				inventoryItemWithData.Item.Count += sourceItem.Item.Count;
				inventoryItemWithData.IsoGained += sourceItem.IsoGained;
				continue;
			}
			goto IL_00a4;
			IL_00a4:
			targetItems.Add(new InventoryItemWithData((InventoryItem)sourceItem.Item.Clone(), sourceItem.IsoGained));
		}
		return targetItems;
	}

	public static bool IsInventoryItemTypeCollectable(InventoryItemType itemType)
	{
		if (itemType != InventoryItemType.TitleID)
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
			if (itemType != InventoryItemType.BannerID)
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
				if (itemType != InventoryItemType.Skin && itemType != InventoryItemType.Texture)
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
					if (itemType != InventoryItemType.Style)
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
						if (itemType != InventoryItemType.Taunt)
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
							if (itemType != InventoryItemType.Mod)
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
								if (itemType != InventoryItemType.ChatEmoji)
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
									if (itemType != InventoryItemType.Overcon)
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
										if (itemType != InventoryItemType.AbilityVfxSwap)
										{
											return false;
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
						}
					}
				}
			}
		}
		return true;
	}
}

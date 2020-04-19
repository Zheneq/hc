using System;
using System.Collections.Generic;
using System.Security.Cryptography;

public static class InventoryUtility
{
	public static double GetRandomNumber(double minimum, double maximum)
	{
		RNGCryptoServiceProvider rngcryptoServiceProvider = new RNGCryptoServiceProvider();
		byte[] array = new byte[8];
		rngcryptoServiceProvider.GetNonZeroBytes(array);
		double num = BitConverter.ToUInt64(array, 0) / 1.8446744073709552E+19;
		return num * (maximum - minimum) + minimum;
	}

	public static int GetRandomNumber(int minimum, int maximum)
	{
		return (int)InventoryUtility.GetRandomNumber((double)minimum, (double)maximum);
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
			RuntimeMethodHandle runtimeMethodHandle = methodof(InventoryUtility.RollWeights(float[])).MethodHandle;
		}
		float num2 = (float)InventoryUtility.GetRandomNumber(0.0, (double)num);
		int result = 0;
		for (int j = 0; j < array.Length; j++)
		{
			if (num2 <= array[j])
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
				result = j;
				return result;
			}
		}
		for (;;)
		{
			switch (7)
			{
			case 0:
				continue;
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
		case InventoryItemType.Overcon:
			return StringUtil.TR("Overcon", "Inventory");
		case InventoryItemType.Faction:
			return StringUtil.TR("Faction", "Inventory");
		case InventoryItemType.Unlock:
			return StringUtil.TR("Unlock", "Inventory");
		case InventoryItemType.Conveyance:
			return StringUtil.TR("Unlock", "Inventory");
		case InventoryItemType.FreelancerExpBonus:
			return StringUtil.TR("FreelancerExpBonus", "Global");
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
		}
		return string.Empty;
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
		}
		return string.Empty;
	}

	public static void PlaySound(this InventoryItemRarity itemRarity)
	{
		switch (itemRarity)
		{
		case InventoryItemRarity.Uncommon:
			UIFrontEnd.PlaySound(FrontEndButtonSounds.LockboxUnlockUncommon);
			return;
		case InventoryItemRarity.Rare:
			UIFrontEnd.PlaySound(FrontEndButtonSounds.LockboxUnlockRare);
			return;
		case InventoryItemRarity.Epic:
			UIFrontEnd.PlaySound(FrontEndButtonSounds.LockboxUnlockEpic);
			return;
		case InventoryItemRarity.Legendary:
			UIFrontEnd.PlaySound(FrontEndButtonSounds.LockboxUnlockLegendary);
			return;
		}
		UIFrontEnd.PlaySound(FrontEndButtonSounds.LockboxUnlock);
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
							RuntimeMethodHandle runtimeMethodHandle = methodof(List<InventoryItem>.MergeByTemplateId(List<InventoryItem>, bool)).MethodHandle;
						}
						if (!inventoryItem.IsStackable())
						{
							goto IL_7B;
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
					inventoryItem.Count += sourceItem.Count;
					continue;
				}
				IL_7B:
				targetItems.Add((InventoryItem)sourceItem.Clone());
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
		}
		return targetItems;
	}

	public static List<InventoryItemWithData> MergeByTemplateId(this List<InventoryItemWithData> targetItems, List<InventoryItemWithData> sourceItems, bool forceStack = false)
	{
		using (List<InventoryItemWithData>.Enumerator enumerator = sourceItems.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				InventoryItemWithData sourceItem = enumerator.Current;
				InventoryItemWithData inventoryItemWithData = targetItems.Find((InventoryItemWithData i) => i.Item.TemplateId == sourceItem.Item.TemplateId);
				if (inventoryItemWithData != null)
				{
					if (!forceStack)
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
							RuntimeMethodHandle runtimeMethodHandle = methodof(List<InventoryItemWithData>.MergeByTemplateId(List<InventoryItemWithData>, bool)).MethodHandle;
						}
						if (!inventoryItemWithData.Item.IsStackable())
						{
							goto IL_A4;
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
					inventoryItemWithData.Item.Count += sourceItem.Item.Count;
					inventoryItemWithData.IsoGained += sourceItem.IsoGained;
					continue;
				}
				IL_A4:
				targetItems.Add(new InventoryItemWithData((InventoryItem)sourceItem.Item.Clone(), sourceItem.IsoGained));
			}
		}
		return targetItems;
	}

	public static bool IsInventoryItemTypeCollectable(InventoryItemType itemType)
	{
		if (itemType != InventoryItemType.TitleID)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(InventoryUtility.IsInventoryItemTypeCollectable(InventoryItemType)).MethodHandle;
			}
			if (itemType != InventoryItemType.BannerID)
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
				if (itemType != InventoryItemType.Skin && itemType != InventoryItemType.Texture)
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
					if (itemType != InventoryItemType.Style)
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
						if (itemType != InventoryItemType.Taunt)
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
							if (itemType != InventoryItemType.Mod)
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
								if (itemType != InventoryItemType.ChatEmoji)
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
									if (itemType != InventoryItemType.Overcon)
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
										if (itemType != InventoryItemType.AbilityVfxSwap)
										{
											return false;
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
						}
					}
				}
			}
		}
		return true;
	}
}

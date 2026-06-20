using System.Collections.Generic;

public class UIStoreFeaturedPanel : UICashShopPanelBase
{
	private class ItemPair
	{
		public UIPurchaseableItem Item;

		public int SortOrder;
	}

	protected override UIPurchaseableItem[] GetPurchasableItems()
	{
		List<ItemPair> list = new List<ItemPair>();
		List<CashShopFeaturedItem> featuredItems = StoreWideData.Get().m_featuredItems;
		for (int i = 0; i < featuredItems.Count; i++)
		{
			if (!QuestWideData.AreConditionsMet(featuredItems[i].Prerequisites.Conditions, featuredItems[i].Prerequisites.LogicStatement))
			{
				continue;
			}
			UIPurchaseableItem uIPurchaseableItem = new UIPurchaseableItem();
			uIPurchaseableItem.m_purchaseForCash = true;
			uIPurchaseableItem.m_itemType = featuredItems[i].ItemType;
			ItemPair itemPair = new ItemPair();
			itemPair.Item = uIPurchaseableItem;
			itemPair.SortOrder = featuredItems[i].SortOrder * featuredItems.Count + i;
			switch (uIPurchaseableItem.m_itemType)
			{
			case PurchaseItemType.Character:
				if (GameManager.Get().IsCharacterAllowedForPlayers(featuredItems[i].CharacterType))
				{
					uIPurchaseableItem.m_charLink = GameWideData.Get().GetCharacterResourceLink(featuredItems[i].CharacterType);
					list.Add(itemPair);
				}
				break;
			case PurchaseItemType.GGBoost:
			{
				int num3 = 0;
				while (true)
				{
					if (num3 < GameWideData.Get().m_ggPackData.m_ggPacks.Length)
					{
						GGPack gGPack = GameWideData.Get().m_ggPackData.m_ggPacks[num3];
						if (gGPack.Index == featuredItems[i].TypeSpecificData)
						{
							uIPurchaseableItem.m_ggPack = gGPack;
							list.Add(itemPair);
							break;
						}
						num3++;
						continue;
					}
					break;
				}
				break;
			}
			case PurchaseItemType.Game:
			{
				for (int l = 0; l < GameWideData.Get().m_gamePackData.m_gamePacks.Length; l++)
				{
					GamePack gamePack = GameWideData.Get().m_gamePackData.m_gamePacks[l];
					if (gamePack.Index == featuredItems[i].TypeSpecificData)
					{
						uIPurchaseableItem.m_gamePack = gamePack;
						list.Add(itemPair);
						break;
					}
				}
				break;
			}
			case PurchaseItemType.LootMatrixPack:
			{
				int num = 0;
				while (true)
				{
					if (num < GameWideData.Get().m_lootMatrixPackData.m_lootMatrixPacks.Length)
					{
						LootMatrixPack lootMatrixPack = GameWideData.Get().m_lootMatrixPackData.m_lootMatrixPacks[num];
						if (lootMatrixPack.IsBundle)
						{
							List<InventoryItemTemplate> templatesFromLootMatrixPack = InventoryWideData.GetTemplatesFromLootMatrixPack(lootMatrixPack);
							if (templatesFromLootMatrixPack == null)
							{
								goto IL_03f6;
							}
							if (templatesFromLootMatrixPack.Count == 0)
							{
								goto IL_03f6;
							}
							List<LootTable> list2 = new List<LootTable>();
							for (int j = 0; j < templatesFromLootMatrixPack.Count; j++)
							{
								list2.Add(InventoryWideData.Get().GetLootTable(templatesFromLootMatrixPack[j].TypeSpecificData[0]));
							}
							if (list2.Count == 0)
							{
								goto IL_03f6;
							}
							if (list2[0] == null)
							{
								goto IL_03f6;
							}
							List<int> list3 = new List<int>();
							for (int k = 0; k < list2.Count; k++)
							{
								list3.AddRange(InventoryWideData.Get().GetAllItemTemplateIDsFromLootTable(list2[k].Index));
							}
							if (templatesFromLootMatrixPack.Count == 0)
							{
								goto IL_03f6;
							}
							bool flag = true;
							int num2 = 0;
							while (true)
							{
								if (num2 < list3.Count)
								{
									if (!InventoryWideData.IsOwned(list3[num2]))
									{
										flag = false;
										break;
									}
									num2++;
									continue;
								}
								break;
							}
							if (flag)
							{
								goto IL_03f6;
							}
						}
						if (lootMatrixPack.Index == featuredItems[i].TypeSpecificData)
						{
							uIPurchaseableItem.m_lootMatrixPack = lootMatrixPack;
							list.Add(itemPair);
							break;
						}
						goto IL_03f6;
					}
					break;
					IL_03f6:
					num++;
				}
				break;
			}
			case PurchaseItemType.Overcon:
				uIPurchaseableItem.m_overconID = featuredItems[i].TypeSpecificData;
				uIPurchaseableItem.m_purchaseForCash = featuredItems[i].IsCash;
				list.Add(itemPair);
				break;
			case PurchaseItemType.Tint:
				uIPurchaseableItem.m_charLink = GameWideData.Get().GetCharacterResourceLink(featuredItems[i].CharacterType);
				uIPurchaseableItem.m_tintIndex = featuredItems[i].TypeSpecificData;
				uIPurchaseableItem.m_skinIndex = featuredItems[i].SkinIndex;
				uIPurchaseableItem.m_textureIndex = featuredItems[i].PatternIndex;
				uIPurchaseableItem.m_purchaseForCash = featuredItems[i].IsCash;
				list.Add(itemPair);
				break;
			case PurchaseItemType.Taunt:
				uIPurchaseableItem.m_charLink = GameWideData.Get().GetCharacterResourceLink(featuredItems[i].CharacterType);
				uIPurchaseableItem.m_tauntIndex = featuredItems[i].TypeSpecificData;
				uIPurchaseableItem.m_purchaseForCash = featuredItems[i].IsCash;
				list.Add(itemPair);
				break;
			case PurchaseItemType.InventoryItem:
				uIPurchaseableItem.m_inventoryTemplateId = featuredItems[i].TypeSpecificData;
				uIPurchaseableItem.m_purchaseForCash = featuredItems[i].IsCash;
				uIPurchaseableItem.m_overlayText = featuredItems[i].TextOverlay;
				list.Add(itemPair);
				break;
			case PurchaseItemType.Banner:
				uIPurchaseableItem.m_bannerID = featuredItems[i].TypeSpecificData;
				uIPurchaseableItem.m_purchaseForCash = featuredItems[i].IsCash;
				list.Add(itemPair);
				break;
			case PurchaseItemType.Title:
				uIPurchaseableItem.m_titleID = featuredItems[i].TypeSpecificData;
				uIPurchaseableItem.m_purchaseForCash = featuredItems[i].IsCash;
				list.Add(itemPair);
				break;
			case PurchaseItemType.Emoticon:
				uIPurchaseableItem.m_emoticonID = featuredItems[i].TypeSpecificData;
				uIPurchaseableItem.m_purchaseForCash = featuredItems[i].IsCash;
				list.Add(itemPair);
				break;
			case PurchaseItemType.LoadingScreenBackground:
				uIPurchaseableItem.m_loadingScreenBackgroundId = featuredItems[i].TypeSpecificData;
				uIPurchaseableItem.m_purchaseForCash = featuredItems[i].IsCash;
				list.Add(itemPair);
				break;
			}
		}
		
		list.Sort(((ItemPair x, ItemPair y) => x.SortOrder - y.SortOrder));
		List<UIPurchaseableItem> list4 = new List<UIPurchaseableItem>();
		for (int m = 0; m < list.Count; m++)
		{
			list4.Add(list[m].Item);
		}
		while (true)
		{
			return list4.ToArray();
		}
	}

	public void Refresh()
	{
		Reinitialize();
	}
}

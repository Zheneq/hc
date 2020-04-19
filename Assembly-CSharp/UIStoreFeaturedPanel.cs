using System;
using System.Collections.Generic;

public class UIStoreFeaturedPanel : UICashShopPanelBase
{
	protected override UIPurchaseableItem[] GetPurchasableItems()
	{
		List<UIStoreFeaturedPanel.ItemPair> list = new List<UIStoreFeaturedPanel.ItemPair>();
		List<CashShopFeaturedItem> featuredItems = StoreWideData.Get().m_featuredItems;
		for (int i = 0; i < featuredItems.Count; i++)
		{
			if (!QuestWideData.AreConditionsMet(featuredItems[i].Prerequisites.Conditions, featuredItems[i].Prerequisites.LogicStatement, false))
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(UIStoreFeaturedPanel.GetPurchasableItems()).MethodHandle;
				}
			}
			else
			{
				UIPurchaseableItem uipurchaseableItem = new UIPurchaseableItem();
				uipurchaseableItem.m_purchaseForCash = true;
				uipurchaseableItem.m_itemType = featuredItems[i].ItemType;
				UIStoreFeaturedPanel.ItemPair itemPair = new UIStoreFeaturedPanel.ItemPair();
				itemPair.Item = uipurchaseableItem;
				itemPair.SortOrder = featuredItems[i].SortOrder * featuredItems.Count + i;
				switch (uipurchaseableItem.m_itemType)
				{
				case PurchaseItemType.Overcon:
					uipurchaseableItem.m_overconID = featuredItems[i].TypeSpecificData;
					uipurchaseableItem.m_purchaseForCash = featuredItems[i].IsCash;
					list.Add(itemPair);
					break;
				case PurchaseItemType.Character:
					if (GameManager.Get().IsCharacterAllowedForPlayers(featuredItems[i].CharacterType))
					{
						uipurchaseableItem.m_charLink = GameWideData.Get().GetCharacterResourceLink(featuredItems[i].CharacterType);
						list.Add(itemPair);
					}
					break;
				case PurchaseItemType.Tint:
					uipurchaseableItem.m_charLink = GameWideData.Get().GetCharacterResourceLink(featuredItems[i].CharacterType);
					uipurchaseableItem.m_tintIndex = featuredItems[i].TypeSpecificData;
					uipurchaseableItem.m_skinIndex = featuredItems[i].SkinIndex;
					uipurchaseableItem.m_textureIndex = featuredItems[i].PatternIndex;
					uipurchaseableItem.m_purchaseForCash = featuredItems[i].IsCash;
					list.Add(itemPair);
					break;
				case PurchaseItemType.GGBoost:
					for (int j = 0; j < GameWideData.Get().m_ggPackData.m_ggPacks.Length; j++)
					{
						GGPack ggpack = GameWideData.Get().m_ggPackData.m_ggPacks[j];
						if (ggpack.Index == featuredItems[i].TypeSpecificData)
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
							uipurchaseableItem.m_ggPack = ggpack;
							list.Add(itemPair);
							goto IL_638;
						}
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
					break;
				case PurchaseItemType.Taunt:
					uipurchaseableItem.m_charLink = GameWideData.Get().GetCharacterResourceLink(featuredItems[i].CharacterType);
					uipurchaseableItem.m_tauntIndex = featuredItems[i].TypeSpecificData;
					uipurchaseableItem.m_purchaseForCash = featuredItems[i].IsCash;
					list.Add(itemPair);
					break;
				case PurchaseItemType.InventoryItem:
					uipurchaseableItem.m_inventoryTemplateId = featuredItems[i].TypeSpecificData;
					uipurchaseableItem.m_purchaseForCash = featuredItems[i].IsCash;
					uipurchaseableItem.m_overlayText = featuredItems[i].TextOverlay;
					list.Add(itemPair);
					break;
				case PurchaseItemType.Game:
					for (int k = 0; k < GameWideData.Get().m_gamePackData.m_gamePacks.Length; k++)
					{
						GamePack gamePack = GameWideData.Get().m_gamePackData.m_gamePacks[k];
						if (gamePack.Index == featuredItems[i].TypeSpecificData)
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
							uipurchaseableItem.m_gamePack = gamePack;
							list.Add(itemPair);
							break;
						}
					}
					break;
				case PurchaseItemType.LootMatrixPack:
				{
					int l = 0;
					while (l < GameWideData.Get().m_lootMatrixPackData.m_lootMatrixPacks.Length)
					{
						LootMatrixPack lootMatrixPack = GameWideData.Get().m_lootMatrixPackData.m_lootMatrixPacks[l];
						if (!lootMatrixPack.IsBundle)
						{
							goto IL_3C3;
						}
						for (;;)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							break;
						}
						List<InventoryItemTemplate> templatesFromLootMatrixPack = InventoryWideData.GetTemplatesFromLootMatrixPack(lootMatrixPack);
						if (templatesFromLootMatrixPack != null)
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
							if (templatesFromLootMatrixPack.Count == 0)
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
							}
							else
							{
								List<LootTable> list2 = new List<LootTable>();
								for (int m = 0; m < templatesFromLootMatrixPack.Count; m++)
								{
									list2.Add(InventoryWideData.Get().GetLootTable(templatesFromLootMatrixPack[m].TypeSpecificData[0]));
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
								if (list2.Count != 0)
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
									if (list2[0] == null)
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
									}
									else
									{
										List<int> list3 = new List<int>();
										for (int n = 0; n < list2.Count; n++)
										{
											list3.AddRange(InventoryWideData.Get().GetAllItemTemplateIDsFromLootTable(list2[n].Index));
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
										if (templatesFromLootMatrixPack.Count == 0)
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
										}
										else
										{
											bool flag = true;
											int num = 0;
											while (num < list3.Count)
											{
												if (!InventoryWideData.IsOwned(list3[num]))
												{
													flag = false;
													IL_3BD:
													if (flag)
													{
														goto IL_3F6;
													}
													goto IL_3C3;
												}
												else
												{
													num++;
												}
											}
											for (;;)
											{
												switch (3)
												{
												case 0:
													continue;
												}
												goto IL_3BD;
											}
										}
									}
								}
							}
						}
						IL_3F6:
						l++;
						continue;
						IL_3C3:
						if (lootMatrixPack.Index == featuredItems[i].TypeSpecificData)
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
							uipurchaseableItem.m_lootMatrixPack = lootMatrixPack;
							list.Add(itemPair);
							goto IL_638;
						}
						goto IL_3F6;
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
					break;
				}
				case PurchaseItemType.Banner:
					uipurchaseableItem.m_bannerID = featuredItems[i].TypeSpecificData;
					uipurchaseableItem.m_purchaseForCash = featuredItems[i].IsCash;
					list.Add(itemPair);
					break;
				case PurchaseItemType.Title:
					uipurchaseableItem.m_titleID = featuredItems[i].TypeSpecificData;
					uipurchaseableItem.m_purchaseForCash = featuredItems[i].IsCash;
					list.Add(itemPair);
					break;
				case PurchaseItemType.Emoticon:
					uipurchaseableItem.m_emoticonID = featuredItems[i].TypeSpecificData;
					uipurchaseableItem.m_purchaseForCash = featuredItems[i].IsCash;
					list.Add(itemPair);
					break;
				case PurchaseItemType.LoadingScreenBackground:
					uipurchaseableItem.m_loadingScreenBackgroundId = featuredItems[i].TypeSpecificData;
					uipurchaseableItem.m_purchaseForCash = featuredItems[i].IsCash;
					list.Add(itemPair);
					break;
				}
			}
			IL_638:;
		}
		List<UIStoreFeaturedPanel.ItemPair> list4 = list;
		if (UIStoreFeaturedPanel.<>f__am$cache0 == null)
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
			UIStoreFeaturedPanel.<>f__am$cache0 = ((UIStoreFeaturedPanel.ItemPair x, UIStoreFeaturedPanel.ItemPair y) => x.SortOrder - y.SortOrder);
		}
		list4.Sort(UIStoreFeaturedPanel.<>f__am$cache0);
		List<UIPurchaseableItem> list5 = new List<UIPurchaseableItem>();
		for (int num2 = 0; num2 < list.Count; num2++)
		{
			list5.Add(list[num2].Item);
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
		return list5.ToArray();
	}

	public void Refresh()
	{
		base.Reinitialize();
	}

	private class ItemPair
	{
		public UIPurchaseableItem Item;

		public int SortOrder;
	}
}

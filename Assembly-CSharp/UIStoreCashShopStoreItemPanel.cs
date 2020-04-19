using System;
using System.Collections.Generic;

public class UIStoreCashShopStoreItemPanel : UICashShopPanelBase
{
	protected override UIPurchaseableItem[] GetPurchasableItems()
	{
		List<UIPurchaseableItem> list = new List<UIPurchaseableItem>();
		GameBalanceVars.StoreItemForPurchase[] storeItemsForPurchase = GameBalanceVars.Get().StoreItemsForPurchase;
		for (int i = 0; i < storeItemsForPurchase.Length; i++)
		{
			string accountCurrency = HydrogenConfig.Get().Ticket.AccountCurrency;
			if (storeItemsForPurchase[i].Prices.GetPrice(accountCurrency) > 0f)
			{
				if (GameBalanceVarsExtensions.MeetsVisibilityConditions(storeItemsForPurchase[i]))
				{
					list.Add(new UIPurchaseableItem
					{
						m_purchaseForCash = true,
						m_itemType = PurchaseItemType.InventoryItem,
						m_inventoryTemplateId = storeItemsForPurchase[i].m_itemTemplateId,
						m_overlayText = storeItemsForPurchase[i].m_overlayText,
						m_sortOrder = storeItemsForPurchase[i].m_sortOrder * storeItemsForPurchase.Length + i
					});
				}
			}
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
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(UIStoreCashShopStoreItemPanel.GetPurchasableItems()).MethodHandle;
		}
		List<UIPurchaseableItem> list2 = list;
		if (UIStoreCashShopStoreItemPanel.<>f__am$cache0 == null)
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
			UIStoreCashShopStoreItemPanel.<>f__am$cache0 = ((UIPurchaseableItem a, UIPurchaseableItem b) => a.m_sortOrder - b.m_sortOrder);
		}
		list2.Sort(UIStoreCashShopStoreItemPanel.<>f__am$cache0);
		return list.ToArray();
	}
}

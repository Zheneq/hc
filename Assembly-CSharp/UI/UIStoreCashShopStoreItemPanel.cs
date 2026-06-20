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
			if (!(storeItemsForPurchase[i].Prices.GetPrice(accountCurrency) <= 0f) && GameBalanceVarsExtensions.MeetsVisibilityConditions(storeItemsForPurchase[i]))
			{
				UIPurchaseableItem uIPurchaseableItem = new UIPurchaseableItem();
				uIPurchaseableItem.m_purchaseForCash = true;
				uIPurchaseableItem.m_itemType = PurchaseItemType.InventoryItem;
				uIPurchaseableItem.m_inventoryTemplateId = storeItemsForPurchase[i].m_itemTemplateId;
				uIPurchaseableItem.m_overlayText = storeItemsForPurchase[i].m_overlayText;
				uIPurchaseableItem.m_sortOrder = storeItemsForPurchase[i].m_sortOrder * storeItemsForPurchase.Length + i;
				list.Add(uIPurchaseableItem);
			}
		}
		while (true)
		{
			
			list.Sort(((UIPurchaseableItem a, UIPurchaseableItem b) => a.m_sortOrder - b.m_sortOrder));
			return list.ToArray();
		}
	}
}

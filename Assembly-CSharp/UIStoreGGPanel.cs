using System;

public class UIStoreGGPanel : UICashShopPanelBase
{
	protected override UIPurchaseableItem[] GetPurchasableItems()
	{
		GGPack[] ggPacks = GameWideData.Get().m_ggPackData.m_ggPacks;
		UIPurchaseableItem[] array = new UIPurchaseableItem[ggPacks.Length];
		for (int i = 0; i < ggPacks.Length; i++)
		{
			array[i] = new UIPurchaseableItem();
			array[i].m_purchaseForCash = true;
			array[i].m_itemType = PurchaseItemType.GGBoost;
			array[i].m_ggPack = ggPacks[i];
		}
		return array;
	}
}

using System;
using System.Collections.Generic;

public class UIStoreLootMatrixPanel : UICashShopPanelBase
{
	private DateTime m_nextExpireDateTime;

	protected override UIPurchaseableItem[] GetPurchasableItems()
	{
		m_nextExpireDateTime = DateTime.MaxValue;
		DateTime lastPacificTimePriceRequestWithServerTimeOffset = CommerceClient.Get().LastPacificTimePriceRequestWithServerTimeOffset;
		List<UIPurchaseableItem> list = new List<UIPurchaseableItem>();
		LootMatrixPack[] lootMatrixPacks = GameWideData.Get().m_lootMatrixPackData.m_lootMatrixPacks;
		for (int i = 0; i < lootMatrixPacks.Length; i++)
		{
			LootMatrixPack lootMatrixPack = lootMatrixPacks[i];
			bool flag = lootMatrixPack.IsInEvent();
			if (flag)
			{
				if (!lootMatrixPack.EventHidden)
				{
					goto IL_007c;
				}
			}
			if (!flag && !lootMatrixPack.NonEventHidden)
			{
				goto IL_007c;
			}
			goto IL_00c2;
			IL_00c2:
			if (lootMatrixPack.NonEventHidden == lootMatrixPack.EventHidden)
			{
				continue;
			}
			DateTime dateTime;
			if (flag)
			{
				if (lootMatrixPack.EventEndPacific.IsNullOrEmpty())
				{
					continue;
				}
				dateTime = Convert.ToDateTime(lootMatrixPack.EventEndPacific);
			}
			else
			{
				if (lootMatrixPack.EventStartPacific.IsNullOrEmpty())
				{
					continue;
				}
				dateTime = Convert.ToDateTime(lootMatrixPack.EventStartPacific);
			}
			if (dateTime > lastPacificTimePriceRequestWithServerTimeOffset)
			{
				if (dateTime < m_nextExpireDateTime)
				{
					m_nextExpireDateTime = dateTime;
				}
			}
			continue;
			IL_007c:
			if (!lootMatrixPack.IsBundle)
			{
				UIPurchaseableItem uIPurchaseableItem = new UIPurchaseableItem();
				uIPurchaseableItem.m_purchaseForCash = true;
				uIPurchaseableItem.m_itemType = PurchaseItemType.LootMatrixPack;
				uIPurchaseableItem.m_lootMatrixPack = lootMatrixPack;
				uIPurchaseableItem.m_sortOrder = lootMatrixPack.SortOrder * lootMatrixPacks.Length + i;
				list.Add(uIPurchaseableItem);
			}
			goto IL_00c2;
		}
		while (true)
		{
			
			list.Sort(((UIPurchaseableItem a, UIPurchaseableItem b) => a.m_sortOrder - b.m_sortOrder));
			return list.ToArray();
		}
	}

	private void Update()
	{
		DateTime lastPacificTimePriceRequestWithServerTimeOffset = CommerceClient.Get().LastPacificTimePriceRequestWithServerTimeOffset;
		if (!(lastPacificTimePriceRequestWithServerTimeOffset >= m_nextExpireDateTime))
		{
			return;
		}
		while (true)
		{
			Reinitialize();
			return;
		}
	}
}

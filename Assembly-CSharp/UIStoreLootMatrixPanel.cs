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
				while (true)
				{
					switch (7)
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
			}
			if (!flag && !lootMatrixPack.NonEventHidden)
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
				goto IL_007c;
			}
			goto IL_00c2;
			IL_00c2:
			if (lootMatrixPack.NonEventHidden == lootMatrixPack.EventHidden)
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
				continue;
			}
			DateTime dateTime;
			if (flag)
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
				if (lootMatrixPack.EventEndPacific.IsNullOrEmpty())
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
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
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
			switch (2)
			{
			case 0:
				continue;
			}
			if (_003C_003Ef__am_0024cache0 == null)
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
				_003C_003Ef__am_0024cache0 = ((UIPurchaseableItem a, UIPurchaseableItem b) => a.m_sortOrder - b.m_sortOrder);
			}
			list.Sort(_003C_003Ef__am_0024cache0);
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
			switch (4)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			Reinitialize();
			return;
		}
	}
}

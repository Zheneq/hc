using System;
using System.Collections.Generic;

public class UIStoreLootMatrixPanel : UICashShopPanelBase
{
	private DateTime m_nextExpireDateTime;

	protected override UIPurchaseableItem[] GetPurchasableItems()
	{
		this.m_nextExpireDateTime = DateTime.MaxValue;
		DateTime lastPacificTimePriceRequestWithServerTimeOffset = CommerceClient.Get().LastPacificTimePriceRequestWithServerTimeOffset;
		List<UIPurchaseableItem> list = new List<UIPurchaseableItem>();
		LootMatrixPack[] lootMatrixPacks = GameWideData.Get().m_lootMatrixPackData.m_lootMatrixPacks;
		int i = 0;
		while (i < lootMatrixPacks.Length)
		{
			LootMatrixPack lootMatrixPack = lootMatrixPacks[i];
			bool flag = lootMatrixPack.IsInEvent();
			if (!flag)
			{
				goto IL_65;
			}
			if (!lootMatrixPack.EventHidden)
			{
				goto IL_7C;
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIStoreLootMatrixPanel.GetPurchasableItems()).MethodHandle;
				goto IL_65;
			}
			goto IL_65;
			IL_C2:
			if (lootMatrixPack.NonEventHidden == lootMatrixPack.EventHidden)
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
			}
			else
			{
				DateTime dateTime;
				if (flag)
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
					if (lootMatrixPack.EventEndPacific.IsNullOrEmpty())
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
						goto IL_16C;
					}
					dateTime = Convert.ToDateTime(lootMatrixPack.EventEndPacific);
				}
				else
				{
					if (lootMatrixPack.EventStartPacific.IsNullOrEmpty())
					{
						goto IL_16C;
					}
					dateTime = Convert.ToDateTime(lootMatrixPack.EventStartPacific);
				}
				if (dateTime > lastPacificTimePriceRequestWithServerTimeOffset)
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
					if (dateTime < this.m_nextExpireDateTime)
					{
						this.m_nextExpireDateTime = dateTime;
					}
				}
			}
			IL_16C:
			i++;
			continue;
			IL_65:
			if (flag || lootMatrixPack.NonEventHidden)
			{
				goto IL_C2;
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
			IL_7C:
			if (!lootMatrixPack.IsBundle)
			{
				list.Add(new UIPurchaseableItem
				{
					m_purchaseForCash = true,
					m_itemType = PurchaseItemType.LootMatrixPack,
					m_lootMatrixPack = lootMatrixPack,
					m_sortOrder = lootMatrixPack.SortOrder * lootMatrixPacks.Length + i
				});
				goto IL_C2;
			}
			goto IL_C2;
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
		List<UIPurchaseableItem> list2 = list;
		if (UIStoreLootMatrixPanel.<>f__am$cache0 == null)
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
			UIStoreLootMatrixPanel.<>f__am$cache0 = ((UIPurchaseableItem a, UIPurchaseableItem b) => a.m_sortOrder - b.m_sortOrder);
		}
		list2.Sort(UIStoreLootMatrixPanel.<>f__am$cache0);
		return list.ToArray();
	}

	private void Update()
	{
		DateTime lastPacificTimePriceRequestWithServerTimeOffset = CommerceClient.Get().LastPacificTimePriceRequestWithServerTimeOffset;
		if (lastPacificTimePriceRequestWithServerTimeOffset >= this.m_nextExpireDateTime)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIStoreLootMatrixPanel.Update()).MethodHandle;
			}
			base.Reinitialize();
		}
	}
}

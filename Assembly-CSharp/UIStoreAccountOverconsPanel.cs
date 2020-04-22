using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class UIStoreAccountOverconsPanel : UIStoreBaseInventoryPanel
{
	public Toggle m_ownedToggle;

	protected new void Start()
	{
		base.Start();
		UITooltipHoverObject component = m_ownedToggle.GetComponent<UITooltipHoverObject>();
		
		component.Setup(TooltipType.Simple, delegate(UITooltipBase tooltip)
			{
				UISimpleTooltip uISimpleTooltip = (UISimpleTooltip)tooltip;
				uISimpleTooltip.Setup(StringUtil.TR("Owned", "Store"));
				return true;
			});
	}

	protected override GameBalanceVars.PlayerUnlockable[] GetRawItemsList()
	{
		List<GameBalanceVars.PlayerUnlockable> list = new List<GameBalanceVars.PlayerUnlockable>();
		foreach (UIOverconData.NameToOverconEntry item in UIOverconData.Get().m_nameToOverconEntry)
		{
			list.Add(item.CreateUnlockDataEntry());
		}
		return SortItems(list).ToArray();
	}

	protected override Toggle[] GetFilters()
	{
		return new Toggle[1]
		{
			m_ownedToggle
		};
	}

	protected override bool ShouldCheckmark(GameBalanceVars.PlayerUnlockable item)
	{
		return false;
	}

	protected override bool ShouldFilter(GameBalanceVars.PlayerUnlockable item)
	{
		if (m_ownedToggle.isOn)
		{
			if (ClientGameManager.Get() == null || !ClientGameManager.Get().IsPlayerAccountDataAvailable())
			{
				return true;
			}
			if (!ClientGameManager.Get().GetPlayerAccountData().AccountComponent.IsOverconUnlocked(item.ID))
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						return true;
					}
				}
			}
		}
		return false;
	}

	public override TooltipType? GetItemTooltipType()
	{
		return TooltipType.Titled;
	}

	public override bool ItemTooltipPopulate(UITooltipBase tooltip, UIStoreItemBtn slot, GameBalanceVars.PlayerUnlockable item)
	{
		UIOverconData.NameToOverconEntry nameToOverconEntry = null;
		int num = 0;
		while (true)
		{
			if (num < UIOverconData.Get().m_nameToOverconEntry.Count)
			{
				if (UIOverconData.Get().m_nameToOverconEntry[num].m_overconId == item.ID)
				{
					nameToOverconEntry = UIOverconData.Get().m_nameToOverconEntry[num];
					break;
				}
				num++;
				continue;
			}
			break;
		}
		if (nameToOverconEntry == null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return false;
				}
			}
		}
		string text = StringUtil.TR("/overcon", "SlashCommand") + " " + nameToOverconEntry.GetCommandName();
		string text2 = nameToOverconEntry.GetObtainedDescription().Trim();
		if (!text2.IsNullOrEmpty())
		{
			text = text + Environment.NewLine + text2;
		}
		UITitledTooltip uITitledTooltip = tooltip as UITitledTooltip;
		uITitledTooltip.Setup(nameToOverconEntry.GetDisplayName(), text, string.Empty);
		return true;
	}

	protected override void PurchaseItem(GameBalanceVars.PlayerUnlockable item, CurrencyType type)
	{
		UIPurchaseableItem uIPurchaseableItem = new UIPurchaseableItem();
		uIPurchaseableItem.m_itemType = PurchaseItemType.Overcon;
		uIPurchaseableItem.m_overconID = item.ID;
		uIPurchaseableItem.m_currencyType = type;
		UIStorePanel.Get().OpenPurchaseDialog(uIPurchaseableItem);
	}
}

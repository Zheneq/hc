using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class UIStoreAccountOverconsPanel : UIStoreBaseInventoryPanel
{
	public Toggle m_ownedToggle;

	protected new void Start()
	{
		base.Start();
		UITooltipHoverObject component = this.m_ownedToggle.GetComponent<UITooltipHoverObject>();
		UITooltipObject uitooltipObject = component;
		TooltipType tooltipType = TooltipType.Simple;
		if (UIStoreAccountOverconsPanel.<>f__am$cache0 == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIStoreAccountOverconsPanel.Start()).MethodHandle;
			}
			UIStoreAccountOverconsPanel.<>f__am$cache0 = delegate(UITooltipBase tooltip)
			{
				UISimpleTooltip uisimpleTooltip = (UISimpleTooltip)tooltip;
				uisimpleTooltip.Setup(StringUtil.TR("Owned", "Store"));
				return true;
			};
		}
		uitooltipObject.Setup(tooltipType, UIStoreAccountOverconsPanel.<>f__am$cache0, null);
	}

	protected override GameBalanceVars.PlayerUnlockable[] GetRawItemsList()
	{
		List<GameBalanceVars.PlayerUnlockable> list = new List<GameBalanceVars.PlayerUnlockable>();
		foreach (UIOverconData.NameToOverconEntry nameToOverconEntry in UIOverconData.Get().m_nameToOverconEntry)
		{
			list.Add(nameToOverconEntry.CreateUnlockDataEntry());
		}
		return base.SortItems(list).ToArray();
	}

	protected override Toggle[] GetFilters()
	{
		return new Toggle[]
		{
			this.m_ownedToggle
		};
	}

	protected override bool ShouldCheckmark(GameBalanceVars.PlayerUnlockable item)
	{
		return false;
	}

	protected override bool ShouldFilter(GameBalanceVars.PlayerUnlockable item)
	{
		if (this.m_ownedToggle.isOn)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIStoreAccountOverconsPanel.ShouldFilter(GameBalanceVars.PlayerUnlockable)).MethodHandle;
			}
			if (ClientGameManager.Get() == null || !ClientGameManager.Get().IsPlayerAccountDataAvailable())
			{
				return true;
			}
			if (!ClientGameManager.Get().GetPlayerAccountData().AccountComponent.IsOverconUnlocked(item.ID))
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
				return true;
			}
		}
		return false;
	}

	public override TooltipType? GetItemTooltipType()
	{
		return new TooltipType?(TooltipType.Titled);
	}

	public override bool ItemTooltipPopulate(UITooltipBase tooltip, UIStoreItemBtn slot, GameBalanceVars.PlayerUnlockable item)
	{
		UIOverconData.NameToOverconEntry nameToOverconEntry = null;
		int i = 0;
		while (i < UIOverconData.Get().m_nameToOverconEntry.Count)
		{
			if (UIOverconData.Get().m_nameToOverconEntry[i].m_overconId == item.ID)
			{
				nameToOverconEntry = UIOverconData.Get().m_nameToOverconEntry[i];
				IL_63:
				if (nameToOverconEntry == null)
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
					return false;
				}
				string text = StringUtil.TR("/overcon", "SlashCommand") + " " + nameToOverconEntry.GetCommandName();
				string text2 = nameToOverconEntry.GetObtainedDescription().Trim();
				if (!text2.IsNullOrEmpty())
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
					text = text + Environment.NewLine + text2;
				}
				UITitledTooltip uititledTooltip = tooltip as UITitledTooltip;
				uititledTooltip.Setup(nameToOverconEntry.GetDisplayName(), text, string.Empty);
				return true;
			}
			else
			{
				i++;
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
			RuntimeMethodHandle runtimeMethodHandle = methodof(UIStoreAccountOverconsPanel.ItemTooltipPopulate(UITooltipBase, UIStoreItemBtn, GameBalanceVars.PlayerUnlockable)).MethodHandle;
			goto IL_63;
		}
		goto IL_63;
	}

	protected override void PurchaseItem(GameBalanceVars.PlayerUnlockable item, CurrencyType type)
	{
		UIPurchaseableItem uipurchaseableItem = new UIPurchaseableItem();
		uipurchaseableItem.m_itemType = PurchaseItemType.Overcon;
		uipurchaseableItem.m_overconID = item.ID;
		uipurchaseableItem.m_currencyType = type;
		UIStorePanel.Get().OpenPurchaseDialog(uipurchaseableItem, null);
	}
}

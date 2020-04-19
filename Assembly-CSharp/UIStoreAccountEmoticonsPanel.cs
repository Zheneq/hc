using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class UIStoreAccountEmoticonsPanel : UIStoreBaseInventoryPanel
{
	public Toggle m_ownedToggle;

	private void Awake()
	{
		UITooltipHoverObject component = this.m_ownedToggle.GetComponent<UITooltipHoverObject>();
		UITooltipObject uitooltipObject = component;
		TooltipType tooltipType = TooltipType.Simple;
		if (UIStoreAccountEmoticonsPanel.<>f__am$cache0 == null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIStoreAccountEmoticonsPanel.Awake()).MethodHandle;
			}
			UIStoreAccountEmoticonsPanel.<>f__am$cache0 = delegate(UITooltipBase tooltip)
			{
				UISimpleTooltip uisimpleTooltip = (UISimpleTooltip)tooltip;
				uisimpleTooltip.Setup(StringUtil.TR("Owned", "Store"));
				return true;
			};
		}
		uitooltipObject.Setup(tooltipType, UIStoreAccountEmoticonsPanel.<>f__am$cache0, null);
	}

	protected override GameBalanceVars.PlayerUnlockable[] GetRawItemsList()
	{
		return base.SortItems(new List<GameBalanceVars.PlayerUnlockable>(GameBalanceVars.Get().ChatEmojis)).ToArray();
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
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIStoreAccountEmoticonsPanel.ShouldFilter(GameBalanceVars.PlayerUnlockable)).MethodHandle;
			}
			if (!(ClientGameManager.Get() == null))
			{
				if (!ClientGameManager.Get().IsPlayerAccountDataAvailable())
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
				}
				else
				{
					if (!ClientGameManager.Get().GetPlayerAccountData().AccountComponent.IsChatEmojiUnlocked(item.ID))
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
						return true;
					}
					return false;
				}
			}
			return true;
		}
		return false;
	}

	public override TooltipType? GetItemTooltipType()
	{
		return new TooltipType?(TooltipType.Titled);
	}

	public override bool ItemTooltipPopulate(UITooltipBase tooltip, UIStoreItemBtn slot, GameBalanceVars.PlayerUnlockable item)
	{
		string tooltipText = string.Format(StringUtil.TR("ChatEmojiTagTooltip", "ChatEmoji"), StringUtil.TR_EmojiTag(item.ID));
		UITitledTooltip uititledTooltip = tooltip as UITitledTooltip;
		uititledTooltip.Setup(StringUtil.TR_EmojiName(item.ID), tooltipText, string.Empty);
		return true;
	}

	protected override void PurchaseItem(GameBalanceVars.PlayerUnlockable item, CurrencyType type)
	{
		UIPurchaseableItem uipurchaseableItem = new UIPurchaseableItem();
		uipurchaseableItem.m_itemType = PurchaseItemType.Emoticon;
		uipurchaseableItem.m_emoticonID = item.ID;
		uipurchaseableItem.m_currencyType = type;
		UIStorePanel.Get().OpenPurchaseDialog(uipurchaseableItem, null);
	}
}

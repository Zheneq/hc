using System.Collections.Generic;
using UnityEngine.UI;

public class UIStoreAccountEmoticonsPanel : UIStoreBaseInventoryPanel
{
	public Toggle m_ownedToggle;

	private void Awake()
	{
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
		return SortItems(new List<GameBalanceVars.PlayerUnlockable>(GameBalanceVars.Get().ChatEmojis)).ToArray();
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
			if (!(ClientGameManager.Get() == null))
			{
				if (ClientGameManager.Get().IsPlayerAccountDataAvailable())
				{
					if (!ClientGameManager.Get().GetPlayerAccountData().AccountComponent.IsChatEmojiUnlocked(item.ID))
					{
						while (true)
						{
							switch (7)
							{
							case 0:
								break;
							default:
								return true;
							}
						}
					}
					goto IL_007b;
				}
			}
			return true;
		}
		goto IL_007b;
		IL_007b:
		return false;
	}

	public override TooltipType? GetItemTooltipType()
	{
		return TooltipType.Titled;
	}

	public override bool ItemTooltipPopulate(UITooltipBase tooltip, UIStoreItemBtn slot, GameBalanceVars.PlayerUnlockable item)
	{
		string tooltipText = string.Format(StringUtil.TR("ChatEmojiTagTooltip", "ChatEmoji"), StringUtil.TR_EmojiTag(item.ID));
		UITitledTooltip uITitledTooltip = tooltip as UITitledTooltip;
		uITitledTooltip.Setup(StringUtil.TR_EmojiName(item.ID), tooltipText, string.Empty);
		return true;
	}

	protected override void PurchaseItem(GameBalanceVars.PlayerUnlockable item, CurrencyType type)
	{
		UIPurchaseableItem uIPurchaseableItem = new UIPurchaseableItem();
		uIPurchaseableItem.m_itemType = PurchaseItemType.Emoticon;
		uIPurchaseableItem.m_emoticonID = item.ID;
		uIPurchaseableItem.m_currencyType = type;
		UIStorePanel.Get().OpenPurchaseDialog(uIPurchaseableItem);
	}
}

using System;
using System.Collections.Generic;

public class UIStoreFreelancerTauntsPanel : UIStoreBaseInventoryPanel
{
	private const string c_tauntVideoDir = "Video/taunts/";

	protected override GameBalanceVars.PlayerUnlockable[] GetRawItemsList()
	{
		if (this.m_charType != CharacterType.None)
		{
			List<GameBalanceVars.PlayerUnlockable> list = new List<GameBalanceVars.PlayerUnlockable>();
			GameBalanceVars gameBalanceVars = GameBalanceVars.Get();
			foreach (GameBalanceVars.TauntUnlockData item in gameBalanceVars.GetCharacterUnlockData(this.m_charType).tauntUnlockData)
			{
				list.Add(item);
			}
			return base.SortItems(list).ToArray();
		}
		return new GameBalanceVars.PlayerUnlockable[0];
	}

	public override TooltipType? GetItemTooltipType()
	{
		return new TooltipType?(TooltipType.TauntPreview);
	}

	public override bool ItemTooltipPopulate(UITooltipBase tooltip, UIStoreItemBtn slot, GameBalanceVars.PlayerUnlockable item)
	{
		UIFrontendTauntMouseoverVideo uifrontendTauntMouseoverVideo = tooltip as UIFrontendTauntMouseoverVideo;
		if (!(uifrontendTauntMouseoverVideo == null))
		{
			if (!(item is GameBalanceVars.TauntUnlockData))
			{
			}
			else
			{
				CharacterTaunt characterTaunt = GameWideData.Get().GetCharacterResourceLink((CharacterType)item.Index1).m_taunts[item.ID];
				if (characterTaunt.m_tauntVideoPath.Length > 0)
				{
					uifrontendTauntMouseoverVideo.Setup("Video/taunts/" + characterTaunt.m_tauntVideoPath);
					return true;
				}
				return false;
			}
		}
		return false;
	}

	protected override void PurchaseItem(GameBalanceVars.PlayerUnlockable item, CurrencyType type)
	{
		UIPurchaseableItem uipurchaseableItem = new UIPurchaseableItem();
		uipurchaseableItem.m_itemType = PurchaseItemType.Taunt;
		uipurchaseableItem.m_charLink = GameWideData.Get().GetCharacterResourceLink((CharacterType)item.Index1);
		uipurchaseableItem.m_tauntIndex = item.ID;
		uipurchaseableItem.m_currencyType = type;
		UIStorePanel.Get().OpenPurchaseDialog(uipurchaseableItem);
	}
}

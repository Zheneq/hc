using System.Collections.Generic;

public class UIStoreFreelancerTauntsPanel : UIStoreBaseInventoryPanel
{
	private const string c_tauntVideoDir = "Video/taunts/";

	protected override GameBalanceVars.PlayerUnlockable[] GetRawItemsList()
	{
		if (m_charType != 0)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
				{
					List<GameBalanceVars.PlayerUnlockable> list = new List<GameBalanceVars.PlayerUnlockable>();
					GameBalanceVars gameBalanceVars = GameBalanceVars.Get();
					GameBalanceVars.TauntUnlockData[] tauntUnlockData = gameBalanceVars.GetCharacterUnlockData(m_charType).tauntUnlockData;
					foreach (GameBalanceVars.TauntUnlockData item in tauntUnlockData)
					{
						list.Add(item);
					}
					return SortItems(list).ToArray();
				}
				}
			}
		}
		return new GameBalanceVars.PlayerUnlockable[0];
	}

	public override TooltipType? GetItemTooltipType()
	{
		return TooltipType.TauntPreview;
	}

	public override bool ItemTooltipPopulate(UITooltipBase tooltip, UIStoreItemBtn slot, GameBalanceVars.PlayerUnlockable item)
	{
		UIFrontendTauntMouseoverVideo uIFrontendTauntMouseoverVideo = tooltip as UIFrontendTauntMouseoverVideo;
		if (!(uIFrontendTauntMouseoverVideo == null))
		{
			if (item is GameBalanceVars.TauntUnlockData)
			{
				CharacterTaunt characterTaunt = GameWideData.Get().GetCharacterResourceLink((CharacterType)item.Index1).m_taunts[item.ID];
				if (characterTaunt.m_tauntVideoPath.Length > 0)
				{
					uIFrontendTauntMouseoverVideo.Setup("Video/taunts/" + characterTaunt.m_tauntVideoPath);
					return true;
				}
				return false;
			}
		}
		return false;
	}

	protected override void PurchaseItem(GameBalanceVars.PlayerUnlockable item, CurrencyType type)
	{
		UIPurchaseableItem uIPurchaseableItem = new UIPurchaseableItem();
		uIPurchaseableItem.m_itemType = PurchaseItemType.Taunt;
		uIPurchaseableItem.m_charLink = GameWideData.Get().GetCharacterResourceLink((CharacterType)item.Index1);
		uIPurchaseableItem.m_tauntIndex = item.ID;
		uIPurchaseableItem.m_currencyType = type;
		UIStorePanel.Get().OpenPurchaseDialog(uIPurchaseableItem);
	}
}

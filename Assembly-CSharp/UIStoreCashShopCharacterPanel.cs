using System;
using System.Collections.Generic;

public class UIStoreCashShopCharacterPanel : UICashShopPanelBase
{
	protected override void Start()
	{
		ClientGameManager.Get().OnCharacterDataUpdated += this.RefreshOwnedCharacters;
		base.Start();
	}

	private void OnDestroy()
	{
		if (ClientGameManager.Get() != null)
		{
			ClientGameManager.Get().OnCharacterDataUpdated -= this.RefreshOwnedCharacters;
		}
	}

	protected override UIPurchaseableItem[] GetPurchasableItems()
	{
		List<UIPurchaseableItem> list = new List<UIPurchaseableItem>();
		CharacterResourceLink[] characterResourceLinks = GameWideData.Get().m_characterResourceLinks;
		for (int i = 0; i < characterResourceLinks.Length; i++)
		{
			if (GameManager.Get().IsCharacterAllowedForPlayers(characterResourceLinks[i].m_characterType))
			{
				list.Add(new UIPurchaseableItem
				{
					m_purchaseForCash = true,
					m_itemType = PurchaseItemType.Character,
					m_charLink = characterResourceLinks[i]
				});
			}
		}
		return list.ToArray();
	}

	private void RefreshOwnedCharacters(PersistedCharacterData charData)
	{
		base.RefreshPage();
	}
}

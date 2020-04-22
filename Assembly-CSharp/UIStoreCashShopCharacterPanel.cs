using System.Collections.Generic;

public class UIStoreCashShopCharacterPanel : UICashShopPanelBase
{
	protected override void Start()
	{
		ClientGameManager.Get().OnCharacterDataUpdated += RefreshOwnedCharacters;
		base.Start();
	}

	private void OnDestroy()
	{
		if (!(ClientGameManager.Get() != null))
		{
			return;
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			ClientGameManager.Get().OnCharacterDataUpdated -= RefreshOwnedCharacters;
			return;
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
				UIPurchaseableItem uIPurchaseableItem = new UIPurchaseableItem();
				uIPurchaseableItem.m_purchaseForCash = true;
				uIPurchaseableItem.m_itemType = PurchaseItemType.Character;
				uIPurchaseableItem.m_charLink = characterResourceLinks[i];
				list.Add(uIPurchaseableItem);
			}
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			return list.ToArray();
		}
	}

	private void RefreshOwnedCharacters(PersistedCharacterData charData)
	{
		RefreshPage();
	}
}

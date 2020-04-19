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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIStoreCashShopCharacterPanel.OnDestroy()).MethodHandle;
			}
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
		for (;;)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			break;
		}
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(UIStoreCashShopCharacterPanel.GetPurchasableItems()).MethodHandle;
		}
		return list.ToArray();
	}

	private void RefreshOwnedCharacters(PersistedCharacterData charData)
	{
		base.RefreshPage();
	}
}

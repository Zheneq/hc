using System.Collections.Generic;
using UnityEngine.UI;

public class UIStoreAccountCashPanel : UIStoreBaseInventoryPanel
{
	public Toggle m_ownedToggle;

	private UIPurchaseableItem m_newItemToPurchase;

	protected new void Start()
	{
		base.Start();
		ClientGameManager.Get().OnCharacterDataUpdated += OnCharacterUpdated;
		UITooltipHoverObject component = m_ownedToggle.GetComponent<UITooltipHoverObject>();
		if (_003C_003Ef__am_0024cache0 == null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			_003C_003Ef__am_0024cache0 = delegate(UITooltipBase tooltip)
			{
				UISimpleTooltip uISimpleTooltip = (UISimpleTooltip)tooltip;
				uISimpleTooltip.Setup(StringUtil.TR("Owned", "Store"));
				return true;
			};
		}
		component.Setup(TooltipType.Simple, _003C_003Ef__am_0024cache0);
	}

	protected new void OnDestroy()
	{
		base.OnDestroy();
		if (!(ClientGameManager.Get() != null))
		{
			return;
		}
		while (true)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			ClientGameManager.Get().OnCharacterDataUpdated -= OnCharacterUpdated;
			return;
		}
	}

	private void OnCharacterUpdated(PersistedCharacterData charData)
	{
		RefreshPage();
	}

	protected override GameBalanceVars.PlayerUnlockable[] GetRawItemsList()
	{
		List<GameBalanceVars.PlayerUnlockable> list = new List<GameBalanceVars.PlayerUnlockable>();
		List<GameBalanceVars.PlayerUnlockable> list2 = new List<GameBalanceVars.PlayerUnlockable>();
		GameBalanceVars.CharacterUnlockData[] characterUnlockData = GameBalanceVars.Get().characterUnlockData;
		foreach (GameBalanceVars.CharacterUnlockData characterUnlockData2 in characterUnlockData)
		{
			if (GameWideData.Get().GetCharacterResourceLink(characterUnlockData2.character).m_isHidden)
			{
				continue;
			}
			GameBalanceVars.SkinUnlockData[] skinUnlockData = characterUnlockData2.skinUnlockData;
			foreach (GameBalanceVars.SkinUnlockData skinUnlockData2 in skinUnlockData)
			{
				GameBalanceVars.PatternUnlockData[] patternUnlockData = skinUnlockData2.patternUnlockData;
				int num = 0;
				while (num < patternUnlockData.Length)
				{
					GameBalanceVars.PatternUnlockData patternUnlockData2 = patternUnlockData[num];
					GameBalanceVars.ColorUnlockData[] colorUnlockData = patternUnlockData2.colorUnlockData;
					foreach (GameBalanceVars.ColorUnlockData colorUnlockData2 in colorUnlockData)
					{
						if (colorUnlockData2.GetRealCurrencyPrice() > 0f)
						{
							list2.Add(colorUnlockData2);
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
						num++;
						goto IL_00c6;
					}
					IL_00c6:;
				}
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						goto end_IL_00ce;
					}
					continue;
					end_IL_00ce:
					break;
				}
			}
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			list.AddRange(SortItems(list2));
			list2.Clear();
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			GameBalanceVars.StoreItemForPurchase[] storeItemsForPurchase = GameBalanceVars.Get().StoreItemsForPurchase;
			foreach (GameBalanceVars.StoreItemForPurchase storeItemForPurchase in storeItemsForPurchase)
			{
				if (storeItemForPurchase.GetRealCurrencyPrice() > 0f)
				{
					list2.Add(storeItemForPurchase);
				}
			}
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				list.AddRange(SortItems(list2));
				list2.Clear();
				return list.ToArray();
			}
		}
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
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				if (ClientGameManager.Get().IsPlayerAccountDataAvailable())
				{
					if (item is GameBalanceVars.ColorUnlockData)
					{
						while (true)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							CharacterType index = (CharacterType)item.Index1;
							CharacterComponent characterComponent = ClientGameManager.Get().GetPlayerCharacterData(index).CharacterComponent;
							return !characterComponent.GetSkin(item.Index2).GetPattern(item.Index3).GetColor(item.ID)
								.Unlocked;
						}
					}
					if (item is GameBalanceVars.StoreItemForPurchase)
					{
						while (true)
						{
							switch (1)
							{
							case 0:
								break;
							default:
								return true;
							}
						}
					}
					goto IL_00c3;
				}
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			return true;
		}
		goto IL_00c3;
		IL_00c3:
		return false;
	}

	protected override void ItemClicked(GameBalanceVars.PlayerUnlockable item)
	{
	}

	public override TooltipType? GetItemTooltipType()
	{
		return TooltipType.Titled;
	}

	public override bool ItemTooltipPopulate(UITooltipBase tooltip, UIStoreItemBtn slot, GameBalanceVars.PlayerUnlockable item)
	{
		if (item is GameBalanceVars.ColorUnlockData)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			CharacterType index = (CharacterType)item.Index1;
			string text = StringUtil.TR_CharacterPatternColorDescription(index.ToString(), item.Index2 + 1, item.Index3 + 1, item.ID + 1);
			if (text.Trim().Length > 0)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
					{
						UITitledTooltip uITitledTooltip = tooltip as UITitledTooltip;
						CharacterType index2 = (CharacterType)item.Index1;
						string tooltipTitle = StringUtil.TR_CharacterPatternColorName(index2.ToString(), item.Index2 + 1, item.Index3 + 1, item.ID + 1);
						uITitledTooltip.Setup(tooltipTitle, text, string.Empty);
						return true;
					}
					}
				}
			}
		}
		else if (item is GameBalanceVars.StoreItemForPurchase)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			string text2 = StringUtil.TR_InventoryItemDescription((item as GameBalanceVars.StoreItemForPurchase).m_itemTemplateId);
			if (text2.Trim().Length > 0)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
					{
						UITitledTooltip uITitledTooltip2 = tooltip as UITitledTooltip;
						string tooltipTitle2 = StringUtil.TR_InventoryItemName((item as GameBalanceVars.StoreItemForPurchase).m_itemTemplateId);
						uITitledTooltip2.Setup(tooltipTitle2, text2, string.Empty);
						return true;
					}
					}
				}
			}
		}
		return false;
	}

	protected override void PurchaseItem(GameBalanceVars.PlayerUnlockable item, CurrencyType type)
	{
		m_newItemToPurchase = new UIPurchaseableItem();
		m_newItemToPurchase.m_currencyType = type;
		if (item is GameBalanceVars.ColorUnlockData)
		{
			GameBalanceVars.ColorUnlockData colorUnlockData = item as GameBalanceVars.ColorUnlockData;
			m_newItemToPurchase.m_itemType = PurchaseItemType.Tint;
			m_newItemToPurchase.m_charLink = GameWideData.Get().GetCharacterResourceLink((CharacterType)colorUnlockData.Index1);
			m_newItemToPurchase.m_skinIndex = colorUnlockData.Index2;
			m_newItemToPurchase.m_textureIndex = colorUnlockData.Index3;
			m_newItemToPurchase.m_tintIndex = colorUnlockData.ID;
		}
		else if (item is GameBalanceVars.StoreItemForPurchase)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_newItemToPurchase.m_itemType = PurchaseItemType.InventoryItem;
			m_newItemToPurchase.m_inventoryTemplateId = (item as GameBalanceVars.StoreItemForPurchase).m_itemTemplateId;
			m_newItemToPurchase.m_overlayText = (item as GameBalanceVars.StoreItemForPurchase).m_overlayText;
		}
		if (type == CurrencyType.NONE)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			m_newItemToPurchase.m_purchaseForCash = true;
		}
		else
		{
			m_newItemToPurchase.m_currencyType = type;
		}
		UIStorePanel.Get().OpenPurchaseDialog(m_newItemToPurchase);
	}
}

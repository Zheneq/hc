using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class UIStoreAccountCashPanel : UIStoreBaseInventoryPanel
{
	public Toggle m_ownedToggle;

	private UIPurchaseableItem m_newItemToPurchase;

	protected new void Start()
	{
		base.Start();
		ClientGameManager.Get().OnCharacterDataUpdated += this.OnCharacterUpdated;
		UITooltipHoverObject component = this.m_ownedToggle.GetComponent<UITooltipHoverObject>();
		UITooltipObject uitooltipObject = component;
		TooltipType tooltipType = TooltipType.Simple;
		if (UIStoreAccountCashPanel.<>f__am$cache0 == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIStoreAccountCashPanel.Start()).MethodHandle;
			}
			UIStoreAccountCashPanel.<>f__am$cache0 = delegate(UITooltipBase tooltip)
			{
				UISimpleTooltip uisimpleTooltip = (UISimpleTooltip)tooltip;
				uisimpleTooltip.Setup(StringUtil.TR("Owned", "Store"));
				return true;
			};
		}
		uitooltipObject.Setup(tooltipType, UIStoreAccountCashPanel.<>f__am$cache0, null);
	}

	protected new void OnDestroy()
	{
		base.OnDestroy();
		if (ClientGameManager.Get() != null)
		{
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIStoreAccountCashPanel.OnDestroy()).MethodHandle;
			}
			ClientGameManager.Get().OnCharacterDataUpdated -= this.OnCharacterUpdated;
		}
	}

	private void OnCharacterUpdated(PersistedCharacterData charData)
	{
		base.RefreshPage();
	}

	protected override GameBalanceVars.PlayerUnlockable[] GetRawItemsList()
	{
		List<GameBalanceVars.PlayerUnlockable> list = new List<GameBalanceVars.PlayerUnlockable>();
		List<GameBalanceVars.PlayerUnlockable> list2 = new List<GameBalanceVars.PlayerUnlockable>();
		foreach (GameBalanceVars.CharacterUnlockData characterUnlockData2 in GameBalanceVars.Get().characterUnlockData)
		{
			if (!GameWideData.Get().GetCharacterResourceLink(characterUnlockData2.character).m_isHidden)
			{
				foreach (GameBalanceVars.SkinUnlockData skinUnlockData2 in characterUnlockData2.skinUnlockData)
				{
					foreach (GameBalanceVars.PatternUnlockData patternUnlockData2 in skinUnlockData2.patternUnlockData)
					{
						foreach (GameBalanceVars.ColorUnlockData colorUnlockData2 in patternUnlockData2.colorUnlockData)
						{
							if (colorUnlockData2.GetRealCurrencyPrice() > 0f)
							{
								list2.Add(colorUnlockData2);
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
							RuntimeMethodHandle runtimeMethodHandle = methodof(UIStoreAccountCashPanel.GetRawItemsList()).MethodHandle;
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
				}
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				list.AddRange(base.SortItems(list2));
				list2.Clear();
			}
		}
		for (;;)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			break;
		}
		foreach (GameBalanceVars.StoreItemForPurchase storeItemForPurchase in GameBalanceVars.Get().StoreItemsForPurchase)
		{
			if (storeItemForPurchase.GetRealCurrencyPrice() > 0f)
			{
				list2.Add(storeItemForPurchase);
			}
		}
		for (;;)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			break;
		}
		list.AddRange(base.SortItems(list2));
		list2.Clear();
		return list.ToArray();
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
			if (!(ClientGameManager.Get() == null))
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(UIStoreAccountCashPanel.ShouldFilter(GameBalanceVars.PlayerUnlockable)).MethodHandle;
				}
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
					if (item is GameBalanceVars.ColorUnlockData)
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
						CharacterType index = (CharacterType)item.Index1;
						CharacterComponent characterComponent = ClientGameManager.Get().GetPlayerCharacterData(index).CharacterComponent;
						return !characterComponent.GetSkin(item.Index2).GetPattern(item.Index3).GetColor(item.ID).Unlocked;
					}
					if (item is GameBalanceVars.StoreItemForPurchase)
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
						return true;
					}
					return false;
				}
			}
			return true;
		}
		return false;
	}

	protected override void ItemClicked(GameBalanceVars.PlayerUnlockable item)
	{
	}

	public override TooltipType? GetItemTooltipType()
	{
		return new TooltipType?(TooltipType.Titled);
	}

	public override bool ItemTooltipPopulate(UITooltipBase tooltip, UIStoreItemBtn slot, GameBalanceVars.PlayerUnlockable item)
	{
		if (item is GameBalanceVars.ColorUnlockData)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIStoreAccountCashPanel.ItemTooltipPopulate(UITooltipBase, UIStoreItemBtn, GameBalanceVars.PlayerUnlockable)).MethodHandle;
			}
			CharacterType index = (CharacterType)item.Index1;
			string text = StringUtil.TR_CharacterPatternColorDescription(index.ToString(), item.Index2 + 1, item.Index3 + 1, item.ID + 1);
			if (text.Trim().Length > 0)
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				UITitledTooltip uititledTooltip = tooltip as UITitledTooltip;
				CharacterType index2 = (CharacterType)item.Index1;
				string tooltipTitle = StringUtil.TR_CharacterPatternColorName(index2.ToString(), item.Index2 + 1, item.Index3 + 1, item.ID + 1);
				uititledTooltip.Setup(tooltipTitle, text, string.Empty);
				return true;
			}
		}
		else if (item is GameBalanceVars.StoreItemForPurchase)
		{
			for (;;)
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
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				UITitledTooltip uititledTooltip2 = tooltip as UITitledTooltip;
				string tooltipTitle2 = StringUtil.TR_InventoryItemName((item as GameBalanceVars.StoreItemForPurchase).m_itemTemplateId);
				uititledTooltip2.Setup(tooltipTitle2, text2, string.Empty);
				return true;
			}
		}
		return false;
	}

	protected override void PurchaseItem(GameBalanceVars.PlayerUnlockable item, CurrencyType type)
	{
		this.m_newItemToPurchase = new UIPurchaseableItem();
		this.m_newItemToPurchase.m_currencyType = type;
		if (item is GameBalanceVars.ColorUnlockData)
		{
			GameBalanceVars.ColorUnlockData colorUnlockData = item as GameBalanceVars.ColorUnlockData;
			this.m_newItemToPurchase.m_itemType = PurchaseItemType.Tint;
			this.m_newItemToPurchase.m_charLink = GameWideData.Get().GetCharacterResourceLink((CharacterType)colorUnlockData.Index1);
			this.m_newItemToPurchase.m_skinIndex = colorUnlockData.Index2;
			this.m_newItemToPurchase.m_textureIndex = colorUnlockData.Index3;
			this.m_newItemToPurchase.m_tintIndex = colorUnlockData.ID;
		}
		else if (item is GameBalanceVars.StoreItemForPurchase)
		{
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIStoreAccountCashPanel.PurchaseItem(GameBalanceVars.PlayerUnlockable, CurrencyType)).MethodHandle;
			}
			this.m_newItemToPurchase.m_itemType = PurchaseItemType.InventoryItem;
			this.m_newItemToPurchase.m_inventoryTemplateId = (item as GameBalanceVars.StoreItemForPurchase).m_itemTemplateId;
			this.m_newItemToPurchase.m_overlayText = (item as GameBalanceVars.StoreItemForPurchase).m_overlayText;
		}
		if (type == CurrencyType.NONE)
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
			this.m_newItemToPurchase.m_purchaseForCash = true;
		}
		else
		{
			this.m_newItemToPurchase.m_currencyType = type;
		}
		UIStorePanel.Get().OpenPurchaseDialog(this.m_newItemToPurchase, null);
	}
}

using System;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class UIStoreFreelancerStylesPanel : UIStoreBaseInventoryPanel
{
	public _SelectableBtn m_equipBtn;

	private CharacterVisualInfo m_currentVisualInfo;

	private GameBalanceVars.ColorUnlockData m_selectedItem;

	public bool IsVisible { get; private set; }

	private void Awake()
	{
		this.m_equipBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.EquipClicked);
		this.m_selectedItem = null;
	}

	protected override GameBalanceVars.PlayerUnlockable[] GetRawItemsList()
	{
		if (this.m_charType != CharacterType.None)
		{
			this.m_currentVisualInfo = new CharacterVisualInfo(0, 0, 0);
			List<GameBalanceVars.PlayerUnlockable> list = new List<GameBalanceVars.PlayerUnlockable>();
			GameBalanceVars gameBalanceVars = GameBalanceVars.Get();
			foreach (GameBalanceVars.SkinUnlockData skinUnlockData2 in gameBalanceVars.GetCharacterUnlockData(this.m_charType).skinUnlockData)
			{
				foreach (GameBalanceVars.PatternUnlockData patternUnlockData2 in skinUnlockData2.patternUnlockData)
				{
					foreach (GameBalanceVars.ColorUnlockData item in patternUnlockData2.colorUnlockData)
					{
						list.Add(item);
					}
				}
			}
			return base.SortItems(list).ToArray();
		}
		return new GameBalanceVars.PlayerUnlockable[0];
	}

	private void OnEnable()
	{
		this.IsVisible = true;
		this.Display3dModel(true);
	}

	private void OnDisable()
	{
		this.IsVisible = false;
	}

	protected override void OnHidden()
	{
		this.Display3dModel(false);
	}

	public void Display3dModel(bool visible)
	{
		bool flag;
		if (visible)
		{
			flag = this.IsVisible;
		}
		else
		{
			flag = false;
		}
		visible = flag;
		if (UICharacterStoreAndProgressWorldObjects.Get() == null)
		{
			return;
		}
		if (visible)
		{
			UICharacterStoreAndProgressWorldObjects.Get().LoadCharacterIntoSlot(this.m_charType, 0, string.Empty, this.m_currentVisualInfo, false);
		}
		UICharacterStoreAndProgressWorldObjects.Get().SetVisible(visible);
	}

	protected override void ItemSelected(GameBalanceVars.PlayerUnlockable item)
	{
		if (!(item is GameBalanceVars.ColorUnlockData))
		{
			this.m_selectedItem = null;
			return;
		}
		this.m_selectedItem = (item as GameBalanceVars.ColorUnlockData);
		CharacterResourceLink characterResourceLink = GameWideData.Get().GetCharacterResourceLink((CharacterType)item.Index1);
		this.m_currentVisualInfo = new CharacterVisualInfo(item.Index2, item.Index3, item.ID);
		UICharacterStoreAndProgressWorldObjects.Get().LoadCharacterIntoSlot(characterResourceLink, 0, string.Empty, this.m_currentVisualInfo, false, true);
	}

	private void EquipClicked(BaseEventData data)
	{
		if (this.m_selectedItem == null)
		{
			return;
		}
	}

	public override TooltipType? GetItemTooltipType()
	{
		return new TooltipType?(TooltipType.Titled);
	}

	public override bool ItemTooltipPopulate(UITooltipBase tooltip, UIStoreItemBtn slot, GameBalanceVars.PlayerUnlockable item)
	{
		CharacterType index = (CharacterType)item.Index1;
		string text = StringUtil.TR_CharacterPatternColorDescription(index.ToString(), item.Index2 + 1, item.Index3 + 1, item.ID + 1);
		if (text.Trim().Length > 0)
		{
			UITitledTooltip uititledTooltip = tooltip as UITitledTooltip;
			CharacterType index2 = (CharacterType)item.Index1;
			string tooltipTitle = StringUtil.TR_CharacterPatternColorName(index2.ToString(), item.Index2 + 1, item.Index3 + 1, item.ID + 1);
			uititledTooltip.Setup(tooltipTitle, text, string.Empty);
			return true;
		}
		return false;
	}

	protected override void PurchaseItem(GameBalanceVars.PlayerUnlockable item, CurrencyType type)
	{
		UIPurchaseableItem uipurchaseableItem = new UIPurchaseableItem();
		uipurchaseableItem.m_itemType = PurchaseItemType.Tint;
		uipurchaseableItem.m_charLink = GameWideData.Get().GetCharacterResourceLink((CharacterType)item.Index1);
		uipurchaseableItem.m_skinIndex = item.Index2;
		uipurchaseableItem.m_textureIndex = item.Index3;
		uipurchaseableItem.m_tintIndex = item.ID;
		if (type == CurrencyType.NONE)
		{
			uipurchaseableItem.m_purchaseForCash = true;
		}
		else
		{
			uipurchaseableItem.m_currencyType = type;
		}
		UIStorePanel.Get().OpenPurchaseDialog(uipurchaseableItem);
	}
}

using System.Collections.Generic;
using UnityEngine.EventSystems;

public class UIStoreFreelancerStylesPanel : UIStoreBaseInventoryPanel
{
	public _SelectableBtn m_equipBtn;

	private CharacterVisualInfo m_currentVisualInfo;

	private GameBalanceVars.ColorUnlockData m_selectedItem;

	public bool IsVisible
	{
		get;
		private set;
	}

	private void Awake()
	{
		m_equipBtn.spriteController.callback = EquipClicked;
		m_selectedItem = null;
	}

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
					m_currentVisualInfo = new CharacterVisualInfo(0, 0, 0);
					List<GameBalanceVars.PlayerUnlockable> list = new List<GameBalanceVars.PlayerUnlockable>();
					GameBalanceVars gameBalanceVars = GameBalanceVars.Get();
					GameBalanceVars.SkinUnlockData[] skinUnlockData = gameBalanceVars.GetCharacterUnlockData(m_charType).skinUnlockData;
					foreach (GameBalanceVars.SkinUnlockData skinUnlockData2 in skinUnlockData)
					{
						GameBalanceVars.PatternUnlockData[] patternUnlockData = skinUnlockData2.patternUnlockData;
						foreach (GameBalanceVars.PatternUnlockData patternUnlockData2 in patternUnlockData)
						{
							GameBalanceVars.ColorUnlockData[] colorUnlockData = patternUnlockData2.colorUnlockData;
							foreach (GameBalanceVars.ColorUnlockData item in colorUnlockData)
							{
								list.Add(item);
							}
							while (true)
							{
								switch (3)
								{
								case 0:
									break;
								default:
									goto end_IL_0097;
								}
								continue;
								end_IL_0097:
								break;
							}
						}
						while (true)
						{
							switch (3)
							{
							case 0:
								break;
							default:
								goto end_IL_00af;
							}
							continue;
							end_IL_00af:
							break;
						}
					}
					while (true)
					{
						switch (5)
						{
						case 0:
							break;
						default:
							return SortItems(list).ToArray();
						}
					}
				}
				}
			}
		}
		return new GameBalanceVars.PlayerUnlockable[0];
	}

	private void OnEnable()
	{
		IsVisible = true;
		Display3dModel(true);
	}

	private void OnDisable()
	{
		IsVisible = false;
	}

	protected override void OnHidden()
	{
		Display3dModel(false);
	}

	public void Display3dModel(bool visible)
	{
		int num;
		if (visible)
		{
			num = (IsVisible ? 1 : 0);
		}
		else
		{
			num = 0;
		}
		visible = ((byte)num != 0);
		if (UICharacterStoreAndProgressWorldObjects.Get() == null)
		{
			while (true)
			{
				switch (4)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
		if (visible)
		{
			UICharacterStoreAndProgressWorldObjects.Get().LoadCharacterIntoSlot(m_charType, 0, string.Empty, m_currentVisualInfo, false);
		}
		UICharacterStoreAndProgressWorldObjects.Get().SetVisible(visible);
	}

	protected override void ItemSelected(GameBalanceVars.PlayerUnlockable item)
	{
		if (!(item is GameBalanceVars.ColorUnlockData))
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					m_selectedItem = null;
					return;
				}
			}
		}
		m_selectedItem = (item as GameBalanceVars.ColorUnlockData);
		CharacterResourceLink characterResourceLink = GameWideData.Get().GetCharacterResourceLink((CharacterType)item.Index1);
		m_currentVisualInfo = new CharacterVisualInfo(item.Index2, item.Index3, item.ID);
		UICharacterStoreAndProgressWorldObjects.Get().LoadCharacterIntoSlot(characterResourceLink, 0, string.Empty, m_currentVisualInfo, false, true);
	}

	private void EquipClicked(BaseEventData data)
	{
		if (m_selectedItem != null)
		{
			return;
		}
		while (true)
		{
			return;
		}
	}

	public override TooltipType? GetItemTooltipType()
	{
		return TooltipType.Titled;
	}

	public override bool ItemTooltipPopulate(UITooltipBase tooltip, UIStoreItemBtn slot, GameBalanceVars.PlayerUnlockable item)
	{
		CharacterType index = (CharacterType)item.Index1;
		string text = StringUtil.TR_CharacterPatternColorDescription(index.ToString(), item.Index2 + 1, item.Index3 + 1, item.ID + 1);
		if (text.Trim().Length > 0)
		{
			UITitledTooltip uITitledTooltip = tooltip as UITitledTooltip;
			CharacterType index2 = (CharacterType)item.Index1;
			string tooltipTitle = StringUtil.TR_CharacterPatternColorName(index2.ToString(), item.Index2 + 1, item.Index3 + 1, item.ID + 1);
			uITitledTooltip.Setup(tooltipTitle, text, string.Empty);
			return true;
		}
		return false;
	}

	protected override void PurchaseItem(GameBalanceVars.PlayerUnlockable item, CurrencyType type)
	{
		UIPurchaseableItem uIPurchaseableItem = new UIPurchaseableItem();
		uIPurchaseableItem.m_itemType = PurchaseItemType.Tint;
		uIPurchaseableItem.m_charLink = GameWideData.Get().GetCharacterResourceLink((CharacterType)item.Index1);
		uIPurchaseableItem.m_skinIndex = item.Index2;
		uIPurchaseableItem.m_textureIndex = item.Index3;
		uIPurchaseableItem.m_tintIndex = item.ID;
		if (type == CurrencyType.NONE)
		{
			uIPurchaseableItem.m_purchaseForCash = true;
		}
		else
		{
			uIPurchaseableItem.m_currencyType = type;
		}
		UIStorePanel.Get().OpenPurchaseDialog(uIPurchaseableItem);
	}
}

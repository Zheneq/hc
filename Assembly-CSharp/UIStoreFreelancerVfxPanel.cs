using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class UIStoreFreelancerVfxPanel : UIStoreBaseInventoryPanel
{
	public Toggle m_ownedToggle;

	public Toggle[] m_abilityToggles;

	private const string c_videoDir = "Video/AbilityPreviews/";

	private void Awake()
	{
		this.SetupTooltip(this.m_ownedToggle, StringUtil.TR("Owned", "Store"));
		for (int i = 0; i < this.m_abilityToggles.Length; i++)
		{
			this.SetupTooltip(this.m_abilityToggles[i], string.Format(StringUtil.TR("AbilityFilterTooltip", "Store"), i + 1));
		}
	}

	protected override GameBalanceVars.PlayerUnlockable[] GetRawItemsList()
	{
		if (this.m_charType == CharacterType.None)
		{
			return new GameBalanceVars.PlayerUnlockable[0];
		}
		return base.SortItems(new List<GameBalanceVars.PlayerUnlockable>(GameBalanceVars.Get().GetCharacterUnlockData(this.m_charType).abilityVfxUnlockData)).ToArray();
	}

	protected override Toggle[] GetFilters()
	{
		return new List<Toggle>(this.m_abilityToggles)
		{
			this.m_ownedToggle
		}.ToArray();
	}

	protected override bool ShouldFilter(GameBalanceVars.PlayerUnlockable item)
	{
		if (this.m_ownedToggle.isOn)
		{
			if (!item.IsOwned())
			{
				return true;
			}
		}
		if (item is GameBalanceVars.AbilityVfxUnlockData)
		{
			if (item.Index1 == (int)this.m_charType)
			{
				bool result = false;
				for (int i = 0; i < this.m_abilityToggles.Length; i++)
				{
					if (this.m_abilityToggles[i].isOn)
					{
						if (i == item.Index2)
						{
							return false;
						}
						result = true;
					}
				}
				return result;
			}
		}
		return true;
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
			if (item is GameBalanceVars.AbilityVfxUnlockData)
			{
				CharacterResourceLink characterResourceLink = GameWideData.Get().GetCharacterResourceLink((CharacterType)item.Index1);
				List<CharacterAbilityVfxSwap> availableVfxSwapsForAbilityIndex = characterResourceLink.GetAvailableVfxSwapsForAbilityIndex(item.Index2);
				CharacterAbilityVfxSwap characterAbilityVfxSwap = null;
				for (int i = 0; i < availableVfxSwapsForAbilityIndex.Count; i++)
				{
					if (availableVfxSwapsForAbilityIndex[i].m_uniqueID == item.ID)
					{
						characterAbilityVfxSwap = availableVfxSwapsForAbilityIndex[i];
						break;
					}
				}
				if (characterAbilityVfxSwap != null)
				{
					if (!characterAbilityVfxSwap.m_swapVideoPath.IsNullOrEmpty())
					{
						uifrontendTauntMouseoverVideo.Setup("Video/AbilityPreviews/" + characterAbilityVfxSwap.m_swapVideoPath);
						return true;
					}
				}
				return false;
			}
		}
		return false;
	}

	protected override void PurchaseItem(GameBalanceVars.PlayerUnlockable item, CurrencyType type)
	{
		UIPurchaseableItem uipurchaseableItem = new UIPurchaseableItem();
		uipurchaseableItem.m_itemType = PurchaseItemType.AbilityVfx;
		uipurchaseableItem.m_charLink = GameWideData.Get().GetCharacterResourceLink((CharacterType)item.Index1);
		uipurchaseableItem.m_abilityID = item.Index2;
		uipurchaseableItem.m_abilityVfxID = item.ID;
		uipurchaseableItem.m_currencyType = type;
		UIStorePanel.Get().OpenPurchaseDialog(uipurchaseableItem, null);
	}

	private void SetupTooltip(Toggle toggle, string text)
	{
		UITooltipHoverObject component = toggle.GetComponent<UITooltipHoverObject>();
		component.Setup(TooltipType.Simple, delegate(UITooltipBase tooltip)
		{
			UISimpleTooltip uisimpleTooltip = (UISimpleTooltip)tooltip;
			uisimpleTooltip.Setup(text);
			return true;
		}, null);
	}
}

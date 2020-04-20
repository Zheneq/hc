using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class UIStoreAccountLoadingScreenPanel : UIStoreBaseInventoryPanel
{
	public Toggle m_ownedToggle;

	protected new void Start()
	{
		base.Start();
		ClientGameManager.Get().OnLoadingScreenBackgroundToggled += this.OnLoadingScreenBackgroundToggled;
		UITooltipHoverObject component = this.m_ownedToggle.GetComponent<UITooltipHoverObject>();
		component.Setup(TooltipType.Simple, delegate(UITooltipBase tooltip)
		{
			UISimpleTooltip uisimpleTooltip = (UISimpleTooltip)tooltip;
			uisimpleTooltip.Setup(StringUtil.TR("Owned", "Store"));
			return true;
		}, null);
	}

	private void OnLoadingScreenBackgroundToggled(int id, bool isActive)
	{
		base.RefreshPage();
	}

	protected new void OnDestroy()
	{
		base.OnDestroy();
		if (ClientGameManager.Get() != null)
		{
			ClientGameManager.Get().OnLoadingScreenBackgroundToggled -= this.OnLoadingScreenBackgroundToggled;
		}
	}

	protected override GameBalanceVars.PlayerUnlockable[] GetRawItemsList()
	{
		return base.SortItems(new List<GameBalanceVars.PlayerUnlockable>(GameBalanceVars.Get().LoadingScreenBackgrounds)).ToArray();
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
		if (item is GameBalanceVars.LoadingScreenBackground)
		{
			if (ClientGameManager.Get() != null)
			{
				if (ClientGameManager.Get().IsPlayerAccountDataAvailable())
				{
					return ClientGameManager.Get().GetPlayerAccountData().AccountComponent.IsLoadingScreenBackgroundActive(item.ID);
				}
			}
			return false;
		}
		return false;
	}

	protected override bool ShouldFilter(GameBalanceVars.PlayerUnlockable item)
	{
		if (this.m_ownedToggle.isOn)
		{
			if (!(ClientGameManager.Get() == null))
			{
				if (!ClientGameManager.Get().IsPlayerAccountDataAvailable())
				{
				}
				else
				{
					if (!ClientGameManager.Get().GetPlayerAccountData().AccountComponent.IsLoadingScreenBackgroundUnlocked(item.ID))
					{
						return true;
					}
					return false;
				}
			}
			return true;
		}
		return false;
	}

	public override TooltipType? GetItemTooltipType()
	{
		return new TooltipType?(TooltipType.Titled);
	}

	public override bool ItemTooltipPopulate(UITooltipBase tooltip, UIStoreItemBtn slot, GameBalanceVars.PlayerUnlockable item)
	{
		GameBalanceVars.LoadingScreenBackground loadingScreenBackground = item as GameBalanceVars.LoadingScreenBackground;
		string tooltipText = loadingScreenBackground.GetObtainedDescription() + Environment.NewLine + loadingScreenBackground.GetPurchaseDescription();
		UITitledTooltip uititledTooltip = tooltip as UITitledTooltip;
		uititledTooltip.Setup(loadingScreenBackground.GetLoadingScreenBackgroundName(), tooltipText, string.Empty);
		return true;
	}

	protected override void ItemClicked(GameBalanceVars.PlayerUnlockable item)
	{
		if (!item.IsOwned())
		{
			return;
		}
		if (item is GameBalanceVars.LoadingScreenBackground)
		{
			bool flag = ClientGameManager.Get().GetPlayerAccountData().AccountComponent.IsLoadingScreenBackgroundActive(item.ID);
			ClientGameManager.Get().RequestLoadingScreenBackgroundToggle(item.ID, !flag, null);
		}
	}

	protected override void PurchaseItem(GameBalanceVars.PlayerUnlockable item, CurrencyType type)
	{
		UIPurchaseableItem uipurchaseableItem = new UIPurchaseableItem();
		uipurchaseableItem.m_itemType = PurchaseItemType.LoadingScreenBackground;
		uipurchaseableItem.m_loadingScreenBackgroundId = item.ID;
		uipurchaseableItem.m_currencyType = type;
		UIStorePanel.Get().OpenPurchaseDialog(uipurchaseableItem, null);
	}
}

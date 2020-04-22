using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class UIStoreAccountLoadingScreenPanel : UIStoreBaseInventoryPanel
{
	public Toggle m_ownedToggle;

	protected new void Start()
	{
		base.Start();
		ClientGameManager.Get().OnLoadingScreenBackgroundToggled += OnLoadingScreenBackgroundToggled;
		UITooltipHoverObject component = m_ownedToggle.GetComponent<UITooltipHoverObject>();
		component.Setup(TooltipType.Simple, delegate(UITooltipBase tooltip)
		{
			UISimpleTooltip uISimpleTooltip = (UISimpleTooltip)tooltip;
			uISimpleTooltip.Setup(StringUtil.TR("Owned", "Store"));
			return true;
		});
	}

	private void OnLoadingScreenBackgroundToggled(int id, bool isActive)
	{
		RefreshPage();
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
			switch (6)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			ClientGameManager.Get().OnLoadingScreenBackgroundToggled -= OnLoadingScreenBackgroundToggled;
			return;
		}
	}

	protected override GameBalanceVars.PlayerUnlockable[] GetRawItemsList()
	{
		return SortItems(new List<GameBalanceVars.PlayerUnlockable>(GameBalanceVars.Get().LoadingScreenBackgrounds)).ToArray();
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
		if (item is GameBalanceVars.LoadingScreenBackground)
		{
			while (true)
			{
				int result;
				switch (6)
				{
				case 0:
					break;
				default:
					{
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						if (ClientGameManager.Get() != null)
						{
							while (true)
							{
								switch (6)
								{
								case 0:
									continue;
								}
								break;
							}
							if (ClientGameManager.Get().IsPlayerAccountDataAvailable())
							{
								while (true)
								{
									switch (4)
									{
									case 0:
										continue;
									}
									break;
								}
								result = (ClientGameManager.Get().GetPlayerAccountData().AccountComponent.IsLoadingScreenBackgroundActive(item.ID) ? 1 : 0);
								goto IL_0071;
							}
						}
						result = 0;
						goto IL_0071;
					}
					IL_0071:
					return (byte)result != 0;
				}
			}
		}
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
					switch (6)
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
					if (!ClientGameManager.Get().GetPlayerAccountData().AccountComponent.IsLoadingScreenBackgroundUnlocked(item.ID))
					{
						return true;
					}
					goto IL_0073;
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
		goto IL_0073;
		IL_0073:
		return false;
	}

	public override TooltipType? GetItemTooltipType()
	{
		return TooltipType.Titled;
	}

	public override bool ItemTooltipPopulate(UITooltipBase tooltip, UIStoreItemBtn slot, GameBalanceVars.PlayerUnlockable item)
	{
		GameBalanceVars.LoadingScreenBackground loadingScreenBackground = item as GameBalanceVars.LoadingScreenBackground;
		string tooltipText = loadingScreenBackground.GetObtainedDescription() + Environment.NewLine + loadingScreenBackground.GetPurchaseDescription();
		UITitledTooltip uITitledTooltip = tooltip as UITitledTooltip;
		uITitledTooltip.Setup(loadingScreenBackground.GetLoadingScreenBackgroundName(), tooltipText, string.Empty);
		return true;
	}

	protected override void ItemClicked(GameBalanceVars.PlayerUnlockable item)
	{
		if (!item.IsOwned() || !(item is GameBalanceVars.LoadingScreenBackground))
		{
			return;
		}
		while (true)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			bool flag = ClientGameManager.Get().GetPlayerAccountData().AccountComponent.IsLoadingScreenBackgroundActive(item.ID);
			ClientGameManager.Get().RequestLoadingScreenBackgroundToggle(item.ID, !flag, null);
			return;
		}
	}

	protected override void PurchaseItem(GameBalanceVars.PlayerUnlockable item, CurrencyType type)
	{
		UIPurchaseableItem uIPurchaseableItem = new UIPurchaseableItem();
		uIPurchaseableItem.m_itemType = PurchaseItemType.LoadingScreenBackground;
		uIPurchaseableItem.m_loadingScreenBackgroundId = item.ID;
		uIPurchaseableItem.m_currencyType = type;
		UIStorePanel.Get().OpenPurchaseDialog(uIPurchaseableItem);
	}
}

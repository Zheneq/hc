using System.Collections.Generic;
using UnityEngine.UI;

public class UIStoreFreelancerVfxPanel : UIStoreBaseInventoryPanel
{
	public Toggle m_ownedToggle;

	public Toggle[] m_abilityToggles;

	private const string c_videoDir = "Video/AbilityPreviews/";

	private void Awake()
	{
		SetupTooltip(m_ownedToggle, StringUtil.TR("Owned", "Store"));
		for (int i = 0; i < m_abilityToggles.Length; i++)
		{
			SetupTooltip(m_abilityToggles[i], string.Format(StringUtil.TR("AbilityFilterTooltip", "Store"), i + 1));
		}
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			return;
		}
	}

	protected override GameBalanceVars.PlayerUnlockable[] GetRawItemsList()
	{
		if (m_charType == CharacterType.None)
		{
			return new GameBalanceVars.PlayerUnlockable[0];
		}
		return SortItems(new List<GameBalanceVars.PlayerUnlockable>(GameBalanceVars.Get().GetCharacterUnlockData(m_charType).abilityVfxUnlockData)).ToArray();
	}

	protected override Toggle[] GetFilters()
	{
		List<Toggle> list = new List<Toggle>(m_abilityToggles);
		list.Add(m_ownedToggle);
		return list.ToArray();
	}

	protected override bool ShouldFilter(GameBalanceVars.PlayerUnlockable item)
	{
		if (m_ownedToggle.isOn)
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
			if (!item.IsOwned())
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						return true;
					}
				}
			}
		}
		if (item is GameBalanceVars.AbilityVfxUnlockData)
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
			if (item.Index1 == (int)m_charType)
			{
				bool result = false;
				for (int i = 0; i < m_abilityToggles.Length; i++)
				{
					if (m_abilityToggles[i].isOn)
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
						if (i == item.Index2)
						{
							return false;
						}
						result = true;
					}
				}
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					return result;
				}
			}
		}
		return true;
	}

	public override TooltipType? GetItemTooltipType()
	{
		return TooltipType.TauntPreview;
	}

	public override bool ItemTooltipPopulate(UITooltipBase tooltip, UIStoreItemBtn slot, GameBalanceVars.PlayerUnlockable item)
	{
		UIFrontendTauntMouseoverVideo uIFrontendTauntMouseoverVideo = tooltip as UIFrontendTauntMouseoverVideo;
		if (!(uIFrontendTauntMouseoverVideo == null))
		{
			while (true)
			{
				switch (3)
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
			if (item is GameBalanceVars.AbilityVfxUnlockData)
			{
				CharacterResourceLink characterResourceLink = GameWideData.Get().GetCharacterResourceLink((CharacterType)item.Index1);
				List<CharacterAbilityVfxSwap> availableVfxSwapsForAbilityIndex = characterResourceLink.GetAvailableVfxSwapsForAbilityIndex(item.Index2);
				CharacterAbilityVfxSwap characterAbilityVfxSwap = null;
				for (int i = 0; i < availableVfxSwapsForAbilityIndex.Count; i++)
				{
					if (availableVfxSwapsForAbilityIndex[i].m_uniqueID == item.ID)
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
						characterAbilityVfxSwap = availableVfxSwapsForAbilityIndex[i];
						break;
					}
				}
				if (characterAbilityVfxSwap != null)
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
					if (!characterAbilityVfxSwap.m_swapVideoPath.IsNullOrEmpty())
					{
						uIFrontendTauntMouseoverVideo.Setup("Video/AbilityPreviews/" + characterAbilityVfxSwap.m_swapVideoPath);
						return true;
					}
				}
				return false;
			}
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		return false;
	}

	protected override void PurchaseItem(GameBalanceVars.PlayerUnlockable item, CurrencyType type)
	{
		UIPurchaseableItem uIPurchaseableItem = new UIPurchaseableItem();
		uIPurchaseableItem.m_itemType = PurchaseItemType.AbilityVfx;
		uIPurchaseableItem.m_charLink = GameWideData.Get().GetCharacterResourceLink((CharacterType)item.Index1);
		uIPurchaseableItem.m_abilityID = item.Index2;
		uIPurchaseableItem.m_abilityVfxID = item.ID;
		uIPurchaseableItem.m_currencyType = type;
		UIStorePanel.Get().OpenPurchaseDialog(uIPurchaseableItem);
	}

	private void SetupTooltip(Toggle toggle, string text)
	{
		UITooltipHoverObject component = toggle.GetComponent<UITooltipHoverObject>();
		component.Setup(TooltipType.Simple, delegate(UITooltipBase tooltip)
		{
			UISimpleTooltip uISimpleTooltip = (UISimpleTooltip)tooltip;
			uISimpleTooltip.Setup(text);
			return true;
		});
	}
}

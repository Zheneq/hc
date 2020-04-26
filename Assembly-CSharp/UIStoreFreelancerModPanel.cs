using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class UIStoreFreelancerModPanel : UIStoreBaseInventoryPanel
{
	public Toggle m_ownedToggle;

	public Toggle[] m_abilityToggles;

	private void Awake()
	{
		SetupTooltip(m_ownedToggle, StringUtil.TR("Owned", "Store"));
		for (int i = 0; i < m_abilityToggles.Length; i++)
		{
			SetupTooltip(m_abilityToggles[i], string.Format(StringUtil.TR("AbilityFilterTooltip", "Store"), i + 1));
		}
		while (true)
		{
			return;
		}
	}

	protected override GameBalanceVars.PlayerUnlockable[] GetRawItemsList()
	{
		if (m_charType == CharacterType.None)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return new GameBalanceVars.PlayerUnlockable[0];
				}
			}
		}
		List<GameBalanceVars.PlayerUnlockable> list = new List<GameBalanceVars.PlayerUnlockable>();
		CharacterResourceLink characterResourceLink = GameWideData.Get().GetCharacterResourceLink(m_charType);
		AbilityData component = characterResourceLink.ActorDataPrefab.GetComponent<AbilityData>();
		for (int i = 0; i < 14; i++)
		{
			List<AbilityMod> availableModsForAbility;
			switch (i)
			{
			case 0:
				availableModsForAbility = AbilityModHelper.GetAvailableModsForAbility(component.m_ability0);
				break;
			case 1:
				availableModsForAbility = AbilityModHelper.GetAvailableModsForAbility(component.m_ability1);
				break;
			case 2:
				availableModsForAbility = AbilityModHelper.GetAvailableModsForAbility(component.m_ability2);
				break;
			case 3:
				availableModsForAbility = AbilityModHelper.GetAvailableModsForAbility(component.m_ability3);
				break;
			case 4:
				availableModsForAbility = AbilityModHelper.GetAvailableModsForAbility(component.m_ability4);
				break;
			case 5:
				availableModsForAbility = AbilityModHelper.GetAvailableModsForAbility(component.m_ability5);
				break;
			case 6:
				availableModsForAbility = AbilityModHelper.GetAvailableModsForAbility(component.m_ability6);
				break;
			default:
				continue;
			}
			for (int j = 0; j < availableModsForAbility.Count; j++)
			{
				list.Add(availableModsForAbility[j].GetAbilityModUnlockData(m_charType, i));
			}
		}
		while (true)
		{
			return SortItems(list).ToArray();
		}
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
			if (!item.IsOwned())
			{
				return true;
			}
		}
		if (item is GameBalanceVars.AbilityModUnlockData)
		{
			if (item.Index1 == (int)m_charType)
			{
				bool result = false;
				for (int i = 0; i < m_abilityToggles.Length; i++)
				{
					if (m_abilityToggles[i].isOn)
					{
						if (i == item.Index2)
						{
							return false;
						}
						result = true;
					}
				}
				while (true)
				{
					return result;
				}
			}
		}
		return true;
	}

	protected override void PurchaseItem(GameBalanceVars.PlayerUnlockable item, CurrencyType type)
	{
		if (type != CurrencyType.FreelancerCurrency)
		{
			throw new Exception("You can't purchase mods without flux");
		}
		ClientGameManager.Get().PurchaseMod(m_charType, item.Index2, item.ID);
		PersistedCharacterData playerCharacterData = ClientGameManager.Get().GetPlayerCharacterData(m_charType);
		PlayerModData item2 = default(PlayerModData);
		item2.AbilityId = item.Index2;
		item2.AbilityModID = item.ID;
		playerCharacterData.CharacterComponent.Mods.Add(item2);
		UpdatePanel();
	}

	protected override CurrencyData GetItemCost(GameBalanceVars.PlayerUnlockable item, CurrencyType type)
	{
		if (type != CurrencyType.FreelancerCurrency)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return base.GetItemCost(item, type);
				}
			}
		}
		CurrencyData currencyData = new CurrencyData();
		currencyData.Amount = GameBalanceVars.Get().FreelancerCurrencyToUnlockMod;
		currencyData.Type = CurrencyType.FreelancerCurrency;
		return currencyData;
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

	public override TooltipType? GetItemTooltipType()
	{
		return TooltipType.Titled;
	}

	public override bool ItemTooltipPopulate(UITooltipBase tooltip, UIStoreItemBtn slot, GameBalanceVars.PlayerUnlockable item)
	{
		Ability ability;
		AbilityMod abilityMod = GetAbilityMod(item, out ability);
		string fullTooltip = abilityMod.GetFullTooltip(ability);
		if (fullTooltip.Trim().Length > 0)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
				{
					UITitledTooltip uITitledTooltip = tooltip as UITitledTooltip;
					uITitledTooltip.Setup(abilityMod.GetName(), fullTooltip, string.Empty);
					return true;
				}
				}
			}
		}
		return false;
	}

	private AbilityMod GetAbilityMod(GameBalanceVars.PlayerUnlockable item, out Ability ability)
	{
		CharacterResourceLink characterResourceLink = GameWideData.Get().GetCharacterResourceLink(m_charType);
		AbilityData component = characterResourceLink.ActorDataPrefab.GetComponent<AbilityData>();
		ability = null;
		switch (item.Index2)
		{
		case 0:
			ability = component.m_ability0;
			break;
		case 1:
			ability = component.m_ability1;
			break;
		case 2:
			ability = component.m_ability2;
			break;
		case 3:
			ability = component.m_ability3;
			break;
		case 4:
			ability = component.m_ability4;
			break;
		case 5:
			ability = component.m_ability5;
			break;
		case 6:
			ability = component.m_ability6;
			break;
		}
		return AbilityModHelper.GetAvailableModsForAbility(ability).Find((AbilityMod x) => x.m_abilityScopeId == item.ID);
	}
}

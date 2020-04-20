using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class UIStoreFreelancerModPanel : UIStoreBaseInventoryPanel
{
	public Toggle m_ownedToggle;

	public Toggle[] m_abilityToggles;

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
		List<GameBalanceVars.PlayerUnlockable> list = new List<GameBalanceVars.PlayerUnlockable>();
		CharacterResourceLink characterResourceLink = GameWideData.Get().GetCharacterResourceLink(this.m_charType);
		AbilityData component = characterResourceLink.ActorDataPrefab.GetComponent<AbilityData>();
		int i = 0;
		while (i < 0xE)
		{
			List<AbilityMod> availableModsForAbility;
			switch (i)
			{
			case 0:
				availableModsForAbility = AbilityModHelper.GetAvailableModsForAbility(component.m_ability0);
				goto IL_F2;
			case 1:
				availableModsForAbility = AbilityModHelper.GetAvailableModsForAbility(component.m_ability1);
				goto IL_F2;
			case 2:
				availableModsForAbility = AbilityModHelper.GetAvailableModsForAbility(component.m_ability2);
				goto IL_F2;
			case 3:
				availableModsForAbility = AbilityModHelper.GetAvailableModsForAbility(component.m_ability3);
				goto IL_F2;
			case 4:
				availableModsForAbility = AbilityModHelper.GetAvailableModsForAbility(component.m_ability4);
				goto IL_F2;
			case 5:
				availableModsForAbility = AbilityModHelper.GetAvailableModsForAbility(component.m_ability5);
				goto IL_F2;
			case 6:
				availableModsForAbility = AbilityModHelper.GetAvailableModsForAbility(component.m_ability6);
				goto IL_F2;
			}
			IL_12F:
			i++;
			continue;
			IL_F2:
			for (int j = 0; j < availableModsForAbility.Count; j++)
			{
				list.Add(availableModsForAbility[j].GetAbilityModUnlockData(this.m_charType, i));
			}
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				goto IL_12F;
			}
		}
		return base.SortItems(list).ToArray();
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
		if (item is GameBalanceVars.AbilityModUnlockData)
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

	protected override void PurchaseItem(GameBalanceVars.PlayerUnlockable item, CurrencyType type)
	{
		if (type != CurrencyType.FreelancerCurrency)
		{
			throw new Exception("You can't purchase mods without flux");
		}
		ClientGameManager.Get().PurchaseMod(this.m_charType, item.Index2, item.ID);
		PersistedCharacterData playerCharacterData = ClientGameManager.Get().GetPlayerCharacterData(this.m_charType);
		PlayerModData item2 = default(PlayerModData);
		item2.AbilityId = item.Index2;
		item2.AbilityModID = item.ID;
		playerCharacterData.CharacterComponent.Mods.Add(item2);
		base.UpdatePanel();
	}

	protected override CurrencyData GetItemCost(GameBalanceVars.PlayerUnlockable item, CurrencyType type)
	{
		if (type != CurrencyType.FreelancerCurrency)
		{
			return base.GetItemCost(item, type);
		}
		return new CurrencyData
		{
			Amount = GameBalanceVars.Get().FreelancerCurrencyToUnlockMod,
			Type = CurrencyType.FreelancerCurrency
		};
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

	public override TooltipType? GetItemTooltipType()
	{
		return new TooltipType?(TooltipType.Titled);
	}

	public override bool ItemTooltipPopulate(UITooltipBase tooltip, UIStoreItemBtn slot, GameBalanceVars.PlayerUnlockable item)
	{
		Ability ability;
		AbilityMod abilityMod = this.GetAbilityMod(item, out ability);
		string fullTooltip = abilityMod.GetFullTooltip(ability);
		if (fullTooltip.Trim().Length > 0)
		{
			UITitledTooltip uititledTooltip = tooltip as UITitledTooltip;
			uititledTooltip.Setup(abilityMod.GetName(), fullTooltip, string.Empty);
			return true;
		}
		return false;
	}

	private AbilityMod GetAbilityMod(GameBalanceVars.PlayerUnlockable item, out Ability ability)
	{
		CharacterResourceLink characterResourceLink = GameWideData.Get().GetCharacterResourceLink(this.m_charType);
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

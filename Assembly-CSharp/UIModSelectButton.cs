using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIModSelectButton : MonoBehaviour
{
	public TextMeshProUGUI[] m_abilityName;

	public TextMeshProUGUI[] m_description;

	public Image[] m_selectedModIcon;

	public Image[] m_costNotches;

	public RectTransform m_SelectedContainer;

	public Image m_disabled;

	public Image m_modDisabled;

	public Image m_defaultBG;

	public RectTransform[] m_lockIcon;

	public _SelectableBtn m_selectBtn;

	public _ButtonSwapSprite m_buttonHitBox;

	private CharacterType m_character;

	private int m_abilityId;

	private AbilityMod m_modReference;

	private bool m_lockVisible;

	public AbilityMod GetMod()
	{
		return m_modReference;
	}

	private void SetAbilityName(string text)
	{
		for (int i = 0; i < m_abilityName.Length; i++)
		{
			m_abilityName[i].text = text;
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
			return;
		}
	}

	private void SetCostNotches(int cost)
	{
		for (int i = 0; i < m_costNotches.Length; i++)
		{
			UIManager.SetGameObjectActive(m_costNotches[i], i < cost);
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
			return;
		}
	}

	private void SetDescription(string text)
	{
		for (int i = 0; i < m_description.Length; i++)
		{
			m_description[i].text = text;
		}
		while (true)
		{
			switch (1)
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

	private void SetModIconSprite(Sprite sprite)
	{
		for (int i = 0; i < m_selectedModIcon.Length; i++)
		{
			m_selectedModIcon[i].sprite = sprite;
		}
	}

	public bool IsLockVisible()
	{
		return m_lockVisible;
	}

	public bool CanBeSelected()
	{
		return !IsLockVisible();
	}

	public void SetMod(AbilityMod theMod, Ability ability, int abilityId, CharacterType character)
	{
		m_modReference = theMod;
		if (theMod != null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
				{
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					UIManager.SetGameObjectActive(base.gameObject, true);
					SetAbilityName(theMod.GetName());
					SetCostNotches(theMod.m_equipCost);
					string text = theMod.GetFullTooltip(ability);
					if (!theMod.m_flavorText.IsNullOrEmpty())
					{
						string text2 = text;
						text = text2 + Environment.NewLine + "<i>" + theMod.m_flavorText + "</i>";
					}
					SetDescription(text);
					SetModIconSprite(theMod.m_iconSprite);
					m_character = character;
					m_abilityId = abilityId;
					SetLockIcons();
					return;
				}
				}
			}
		}
		UIManager.SetGameObjectActive(base.gameObject, false);
	}

	public void SetAsUnselected(UIModSelectButton referenceButton)
	{
		m_modReference = null;
		UIManager.SetGameObjectActive(base.gameObject, true);
		SetAbilityName(StringUtil.TR("Empty", "Global"));
		SetCostNotches(0);
		SetDescription(StringUtil.TR("NoModSelected", "Global"));
		SetModIconSprite(referenceButton.m_selectedModIcon[0].sprite);
		m_abilityId = -1;
		SetLockIcons();
	}

	private void SetLockVisible(bool visible)
	{
		m_lockVisible = visible;
		for (int i = 0; i < m_lockIcon.Length; i++)
		{
			UIManager.SetGameObjectActive(m_lockIcon[i], visible);
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
			return;
		}
	}

	public void SetLockIcons()
	{
		if (!(m_modReference != null))
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
			if (m_lockIcon == null)
			{
				return;
			}
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				bool flag = true;
				PersistedCharacterData playerCharacterData = ClientGameManager.Get().GetPlayerCharacterData(m_character);
				if (playerCharacterData != null)
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
					flag = ((!playerCharacterData.CharacterComponent.IsModUnlocked(m_abilityId, m_modReference.m_abilityScopeId) && !GameManager.Get().GameplayOverrides.EnableAllMods) ? true : false);
				}
				SetLockVisible(flag);
				if (m_disabled != null)
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
					UIManager.SetGameObjectActive(m_disabled, flag);
				}
				if (m_modDisabled != null)
				{
					UIManager.SetGameObjectActive(m_modDisabled, flag);
				}
				return;
			}
		}
	}

	public void SetCallback(_ButtonSwapSprite.ButtonClickCallback callbackFunc)
	{
		m_buttonHitBox.callback = callbackFunc;
	}

	public void SetSelected(bool selected, bool forceAnimation = false)
	{
		if (m_SelectedContainer != null)
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
			UIManager.SetGameObjectActive(m_SelectedContainer, selected);
		}
		if (m_selectBtn != null)
		{
			m_selectBtn.SetSelected(selected, forceAnimation, string.Empty, string.Empty);
		}
	}

	public bool AvailableForPurchase()
	{
		if (m_lockIcon != null)
		{
			return IsLockVisible();
		}
		return false;
	}

	public void AskForPurchase()
	{
		if (m_modReference == null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return;
				}
			}
		}
		GameBalanceVars gameBalanceVars = GameBalanceVars.Get();
		int num = 1;
		if (gameBalanceVars.UseModEquipCostAsModUnlockCost)
		{
			num = m_modReference.m_equipCost;
		}
		int currentAmount = ClientGameManager.Get().PlayerWallet.GetCurrentAmount(CurrencyType.ModToken);
		if (num <= currentAmount)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
				{
					int num2;
					if (GameBalanceVars.Get().UseModEquipCostAsModUnlockCost)
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
						num2 = m_modReference.m_equipCost;
					}
					else
					{
						num2 = 1;
					}
					int num3 = num2;
					string text;
					if (currentAmount > 1)
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
						text = StringUtil.TR("UnlockModTokensConfirm", "Global");
					}
					else
					{
						text = StringUtil.TR("UnlockModTokenConfirm", "Global");
					}
					string format = text;
					string description = string.Format(format, num3, currentAmount);
					UIDialogPopupManager.OpenTwoButtonDialog(StringUtil.TR("UnlockMod", "Global"), description, StringUtil.TR("Yes", "Global"), StringUtil.TR("No", "Global"), delegate
					{
						RequestPurchaseMod();
					});
					return;
				}
				}
			}
		}
		UIDialogPopupManager.OpenOneButtonDialog(StringUtil.TR("InsufficientFunds", "Global"), StringUtil.TR("InsufficientFundsBody", "Global"), StringUtil.TR("Ok", "Global"));
	}

	public void RequestPurchaseMod()
	{
		ClientGameManager.Get().PurchaseMod(m_character, m_abilityId, m_modReference.m_abilityScopeId);
	}
}

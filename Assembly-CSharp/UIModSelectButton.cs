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
		foreach (TextMeshProUGUI abilityName in m_abilityName)
		{
			abilityName.text = text;
		}
	}

	private void SetCostNotches(int cost)
	{
		for (int i = 0; i < m_costNotches.Length; i++)
		{
			UIManager.SetGameObjectActive(m_costNotches[i], i < cost);
		}
	}

	private void SetDescription(string text)
	{
		foreach (TextMeshProUGUI desc in m_description)
		{
			desc.text = text;
		}
	}

	private void SetModIconSprite(Sprite sprite)
	{
		foreach (Image mod in m_selectedModIcon)
		{
			mod.sprite = sprite;
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
			UIManager.SetGameObjectActive(gameObject, true);
			SetAbilityName(theMod.GetName());
			SetCostNotches(theMod.m_equipCost);
			string text = theMod.GetFullTooltip(ability);
			if (!theMod.m_flavorText.IsNullOrEmpty())
			{
				text += Environment.NewLine + "<i>" + theMod.m_flavorText + "</i>";
			}
			SetDescription(text);
			SetModIconSprite(theMod.m_iconSprite);
			m_character = character;
			m_abilityId = abilityId;
			SetLockIcons();
		}
		else
		{
			UIManager.SetGameObjectActive(gameObject, false);
		}
	}

	public void SetAsUnselected(UIModSelectButton referenceButton)
	{
		m_modReference = null;
		UIManager.SetGameObjectActive(gameObject, true);
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
		foreach (RectTransform lockIcon in m_lockIcon)
		{
			UIManager.SetGameObjectActive(lockIcon, visible);
		}
	}

	public void SetLockIcons()
	{
		if (m_modReference == null || m_lockIcon == null)
		{
			return;
		}
		bool isLocked = true;
		PersistedCharacterData playerCharacterData = ClientGameManager.Get().GetPlayerCharacterData(m_character);
		if (playerCharacterData != null)
		{
			isLocked = !playerCharacterData.CharacterComponent.IsModUnlocked(m_abilityId, m_modReference.m_abilityScopeId)
			       && !GameManager.Get().GameplayOverrides.EnableAllMods;
		}
		SetLockVisible(isLocked);
		if (m_disabled != null)
		{
			UIManager.SetGameObjectActive(m_disabled, isLocked);
		}

		if (m_modDisabled != null)
		{
			UIManager.SetGameObjectActive(m_modDisabled, isLocked);
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
			UIManager.SetGameObjectActive(m_SelectedContainer, selected);
		}
		if (m_selectBtn != null)
		{
			m_selectBtn.SetSelected(selected, forceAnimation, string.Empty, string.Empty);
		}
	}

	public bool AvailableForPurchase()
	{
		return m_lockIcon != null && IsLockVisible();
	}

	public void AskForPurchase()
	{
		if (m_modReference == null)
		{
			return;
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
			int equipCost = GameBalanceVars.Get().UseModEquipCostAsModUnlockCost ? m_modReference.m_equipCost : 1;
			string format = StringUtil.TR(currentAmount > 1 ? "UnlockModTokensConfirm" : "UnlockModTokenConfirm", "Global");
			string description = string.Format(format, equipCost, currentAmount);
			UIDialogPopupManager.OpenTwoButtonDialog(
				StringUtil.TR("UnlockMod", "Global"),
				description,
				StringUtil.TR("Yes", "Global"),
				StringUtil.TR("No", "Global"),
				delegate
				{
					RequestPurchaseMod();
				});
		}
		else
		{
			UIDialogPopupManager.OpenOneButtonDialog(
				StringUtil.TR("InsufficientFunds", "Global"),
				StringUtil.TR("InsufficientFundsBody", "Global"),
				StringUtil.TR("Ok", "Global"));
		}
	}

	public void RequestPurchaseMod()
	{
		ClientGameManager.Get().PurchaseMod(m_character, m_abilityId, m_modReference.m_abilityScopeId);
	}
}

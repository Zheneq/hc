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
		return this.m_modReference;
	}

	private void SetAbilityName(string text)
	{
		for (int i = 0; i < this.m_abilityName.Length; i++)
		{
			this.m_abilityName[i].text = text;
		}
		for (;;)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			break;
		}
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(UIModSelectButton.SetAbilityName(string)).MethodHandle;
		}
	}

	private void SetCostNotches(int cost)
	{
		for (int i = 0; i < this.m_costNotches.Length; i++)
		{
			UIManager.SetGameObjectActive(this.m_costNotches[i], i < cost, null);
		}
		for (;;)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			break;
		}
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(UIModSelectButton.SetCostNotches(int)).MethodHandle;
		}
	}

	private void SetDescription(string text)
	{
		for (int i = 0; i < this.m_description.Length; i++)
		{
			this.m_description[i].text = text;
		}
		for (;;)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			break;
		}
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(UIModSelectButton.SetDescription(string)).MethodHandle;
		}
	}

	private void SetModIconSprite(Sprite sprite)
	{
		for (int i = 0; i < this.m_selectedModIcon.Length; i++)
		{
			this.m_selectedModIcon[i].sprite = sprite;
		}
	}

	public bool IsLockVisible()
	{
		return this.m_lockVisible;
	}

	public bool CanBeSelected()
	{
		return !this.IsLockVisible();
	}

	public void SetMod(AbilityMod theMod, Ability ability, int abilityId, CharacterType character)
	{
		this.m_modReference = theMod;
		if (theMod != null)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIModSelectButton.SetMod(AbilityMod, Ability, int, CharacterType)).MethodHandle;
			}
			UIManager.SetGameObjectActive(base.gameObject, true, null);
			this.SetAbilityName(theMod.GetName());
			this.SetCostNotches(theMod.m_equipCost);
			string text = theMod.GetFullTooltip(ability);
			if (!theMod.m_flavorText.IsNullOrEmpty())
			{
				string text2 = text;
				text = string.Concat(new string[]
				{
					text2,
					Environment.NewLine,
					"<i>",
					theMod.m_flavorText,
					"</i>"
				});
			}
			this.SetDescription(text);
			this.SetModIconSprite(theMod.m_iconSprite);
			this.m_character = character;
			this.m_abilityId = abilityId;
			this.SetLockIcons();
		}
		else
		{
			UIManager.SetGameObjectActive(base.gameObject, false, null);
		}
	}

	public void SetAsUnselected(UIModSelectButton referenceButton)
	{
		this.m_modReference = null;
		UIManager.SetGameObjectActive(base.gameObject, true, null);
		this.SetAbilityName(StringUtil.TR("Empty", "Global"));
		this.SetCostNotches(0);
		this.SetDescription(StringUtil.TR("NoModSelected", "Global"));
		this.SetModIconSprite(referenceButton.m_selectedModIcon[0].sprite);
		this.m_abilityId = -1;
		this.SetLockIcons();
	}

	private void SetLockVisible(bool visible)
	{
		this.m_lockVisible = visible;
		for (int i = 0; i < this.m_lockIcon.Length; i++)
		{
			UIManager.SetGameObjectActive(this.m_lockIcon[i], visible, null);
		}
		for (;;)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			break;
		}
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(UIModSelectButton.SetLockVisible(bool)).MethodHandle;
		}
	}

	public void SetLockIcons()
	{
		if (this.m_modReference != null)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIModSelectButton.SetLockIcons()).MethodHandle;
			}
			if (this.m_lockIcon != null)
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				bool flag = true;
				PersistedCharacterData playerCharacterData = ClientGameManager.Get().GetPlayerCharacterData(this.m_character);
				if (playerCharacterData != null)
				{
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					flag = (!playerCharacterData.CharacterComponent.IsModUnlocked(this.m_abilityId, this.m_modReference.m_abilityScopeId) && !GameManager.Get().GameplayOverrides.EnableAllMods);
				}
				this.SetLockVisible(flag);
				if (this.m_disabled != null)
				{
					for (;;)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					UIManager.SetGameObjectActive(this.m_disabled, flag, null);
				}
				if (this.m_modDisabled != null)
				{
					UIManager.SetGameObjectActive(this.m_modDisabled, flag, null);
				}
			}
		}
	}

	public void SetCallback(_ButtonSwapSprite.ButtonClickCallback callbackFunc)
	{
		this.m_buttonHitBox.callback = callbackFunc;
	}

	public void SetSelected(bool selected, bool forceAnimation = false)
	{
		if (this.m_SelectedContainer != null)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIModSelectButton.SetSelected(bool, bool)).MethodHandle;
			}
			UIManager.SetGameObjectActive(this.m_SelectedContainer, selected, null);
		}
		if (this.m_selectBtn != null)
		{
			this.m_selectBtn.SetSelected(selected, forceAnimation, string.Empty, string.Empty);
		}
	}

	public bool AvailableForPurchase()
	{
		return this.m_lockIcon != null && this.IsLockVisible();
	}

	public void AskForPurchase()
	{
		if (this.m_modReference == null)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIModSelectButton.AskForPurchase()).MethodHandle;
			}
			return;
		}
		GameBalanceVars gameBalanceVars = GameBalanceVars.Get();
		int num = 1;
		if (gameBalanceVars.UseModEquipCostAsModUnlockCost)
		{
			num = this.m_modReference.m_equipCost;
		}
		int currentAmount = ClientGameManager.Get().PlayerWallet.GetCurrentAmount(CurrencyType.ModToken);
		if (num <= currentAmount)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			int num2;
			if (GameBalanceVars.Get().UseModEquipCostAsModUnlockCost)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				num2 = this.m_modReference.m_equipCost;
			}
			else
			{
				num2 = 1;
			}
			int num3 = num2;
			string text;
			if (currentAmount > 1)
			{
				for (;;)
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
			UIDialogPopupManager.OpenTwoButtonDialog(StringUtil.TR("UnlockMod", "Global"), description, StringUtil.TR("Yes", "Global"), StringUtil.TR("No", "Global"), delegate(UIDialogBox dialogReference)
			{
				this.RequestPurchaseMod();
			}, null, false, false);
		}
		else
		{
			UIDialogPopupManager.OpenOneButtonDialog(StringUtil.TR("InsufficientFunds", "Global"), StringUtil.TR("InsufficientFundsBody", "Global"), StringUtil.TR("Ok", "Global"), null, -1, false);
		}
	}

	public void RequestPurchaseMod()
	{
		ClientGameManager.Get().PurchaseMod(this.m_character, this.m_abilityId, this.m_modReference.m_abilityScopeId);
	}
}

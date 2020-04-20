using System;
using TMPro;
using UnityEngine;

public class UIAbilitySelectPanel : MonoBehaviour
{
	public _SelectableBtn m_ability1Btn;

	public _SelectableBtn m_ability2Btn;

	public _SelectableBtn m_ability3Btn;

	public _SelectableBtn m_ability4Btn;

	public _SelectableBtn m_ability5Btn;

	public _SelectableBtn m_catalyst1Btn;

	public _SelectableBtn m_catalyst2Btn;

	public _SelectableBtn m_catalyst3Btn;

	public CanvasGroup m_closeButtonHover;

	public GameObject m_closeButton;

	public GameObject m_line;

	private KeyPreference m_hoverAbility;

	private void Awake()
	{
	}

	public void Init(AbilityData abilityData)
	{
		AbilityData.AbilityEntry[] abilityEntries = abilityData.abilityEntries;
		TextMeshProUGUI[] componentsInChildren = this.m_ability1Btn.gameObject.GetComponentsInChildren<TextMeshProUGUI>();
		foreach (TextMeshProUGUI textMeshProUGUI in componentsInChildren)
		{
			textMeshProUGUI.text = abilityEntries[0].ability.GetNameString();
		}
		TextMeshProUGUI[] componentsInChildren2 = this.m_ability2Btn.gameObject.GetComponentsInChildren<TextMeshProUGUI>();
		foreach (TextMeshProUGUI textMeshProUGUI2 in componentsInChildren2)
		{
			textMeshProUGUI2.text = abilityEntries[1].ability.GetNameString();
		}
		TextMeshProUGUI[] componentsInChildren3 = this.m_ability3Btn.gameObject.GetComponentsInChildren<TextMeshProUGUI>();
		foreach (TextMeshProUGUI textMeshProUGUI3 in componentsInChildren3)
		{
			textMeshProUGUI3.text = abilityEntries[2].ability.GetNameString();
		}
		TextMeshProUGUI[] componentsInChildren4 = this.m_ability4Btn.gameObject.GetComponentsInChildren<TextMeshProUGUI>();
		foreach (TextMeshProUGUI textMeshProUGUI4 in componentsInChildren4)
		{
			textMeshProUGUI4.text = abilityEntries[3].ability.GetNameString();
		}
		TextMeshProUGUI[] componentsInChildren5 = this.m_ability5Btn.gameObject.GetComponentsInChildren<TextMeshProUGUI>();
		foreach (TextMeshProUGUI textMeshProUGUI5 in componentsInChildren5)
		{
			textMeshProUGUI5.text = abilityEntries[4].ability.GetNameString();
		}
		if (abilityEntries[7].ability == null)
		{
			UIManager.SetGameObjectActive(this.m_catalyst1Btn, false, null);
		}
		else
		{
			UIManager.SetGameObjectActive(this.m_catalyst1Btn, true, null);
			TextMeshProUGUI[] componentsInChildren6 = this.m_catalyst1Btn.gameObject.GetComponentsInChildren<TextMeshProUGUI>();
			foreach (TextMeshProUGUI textMeshProUGUI6 in componentsInChildren6)
			{
				textMeshProUGUI6.text = abilityEntries[7].ability.GetNameString();
			}
		}
		if (abilityEntries[8].ability == null)
		{
			UIManager.SetGameObjectActive(this.m_catalyst2Btn, false, null);
		}
		else
		{
			UIManager.SetGameObjectActive(this.m_catalyst2Btn, true, null);
			TextMeshProUGUI[] componentsInChildren7 = this.m_catalyst2Btn.gameObject.GetComponentsInChildren<TextMeshProUGUI>();
			foreach (TextMeshProUGUI textMeshProUGUI7 in componentsInChildren7)
			{
				textMeshProUGUI7.text = abilityEntries[8].ability.GetNameString();
			}
		}
		if (abilityEntries[9].ability == null)
		{
			UIManager.SetGameObjectActive(this.m_catalyst3Btn, false, null);
		}
		else
		{
			UIManager.SetGameObjectActive(this.m_catalyst3Btn, true, null);
			TextMeshProUGUI[] componentsInChildren8 = this.m_catalyst3Btn.gameObject.GetComponentsInChildren<TextMeshProUGUI>();
			foreach (TextMeshProUGUI textMeshProUGUI8 in componentsInChildren8)
			{
				textMeshProUGUI8.text = abilityEntries[9].ability.GetNameString();
			}
		}
		this.m_ability1Btn.SetSelected(false, false, string.Empty, string.Empty);
		this.m_ability2Btn.SetSelected(false, false, string.Empty, string.Empty);
		this.m_ability3Btn.SetSelected(false, false, string.Empty, string.Empty);
		this.m_ability4Btn.SetSelected(false, false, string.Empty, string.Empty);
		this.m_ability5Btn.SetSelected(false, false, string.Empty, string.Empty);
		this.m_catalyst1Btn.SetSelected(false, false, string.Empty, string.Empty);
		this.m_catalyst2Btn.SetSelected(false, false, string.Empty, string.Empty);
		this.m_catalyst3Btn.SetSelected(false, false, string.Empty, string.Empty);
		this.m_ability1Btn.SetDisabled(HUD_UI.Get().m_mainScreenPanel.m_abilityBar.m_abilityButtons[0].m_disabled.gameObject.activeSelf);
		this.m_ability2Btn.SetDisabled(HUD_UI.Get().m_mainScreenPanel.m_abilityBar.m_abilityButtons[1].m_disabled.gameObject.activeSelf);
		this.m_ability3Btn.SetDisabled(HUD_UI.Get().m_mainScreenPanel.m_abilityBar.m_abilityButtons[2].m_disabled.gameObject.activeSelf);
		this.m_ability4Btn.SetDisabled(HUD_UI.Get().m_mainScreenPanel.m_abilityBar.m_abilityButtons[3].m_disabled.gameObject.activeSelf);
		this.m_ability5Btn.SetDisabled(HUD_UI.Get().m_mainScreenPanel.m_abilityBar.m_abilityButtons[4].m_disabled.gameObject.activeSelf);
		this.m_catalyst1Btn.SetDisabled(HUD_UI.Get().m_mainScreenPanel.m_cardBar.m_cardButtons[0].m_disabled.gameObject.activeSelf);
		this.m_catalyst2Btn.SetDisabled(HUD_UI.Get().m_mainScreenPanel.m_cardBar.m_cardButtons[1].m_disabled.gameObject.activeSelf);
		this.m_catalyst3Btn.SetDisabled(HUD_UI.Get().m_mainScreenPanel.m_cardBar.m_cardButtons[2].m_disabled.gameObject.activeSelf);
		this.m_hoverAbility = KeyPreference.NullPreference;
	}

	private void SetSelectedButton(KeyPreference hoverAbility, bool clear = false)
	{
		_SelectableBtn selectableBtn = null;
		switch (hoverAbility)
		{
		case KeyPreference.Ability1:
			selectableBtn = this.m_ability1Btn;
			break;
		case KeyPreference.Ability2:
			selectableBtn = this.m_ability2Btn;
			break;
		case KeyPreference.Ability3:
			selectableBtn = this.m_ability3Btn;
			break;
		case KeyPreference.Ability4:
			selectableBtn = this.m_ability4Btn;
			break;
		case KeyPreference.Ability5:
			selectableBtn = this.m_ability5Btn;
			break;
		case KeyPreference.Card1:
			selectableBtn = this.m_catalyst1Btn;
			break;
		case KeyPreference.Card2:
			selectableBtn = this.m_catalyst2Btn;
			break;
		case KeyPreference.Card3:
			selectableBtn = this.m_catalyst3Btn;
			break;
		}
		if (selectableBtn != null)
		{
			if (!selectableBtn.IsDisabled)
			{
				selectableBtn.SetSelected(!clear, false, string.Empty, string.Empty);
			}
		}
	}

	public void SelectAbilityButtonFromAngle(float angle, float lineSize)
	{
		if (lineSize == 0f && this.m_hoverAbility != KeyPreference.NullPreference)
		{
			this.SetSelectedButton(this.m_hoverAbility, true);
			this.m_hoverAbility = KeyPreference.NullPreference;
			return;
		}
		if (lineSize > 0f)
		{
			KeyPreference keyPreference = KeyPreference.NullPreference;
			angle += 10f;
			if (angle < 0f)
			{
				angle += 360f;
			}
			switch ((int)(angle / 360f * 8f))
			{
			case 0:
				keyPreference = KeyPreference.Ability3;
				break;
			case 1:
				keyPreference = KeyPreference.Ability2;
				break;
			case 2:
				keyPreference = KeyPreference.Ability1;
				break;
			case 3:
				keyPreference = KeyPreference.Card3;
				break;
			case 4:
				keyPreference = KeyPreference.Card2;
				break;
			case 5:
				keyPreference = KeyPreference.Card1;
				break;
			case 6:
				keyPreference = KeyPreference.Ability5;
				break;
			case 7:
				keyPreference = KeyPreference.Ability4;
				break;
			}
			if (keyPreference != this.m_hoverAbility)
			{
				if (this.m_hoverAbility != KeyPreference.NullPreference)
				{
					this.SetSelectedButton(this.m_hoverAbility, true);
				}
				this.m_hoverAbility = keyPreference;
				this.SetSelectedButton(this.m_hoverAbility, false);
			}
		}
	}

	public KeyPreference GetAbilityHover()
	{
		return this.m_hoverAbility;
	}
}

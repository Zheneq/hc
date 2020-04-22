using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISpellsSelectButton : MonoBehaviour
{
	public TextMeshProUGUI[] m_abilityName;

	public TextMeshProUGUI[] m_description;

	public Image[] m_abilityIcon;

	public Image m_disabled;

	public RectTransform m_freeActionContainer;

	public RectTransform m_selectedContainer;

	public _ButtonSwapSprite m_buttonHitBox;

	private bool m_selected;

	private _SelectableBtn selectBtn;

	private Card m_card;

	private AbilityRunPhase m_cardPhase;

	private void Start()
	{
		if (!(m_buttonHitBox != null))
		{
			return;
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
			m_buttonHitBox.callback = SpellClicked;
			return;
		}
	}

	public Card GetCard()
	{
		return m_card;
	}

	public AbilityRunPhase GetPhase()
	{
		return m_cardPhase;
	}

	public void SpellClicked(BaseEventData data)
	{
		UICharacterSelectScreenController.Get().m_charSettingsPanel.m_spellsSubPanel.SpellClicked(this);
		if (UIRankedCharacterSelectSettingsPanel.Get() != null)
		{
			UIRankedCharacterSelectSettingsPanel.Get().m_spellsSubPanel.SpellClicked(this);
		}
		UIFrontEnd.PlaySound(FrontEndButtonSounds.CharacterSelectModAdd);
	}

	private void SetAbilityName(string text)
	{
		for (int i = 0; i < m_abilityName.Length; i++)
		{
			m_abilityName[i].text = text;
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
			switch (3)
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

	private void SetCatalystIcon(Sprite sprite)
	{
		for (int i = 0; i < m_abilityIcon.Length; i++)
		{
			m_abilityIcon[i].sprite = sprite;
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

	public void Setup(Card card)
	{
		m_card = card;
		UIManager.SetGameObjectActive(base.gameObject, true);
		SetAbilityName(card.GetDisplayName());
		string text = card.m_useAbility.GetFullTooltip();
		if (!card.m_useAbility.m_flavorText.IsNullOrEmpty())
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			string text2 = text;
			text = text2 + Environment.NewLine + "<i>" + card.m_useAbility.m_flavorText + "</i>";
		}
		SetDescription(text);
		SetCatalystIcon(card.GetIconSprite());
		SetSelectState();
		UIManager.SetGameObjectActive(m_freeActionContainer, card.IsFreeAction());
		m_cardPhase = card.GetAbilityRunPhase();
	}

	public void SetSelectState(bool forceAnimPlay = false)
	{
		if (selectBtn == null)
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
			selectBtn = GetComponent<_SelectableBtn>();
		}
		if (selectBtn != null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					selectBtn.SetSelected(m_selected, forceAnimPlay, string.Empty, string.Empty);
					return;
				}
			}
		}
		UIManager.SetGameObjectActive(m_selectedContainer, m_selected);
	}

	public bool IsSelected()
	{
		return m_selected;
	}

	public void SetSelected(bool selected)
	{
		m_selected = selected;
		SetSelectState();
	}
}

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
		if (this.m_buttonHitBox != null)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UISpellsSelectButton.Start()).MethodHandle;
			}
			this.m_buttonHitBox.callback = new _ButtonSwapSprite.ButtonClickCallback(this.SpellClicked);
		}
	}

	public Card GetCard()
	{
		return this.m_card;
	}

	public AbilityRunPhase GetPhase()
	{
		return this.m_cardPhase;
	}

	public void SpellClicked(BaseEventData data)
	{
		UICharacterSelectScreenController.Get().m_charSettingsPanel.m_spellsSubPanel.SpellClicked(this, true);
		if (UIRankedCharacterSelectSettingsPanel.Get() != null)
		{
			UIRankedCharacterSelectSettingsPanel.Get().m_spellsSubPanel.SpellClicked(this, true);
		}
		UIFrontEnd.PlaySound(FrontEndButtonSounds.CharacterSelectModAdd);
	}

	private void SetAbilityName(string text)
	{
		for (int i = 0; i < this.m_abilityName.Length; i++)
		{
			this.m_abilityName[i].text = text;
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
			switch (3)
			{
			case 0:
				continue;
			}
			break;
		}
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(UISpellsSelectButton.SetDescription(string)).MethodHandle;
		}
	}

	private void SetCatalystIcon(Sprite sprite)
	{
		for (int i = 0; i < this.m_abilityIcon.Length; i++)
		{
			this.m_abilityIcon[i].sprite = sprite;
		}
		for (;;)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			break;
		}
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(UISpellsSelectButton.SetCatalystIcon(Sprite)).MethodHandle;
		}
	}

	public void Setup(Card card)
	{
		this.m_card = card;
		UIManager.SetGameObjectActive(base.gameObject, true, null);
		this.SetAbilityName(card.GetDisplayName());
		string text = card.m_useAbility.GetFullTooltip();
		if (!card.m_useAbility.m_flavorText.IsNullOrEmpty())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UISpellsSelectButton.Setup(Card)).MethodHandle;
			}
			string text2 = text;
			text = string.Concat(new string[]
			{
				text2,
				Environment.NewLine,
				"<i>",
				card.m_useAbility.m_flavorText,
				"</i>"
			});
		}
		this.SetDescription(text);
		this.SetCatalystIcon(card.GetIconSprite());
		this.SetSelectState(false);
		UIManager.SetGameObjectActive(this.m_freeActionContainer, card.IsFreeAction(), null);
		this.m_cardPhase = card.GetAbilityRunPhase();
	}

	public void SetSelectState(bool forceAnimPlay = false)
	{
		if (this.selectBtn == null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UISpellsSelectButton.SetSelectState(bool)).MethodHandle;
			}
			this.selectBtn = base.GetComponent<_SelectableBtn>();
		}
		if (this.selectBtn != null)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			this.selectBtn.SetSelected(this.m_selected, forceAnimPlay, string.Empty, string.Empty);
		}
		else
		{
			UIManager.SetGameObjectActive(this.m_selectedContainer, this.m_selected, null);
		}
	}

	public bool IsSelected()
	{
		return this.m_selected;
	}

	public void SetSelected(bool selected)
	{
		this.m_selected = selected;
		this.SetSelectState(false);
	}
}

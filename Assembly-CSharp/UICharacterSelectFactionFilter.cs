using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UICharacterSelectFactionFilter : MonoBehaviour
{
	public _SelectableBtn m_btn;

	public Image m_icon;

	private List<CharacterType> m_characters;

	private Action<UICharacterSelectFactionFilter> m_callback;

	private void Awake()
	{
		m_btn.SetSelected(false, false, string.Empty, string.Empty);
		m_btn.spriteController.callback = OnClick;
	}

	public void Setup(FactionGroup factionGroup, Action<UICharacterSelectFactionFilter> callback)
	{
		m_callback = callback;
		m_icon.sprite = Resources.Load<Sprite>(factionGroup.IconPath);
		m_characters = factionGroup.Characters;
	}

	public void Setup(List<CharacterType> characters, Action<UICharacterSelectFactionFilter> callback)
	{
		m_callback = callback;
		m_characters = characters;
	}

	private void OnClick(BaseEventData data)
	{
		m_btn.SetSelected(!m_btn.IsSelected(), false, string.Empty, string.Empty);
		m_callback(this);
	}

	public bool IsAvailable(CharacterType type)
	{
		if (!m_btn.IsSelected())
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return false;
				}
			}
		}
		return m_characters.Contains(type);
	}
}

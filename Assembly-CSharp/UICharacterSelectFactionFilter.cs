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
		this.m_btn.SetSelected(false, false, string.Empty, string.Empty);
		this.m_btn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.OnClick);
	}

	public void Setup(FactionGroup factionGroup, Action<UICharacterSelectFactionFilter> callback)
	{
		this.m_callback = callback;
		this.m_icon.sprite = Resources.Load<Sprite>(factionGroup.IconPath);
		this.m_characters = factionGroup.Characters;
	}

	public void Setup(List<CharacterType> characters, Action<UICharacterSelectFactionFilter> callback)
	{
		this.m_callback = callback;
		this.m_characters = characters;
	}

	private void OnClick(BaseEventData data)
	{
		this.m_btn.SetSelected(!this.m_btn.IsSelected(), false, string.Empty, string.Empty);
		this.m_callback(this);
	}

	public bool IsAvailable(CharacterType type)
	{
		if (!this.m_btn.IsSelected())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterSelectFactionFilter.IsAvailable(CharacterType)).MethodHandle;
			}
			return false;
		}
		return this.m_characters.Contains(type);
	}
}

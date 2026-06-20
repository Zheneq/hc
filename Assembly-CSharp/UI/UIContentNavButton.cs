using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIContentNavButton : MonoBehaviour
{
	public _ButtonSwapSprite m_hitbox;

	public Image m_selectedImage;

	public bool m_tempDisable;

	public FrontEndButtonSounds m_buttonSound;

	private Action<UIContentNavButton> m_buttonClickedCallback;

	private void Awake()
	{
		m_hitbox.callback = ClickedMenuButton;
		if (!m_tempDisable)
		{
			return;
		}
		while (true)
		{
			m_hitbox.m_defaultImage.color = Color.white * 0.5f;
			m_hitbox.m_hoverImage.color = Color.white * 0.5f;
			m_hitbox.m_pressedImage.color = Color.white * 0.5f;
			return;
		}
	}

	public void SetPlaybackSound(FrontEndButtonSounds buttonSound)
	{
		m_buttonSound = buttonSound;
	}

	public void RegisterClickCallback(Action<UIContentNavButton> callback)
	{
		m_buttonClickedCallback = callback;
	}

	public void ClickedMenuButton(BaseEventData data)
	{
		if (m_tempDisable)
		{
			return;
		}
		while (true)
		{
			if (m_buttonClickedCallback == null)
			{
				return;
			}
			while (true)
			{
				if (m_buttonSound != 0)
				{
					UIFrontEnd.PlaySound(m_buttonSound);
				}
				m_buttonClickedCallback(this);
				return;
			}
		}
	}

	public void SetSelected(bool selected)
	{
		m_hitbox.selectableButton.SetSelected(selected, false, string.Empty, string.Empty);
	}
}

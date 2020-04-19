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
		this.m_hitbox.callback = new _ButtonSwapSprite.ButtonClickCallback(this.ClickedMenuButton);
		if (this.m_tempDisable)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIContentNavButton.Awake()).MethodHandle;
			}
			this.m_hitbox.m_defaultImage.color = Color.white * 0.5f;
			this.m_hitbox.m_hoverImage.color = Color.white * 0.5f;
			this.m_hitbox.m_pressedImage.color = Color.white * 0.5f;
		}
	}

	public void SetPlaybackSound(FrontEndButtonSounds buttonSound)
	{
		this.m_buttonSound = buttonSound;
	}

	public void RegisterClickCallback(Action<UIContentNavButton> callback)
	{
		this.m_buttonClickedCallback = callback;
	}

	public void ClickedMenuButton(BaseEventData data)
	{
		if (!this.m_tempDisable)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIContentNavButton.ClickedMenuButton(BaseEventData)).MethodHandle;
			}
			if (this.m_buttonClickedCallback != null)
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
				if (this.m_buttonSound != FrontEndButtonSounds.None)
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
					UIFrontEnd.PlaySound(this.m_buttonSound);
				}
				this.m_buttonClickedCallback(this);
			}
		}
	}

	public void SetSelected(bool selected)
	{
		this.m_hitbox.selectableButton.SetSelected(selected, false, string.Empty, string.Empty);
	}
}

using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIStoreNavBtn : MonoBehaviour
{
	public _ButtonSwapSprite m_hitbox;

	public UIStoreBasePanel m_storePanel;

	public bool m_isEnabled;

	private void Awake()
	{
		this.m_hitbox.callback = new _ButtonSwapSprite.ButtonClickCallback(this.ButtonClicked);
		this.SetDisabled(!this.m_isEnabled);
	}

	public void SetDisabled(bool disabled)
	{
		this.m_hitbox.SetClickable(!disabled);
		UIManager.SetGameObjectActive(this.m_hitbox.m_defaultImage, !disabled, null);
		UIManager.SetGameObjectActive(this.m_hitbox.m_hoverImage, !disabled, null);
		UIManager.SetGameObjectActive(this.m_hitbox.m_pressedImage, !disabled, null);
	}

	public bool IsSelected()
	{
		return this.m_hitbox.selectableButton.IsSelected();
	}

	public void ButtonClicked(BaseEventData data)
	{
		if (this.m_isEnabled)
		{
			UIFrontEnd.PlaySound(FrontEndButtonSounds.PlayCategorySelect);
			UIStorePanel.Get().NotifyNavBtnClicked(this);
		}
	}

	public void SetSelected(bool selected)
	{
		this.m_hitbox.selectableButton.SetSelected(selected, false, string.Empty, string.Empty);
	}
}

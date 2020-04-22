using UnityEngine;
using UnityEngine.EventSystems;

public class UIStoreNavBtn : MonoBehaviour
{
	public _ButtonSwapSprite m_hitbox;

	public UIStoreBasePanel m_storePanel;

	public bool m_isEnabled;

	private void Awake()
	{
		m_hitbox.callback = ButtonClicked;
		SetDisabled(!m_isEnabled);
	}

	public void SetDisabled(bool disabled)
	{
		m_hitbox.SetClickable(!disabled);
		UIManager.SetGameObjectActive(m_hitbox.m_defaultImage, !disabled);
		UIManager.SetGameObjectActive(m_hitbox.m_hoverImage, !disabled);
		UIManager.SetGameObjectActive(m_hitbox.m_pressedImage, !disabled);
	}

	public bool IsSelected()
	{
		return m_hitbox.selectableButton.IsSelected();
	}

	public void ButtonClicked(BaseEventData data)
	{
		if (!m_isEnabled)
		{
			return;
		}
		while (true)
		{
			UIFrontEnd.PlaySound(FrontEndButtonSounds.PlayCategorySelect);
			UIStorePanel.Get().NotifyNavBtnClicked(this);
			return;
		}
	}

	public void SetSelected(bool selected)
	{
		m_hitbox.selectableButton.SetSelected(selected, false, string.Empty, string.Empty);
	}
}
